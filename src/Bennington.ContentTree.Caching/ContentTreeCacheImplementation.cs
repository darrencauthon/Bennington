using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Data;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Caching
{
    public class ContentTreeCacheImplementation : Bennington.ContentTree.ContentTree, IContentTree
    {
        private static Object lockObject = new Object();
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;
        public const string ContentTreeOutputCacheKey = "Bennington.ContentTree";

        public ContentTreeCacheImplementation(ITreeNodeRepository treeNodeRepository, 
                           IContentTreeNodeProviderContext contentTreeNodeProviderContext, 
                           ICommandBus commandBus, 
                           IGuidGetter guidGetter,
                           IGetPathToDataDirectoryService getPathToDataDirectoryService) : base(treeNodeRepository, contentTreeNodeProviderContext, commandBus, guidGetter)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
        }

        public override IEnumerable<ContentTreeNode> GetChildren(string parentNodeId)
        {
            return GetAllContentTreeNodes().Where(node => node.ParentTreeNodeId == parentNodeId);
        }

        public override ContentTreeNode GetById(string nodeId)
        {
            return GetAllContentTreeNodes().SingleOrDefault(node => node.Id == nodeId);
        }

        public override IEnumerable<ContentTreeNode> GetAllContentTreeNodes()
        {
            var contentTreeNodes = cache[GetType().AssemblyQualifiedName] as List<ContentTreeNode>;

            if (contentTreeNodes == null)
            {
                lock (lockObject)
                {
                    if (contentTreeNodes == null)
                    {
                        contentTreeNodes = base.GetChildren(null).ToList();

                        var policy = new CacheItemPolicy();

                        policy.ChangeMonitors.Add(new HostFileChangeMonitor(GetListOfFilePathDependencies()));

                        cache.Add(GetType().AssemblyQualifiedName, contentTreeNodes, policy);                                            
                    }
                }
            }

            return contentTreeNodes;
        }

        private IList<string> GetListOfFilePathDependencies()
        {
            return new List<string>
                {
                    Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"TreeNodex.xml"),
                    Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"SectionNodeProviderDrafts.xml"),
                    Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"ContentNodeProviderPublishedVersions.xml"),
                    Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), typeof(ToolLinkProviderDraft).FullName),
                };
        }
    }
}