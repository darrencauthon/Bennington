using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Controllers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Tests.Controllers
{
	[TestClass]
	public class ContentTreeNodeController_ContentItemNavigation
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}


		[TestMethod]
		public void Returns_content_item_ids_from_provider()
		{
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll()).Returns(new TreeNode[]
			                                                        {
            															new TreeNode()
            																{
            																	TreeNodeId = "1",
																				Type = "providertype",
            																}, 
																	}.AsQueryable());
			mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderByTypeName("providertype"))
				.Returns(new FakeIamATreeNodeProvider());

			var result = mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");
			var model = (ContentItemNavigationViewModel) ((ViewResult) result).ViewData.Model;

			Assert.AreEqual(3, model.ContentTreeNodeContentItems.Count());
			Assert.AreEqual(1, model.ContentTreeNodeContentItems.Where(a => a.ControllerAction == "id1").Count());
			Assert.AreEqual(1, model.ContentTreeNodeContentItems.Where(a => a.ControllerAction == "id2").Count());
			Assert.AreEqual(1, model.ContentTreeNodeContentItems.Where(a => a.ControllerAction == "id3").Count());
		}

		[TestMethod]
		public void Sets_treeNodeId_of_view_model()
		{
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll()).Returns(new TreeNode[]
			                                                        {
            															new TreeNode()
            																{
            																	TreeNodeId = "1",
																				Type = "providertype",
            																}, 
																	}.AsQueryable());
			mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderByTypeName("providertype"))
				.Returns(new FakeIamATreeNodeProvider());

			var result = mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");
			var model = (ContentItemNavigationViewModel)((ViewResult)result).ViewData.Model;

			Assert.AreEqual("1", model.TreeNodeId);
		}

		public class FakeIamATreeNodeProvider : IContentTreeNodeProvider
		{
			public IQueryable<ContentTreeNode> GetAll()
			{
				throw new NotImplementedException();
			}

			public string Name
			{
				get { throw new NotImplementedException(); }
			}

			public string ControllerToUseForModification
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string ActionToUseForModification
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string ControllerToUseForCreation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string ActionToUseForCreation
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public IRouteConstraint IgnoreConstraint
			{
				get { throw new NotImplementedException(); }
			}

			public IEnumerable<Action> Actions
			{
				get { return new Action[]
				             	{
				             		new Action()
				             			{
				             				ControllerAction = "id1",
											DisplayName = "id 1",
				             			}, 
									new Action()
				             			{
				             				ControllerAction = "id2",
											DisplayName = "id 2",
				             			}, 
									new Action()
				             			{
				             				ControllerAction = "id3",
											DisplayName = "id 3",
				             			}, 
				}; }
				set { throw new NotImplementedException(); }
			}

			public bool MayHaveChildNodes
			{
				get { return true; }
				set { throw new NotImplementedException(); }
			}

            public string Controller { get { return "Controller"; } set { if (value == "test") throw new Exception(value); } }

		    public void RegisterRouteForTreeNodeId(string treeNodeId)
			{
				throw new NotImplementedException();
			}
		}

		[TestMethod]
		public void Returns_view_model()
		{
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll()).Returns(new TreeNode[]
			                                                        {
            															new TreeNode()
            																{
            																	TreeNodeId = "1",
																				Type = "providertype",
            																}, 
																	}.AsQueryable());
			mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderByTypeName("providertype"))
				.Returns(new FakeIamATreeNodeProvider());

			var result = mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");

			Assert.IsInstanceOfType(((ViewResult)result).ViewData.Model, typeof(ContentItemNavigationViewModel));
		}

		[TestMethod]
		public void Returns_view_with_correct_name()
		{
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll()).Returns(new TreeNode[]
			                                                        {
            															new TreeNode()
            																{
            																	TreeNodeId = "1",
																				Type = "providertype",
            																}, 
																	}.AsQueryable());
			mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderByTypeName("providertype"))
				.Returns(new FakeIamATreeNodeProvider());
	
			var result = mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");

			Assert.AreEqual("ContentItemNavigation", ((ViewResult)result).ViewName);
		}

		[TestMethod]
		public void Returns_null_when_the_there_are_no_content_items()
		{
			var result = mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");

			Assert.IsNull(result);
		}

        [TestMethod]
        public void Sets_Controller_property_of_provider_to_ControllerName_of_tree_node()
        {
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll()).Returns(new TreeNode[]
			                                                        {
            															new TreeNode()
            																{
            																	TreeNodeId = "1",
																				Type = "providertype",
                                                                                ControllerName = "test",
            																}, 
																	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderByTypeName("providertype"))
                .Returns(new FakeIamATreeNodeProvider());

            try
            {
                mocker.Resolve<ContentTreeNodeController>().ContentItemNavigation("1");
            } catch(Exception e)
            {
                if (e.Message == "test") return;   
            }

            throw new Exception("controller name not set");
        }

	}
}
