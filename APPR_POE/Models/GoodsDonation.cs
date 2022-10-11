using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class GoodsDonation
    {
        [Key]
        public int id { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public int quantity { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public bool anonymous { get; set; }
    }
}
