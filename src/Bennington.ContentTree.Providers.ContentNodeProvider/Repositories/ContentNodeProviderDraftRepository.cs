using System;
using System.Collections.Generic;
using System.Linq;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Repositories
{
	public interface IContentNodeProviderDraftRepository
	{
		IQueryable<ContentNodeProviderDraft> GetAllContentNodeProviderDrafts();
		void Update(ContentNodeProviderDraft instance);
		void Create(ContentNodeProviderDraft instance);
		void Delete(ContentNodeProviderDraft instance);
	}

	public class ContentNodeProviderDraftRepository : IContentNodeProviderDraftRepository
	{
	    private readonly IDatabaseRetriever databaseRetriever;

	    public ContentNodeProviderDraftRepository(IDatabaseRetriever databaseRetriever)
	    {
	        this.databaseRetriever = databaseRetriever;
	    }

	    public IQueryable<ContentNodeProviderDraft> GetAllContentNodeProviderDrafts()
		{
            var db = databaseRetriever.GetDatabase();
            var list = new List<ContentNodeProviderDraft>();
            list.AddRange(db.ContentNodeProviderDrafts.All().Cast<ContentNodeProviderDraft>());
            return list.AsQueryable();
		}

		public void Delete(ContentNodeProviderDraft instance)
		{
            var db = databaseRetriever.GetDatabase();
            db.ContentNodeProviderDrafts.Delete(PageId: instance.PageId);
		}

		public void Update(ContentNodeProviderDraft instance)
		{
            var db = databaseRetriever.GetDatabase();
            db.ContentNodeProviderDrafts.UpdateByPageId(instance);
		}

		public void Create(ContentNodeProviderDraft instance)
		{
            var db = databaseRetriever.GetDatabase();
            if (instance.LastModifyDate == DateTime.MinValue) instance.LastModifyDate = new DateTime(1753, 1, 1);
            db.ContentNodeProviderDrafts.Insert(instance);
		}
	}
}