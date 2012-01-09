using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.Caching.Blades
{
    public class ContentTreeCacheImplementationRegistrationBlade : Blade
    {
        public override void Spin(IRotorContext context)
        {
            context.ServiceLocator.Register<IContentTree, Bennington.ContentTree.Caching.ContentTreeCacheImplementation>();
        }
    }
}
