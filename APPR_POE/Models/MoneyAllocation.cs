using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class MoneyAllocation
    {
        [Key]
        public int id { get; set; }
        public double amount { get; set; }
        public int disaster_id { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
    }
}
