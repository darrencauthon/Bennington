using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Data;
using Bennington.Core.Helpers;
using Bennington.Repository;
using Bennington.Repository.Helpers;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Repositories
{
    public interface IToolLinkProviderDraftRepository
    {
        IQueryable<ToolLinkProviderDraft> GetAll();
        string SaveAndReturnId(ToolLinkProviderDraft toolLinkProviderDraft);
    }

    public class ToolLinkProviderDraftRepository : ObjectStore<ToolLinkProviderDraft>, IToolLinkProviderDraftRepository
    {
        private readonly ObjectCache cache = MemoryCache.Default;

        public ToolLinkProviderDraftRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType,
                                               IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem)
            : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
        }

        public IQueryable<ToolLinkProviderDraft> GetAll()
        {
            var toolLinks = cache["ToolLinks"] as IQueryable<ToolLinkProviderDraft>;

            if (toolLinks == null)
            {
                toolLinks = base.GetAll().AsQueryable();

                var localWorkingFolder = Path.Combine(ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"], @"BenningtonData\");
                var policy = new CacheItemPolicy();

                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { localWorkingFolder }));

                cache.Add("ToolLinks", toolLinks, policy);
            }

            return toolLinks;
        }
    }
}