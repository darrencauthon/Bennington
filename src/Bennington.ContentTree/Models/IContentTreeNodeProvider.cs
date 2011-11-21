using System.Collections.Generic;
using System.Linq;

namespace Bennington.ContentTree.Models
{
	public interface IContentTreeNodeProvider
	{
		IQueryable<ContentTreeNode> GetAll();
		string Name { get; }
		string ControllerToUseForModification { get; set; }
		string ActionToUseForModification { get; set; }
		string ControllerToUseForCreation { get; set; }
		string ActionToUseForCreation { get; set; }
		IEnumerable<Action> Actions { get; set; }
		bool MayHaveChildNodes { get; set; }
        string Controller { get; set; }
	}
}
