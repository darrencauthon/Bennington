using System;
using System.Collections.Generic;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Action = Bennington.ContentTree.Models.Action;
using ContentTreeNode = Bennington.ContentTree.Models.ContentTreeNode;

namespace Bennington.ContentTree.Tests.Contexts
{
	[TestClass]
	public class TreeNodeSummaryContextTests_GetChildren
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_1_result_with_ControllerToUseForCreation_property_set_from_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
			fakeTreeNodeExtensionProvider.SetupProperty(a => a.ControllerToUseForCreation, "FakeTreeNodeExtensionProviderController");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				.Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("FakeTreeNodeExtensionProviderController", result.First().ControllerToUseForCreation);
		}

		[TestMethod]
		public void Returns_1_result_with_ActionToUseForCreation_property_set_from_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
			fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForCreation, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				.Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("FakeTreeNodeExtensionProviderAction", result.First().ActionToUseForCreation);
		}

		[TestMethod]
		public void Returns_1_result_with_ActionToUseForModification_property_set_from_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
			fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForModification, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				.Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("FakeTreeNodeExtensionProviderAction", result.First().ActionToUseForModification);
		}

		[TestMethod]
		public void Returns_1_result_with_ControllerToUseForModification_property_set_from_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
			fakeTreeNodeExtensionProvider.SetupProperty(a => a.ControllerToUseForModification, "FakeTreeNodeExtensionProviderController");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                            .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("FakeTreeNodeExtensionProviderController", result.First().ControllerToUseForModification);
		}

		[TestMethod]
		public void Returns_1_result_with_HasChildren_property_set_to_true_when_the_node_has_children()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.IsTrue(result.First().HasChildren);
		}

		[TestMethod]
		public void Returns_1_result_with_correct_id_set_from_node_found_by_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("2", result.First().Id);
		}

		[TestMethod]
		public void Returns_1_result_with_correct_Hidden_set_from_node_found_by_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
										Hidden = true
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.IsTrue(result.First().Hidden);
		}

		[TestMethod]
		public void Returns_1_result_with_correct_name_set_from_node_found_by_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			}, 
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual("fake tree node name", result.First().Name);
		}

		[TestMethod]
		public void Returns_2_results_when_the_specified_parent_node_has_2_child_nodes()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "2",
										ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(2, result.Count());
		}

        [TestMethod]
        public void Returns_flat_set_of_results_instead_of_filtering_by_parent_when_passed_null()
        {
            var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
            fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
                .Returns(new[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
									}, 
								new ContentTreeNode()
									{
										Id = "3",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
                                                             .Returns(fakeTreeNodeExtensionProvider.Object);
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
                .Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "2",
										ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
							}.AsQueryable());

            var treeNodeSummarContext = mocker.Resolve<ContentTree>();
            var result = treeNodeSummarContext.GetChildren(null);

            Assert.AreEqual(3, result.Count());
        }

		[TestMethod]
		public void Returns_empty_set_when_passed_null()
		{
			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren(null);

			Assert.AreEqual(0, result.Count());
		}

		[TestMethod]
		public void Returns_1_result_when_the_specified_parent_node_has_2_child_nodes_but_only_1_is_found_by_provider()
		{
			var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
			fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
				.Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
									}, 
				         	}.AsQueryable());
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
				                                             .Returns(fakeTreeNodeExtensionProvider.Object);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
				.Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
										ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
				         		new TreeNode()
				         			{
										TreeNodeId = "2",
										ParentTreeNodeId = "1",
										Type = "FakeTreeNodeExtensionProvider",
				         			}, 
							}.AsQueryable());

			var treeNodeSummarContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummarContext.GetChildren("1");

			Assert.AreEqual(1, result.Count());
		}

        [TestMethod]
        public void Returns_1_result_with_LastModifyBy_property_set_from_provider()
        {
            var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
            fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
                .Returns(new ContentTreeNode[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
                                        LastModifyBy = "test"
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
                                        LastModifyBy = "test"
									}, 
								new ContentTreeNode()
									{
										Id = "3",
                                        LastModifyBy = "test"
									}, 
				         	}.AsQueryable());
            fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForModification, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
                                                             .Returns(fakeTreeNodeExtensionProvider.Object);
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
                .Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

            var treeNodeSummarContext = mocker.Resolve<ContentTree>();
            var result = treeNodeSummarContext.GetChildren("1");

            Assert.AreEqual("test", result.First().LastModifyBy);
        }

        [TestMethod]
        public void Returns_1_result_with_LastModifyDate_property_set_from_provider()
        {
            var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
            fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
                .Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
                                        LastModifyDate = new DateTime(2010, 1, 1)
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
                                        LastModifyDate = new DateTime(2010, 1, 1)
									}, 
								new ContentTreeNode()
									{
										Id = "3",
                                        LastModifyDate = new DateTime(2010, 1, 1)
									}, 
				         	}.AsQueryable());
            fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForModification, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
                                                             .Returns(fakeTreeNodeExtensionProvider.Object);
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
                .Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

            var treeNodeSummarContext = mocker.Resolve<ContentTree>();
            var result = treeNodeSummarContext.GetChildren("1");

            Assert.AreEqual(new DateTime(2010, 1, 1), result.First().LastModifyDate);
        }

        [TestMethod]
        public void Returns_1_result_with_Active_property_set_from_provider()
        {
            var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
            fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
                .Returns(new []
				         	{
								new ContentTreeNode()
									{
										Id = "1",
                                        Inactive = false
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
                                        Inactive = false
									}, 
								new ContentTreeNode()
									{
										Id = "3",
                                        Inactive = false
									}, 
				         	}.AsQueryable());
            fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForModification, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
                                                             .Returns(fakeTreeNodeExtensionProvider.Object);
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
                .Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

            var treeNodeSummarContext = mocker.Resolve<ContentTree>();
            var result = treeNodeSummarContext.GetChildren("1");
            
            Assert.IsFalse(result.First().Inactive);
        }

        [TestMethod]
        public void Returns_1_result_with_ControllerName_property_set_from_provider()
        {
            var fakeTreeNodeExtensionProvider = new Mock<IContentTreeNodeProvider>();
            fakeTreeNodeExtensionProvider.Setup(a => a.Controller).Returns("controller");
            fakeTreeNodeExtensionProvider.Setup(a => a.GetAll())
                .Returns(new[]
				         	{
								new ContentTreeNode()
									{
										Id = "1",
                                        Inactive = false
									}, 
								new ContentTreeNode()
									{
										Id = "2",
										Name = "fake tree node name",
                                        Inactive = false,
									}, 
								new ContentTreeNode()
									{
										Id = "3",
                                        Inactive = false
									}, 
				         	}.AsQueryable());
            fakeTreeNodeExtensionProvider.SetupProperty(a => a.ActionToUseForModification, "FakeTreeNodeExtensionProviderAction");
            mocker.GetMock<IContentTreeNodeProviderContext>().Setup(a => a.GetProviderForTreeNode(It.Is<TreeNode>(b => b.Type == "FakeTreeNodeExtensionProvider")))
                                                             .Returns(fakeTreeNodeExtensionProvider.Object);
            mocker.GetMock<ITreeNodeRepository>().Setup(a => a.GetAll())
                .Returns(new TreeNode[]
				         	{
				         		new TreeNode()
				         			{
										TreeNodeId = "1",
				         			},
				         		new TreeNode()
				         			{
				         				ParentTreeNodeId = "1",
										TreeNodeId = "2",
										Type = "FakeTreeNodeExtensionProvider",
				         			},
				         		new TreeNode()
				         			{
										TreeNodeId = "3",
				         				ParentTreeNodeId = "2",
				         			},
							}.AsQueryable());

            var treeNodeSummarContext = mocker.Resolve<ContentTree>();
            var result = treeNodeSummarContext.GetChildren("1");

            Assert.AreEqual("controller", result.First().ControllerName);
        }
	}
}
