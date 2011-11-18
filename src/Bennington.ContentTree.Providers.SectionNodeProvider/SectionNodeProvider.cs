using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Bennington.ContentTree.Models;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;
using Bennington.Core;
using Action = Bennington.ContentTree.Models.Action;

namespace Bennington.ContentTree.Providers.SectionNodeProvider
{
	public class SectionNodeProvider : IContentTreeNodeProvider
	{
		private readonly IContentTreeSectionNodeRepository contentTreeSectionNodeRepository;
		private readonly IVersionContext versionContext;

		public SectionNodeProvider(IContentTreeSectionNodeRepository contentTreeSectionNodeRepository,
									IVersionContext versionContext)
		{
			this.versionContext = versionContext;
			this.contentTreeSectionNodeRepository = contentTreeSectionNodeRepository;
		}

		public IQueryable<ContentTreeNode> GetAll()
		{
			var query = from item in contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes().Where(a => a.Inactive == false || versionContext.GetCurrentVersionId() == VersionContext.Manage)
						select item;
			
			return query;
		}

		public string Name
		{
			get { return "Section"; }
		}

		public string ControllerToUseForCreation
		{
			get { return ControllerToUseForModification; }
			set { throw new NotImplementedException(); }
		}

		public string ActionToUseForCreation
		{
			get { return "Create"; }
			set { throw new NotImplementedException(); }
		}

		public IRouteConstraint IgnoreConstraint
		{
			get { return null; }
		}

		public IEnumerable<Action> Actions
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public bool MayHaveChildNodes
		{
			get { return true; }
			set { throw new NotImplementedException(); }
		}

	    public string Controller
	    {
            get { return "ContentTreeSection"; }
	        set { throw new NotImplementedException(); }
	    }

		public string ControllerToUseForModification
		{
			get { return "ContentTreeSectionNode"; }
			set { throw new NotImplementedException(); }
		}

		public string ActionToUseForModification
		{
			get { return "Modify"; }
			set { throw new NotImplementedException(); }
		}
	}
}