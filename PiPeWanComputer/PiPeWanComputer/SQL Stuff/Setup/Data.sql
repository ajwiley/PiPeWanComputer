USE PipeWan;
GO

INSERT INTO [dbo].[Node] ([IPAddress], [NodeName], [LocationName]) VALUES
	('192.168.1.15', 'Aerosteon', 'Basement'),
	('192.168.1.42', 'Baryonyx', 'North Entrance'),
	('192.168.1.66', 'Chilesaurus', 'GoCreate'),
	('192.168.1.3', 'Ammosaurus', 'South Fountain');

INSERT INTO [dbo].[User] ([UserName], [PasswordHash]) VALUES
	('arossillon', 0x61726F7373696C6C6F6E303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030303030);