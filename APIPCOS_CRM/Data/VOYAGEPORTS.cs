namespace APIPCOS_CRM.Data
{
    public class VOYAGEPORTS
    {
        public Guid? ROW_ID{get;set;}
        public DateTime? UPDATE_TIME{get;set;}
        public DateTime? UPDATE_STAFF{get;set;}
        public string? CREATE_STAFF{get;set;}
        public DateTime? CREATE_TIME{get;set;}
        public string? LOADING_PORT{get;set;}
        public string? DISCHARGE_PORT{get;set;}
        public string SO{get;set;}
        public DateTime ETA{get;set;}
        public Guid? ROW_ID_BERTHPLAN { get; set; }

    }
}
