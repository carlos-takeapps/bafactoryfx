using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MVCExtension;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions
{
    public class FilterInformation
    {
        public class UserFilter : MembershipBaseFilter
        {
            public string FullName { get; set; }
        }

        public class AreaFilter : MembershipBaseFilter { }

        public class ModuleFilter : MembershipBaseFilter { }

        public class ActionFilter : MembershipBaseFilter { }

        public string Sort { get; set; }

        public int PageNumber { get; set; }

        public AreaFilter Area { get; set; }

        public ModuleFilter Module { get; set; }

        public ActionFilter Action { get; set; }

        public UserFilter User { get; set; }

        public FilterInformation()
        {
            Sort = string.Empty;
            Area = new AreaFilter();
            Module = new ModuleFilter();
            Action = new ActionFilter();
            User = new UserFilter();
        }
    }
}