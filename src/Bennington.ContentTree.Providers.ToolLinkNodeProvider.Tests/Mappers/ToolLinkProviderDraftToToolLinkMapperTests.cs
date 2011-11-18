using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Data;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Mappers;
using Bennington.ContentTree.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Tests.Mappers
{
	[TestClass]
	public class ToolLinkProviderDraftToToolLinkMapperTests
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
			var mapper = mocker.Resolve<ToolLinkProviderDraftToToolLinkMapper>();
			mapper.AssertConfigurationIsValid();
		}

        [TestMethod]
        public void Sets_ControllerName_property_using_ITreeNodeRepository()
        {
            mocker.GetMock<ITreeNodeRepository>()
                .Setup(a => a.GetAll())
                .Returns(new TreeNode[]
                             {
                                 new TreeNode()
                                     {
                                         TreeNodeId = "1",
                                         ControllerName = "controller",
                                     }, 
                             }.AsQueryable());



            var result = mocker.Resolve<ToolLinkProviderDraftToToolLinkMapper>()
                .CreateInstance(new ToolLinkProviderDraft()
                                    {
                                        Id = "1",
                                    });

            Assert.AreEqual("controller", result.ControllerName);
        }
	}
}
