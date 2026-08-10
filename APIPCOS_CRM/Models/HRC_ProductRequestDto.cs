namespace APIPCOS_CRM.Models
{
    public class HRC_ProductRequestDto
    {
        public string SO { get; set; } = null!;
        public string CustomerCode { get; set; } = null!;
        public List<string> ListID { get; set; } = new();
        public string? Transporter { get; set; } = null!;
        public string? EndUser { get; set; } = null!;
    }
}
