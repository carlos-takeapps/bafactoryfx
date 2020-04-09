using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Security.MembershipProvider
{
    internal class MembershipProvider
    {
        internal MembershipProvider()
        {
        }

        internal static bool ValidateCredentials(string organizationName, string username, string password)
        {
            bool validated = false;

            MembershipSchemaEntities entitiesDataContext = new MembershipSchemaEntities();

            var users = from uo in entitiesDataContext.UserOrganizations
                        where uo.User.UserName == username
                        && uo.User.Password == password
                        && uo.Organization.Name == organizationName
                        select uo.User;

            if (users != null && users.Count() == 1)
            {
                validated = true;
            }

            return validated;
        }

        internal bool ValidateUserCredentials(string organizationName, string username, string password)
        {
            return MembershipProvider.ValidateCredentials(organizationName, username, password);
        }

        internal bool AuthorizeViewAccess(long userId, long organizationId, string area, string module, string action)
        {
            bool validated = false;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            var users = from p in entitiesDataContext.Permissions
                        where p.Action.Name == action        // The Action
                        && p.Action.Module.Name == module    // for the specific View
                        && p.Action.Module.Area.Name == area // for the specific Area
                        && p.UsersOrganization.IdUser == userId// is GRANTED to the User
                        && p.UsersOrganization.IdOrganization == organizationId
                        select p.UsersOrganization.User;

            if (users != null && users.Count() == 1)
            {
                validated = true;
            }

            return validated;
        }

        #region Users
        internal long CreateUser(User user)
        {
            long id = 0;

            if (user == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            entitiesDataContext.Users.AddObject(user);

            if (SaveChanges(entitiesDataContext)) id = user.Id;

            return id;
        }

        internal List<User> GetUsersList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Users.ToList<User>();
        }

        internal User GetUser(string username)
        {
            User result = null;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            IQueryable<User> users = from u in entitiesDataContext.Users
                                     where u.UserName == username
                                     select u;
            if (users != null && users.Count() > 0)
            {
                result = users.First();
            }
            return result;
        }
        internal User GetUser(long id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return (from u in entitiesDataContext.Users
                    where u.Id == id
                    select u).First();
        }

        internal bool UpdateUser(User user)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            User orig = entitiesDataContext.Users.Where(x => x.Id == user.Id).First();
            orig = user;

            entitiesDataContext.Users.ApplyCurrentValues(user);

            return SaveChanges(entitiesDataContext);
        }

        internal bool DeleteUser(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            User user = (from u in entitiesDataContext.Users
                         where u.Id == id
                         select u).First();

            entitiesDataContext.Users.DeleteObject(user);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Roles
        internal long CreateRol(Role role)
        {
            long id = 0;

            if (role == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            entitiesDataContext.Roles.AddObject(role);

            if (SaveChanges(entitiesDataContext)) id = role.Id;

            return id;
        }

        internal List<Role> GetRoles()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Roles.ToList<Role>();
        }
        #endregion

        #region Area
        internal long CreateArea(Area area)
        {
            long id = 0;

            if (area == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            entitiesDataContext.Areas.AddObject(area);

            if (SaveChanges(entitiesDataContext)) id = area.Id;

            return id;
        }

        internal List<Area> GetAreasList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Areas.ToList<Area>();
        }
        internal Area GetArea(long id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return (from a in entitiesDataContext.Areas
                    where a.Id == id
                    select a).First();
        }

        internal bool UpdateArea(Area updated)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            Area orig = entitiesDataContext.Areas.Where(x => x.Id == updated.Id).First();
            orig = updated;
            entitiesDataContext.Areas.ApplyCurrentValues(orig);

            return SaveChanges(entitiesDataContext);
        }

        internal bool DeleteArea(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Area toDelete = (from a in entitiesDataContext.Areas
                             where a.Id == id
                             select a).First();

            entitiesDataContext.Areas.DeleteObject(toDelete);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Module
        internal long CreateModule(Module module)
        {
            long id = 0;

            if (module == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            module.Area = (entitiesDataContext.Areas.Where(x => x.Id == module.IdArea)).First();

            entitiesDataContext.Modules.AddObject(module);

            if (SaveChanges(entitiesDataContext)) id = module.Id;

            return id;
        }

        internal List<Module> GetModulesList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Modules.ToList<Module>();
        }
        internal Module GetModule(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return (from m in entitiesDataContext.Modules
                    where m.Id == id
                    select m).First();
        }

        internal bool UpdateModule(Module module)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            Module orig = entitiesDataContext.Modules.Where(x => x.Id == module.Id).First();
            orig = module;
            entitiesDataContext.Modules.ApplyCurrentValues(orig);

            return SaveChanges(entitiesDataContext);
        }

        internal bool DeleteModule(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Module toDelete = (from m in entitiesDataContext.Modules
                               where m.Id == id
                               select m).First();

            entitiesDataContext.Modules.DeleteObject(toDelete);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Action

        internal long CreateAction(Action action)
        {
            long id = 0;

            if (action == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            action.Module = (from m in entitiesDataContext.Modules where m.Id == action.IdModule select m).First();

            entitiesDataContext.Actions.AddObject(action);

            if (SaveChanges(entitiesDataContext)) id = action.Id;

            return id;
        }

        internal List<Action> GetActionsList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Actions.ToList<Action>();
        }

        internal Action GetAction(long id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return (from a in entitiesDataContext.Actions
                    where a.Id == id
                    select a).First();
        }

        internal bool UpdateAction(Action action)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            Action orig = entitiesDataContext.Actions.Where(x => x.Id == action.Id).First();
            orig = action;
            entitiesDataContext.Actions.ApplyCurrentValues(orig);

            return SaveChanges(entitiesDataContext);
        }

        internal bool DeleteAction(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Action toDelete = (from a in entitiesDataContext.Actions where a.Id == id select a).First();

            entitiesDataContext.Actions.DeleteObject(toDelete);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Permission
        internal long CreatePermission(Permission permission)
        {
            long id = 0;

            if (permission == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            permission.UsersOrganization = (from uo in entitiesDataContext.UserOrganizations
                                            where uo.User.Id == permission.UsersOrganization.IdUser
                                            && uo.Organization.Id == permission.UsersOrganization.IdOrganization
                                            select uo).First();
            permission.Action = (from a in entitiesDataContext.Actions
                                 where a.Id == permission.IdAction
                                 select a).First();

            entitiesDataContext.Permissions.AddObject(permission);

            if (SaveChanges(entitiesDataContext)) id = permission.Id;

            return id;
        }

        internal List<Permission> GetPermissionsList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Permissions.ToList<Permission>();
        }

        internal Permission GetPermission(long userId, long organizationId, long actionId)
        {
            Permission result = null;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            IQueryable<Permission> permissions = from p in entitiesDataContext.Permissions
                                                 where p.UsersOrganization.IdUser == userId
                                                 && p.Id == actionId
                                                 select p;
            if (permissions != null && permissions.Count() > 0)
            {
                result = permissions.First();
            }
            return result;
        }
        internal Permission GetPermission(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return (from p in entitiesDataContext.Permissions
                    where p.Id == id
                    select p).First();
        }

        internal List<Permission> GetPermissionsForModuleUser(long userId, long organizatinoId, long moduleId)
        {
            List<Permission> result = new List<Permission>();

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            IQueryable<Permission> permissions = from p in entitiesDataContext.Permissions
                                                 where p.UsersOrganization.IdUser == userId
                                                 && p.UsersOrganization.IdOrganization == organizatinoId
                                                 && p.Action.IdModule == moduleId
                                                 select p;
            if (permissions != null)
            {
                result = permissions.ToList<Permission>();
            }

            return result;
        }

        //internal bool UpdatePermission(Permission permission)
        //{
        //    MembershipSchemaEntities entitiesDataContext = GetDataContext();

        //    entitiesDataContext.Permissions.ApplyCurrentValues(permission);

        //    return SaveChanges(entitiesDataContext);
        //}

        internal bool DeletePermission(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Permission permission = (from p in entitiesDataContext.Permissions where p.Id == id select p).First();

            entitiesDataContext.Permissions.DeleteObject(permission);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Organization
        internal List<Organization> GetOrganizationsList()
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            return entitiesDataContext.Organizations.ToList<Organization>();
        }

        internal Organization GetOrganization(string organizationName)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Organization org = (from o in entitiesDataContext.Organizations
                                where o.Name == organizationName
                                select o).First();

            return org;
        }
        internal Organization GetOrganization(long id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Organization org = (from o in entitiesDataContext.Organizations
                                where o.Id == id
                                select o).First();

            return org;
        }


        internal bool UpdateOrganization(Organization org)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            Organization orig = entitiesDataContext.Organizations.Where(x => x.Id == org.Id).First();
            orig = org;
            entitiesDataContext.Organizations.ApplyCurrentValues(orig);

            return SaveChanges(entitiesDataContext);
        }

        internal bool DeleteOrganizatino(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            Organization org = (from o in entitiesDataContext.Organizations where o.Id == id select o).First();

            entitiesDataContext.Organizations.DeleteObject(org);

            return SaveChanges(entitiesDataContext);
        }
        #endregion

        #region Membership

        internal long CreateMembership(UserOrganization membership)
        {
            long id = 0;

            if (membership == null) return id;

            MembershipSchemaEntities entitiesDataContext = GetDataContext();

            membership.User = (from u in entitiesDataContext.Users
                               where u.Id == membership.IdUser
                               select u).First();
            membership.Organization = (from o in entitiesDataContext.Organizations
                                       where o.Id == membership.IdOrganization
                                       select o).First();

            entitiesDataContext.UserOrganizations.AddObject(membership);

            if (SaveChanges(entitiesDataContext)) id = membership.Id;

            return id;
        }

        internal List<UserOrganization> GetMembershipsList()
        {
            MembershipSchemaEntities dc = GetDataContext();
            IQueryable<UserOrganization> memberships = from uo in dc.UserOrganizations
                                                       select uo;
            return memberships.ToList<UserOrganization>();
        }

        internal UserOrganization GetMembership(int id)
        {
            MembershipSchemaEntities dc = GetDataContext();
            UserOrganization membership = (from uo in dc.UserOrganizations
                                           where uo.Id == id
                                           select uo).First();
            return membership;
        }
        internal UserOrganization GetMembership(long userId, long organizationId)
        {
            MembershipSchemaEntities dc = GetDataContext();
            UserOrganization membership = (from uo in dc.UserOrganizations
                                           where uo.IdUser == userId
                                           && uo.IdOrganization == organizationId
                                           select uo).First();
            return membership;
        }
        //internal bool UpdateMembership(UserOrganization membership)
        //{
        //    MembershipSchemaEntities entitiesDataContext = GetDataContext();

        //    entitiesDataContext.UserOrganizations.ApplyCurrentValues(membership);

        //    return SaveChanges(entitiesDataContext);
        //}

        internal UserOrganization GetMembership(string username, string organizationName)
        {
            MembershipSchemaEntities dc = GetDataContext();
            UserOrganization membership = (from uo in dc.UserOrganizations
                                           where uo.User.UserName == username
                                           && uo.Organization.Name == organizationName
                                           select uo).First();
            return membership;
        }

        internal bool DeleteMembership(int id)
        {
            MembershipSchemaEntities entitiesDataContext = GetDataContext();
            UserOrganization membership = (from uo in entitiesDataContext.UserOrganizations
                                           where uo.Id == id
                                           select uo).First();
            entitiesDataContext.UserOrganizations.DeleteObject(membership);
            return SaveChanges(entitiesDataContext);
        }

        #endregion

        private bool SaveChanges(MembershipSchemaEntities dc)
        {
            int affected = 0;
            if (dc != null)
                affected = dc.SaveChanges();
            return affected > 0;
        }

        private MembershipSchemaEntities GetDataContext()
        {
            return new MembershipSchemaEntities();
        }

        private void DisposeDataContext(MembershipSchemaEntities dc)
        {
            dc.Dispose();
        }
    }
}
