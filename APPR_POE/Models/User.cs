using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class User
    {
        [Key]
        [DisplayName("Email")]
        public string email { get; set; }
        [DisplayName("Password")]
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
    }
}
