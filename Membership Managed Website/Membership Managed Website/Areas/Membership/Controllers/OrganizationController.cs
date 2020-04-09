using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.Areas.Membership.Extensions;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    public class OrganizationController : BaseController
    {
        //
        // GET: /Membership/Organization/

        public ActionResult Index()
        {
            List<Organization> list = manager.GetOrganizationsList();
            return View(list);
        }

        //
        // GET: /Membership/Organization/Details/5

        public ActionResult Details(int id)
        {
            Organization org = manager.GetOrganization(id);
            return View(org);
        }

        //
        // GET: /Membership/Organization/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Membership/Organization/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Membership/Organization/Edit/5
 
        public ActionResult Edit(int id)
        {
            Organization org = manager.GetOrganization(id);
            return View(org);
        }

        //
        // POST: /Membership/Organization/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                Organization org = manager.GetOrganization(id);
                EntityPropertiesLoader.UpdateProperties<Organization>(ref org, collection);
                success = manager.UpdateOrganization(org);
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
        // GET: /Membership/Organization/Delete/5
 
        public ActionResult Delete(int id)
        {
            Organization org = manager.GetOrganization(id);
            return View(org);
        }

        //
        // POST: /Membership/Organization/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool success = false;
            try
            {
                success = manager.DeleteOrganization(id);
            }
            catch
            {
            }

            if (success)
                return RedirectToAction("Index");
            else
                return View();
        }

        protected override void LoadViewData()
        {
            return;
        }

        protected override void LoadControllerData()
        {
            return;
        }

        protected override void SetViewBagData()
        {
            return;
        }
    }
}
