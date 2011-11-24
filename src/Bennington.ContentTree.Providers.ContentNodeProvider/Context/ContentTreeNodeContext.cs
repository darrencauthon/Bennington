using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Context
{
	public interface IContentTreeNodeContext
	{
		IEnumerable<ContentTreePageNode> GetContentTreeNodesByTreeId(string nodeId);
	}

	public class ContentTreeNodeContext : IContentTreeNodeContext
	{
        public const string RootNodeId = ContentTree.RootNodeId;
		
		private readonly IContentTreeNodeVersionContext contentTreeNodeVersionContext;

		public ContentTreeNodeContext(IContentTreeNodeVersionContext contentTreeNodeVersionContext)
		{
			this.contentTreeNodeVersionContext = contentTreeNodeVersionContext;
		}

		public IEnumerable<ContentTreePageNode> GetContentTreeNodesByTreeId(string treeNodeId)
		{
			var contentTreeNodes = contentTreeNodeVersionContext.GetAllContentTreeNodes().Where(a => a.Id == treeNodeId);
			return contentTreeNodes.ToArray();
		}
	}
}
