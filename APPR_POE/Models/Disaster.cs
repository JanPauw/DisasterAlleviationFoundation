using System.ComponentModel.DataAnnotations;

namespace APPR_POE.Models
{
    public class Disaster
    {
        [Key]
        public int id { get; set; }
        [DataType(DataType.Date)]
        public DateTime start_date { get; set; }
        [DataType(DataType.Date)]
        public DateTime end_date { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string aid_types { get; set; }
        public string created_by { get; set; }
    }
}
