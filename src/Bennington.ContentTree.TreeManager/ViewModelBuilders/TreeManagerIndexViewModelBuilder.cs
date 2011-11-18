using System.Linq;
using Bennington.ContentTree.TreeManager.Models;

namespace Bennington.ContentTree.TreeManager.ViewModelBuilders
{
    public interface ITreeManagerIndexViewModelBuilder
    {
        TreeManagerIndexViewModel BuildViewModel();
    }

    public class TreeManagerIndexViewModelBuilder : ITreeManagerIndexViewModelBuilder
    {
        private readonly IContentTree contentTree;

        public TreeManagerIndexViewModelBuilder(IContentTree contentTree)
        {
            this.contentTree = contentTree;
        }

        public TreeManagerIndexViewModel BuildViewModel()
        {
            var rootChildren = contentTree.GetChildren(ContentTree.RootNodeId);
            return new TreeManagerIndexViewModel()
                       {
                           ContentTreeHasNodes = rootChildren.Any(),
                           TreeNodeCreationInputModel = new TreeNodeCreationInputModel()
                                                            {
                                                                ParentTreeNodeId = ContentTree.RootNodeId
                                                            }
                       };
        }
    }
}