using System.Web.Mvc;
using BAFactory.Fx.Security.MVCExtension;

namespace BAFactory.Fx.Security.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Bienvenid@ a su website.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
    }
}
