CREATE TABLE [membership].[Permissions] (
    [Id]                 BIGINT IDENTITY (1, 1) NOT NULL,
    [IdUserOrganization] BIGINT NOT NULL,
    [IdAction]           BIGINT NOT NULL
);



