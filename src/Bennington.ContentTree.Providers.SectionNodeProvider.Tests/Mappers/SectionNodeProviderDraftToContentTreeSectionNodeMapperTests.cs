using AutoMapperAssist;
using AutoMoq;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Tests.Mappers
{
	[TestClass]
	public class SectionNodeProviderDraftToContentTreeSectionNodeMapperTests
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
			var mapper = mocker.Resolve<SectionNodeProviderDraftToContentTreeSectionNodeMapper>();
			mapper.AssertConfigurationIsValid();
		}

        [TestMethod]
        public void Maps_Id()
        {
            var result = mocker.Resolve<SectionNodeProviderDraftToContentTreeSectionNodeMapper>().CreateInstance(new SectionNodeProviderDraft()
                                                                                                        {
                                                                                                            TreeNodeId = "1",
                                                                                                        });
            Assert.AreEqual("1", result.Id);
        }
	}
}
