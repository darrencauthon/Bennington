using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bennington.ContentTree.Data;
using MvcTurbine.ComponentModel;

namespace Bennington.ContentTree.Registration
{
    public class TreeNodeDataContextRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            locator.Register<ITreeNodeDataContext, TreeNodeDataContext>();
        }
    }
}