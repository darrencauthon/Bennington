using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Cms.ReportManagement.Controllers;
using MvcTurbine.Routing;

namespace Bennington.Cms.ReportManagement.Routing
{
    public class ReportManagementRouting : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("ReportManagement", "ReportManagement/{action}", new { controller = typeof(ReportManagementController).Name.Replace("Controller", string.Empty), action = "Index" });
        }
    }
}
