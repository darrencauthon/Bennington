using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = Bennington.ContentTree.Models.Action;

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
                .Setup(a => a.ResolveServices<IContentTreeNodeProvider>())
                .Returns(new List<IContentTreeNodeProvider>());
            mocker.GetMock<IServiceLocatorWrapper>()
                .Setup(a => a.ResolveServices<IContentTreeNodeProviderFactory>())
                .Returns(new List<IContentTreeNodeProviderFactory>()
                             {
                                 new FakeTreeNodeExtensionProviderFactory()
                             });

            var result = mocker.Resolve<ContentTreeNodeProviderContext>().GetAllTreeNodeProviders();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(typeof(FakeNodeProvider), result.First().GetType());
        }

        public class FakeTreeNodeExtensionProviderFactory : IContentTreeNodeProviderFactory
        {
            public IContentTreeNodeProvider[] GetTreeNodeExtensionProviders()
            {
                return new IContentTreeNodeProvider[]
                           {
                                new FakeNodeProvider()  
                            };
            }
        }
    }

    public class FakeNodeProvider : IContentTreeNodeProvider
    {
        public IQueryable<IContentTreeNode> GetAll()
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

        public IEnumerable<Action> Actions
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
