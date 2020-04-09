using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.Areas.Membership.Extensions;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    public class MembershipController : BaseController
    {
        //
        // GET: /Membership/Membership/

        public ActionResult Index()
        {
            List<UserOrganization> memberships = manager.GetMembershipsList();
            return View(memberships);
        }

        //
        // GET: /Membership/Membership/Details/5

        public ActionResult Details(int id)
        {
            UserOrganization membership = manager.GetMembership(id);
            return View(membership);
        }

        //
        // GET: /Membership/Membership/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Membership/Membership/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool success = false;
            try
            {
                UserOrganization membership = EntityPropertiesLoader.PopulateProperties<UserOrganization>(collection);
                success = manager.CreateMembership(membership);
            }
            catch
            {
            }

            if (success)
                return RedirectToAction("Index");
            else
                return View();
        }
        
        ////
        //// GET: /Membership/Membership/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    UserOrganization membership = manager.GetMembership(id);
        //    return View(membership);
        //}

        ////
        //// POST: /Membership/Membership/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    bool success = false;
        //    try
        //    {
        //        UserOrganization membership = manager.GetMembership(id);
        //        EntityPropertiesLoader.UpdateProperties<UserOrganization>(ref membership, collection);
        //        success = manager.UpdateMembership(membership);
        //    }
        //    catch
        //    {
        //    }

        //    if (success)
        //        return RedirectToAction("Index");
        //    else
        //        return View();
        //}

        //
        // GET: /Membership/Membership/Delete/5
 
        public ActionResult Delete(int id)
        {
            UserOrganization uo = manager.GetMembership(id);
            return View(uo);
        }

        //
        // POST: /Membership/Membership/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool succeded = false;
            try
            {
                succeded = manager.DeleteMembership(id);
            }
            catch
            {
            }

            if (succeded)
                return RedirectToAction("Index");
            else
                return View();
        }

        protected override void LoadViewData()
        {
        }

        protected override void LoadControllerData()
        {
        }

        protected override void SetViewBagData()
        {
        }
    }
}
