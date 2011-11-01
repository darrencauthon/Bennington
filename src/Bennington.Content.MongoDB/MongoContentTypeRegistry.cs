using Bennington.Content.Data;
using MongoDB.Driver;

namespace Bennington.Content.MongoDB
{
    public class MongoContentTypeRegistry : IContentTypeRegistry
    {
        private readonly MongoServer mongoServer;
        private readonly string collectionName;
        private readonly string databaseName;

        public MongoContentTypeRegistry(string connectionString, string collectionName)
        {
            var builder = new MongoUrlBuilder(connectionString);
            this.collectionName = collectionName;
            databaseName = builder.DatabaseName;
            mongoServer = MongoServer.Create(connectionString);

            DropContentTypesCollection();
        }

        private void DropContentTypesCollection()
        {
            var database = mongoServer.GetDatabase(databaseName);
            var collection = database.GetCollection(collectionName);
            collection.Drop();
        }

        public void Save(params ContentType[] contentTypes)
        {
            var database = mongoServer.GetDatabase(databaseName);
            var collection = database.GetCollection(collectionName);

            collection.InsertBatch(contentTypes);
        }
    }
}