using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting
{
    internal class SortModuleByPathDevice : ISortingDevice<Module>
    {
        public void ApplySorting(ref List<Module> elements)
        {
            elements.Sort(new Comparison<Module>(SortModuleByPath));
        }

        private static int SortModuleByPath(Module a, Module b)
        {
            string aPath = string.Concat((a.Area.Name == string.Empty ? "_" : a.Area.Name), a.Name);
            string bPath = string.Concat((b.Area.Name == string.Empty ? "_" : b.Area.Name), b.Name);

            return string.Compare(aPath, bPath);
        }


        public void ApplySorting(ref List<Module> elements, string member)
        {
            throw new NotImplementedException();
        }
    }

}