using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Content.Data;

namespace Bennington.Content.Routing
{
    public class ContentRoute : Route, IRouteConstraint
    {
        private readonly ContentTree contentTree;

        public ContentRoute(IContentTreeProvider contentTreeProvider)
            : base(string.Empty, new MvcRouteHandler())
        {
            contentTree = contentTreeProvider.GetContentTree();
            Url = GetUrlPattern();
            Constraints = GetConstraints();
            Defaults = GetDefaults();
        }

        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            var segments = GetRequestUrlSegments(httpContext);
            var treeNode = contentTree.FindPath(segments);

            return treeNode == null ? null : GetRouteData(treeNode, httpContext);
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            var routeValues = new RouteValueDictionary(values);
            var action = (string)routeValues["action"];
            var controller = (string)routeValues["controller"];

            var treeNode = contentTree.Find(action, controller);
            var routeData = treeNode == null ? null : GetRouteData(treeNode);

            if(routeData == null)
                return null;

            routeValues.Remove("controller");
            routeValues.Remove("action");

            return base.GetVirtualPath(new RequestContext(requestContext.HttpContext, routeData), routeValues);
        }

        private static string[] GetRequestUrlSegments(HttpContextBase httpContext)
        {
            return httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2).Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private RouteData GetRouteData(ContentTreeNode treeNode, HttpContextBase httpContext)
        {
            var routeData = GetRouteData(treeNode);
            var queryString = HttpUtility.ParseQueryString(httpContext.Request.Url.Query);
            queryString.CopyTo(routeData.Values);

            return routeData;
        }

        private RouteData GetRouteData(ContentTreeNode treeNode)
        {
            var routeData = new RouteData(this, RouteHandler);
            var segments = treeNode.GetPath();

            for (var i = 0; i < segments.Length; i++)
                routeData.Values.Add(i.ToString(), segments[i]);

            routeData.DataTokens.Add("ContentId", treeNode.Id);
            routeData.Values.Add("action", treeNode.Action);
            routeData.Values.Add("controller", treeNode.Controller);

            return routeData;
        }

        public RouteValueDictionary GetDefaults()
        {
            return new RouteValueDictionary(Enumerable.Range(0, contentTree.MaxDepth).ToDictionary(depth => depth.ToString(), depth => (object)UrlParameter.Optional));
        }

        public RouteValueDictionary GetConstraints()
        {
            return new RouteValueDictionary { { "constraint", this } };
        }

        public string GetUrlPattern()
        {
            if (contentTree.MaxDepth == 0) return string.Empty;
            return Enumerable.Range(0, contentTree.MaxDepth).Aggregate(new StringBuilder(), (builder, depth) => builder.AppendFormat("{{{0}}}/", depth), url => url.Remove(url.Length - 1, 1)).ToString();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return contentTree.PathExists(GetRequestUrlSegments(httpContext));
        }
    }
}