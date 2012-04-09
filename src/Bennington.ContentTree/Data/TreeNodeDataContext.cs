using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bennington.ContentTree.Data
{
    public interface ITreeNodeDataContext
    {
        IQueryable<TreeNode> TreeNodes { get; set; }
        void Create(TreeNode treeNode);
        void Delete(string id);
        void Update(TreeNode treeNode);
    }

    public class TreeNodeDataContext : ITreeNodeDataContext
    {
        public IQueryable<TreeNode> TreeNodes
        {
            get
            {
                dynamic db = GetDatabase();
                var list = new List<TreeNode>();
                list.AddRange(db.TreeNodes.All().Cast<TreeNode>());
                return list.AsQueryable();
            }

            set { throw new NotImplementedException(); }
        }

        public void Create(TreeNode treeNode)
        {
            dynamic db = GetDatabase();
            db.TreeNodes.Insert(treeNode);
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