using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting
{
    internal interface ISortingDevice<T>
    {
        void ApplySorting(ref List<T> elements);

        void ApplySorting(ref List<T> elements, string member);
    }
}