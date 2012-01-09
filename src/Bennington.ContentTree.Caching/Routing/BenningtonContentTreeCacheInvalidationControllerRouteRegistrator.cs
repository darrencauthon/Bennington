using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.Caching.Routing
{
    public class BenningtonContentTreeCacheInvalidationControllerRouteRegistrator : IRouteRegistrator
    {
        public void Register(RouteCollection routes)
        {
            routes.MapRoute("BenningtonContentTreeCacheInvalidationControllerRouteRegistrator", "InvalidateCache/{guid}/{cacheKey}", new { controller = "BenningtonContentTreeCacheInvalidation", action = "Invalidate", cacheKey = "UNKNOWN" });
        }
    }
}
