using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.Content.Data;
using Bennington.ContentTree.Models;
using Bennington.Core;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class EngineTreeNodeProviderFactory : IContentTreeNodeProviderFactory
    {
        private readonly IContentTypeRegistry contentTypeRegistry;
        private readonly IServiceLocatorWrapper serviceLocator;

        public EngineTreeNodeProviderFactory(IContentTypeRegistry contentTypeRegistry,
                                                      IServiceLocatorWrapper serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            this.contentTypeRegistry = contentTypeRegistry;
        }

        public IContentTreeNodeProvider[] GetTreeNodeExtensionProviders()
        {
            var list = new List<IContentTreeNodeProvider>();
            foreach (var item in contentTypeRegistry.GetContentTypes())
            {
                var engineTreeNodeProvider = serviceLocator.Resolve<EngineTreeNodeProvider>();
                engineTreeNodeProvider.Controller = item.ControllerName;
                engineTreeNodeProvider.Name = item.DisplayName;
                list.Add(engineTreeNodeProvider);
            }

            return list.ToArray();
        }
    }
}
