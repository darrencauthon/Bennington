using System;
using System.ServiceModel;

namespace Bennington.Core.Caching
{
    [ServiceContract]
    public interface IInvalidateCacheService
    {
        [OperationContract(IsOneWay = true)]
        void Invalidate(string cacheKey);

        [OperationContract(IsOneWay = true)]
        void Stop();
    }

    public class CacheInvalidatedEventArgs : EventArgs
    {
        public string CacheKey { get; private set; }

        public CacheInvalidatedEventArgs(string cacheKey)
        {
            this.CacheKey = cacheKey;
        }
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InvalidateCacheService : IInvalidateCacheService
    {
        private readonly EventHandler<CacheInvalidatedEventArgs> invalidateCache;
        private readonly Action closeService;

        public InvalidateCacheService(EventHandler<CacheInvalidatedEventArgs> invalidateCache, Action closeService)
        {
            this.invalidateCache = invalidateCache;
            this.closeService = closeService;
        }

        public void Invalidate(string cacheKey)
        {
            invalidateCache(this, new CacheInvalidatedEventArgs(cacheKey));
        }

        public void Stop()
        {
            closeService();
        }
    }
}