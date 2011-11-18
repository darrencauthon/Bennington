using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Contexts;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider
{
	public class ToolLinkNodeProvider : IContentTreeNodeProvider
	{
		private readonly IToolLinkContext toolLinkContext;

		public ToolLinkNodeProvider(IToolLinkContext toolLinkContext)
		{
			this.toolLinkContext = toolLinkContext;
		}

		public IQueryable<IContentTreeNode> GetAll()
		{
			return toolLinkContext.GetAllToolLinks().AsQueryable();
		}

		public string Name
		{
			get { return "Tool Link"; }
		}

		public string ControllerToUseForModification
		{
			get { return "ToolLinkProviderNode"; }
			set { throw new NotImplementedException(); }
		}

		public string ActionToUseForModification
		{
			get { return "Modify"; }
			set { throw new NotImplementedException(); }
		}

		public string ControllerToUseForCreation
		{
			get { return "ToolLinkProviderNode"; }
			set { throw new NotImplementedException(); }
		}

		public string ActionToUseForCreation
		{
			get { return "Create"; }
			set { throw new NotImplementedException(); }
		}

	    public string Controller
	    {
            get { return "ToolLink"; }
	        set { }
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
		}
	}
}