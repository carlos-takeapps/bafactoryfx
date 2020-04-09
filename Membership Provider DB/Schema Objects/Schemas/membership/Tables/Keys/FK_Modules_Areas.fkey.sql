ALTER TABLE [membership].[Modules]
    ADD CONSTRAINT [FK_Modules_Areas] FOREIGN KEY ([IdArea]) REFERENCES [membership].[Areas] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;

