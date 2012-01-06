using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.EmailCommunication.Validators;
using Bennington.EmailCommunication.Validators.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.EmailCommunication.Tests.Validators.Helpers
{
    [TestClass]
    public class ValidateSemiColonDelimitedEmailListHelperTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Returns_false_for_null()
        {
            var result = mocker.Resolve<ValidateSemiColonDelimitedEmailListHelper>().IsThisAValidEmailList(null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Returns_false_for_empty()
        {
            var result = mocker.Resolve<ValidateSemiColonDelimitedEmailListHelper>().IsThisAValidEmailList(string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Calls_IEmailValidator_against_all_emails_in_the_list()
        {
            mocker.GetMock<IEmailValidator>().Setup(a => a.Validate(It.IsAny<string>())).Returns(true);

            mocker.Resolve<ValidateSemiColonDelimitedEmailListHelper>().IsThisAValidEmailList("1;2;3");

            mocker.GetMock<IEmailValidator>().Verify(a => a.Validate("1"), Times.Once());
            mocker.GetMock<IEmailValidator>().Verify(a => a.Validate("2"), Times.Once());
            mocker.GetMock<IEmailValidator>().Verify(a => a.Validate("3"), Times.Once());
        }

        [TestMethod]
        public void Returns_false_if_IEmailValidator_returns_false_against_any_of_the_emails_in_the_list()
        {
            mocker.GetMock<IEmailValidator>().Setup(a => a.Validate("1")).Returns(true);
            mocker.GetMock<IEmailValidator>().Setup(a => a.Validate("2")).Returns(false);
            mocker.GetMock<IEmailValidator>().Setup(a => a.Validate("3")).Returns(false);

            var result = mocker.Resolve<ValidateSemiColonDelimitedEmailListHelper>().IsThisAValidEmailList("1;2;3");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Returns_true_when_IEmailValidator_returns_validates_all_emails()
        {
            mocker.GetMock<IEmailValidator>().Setup(a => a.Validate(It.IsAny<string>())).Returns(true);

            var result = mocker.Resolve<ValidateSemiColonDelimitedEmailListHelper>().IsThisAValidEmailList("1;2;3");

            Assert.IsTrue(result);
        }
    }
}
