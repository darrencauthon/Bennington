using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Bennington.Content.Sql.Data;
using Bennington.ContentTree.Domain.Events.Page;
using SimpleCqrs.Eventing;

namespace Bennington.Content.Sql.Denormalizers
{
    public class ContentRoutingDenormalizer : IHandleDomainEvents<PagePublishedEvent>,
                                                            IHandleDomainEvents<PageDeletedEvent>
    {
        public ContentRoutingDenormalizer()
        {
        }

        public void Handle(PagePublishedEvent domainEvent)
        {
            using (var dataContext = new ContentDataContext(ConfigurationManager.ConnectionStrings["Bennington.ContentTree.Domain.ConnectionString"].ToString()))
            {

            }
        }

        public void Handle(PageDeletedEvent domainEvent)
        {
        }
    }
}
