namespace APIPCOS_CRM.Models
{
    public class HRC_GetDataResult
    {
        public HRC_CertificateResponseDto? Certificate { get; set; }
        public List<string> AlertIDs { get; set; } = new();
    }
}
