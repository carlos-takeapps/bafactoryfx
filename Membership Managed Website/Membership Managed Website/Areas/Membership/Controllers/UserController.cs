using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;
using BAFactory.Fx.Security.Areas.Membership.Extensions;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class UserController : BaseController
    {
        public UserController()
            : base()
        { }

        //
        // GET: /Membership/Usuario/
        public ActionResult Index()
        {
            List<User> usuarios = manager.GetUsersList();

            ApplySorting(ref usuarios, "UserName");

            return View(usuarios);
        }

        public ActionResult Details(int id)
        {
            User user = manager.GetUser(id);
            return View(user);
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
                User user = EntityPropertiesLoader.PopulateProperties<User>(collection);
                // TODO: set password if not AD integrated
                user.Password = string.Empty;
                success = manager.CreateUser(string.Empty, user);
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

            User user = manager.GetUser(id);
            return View(user);
        }

        //
        // POST: /Membership/Accion/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                User user = manager.GetUser(id);
                EntityPropertiesLoader.UpdateProperties<User>(ref user, collection);
                success = manager.UpdateUser(user);
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
            User user = manager.GetUser(id);
            return View(user);
        }

        //
        // POST: /Membership/Accion/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool succeded = false;
            try
            {
                succeded = manager.DeleteUser(id);
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
        public JsonResult Suggest(string term)
        {
            List<User> usuarios = manager.GetUsersList();
            List<User> filtrados = usuarios.Where(x => x.UserName.Contains(term)).ToList<User>();

            List<string> nombres = filtrados.ConvertAll<string>(x => x.UserName);

            return ReturnAllowedJsonGet(nombres);
        }

        protected override void LoadViewData()
        {
            //throw new NotImplementedException();
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
