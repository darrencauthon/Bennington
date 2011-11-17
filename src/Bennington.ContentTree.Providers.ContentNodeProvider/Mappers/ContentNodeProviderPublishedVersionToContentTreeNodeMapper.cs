using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
		IEnumerable<Models.ContentTreeNode> CreateSet(IEnumerable<ContentNodeProviderPublishedVersion> source);
	}

	public class ContentNodeProviderPublishedVersionToContentTreeNodeMapper : Mapper<ContentNodeProviderPublishedVersion, Models.ContentTreeNode>, IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ContentNodeProviderPublishedVersion, Models.ContentTreeNode>()
                    .ForMember(a => a.IconUrl, b => b.Ignore())
                    .ForMember(a => a.ControllerName, b => b.Ignore())
                ;
        }
	}
}
