﻿using Bennington.ContentTree.Helpers;
using Bennington.ContentTree.Repositories;
using Bennington.Core.Helpers;
using MvcTurbine;
using MvcTurbine.Blades;
using SimpleCqrs;

namespace Bennington.ContentTree.Blades
{
	public class RegisterAssemblyWithSimpleCqrsServiceLocatorBlade : IBlade
	{
		private readonly IServiceLocator simpleCqrsServiceLocator;

		public RegisterAssemblyWithSimpleCqrsServiceLocatorBlade(SimpleCqrs.IServiceLocator simpleCqrsServiceLocator)
		{
			this.simpleCqrsServiceLocator = simpleCqrsServiceLocator;
		}

		public void Dispose()
		{
		}

		public void Initialize(IRotorContext context)
		{
		}

		public void Spin(IRotorContext context)
		{
			simpleCqrsServiceLocator.Register(context.ServiceLocator.Resolve<ITreeNodeRepository>());
            simpleCqrsServiceLocator.Register(context.ServiceLocator.Resolve<IConnectionStringRetriever>());
			simpleCqrsServiceLocator.Register(context.ServiceLocator.Resolve<IGetPathToDataDirectoryService>());
		}
	}
}