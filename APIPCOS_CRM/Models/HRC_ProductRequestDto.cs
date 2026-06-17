namespace APIPCOS_CRM.Models
{
    public class HRC_ProductRequestDto
    {
        public string SO { get; set; } = null!;
        public string? CustomerCode { get; set; }
        public List<string> ListID { get; set; } = new();
        public string Transporter { get; set; } = null!;
    }
}
