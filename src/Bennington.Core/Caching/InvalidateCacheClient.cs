using System;
using System.ServiceModel;

namespace Bennington.Core.Caching
{
    public class InvalidateCacheClient
    {
        public void Invalidate(Uri invalidateCacheUri)
        {
            Invalidate(invalidateCacheUri, string.Empty);
        }

        public void Invalidate(Uri invalidateCacheUri, string cacheKey)
        {
            var client = new ChannelFactory<IInvalidateCacheService>(new NetNamedPipeBinding(), new EndpointAddress(invalidateCacheUri)).CreateChannel();
            client.Invalidate(cacheKey);
        }
    }
}