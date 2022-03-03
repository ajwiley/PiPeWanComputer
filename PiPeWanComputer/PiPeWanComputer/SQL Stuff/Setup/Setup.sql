DROP DATABASE IF EXISTS PipeWan

CREATE DATABASE PipeWan ON PRIMARY
(
	NAME = PipeWanData,
	FILENAME = 'C:\Users\mrd\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\\PipeWan.mdf',
	SIZE = 2MB, MAXSIZE = 10MB, FILEGROWTH = 10%)
LOG ON 
(
	NAME = PipeWanLog,
	FILENAME = 'C:\Users\mrd\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\\PipeWanLog.ldf',
	SIZE = 1MB,
	MAXSIZE = 5MB,
	FILEGROWTH = 10%
)