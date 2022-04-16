-- Update Node Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateNode]
	@NodeID INT,
	@IPAddress NVARCHAR(12) = NULL,
	@NodeName NVARCHAR(128) = NULL,
	@LocationName NVARCHAR(128) = NULL

AS 

UPDATE [Node]
	SET
		[IPAdress] = CASE WHEN @IPAddress IS NOT NULL THEN @IPAddress ELSE [IPAdress] END,
		[NodeName] = CASE WHEN @NodeName IS NOT NULL THEN @NodeName ELSE [NodeName] END,
		[LocationName] = CASE WHEN @LocationName IS NOT NULL THEN @LocationName ELSE [LocationName] END
	WHERE [NodeID] = @NodeID
