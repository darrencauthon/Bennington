using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Routing
{
    public class ContentTreeRouteRegistrator : IRouteRegistrator
    {
		public const int MaxDepthForContentTreeUrlSegments = 25;

    	public void Register(RouteCollection routes)
        {
            routes.MapRoute(
                "ContentTreeNode", 
                "ContentTreeNode/{action}", 
                new { controller = "ContentTreeNode", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}