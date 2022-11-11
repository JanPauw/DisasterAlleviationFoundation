using APPR_POE.Models;

namespace APPR_POE.Data
{
    public class Validation
    {
        //Test Valid Date Entries
        public bool IsValidDate(DateTime date)
        {
            Disaster testDisaster = new Disaster();

            try
            {
                testDisaster.start_date = date;
                testDisaster.end_date = date;

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
