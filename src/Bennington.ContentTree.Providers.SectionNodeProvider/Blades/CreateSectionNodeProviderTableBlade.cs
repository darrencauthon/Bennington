using System.Data.SqlClient;
using Bennington.ContentTree.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Blades
{
    public class CreateSectionNodeProviderTableBlade : IBlade
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;

        public CreateSectionNodeProviderTableBlade(IConnectionStringRetriever connectionStringRetriever)
        {
            this.connectionStringRetriever = connectionStringRetriever;
        }

        public void Spin(IRotorContext context)
        {
            using (var sqlConnection = new SqlConnection(connectionStringRetriever.GetConnectionString()))
            {
                sqlConnection.Open();
                var command = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SectionNodeProviderDrafts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SectionNodeProviderDrafts](
	[SectionId] [nvarchar](500) NULL,
	[TreeNodeId] [nvarchar](500) NULL,
	[Sequence] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[UrlSegment] [nvarchar](max) NULL,
	[DefaultTreeNodeId] [nvarchar](500) NULL,
	[Inactive] [bit] NULL,
	[Hidden] [bit] NULL,
	[LastModifyDate] [datetime] NULL,
	[LastModifyBy] [nvarchar](max) NULL
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