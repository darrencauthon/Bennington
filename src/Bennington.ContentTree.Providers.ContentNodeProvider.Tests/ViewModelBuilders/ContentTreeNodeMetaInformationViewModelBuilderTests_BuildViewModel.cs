using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.ViewModelBuilders
{
    [TestClass]
    public class ContentTreeNodeMetaInformationViewModelBuilderTests_BuildViewModel
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_view_model_passed_in()
        {
            var result = mocker.Resolve<ContentTreeNodeMetaInformationViewModelBuilder>()
                                    .BuildViewModel(new ContentTreeNodeMetaInformationInputModel()
                                                        {
                                                            MetaDescription = "test",
                                                        });

            Assert.AreEqual("test", result.ContentTreeNodeMetaInformationInputModel.MetaDescription);
        }
    }
}
