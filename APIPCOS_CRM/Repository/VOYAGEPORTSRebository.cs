using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIPCOS_CRM.Repository
{
    public class VOYAGEPORTSRebository : IRepository<VOYAGEPORTSMDModel>
    {
        private PLCOS_Context context;
        public VOYAGEPORTSRebository(PLCOS_Context context)
        {
            this.context = context;
        }
        public Task<MsgModel> Create(VOYAGEPORTSMDModel model)
        {
            throw new NotImplementedException();
        }

        public Task<MsgModel> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<VOYAGEPORTSMDModel> FindByID(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<VOYAGEPORTSMDModel>> FindByIDSchedule(Guid RowID)
        {
            var res = await context.VOYAGEPORTS
            .Where(x => x.ROW_ID_BERTHPLAN == RowID)
            .Select(y => new VOYAGEPORTSMDModel
            {
                ROW_ID = y.ROW_ID,
                 DISCHARGE_PORT=y.DISCHARGE_PORT,
                 LOADING_PORT=y.LOADING_PORT,
                 ETA = y.ETA,
                 SO=y.SO,
                 ROW_ID_BERTHPLAN=y.ROW_ID_BERTHPLAN,
            }).ToListAsync();
            return res;
        }
            public Task<List<VOYAGEPORTSMDModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<MsgModel> Update(VOYAGEPORTSMDModel model)
        {
            throw new NotImplementedException();
        }
    }
}
