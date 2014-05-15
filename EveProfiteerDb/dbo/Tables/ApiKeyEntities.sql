﻿CREATE TABLE [dbo].[ApiKeyEntities] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [EntityId]  BIGINT         NOT NULL,
    [Name]      NVARCHAR (MAX) NULL,
    [Type]      NVARCHAR (MAX) NULL,
    [ImagePath] NVARCHAR (MAX) NULL,
    [IsActive]  BIT            NOT NULL,
    CONSTRAINT [PK_dbo.ApiKeyEntities] PRIMARY KEY CLUSTERED ([Id] ASC)
);
