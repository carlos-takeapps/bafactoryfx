CREATE TABLE [membership].[Modules] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdArea]      BIGINT        NOT NULL,
    [Name]        VARCHAR (100) NOT NULL,
    [Description] VARCHAR (250) NOT NULL
);

