using System.Web.Mvc;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Bennington.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
	[TestClass]
	public class ContentTreeNodeControllerTests_Display
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_view_model_from_viewModelBuilder()
		{
			var expectedViewModel = new ContentTreeNodeDisplayViewModel()
			                        	{
			                        		Header = "Header",
			                        	};
			mocker.GetMock<IContentTreeNodeDisplayViewModelBuilder>().Setup(a => a.BuildViewModel("treeNodeId", "actionId"))
				.Returns(expectedViewModel);

			var result = (ContentTreeNodeDisplayViewModel) ((PartialViewResult) mocker.Resolve<ContentTreeController>().Display("treeNodeId", "actionId")).ViewData.Model;

			Assert.AreEqual(expectedViewModel, result);
		}

		[TestMethod]
		public void Returns_view_name()
		{
			var result = mocker.Resolve<ContentTreeController>().Display("controller", "action");

            Assert.AreEqual("Display", ((PartialViewResult)result).ViewName);
		}
	}
}
