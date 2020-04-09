using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAFactory.Fx.Security.Areas.Membership.Extensions;
using BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class ModuleController : BaseController
    {
        public ModuleController()
            : base()
        {
        }

        //
        // GET: /Membership/Modulo/
        public ActionResult Index()
        {
            List<Module> modules = manager.GetModulesList();

            ApplySorting<Module, SortModuleByPathDevice>(ref modules);

            return View(modules);
        }

        //
        // GET: /Membership/Modulo/Details/5
        public ActionResult Details(int id)
        {
            Module modules = manager.GetModule(id);
            return View(modules);
        }

        //
        // GET: /Membership/Modulo/Create
        public ActionResult Create()
        {
            LoadViewData();

            return View();
        }

        //
        // POST: /Membership/Modulo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            LoadViewData();

            bool success = false;
            try
            {
                Module m = EntityPropertiesLoader.PopulateProperties<Module>(collection);
                success = manager.CreateModule(m);
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
        // GET: /Membership/Modulo/Edit/5
        public ActionResult Edit(int id)
        {
            Module module = manager.GetModule(id);
            return View(module);
        }
        //
        // POST: /Membership/Modulo/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                Module module = manager.GetModule(id);
                EntityPropertiesLoader.UpdateProperties(ref module, collection);
                success = manager.UpdateModule(module);
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
        // GET: /Membership/Modulo/Delete/5
        public ActionResult Delete(int id)
        {
            Module module = manager.GetModule(id);
            return View(module);
        }

        //
        // POST: /Membership/Modulo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                success = manager.DeleteModule(id);
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
        public JsonResult ListByArea(int id)
        {
            Area area = manager.GetArea(id);
            List<Module> modules = area.Modules.ToList<Module>();

            List<Tuple<long, string>> result = modules.ConvertAll<Tuple<long, string>>(x => new Tuple<long, string>(x.Id, x.Name));

            return ReturnAllowedJsonGet(result);
        }

        protected override void LoadViewData()
        {
            SelectList areas = new SelectList(manager.GetAreasList(), "id", "name");
            ViewData[ViewDataKeys.SystemAreas] = areas;
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
