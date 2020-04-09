CREATE TABLE [membership].[UsersOrganizations] (
    [Id]             BIGINT IDENTITY (1, 1) NOT NULL,
    [IdUser]         BIGINT NOT NULL,
    [IdOrganization] BIGINT NOT NULL
);

