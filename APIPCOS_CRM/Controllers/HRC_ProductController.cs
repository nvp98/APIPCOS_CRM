using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

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
    }
}
