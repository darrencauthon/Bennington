using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Bennington.ContentTree.Helpers
{
    public interface IConnectionStringRetriever
    {
        string GetConnectionString();
    }

    public class ConnectionStringRetriever : IConnectionStringRetriever
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString;
        }
    }
}