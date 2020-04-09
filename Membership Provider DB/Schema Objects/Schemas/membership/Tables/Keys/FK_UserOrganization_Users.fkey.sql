ALTER TABLE [membership].[UsersOrganizations]
    ADD CONSTRAINT [FK_UserOrganization_Users] FOREIGN KEY ([IdUser]) REFERENCES [membership].[Users] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

