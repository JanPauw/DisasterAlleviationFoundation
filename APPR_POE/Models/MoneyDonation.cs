using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class MoneyDonation
    {
        [Key]
        public int id { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public double amount { get; set; }
        public string email { get; set; }
        public bool anonymous { get; set; }
    }
}
