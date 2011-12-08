using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
    public interface IContentTreeNodeDisplayMetaViewModelBuilder
    {
        ContentTreeNodeDisplayMetaViewModel BuildViewModel(string treeNodeId, string actionId);
    }

    public class ContentTreeNodeDisplayMetaViewModelBuilder : IContentTreeNodeDisplayMetaViewModelBuilder
    {
        public ContentTreeNodeDisplayMetaViewModel BuildViewModel(string treeNodeId, string actionId)
        {
            throw new NotImplementedException();
        }
    }
}
