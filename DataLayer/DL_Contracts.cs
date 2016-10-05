using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;

namespace DataLayer
{
    public class DL_Contracts
    {
        public DataSet getContractsData(Contracts objPropContracts)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT c.Job, \n");
            //varname1.Append("       j.ctype, \n");
            //varname1.Append("       j.fdesc, \n");
            //varname1.Append("       c.BAmt, \n");
            //varname1.Append("       c.Hours, \n");
            //varname1.Append("       l.ID                   AS locid, \n");
            //varname1.Append("       l.Tag, \n");
            //varname1.Append("       (SELECT TOP 1 name \n");
            //varname1.Append("        FROM   rol r \n");
            //varname1.Append("        WHERE  l.rol = r.id \n");
            //varname1.Append("               AND r.type = 4)AS name, \n");
            //varname1.Append("       Round (CASE c.BCycle \n");
            //varname1.Append("                WHEN 0 THEN c.BAmt \n");
            //varname1.Append("                WHEN 1 THEN c.BAmt / 6 \n");
            //varname1.Append("                WHEN 2 THEN c.BAmt / 4 \n");
            //varname1.Append("                WHEN 3 THEN c.BAmt / 3 \n");
            //varname1.Append("                WHEN 4 THEN c.BAmt / 2 \n");
            //varname1.Append("                WHEN 5 THEN c.BAmt / 12 \n");
            //varname1.Append("                WHEN 6 THEN 0 \n");
            //varname1.Append("              END, 2)         AS MonthlyBill, \n");
            //varname1.Append("       Round (CASE c.SCycle \n");
            //varname1.Append("                WHEN 0 THEN c.Hours \n");
            //varname1.Append("                WHEN 1 THEN c.Hours / 6 \n");
            //varname1.Append("                WHEN 2 THEN c.Hours / 4 \n");
            //varname1.Append("                WHEN 3 THEN c.Hours / 3 \n");
            //varname1.Append("                WHEN 4 THEN c.Hours / 2 \n");
            //varname1.Append("                WHEN 5 THEN c.Hours / 12 \n");
            //varname1.Append("                WHEN 6 THEN 0 \n");
            //varname1.Append("              END, 2)         AS MonthlyHours, \n");
            //varname1.Append("       CASE c.bcycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            //varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            //varname1.Append("         WHEN 4 THEN 'Semi-Anually' \n");
            //varname1.Append("         WHEN 5 THEN 'Anually' \n");
            //varname1.Append("         WHEN 6 THEN 'Never' \n");
            //varname1.Append("       END                    Freqency, \n");
            //varname1.Append("       CASE c.scycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            //varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            //varname1.Append("         WHEN 4 THEN 'Semi-Anually' \n");
            //varname1.Append("         WHEN 5 THEN 'Anually' \n");
            //varname1.Append("         WHEN 6 THEN 'Never' \n");
            //varname1.Append("       END                    TicketFreq, \n");
            //varname1.Append("       CASE j.Status \n");
            //varname1.Append("         WHEN 0 THEN 'Active' \n");
            //varname1.Append("         WHEN 1 THEN 'Closed' \n");
            //varname1.Append("         WHEN 2 THEN 'Hold' \n");
            //varname1.Append("         WHEN 3 THEN 'Completed' \n");
            //varname1.Append("       END                    Status \n");
            //varname1.Append("FROM   job j \n");
            //varname1.Append("       INNER JOIN Contract c \n");
            //varname1.Append("               ON j.id = c.Job \n");
            //varname1.Append("       LEFT OUTER JOIN Loc l \n");
            //varname1.Append("                    ON l.Loc = c.Loc \n");
            //varname1.Append("       LEFT OUTER JOIN owner o \n");
            //varname1.Append("                    ON o.id = l.owner \n");
            //varname1.Append("       LEFT OUTER JOIN rol r \n");
            //varname1.Append("                    ON o.rol = r.id \n");
            //varname1.Append("WHERE  j.type = 0 ");

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT c.expirationdate, c.Job, \n");
            varname1.Append("       j.ctype, \n");
            varname1.Append("       j.fdesc, \n");
            varname1.Append("       c.BAmt, \n");
            varname1.Append("       c.Hours, \n");
            varname1.Append("       l.ID                   AS locid, \n");
            varname1.Append("       l.Tag, \n");
            varname1.Append("       (SELECT TOP 1 name \n");
            varname1.Append("        FROM   rol r \n");
            varname1.Append("        WHERE  o.rol = r.id \n");
            varname1.Append("               )AS name, \n");
            varname1.Append("       Round (CASE c.BCycle \n");
            varname1.Append("                WHEN 0 THEN c.BAmt \n");
            varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
            varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
            varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
            varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
            varname1.Append("              END, 2)         AS MonthlyBill, \n");
            varname1.Append("       Round (CASE c.SCycle \n");
            varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
            varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
            varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
            varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            varname1.Append("                --WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
            varname1.Append("                --WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
            varname1.Append("                WHEN 10 THEN c.Hours / 12*2 --Every 2 Years \n");
            varname1.Append("                WHEN 8 THEN c.Hours / 12*3 --Every 3 Years \n");
            varname1.Append("                WHEN 9 THEN c.Hours / 12*5 --Every 5 Years \n");
            varname1.Append("                WHEN 11 THEN c.Hours / 12*7 --Every 7 Years \n");
            varname1.Append("              END, 2)         AS MonthlyHours, \n");
            varname1.Append("       CASE c.bcycle \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            varname1.Append("         WHEN 4 THEN 'Semi-Annually' \n");
            varname1.Append("         WHEN 5 THEN 'Anually' \n");
            varname1.Append("         WHEN 6 THEN 'Never' \n");
            varname1.Append("       END                    Freqency, \n");
            varname1.Append("       CASE c.scycle \n");
            varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 3 THEN 'Semi-Anually' \n");
            varname1.Append("         WHEN 4 THEN 'Anually' \n");
            varname1.Append("        -- WHEN 5 THEN 'Weekly' \n");
            varname1.Append("       --  WHEN 6 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 7 THEN 'Every 13 Weeks' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Years' \n");
            varname1.Append("         WHEN 8 THEN 'Every 3 Years' \n");
            varname1.Append("         WHEN 9 THEN 'Every 5 Years' \n");
            varname1.Append("         WHEN 11 THEN 'Every 7 Years' \n");
            varname1.Append("         WHEN 12 THEN 'On-Demand' \n");
            varname1.Append("       END                    TicketFreq, \n");
            varname1.Append("       CASE j.Status \n");
            varname1.Append("         WHEN 0 THEN 'Active' \n");
            varname1.Append("         WHEN 1 THEN 'Closed' \n");
            varname1.Append("         WHEN 2 THEN 'Hold' \n");
            varname1.Append("         WHEN 3 THEN 'Completed' \n");
            varname1.Append("       END                    Status \n");
            varname1.Append("FROM   job j \n");
            varname1.Append("       INNER JOIN Contract c \n");
            varname1.Append("               ON j.id = c.Job \n");
            varname1.Append("       LEFT OUTER JOIN Loc l \n");
            varname1.Append("                    ON l.Loc = c.Loc \n");
            varname1.Append("       LEFT OUTER JOIN owner o \n");
            varname1.Append("                    ON o.id = l.owner \n");
            varname1.Append("       LEFT OUTER JOIN rol r \n");
            varname1.Append("                    ON o.rol = r.id \n");
            varname1.Append("WHERE  j.type = 0 ");

            if (!string.IsNullOrEmpty (objPropContracts.SearchBy))
            {
                if (objPropContracts.SearchBy == "r.name" || objPropContracts.SearchBy == "l.tag")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '%" + objPropContracts.SearchValue + "%'");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "'");
                }
            }
            if (objPropContracts.ExpirationDate != DateTime.MinValue)
            {
                int days = DateTime.DaysInMonth(objPropContracts.ExpirationDate.Year, objPropContracts.ExpirationDate.Month);
                int date = days - objPropContracts.ExpirationDate.Day;
                DateTime datec = objPropContracts.ExpirationDate.AddDays(date);
                varname1.Append(" and ExpirationDate <= '" + datec + "'");
                //varname1.Append(" and month(ExpirationDate) = month('" + objPropContracts.ExpirationDate + "') and year(ExpirationDate) = year('" + objPropContracts.ExpirationDate + "') ");
            }

            varname1.Append("order by c.job ");

            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetContract(Contracts objPropContracts)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("SELECT j.Loc, \n");
            QueryText.Append("       j.Owner, \n");
            QueryText.Append("       Custom20, \n");
            QueryText.Append("       j.Status, \n");
            QueryText.Append("       BStart, \n");
            QueryText.Append("       BCycle, \n");
            QueryText.Append("       BAmt, \n");
            QueryText.Append("       c.SStart, \n");
            QueryText.Append("       sCycle, \n");
            QueryText.Append("       SDate, \n");
            QueryText.Append("       SDay, \n");
            QueryText.Append("       STime, \n");
            QueryText.Append("       CreditCard, \n");
            QueryText.Append("       j.Remarks, \n");
            QueryText.Append("       l.tag AS locname, \n");
            QueryText.Append("       swe, \n");
            QueryText.Append("       c.hours, \n");
            QueryText.Append("       j.ctype, \n");
            QueryText.Append("       j.fdesc, \n");
            QueryText.Append("       j.id, \n");
            QueryText.Append("       ExpirationDate, Expiration, frequencies, \n");
            QueryText.Append("       l.Billing, \n");
            QueryText.Append("       o.Billing AS CustBilling, \n");
            QueryText.Append("       o.Central, \n");
            QueryText.Append("       c.Chart, \n");
            QueryText.Append("       ch.fDesc as GLAcct, \n");
            QueryText.Append("       BEscType, \n");
            QueryText.Append("       BEscCycle, \n");
            QueryText.Append("       BEscFact, \n");
            QueryText.Append("       EscLast, \n");
            QueryText.Append("       isnull(j.BillRate,0) as BillRate, \n");
            QueryText.Append("       isnull(j.RateOT,0) as RateOT, \n");
            QueryText.Append("       isnull(j.RateNT,0) as RateNT, \n");
            QueryText.Append("       isnull(j.RateMileage,0) as RateMileage, \n");
            QueryText.Append("       isnull(j.RateDT,0) as RateDT, \n");
            QueryText.Append("       isnull(j.RateTravel,0) as RateTravel, \n");
            QueryText.Append("       isnull(j.PO,'') as PO \n");
            QueryText.Append("FROM   Job j \n");
            QueryText.Append("       INNER JOIN Contract c \n");
            QueryText.Append("               ON c.Job = j.ID \n");
            QueryText.Append("       LEFT JOIN Chart ch ON ch.ID = c.Chart \n");
            QueryText.Append("       INNER JOIN Loc l \n");
            QueryText.Append("               ON l.Loc = j.Loc \n");
            QueryText.Append("       INNER JOIN Owner o \n");
            QueryText.Append("               ON o.ID = l.Owner \n");
            QueryText.Append("WHERE  j.ID = " + objPropContracts.JobId);

            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetElevContract(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select Elev,Price,hours from tblJoinElevJob where Job=" + objPropContracts.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJstatus(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select status from jstatus");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddContract(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[40];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropContracts.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "hours";
            para[17].SqlDbType = SqlDbType.Decimal;
            para[17].Value = objPropContracts.Hours;

            para[18] = new SqlParameter();
            para[18].ParameterName = "fdesc";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropContracts.Description;

            para[19] = new SqlParameter();
            para[19].ParameterName = "ctype";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Ctype;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ExpirationDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.ExpirationDate != System.DateTime.MinValue)
                para[20].Value = objPropContracts.ExpirationDate;
            else
                para[20].Value = DBNull.Value;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationFreq";
            para[21].SqlDbType = SqlDbType.SmallInt;
            para[21].Value = objPropContracts.expirationfreq;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Expiration";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = objPropContracts.Expiration;

            para[23] = new SqlParameter();                   //added by Mayuri 25th dec,15
            para[23].ParameterName = "ContractBill";
            para[23].SqlDbType = SqlDbType.TinyInt;
            para[23].Value = objPropContracts.ContractBill;

            para[24] = new SqlParameter();
            para[24].ParameterName = "CustomerBill";
            para[24].SqlDbType = SqlDbType.TinyInt;
            para[24].Value = objPropContracts.CustBilling;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Central";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropContracts.Central;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Chart";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.Chart;

            para[27] = new SqlParameter();
            para[27].ParameterName = "JobT";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropContracts.JobTID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "CustomItems";
            para[28].SqlDbType = SqlDbType.Structured;
            para[28].Value = objPropContracts.DtCustom;

            para[29] = new SqlParameter();
            para[29].ParameterName = "@EscalationType";
            para[29].SqlDbType = SqlDbType.Int;
            para[29].Value = objPropContracts.EscalationType;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropContracts.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.EscalationLast != DateTime.MinValue)
                para[32].Value = objPropContracts.EscalationLast;
            else
                para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "BillRate";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = objPropContracts.BillRate;

            para[34] = new SqlParameter();
            para[34].ParameterName = "RateOT";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = objPropContracts.RateOT;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateNT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = objPropContracts.RateNT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateDT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = objPropContracts.RateDT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateTravel";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = objPropContracts.RateTravel;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Mileage";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = objPropContracts.Mileage;

            para[39] = new SqlParameter();
            para[39].ParameterName = "PO";
            para[39].SqlDbType = SqlDbType.VarChar;
            para[39].Value = objPropContracts.PO;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spADDContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddContractTemp(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[20];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropContracts.Locaname;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropContracts.BcycleName;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropContracts.ScycleName;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "hours";
            para[17].SqlDbType = SqlDbType.Decimal;
            para[17].Value = objPropContracts.Hours;

            para[18] = new SqlParameter();
            para[18].ParameterName = "fdesc";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropContracts.Description;

            para[19] = new SqlParameter();
            para[19].ParameterName = "ctype";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Ctype;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spADDContractTemp", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateContract(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[41];

            para[0] = new SqlParameter();
            para[0].ParameterName = "loc";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.Loc;

            para[1] = new SqlParameter();
            para[1].ParameterName = "owner";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropContracts.Owner;

            para[2] = new SqlParameter();
            para[2].ParameterName = "date";
            para[2].SqlDbType = SqlDbType.DateTime;
            para[2].Value = objPropContracts.Date;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropContracts.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "CreditCard";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.CreditCard;

            para[5] = new SqlParameter();
            para[5].ParameterName = "Remarks";
            para[5].SqlDbType = SqlDbType.Text;
            para[5].Value = objPropContracts.Remarks;

            para[6] = new SqlParameter();
            para[6].ParameterName = "BStart";
            para[6].SqlDbType = SqlDbType.DateTime;
            para[6].Value = objPropContracts.BStart;

            para[7] = new SqlParameter();
            para[7].ParameterName = "BCycle";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropContracts.BCycle;

            para[8] = new SqlParameter();
            para[8].ParameterName = "BAmt";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.BAMT;

            para[9] = new SqlParameter();
            para[9].ParameterName = "SStart";
            para[9].SqlDbType = SqlDbType.DateTime;
            para[9].Value = objPropContracts.SStart;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Cycle";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Cycle;

            para[11] = new SqlParameter();
            para[11].ParameterName = "SWE";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.SWE;

            para[12] = new SqlParameter();
            para[12].ParameterName = "stime";
            para[12].SqlDbType = SqlDbType.DateTime;
            para[12].Value = objPropContracts.STime;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Sday";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Sday;

            para[14] = new SqlParameter();
            para[14].ParameterName = "SDate";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropContracts.Sdate;

            para[15] = new SqlParameter();
            para[15].ParameterName = "ElevJobData";
            para[15].SqlDbType = SqlDbType.Structured;
            para[15].Value = objPropContracts.DtElevJob;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Route";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropContracts.Route;

            para[17] = new SqlParameter();
            para[17].ParameterName = "job";
            para[17].SqlDbType = SqlDbType.Int;
            para[17].Value = objPropContracts.JobId;

            para[18] = new SqlParameter();
            para[18].ParameterName = "hours";
            para[18].SqlDbType = SqlDbType.Decimal;
            para[18].Value = objPropContracts.Hours;

            para[19] = new SqlParameter();
            para[19].ParameterName = "fdesc";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.Description;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ctype";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropContracts.Ctype;

            para[21] = new SqlParameter();
            para[21].ParameterName = "ExpirationDate";
            para[21].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.ExpirationDate != System.DateTime.MinValue)
                para[21].Value = objPropContracts.ExpirationDate;
            else
                para[21].Value = DBNull.Value;

            para[22] = new SqlParameter();
            para[22].ParameterName = "ExpirationFreq";
            para[22].SqlDbType = SqlDbType.SmallInt;
            para[22].Value = objPropContracts.expirationfreq;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Expiration";
            para[23].SqlDbType = SqlDbType.SmallInt;
            para[23].Value = objPropContracts.Expiration;

            para[24] = new SqlParameter();
            para[24].ParameterName = "ContractBill";
            para[24].SqlDbType = SqlDbType.SmallInt;
            para[24].Value = objPropContracts.ContractBill;

            para[25] = new SqlParameter();
            para[25].ParameterName = "CustomerBill";
            para[25].SqlDbType = SqlDbType.TinyInt;
            para[25].Value = objPropContracts.CustBilling;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Central";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.Central;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Chart";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropContracts.Chart;

            para[28] = new SqlParameter();
            para[28].ParameterName = "JobT";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.JobTID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CustomItems";
            para[29].SqlDbType = SqlDbType.Structured;
            para[29].Value = objPropContracts.DtCustom;

            para[30] = new SqlParameter();
            para[30].ParameterName = "@EscalationCycle";
            para[30].SqlDbType = SqlDbType.Int;
            para[30].Value = objPropContracts.EscalationCycle;

            para[31] = new SqlParameter();
            para[31].ParameterName = "@EscalationFactor";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropContracts.EscalationFactor;

            para[32] = new SqlParameter();
            para[32].ParameterName = "@EscalationLast";
            para[32].SqlDbType = SqlDbType.DateTime;
            if (objPropContracts.EscalationLast != DateTime.MinValue)
                para[32].Value = objPropContracts.EscalationLast;
            else
                para[32].Value = DBNull.Value;

            para[33] = new SqlParameter();
            para[33].ParameterName = "@EscalationType";
            para[33].SqlDbType = SqlDbType.Int;
            para[33].Value = objPropContracts.EscalationType;

            para[34] = new SqlParameter();
            para[34].ParameterName = "BillRate";
            para[34].SqlDbType = SqlDbType.Decimal;
            para[34].Value = objPropContracts.BillRate;

            para[35] = new SqlParameter();
            para[35].ParameterName = "RateOT";
            para[35].SqlDbType = SqlDbType.Decimal;
            para[35].Value = objPropContracts.RateOT;

            para[36] = new SqlParameter();
            para[36].ParameterName = "RateNT";
            para[36].SqlDbType = SqlDbType.Decimal;
            para[36].Value = objPropContracts.RateNT;

            para[37] = new SqlParameter();
            para[37].ParameterName = "RateDT";
            para[37].SqlDbType = SqlDbType.Decimal;
            para[37].Value = objPropContracts.RateDT;

            para[38] = new SqlParameter();
            para[38].ParameterName = "RateTravel";
            para[38].SqlDbType = SqlDbType.Decimal;
            para[38].Value = objPropContracts.RateTravel;

            para[39] = new SqlParameter();
            para[39].ParameterName = "Mileage";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropContracts.Mileage;

            para[40] = new SqlParameter();
            para[40].ParameterName = "PO";
            para[40].SqlDbType = SqlDbType.VarChar;
            para[40].Value = objPropContracts.PO;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spUpdateContract", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteContract(Contracts objPropContracts)
        {
            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("DELETE FROM Job \n");
            //QueryText.Append("WHERE  ID = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM Contract \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);
            //QueryText.Append(" \n");
            //QueryText.Append("DELETE FROM tblJoinElevJob \n");
            //QueryText.Append("WHERE  Job = " + objPropContracts.JobId);

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "Spdeletecontract", objPropContracts.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddRecurringTickets(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spAddRecurringTickets", objPropContracts.Loc, objPropContracts.Remarks, objPropContracts.PerContract, objPropContracts.ContractRemarks, objPropContracts.Owner, objPropContracts.Route, objPropContracts.StartDt, objPropContracts.EndDt, objPropContracts.OnDemand);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLastProcessDate(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom19,Custom16 from job order by CONVERT (datetime,custom19) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceLastProcessDate(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom17,custom15 from job order by CONVERT (datetime,custom17) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void CreateRecurringTickets(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RecurringTicket";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "RemarksOpt";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropContracts.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "JobRemarksOpt";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.ContractRemarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "ProcessPeriod";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropContracts.ProcessPeriod;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateRecurringTickets", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CreateRecurringInvoices(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter();
            para[0].ParameterName = "RecurringInvoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "InvoiceDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "PayTerms";
            para[2].SqlDbType = SqlDbType.Int;
            para[2].Value = objPropContracts.PaymentTerms;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Notes";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropContracts.Remarks;

            para[4] = new SqlParameter();
            para[4].ParameterName = "ParaStax";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropContracts.Taxable;

            para[5] = new SqlParameter();
            para[5].ParameterName = "ProcessPeriod";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.ProcessPeriod;

            para[6] = new SqlParameter();
            para[6].ParameterName = "cfUser";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.Fuser;
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateRecurringInvoices", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillingFieldByLoc(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select Billing from loc where Loc=" + objPropContracts.Loc + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int CreateInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[29];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            //para[16] = new SqlParameter();
            //para[16].ParameterName = "batch";
            //para[16].SqlDbType = SqlDbType.Int;
            //para[16].Value = objPropContracts.Batch;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "TicketIDs";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.Tickets;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            para[28] = new SqlParameter();
            para[28].ParameterName = "returnval";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Direction = ParameterDirection.ReturnValue;
            //para[28] = new SqlParameter();
            //para[28].ParameterName = "TransID";
            //para[28].SqlDbType = SqlDbType.Int;
            //para[28].Value = objPropContracts.TransID;
            try
            {
                //DataSet _ds =
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spCreateInvoice", para);
                return Convert.ToInt32(para[28].Value);
                //return Convert.ToInt32(_ds.Tables[0].Rows[0]["Ref"]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQBInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "job";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.JobId;

            para[11] = new SqlParameter();
            para[11].ParameterName = "po";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropContracts.PO;

            para[12] = new SqlParameter();
            para[12].ParameterName = "status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "batch";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Batch;

            para[14] = new SqlParameter();
            para[14].ParameterName = "remarks";
            para[14].SqlDbType = SqlDbType.Text;
            para[14].Value = objPropContracts.Remarks;

            para[15] = new SqlParameter();
            para[15].ParameterName = "gtax";
            para[15].SqlDbType = SqlDbType.Money;
            para[15].Value = objPropContracts.Gtax;

            para[16] = new SqlParameter();
            para[16].ParameterName = "mech";
            para[16].SqlDbType = SqlDbType.Int;
            para[16].Value = objPropContracts.Mech;

            para[17] = new SqlParameter();
            para[17].ParameterName = "TaxRegion2";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropContracts.TaxRegion2;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Taxrate2";
            para[18].SqlDbType = SqlDbType.Money;
            para[18].Value = objPropContracts.Taxrate2;

            para[19] = new SqlParameter();
            para[19].ParameterName = "BillTo";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.BillTo;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Idate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropContracts.Idate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Fuser";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.Fuser;

            para[22] = new SqlParameter();
            para[22].ParameterName = "staxI";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropContracts.StaxI;

            para[23] = new SqlParameter();
            para[23].ParameterName = "invoiceID";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.InvoiceIDCustom;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBLOCID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropContracts.QBCustomerID;

            para[25] = new SqlParameter();
            para[25].ParameterName = "QBTERMSID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.QBTermsID;

            para[26] = new SqlParameter();
            para[26].ParameterName = "QBjobtypeID";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.QBJobtypeID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "QBInvoiceID";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropContracts.QBInvID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "TicketID";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.TicketID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "LastUpdateDate";
            para[29].SqlDbType = SqlDbType.DateTime;
            para[29].Value = objPropContracts.LastUpdateDate;

            try
            {
                SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spQBCreateInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateQBInvoiceMapping(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[30];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "job";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.JobId;

            para[11] = new SqlParameter();
            para[11].ParameterName = "po";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropContracts.PO;

            para[12] = new SqlParameter();
            para[12].ParameterName = "status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "batch";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Batch;

            para[14] = new SqlParameter();
            para[14].ParameterName = "remarks";
            para[14].SqlDbType = SqlDbType.Text;
            para[14].Value = objPropContracts.Remarks;

            para[15] = new SqlParameter();
            para[15].ParameterName = "gtax";
            para[15].SqlDbType = SqlDbType.Money;
            para[15].Value = objPropContracts.Gtax;

            para[16] = new SqlParameter();
            para[16].ParameterName = "mech";
            para[16].SqlDbType = SqlDbType.Int;
            para[16].Value = objPropContracts.Mech;

            para[17] = new SqlParameter();
            para[17].ParameterName = "TaxRegion2";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropContracts.TaxRegion2;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Taxrate2";
            para[18].SqlDbType = SqlDbType.Money;
            para[18].Value = objPropContracts.Taxrate2;

            para[19] = new SqlParameter();
            para[19].ParameterName = "BillTo";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.BillTo;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Idate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropContracts.Idate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Fuser";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.Fuser;

            para[22] = new SqlParameter();
            para[22].ParameterName = "staxI";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropContracts.StaxI;

            para[23] = new SqlParameter();
            para[23].ParameterName = "invoiceID";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.InvoiceIDCustom;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBLOCID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropContracts.QBCustomerID;

            para[25] = new SqlParameter();
            para[25].ParameterName = "QBTERMSID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.QBTermsID;

            para[26] = new SqlParameter();
            para[26].ParameterName = "QBjobtypeID";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropContracts.QBJobtypeID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "QBInvoiceID";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropContracts.QBInvID;

            para[28] = new SqlParameter();
            para[28].ParameterName = "TicketID";
            para[28].SqlDbType = SqlDbType.Int;
            para[28].Value = objPropContracts.TicketID;

            para[29] = new SqlParameter();
            para[29].ParameterName = "LastUpdateDate";
            para[29].SqlDbType = SqlDbType.DateTime;
            para[29].Value = objPropContracts.LastUpdateDate;

            try
            {
                SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spQBCreateInvoiceMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateInvoice(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[28];
            para[0] = new SqlParameter();
            para[0].ParameterName = "Invoice";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropContracts.DtRecContr;

            para[1] = new SqlParameter();
            para[1].ParameterName = "fdate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.Date;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Fdesc";
            para[2].SqlDbType = SqlDbType.Text;
            para[2].Value = objPropContracts.Remarks;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "stax";
            para[4].SqlDbType = SqlDbType.Money;
            para[4].Value = objPropContracts.Staxtotal;

            para[5] = new SqlParameter();
            para[5].ParameterName = "total";
            para[5].SqlDbType = SqlDbType.Money;
            para[5].Value = objPropContracts.Total;

            para[6] = new SqlParameter();
            para[6].ParameterName = "taxRegion";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.TaxRegion;

            para[7] = new SqlParameter();
            para[7].ParameterName = "taxrate";
            para[7].SqlDbType = SqlDbType.Money;
            para[7].Value = objPropContracts.Taxrate;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Taxfactor";
            para[8].SqlDbType = SqlDbType.Money;
            para[8].Value = objPropContracts.Taxfactor;

            para[9] = new SqlParameter();
            para[9].ParameterName = "taxable";
            para[9].SqlDbType = SqlDbType.Int;
            para[9].Value = objPropContracts.Taxable;

            para[10] = new SqlParameter();
            para[10].ParameterName = "type";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.Type;

            para[11] = new SqlParameter();
            para[11].ParameterName = "job";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.JobId;

            para[12] = new SqlParameter();
            para[12].ParameterName = "loc";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Loc;

            para[13] = new SqlParameter();
            para[13].ParameterName = "terms";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropContracts.Terms;

            para[14] = new SqlParameter();
            para[14].ParameterName = "po";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.PO;

            para[15] = new SqlParameter();
            para[15].ParameterName = "status";
            para[15].SqlDbType = SqlDbType.Int;
            para[15].Value = objPropContracts.Status;

            //para[16] = new SqlParameter();
            //para[16].ParameterName = "batch";
            //para[16].SqlDbType = SqlDbType.Int;
            //para[16].Value = objPropContracts.Batch;

            para[16] = new SqlParameter();
            para[16].ParameterName = "remarks";
            para[16].SqlDbType = SqlDbType.Text;
            para[16].Value = objPropContracts.Remarks;

            para[17] = new SqlParameter();
            para[17].ParameterName = "gtax";
            para[17].SqlDbType = SqlDbType.Money;
            para[17].Value = objPropContracts.Gtax;

            para[18] = new SqlParameter();
            para[18].ParameterName = "mech";
            para[18].SqlDbType = SqlDbType.Int;
            para[18].Value = objPropContracts.Mech;

            para[19] = new SqlParameter();
            para[19].ParameterName = "TaxRegion2";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropContracts.TaxRegion2;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Taxrate2";
            para[20].SqlDbType = SqlDbType.Money;
            para[20].Value = objPropContracts.Taxrate2;

            para[21] = new SqlParameter();
            para[21].ParameterName = "BillTo";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropContracts.BillTo;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Idate";
            para[22].SqlDbType = SqlDbType.DateTime;
            para[22].Value = objPropContracts.Idate;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Fuser";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropContracts.Fuser;

            para[24] = new SqlParameter();
            para[24].ParameterName = "staxI";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropContracts.StaxI;

            para[25] = new SqlParameter();
            para[25].ParameterName = "invoiceID";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropContracts.InvoiceIDCustom;

            para[26] = new SqlParameter();
            para[26].ParameterName = "InvID";
            para[26].SqlDbType = SqlDbType.Int;
            para[26].Value = objPropContracts.InvoiceID;

            para[27] = new SqlParameter();
            para[27].ParameterName = "ddate";
            para[27].SqlDbType = SqlDbType.DateTime;
            para[27].Value = objPropContracts.DueDate;

            //para[28] = new SqlParameter();
            //para[28].ParameterName = "TransID";
            //para[28].SqlDbType = SqlDbType.Int;
            //para[28].Value = objPropContracts.TransID;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spUpdateInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringInvoices(Contracts objPropContracts)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT Getdate()                           AS fdate, \n");
            //varname1.Append("       ''                                  AS fdesc, \n");
            //varname1.Append("       c.BAmt as amount, \n");
            //varname1.Append("       0                                   AS stax, \n");
            //varname1.Append("       0.00                                AS Taxregion, \n");
            //varname1.Append("       ( st.Rate+c.BAmt )                  AS total, \n");
            //varname1.Append("       st.Rate                             AS taxrate, \n");
            //varname1.Append("       100.00                              AS taxfactor, \n");
            //varname1.Append("       0                                   AS taxable, \n");
            //varname1.Append("       0                                   AS type, \n");
            //varname1.Append("       j.ID                                AS job, \n");
            //varname1.Append("       l.Loc, \n");
            //varname1.Append("       ''                                  AS terms, \n");
            //varname1.Append("       j.PO, \n");
            //varname1.Append("       --CASE l.Status      \n");
            //varname1.Append("       --  WHEN 0 THEN 'open'      \n");
            //varname1.Append("       --  WHEN 1 THEN 'paid'      \n");
            //varname1.Append("       --END                                 AS Status,      \n");
            //varname1.Append("       l.status, \n");
            //varname1.Append("       '0'                                 AS batch, \n");
            //varname1.Append("       'Recurring'                         AS remarks, \n");
            //varname1.Append("       0                                   AS gtax, \n");
            //varname1.Append("       j.Custom20                          AS worker, \n");
            //varname1.Append("       ''                                  AS Taxregion2, \n");
            //varname1.Append("       0.00                                AS taxrate2, \n");
            //varname1.Append("       ''                                  AS billto, \n");
            //varname1.Append("       Getdate()                           AS Idate, \n");
            //varname1.Append("       ''                                  AS fuser, \n");
            //varname1.Append("       0                                   AS acct, \n");
            //varname1.Append("       1.00                                AS Quan, \n");
            //varname1.Append("       0                                   AS price, \n");
            //varname1.Append("       1                                   AS jobitem, \n");
            //varname1.Append("       (SELECT measure \n");
            //varname1.Append("        FROM   Inv I \n");
            //varname1.Append("        WHERE  I.Name = 'Recurring')       AS measure, \n");
            //varname1.Append("       CASE c.BCycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly recurring billing' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly recurring billing' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly recurring billing' \n");
            //varname1.Append("         WHEN 3 THEN 'Semi-Anually recurring billing' \n");
            //varname1.Append("         WHEN 4 THEN 'Anually recurring billing' \n");
            //varname1.Append("       END                                 AS fdescI, \n");
            //varname1.Append("       CASE c.bcycle \n");
            //varname1.Append("         WHEN 0 THEN 'Monthly' \n");
            //varname1.Append("         WHEN 1 THEN 'Bi-Monthly' \n");
            //varname1.Append("         WHEN 2 THEN 'Quarterly' \n");
            //varname1.Append("         WHEN 3 THEN '3 Times/Year' \n");
            //varname1.Append("         WHEN 4 THEN 'Semi-Anually' \n");
            //varname1.Append("         WHEN 5 THEN 'Anually' \n");
            //varname1.Append("         WHEN 6 THEN 'Never' \n");
            //varname1.Append("       END                                 Frequency, \n");
            //varname1.Append("       --case when @stax=1 then l.STax               \n");
            //varname1.Append("       --else null end as  staxI,             \n");
            //varname1.Append("       st.Name, \n");
            //varname1.Append("       (SELECT TOP 1 name \n");
            //varname1.Append("        FROM   rol \n");
            //varname1.Append("        WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                     FROM   owner \n");
            //varname1.Append("                     WHERE  id = l.Owner)) AS customername, \n");
            //varname1.Append("       (SELECT TOP 1 Tag \n");
            //varname1.Append("        FROM   Loc \n");
            //varname1.Append("        WHERE  Loc = l.Loc)                AS locid, \n");
            //varname1.Append("       (SELECT TOP 1 name \n");
            //varname1.Append("        FROM   rol \n");
            //varname1.Append("        WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                     FROM   Loc \n");
            //varname1.Append("                     WHERE  Loc = l.Loc))  AS locname, \n");
            //varname1.Append("       (SELECT Name \n");
            //varname1.Append("        FROM   Route ro \n");
            //varname1.Append("        WHERE  ro.ID = j.Custom20)         AS dworker \n");
            //varname1.Append("FROM   Loc l \n");
            //varname1.Append("       LEFT OUTER JOIN STax st \n");
            //varname1.Append("                    ON l.STax = st.Name \n");
            //varname1.Append("       INNER JOIN Job j \n");
            //varname1.Append("               ON l.Loc = j.Loc \n");
            //varname1.Append("       INNER JOIN Contract c \n");
            //varname1.Append("               ON j.ID = c.Job ");
            //varname1.Append("WHERE               j.custom17 is null \n");
            //varname1.Append("and  c.BStart >= '"+ objPropContracts.StartDt+"' \n");
            //varname1.Append("       AND c.BStart <= '" + objPropContracts.EndDt + "'");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, "spAddRecurringInvoices", objPropContracts.Loc, objPropContracts.Owner, objPropContracts.Month, objPropContracts.Year);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("                i.fDate, \n");
            varname1.Append("                l.Loc, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag, \n");
            varname1.Append("                i.fdesc, \n");
            varname1.Append("                isnull(l.Remarks,'') as locRemarks, \n");
			varname1.Append("  	             isnull(j.Remarks,'') as JobRemarks, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            varname1.Append("                i.Total, \n");
            varname1.Append("                isnull(i.status,0) as InvStatus,");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS customername, \n");
            varname1.Append("                (SELECT Type \n");
            varname1.Append("                 FROM   JobType jt \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type, \n");
            varname1.Append("                  i.Batch, \n");
            //varname1.Append("    case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance, ");
            varname1.Append("                 isnull((Select (i.Total - sum(tr.Amount)) FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) AS balance \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r \n");
            varname1.Append("               ON o.rol = r.id \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("               ON i.ref = ip.ref \n");
            varname1.Append("       LEFT OUTER JOIN Job j ON i.Job=j.ID \n");
            varname1.Append("       WHERE i.ref is not null \n");
            if (objPropContracts.SearchBy != string.Empty && objPropContracts.SearchBy != null)
            {
                if (objPropContracts.SearchBy == "i.fdate")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.owner")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "i.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '" + objPropContracts.SearchValue + "%' \n");
                }
            }
            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate >='" + objPropContracts.StartDate + "'\n");
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1) + "'");
            }
            if (objPropContracts.CustID != 0)
            {
                varname1.Append(" and l.owner =" + objPropContracts.CustID + "");
            }
            if (objPropContracts.Loc != 0)
            {
                varname1.Append(" and l.loc =" + objPropContracts.Loc + "");
            }
            if (objPropContracts.jobid != 0)
            {
                varname1.Append(" and i.job =" + objPropContracts.jobid + "");
            }
            if (objPropContracts.Paid == 1)
            {
                //varname1.Append(" and i.status = 0");
                //varname1.Append(" and isnull(i.paid,0) = 0 and i.status = 0");
                varname1.Append(" and isnull( ip.paid,0) = 0 and i.status = 0");
            }
            if (objPropContracts.RoleId != 0)
                varname1.Append(" and isnull(l.roleid,0)=" + objPropContracts.RoleId);

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAPInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT JobI.Phase, \n");
            varname1.Append("       PJ.* \n");
            varname1.Append("FROM   JobI \n");
            varname1.Append("       LEFT OUTER JOIN Trans \n");
            varname1.Append("              ON Abs(JobI.TransID) = Trans.ID \n");
            varname1.Append("       LEFT OUTER JOIN PJ \n");
            varname1.Append("                    ON Trans.Batch = PJ.Batch \n");
            varname1.Append("WHERE  JobI.Job = " + objPropContracts.jobid + " \n");
            varname1.Append("       AND APTicket = 0 \n");
            varname1.Append("       AND JobI.Type = 1 \n");
            varname1.Append("       AND Trans.Type in (41,50) \n");
            varname1.Append("ORDER  BY JobI.fDate, \n");
            varname1.Append("          JobI.Type, \n");
            varname1.Append("          JobI.Ref ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetJobCostItems(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT JobI.fDate, \n");
            varname1.Append("       JobI.Ref, \n");
            varname1.Append("       LEFT(JobI.fDesc, 100) AS fDesc, \n");
            varname1.Append("       JobI.Amount, \n");
            varname1.Append("       ( CASE JobI.Type \n");
            varname1.Append("           WHEN 1 THEN NULL \n");
            varname1.Append("           ELSE JobI.Amount \n");
            varname1.Append("         END )               AS RAmt, \n");
            varname1.Append("       ( CASE JobI.Type \n");
            varname1.Append("           WHEN 1 THEN JobI.Amount \n");
            varname1.Append("           ELSE NULL \n");
            varname1.Append("         END )               AS EAmt, \n");
            varname1.Append("       JobI.Type, \n");
            varname1.Append("       JobI.Phase, \n");
            varname1.Append("       JobI.TransID, \n");
            varname1.Append("       ( CASE \n");
            varname1.Append("           WHEN JobI.TransID > 0 THEN Trans.Type \n");
            varname1.Append("           WHEN JobI.TransID = 0 THEN 1 \n");
            varname1.Append("           ELSE -1 \n");
            varname1.Append("         END )               AS TType \n");
            varname1.Append("FROM   JobI \n");
            varname1.Append("       LEFT JOIN Trans \n");
            varname1.Append("              ON Abs(JobI.TransID) = Trans.ID \n");
            varname1.Append("WHERE  JobI.Job = " + objPropContracts.jobid + " \n");
            varname1.Append("--and  \n");
            varname1.Append("--APTicket=0 and JobI.Type= 1 and Trans.Type=41  \n");
            varname1.Append("ORDER  BY JobI.fDate, \n");
            varname1.Append("          JobI.Type, \n");
            varname1.Append("          JobI.Ref ");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetInvoicesByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  Ref =  " + objPropContracts.InvoiceID + "");


            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT i.Ref, \n");
            varname11.Append("       i.Line, \n");
            varname11.Append("       i.Acct, \n");
            varname11.Append("       i.Quan, \n");
            varname11.Append("       i.fDesc, \n");
            varname11.Append("       i.Price, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS Amount, \n");
            varname11.Append("       i.STax, \n");
            varname11.Append("       i.Job, \n");
            varname11.Append("       i.JobItem, \n");
            varname11.Append("       i.TransID, \n");
            varname11.Append("       i.Measure, \n");
            varname11.Append("       i.Disc, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname11.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname11.Append("       (SELECT Name \n");
            varname11.Append("        FROM   Inv \n");
            varname11.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code \n");
            if (objPropContracts.isTS == 0)
                varname11.Append("FROM   InvoiceI i \n");
            else
                varname11.Append("FROM   MS_InvoiceI i \n");
            if (objPropContracts.isTS == 0)
                varname11.Append("       INNER JOIN Invoice inv \n");
            else
                varname11.Append("       INNER JOIN MS_Invoice inv \n");
            varname11.Append("               ON inv.Ref = i.Ref \n");
            varname11.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");

            //StringBuilder varname11 = new StringBuilder();
            //varname11.Append(" \n");
            //varname11.Append("SELECT *, \n");
            //varname11.Append("       ( quan * price )     AS pricequant, \n");
            //varname11.Append("       (SELECT Name \n");
            //varname11.Append("        FROM   Inv \n");
            //varname11.Append("        WHERE  ID = i.Acct) AS billcode \n");
            //varname11.Append("FROM   InvoiceI i \n");
            //varname11.Append("WHERE  Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoicesAmount(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("       ( Isnull(i.total, 0) - Isnull(ip.balance, 0) ) AS balance \n");
            varname1.Append("FROM   invoice i \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip \n");
            varname1.Append("                    ON ip.Ref = i.Ref \n");
            varname1.Append("WHERE  i.Ref IN(" + objPropContracts.InvoiceIDCustom + ") ");
            // "select isnull(total,0) as total, ref from invoice where Ref in(" + objPropContracts.InvoiceIDCustom + ")"
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoicesStatus(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.ref \n");
            varname1.Append("FROM   invoice i \n");
            varname1.Append("left outer join tblInvoicePayment ip on i.ref=ip.ref \n");
            varname1.Append("WHERE  i.Ref IN ( " + objPropContracts.InvoiceIDCustom + " ) \n");
            varname1.Append("       AND ( Isnull(ip.paid, 0) = 1 \n");
            varname1.Append("              OR Isnull(i.status, 0) = 1 ) ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInvoice(Contracts objPropContracts)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[3];

                para[0] = new SqlParameter();
                para[0].ParameterName = "Ref";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = objPropContracts.Ref;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Batch";
                para[1].SqlDbType = SqlDbType.Int;
                para[1].Value = objPropContracts.Batch;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Loc";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = objPropContracts.Loc;

                SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "spDeleteInvoice", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteInvoiceByListID(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spDeleteInvoiceByListID", objPropContracts.QBInvID);// CommandType.Text, "delete from invoicei where ref=(select ref from invoice where isnull(qbinvoiceid,'')<>'' and qbinvoiceid='" + objPropContracts.QBInvID + "') delete from invoice where isnull(qbinvoiceid,'')<>'' and qbinvoiceid='" + objPropContracts.QBInvID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetBillcodesforticket(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 0    AS Ref, \n");
            varname1.Append("       0    AS line, \n");
            varname1.Append("       id   AS acct, \n");
            varname1.Append("       0.00    AS Quan, \n");
            varname1.Append("       fDesc, \n");
            varname1.Append("       0.00    AS price, \n");
            varname1.Append("       0.00    AS amount, \n");
            varname1.Append("       0.00    AS stax, \n");
            varname1.Append("       0    AS Job, \n");
            varname1.Append("       0    AS JobItem, \n");
            varname1.Append("       0    AS TransID, \n");
            varname1.Append("       ''   AS Measure, \n");
            varname1.Append("       0    AS Disc, \n");
            varname1.Append("       0.00    AS STaxAmt, \n");
            varname1.Append("       0.00    AS pricequant, \n");
            varname1.Append("       Name AS billcode, 0 as code \n");
            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  Name IN (" + objPropContracts.TicketLineItems + " ) \n");
            varname1.Append("ORDER  BY billcode ");
            //'expenses', 'mileage', 'Time Spent' 
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillcodesforQBChargeableticket(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT 0    AS Ref, \n");
            varname1.Append("       0    AS line, \n");
            varname1.Append("       id   AS acct, \n");
            varname1.Append("       qbinvid , \n");
            varname1.Append("       0.00    AS Quan, \n");
            varname1.Append("       fDesc, \n");
            varname1.Append("       isnull(Price1,0)    AS price, \n");
            varname1.Append("       0.00    AS amount, \n");
            varname1.Append("       0.00    AS stax, \n");
            varname1.Append("       0    AS Job, \n");
            varname1.Append("       0    AS JobItem, \n");
            varname1.Append("       0    AS TransID, \n");
            varname1.Append("       ''   AS Measure, \n");
            varname1.Append("       0    AS Disc, \n");
            varname1.Append("       0.00    AS STaxAmt, \n");
            varname1.Append("       0.00    AS pricequant, \n");
            varname1.Append("       Name AS billcode \n");
            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  i.QBInvID ='" + objPropContracts.QBInvID + "' \n");
            if (objPropContracts.TicketLineItems != string.Empty)
            {
                varname1.Append(" or Name IN (" + objPropContracts.TicketLineItems + " ) \n");
            }
            if (objPropContracts.MileageItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.MileageItem + "' \n");
            }
            if (objPropContracts.LaborItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.LaborItem + "' \n");
            }
            if (objPropContracts.ExpenseItem != string.Empty)
            {
                varname1.Append(" or i.QBInvID = '" + objPropContracts.ExpenseItem + "' \n");
            }
            varname1.Append("ORDER  BY billcode ");
            //'expenses', 'mileage', 'Time Spent' 

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object AddPayment(Contracts objPropContracts)
        {
            SqlParameter[] para = new SqlParameter[15];

            para[0] = new SqlParameter();
            para[0].ParameterName = "InvoiceID";
            para[0].SqlDbType = SqlDbType.Int;
            para[0].Value = objPropContracts.InvoiceID;

            para[1] = new SqlParameter();
            para[1].ParameterName = "TransDate";
            para[1].SqlDbType = SqlDbType.DateTime;
            para[1].Value = objPropContracts.TransDate;

            para[2] = new SqlParameter();
            para[2].ParameterName = "CardNumber";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropContracts.CardNumber;

            para[3] = new SqlParameter();
            para[3].ParameterName = "Amount";
            para[3].SqlDbType = SqlDbType.Money;
            para[3].Value = objPropContracts.Amount;

            para[4] = new SqlParameter();
            para[4].ParameterName = "response";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropContracts.Response;

            para[5] = new SqlParameter();
            para[5].ParameterName = "refid";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropContracts.PaymentRefID;

            para[6] = new SqlParameter();
            para[6].ParameterName = "UserID";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropContracts.UserID;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Screen";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropContracts.Screen;

            para[8] = new SqlParameter();
            para[8].ParameterName = "ResponseCodes";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropContracts.ResponseCodes;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Approved";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropContracts.Approved;

            para[10] = new SqlParameter();
            para[10].ParameterName = "IsSuccess";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropContracts.IsSuccess;

            para[11] = new SqlParameter();
            para[11].ParameterName = "CustomerID";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropContracts.CustID;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Status";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropContracts.Status;

            para[13] = new SqlParameter();
            para[13].ParameterName = "PaymentUID";
            para[13].SqlDbType = SqlDbType.UniqueIdentifier;
            para[13].Value = objPropContracts.PaymentUID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "GatewayOrderID";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropContracts.GatewayOrderID;

            try
            {
                return SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.StoredProcedure, "Spaddpayment", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPaymentHistory(Contracts objPropContracts)
        {
            string strQuery = "select p.*,(select name from rol r where r.id = (select rol from owner o where o.id= p.customerid )) as owner, l.tag from tblPaymentHistory p ";
            strQuery += " inner join Invoice i on i.Ref=p.InvoiceID inner join loc l on l.loc=i.loc ";
            strQuery += " where transactionid is not null";

            if (objPropContracts.RoleId != 0)
            {
                strQuery += " and isnull( l.RoleID,0) =" + objPropContracts.RoleId;
            }
            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate >='" + objPropContracts.StartDate + "'";
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                strQuery += " and transdate <'" + objPropContracts.EndDate.AddDays(1) + "'";
            }
            if (objPropContracts.InvoiceID != 0)
            {
                strQuery += " and InvoiceID = " + objPropContracts.InvoiceID;
            }
            if (objPropContracts.Medium != string.Empty)
            {
                strQuery += " and Medium ='" + objPropContracts.Medium + "'";
            }
            //if (objPropContracts.UserID != string.Empty)
            //{
            //    strQuery += " and userid='" + objPropContracts.UserID + "'";
            //}
            if (objPropContracts.CustID != 0)
            {
                strQuery += " and isnull( customerid ,0) =" + objPropContracts.CustID;
            }
            if (objPropContracts.IsSuccess != -1)
                strQuery += " and isnull( isSuccess ,0) =" + objPropContracts.IsSuccess;

            strQuery += " order by transdate desc";

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPaymentGatewayInfo(Contracts objPropContracts)
        {
            string strQuery = "select * from tblGatewayInfo";

            if (!string.IsNullOrEmpty(objPropContracts.MerchantID))
            {
                strQuery += " where id=" + Convert.ToInt32(objPropContracts.MerchantID);
            }

            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMerchant(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "Spaddmerchant", objPropContracts.MerchantID, objPropContracts.LoginID, objPropContracts.PaymentUser, objPropContracts.PaymentPass, objPropContracts.MerchantInfoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteMerchant(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.Text, "delete from tblgatewayinfo where id=" + objPropContracts.MerchantInfoID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMaxQBInvoiceID(Contracts objPropContracts)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "select isnull(MAX(cast(Custom1 as int)),0)+1 from Invoice where QBInvoiceID is not null and IsNumeric(Custom1) = 1"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetBillcodesforTimeSheet(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       QBInvID, \n");
            varname1.Append("       Name AS billcode \n");
            varname1.Append("FROM   Inv i \n");
            varname1.Append("WHERE  QBInvID IS NOT NULL \n");
            varname1.Append("       AND type = 1 \n");
            varname1.Append("ORDER  BY billcode ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollforTimeSheet(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       QBwageID, \n");
            varname1.Append("       fdesc  \n");
            varname1.Append("FROM   prwage i \n");
            varname1.Append("WHERE  QBwageID IS NOT NULL \n");
            varname1.Append("ORDER  BY fdesc ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetPayrollByAccount(Contracts objPropContracts)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT TOP 1 QBwageID \n");
            //varname1.Append("FROM   prwage \n");
            //varname1.Append("WHERE  Isnull(QBAccountID, '') IS NOT NULL \n");
            //varname1.Append("       AND QBAccountID = (SELECT TOP 1 QBAccountID \n");
            //varname1.Append("                          FROM   Inv \n");
            //varname1.Append("                          WHERE  Isnull(QBAccountID, '') IS NOT NULL \n");
            //varname1.Append("                                 AND QBInvID = '"+objPropContracts.QBInvID+"') ");

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TOP 1 QBPayrollItem \n");
            varname1.Append("FROM   TicketD \n");
            varname1.Append("WHERE  QBServiceItem = '" + objPropContracts.QBInvID + "' \n");
            varname1.Append("GROUP  BY QBPayrollItem \n");
            varname1.Append("ORDER  BY Count(QBPayrollItem) DESC ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAddress(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.Ds = SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + objPropContracts.Owner);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesByBatch(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       i.Ref, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  i.Batch =  " + objPropContracts.Batch + "");
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerBalance(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spUpdateCustomerLocBalance", objPropContracts.Loc, objPropContracts.Amount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistContractByLoc(Contracts objPropContracts)
        {
            try
            {
                return objPropContracts.IsExistContract = Convert.ToBoolean(SqlHelper.ExecuteScalar(objPropContracts.ConnConfig, CommandType.Text, "SELECT  CAST( CASE WHEN EXISTS(SELECT Job, Loc, Owner FROM Contract WHERE Loc=" + objPropContracts.Loc + ")THEN 1  ELSE 0  END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetLastProcessDateOfInvoice(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "select top 1  custom15,Custom17 from job order by CONVERT (datetime,custom17) desc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoicesDetailsByID(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            //varname1.Append("(Select TOP 1 (case when Billing = '1' then (select l.tag from Loc as l where l.Loc = Central) when Billing = '0' then l.Tag end) \n");
            //varname1.Append("from owner where ID = l.Owner) as locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       i.Terms as payterms, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  Ref =  " + objPropContracts.InvoiceID + "");


            StringBuilder varname11 = new StringBuilder();
            varname11.Append(" \n");
            varname11.Append("SELECT  \n");
            varname11.Append("       i.Line, \n");
            varname11.Append("       i.Acct, \n");
            varname11.Append("       i.Quan, \n");
            varname11.Append("       i.fDesc as ifDesc, \n");
            varname11.Append("       i.Price, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS iAmount, \n");
            varname11.Append("       i.STax as iSTax, \n");
            //varname11.Append("       i.Job, \n");
            varname11.Append("       i.JobItem, \n");
            //arname11.Append("       i.TransID, \n");
            varname11.Append("       i.Measure, \n");
            varname11.Append("       i.Disc, \n");
            varname11.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname11.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname11.Append("       (SELECT Name \n");
            varname11.Append("        FROM   Inv \n");
            varname11.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code \n");
            if (objPropContracts.isTS == 0)
                varname11.Append("FROM   InvoiceI i \n");
            else
                varname11.Append("FROM   MS_InvoiceI i \n");
            if (objPropContracts.isTS == 0)
                varname11.Append("       INNER JOIN Invoice inv \n");
            else
                varname11.Append("       INNER JOIN MS_Invoice inv \n");
            varname11.Append("               ON inv.Ref = i.Ref \n");
            varname11.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString() + Environment.NewLine + varname11.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetEmailDetailByLoc(Contracts objPropContracts)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, "SELECT i.Loc,l.ID, l.Tag, l.custom12, l.custom13 FROM Invoice i INNER JOIN Loc l ON l.Loc = i.Loc where i.Ref="+objPropContracts.Ref);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public DataSet GetInvoicesByRef(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.*, \n");
            varname1.Append("       '' as DueDate, \n");
            varname1.Append("       (SELECT TOP 1 Name \n");
            varname1.Append("        FROM   rol \n");
            varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
            varname1.Append("                     FROM   Owner \n");
            varname1.Append("                     WHERE  ID = l.Owner)) AS customerName, \n");
            varname1.Append("       l.tag                               AS locname, \n");
            //varname1.Append("(Select TOP 1 (case when Billing = '1' then (select l.tag from Loc as l where l.Loc = Central) when Billing = '0' then l.Tag end) \n");
            //varname1.Append("from owner where ID = l.Owner) as locname, \n");
            varname1.Append("       l.owner, \n");
            varname1.Append("       l.Address, \n");
            varname1.Append("       (CASE i.status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Paid' \n");
            varname1.Append("         WHEN 2 THEN 'Voided' \n");
            varname1.Append("         WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("         WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("         WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("       END  + case (select paid from tblinvoicepayment where ref=i.ref) WHEN 1 THEN '/Paid by MOM' else '' end )     AS statusname, \n");
            varname1.Append("       (SELECT fdesc \n");
            varname1.Append("        FROM   tblWork \n");
            varname1.Append("        WHERE  ID = i.Mech)                AS MechName, \n");
            varname1.Append("       (SELECT Type \n");
            varname1.Append("        FROM   JobType jt \n");
            varname1.Append("        WHERE  jt.ID = i.Type)             AS typeName, \n");
            varname1.Append("       CASE i.Terms \n");
            varname1.Append("         WHEN 0 THEN 'Upon Receipt' \n");
            varname1.Append("         WHEN 1 THEN 'Net 10 Days' \n");
            varname1.Append("         WHEN 2 THEN 'Net 15 Days' \n");
            varname1.Append("         WHEN 3 THEN 'Net 30 Days' \n");
            varname1.Append("         WHEN 4 THEN 'Net 45 Days' \n");
            varname1.Append("         WHEN 5 THEN 'Net 60 Days' \n");
            varname1.Append("         WHEN 6 THEN '2%-10/Net 30 Days' \n");
            varname1.Append("         WHEN 7 THEN 'Net 90 Days' \n");
            varname1.Append("         WHEN 8 THEN 'Net 180 Days' \n");
            varname1.Append("         WHEN 9 THEN 'COD' \n");
            varname1.Append("       END                                 AS termsText, \n");
            varname1.Append("       i.Terms as payterms, \n");
            varname1.Append("       isnull((select paid from tblinvoicepayment where ref=i.ref),0)                                 AS paidcc, \n");
            varname1.Append("      convert(numeric(30,2), (isnull(i.total,0) - isnull((select balance from tblinvoicepayment where ref=i.ref),0) )) AS balance, \n");
            varname1.Append("      convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");

            varname1.Append("WHERE  Ref =  " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInvoiceItemByRef(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append(" \n");
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("       i.Line, \n");
            varname1.Append("       i.Acct, \n");
            varname1.Append("       i.Quan, \n");
            varname1.Append("       i.fDesc as fDesc, \n");
            varname1.Append("       i.Price, \n");
            //varname1.Append("       case isnull(i.stax,0) when 1 then ( i.Quan * i.Price ) + ( ( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100 ) else ( i.Quan * i.Price ) end AS Amount, \n");
            varname1.Append("       i.Amount, \n");
            varname1.Append("       i.STax as STax, \n");
            varname1.Append("       i.JobItem, \n");
            varname1.Append("       i.Measure, \n");
            varname1.Append("       i.Disc, \n");
            varname1.Append("       isnull(i.staxAmt,0) as staxAmt, \n ");
            //varname1.Append("       case isnull(i.stax,0) when 1 then convert(numeric(30,2),( ( i.Quan * i.Price ) * isnull(inv.TaxRate,0) ) / 100)  else 0 end                       AS StaxAmt, \n");
            varname1.Append("       ( i.Quan * i.Price )                                                    AS pricequant, \n");
            varname1.Append("       (SELECT Name \n");
            varname1.Append("        FROM   Inv \n");
            varname1.Append("        WHERE  ID = i.Acct)                                                    AS billcode, isnull(i.jobitem,0) as code, \n");
            varname1.Append("        convert(numeric(30,2), (isnull(inv.total,0) - isnull((select balance from tblinvoicepayment where ref=inv.ref),0) )) AS balance, \n");
            varname1.Append("        convert(numeric(30,2),   isnull((select balance from tblinvoicepayment where ref=i.ref),0) ) AS amtpaid,");
            varname1.Append("        inv.Total ");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   InvoiceI i \n");
            else
                varname1.Append("FROM   MS_InvoiceI i \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("       INNER JOIN Invoice inv \n");
            else
                varname1.Append("       INNER JOIN MS_Invoice inv \n");
            varname1.Append("               ON inv.Ref = i.Ref \n");
            varname1.Append("WHERE  i.Ref = " + objPropContracts.InvoiceID + "");

            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVoidInvoiceDetails(Contracts objPropContracts)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, "spVoidInvoice", objPropContracts.Ref, objPropContracts.Date.ToShortDateString(), objPropContracts.Fuser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetARInvoices(Contracts objPropContracts)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT i.Ref, \n");
            varname1.Append("                i.fDate, \n");
            varname1.Append("                l.Loc, \n");
            varname1.Append("                l.ID, \n");
            varname1.Append("                l.Tag as locid, \n");
            varname1.Append("                isnull(l.Remarks,'') as locRemarks, \n");
            varname1.Append("  	             isnull(j.Remarks,'') as JobRemarks, \n");
            varname1.Append("                i.Amount, \n");
            varname1.Append("                i.STax, \n");
            //varname1.Append("                i.Total, \n");
            varname1.Append("                isnull(i.status,0) as InvStatus,");
            varname1.Append("                i.custom1 as manualInv, \n");
            varname1.Append("                (CASE i.status \n");
            varname1.Append("                  WHEN 0 THEN 'Open' \n");
            varname1.Append("                  WHEN 1 THEN 'Paid' \n");
            varname1.Append("                  WHEN 2 THEN 'Voided' \n");
            varname1.Append("                  WHEN 4 THEN 'Marked as Pending' \n");
            varname1.Append("                  WHEN 5 THEN 'Paid by Credit Card' \n");
            varname1.Append("                  WHEN 3 THEN 'Partially Paid' \n");
            varname1.Append("                END + case isnull( ip.paid ,0) WHEN 1 THEN '/Paid by MOM' else '' end )                    AS status, \n");
            varname1.Append("                i.PO, \n");
            varname1.Append("                r.Name                  AS cid, \n");
            varname1.Append("                i.Idate as Due, i.fDesc,        \n");
            varname1.Append("                isnull(DATEDIFF(day,i.idate,GETDATE()),0) AS DueIn,   \n");
            varname1.Append("                (isnull(i.Total,0) - isnull(t.Total,0)) as total,   \n");
            varname1.Append("                CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 7) \n");
            varname1.Append("                   THEN        \n");
            varname1.Append("    	              (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("                   ELSE 0          \n");
            varname1.Append("               END as SevenDay     \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 30)   \n");
            varname1.Append("    	            THEN            \n");
            varname1.Append("    	                (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("    	            ELSE 0          \n");
            varname1.Append("                   END as ThirtyDay   \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 60)   \n");
            varname1.Append("    	            THEN            \n");
            varname1.Append("    	                 (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("    	            ELSE 0          \n");
            varname1.Append("               END as SixtyDay	   \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 61)  \n");
            varname1.Append("               THEN                \n");
            varname1.Append("     	                 (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("                   ELSE 0          \n");
            varname1.Append("               END as SixtyOneDay   \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 31)   \n");
            varname1.Append("               THEN            \n");
            varname1.Append("                        (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("               ELSE 0   \n");
            varname1.Append("               END as ZeroThirtyDay  \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 61) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 90)   \n");
            varname1.Append("               THEN            \n");
            varname1.Append("                        (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("               ELSE 0   \n");
            varname1.Append("               END as NintyDay  \n");
            varname1.Append("               ,CASE WHEN (isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 91)   \n");
            varname1.Append("                    THEN        \n");
            varname1.Append("                        (isnull(i.Total,0) - isnull(t.Total,0))  \n");
            varname1.Append("                    ELSE 0     \n");
            varname1.Append("               END as NintyOneDay	  	  \n");
            //varname1.Append("                (IIF((isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 0) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 7) ,isnull((Select (i.Total - sum(tr.Amount)) as Total FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.total), 0)) as SevenDay,             \n");
            //varname1.Append("                (IIF((isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 8) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 30) , isnull((Select (i.Total - sum(tr.Amount)) as Total FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.total), 0)) as ThirtyDay,         \n");
            //varname1.Append("                (IIF((isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 31) AND (isnull(DATEDIFF(day,i.idate,GETDATE()),0) <= 60) , isnull((Select (i.Total - sum(tr.Amount)) as Total FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.total), 0)) as SixtyDay,         \n");
            //varname1.Append("                (IIF((isnull(DATEDIFF(day,i.idate,GETDATE()),0) >= 61) , isnull((Select (i.Total - sum(tr.Amount)) as Total FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.total) , 0)) as SixtyOneDay,         \n");
            varname1.Append("                ,(SELECT Type           \n");
            varname1.Append("                 FROM   JobType jt     \n");
            varname1.Append("                 WHERE  jt.ID = i.Type) AS type,   \n");
            varname1.Append("                 case isnull( i.status ,0) WHEN 1 THEN 0 else convert(numeric(30,2) , (isnull(i.total,0) - isnull(ip.balance,0)) ) end as balance, i.Batch, 'Inv' as Type     \n");
            if (objPropContracts.isTS == 0)
                varname1.Append("FROM   Invoice i \n");
            else
                varname1.Append("FROM   MS_Invoice i \n");
            varname1.Append("       INNER JOIN Loc l \n");
            varname1.Append("               ON l.Loc = i.Loc \n");
            varname1.Append("       INNER JOIN owner o \n");
            varname1.Append("               ON o.id = l.owner \n");
            varname1.Append("       INNER JOIN rol r                        \n");
            varname1.Append("               ON o.rol = r.id                 \n");
            varname1.Append("       LEFT OUTER JOIN tblInvoicePayment ip    \n");
            varname1.Append("               ON i.ref = ip.ref \n");
            varname1.Append("       LEFT OUTER JOIN Job j ON i.Job=j.ID     \n");
            varname1.Append("       LEFT OUTER JOIN (Select i.Ref, sum(tr.Amount) as Total from Trans tr        \n");
            varname1.Append("                       Inner join  Invoice i on i.Ref = tr.Ref and tr.Type = 98    \n");
            varname1.Append("                       group by i.Ref) as t on t.Ref = i.Ref                       \n");
          //  varname1.Append("       INNER JOIN (Select (i.Total - sum(tr.Amount)) as Total, i.ref FROM Trans tr where tr.Type = 98 and tr.Ref=i.Ref),i.Total) as t ON i.ref = t.ref   \n");
            varname1.Append("       WHERE i.ref is not null                 \n");
            if (objPropContracts.SearchBy != string.Empty && objPropContracts.SearchBy != null)
            {
                if (objPropContracts.SearchBy == "i.fdate")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.owner")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "i.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = '" + objPropContracts.SearchValue + "' \n");
                }
                else if (objPropContracts.SearchBy == "l.loc")
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " = " + objPropContracts.SearchValue + " \n");
                }
                else
                {
                    varname1.Append(" and " + objPropContracts.SearchBy + " like '" + objPropContracts.SearchValue + "%' \n");
                }
            }
            if (objPropContracts.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate >='" + objPropContracts.StartDate + "'\n");
            }
            if (objPropContracts.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and i.fdate <'" + objPropContracts.EndDate.AddDays(1) + "'");
            }
            if (objPropContracts.CustID != 0)
            {
                varname1.Append(" and l.owner =" + objPropContracts.CustID + "");
            }
            if (objPropContracts.Loc != 0)
            {
                varname1.Append(" and l.loc =" + objPropContracts.Loc + "");
            }
            if (objPropContracts.jobid != 0)
            {
                varname1.Append(" and i.job =" + objPropContracts.jobid + "");
            }
            //if (objPropContracts.Paid == 1)
            //{
            //    //varname1.Append(" and i.status = 0");
            //    //varname1.Append(" and isnull(i.paid,0) = 0 and i.status = 0");
            //    varname1.Append(" and isnull( ip.paid,0) = 0 and i.status = 0");
            //}
            if (objPropContracts.RoleId != 0)
                varname1.Append(" and isnull(l.roleid,0)=" + objPropContracts.RoleId);
            
            varname1.Append("     and i.Status != 1 AND i.Status != 2    \n");
           
            try
            {
                return SqlHelper.ExecuteDataset(objPropContracts.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateExpirationDate(Contracts objPropContracts)
        {
            try
            {
                string str = "update contract set expirationdate = '"+objPropContracts.ExpirationDate+"' where job in ("+objPropContracts.jobids+")";
                SqlHelper.ExecuteNonQuery(objPropContracts.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}

