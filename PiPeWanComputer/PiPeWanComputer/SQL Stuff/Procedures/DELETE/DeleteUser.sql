-- Delete User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[DeleteUser]
	@UserID INT = 0
AS 

DELETE
FROM [User]
WHERE [UserID] = @UserID

