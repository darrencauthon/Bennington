using System.Collections.Generic;
using System.Linq;
using Bennington.Core;

namespace Bennington.ContentTree.Contexts
{
	public interface ITreeNodeProviderContext
	{
		IAmATreeNodeExtensionProvider GetProviderByTypeName(string providerTypeName);
		IEnumerable<IAmATreeNodeExtensionProvider> GetAllTreeNodeProviders();
	}

	public class TreeNodeProviderContext : ITreeNodeProviderContext
	{
		private readonly IServiceLocatorWrapper serviceLocator;

		public TreeNodeProviderContext(IServiceLocatorWrapper serviceLocator)
		{
			this.serviceLocator = serviceLocator;
		}

		public IEnumerable<IAmATreeNodeExtensionProvider> GetAllTreeNodeProviders()
		{
		    var treeNodeExtensionProviders = new List<IAmATreeNodeExtensionProvider>();
            treeNodeExtensionProviders.AddRange(serviceLocator.ResolveServices<IAmATreeNodeExtensionProvider>());
		    foreach (var service in serviceLocator.ResolveServices<IAmATreeNodeExtensionProviderFactory>())
		    {
		        treeNodeExtensionProviders.AddRange(service.GetTreeNodeExtensionProviders());
		    }

			return treeNodeExtensionProviders;
		}

		public IAmATreeNodeExtensionProvider GetProviderByTypeName(string providerTypeName)
		{
		    var allProviders = GetAllTreeNodeProviders().Where(a => a.GetType().AssemblyQualifiedName.StartsWith(providerTypeName)).ToArray();
            return allProviders.FirstOrDefault();
		}
	}
}