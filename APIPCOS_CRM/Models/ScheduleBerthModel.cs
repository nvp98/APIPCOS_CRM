namespace APIPCOS_CRM.Models
{
    public class ScheduleBerthModel
    {
        public Guid ROW_ID { get; set; }
        public string? VESSEL_ID { get; set; }
        public string? VESSEL_NAME { get; set; }
        public string? CALL_SEQ { get; set; }
        public string? CALL_YEAR { get; set; }
        public string? ENTERPRISE_ID { get; set; }
        public string? CARGO_NAME { get; set; }
        public string? CARGO_TYPE { get; set; }
        public decimal? CARGO_WEIGHT { get; set; }
        public string? VESSEL_SIZE_NAME { get; set; }
        public decimal? FROM_CARGO_WEIGHT { get; set; }
        public decimal? TO_CARGO_WEIGHT { get; set; }
        public string? CATEGORY_CLASS { get; set; }
        public string? PURCHASE_ORDER_NO { get; set; }
        public string? CUSTOMS_DECLARATION_NO { get; set; }
        public string? LAST_PORT { get; set; }
        public string? NEXT_PORT { get; set; }
        public decimal? NORM { get; set; }
        public string? CHARTER_PARTY_NAME { get; set; }
        public string? BL_NO { get; set; }
        public string? DELIVERY_METHOD_FOR_WEIGHT { get; set; }
        public DateTime? CARGO_READINESS_DATE { get; set; }
        public string? INSPECTION_QUALITY_COMPANY { get; set; }
        public string? INSPECTION_WEIGH_COMPANY { get; set; }
        public bool? IS_INSPECTION { get; set; }
        public DateTime? LATEST_SHIPMENT_DATE { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ETB { get; set; }
        public DateTime? ETC { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETW { get; set; }
        public string? SHIPPING_AGENCY_NAME { get; set; }
        public string? IMO { get; set; }
        public string? CALL_SIGN { get; set; }
        public decimal? ARRIVAL_DRAUGHT { get; set; }
        public decimal? AIR_DRAUGHT_IN { get; set; }
        public decimal? AIR_DRAUGHT_OUT { get; set; }
        public DateTime? ATA { get; set; }
        public DateTime? ATB { get; set; }
        public DateTime? ATC { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATW { get; set; }
        public string? INBOUND_DELIVERY_NO { get; set; }
        public string? SALES_ORDER_NO { get; set; }
        public string? Subscribe_Confirm { get; set; }
        public string? CREATE_BY { get; set; }
        public string? UPDATE_STAFF { get; set; }
        public DateTime? UPDATE_TIME { get; set; }
        public List<VOYAGEPORTSMDModel> VOYAGEPORTSMDModels { get; set; }

    }

    
}
