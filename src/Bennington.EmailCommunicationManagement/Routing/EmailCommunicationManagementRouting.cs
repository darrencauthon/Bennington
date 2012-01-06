using System.Web.Mvc;
using System.Web.Routing;
using Bennington.EmailCommunicationManagement.Controllers;
using MvcTurbine.Routing;

namespace Bennington.EmailCommunicationManagement.Routing
{
    public class EmailCommunicationManagementRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("EmailCommunicationManagement", "EmailManagement/{action}", new { controller = typeof(EmailCommunicationManagementController).Name.Replace("Controller", string.Empty), action = "Index" });
        }
    }
}