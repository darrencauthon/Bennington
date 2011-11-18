using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Data;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Mappers
{
	public interface IToolLinkProviderDraftToToolLinkMapper
	{
		IEnumerable<ContentTreeToolLinkNode> CreateSet(IEnumerable<ToolLinkProviderDraft> source);
	}

	public class ToolLinkProviderDraftToToolLinkMapper : Mapper<ToolLinkProviderDraft, ContentTreeToolLinkNode>, IToolLinkProviderDraftToToolLinkMapper
	{
        public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<ToolLinkProviderDraft, ContentTreeToolLinkNode>()
                    .ForMember(a => a.IconUrl, b => b.UseValue("Content/ToolLinkProviderNode/ToolLinkProviderNode.gif"))
                    .ForMember(a => a.Id, b => b.MapFrom(c => c.Id))
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