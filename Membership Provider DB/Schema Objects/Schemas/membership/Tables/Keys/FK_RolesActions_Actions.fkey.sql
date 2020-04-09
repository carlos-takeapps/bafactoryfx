ALTER TABLE [membership].[RolesActions]
    ADD CONSTRAINT [FK_RolesActions_Actions] FOREIGN KEY ([IdAction]) REFERENCES [membership].[Actions] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

