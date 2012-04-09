using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Data;
using Bennington.Core.Helpers;

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
	    private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;

	    public ContentNodeProviderDraftRepository(IDatabaseRetriever databaseRetriever, IGetPathToDataDirectoryService getPathToDataDirectoryService)
	    {
	        this.getPathToDataDirectoryService = getPathToDataDirectoryService;
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
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
		}

	    public void Update(ContentNodeProviderDraft instance)
		{
            var db = databaseRetriever.GetDatabase();
            db.ContentNodeProviderDrafts.UpdateByPageId(instance);
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
		}

		public void Create(ContentNodeProviderDraft instance)
		{
            var db = databaseRetriever.GetDatabase();
            if (instance.LastModifyDate == DateTime.MinValue) instance.LastModifyDate = new DateTime(1753, 1, 1);
            db.ContentNodeProviderDrafts.Insert(instance);
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
		}

        private void TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges()
        {
            var path = string.Format("{0}ContentNodeProviderDrafts.xml", getPathToDataDirectoryService.GetPathToDirectory());

            if (!File.Exists(path))
            {
                using (var fileStream = File.Create(path))
                {
                }    
            }

            using (var writer = File.AppendText(path))
            {
                writer.WriteLine(string.Empty);
            }
        }
	}
}