CREATE PROCEDURE [dbo].[spCreatePDA_Userid]
@Userid int
as
declare @Text varchar(max)

set @Text='
if not exists(select table_name from information_schema.TABLES where table_name = ''PDA_'+CAST( @Userid as varchar(20) )+''')
begin
CREATE TABLE [dbo].[PDA_'+CAST( @Userid as varchar(20) )+'](	
	[PDATicketID] [int] NULL,
	[SignatureType] [varchar](1) NULL,
	[Signature] [image] NULL,
	[AID] [uniqueidentifier] NOT NULL
)
end 
'

exec (@Text)
