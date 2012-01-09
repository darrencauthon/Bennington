using System;
using System.Configuration;
using System.Net;
using Bennington.Core.Caching;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Caching.Blades
{
    public class ContentTreeCacheInvalidationBlade : Blade
    {
        public const int ContentTreeOutputCacheDuration = 28800; // 8 hours
        public const string ContentTreeApplicationSessionKey = "Bennington.ContentTree.Cache";

        public override void Spin(IRotorContext context)
        {
            if (string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["Bennington.Content.Routing.Cache.Listener.CacheKey"])) return;

            var invalidationUri = new Uri(string.Format("net.pipe://localhost/caching/{0}/content_tree", ConfigurationManager.AppSettings["Bennington.Content.Routing.Cache.Listener.CacheKey"]));
            var invalidateCacheEndpoint = new InvalidateCacheEndpoint(invalidationUri, InvalidateCache);
            invalidateCacheEndpoint.Open();
        }

        private static void InvalidateCache(string cacheKey)
        {
            using (var client = new WebClient())
            {
                var url = string.Format("{0}InvalidateCache/{1}/{2}",
                                        ConfigurationManager.AppSettings["Bennington.ContentTree.Cahce.InvalidationUrl"],
                                        ConfigurationManager.AppSettings["Bennington.ContentTree.Cahce.InvalidationUrlGuid"],
                                        cacheKey);

                try
                {
                    client.DownloadString(url);
                }
                catch(Exception)
                {
                }
            }
        }
    }
}
