CREATE FUNCTION [dbo].[GetSignature] ( @Ticket int, @Workid int)
RETURNS int  
BEGIN
declare @Count int
declare @text as varchar(max)
declare @sign table(Counts int)

set @text = 'insert into @sign SELECT TOP 1 COUNT(1) FROM   PDA_'+convert(varchar(100), @Workid)+' WHERE  pdaticketid ='+convert(varchar(100), @Ticket)+''

--insert into @sign
exec @text

select @Count = Counts from @sign

return @Count

END