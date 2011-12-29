using System;
using System.Configuration;
using MvcTurbine.ComponentModel;
using SimpleCqrs;
using SimpleCqrs.Commanding;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.Eventing;
using IServiceLocator = MvcTurbine.ComponentModel.IServiceLocator;

namespace Bennington.ContentTree.Domain.Registration
{
	public class SimpleCqrsRuntimeBootstrapper : IServiceRegistration
	{
	    private readonly IServiceLocator serviceLocator;

        public SimpleCqrsRuntimeBootstrapper(IServiceLocator serviceLocator)
	    {
	        this.serviceLocator = serviceLocator;
	    }

	    public void Register(IServiceLocator locator)
	    {
            var simpleCqrsRuntimes = serviceLocator.ResolveServices<ISimpleCqrsRuntime>();
            if (simpleCqrsRuntimes.Count > 0) return;

            var benningtonContentTreeSimpleCqrsRuntime = new BenningtonContentTreeSimpleCqrsRuntime();

            benningtonContentTreeSimpleCqrsRuntime.Start();

            var connectionStringSettings = ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"];
            if (connectionStringSettings != null)
            {
                benningtonContentTreeSimpleCqrsRuntime.ServiceLocator.Register<IEventStore>(
                        new SqlServerEventStore(
                            new SqlServerConfiguration(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ConnectionString),
                            new SimpleCqrs.EventStore.SqlServer.Serializers.JsonDomainEventSerializer()));
            }
            else
            {
                throw new Exception("Cannot find connection string for 'Bennington.ContentTree.Domain.ConnectionString' in the event store");
            }

            var commandBus = benningtonContentTreeSimpleCqrsRuntime.ServiceLocator.Resolve<ICommandBus>();
            serviceLocator.Register(commandBus);

            serviceLocator.Register<SimpleCqrs.IServiceLocator>(benningtonContentTreeSimpleCqrsRuntime.ServiceLocator);
	    }
	}
}
