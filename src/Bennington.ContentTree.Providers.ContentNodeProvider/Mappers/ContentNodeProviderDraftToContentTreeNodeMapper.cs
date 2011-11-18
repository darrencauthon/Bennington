using System.Collections.Generic;
using AutoMapperAssist;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentNodeProviderDraftToContentTreeNodeMapper
	{
		IEnumerable<Models.ContentTreePageNode> CreateSet(IEnumerable<Data.ContentNodeProviderDraft> source);
	}

	public class ContentNodeProviderDraftToContentTreeNodeMapper : Mapper<Data.ContentNodeProviderDraft, Models.ContentTreePageNode>, IContentNodeProviderDraftToContentTreeNodeMapper
	{
		public override void DefineMap(AutoMapper.IConfiguration configuration)
		{
			configuration.CreateMap<Data.ContentNodeProviderDraft, Models.ContentTreePageNode>()
					.ForMember(a => a.Body, opt => opt.MapFrom(c => c.Body))
                    .ForMember(a => a.Id, b => b.MapFrom(c => c.TreeNodeId))
                    .ForMember(a => a.IconUrl, b => b.Ignore())
                    .ForMember(a => a.ControllerName, b => b.Ignore())
					.ForMember(a => a.Action, opt => opt.MapFrom(c => c.Action))
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
