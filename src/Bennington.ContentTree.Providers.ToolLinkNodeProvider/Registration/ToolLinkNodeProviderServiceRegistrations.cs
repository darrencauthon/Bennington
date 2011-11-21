using Bennington.ContentTree.Models;
using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Registration
{
	public class ToolLinkNodeProviderServiceRegistrations : IServiceRegistration
	{
		public void Register(IServiceLocator locator)
		{
			locator.Register<IContentTreeNodeProvider, ToolLinkNodeProvider>(typeof(ToolLinkNodeProvider).AssemblyQualifiedName);
		}
	}
}