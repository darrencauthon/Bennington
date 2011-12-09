using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Bennington.Core.Helpers;
using Bennington.FileUploadHandling.Helpers;

namespace Bennington.FileUploadHandling.Context
{
    public interface IFileUploadContext
    {
        FileInfo GetFileFromId(string inputModelName, string inputModelPropertyName, string id);
        string GetUrlRelativeToUploadRoot(string inputModelName, string inputModelPropertyName, string id);
        string GetUrlForFileUploadFolder();
    }

    public class FileUploadContext : IFileUploadContext
    {
        private readonly IGetRootPathToSaveUploadedFilesToHelper getRootPathToSaveUploadedFilesToHelper;
        private readonly IGetUrlToResourceFolderHelper getUrlToResourceFolderHelper;

        public FileUploadContext(IGetRootPathToSaveUploadedFilesToHelper getRootPathToSaveUploadedFilesToHelper,
                                 IGetUrlToResourceFolderHelper getUrlToResourceFolderHelper)
        {
            this.getUrlToResourceFolderHelper = getUrlToResourceFolderHelper;
            this.getRootPathToSaveUploadedFilesToHelper = getRootPathToSaveUploadedFilesToHelper;
        }

        public FileInfo GetFileFromId(string inputModelName, string inputModelPropertyName, string id)
        {
            var rootPathToSaveTo = getRootPathToSaveUploadedFilesToHelper.GetRootPathToSaveUploadedFilesTo(HttpContext.Current);

            var pathToCheckForFile = string.Format("{0}{1}{4}{3}{2}{3}{5}{3}",
                                 rootPathToSaveTo,
                                 rootPathToSaveTo.EndsWith(Path.DirectorySeparatorChar + string.Empty)
                                     ? string.Empty
                                     : rootPathToSaveTo + Path.DirectorySeparatorChar,
                                 id,
                                 Path.DirectorySeparatorChar,
                                 GetNameFromFullname(inputModelName),
                                 inputModelPropertyName);

            if (!Directory.Exists(pathToCheckForFile))
                return null;

            var files = Directory.GetFiles(pathToCheckForFile);
            if (files.Count() == 0) return null;

            var pathToFile = files.First();

            if (!File.Exists(pathToFile)) return null;

            return new FileInfo(pathToFile);
        }

        public string GetUrlRelativeToUploadRoot(string inputModelName, string inputModelPropertyName, string id)
        {
            var fileInfo = GetFileFromId(inputModelName, inputModelPropertyName, id);
            return GetUrlRelativeToUploadRoot(fileInfo);
        }

        public string GetUrlForFileUploadFolder()
        {
            return getUrlToResourceFolderHelper.GetUrlToResourceFolder();
        }

        private string GetUrlRelativeToUploadRoot(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                return null;
            }
            if (!fileInfo.DirectoryName.StartsWith(getRootPathToSaveUploadedFilesToHelper.GetRootPathToSaveUploadedFilesTo(HttpContext.Current)))
            {
                return null;
            }

            var pathArray = fileInfo.DirectoryName.Split(Path.DirectorySeparatorChar);

            var list = new List<string>();
            for (var n = pathArray.Length - 1; n >= pathArray.Length - 3; n--)
            {
                list.Add(pathArray[n]);
            }
            list.Reverse();
            var stringBuilder = new StringBuilder();
            foreach (var chunk in list)
            {
                stringBuilder.Append(chunk + "/");
            }

            return string.Format("{0}{1}", stringBuilder, fileInfo.Name);
        }

        private static string GetNameFromFullname(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
                throw new Exception("Empty model name");

            var array = modelName.Split('.');
            if (array.Length > 1)
                return modelName.Split('.')[array.Length - 1];

            return modelName;
        }
    }
}
