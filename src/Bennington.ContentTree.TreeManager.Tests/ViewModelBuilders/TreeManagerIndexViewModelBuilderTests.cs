using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.TreeManager.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.TreeManager.Tests.ViewModelBuilders
{
    [TestClass]
    public class TreeManagerIndexViewModelBuilderTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Sets_ContentTreeHasNodes_to_true_when_the_root_node_has_children()
        {
            mocker.GetMock<IContentTree>()
                .Setup(a => a.GetChildren(ContentTree.RootNodeId))
                .Returns(new ContentTreeNode[]
                             {
                                 new ContentTreeNode()
                                     {
                                         Id = "root",
                                         ParentTreeNodeId = ContentTree.RootNodeId,
                                     }, 
                                 new ContentTreeNode()
                                     {
                                         ParentTreeNodeId = "root",

                                     }
                             });

            var result = mocker.Resolve<TreeManagerIndexViewModelBuilder>().BuildViewModel();

            Assert.IsTrue(result.ContentTreeHasNodes);
        }

        [TestMethod]
        public void Sets_ParentTreeNodeId_property_of_TreeNodeCreationInputModel_to_RootNodeId()
        {
            var result = mocker.Resolve<TreeManagerIndexViewModelBuilder>().BuildViewModel();

            Assert.AreEqual(ContentTree.RootNodeId, result.TreeNodeCreationInputModel.ParentTreeNodeId);
        }
    }
}
