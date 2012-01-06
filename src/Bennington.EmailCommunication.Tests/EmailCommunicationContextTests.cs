using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Bennington.EmailCommunication.Repositories;
using Bennington.EmailCommunication.TextProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.EmailCommunication.Tests
{
    [TestClass]
    public class EmailCommunicationContextTests
    {
        private AutoMoqer mocker;
        private string model = "model";

        [TestInitialize]
        public void Init()
        {
            mocker = new AutoMoqer();
            mocker.GetMock<IEmailGroupRepository>()
                .Setup(a => a.GetAll())
                .Returns(new []
                             {
                                 new EmailGroup()
                                     {
                                         EmailCount = 1,
                                         EngineerId = "email group id",
                                         EmailModels = new EmailModel[]
                                                           {
                                                               new EmailModel()
                                                                   {
                                                                       EngineerId = "email1",
                                                                       Subject = "email1 subject",
                                                                       BodyText = "body1"
                                                                   }, 
                                                               new EmailModel()
                                                                   {
                                                                       EngineerId = "email2",
                                                                       Subject = "email2 subject",
                                                                       BodyText = "body2"
                                                                   }, 
                                                           }
                                     }, 
                             });

            mocker.GetMock<IEmailModelToMailMessageWithAnIdMapper>()
                .Setup(a => a.CreateSet(It.IsAny<IEnumerable<EmailModel>>()))
                .Returns(new[]
                             {
                                 new MailMessageWithAnId()
                                     {
                                         EngineerId = "email1",
                                         Subject = "email1 subject",
                                         Body = "body1",
                                     }, 
                                 new MailMessageWithAnId()
                                     {
                                         EngineerId = "email2",
                                         Subject = "email2 subject",
                                         Body = "body2",
                                     }, 
                             });
        }

        [TestMethod]
        public void Passes_emails_from_matching_email_group_into_IEmailModelToMailMessageWithAnIdMapper()
        {
            mocker.Resolve<EmailCommunicationContext>().GetEmailsFromEmailGroup("email group id", null);

            mocker.GetMock<IEmailModelToMailMessageWithAnIdMapper>()
                .Verify(a => a.CreateSet(It.Is<IEnumerable<EmailModel>>(b => b.Where(c => c.EngineerId == "email1").Any() && b.Where(c => c.EngineerId == "email2").Any())), Times.Once());
        }

        [TestMethod]
        public void Returns_empty_set_when_email_group_is_not_found()
        {
            var result = mocker.Resolve<EmailCommunicationContext>().GetEmailsFromEmailGroup("zzz", null);

            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Sets_subject_of_emails_from_IEmailModelToMailMessageWithAnIdMapper_with_result_from_ITagFillerService()
        {
            mocker.GetMock<ITagFillerService>()
                .Setup(a => a.AutoFillTagsFromModel("email1 subject", model))
                .Returns("expected subject for email1");
            mocker.GetMock<ITagFillerService>()
                .Setup(a => a.AutoFillTagsFromModel("email2 subject", model))
                .Returns("expected subject for email2");

            var result = mocker.Resolve<EmailCommunicationContext>().GetEmailsFromEmailGroup("email group id", model);

            Assert.AreEqual("expected subject for email1", result.Where(a => a.EngineerId == "email1").First().Subject);
            Assert.AreEqual("expected subject for email2", result.Where(a => a.EngineerId == "email2").First().Subject);
        }

        [TestMethod]
        public void Sets_body_of_emails_from_IEmailModelToMailMessageWithAnIdMapper_with_result_from_ITagFillerService()
        {
            mocker.GetMock<ITagFillerService>()
                .Setup(a => a.AutoFillTagsFromModel("body1", model))
                .Returns("expected body for email1");
            mocker.GetMock<ITagFillerService>()
                .Setup(a => a.AutoFillTagsFromModel("body2", model))
                .Returns("expected body for email2");

            var result = mocker.Resolve<EmailCommunicationContext>().GetEmailsFromEmailGroup("email group id", model);

            Assert.AreEqual("expected body for email1", result.Where(a => a.EngineerId == "email1").First().Body);
            Assert.AreEqual("expected body for email2", result.Where(a => a.EngineerId == "email2").First().Body);
        }
    }
}
