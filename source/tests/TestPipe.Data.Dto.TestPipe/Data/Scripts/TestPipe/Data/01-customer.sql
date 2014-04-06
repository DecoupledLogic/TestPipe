USE [TestPipe]
GO

SET IDENTITY_INSERT [dbo].[Customer] ON
GO

INSERT INTO [TestPipe].[dbo].[Customer]
([CustomerId],[CompanyName]
VALUES(10,'Test Customer')
GO

SET IDENTITY_INSERT [dbo].[Customer] OFF
GO