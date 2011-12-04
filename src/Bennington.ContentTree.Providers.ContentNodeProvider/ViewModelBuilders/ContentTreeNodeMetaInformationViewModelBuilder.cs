using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
    public interface IContentTreeNodeMetaInformationViewModelBuilder
    {
        ContentTreeNodeMetaInformationViewModel BuildViewModel(ContentTreeNodeMetaInformationInputModel contentTreeNodeMetaInformationInputModel);
        ContentTreeNodeMetaInformationViewModel BuildViewModel(string treeNodeId, string contentItemId);
    }

    public class ContentTreeNodeMetaInformationViewModelBuilder : IContentTreeNodeMetaInformationViewModelBuilder
    {
        private readonly IContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper contentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper;
        private readonly IContentTreePageNodeContext contentTreePageNodeContext;

        public ContentTreeNodeMetaInformationViewModelBuilder(IContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper contentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper,
                                                              IContentTreePageNodeContext contentTreePageNodeContext)
        {
            this.contentTreePageNodeContext = contentTreePageNodeContext;
            this.contentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper = contentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper;
        }

        public ContentTreeNodeMetaInformationViewModel BuildViewModel(ContentTreeNodeMetaInformationInputModel contentTreeNodeMetaInformationInputModel)
        {
            return new ContentTreeNodeMetaInformationViewModel()
                       {
                           ContentTreeNodeMetaInformationInputModel = contentTreeNodeMetaInformationInputModel
                       };
        }

        public ContentTreeNodeMetaInformationViewModel BuildViewModel(string treeNodeId, string contentItemId)
        {
            var contentTreePageNode = contentTreePageNodeContext
                                        .GetAllContentTreePageNodes()
                                            .Where(a => a.Id == treeNodeId && a.Action == contentItemId)
                                            .FirstOrDefault();

            return new ContentTreeNodeMetaInformationViewModel()
                       {
                           ContentTreeNodeMetaInformationInputModel = contentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper.CreateInstance(contentTreePageNode)
                       };
        }
    }
}
