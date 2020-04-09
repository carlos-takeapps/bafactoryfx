using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.MVCExtension;
using mp = BAFactory.Fx.Security.MembershipProvider;
using BAFactory.Fx.Security.Areas.Membership.Extensions;
using System.Collections;
using System.Reflection;

namespace BAFactory.Fx.Security.Areas.Membership.Controllers
{
    [MembershipCustomAuthorize]
    public class PermissionController : BaseController
    {
        public PermissionController()
            : base()
        {
        }

        //
        // GET: /Membership/Permiso/
        public ActionResult Index(int? page)
        {
            UpdateFilterInformation(page);

            LoadViewData();

            List<Permission> permissions = manager.GetPermissionsList();

            ApplyUserFilter(ref permissions);

            ApplySorting(ref permissions);

            SetViewBagData();

            return View(permissions);
        }

        //
        // GET: /Membership/Permiso/Details/5
        public ActionResult Details(int id)
        {
            Permission permission = manager.GetPermission(id);
            return View(permission);
        }

        //
        // GET: /Membership/Permiso/Create
        public ActionResult Create()
        {
            LoadViewData();

            return View();
        }

        //
        // POST: /Membership/Permiso/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            bool succeded = false;

            string username = ExtractUsername(collection);
            long organizationId = SessionIdentification.OrganizationId;

            List<long> actionsIds = ExtractActionsIds(collection);
            long moduleId = ExctractModuleId(collection);

            if (!string.IsNullOrEmpty(username) && actionsIds != null && actionsIds.Count > 0)
            {
                User user = manager.GetUser(username);

                if (user != null)
                {
                    List<long> userModuleActionsIds = GetUserModulesActionsIds(user.Id, organizationId, moduleId);
                    List<long> missingActionsIds = GetUserMissingActions(userModuleActionsIds, actionsIds);
                    List<long> deltedActionsIds = GetUserRevokedActions(userModuleActionsIds, actionsIds);

                    succeded = CreatePermissions(user.Id, organizationId, missingActionsIds);
                    succeded = DeletePermissions(user, organizationId, deltedActionsIds);
                }
            }

            if (succeded)
                return RedirectToAction("Index");
            else
                return View();
        }

        //
        // GET: /Membership/Permiso/Delete/5
        public ActionResult Delete(int id)
        {
            Permission permissino = manager.GetPermission(id);
            return View(permissino);
        }

        //
        // POST: /Membership/Permiso/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            bool succeded = false;
            try
            {
                succeded = manager.DeletePermission(id);
            }
            catch
            {
            }

            if (succeded)
                return RedirectToAction("Index");
            else
                return View();
        }

        override protected void SetViewBagData()
        {
        }

        override protected void LoadViewData()
        {
            SelectList areas = new SelectList(manager.GetAreasList(), "id", "name", FilterInfo.Area.Id);
            ViewData[ViewDataKeys.SystemAreas] = areas;

            SelectList modules = new SelectList(manager.GetModulesList(), "id", "name", FilterInfo.Module.Id);
            ViewData[ViewDataKeys.SystemModules] = modules;

            SelectList actions = new SelectList(manager.GetActionsList(), "id", "name", FilterInfo.Action.Id);
            ViewData[ViewDataKeys.SystemActions] = actions;

            SelectList users = new SelectList(manager.GetUsersList(), "id", "username", FilterInfo.User.Id);
            ViewData[ViewDataKeys.SystemUsers] = users;
        }

        private List<long> GetUserModulesActionsIds(long userId, long organizationId, long moduleId)
        {
            List<long> result = null;
            List<Permission> userActions = manager.GetPermissions(userId, organizationId, moduleId);

            if (userActions != null)
            {
                result = userActions.ConvertAll<long>(x => x.IdAction);
            }

            return result;
        }

        private long ExctractModuleId(FormCollection collection)
        {
            long moduleId;
            long.TryParse((string)collection[FormsFieldsNames.ModuleId], out moduleId);
            return moduleId;
        }

        private static int SortPermissionsByActionPath(Permission a, Permission b)
        {
            string aPath = string.Concat((a.Action.Module.Area.Name == string.Empty ? "_" : a.Action.Module.Area.Name), a.Action.Module.Name, a.Action.Name);
            string bPath = string.Concat((b.Action.Module.Area.Name == string.Empty ? "_" : b.Action.Module.Area.Name), b.Action.Module.Name, b.Action.Name);

            return string.Compare(aPath, bPath);
        }

        private void ApplyUserFilter(ref List<Permission> permissions)
        {
            if (FilterInfo.User.Id < 1) return;

            User user = manager.GetUser(FilterInfo.User.Id);

            if (user == null)
                permissions = new List<Permission>();
            else
                permissions = permissions.FindAll(x => x.UsersOrganization.User.Id == user.Id);

            if (FilterInfo.Area.Id < 1)
            {
                return;
            }

            permissions = permissions.FindAll(x => x.Action.Module.IdArea == FilterInfo.Area.Id);
        }

        private void ApplySorting(ref List<Permission> permissions)
        {
            if (string.IsNullOrEmpty(FilterInfo.Sort))
            {
                permissions.Sort(new Comparison<Permission>(SortPermissionsByActionPath));
            }
        }

        private bool DeletePermissions(User user, long organizationId, List<long> deltedActionsIds)
        {
            bool succeded = true;
            foreach (long deletedActionId in deltedActionsIds)
            {
                succeded &= DeletePermission(user, organizationId, deletedActionId);
            }

            return succeded;
        }

        private bool DeletePermission(User user, long organizationId, long deletedActionId)
        {
            bool succeded = false;
            if (user != null)
            {
                Permission toDelete = manager.GetPermission(user.Id, organizationId, deletedActionId);

                if (toDelete != null)
                {
                    succeded = manager.DeletePermission((int)toDelete.Id);
                }
            }
            return succeded;
        }

        private List<long> GetUserRevokedActions(List<long> userActionsIds, List<long> actionsIds)
        {
            List<long> missingPermissionsIds = userActionsIds.FindAll(x => !actionsIds.Contains(x));

            return missingPermissionsIds;
        }

        private List<long> GetUserActionsIds(long userId, long organizationId)
        {
            List<Permission> allPermissions = manager.GetPermissionsList();
            List<Permission> userPermissions = allPermissions.FindAll(x => x.UsersOrganization.IdUser == userId && x.UsersOrganization.IdOrganization == organizationId);
            return userPermissions.ConvertAll<long>(x => x.Id);
        }

        private List<long> ExtractActionsIds(FormCollection collection)
        {
            List<long> result = null;
            if (collection[FormsFieldsNames.ActionId] != null)
            {
                string[] idsstrings = ((string)collection[FormsFieldsNames.ActionId]).Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (idsstrings != null && idsstrings.Length > 0)
                {
                    result = new List<long>();
                    foreach (string idstring in idsstrings)
                    {
                        long id;
                        if (long.TryParse(idstring, out id))
                        {
                            result.Add(id);
                        }
                    }
                }
            }
            return result;
        }

        private string ExtractUsername(FormCollection collection)
        {
            return collection[FormsFieldsNames.Username] as string;
        }

        private bool CreatePermissions(long userId, long organizationId, List<long> missingActionsIds)
        {
            bool succeded = true;
            foreach (long missingActionId in missingActionsIds)
            {
                succeded &= CreatePermission(userId, organizationId, missingActionId);
            }

            return succeded;
        }

        private bool CreatePermission(long userId, long organizationId, long missingActionId)
        {
            bool succeded = false;

            Permission permission = new Permission();

            permission.UsersOrganization = new UserOrganization() { IdOrganization = SessionIdentification.OrganizationId, IdUser = userId };
            permission.IdAction = missingActionId;

            succeded = manager.CreatePermission(permission);

            return succeded;
        }

        private List<long> GetUserMissingActions(List<long> userActions, List<long> actionsIds)
        {
            List<long> missingPermissionsIds = actionsIds.FindAll(x => !userActions.Contains(x));

            return missingPermissionsIds;
        }

        protected override void LoadControllerData()
        {
            return;
        }
    }
}
