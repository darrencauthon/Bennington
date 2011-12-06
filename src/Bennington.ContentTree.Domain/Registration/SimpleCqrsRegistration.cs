using System;
using System.Configuration;
using MvcTurbine.ComponentModel;
using SimpleCqrs.Commanding;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Registration
{
	public class SimpleCqrsRegistration : IServiceRegistration
	{
	    private readonly BenningtonContentTreeSimpleCqrsRuntime benningtonContentTreeSimpleCqrsRuntime;

	    public SimpleCqrsRegistration(BenningtonContentTreeSimpleCqrsRuntime benningtonContentTreeSimpleCqrsRuntime)
	    {
	        this.benningtonContentTreeSimpleCqrsRuntime = benningtonContentTreeSimpleCqrsRuntime;
	    }

	    public void Register(IServiceLocator locator)
		{
			benningtonContentTreeSimpleCqrsRuntime.Start();

	        var connectionStringSettings = ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"];
            if (connectionStringSettings != null)
            {
                benningtonContentTreeSimpleCqrsRuntime.ServiceLocator.Register<IEventStore>(
                        new SqlServerEventStore(
                            new SqlServerConfiguration(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString),
                            new SimpleCqrs.EventStore.SqlServer.Serializers.JsonDomainEventSerializer()));                
            } else
            {
                throw new Exception("Cannot find connection string for 'Bennington.ContentTree.Domain.ConnectionString' in the event store");
            }

            var commandBus = benningtonContentTreeSimpleCqrsRuntime.ServiceLocator.Resolve<ICommandBus>();
			locator.Register(commandBus);

			locator.Register<SimpleCqrs.IServiceLocator>(benningtonContentTreeSimpleCqrsRuntime.ServiceLocator);
		}
	}
}
