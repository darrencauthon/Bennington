using System;
using Bennington.Content.Data;
using Bennington.Core.Caching;
using MongoDB.Driver;

namespace Bennington.Content.MongoDB
{
    public class MongoContentTreeProvider : IContentTreeProvider
    {
        private readonly MongoServer mongoServer;
        private readonly string collectionName;
        private readonly string databaseName;
        private readonly InvalidateCacheEndpoint cacheEndpoint;

        public MongoContentTreeProvider(string connectionString, string collectionName, Uri invalidateCacheUri)
        {
            ContentChanged += (sender, e) => { };
            var builder = new MongoUrlBuilder(connectionString);
            this.collectionName = collectionName;
            databaseName = builder.DatabaseName;
            mongoServer = MongoServer.Create(connectionString);
            cacheEndpoint = new InvalidateCacheEndpoint(invalidateCacheUri, InvalidateCache);
            cacheEndpoint.Open();
        }

        private void InvalidateCache(string cacheKey)
        {
            ContentChanged(this, new EventArgs());
        }

        public event EventHandler<EventArgs> ContentChanged;

        public void Refresh()
        {
            InvalidateCache(string.Empty);
        }

        public ContentTree GetContentTree()
        {
            var database = mongoServer.GetDatabase(databaseName);
            var collection = database.GetCollection(collectionName);
            var nodes = collection.FindAllAs<ContentNode>();

            return ContentTree.BuildTree(nodes);
        }

        public void Dispose()
        {
            cacheEndpoint.Dispose();
        }
    }
}