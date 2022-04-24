﻿CREATE TABLE dbo.[User]
(
	UserName NVARCHAR(15) PRIMARY KEY NOT NULL,
	PasswordHash BINARY(64) NOT NULL,
	AccessLevel INT NOT NULL DEFAULT 0
)