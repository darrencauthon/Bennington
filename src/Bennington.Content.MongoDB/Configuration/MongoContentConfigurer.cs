using System;
using Bennington.Content.Configuration;
using Bennington.Core.Configuration;

namespace Bennington.Content.MongoDB.Configuration
{
    public class MongoContentConfigurer : ContentConfigurer
    {
        private readonly string connectionString;
        private string contentTreeCollectionName;
        private string contentTypeCollectionName;
        private Uri invalidateCacheUrl;

        public MongoContentConfigurer(Configurer parentConfigurer, string connectionString)
            : base(parentConfigurer)
        {
            this.connectionString = connectionString;
            contentTreeCollectionName = "content_tree";
            contentTypeCollectionName = "content_type";
            invalidateCacheUrl = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ApplicationAssembly.GetName().Name.ToLower()));
        }

        public MongoContentConfigurer UseCollectionNames(string contentTreeName, string contentTypeName)
        {
            contentTreeCollectionName = contentTreeName;
            contentTypeCollectionName = contentTypeName;
            return this;
        }

        public MongoContentConfigurer UseInvalidateCacheUri(string uri)
        {
            invalidateCacheUrl = new Uri(uri);
            return this;
        }

        public MongoContentConfigurer UseInvalidateCacheUri(Uri uri)
        {
            invalidateCacheUrl = uri;
            return this;
        }

        public override void Run()
        {
            ServiceLocator.Register<IContentTreeProvider>(new MongoContentTreeProvider(connectionString, contentTreeCollectionName, invalidateCacheUrl));
            ServiceLocator.Register<IContentTypeRegistry>(new MongoContentTypeRegistry(connectionString, contentTypeCollectionName));

            base.Run();
        }
    }
}