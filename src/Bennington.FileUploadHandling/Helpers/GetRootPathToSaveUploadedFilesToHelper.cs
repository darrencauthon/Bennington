using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace Bennington.FileUploadHandling.Helpers
{
    public interface IGetRootPathToSaveUploadedFilesToHelper
    {
        string GetRootPathToSaveUploadedFilesTo(HttpContext httpContext);
    }

    public class GetRootPathToSaveUploadedFilesToHelper : IGetRootPathToSaveUploadedFilesToHelper
    {
        public string GetRootPathToSaveUploadedFilesTo(HttpContext httpContext)
        {
            if (string.IsNullOrEmpty(GetAppSettingsFileUploadPath()))
                return httpContext.Server.MapPath("~/App_Data");

            return GetAppSettingsFileUploadPath();
        }

        private static string GetAppSettingsFileUploadPath()
        {
            return ConfigurationManager.AppSettings["Bennington.LocalWorkingFolder"];
        }
    }
}
