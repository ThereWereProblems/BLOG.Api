CREATE TABLE [dbo].[Posts] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (MAX) NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [Content]     NVARCHAR (MAX) NOT NULL,
    [UserId]      NVARCHAR (450) NULL,
    [PublishedAt] DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NOT NULL,
    [Image]       NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Posts_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_Posts_UserId]
    ON [dbo].[Posts]([UserId] ASC);

