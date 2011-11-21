using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Models;
using Bennington.Core;

namespace Bennington.ContentTree
{
	public interface IContentTreeNodeProviderContext
	{
		IContentTreeNodeProvider GetProviderByTypeName(string providerTypeName);
		IEnumerable<IContentTreeNodeProvider> GetAllTreeNodeProviders();
	}

	public class ContentTreeNodeProviderContext : IContentTreeNodeProviderContext
	{
		private readonly IServiceLocatorWrapper serviceLocator;

		public ContentTreeNodeProviderContext(IServiceLocatorWrapper serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public IEnumerable<IContentTreeNodeProvider> GetAllTreeNodeProviders()
		{
		    var treeNodeExtensionProviders = new List<IContentTreeNodeProvider>();
            treeNodeExtensionProviders.AddRange(serviceLocator.ResolveServices<IContentTreeNodeProvider>());
		    foreach (var service in serviceLocator.ResolveServices<IContentTreeNodeProviderFactory>())
		    {
		        treeNodeExtensionProviders.AddRange(service.GetTreeNodeExtensionProviders());
		    }

			return treeNodeExtensionProviders;
		}

		public IContentTreeNodeProvider GetProviderByTypeName(string providerTypeName)
		{
		    var allProviders = GetAllTreeNodeProviders().Where(a => a.GetType().AssemblyQualifiedName.StartsWith(providerTypeName)).ToArray();
            return allProviders.FirstOrDefault();
		}
	}
}