using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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

        [TestMethod]
        public void Returns_view_model_with_input_model_populated_with_treeNodeId_and_contentItemid()
        {
            mocker.GetMock<IContentTreePageNodeContext>()
                .Setup(a => a.GetAllContentTreePageNodes())
                .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         Id = "treeNodeId",
                                         Action = "contentItemId"
                                     }, 
                             }.AsQueryable());
            mocker.GetMock<IContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper>()
                .Setup(a => a.CreateInstance(It.Is<ContentTreePageNode>(b => b.Id == "treeNodeId")))
                .Returns(new ContentTreeNodeMetaInformationInputModel()
                             {
                                 TreeNodeId = "treeNodeId",
                                 ContentItemId = "contentItemId"
                             });

            var result = mocker.Resolve<ContentTreeNodeMetaInformationViewModelBuilder>()
                                    .BuildViewModel("treeNodeId", "contentItemId");

            Assert.AreEqual("treeNodeId", result.ContentTreeNodeMetaInformationInputModel.TreeNodeId);
            Assert.AreEqual("contentItemId", result.ContentTreeNodeMetaInformationInputModel.ContentItemId);
        }

    }
}
