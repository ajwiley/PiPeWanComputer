﻿USE PipeWan;
GO

INSERT INTO [dbo].[Node] ([IPAddress], [NodeName], [LocationName]) VALUES
	('127.0.0.1', 'SparkFun', 'Desk');

INSERT INTO [dbo].[User] ([UserName], [PasswordHash]) VALUES
	('arossillon', 0x61726F7373696C6C6F6E303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030);