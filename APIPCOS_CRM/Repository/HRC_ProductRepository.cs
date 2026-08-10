using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Repository
{
    public class HRC_ProductRepository
    {
        private readonly Bkmis11_Context _bkmis11;
        private readonly Bkmis13_Context _bkmis13;

        private static double? ScaleAndRound(double? value, double factor)
            => value.HasValue ? Math.Round(value.Value * factor, MidpointRounding.AwayFromZero) : null;

        // x100: C, Si, Mn, CEV | x1000: S, P, Cu, Ni, Cr, Mo | round only: V, Ti, Al, B, Ca
        private static readonly Dictionary<string, Func<HRC_Product, double?>> _chemicalMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["C"]   = p => ScaleAndRound(p.C,   100),
                ["Si"]  = p => ScaleAndRound(p.Si,  100),
                ["Mn"]  = p => ScaleAndRound(p.Mn,  100),
                ["CEV"] = p => ScaleAndRound(p.CEV, 100),
                ["S"]   = p => ScaleAndRound(p.S,   1000),
                ["P"]   = p => ScaleAndRound(p.P,   1000),
                ["Cu"]  = p => ScaleAndRound(p.Cu,  1000),
                ["Ni"]  = p => ScaleAndRound(p.Ni,  1000),
                ["Cr"]  = p => ScaleAndRound(p.Cr,  1000),
                ["Mo"]  = p => ScaleAndRound(p.Mo,  1000),
                ["V"]   = p => ScaleAndRound(p.V,   1),
                ["Ti"]  = p => ScaleAndRound(p.Ti,  1),
                ["Al"]  = p => ScaleAndRound(p.Al,  1),
                ["B"]   = p => ScaleAndRound(p.B,   1),
                ["CA"]  = p => ScaleAndRound(p.Ca,  1),
                ["O"]  = p => ScaleAndRound(p.O,   1),
                ["N"]   = p => ScaleAndRound(p.N,   1),
                ["H"]   = p => ScaleAndRound(p.H,   1),
                ["Nb"]  = p => ScaleAndRound(p.Nb,  1)
            };

        private const string DefaultConfig = "C;Si;Mn;S;P;Cu;Ni;Cr;Mo;V;Ti;Al;B;CA;CEV;O;N;H;Nb";

        public HRC_ProductRepository(Bkmis11_Context bkmis11, Bkmis13_Context bkmis13)
        {
            _bkmis11 = bkmis11;
            _bkmis13 = bkmis13;
        }

        public async Task<HRC_GetDataResult> GetDataAsync(HRC_ProductRequestDto request)
        {

            if(request.CustomerCode == null)
            {
                throw new ArgumentException("CustomerCode cannot be null", nameof(request.CustomerCode));
            }
            // 1. Lấy tất cả ProductName thuộc SO + Transporter
            var productNamesInSO = await GetProductNamesBySOAsync(request);

            // 2. Phân loại ListID: valid (có trong SO) vs alert (không có trong SO)
            var validIDs = request.ListID.Where(id => productNamesInSO.Contains(id)).ToList();
            var alertIDs = request.ListID.Where(id => !productNamesInSO.Contains(id)).ToList();

            // 3. Query PhieuXuatHang theo validIDs → dùng cho header + tính tổng Weight
            var phieuXuatList = await GetPhieuXuatHangByValidIDsAsync(request, validIDs);

            // 4. Query HRC_Product theo validIDs → dùng cho HPDQ_Data__c
            var productList = await GetHRCProductsAsync(validIDs);

            // 5. Build response
            var certificate = BuildResponse(request, phieuXuatList, productList);

            return new HRC_GetDataResult
            {
                Certificate = certificate,
                AlertIDs    = alertIDs
            };
        }

        public const int DefaultTransporterPageSize = 30;
        public const int MaxTransporterPageSize = 100;
        public const int MinTransporterSearchLength = 2;

        // Lấy danh sách Transporter distinct (không rỗng), có paging + bắt buộc tìm kiếm (autocomplete)
        public async Task<(List<string> Items, int TotalCount)> GetDistinctTransportersAsync(int page, int pageSize, string searchText)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = DefaultTransporterPageSize;
            if (pageSize > MaxTransporterPageSize) pageSize = MaxTransporterPageSize;

            // Scan 1 lần duy nhất (DISTINCT trên toàn bảng vốn đã nặng) rồi paging trong memory,
            // thay vì gọi CountAsync + ToListAsync (2 lần full scan) như trước -> tránh timeout.
            _bkmis13.Database.SetCommandTimeout(120);

            var query = _bkmis13.PhieuXuatHang_HRCs
                .Where(p => p.Transporter != null && p.Transporter != "")
                .Where(p => EF.Functions.Like(p.Transporter, $"%{searchText}%"));

            var allTransporters = await query
                .Select(p => p.Transporter!)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();

            var totalCount = allTransporters.Count;

            var items = allTransporters
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }

        // Lấy tất cả ProductName thuộc SO + filter Transporter LIKE
        private async Task<HashSet<string>> GetProductNamesBySOAsync(HRC_ProductRequestDto request)
        {
            var query = _bkmis13.PhieuXuatHang_HRCs
                .Where(p => p.SO == request.SO && p.ProductName != null);

            if (!string.IsNullOrWhiteSpace(request.Transporter))
                query = query.Where(p => EF.Functions.Like(p.Transporter, $"%{request.Transporter}%"));

            var names = await query
                .Select(p => p.ProductName!)
                .Distinct()
                .ToListAsync();

            return names.ToHashSet();
        }

        // Lấy PhieuXuatHang theo SO + validIDs + Transporter LIKE
        private async Task<List<PhieuXuatHang_HRC>> GetPhieuXuatHangByValidIDsAsync(
            HRC_ProductRequestDto request,
            List<string> validIDs)
        {
            if (!validIDs.Any())
                return new List<PhieuXuatHang_HRC>();

            var query = _bkmis13.PhieuXuatHang_HRCs
                .Where(p => p.SO == request.SO
                         && p.ProductName != null
                         && validIDs.Contains(p.ProductName));

            if (!string.IsNullOrWhiteSpace(request.Transporter))
                query = query.Where(p => EF.Functions.Like(p.Transporter, $"%{request.Transporter}%"));

            // 1 SO có thể có nhiều Transporter -> lấy theo InTime sớm nhất để BuildResponse.FirstOrDefault() ổn định
            return await query.OrderBy(p => p.InTime).ToListAsync();
        }

        // Lấy HRC_Product theo validIDs
        private async Task<List<HRC_Product>> GetHRCProductsAsync(List<string> validIDs)
        {
            if (!validIDs.Any())
                return new List<HRC_Product>();

            return await _bkmis11.HRC_Products
                .Where(p => validIDs.Contains(p.ProductName))
                .ToListAsync();
        }

        private HRC_CertificateResponseDto BuildResponse(
            HRC_ProductRequestDto request,
            List<PhieuXuatHang_HRC> phieuXuatList,
            List<HRC_Product> productList)
        {
            var first      = phieuXuatList.FirstOrDefault();
            var configKeys = DefaultConfig.Split(';');

            // Build HPDQ_Data__c: mỗi HRC_Product → 1 Dictionary, chỉ trả đúng thành phần trong config
            var dataItems = productList.Select((p, index) =>
            {
                var obj = new Dictionary<string, object?>
                {
                    ["stt"]                  = index + 1,
                    ["coil_no"]              = p.ProductName,
                    ["thickness_mm"]         = p.Thick,
                    ["width_mm"]             = p.Width, 
                    ["length"]               = p.Length,
                    ["weight_kg"]            = p.Weight,
                    ["heat_no"]              = p.BilletLotname,
                    ["yield_strength_MPa"]   = p.Yeild,
                    ["tensile_strength_MPa"] = p.Tensile,
                    ["elongation_pct"]       = p.Elongation,
                    ["hardness_HRB"]         = p.HRB,
                    ["impact_energy"]     = p.ImpactEnergy,
                    ["chemical_composition"]  = p.ChemicalDetail,
                    ["bending_test"]         = p.BendTest,
                };

                foreach (var key in configKeys)
                {
                    double? val = _chemicalMap.TryGetValue(key, out var getter) ? getter(p) : null;
                    obj[key] = val;
                }

                obj["remarks"] = "";
                return obj;
            }).ToList();

            var Name = $"Phiếu chứng nhận chất lượng - {first?.SO} - {first?.PartnerName}";

            return new HRC_CertificateResponseDto
            {
                Name = Name,
                HPDQ_Certificate_No__c    = $"0000-07{DateTime.Now:MMyy}/HOAPHAT",
                HPDQ_Issue_Date__c        = DateTime.Now.ToString("yyyy-MM-dd"),
                HPDQ_Project__c           = first?.PartnerName,
                HPDQ_Grade__c             = first?.GradeCode,
                HPDQ_SAP_Customer_Code__c = request.CustomerCode,
                HPDQ_Standard__c          = first?.StandardCode,
                HPDQ_Contract__c          = first != null ? $"{first.SO} - {first.PurchaseOrderCode}" : null,
                HPDQ_Total_Weight__c      = phieuXuatList.Sum(p => p.Weight ?? 0),
                HPDQ_Total_Coils__c       = request.ListID.Count,
                HPDQ_Configuration__c     = DefaultConfig,
                HPDQ_SO                   = first?.SO,
                HPDQ_Transport            = first?.Transporter,
                EndUser                   = request.EndUser,
                HPDQ_Data__c              = dataItems
            };
        }
    }
}
