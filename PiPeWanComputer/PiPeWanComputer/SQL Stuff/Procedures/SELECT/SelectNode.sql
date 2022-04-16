-- Select Node Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[SelectNode]
	@NodeID INT = -1
AS 

SELECT *
FROM [Node]
WHERE [NodeID] = @NodeID OR @NodeID = -1

-- Return only the Node with the given NodeID or
-- return all nodes if the procedure is called with no NodeID (Null)

