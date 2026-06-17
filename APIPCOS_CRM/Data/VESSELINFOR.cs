using System.ComponentModel.DataAnnotations;

namespace APIPCOS_CRM.Data
{
    public class VESSELINFOR
    {
        [Key]
        public Guid RowId { get; set; }

        public string? UpdateStaff { get; set; }

        public DateTime? UpdateTime { get; set; }

        [Required]
        [MaxLength(20)]
        public string VesselId { get; set; }

        [MaxLength(30)]
        public string? VesselName { get; set; }

        [MaxLength(20)]
        public string? CustomerId { get; set; }

        [MaxLength(200)]
        public string? Owner { get; set; }

        public decimal? NetWeight { get; set; }

        public decimal? GrossWeight { get; set; }

        public decimal? DeadWeight { get; set; }

        public decimal? LoadedDisplacement { get; set; }

        public decimal? LOA { get; set; }

        public decimal? LBP { get; set; }

        public decimal? Beam { get; set; }

        public decimal? MaxDraft { get; set; }

        public decimal? Depth { get; set; }

        [MaxLength(100)]
        public string? Flag { get; set; }

        [MaxLength(200)]
        public string? Remark { get; set; }

        [MaxLength(20)]
        public string? CallSign { get; set; }

        public int? HatchQty { get; set; }

        public int? CraneQty { get; set; }

        [MaxLength(10)]
        public string? InmarsatNo { get; set; }

        [MaxLength(10)]
        public string? LloydNo { get; set; }

        public int? RowsOfDeck { get; set; }

        public int? RowsOfHold { get; set; }

        public decimal? Tiers { get; set; }

        public int? MaxTeu { get; set; }

        public decimal? AntennaHeight { get; set; }

        [MaxLength(20)]
        public string? VesselType { get; set; }

        [MaxLength(20)]
        public string? VesselType2 { get; set; }

        [MaxLength(20)]
        public string? VesselType3 { get; set; }

        [MaxLength(20)]
        public string? EnterpriseId { get; set; }

        public string? FileUpload { get; set; }

        public string? FileName { get; set; }

        [MaxLength(40)]
        public string? IMO { get; set; }

        public string? GeneralArrangementUpload { get; set; }

        public string? GeneralArrangementName { get; set; }

        public string? ShipParticularUpload { get; set; }

        public string? ShipParticularName { get; set; }

        [MaxLength(20)]
        public string? VesselSizeId { get; set; }

        public decimal? TPC { get; set; }

        [MaxLength(4)]
        public string? ShipBuildingYear { get; set; }
    }
}
