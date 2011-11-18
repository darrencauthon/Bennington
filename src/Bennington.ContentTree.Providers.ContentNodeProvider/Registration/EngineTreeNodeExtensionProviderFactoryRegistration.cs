using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Registration
{
    public class EngineTreeNodeExtensionProviderFactoryRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            locator.Register<IContentTreeNodeProviderFactory, EngineTreeNodeExtensionProviderFactory>(Guid.NewGuid().ToString());
        }
    }
}
