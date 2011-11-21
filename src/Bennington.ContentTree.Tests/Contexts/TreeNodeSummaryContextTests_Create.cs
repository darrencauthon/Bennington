using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Commands;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleCqrs.Commanding;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Tests.Contexts
{
	[TestClass]
	public class TreeNodeSummaryContextTests_Create
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void INit()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_newly_created_tree_node()
		{
			var guid = new Guid();
			mocker.GetMock<IGuidGetter>().Setup(a => a.GetGuid()).Returns(guid);
			mocker.GetMock<ITreeNodeRepository>().Setup(a => a.Create(It.IsAny<TreeNode>()))
				.Returns(new TreeNode()
				         	{
				         		TreeNodeId = "id",
				         	});

			var treeNodeSummaryContext = mocker.Resolve<ContentTree>();
			var result = treeNodeSummaryContext.Create("parentNodeId", typeof(FakeTreeNodeProvider).AssemblyQualifiedName, null);

			Assert.AreEqual(guid.ToString(), result);
		}

		[TestMethod]
		public void Does_not_throw_exception_when_passing_type_that_does_implements_IAmATreeNodeExtensionProvider()
		{
			var treeNodeSummaryContext = mocker.Resolve<ContentTree>();
			treeNodeSummaryContext.Create(null, typeof(FakeTreeNodeProvider).AssemblyQualifiedName, null);
		}

		[TestMethod]
		public void Throws_exception_when_passing_type_that_does_not_implement_IAmATreeNodeExtensionProvider()
		{
			var treeNodeSummaryContext = mocker.Resolve<ContentTree>();

			try
			{
				treeNodeSummaryContext.Create(null, typeof(string).AssemblyQualifiedName, null);
			}
			catch (Exception e)
			{
				Assert.IsTrue(e.Message.StartsWith("Provider type must implement "));
				return;
			}
			throw new Exception("Should throw excpetion above");
		}

		public class FakeProvider : IContentTreeNodeProvider
		{
			public IQueryable<Models.ContentTreeNode> GetAll()
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

		    public string Controller
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
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public bool MayHaveChildNodes
			{
				get { return false; }
				set { throw new NotImplementedException(); }
			}

			public void RegisterRouteForTreeNodeId(string treeNodeId)
			{
				throw new NotImplementedException();
			}
		}

		[TestMethod]
		public void Create_method_sends_CreateTreeNodeCommand()
		{
			var guid = new Guid();
			mocker.GetMock<IGuidGetter>().Setup(a => a.GetGuid()).Returns(guid);

            mocker.Resolve<ContentTree>().Create("parentNodeId", typeof(FakeProvider).AssemblyQualifiedName, "controllerName");

			mocker.GetMock<ICommandBus>().Verify(a => a.Send(It.Is<CreateTreeNodeCommand>(b => b.ParentId == "parentNodeId"
																									&& b.TreeNodeId == guid
																									&& b.Type == typeof(FakeProvider)
                                                                                                    && b.ControllerName == "controllerName"
																									&& b.AggregateRootId == guid)), Times.Once());
		}

		private class FakeTreeNodeProvider : IContentTreeNodeProvider
		{
			public IQueryable<Models.ContentTreeNode> GetAll()
			{
				throw new NotImplementedException();
			}

			public string Name
			{
				get { throw new NotImplementedException(); }
			}

			public string ControllerToUseForProcessing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
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
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public bool MayHaveChildNodes
			{
				get { return false; }
				set { throw new NotImplementedException(); }
			}

		    public string Controller
		    {
		        get { throw new NotImplementedException(); }
		        set { throw new NotImplementedException(); }
		    }

		    public void RegisterRouteForTreeNodeId(string treeNodeId)
			{
				throw new NotImplementedException();
			}
		}
	}
}
