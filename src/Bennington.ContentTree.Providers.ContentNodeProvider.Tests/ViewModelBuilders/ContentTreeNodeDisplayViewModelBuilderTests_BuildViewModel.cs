using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Context;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders;
using Bennington.Core.Helpers;
using Bennington.FileUploadHandling.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ContentTreeNode = Bennington.ContentTree.Models.ContentTreeNode;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.ViewModelBuilders
{
	[TestClass]
	public class ContentTreeNodeDisplayViewModelBuilderTests_BuildViewModel
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();

            mocker.GetMock<IContentTreeNodeVersionContext>().Setup(a => a.GetAllContentTreeNodes())
                        .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         PageId = "actionId1",
                                         Id = "treeId",
                                     }, 
                                 new ContentTreePageNode()
                                     {
                                         Id = "treeId",
                                         PageId = "actionId",
                                         HeaderText = "header",
                                         HeaderImage = "headerImageId",
                                         Body = "body",
                                     }, 
                                new ContentTreePageNode()
                                     {
                                         Id = "treeId",
                                         PageId = "actionId2"
                                     }, 
                             }.AsQueryable());
		}

        [TestMethod]
        public void Returns_empty_view_model_when_a_matching_page_is_not_found()
        {
            var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel(null, null);

            Assert.AreEqual(null, result.Body);
            Assert.AreEqual(null, result.Header);
            Assert.AreEqual(null, result.HeaderImage);
        }

        [TestMethod]
        public void Returns_correct_body_text_when_a_matching_page_is_found()
        {
            var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("treeId", "actionId");

            Assert.AreEqual("body", result.Body);
        }

        [TestMethod]
        public void Returns_correct_header_text_when_a_matching_page_is_found()
        {
            var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("treeId", "actionId");

            Assert.AreEqual("header", result.Header);
        }

        [TestMethod]
        public void Returns_url_to_header_image_when_a_matching_page_is_found_and_it_has_a_header_image_id_set()
        {
            mocker.GetMock<IFileUploadContext>()
                .Setup(a => a.GetUrlForFileUploadFolder())
                .Returns("/Resource_/");
            mocker.GetMock<IFileUploadContext>()
                .Setup(a => a.GetUrlRelativeToUploadRoot("ContentTreeNodeInputModel", "HeaderImage", "headerImageId"))
                .Returns("ContentTreeNodeInputModel/guid/HeaderImage/test.jpg");

            var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("treeId", "actionId");

            Assert.AreEqual("/Resource_/ContentTreeNodeInputModel/guid/HeaderImage/test.jpg", result.HeaderImage);
        }

        [TestMethod]
        public void Sets_url_to_header_image_to_null_when_a_matching_page_is_found_but_it_has_no_header_image_id_set()
        {
            mocker.GetMock<IContentTreeNodeVersionContext>().Setup(a => a.GetAllContentTreeNodes())
            .Returns(new ContentTreePageNode[]
                             {
                                 new ContentTreePageNode()
                                     {
                                         PageId = "actionId",
                                         HeaderText = "header",
                                         Body = "body",
                                         Id = "treeId"
                                     }, 
                             }.AsQueryable());

            var result = mocker.Resolve<ContentTreeNodeDisplayViewModelBuilder>().BuildViewModel("treeId", "actionId");

            Assert.AreEqual(null, result.HeaderImage);
        }
	}
}
