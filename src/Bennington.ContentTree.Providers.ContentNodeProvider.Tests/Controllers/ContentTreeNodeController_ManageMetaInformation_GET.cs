using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
    [TestClass]
    public class ContentTreeNodeController_ManageMetaInformation_GET
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_correct_view_name()
        {
            var result = mocker.Resolve<ContentTreeNodeController>()
                                .ManageMetaInformation("id", "pageId") as ViewResult;

            Assert.AreEqual("ManageMetaInformation", result.ViewName);
        }

        [TestMethod]
        public void Passes_input_model_with_contentItemId_and_treeNodeId_set()
        {
            mocker.Resolve<ContentTreeNodeController>().ManageMetaInformation("treeNodeId", "pageId");

            mocker.GetMock<IContentTreeNodeMetaInformationViewModelBuilder>()
                .Verify(a => a.BuildViewModel(It.Is<ContentTreeNodeMetaInformationInputModel>(b => b.PageId == "pageId" && b.TreeNodeId == "treeNodeId")), Times.Once());
        }

        [TestMethod]
        public void Returns_view_model_from_IContentTreeNodeMetaInformationViewModelBuilder()
        {
            mocker.GetMock<IContentTreeNodeMetaInformationViewModelBuilder>()
                .Setup(a => a.BuildViewModel(It.Is<ContentTreeNodeMetaInformationInputModel>(b => b.PageId == "pageId" && b.TreeNodeId == "tree node id")))
                .Returns(new ContentTreeNodeMetaInformationViewModel()
                             {
                                 ContentTreeNodeMetaInformationInputModel = new ContentTreeNodeMetaInformationInputModel()
                                                                                {
                                                                                    PageId = "pageId",
                                                                                    TreeNodeId = "tree node id"
                                                                                }
                             });

            var result = mocker.Resolve<ContentTreeNodeController>()
                                .ManageMetaInformation("tree node id", "pageId") as ViewResult;

            Assert.AreEqual("tree node id", (result.ViewData.Model as ContentTreeNodeMetaInformationViewModel).ContentTreeNodeMetaInformationInputModel.TreeNodeId);
        }
    }
}
