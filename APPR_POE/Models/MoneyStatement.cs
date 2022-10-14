using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class MoneyStatement
    {
        [Key]
        public int id { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public double amount { get; set; }
        public string type { get; set; }
        public string donor { get; set; }
    }
}
