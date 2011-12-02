using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events.Page
{
	public class PageMetaTitleSetEvent : DomainEvent
	{
		public string MetaTitle { get; set; }
	}
}
