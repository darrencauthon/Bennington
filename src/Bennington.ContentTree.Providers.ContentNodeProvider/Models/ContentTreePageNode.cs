using System;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Models
{
	public class ContentTreePageNode : ContentTreeNode
	{
        private string iconUrl = "Content/ContentNodeProvider/page.gif";
		public string PageId { get; set; }
		public string Body { get; set; }
		public string Action { get; set; }
		public string HeaderText { get; set; }
		public string HeaderImage { get; set; }
        public string ControllerName { get; set; }
        public override string IconUrl 
        {
            get { return iconUrl; }
            set { iconUrl = value; }
        }
	}
}
