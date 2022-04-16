-- Add NodeData Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[AddNodeData]
	@NodeID INT,
	@Battery FLOAT = 0.0,
	@Temperature FLOAT = 0.0,
	@Flow FLOAT = 0.0,
	@Status NVARCHAR = 'IDLE'
AS 

IF EXISTS (SELECT * FROM [Node] WHERE [NodeID] = @NodeID)
	BEGIN
		INSERT INTO [NodeData] ([NodeID], [Battery], [Temperature], [Flow], [Status])
		VALUES (@NodeID, @Battery, @Temperature, @Flow, @Status)
	END

