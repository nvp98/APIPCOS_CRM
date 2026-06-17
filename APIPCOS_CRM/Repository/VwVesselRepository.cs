using APIPCOS_CRM.Data;
using APIPCOS_CRM.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPCOS_CRM.Repository
{
    public class VwVesselRepository : IRepository<VwVesselModel>
    {
        private PLCOS_Context context;
        public VwVesselRepository(PLCOS_Context _context)
        {
            this.context = _context;
        }
        public Task<MsgModel> Create(VwVesselModel model)
        {
            throw new NotImplementedException();
        }

        public Task<MsgModel> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<VwVesselModel> FindByID(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<VesselShow>> VesselInfor(string? id)
        {
            var data = await context.VESSELS.Select(x=>new VesselShow() { CALL_SIGN=x.CALL_SIGN,LOA=x.LOA,BEAM=x.BEAM,IMO=x.IMO,VesselId=x.VESSEL_ID,VesselName=x.VESSEL_NAME,VesselType=x.VESSEL_TYPE2}).ToListAsync();

            if (data == null || data.Count == 0)
            {
                return new List<VesselShow>(); // Trả về danh sách rỗng nếu không có dữ liệu
            }

            return string.IsNullOrEmpty(id) ? data : data.Where(x => x.VesselId.ToLower() == id.Trim().ToLower()).ToList();
        }


        public async Task<List<VwVesselModel>> GetAll()
        {
            var data = await context.vw_vessels.Select(x => new VwVesselModel()
            {
                VesselId = x.VesselId,
                VesselName = x.VesselName,
                NetWeight = x.NetWeight,
                Beam = x.Beam,
                DeadWeight = x.DeadWeight,
                FromCargoWeight = x.FromCargoWeight,
                GrossWeight = x.GrossWeight,
                Lbp = x.Lbp,
                Loa = x.Loa,
                ToCargoWeight = x.ToCargoWeight,
                UpdateStaff = x.UpdateStaff,
                UpdateTime = x.UpdateTime,
                VesselSizeId = x.VesselSizeId,
                VesselSizeName = x.VesselSizeName
            }).ToListAsync();
            return data;
        }

        public Task<MsgModel> Update(VwVesselModel model)
        {
            throw new NotImplementedException();
        }
    }
}
