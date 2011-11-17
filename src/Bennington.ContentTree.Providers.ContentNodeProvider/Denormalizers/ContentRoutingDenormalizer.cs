using System;
using System.Linq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers
{
    public class ContentRoutingDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
                                              IHandleDomainEvents<PageDeletedEvent>
    {
        private readonly ITreeNodeSummaryContext treeNodeSummaryContext;
        private readonly ITreeNodeProviderContext treeNodeProviderContext;
        private readonly IContentTreeRepository contentTreeRepository;
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;

        public ContentRoutingDenormalizer(ITreeNodeSummaryContext treeNodeSummaryContext,
                                          ITreeNodeProviderContext treeNodeProviderContext,
                                          IContentTreeRepository contentTreeRepository,
                                          IContentNodeProviderDraftRepository contentNodeProviderDraftRepository)
        {
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
            this.contentTreeRepository = contentTreeRepository;
            this.treeNodeProviderContext = treeNodeProviderContext;
            this.treeNodeSummaryContext = treeNodeSummaryContext;
        }

        public void Handle(PagePublishedEvent domainEvent)
        {
            var contentNodeProviderDraft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
            if (contentNodeProviderDraft == null)
                throw new Exception("Draft version not found: " + domainEvent.AggregateRootId);

            var treeNode = treeNodeSummaryContext.GetTreeNodeSummaryByTreeNodeId(contentNodeProviderDraft.TreeNodeId);
            if (treeNode == null)
                throw new Exception("Tree node not found: " + domainEvent.AggregateRootId);

            var provider = treeNodeProviderContext.GetProviderByTypeName(treeNode.Type);
            provider.Controller = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.ControllerName != null && a.TreeNodeId == contentNodeProviderDraft.TreeNodeId).FirstOrDefault().ControllerName;

            foreach (var action in provider.ContentTreeNodeContentItems)
            {
                var draft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString() && a.Action == action.Id).FirstOrDefault();
                if (draft == null) continue;

                contentTreeRepository.Save(new ContentTreeNode()
                                           {
                                               Action = action.Id,
                                               Controller = provider.Controller,
                                               Id = draft.PageId,
                                               ParentId = treeNode.ParentTreeNodeId,
                                               Segment = contentNodeProviderDraft.UrlSegment ?? action.Id,
                                               TreeNodeId = treeNode.Id,
                                           });                
            }
        }

        public void Handle(PageDeletedEvent domainEvent)
        {
        }
    }
}
