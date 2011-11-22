﻿using System;
using System.Linq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Events.Section;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Repositories;
using SimpleCqrs.Eventing;
using IDataModelDataContext = Bennington.ContentTree.Providers.SectionNodeProvider.Data.IDataModelDataContext;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Denormalizers
{
	public class SectionRoutingDenormalizer : IHandleDomainEvents<SectionDeletedEvent>,
														IHandleDomainEvents<SectionUrlSegmentSetEvent>,
														//IHandleDomainEvents<SectionDefaultTreeNodeIdSetEvent>,
														IHandleDomainEvents<SectionInactiveSetEvent>
	{
		private readonly IDataModelDataContext dataModelDataContext;
	    private readonly IContentTreeRepository contentTreeRepository;
	    private readonly ITreeNodeRepository treeNodeRepository;

	    public SectionRoutingDenormalizer(IDataModelDataContext dataModelDataContext,
                                          IContentTreeRepository contentTreeRepository,
                                          ITreeNodeRepository treeNodeRepository)
	    {
	        this.treeNodeRepository = treeNodeRepository;
	        this.contentTreeRepository = contentTreeRepository;
	        this.dataModelDataContext = dataModelDataContext;
	    }

	    public void Handle(SectionDeletedEvent domainEvent)
		{
            foreach (var section in contentTreeRepository.GetAll().Where(a => a.TreeNodeId == domainEvent.AggregateRootId.ToString()))
	        {
	            contentTreeRepository.Delete(section.Id);
	        }
		}

		public void Handle(SectionUrlSegmentSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);

		    RecreateRoutesForDefaultPageOfSection(sectionNodeProviderDraft);
		}

	    private void RecreateRoutesForDefaultPageOfSection(SectionNodeProviderDraft sectionNodeProviderDraft)
	    {
	        foreach (var contentTreeRoute in contentTreeRepository.GetAll().Where(a => a.TreeNodeId == sectionNodeProviderDraft.TreeNodeId))
	        {
	            contentTreeRepository.Delete(contentTreeRoute.Id);
	        }

            if (sectionNodeProviderDraft.Inactive) return;
            if (string.IsNullOrEmpty(sectionNodeProviderDraft.DefaultTreeNodeId)) return;

            var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == sectionNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null)
                throw new Exception("Tree node not found: " + sectionNodeProviderDraft.TreeNodeId);

            contentTreeRepository.Save(new ContentTreeNode()
            {
                Action = "Index",
                Controller = "ContentTreeSection",
                Id = Guid.NewGuid().ToString(),
                ParentId = GetParentId(treeNode),
                Segment = sectionNodeProviderDraft.UrlSegment,
                TreeNodeId = treeNode.TreeNodeId,
                ActionId = null
            });
	    }

        //public void Handle(SectionDefaultTreeNodeIdSetEvent domainEvent)
        //{
        //    var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
        //    RecreateRoutesForDefaultPageOfSection(sectionNodeProviderDraft);
        //}

		public void Handle(SectionInactiveSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
		    RecreateRoutesForDefaultPageOfSection(sectionNodeProviderDraft);
		}

        private SectionNodeProviderDraft GetSectionNodeProviderDraftFromDomainEvent(DomainEvent domainEvent)
        {
            return dataModelDataContext.GetAllSectionNodeProviderDrafts().Where(a => a.SectionId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
        }

        private string GetParentId(TreeNode treeNode)
        {
            var contentTreeRow = contentTreeRepository.GetAll().Where(a => a.TreeNodeId == treeNode.ParentTreeNodeId).FirstOrDefault();
            return contentTreeRow != null ? contentTreeRow.Id : treeNode.ParentTreeNodeId;
        }
	}
}
