using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.ViewModelBuilders
{
    public interface IContentTreeNodeDisplayMetaViewModelBuilder
    {
        ContentTreeNodeDisplayMetaViewModel BuildViewModel(string treeNodeId, string actionId);
    }

    public class ContentTreeNodeDisplayMetaViewModelBuilder : IContentTreeNodeDisplayMetaViewModelBuilder
    {
        private readonly IContentTreePageNodeContext contentTreePageNodeContext;
        private readonly IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper contentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper;

        public ContentTreeNodeDisplayMetaViewModelBuilder(IContentTreePageNodeContext contentTreePageNodeContext,
                                                          IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper contentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper)
        {
            this.contentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper = contentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper;
            this.contentTreePageNodeContext = contentTreePageNodeContext;
        }

        public ContentTreeNodeDisplayMetaViewModel BuildViewModel(string treeNodeId, string actionId)
        {
            var page = contentTreePageNodeContext.GetAllContentTreePageNodes()
                                                    .Where(a => a.Id == treeNodeId && a.PageId == actionId)
                                                    .FirstOrDefault();

            if (page == null)
                return new ContentTreeNodeDisplayMetaViewModel();

            return contentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper.CreateInstance(page);
        }
    }
}
