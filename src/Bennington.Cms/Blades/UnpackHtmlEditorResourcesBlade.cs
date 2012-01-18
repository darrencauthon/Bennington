using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Bennington.Core.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.Cms.Blades
{
    public class UnpackHtmlEditorResourcesBlade : Blade
    {
        private readonly IGetPathToWebrootHelper getPathToWebrootHelper;

        public UnpackHtmlEditorResourcesBlade(IGetPathToWebrootHelper getPathToWebrootHelper)
        {
            this.getPathToWebrootHelper = getPathToWebrootHelper;
        }

        public override void Spin(IRotorContext context)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();
            var resourceNames = thisAssembly.GetManifestResourceNames();
            var pathToWebroot = getPathToWebrootHelper.GetPathToWebroot();
            if (!pathToWebroot.EndsWith(Path.DirectorySeparatorChar + ""))
                pathToWebroot = pathToWebroot + Path.DirectorySeparatorChar;

            foreach (var resourceName in resourceNames)
            {
                var partialPathToResource = GetFilenameFromResourceName(thisAssembly, resourceName);
                partialPathToResource = partialPathToResource.Substring(1, partialPathToResource.Length - 1);

                var pathToCopyTo = pathToWebroot + partialPathToResource;
                if (File.Exists(pathToCopyTo)) continue;

                using (var sourceFile = thisAssembly.GetManifestResourceStream(resourceName))
                {
                    if (sourceFile == null) continue;
                    
                    var folderToCopyInto = Path.GetDirectoryName(pathToCopyTo);
                    if (folderToCopyInto == null) continue;

                    if (!folderToCopyInto.Contains(string.Format("Scripts{0}HtmlEditor{0}", Path.DirectorySeparatorChar)))
                        continue;

                    if (!Directory.Exists(folderToCopyInto))
                        Directory.CreateDirectory(folderToCopyInto);

                    using (var destinationFile = new FileStream(pathToCopyTo, FileMode.CreateNew))
                    {
                        sourceFile.CopyTo(destinationFile);
                    }
                }
            }
        }

        private string GetFilenameFromResourceName(Assembly thisAssembly, string resource)
        {
            var filename = resource.Replace(thisAssembly.FullName.Split(',')[0], string.Empty).Replace('.', Path.DirectorySeparatorChar);
            var filenameChunks = filename.Split(Path.DirectorySeparatorChar);
            var fileExtension = filenameChunks[filenameChunks.Count() - 1];
            
            if (filename.Length > 4)
                filename = filename.Remove(filename.Length - (fileExtension.Length + 1));
            
            return filename + "." + fileExtension;
        }
    }
}