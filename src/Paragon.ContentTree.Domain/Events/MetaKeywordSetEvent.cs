﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleCqrs.Eventing;

namespace Paragon.ContentTree.Domain.Events
{
	public class MetaKeywordSetEvent : DomainEvent
	{
		public string MetaKeyword { get; set; }
	}
}
