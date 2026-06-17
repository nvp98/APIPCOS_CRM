namespace APIPCOS_CRM.Models
{
    public class VesselShow
    {
        public string VesselId { get; set; }
        public string VesselName { get; set; }
        public string VesselType { get; set; }
        public decimal? LOA { get; set; }
        public decimal? BEAM { get; set; }
        public string IMO { get; set; }
        public string CALL_SIGN { get; set; }
    }
}
