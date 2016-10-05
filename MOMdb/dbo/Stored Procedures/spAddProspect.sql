CREATE procedure [dbo].[spAddProspect]
@Name varchar(75),
@address varchar(255),
@City varchar(50),
@State varchar(2),
@zip varchar(10),
@phone varchar(28),
@contact varchar(50),
@remarks text,
@type varchar(20),
@Status smallint,
@cell varchar(28),
@Email varchar(50),
@CustomerName varchar(50),
@SalesPerson int,
@BillAddress varchar(255),
@BillCity varchar(50),
@BillState varchar(2),
@Billzip varchar(10),
@Billphone varchar(28),
@Fax varchar(28),
@Website varchar(50),
@Lat varchar(100),
@Lng varchar(100),
@ContactData As [dbo].[tblTypeContact] Readonly,
@UpdateUser varchar(50),
@Source varchar(50)


as
declare @DucplicateProspectName int
DECLARE @ProspectID INT
declare @Rol int



select @DucplicateProspectName = COUNT(1) from Rol r inner join Prospect p on p.Rol=r.ID where Name = @Name 

				if(@DucplicateProspectName = 0)
				begin
            
            BEGIN TRANSACTION
                SELECT @ProspectID = isnull(Max(ID) ,0) + 1
                FROM   Prospect
				--select @Rol= MAX(ID)+1 from Rol

                INSERT INTO Rol
                            (
                            --ID,
                            Name,
                             Address,
                             City,
                             State,
                             Zip,
                             Phone,
                             Contact,
                             Remarks,
                             Type,
                             GeoLock,
                             fLong,
                             Latt,
                             Since,
                             Last,
                             EN,
                             Cellular,
                             Country,
                             EMail,
                             Fax,
                             Website,
                             Lat,Lng)
                VALUES      (
                 --@Rol,
								@Name,
                              @Address,
                              @City,
                              @State,
                              @zip,
                              @phone,
                              @contact,
                              @Remarks,
                              3,
                              0,
                              0,
                              0,
                              Getdate(),
                              Getdate(),
                              1,
                              @Cell,
                              'United States',
                              @Email,
                              @Fax,
                              @Website,
                              @Lat,@Lng )

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END

                SET @Rol=Scope_identity()

                INSERT INTO Prospect
                            (ID,
                             Rol,
                             Type,
                             Level,
                             Status,
                             LDate,
                             LTime,
                             Program,
                             NDate,
                             PriceL,
                             CustomerName,
                             Terr,
                             address,
                             city,
                             state,
                             zip,
                             phone,
                              CreateDate,
                       CreatedBy,
                       LastUpdateDate,
                       LastUpdatedBy,
                       [Source]                       
                             )
                VALUES      ( @ProspectID,
                              @Rol,
                              @type,
                              1,
                              0,
                              Getdate(),
                              Cast(Cast('12/30/1899' AS DATE) AS DATETIME)
                                                            + Cast(cast(Getdate() as time)AS datetime),
                              0,
                              Getdate(),
                              0,
                              case rtrim(ltrim(@CustomerName)) when '' then @Name else @CustomerName end,
                              @SalesPerson,
                              @BillAddress,
                              @BillCity,
                              @BillState,
                              @Billzip,
                              @Billphone,
                              GETDATE(),
                        @UpdateUser ,
                         GETDATE(),
                        @UpdateUser,
                        @Source                       
                        )

                IF @@ERROR <> 0
                   AND @@TRANCOUNT > 0
                  BEGIN
                      RAISERROR ('Error Occured',16,1)

                      ROLLBACK TRANSACTION

                      RETURN
                  END     
                  
                  update PType set [Count] = [Count]+1 where [Type] =@type
                           
                           
                insert into Phone
				 (
				 Rol,
				 fDesc,
				 Phone,
				 Fax, 
				 Cell,
				 Email
				 )
				 select @Rol,name,Phone,fax,cell,email from @ContactData
				 
				 IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				 BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
					RETURN
				 END            
                                  
                 if not exists(select 1 from Phone where Rol =@Rol and fDesc = @contact)
                 begin 
                 insert into Phone
				 (
				 Rol,fDesc,Phone,Fax,Cell,Email
				 )
				 values
				 (
				 @Rol,@contact,@phone,@Fax,@cell,@Email
				 )
                 end                 
                      
                IF @@ERROR <> 0 AND @@TRANCOUNT > 0
				 BEGIN  
					RAISERROR ('Error Occured', 16, 1)  
					ROLLBACK TRANSACTION    
					RETURN
				 END                 
                                       
                   COMMIT TRANSACTION 
                  end
                  else
                  begin
                   RAISERROR ('Prospect name already exists, please use different Prospect name !', 16, 1)  
				 RETURN

				end

