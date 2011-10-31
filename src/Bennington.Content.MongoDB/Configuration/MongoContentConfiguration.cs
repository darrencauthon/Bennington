using System.Configuration;
using Bennington.Content.Configuration;
using Bennington.Content.MongoDB.Configuration;

namespace Bennington.Core.Configuration
{
    public static class MongoContentConfiguration
    {
        public static MongoContentConfigurer UseMongoDb(this ContentConfigurer configurer, string name)
        {
            return new MongoContentConfigurer(configurer, ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}