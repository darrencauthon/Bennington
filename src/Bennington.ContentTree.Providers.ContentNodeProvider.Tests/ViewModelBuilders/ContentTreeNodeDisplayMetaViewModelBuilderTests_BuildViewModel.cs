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
    public class ContentTreeNodeDisplayMetaViewModelBuilderTests_BuildViewModel
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();

            mocker.GetMock<IContentTreePageNodeContext>()
                .Setup(a => a.GetAllContentTreePageNodes())
                .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         Id = "treeNodeId",
                                         PageId = "actionId",
                                     }, 
                             }.AsQueryable());
        }

        [TestMethod]
        public void Verify_mapper_is_called_with_ContentTreePageNode_which_matches_tree_node_id_and_action_id_passed_in()
        {
            mocker.Resolve<ContentTreeNodeDisplayMetaViewModelBuilder>().BuildViewModel("treeNodeId", "actionId");

            mocker.GetMock<IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper>()
                .Verify(a => a.CreateInstance(It.Is<ContentTreePageNode>(b => b.Id == "treeNodeId" && b.PageId == "actionId")), Times.Once());
        }

        [TestMethod]
        public void Returns_result_from_mapper()
        {
            mocker.GetMock<IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper>()
                .Setup(a => a.CreateInstance(It.IsAny<ContentTreePageNode>()))
                .Returns(new ContentTreeNodeDisplayMetaViewModel()
                             {
                                 MetaDescription = "test",
                             });

            var result = mocker.Resolve<ContentTreeNodeDisplayMetaViewModelBuilder>().BuildViewModel("treeNodeId", "actionId");

            Assert.AreEqual("test", result.MetaDescription);
        }

        [TestMethod]
        public void Returns_empty_view_model_when_a_ContentTreePageNode_is_not_found()
        {
            mocker.GetMock<IContentTreePageNodeContext>()
                .Setup(a => a.GetAllContentTreePageNodes())
                .Returns(new ContentTreePageNode[]{}.AsQueryable());

            var result = mocker.Resolve<ContentTreeNodeDisplayMetaViewModelBuilder>().BuildViewModel("treeNodeId", "actionId");

            Assert.IsNotNull(result);
        }
    }
}
