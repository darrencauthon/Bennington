using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.ContentTree.Repositories;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Mappers
{
	public interface IContentNodeProviderDraftToContentTreeNodeMapper
	{
		IEnumerable<Models.ContentTreePageNode> CreateSet(IEnumerable<Data.ContentNodeProviderDraft> source);
	}

	public class ContentNodeProviderDraftToContentTreeNodeMapper : Mapper<Data.ContentNodeProviderDraft, Models.ContentTreePageNode>, IContentNodeProviderDraftToContentTreeNodeMapper
	{
	    private ITreeNodeRepository treeNodeRepository;

	    public ContentNodeProviderDraftToContentTreeNodeMapper(ITreeNodeRepository treeNodeRepository)
	    {
	        this.treeNodeRepository = treeNodeRepository;
	    }

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
                    .ForMember(a => a.ControllerName, b => b.MapFrom(c => GetControllerName(c)))
                    ;
		}

	    private string GetControllerName(ContentNodeProviderDraft contentNodeProviderDraft)
	    {
            var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == contentNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null) return null;

	        return treeNode.ControllerName;
	    }
	}
}
