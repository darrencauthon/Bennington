using System.Linq;
using AutoMapper;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Repositories;
using Bennington.FileUploadHandling.Models;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentTreeNodeToContentTreeNodeInputModelMapper
	{
		ContentTreeNodeInputModel CreateInstance(ContentTreePageNode source);
	}

	public class ContentTreeNodeToContentTreeNodeInputModelMapper : Mapper<ContentTreePageNode, ContentTreeNodeInputModel>, IContentTreeNodeToContentTreeNodeInputModelMapper
	{
		private readonly ITreeNodeRepository treeNodeRepository;

		public ContentTreeNodeToContentTreeNodeInputModelMapper(ITreeNodeRepository treeNodeRepository)
		{
			this.treeNodeRepository = treeNodeRepository;
		}

		public override void DefineMap(IConfiguration configuration)
		{
		    configuration.CreateMap<string, FileUploadInputModel>()
                .ForMember(a => a.ContainerName, b => b.Ignore())
                .ForMember(a => a.DirectoryName, b=> b.Ignore())
                .ForMember(a => a.Filename, b => b.Ignore())
                .ForMember(a => a.FileUpload, b => b.Ignore())
                .ForMember(a => a.Id, b => b.MapFrom(c => c))
                .ForMember(a => a.UrlRelativeToFileUploadRoot, b => b.Ignore())
                .ForMember(a => a.Remove, b => b.Ignore())
                ;
			configuration.CreateMap<ContentTreePageNode, ContentTreeNodeInputModel>()
				.ForMember(dest => dest.FormAction, opt => opt.Ignore())
                .ForMember(a => a.TreeNodeId, b=> b.MapFrom(c => c.Id))
				.ForMember(dest => dest.ParentTreeNodeId, opt => opt.Ignore())
                .ForMember(a => a.HeaderImage, b => b.Ignore());
		}

		public override ContentTreeNodeInputModel CreateInstance(ContentTreePageNode source)
		{
			var returnInstance = base.CreateInstance(source);
			
			var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == source.Id).FirstOrDefault();
			if (treeNode != null)
			{
                returnInstance.Type = treeNode.Type;
			    returnInstance.ControllerName = returnInstance.ControllerName ?? treeNode.ControllerName;
			}
				
            returnInstance.HeaderImage = new FileUploadInputModel()
                                             {
                                                 Id = source.HeaderImage
                                             };

			return returnInstance;
		}
	}
}
