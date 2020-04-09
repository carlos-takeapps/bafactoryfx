ALTER TABLE [membership].[RolesActions]
    ADD CONSTRAINT [FK_CompanyRolesViews_CompanyRoles] FOREIGN KEY ([IdRol]) REFERENCES [membership].[Roles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

