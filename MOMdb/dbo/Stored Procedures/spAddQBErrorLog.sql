CREATE proc [dbo].[spAddQBErrorLog]
@API	varchar(50),
@requestID	varchar(50),
@StatusCode	varchar(50),
@statusSeverity	varchar(50),
@statusMessage	varchar(250)
as

--delete from tblQBResponseLog where DateTime < dateadd(day,-3,GETDATE())

--if exists (select 1 from tblQBResponseLog 
--where 
--API=@API 
--and requestID=@requestID 
--and StatusCode=@StatusCode 
--and statusSeverity=@statusSeverity 
--and statusMessage=@statusMessage)
--begin
--update tblQBresponselog set datetime = getdate() where API=@API 
--and requestID=@requestID 
--and StatusCode=@StatusCode 
--and statusSeverity=@statusSeverity 
--and statusMessage=@statusMessage
--end
--else
--begin
--insert into tblQBresponselog
--(
--API,
--requestID,
--StatusCode,
--statusSeverity,
--statusMessage,
--DateTime
--)
--values
--(
--@API,
--@requestID,
--@StatusCode,
--@statusSeverity,
--@statusMessage,
--GETDATE()
--)
--end
