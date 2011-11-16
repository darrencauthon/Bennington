using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.AggregateRoots
{
    public class PageControllerNameSetEvent : DomainEvent
    {
        public string ControllerName { get; set; }
    }
}