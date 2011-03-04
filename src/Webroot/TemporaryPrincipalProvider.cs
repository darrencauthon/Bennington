﻿using System.Security.Principal;
using MvcTurbine.MembershipProvider;

namespace Webroot.Principals
{
    public class TemporaryPrincipalProvider : IPrincipalProvider
    {
        public PrincipalProviderResult GetPrincipal(string userId, string password)
        {
            return new PrincipalProviderResult
                       {
                           Principal = new GenericPrincipal(new GenericIdentity(userId), new string[] {})
                       };
        }

        public IPrincipal CreatePrincipalFromTicketData(string userName, string userData)
        {
            return new GenericPrincipal(new GenericIdentity(userName), new string[] {});
        }

        public TicketData ConvertPrincipalToTicketData(IPrincipal principal)
        {
            return new TicketData
                       {
                           IsPersistent = true,
                           NumberOfMinutesUntilExpiration = 15,
                           Username = "admin"
                       };
        }
    }
}