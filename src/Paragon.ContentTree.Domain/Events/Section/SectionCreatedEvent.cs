﻿using System;
using SimpleCqrs.Eventing;

namespace Paragon.ContentTree.Domain.Events.Section
{
	public class SectionCreatedEvent : DomainEvent
	{
		public Guid SectionId { get; set; }
	}
}
