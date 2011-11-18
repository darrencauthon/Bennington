using System.Collections.Generic;
using System.Linq;
using Bennington.Core;

namespace Bennington.ContentTree.Contexts
{
	public interface ITreeNodeProviderContext
	{
		IContentTreeNodeProvider GetProviderByTypeName(string providerTypeName);
		IEnumerable<IContentTreeNodeProvider> GetAllTreeNodeProviders();
	}

	public class TreeNodeProviderContext : ITreeNodeProviderContext
	{
		private readonly IServiceLocatorWrapper serviceLocator;

		public TreeNodeProviderContext(IServiceLocatorWrapper serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public IEnumerable<IContentTreeNodeProvider> GetAllTreeNodeProviders()
		{
		    var treeNodeExtensionProviders = new List<IContentTreeNodeProvider>();
            treeNodeExtensionProviders.AddRange(serviceLocator.ResolveServices<IContentTreeNodeProvider>());
		    foreach (var service in serviceLocator.ResolveServices<IAmATreeNodeExtensionProviderFactory>())
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