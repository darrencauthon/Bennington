using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.EmailCommunication.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.EmailCommunication.Tests.Mappers
{
    [TestClass]
    public class EmailGroupInputModelToEmailGroupMapperTests
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
            var mapper = mocker.Resolve<EmailGroupInputModelToEmailGroupMapper>();
            mapper.AssertConfigurationIsValid();
        }
    }
}
