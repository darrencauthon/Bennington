using System;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Models
{
	public class ContentTreeSectionNode : ContentTreeNode
	{
		public string SectionId { get; set; }
		
        public string DefaultTreeNodeId { get; set; }

	    public override string IconUrl
	    {
            get { return "Content/SectionNodeProvider/section.png"; }
	    }
	}
}
