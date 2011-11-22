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

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class InvalidateCacheService : IInvalidateCacheService
    {
        private readonly Action<string> invalidateCache;
        private readonly Action closeService;

        public InvalidateCacheService(Action<string> invalidateCache, Action closeService)
        {
            this.invalidateCache = invalidateCache;
            this.closeService = closeService;
        }

        public void Invalidate(string cacheKey)
        {
            invalidateCache(cacheKey);
        }

        public void Stop()
        {
            closeService();
        }
    }
}