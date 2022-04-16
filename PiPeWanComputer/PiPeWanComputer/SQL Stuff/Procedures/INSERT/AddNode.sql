-- Add Node Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[AddNode]
	@IPAddress NVARCHAR(12),
	@NodeName NVARCHAR(128) = '',
	@LocationName NVARCHAR(128) = ''
AS 

INSERT INTO [Node] ([IPAddress], [NodeName], [LocationName])
VALUES (@IPAddress, @NodeName, @LocationName)


