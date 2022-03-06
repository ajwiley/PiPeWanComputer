-- Select Cart Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[SelectUser]
	@UserID INT = 0
AS 

SELECT *
FROM [User]
WHERE [UserID] = @UserID OR @UserID = 0

