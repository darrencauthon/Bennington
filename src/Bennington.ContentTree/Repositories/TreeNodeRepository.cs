using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Data;

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
        private readonly IDataModelDataContext dataModelDataContext;
        private readonly ObjectCache cache = MemoryCache.Default;

        public TreeNodeRepository(IDataModelDataContext dataModelDataContext)
        {
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<TreeNode> GetAll()
        {
            var treeNodes = cache["TreeNodes"] as IQueryable<TreeNode>;

            if (treeNodes == null)
            {
                treeNodes = dataModelDataContext.TreeNodes;

                var localWorkingFolder = Path.Combine(ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"], @"BenningtonData\");
                var policy = new CacheItemPolicy();

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { localWorkingFolder }));

                cache.Add("TreeNodes", treeNodes, policy);
            }

            return treeNodes;
        }

        public TreeNode Create(TreeNode treeNode)
        {
            dataModelDataContext.Create(treeNode);
            return treeNode;
        }

        public void Delete(string id)
        {
            dataModelDataContext.Delete(id);
        }

        public void Update(TreeNode treeNode)
        {
            dataModelDataContext.Update(treeNode);
        }
    }
}