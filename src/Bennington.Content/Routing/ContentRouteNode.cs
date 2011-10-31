using System;

namespace Bennington.Content.Routing
{
    [Serializable]
    public class ContentRouteNode
    {
        public object Id { get; set; }
        public object ParentId { get; set; }
        public string Segment { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}