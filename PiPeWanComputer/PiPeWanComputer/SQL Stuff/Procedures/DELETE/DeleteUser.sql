-- Delete User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[DeleteUser]
	@UserName NVARCHAR(128) = ''
AS 

DELETE
FROM [User]
WHERE [UserName] = @UserName

