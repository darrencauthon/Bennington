namespace Bennington.ContentTree.Data
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
