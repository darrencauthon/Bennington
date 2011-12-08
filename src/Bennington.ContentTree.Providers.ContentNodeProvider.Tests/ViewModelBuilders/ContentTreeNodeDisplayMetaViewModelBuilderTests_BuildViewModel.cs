using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
        }

        [TestMethod]
        public void TestMethod1()
        {
            var result = mocker.Resolve<ContentTreeNodeDisplayMetaViewModelBuilder>().BuildViewModel("treeNodeId", "actionId");

            throw new NotImplementedException();
        }
    }
}
