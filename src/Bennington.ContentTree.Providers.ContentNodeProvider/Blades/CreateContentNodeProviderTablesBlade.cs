using System.Data.SqlClient;
using Bennington.ContentTree.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Providers.ContentNodeProvider.Blades
{
    public class CreateContentNodeProviderTablesBlade : IBlade
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;

        public CreateContentNodeProviderTablesBlade(IConnectionStringRetriever connectionStringRetriever)
        {
            this.connectionStringRetriever = connectionStringRetriever;
        }

        public void Spin(IRotorContext context)
        {
            using (var sqlConnection = new SqlConnection(connectionStringRetriever.GetConnectionString()))
            {
                sqlConnection.Open();
                var command = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentNodeProviderDrafts]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentNodeProviderDrafts](
	[PageId] [nvarchar](500) NULL,
	[TreeNodeId] [nvarchar](500) NULL,
	[UrlSegment] [nvarchar](max) NULL,
	[Sequence] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[HeaderText] [nvarchar](max) NULL,
	[HeaderImage] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Inactive] [bit] NULL,
	[Hidden] [bit] NULL,
	[LastModifyDate] [datetime] NULL,
	[LastModifyBy] [nvarchar](max) NULL
) ON [PRIMARY]
END
", sqlConnection);
                command.ExecuteNonQuery();

                var command2 = new SqlCommand(@"IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ContentNodeProviderPublishedVersions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ContentNodeProviderPublishedVersions](
	[PageId] [nvarchar](500) NULL,
	[TreeNodeId] [nvarchar](500) NULL,
	[UrlSegment] [nvarchar](max) NULL,
	[Sequence] [int] NULL,
	[Name] [nvarchar](max) NULL,
	[Action] [nvarchar](max) NULL,
	[MetaTitle] [nvarchar](max) NULL,
	[MetaDescription] [nvarchar](max) NULL,
	[MetaKeywords] [nvarchar](max) NULL,
	[HeaderText] [nvarchar](max) NULL,
	[HeaderImage] [nvarchar](max) NULL,
	[Body] [nvarchar](max) NULL,
	[Inactive] [bit] NULL,
	[Hidden] [bit] NULL,
	[LastModifyDate] [datetime] NULL,
	[LastModifyBy] [nvarchar](max) NULL
) ON [PRIMARY]
END
", sqlConnection);
                command2.ExecuteNonQuery();
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