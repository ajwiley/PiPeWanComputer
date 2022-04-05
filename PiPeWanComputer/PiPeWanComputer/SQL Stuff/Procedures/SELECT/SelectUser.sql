-- Select Cart Procedure  --
USE PipeWan;
GO

CREATE OR ALTER PROCEDURE [dbo].[SelectUser]
	@UserName NVARCHAR(15) = ''
AS 

SELECT *
FROM [User]
WHERE [UserName] = @UserName OR @UserName = ''

-- Return only the user with the given user name or
-- return all users if the procedure is called with an empty string, ''

