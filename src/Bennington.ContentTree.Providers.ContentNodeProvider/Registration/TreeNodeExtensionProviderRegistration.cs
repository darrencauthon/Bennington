using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Registration
{
	public class TreeNodeExtensionProviderRegistration : IServiceRegistration
	{
		public void Register(IServiceLocator locator)
		{
			var typeName = typeof(ContentNodeProvider).Name;
			locator.Register<IContentTreeNodeProvider, ContentNodeProvider>(typeName);
		}
	}
}
