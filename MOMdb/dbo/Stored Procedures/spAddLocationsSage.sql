CREATE proc [dbo].[spAddLocationsSage]
as
SELECT (SELECT SageID 
        FROM   Owner 
        WHERE  ID = l.Owner)                                            AS sagecustomerid, 
        SageID	, 
       --dbo.createSageJobID(NEWID()) as job,
       substring(l.ID,1, 11)											AS job,
       Substring(tag, 1, 30)                                            AS tag, 
       Loc                                                              AS ID, 
       l.Remarks, 
       CASE l.Status 
         WHEN 0 THEN 'Active' 
         ELSE 'Inactive' 
       END                                                              AS Status, 
       
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(l.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 1), 1, 30) AS Address1, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(l.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 2), 1, 30) AS Address2, 
                  
		Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 1), 1, 30) AS billAddress1, 
       Substring((SELECT ltrim(rtrim(replace(replace(items,Char(10),''),CHAR(13),''))) 
                  FROM   dbo.Idsplit(r.Address, Char(10) + Char(13)) spl 
                  WHERE  spl.row = 2), 1, 30) AS billAddress2, 
       
       --(SELECT items 
       -- FROM   dbo.Splitsentence(Isnull(l.Address, ''), 30) spl 
       -- WHERE  spl.id = 1)                                              AS Address1, 
       --(SELECT items 
       -- FROM   dbo.Splitsentence(Isnull(l.Address, ''), 30) spl 
       -- WHERE  spl.id = 2)                                              AS Address2, 
       Substring(l.City, 1, 15)                                         AS City, 
       Substring(l.State, 1, 4)                                         AS State, 
       Substring(l.Zip, 1, 10)                                          AS Zip, Substring(l.Type, 1, 15) as type ,
       Substring(r.Contact,1,30) as contact,
       Substring(r.City,1,15) as billcity,
		Substring(r.State, 1, 4)                                         AS BillState,
		Substring(r.Zip, 1, 10)                                          AS billZip,
		--(SELECT items 
  --      FROM   dbo.Splitsentence(Isnull(r.Address, ''), 30) spl 
  --      WHERE  spl.id = 1)                                              AS billAddress1, 
  --     (SELECT items 
  --      FROM   dbo.Splitsentence(Isnull(r.Address, ''), 30) spl 
  --      WHERE  spl.id = 2)                                              AS billAddress2, 
		Substring(r.Phone,1,15) as phone,
		r.Website,
		r.EMail,
		Substring(r.Cellular,1,15) as Cellular,
		Substring(r.Fax,1,15) as Fax,
		l.Owner,
		Substring(Custom14,1,50) as Custom14,
		case substring(l.ID,9, 11) 
		when '-10' then '1-10' 
		when '-20' then '1-20' 
		when '-30' then '1-30' 
		when '-40' then '1-30' 
		end as costaccountprefix
FROM   Loc l inner join Rol r on r.ID=l.Rol
WHERE  
--SageID IS NULL 
isnull(SageID,'NA') = 'NA'
