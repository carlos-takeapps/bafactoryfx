/************************************************/
/* Database cleanup script                      */
/************************************************/


-- =============================================
-- Delete all content from database
-- =============================================
delete from membership.Actions
delete from membership.Modules
delete from membership.Areas
delete from membership.UsersOrganizations
delete from membership.OrganizationsUsersRoles
delete from membership.Organizations
delete from membership.Users
delete from membership.Permissions
delete from membership.Roles
delete from membership.RolesActions
delete from membership.UsersActionsLog
