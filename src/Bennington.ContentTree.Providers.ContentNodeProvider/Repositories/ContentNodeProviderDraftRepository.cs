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
	    private readonly IConnectionStringRetriever connectionStringRetriever;

	    public ContentNodeProviderDraftRepository(IConnectionStringRetriever connectionStringRetriever)
	    {
	        this.connectionStringRetriever = connectionStringRetriever;
	    }

	    public IQueryable<ContentNodeProviderDraft> GetAllContentNodeProviderDrafts()
		{
            dynamic db = GetDatabase();
            var list = new List<ContentNodeProviderDraft>();
            list.AddRange(db.ContentNodeProviderDrafts.All().Cast<ContentNodeProviderDraft>());
            return list.AsQueryable();
		}

		public void Delete(ContentNodeProviderDraft instance)
		{
            dynamic db = GetDatabase();
            db.ContentNodeProviderDrafts.Delete(PageId: instance.PageId);
		}

		public void Update(ContentNodeProviderDraft instance)
		{
            dynamic db = GetDatabase();
            db.ContentNodeProviderDrafts.UpdateByPageId(instance);
		}

		public void Create(ContentNodeProviderDraft instance)
		{
            dynamic db = GetDatabase();
            if (instance.LastModifyDate == DateTime.MinValue) instance.LastModifyDate = new DateTime(1753, 1, 1);
            db.ContentNodeProviderDrafts.Insert(instance);
		}

        private object GetDatabase()
        {
            return Simple.Data.Database.OpenConnection(connectionStringRetriever.GetConnectionString());
        }
	}
}