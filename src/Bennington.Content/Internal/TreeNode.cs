using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bennington.Content.Routing;

namespace Bennington.Content.Internal
{
    internal class TreeNode
    {
        private int maxDepth = 1;
        private readonly IDictionary<ContentRouteNode, TreeNode> values = new Dictionary<ContentRouteNode, TreeNode>();

        public TreeNode(ContentRouteNode value)
        {
            Value = value;
            RootNode = this;
            values.Add(value, this);
            Children = new TreeNodeCollection(this, this);
        }

        private TreeNode(ContentRouteNode value, TreeNode rootNode, TreeNode parentNode, int level)
        {
            Value = value;
            RootNode = rootNode;
            Parent = parentNode;
            Level = level;
            Children = new TreeNodeCollection(rootNode, this);
            SetMaxDepth(rootNode, level);
            rootNode.values.Add(value, this);
        }

        public ContentRouteNode Value { get; set; }
        public TreeNode RootNode { get; private set; }
        public TreeNode Parent { get; private set; }
        public TreeNodeCollection Children { get; private set; }
        public int Level { get; private set; }

        public TreeNode Find(string action, string controller)
        {
            return (from entry in values
                    where entry.Key.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase)
                    where entry.Key.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase)
                    select entry.Value).FirstOrDefault();
        }

        public TreeNode FindPath(string[] segments)
        {
            if(segments.Length == 0 || !RootNode.Value.Segment.Equals(segments[0], StringComparison.InvariantCultureIgnoreCase))
                return null;

            var treeNode = RootNode;
            foreach(var segment in segments.Skip(1))
            {
                treeNode = treeNode.Children.Find(segment);
                if(treeNode == null) return null;
            }

            return treeNode;
        }

        public bool PathExists(string[] segments)
        {
            return FindPath(segments) != null;
        }

        public string[] GetPathSegments()
        {
            var segments = new List<string>();

            var treeNode = this;
            while(treeNode != null)
            {
                segments.Add(treeNode.Value.Segment);
                treeNode = treeNode.Parent;
            }
            segments.Reverse();
            return segments.ToArray();
        }

        public int GetMaxDepth()
        {
            return RootNode.maxDepth;
        }

        private void SetMaxDepth(TreeNode rootNode, int level)
        {
            if(level >= maxDepth)
                rootNode.maxDepth = level + 1;
        }

        public static TreeNode BuildTree(IEnumerable<ContentRouteNode> nodes)
        {
            var rootNode = nodes.SingleOrDefault(node => node.ParentId == null);
            if(rootNode == null) return new TreeNode(new ContentRouteNode());

            var rootTreeNode = new TreeNode(rootNode);
            AddChildren(rootTreeNode, nodes);

            return rootTreeNode;
        }

        private static void AddChildren(TreeNode parentNode, IEnumerable<ContentRouteNode> nodes)
        {
            var childNodes = nodes.Where(node => parentNode.Value.Id.Equals(node.ParentId));

            foreach(var childNode in childNodes)
            {
                var treeNode = parentNode.Children.AddChild(childNode);
                AddChildren(treeNode, nodes);
            }
        }

        public class TreeNodeCollection : IEnumerable<TreeNode>
        {
            private readonly TreeNode parentNode;
            private readonly TreeNode rootNode;
            private readonly List<TreeNode> nodes = new List<TreeNode>();

            public TreeNodeCollection(TreeNode rootNode, TreeNode parentNode)
            {
                this.parentNode = parentNode;
                this.rootNode = rootNode;
            }

            public TreeNode AddChild(ContentRouteNode value)
            {
                var treeNode = new TreeNode(value, rootNode, parentNode, parentNode.Level + 1);
                nodes.Add(treeNode);

                return treeNode;
            }

            public IEnumerator<TreeNode> GetEnumerator()
            {
                return nodes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public TreeNode Find(string segment)
            {
                return nodes.SingleOrDefault(node => node.Value.Segment.Equals(segment, StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}