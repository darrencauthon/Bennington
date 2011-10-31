using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bennington.Content.Internal;

namespace Bennington.Content.Routing
{
    public class ContentRouteTree : IRouteConstraint
    {
        private readonly Route route;
        private readonly IRouteHandler routeHandler;
        private readonly TreeNode rootNode;

        public ContentRouteTree(IEnumerable<ContentRouteNode> nodes, Route route, IRouteHandler routeHandler)
        {
            this.route = route;
            this.routeHandler = routeHandler;
            rootNode = TreeNode.BuildTree(nodes);
        }

        public RouteData GetRouteData(string action, string controller)
        {
            var treeNode = rootNode.Find(action, controller);
            return treeNode == null ? null : GetRouteData(treeNode);
        }

        public RouteData GetRouteData(HttpContextBase httpContext)
        {
            var segments = GetRequestUrlSegments(httpContext);
            var treeNode = rootNode.FindPath(segments);

            return treeNode == null ? null : GetRouteData(treeNode, httpContext);
        }

        public RouteValueDictionary GetDefaults()
        {
            return new RouteValueDictionary(Enumerable.Range(0, rootNode.GetMaxDepth()).ToDictionary(depth => depth.ToString(), depth => (object)UrlParameter.Optional));
        }

        public RouteValueDictionary GetConstraints()
        {
            return new RouteValueDictionary {{"constraint", this}};
        }

        public string GetUrlPattern()
        {
            return Enumerable.Range(0, rootNode.GetMaxDepth()).Aggregate(new StringBuilder(), (builder, depth) => builder.AppendFormat("{{{0}}}/", depth), url => url.Remove(url.Length - 1, 1)).ToString();
        }

        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            return rootNode.PathExists(GetRequestUrlSegments(httpContext));
        }

        private static string[] GetRequestUrlSegments(HttpContextBase httpContext)
        {
            return httpContext.Request.AppRelativeCurrentExecutionFilePath.Substring(2).Split(new []{'/'}, StringSplitOptions.RemoveEmptyEntries);
        }

        private RouteData GetRouteData(TreeNode treeNode, HttpContextBase httpContext)
        {
            var routeData = GetRouteData(treeNode);
            var queryString = HttpUtility.ParseQueryString(httpContext.Request.Url.Query);
            queryString.CopyTo(routeData.Values);

            return routeData;
        }

        private RouteData GetRouteData(TreeNode treeNode)
        {
            var routeData = new RouteData(route, routeHandler);
            var segments = treeNode.GetPathSegments();

            for(var i = 0; i < segments.Length; i++)
                routeData.Values.Add(i.ToString(), segments[i]);

            routeData.Values.Add("action", treeNode.Value.Action);
            routeData.Values.Add("controller", treeNode.Value.Controller);

            return routeData;
        }
    }
}