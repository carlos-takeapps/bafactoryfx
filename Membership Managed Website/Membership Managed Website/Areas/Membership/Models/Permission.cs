using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Models
{
    public class AssignPermissionModel: Permission
    {
        public long IdModule
        {
            get { return base.Action.IdModule; }
            set { base.Action.IdModule = value; }
        }
    }
}