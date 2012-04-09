using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Helpers;
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
        private readonly IDatabaseRetriever databaseRetriever;
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

        public TreeNodeRepository(IDatabaseRetriever databaseRetriever,
                                  IGetPathToDataDirectoryService getPathToDataDirectoryService)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
            this.databaseRetriever = databaseRetriever;
        }

        public IQueryable<TreeNode> GetAll()
        {
            var db = databaseRetriever.GetDatabase();
            var list = new List<TreeNode>();
            list.AddRange(db.TreeNodes.All().Cast<TreeNode>());
            return list.AsQueryable();
        }

        public TreeNode Create(TreeNode treeNode)
        {
            var db = databaseRetriever.GetDatabase();
            db.TreeNodes.Insert(treeNode);
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
            return treeNode;
        }

        public void Delete(string id)
        {
            var db = databaseRetriever.GetDatabase();
            db.TreeNodes.Delete(TreeNodeId: id);
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
        }

        public void Update(TreeNode treeNode)
        {
            var db = databaseRetriever.GetDatabase();
            db.TreeNodes.UpdateByTreeNodeId(treeNode);
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
        }

        private void TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges()
        {
            var path = string.Format("{0}TreeNodes.xml", getPathToDataDirectoryService.GetPathToDirectory());

            if (!File.Exists(path))
            {
                using (var fileStream = File.Create(path))
                {
                }
            }

            using (var writer = File.AppendText(path))
            {
                writer.WriteLine(string.Empty);
            }
        }
    }
}