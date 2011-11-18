using System.Web.Mvc;
using System.Web.Routing;
using MvcTurbine.Routing;

namespace Bennington.ContentTree.TreeManager.Routing
{
	public class RegisterSmsRoutesForAllProviders : IRouteRegistrator
	{
		private readonly IContentTreeNodeProviderContext contentTreeNodeProviderContext;

		public RegisterSmsRoutesForAllProviders(IContentTreeNodeProviderContext contentTreeNodeProviderContext)
		{
			this.contentTreeNodeProviderContext = contentTreeNodeProviderContext;
		}

		public void Register(RouteCollection routes)
		{
			foreach (var provider in contentTreeNodeProviderContext.GetAllTreeNodeProviders())
			{
				routes.MapRoute(
							null,
							string.Format("{0}/{{action}}", provider.ControllerToUseForCreation, provider.ActionToUseForCreation),
							new { controller = provider.ControllerToUseForCreation, action = provider.ActionToUseForCreation}
						);

				routes.MapRoute(
							null,
							string.Format("{0}/{{action}}", provider.ControllerToUseForModification, provider.ActionToUseForModification),
							new { controller = provider.ControllerToUseForModification, action = provider.ActionToUseForModification }
						);
			}
		}
	}
}