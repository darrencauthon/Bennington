using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Mappers
{
    [TestClass]
    public class ContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapperTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Assert_configuration_is_valid()
        {
            var mapper = mocker.Resolve<ContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper>();
            mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Sets_TreeNodeId_correctly()
        {
            var result = mocker.Resolve<ContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper>()
                                            .CreateInstance(new ContentTreePageNode()
                                                                {
                                                                    Id = "tree node id"
                                                                });

            Assert.AreEqual("tree node id", result.TreeNodeId);
        }

        [TestMethod]
        public void Sets_ContentItemId_correctly()
        {
            var result = mocker.Resolve<ContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper>()
                                            .CreateInstance(new ContentTreePageNode()
                                                                {
                                                                    Action = "action"
                                                                });

            Assert.AreEqual("action", result.ContentItemId);
        }
    }
}
