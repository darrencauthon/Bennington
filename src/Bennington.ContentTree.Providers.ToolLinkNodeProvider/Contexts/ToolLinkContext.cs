using System.Collections.Generic;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Models;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider.Repositories;

namespace Bennington.ContentTree.Providers.ToolLinkNodeProvider.Contexts
{
	public interface IToolLinkContext
	{
		IEnumerable<ContentTreeToolLinkNode> GetAllToolLinks();
	}

	public class ToolLinkContext : IToolLinkContext
	{
		private readonly IToolLinkProviderDraftToToolLinkMapper toolLinkProviderDraftToToolLinkMapper;
		private readonly IToolLinkProviderDraftRepository toolLinkProviderDraftRepository;

		public ToolLinkContext(IToolLinkProviderDraftToToolLinkMapper toolLinkProviderDraftToToolLinkMapper,
								IToolLinkProviderDraftRepository toolLinkProviderDraftRepository)
		{
			this.toolLinkProviderDraftRepository = toolLinkProviderDraftRepository;
			this.toolLinkProviderDraftToToolLinkMapper = toolLinkProviderDraftToToolLinkMapper;
		}

		public IEnumerable<ContentTreeToolLinkNode> GetAllToolLinks()
		{
			return toolLinkProviderDraftToToolLinkMapper.CreateSet(toolLinkProviderDraftRepository.GetAll());
		}
	}
}