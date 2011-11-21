using System;
using System.Linq;
using Bennington.ContentTree.Domain.Events.Section;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Denormalizers
{
	public class SectionRoutingDenormalizer : IHandleDomainEvents<SectionDeletedEvent>,
														IHandleDomainEvents<SectionUrlSegmentSetEvent>,
														IHandleDomainEvents<SectionDefaultTreeNodeIdSetEvent>,
														IHandleDomainEvents<SectionInactiveSetEvent>
	{
		private readonly IDataModelDataContext dataModelDataContext;

        public SectionRoutingDenormalizer(IDataModelDataContext dataModelDataContext)
		{
			this.dataModelDataContext = dataModelDataContext;
		}

		public void Handle(SectionDeletedEvent domainEvent)
		{
			var sectionNodeProviderDraft = dataModelDataContext.GetAllSectionNodeProviderDrafts().Where(a => a.TreeNodeId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
			//dataModelDataContext.Delete(sectionNodeProviderDraft);
		}

		public void Handle(SectionUrlSegmentSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
			sectionNodeProviderDraft.UrlSegment = domainEvent.UrlSegment;
			//dataModelDataContext.Update(sectionNodeProviderDraft);
		}

		public void Handle(SectionDefaultTreeNodeIdSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
			sectionNodeProviderDraft.DefaultTreeNodeId = domainEvent.DefaultTreeNodeId.ToString();
			//dataModelDataContext.Update(sectionNodeProviderDraft);
		}

		public void Handle(SectionInactiveSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
			sectionNodeProviderDraft.Inactive = domainEvent.Inactive;
			//dataModelDataContext.Update(sectionNodeProviderDraft);
		}

        private SectionNodeProviderDraft GetSectionNodeProviderDraftFromDomainEvent(DomainEvent domainEvent)
        {
            return dataModelDataContext.GetAllSectionNodeProviderDrafts().Where(a => a.SectionId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
        }
	}
}
