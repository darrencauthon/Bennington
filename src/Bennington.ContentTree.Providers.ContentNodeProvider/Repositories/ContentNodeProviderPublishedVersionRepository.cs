using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;

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

        public ContentNodeProviderPublishedVersionRepository(IDataModelDataContext dataModelDataContext)
        {
            this.dataModelDataContext = dataModelDataContext;
        }

        public IQueryable<ContentNodeProviderPublishedVersion> GetAllContentNodeProviderPublishedVersions()
        {
            var items = cache["ContentNodeProviderPublishedVersions"] as IQueryable<ContentNodeProviderPublishedVersion>;

            if (items == null)
            {
                items = dataModelDataContext.ContentNodeProviderPublishedVersions;

                var localWorkingFolder = Path.Combine(ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"], @"BenningtonData\");
                var policy = new CacheItemPolicy();

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { localWorkingFolder }));

                cache.Add("ContentNodeProviderPublishedVersions", items, policy);
            }

            return items;
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