using System.Collections.Generic;
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
        private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

        public ToolLinkProviderDraftRepository(IXmlFileSerializationHelper xmlFileSerializationHelper, IGetDataPathForType getDataPathForType,
                                               IGetValueOfIdPropertyForInstance getValueOfIdPropertyForInstance, IGuidGetter guidGetter, IFileSystem fileSystem, IGetPathToDataDirectoryService getPathToDataDirectoryService)
            : base(xmlFileSerializationHelper, getDataPathForType, getValueOfIdPropertyForInstance, guidGetter, fileSystem)
        {
            this.getPathToDataDirectoryService = getPathToDataDirectoryService;
        }

        public IQueryable<ToolLinkProviderDraft> GetAll()
        {
            var toolLinks = cache[GetType().AssemblyQualifiedName] as ToolLinkProviderDraft[];

            if (toolLinks == null)
            {
                toolLinks = base.GetAll().ToArray();

                var pathToDataStore = Path.Combine(getPathToDataDirectoryService.GetPathToDirectory(), typeof(ToolLinkProviderDraft).FullName);
                var policy = new CacheItemPolicy();
                policy.ChangeMonitors.Add(new HostFileChangeMonitor(new List<string> { pathToDataStore }));
                cache.Add(GetType().AssemblyQualifiedName, toolLinks, policy);
            }

            return toolLinks.AsQueryable();
        }
    }
}