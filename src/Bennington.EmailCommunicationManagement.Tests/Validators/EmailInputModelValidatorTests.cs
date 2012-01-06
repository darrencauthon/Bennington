using System.Linq;
using AutoMoq;
using Bennington.EmailCommunicationManagement.Models;
using Bennington.EmailCommunicationManagement.Validators;
using Bennington.EmailCommunicationManagement.Validators.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.EmailCommunicationManagement.Tests.Validators
{
    [TestClass]
    public class EmailInputModelValidatorTests
    {
        private AutoMoqer mocker;

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
        }

        [TestMethod]
        public void Does_not_return_an_error_for_cc_emails_when_IValidateSemiColonDelimitedEmailListHelper_returns_true()
        {
            mocker.GetMock<IValidateSemiColonDelimitedEmailListHelper>().Setup(a => a.IsThisAValidEmailList(It.IsAny<string>())).Returns(true);

            var result = mocker.Resolve<EmailInputModelValidator>().Validate(new EmailInputModel()
                                                                                 {
                                                                                     CcEmails = "1@1.net;2@2.net"
                                                                                 });

            Assert.AreEqual(0, result.Errors.Where(a => a.PropertyName == "CcEmails").Count());
        }

        [TestMethod]
        public void Error_for_invalid_cc_emails_when_IValidateSemiColonDelimitedEmailListHelper_returns_false()
        {
            mocker.GetMock<IValidateSemiColonDelimitedEmailListHelper>().Setup(a => a.IsThisAValidEmailList(It.IsAny<string>())).Returns(false);

            var result = mocker.Resolve<EmailInputModelValidator>().Validate(new EmailInputModel()
            {
                CcEmails = "zzz"
            });

            Assert.AreEqual(1, result.Errors
                                        .Where(a => a.PropertyName == "CcEmails")
                                        .Where(a => a.ErrorMessage == "Invalid email address")
                                        .Count());
        }

        [TestMethod]
        public void Error_for_invalid_bcc_emails_when_IValidateSemiColonDelimitedEmailListHelper_returns_false()
        {
            mocker.GetMock<IValidateSemiColonDelimitedEmailListHelper>().Setup(a => a.IsThisAValidEmailList(It.IsAny<string>())).Returns(false);

            var result = mocker.Resolve<EmailInputModelValidator>().Validate(new EmailInputModel()
            {
                BccEmails = "zzz"
            });

            Assert.AreEqual(1, result.Errors
                                        .Where(a => a.PropertyName == "BccEmails")
                                        .Where(a => a.ErrorMessage == "Invalid email address")
                                        .Count());
        }

        [TestMethod]
        public void No_error_for_bcc_emails_when_IValidateSemiColonDelimitedEmailListHelper_returns_false_but_there_are_no_emails_in_list()
        {
            mocker.GetMock<IValidateSemiColonDelimitedEmailListHelper>().Setup(a => a.IsThisAValidEmailList(It.IsAny<string>())).Returns(false);

            var result = mocker.Resolve<EmailInputModelValidator>().Validate(new EmailInputModel());

            Assert.AreEqual(0, result.Errors
                                        .Where(a => a.PropertyName == "BccEmails")
                                        .Count());
        }

        [TestMethod]
        public void No_error_for_cc_emails_when_IValidateSemiColonDelimitedEmailListHelper_returns_false_but_there_are_no_emails_in_list()
        {
            mocker.GetMock<IValidateSemiColonDelimitedEmailListHelper>().Setup(a => a.IsThisAValidEmailList(It.IsAny<string>())).Returns(false);

            var result = mocker.Resolve<EmailInputModelValidator>().Validate(new EmailInputModel());

            Assert.AreEqual(0, result.Errors
                                        .Where(a => a.PropertyName == "CcEmails")
                                        .Count());
        }
    }
}
