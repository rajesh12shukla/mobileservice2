using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_Report
    {
        //public DataSet GetChartByAcctType(Chart _objChart)
        //{
        //    try
        //    {
        //        return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, "SELECT ID, Acct, fDesc, Balance, Type, Sub, Remarks, Control, InUse, Detail, CAlias, Status, Sub2, DAT, Branch, CostCenter, AcctRoot, QBAccountID, LastUpdateDate FROM Chart WHERE Balance <> '0.00' AND Type = " + _objChart.Type + " Order by ID");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetTypeForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            try
            {
                //StringBuilder varname1 = new StringBuilder();
                //varname1.Append("SELECT Type AS ID,  \n");
                //varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                //varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                //varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                //varname1.Append("     WHEN 3 THEN 'Revenue' \n");
                //varname1.Append("     WHEN 4 THEN 'Cost' \n");
                //varname1.Append("     WHEN 5 THEN 'Expense' \n");
                //varname1.Append("     WHEN 6 THEN 'Bank' \n");
                //varname1.Append("     END) AS fDesc FROM Chart GROUP BY Type \n");

                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE Type < 3  \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetDataForBalanceSheet(Chart _objChart) //For Balance sheet
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT  \n");
                varname1.Append("	 c.fDesc, \n");
                varname1.Append("    t.Acct, \n");
                varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                varname1.Append("    '" + _objChart.Type + "' AS Type  \n");
                varname1.Append("    FROM Trans as t, Chart as c \n");
                varname1.Append("    WHERE c.ID = t.Acct \n");
                //varname1.Append("    AND c.Type < 3  \n");
                varname1.Append("    AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "' ");
                varname1.Append("    GROUP BY t.Acct, c.fDesc \n");
                varname1.Append("    HAVING SUM(t.Amount) <> 0.00 \n");
                varname1.Append("    ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTypeForTrialBalance(Chart _objChart) //For Trial Balance
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("     WHEN 1 THEN 'Liability'  \n");
                varname1.Append("     WHEN 2 THEN 'Equity'  \n");
                varname1.Append("     WHEN 3 THEN 'Revenue'  \n");
                varname1.Append("     WHEN 5 THEN 'Expense'  \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE Type < 4 OR Type = 5 \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTypeForIncome(Chart _objChart) //For Income Statement
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT Type AS ID,  \n");
                varname1.Append("     (CASE Type WHEN 3 THEN 'Revenue' \n");
                varname1.Append("     WHEN 5 THEN 'Expense' \n");
                varname1.Append("     END) AS fDesc FROM Chart \n");
                varname1.Append("     WHERE  \n");
                //varname1.Append("     Type = 1 OR \n");
                varname1.Append("     Type = 3 OR Type = 5 \n");
                varname1.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetIncomeTotal(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT Type AS ID,  \n");
                varname.Append("     (CASE Type WHEN 3 THEN 'Revenue' \n");
                varname.Append("     WHEN 5 THEN 'Expense' \n");
                varname.Append("     END) AS fDesc FROM Chart \n");
                varname.Append("     WHERE  \n");
                varname.Append("     Type = 3 OR Type = 5 \n");
                varname.Append("     GROUP BY Type \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSubCategory(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT Sub \n");
                varname.Append("FROM Chart \n");
                varname.Append("WHERE Type = "+_objChart.Type+" \n");
                varname.Append("GROUP BY Sub \n");
                varname.Append("HAVING Sub <> '' \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAcctDetailsBySubCat(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append("c.fDesc, t.Acct, \n");
                varname.Append("SUM(t.Amount) AS Balance, \n");
                varname.Append("'" + _objChart.Type + "' AS Type \n");
                varname.Append("FROM Trans AS t, Chart AS c \n");
                varname.Append("WHERE c.ID = t.Acct AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "'  \n");
                varname.Append("AND c.Sub Like '" + _objChart.Sub + "' \n");
                varname.Append("GROUP BY t.Acct, c.fDesc \n");
                varname.Append("HAVING SUM(t.Amount) <> 0.00 \n");
                varname.Append("ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOtherAcctDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append("c.fDesc, t.Acct, \n");
                varname.Append("SUM(t.Amount) AS Balance, \n");
                varname.Append("'" + _objChart.Type + "' AS Type \n");
                varname.Append("FROM Trans AS t, Chart AS c \n");
                varname.Append("WHERE c.ID = t.Acct AND c.Type = " + _objChart.Type + " AND t.fDate > '" + _objChart.StartDate.Date + "' AND t.fDate < '" + _objChart.EndDate.Date + "'  \n");
                varname.Append("AND (c.Sub IS NULL OR c.Sub = '') \n");
                varname.Append("GROUP BY t.Acct, c.fDesc \n");
                varname.Append("HAVING SUM(t.Amount) <> 0.00 \n");
                varname.Append("ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetFiscalYearData(User _objPropUser)
        {
            try
            {
                return _objPropUser.YE = Convert.ToInt32(SqlHelper.ExecuteScalar(_objPropUser.ConnConfig, CommandType.Text, "SELECT ISNULL(YE,'') FROM Control"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBalanceSheetDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("   SELECT t.Acct, t.fDesc, sum(isnull(t.Amount,0)) as Amount, t.Type, t.TypeName, t.Sub, t.Url  from( ");
                varname1.Append("SELECT                         \n");
                varname1.Append("	 c.Acct+'  '+c.fDesc AS fDesc, \n");
                varname1.Append("    c.ID As Acct,                    \n");
                //varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                //varname1.Append("    t.Amount,                  \n");
                varname1.Append("    (CASE c.Type WHEN 1 THEN   \n");
                varname1.Append("       (isnull(t.Amount,0) * -1)         \n");       //change by Mayuri on 16th Sep,16 to show Liability amount 
                varname1.Append("       WHEN 2 THEN (isnull(t.Amount,0) * -1)         \n"); 
                varname1.Append("       ELSE isnull(t.Amount,0) END) as Amount, \n");
                varname1.Append("   case c.Type when 6 then 0 else c.Type end as Type,    \n");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset' \n");
                varname1.Append("    WHEN 1 THEN 'Liability'    \n");
                varname1.Append("    WHEN 2 THEN 'Equity'       \n");
                varname1.Append("    WHEN 3 THEN 'Revenue'      \n");
                varname1.Append("    WHEN 4 THEN 'Cost'         \n");
                varname1.Append("    WHEN 5 THEN 'Expense'      \n");
                varname1.Append("    WHEN 6 THEN 'Asset'  --Bank       \n");        //change to show bank account under Asset type accounts
                varname1.Append("    END) AS TypeName,          \n");
                varname1.Append("     (CASE c.Sub WHEN '' THEN  \n");
                varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'  \n");
                varname1.Append("         WHEN 1 THEN 'Liability'   \n");
                varname1.Append("         WHEN 2 THEN 'Equity'      \n");
                varname1.Append("         WHEN 3 THEN 'Revenue'     \n");
                varname1.Append("         WHEN 4 THEN 'Cost'        \n");
                varname1.Append("         WHEN 5 THEN 'Expense'     \n");
                varname1.Append("         WHEN 6 THEN 'Bank'        \n");
                varname1.Append("   END)                            \n");
                varname1.Append("   ELSE c.Sub END) As Sub, '' as Url   \n");
                varname1.Append("    FROM Trans as t RIGHT JOIN Chart as c  ON c.ID = t.Acct  \n");
                varname1.Append("    WHERE (c.Type < 3 OR c.Type = 6) AND t.Amount <> 0.00    \n");
                varname1.Append("    AND t.fDate <= '" + _objChart.EndDate.Date + "'    \n");
                //varname1.Append("    AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "' \n");
                varname1.Append("      ) t  \n");
                varname1.Append("       GROUP BY t.Acct, t.fDesc, t.Type, t.TypeName, t.Sub, t.Url  \n");
                varname1.Append("       HAVING Sum(isnull(Amount,0)) <> 0       \n");
                varname1.Append("       ORDER BY t.Acct     \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetIncomeStatementDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT             \n");
                varname1.Append("	 c.Acct +'  '+c.fDesc AS fDesc, \n");
                varname1.Append("    t.Acct,                        \n");
                //varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                //varname1.Append("    t.Amount,  \n");
                //varname1.Append("    '" + _objChart.Type + "' AS Type,  \n");
                varname1.Append("   CASE c.Type                         \n");
	            varname1.Append("       WHEN 3 THEN (t.Amount * -1)     \n");
                varname1.Append("       ELSE t.Amount END As Amount,    \n");
                varname1.Append("   c.Type AS Type,                     \n");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset'    \n");
                varname1.Append("    WHEN 1 THEN 'Liability'            \n");
                varname1.Append("    WHEN 2 THEN 'Equity'               \n");
                varname1.Append("    WHEN 3 THEN 'Revenue'              \n");
                varname1.Append("    WHEN 4 THEN 'Cost of Sales'        \n");
                varname1.Append("    WHEN 5 THEN 'Expense'              \n");
                varname1.Append("    WHEN 6 THEN 'Bank'                 \n");
                varname1.Append("    END) AS TypeName,                  \n");
                varname1.Append("     (CASE c.Sub WHEN '' THEN          \n");
                varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'        \n");
                varname1.Append("         WHEN 1 THEN 'Liability'       \n");
                varname1.Append("         WHEN 2 THEN 'Equity'          \n");
                varname1.Append("         WHEN 3 THEN 'Revenue'         \n");
                varname1.Append("         WHEN 4 THEN 'Cost of Sales'   \n");
                varname1.Append("         WHEN 5 THEN 'Expense'         \n");
                varname1.Append("         WHEN 6 THEN 'Bank'            \n");
                varname1.Append("   END)            \n");
                varname1.Append("   ELSE c.Sub END) As Sub      \n");
                //varname1.Append("   (CASE c.Sub WHEN '' THEN 'Asset' ELSE c.Sub END) As Sub \n");
                varname1.Append("    FROM Trans as t INNER JOIN Chart as c    \n");
                varname1.Append("    ON c.ID = t.Acct            \n");
                varname1.Append("    WHERE c.Type IN (3, 4, 5)   \n");
                varname1.Append("    AND t.Amount <> 0       \n");
                varname1.Append("    AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "'  \n");
                //varname1.Append("    GROUP BY t.Acct, c.fDesc \n");
                //varname1.Append("    HAVING SUM(t.Amount) <> 0.00 \n");
                varname1.Append("    ORDER BY t.Acct        \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetTrialBalanceDetails(Chart _objChart)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT  \n");
                varname1.Append("	 c.Acct +'  '+c.fDesc AS fDesc, \n");
                varname1.Append("    t.Acct, \n");
                //varname1.Append("    SUM(t.Amount) AS Balance,  \n");
                //varname1.Append("   (CASE WHEN t.Amount > 0 THEN t.Amount ELSE '0.00' END) AS DebitAmount,");
                //varname1.Append("   (CASE WHEN t.Amount < 0 THEN t.Amount * -1 ELSE '0.00' END) AS CreditAmount,");
                varname1.Append("    t.Amount,  \n");
                //varname1.Append("    '" + _objChart.Type + "' AS Type,  \n");
                varname1.Append("   c.Type AS Type,");
                varname1.Append("   (CASE c.Type WHEN 0 THEN 'Asset' ");
                varname1.Append("    WHEN 1 THEN 'Liability' ");
                varname1.Append("    WHEN 2 THEN 'Equity' ");
                varname1.Append("    WHEN 3 THEN 'Revenue' ");
                varname1.Append("    WHEN 4 THEN 'Cost of Sales' \n");
                varname1.Append("    WHEN 5 THEN 'Expense'  ");
                varname1.Append("    WHEN 6 THEN 'Bank' \n");
                varname1.Append("    END) AS TypeName, ");
                varname1.Append("     (CASE c.Sub WHEN '' THEN  ");
                varname1.Append("     (CASE c.Type WHEN 0 THEN 'Asset'  ");
                varname1.Append("         WHEN 1 THEN 'Liability' ");
                varname1.Append("         WHEN 2 THEN 'Equity' ");
                varname1.Append("         WHEN 3 THEN 'Revenue' ");
                varname1.Append("         WHEN 4 THEN 'Cost of Sales' \n");
                varname1.Append("         WHEN 5 THEN 'Expense' ");
                varname1.Append("         WHEN 6 THEN 'Bank' \n");
                varname1.Append("   END)");
                varname1.Append("   ELSE c.Sub END) As Sub");
                //varname1.Append("   (CASE c.Sub WHEN '' THEN 'Asset' ELSE c.Sub END) As Sub \n");
                varname1.Append("    FROM Trans as t, Chart as c \n");
                varname1.Append("    WHERE c.ID = t.Acct \n");
                varname1.Append("    AND (c.Type <= 4 OR c.Type = 5)  ");
                varname1.Append("    AND t.Amount <> 0.00 ");
                //varname1.Append("    AND c.Type < 3  AND c.Type = " + _objChart.Type + " \n");
                varname1.Append("    AND t.fDate >= '" + _objChart.StartDate.Date + "' AND t.fDate <= '" + _objChart.EndDate.Date + "' ");
                //varname1.Append("    GROUP BY t.Acct, c.fDesc \n");
                //varname1.Append("    HAVING SUM(t.Amount) <> 0.00 \n");
                varname1.Append("    ORDER BY t.Acct \n");
                return _objChart.Ds = SqlHelper.ExecuteDataset(_objChart.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetPeriodClosedYear(Journal _objJournal)
        //{
        //    try
        //    {
        //        StringBuilder varname = new StringBuilder();
        //        varname.Append("    SELECT fDate  \n");
        //        varname.Append("    FROM Trans  \n");
        //        varname.Append("    WHERE fDesc LIKE '%Year-end Transfer%'  \n");
        //        varname.Append("    GROUP BY fDate ");
        //        return _objJournal.DsTrans = SqlHelper.ExecuteDataset(_objJournal.ConnConfig, CommandType.Text, varname.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public DataSet GetPeriodClosedYear(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT fUser, fStart, fEnd FROM tblUser WHERE fUser ='"+objPropUser.Username+"'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetIncomestatementBalance(Chart objChart)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("    select t.Type, t.TypeName, sum(t.NAmt) as NAmt from     \n");
                varname.Append("        (select t.ID, c.Type, case c.type when 3 then 'Revenue' when 4 then 'Cost of sales' when 5 then 'Expenses' end as typename,     \n");
                varname.Append("        case c.type when 3 then (isnull(amount,0) * -1) else isnull(amount,0) end as NAmt       \n");
                varname.Append("        from trans t inner join chart c on c.ID = t.Acct                                        \n");
                varname.Append("            where c.type in (3,4,5) and t.fdate <= '"+ objChart.EndDate +"') as t   \n");
                varname.Append("            group by t.Type, t.TypeName     \n");
                return objChart.Ds = SqlHelper.ExecuteDataset(objChart.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //Rahil's Implement

        public DataSet GetPurchaseJournal(OpenAP _objOpenAP)
        {
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT      p.ID,                   \n");
                varname1.Append("	 p.fDate as Post, \n");
                varname1.Append("    o.Due,p.Ref,p.fDesc,p.Vendor,                 \n");
                varname1.Append("  r.Name AS VendorName,  \n");
                varname1.Append("      DATEPART(wk, o.Due) as WeekCount,        \n");
                varname1.Append("      DATEADD(dd, -1, DATEADD(wk,DATEDIFF(wk,0,o.Due),0)) as WeekDate,        \n");
                varname1.Append("     isnull(p.Amount,0) as Amount, \n");
                varname1.Append("   p.Status,    \n");
                varname1.Append("  (CASE p.Status WHEN 0 THEN 'Open'        \n");
                varname1.Append("   WHEN 1 THEN 'Closed'           \n");
                varname1.Append("    WHEN 2 THEN 'Void'  END) AS StatusName       \n");
                varname1.Append("   FROM PJ AS p            \n");
                varname1.Append("   inner join Vendor AS v on p.Vendor = v.ID             \n");
                varname1.Append("    inner join Rol AS r on v.Rol = r.ID        \n");
                varname1.Append("   left join openAP AS o on p.ID = o.PJID        \n");       
                varname1.Append("    WHERE  o.Balance<>0 AND o.Original<>o.Selected         \n");
                varname1.Append("    AND o.Due <= '" + _objOpenAP.Due.Date + "'    \n");
                varname1.Append("    ORDER BY o.Due \n");             
                return _objOpenAP.Ds = SqlHelper.ExecuteDataset(_objOpenAP.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
    }
}
