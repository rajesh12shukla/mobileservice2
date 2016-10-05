CREATE PROCEDURE [dbo].[Spdeletecontract] @Job INT
AS
    IF NOT EXISTS (SELECT 1
                   FROM   TicketO
                   WHERE  Job = @Job
                   UNION
                   SELECT 1
                   FROM   TicketD
                   WHERE  Job = @Job)
      BEGIN
          BEGIN TRANSACTION

          UPDATE Loc
          SET    Maint = 0
          WHERE  Loc = (SELECT Loc
                        FROM   Job
                        WHERE  ID = @Job)

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DELETE FROM Job
          WHERE  ID = @Job

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DELETE FROM Contract
          WHERE  Job = @Job

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          DELETE FROM tblJoinElevJob
          WHERE  Job = @Job

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          COMMIT TRANSACTION
      END
    ELSE
      BEGIN
          RAISERROR ('Tickets exists for the selected contract. Contract can not be deleted.',16,1)

          RETURN
      END