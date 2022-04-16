-- Delete Node Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[DeleteNode]
	@NodeID INT
AS 

DELETE
FROM [NodeData]
WHERE [NodeID] = @NodeID

