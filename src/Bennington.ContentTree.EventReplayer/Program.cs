using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bennington.ContentTree;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Denormalizers;
using Bennington.ContentTree.Domain;
using Bennington.ContentTree.Domain.Events.TreeNode;
using Bennington.ContentTree.Domain.SimpleCqrsRuntime;
using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Providers.ContentNodeProvider;
using Bennington.ContentTree.Providers.ContentNodeProvider.Denormalizers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Mappers;
using Bennington.ContentTree.Providers.ContentNodeProvider.Repositories;
using Bennington.ContentTree.Providers.SectionNodeProvider;
using Bennington.ContentTree.Providers.SectionNodeProvider.Denormalizers;
using Bennington.ContentTree.Providers.ToolLinkNodeProvider;
using Bennington.ContentTree.Repositories;
using Bennington.Core;
using Bennington.Core.Helpers;
using SimpleCqrs;
using SimpleCqrs.Eventing;
using SimpleCqrs.EventStore.SqlServer;
using SimpleCqrs.EventStore.SqlServer.Serializers;
using SimpleCqrs.Utilites;

namespace Bennington.ContentTreeEventReplayer
{
    class Program
    {
        static void Main(string[] args)
        {
            var runtime = new BenningtonContentTreeSimpleCqrsRuntime();
            runtime.Start();

            var y = typeof(ContentNodeProvider);
            var z = typeof(SectionNodeProvider);
            var a = typeof(ToolLinkNodeProvider);

            var domainEventReplayer = new DomainEventReplayer(runtime);

            var eventDenormalizerTypes = GetEventDenormalizerTypes();
            foreach (var eventType in eventDenormalizerTypes)
            {
                Console.Write(string.Format("Replaying events for {0} ...", eventType.Name));
                domainEventReplayer.ReplayEventsForHandlerType(eventType);
                Console.WriteLine(" complete.");
            }
        }

        public class Registerer : IRegisterComponents
        {
            public void Register(IServiceLocator serviceLocator)
            {
                serviceLocator.Register<IEventStore>(new SqlServerEventStore(
                        new SqlServerConfiguration(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ToString()),
                        new JsonDomainEventSerializer()));

                serviceLocator.Register<ITreeNodeRepository>(new TreeNodeRepository(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
                serviceLocator.Register<IContentNodeProviderDraftRepository>(new ContentNodeProviderDraftRepository(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
                serviceLocator.Register<IContentNodeProviderPublishedVersionRepository>(new ContentNodeProviderPublishedVersionRepository(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
                serviceLocator.Register<ContentTree.Providers.SectionNodeProvider.Data.IDataModelDataContext>(new Bennington.ContentTree.Providers.SectionNodeProvider.Data.DataModelDataContext(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
                serviceLocator.Register<IContentNodeProviderPublishedVersionRepository>(new ContentNodeProviderPublishedVersionRepository(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
                serviceLocator.Register<IContentNodeProviderDraftToContentNodeProviderPublishedVersionMapper>(new ContentNodeProviderDraftToContentNodeProviderPublishedVersionMapper());
                serviceLocator.Register<IContentNodeProviderDraftRepository>(new ContentNodeProviderDraftRepository(new DatabaseRetriever(new ConnectionStringRetriever()), new GetPathToDataDirectoryService(new ApplicationSettingsValueGetter(), new GetPathToWorkingDirectoryService(new ApplicationSettingsValueGetter()))));
            }
        }

        private static List<Type> GetEventDenormalizerTypes()
        {
            return new List<Type>()
                       {
                           typeof(TreeNodeDenormalizer), 
                           typeof (ContentNodeProviderDraftDenormalizer),
                           typeof(ContentNodeProviderPublishDenormalizer),
                           typeof(SectionNodeProviderDraftDenormalizer),
                           //typeof(ContentRoutingDenormalizer),
                           //typeof(SectionRoutingDenormalizer),
                       };
        }
    }
}
