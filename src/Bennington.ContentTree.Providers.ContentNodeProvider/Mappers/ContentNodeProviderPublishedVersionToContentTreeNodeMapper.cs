using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
		IEnumerable<Models.Contenttreenode> CreateSet(IEnumerable<ContentNodeProviderPublishedVersion> source);
	}

	public class ContentNodeProviderPublishedVersionToContentTreeNodeMapper : Mapper<ContentNodeProviderPublishedVersion, Models.Contenttreenode>, IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ContentNodeProviderPublishedVersion, Models.Contenttreenode>()
                    .ForMember(a => a.IconUrl, b => b.Ignore())
                    .ForMember(a => a.ControllerName, b => b.Ignore())
                    .ForMember(a => a.Id, b => b.MapFrom(c => c.TreeNodeId))
                ;
        }
	}
}
