using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Data
{
    public class ContentTreeNode
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Segment { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string TreeNodeId { get; set; }
        public string ActionId { get; set; }
    }
}
