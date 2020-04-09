using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting
{
    static class SortingFactory
    {
        internal static ISortingDevice<Tsorted> GetSortingDevice<Tsorted, Tsorter>()
        {
            ISortingDevice<Tsorted> sortingDevice = null;

            sortingDevice = (ISortingDevice<Tsorted>)(Activator.CreateInstance(typeof(Tsorter)));

            if (sortingDevice == null)
            {
                sortingDevice = (ISortingDevice<Tsorted>)(new DefaultSortingDevice<Tsorted>());
            }

            return sortingDevice;
        }

    }
}