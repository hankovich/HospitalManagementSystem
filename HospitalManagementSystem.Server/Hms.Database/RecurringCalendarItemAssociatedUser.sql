CREATE TABLE [dbo].[RecurringCalendarItemAssociatedUser]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[RecurringCalendarItemId] INT NOT NULL REFERENCES [RecurringCalendarItem]([Id]) ON DELETE CASCADE,
	[UserId] INT NOT NULL REFERENCES [User]([Id]) ON DELETE CASCADE,
	[FromDate] DATETIME NOT NULL,
	[ToDate] DATETIME NOT NULL
)
