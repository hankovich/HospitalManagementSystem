CREATE TABLE [dbo].[Gadget] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [Identifier]   NVARCHAR (50)    NOT NULL,
    [ClientSecret] NVARCHAR (MAX)   NOT NULL,
    [PublicKey]    VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);