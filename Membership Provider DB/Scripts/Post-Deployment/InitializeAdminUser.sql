/************************************************/
/* Admin user initialization script             */
/************************************************/

-- =============================================
-- Create admin user
-- =============================================
insert into membership.Users values ('admin','admin', 'Administrator', 'admin@bafactory.net')

-- =============================================
-- Create default organization
-- =============================================
insert into membership.Organizations values ('default', 'default organization')

-- =============================================
-- Associate admin user with organization
-- =============================================
insert into membership.UsersOrganizations select u.id, o.id from membership.Users as u, membership.Organizations as o

-- =============================================
-- Assign all permisions to admin user
-- =============================================
insert into membership.Permissions select uo.id, a.id from membership.Actions as a, membership.UsersOrganizations as uo
