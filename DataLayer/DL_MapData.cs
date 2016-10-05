using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.IO;

namespace DataLayer
{
    public class DL_MapData
    {
        public void AddMapData(MapData objMapData)
        {
            SqlParameter para = new SqlParameter();
            para.ParameterName = "DtMapData";
            para.SqlDbType = SqlDbType.Structured;
            para.Value = objMapData.LocData;

            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.StoredProcedure, "spAddMapData", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTicket(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set edate='" + objMapData.Date + "', Assigned=" + objMapData.Assigned + " ,DWork='" + objMapData.Tech + "', fwork=(select top 1 w.ID from tblwork w where w.fdesc= '" + objMapData.Tech + "') where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTicket(MapData objMapData)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketO where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketD where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from TicketDPDA where ID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from Documents where screen='Ticket' and screenID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "delete from PDATicketSignature where PDATicketID=" + objMapData.TicketID);
                //SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "insert into tblticketdeleted(ticketid, date) values (" + objMapData.TicketID + ",getdate())");

                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, "spDeleteTicket", objMapData.TicketID, objMapData.Worker);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateTicketStatus(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set  Assigned=" + objMapData.Assigned + " where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTicketResize(MapData objMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objMapData.ConnConfig, CommandType.Text, "update TicketO set edate='" + objMapData.Date + "',est=" + objMapData.Resize + " where ID=" + objMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTicket(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraticketout = new SqlParameter();
                paraticketout.ParameterName = "TicketIDOut";
                paraticketout.SqlDbType = SqlDbType.Int;
                paraticketout.Direction = ParameterDirection.Output;
                int ticid;

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";

                ticid = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddTicket", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, DBNull.Value, paraEquip));
                //objMapData.TicketID =Convert.ToInt32( paraticketout.Value);
                objMapData.TicketID = ticid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTicketTS(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraticketout = new SqlParameter();
                paraticketout.ParameterName = "TicketIDOut";
                paraticketout.SqlDbType = SqlDbType.Int;
                paraticketout.Direction = ParameterDirection.Output;
                int ticid;

                ticid = Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spAddTicketTS", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.EST, objMapData.CompDescription, paraticketout, System.Guid.NewGuid(), objMapData.Who, objMapData.Signature, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Remarks, objMapData.Level, objMapData.Department, objMapData.jobid, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.IsRecurring, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobcode, objMapData.JobTemplateID, objMapData.fBy));
                //objMapData.TicketID =Convert.ToInt32( paraticketout.Value);
                objMapData.TicketID = ticid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTicketInfo(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                SqlParameter paraEquip = new SqlParameter();
                paraEquip.ParameterName = "Equipments";
                paraEquip.SqlDbType = SqlDbType.Structured;
                paraEquip.Value = objMapData.dtEquips;
                paraEquip.TypeName = "tblTypeMultipleEequipments";
                
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicket", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.TicketID, objMapData.EST, objMapData.CompDescription, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Who, objMapData.Signature, objMapData.Remarks, objMapData.Department, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobid, objMapData.jobcode, objMapData.JobTemplateID, objMapData.WageID, objMapData.fBy, paraEquip));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateTicketInfoTS(MapData objMapData)
        {
            try
            {
                SqlParameter paraSchDate = new SqlParameter();
                paraSchDate.ParameterName = "SchDt";
                paraSchDate.SqlDbType = SqlDbType.DateTime;
                if (objMapData.SchDate == System.DateTime.MinValue)
                {
                    paraSchDate.Value = DBNull.Value;
                }
                else
                {
                    paraSchDate.Value = objMapData.SchDate;
                }

                SqlParameter paraEntime = new SqlParameter();
                paraEntime.ParameterName = "EnrouteTime";
                paraEntime.SqlDbType = SqlDbType.DateTime;
                if (objMapData.EnrouteTime == System.DateTime.MinValue)
                {
                    paraEntime.Value = DBNull.Value;
                }
                else
                {
                    paraEntime.Value = objMapData.EnrouteTime;
                }

                SqlParameter paraOnsite = new SqlParameter();
                paraOnsite.ParameterName = "Onsite";
                paraOnsite.SqlDbType = SqlDbType.DateTime;
                if (objMapData.OnsiteTime == System.DateTime.MinValue)
                {
                    paraOnsite.Value = DBNull.Value;
                }
                else
                {
                    paraOnsite.Value = objMapData.OnsiteTime;
                }

                SqlParameter paraComp = new SqlParameter();
                paraComp.ParameterName = "Complete";
                paraComp.SqlDbType = SqlDbType.DateTime;
                if (objMapData.ComplTime == System.DateTime.MinValue)
                {
                    paraComp.Value = DBNull.Value;
                }
                else
                {
                    paraComp.Value = objMapData.ComplTime;
                }

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objMapData.ConnConfig, "spUpdateTicketTS", objMapData.LocID, objMapData.LocTag, objMapData.LocAddress, objMapData.City, objMapData.State, objMapData.Zip, objMapData.Phone, objMapData.Cell, objMapData.Worker, objMapData.CallDate, paraSchDate, objMapData.Assigned, paraEntime, paraOnsite, paraComp, objMapData.Category, objMapData.Unit, objMapData.Reason, objMapData.CustomerName, objMapData.CustID, objMapData.TicketID, objMapData.EST, objMapData.CompDescription, objMapData.RT, objMapData.OT, objMapData.NT, objMapData.TT, objMapData.DT, objMapData.Total, objMapData.Charge, objMapData.Review, objMapData.Who, objMapData.Signature, objMapData.Remarks, objMapData.Department, objMapData.Custom1, objMapData.Custom2, objMapData.Custom3, objMapData.Custom4, objMapData.Custom5, objMapData.Custom6, objMapData.Custom7, objMapData.Workorder, objMapData.WorkComplete, objMapData.MiscExpense, objMapData.TollExpense, objMapData.ZoneExpense, objMapData.MileStart, objMapData.MileEnd, objMapData.Internet, objMapData.ManualInvoiceID, objMapData.TimeTransfer, objMapData.CreditHold, objMapData.DispAlert, objMapData.CreditReason, objMapData.QBServiceID, objMapData.QBPayrollID, objMapData.LastUpdatedBy, objMapData.MainContact, objMapData.Recommendation, objMapData.CustomTick1, objMapData.CustomTick2, objMapData.CustomTick3, objMapData.CustomTick4, objMapData.Lat, objMapData.Lng, objMapData.DefaultWorker, objMapData.CustomTick5, objMapData.jobid, objMapData.jobcode, objMapData.JobTemplateID, objMapData.fBy));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTimestmpLocation(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocation", objPropMapData.Tech, objPropMapData.Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getlocationAddress(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "SpgetlocationLatlong", objPropMapData.Tech, objPropMapData.Date, objPropMapData.CallDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenTicket(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct t.id, assigned, ldesc1, edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address  from TicketO t where DWork='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' and Assigned not in (0) union select distinct  d.ID ,4 as assigned ,l.ID as ldesc1 ,edate,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address  from TicketD d inner join tblWork w on d.fWork=w.ID inner join Loc l on d.Loc=l.Loc where w.fDesc='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' order by EDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetOpenTicketScheduler(MapData objPropMapData)
        {
            //select distinct 0 as comp, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname, t.phone, edate, t.id, assigned, (ldesc1+' - '+ldesc2) as name,est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , UPPER( dwork) as [column], 0 as allday, case when Assigned=1 then 'White' when Assigned=2 then 'Green' when Assigned=3 then 'orange' when Assigned=4 then 'blue' when Assigned=5 then 'yellow' end as color, case  when Assigned in (4,3,2) then TimeRoute else EDate end as start, case  when Assigned= 4 then TimeComp when Assigned in (3,2) then dateadd(MINUTE,Est*60 ,TimeRoute)  else dateadd(MINUTE,Est*60 ,edate) end as [end] from TicketO t inner join tblUser u on t.DWork=u.fUser where Assigned not in (0,4)
            //union all select distinct 1 as comp, 'Completed' as assignname,'' as phone, d.edate, d.id, 4 as assigned,(l.ID+' - '+l.Tag) as name, d.Est,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address , UPPER(w.fDesc) as [column], 0 as allday, 'blue' as color, d.TimeRoute as start, d.TimeComp as [end] from TicketD d inner join Loc l on l.Loc=d.Loc inner join tblWork w on d.fWork=w.ID
            //union all select distinct 0 as comp, 'Completed' as assignname, t.phone, dp.edate, t.id, 4 as assigned,(ldesc1+' - '+ldesc2) as name,dp.Est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , UPPER( dwork) as [column], 0 as allday, 'blue' as color, dp.TimeRoute as start, dp.TimeComp as [end] from TicketDPDA dp inner join TicketO t on t.ID= dp.ID inner join tblWork w on dp.fWork=w.ID
            try
            {
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("SELECT DISTINCT ''                                                         AS descres, CONVERT(VARCHAR(MAX), t.fdesc) as fdesc , \n");
                varname1.Append("                t.cat, \n");
                varname1.Append("                Isnull(Confirmed, 0)                                       AS Confirmed, \n");
                varname1.Append("                0                                                          AS comp, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned = 1 THEN 'Assigned' \n");
                varname1.Append("                  WHEN Assigned = 2 THEN 'Enroute' \n");
                varname1.Append("                  WHEN Assigned = 3 THEN 'Onsite' \n");
                varname1.Append("                  WHEN Assigned = 4 THEN 'Completed' \n");
                varname1.Append("                  WHEN Assigned = 5 THEN 'Hold' \n");
                varname1.Append("                END                                                        AS assignname, \n");
                varname1.Append("                t.phone, \n");
                varname1.Append("                edate, \n");
                varname1.Append("                t.id, \n");
                varname1.Append("                assigned, \n");
                varname1.Append("                ( 'Ticket #: ' + CONVERT(VARCHAR(50), t.id ) \n");
                varname1.Append("                  + ', ' + ldesc1 + ' - ' + ldesc2 )                       AS name, \n");
                varname1.Append("                est, \n");
                varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
                varname1.Append("                Upper(dwork)                                               AS [column], \n");
                varname1.Append("                0                                                          AS allday, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned = 1 THEN 'White' \n");
                varname1.Append("                  WHEN Assigned = 2 THEN '#9EF767' \n");
                varname1.Append("                  WHEN Assigned = 3 THEN 'orange' \n");
                varname1.Append("                  WHEN Assigned = 4 THEN 'DeepSkyBlue' \n");
                varname1.Append("                  WHEN Assigned = 5 THEN 'yellow' \n");
                varname1.Append("                END                                                        AS color, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned IN ( 4, 3, 2 ) THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                                    +cast( Cast(Isnull( TimeRoute, '7/9/2012 12:00:00 AM') AS TIME)as datetime) \n");
                varname1.Append("                  ELSE EDate \n");
                varname1.Append("                END                                                        AS start, \n");
                varname1.Append("                CASE \n");
                varname1.Append("                  WHEN Assigned = 4 THEN Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                varname1.Append("                                         +Cast( Cast(Isnull( TimeComp, '7/9/2012 12:01:00 AM') AS TIME)as datetime) \n");
                varname1.Append("                  WHEN assigned = 3 THEN Cast(Cast(edate AS DATE) AS DATETIME) \n");
                varname1.Append("                                         +Cast( Cast(Isnull( TimeSite, '7/9/2012 12:01:00 AM') AS TIME)as datetime) \n");
                varname1.Append("                  WHEN assigned = 2 THEN Dateadd(MINUTE, est * 60, Cast( Cast(edate AS DATE) AS DATETIME ) \n");
                varname1.Append("                                                                   +Cast( Cast(Isnull( TimeRoute, '7/9/2012 12:01:00 AM') AS TIME)as datetime)) \n");
                varname1.Append("                  ELSE Dateadd(MINUTE, isnull(Est,0) * 60, edate) \n");
                varname1.Append("                END                                                        AS [end], \n");
                varname1.Append("                0                                                          AS ClearCheck, \n");
                varname1.Append("                (SELECT Count(1) \n");
                varname1.Append("                 FROM   documents \n");
                varname1.Append("                 WHERE  screen = 'Ticket' \n");
                varname1.Append("                        AND screenid = t.id)                               AS DocumentCount, \n");
                varname1.Append("                t.fwork                                                    AS workerid, \n");
                varname1.Append("                0                                                          AS invoice, \n");
                varname1.Append("                0                                                          AS charge, \n");
                varname1.Append("                ''                                                         AS manualinvoice, \n");
                varname1.Append("                ''                                                         AS qbinvoiceid, \n");
                varname1.Append("                owner                                                      AS ownerid, \n");
                varname1.Append("               (select ISNULL(Credit,0) from Loc l where l.Loc=t.LID)as credithold, \n");
                varname1.Append("               (select ISNULL(DispAlert,0) from Loc l where l.Loc=t.LID)as DispAlert \n");
                varname1.Append("FROM   TicketO t \n");
                varname1.Append("       LEFT OUTER JOIN tblWork w \n");
                varname1.Append("                    ON w.fDesc = t.DWork \n");
                varname1.Append("WHERE  Assigned NOT IN ( 0, 4 ) ");


                //string str = "   select distinct '' as descres, t.cat,  isnull(Confirmed,0) as Confirmed, 0 as comp, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname, t.phone, edate, t.id, assigned, ('Ticket #: '+convert(varchar(50), t.id )+', '+ldesc1+' - '+ldesc2) as name,est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , UPPER( dwork) as [column], 0 as allday, case when Assigned=1 then 'White' when Assigned=2 then '#9EF767' when Assigned=3 then 'orange' when Assigned=4 then 'DeepSkyBlue' when Assigned=5 then 'yellow' end as color, case  when Assigned in (4,3,2) then CAST(CAST(EDate AS DATE) AS DATETIME) + CAST(TimeRoute AS TIME) else EDate end as start, case  when Assigned= 4 then CAST(CAST(EDate AS DATE) AS DATETIME) + CAST(TimeComp AS TIME)  WHEN assigned = 3 THEN CAST(CAST(edate AS DATE) AS DATETIME) + CAST(TimeSite AS TIME) WHEN assigned = 2 THEN Dateadd(MINUTE, est * 60, CAST( CAST(edate AS DATE) AS DATETIME ) +  CAST( TimeRoute AS TIME))  else dateadd(MINUTE,Est*60 ,edate) end as [end], 0 as ClearCheck, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, 0 as invoice, 0 as charge, '' as manualinvoice, owner as ownerid from TicketO t left outer join tblWork w on w.fDesc=t.DWork where Assigned not in (0,4)   ";
                string str = varname1.ToString();

                str += " and t.ID is not null ";

                if (objPropMapData.Worker != string.Empty)
                {
                    str += " and DWork='" + objPropMapData.Worker + "'";
                }

                if (objPropMapData.Assigned != -1)
                {
                    str += " and Assigned=" + objPropMapData.Assigned;
                }

                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    str += " and w.super ='" + objPropMapData.Supervisor + "'";
                }

                if (objPropMapData.StartDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) >='" + objPropMapData.StartDate + "'";
                }

                if (objPropMapData.EndDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) <='" + objPropMapData.EndDate + "'";
                }

                if (objPropMapData.Department != 0)
                {
                    str += " and t.type=" + objPropMapData.Department;
                }

                if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
                {
                    // str += " union all    select distinct CONVERT(VARCHAR(MAX), descres) as descres, d.cat,  0 as Confirmed, 1 as comp, 'Completed' as assignname,'' as phone, d.edate, d.id, 4 as assigned,('Ticket #: '+convert(varchar(50), d.id ) +', '+l.ID+' - '+l.Tag) as name, d.Est,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address , UPPER(w.fDesc) as [column], 0 as allday, 'DeepSkyBlue' as color, CAST(CAST(EDate AS DATE) AS DATETIME) + CAST(d.TimeRoute AS TIME)  as start,CAST(CAST(EDate AS DATE) AS DATETIME) + CAST(d.TimeComp AS TIME)  as [end], isnull(ClearCheck, 0) as ClearCheck , (select count(1) from documents where screen='Ticket' and screenid=d.id) as DocumentCount, d.fwork as workerid, isnull( invoice,0) as invoice, charge, manualinvoice, 0 as ownerid from TicketD d inner join Loc l on l.Loc=d.Loc inner join tblWork w on d.fWork=w.ID ";

                    str += " union all ";

                    StringBuilder varname2 = new StringBuilder();
                    varname2.Append("SELECT DISTINCT CONVERT(VARCHAR(MAX), descres)                                AS descres, CONVERT(VARCHAR(MAX), d.fdesc) as fdesc , \n");
                    varname2.Append("                d.cat, \n");
                    varname2.Append("                0                                                             AS Confirmed, \n");
                    varname2.Append("                1                                                             AS comp, \n");
                    varname2.Append("                'Completed'                                                   AS assignname, \n");
                    varname2.Append("                ''                                                            AS phone, \n");
                    varname2.Append("                d.edate, \n");
                    varname2.Append("                d.id, \n");
                    varname2.Append("                4                                                             AS assigned, \n");
                    varname2.Append("                ( 'Ticket #: ' + CONVERT(VARCHAR(50), d.id ) \n");
                    varname2.Append("                  + ', ' + l.ID + ' - ' + l.Tag )                             AS name, \n");
                    varname2.Append("                d.Est, \n");
                    varname2.Append("                ( l.Address + ', ' + l.City + ', ' + l.State + ', ' + l.Zip ) AS address, \n");
                    varname2.Append("                Upper(w.fDesc)                                                AS [column], \n");
                    varname2.Append("                0                                                             AS allday, \n");
                    varname2.Append("                'DeepSkyBlue'                                                 AS color, \n");
                    varname2.Append("                CASE \n");
                    varname2.Append("                  WHEN Cast(Isnull(TimeRoute, '7/9/2012 12:00:00 AM') AS TIME) = Cast('7/9/2012 12:00:00 AM' AS TIME) THEN edate \n");
                    varname2.Append("                  ELSE Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                    varname2.Append("                       + Cast( Cast( TimeRoute AS TIME)as datetime) \n");
                    varname2.Append("                END                                                           AS start, \n");
                    varname2.Append("                CASE \n");
                    varname2.Append("                  WHEN Cast(Isnull(TimeComp, '7/9/2012 12:00:00 AM') AS TIME) = Cast('7/9/2012 12:00:00 AM' AS TIME) THEN Dateadd(MINUTE, isnull(total,0) * 60, edate) \n");
                    varname2.Append("                  ELSE Cast(Cast(EDate AS DATE) AS DATETIME) \n");
                    varname2.Append("                       + Cast(Cast( TimeComp AS TIME)AS DATETIME) \n");
                    varname2.Append("                END                                                           AS [end], \n");
                    varname2.Append("                Isnull(ClearCheck, 0)                                         AS ClearCheck, \n");
                    varname2.Append("                (SELECT Count(1) \n");
                    varname2.Append("                 FROM   documents \n");
                    varname2.Append("                 WHERE  screen = 'Ticket' \n");
                    varname2.Append("                        AND screenid = d.id)                                  AS DocumentCount, \n");
                    varname2.Append("                d.fwork                                                       AS workerid, \n");
                    varname2.Append("                Isnull(invoice, 0)                                            AS invoice, \n");
                    varname2.Append("                charge, \n");
                    varname2.Append("                manualinvoice, \n");
                    varname2.Append("               isnull( qbinvoiceid,'') as qbinvoiceid, \n");
                    varname2.Append("                0                                                             AS ownerid, \n");
                    varname2.Append("                l.credit as  credithold,l.DispAlert \n");
                    varname2.Append("FROM   TicketD d \n");
                    varname2.Append("       INNER JOIN Loc l \n");
                    varname2.Append("               ON l.Loc = d.Loc \n");
                    varname2.Append("       INNER JOIN tblWork w \n");
                    varname2.Append("               ON d.fWork = w.ID ");


                    str += varname2.ToString();

                    str += " where d.ID is not null ";//and TimeComp is not null and TimeRoute is not null
                    if (objPropMapData.Worker != string.Empty)
                    {
                        str += " and w.fdesc='" + objPropMapData.Worker + "'";
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and w.super ='" + objPropMapData.Supervisor + "'";
                    }

                    if (objPropMapData.StartDate != DateTime.MinValue)
                    {
                        str += " and DATEADD(d, 0, DATEDIFF(d, 0, d.edate)) >='" + objPropMapData.StartDate + "'";
                    }

                    if (objPropMapData.EndDate != DateTime.MinValue)
                    {
                        str += " and DATEADD(d, 0, DATEDIFF(d, 0, d.edate)) <='" + objPropMapData.EndDate + "'";
                    }

                    if (objPropMapData.Department != 0)
                    {
                        str += " and d.type=" + objPropMapData.Department;
                    }
                }


                // str += " union all select distinct CONVERT(VARCHAR(MAX), descres) as descres, t.cat, 1 as Confirmed, 2 as comp, 'Completed' as assignname, t.phone, dp.edate, t.id, 4 as assigned,('Ticket #: '+convert(varchar(50), t.id ) +', '+ldesc1+' - '+ldesc2) as name,dp.Est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , UPPER( dwork) as [column], 0 as allday, 'DeepSkyBlue' as color, CAST(CAST(dp.EDate AS DATE) AS DATETIME) + CAST(dp.TimeRoute AS TIME)  as start, CAST(CAST(dp.EDate AS DATE) AS DATETIME) + CAST(dp.TimeComp AS TIME)  as [end], 0 as ClearCheck, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, 0 as invoice, charge, '' as manualinvoice, t.owner as ownerid from TicketDPDA dp inner join TicketO t on t.ID= dp.ID inner join tblWork w on dp.fWork=w.ID ";
                str += " union all ";

                StringBuilder varname3 = new StringBuilder();
                varname3.Append("SELECT DISTINCT CONVERT(VARCHAR(MAX), descres)                                AS descres, CONVERT(VARCHAR(MAX), dp.fdesc) as fdesc , \n");
                varname3.Append("                t.cat, \n");
                varname3.Append("                1                                                             AS Confirmed, \n");
                varname3.Append("                2                                                             AS comp, \n");
                varname3.Append("                'Completed'                                                   AS assignname, \n");
                varname3.Append("                t.phone, \n");
                varname3.Append("                dp.edate, \n");
                varname3.Append("                t.id, \n");
                varname3.Append("                4                                                             AS assigned, \n");
                varname3.Append("                ( 'Ticket #: ' + CONVERT(VARCHAR(50), t.id ) \n");
                varname3.Append("                  + ', ' + ldesc1 + ' - ' + ldesc2 )                          AS name, \n");
                varname3.Append("                dp.Est, \n");
                varname3.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip )    AS address, \n");
                varname3.Append("                Upper(dwork)                                                  AS [column], \n");
                varname3.Append("                0                                                             AS allday, \n");
                varname3.Append("                'DeepSkyBlue'                                                 AS color, \n");
                varname3.Append("                Cast(Cast(dp.EDate AS DATE) AS DATETIME) \n");
                varname3.Append("                + Cast(Cast(Isnull( dp.TimeRoute, '7/9/2012 12:00:00 AM') AS TIME)as datetime) AS start, \n");
                varname3.Append("                Cast(Cast(dp.EDate AS DATE) AS DATETIME) \n");
                varname3.Append("                + Cast(Cast(Isnull( dp.TimeComp, '7/9/2012 12:01:00 AM') AS TIME)as datetime)  AS [end], \n");
                varname3.Append("                0                                                             AS ClearCheck, \n");
                varname3.Append("                (SELECT Count(1) \n");
                varname3.Append("                 FROM   documents \n");
                varname3.Append("                 WHERE  screen = 'Ticket' \n");
                varname3.Append("                        AND screenid = t.id)                                  AS DocumentCount, \n");
                varname3.Append("                t.fwork                                                       AS workerid, \n");
                varname3.Append("                0                                                             AS invoice, \n");
                varname3.Append("                charge, \n");
                varname3.Append("                ''                                                            AS manualinvoice, \n");
                varname3.Append("                ''                                                            AS qbinvoiceid, \n");
                varname3.Append("                t.owner                                                       AS ownerid, \n");
                varname3.Append("               (select ISNULL(Credit,0) from Loc l where l.Loc=t.LID)as credithold, \n");
                varname3.Append("               (select ISNULL(DispAlert,0) from Loc l where l.Loc=t.LID)as DispAlert \n");
                varname3.Append("FROM   TicketDPDA dp \n");
                varname3.Append("       INNER JOIN TicketO t \n");
                varname3.Append("               ON t.ID = dp.ID \n");
                varname3.Append("       INNER JOIN tblWork w \n");
                varname3.Append("               ON dp.fWork = w.ID ");

                str += varname3.ToString();

                str += " where t.ID is not null "; //and dp.TimeComp is not null and dp.TimeRoute is not null
                if (objPropMapData.Worker != string.Empty)
                {
                    str += " and DWork='" + objPropMapData.Worker + "'";
                }

                if (objPropMapData.Assigned != -1)
                {
                    str += " and Assigned=" + objPropMapData.Assigned;
                }
                if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                {
                    str += " and w.super ='" + objPropMapData.Supervisor + "'";
                }
                if (objPropMapData.StartDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) >='" + objPropMapData.StartDate + "'";
                }

                if (objPropMapData.EndDate != DateTime.MinValue)
                {
                    str += " and DATEADD(d, 0, DATEDIFF(d, 0, t.edate)) <='" + objPropMapData.EndDate + "'";
                }

                if (objPropMapData.Department != 0)
                {
                    str += " and t.type=" + objPropMapData.Department;
                }

                str += " order by EDate";

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);//Assigned not in (0,4)

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCallHistoryLocationOLD(MapData objPropMapData)
        {
            string str = "SELECT fdesc,timeroute, timesite,timecomp, case when exists ( select 1 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp, dwork, id, (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = t.owner)) AS customername, t.ldesc2 AS locname, t.ldesc4 AS address, phone, cat, edate, cdate, (select top 1 descres from TicketDPDA where ID=t.ID) AS descres, CASE WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, est,isnull( (select top 1 isnull(Total,0.00) from TicketDPDA where ID=t.ID),0.00)as Total FROM ticketo t WHERE assigned NOT IN (0) and t.LID=" + objPropMapData.LocID;

            if (objPropMapData.Assigned != -1)
            {
                str += " and Assigned=" + objPropMapData.Assigned;
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                str += " and edate >='" + objPropMapData.StartDate + "'";
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
            }

            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)
            {
                str += " UNION ALL SELECT t.fdesc,timeroute, timesite,timecomp, 1 as comp, w.fdesc AS dwork, t.id, (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = l.owner)) AS customername, l.tag AS locname, l.address, '' AS phone, '' AS cat, edate, cdate, descres, 'Completed' AS assignname, est,Total FROM ticketd t INNER JOIN loc l ON l.loc = t.loc INNER JOIN tblwork w ON t.fwork = w.id WHERE l.Loc=" + objPropMapData.LocID;

                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
                }
            }

            str += " order by edate desc";

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCallHistory(MapData objPropMapData)
        {
            string str = "";

            //if (objPropMapData.Mobile != 1)
            //{
            if (objPropMapData.Status != 1)
            {
                if (objPropMapData.FilterReview != "1")
                {
                    str = "SELECT t.who, t.lid, l.id as locid, assigned, ( l.address + ', ' + l.city + ', ' + l.state + ', ' + l.zip ) AS fulladdress, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT, dp.Total, 0 AS ClearCheck, charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, dwork, dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, ";
                    str += " l.Tag  AS locname, l.Address  AS address, t.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime, Isnull(dp.Total, 0.00) - DATEDIFF(HOUR,dp.TimeRoute,dp.TimeComp ) as timediff, t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                    str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, isnull(t.high,0) as high,e.id as unitid, e.unit, (select Name from Route where ID=l.Route ) as defaultworker";
                    str += ", (select type from jobtype where id = t.type) as department, t.bremarks, 0 as laborexp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature, l.state ";
                    str += ", (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join Loc l on l.Loc=t.lid  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol  left outer join Elev e on e.ID=t.LElev WHERE t.id is not null and t.owner is not null "; //assigned NOT IN ( 0 )

                    if (objPropMapData.IsList != 1)
                    {
                        str += " and assigned NOT IN ( 0 )";
                    }

                    if (objPropMapData.Assigned != -1)//&& objPropMapData.Assigned != 0
                    {
                        if (objPropMapData.Assigned == -2)
                        {
                            str += " and t.Assigned <> 4";
                        }
                        else
                        {
                            str += " and t.Assigned=" + objPropMapData.Assigned;
                        }
                    }
                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {
                        str += " and t.DWork='" + objPropMapData.Worker + "'";
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and t.LID=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and t.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    //if (objPropMapData.FilterReview == "1")
                    //{
                    //    str += " and charge =9";                   
                    //}                
                    if (objPropMapData.Mobile == 2)
                    {
                        str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                    }
                    if (objPropMapData.Mobile == 1)
                    {
                        str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                    {
                        str += " and t.cat='" + objPropMapData.Category + "'";
                    }
                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull(t.bremarks,'')<>''";
                        else
                            str += " and isnull(t.bremarks,'')=''";
                    }

                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                    {
                        string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                        if (SearchBy == "t.ID")
                        {
                            str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                        }
                        else
                        {
                            str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }

                    if (objPropMapData.InvoiceID != 0 )
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(charge,0)= 1";
                        }
                    }

                    //}

                    if (objPropMapData.LocID == 0)
                    {
                        str += " Union all ";
                        str += " SELECT t.who, t.lid, '--' as locid, assigned, ( r.address + ', ' + r.city + ', ' + r.state + ', ' + r.zip ) AS fulladdress, t.WorkOrder, dp.Reg, dp.OT, dp.NT, dp.DT, dp.TT, dp.Total, 0 AS ClearCheck, charge, t.fDesc, t.TimeRoute, t.TimeSite, t.TimeComp, CASE WHEN EXISTS (SELECT 1 FROM TicketDPDA WHERE ID = t.ID) THEN 2 ELSE 0 END AS comp, dwork,dwork as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.ID, r.Name AS customername, ";
                        str += " r.name  AS locname, l.Address  AS address, t.phone, t.Cat, t.EDate AS edate, t.CDate, dp.descres, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, Isnull(dp.Total, 0.00) AS Tottime, Isnull(dp.Total, 0.00) - DATEDIFF(HOUR,dp.TimeRoute,dp.TimeComp ) as timediff, t.workorder, (isnull(dp.zone,0)+ isnull(dp.toll,0) + isnull(dp.othere,0)) as expenses, isnull( dp.zone,0) as zone, isnull( dp.toll,0) as toll , isnull(dp.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(dp.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(dp.custom2)) else 0 end as extraexp, ((isnull(dp.emile,0)-isnull(dp.smile,0))*0.26) as mileagetravel, ";
                        str += " (isnull(dp.emile,0)-isnull(dp.smile,0)) as mileage ,(select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount, (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description, (select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, 0 as invoice, isnull(Confirmed,0) as Confirmed, '' as manualinvoice, '' as invoiceno, t.owner as ownerid, '' as QBinvoiceid, 0 as TransferTime, ";
                        str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                        str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                        str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                        if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                        }
                        str += ", 0 as dispalert, 0 as credithold, 0 as high,e.id as unitid, e.unit, '' as defaultworker";
                        str += ", (select type from jobtype where id = t.type) as department, t.bremarks, 0 as laborexp ";
                        str += ", (select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                        str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                        str += " FROM ticketo t LEFT OUTER JOIN TicketDPDA dp ON t.ID = dp.ID inner join prospect l on l.ID=t.lid INNER JOIN Rol r ON r.ID = l.Rol  left outer join Elev e on e.ID=t.LElev WHERE t.id is not null and t.owner is null and t.LType=1 ";

                        if (objPropMapData.IsList != 1)
                        {
                            str += " and assigned NOT IN ( 0 )";
                        }

                        if (objPropMapData.Assigned != -1)
                        {
                            if (objPropMapData.Assigned == -2)
                            {
                                str += " and t.Assigned <> 4";
                            }
                            else
                            {
                                str += " and t.Assigned=" + objPropMapData.Assigned;
                            }
                        }
                        if (objPropMapData.StartDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) >='" + objPropMapData.StartDate + "'";
                        }
                        if (objPropMapData.EndDate != System.DateTime.MinValue)
                        {
                            str += " and isnull(t.edate,t.cdate) <'" + objPropMapData.EndDate.AddDays(1) + "'";
                        }
                        if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                        {
                            str += " and t.DWork='" + objPropMapData.Worker + "'";
                        }
                        if (objPropMapData.LocID != 0)
                        {
                            str += " and t.LID=" + objPropMapData.LocID;
                        }
                        if (objPropMapData.CustID != 0)
                        {
                            str += " and t.Owner=" + objPropMapData.CustID;
                        }
                        if (objPropMapData.jobid != 0)
                        {
                            str += " and t.job =" + objPropMapData.jobid;
                        }
                        if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                        {
                            str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                        }
                        if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) ='" + objPropMapData.Supervisor + "'";
                        }
                        if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                        {
                            str += " and (select Super from tblWork w where w.fdesc=t.dwork ) <>'" + objPropMapData.NonSuper + "'";
                        }
                        if (objPropMapData.Mobile == 2)
                        {
                            str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=2";
                        }
                        if (objPropMapData.Mobile == 1)
                        {
                            str += " and (select case when exists ( select 01 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp)=0";
                        }
                        if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                        {
                            str += " and t.workorder='" + objPropMapData.Workorder + "'";
                        }
                        if (objPropMapData.Department != -1)
                        {
                            str += " and t.type=" + objPropMapData.Department;
                        }
                        if (objPropMapData.Category != string.Empty && objPropMapData.Category != null)
                        {
                            str += " and t.cat='" + objPropMapData.Category+"'";
                        }
                        if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                        {
                            if (objPropMapData.Bremarks == "1")
                                str += " and isnull(t.bremarks,'')<>''";
                            else
                                str += " and isnull(t.bremarks,'')=''";
                        }
                        if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                        {
                            string SearchBy = objPropMapData.SearchBy.Replace("t.descres", "dp.descres");
                            if (SearchBy == "t.ID")
                            {
                                str += " and " + SearchBy + " = '" + objPropMapData.SearchValue + "'";
                            }
                            else
                            {
                                if (SearchBy == "l.tag")
                                    SearchBy = "r.name";
                                str += " and " + SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                            }
                        }
                        if (objPropMapData.InvoiceID != 0)
                        {
                            if (objPropMapData.InvoiceID == 1)
                            {
                                str += " and isnull(Invoice,0) <> 0";
                            }
                            else if (objPropMapData.InvoiceID == 2)
                            {
                                str += " and isnull(Invoice,0) = 0 and isnull(charge,0)= 1";
                            }
                        }
                    }



                }
            }
            if (objPropMapData.Assigned == 4 || objPropMapData.Assigned == -1)//|| objPropMapData.Assigned == 0
            {
                if (objPropMapData.Mobile != 2)
                {
                    //if (objPropMapData.Mobile == 0)
                    //{
                    if (objPropMapData.Status != 1)
                    {
                        if (objPropMapData.FilterReview != "1")
                        {
                            str += " UNION ALL";
                        }
                    }
                    //}
                    //if (objPropMapData.Mobile != 2)
                    //{
                    str += " SELECT t.who, t.loc as lid, l.id as locid, 4 as assigned, (l.address+', '+l.city+', '+l.state+', '+l.zip) as fulladdress, t.WorkOrder, Reg, OT, NT, DT,TT, Total,isnull( ClearCheck ,0) as ClearCheck ,charge, t.fdesc,timeroute, timesite,timecomp, 1 as comp, (select w.fdesc from tblWork w where t.fwork = w.id) AS dwork,(select w.fdesc from tblWork w where t.fwork = w.id) as lastname,(select isnull(hourlyrate,0) from tblwork where id=t.fwork) as hourlyrate, t.id,  r.Name  AS customername, l.tag AS locname, l.address, (select top 1 Phone from rol where ID=l.Rol) AS phone,  t.cat, edate, cdate, descres, 'Completed' AS assignname, est,Total as tottime , Isnull(Total, 0.00) - DATEDIFF(HOUR,TimeRoute,TimeComp ) as timediff, t.workorder, (isnull(t.zone,0)+ isnull(t.toll,0) + isnull(t.othere,0)) as expenses, isnull( t.zone,0) as zone, isnull( t.toll,0) as toll , isnull(t.othere,0) as othere, case ISNUMERIC(dbo.udf_GetNumeric(t.custom2)) when 1 then CONVERT(money ,dbo.udf_GetNumeric(t.custom2)) else 0 end as extraexp, ((isnull(t.emile,0)-isnull(t.smile,0))*0.26) as mileagetravel, (isnull(t.emile,0)-isnull(t.smile,0)) as mileage, (select top 1 count(1) as signatureCount  from pdaticketsignature where pdaticketid=t.ID ) as signatureCount,  (select count(1) from documents where screen='Ticket' and screenid=t.id) as DocumentCount, t.fwork as workerid, (select top 1 * from dbo.split(descres,'|')) as description,(select top 1 * from dbo.split(t.fdesc,'|')) as fdescreason, isnull(invoice,0) as invoice, 0 as Confirmed,  manualinvoice, case  when ( Isnull(invoice, 0) =  0 ) then Manualinvoice else CONVERT(varchar(50), Invoice) end as invoiceno, 0 as ownerid, isnull(QBinvoiceID,'')as QBinvoiceid, isnull(TransferTime,0) as TransferTime, ";
                    str += "  (select name from inv where qbinvid= t.QBserviceitem) as serviceitem, ";
                    str += "  (select fdesc from prwage where QBwageID= t.QBPayrollItem) as PayrollItem, ";
                    str += "  (ISNULL( reg ,0) + ISNULL( OT ,0) +ISNULL( TT ,0))     as RTOTTT ";
                    if (objPropMapData.StartDate != System.DateTime.MinValue || objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " ,(SELECT top 1 Signature	FROM   PDATimeSign p	WHERE  p.fWork = t.fwork and (p.EDate BETWEEN '" + objPropMapData.StartDate + "' and '" + objPropMapData.EndDate + "')	ORDER  BY EDate DESC ) as timesign";
                    }
                    str += ", isnull(l.dispalert,0)as dispalert, isnull(l.credit,0)as credithold, 0 as high ,e.id as unitid , e.unit, (select Name from Route where ID=l.Route ) as defaultworker";
                    str += ", (select type from jobtype where id = t.type) as department,t.bremarks ";
                    str += ", CONVERT(NUMERIC(30, 2),(((isnull(t.Reg,0) + isnull(t.RegTrav,0)) +  ((isnull(t.OT,0) + isnull(t.OTTrav,0)) * 1.5) + ((isnull(t.DT,0) + isnull(t.DTTrav,0)) * 2) + ((isnull(t.NT,0) + isnull(t.NTTrav,0)) * 1.7) + (isnull(t.TT,0))) * (SELECT Isnull(w.HourlyRate, 0)FROM   tblWork w WHERE  w.ID = t.fWork))) AS LaborExp ";
                    str += ", (select top 1 signature from pdaticketsignature where pdaticketid=t.ID ) as signature , l.state";
                    str += ",  (select top 1 isnull( Rate,0)  from PROther where Emp = (select top 1 id from emp where fWork = t.fwork) and Cat = 5) as mileagepr ";
                    str += " FROM ticketd t INNER JOIN loc l ON l.loc = t.loc  inner join Owner o on l.Owner=o.ID INNER JOIN Rol r ON r.ID = o.Rol   left outer join Elev e on e.ID=t.Elev WHERE t.id is not null ";

                    if (objPropMapData.StartDate != System.DateTime.MinValue)
                    {
                        str += " and edate >='" + objPropMapData.StartDate + "'";
                    }
                    if (objPropMapData.EndDate != System.DateTime.MinValue)
                    {
                        str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                    }
                    if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                    {
                        str += " and (select w.fdesc from tblWork w where w.ID=t.fwork )='" + objPropMapData.Worker + "'";
                    }
                    if (objPropMapData.LocID != 0)
                    {
                        str += " and l.loc=" + objPropMapData.LocID;
                    }
                    if (objPropMapData.CustID != 0)
                    {
                        str += " and l.Owner=" + objPropMapData.CustID;
                    }
                    if (objPropMapData.jobid != 0)
                    {
                        str += " and t.job =" + objPropMapData.jobid;
                    }
                    if (objPropMapData.FilterCharge != null && objPropMapData.FilterCharge != string.Empty)
                    {
                        str += " and isnull(charge,0)=" + Convert.ToInt32(objPropMapData.FilterCharge);
                    }
                    if (objPropMapData.Timesheet != null && objPropMapData.Timesheet != string.Empty)
                    {
                        str += " and isnull(TransferTime,0)=" + Convert.ToInt32(objPropMapData.Timesheet);
                    }
                    if (objPropMapData.Supervisor != null && objPropMapData.Supervisor != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) ='" + objPropMapData.Supervisor + "'";
                    }
                    if (objPropMapData.NonSuper != null && objPropMapData.NonSuper != string.Empty)
                    {
                        str += " and (select Super from tblWork w where w.ID=t.fwork ) <>'" + objPropMapData.NonSuper + "'";
                    }
                    if (objPropMapData.FilterReview != null && objPropMapData.FilterReview != string.Empty)
                    {
                        str += " and isnull( ClearCheck ,0) =" + Convert.ToInt32(objPropMapData.FilterReview);
                    }
                    if (objPropMapData.Workorder != string.Empty && objPropMapData.Workorder != null)
                    {
                        str += " and t.workorder='" + objPropMapData.Workorder + "'";
                    }
                    if (objPropMapData.Department != -1)
                    {
                        str += " and t.type=" + objPropMapData.Department;
                    }
                    if (objPropMapData.Category != string.Empty && objPropMapData.Category !=null)
                    {
                        str += " and t.cat='" + objPropMapData.Category + "'";
                    }
                    if (objPropMapData.Bremarks != null && objPropMapData.Bremarks != string.Empty)
                    {
                        if (objPropMapData.Bremarks == "1")
                            str += " and isnull(t.bremarks,'')<>''";
                        else
                            str += " and isnull(t.bremarks,'')=''";
                    }
                    if (objPropMapData.SearchBy != "" && objPropMapData.SearchBy != null)
                    {
                        if (objPropMapData.SearchBy == "t.ldesc4")
                        {
                            str += " and l.address like '%" + objPropMapData.SearchValue + "%'";
                        }
                        else if (objPropMapData.SearchBy == "t.ID")
                        {
                            str += " and " + objPropMapData.SearchBy + " = '" + objPropMapData.SearchValue + "'";
                        }
                        else
                        {
                            str += " and " + objPropMapData.SearchBy + " like '%" + objPropMapData.SearchValue + "%'";
                        }
                    }
                    if (objPropMapData.Status == 1)
                    {
                        str += " and isnull( t.status ,0) <> 1 and isnull(t.internet,0) = 1";
                    }
                    //if (!string.IsNullOrEmpty( objPropMapData.LocIDs ))
                    //{
                    //    str += " and l.loc in (" + objPropMapData.LocIDs + ") ";
                    //}
                    if (objPropMapData.RoleID != 0)
                        str += " and isnull(l.roleid,0)=" + objPropMapData.RoleID;

                    //}
                    if (objPropMapData.InvoiceID != 0)
                    {
                        if (objPropMapData.InvoiceID == 1)
                        {
                            str += " and isnull(Invoice,0) <> 0";
                        }
                        else if (objPropMapData.InvoiceID == 2)
                        {
                            str += " and isnull(Invoice,0) = 0 and isnull(charge,0)= 1";
                        }
                    }
                }
            }

            if (str != string.Empty)
            {
                if (!string.IsNullOrEmpty(objPropMapData.OrderBy))
                {
                    string order = objPropMapData.OrderBy;
                    if (order == "WorkOrder")
                        order = "t." + order;

                    str += " order by " + order;
                }
                else
                {
                    str += " order by edate";
                }
            }

            try
            {
                if (str != string.Empty)
                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
                else
                    return objPropMapData.Ds = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportTicket(MapData objPropMapData)
        {
            try
            {
                string str = " select cat,timeroute,timesite,timecomp, isnull(Confirmed,0) as Confirmed, 0 as comp, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname, t.phone, edate, t.id, assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,ldesc2 as locname,est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address ,fdesc , '' as descres, dwork, 0.00 as Reg, 0.00 as OT,0.00 as  NT, 0.00 as DT,0.00 as TT,0.00 as  Total,0.00 as  Tottime  from TicketO t  where Assigned not in (0,4)   ";

                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  DWork='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }

                str += " union all select cat,timeroute,timesite,timecomp,  1 as Confirmed, 1 as comp, 'Completed' as assignname,'' as phone, d.edate, d.id, 4 as assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=l.Owner)) as customerName, l.Tag as locname, d.Est,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address , d.fdesc, descres,w.fDesc as dwork, reg, OT,nt,dt,tt,total, total as tottime from TicketD d inner join Loc l on l.Loc=d.Loc inner join tblWork w on d.fWork=w.ID ";
                str += " where d.ID is not null and TimeComp is not null and TimeRoute is not null";
                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  w.fDesc='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }

                str += " union all select t.cat, t.timeroute,t.timesite,t.timecomp, 1 as Confirmed, 2 as comp, 'Completed' as assignname, t.phone, dp.edate, t.id, 4 as assigned,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,ldesc2 as locname,dp.Est,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address , t.fDesc, descres,w.fDesc as dwork, reg, OT,nt,dt,tt,total, total as tottime from TicketDPDA dp inner join TicketO t on t.ID= dp.ID inner join tblWork w on dp.fWork=w.ID ";
                str += " where t.ID is not null and dp.TimeComp is not null and dp.TimeRoute is not null";
                //if (objPropMapData.Worker != "-1")
                if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
                {
                    str += " and  t.DWork='" + objPropMapData.Worker + "'";
                }
                if (objPropMapData.StartDate != System.DateTime.MinValue)
                {
                    str += " and t.edate >='" + objPropMapData.StartDate + "'";
                }
                if (objPropMapData.EndDate != System.DateTime.MinValue)
                {
                    str += " and t.edate <'" + objPropMapData.EndDate.AddDays(1) + "'";
                }


                str += " order by EDate";

                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetTicketsByWorkerDateOLD(MapData objPropMapData)
        {
            string str = " select 0.00 as tottime, timeroute, timesite, timecomp,dwork,ID, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName, t.LDesc2 as locname, t.Ldesc4 as address, Phone, Cat, EDate, CDate, '' as descres, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname ,Est from TicketO t where  assigned not in ( 0) ";

            if (objPropMapData.Worker != "-1")
            {
                str += " and  DWork='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                str += " and edate >='" + objPropMapData.StartDate + "'";
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
            }

            str += " union all select total as tottime, timeroute, timesite, timecomp, w.fDesc as dwork,t.ID, l.Tag as customerName, l.tag as locname, l.Address,  (select top 1 Phone from rol where ID=l.Rol) as Phone, cat, EDate, CDate, DescRes, 'Completed' as assignname , Est from TicketD t inner join Loc l on l.Loc=t.Loc inner join tblWork w on t.fWork=w.ID where t.id is not null ";

            if (objPropMapData.Worker != "-1")
            {
                str += " and w.fDesc='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                str += " and edate >='" + objPropMapData.StartDate + "'";
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                str += " and edate <='" + objPropMapData.EndDate.AddDays(1) + "'";
            }

            str += " order by edate desc";

            //select dwork,ID, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName, t.LDesc2 as locname, t.Ldesc4 as address, Phone, Cat, EDate, CDate, '' as descres, case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname ,Est from TicketO t where  assigned not in ( 0) and  DWork='" + objPropMapData.Worker + "' and EDate between '" + objPropMapData.StartDate + "' and DATEADD(day,1,'" + objPropMapData.EndDate + "') union all select w.fDesc as dwork,t.ID, l.Tag as customerName, l.tag as locname, l.Address,  '' as Phone, '' as cat, EDate, CDate, DescRes, 'Completed' as assignname , Est from TicketD t inner join Loc l on l.Loc=t.Loc inner join tblWork w on t.fWork=w.ID where w.fDesc='" + objPropMapData.Worker + "' and EDate between '" + objPropMapData.StartDate + "' and DATEADD(day,1,'" + objPropMapData.EndDate + "')"

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetClosedTicket(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct t.id,  ldesc1, edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address  from TicketO t inner join tblUser u on t.DWork=u.fUser where DWork='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' and Assigned =4  order by EDate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetClosedTicketDTable(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select distinct  l.ID , edate,(l.Address+', '+l.City+', '+l.State+', '+l.Zip ) as address  from TicketD d inner join tblWork w on d.fWork=w.ID inner join Loc l on d.Loc=l.Loc where w.fDesc='" + objPropMapData.Tech + "' and  DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropMapData.Date + "' order by EDate ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkers(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT TOP 10 dbo.Distancebetween(latitude, longitude, " + objPropMapData.Latitude + ", " + objPropMapData.Longitude + ") AS distance, \n");
            varname1.Append("                       latitude, \n");
            varname1.Append("                       longitude, \n");
            varname1.Append("                       deviceId, \n");
            varname1.Append("                       date, \n");
            varname1.Append("                       (SELECT CallSign \n");
            varname1.Append("                        FROM   emp e \n");
            varname1.Append("                        WHERE  e.deviceid = m.deviceId)                                                             AS emp \n");
            varname1.Append("FROM   [MSM2_Admin].dbo.MapData m \n");
            varname1.Append("WHERE  m.ID IN (SELECT DISTINCT Max(ID) \n");
            varname1.Append("                FROM   [MSM2_Admin].dbo.mapdata \n");
            varname1.Append("                WHERE  date BETWEEN Dateadd(MINUTE, -15, '" + objPropMapData.Date + "') AND Dateadd(MINUTE, 15, '" + objPropMapData.Date + "') \n");
            varname1.Append("                       AND deviceId IN (SELECT DISTINCT e.deviceid \n");
            varname1.Append("                                        FROM   emp e \n");
            varname1.Append("                                        WHERE  e.deviceid IS NOT NULL \n");
            varname1.Append("                                               AND e.deviceid <> '') \n");
            varname1.Append("                GROUP  BY deviceId) \n");
            varname1.Append("ORDER  BY distance ");

            //////varname1.Append("select name as emp, 20 as distance from route");

            try
            {
                //return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetNearWorkers", objPropMapData.Date, objPropMapData.Latitude, objPropMapData.Longitude);
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkersByTime(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetNearWorkerByTime", objPropMapData.Lat, objPropMapData.Lng, objPropMapData.Worker);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetNearWorkersDummy(MapData objPropMapData)
        {
            string str = "select name as emp, 20 as distance from route";
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetTimestmpLocationList(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTimestmpLocationList", objPropMapData.Tech, objPropMapData.Date);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCurrentLocation(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT latitude, \n");
            varname1.Append("                longitude, \n");
            varname1.Append("                deviceId, \n");
            varname1.Append("                date, \n");
            varname1.Append("                (SELECT TOP 1 e.CallSign \n");
            varname1.Append("                 FROM   emp e \n");
            varname1.Append("                 WHERE  e.deviceid = m.deviceId) AS callsign \n");
            varname1.Append("FROM   [MSM2_Admin].dbo.MapData m \n");
            varname1.Append("WHERE  m.ID IN (SELECT Max(ID) \n");
            varname1.Append("                FROM   [MSM2_Admin].dbo.mapdata \n");
            varname1.Append("                WHERE  Dateadd(DAY, Datediff(DAY, 0, date), 0) = Dateadd(DAY, Datediff(DAY, 0, Getdate()), 0) \n");
            varname1.Append("                       AND deviceId IN (SELECT DISTINCT e.deviceid \n");
            varname1.Append("                                        FROM   emp e \n");
            varname1.Append("                                        WHERE  e.deviceid IS NOT NULL \n");
            varname1.Append("                                               AND e.deviceid <> '') \n");
            varname1.Append("                GROUP  BY deviceId) ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTechCurrentLocation(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTechCurrentLocation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetWorkorderDate(MapData objPropMapData)
        {
            string str = string.Empty;
            if (objPropMapData.CustID != 0)
            {
                str = " select top 1 WorkOrder,ID, edate, fdesc,who, cdate from ticketd where WorkOrder=cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "'";
                str += " select top 1 WorkOrder,ID, edate from ticketd where  WorkOrder <> cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "' order by EDate";
            }
            else
            {
                str = " select top 1 WorkOrder,ID, edate, fdesc,who, cdate from ticketd where WorkOrder=cast(id as varchar(10)) and WorkOrder = '" + objPropMapData.Workorder + "'";
                str += " union  all select top 1 WorkOrder,ID, edate, fdesc,who, cdate from TicketO where WorkOrder=cast(id as varchar(10)) and WorkOrder =  '" + objPropMapData.Workorder + "'";

                System.Text.StringBuilder varname1 = new System.Text.StringBuilder();
                varname1.Append("SELECT TOP 1 * \n");
                varname1.Append("FROM   (SELECT WorkOrder, \n");
                varname1.Append("               ID, \n");
                varname1.Append("               edate \n");
                varname1.Append("        FROM   ticketd \n");
                varname1.Append("        WHERE  WorkOrder <> Cast(id AS VARCHAR(10)) \n");
                varname1.Append("               AND WorkOrder =  '" + objPropMapData.Workorder + "' \n");
                varname1.Append("        UNION ALL \n");
                varname1.Append("        SELECT WorkOrder, \n");
                varname1.Append("               ID, \n");
                varname1.Append("               edate \n");
                varname1.Append("        FROM   TicketO \n");
                varname1.Append("        WHERE  WorkOrder <> Cast(id AS VARCHAR(10)) \n");
                varname1.Append("               AND WorkOrder =  '" + objPropMapData.Workorder + "')AS tt \n");
                varname1.Append("ORDER  BY EDate ");

                str += varname1.ToString();
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketbyWorkorder(MapData objPropMapData)
        {
            System.Text.StringBuilder varname1 = new System.Text.StringBuilder();
            varname1.Append("SELECT 1 as comp, ID, \n");
            varname1.Append("     cdate,  EDate, \n");
            varname1.Append("     workorder, \n");
            varname1.Append("       REPLACE(REPLACE(convert(varchar(max),fdesc), CHAR(13), ''), CHAR(10), '') fdesc, \n");
            varname1.Append("       REPLACE(REPLACE(convert(varchar(max),descres), CHAR(13), ''), CHAR(10), '') descres, \n");
            varname1.Append("       (SELECT TOP 1 fDesc \n");
            varname1.Append("        FROM   tblwork \n");
            varname1.Append("        WHERE  ID = d.fWork)                              AS dwork, \n");
            varname1.Append("       fWork, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timeroute AS TIME), 100) ER, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timesite AS TIME), 100)  OS, \n");
            varname1.Append("       CONVERT(VARCHAR(15), Cast(timecomp AS TIME), 100)  CT, \n");
            varname1.Append("       Cat \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union ALL \n");
                varname1.Append("  \n");
                varname1.Append(" SELECT 0 as comp, o.ID, \n");
                varname1.Append("     o.CDate,  o.EDate, \n");
                varname1.Append("     o.WorkOrder, \n");
                varname1.Append("       REPLACE(REPLACE(CONVERT(varchar(max),o.fDesc), CHAR(13), ''), CHAR(10), '') fdesc, \n");
                varname1.Append("       REPLACE(REPLACE(CONVERT(varchar(max),descres), CHAR(13), ''), CHAR(10), '') descres, \n");
                varname1.Append("       (SELECT TOP 1 fDesc \n");
                varname1.Append("        FROM   tblwork \n");
                varname1.Append("        WHERE  ID = o.fWork)                              AS dwork, \n");
                varname1.Append("       o.fWork, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeRoute AS TIME), 100) ER, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeSite AS TIME), 100)  OS, \n");
                varname1.Append("       CONVERT(VARCHAR(15), Cast(o.TimeComp AS TIME), 100)  CT, \n");
                varname1.Append("       o.Cat \n");
                varname1.Append("FROM   TicketO o LEFT OUTER JOIN TicketDPDA dp ON dp.ID=o.ID \n");
                varname1.Append("WHERE  o.WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.LID =  " + objPropMapData.LocID + " \n");
            }

            varname1.Append("       order by edate ");


            varname1.Append(" SELECT distinct e.Unit, \n");
            varname1.Append("       e.Type, \n");
            varname1.Append("       e.Manuf, \n");
            varname1.Append("       e.Serial, \n");
            varname1.Append("       e.State \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("       INNER JOIN Elev e \n");
            varname1.Append("               ON e.ID = d.Elev ");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            varname1.Append("union SELECT DISTINCT e.Unit, \n");
            varname1.Append("                e.Type, \n");
            varname1.Append("                e.Manuf, \n");
            varname1.Append("                e.Serial, \n");
            varname1.Append("                e.State \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("       INNER JOIN multiple_equipments me \n");
            varname1.Append("               ON d.ID = me.ticket_id \n");
            varname1.Append("       INNER JOIN elev e \n");
            varname1.Append("               ON e.id = me.elev_id ");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union \n");
                varname1.Append("  \n");
                varname1.Append("SELECT DISTINCT e.Unit, \n");
                varname1.Append("                e.Type, \n");
                varname1.Append("                e.Manuf, \n");
                varname1.Append("                e.Serial, \n");
                varname1.Append("                e.State \n");
                varname1.Append("FROM   TicketO o \n");
                varname1.Append("       INNER JOIN Elev e \n");
                varname1.Append("               ON e.ID = o.LElev \n");
                varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.LID = " + objPropMapData.LocID + " \n");
                
                varname1.Append("union SELECT DISTINCT e.Unit, \n");
                varname1.Append("                e.Type, \n");
                varname1.Append("                e.Manuf, \n");
                varname1.Append("                e.Serial, \n");
                varname1.Append("                e.State \n");
                varname1.Append("FROM   Ticketo d \n");
                varname1.Append("       INNER JOIN multiple_equipments me \n");
                varname1.Append("               ON d.ID = me.ticket_id \n");
                varname1.Append("       INNER JOIN elev e \n");
                varname1.Append("               ON e.id = me.elev_id ");
                varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND d.LID = " + objPropMapData.LocID + " \n");

            }
            varname1.Append("       order by e.Unit ");



            varname1.Append(" SELECT distinct \n");
            varname1.Append("       (SELECT TOP 1 fDesc \n");
            varname1.Append("        FROM   tblwork \n");
            varname1.Append("        WHERE  ID = d.fWork)                              AS dwork \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND d.loc = " + objPropMapData.LocID + " \n");
            if (objPropMapData.CustID != 0)
            {
                varname1.Append("       AND Isnull(d.ClearCheck, 0) = 1 \n");
                varname1.Append("       AND Isnull(d.Status, 0) <> 1 \n");
                varname1.Append("       AND Isnull(d.Internet, 0) = 1 ");
            }
            else
            {
                varname1.Append("union \n");
                varname1.Append("  \n");
                varname1.Append("SELECT DISTINCT (SELECT TOP 1 fDesc \n");
                varname1.Append("                 FROM   tblwork \n");
                varname1.Append("                 WHERE  ID = o.fWork) AS dwork \n");
                varname1.Append("FROM   TicketO o \n");
                varname1.Append("WHERE  WorkOrder ='" + objPropMapData.Workorder + "' \n");
                varname1.Append("       AND o.lid =" + objPropMapData.LocID + " \n");
            }
            varname1.Append("       order by dwork ");


            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetTicketByID(MapData objPropMapData)
        {
            try
            {
                StringBuilder varname3 = new StringBuilder();
                varname3.Append(" SELECT Quan, fDesc FROM POItem where Ticket = " + objPropMapData.TicketID);
                varname3.Append(" SELECT Quan, fDesc FROM TicketI where Ticket = " + objPropMapData.TicketID);

                StringBuilder varname2 = new StringBuilder();
                varname2.Append("SELECT CASE \n");
                varname2.Append("         WHEN ( p.id IS NULL ) THEN 0 \n");
                varname2.Append("         ELSE 2 \n");
                varname2.Append("       END \n");
                varname2.Append("FROM   TicketO o \n");
                varname2.Append("       LEFT OUTER JOIN TicketDPDA p \n");
                varname2.Append("                    ON o.ID = p.ID \n");
                varname2.Append("WHERE  o.ID = " + objPropMapData.TicketID + " \n");
                varname2.Append("UNION \n");
                varname2.Append("SELECT 1 \n");
                varname2.Append("FROM   Ticketd \n");
                varname2.Append("WHERE  ID = " + objPropMapData.TicketID);

                int ISTicketD = 0;
                string strComp = Convert.ToString(SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.Text, varname2.ToString()));
                if (!string.IsNullOrEmpty(strComp))
                    ISTicketD = Convert.ToInt16(strComp);

                if (ISTicketD == 0)
                {
                    StringBuilder varname1 = new StringBuilder();
                    varname1.Append("SELECT t.*, \n");
                    varname1.Append("       (SELECT unit \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitname, \n");
                    varname1.Append("       (SELECT state \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitstate, \n");
                    varname1.Append("       0                                   AS ClearCheck1, \n");
                    varname1.Append("       0                                   AS Chargen, \n");
                    varname1.Append("       0.00                                AS Reg, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS Total, \n");
                    varname1.Append("       Upper(t.DWork)                      AS dworkup, \n");
                    varname1.Append("       (SELECT Super \n");
                    varname1.Append("        FROM   tblWork w \n");
                    varname1.Append("        WHERE  w.fdesc = t.dwork)          AS superv, \n");
                    varname1.Append("       (SELECT TOP 1 signature \n");
                    varname1.Append("        FROM   pdaticketsignature \n");
                    varname1.Append("        WHERE  pdaticketid = t.ID)         AS signature, \n");
                    varname1.Append("       0                                   AS tottime, \n");
                    varname1.Append("       0                                   AS Reg, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       t.LDesc2                            AS locname, \n");
                    varname1.Append("       (SELECT TOP 1 Name \n");
                    varname1.Append("        FROM   rol \n");
                    varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
                    varname1.Append("                     FROM   Owner \n");
                    varname1.Append("                     WHERE  ID = t.Owner)) AS customerName, \n");
                    varname1.Append("       ''                                  AS descres, \n");
                    varname1.Append("       CASE \n");
                    varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
                    varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
                    varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
                    varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
                    varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
                    varname1.Append("       END                                 AS assignname, \n");
                    varname1.Append("       t.bremarks, \n");
                    varname1.Append("       ( ldesc3 + ' ' + ldesc4 )           AS address, \n");
                    varname1.Append("       1                                   AS workcmpl, \n");
                    varname1.Append("       0                                   AS othere, \n");
                    varname1.Append("       0                                   AS toll, \n");
                    varname1.Append("       0                                   AS zone, \n");
                    varname1.Append("       0                                   AS Smile, \n");
                    varname1.Append("       0                                   AS emile, \n");
                    varname1.Append("       0                                   AS internet, \n");
                    varname1.Append("       0                                   AS invoice, \n");
                    varname1.Append("       ''                                  AS manualinvoice, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.contact \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS contact, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Phone \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Phone, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Cellular \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Cellular, \n");
                    varname1.Append("       ''                                  AS QBinvoiceID, \n");
                    varname1.Append("       0                                   AS timetransfer, \n");
                    varname1.Append("       ''                                   AS QBServiceItem, \n");
                    varname1.Append("       ''                                   AS QBPayrollItem, \n");
                    varname1.Append("       isnull(t.high,0) as highdecline, \n");
                    varname1.Append("       isnull(Customtick3,0)AS Customticket3, \n");
                    varname1.Append("       isnull(Customtick4,0)AS Customticket4, \n");
                    varname1.Append("       (select top 1 sageid from owner where id = t.owner) as sagecust,");
                    varname1.Append("       (select top 1 id from loc where loc = t.lid) as sageloc,");
                    varname1.Append("       0 as wagec,0 as Phase, (select top 1 type from jobtype where ID = t.type) as department, ");
                    varname1.Append("       (select convert(varchar(20),t.job )+'-'+ fdesc from job where ID = t.job) as jobdesc, ");
                    varname1.Append("       jobitemdesc");
                    varname1.Append(" FROM   TicketO t where t.ID =" + objPropMapData.TicketID);


                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString() + varname3.ToString());
                    //select t.*,(select unit from elev where id =t.lelev)as unitname,(select state from elev where id =t.lelev)as unitstate,0 as ClearCheck1 ,0 as Chargen,0.00 as Reg, 0 as OT,0 as  NT, 0 as DT,0 as TT,0 as  Total,UPPER(t.DWork)as dworkup, (SELECT Super FROM   tblWork w WHERE  w.fdesc = t.dwork) as superv,(select top 1 signature  from pdaticketsignature where pdaticketid=t.ID ) as signature,0 as tottime,0 as Reg,0 as NT, 0 as OT, 0 as TT, 0 as DT,t.LDesc2 as locname,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,'' as descres,case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname ,t.bremarks, (ldesc3+' '+ldesc4) as address, 1 as workcmpl, 0 as othere, 0 as toll, 0 as zone, 0 as Smile, 0 as emile,0 as internet, 0 as invoice, '' as manualinvoice, r.contact , r.phone, r.cellular, '' as QBinvoiceID, 0 as timetransfer from TicketO t inner join Loc l on l.loc=t.lid inner join rol r on r.id=l.rol
                }
                else if (ISTicketD == 1)
                {
                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select t.*,(select unit from elev where id =t.elev)as unitname,(select state from elev where id =t.elev)as unitstate,isnull(ClearCheck ,0 ) as ClearCheck1 ,isnull(Charge,0)as chargen,t.Reg, t.OT,t.NT, t.DT,t.TT, t.Total,UPPER(w.fDesc)as dworkup, w.super as superv,(select top 1 signature from pdaticketsignature where pdaticketid=t.ID)as signature,(reg + NT + OT + TT + DT)as tottime, (select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=l.Owner)) as customerName,l.tag as locname, l.Owner,l.Loc as lid, l.Address as ldesc3,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as ldesc4,(l.Address+', '+l.City+', '+ l.State+', '+ l.Zip) as Address, cat, l.City, l.State, l.Zip, Elev as lelev, (select top 1 Phone from rol where ID=l.Rol) as phone, CPhone,4 as assigned,UPPER( w.fDesc )as dwork, descres, 'Completed' as assignname,bremarks, isnull( t.workcomplete,0) as workcmpl, isnull(invoice,0) as invoice, manualinvoice,r.contact , r.phone, r.cellular, isnull( QBinvoiceID,'') as QBinvoiceID, isnull( transfertime,0) as timetransfer , isnull(Customtick3,0)AS Customticket3, isnull(Customtick4,0)AS Customticket4, 0 as highdecline ,  (select top 1 sageid from owner where id = (select top 1 owner from loc where loc = t.loc)) as sagecust, (select top 1 id from loc where loc = t.loc) as sageloc, (select top 1 type from jobtype where ID = t.type) as department ,(select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc,jobitemdesc from TicketD t inner join Loc l on l.Loc=t.Loc left outer join tblWork w on t.fWork=w.ID  inner join rol r on r.id=l.rol  where t.ID =" + objPropMapData.TicketID + "  " + varname3.ToString());
                }
                else
                {
                    DataSet ds = new DataSet();
                    int Workid = 0;
                    ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select fWork from TicketO where ID=" + objPropMapData.TicketID);
                    Workid = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    StringBuilder varname1 = new StringBuilder();
                    varname1.Append("SELECT t.*, \n");
                    varname1.Append("       (SELECT unit \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitname, \n");
                    varname1.Append("       (SELECT state \n");
                    varname1.Append("        FROM   elev \n");
                    varname1.Append("        WHERE  id = t.lelev)               AS unitstate, \n");
                    varname1.Append("       0                                   AS ClearCheck1, \n");
                    varname1.Append("       Isnull(Charge, 0)                   AS chargen, \n");
                    varname1.Append("       dp.Reg, \n");
                    varname1.Append("       dp.OT, \n");
                    varname1.Append("       dp.NT, \n");
                    varname1.Append("       dp.DT, \n");
                    varname1.Append("       dp.TT, \n");
                    varname1.Append("       dp.Total, \n");
                    varname1.Append("       Upper(DWork)                        AS dworkup, \n");
                    varname1.Append("       (SELECT Super \n");
                    varname1.Append("        FROM   tblWork w \n");
                    varname1.Append("        WHERE  w.fdesc = DWork)            AS superv, \n");
                    varname1.Append("       (SELECT TOP 1 signature \n");
                    varname1.Append("        FROM   PDA_" + Workid + " \n");
                    varname1.Append("        WHERE  pdaticketid = t.ID)         AS signature, \n");
                    varname1.Append("       0                                   AS tottime, \n");
                    varname1.Append("       0                                   AS Reg, \n");
                    varname1.Append("       0                                   AS NT, \n");
                    varname1.Append("       0                                   AS OT, \n");
                    varname1.Append("       0                                   AS TT, \n");
                    varname1.Append("       0                                   AS DT, \n");
                    varname1.Append("       t.LDesc2                            AS locname, \n");
                    varname1.Append("       (SELECT TOP 1 NAME \n");
                    varname1.Append("        FROM   rol \n");
                    varname1.Append("        WHERE  ID = (SELECT TOP 1 Rol \n");
                    varname1.Append("                     FROM   Owner \n");
                    varname1.Append("                     WHERE  ID = t.Owner)) AS customerName, \n");
                    varname1.Append("       dp.descres, \n");
                    varname1.Append("       CASE \n");
                    varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
                    varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
                    varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
                    varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
                    varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
                    varname1.Append("       END                                 AS assignname, \n");
                    varname1.Append("       t.bremarks, \n");
                    varname1.Append("       ( ldesc3 + ' ' + ldesc4 )           AS address, \n");
                    varname1.Append("       Isnull(dp.workcomplete, 0)          AS workcmpl, \n");
                    varname1.Append("       dp.othere, \n");
                    varname1.Append("       dp.toll, \n");
                    varname1.Append("       dp.zone, \n");
                    varname1.Append("       dp.Smile, \n");
                    varname1.Append("       dp.emile, \n");
                    varname1.Append("       dp.internet, \n");
                    varname1.Append("       0                                   AS invoice, \n");
                    varname1.Append("       ''                                  AS manualinvoice, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.contact \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS contact, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Phone \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Phone, \n");
                    varname1.Append("       ( CASE \n");
                    varname1.Append("           WHEN t.Owner IS NULL THEN (SELECT r.Cellular \n");
                    varname1.Append("                                      FROM   Rol r \n");
                    varname1.Append("                                             INNER JOIN Prospect p \n");
                    varname1.Append("                                                     ON p.Rol = r.ID \n");
                    varname1.Append("                                      WHERE  p.ID = t.LID) \n");
                    varname1.Append("           ELSE (SELECT r.contact \n");
                    varname1.Append("                 FROM   Rol r \n");
                    varname1.Append("                        INNER JOIN Loc l \n");
                    varname1.Append("                                ON l.Rol = r.ID \n");
                    varname1.Append("                 WHERE  l.Loc = t.LID) \n");
                    varname1.Append("         END )                             AS Cellular, \n");
                    varname1.Append("       ''                                  AS QBinvoiceID, \n");
                    varname1.Append("       0                                   AS timetransfer, \n");
                    varname1.Append("       ''                                  AS QBServiceItem, \n");
                    varname1.Append("       ''                                  AS QBPayrollItem, \n");
                    varname1.Append("       0                                  AS highdecline, \n");
                    varname1.Append("       Isnull(Customtick3, 0)              AS Customticket3, \n");
                    varname1.Append("       Isnull(Customtick4, 0)              AS Customticket4, \n");
                    varname1.Append("       (select top 1 sageid from owner where id = t.owner) as sagecust,");
                    varname1.Append("       (select top 1 id from loc where loc = t.lid) as sageloc,");
                    varname1.Append("       dp.wagec, dp.Phase, (select top 1 type from jobtype where ID = t.type) as department, (select convert(varchar(20),t.job ) +'-'+ fdesc from job where ID = t.job) as jobdesc, jobitemdesc");
                    varname1.Append(" FROM   TicketO t \n");
                    varname1.Append("       INNER JOIN TicketDPDA dp \n");
                    varname1.Append("               ON dp.ID = t.ID \n");
                    varname1.Append("WHERE  t.ID = " + objPropMapData.TicketID);

                    return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString() + varname3.ToString());

                    //return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select t.*,(select unit from elev where id =t.lelev)as unitname,(select state from elev where id =t.lelev)as unitstate,0 as ClearCheck1 ,isnull(Charge,0)as chargen,dp.Reg, dp.OT, dp.NT, dp.DT,dp.TT, dp.Total,UPPER(DWork)as dworkup, (SELECT Super FROM   tblWork w WHERE  w.fdesc = DWork) as superv,( select top 1 signature from PDA_" + Workid + " where pdaticketid=t.ID) as signature,0 as tottime,0 as Reg,0 as NT, 0 as OT, 0 as TT, 0 as DT,t.LDesc2 as locname,(select top 1 Name from rol where ID=(select top 1 Rol from Owner where ID=t.Owner)) as customerName,dp.descres,case when Assigned=1 then 'Assigned' when Assigned=2 then 'Enroute' when Assigned=3 then 'Onsite' when Assigned=4 then 'Completed' when Assigned=5 then 'Hold' end as assignname,t.bremarks,(ldesc3+' '+ldesc4) as address, isnull(dp.workcomplete,0) as workcmpl, dp.othere,dp.toll,dp.zone,dp.Smile,dp.emile,dp.internet, 0 as invoice, '' as manualinvoice,r.contact , r.phone, r.cellular, '' as QBinvoiceID , 0 as timetransfer, '' as QBServiceItem, '' as QBPayrollItem,isnull(Customtick3,0)AS Customticket3, isnull(Customtick4,0)AS Customticket4 from TicketO t inner join TicketDPDA dp on dp.ID=t.ID inner join Loc l on l.Loc=t.Lid inner join rol r on r.id=l.rol where t.ID =" + objPropMapData.TicketID);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTicketdetailsReport(MapData objPropMapData)
        {
            SqlParameter para = new SqlParameter
            {
                ParameterName = "@Tickets",
                SqlDbType = SqlDbType.Structured,
                Value = objPropMapData.dtTickets
            };

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spGetTicketDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChargeableTickets(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetChargeableTickets");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetChargeableTicketsMapping(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetChargeableTicketsMapping");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetopportunityTicket(MapData objPropMapData)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropMapData.ConnConfig, CommandType.Text, "select top 1 ID from Lead where TicketID=" + objPropMapData.TicketID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBInvoiceTicketID(MapData objPropMapData)
        {
            string strQuery = " update ticketd set QBinvoiceID='" + objPropMapData.QBInvoiceID + "' where ID='" + objPropMapData.TicketID + "'";

            //strQuery += " update ticketdpda set QBinvoiceID='" + objPropMapData.QBInvoiceID + "' where ID='" + objPropMapData.TicketID + "'";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBTimeTxnIDTicket(MapData objPropMapData)
        {
            string strQuery = " update ticketd set qbtimetxnid = '" + objPropMapData.QBInvoiceID + "'";
            strQuery += " where ID='" + objPropMapData.TicketID + "'";

            strQuery += " Insert into tblQBtimesheetticket (ticketid,time,qbtimetxnid) values (" + objPropMapData.TicketID + ",'" + objPropMapData.Custom1 + "','" + objPropMapData.QBInvoiceID + "' )";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecurringTickets(MapData objPropMapData)
        {
            string str = "SELECT  case when exists ( select 1 from TicketDPDA where ID=t.ID) then 2 else 0 end as comp, (select Unit from elev where id=t.LElev)as unit,  dwork, t.ID, (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = t.owner)) AS customername,t.LDesc1 as locid, t.ldesc2 AS locname, t.ldesc4 AS address, phone, t.Cat, t.EDate, t.CDate,  (select top 1 descres from TicketDPDA where ID=t.ID) AS descres, CASE WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname, t.Est, isnull( (select top 1 isnull(Total,0.00) from TicketDPDA where ID=t.ID),0.00)as Tottime FROM ticketo t left outer join TicketDPDA dp on t.ID=dp.ID left outer join tblWork w on w.fDesc=t.DWork  WHERE assigned NOT IN (0) and t.Level=10 ";

            if (objPropMapData.Worker != string.Empty && objPropMapData.Worker != null)
            {
                str += " and w.fdesc='" + objPropMapData.Worker + "'";
            }
            if (objPropMapData.LocTag != "")
            {
                str += " and ldesc2 like '" + objPropMapData.LocTag + "%'";
            }
            if (objPropMapData.CustomerName != "")
            {
                str += " and (SELECT TOP 1 name FROM rol WHERE id = (SELECT TOP 1 rol FROM owner WHERE id = t.owner)) like '" + objPropMapData.CustomerName + "%'";
            }

            str += " order by id desc";

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, "Spadddocument", objPropMapData.Screen, objPropMapData.TicketID, objPropMapData.FileName, objPropMapData.FilePath, objPropMapData.DocTypeMIME, objPropMapData.TempId, objPropMapData.Subject, objPropMapData.Body, objPropMapData.Mode, objPropMapData.DocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, "Spupdatedocument", objPropMapData.Screen, objPropMapData.TicketID, objPropMapData.TempId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteFile(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.Text, "delete from documents where id=" + objPropMapData.DocumentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet SelectTempDocumentFile(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("select id,path FROM Documents \n");
            varname1.Append("WHERE  Screen = 'Temp' \n");
            varname1.Append("       AND ScreenID = 0 \n");
            varname1.Append("       AND Dateadd(D, 0, Datediff(D, 0, date)) < Dateadd(D, 0, Datediff(D, 0, Getdate())) ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetDocuments(MapData objPropMapData)
        {
            string strQuerytext = "select d.*, case when isnull(filename,'') <> '' then case when reverse(left(reverse(Filename),charindex('.',reverse(Filename))-1)) in ('jpg', 'jpeg', 'bmp', 'png', 'gif') then 'Picture' else 'Document' end else '' end as doctype  from documents d where screenid=" + objPropMapData.TicketID + " and screen='" + objPropMapData.Screen + "'";

            if (objPropMapData.TicketID == 0)
            {
                strQuerytext += " and tempid='" + objPropMapData.TempId + "'";
            }

            if (objPropMapData.Mode == 1)
            {
                strQuerytext += " and isnull(filename,'') <> ''";
            }

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, strQuerytext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetLibrary(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();

            varname1.Append("SELECT d.ID, \n");
            varname1.Append("       Screen, \n");
            varname1.Append("       ScreenID, \n");
            varname1.Append("       Line, \n");
            varname1.Append("       Filename, \n");
            varname1.Append("       Path, \n");
            varname1.Append("       Type, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       TempID, \n");
            varname1.Append("       Date, \n");
            varname1.Append("       Subject, \n");
            varname1.Append("       body, \n");
            varname1.Append("       '' AS location \n");
            varname1.Append("FROM   documents d \n");
            varname1.Append("WHERE  screenid = " + objPropMapData.TicketID + " \n");
            varname1.Append("       AND screen = 'customer' and Portal = 1 \n");
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date >='" + objPropMapData.StartDate + "'\n");
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date <'" + objPropMapData.EndDate.AddDays(1) + "'");
            }
            if (objPropMapData.SearchBy == "filename" || objPropMapData.SearchBy == "remarks")
            {
                varname1.Append(" and " + objPropMapData.SearchBy + " like '" + objPropMapData.SearchValue + "%'");
            }
            if (objPropMapData.SearchBy == "loc")
            {
                varname1.Append(" and screenid = -1");
            }

            varname1.Append(" UNION \n");

            varname1.Append("SELECT d.ID, \n");
            varname1.Append("       Screen, \n");
            varname1.Append("       ScreenID, \n");
            varname1.Append("       Line, \n");
            varname1.Append("       Filename, \n");
            varname1.Append("       Path, \n");
            varname1.Append("       Type, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       TempID, \n");
            varname1.Append("       Date, \n");
            varname1.Append("       Subject, \n");
            varname1.Append("       body, \n");
            varname1.Append("       (SELECT TOP 1 tag \n");
            varname1.Append("        FROM   Loc \n");
            varname1.Append("        WHERE  Loc = ScreenID) \n");
            varname1.Append("FROM   documents d \n");
            varname1.Append("WHERE  (SELECT TOP 1 Owner \n");
            varname1.Append("        FROM   Loc \n");
            varname1.Append("        WHERE  Loc = ScreenID) = " + objPropMapData.TicketID + " \n");
            varname1.Append("       AND screen = 'location' and Portal = 1 ");
            if (objPropMapData.roleid != 0)
            {
                varname1.Append(" and (SELECT TOP 1 Isnull(RoleID,0) \n");
                varname1.Append("        FROM   Loc \n");
                varname1.Append("        WHERE  Loc = ScreenID) = " + objPropMapData.roleid);
            }
            if (objPropMapData.StartDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date >='" + objPropMapData.StartDate + "'\n");
            }
            if (objPropMapData.EndDate != System.DateTime.MinValue)
            {
                varname1.Append(" and date <'" + objPropMapData.EndDate.AddDays(1) + "'");
            }
            if (objPropMapData.SearchBy == "filename" || objPropMapData.SearchBy == "remarks")
            {
                varname1.Append(" and " + objPropMapData.SearchBy + " like '" + objPropMapData.SearchValue + "%'");
            }
            if (objPropMapData.SearchBy == "loc")
            {
                varname1.Append(" and screenid = " + objPropMapData.SearchValue);
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSignature(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetSignature", objPropMapData.WorkID, objPropMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketSignature(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, "spGetTicketSignature", objPropMapData.WorkID, objPropMapData.TicketID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetInvoiceTicketByWorkorder(MapData objPropMapData)
        {
            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, "select ID, total, (othere+toll+zone) as expenses, (emile-Smile) as mileage from TicketD where Charge=1 and WorkOrder=(select top 1 WorkOrder from TicketD where ID=" + objPropMapData.TicketID + ")");//and ID<>" + objPropMapData.TicketID + "
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketsByWorkorder(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       Assigned, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       LDesc2                              AS locname, \n");
            varname1.Append("       CONVERT(VARCHAR(30), fDesc) AS fdesc, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Assigned = 1 THEN 'Assigned' \n");
            varname1.Append("         WHEN Assigned = 2 THEN 'Enroute' \n");
            varname1.Append("         WHEN Assigned = 3 THEN 'Onsite' \n");
            varname1.Append("         WHEN Assigned = 4 THEN 'Completed' \n");
            varname1.Append("         WHEN Assigned = 5 THEN 'Hold' \n");
            varname1.Append("       END                                 AS assignname, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Assigned = 1 THEN 'White' \n");
            varname1.Append("         WHEN Assigned = 2 THEN '#9EF767' \n");
            varname1.Append("         WHEN Assigned = 3 THEN 'orange' \n");
            varname1.Append("         WHEN Assigned = 4 THEN 'DeepSkyBlue' \n");
            varname1.Append("         WHEN Assigned = 5 THEN 'yellow' \n");
            varname1.Append("       END                                 AS color, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                      FROM   TicketDPDA \n");
            varname1.Append("                      WHERE  ID = t.ID) THEN 2 \n");
            varname1.Append("         ELSE 0 \n");
            varname1.Append("       END                                 AS comp \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' \n");
            varname1.Append("       AND t.ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("UNION all \n");
            varname1.Append("SELECT ID, \n");
            varname1.Append("       4                                   AS Assigned, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       (SELECT TOP 1 tag \n");
            varname1.Append("        FROM   loc l \n");
            varname1.Append("        WHERE  l.Loc = d.Loc)              AS locname, \n");
            varname1.Append("       CONVERT(VARCHAR(30), fDesc) AS fdesc, \n");
            varname1.Append("       'Completed'                         AS assignname, \n");
            varname1.Append("       'DeepSkyBlue'                       AS color, \n");
            varname1.Append("       1                                   AS comp \n");
            varname1.Append("FROM   TicketD d \n");
            varname1.Append("WHERE  WorkOrder = '" + objPropMapData.Workorder + "' ");
            varname1.Append("       AND d.ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("       order by id");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetRecentCallsLoc(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT TOP 5 *, \n");
            varname1.Append("             CASE \n");
            varname1.Append("               WHEN Assigned = 1 THEN 'Assigned' \n");
            varname1.Append("               WHEN Assigned = 2 THEN 'Enroute' \n");
            varname1.Append("               WHEN Assigned = 3 THEN 'Onsite' \n");
            varname1.Append("               WHEN Assigned = 4 THEN 'Completed' \n");
            varname1.Append("               WHEN Assigned = 5 THEN 'Hold' \n");
            varname1.Append("             END AS assignname \n");
            varname1.Append("FROM   (SELECT 1                          AS comp, \n");
            varname1.Append("               4                          AS assigned, \n");
            varname1.Append("               CONVERT(VARCHAR(20), EDate)EDate, \n");
            varname1.Append("               ID, elev, \n");
            varname1.Append("               (SELECT Unit \n");
            varname1.Append("                FROM   Elev \n");
            varname1.Append("                WHERE  ID = Elev)         AS elevname, cat, (select fdesc from tblwork where id = fwork ) as worker \n");

            varname1.Append("        FROM   TicketD \n");
            varname1.Append("        WHERE  Loc = " + objPropMapData.LocID + " and ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("        UNION ALL \n");
            varname1.Append("        SELECT CASE \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketDPDA \n");
            varname1.Append("                              WHERE  ID = d.ID) THEN 2 \n");
            varname1.Append("                 ELSE 0 \n");
            varname1.Append("               END                 AS comp, \n");
            varname1.Append("               Assigned, \n");
            varname1.Append("               EDate, \n");
            varname1.Append("               ID, lelev as elev, \n");
            varname1.Append("               (SELECT Unit \n");
            varname1.Append("                FROM   Elev \n");
            varname1.Append("                WHERE  ID = LElev) AS elevname, cat, (select fdesc from tblwork where id = fwork ) as worker \n");
            varname1.Append("        FROM   TicketO d \n");
            varname1.Append("        WHERE  LID = " + objPropMapData.LocID + "  and ID <> " + objPropMapData.TicketID + " \n");
            varname1.Append("               AND Isnull(Owner, 0) <> 0)AS t \n");
            varname1.Append("ORDER  BY EDate DESC ");

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet GetTicketTime(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT t.ID, \n");
            varname1.Append("       t.DescRes, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       QBEmployeeID, \n");
            varname1.Append("       QBTimeTxnID, \n");
            varname1.Append("       t.LastUpdateDate, \n");
            varname1.Append("       reg, \n");
            varname1.Append("       ot, \n");
            varname1.Append("       dt, \n");
            varname1.Append("       tt, \n");
            varname1.Append("       nt, \n");
            varname1.Append("       Total, \n");
            varname1.Append("       (SELECT qblocid \n");
            varname1.Append("        FROM   loc \n");
            varname1.Append("        WHERE  loc = t.loc) AS QBcustID, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBPayrollItem, '') = '' THEN (SELECT TOP 1 qbwageid \n");
            varname1.Append("                                                   FROM   prwage \n");
            varname1.Append("                                                   WHERE  fdesc = 'Mobile Service Manager') \n");
            varname1.Append("         ELSE qbpayrollitem \n");
            varname1.Append("       END                  AS qbwageid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBServiceItem, '') = '' THEN (SELECT QBInvID \n");
            varname1.Append("                                                   FROM   Inv \n");
            varname1.Append("                                                   WHERE  Name = 'time spent') \n");
            varname1.Append("         ELSE QBServiceItem \n");
            varname1.Append("       END                  AS QBitemID \n");
            varname1.Append("FROM   TicketD t \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON w.ID = t.fWork \n");
            varname1.Append("       INNER JOIN tblUser u \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Isnull(clearcheck, 0) = 1 \n");
            varname1.Append("       AND Isnull(TransferTime, 0) = 1 ");

            if (objPropMapData.SearchValue == "1")
            {
                varname1.Append(" and  QBTimeTxnID not NULL and t.LastUpdateDate >= (select QBLastSync from Control) ");
            }
            else
            {
                varname1.Append(" and  QBTimeTxnID IS NULL ");
            }

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTicketTimeMapping(MapData objPropMapData)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT t.ID, \n");
            varname1.Append("       t.DescRes, \n");
            varname1.Append("       EDate, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       QBEmployeeID, \n");
            varname1.Append("       QBTimeTxnID, \n");
            varname1.Append("       t.LastUpdateDate, \n");
            varname1.Append("       reg, \n");
            varname1.Append("       ot, \n");
            varname1.Append("       dt, \n");
            varname1.Append("       tt, \n");
            varname1.Append("       nt, \n");
            varname1.Append("       Total, \n");
            varname1.Append("       (SELECT qblocid \n");
            varname1.Append("        FROM   loc \n");
            varname1.Append("        WHERE  loc = t.loc) AS QBcustID, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBPayrollItem, '') = '' THEN (SELECT TOP 1 qbwageid \n");
            varname1.Append("                                                   FROM   prwage \n");
            varname1.Append("                                                   WHERE  fdesc = 'Mobile Service Manager') \n");
            varname1.Append("         ELSE qbpayrollitem \n");
            varname1.Append("       END                  AS qbwageid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(QBServiceItem, '') = '' THEN (SELECT QBInvID \n");
            varname1.Append("                                                   FROM   Inv \n");
            varname1.Append("                                                   WHERE  Name = 'time spent') \n");
            varname1.Append("         ELSE QBServiceItem \n");
            varname1.Append("       END                  AS QBitemID \n");
            varname1.Append("FROM   TicketD t \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON w.ID = t.fWork \n");
            varname1.Append("       INNER JOIN tblUser u \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Isnull(clearcheck, 0) = 1 \n");
            varname1.Append("       AND Isnull(TransferTime, 0) = 1 ");

            if (objPropMapData.SearchValue == "1")
            {
                varname1.Append(" and  QBTimeTxnID not NULL and QBEmployeeID is not null and (select top 1 QBLocID from Loc where Loc = t.Loc) is not null and t.LastUpdateDate >= (select QBLastSync from Control) ");
            }
            else
            {
                varname1.Append(" and  QBTimeTxnID IS NULL and QBEmployeeID is not null and (select top 1 QBLocID from Loc where Loc = t.Loc) is not null ");
            }

            try
            {
                return objPropMapData.Ds = SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateReviewStatus(MapData objPropMapData)
        {
            SqlParameter[] para = new SqlParameter[4];
            para[0] = new SqlParameter
             {
                 ParameterName = "Data",
                 SqlDbType = SqlDbType.Structured,
                 Value = objPropMapData.dtReview
             };

            para[1] = new SqlParameter
            {
                ParameterName = "department",
                SqlDbType = SqlDbType.Int,
                Value = objPropMapData.Department
            };

            para[2] = new SqlParameter
            {
                ParameterName = "QBpayroll",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropMapData.QBPayrollID
            };

            para[3] = new SqlParameter
            {
                ParameterName = "QBservice",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropMapData.QBServiceID
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropMapData.ConnConfig, CommandType.StoredProcedure, "spMassUpdateReview", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void IndexMapdata(MapData objPropMapData)
        {
            try
            {
                SqlHelper.ExecuteDataset(Config.MS, CommandType.StoredProcedure, "spIndexMapdata");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevByTicket(MapData objPropMapData)
        {
            string str = "select (select top 1 unit from elev where id = me.elev_id) as unit, elev_id, labor_percentage from multiple_equipments me";
            str += " where ticket_id=" + objPropMapData.TicketID ;
            str += " union select (select top 1 unit from elev where id = t.lelev) as unit, t.lelev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.lelev  and ticket_id=t.id) as labor_percentage  from ticketo t where t.lelev is not null and  t.lelev <> 0 and  id = " + objPropMapData.TicketID;
            str += " union select (select top 1 unit from elev where id =  t.elev) as unit,  t.elev as elev_id, (select labor_percentage from multiple_equipments where elev_id =  t.elev  and ticket_id=t.id) as labor_percentage  from ticketd t  where t.elev is not null and t.elev <> 0 and id = " + objPropMapData.TicketID;

            try
            {
                return SqlHelper.ExecuteDataset(objPropMapData.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
    }
}
