namespace Bennington.ContentTree.Data
{
	public class TreeNode
	{
		public string TreeNodeId { get; set; }
		public string ParentTreeNodeId {get; set;}
		public string Type {get; set;}
        public string ControllerName { get; set; }
	}
}