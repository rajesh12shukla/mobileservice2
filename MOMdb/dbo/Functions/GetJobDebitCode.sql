create  FUNCTION [dbo].[GetJobDebitCode]
(@strSageID VARCHAR(50))
RETURNS CHAR(2)
AS
BEGIN
set @strSageID =
case 
when SUBSTRING(@strSageID,LEN(@strSageID)-1,LEN(@strSageID)) in ('30','40') then '30'
when SUBSTRING(@strSageID,LEN(@strSageID)-1,LEN(@strSageID)) in ('10','20') then SUBSTRING(@strSageID,LEN(@strSageID)-1,LEN(@strSageID)) 
end
RETURN @strSageID
END
