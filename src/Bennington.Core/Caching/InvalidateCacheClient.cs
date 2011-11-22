using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Bennington.Core.Caching
{
    public static class InvalidateCacheClient
    {
        public static void Invalidate(Uri invalidateCacheUri)
        {
            Invalidate(invalidateCacheUri, string.Empty);
        }

        public static void Invalidate(Uri invalidateCacheUri, string cacheKey)
        {
            try
            {
                var client = new ChannelFactory<IInvalidateCacheService>(new NetNamedPipeBinding(), new EndpointAddress(invalidateCacheUri)).CreateChannel();

                client.Invalidate(cacheKey);

                ((IChannel)client).Close();

            }catch(Exception){}
        }
    }
}