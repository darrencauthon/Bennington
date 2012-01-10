using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Web;
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

            var invalidateCacheUri = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", "Bennington.ContentTree.CacheKey"));
            cacheEndpoint = new InvalidateCacheEndpoint(invalidateCacheUri);
            cacheEndpoint.CacheInvalidated += InvalidateCache;
            cacheEndpoint.Open();
        }

        private void InvalidateCache(object sender, CacheInvalidatedEventArgs e)
        {
            var cache = MemoryCache.Default;
            cache.Set(ContentTreeCacheImplementation.ContentTreeOutputCacheKey, Guid.NewGuid().ToString(), new DateTimeOffset(DateTime.Now.AddHours(8)));
        }
    }
}
