using AutoMapper;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentTreeNodeInputModelToContentTreeNodeMapper
	{
		ContentTreePageNode CreateInstance(ContentTreeNodeInputModel source);
		void LoadIntoInstance(ContentTreeNodeInputModel source, ContentTreePageNode destination);
	}

	public class ContentTreeNodeInputModelToContentTreeNodeMapper : Mapper<ContentTreeNodeInputModel, ContentTreePageNode>, IContentTreeNodeInputModelToContentTreeNodeMapper
	{
		public override void DefineMap(IConfiguration configuration)
		{
			configuration.CreateMap<ContentTreeNodeInputModel, ContentTreePageNode>()
                    .ForMember(a => a.IconUrl, b=>b.Ignore())
                    .ForMember(a => a.LastModifyBy, b => b.Ignore())
                    .ForMember(a => a.LastModifyDate, b => b.Ignore())
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
