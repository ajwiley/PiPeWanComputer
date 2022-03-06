-- Add User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[AddUser]
	@UserName NVARCHAR(128),
	@PasswordHash BINARY(64)
AS 

DECLARE @Response NVARCHAR(128) = ''

IF NOT EXISTS (SELECT * FROM [User] WHERE [UserName] = @UserName OR [PasswordHash] = @PasswordHash)
BEGIN
	INSERT INTO [User] ([UserName], [PasswordHash])
	VALUES (@UserName, @PasswordHash)

	SET @Response = 'Success'

	SELECT @Response [Response], SCOPE_IDENTITY() [UserID]
END
ELSE
BEGIN
	SET @Response = 'Cannot have duplicate value for barcode or User name'

	SELECT @Response [Response], 0 [UserID]
END


