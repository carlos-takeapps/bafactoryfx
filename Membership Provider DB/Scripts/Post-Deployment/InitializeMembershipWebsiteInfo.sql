/************************************************/
/* Membership admin website initialization script */
/************************************************/

-- =============================================
-- Create Membership administration area
-- =============================================
insert into membership.Areas values ('Membership', 'Administración de usuarios')

-- =============================================
-- Create Membership administration modules
-- =============================================
insert into membership.Modules select id, 'Home', 'Módulo de ingreso a la administración de usuarios' from membership.Areas 
insert into membership.Modules select id, 'Action', 'Módulo de administración de Acciones' from membership.Areas 
insert into membership.Modules select id, 'Area', 'Módulo de administración de Áreas' from membership.Areas 
insert into membership.Modules select id, 'Module', 'Módulo de administración de Módulos' from membership.Areas 
insert into membership.Modules select id, 'Permission', 'Módulo de administración de Permisos' from membership.Areas 
insert into membership.Modules select id, 'User', 'Módulo de administración de Usuarios' from membership.Areas 

-- =============================================
-- Create Membership administration actions
-- =============================================
/* for all modules there is an index page */
insert into membership.Actions select id, 'Index', 'Landing page administración de usuarios' from membership.Modules
insert into membership.Actions select id, 'Details', '' from membership.Modules where Name <> 'Home'
insert into membership.Actions select id, 'Create', '' from membership.Modules where Name <> 'Home'
insert into membership.Actions select id, 'Delete', '' from membership.Modules where Name <> 'Home'
insert into membership.Actions select id, 'Edit', '' from membership.Modules where Name <> 'Permission' and Name <> 'Home'
insert into membership.Actions select id, 'ListByModule', '' from membership.Modules where Name = 'Action'
insert into membership.Actions select id, 'ListByModuleAndUsername', '' from membership.Modules where Name = 'Action'
insert into membership.Actions select id, 'ListAreas', '' from membership.Modules where Name = 'Area'
insert into membership.Actions select id, 'Suggest', '' from membership.Modules where Name = 'Area' or Name = 'User'
insert into membership.Actions select id, 'ListByArea', '' from membership.Modules where Name = 'Module'