using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class GoodsAllocation
    {
        [Key]
        public int id { get; set; }
        public int quantity { get; set; }
        public int disaster_id { get; set; }
        [DataType(DataType.Date)]
        public DateTime date { get; set; }
        public string category { get; set; }
    }
}
