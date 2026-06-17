using System.ComponentModel.DataAnnotations;

namespace APIPCOS_CRM.Data
{
    public class PhieuXuatHang_HRC
    {
        public int TicketID { get; set; }
        public string? TicketCode { get; set; }
        public string? TicketName { get; set; }
        public DateTime IssueDate { get; set; }
        public string? TicketTypeName { get; set; }
        public int? PartnerID { get; set; }
        public string? PartnerName { get; set; }
        public string? Address { get; set; }
        public string? TruckNo { get; set; }
        public string? Transporter { get; set; }
        public string? DeliveryOrderCode { get; set; }
        public string? PurchaseOrderCode { get; set; }
        public string? SO { get; set; }
        public double? InWeight { get; set; }
        public double? OutWeight { get; set; }
        public double NetWeight { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string? ProductName { get; set; }
        public string? SAPCode { get; set; }
        public string? SAPDescription { get; set; }
        public string? SIZE_SX_SO { get; set; }
        public string? ProductLotName { get; set; }
        public string? GradeCode { get; set; }
        public string? BilletLotname { get; set; }
        public double? Weight { get; set; }
        public double? NumOfBar { get; set; }
        public int NumOfProduct { get; set; }
        public DateOnly? ProductionDate { get; set; }
        public string? ShiftName { get; set; }
        public string? StandardCode { get; set; }
    }
}
