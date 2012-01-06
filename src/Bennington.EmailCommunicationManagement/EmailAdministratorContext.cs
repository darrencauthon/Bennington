using System.Web;

namespace Bennington.EmailCommunicationManagement
{
    public interface IEmailAdministratorContext
    {
        bool IsThisUserASuperUser();
    }

    public class EmailAdministratorContext : IEmailAdministratorContext
    {
        public bool IsThisUserASuperUser()
        {
            return string.Equals(HttpContext.Current.User.Identity.Name, "admin");
        }
    }
}