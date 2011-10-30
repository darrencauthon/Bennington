using System;
using System.ServiceModel;

namespace Bennington.Core.Caching
{
    [ServiceContract]
    public interface IInvalidateCacheService
    {
        [OperationContract]
        void Invalidate(string cacheKey);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InvalidateCacheService : IInvalidateCacheService
    {
        private readonly Action<string> invalidateCache;

        public InvalidateCacheService(Action<string> invalidateCache)
        {
            this.invalidateCache = invalidateCache;
        }

        public void Invalidate(string cacheKey)
        {
            invalidateCache(cacheKey);
        }
    }
}