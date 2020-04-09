ALTER TABLE [membership].[Permissions]
    ADD CONSTRAINT [FK_Permission_Actions] FOREIGN KEY ([IdAction]) REFERENCES [membership].[Actions] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

