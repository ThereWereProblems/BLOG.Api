CREATE TABLE [dbo].[AuditLogs] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [User]       NVARCHAR (MAX) NULL,
    [EntityName] NVARCHAR (MAX) NOT NULL,
    [Action]     NVARCHAR (MAX) NOT NULL,
    [Timestamp]  DATETIME2 (7)  NOT NULL,
    [Changes]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
);

