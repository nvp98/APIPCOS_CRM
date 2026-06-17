using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace APIPCOS_CRM.Repository
{
    public class ScheduleBerthRebository : IRepository<ScheduleBerthModel>
    {
        private PLCOS_Context context;

        public ScheduleBerthRebository(PLCOS_Context context)
        {
            this.context = context;
        }
        public Task<MsgModel> Create(ScheduleBerthModel model)
        {
            throw new NotImplementedException();
        }

        public Task<MsgModel> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduleBerthModel> FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ScheduleBerthModel>> GetAll()
        {
            throw new NotImplementedException();
        }
        public async Task<MsgModel> GetByDate(DateTime fromDate,DateTime toDate)
        {
            MsgModel msg = new MsgModel();
            try
            {
                var fromDayParam = new SqlParameter("@fromDay", SqlDbType.DateTime)
                {
                    Value = fromDate
                };
                var toDayParam = new SqlParameter("@toDay", SqlDbType.DateTime)
                {
                    Value = toDate
                };

                // Define SQL query with parameters
                var query = "EXEC sp_schedule_berth @fromDay, @toDay";

                // Execute raw SQL query with parameters
                /*  string a =  context.ScheduleBerth
                      .FromSqlRaw(query, fromDayParam, toDayParam)
                      .ToQueryString();*/
                var res = await context.ScheduleBerth
                    .FromSqlRaw(query, fromDayParam, toDayParam)
                    .ToListAsync();
                msg.status = 1;
                msg.msg = "success";
                msg.datas = res;
                return await Task.FromResult(msg);
            }catch (Exception ex)
            {
                msg.status = 0;
                msg.msg = ex.Message;
                
                return await Task.FromResult(msg);
            }
        }

        public Task<MsgModel> Update(ScheduleBerthModel model)
        {
            throw new NotImplementedException();
        }
    }
}
