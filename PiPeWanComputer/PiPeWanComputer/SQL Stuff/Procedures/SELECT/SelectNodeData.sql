-- Select NodeData Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[SelectNodeData]
	@NodeID INT = NULL,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL
AS 

IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
	BEGIN
		SELECT *
		FROM [NodeData]
		WHERE ([NodeID] = @NodeID OR @NodeID IS NULL) AND [TimeStamp] >= @StartDate AND [TimeStamp] =< @EndDate 
	END
ELSE
	BEGIN
		SELECT *
		FROM [NodeData]
		WHERE [NodeID] = @NodeID OR @NodeID IS NULL
	END


-- Return a list of NodeData with the given NodeID. If no NodeID is provided, @NodeID is NULL with evaluate to true.
-- If both a start and end date are provided, the TimeStamp of each row is filtered to be in the specified timerange (inclusive).
