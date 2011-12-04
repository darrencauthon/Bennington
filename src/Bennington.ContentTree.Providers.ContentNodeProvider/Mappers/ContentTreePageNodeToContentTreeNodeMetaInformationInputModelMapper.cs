using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
    public interface IContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper
    {
        ContentTreeNodeMetaInformationInputModel CreateInstance(ContentTreePageNode source);
    }

    public class ContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper : Mapper<ContentTreePageNode, ContentTreeNodeMetaInformationInputModel>, IContentTreePageNodeToContentTreeNodeMetaInformationInputModelMapper
    {
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ContentTreePageNode, ContentTreeNodeMetaInformationInputModel>()
                    .ForMember(a => a.TreeNodeId, b => b.MapFrom(c => c.Id))
                    .ForMember(a => a.ContentItemId, b => b.MapFrom(c => c.Action))
                ;
        }
    }
}
