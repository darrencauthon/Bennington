using System;

namespace Bennington.Content
{
    [Serializable]
    public class ContentNode
    {
        public object Id { get; set; }
        public object ParentId { get; set; }
        public string Segment { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
    }
}