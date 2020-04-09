ALTER TABLE [membership].[Actions]
    ADD CONSTRAINT [FK_Actions_Modules] FOREIGN KEY ([IdModule]) REFERENCES [membership].[Modules] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

