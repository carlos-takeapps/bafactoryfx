using System.Web.Mvc;
using System.Web.Routing;
using BAFactory.Fx.Security.Areas.Membership.Extensions;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;

namespace BAFactory.Fx.Security
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "BAFactory.Fx.Security.Controllers" } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            InitializeApplicationObjects();
        }

        protected void Request_Begin()
        {

        }

        protected void Session_Start()
        {
            InitializeSessionObjects();
        }

        protected void Session_End()
        {
            MembershipSessionDataProvider.RequestSession.Clear();
            MembershipSessionDataProvider.RequestSession.Abandon();
            MembershipSessionDataProvider.RequestSession = null;
        }

        private void InitializeApplicationObjects()
        {
        }

        private void InitializeSessionObjects()
        {
            if (Session.IsNewSession && !string.IsNullOrEmpty(@User.Identity.Name))
            {
                MembershipSessionDataProvider.RequestSession = Session;

                MembershipSessionDataProvider.SessionUser = new MembershipManager().GetUser(@User.Identity.Name);
                MembershipSessionDataProvider.FilterInformation = new FilterInformation();
            }
        }
    }
}