CREATE function dbo.createSageJobID( @uniqueid varchar(150))
returns varchar(150)
as 
begin
declare @jid varchar(150)
set @jid = Replace(CONVERT(VARCHAR(150), @uniqueid), '-', '')
select @jid=
(SUBSTRING(CONVERT(VARCHAR(50), @jid), 1, 2)
+'-'+
SUBSTRING(CONVERT(VARCHAR(50), @jid), 3, 5)
+'-'+
SUBSTRING(CONVERT(VARCHAR(50), @jid), 8, 2) )
return @jid
end
