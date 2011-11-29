using System;
using System.Configuration;
using System.Linq;
using Bennington.Content.Configuration;
using Bennington.Content.Data;
using Bennington.Content.Sql.Data;
using Bennington.Core.Configuration;

namespace Bennington.Content.Sql.Configuration
{
    public class SqlContentConfigurer : ContentConfigurer
    {
        private readonly string connectionString;
        private Uri invalidateCacheUrl;

        public SqlContentConfigurer(Configurer parentConfigurer, string connectionString)
            : base(parentConfigurer)
        {
            this.connectionString = connectionString;
            invalidateCacheUrl = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", GetKeyForEndpointListener()));
        }

        private string GetKeyForEndpointListener()
        {
            var cacheListenerCachekey = "Bennington.Content.Routing.Cache.Listener.CacheKey";
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings[cacheListenerCachekey]))
                return ApplicationAssembly.GetName().Name.ToLower();

            return ConfigurationManager.AppSettings[cacheListenerCachekey];
        }

        public SqlContentConfigurer UseInvalidateCacheUri(string uri)
        {
            invalidateCacheUrl = new Uri(uri);
            return this;
        }

        public SqlContentConfigurer UseInvalidateCacheUri(Uri uri)
        {
            invalidateCacheUrl = uri;
            return this;
        }

        public override void Run()
        {
            EnsureNecessaryTablesExist(connectionString);

            ServiceLocator.Register<IContentTreeProvider>(new SqlContentTreeProvider(connectionString, invalidateCacheUrl));
            ServiceLocator.Register<IContentTypeRegistry>(new SqlContentTypeRegistry(connectionString));

            base.Run();
        }

        private void EnsureNecessaryTablesExist(string connectionString)
        {
            using (var dataContext = new ContentDataContext(connectionString))
            {
                CreateContentTreeTableIfNecessary(dataContext);
                CreateContentTypesTableIfNecessary(dataContext);
                CreateContentActionsTableIfNecessary(dataContext);
            }
        }

        private void CreateContentTypesTableIfNecessary(ContentDataContext dataContext)
        {
            try
            {
                var items = dataContext.ContentTypeItems.Take(1).ToArray();
            }
            catch (Exception exception)
            {
                if (exception.Message == "Invalid object name 'dbo.ContentTypes'.")
                {
                    dataContext.ExecuteCommand(string.Format(@"
                                        CREATE TABLE [dbo].[ContentTypes](
	                                        [ContentTypeId] [int] IDENTITY(1,1) NOT NULL,
	                                        [Type] [nvarchar](500) NULL,
	                                        [ControllerName] [nvarchar](500) NULL,
	                                        [DisplayName] [nvarchar](500) NULL,
                                         CONSTRAINT [PK_ContentTypeItem] PRIMARY KEY CLUSTERED 
                                        (
	                                        [ContentTypeId] ASC
                                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]
                                        "), new object[] { });
                }
            }
        }

        private void CreateContentActionsTableIfNecessary(ContentDataContext dataContext)
        {
            try
            {
                var items = dataContext.ContentActionItems.Take(1).ToArray();
            }
            catch (Exception exception)
            {
                if (exception.Message == "Invalid object name 'dbo.ContentActions'.")
                {
                    dataContext.ExecuteCommand(string.Format(@"
                                        CREATE TABLE [dbo].[ContentActions](
	                                        [ContentActionId] [int] IDENTITY(1,1) NOT NULL,
	                                        [ContentTypeId] [int] NULL,
	                                        [ContentType] [nvarchar](500) NULL,
	                                        [Action] [nvarchar](500) NULL,
	                                        [DisplayName] [nvarchar](500) NULL,
                                            CONSTRAINT [PK_ContentActionItem] PRIMARY KEY CLUSTERED 
                                        (
	                                        [ContentActionId] ASC
                                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]
                                        "), new object[] { });
                }
            }
        }


        private void CreateContentTreeTableIfNecessary(ContentDataContext dataContext)
        {
            try
            {
                var items = dataContext.ContentTreeItems.Take(1).ToArray();
            }catch(Exception exception)
            {
                if (exception.Message == "Invalid object name 'dbo.ContentTree'.")
                {
                    dataContext.ExecuteCommand(string.Format(@"
                                        CREATE TABLE [dbo].[ContentTree](
	                                        [Id] [nvarchar](50) NOT NULL,
	                                        [ParentId] [nvarchar](50) NOT NULL,
	                                        [Segment] [nvarchar](500) NULL,
	                                        [Action] [nvarchar](500) NULL,
	                                        [Controller] [nvarchar](500) NULL,
	                                        [TreeNodeId] [nvarchar](50) NULL,
	                                        [ActionId] [nvarchar](50) NULL,
                                            CONSTRAINT [PK_ContentTreeItem] PRIMARY KEY CLUSTERED 
                                        (
	                                        [Id] ASC
                                        )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                                        ) ON [PRIMARY]
                                        "), new object[]{});
                }
            }
        }
    }
}