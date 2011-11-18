using System.Linq;
using AutoMapper;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Repositories;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentTreeNodeToContentTreeNodeInputModelMapper
	{
		ContentTreeNodeInputModel CreateInstance(Contenttreenode source);
	}

	public class ContentTreeNodeToContentTreeNodeInputModelMapper : Mapper<Contenttreenode, ContentTreeNodeInputModel>, IContentTreeNodeToContentTreeNodeInputModelMapper
	{
		private readonly ITreeNodeRepository treeNodeRepository;

		public ContentTreeNodeToContentTreeNodeInputModelMapper(ITreeNodeRepository treeNodeRepository)
		{
			this.treeNodeRepository = treeNodeRepository;
		}

		public override void DefineMap(IConfiguration configuration)
		{
			configuration.CreateMap<Contenttreenode, ContentTreeNodeInputModel>()
				.ForMember(dest => dest.FormAction, opt => opt.Ignore())
				.ForMember(dest => dest.RemoveHeaderImage, opt => opt.Ignore())
                .ForMember(a => a.TreeNodeId, b=> b.MapFrom(c => c.Id))
				.ForMember(dest => dest.ParentTreeNodeId, opt => opt.Ignore());
		}

		public override ContentTreeNodeInputModel CreateInstance(Contenttreenode source)
		{
			var returnInstance = base.CreateInstance(source);
			
			var treeNode = treeNodeRepository.GetAll().Where(a => a.Id == source.Id).FirstOrDefault();
			if (treeNode != null)
			{
                returnInstance.Type = treeNode.Type;
			    returnInstance.ControllerName = returnInstance.ControllerName ?? treeNode.ControllerName;
			}
				
			return returnInstance;
		}
	}
}
