using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAFactory.Fx.Security.MVCExtension
{
    internal class MembershipSessionDataKeys
    {
        public static string Filter
        {
            get { return "FILTER"; }
            private set { return; }
        }

        public static string User
        {
            get { return "USER"; }
            private set { return; }
        }

        public static string SessionIdentifier
        {
            get { return "SESSION_IDENTIFIER"; }
            private set { return; }
        }

        public static string Organization
        {
            get { return "ORGANIZATION"; }
            set { return; }
        }

        public static string UserOrganization
        {
            get { return "USER_ORGANIZATION"; }
            set { return; }
        }
    }
}