using System;
using System.Configuration;
using System.Linq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Caching;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers
{
    public class ContentRoutingDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
                                              IHandleDomainEvents<PageDeletedEvent>
    {
        private readonly IContentTreeNodeProviderContext contentTreeNodeProviderContext;
        private readonly IContentTreeRepository contentTreeRepository;
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;
        private readonly ITreeNodeRepository treeNodeRepository;

        public ContentRoutingDenormalizer(IContentTreeNodeProviderContext contentTreeNodeProviderContext,
                                          IContentTreeRepository contentTreeRepository,
                                          IContentNodeProviderDraftRepository contentNodeProviderDraftRepository,
                                          ITreeNodeRepository treeNodeRepository)
        {
            this.treeNodeRepository = treeNodeRepository;
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
            this.contentTreeRepository = contentTreeRepository;
            this.contentTreeNodeProviderContext = contentTreeNodeProviderContext;
        }

        public void Handle(PagePublishedEvent domainEvent)
        {
            var contentNodeProviderDraft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
            if (contentNodeProviderDraft == null)
                throw new Exception("Draft version not found: " + domainEvent.AggregateRootId);

            var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == contentNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null)
                throw new Exception("Tree node not found: " + domainEvent.AggregateRootId);

            var provider = contentTreeNodeProviderContext.GetProviderForTreeNode(treeNode);
            provider.Controller = treeNode.ControllerName;

            foreach (var contentTreeNode in contentTreeRepository.GetAll().Where(a => a.TreeNodeId == treeNode.TreeNodeId))
            {
                contentTreeRepository.Delete(contentTreeNode.Id);
            }

            var indexDraft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.TreeNodeId == treeNode.TreeNodeId && a.Action == "Index").FirstOrDefault();
            if (indexDraft == null) return;
            if (indexDraft.Inactive) return;

            foreach (var action in provider.Actions.OrderBy(a => a.ControllerAction == "Index" ? 10 : 20))
            {
                var draft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.TreeNodeId == treeNode.TreeNodeId && a.Action == action.ControllerAction).FirstOrDefault();

                contentTreeRepository.Save(new ContentTreeNode()
                                           {
                                               Action = action.ControllerAction,
                                               Controller = provider.Controller,
                                               Id = Guid.NewGuid().ToString(),
                                               ParentId = GetParentId(treeNode, action.ControllerAction),
                                               Segment = draft != null ? draft.UrlSegment ?? action.ControllerAction : action.ControllerAction,
                                               TreeNodeId = treeNode.TreeNodeId,
                                               ActionId = GetActionId(draft)
                                           });                
            }

            InvalidateRoutingCache();
        }

        private void InvalidateRoutingCache()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Bennington.Content.RoutingCacheKey"]))
                return;
            InvalidateCacheClient.Invalidate(new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ConfigurationManager.AppSettings["Bennington.Content.RoutingCacheKey"])));
        }

        private string GetActionId(ContentNodeProviderDraft draft)
        {
            return draft != null ? draft.PageId : null;
        }

        private string GetParentId(TreeNode treeNode, string action)
        {
            if (action == "Index")
            {
                var contentTreeRow = contentTreeRepository.GetAll().Where(a => a.TreeNodeId == treeNode.ParentTreeNodeId).FirstOrDefault();   
                return contentTreeRow != null ? contentTreeRow.Id : treeNode.ParentTreeNodeId;                
            }

            var parentContentTreeRow = contentTreeRepository.GetAll().Where(a => a.TreeNodeId == treeNode.TreeNodeId && a.Action == "Index").FirstOrDefault();
            return parentContentTreeRow == null ? treeNode.ParentTreeNodeId : parentContentTreeRow.Id;
        }

        public void Handle(PageDeletedEvent domainEvent)
        {
            foreach (var contentTreeNodeRow in contentTreeRepository.GetAll().Where(a =>a.TreeNodeId == domainEvent.TreeNodeId.ToString()))
            {
                contentTreeRepository.Delete(contentTreeNodeRow.Id);
            }

            InvalidateRoutingCache();
        }
    }
}
