using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Registration
{
	public class TreeNodeExtensionProviderRegistration : IServiceRegistration
	{
		public void Register(IServiceLocator locator)
		{
			locator.Register<IContentTreeNodeProvider, SectionNodeProvider>(typeof(SectionNodeProvider).Name);
		}
	}
}
