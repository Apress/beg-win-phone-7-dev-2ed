USE [NotepadDB]
GO

CREATE TABLE [dbo].[User]
(
	[UserId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_User] PRIMARY KEY ( [UserId] )
)
Go

CREATE TABLE [dbo].[Note]
(
	[NoteId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[NoteText] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
    CONSTRAINT [PK_Note] PRIMARY KEY CLUSTERED ( [NoteId] )
)
GO

ALTER TABLE [dbo].[Note]  
	WITH CHECK ADD CONSTRAINT [FK_Note_User] FOREIGN KEY([UserId])
	REFERENCES [dbo].[User] ([UserId])
GO

ALTER TABLE [dbo].[Note] CHECK CONSTRAINT [FK_Note_User]
GO
