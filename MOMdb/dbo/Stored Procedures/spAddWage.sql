CREATE proc [dbo].[spAddWage]
@Name varchar(50),
@Field int,
@Reg numeric(30,4),
@OT1 numeric(30,4),
@OT2 numeric(30,4),
@TT numeric(30,4),
@FIT smallint,
@FICA smallint, 
@MEDI smallint,
@FUTA smallint,
@SIT smallint,
@Vac smallint,
@WC smallint,
@Uni smallint,
@GL int,
@NT numeric(30,4),
@MileageGL int,
@ReimGL int,
@ZoneGL int,
@Globe smallint,
@Status smallint,
@CReg numeric(30,4),
@COT numeric(30,4),
@CDT numeric(30,4),
@CNT numeric(30,4),
@CTT numeric(30,4),
@Remarks varchar(8000),
@RegGL int,
@OTGL int,
@NTGL int,
@DTGL int,
@TTGL int
as

if not exists( select 1 from PRWage where fDesc=@Name)
begin

--insert into prwage
--(ID,fdesc,Remarks,status,Field,FIT,FICA,MEDI,FUTA,SIT,Vac,WC,Uni)
--values
--((select isnull(max(ID),0) +1 from prwage),@Name,@Remarks,0,1,1,1,1,1,1,1,1,1)

--SET IDENTITY_INSERT [PRWage] ON 


INSERT INTO [dbo].[PRWage]
           ([fDesc],[Field],[Reg],[OT1],[OT2],[TT],[FIT],[FICA],[MEDI],[FUTA]
           ,[SIT],[Vac],[WC],[Uni],[Remarks],[GL],[NT],[MileageGL],[ReimburseGL],[ZoneGL],[Globe],[Status],[CReg]
           ,[COT],[CDT],[CNT],[CTT],[RegGL],[OTGL],[NTGL],[DTGL],[TTGL])
     VALUES
           (@Name, @Field, @Reg, @OT1 ,@OT2 ,@TT ,@FIT ,@FICA ,@MEDI ,@FUTA 
           ,@SIT ,@Vac ,@WC ,@Uni ,@Remarks ,@GL ,@NT ,@MileageGL ,@ReimGL ,@ZoneGL ,@Globe ,@Status ,@CReg 
           ,@COT ,@CDT ,@CNT ,@CTT ,@RegGL ,@OTGL ,@NTGL ,@DTGL ,@TTGL)	  

--SET IDENTITY_INSERT [PRWage] OFF


end
else
BEGIN
  RAISERROR ('Wage Category already exists, please use different name !',16,1)
  RETURN
END
GO

