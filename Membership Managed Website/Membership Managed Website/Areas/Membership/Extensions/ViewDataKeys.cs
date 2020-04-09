using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions
{
    public static class ViewDataKeys
    {
        public static string SystemModules
        {
            get { return "SYSTEM_MODULES"; }
            private set { return; }
        }

        public static string SystemAreas
        {
            get { return "SYSTEM_AREAS"; }
            private set { return; }
        }

        public static string SystemActions
        {
            get { return "SYSTEM_ACTIONS"; }
            private set { return; }
        }

        public static string SystemUsers
        {
            get { return "SYSTEM_Users"; }
            private set { return; }
        }

        public static string FilterInformation
        {
            get { return "FILTER_INFORMATION"; }
            private set { return; }
        }

        public static string ShowCreateLink
        {
            get { return "SHOW_CREATE_LINK"; }
            set {return ;}
        }
    }
}