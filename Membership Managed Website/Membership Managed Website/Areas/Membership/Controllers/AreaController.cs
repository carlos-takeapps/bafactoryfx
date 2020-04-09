using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;
using BAFactory.Fx.Security.Areas.Membership.Extensions;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class AreaController : BaseController
    {
        public AreaController()
            : base()
        {
        }

        //
        // GET: /Membership/Area/
        public ActionResult Index()
        {
            List<Area> areas = manager.GetAreasList();

            ApplySorting(ref areas);

            return View(areas);
        }

        //
        // GET: /Membership/Area/Details/5
        public ActionResult Details(int id)
        {
            Area area = manager.GetArea(id);
            return View(area);
        }

        //
        // GET: /Membership/Area/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Membership/Area/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool success = false;
            try
            {
                Area area = EntityPropertiesLoader.PopulateProperties<Area>(collection);
                success = manager.CreateArea(area);
            }
            catch
            {
            }

            if (success)
                return RedirectToAction("Index");
            else
                return View();
        }

        //
        // GET: /Membership/Area/Edit/5
        public ActionResult Edit(int id)
        {
            Area area = manager.GetArea(id);
            return View(area);
        }

        //
        // POST: /Membership/Area/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                Area area = manager.GetArea(id);
                EntityPropertiesLoader.UpdateProperties<Area>(ref area, collection);
                success = manager.UpdateArea(area);
            }
            catch
            {
            }

            if (success)
                return RedirectToAction("Index");
            else
                return Edit(id);
        }

        //
        // GET: /Membership/Area/Delete/5
        public ActionResult Delete(int id)
        {
            Area area = manager.GetArea(id);
            return View(area);
        }

        //
        // POST: /Membership/Area/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                success = manager.DeleteArea(id);
            }
            catch
            {
            }

            if (success)
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpGet]
        public JsonResult ListAreas()
        {
            List<string> result = manager.GetAreasList().ConvertAll(x => x.Name);
            return ReturnAllowedJsonGet(result);
        }

        [HttpGet]
        public JsonResult Suggest(string term)
        {
            List<Area> areas = manager.GetAreasList();
            List<Area> filtrados = areas.Where(x => x.Name.Contains(term)).ToList<Area>();

            List<string> result = filtrados.ConvertAll<string>(x => x.Name);

            return ReturnAllowedJsonGet(result);
        }

        protected override void LoadViewData()
        {
            return;
        }

        protected override void SetViewBagData()
        {
            return;
        }

        protected override void LoadControllerData()
        {
            return;
        }
    }
}
