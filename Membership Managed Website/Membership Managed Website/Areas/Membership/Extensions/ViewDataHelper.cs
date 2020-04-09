using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BAFactory.Fx.Security.Areas.Membership.Extensions
{
    public class ViewDataHelper : ViewDataDictionary
    {
        public static ViewDataDictionary AddData(ViewDataDictionary viewData, string key, object value)
        {
            viewData.Add(new KeyValuePair<string, object>(key, value));
            return viewData;
        }

        public ViewDataHelper(string key, object value)
            : base()
        {
            this.Add(new KeyValuePair<string, object>(key, value));
        }
    }
}