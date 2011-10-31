using System.Configuration;
using Bennington.Content.Configuration;
using Bennington.Content.Sql.Configuration;

namespace Bennington.Core.Configuration
{
    public static class SqlContentConfiguration
    {
        public static SqlContentConfigurer UseSql(this ContentConfigurer configurer, string name)
        {
            return new SqlContentConfigurer(configurer, ConfigurationManager.ConnectionStrings[name].ConnectionString);
        }
    }
}