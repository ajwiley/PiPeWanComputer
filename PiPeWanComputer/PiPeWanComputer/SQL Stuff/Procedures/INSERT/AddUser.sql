-- Add User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[AddUser]
	@UserName NVARCHAR(128),
	@PasswordHash BINARY(64),
	@AccessLevel INT = 0
AS 

DECLARE @Response NVARCHAR(128) = ''

IF NOT EXISTS (SELECT * FROM [User] WHERE [UserName] = @UserName)
	BEGIN
		INSERT INTO [User] ([UserName], [PasswordHash], [AccessLevel])
		VALUES (@UserName, @PasswordHash, @AccessLevel)

		SET @Response = 'Success'

		SELECT @Response [Response], SCOPE_IDENTITY() [UserID]
	END
ELSE
	BEGIN
		SET @Response = 'Cannot have duplicate value for User name'

		SELECT @Response [Response], 0 [UserID]
	END


