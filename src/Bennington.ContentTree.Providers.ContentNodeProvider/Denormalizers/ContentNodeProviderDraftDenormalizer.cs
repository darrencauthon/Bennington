using System;
using System.Linq;
using Bennington.ContentTree.Domain.AggregateRoots;
using Bennington.ContentTree.Domain.Events.Page;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.Core.Helpers;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers
{
	public class ContentNodeProviderDraftDenormalizer : IHandleDomainEvents<PageCreatedEvent>,
														IHandleDomainEvents<PageDeletedEvent>,
														IHandleDomainEvents<PageTreeNodeIdSetEvent>,
														IHandleDomainEvents<PageNameSetEvent>,
														IHandleDomainEvents<PageActionSetEvent>,
														IHandleDomainEvents<MetaTitleSetEvent>,
                                                        IHandleDomainEvents<MetaKeywordSetEvent>,
														IHandleDomainEvents<MetaDescriptionSetEvent>,
														IHandleDomainEvents<HeaderTextSetEvent>,
														IHandleDomainEvents<PageHeaderImageSetEvent>,
														IHandleDomainEvents<BodySetEvent>,
														IHandleDomainEvents<PageUrlSegmentSetEvent>,
														IHandleDomainEvents<PageSequenceSetEvent>,
														IHandleDomainEvents<PageHiddenSetEvent>,
														IHandleDomainEvents<PageInactiveSetEvent>,
                                                        IHandleDomainEvents<PageLastModifyBySetEvent>,
                                                        IHandleDomainEvents<PageLastModifyDateSetEvent>
	{
		private readonly IContentNodeProviderDraftRepository contentNodeProviderDraftRepository;

		public ContentNodeProviderDraftDenormalizer(IContentNodeProviderDraftRepository contentNodeProviderDraftRepository)
		{
			this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
		}

		public void Handle(PageCreatedEvent domainEvent)
		{
			contentNodeProviderDraftRepository.Create(new ContentNodeProviderDraft()
			                                          	{
			                                          		PageId = domainEvent.AggregateRootId.ToString()
			                                          	});
		}

		private ContentNodeProviderDraft GetContentNodeProviderDraft(DomainEvent domainEvent)
		{
			return contentNodeProviderDraftRepository
						.GetAllContentNodeProviderDrafts().Where(a => a.PageId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
		}

		public void Handle(PageNameSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Name = domainEvent.Name;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageActionSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Action = domainEvent.Action;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(MetaTitleSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.MetaTitle = domainEvent.MetaTitle;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(MetaDescriptionSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.MetaDescription = domainEvent.MetaDescription;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageUrlSegmentSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.UrlSegment = domainEvent.UrlSegment;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(HeaderTextSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.HeaderText = domainEvent.HeaderText;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(BodySetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Body = domainEvent.Body;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageSequenceSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Sequence = domainEvent.PageSequence;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageDeletedEvent domainEvent)
		{
			foreach (var contentNodeProviderDraft in contentNodeProviderDraftRepository.GetAllContentNodeProviderDrafts().Where(a => a.TreeNodeId == domainEvent.TreeNodeId.ToString()))
			{
				contentNodeProviderDraftRepository.Delete(contentNodeProviderDraft);
			}
		}

		public void Handle(PageTreeNodeIdSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.TreeNodeId = domainEvent.TreeNodeId.ToString();
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageHiddenSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Hidden = domainEvent.Hidden;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageInactiveSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.Inactive = domainEvent.Inactive;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

		public void Handle(PageHeaderImageSetEvent domainEvent)
		{
			var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
			contentNodeProviderDraft.HeaderImage = domainEvent.HeaderImage;
			contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
		}

	    public void Handle(PageLastModifyBySetEvent domainEvent)
	    {
            var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
            contentNodeProviderDraft.LastModifyBy = domainEvent.LastModifyBy;
            contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
	    }

	    public void Handle(PageLastModifyDateSetEvent domainEvent)
	    {
            var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
            contentNodeProviderDraft.LastModifyDate = domainEvent.DateTime;
            contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
	    }

	    public void Handle(MetaKeywordSetEvent domainEvent)
	    {
            var contentNodeProviderDraft = GetContentNodeProviderDraft(domainEvent);
            if (contentNodeProviderDraft == null)
                return;
            contentNodeProviderDraft.MetaKeywords = domainEvent.MetaKeywords;
            contentNodeProviderDraftRepository.Update(contentNodeProviderDraft);
        }
	}
}