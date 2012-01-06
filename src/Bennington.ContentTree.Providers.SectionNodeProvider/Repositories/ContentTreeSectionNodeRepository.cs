using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Repositories;

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

        public ContentTreeSectionNodeRepository(IDataModelDataContext dataModelDataContext, ITreeNodeRepository treeNodeRepository,
                                                ISectionNodeProviderDraftToContentTreeSectionNodeMapper sectionNodeProviderDraftToContentTreeSectionNodeMapper)
        {
            this.sectionNodeProviderDraftToContentTreeSectionNodeMapper = sectionNodeProviderDraftToContentTreeSectionNodeMapper;
            this.treeNodeRepository = treeNodeRepository;
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<ContentTreeSectionNode> GetAllContentTreeSectionNodes()
        {
            var items = cache["ContentTreeSectionNodes"] as IQueryable<ContentTreeSectionNode>;

            if (items == null)
            {
                items = sectionNodeProviderDraftToContentTreeSectionNodeMapper.CreateSet(dataModelDataContext.GetAllSectionNodeProviderDrafts()).AsQueryable();

                var localWorkingFolder = Path.Combine(ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"], @"BenningtonData\");
                var policy = new CacheItemPolicy();

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { localWorkingFolder }));

                cache.Add("ContentTreeSectionNodes", items, policy);
            }

            return items;
        }

        public void Delete(ContentTreeSectionNode instance)
        {
            treeNodeRepository.Delete(instance.Id);
        }
    }
}