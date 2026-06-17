namespace APIPCOS_CRM.Data
{
    public class VwVessel
    {
        public string VesselId { get; set; } = null!;
        public string? VesselName { get; set; }
        public decimal NetWeight { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal DeadWeight { get; set; }
        public decimal Loa { get; set; }
        public decimal Lbp { get; set; }
        public decimal Beam { get; set; }
        public string? VesselSizeId { get; set; }
        public string? VesselSizeName { get; set; }
        public string? UpdateStaff { get; set; }
        public DateTime? UpdateTime { get; set; }
        public decimal FromCargoWeight { get; set; }
        public decimal ToCargoWeight { get; set; }
    }
}
