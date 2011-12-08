using System;
using System.Configuration;
using System.Linq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Events.Section;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Caching;
using SimpleCqrs.Eventing;
using IDataModelDataContext = Bennington.ContentTree.Providers.SectionNodeProvider.Data.IDataModelDataContext;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Denormalizers
{
	public class SectionRoutingDenormalizer : IHandleDomainEvents<SectionDeletedEvent>,
														IHandleDomainEvents<SectionUrlSegmentSetEvent>,
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

		    CreateRouteForSection(sectionNodeProviderDraft);
		}

	    private void CreateRouteForSection(SectionNodeProviderDraft sectionNodeProviderDraft)
	    {
            if (sectionNodeProviderDraft.Inactive) return;

            var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == sectionNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null)
                throw new Exception("Tree node not found: " + sectionNodeProviderDraft.TreeNodeId);

            contentTreeRepository.Save(new ContentTreeTableRow()
                                            {
                                                Action = "Index",
                                                Controller = "ContentTreeSection",
                                                Id = GetIdForContentTreeRow(treeNode.TreeNodeId),
                                                ParentId = GetParentId(treeNode),
                                                Segment = sectionNodeProviderDraft.UrlSegment,
                                                TreeNodeId = treeNode.TreeNodeId,
                                                ActionId = null
                                            });

            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Bennington.Content.RoutingCacheKey"]))
                return;
            InvalidateCacheClient.Invalidate(new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ConfigurationManager.AppSettings["Bennington.Content.RoutingCacheKey"])));
	    }

		public void Handle(SectionInactiveSetEvent domainEvent)
		{
			var sectionNodeProviderDraft = GetSectionNodeProviderDraftFromDomainEvent(domainEvent);
		    CreateRouteForSection(sectionNodeProviderDraft);
		}

        private string GetIdForContentTreeRow(string treeNodeId)
        {
            var contentTreeRow = contentTreeRepository.GetAll().Where(a => a.TreeNodeId == treeNodeId).FirstOrDefault();
            return contentTreeRow == null ? Guid.NewGuid().ToString() : contentTreeRow.Id;
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
