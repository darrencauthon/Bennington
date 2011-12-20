using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using MvcTurbine.ComponentModel;
using MvcTurbine.MembershipProvider;

namespace Bennington.Cms.PrincipalProvider
{
    public class UnauthenticatedPrincipalCreator : IUnauthenticatedPrincipalCreator, IServiceRegistration
    {
        public IPrincipal Create()
        {
            return new GenericPrincipal(new GenericIdentity("Unauthenticated Anonymous"), new string[] {});
        }

        public void Register(IServiceLocator locator)
        {
            locator.Register<IUnauthenticatedPrincipalCreator, UnauthenticatedPrincipalCreator>();
        }
    }
}