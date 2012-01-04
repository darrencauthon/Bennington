using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.EmailCommunication.Controllers;
using MvcTurbine.Routing;

namespace Bennington.EmailCommunication.Routing
{
    public class EmailCommunicationManagementRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("EmailCommunicationManagement", "EmailManagement/{action}", new { controller = typeof(EmailCommunicationManagementController).Name.Replace("Controller", string.Empty), action = "Index" });
        }
    }
}