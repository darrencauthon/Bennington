using System;
using System.Collections.Generic;
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

            var url = GetUrlForTreeNode(treeNode);
            var stringBuilder = new StringBuilder();
            var count = 0;
            foreach (var routeValueKey in values.Keys)
            {
                if (routeValueKey.Equals("controller", StringComparison.CurrentCultureIgnoreCase)) continue;
                if (routeValueKey.Equals("action", StringComparison.CurrentCultureIgnoreCase)) continue;
                stringBuilder.Append(string.Format("{0}{1}={2}", count > 0 ? "&" : string.Empty, routeValueKey, routeValues[routeValueKey]));
                count++;
            }
            if (!string.IsNullOrEmpty(stringBuilder.ToString()))
                url = url + "?" + stringBuilder.ToString();
            return new VirtualPathData(null, url);

            //routeValues.Remove("controller");
            //routeValues.Remove("action");

            //return base.GetVirtualPath(new RequestContext(requestContext.HttpContext, routeData), routeValues);
        }

        private string GetUrlForTreeNode(ContentTreeNode contentTreeNode)
        {
            var segments = contentTreeNode.GetPath();
            var sb = new StringBuilder();
            for(var n = 0; n < segments.Count(); n++)
            {
                sb.Append(segments[n]);
                if (n < segments.Count() - 1) sb.Append("/");
            }
            return sb.ToString();
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

            routeData.Values.Add("TreeNodeId", treeNode.TreeNodeId);
            routeData.Values.Add("ActionId", treeNode.ActionId);
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