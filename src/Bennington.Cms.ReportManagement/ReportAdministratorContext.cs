using System.Web;

namespace Bennington.Cms.ReportManagement
{
    public interface IReportAdministratorContext
    {
        bool IsThisUserASuperUser();
    }

    public class ReportAdministratorContext : IReportAdministratorContext
    {
        public bool IsThisUserASuperUser()
        {
            return string.Equals(HttpContext.Current.User.Identity.Name, "admin");
        }
    }
}