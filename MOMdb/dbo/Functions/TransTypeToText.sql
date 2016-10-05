

CREATE FUNCTION [dbo].[TransTypeToText] (@idType INT) RETURNS VARCHAR(50) AS
BEGIN

RETURN CASE @idType
       WHEN 1 THEN 'AR Invoice'
       WHEN 2 THEN 'AR Invoice Line Item'
       WHEN 3 THEN 'AR Invoice Sales Tax'
       WHEN 4 THEN 'AR Invoice Inventory Item - Cost'
       WHEN 5 THEN 'Deposit'
       WHEN 6 THEN 'Deposit Line Items'
       WHEN 20 THEN 'AP Check - Credit (Neg)'
       WHEN 21 THEN 'AP Check - Debit (Pos)'
       WHEN 30 THEN 'Bank Adjustment'
       WHEN 31 THEN 'Bank Adjustment Line Item'
       WHEN 40 THEN 'AP Invoice'
       WHEN 41 THEN 'AP Invoice Line Items'
       WHEN 50 THEN 'GL Adjustment'
       WHEN 60 THEN 'Inv Adjustment - Asset side'
       WHEN 61 THEN 'Inv Adjustment - Expense side'
       WHEN 70 THEN 'Inventory Items on Tickets'
       WHEN 71 THEN 'Inventory Items on Tickets (Cost)'
       WHEN 90 THEN 'Payroll Check'
       WHEN 91 THEN 'Payroll Line Items'
       WHEN 97 THEN 'Inventory Transfer'
	   WHEN 98 THEN 'Received Payment'
	   WHEN 99 THEN 'Received Payment'
       ELSE 'UNKNOWN'

END

END