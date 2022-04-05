-- Update User Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateUser]
	@UserName NVARCHAR(15),
	@PasswordHash BINARY(64) = 0x00,
	@AccessLevel INT = -1
AS 

UPDATE [UpdateUser]
	SET
		[PasswordHash] = IFF(@PasswordHash > 0x00, [PasswordHash], @PasswordHash),
		[AccessLevel] = IFF(@AccessLevel > -1, [AccessLevel], @PasswordHash)
	WHERE [UserName] = @UserName
