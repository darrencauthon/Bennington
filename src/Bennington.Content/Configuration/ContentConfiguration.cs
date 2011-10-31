using Bennington.Content.Configuration;

namespace Bennington.Core.Configuration
{
    public static class ContentConfiguration
    {
        public static ContentConfigurer Content(this Configurer configurer)
        {
            return new ContentConfigurer(configurer);
        }
    }
}