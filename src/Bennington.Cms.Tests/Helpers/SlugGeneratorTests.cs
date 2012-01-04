using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.Cms.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.Cms.Tests.Helpers
{
    [TestClass]
    public class SlugGeneratorTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_original_string_when_the_string_doesnt_contain_special_chars()
        {
            var result = mocker.Resolve<SlugGenerator>().GenerateSlug("zz");

            Assert.AreEqual("zz", result);
        }

        [TestMethod]
        public void Returns_original_string_with_bad_chars_removed_when_original_string_contains_non_alphanumeric_chars()
        {
            var result = mocker.Resolve<SlugGenerator>().GenerateSlug("z!@#$%^&*()z");

            Assert.AreEqual("zz", result);
        }

        [TestMethod]
        public void Replaces_spaces_with_dashes()
        {
            var result = mocker.Resolve<SlugGenerator>().GenerateSlug("1 2");

            Assert.AreEqual("1-2", result);
        }
    }
}
