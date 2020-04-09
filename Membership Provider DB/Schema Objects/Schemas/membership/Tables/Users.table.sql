CREATE TABLE [membership].[Users] (
    [Id]       BIGINT        IDENTITY (1, 1) NOT NULL,
    [UserName] VARCHAR (30)  NOT NULL,
    [Password] CHAR (20)     NOT NULL,
    [FullName] VARCHAR (100) NOT NULL,
    [email]    VARCHAR (50)  NULL
);

