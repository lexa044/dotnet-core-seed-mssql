-- 1.Create Database
CREATE DATABASE DNSeed;
GO
USE [DNSeed]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Authentication](
	[Id] [int] NOT NULL,
	[LoginName] [varchar](16) NOT NULL,
	[Password] [varchar](64) NOT NULL,
	[LastLogin] [datetime] NOT NULL,
	[LastIPAddress] [varchar](64) NOT NULL,
	[LastDeviceId] [varchar](256) NOT NULL,
	[Token] [varchar](64) NOT NULL,
 CONSTRAINT [PK_Authentication] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
       [Id] [int] IDENTITY(1,1) NOT NULL,
       [SKU] [nvarchar](50) NOT NULL,
       [Name] [nvarchar](250) NOT NULL,
       [ImageUri] [nvarchar](250) NULL,
       --[ProductTypeID] [uniqueidentifier] NOT NULL,
       [Status] [smallint] NOT NULL,
       [CreatedDate] [datetime] NOT NULL,
       [UpdatedDate] [datetime] NULL,
       [UpdatedUser] [uniqueidentifier] NULL,
CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED
(
       [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT [IX_Product] UNIQUE NONCLUSTERED
(
       [SKU] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- 2. Add a demo user
INSERT INTO [dbo].[Authentication]
           ([Id]
           ,[LoginName]
           ,[Password]
           ,[LastLogin]
           ,[LastIPAddress]
           ,[LastDeviceId]
           ,[Token])
     SELECT
           2
           ,'demo2'
           ,'demo2_Sup3rPwd'
           ,GETDATE()
           ,''
           ,''
           ,''
WHERE NOT EXISTS (
	SELECT 1
	FROM [dbo].[Authentication]
	WHERE Id=2
);
