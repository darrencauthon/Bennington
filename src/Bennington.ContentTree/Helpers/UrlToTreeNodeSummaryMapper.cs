using System;
using System.Linq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Helpers
{
	public interface IUrlToTreeNodeSummaryMapper
	{
		TreeNodeSummary CreateInstance(string rawUrl);
	}

	public class UrlToTreeNodeSummaryMapper : IUrlToTreeNodeSummaryMapper
	{
		private readonly IContentTree contentTree;

		public UrlToTreeNodeSummaryMapper(IContentTree contentTree)
		{
			this.contentTree = contentTree;
		}

		public TreeNodeSummary CreateInstance(string rawUrl)
		{
			var nodeSegments = ScrubUrlAndReturnEnumerableOfNodeSegments(rawUrl);

			TreeNodeSummary treeNodeSummary = null;

			var workingTreeNodeId = Constants.RootNodeId;
			foreach(var nodeSegment in nodeSegments)
			{
				treeNodeSummary = contentTree.GetChildren(workingTreeNodeId).Where(a => string.Equals(a.UrlSegment, nodeSegment, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
				if (treeNodeSummary == null) return null;
				if (treeNodeSummary.MayHaveChildNodes == false) return treeNodeSummary;
				workingTreeNodeId = treeNodeSummary.Id;
			}

			return treeNodeSummary;
		}

		private static string[] ScrubUrlAndReturnEnumerableOfNodeSegments(string rawUrl)
		{
			if (rawUrl == null) rawUrl = string.Empty;
			if ((rawUrl.StartsWith("/")) && (rawUrl.Length > 1)) rawUrl = rawUrl.Substring(1, rawUrl.Length - 1);
			if (rawUrl.Contains("?"))
				rawUrl = rawUrl.Split('?')[0];
			return rawUrl.Split('/');
		}

	}
}