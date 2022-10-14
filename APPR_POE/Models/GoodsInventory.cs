using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class GoodsInventory
    {
        [Key]
        public int id { get; set; }
        public string category { get; set; }
        public int quantity { get; set; }
    }
}
