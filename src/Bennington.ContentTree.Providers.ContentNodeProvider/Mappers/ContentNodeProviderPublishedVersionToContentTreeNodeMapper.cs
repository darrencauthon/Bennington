using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
		IEnumerable<Models.ContentTreePageNode> CreateSet(IEnumerable<ContentNodeProviderPublishedVersion> source);
	}

	public class ContentNodeProviderPublishedVersionToContentTreeNodeMapper : Mapper<ContentNodeProviderPublishedVersion, Models.ContentTreePageNode>, IContentNodeProviderPublishedVersionToContentTreeNodeMapper
	{
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ContentNodeProviderPublishedVersion, Models.ContentTreePageNode>()
                    .ForMember(a => a.IconUrl, b => b.Ignore())
                    .ForMember(a => a.ControllerName, b => b.Ignore())
                    .ForMember(a => a.Id, b => b.MapFrom(c => c.TreeNodeId))
                    .ForMember(a => a.ActionToUseForCreation, b => b.Ignore())
                    .ForMember(a => a.ActionToUseForModification, b => b.Ignore())
                    .ForMember(a => a.RouteValuesForCreation, b => b.Ignore())
                    .ForMember(a => a.RouteValuesForModification, b => b.Ignore())
                    .ForMember(a => a.ParentTreeNodeId, b => b.Ignore())
                    .ForMember(a => a.ControllerToUseForCreation, b => b.Ignore())
                    .ForMember(e => e.ControllerToUseForModification, b => b.Ignore())
                    .ForMember(a => a.MayHaveChildNodes, b => b.Ignore())
                    .ForMember(a => a.HasChildren, b => b.Ignore())
                    ;
        }
	}
}
