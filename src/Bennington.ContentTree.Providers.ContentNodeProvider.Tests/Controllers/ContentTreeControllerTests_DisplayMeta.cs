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

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
    [TestClass]
    public class ContentTreeControllerTests_DisplayMeta
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
            var result = mocker.Resolve<ContentTreeController>().DisplayMeta("treeNodeId", "actionId") as PartialViewResult;

            Assert.AreEqual("DisplayMeta", result.ViewName);
        }

        [TestMethod]
        public void Returns_model_from_IContentTreeNodeDisplayMetaViewModelBuilder()
        {
            mocker.GetMock<IContentTreeNodeDisplayMetaViewModelBuilder>()
                .Setup(a => a.BuildViewModel("treeNodeId", "actionId"))
                .Returns(new ContentTreeNodeDisplayMetaViewModel()
                             {
                                 MetaDescription = "meta description",
                             });

            var result = mocker.Resolve<ContentTreeController>().DisplayMeta("treeNodeId", "actionId") as PartialViewResult;

            Assert.AreEqual("meta description", (result.ViewData.Model as ContentTreeNodeDisplayMetaViewModel).MetaDescription);
        }
    }
}
