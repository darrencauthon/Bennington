using System;
using System.Linq;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Models;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.Core;

namespace Bennington.ContentTree.Providers.ContentNodeProvider
{
	public interface IContentTreeNodeVersionContext
	{
		IQueryable<ContentTreePageNode> GetAllContentTreePageNodes();
	}

	public class ContentTreeNodeVersionContext : IContentTreeNodeVersionContext
	{
		private readonly Func<IContentNodeProviderDraftRepository> contentNodeProviderDraftRepository;
		private readonly Func<IContentNodeProviderDraftToContentTreeNodeMapper> contentNodeProviderDraftToContentTreeNodeMapper;
		private readonly Func<IVersionContext> versionContext;
		private readonly Func<IContentNodeProviderPublishedVersionToContentTreeNodeMapper> contentNodeProviderPublishedVersionToContentTreeNodeMapper;
		private readonly Func<IContentNodeProviderPublishedVersionRepository> contentNodeProviderPublishedVersionRepository;

		public ContentTreeNodeVersionContext(Func<IContentNodeProviderDraftRepository> contentNodeProviderDraftRepository,
										Func<IContentNodeProviderDraftToContentTreeNodeMapper> contentNodeProviderDraftToContentTreeNodeMapper,
										Func<IVersionContext> versionContext,
										Func<IContentNodeProviderPublishedVersionToContentTreeNodeMapper> contentNodeProviderPublishedVersionToContentTreeNodeMapper,
										Func<IContentNodeProviderPublishedVersionRepository> contentNodeProviderPublishedVersionRepository)
		{
			this.contentNodeProviderPublishedVersionRepository = contentNodeProviderPublishedVersionRepository;
			this.contentNodeProviderPublishedVersionToContentTreeNodeMapper = contentNodeProviderPublishedVersionToContentTreeNodeMapper;
			this.versionContext = versionContext;
			this.contentNodeProviderDraftToContentTreeNodeMapper = contentNodeProviderDraftToContentTreeNodeMapper;
			this.contentNodeProviderDraftRepository = contentNodeProviderDraftRepository;
		}

		public IQueryable<ContentTreePageNode> GetAllContentTreePageNodes()
		{
		    var versionContextImpl = versionContext();
            if (versionContextImpl.GetCurrentVersionId() == VersionContext.Publish)
			{
			    var contentNodeProviderPublishedVersions = contentNodeProviderPublishedVersionRepository().GetAllContentNodeProviderPublishedVersions().Where(a => a.Inactive == false);
				return contentNodeProviderPublishedVersionToContentTreeNodeMapper().CreateSet(contentNodeProviderPublishedVersions).AsQueryable();
			}

            if (versionContextImpl.GetCurrentVersionId() == VersionContext.Manage)
			{
				return contentNodeProviderDraftToContentTreeNodeMapper()
						.CreateSet(contentNodeProviderDraftRepository().GetAllContentNodeProviderDrafts()).AsQueryable();
			}

			return contentNodeProviderDraftToContentTreeNodeMapper()
					.CreateSet(contentNodeProviderDraftRepository().GetAllContentNodeProviderDrafts().Where(a => a.Inactive == false)).AsQueryable();
		}
	}
}