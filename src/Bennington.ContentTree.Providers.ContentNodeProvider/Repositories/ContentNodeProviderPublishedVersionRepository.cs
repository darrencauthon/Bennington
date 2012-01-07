using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Repositories
{
    public interface IContentNodeProviderPublishedVersionRepository
    {
        IQueryable<ContentNodeProviderPublishedVersion> GetAllContentNodeProviderPublishedVersions();
        void Update(ContentNodeProviderPublishedVersion instance);
        void Create(ContentNodeProviderPublishedVersion instance);
        void Delete(ContentNodeProviderPublishedVersion instance);
    }

    public class ContentNodeProviderPublishedVersionRepository : IContentNodeProviderPublishedVersionRepository
    {
        private readonly IDataModelDataContext dataModelDataContext;
        private readonly ObjectCache cache = MemoryCache.Default;
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

        public ContentNodeProviderPublishedVersionRepository(IDataModelDataContext dataModelDataContext,
                                                            IGetPathToDataDirectoryService getPathToDataDirectoryService)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<ContentNodeProviderPublishedVersion> GetAllContentNodeProviderPublishedVersions()
        {
            var items = cache[GetType().AssemblyQualifiedName] as ContentNodeProviderPublishedVersion[];

            if (items == null)
            {
                items = dataModelDataContext.ContentNodeProviderPublishedVersions.ToArray();
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"ContentNodeProviderPublishedVersions.xml"),
                                                                                        Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), @"ContentNodeProviderDrafts.xml") }));

                cache.Add(GetType().AssemblyQualifiedName, items, policy);
            }

            return items.AsQueryable();
        }

        public void Update(ContentNodeProviderPublishedVersion instance)
        {
            dataModelDataContext.Update(instance);
        }

        public void Create(ContentNodeProviderPublishedVersion instance)
        {
            dataModelDataContext.Create(instance);
        }

        public void Delete(ContentNodeProviderPublishedVersion instance)
        {
            dataModelDataContext.Delete(instance);
        }
    }
}