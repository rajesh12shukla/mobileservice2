
CREATE PROCEDURE [dbo].[Spaddpayment] @InvoiceID     INT,
                                     @TransDate     DATETIME,
                                     @CardNumber    VARCHAR(200),
                                     @Amount        MONEY,
                                     @response      VARCHAR(max),
                                     @refid         VARCHAR(500),
                                     @UserID        VARCHAR(50),
                                     @Screen        VARCHAR(15),
                                     @ResponseCodes VARCHAR(max),
                                     @Approved      VARCHAR(50),
                                     @IsSuccess     SMALLINT,
                                     @CustomerID    INT,
                                     @Status        SMALLINT,
                                     @PaymentUID    UNIQUEIDENTIFIER,
                                     @GatewayOrderID varchar(100)
AS
    BEGIN TRANSACTION

    INSERT INTO tblPaymentHistory
                (InvoiceID,
                 TransDate,
                 CardNumber,
                 Amount,
                 Response,
                 RefID,
                 UserID,
                 Screen,
                 Medium,
                 ResponseCodes,
                 Approved,
                 IsSuccess,
                 CustomerID,
                 PaymentUID,
                 GatewayOrderID)
    VALUES      (@InvoiceID,
                 @TransDate,
                 @CardNumber,
                 @Amount,
                 @response,
                 @refid,
                 @UserID,
                 @Screen,
                 'MSM',
                 @ResponseCodes,
                 @Approved,
                 @IsSuccess,
                 @CustomerID,
                 @PaymentUID,
                 @GatewayOrderID)

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END
	
	DECLARE @balance MONEY=0
    IF( @IsSuccess = 1 )
      BEGIN
          --DECLARE @balance MONEY
          DECLARE @paid SMALLINT = 0
          DECLARE @paidAmt MONEY=0

          SET @paidAmt= ( Isnull((SELECT balance FROM tblinvoicepayment WHERE ref=@InvoiceID), 0) + @Amount )
          SET @balance = (SELECT Isnull(total, 0)
                          FROM   Invoice
                          WHERE  Ref = @InvoiceID) - @paidAmt

          IF( @balance <= 0 )
            BEGIN
                SET @paid=1
            END

          IF( @Status != 5 )
            BEGIN
                UPDATE Invoice
                SET    Status = @paid,
                       --paid = 1,
                       Remarks = CONVERT(VARCHAR(max), Remarks)+ Char(13) + Char(10)+
                       CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + @CardNumber+ '    Amount $'+ CONVERT(VARCHAR(50), @Amount) ,
                       --+ Char(13) + Char(10)
                       --          + CONVERT(VARCHAR(max), Remarks),
                       fdesc = CONVERT(VARCHAR(max), fDesc)+ Char(13) + Char(10)+
                       CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + @CardNumber+ '    Amount $'+ CONVERT(VARCHAR(50), @Amount) 
                       --+ Char(13) + Char(10)
                       --        + CONVERT(VARCHAR(max), fDesc)
                WHERE  Ref = @InvoiceID
            END
          ELSE
            BEGIN
                UPDATE Invoice
                SET    --paid = 1,
                Remarks =  CONVERT(VARCHAR(max), Remarks) + Char(13) + Char(10) +
                 CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + @CardNumber + '    Amount $'+ CONVERT(VARCHAR(50), @Amount), 
                --+ Char(13) + Char(10)
                --          + CONVERT(VARCHAR(max), Remarks),
                fdesc = CONVERT(VARCHAR(max), fDesc)+ Char(13) + Char(10)+
                 CONVERT(VARCHAR(50), @TransDate)+ ', Paid by CC ' + @CardNumber + '    Amount $'+ CONVERT(VARCHAR(50), @Amount) 
                --+ Char(13) + Char(10)
                --        + CONVERT(VARCHAR(max), fDesc)
                WHERE  Ref = @InvoiceID
            END

          IF @@ERROR <> 0
             AND @@TRANCOUNT > 0
            BEGIN
                RAISERROR ('Error Occured',16,1)

                ROLLBACK TRANSACTION

                RETURN
            END

          IF NOT EXISTS(SELECT 1
                        FROM   tblinvoicepayment
                        WHERE  ref = @InvoiceID)
            BEGIN
                INSERT INTO tblinvoicepayment
                            (ref,
                             paid,
                             balance)
                VALUES      ( @invoiceid,
                              @paid,
                              @paidAmt)
            END
          ELSE
            BEGIN
                UPDATE tblinvoicepayment
                SET    paid = @paid,
                       balance = @paidAmt
                WHERE  ref = @InvoiceID
            END
      END

    IF @@ERROR <> 0
       AND @@TRANCOUNT > 0
      BEGIN
          RAISERROR ('Error Occured',16,1)

          ROLLBACK TRANSACTION

          RETURN
      END

	select CONVERT(numeric(30,2), @balance)
	
    COMMIT TRANSACTION
