using System.Collections.Generic;
using System.Text;

namespace Bennington.ContentTree.Helpers
{
	public interface ITreeNodeIdToUrl
	{
		string GetUrlByTreeNodeId(string treeNodeId);
	}

	public class TreeNodeIdToUrl : ITreeNodeIdToUrl
	{
		private readonly IContentTree contentTree;

		public TreeNodeIdToUrl(IContentTree contentTree)
		{
			this.contentTree = contentTree;
		}

		public string GetUrlByTreeNodeId(string treeNodeId)
		{
			var contentTreeNode = contentTree.GetById(treeNodeId);
			if (contentTreeNode == null) return null;

			var segments = new List<string>();
			do
			{
				segments.Add(contentTreeNode.UrlSegment);
				contentTreeNode = contentTree.GetById(contentTreeNode.ParentTreeNodeId);
			} while (contentTreeNode != null);


			segments.Reverse();
			var stringBuilder = new StringBuilder("/");
			foreach(var segment in segments)
			{
				stringBuilder.Append(segment + "/");
			}

			var url = stringBuilder.ToString();
			return url.Substring(0, url.Length - 1);
		}
	}
}