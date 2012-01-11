using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.EmailCommunication.Mappers;
using Bennington.EmailCommunication.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.EmailCommunication.Tests.Mappers
{
    [TestClass]
    public class EmailModelToMailMessageWithAnIdMapperTests
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
            var mapper = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>();
            mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Sets_Cc_correctly()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>()
                                    .CreateInstance(new EmailModel()
                                                        {
                                                            ToEmail = "to@to.net",
                                                            FromEmail = "from@from.net",
                                                            CcEmails = "1@1.net;2@2.net"
                                                        });

            Assert.AreEqual(2, result.CC.Count);
            Assert.AreEqual(1, result.CC.Where(a => a.Address == "1@1.net").Count());
            Assert.AreEqual(1, result.CC.Where(a => a.Address == "2@2.net").Count());
        }

        [TestMethod]
        public void Sets_To_correctly()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
                                                                    {
                                                                        ToEmail = "to@to.net",
                                                                        FromEmail = "from@from.net",
                                                                        CcEmails = "1@1.net;2@2.net"
                                                                    });

            Assert.AreEqual("to@to.net", result.To.First().Address);
        }

        [TestMethod]
        public void Sets_From_correctly()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
            {
                ToEmail = "to@to.net",
                FromEmail = "from@from.net",
                CcEmails = "1@1.net;2@2.net"
            });

            Assert.AreEqual("from@from.net", result.From.Address);
        }

        [TestMethod]
        public void Sets_Bcc_correctly()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
            {
                ToEmail = "to@to.net",
                FromEmail = "from@from.net",
                BccEmails = "1@1.net;2@2.net"
            });

            Assert.AreEqual(2, result.Bcc.Count);
            Assert.AreEqual(1, result.Bcc.Where(a => a.Address == "1@1.net").Count());
            Assert.AreEqual(1, result.Bcc.Where(a => a.Address == "2@2.net").Count());
        }

        [TestMethod]
        public void Sets_Body_correctly()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
            {
                ToEmail = "to@to.net",
                FromEmail = "from@from.net",
                BccEmails = "1@1.net;2@2.net",
                BodyText = "body text"
            });

            Assert.AreEqual("body text", result.Body);
        }

        [TestMethod]
        public void Sets_Body_to_be_html_correctly_when_body_is_not_html()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
            {
                ToEmail = "to@to.net",
                FromEmail = "from@from.net",
                BccEmails = "1@1.net;2@2.net",
                IsBodyHtml = false,
                BodyText = "body text",
            });

            Assert.IsFalse(result.IsBodyHtml);
        }

        [TestMethod]
        public void Sets_Body_to_be_html_correctly_when_body_is_html()
        {
            var result = mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>().CreateInstance(new EmailModel()
            {
                ToEmail = "to@to.net",
                FromEmail = "from@from.net",
                BccEmails = "1@1.net;2@2.net",
                IsBodyHtml = true,
                BodyText = "body text",
            });

            Assert.IsTrue(result.IsBodyHtml);
        }

        [TestMethod]
        public void Does_not_throw_when_to_email_is_not_provided()
        {
            mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>()
                            .CreateInstance(new EmailModel()
                                {
                                    FromEmail = "from@from.net",
                                    BccEmails = "1@1.net;2@2.net",
                                    IsBodyHtml = true,
                                    BodyText = "body text",
                                });
        }

        [TestMethod]
        public void Does_not_throw_when_from_email_is_not_provided()
        {
            mocker.Resolve<EmailModelToMailMessageWithAnIdMapper>()
                            .CreateInstance(new EmailModel()
                            {
                                ToEmail = "to@to.net",
                                BccEmails = "1@1.net;2@2.net",
                                IsBodyHtml = true,
                                BodyText = "body text",
                            });
        }
    }
}
