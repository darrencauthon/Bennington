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
                                .ManageMetaInformation("id", "Index") as ViewResult;

            Assert.AreEqual("ManageMetaInformation", result.ViewName);
        }

        [TestMethod]
        public void Returns_view_model_from_IContentTreeNodeMetaInformationViewModelBuilder()
        {
            mocker.GetMock<IContentTreeNodeMetaInformationViewModelBuilder>()
                .Setup(a => a.BuildViewModel(null))
                .Returns(new ContentTreeNodeMetaInformationViewModel()
                             {
                                 ContentTreeNodeMetaInformationInputModel = new ContentTreeNodeMetaInformationInputModel()
                                                                                {
                                                                                    MetaDescription = "test",
                                                                                }
                             });

            var result = mocker.Resolve<ContentTreeNodeController>()
                                .ManageMetaInformation(null, null) as ViewResult;

            Assert.AreEqual("test", (result.ViewData.Model as ContentTreeNodeMetaInformationViewModel).ContentTreeNodeMetaInformationInputModel.MetaDescription);
        }
    }
}
