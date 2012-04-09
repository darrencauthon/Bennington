using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bennington.ContentTree.Helpers
{
    public interface IDatabaseRetriever
    {
        dynamic GetDatabase();
    }

    public class DatabaseRetriever : IDatabaseRetriever
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;

        public DatabaseRetriever(IConnectionStringRetriever connectionStringRetriever)
        {
            this.connectionStringRetriever = connectionStringRetriever;
        }

        public dynamic GetDatabase()
        {
            return Simple.Data.Database.OpenConnection(connectionStringRetriever.GetConnectionString());
        }
    }
}