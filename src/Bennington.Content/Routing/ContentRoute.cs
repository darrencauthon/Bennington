using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Bennington.Content.Routing
{
    public class ContentRoute : Route
    {
        private readonly ContentRouteTree contentRouteTree;

        public ContentRoute(IContentTreeProvider contentTreeProvider)
            : base(string.Empty, new MvcRouteHandler())
        {
            contentRouteTree = contentTreeProvider.GetRouteTree(this, RouteHandler);
            Url = contentRouteTree.GetUrlPattern();
            Constraints = contentRouteTree.GetConstraints();
            Defaults = contentRouteTree.GetDefaults();
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            return contentRouteTree.GetRouteData(httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var routeValues = new RouteValueDictionary(values);
            var action = (string)routeValues["action"];
            var controller = (string)routeValues["controller"];
            var routeData = contentRouteTree.GetRouteData(action, controller);

            if(routeData == null)
                return null;

            routeValues.Remove("controller");
            routeValues.Remove("action");

            return base.GetVirtualPath(new RequestContext(requestContext.HttpContext, routeData), routeValues);
        }
    }
}