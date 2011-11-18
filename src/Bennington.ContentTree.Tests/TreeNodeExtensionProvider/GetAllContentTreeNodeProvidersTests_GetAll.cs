using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using AutoMoq;
using Bennington.ContentTree.Models;
using Bennington.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Tests.TreeNodeExtensionProvider
{
	[TestClass]
	public class GetAllContentTreeNodeProvidersTests_GetAll
	{
		private AutoMoqer mocker;

		[TestInitialize]
		public void INit()
		{
			mocker = new AutoMoqer();
		}

		[TestMethod]
		public void Returns_service_that_implements_IAmATreeNodeExtensionProvider_and_matches_specified_type()
		{
		    mocker.GetMock<IServiceLocatorWrapper>()
		        .Setup(a => a.ResolveServices<IContentTreeNodeProviderFactory>())
		        .Returns(new List<IContentTreeNodeProviderFactory>());
			mocker.GetMock<IServiceLocatorWrapper>().Setup(a => a.ResolveServices<IContentTreeNodeProvider>())
				.Returns(new List<IContentTreeNodeProvider>()
				         	{
								new IamATreeNodeProvider1(),
								new IamATreeNodeProvider2(),
				         	});

			var getAllContentTreeNodeProviders = mocker.Resolve<ContentTreeNodeProviderContext>();
			var result = getAllContentTreeNodeProviders.GetProviderByTypeName(typeof (IamATreeNodeProvider2).AssemblyQualifiedName);

			Assert.AreEqual("IamATreeNodeProvider2", result.Name);
		}

		private class IamATreeNodeProvider2 : IContentTreeNodeProvider
		{
			public IQueryable<ContentTreeNode> GetAll()
			{
				throw new NotImplementedException();
			}

			public string Name
			{
				get { return "IamATreeNodeProvider2"; }
			}

			public string ControllerToUseForProcessing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string Type
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

		private class IamATreeNodeProvider1 : IContentTreeNodeProvider
		{
			public IQueryable<ContentTreeNode> GetAll()
			{
				throw new NotImplementedException();
			}

			public string Name
			{
				get { return "IamATreeNodeProvider1"; }
			}

			public string ControllerToUseForProcessing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			public string Type
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

			public void RegisterRouteFsorTreeNodeId(string treeNodeId)
			{
				throw new NotImplementedException();
			}
		}
	}
}
