CREATE TABLE [dbo].[User] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [UserName]     NVARCHAR (100) NOT NULL,
    [PasswordHash] NVARCHAR (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([UserName] ASC)
);