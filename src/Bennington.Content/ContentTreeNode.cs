using System.Collections.Generic;

namespace Bennington.Content
{
    public class ContentTreeNode
    {
        public ContentTreeNode(string segment, string action, string controller, object id, object parentId)
        {
            Segment = segment;
            Action = action;
            Controller = controller;
            Id = id;
            ParentId = parentId;
        }

        public string[] GetPath()
        {
            var segments = new List<string> { Segment };
            var parent = Parent;

            while (parent != null)
            {
                segments.Add(parent.Segment);
                parent = parent.Parent;
            }

            segments.Reverse();
            return segments.ToArray();
        }

        public object Id { get; set; }
        public object ParentId { get; set; }
        public ContentTreeNode Parent { get; set; }
        public string Action { get; private set; }
        public string Controller { get; private set; }
        public string Segment { get; private set; }
    }
}