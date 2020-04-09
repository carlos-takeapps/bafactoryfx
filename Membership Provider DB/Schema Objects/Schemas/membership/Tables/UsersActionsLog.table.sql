CREATE TABLE [membership].[UsersActionsLog] (
    [Id]         BIGINT        IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME2 (7) NOT NULL,
    [UserName]   VARCHAR (30)  NULL,
    [ModuleName] VARCHAR (100) NOT NULL,
    [ActionName] VARCHAR (100) NOT NULL,
    [Url]        VARCHAR (250) NOT NULL,
    [EntityId]   BIGINT        NULL,
    [Detail]     XML           NULL
);

