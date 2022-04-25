USE PipeWan;
GO

INSERT INTO [dbo].[Node] ([IPAddress], [NodeName], [LocationName]) VALUES
	('192.168.1.15', 'Aerosteon', 'Basement'),
	('192.168.1.42', 'Baryonyx', 'North Entrance'),
	('192.168.1.66', 'Chilesaurus', 'GoCreate'),
	('192.168.1.3', 'Ammosaurus', 'South Fountain');

INSERT INTO [dbo].[User] ([UserName], [PasswordHash]) VALUES
	('Alex213', 0x010203040506),
	('Brondo', 0x020304050607),
	('Pilot1967', 0x030405060708);