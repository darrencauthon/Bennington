using AutoMoq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Context
{
	[TestClass]
	public class ContentTreeNodeContextTests_CreateTreeNodeAndReturnTreeNodeId
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Calls_Create_method_of_ITreeNodeSummaryContext_with_type_from_input_model()
		{
			var contentTreeNodeContext = mocker.Resolve<ContentTreeNodeContext>();
			var result = contentTreeNodeContext.CreateTreeNodeAndReturnTreeNodeId(new ContentTreeNodeInputModel()
			                                                                      	{
			                                                                      		ParentTreeNodeId = "parentTreeNodeId",
																						Type = "provider type",
			                                                                      	});
			mocker.GetMock<ITreeNodeSummaryContext>().Verify(a => a.Create("parentTreeNodeId", "provider type", It.IsAny<string>()), Times.Once());
		}

        [TestMethod]
        public void Calls_Create_method_of_ITreeNodeSummaryContext_with_controllerName_from_input_model()
        {
            var contentTreeNodeContext = mocker.Resolve<ContentTreeNodeContext>();
            var result = contentTreeNodeContext.CreateTreeNodeAndReturnTreeNodeId(new ContentTreeNodeInputModel()
            {
                ParentTreeNodeId = "parentTreeNodeId",
                Type = "provider type",
                ControllerName = "controllerName"
            });
            mocker.GetMock<ITreeNodeSummaryContext>().Verify(a => a.Create("parentTreeNodeId", It.IsAny<string>(), "controllerName"), Times.Once());
        }

		[TestMethod]
		public void Returns_newly_created_tree_node_id()
		{
			mocker.GetMock<ITreeNodeSummaryContext>().Setup(a => a.Create(It.Is<string>(b => b == "parentTreeNodeId"), It.IsAny<string>(), It.IsAny<string>())).Returns("newTreeNodeId");

			var ContentTreeNodeContext = mocker.Resolve<ContentTreeNodeContext>();
			var result = ContentTreeNodeContext.CreateTreeNodeAndReturnTreeNodeId(new ContentTreeNodeInputModel()
																						{
																							ParentTreeNodeId = "parentTreeNodeId",
																						});

			Assert.AreEqual("newTreeNodeId", result);
		}
	}
}
