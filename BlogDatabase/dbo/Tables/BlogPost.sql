CREATE TABLE [dbo].[BlogPost] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [UserId]            INT           NULL,
    [Title]             VARCHAR (70)  NOT NULL,
    [Text]              VARCHAR (MAX) NOT NULL,
    [FontSize]          INT           NULL,
    [FontColorId]       INT           NULL,
    [BackgroundColorId] INT           NULL,
    [Image]             IMAGE         NULL,
    [IsVisible]         BIT           NULL,
    CONSTRAINT [PK_BlogPost] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_BlogPost_Color] FOREIGN KEY ([FontColorId]) REFERENCES [dbo].[Color] ([Id]),
    CONSTRAINT [FK_BlogPost_Color1] FOREIGN KEY ([BackgroundColorId]) REFERENCES [dbo].[Color] ([Id]),
    CONSTRAINT [FK_BlogPost_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
);

