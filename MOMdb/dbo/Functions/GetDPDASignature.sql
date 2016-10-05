CREATE  FUNCTION [dbo].[GetDPDASignature] ( @Ticket int, @Workid int)
RETURNS VARBINARY(max)
begin
declare @Count VARBINARY(max)
declare @text as varchar(max)
declare @sign table([Signature] image) 
set @text = 'insert into @sign  SELECT TOP 1 signature FROM  PDA_'+convert(varchar(100), @Workid)+' WHERE  pdaticketid ='+convert(varchar(100), @Ticket)+''
EXEC sp_executeSQL @text
select @Count = CONVERT(VARBINARY, [Signature]) from @sign
--RETURN CAST(N'' AS XML).value('xs:base64Binary(xs:hexBinary(sql:variable("@Count")))','VARCHAR(MAX)')
return @Count
end
