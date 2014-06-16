﻿CREATE TABLE [dbo].[OperatingCosts]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [TransactionId] INT NOT NULL, 
    [JournalEntryId] INT NULL, 
    [Category] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(MAX) NULL, 
    [CustomCost] DECIMAL(18, 5) NULL, 
    CONSTRAINT [FK_OperatingCosts_ToTransactions] FOREIGN KEY ([TransactionId]) REFERENCES [Transactions]([Id]),
    CONSTRAINT [FK_OperatingCosts_ToJournalEntries] FOREIGN KEY ([JournalEntryId]) REFERENCES [JournalEntries]([Id])
)