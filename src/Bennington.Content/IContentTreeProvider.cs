using System;
using System.Web.Routing;
using Bennington.Content.Routing;

namespace Bennington.Content
{
    public interface IContentTreeProvider : IDisposable
    {
        event EventHandler<EventArgs> ContentChanged;
        void Refresh();
        ContentRouteTree GetRouteTree(Route route, IRouteHandler routeHandler);
    }
}