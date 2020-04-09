ALTER TABLE [membership].[Permissions]
    ADD CONSTRAINT [FK_Permissions_UsersOrganizations] FOREIGN KEY ([IdUserOrganization]) REFERENCES [membership].[UsersOrganizations] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

