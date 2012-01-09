using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Caching
{
    public class ContentTree : IContentTree
    {
        public const string RootNodeId = "00000000-0000-0000-0000-000000000000";
        private readonly ITreeNodeRepository treeNodeRepository;
        private readonly IContentTreeNodeProviderContext contentTreeNodeProviderContext;
        private readonly ICommandBus commandBus;
        private readonly IGuidGetter guidGetter;
        private readonly ObjectCache cache = MemoryCache.Default;

        public ContentTree(ITreeNodeRepository treeNodeRepository,
                           IContentTreeNodeProviderContext contentTreeNodeProviderContext,
                           ICommandBus commandBus,
                           IGuidGetter guidGetter)
        {
            this.guidGetter = guidGetter;
            this.commandBus = commandBus;
            this.contentTreeNodeProviderContext = contentTreeNodeProviderContext;
            this.treeNodeRepository = treeNodeRepository;
        }

        public IEnumerable<ContentTreeNode> GetRootNodes()
        {
            return GetChildren(RootNodeId);
        }

        public string Create(string parentNodeId, string providerTypeAssemblyQualifiedName, string controllerName)
        {
            ThrowExceptionIfTheProviderTypeDoesNotImplementIContentTreeNodeProvider(Type.GetType(providerTypeAssemblyQualifiedName));

            var guid = guidGetter.GetGuid();
            commandBus.Send(new CreateTreeNodeCommand
            {
                ParentId = parentNodeId,
                Type = Type.GetType(providerTypeAssemblyQualifiedName),
                TreeNodeId = guid,
                AggregateRootId = guid,
                ControllerName = controllerName
            });

            return guid.ToString();
        }

        private static void ThrowExceptionIfTheProviderTypeDoesNotImplementIContentTreeNodeProvider(Type providerType)
        {
            if (!typeof(IContentTreeNodeProvider).IsAssignableFrom(providerType))
                throw new Exception(string.Format("Provider type must implement {0}", typeof(IContentTreeNodeProvider).FullName));
        }

        public IEnumerable<ContentTreeNode> GetChildren(string parentNodeId)
        {
            return GetAllContentTreeNodes().Where(node => node.ParentTreeNodeId == parentNodeId);
        }

        public ContentTreeNode GetById(string nodeId)
        {
            return GetAllContentTreeNodes().SingleOrDefault(node => node.Id == nodeId);
        }

        public IEnumerable<ContentTreeNode> GetAllContentTreeNodes()
        {
            var contentTreeNodes = cache["GetAllContentTreeNodes"] as List<ContentTreeNode>;

            if (contentTreeNodes == null)
            {
                var treeNodes = treeNodeRepository.GetAll();
                contentTreeNodes = treeNodes.Select(treeNode => GetTreeNodeSummaryForTreeNode(treeNode)).Where(node => node != null).ToList();

                var localWorkingFolder = Path.Combine(ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"], @"BenningtonData\");
                var policy = new CacheItemPolicy();

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { localWorkingFolder }));

                cache.Add("GetAllContentTreeNodes", contentTreeNodes, policy);
            }

            return contentTreeNodes;
        }

        private ContentTreeNode GetTreeNodeSummaryForTreeNode(TreeNode treeNode)
        {
            var provider = contentTreeNodeProviderContext.GetProviderForTreeNode(treeNode);
            if (provider == null) throw new Exception(string.Format("Content tree node provider for type: {0} not found.", treeNode.Type));

            var contentTreeNode = provider.GetAll().Where(a => a.Id == treeNode.TreeNodeId).FirstOrDefault();
            if (contentTreeNode == null) return null;

            var treeNodeSummary = new ContentTreeNode
            {
                Name = contentTreeNode.Name,
                Id = treeNode.TreeNodeId,
                UrlSegment = contentTreeNode.UrlSegment,
                HasChildren = treeNodeRepository.GetAll().Where(a => a.ParentTreeNodeId == treeNode.TreeNodeId).Any(),
                ControllerToUseForModification = provider.ControllerToUseForModification,
                ActionToUseForModification = provider.ActionToUseForModification,
                ControllerToUseForCreation = provider.ControllerToUseForCreation,
                ActionToUseForCreation = provider.ActionToUseForCreation,
                RouteValuesForModification = new { treeNode.TreeNodeId },
                RouteValuesForCreation = new { ParentTreeNodeId = treeNode.TreeNodeId },
                ParentTreeNodeId = treeNode.ParentTreeNodeId,
                Sequence = contentTreeNode.Sequence,
                Type = treeNode.Type,
                MayHaveChildNodes = provider.MayHaveChildNodes,
                Hidden = contentTreeNode.Hidden,
                IconUrl = contentTreeNode.IconUrl,
                LastModifyBy = contentTreeNode.LastModifyBy,
                LastModifyDate = contentTreeNode.LastModifyDate,
                Inactive = contentTreeNode.Inactive,
                ControllerName = provider.Controller
            };

            return treeNodeSummary;
        }
    }
}