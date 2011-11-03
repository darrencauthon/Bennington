using System;
using System.Collections.Generic;
using System.Linq;

namespace Bennington.Content
{
    public class ContentTree
    {
        private readonly List<ContentTreeNode> treeNodes;

        private ContentTree(List<ContentTreeNode> treeNodes)
        {
            this.treeNodes = treeNodes;
            if (treeNodes.Count == 0) MaxDepth = 0;
            else MaxDepth = treeNodes.Max(t => t.GetPath().Length);
        }

        public int MaxDepth { get; private set; }

        public static ContentTree BuildTree(IEnumerable<ContentNode> nodes)
        {
            var treeNodes = nodes.Select(node => new ContentTreeNode(node.Segment, node.Action, node.Controller, node.Id, node.ParentId)).ToList();
            treeNodes.ForEach(treeNode => SetTreeNodeParent(treeNode, treeNodes));
            return new ContentTree(treeNodes);
        }

        private static void SetTreeNodeParent(ContentTreeNode treeNode, IEnumerable<ContentTreeNode> treeNodes)
        {
            treeNode.Parent = treeNodes.SingleOrDefault(t => t.Id.Equals(treeNode.ParentId));
        }

        public ContentTreeNode Find(string action, string controller)
        {
            return treeNodes.FirstOrDefault(treeNode => treeNode.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase) && treeNode.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase));
        }

        public ContentTreeNode FindPath(string[] segments)
        {
            if(segments.Length == 0)
                return null;

            ContentTreeNode treeNode = null;
            foreach(var segment in segments)
            {
                treeNode = FindTreeNode(treeNode, segment);
                if(treeNode == null) return null;
            }

            return treeNode;
        }

        public bool PathExists(string[] segments)
        {
            return FindPath(segments) != null;
        }

        private ContentTreeNode FindTreeNode(ContentTreeNode parent, string segment)
        {
            return (from treeNode in treeNodes
                    where treeNode.Parent == parent
                    where treeNode.Segment.Equals(segment, StringComparison.InvariantCultureIgnoreCase)
                    select treeNode).FirstOrDefault();
        }
    }
}