CREATE PROCEDURE [dbo].[spCreateDB]
@DbName varchar(50)
as
declare @Text varchar(max)

set @Text='
CREATE DATABASE ['+@DbName+'] ON  PRIMARY 
( NAME = N'''+@DbName+''', FILENAME = N''d:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER8\MSSQL\DATA\'+@DbName+'.mdf'' , SIZE = 12288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'''+@DbName+'_log'', FILENAME = N''d:\Program Files\Microsoft SQL Server\MSSQL10.MSSQLSERVER8\MSSQL\DATA\'+@DbName+'.ldf'' , SIZE = 1280KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
'

exec (@Text)
