using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Bennington.ContentTree.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Blades
{
    public class CreateTreeNodeTableBlade : IBlade
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;

        public CreateTreeNodeTableBlade(IConnectionStringRetriever connectionStringRetriever)
        {
            this.connectionStringRetriever = connectionStringRetriever;
        }

        public void Spin(IRotorContext context)
        {
            using (var sqlConnection = new SqlConnection(connectionStringRetriever.GetConnectionString()))
            {
                sqlConnection.Open();
                var command = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TreeNodes]') AND type in (N'U'))
                                                BEGIN
                                                CREATE TABLE [dbo].[TreeNodes](
	                                                [TreeNodeId] [nvarchar](500) NULL,
	                                                [ParentTreeNodeId] [nvarchar](500) NULL,
	                                                [Type] [nvarchar](500) NULL,
	                                                [ControllerName] [nvarchar](500) NULL
                                                ) ON [PRIMARY]
                                                END
                                                ", sqlConnection);
                command.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
        }

        public void Initialize(IRotorContext context)
        {
        }
    }
}