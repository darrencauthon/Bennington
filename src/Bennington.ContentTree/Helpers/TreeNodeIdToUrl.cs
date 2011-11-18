using System.Collections.Generic;
using System.Text;
using Bennington.ContentTree.Contexts;

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
			var treeNodeSummary = contentTree.GetTreeNodeSummaryByTreeNodeId(treeNodeId);
			if (treeNodeSummary == null) return null;

			var segments = new List<string>();
			do
			{
				segments.Add(treeNodeSummary.UrlSegment);
				treeNodeSummary = contentTree.GetTreeNodeSummaryByTreeNodeId(treeNodeSummary.ParentTreeNodeId);
			} while (treeNodeSummary != null);


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