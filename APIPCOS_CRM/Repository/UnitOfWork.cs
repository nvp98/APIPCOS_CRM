using APIPCOS_CRM.Data;

namespace APIPCOS_CRM.Repository
{
    public class UnitOfWork
    {
        private PLCOS_Context context;
        private UserRepository user;
        private VwVesselRepository vwVessel;
        private ScheduleBerthRebository scheduleBerth;
        private VOYAGEPORTSRebository vOYAGEPORTS;


        public UnitOfWork(PLCOS_Context _context)
        {
            context = _context;
        }
        public UserRepository UserRepository
        {
            get
            {
                return this.user ?? new UserRepository(context);
            }
        }
        public VwVesselRepository VwVesselRepository
        {
            get
            {
                return this.vwVessel ?? new VwVesselRepository(context);
            }
        }
        public ScheduleBerthRebository ScheduleBerth
        {
            get
            {
                return this.scheduleBerth ?? new ScheduleBerthRebository(context);
            }
        }
        public VOYAGEPORTSRebository VOYAGEPORTS
        {
            get
            {
                return this.vOYAGEPORTS ?? new VOYAGEPORTSRebository(context);
            }
        }
    }
}
