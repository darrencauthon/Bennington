using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Bennington.EmailCommunication
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