using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.Content.Data;
using Bennington.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests
{
    [TestClass]
    public class EngineTreeNodeExtensionProviderFactoryTests_GetTreeNodeExtensionProviders
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_provider_for_each_type_returned_by_IContentTypeRegistry_with_controller_set_from_IContentTypeRegistry()
        {
            mocker.GetMock<IServiceLocatorWrapper>()
                .Setup(a => a.Resolve<EngineTreeNodeProvider>())
                .Returns(new EngineTreeNodeProvider(mocker.GetMock<MvcTurbine.ComponentModel.IServiceLocator>().Object));
            mocker.GetMock<IContentTypeRegistry>()
                .Setup(a => a.GetContentTypes())
                .Returns(new ContentType[]
                             {
                                new ContentType("Engine", "Homepage", "Home", new ContentAction()
                                                                                  {
                                                                                      Action = "Index",
                                                                                      DisplayName = "Homepage Content",
                                                                                  }),     
                            });

            var result = mocker.Resolve<EngineTreeNodeExtensionProviderFactory>().GetTreeNodeExtensionProviders();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Home", result.First().Controller);
        }

        [TestMethod]
        public void Returns_provider_for_each_type_returned_by_IContentTypeRegistry_with__set_from_IContentTypeRegistry()
        {
            mocker.GetMock<IServiceLocatorWrapper>()
                .Setup(a => a.Resolve<EngineTreeNodeProvider>())
                .Returns(new EngineTreeNodeProvider(mocker.GetMock<MvcTurbine.ComponentModel.IServiceLocator>().Object));
            mocker.GetMock<IContentTypeRegistry>()
                .Setup(a => a.GetContentTypes())
                .Returns(new ContentType[]
                             {
                                new ContentType("Engine", "Homepage", "Home", new ContentAction()
                                                                                  {
                                                                                      Action = "Index",
                                                                                      DisplayName = "Homepage Content",
                                                                                  }),     
                            });

            var result = mocker.Resolve<EngineTreeNodeExtensionProviderFactory>().GetTreeNodeExtensionProviders();

            Assert.AreEqual(1, result.Count());
            Assert.AreEqual("Homepage", result.First().Name);
        }

    }
}
