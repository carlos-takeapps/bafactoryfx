ALTER TABLE [membership].[OrganizationsUsersRoles]
    ADD CONSTRAINT [FK_OrganizationsUsersRoles_UsersOrganizations] FOREIGN KEY ([IdUserOrganizatin]) REFERENCES [membership].[UsersOrganizations] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

