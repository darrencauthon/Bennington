using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using Bennington.ContentTree.Data;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Repositories
{
    public interface ITreeNodeRepository
    {
        IQueryable<TreeNode> GetAll();
        TreeNode Create(TreeNode treeNode);
        void Delete(string id);
        void Update(TreeNode treeNode);
    }

    public class TreeNodeRepository : ITreeNodeRepository
    {
        private readonly ITreeNodeDataContext treeNodeDataContext;

        public TreeNodeRepository(ITreeNodeDataContext treeNodeDataContext)
        {
            this.treeNodeDataContext = treeNodeDataContext;
        }

        public IQueryable<TreeNode> GetAll()
        {
            return treeNodeDataContext.TreeNodes;
        }

        public TreeNode Create(TreeNode treeNode)
        {
            treeNodeDataContext.Create(treeNode);
            Thread.Sleep(1500);
            return treeNode;
        }

        public void Delete(string id)
        {
            treeNodeDataContext.Delete(id);
        }

        public void Update(TreeNode treeNode)
        {
            treeNodeDataContext.Update(treeNode);
        }
    }
}