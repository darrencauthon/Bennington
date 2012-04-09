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
        public IQueryable<TreeNode> GetAll()
        {
            dynamic db = GetDatabase();
            var list = new List<TreeNode>();
            list.AddRange(db.TreeNodes.All().Cast<TreeNode>());
            return list.AsQueryable();
        }

        public TreeNode Create(TreeNode treeNode)
        {
            dynamic db = GetDatabase();
            db.TreeNodes.Insert(treeNode);
            return treeNode;
        }

        public void Delete(string id)
        {
            dynamic db = GetDatabase();
            db.TreeNodes.Delete(TreeNodeId: id);
        }

        public void Update(TreeNode treeNode)
        {
            dynamic db = GetDatabase();
            db.TreeNodes.UpdateByTreeNodeId(treeNode);
        }

        private static object GetDatabase()
        {
            return Simple.Data.Database.OpenConnection(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString);
        }
    }
}