using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events.Page
{
	public class PageMetaKeywordSetEvent : DomainEvent
	{
		public string MetaKeyword { get; set; }
	}
}
