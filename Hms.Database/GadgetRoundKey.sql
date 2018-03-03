CREATE TABLE [dbo].[GadgetRoundKey]
(
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [GadgetIdentifierId] INT             NOT NULL,
    [RoundKey]           VARBINARY (MAX) NOT NULL,
    [SentTimes]          INT             DEFAULT ((0)) NOT NULL,
	[GeneratedAtUTC]     DATETIME        DEFAULT GETUTCDATE() NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GadgetIdentifierId] FOREIGN KEY ([GadgetIdentifierId]) REFERENCES [dbo].[GadgetIdentifier] ([Id])
);

