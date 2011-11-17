using System;
using System.Linq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Repositories;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers
{
    public class ContentRoutingDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
                                              IHandleDomainEvents<PageDeletedEvent>
    {
        private readonly ITreeNodeProviderContext treeNodeProviderContext;
        private readonly IContentTreeRepository contentTreeRepository;
        private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;
        private readonly ITreeNodeRepository treeNodeRepository;

        public ContentRoutingDenormalizer(ITreeNodeProviderContext treeNodeProviderContext,
                                          IContentTreeRepository contentTreeRepository,
                                          IContentNodeProviderDraftRepository contentNodeProviderDraftRepository,
                                          ITreeNodeRepository treeNodeRepository)
        {
            this.treeNodeRepository = treeNodeRepository;
            this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
            this.contentTreeRepository = contentTreeRepository;
            this.treeNodeProviderContext = treeNodeProviderContext;
        }

        public void Handle(PagePublishedEvent domainEvent)
        {
            var contentNodeProviderDraft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
            if (contentNodeProviderDraft == null)
                throw new Exception("Draft version not found: " + domainEvent.AggregateRootId);

            var treeNode = treeNodeRepository.GetAll().Where(a => a.Id == contentNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null)
                throw new Exception("Tree node not found: " + domainEvent.AggregateRootId);

            var provider = treeNodeProviderContext.GetProviderByTypeName(treeNode.Type);
            provider.Controller = treeNode.ControllerName;

            foreach (var action in provider.ContentTreeNodeContentItems)
            {
                var draft = contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString() && a.Action == action.Id).FirstOrDefault();

                contentTreeRepository.Save(new ContentTreeNode()
                                           {
                                               Action = action.Id,
                                               Controller = provider.Controller,
                                               Id = Guid.NewGuid().ToString(),
                                               ParentId = treeNode.ParentTreeNodeId,
                                               Segment = draft != null ? draft.UrlSegment : action.Id,
                                               TreeNodeId = treeNode.Id,
                                               ActionId = draft != null ? draft.PageId : null
                                           });                
            }
        }

        public void Handle(PageDeletedEvent domainEvent)
        {
        }
    }
}
