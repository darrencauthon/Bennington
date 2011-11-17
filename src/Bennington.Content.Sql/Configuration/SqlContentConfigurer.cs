using System;
using Bennington.Content.Configuration;
using Bennington.Content.Data;
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
            invalidateCacheUrl = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ApplicationAssembly.GetName().Name.ToLower()));
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

            ServiceLocator.Register<IContentTreeProvider>(new SqlContentTreeProvider(connectionString, invalidateCacheUrl));
            ServiceLocator.Register<IContentTypeRegistry>(new SqlContentTypeRegistry(connectionString));

            base.Run();
        }
    }
}