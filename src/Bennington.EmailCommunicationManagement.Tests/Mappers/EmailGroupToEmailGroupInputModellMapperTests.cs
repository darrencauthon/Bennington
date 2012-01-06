using AutoMapperAssist;
using AutoMoq;
using Bennington.EmailCommunicationManagement.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.EmailCommunicationManagement.Tests.Mappers
{
    [TestClass]
    public class EmailGroupToEmailGroupInputModellMapperTests
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
            var mapper = mocker.Resolve<EmailGroupToEmailGroupInputModelMapper>();
            mapper.AssertConfigurationIsValid();
        }
    }
}
