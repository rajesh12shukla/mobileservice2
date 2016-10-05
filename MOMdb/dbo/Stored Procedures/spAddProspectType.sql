CREATE PROCEDURE [dbo].[spAddProspectType] @Type    VARCHAR(15),
                                  @Remarks VARCHAR(8000),
                                  @mode    SMALLINT
AS
    IF ( @mode = 0 )
      BEGIN
          IF NOT EXISTS(SELECT 1
                        FROM   ptype
                        WHERE  type = @Type)
            BEGIN
                INSERT INTO ptype
                            (type,
                             remarks)
                VALUES      ( @Type,
                              @Remarks )
            END
          ELSE
            BEGIN
                RAISERROR ('Prospect type already exists.',16,1)

                RETURN
            END
      END
    ELSE IF( @mode = 1 )
      BEGIN
          UPDATE ptype
          SET    remarks = @Remarks
          WHERE  TYPE = @Type
      END
    ELSE IF ( @mode = 2 )
      BEGIN
          IF NOT EXISTS (SELECT 1
                         FROM   Prospect
                         WHERE  Type = @Type)
            BEGIN
                DELETE FROM ptype
                WHERE  type = @Type
            END
          ELSE
            BEGIN
                RAISERROR ('Prospect exists for the selected Prospect type.',16,1)

                RETURN
            END
      END