CREATE PROCEDURE [dbo].[Spaddcontracttemp] @loc        VARCHAR(150),
                                           @owner      INT,
                                           @date       DATETIME,
                                           @Status     INT,
                                           @Creditcard INT,
                                           @Remarks    TEXT,
                                           @BStart     DATETIME,
                                           @Bcycle     VARCHAR(50),
                                           @BAmt       NUMERIC(30, 2),
                                           @SStart     DATETIME,
                                           @Cycle      VARCHAR(50),
                                           @SWE        INT,
                                           @Stime      DATETIME,
                                           @Sday       INT,
                                           @SDate      INT,

                                           @ElevJobData As [dbo].[tblTypeJoinElevJob] Readonly,
                                           @Route      VARCHAR(75),
                                           @hours      NUMERIC(30, 2),
                                           @fdesc      VARCHAR(75),
                                           @CType      VARCHAR(15)
AS
    DECLARE @Job INT

    BEGIN TRANSACTION

    INSERT INTO job
                (Loc,
                 Owner,
                 fDate,
                 Status,
                 CreditCard,
                 Remarks,
                 Rev,
                 Mat,
                 Labor,
                 Cost,
                 Profit,
                 Ratio,
                 Reg,
                 OT,
                 DT,
                 TT,
                 Hour,
                 BRev,
                 BMat,
                 BLabor,
                 BCost,
                 BProfit,
                 BRatio,
                 BHour,
                 Comm,
                 BillRate,
                 NT,
                 Amount,
                 Custom20,
                 Type,
                 fDesc,
                 CType)
    VALUES      ( (SELECT Loc
                   FROM   Loc
                   WHERE  Tag = @loc),
                  (SELECT owner
                   FROM   loc
                   WHERE  Tag = @loc),
                  @date,
                  @Status,
                  @Creditcard,
                  @Remarks,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  0.00,
                  @route,
                  0,
                  @fdesc,
                  @CType )

    SET @Job=Scope_identity()

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    INSERT INTO Contract
                (Job,
                 BStart,
                 BCycle,
                 BAmt,
                 SStart,
                 SCycle,
                 SWE,
                 STime,
                 SDay,
                 SDate,
                 Loc,
                 Owner,
                 Hours)
    VALUES      ( @Job,
                  @BStart,
                  CASE @Bcycle
                    WHEN 'Monthly'THEN 0
                    WHEN 'Quarterly'THEN 2
                    WHEN 'Semi-Annual'THEN 4
                    WHEN 'Annual'THEN 5
                  END,
                  @BAmt,
                  @SStart,
                  CASE @Cycle
                    WHEN 'Monthly'THEN 0
                    WHEN 'Quarterly'THEN 2
                    WHEN 'Semi-Annual'THEN 4
                    WHEN 'Annual'THEN 5
                  END,
                  @SWE,
                  @Stime,
                  @Sday,
                  @SDate,
                  (SELECT Loc
                   FROM   Loc
                   WHERE  Tag = @loc),
                  (SELECT owner
                   FROM   loc
                   WHERE  Tag = @loc),
                  @hours )

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

    --insert into tbljoinElevJob
    --(
    --Job, elev,price,Hours
    --)
    --select @Job,Elevunit,price,hours from @ElevJobData
    --IF @@ERROR <> 0 AND @@TRANCOUNT > 0
    --BEGIN  
    --RAISERROR ('Error Occured', 16, 1)  
    --   ROLLBACK TRANSACTION    
    --   RETURN
    --END
    COMMIT TRANSACTION
