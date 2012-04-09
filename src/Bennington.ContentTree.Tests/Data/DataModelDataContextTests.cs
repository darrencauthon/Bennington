using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Helpers;
using Bennington.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bennington.ContentTree.Tests.Data
{
	[TestClass]
	public class DataModelDataContextTests
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void Init()
		{
			mocker = new AutoMoqer();
			mocker.GetMock<IGetPathToDataDirectoryService>().Setup(a => a.GetPathToDirectory())
				.Returns("path");
			mocker.GetMock<IXmlFileSerializationHelper>().Setup(a => a.DeserializeListFromPath<TreeNode>("path" + Path.DirectorySeparatorChar + "TreeNodes.xml"))
				.Returns(new List<TreeNode>()
				         	{
				         		new TreeNode()
				         			{
				         				TreeNodeId = "1",
				         			},
							});

		}

		[TestMethod]
		public void TreeNodes_property_returns_TreeNodes_from_xml_list()
		{
			var treeNodes = mocker.Resolve<TreeNodeFileBasedDataContext>().TreeNodes;

			Assert.AreEqual("1", treeNodes.First().TreeNodeId);
		}

		[TestMethod]
		public void Create_method_adds_a_TreeNode_to_existing_list()
		{
			mocker.Resolve<TreeNodeFileBasedDataContext>().Create(new TreeNode()
			                                                    {
			                                                        TreeNodeId = "2",
			                                                    });

			mocker.GetMock<IXmlFileSerializationHelper>()
				.Verify(a => a.SerializeListToPath(It.Is<List<TreeNode>>(b => b.Where(c => c.TreeNodeId == "1").Count() == 1 
																					&& b.Where(c => c.TreeNodeId == "2").Count() == 1
																					&& b.Count == 2), It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Update_method_updates_an_existing_TreeNode_to_existing_list()
		{
			mocker.Resolve<TreeNodeFileBasedDataContext>().Update(new TreeNode()
			                                              	{
			                                              		TreeNodeId = "1",
																Type = "test"
			                                              	});

			mocker.GetMock<IXmlFileSerializationHelper>()
				.Verify(a => a.SerializeListToPath(It.Is<List<TreeNode>>(b => b.Where(c => c.TreeNodeId == "1" && c.Type == "test").Count() == 1
																					&& b.Count == 1), It.IsAny<string>()), Times.Once());
		}

		[TestMethod]
		public void Delete_method_existing_TreeNode_from_list()
		{
			mocker.Resolve<TreeNodeFileBasedDataContext>().Delete("1");

			mocker.GetMock<IXmlFileSerializationHelper>()
				.Verify(a => a.SerializeListToPath(It.Is<List<TreeNode>>(b => b.Count == 0), It.IsAny<string>()), Times.Once());
		}
	}
}
