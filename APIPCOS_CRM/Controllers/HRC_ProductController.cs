using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
using APIPCOS_CRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace APIPCOS_CRM.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    [EnableCors("AllowAllOrigins")]
    public class HRC_ProductController : ControllerBase
    {
        private readonly HRC_ProductRepository _repository;
        private readonly BkMisClient _bkMis;

        public HRC_ProductController(Bkmis11_Context bkmis11, Bkmis13_Context bkmis13, BkMisClient bkMis)
        {
            _repository = new HRC_ProductRepository(bkmis11, bkmis13);
            _bkMis      = bkMis;
        }

        [HttpPost("GetData")]
        public async Task<IActionResult> GetData([FromBody] HRC_ProductRequestDto request)
        {
            var result = await _repository.GetDataAsync(request);

            return Ok(new
            {
                status  = 1,
                message = "Success",
                data    = result.Certificate,
                alert   = result.AlertIDs
            });
        }

        /// <summary>
        /// Salesforce đẩy phiếu MTC HRC sang BK-MIS (nút "Đồng bộ BK-MIS" ở Bước 2).
        ///
        /// Đây là CẦU TRUNG CHUYỂN: body do Salesforce dựng đúng cấu trúc BK-MIS
        /// (maChungChi, ngayLap, …, lstChiTiet[]) và được chuyển đi NGUYÊN TRẠNG — không
        /// ánh xạ lại ở đây để logic ánh xạ chỉ nằm MỘT nơi (HPDQClass_BkMisSync bên Apex).
        /// Trả cùng khuôn {status, message} như GetData; `bk` là phản hồi gốc của BK-MIS
        /// để soi khi có lỗi.
        /// </summary>
        [HttpPost("PushCertificate")]
        public async Task<IActionResult> PushCertificate([FromBody] System.Text.Json.JsonElement body)
        {
            if (body.ValueKind != System.Text.Json.JsonValueKind.Object
                || !body.TryGetProperty("maChungChi", out var ma)
                || ma.ValueKind != System.Text.Json.JsonValueKind.String
                || string.IsNullOrWhiteSpace(ma.GetString()))
            {
                return BadRequest(new { status = 0, message = "Body phải là một object có maChungChi" });
            }

            try
            {
                var kq = await _bkMis.PushCertificateHrcAsync(body);
                return Ok(new
                {
                    status  = kq.Success ? 1 : 0,
                    message = kq.Success ? "Success" : (kq.Message ?? "BK-MIS từ chối, không rõ lý do"),
                    bk      = kq.Raw
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(500, new { status = 0, message = ex.Message });
            }
            catch (TaskCanceledException)
            {
                return StatusCode(504, new { status = 0, message = "BK-MIS không phản hồi trong thời gian cho phép" });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new { status = 0, message = "Không kết nối được BK-MIS: " + ex.Message });
            }
        }

        [HttpGet("GetTransporters")]
        public async Task<IActionResult> GetTransporters(int page = 1, int pageSize = HRC_ProductRepository.DefaultTransporterPageSize, string? searchText = null)
        {
            if (string.IsNullOrWhiteSpace(searchText) || searchText.Trim().Length < HRC_ProductRepository.MinTransporterSearchLength)
            {
                return BadRequest(new
                {
                    status  = 0,
                    message = $"searchText phải có ít nhất {HRC_ProductRepository.MinTransporterSearchLength} ký tự"
                });
            }

            try
            {
                var (items, totalCount) = await _repository.GetDistinctTransportersAsync(page, pageSize, searchText);

                return Ok(new
                {
                    status     = 1,
                    message    = "Success",
                    data       = items,
                    page,
                    pageSize,
                    totalCount,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
                });
            }
            catch (SqlException ex) when (ex.Number == -2)
            {
                return StatusCode(408, new
                {
                    status  = 0,
                    message = "Truy vấn quá thời gian cho phép, vui lòng nhập thêm ký tự để tìm kiếm nhanh hơn"
                });
            }
        }
    }
}
