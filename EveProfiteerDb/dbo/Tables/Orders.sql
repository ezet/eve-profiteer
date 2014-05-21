﻿CREATE TABLE [dbo].[OrderData] (
    [Id]                INT             IDENTITY (1, 1) NOT NULL,
    [TypeId]            INT          NOT NULL,
    [BuyQuantity]       INT             NOT NULL,
    [MaxBuyPrice]       DECIMAL (18, 2) NOT NULL,
    [MinSellQuantity]   INT             NOT NULL,
    [MinSellPrice]      DECIMAL (18, 2) NOT NULL,
    [MaxSellQuantity]   INT             NOT NULL,
    [UpdateTime]        DATETIME        NOT NULL,
    [AvgVolume]         FLOAT (53)      NOT NULL,
    [CurrentBuyPrice]   DECIMAL (18, 2) NOT NULL,
    [CurrentSellPrice]  DECIMAL (18, 2) NOT NULL,
    [AvgPrice]          DECIMAL (18, 2) NOT NULL,
    [ApiKeyEntity_Id] INT NULL, 
    [IsSellOrder] BIT NOT NULL, 
    [IsBuyOrder] BIT NOT NULL, 
    [Notes] TEXT NULL, 
    CONSTRAINT [PK_dbo.OrderData] PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_Orders_ToApiKeyEntities] FOREIGN KEY ([ApiKeyEntity_Id]) REFERENCES [ApiKeyEntities]([Id]), 
	UNIQUE ([TypeId])
);

