using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bennington.FileUploadHandling.Context;
using Bennington.FileUploadHandling.Helpers;
using Bennington.FileUploadHandling.Models;

namespace Bennington.FileUploadHandling
{
    public class FileUploadInputModelBinder : DefaultModelBinder
    {
        private readonly IGetRootPathToSaveUploadedFilesToHelper getRootPathToSaveUploadedFilesToHelper;
        private readonly IFileUploadContext fileUploadContext;

        public FileUploadInputModelBinder(IGetRootPathToSaveUploadedFilesToHelper getRootPathToSaveUploadedFilesToHelper,
                                          IFileUploadContext fileUploadContext)
        {
            this.fileUploadContext = fileUploadContext;
            this.getRootPathToSaveUploadedFilesToHelper = getRootPathToSaveUploadedFilesToHelper;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(FileUploadInputModel)) return base.BindModel(controllerContext, bindingContext);

            var propertyName = bindingContext.ModelMetadata.PropertyName;

            var filesCollectionKey = controllerContext.RequestContext.HttpContext.Request.Files.AllKeys.Where(a => a.EndsWith(propertyName + ".FileUpload")).FirstOrDefault();
            if (filesCollectionKey == null)
                return base.BindModel(controllerContext, bindingContext);

            var file = controllerContext.RequestContext.HttpContext.Request.Files[filesCollectionKey];
            if ((file == null) || (string.IsNullOrEmpty(file.FileName))) return base.BindModel(controllerContext, bindingContext);

            var guid = Guid.NewGuid();

            var rootPathToSaveTo = getRootPathToSaveUploadedFilesToHelper.GetRootPathToSaveUploadedFilesTo(HttpContext.Current);
            var containerName = bindingContext.ModelMetadata.ContainerType.Name;
            var pathToSaveTo = string.Format("{0}{1}{5}{3}{2}{3}{6}{3}{4}",
                                             rootPathToSaveTo,
                                             rootPathToSaveTo.EndsWith(Path.DirectorySeparatorChar + string.Empty)
                                                 ? string.Empty
                                                 : rootPathToSaveTo + Path.DirectorySeparatorChar,
                                             guid,
                                             Path.DirectorySeparatorChar,
                                             file.FileName,
                                             containerName,
                                             propertyName);
            if (!Directory.Exists(Path.GetDirectoryName(pathToSaveTo)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(pathToSaveTo));
            }

            file.SaveAs(pathToSaveTo);

            return new FileUploadInputModel()
                       {
                           Id = guid.ToString(),
                           Filename = file.FileName,
                           ContainerName = containerName,
                           DirectoryName = GetDirectoryName(propertyName, guid, containerName),
                           UrlRelativeToFileUploadRoot = fileUploadContext.GetUrlRelativeToUploadRoot(containerName, propertyName, guid.ToString())
                       };
        }

        private string GetDirectoryName(string propertyName, Guid guid, string containerName)
        {
            var fileInfo = fileUploadContext.GetFileFromId(containerName, propertyName, guid.ToString());
            return fileInfo == null ? null : fileInfo.DirectoryName;
        }
    }
}
