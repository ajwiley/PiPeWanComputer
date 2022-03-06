-- Update User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateUser]
	@UserID INT,
	@PasswordHash BINARY(64),
	@UserName NVARCHAR(128)
AS 

IF NOT EXISTS (SELECT * FROM [User] WHERE [PasswordHash] = @PasswordHash)
BEGIN
	Update [User]
	SET
		PasswordHash = @PasswordHash
	WHERE [UserID] = @UserID
END

IF NOT EXISTS (SELECT * FROM [User] WHERE [UserName] = @UserName)
BEGIN
	Update [User]
	SET
		[UserName] = @UserName
	WHERE [UserID] = @UserID
END
