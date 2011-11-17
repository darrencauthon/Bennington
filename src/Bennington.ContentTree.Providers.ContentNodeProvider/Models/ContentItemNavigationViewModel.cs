using System.Collections.Generic;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Models
{
	public class ContentItemNavigationViewModel
	{
		public string TreeNodeId { get; set; }
		public IEnumerable<Action> ContentTreeNodeContentItems { get; set; }
	}
}
