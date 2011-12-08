using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
    public interface IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper
    {
        ContentTreeNodeDisplayMetaViewModel CreateInstance(ContentTreePageNode source);
    }

    public class ContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper : Mapper<ContentTreePageNode, ContentTreeNodeDisplayMetaViewModel>, IContentTreePageNodeToContentTreeNodeDisplayMetaViewModelMapper
    {
    }
}
