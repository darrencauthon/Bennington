using AutoMapper;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentTreeNodeInputModelToContentTreeNodeMapper
	{
		Contenttreenode CreateInstance(ContentTreeNodeInputModel source);
		void LoadIntoInstance(ContentTreeNodeInputModel source, Contenttreenode destination);
	}

	public class ContentTreeNodeInputModelToContentTreeNodeMapper : Mapper<ContentTreeNodeInputModel, Contenttreenode>, IContentTreeNodeInputModelToContentTreeNodeMapper
	{
		public override void DefineMap(IConfiguration configuration)
		{
			configuration.CreateMap<ContentTreeNodeInputModel, Contenttreenode>()
                    .ForMember(a => a.IconUrl, b=>b.Ignore())
                    .ForMember(a => a.LastModifyBy, b => b.Ignore())
                    .ForMember(a => a.LastModifyDate, b => b.Ignore())
                    .ForMember(a => a.Id, b => b.MapFrom(c => c.TreeNodeId))
                ;
		}
	}
}
