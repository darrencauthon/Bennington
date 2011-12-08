using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Routing
{
    public class ContentControllerDisplayRouteRegistrator : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("ContentDisplayRoute", GetType().Name, new {Controller = "ContentTree", Action = "Display"});
            routes.MapRoute("ContentDisplayMetaRoute", GetType().Name, new { Controller = "ContentTree", Action = "DisplayMeta" });
        }
    }
}
