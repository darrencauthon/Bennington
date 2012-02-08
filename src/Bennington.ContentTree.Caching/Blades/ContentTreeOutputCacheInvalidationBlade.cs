using System;
using System.Configuration;
using System.Runtime.Caching;
using System.ServiceModel;
using Bennington.Core.Caching;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Caching.Blades
{
    public class ContentTreeOutputCacheInvalidationBlade : Blade
    {
        private InvalidateCacheEndpoint cacheEndpoint;

        public override void Spin(IRotorContext context)
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Bennington.ContentTree.EnableOutputCacheInvalidation"]))
                return;

            var invalidateCacheUri = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ConfigurationManager.AppSettings["Bennington.ContentTree.OutputCaching.CacheKey"] ?? "Bennington.ContentTree.CacheKey"));
            cacheEndpoint = new InvalidateCacheEndpoint(invalidateCacheUri);
            cacheEndpoint.CacheInvalidated += InvalidateCache;

            try
            {
                cacheEndpoint.Open();
            }
            catch (AddressAlreadyInUseException ex)
            {
                cacheEndpoint.Dispose();
                cacheEndpoint.Open();
            }
        }

        private void InvalidateCache(object sender, CacheInvalidatedEventArgs e)
        {
            var cache = MemoryCache.Default;
            cache.Set(ContentTreeCacheImplementation.ContentTreeOutputCacheKey, Guid.NewGuid().ToString(), new DateTimeOffset(DateTime.Now.AddHours(8)));
        }
    }
}
