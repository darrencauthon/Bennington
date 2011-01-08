using System;
using Paragon.ContentTree.Domain.Events;
using SimpleCqrs.Eventing;

namespace Paragon.ContentTree.Domain.AggregateRoots
{
	public class Section : SimpleCqrs.Domain.AggregateRoot
	{
		public Section()
		{
		}

		public Section(Guid sectionId)
		{
			Apply(new SectionCreatedEvent(){ AggregateRootId = sectionId });
		}

		public void OnSectionCreatedEvent(SectionCreatedEvent sectionCreatedEvent)
		{
			Id = sectionCreatedEvent.AggregateRootId;
		}

		public void SetName(string name)
		{
			Apply(new NameSetEvent() { });
		}

		public void SetDefaultPage(Guid pageId)
		{
			Apply(new DefaultPageSetEvent(){ PageId = pageId });
		}

		public void SetUrlSegment(string urlSegment)
		{
			Apply(new UrlSegmentSetEvent() { UrlSegment = urlSegment });
		}

		public void SetIsActive(bool isActive)
		{
			Apply(new IsActiveSetEvent() { IsActive = isActive });
		}

		public void SetIsVisible(bool isVisible)
		{
			Apply(new IsVisibleSetEvent() { IsVisible = isVisible });
		}

		public void SetParentTreeNodeId(Guid parentTreeNodeId)
		{
			Apply(new ParentTreeNodeIdSetEvent() { ParentTreeNodeId = parentTreeNodeId });
		}

		public void SetSequence(int? sequence)
		{
			Apply(new SequenceSetEvent() { Sequence = sequence });
		}
	}
}