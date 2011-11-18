using System;
using System.Linq;
using Bennington.ContentTree.Contexts;
using Bennington.ContentTree.Providers.SectionNodeProvider.Models;
using Bennington.ContentTree.Providers.SectionNodeProvider.Repositories;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Context
{
	public interface IContentTreeSectionNodeContext
	{
		string CreateAndReturnTreeNodeId(ContentTreeSectionInputModel contentTreeSectionInputModel);
		void Delete(string id);
	}

	public class ContentTreeSectionNodeContext : IContentTreeSectionNodeContext
	{
		private readonly IContentTreeSectionNodeRepository contentTreeSectionNodeRepository;
		private readonly IContentTree contentTree;

		public ContentTreeSectionNodeContext(IContentTreeSectionNodeRepository contentTreeSectionNodeRepository, IContentTree contentTree)
		{
			this.contentTree = contentTree;
			this.contentTreeSectionNodeRepository = contentTreeSectionNodeRepository;
		}

		public string CreateAndReturnTreeNodeId(ContentTreeSectionInputModel contentTreeSectionInputModel)
		{
			throw new NotImplementedException();
			var newTreeNodeId = contentTree.Create(contentTreeSectionInputModel.ParentTreeNodeId, typeof(SectionNodeProvider).AssemblyQualifiedName, null);
			contentTreeSectionInputModel.TreeNodeId = newTreeNodeId;
			return contentTreeSectionInputModel.TreeNodeId;
		}

		public void Delete(string id)
		{
			var node = contentTreeSectionNodeRepository.GetAllContentTreeSectionNodes().Where(a => a.TreeNodeId == id).FirstOrDefault();
			if (node != null)
				contentTreeSectionNodeRepository.Delete(node);
		}
	}
}
