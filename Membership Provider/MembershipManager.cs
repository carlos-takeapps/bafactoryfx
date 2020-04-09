using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BAFactory.Fx.Security.MembershipProvider
{
    // TODO: AGREGAR LOG PARA LAS ACCIONES
    public class MembershipManager
    {
        private MembershipProvider membershipProvider;

        public MembershipManager()
        {
            ConfigureUserManager();
        }

        // TODO: Implement IOC so the UserSecurityManager is loaded in runtime
        private void ConfigureUserManager()
        {
            membershipProvider = new MembershipProvider();
        }

        public bool AuthorizeViewAccess(long userId, long organizationId, string area, string module, string action)
        {
            return membershipProvider.AuthorizeViewAccess(userId, organizationId, area, module, action);
        }

        public static bool ValidateUser(string organizationName, string username, string password)
        {
            return MembershipProvider.ValidateCredentials(organizationName, username, password);
        }

        #region User
        public bool CreateUser(string reqUserName, User newUser)
        {
            // TODO: Authorize requesting user to create new user
            return membershipProvider.CreateUser(newUser) > 0;
        }

        public User GetUser(string username)
        {
            return membershipProvider.GetUser(username);
        }
        public User GetUser(long id)
        {
            return membershipProvider.GetUser(id);
        }

        public List<User> GetUsersList()
        {
            return membershipProvider.GetUsersList();
        }

        public bool UpdateUser(User user)
        {
            return membershipProvider.UpdateUser(user);
        }

        public bool DeleteUser(int id)
        {
            return membershipProvider.DeleteUser(id);
        }
        #endregion

        #region Role
        public List<Role> GetRoles()
        {
            return membershipProvider.GetRoles();
        }
        #endregion

        #region Area
        public bool CreateArea(Area area)
        {
            return membershipProvider.CreateArea(area) > 0;
        }

        public List<Area> GetAreasList()
        {
            return membershipProvider.GetAreasList();
        }

        public Area GetArea(long id)
        {
            return membershipProvider.GetArea(id);
        }

        public bool UpdateArea(Area updated)
        {
            return membershipProvider.UpdateArea(updated);
        }

        public bool DeleteArea(int id)
        {
            return membershipProvider.DeleteArea(id);
        }
        #endregion

        #region Module (Controller)
        public bool CreateModule(Module module)
        {
            return membershipProvider.CreateModule(module) > 0;
        }

        public List<Module> GetModulesList()
        {
            return membershipProvider.GetModulesList();
        }

        public Module GetModule(int id)
        {
            return membershipProvider.GetModule(id);
        }

        public bool UpdateModule(Module module)
        {
            return membershipProvider.UpdateModule(module);
        }

        public bool DeleteModule(int id)
        {
            return membershipProvider.DeleteModule(id);
        }
        #endregion

        #region Action
        public bool CreateAction(Action action)
        {
            return membershipProvider.CreateAction(action) > 0;
        }

        public List<Action> GetActionsList()
        {
            return membershipProvider.GetActionsList();
        }

        public Action GetAction(long id)
        {
            return membershipProvider.GetAction(id);
        }

        public bool UpdateAction(Action action)
        {
            return membershipProvider.UpdateAction(action);
        }

        public bool DeleteAction(int id)
        {
            return membershipProvider.DeleteAction(id);
        }
        #endregion

        #region Permission
        public bool CreatePermission(Permission permission)
        {
            return membershipProvider.CreatePermission(permission) > 0;
        }

        public List<Permission> GetPermissionsList()
        {
            return membershipProvider.GetPermissionsList();
        }

        public Permission GetPermission(int id)
        {
            return membershipProvider.GetPermission(id);
        }
        public Permission GetPermission(long userId, long organizationId, long actionId)
        {
            return membershipProvider.GetPermission(userId, organizationId, actionId);
        }

        public List<Permission> GetPermissions(long userId, long organizationId, long moduleId)
        {
            return membershipProvider.GetPermissionsForModuleUser(userId, organizationId, moduleId);
        }

        //public bool UpdatePermission(Permission permission)
        //{
        //    return membershipProvider.UpdatePermission(permission);
        //}

        public bool DeletePermission(int id)
        {
            return membershipProvider.DeletePermission(id);
        }
        #endregion

        #region Organization
        public Organization GetOrganization(string organizationName)
        {
            return membershipProvider.GetOrganization(organizationName);
        }
        public Organization GetOrganization(long id)
        {
            return membershipProvider.GetOrganization(id);
        }

        public List<Organization> GetOrganizationsList()
        {
            return membershipProvider.GetOrganizationsList();
        }

        public bool UpdateOrganization(Organization org)
        {
            return membershipProvider.UpdateOrganization(org);
        }

        public bool DeleteOrganization(int id)
        {
            return membershipProvider.DeleteOrganizatino(id);
        }
        #endregion

        #region Membership (UserOrganization)

        public bool CreateMembership(UserOrganization membership)
        {
            return membershipProvider.CreateMembership(membership) > 0;
        }
        
        public List<UserOrganization> GetMembershipsList()
        {
            return membershipProvider.GetMembershipsList();
        }

        public UserOrganization GetMembership(int id)
        {
            return membershipProvider.GetMembership(id);
        }
        public UserOrganization GetMembership(long userId, long organizationId)
        {
            return membershipProvider.GetMembership(userId, organizationId);
        }

        public UserOrganization GetMembership(string username, string organizationName)
        {
            return membershipProvider.GetMembership(username, organizationName);
        }
   
        //public bool UpdateMembership(UserOrganization membership)
        //{
        //    return membershipProvider.UpdateMembership(membership);
        //}

        public bool DeleteMembership(int id)
        {
            return membershipProvider.DeleteMembership(id);
        }
        #endregion
    }
}
