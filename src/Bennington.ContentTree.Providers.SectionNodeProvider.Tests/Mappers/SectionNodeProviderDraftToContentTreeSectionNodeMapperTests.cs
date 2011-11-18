using System.Linq;
using AutoMapperAssist;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Mappers;
using Bennington.ContentTree.Repositories;
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
        public void Sets_Controller_property_using_ITreeNodeRepository()
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

            var result = mocker.Resolve<SectionNodeProviderDraftToContentTreeSectionNodeMapper>()
                            .CreateInstance(new SectionNodeProviderDraft()
                                    {
                                        TreeNodeId = "1",
                                    });

            Assert.AreEqual("controller", result.ControllerName);
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
