using System.Web;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.MVCExtension
{
    public class MembershipCustomAuthorizeAttribute : AuthorizeAttribute
    {
        private bool skipAuthorization;

        public MembershipCustomAuthorizeAttribute() : this(true) { }

        public MembershipCustomAuthorizeAttribute(bool authorize)
        {
            skipAuthorization = !authorize;
        }

        /// <summary>
        /// Implements BAF Custom Security Authorization
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (skipAuthorization)
                return true;

            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            MembershipSessionIdentifier sessionId = httpContext.Session[MembershipSessionDataKeys.SessionIdentifier] as MembershipSessionIdentifier;
            if (sessionId == null)
                return false;

            long userId = sessionId.UserId;
            long organizationId = sessionId.OrganizationId;
            
            string moduleName = ((System.Web.Mvc.MvcHandler)(httpContext.Handler)).RequestContext.RouteData.Values["controller"] as string;
            string actionName = ((System.Web.Mvc.MvcHandler)(httpContext.Handler)).RequestContext.RouteData.Values["action"] as string;
            string areaName = ((System.Web.Routing.Route)(((System.Web.Mvc.MvcHandler)(httpContext.Handler)).RequestContext.RouteData.Route)).DataTokens["area"] as string;

            if (MembershipAuthorizationManager.AuthorizeViewAccess(userId, organizationId, areaName, moduleName, actionName))
            {
                return true;
            }

            return false;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);

            filterContext.Result = new RedirectToRouteResult(new MembershipNotAuthorizedRouteInfo());
        }
    }
}
