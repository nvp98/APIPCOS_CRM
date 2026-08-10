using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
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

        public HRC_ProductController(Bkmis11_Context bkmis11, Bkmis13_Context bkmis13)
        {
            _repository = new HRC_ProductRepository(bkmis11, bkmis13);
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
