using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Tests.Mappers
{
    [TestClass]
    public class ContentTreeSectionNodeToContentTreeSectionInputModelMapperTests
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
            var mapper = mocker.Resolve<ContentTreeSectionNodeToContentTreeSectionInputModelMapper>();
            mapper.AssertConfigurationIsValid();
        }
    }
}
