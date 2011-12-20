using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Bennington.Core.Helpers
{
    public interface IGetPathToWebrootHelper
    {
        string GetPathToWebroot();
    }

    public class GetPathToWebrootHelper : IGetPathToWebrootHelper
    {
        private readonly IApplicationSettingsValueGetter applicationSettingsValueGetter;

        public GetPathToWebrootHelper(IApplicationSettingsValueGetter applicationSettingsValueGetter)
        {
            this.applicationSettingsValueGetter = applicationSettingsValueGetter;
        }

        public string GetPathToWebroot()
        {
            var overridePath = applicationSettingsValueGetter.GetValue("Bennington.PathToWebroot");
            if (!string.IsNullOrEmpty(overridePath))
                return overridePath;

            const string localWorkingFolderName = "localWorkingFolder";

            var x = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (x.IndexOf("DevServer", 0) > 0)
            {
                var rootPath = string.Empty;
                try
                {
                    rootPath = HttpContext.Current.Server.MapPath("/");
                }
                catch (Exception)
                {
                    rootPath = HttpContext.Current.Server.MapPath("/Manage");
                }

                return rootPath;
            }

            return Directory.GetParent(HttpContext.Current.Server.MapPath("/")).Parent.FullName + 
                                                    Path.DirectorySeparatorChar;
        }
    }
}
