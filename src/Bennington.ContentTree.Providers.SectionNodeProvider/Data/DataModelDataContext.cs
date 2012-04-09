using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bennington.ContentTree.Helpers;
using Bennington.Core.Helpers;

namespace Bennington.ContentTree.Providers.SectionNodeProvider.Data
{
	public interface IDataModelDataContext
	{
		IEnumerable<SectionNodeProviderDraft> GetAllSectionNodeProviderDrafts();
		void Create(SectionNodeProviderDraft instance);
		void Update(SectionNodeProviderDraft instance);
		void Delete(SectionNodeProviderDraft instance);
	}

	public class DataModelDataContext : IDataModelDataContext
	{
	    private readonly IGetPathToDataDirectoryService getPathToDataDirectoryService;
	    private readonly IDatabaseRetriever databaseRetriever;

	    public DataModelDataContext(IDatabaseRetriever databaseRetriever, 
                                    IGetPathToDataDirectoryService getPathToDataDirectoryService)
	    {
	        this.databaseRetriever = databaseRetriever;
	        this.getPathToDataDirectoryService = getPathToDataDirectoryService;
	    }

	    public IEnumerable<SectionNodeProviderDraft> GetAllSectionNodeProviderDrafts()
	    {
            var db = databaseRetriever.GetDatabase();
            var list = new List<SectionNodeProviderDraft>();
            list.AddRange(db.SectionNodeProviderDrafts.All().Cast<SectionNodeProviderDraft>());

	        return list;
	    }

	    public void Create(SectionNodeProviderDraft instance)
	    {
            var db = databaseRetriever.GetDatabase();
            if (instance.LastModifyDate == DateTime.MinValue) instance.LastModifyDate = new DateTime(1753, 1, 1);
            db.SectionNodeProviderDrafts.Insert(instance);

	        TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
	    }

	    public void Update(SectionNodeProviderDraft instance)
	    {
            var db = databaseRetriever.GetDatabase();
            db.SectionNodeProviderDrafts.UpdateBySectionId(instance);
	        
            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
	    }

	    public void Delete(SectionNodeProviderDraft instance)
	    {
            var db = databaseRetriever.GetDatabase();
            db.SectionNodeProviderDrafts.Delete(SectionId: instance.SectionId);

            TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges();
	    }

        private void TouchLegacyFilestorePathToInvalidateAnyCachesThatAreListeningForChanges()
        {
            var path = string.Format("{0}SectionNodeProviderDrafts.xml", getPathToDataDirectoryService.GetPathToDirectory());

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
