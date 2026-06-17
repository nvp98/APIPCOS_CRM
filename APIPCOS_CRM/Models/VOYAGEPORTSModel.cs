namespace APIPCOS_CRM.Models
{
    public class VOYAGEPORTSMDModel
    {
        public Guid? ROW_ID { get; set; }
        public string? DISCHARGE_PORT { get; set; }
        public string? LOADING_PORT { get; set; }
        public DateTime ETA { get; set; }
        public string SO { get; set; }
        public Guid? ROW_ID_BERTHPLAN { get; set; }
    }
}
