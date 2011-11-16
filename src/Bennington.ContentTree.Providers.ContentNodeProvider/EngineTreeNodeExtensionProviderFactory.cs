using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.Content.Data;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.Core;
using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
    public class EngineTreeNodeExtensionProviderFactory : IAmATreeNodeExtensionProviderFactory
    {
        private readonly IContentTypeRegistry contentTypeRegistry;
        private readonly IServiceLocatorWrapper serviceLocator;

        public EngineTreeNodeExtensionProviderFactory(IContentTypeRegistry contentTypeRegistry,
                                                      IServiceLocatorWrapper serviceLocator)
        {
            this.serviceLocator = serviceLocator;
            this.contentTypeRegistry = contentTypeRegistry;
        }

        public IAmATreeNodeExtensionProvider[] GetTreeNodeExtensionProviders()
        {
            var list = new List<IAmATreeNodeExtensionProvider>();
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

    public class EngineTreeNodeProvider : ContentNodeProvider
    {
        public EngineTreeNodeProvider(IServiceLocator serviceLocator)
            : base(serviceLocator.Resolve<IContentTreeNodeVersionContext>())
        {
        }

        public override string Name { get; set; }
    }
}
