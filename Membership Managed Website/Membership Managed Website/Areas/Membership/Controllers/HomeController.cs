using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAFactory.Fx.Security.MVCExtension;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class HomeController : Controller
    {
        //
        // GET: /Membership/Home/
        [MembershipCustomAuthorizeAttribute]
        public ActionResult Index()
        {
            return View();
        }

    }
}
