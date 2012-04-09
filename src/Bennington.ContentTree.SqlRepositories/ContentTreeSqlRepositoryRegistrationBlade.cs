using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bennington.ContentTree.Data;
using MvcTurbine;
using MvcTurbine.Blades;

namespace Bennington.ContentTree.SqlRepositories
{
    public class ContentTreeSqlRepositoryRegistrationBlade : IBlade
    {
        public void Dispose()
        {
        }

        public void Initialize(IRotorContext context)
        {
        }

        public void Spin(IRotorContext context)
        {
            //context.ServiceLocator.Register<ITreeNodeDataContext, TreeNodeRepositoryBackingStore>();
        }
    }
}
