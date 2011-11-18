using System;
using System.Linq;
using Bennington.ContentTree.Data;
using Bennington.ContentTree.Domain.Events.TreeNode;
using Bennington.ContentTree.Repositories;
using SimpleCqrs.Eventing;

namespace Bennington.ContentTree.Denormalizers
{
	public class TreeNodeDenormalizer : IHandleDomainEvents<TreeNodeDeletedEvent>,
										IHandleDomainEvents<TreeNodeCreatedEvent>,
										IHandleDomainEvents<TreeNodeTypeSetEvent>,
                                        IHandleDomainEvents<TreeNodeControllerNameSetEvent>,
										IHandleDomainEvents<TreeNodeParentTreeNodeIdSetEvent>
	{
		private readonly ITreeNodeRepository treeNodeRepository;

		public TreeNodeDenormalizer(ITreeNodeRepository treeNodeRepository)
		{
			this.treeNodeRepository = treeNodeRepository;
		}

		private TreeNode GetTreeNodeFromDomainEvent(DomainEvent domainEvent)
		{
			return treeNodeRepository.GetAll().Where(a => a.TreeNodeId == domainEvent.AggregateRootId.ToString()).FirstOrDefault();
		}

		public void Handle(TreeNodeDeletedEvent treeNodeDeletedEvent)
		{
			treeNodeRepository.Delete(treeNodeDeletedEvent.TreeNodeId.ToString());
		}

		public void Handle(TreeNodeCreatedEvent domainEvent)
		{
			treeNodeRepository.Create(new TreeNode()
										{
											TreeNodeId = domainEvent.AggregateRootId.ToString(),
										});
		}

		public void Handle(TreeNodeTypeSetEvent domainEvent)
		{
			var treeNode = GetTreeNodeFromDomainEvent(domainEvent);
			treeNode.Type = domainEvent.Type.AssemblyQualifiedName;
			treeNodeRepository.Update(treeNode);
		}

		public void Handle(TreeNodeParentTreeNodeIdSetEvent domainEvent)
		{
			var treeNode = GetTreeNodeFromDomainEvent(domainEvent);
			treeNode.ParentTreeNodeId = domainEvent.ParentTreeNodeId.ToString();
			treeNodeRepository.Update(treeNode);
		}

	    public void Handle(TreeNodeControllerNameSetEvent domainEvent)
	    {
            var treeNode = GetTreeNodeFromDomainEvent(domainEvent);
	        treeNode.ControllerName = domainEvent.ControllerName;
            treeNodeRepository.Update(treeNode);	        
	    }
	}
}