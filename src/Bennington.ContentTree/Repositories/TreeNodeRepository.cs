using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
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
        private readonly IDataModelDataContext dataModelDataContext;
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

        public TreeNodeRepository(IDataModelDataContext dataModelDataContext, IGetPathToDataDirectoryService getPathToDataDirectoryService)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<TreeNode> GetAll()
        {
            var treeNodes = cache[GetType().AssemblyQualifiedName] as TreeNode[];

            if (treeNodes == null)
            {
                treeNodes = dataModelDataContext.TreeNodes.ToArray();

                var pathToDataStore = Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"TreeNodes.xml");
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { pathToDataStore }));

                cache.Add(GetType().AssemblyQualifiedName, treeNodes, policy);
            }

            return treeNodes.AsQueryable();
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