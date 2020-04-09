using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAFactory.Fx.Security.Areas.Membership.Extensions;
using BAFactory.Fx.Security.Areas.Membership.Extensions.Sorting;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;
using mp = BAFactory.Fx.Security.MembershipProvider;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class ActionController : BaseController
    {
        public ActionController()
            : base()
        {
        }

        public ActionResult Index()
        {
            List<mp.Action> acciones = manager.GetActionsList();

            ApplySorting<mp.Action, SortActionByPathDevice>(ref acciones);

            return View(acciones);
        }

        //
        // GET: /Membership/Accion/Details/5
        public ActionResult Details(int id)
        {
            mp.Action action = manager.GetAction(id);
            return View(action);
        }

        //
        // GET: /Membership/Accion/Create
        public ActionResult Create()
        {
            LoadViewData();

            return View();
        }

        //
        // POST: /Membership/Accion/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool success = false;
            try
            {
                mp.Action action = EntityPropertiesLoader.PopulateProperties<mp.Action>(collection);
                success = manager.CreateAction(action);
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
        // GET: /Membership/Accion/Edit/5
        public ActionResult Edit(int id)
        {
            LoadViewData();

            mp.Action action = manager.GetAction(id);
            return View(action);
        }

        //
        // POST: /Membership/Accion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                mp.Action action = manager.GetAction(id);
                EntityPropertiesLoader.UpdateProperties<mp.Action>(ref action, collection);
                success = manager.UpdateAction(action);
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
        // GET: /Membership/Accion/Delete/5
        public ActionResult Delete(int id)
        {
            mp.Action action = manager.GetAction(id);
            return View(action);
        }

        //
        // POST: /Membership/Accion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool succeded = false;
            try
            {
                succeded = manager.DeleteAction(id);
            }
            catch
            {
            }

            if (succeded)
                return RedirectToAction("Index");
            else
                return View();
        }

        [HttpGet]
        public JsonResult ListByModule(int id)
        {
            Module module = manager.GetModule(id);
            List<mp.Action> actions = module.Actions.ToList<mp.Action>();

            List<Tuple<long, string>> result = actions.ConvertAll<Tuple<long, string>>(x => new Tuple<long, string>(x.Id, x.Name));

            return ReturnAllowedJsonGet(result);
        }

        [HttpGet]
        public JsonResult ListByModuleAndUsername(int idModule, string username)
        {
            User user = manager.GetUser(username);

            List<long> result = new List<long>();

            if (user != null)
            {
                List<Permission> userPermissions = manager.GetPermissions(user.Id, SessionIdentification.OrganizationId, (long)idModule);

                result = userPermissions.ConvertAll<long>(x => x.Action.Id);
            }
            return ReturnAllowedJsonGet(result);
        }

        protected override void LoadViewData()
        {
            SelectList areas = new SelectList(manager.GetAreasList(), "id", "name");
            ViewData[ViewDataKeys.SystemAreas] = areas;

            SelectList modulos = new SelectList(manager.GetModulesList(), "id", "name");
            ViewData[ViewDataKeys.SystemModules] = modulos;
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
