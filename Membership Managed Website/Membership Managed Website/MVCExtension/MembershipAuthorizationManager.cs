using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.MVCExtension
{
    internal static class MembershipAuthorizationManager
    {
        static MembershipManager membershipManager;

        static MembershipAuthorizationManager()
        {
            membershipManager = new MembershipManager();
        }

        static public bool AuthorizeViewAccess(long userId, long organizationId, string area, string module, string action)
        {
            return membershipManager.AuthorizeViewAccess(userId, organizationId, area == null ? string.Empty : area, module, action);
        }
    }
}