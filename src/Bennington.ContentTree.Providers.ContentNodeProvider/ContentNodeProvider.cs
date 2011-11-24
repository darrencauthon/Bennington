using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Bennington.ContentTree.Models;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
	public class ContentNodeProvider : IContentTreeNodeProvider
	{
		private readonly IContentTreePageNodeContext contentTreePageNodeContext;

		public ContentNodeProvider(IContentTreePageNodeContext contentTreePageNodeContext)
		{
			this.contentTreePageNodeContext = contentTreePageNodeContext;
		}

		public virtual IQueryable<ContentTreeNode> GetAll()
		{
			var query = from item in contentTreePageNodeContext.GetAllContentTreePageNodes().Where(a => a.Action == "Index")
						select item;
			
			return query;
		}

		public virtual string Name
		{
		    get { return "Page"; }
		    set { throw new NotImplementedException(); }
		}

	    public virtual string ControllerToUseForCreation
		{
			get { return ControllerToUseForModification; }
			set { throw new NotImplementedException(); }
		}

		public virtual string ActionToUseForCreation
		{
			get { return "Create"; }
			set { throw new NotImplementedException(); }
		}

	    public virtual IRouteConstraint IgnoreConstraint
		{
			get { return null; }
		}

		public virtual IEnumerable<Action> Actions
		{
			get { return new Action[]
			             	{
			             		new Action()
			             			{
			             				ControllerAction = "Index",
										DisplayName = "Page Content",
			             			}, 
							}; }
			set { throw new NotImplementedException(); }
		}

		public bool MayHaveChildNodes
		{
			get { return true; }
			set { throw new NotImplementedException(); }
		}

        public virtual string Controller { get { return "ContentTree"; } set { } }

		public virtual string ControllerToUseForModification
		{
			get { return "ContentTreeNode"; }
			set { throw new NotImplementedException(); }
		}

		public virtual string ActionToUseForModification
		{
			get { return "Modify"; }
			set { throw new NotImplementedException(); }
		}
	}
}