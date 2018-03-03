CREATE TABLE [dbo].[GadgetIdentifier] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Identifier]   VARCHAR (50)    NOT NULL,
    [ClientSecret] VARCHAR (MAX)   NOT NULL,
    [PublicKey]    VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);