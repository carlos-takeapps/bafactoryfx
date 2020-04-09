using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAFactory.Fx.Security.MVCExtension
{
    internal class MembershipApplicationDataKeys
    {
        public static string ServerBaseUrl
        {
            get { return "SERVER_BASE_URL"; }
            private set { return; }
        }
    }
}