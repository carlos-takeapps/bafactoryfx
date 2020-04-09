using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mp = BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting
{
    internal class SortActionByPathDevice : ISortingDevice<mp.Action>
    {
        public void ApplySorting(ref List<mp.Action> elements)
        {
            elements.Sort(new Comparison<mp.Action>(SortActionByPath));
        }

        private static int SortActionByPath(mp.Action a, mp.Action b)
        {
            string aPath = string.Concat((a.Module.Area.Name == string.Empty ? "_" : a.Module.Area.Name), a.Module.Name, a.Name);
            string bPath = string.Concat((b.Module.Area.Name == string.Empty ? "_" : b.Module.Area.Name), b.Module.Name, b.Name);

            return string.Compare(aPath, bPath);
        }


        public void ApplySorting(ref List<mp.Action> elements, string member)
        {
            throw new NotImplementedException();
        }
    }
}
