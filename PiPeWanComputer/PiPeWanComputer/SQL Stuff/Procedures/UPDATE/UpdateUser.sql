-- Update User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateUser]
	@UserName NVARCHAR(15),
	@PasswordHash BINARY(64) = NULL,
	@AccessLevel INT = NULL
AS 

UPDATE [User]
	SET
		[PasswordHash] = CASE WHEN @PasswordHash IS NOT NULL THEN @PasswordHash ELSE [PasswordHash] END,
		[AccessLevel] = CASE WHEN @AccessLevel IS NOT NULL THEN @AccessLevel ELSE [AccessLevel] END
	WHERE [UserName] = @UserName
