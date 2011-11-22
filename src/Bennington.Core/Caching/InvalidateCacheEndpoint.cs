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
            try
            {
                OpenServiceHost();
            }
            catch(InvalidOperationException)
            {
                var client = new ChannelFactory<IInvalidateCacheService>(new NetNamedPipeBinding(), new EndpointAddress(invalidateCacheUri)).CreateChannel();
                client.Stop();

                var expirationTime = DateTime.Now.AddSeconds(5);
                var connected = false;

                while(DateTime.Now < expirationTime && connected == false)
                {
                    try
                    {
                        OpenServiceHost();
                        connected = true;
                    }
                    catch
                    {
                        connected = false;
                    }
                }
            }
        }

        public void Dispose()
        {
            if(cacheServiceHost == null) return;

            if(cacheServiceHost.State == CommunicationState.Opened || cacheServiceHost.State == CommunicationState.Opening)
                cacheServiceHost.Close();
        }

        private void OpenServiceHost()
        {
            cacheServiceHost = new ServiceHost(new InvalidateCacheService(invalidateCache, () => cacheServiceHost.Close()), invalidateCacheUri);
            cacheServiceHost.AddServiceEndpoint(typeof(IInvalidateCacheService), new NetNamedPipeBinding(), invalidateCacheUri);
            cacheServiceHost.Open();
        }
    }
}