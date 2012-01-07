using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Repositories
{
    public interface IContentTreeSectionNodeRepository
    {
        IQueryable<ContentTreeSectionNode> GetAllContentTreeSectionNodes();
        void Delete(ContentTreeSectionNode instance);
    }

    public class ContentTreeSectionNodeRepository : IContentTreeSectionNodeRepository
    {
        private readonly IDataModelDataContext dataModelDataContext;
        private readonly ITreeNodeRepository treeNodeRepository;
        private readonly ISectionNodeProviderDraftToContentTreeSectionNodeMapper sectionNodeProviderDraftToContentTreeSectionNodeMapper;
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

        public ContentTreeSectionNodeRepository(IDataModelDataContext dataModelDataContext, ITreeNodeRepository treeNodeRepository,
                                                ISectionNodeProviderDraftToContentTreeSectionNodeMapper sectionNodeProviderDraftToContentTreeSectionNodeMapper,
                                                IGetPathToDataDirectoryService getPathToDataDirectoryService)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
            this.sectionNodeProviderDraftToContentTreeSectionNodeMapper = sectionNodeProviderDraftToContentTreeSectionNodeMapper;
            this.treeNodeRepository = treeNodeRepository;
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<ContentTreeSectionNode> GetAllContentTreeSectionNodes()
        {
            var items = cache[GetType().AssemblyQualifiedName] as ContentTreeSectionNode[];

            if (items == null)
            {
                items = sectionNodeProviderDraftToContentTreeSectionNodeMapper.CreateSet(dataModelDataContext.GetAllSectionNodeProviderDrafts()).ToArray();
                var pathToDataStore = Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"SectionNodeProviderDrafts.xml");
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { pathToDataStore }));

                cache.Add(GetType().AssemblyQualifiedName, items, policy);
            }

            return items.AsQueryable();
        }

        public void Delete(ContentTreeSectionNode instance)
        {
            treeNodeRepository.Delete(instance.Id);
        }
    }
}