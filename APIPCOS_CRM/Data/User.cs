using System.ComponentModel.DataAnnotations;

namespace APIPCOS_CRM.Data
{
    public class User
    {
        [Key]
        public string username { get; set; }
        public string? password { get; set; }
        public int? status { get; set; }
    }
}
