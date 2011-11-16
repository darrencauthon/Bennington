using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Models;
using Bennington.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Tests.Contexts
{
    [TestClass]
    public class TreeNodeProviderContext_GetAllTreeNodeProviders
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_tree_node_extension_providers_from_factory()
        {
            mocker.GetMock<IServiceLocatorWrapper>()
                .Setup(a => a.ResolveServices<IAmATreeNodeExtensionProvider>())
                .Returns(new List<IAmATreeNodeExtensionProvider>());
            mocker.GetMock<IServiceLocatorWrapper>()
                .Setup(a => a.ResolveServices<IAmATreeNodeExtensionProviderFactory>())
                .Returns(new List<IAmATreeNodeExtensionProviderFactory>()
                             {
                                 new FakeTreeNodeExtensionProviderFactory()
                             });

            var result = mocker.Resolve<TreeNodeProviderContext>().GetAllTreeNodeProviders();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(typeof(FakeNodeProvider), result.First().GetType());
        }

        public class FakeTreeNodeExtensionProviderFactory : IAmATreeNodeExtensionProviderFactory
        {
            public IAmATreeNodeExtensionProvider[] GetTreeNodeExtensionProviders()
            {
                return new IAmATreeNodeExtensionProvider[]
                           {
                                new FakeNodeProvider()  
                            };
            }
        }
    }

    public class FakeNodeProvider : IAmATreeNodeExtensionProvider
    {
        public IQueryable<IAmATreeNodeExtension> GetAll()
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string ControllerToUseForModification
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ActionToUseForModification
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ControllerToUseForCreation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string ActionToUseForCreation
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public IEnumerable<ContentTreeNodeContentItem> ContentTreeNodeContentItems
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool MayHaveChildNodes
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public string Controller
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public void RegisterRouteForTreeNodeId(string treeNodeId)
        {
            throw new NotImplementedException();
        }
    }
}
