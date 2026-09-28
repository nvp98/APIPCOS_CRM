using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace APIPCOS_CRM.Services
{
    /// <summary>
    /// Gọi sang BK-MIS (server nội bộ 10.192.214.23:9001) để đẩy phiếu MTC HRC.
    ///
    /// Vì sao có lớp này: Salesforce ở cloud không tới được IP nội bộ của BK-MIS, nên
    /// APIPCOS_CRM (đã public qua apiplcos.hoaphatdungquat.vn) đứng giữa làm cầu. Nhờ vậy
    /// tài khoản BK-MIS chỉ nằm ở đây, không phải cấp cho Salesforce.
    ///
    /// Hợp đồng đo thật với BK-MIS ngày 21/09/2026:
    ///  - CHỈ nhận POST, body là MỘT object (gửi mảng là 400 "could not be converted to
    ///    SharedProject.Models.Certificate").
    ///  - Xác thực Basic trên từng request, không có bước login/token.
    ///  - LỖI VẪN TRẢ HTTP 200: {"success":false,"messages":"..."} — nên phải đọc `success`,
    ///    không được tin mã HTTP.
    /// </summary>
    public class BkMisClient
    {
        private readonly HttpClient _http;
        private readonly string _path;
        private readonly bool _daCauHinh;

        /* KHÔNG ném lỗi trong constructor: lớp này được inject vào HRC_ProductController,
           ném ở đây là GetData/GetTransporters chết theo khi server chưa điền mật khẩu.
           Thiếu cấu hình thì chỉ PushCertificate báo lỗi. */
        public BkMisClient(HttpClient http, IConfiguration config)
        {
            var s = config.GetSection("BkMis");
            var baseUrl = s["BaseUrl"];
            _path = s["CertificateHrcPath"] ?? "/api/HPDQConnect/Certificate_HRC";
            var user = s["Username"] ?? "";
            var pass = s["Password"] ?? "";
            _daCauHinh = !string.IsNullOrWhiteSpace(baseUrl) && !string.IsNullOrEmpty(pass);

            _http = http;
            _http.Timeout = TimeSpan.FromSeconds(int.TryParse(s["TimeoutSeconds"], out var t) ? t : 60);
            if (_daCauHinh)
            {
                _http.BaseAddress = new Uri(baseUrl!);
                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{user}:{pass}")));
            }
        }

        /// <summary>
        /// Đẩy một phiếu (body đã đúng cấu trúc BK-MIS, do Salesforce dựng) và trả lại
        /// phản hồi của BK-MIS nguyên trạng để tầng trên tự quyết.
        /// </summary>
        public async Task<BkMisResult> PushCertificateHrcAsync(JsonElement body)
        {
            if (!_daCauHinh)
                throw new InvalidOperationException(
                    "Chưa cấu hình BkMis:BaseUrl / BkMis:Password trên server APIPCOS_CRM.");

            using var content = new StringContent(body.GetRawText(), Encoding.UTF8, "application/json");
            using var res = await _http.PostAsync(_path, content);
            var text = await res.Content.ReadAsStringAsync();

            var kq = new BkMisResult { HttpStatus = (int)res.StatusCode, Raw = text };
            try
            {
                using var doc = JsonDocument.Parse(text);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("success", out var ok) && ok.ValueKind == JsonValueKind.True)
                        kq.Success = true;
                    if (root.TryGetProperty("messages", out var msg) && msg.ValueKind == JsonValueKind.String)
                        kq.Message = msg.GetString();
                    /* Phản hồi thật 28/09/2026: {"success":true,"error":null,"messages":"OK","data":null}.
                       Khi thất bại BK-MIS có thể để lý do ở "error" thay vì "messages" — đọc cả hai,
                       ưu tiên "error" vì nó cụ thể hơn. */
                    if (!kq.Success && root.TryGetProperty("error", out var err) && err.ValueKind == JsonValueKind.String
                        && !string.IsNullOrWhiteSpace(err.GetString()))
                        kq.Message = err.GetString();
                    // ASP.NET ProblemDetails (400 validation) gói lỗi trong "errors"/"title"
                    if (kq.Message == null && root.TryGetProperty("title", out var title) && title.ValueKind == JsonValueKind.String)
                        kq.Message = title.GetString();
                }
            }
            catch (JsonException)
            {
                // BK trả không phải JSON (trang lỗi IIS…) — giữ Raw để tầng trên báo lại
            }

            if (!res.IsSuccessStatusCode && kq.Message == null)
                kq.Message = $"BK-MIS trả HTTP {(int)res.StatusCode}";
            return kq;
        }
    }

    public class BkMisResult
    {
        public bool Success { get; set; }
        public int HttpStatus { get; set; }
        public string? Message { get; set; }
        public string? Raw { get; set; }
    }
}
