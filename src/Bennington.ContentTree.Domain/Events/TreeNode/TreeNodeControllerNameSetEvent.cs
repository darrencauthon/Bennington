using System;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Domain.Events.TreeNode
{
	public class TreeNodeControllerNameSetEvent : DomainEvent
	{
		public string ControllerName { get; set; }
	}
}
