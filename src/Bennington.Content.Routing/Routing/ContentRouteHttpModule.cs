using System;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Bennington.Content.Data;

namespace Bennington.Content.Routing
{
    public class ContentRouteHttpModule : IHttpModule
    {
        private readonly IContentTreeProvider contentTreeProvider;
        private static volatile bool changePending;
        private readonly object syncLock = new object();

        public ContentRouteHttpModule(IContentTreeProvider contentTreeProvider)
        {
            this.contentTreeProvider = contentTreeProvider;
        }

        public void Init(HttpApplication context)
        {
            AddContentRouteToRouteTable();
            contentTreeProvider.ContentChanged += OnContentChanged;
            context.EndRequest += OnEndRequest;
        }

        private void OnEndRequest(object sender, EventArgs e)
        {
            if(!changePending) return;

            lock (syncLock)
            {
                if(!changePending) return;
                AddContentRouteToRouteTable();
                changePending = false;
            }
        }

        private void AddContentRouteToRouteTable()
        {
            using (RouteTable.Routes.GetWriteLock())
            {
                var contentRoute = RouteTable.Routes.SingleOrDefault(route => route is ContentRoute);
                RouteTable.Routes.Remove(contentRoute);
                RouteTable.Routes.Add(new ContentRoute(contentTreeProvider));
            }
        }

        private static void OnContentChanged(object sender, EventArgs e)
        {
            changePending = true;
        }

        public void Dispose()
        {
            contentTreeProvider.ContentChanged -= OnContentChanged;
        }
    }
}