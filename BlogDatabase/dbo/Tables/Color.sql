CREATE TABLE [dbo].[Color] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED ([Id] ASC)
);

