using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BAFactory.Fx.Security.MVCExtension
{
    public class MembershipNotAuthorizedRouteInfo : RouteValueDictionary
    {
        public MembershipNotAuthorizedRouteInfo()
            : base()
        {
            this["controller"] = "Home";
            this["action"] = "NotAuthorized";
            this["area"] = string.Empty;
        }
    }
}