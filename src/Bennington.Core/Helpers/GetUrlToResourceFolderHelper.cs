using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Bennington.Core.Helpers
{
    public interface IGetUrlToResourceFolderHelper
    {
        string GetUrlToResourceFolder();
    }

    public class GetUrlToResourceFolderHelper : IGetUrlToResourceFolderHelper
    {
        public string GetUrlToResourceFolder()
        {
            return ConfigurationManager.AppSettings["ResourceFolderUrl"];
        }
    }
}
