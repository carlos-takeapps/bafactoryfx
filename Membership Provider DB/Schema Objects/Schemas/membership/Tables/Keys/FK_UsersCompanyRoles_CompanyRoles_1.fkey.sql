﻿ALTER TABLE [membership].[OrganizationsUsersRoles]
    ADD CONSTRAINT [FK_UsersCompanyRoles_CompanyRoles] FOREIGN KEY ([IdRol]) REFERENCES [membership].[Roles] ([Id]) ON DELETE NO ACTION ON UPDATE NO ACTION;
