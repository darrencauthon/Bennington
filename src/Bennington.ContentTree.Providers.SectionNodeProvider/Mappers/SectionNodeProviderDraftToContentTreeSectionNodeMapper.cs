using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapperAssist;
using Bennington.ContentTree.Providers.SectionNodeProvider.Data;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Repositories;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Mappers
{
	public interface ISectionNodeProviderDraftToContentTreeSectionNodeMapper
	{
		IEnumerable<ContentTreeSectionNode> CreateSet(IEnumerable<SectionNodeProviderDraft> source);
	}

	public class SectionNodeProviderDraftToContentTreeSectionNodeMapper : Mapper<SectionNodeProviderDraft, ContentTreeSectionNode>, ISectionNodeProviderDraftToContentTreeSectionNodeMapper
	{
	    private ITreeNodeRepository treeNodeRepository;

	    public SectionNodeProviderDraftToContentTreeSectionNodeMapper(ITreeNodeRepository treeNodeRepository)
	    {
	        this.treeNodeRepository = treeNodeRepository;
	    }

	    public override void DefineMap(AutoMapper.IConfiguration configuration)
        {
            configuration.CreateMap<SectionNodeProviderDraft, ContentTreeSectionNode>()
                .ForMember(a => a.Id, b => b.MapFrom(c => c.TreeNodeId))
                .ForMember(a => a.IconUrl, b => b.Ignore())
                .ForMember(a => a .ActionToUseForCreation,b => b.Ignore())
                .ForMember(a => a.ActionToUseForModification, b => b.Ignore())
                .ForMember(a=> a.RouteValuesForCreation, b => b.Ignore())
                .ForMember(a => a.RouteValuesForModification, b => b.Ignore())
                .ForMember(a => a.ParentTreeNodeId, b => b.Ignore())
                .ForMember(a => a.ControllerToUseForCreation, b => b.Ignore())
                .ForMember(e => e.ControllerToUseForModification, b => b.Ignore())
                .ForMember(a => a.MayHaveChildNodes,b => b.Ignore())
                .ForMember(a => a.HasChildren, b => b.Ignore())
                .ForMember(a => a.ControllerName, b => b.MapFrom(c => GetControllerName(c)))
                ;
        }

	    private string GetControllerName(SectionNodeProviderDraft sectionNodeProviderDraft)
	    {
	        var treeNode = treeNodeRepository.GetAll().Where(a => a.TreeNodeId == sectionNodeProviderDraft.TreeNodeId).FirstOrDefault();
            if (treeNode == null) return null;

	        return treeNode.ControllerName;
	    }
	}
}
