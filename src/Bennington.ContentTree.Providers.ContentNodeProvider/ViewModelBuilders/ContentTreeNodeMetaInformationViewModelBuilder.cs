using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
    public interface IContentTreeNodeMetaInformationViewModelBuilder
    {
        ContentTreeNodeMetaInformationViewModel BuildViewModel(ContentTreeNodeMetaInformationInputModel contentTreeNodeMetaInformationInputModel);
    }

    public class ContentTreeNodeMetaInformationViewModelBuilder : IContentTreeNodeMetaInformationViewModelBuilder
    {
        public ContentTreeNodeMetaInformationViewModel BuildViewModel(ContentTreeNodeMetaInformationInputModel contentTreeNodeMetaInformationInputModel)
        {
            throw new NotImplementedException();
        }
    }
}
