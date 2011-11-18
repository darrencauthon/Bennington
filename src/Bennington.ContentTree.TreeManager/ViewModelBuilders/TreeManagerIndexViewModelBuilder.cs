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
            return new TreeManagerIndexViewModel()
                       {
                           ContentTreeHasNodes = contentTree.GetChildren(ContentTree.RootNodeId).Any(),
                           TreeNodeCreationInputModel = new TreeNodeCreationInputModel()
                                                            {
                                                                ParentTreeNodeId = ContentTree.RootNodeId
                                                            }
                       };
        }
    }
}