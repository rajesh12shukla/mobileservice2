CREATE PROC [dbo].[AddQbJobType] @QBJobTypeID    VARCHAR(100),
                         @Type           VARCHAR(50),
                         @remarks        VARCHAR(255),
                         @LastUpdateDate DATETIME
AS
    IF NOT EXISTS(SELECT 1
                  FROM   JobType
                  WHERE  QBJobTypeID = @QBJobTypeID)
      BEGIN
          INSERT INTO JobType
                      (type,
                       remarks,
                       isdefault,
                       QBJobTypeID
                       --,
                       --LastUpdateDate
                       )
          VALUES      (@type,
                       @remarks,
                       0,
                       @QBJobTypeID
                       --,
                       --GETDATE()
                       )
      END
    ELSE
      BEGIN
          UPDATE JobType
          SET    Type = @type,
                 Remarks = @remarks
                 --,
                 --LastUpdateDate=GETDATE()
          WHERE  id = (SELECT TOP 1 id
                       FROM   jobtype
                       WHERE  QBJobTypeID = @QBJobTypeID)
                 AND ISNULL( LastUpdateDate, '01/01/1900' )< @LastUpdateDate
      END