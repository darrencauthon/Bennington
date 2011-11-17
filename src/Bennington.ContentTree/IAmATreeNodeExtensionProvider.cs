using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Models;

namespace Bennington.ContentTree
{
	public interface IAmATreeNodeExtensionProvider
	{
		IQueryable<IAmATreeNodeExtension> GetAll();
		string Name { get; }
		string ControllerToUseForModification { get; set; }
		string ActionToUseForModification { get; set; }
		string ControllerToUseForCreation { get; set; }
		string ActionToUseForCreation { get; set; }
		IEnumerable<Action> Actions { get; set; }
		bool MayHaveChildNodes { get; set; }
        string Controller { get; set; }
	    void RegisterRouteForTreeNodeId(string treeNodeId);
	}
}
