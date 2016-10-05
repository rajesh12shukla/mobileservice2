
CREATE PROC [dbo].[spAddOpportunity] @ID          INT,
                                    @fdesc       VARCHAR(75),
                                    @rol         INT,
                                    @Probability INT,
                                    @Status      SMALLINT,
                                    @Remarks     VARCHAR(max),
                                    @closedate   DATETIME,
                                    @Mode        SMALLINT,
                                    @owner int,
                                    @NextStep varchar(50),
                                    @desc varchar(max),
                                    @Source varchar(70),
                                    @Amount numeric(30,2),
                                    @Fuser varchar(50),
                                    @UpdateUser varchar(50),
                                    @closed smallint,
                                    @TicketID int
                                   
                                    
AS
    DECLARE @OppID INT

    SELECT @OppID = Isnull(Max(ID), 0) + 1
    FROM   Lead

    DECLARE @address VARCHAR(250)
    DECLARE @city VARCHAR(50)
    DECLARE @state CHAR(2)
    DECLARE @zip VARCHAR(28)
    Declare @RolType smallint

    SELECT @address = Address,
           @city = City,
           @state = State,
           @zip = Zip,
           @RolType=Type           
    FROM   Rol
    WHERE  ID = @rol

    IF ( @Mode = 0 )
      BEGIN
      if exists(select 1 from Lead where fDesc=@fdesc and Rol=@rol)
      begin
      raiserror('Opportunity with this name already exists for the Contact.',16,1)
      return
      end
          INSERT INTO Lead
                      (ID,
                       fDesc,
                       RolType,
                       Rol,
                       Type,
                       Address,
                       City,
                       State,
                       Zip,
                       Owner,
                       Status,
                       Probability,
                       Level,
                       Remarks,
                       closedate,
                       GeoLock,
                       NextStep,
                       [Desc],
                       [Source],
                       Revenue,
                       Cost,Labor,Profit,Ratio, fuser,
                       CreateDate,
                       CreatedBy,
                       LastUpdateDate,
                       LastUpdatedBy,
                       Closed,
                       TicketID
                       )
          VALUES      ( @OppID,
                        @fdesc,
                       case @RolType when 4 then 2 when 3 then 0 end,
                        @rol,
                        'General',
                        @address,
                        @city,
                        @state,
                        @zip,
                        case @RolType
						  when 4 then (select top 1 Loc from Loc where Rol = @rol)
						  when 3 then (select top 1 ID from Prospect where Rol = @rol)
						end,
                        @Status,
                        @Probability,
                        1,
                        @Remarks,
                        @closedate,
                        0 ,
                        @NextStep,
                        @desc,
                        @Source,
                        @Amount,0,0,@Amount,100, @Fuser,
                        GETDATE(),
                        @UpdateUser,
                         GETDATE(),
                        @UpdateUser,
                        @closed,
                        @TicketID
                        )
      END
    ELSE IF( @Mode = 1 )
      BEGIN
      
      if exists(select 1 from Lead where fDesc=@fdesc and Rol=@rol and ID<>@ID)
      begin
      raiserror('Opportunity with this name already exists for the Contact.',16,1)
      return      
      end
		  set @OppID= @ID
         
          UPDATE Lead
          SET    fDesc = @fdesc,
                 RolType = case @RolType when 4 then 2 when 3 then 0 end,
                 Rol = @rol,
                 Address = @address,
                 City = @city,
                 State = @state,
                 Zip = @zip,
                 Status = @Status,
                 Probability = @Probability,
                 Remarks = @Remarks,
                 closedate = @closedate,
                 NextStep=@NextStep,
                 LastUpdateDate=GETDATE(),
                 LastUpdatedBy=@UpdateUser,
                 [Desc]=@desc,
                 [Source]=@Source,
                 fuser=@Fuser,
                 closed=@closed,
                 Revenue=@Amount,
                 profit = (Revenue-(Cost+Labor)),
                 Ratio = case  Revenue when 0 then Ratio else (Profit/Revenue)*100 end,
                 Owner=(case @RolType
						  when 4 then (select top 1 Loc from Loc where Rol = @rol)
						  when 3 then (select top 1 ID from Prospect where Rol = @rol)
						end)
          WHERE  ID = @ID
      END 
      
      --if(@RolType=3)
      --begin
      --if(@closed=1)
      --begin
      
      --exec spConvertProspect @rol,@owner
            
      --end
      --end

select @OppID
