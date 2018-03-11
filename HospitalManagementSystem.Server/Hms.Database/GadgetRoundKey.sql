CREATE TABLE [dbo].[GadgetRoundKey]
(
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [GadgetId]			 INT             NOT NULL,
    [RoundKey]           VARBINARY (MAX) NOT NULL,
    [SentTimes]          INT             DEFAULT ((0)) NOT NULL,
	[GeneratedAtUTC]     DATETIME        DEFAULT GETUTCDATE() NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_GadgetId] FOREIGN KEY ([GadgetId]) REFERENCES [dbo].[Gadget] ([Id])
);

