﻿using System;
using System.Linq;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Helpers
{
	public interface IUrlToTreeNodeSummaryMapper
	{
		ContentTreeNode CreateInstance(string rawUrl);
	}

	public class UrlToTreeNodeSummaryMapper : IUrlToTreeNodeSummaryMapper
	{
		private readonly IContentTree contentTree;

		public UrlToTreeNodeSummaryMapper(IContentTree contentTree)
		{
			this.contentTree = contentTree;
		}

		public ContentTreeNode CreateInstance(string rawUrl)
		{
			var nodeSegments = ScrubUrlAndReturnEnumerableOfNodeSegments(rawUrl);

			ContentTreeNode contentTreeNode = null;

			var workingTreeNodeId = ContentTree.RootNodeId;
			foreach(var nodeSegment in nodeSegments)
			{
				contentTreeNode = contentTree.GetChildren(workingTreeNodeId).Where(a => string.Equals(a.UrlSegment, nodeSegment, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
				if (contentTreeNode == null) return null;
				if (contentTreeNode.MayHaveChildNodes == false) return contentTreeNode;
				workingTreeNodeId = contentTreeNode.Id;
			}

			return contentTreeNode;
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