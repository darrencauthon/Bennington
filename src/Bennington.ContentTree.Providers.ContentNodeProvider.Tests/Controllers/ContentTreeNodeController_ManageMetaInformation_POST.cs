using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMoq;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleCqrs.Commanding;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
    [TestClass]
    public class ContentTreeNodeController_ManageMetaInformation_POST
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_correct_view_name_when_model_state_is_invalid()
        {
            var controller = mocker.Resolve<ContentTreeNodeController>();
            controller.ModelState.AddModelError("key", "error");

            var result = controller.ManageMetaInformation(new ContentTreeNodeMetaInformationInputModel()) as ViewResult;

            Assert.AreEqual("ManageMetaInformation", result.ViewName);
        }

        [TestMethod]
        public void Returns_view_model_from_IContentTreeNodeMetaInformationViewModelBuilder_when_model_state_is_invalid()
        {
            mocker.GetMock<IContentTreeNodeMetaInformationViewModelBuilder>()
                .Setup(a => a.BuildViewModel(It.Is<ContentTreeNodeMetaInformationInputModel>(b => b.MetaDescription == "test")))
                .Returns(new ContentTreeNodeMetaInformationViewModel()
                             {
                                 ContentTreeNodeMetaInformationInputModel = new ContentTreeNodeMetaInformationInputModel()
                                                                                {
                                                                                    MetaDescription = "test",
                                                                                }
                             });
            var controller = mocker.Resolve<ContentTreeNodeController>();
            controller.ModelState.AddModelError("key", "error");

            var result = controller.ManageMetaInformation(new ContentTreeNodeMetaInformationInputModel()
                                                           {
                                                               MetaDescription = "test",
                                                           }) as ViewResult;

            Assert.AreEqual("test", (result.ViewData.Model as ContentTreeNodeMetaInformationViewModel).ContentTreeNodeMetaInformationInputModel.MetaDescription);
        }

        [TestMethod]
        public void Returns_redirect_to_ManageMetaInformation_when_model_state_is_valid()
        {
            var treeNodeId = Guid.NewGuid().ToString();
            var pageId = Guid.NewGuid().ToString();
            mocker.GetMock<IContentTreePageNodeContext>()
                .Setup(a => a.GetAllContentTreePageNodes())
                .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         Action = "action",
                                         Id = treeNodeId,
                                         PageId = pageId,
                                     }, 
                             }.AsQueryable());
            mocker.GetMock<IContentTreeNodeMetaInformationViewModelBuilder>()
                .Setup(a => a.BuildViewModel(It.IsAny<ContentTreeNodeMetaInformationInputModel>()))
                .Returns(new ContentTreeNodeMetaInformationViewModel()
                                {
                                    ContentTreeNodeMetaInformationInputModel = new ContentTreeNodeMetaInformationInputModel()
                                    {
                                        MetaDescription = "test",
                                    }
                                });

            var result = mocker.Resolve<ContentTreeNodeController>()
                                    .ManageMetaInformation(new ContentTreeNodeMetaInformationInputModel()
                                                                {
                                                                    MetaDescription = "test",
                                                                    TreeNodeId = treeNodeId,
                                                                    ContentItemId = "action"
                                                                }) as RedirectToRouteResult;

            Assert.AreEqual(typeof(ContentTreeNodeController).Name.Replace("Controller", string.Empty), result.RouteValues["controller"]);
            Assert.AreEqual("ManageMetaInformation", result.RouteValues["action"]);
            Assert.AreEqual("action", result.RouteValues["contentItemId"]);
            Assert.AreEqual(treeNodeId, result.RouteValues["TreeNodeId"]);
        }

        [TestMethod]
        public void Sends_ModifyPageMetaInformationCommand_when_model_state_is_valid()
        {
            var treeNodeId = Guid.NewGuid().ToString();
            var pageId = Guid.NewGuid().ToString();
            mocker.GetMock<IContentTreePageNodeContext>()
                .Setup(a => a.GetAllContentTreePageNodes())
                .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         Action = "action",
                                         Id = treeNodeId,
                                         PageId = pageId,
                                     }, 
                             }.AsQueryable());
            mocker.Resolve<ContentTreeNodeController>()
                        .ManageMetaInformation(new ContentTreeNodeMetaInformationInputModel()
                                    {
                                        MetaDescription = "test",
                                        MetaKeywords = "keywords",
                                        MetaTitle = "title",
                                        TreeNodeId = treeNodeId,
                                        ContentItemId = "action"
                                    });

            mocker.GetMock<ICommandBus>()
                .Verify(a => a.Send(It.Is<ModifyPageMetaInformationCommand>(b => b.MetaDescription == "test" 
                                                && b.MetaKeywords == "keywords" 
                                                && b.MetaTitle == "title"
                                                && b.TreeNodeId == treeNodeId
                                                && b.Action == "action"
                                                && b.AggregateRootId.ToString() == pageId
                                                )), Times.Once());
        }

        [TestMethod]
        public void Does_not_send_ModifyPageMetaInformationCommand_when_model_state_is_invalid()
        {
            var controller = mocker.Resolve<ContentTreeNodeController>();
            controller.ModelState.AddModelError("key", "error");

            controller.ManageMetaInformation(new ContentTreeNodeMetaInformationInputModel()
                                                        {
                                                            MetaDescription = "test",
                                                            MetaKeywords = "keywords",
                                                            MetaTitle = "title",
                                                        });

            mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.IsAny<ModifyPageMetaInformationCommand>()), Times.Never());
        }
    }
}
