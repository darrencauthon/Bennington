using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events.Page
{
	public class PageMetaDescriptionSetEvent : DomainEvent
	{
		public string MetaDescription { get; set; }
	}
}
