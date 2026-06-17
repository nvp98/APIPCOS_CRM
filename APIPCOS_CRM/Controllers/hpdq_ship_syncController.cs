using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using APIPCOS_CRM.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using X.PagedList.Extensions;

namespace APIPCOS_CRM.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[Controller]")]
    [EnableCors("AllowSpecificOrigins")]
    public class hpdq_ship_syncController : Controller
    {

        private UnitOfWork unitOfWork;
        public hpdq_ship_syncController(PLCOS_Context dbContext)
        {
            unitOfWork = new UnitOfWork(dbContext);
        }
        [HttpGet]
        public async Task<IActionResult> get(DateTime? date, int? offset, int? limit)
        {
            date = date ?? DateTime.Now;
            offset = offset ?? 1;
            limit = limit ?? 10;
            var data = unitOfWork.VwVesselRepository.GetAll().Result
                               .Where(x => x.UpdateTime.HasValue && x.UpdateTime.Value.Date == date.Value.Date)
                              .ToPagedList((int)offset, (int)limit);

            return await Task.FromResult(Ok(new
            {
                VESSELS = data,
                CurrentPage = data.PageNumber,
                PageSize = data.PageSize,
                TotalPages = data.PageCount,
                TotalCount = data.TotalItemCount,
                HasNextPage = data.HasNextPage,
                HasPreviousPage = data.HasPreviousPage,
                IsFirstPage = data.IsFirstPage,
                IsLastPage = data.IsLastPage,
                FirstItemIndex = data.FirstItemOnPage,
                LastItemIndex = data.LastItemOnPage
            }));
        }
        [HttpGet("getInforVessel")]
        public async Task<IActionResult> getInForBerth(string? id, int? offset, int? limit)
        {   
            offset = offset ?? 1;
            limit = limit ?? 10;
            var res = await unitOfWork.VwVesselRepository.VesselInfor(id);
            var data=res.ToPagedList((int)offset, (int)limit);

            return await Task.FromResult(Ok(new
            {
                VESSELS = data,
                CurrentPage = data.PageNumber,
                PageSize = data.PageSize,
                TotalPages = data.PageCount,
                TotalCount = data.TotalItemCount,
                HasNextPage = data.HasNextPage,
                HasPreviousPage = data.HasPreviousPage,
                IsFirstPage = data.IsFirstPage,
                IsLastPage = data.IsLastPage,
                FirstItemIndex = data.FirstItemOnPage,
                LastItemIndex = data.LastItemOnPage
            }));
        }
        [HttpGet("GetScheduleBerth")]
        public async Task<IActionResult> GetScheduleBerth(DateTime fromDate, DateTime toDate, int? offset, int? limit)
        {
            if (fromDate.Date <= toDate.Date)
            {
                    toDate = toDate.AddDays(1).AddTicks(-1);                
            }
            else
            {
                return BadRequest(" Ngày bắt đầu phải bé hơn hoặc bằng ngày kết thúc");
            }
            offset = offset ?? 1;
            limit = limit ?? 10;
            var res = await unitOfWork.ScheduleBerth.GetByDate(fromDate, toDate);
            if (res.status == 1)
            {
                foreach (var item in res.datas as List<ScheduleBerthModel>)
                {
                    var data1 = await unitOfWork.VOYAGEPORTS.FindByIDSchedule(item.ROW_ID);
                    item.VOYAGEPORTSMDModels = data1;
                }
                var data = res.datas.ToPagedList((int)offset, (int)limit);
                return Ok(new
                {
                    status = res.status,
                    messenger = res.msg,
                    data = new
                    {
                        VESSELS = data,
                        CurrentPage = data.PageNumber,
                        PageSize = data.PageSize,
                        TotalPages = data.PageCount,
                        TotalCount = data.TotalItemCount,
                        HasNextPage = data.HasNextPage,
                        HasPreviousPage = data.HasPreviousPage,
                        IsFirstPage = data.IsFirstPage,
                        IsLastPage = data.IsLastPage,
                        FirstItemIndex = data.FirstItemOnPage,
                        LastItemIndex = data.LastItemOnPage
                    }
                });
            }
            else
            {
                return BadRequest(new
                {
                    status = res.status,
                    messenger = res.msg,
                    data = res.datas
                });
            }
        }
    }
}
