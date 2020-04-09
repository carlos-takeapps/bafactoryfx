CREATE TABLE [membership].[Actions] (
    [Id]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [IdModule]    BIGINT        NOT NULL,
    [Name]        VARCHAR (50)  NOT NULL,
    [Description] VARCHAR (250) NOT NULL
);

