using System;
using System.ServiceModel;

namespace Bennington.Core.Caching
{
    public class InvalidateCacheEndpoint : IDisposable
    {
        private readonly Uri invalidateCacheUri;
        private readonly Action<string> invalidateCache;
        private ServiceHost cacheServiceHost;

        public InvalidateCacheEndpoint(Uri invalidateCacheUri, Action<string> invalidateCache)
        {
            this.invalidateCacheUri = invalidateCacheUri;
            this.invalidateCache = invalidateCache;
        }

        public void Open()
        {
            cacheServiceHost = new ServiceHost(new InvalidateCacheService(invalidateCache), invalidateCacheUri);
            cacheServiceHost.AddServiceEndpoint(typeof(IInvalidateCacheService), new NetNamedPipeBinding(), invalidateCacheUri);
            cacheServiceHost.Open();
        }


        public void Dispose()
        {
            if(cacheServiceHost == null) return;

            if(cacheServiceHost.State == CommunicationState.Opened || cacheServiceHost.State == CommunicationState.Opening)
                cacheServiceHost.Close();
        }
    }
}