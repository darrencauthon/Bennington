using System;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Models
{
	public class ContentTreePageNode : ContentTreeNode
	{
		public string PageId { get; set; }
		public string Body { get; set; }
		public string Action { get; set; }
		public string HeaderText { get; set; }
		public string HeaderImage { get; set; }
        public string ControllerName { get; set; }
        public override string IconUrl 
        {
            get { return "Content/ContentNodeProvider/page.gif"; } 
        }
	}
}
