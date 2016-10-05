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
    public class DL_User
    {
        public DataSet getUserAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spLoginAuthorization", objPropUser.Username, objPropUser.Password);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAdminAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select username, password from tbluser where username='" + objPropUser.Username + "' and password='" + objPropUser.Password + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDatabases(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "SELECT name FROM sys.databases WHERE name NOT IN ('master', 'tempdb', 'model', 'msdb') and name='" + objPropUser.DBName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet CheckDB(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select table_name from information_schema.TABLES where table_name = 'Control' select column_name from information_schema.columns where table_name = 'Control' and COLUMN_NAME='MSM'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getTSUserAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spTSLoginAuthorization", objPropUser.Username, objPropUser.Password, objPropUser.DBName);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserLoginAuthorization(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spLoginAuthorization", objPropUser.Username, objPropUser.Password, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMP(User objPropUser)
        {
            try
            {
                string str = "select upper(w.fDesc)as fdesc, w.id from tblwork w where w.id is not null ";//w.status=0

                if (objPropUser.Status == 0)
                {
                    str += "  and w.status=0 ";
                }

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and w.super='" + objPropUser.Supervisor + "'";
                }

                if (objPropUser.Username != null && objPropUser.Username != string.Empty)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id from tblwork w where w.fDesc='" + objPropUser.Username + "'";
                }

                if (objPropUser.WorkId != 0)
                {
                    str += " union select upper(w.fDesc) as fdesc, w.id from tblwork w where w.id=" + objPropUser.WorkId;
                }

                str += " order by upper(w.fDesc)";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getEMPStatus(User objPropUser)
        {
            try
            {
                string str = "select w.status from tblwork w where w.fdesc = '" + objPropUser.Username + "'";

                return Convert.ToInt16(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, str));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPwithDeviceID(User objPropUser)
        {
            try
            {
                string str = "select upper(w.fDesc)as fdesc,w.id from tblwork w inner join emp e on e.callsign=w.fdesc where  isnull(e.deviceID,'') <> '' ";//w.status=0 and

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and w.super='" + objPropUser.Supervisor + "'";
                }

                str += " order by w.fdesc";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPScheduler(User objPropUser)
        {
            try
            {
                string str = "select upper(fDesc)as fdesc,w.ID from tblwork w ";//inner join tblUser u on w.fDesc=u.fUser and u.status=0

                //if (objPropUser.IsTS == "MSM")
                //{
                //    str += " inner join tblUser u on w.fDesc=u.fUser";
                //}

                str += " where w.Status=0";

                //if (objPropUser.IsTS == "TS")
                //{
                str += " and w.dboard=1 and (w.type=0 or w.type=1) ";
                //}
                //else if (objPropUser.IsTS == "MSM")
                //{
                //    str += " and SUBSTRING ( Ticket ,1, 1) = 'Y'";
                //}

                if (objPropUser.Supervisor != null && objPropUser.Supervisor != string.Empty)
                {
                    str += " and super='" + objPropUser.Supervisor + "'";
                }

                str += " order by fdesc";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEMPSuper(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct upper(Super)as Super from tblwork where status=0 and Super is not null and Super <>'' order by Super");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getLoginSuper(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select distinct 1 from tblWork where Super='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int getISSuper(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select count(1) from tblWork where Super='" + objPropUser.Username + "' and fdesc <> '" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getControl(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select *, isnull(msemail,0) as msemailnull, isnull(QBFirstSync,1) as EmpSync, isnull(msrep,0) as msreptemp, isnull(tinternet,0) as tinternett from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getLogo(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select logo from " + objPropUser.DBName + ".dbo.Control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getControlBranch(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF EXISTS(SELECT 1 \n");
            varname1.Append("          FROM   branch b \n");
            varname1.Append("                 INNER JOIN Rol r \n");
            varname1.Append("                         ON r.EN = b.ID \n");
            varname1.Append("                 INNER JOIN loc l \n");
            varname1.Append("                         ON l.Rol = r.ID \n");
            varname1.Append("          WHERE  l.Loc = " + objPropUser.LocID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT ( (SELECT TOP 1 NAME \n");
            varname1.Append("                FROM   Control) \n");
            varname1.Append("               + ',' + Space(1) + b.NAME ) AS NAME, \n");
            varname1.Append("             b.Address, \n");
            varname1.Append("             b.City, \n");
            varname1.Append("             b.State, \n");
            varname1.Append("             b.Zip, \n");
            varname1.Append("             b.Phone, \n");
            varname1.Append("             (SELECT TOP 1 EMail \n");
            varname1.Append("              FROM   Control)              AS EMail, \n");
            varname1.Append("             b.Fax, \n");
            varname1.Append("             b.Logo \n");
            varname1.Append("      FROM   branch b \n");
            varname1.Append("             INNER JOIN Rol r \n");
            varname1.Append("                     ON r.EN = b.ID \n");
            varname1.Append("             INNER JOIN loc l \n");
            varname1.Append("                     ON l.Rol = r.ID \n");
            varname1.Append("      WHERE  l.Loc = " + objPropUser.LocID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      SELECT NAME, \n");
            varname1.Append("             Address, \n");
            varname1.Append("             City, \n");
            varname1.Append("             State, \n");
            varname1.Append("             Zip, \n");
            varname1.Append("             Phone, \n");
            varname1.Append("             EMail, \n");
            varname1.Append("             Fax, \n");
            varname1.Append("             Logo \n");
            varname1.Append("      FROM   Control \n");
            varname1.Append("  END ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getAdminControl(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblcontrol");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAdminControlByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblcontrol where id=" + objPropUser.CtrlID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet getSysAllDB(User objPropUser)
        //{
        //    try
        //    {
        //        return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, SELECT name FROM sys.databases where name=);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet getRoute(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select name,id,remarks,(select top 1 fdesc from tblwork where id = mech) as mechname from route order by name select fUser from tblUser where DefaultWorker = 1 ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTerritory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select t.Name,t.ID from terr t  order by t.name");//inner join Emp e on e.ID=t.sman where Sales='1'
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetUsers", DBNull.Value, DBNull.Value, objPropUser.DBName, objPropUser.IsSuper, objPropUser.Supervisor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserForSupervisor(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  Super = '" + objPropUser.Supervisor + "' and fuser <> '" + objPropUser.Supervisor + "'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSelectedUser(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  w.id in (" + objPropUser.Address + ")");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getUsersSuper(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT e.ID, \n");
            varname1.Append("       e.fFirst, \n");
            varname1.Append("       e.Last, \n");
            varname1.Append("       w.ID AS userid, \n");
            varname1.Append("       fUser, \n");
            varname1.Append("       u.Status, \n");
            varname1.Append("       w.super, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 'Office' \n");
            varname1.Append("         ELSE 'Field' \n");
            varname1.Append("       END  AS usertype, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN 0 \n");
            varname1.Append("         ELSE 1 \n");
            varname1.Append("       END  AS usertypeid, \n");
            varname1.Append("       CASE \n");
            varname1.Append("         WHEN Isnull(fWork, '') = '' THEN '0_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("         ELSE '1_' + CONVERT(VARCHAR(50), u.id) \n");
            varname1.Append("       END  AS userkey \n");
            varname1.Append("FROM   tblUser u \n");
            varname1.Append("       LEFT OUTER JOIN Emp e \n");
            varname1.Append("                    ON u.fUser = e.CallSign \n");
            varname1.Append("       INNER JOIN tblWork w \n");
            varname1.Append("               ON u.fUser = w.fDesc \n");
            varname1.Append("WHERE  fuser <> '" + objPropUser.Supervisor + "'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public DataSet getSupervisor(User objPropUser)
        {
            //select fuser from tbluser
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct upper(super) as fuser from tblwork where super <> ''");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getFieldUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblwork order by fdesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpenCalls(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT t.id, \n");
            varname1.Append("                CASE \n");
            varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
            varname1.Append("                  ELSE ldesc1 \n");
            varname1.Append("                END                                                        AS ldesc1, \n");
            varname1.Append("                cdate, \n");
            varname1.Append("                edate, \n");
            varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
            varname1.Append("                t.cat, \n");
            varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
            varname1.Append("                (SELECT r.Lat \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
            varname1.Append("                (SELECT r.Lng \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
            varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
            varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
            varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS credithold \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("WHERE  Assigned = 0 \n");
            varname1.Append("ORDER  BY EDate ");


            try
            {//"select distinct t.id, ldesc1,cdate ,edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address,t.cat , r.Lat, r.Lng from TicketO t inner join Loc l on t.LID=l.Loc inner join Rol r on r.ID=l.Rol   where Assigned =0  order by EDate"
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// Assigned <>4  and DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropUser.Edate + "' and DWork='" + objPropUser.FieldEmp + "' and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpenCallsMapScreen(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT t.dwork,t.assigned, CASE WHEN assigned = 0 THEN 'Un-Assigned' WHEN assigned = 1 THEN 'Assigned' WHEN assigned = 2 THEN 'Enroute' WHEN assigned = 3 THEN 'Onsite' WHEN assigned = 4 THEN 'Completed' WHEN assigned = 5 THEN 'Hold' END AS assignname,t.id, \n");
            varname1.Append("                CASE \n");
            varname1.Append("                  WHEN t.Owner IS NULL THEN LDesc2 \n");
            varname1.Append("                  ELSE ldesc1 \n");
            varname1.Append("                END                                                        AS ldesc1, \n");
            varname1.Append("                cdate, \n");
            varname1.Append("                edate, \n");
            varname1.Append("                ( LDesc3 + ', ' + t.City + ', ' + t.State + ', ' + t.Zip ) AS address, \n");
            varname1.Append("                t.cat, \n");
            varname1.Append("                Isnull(t.high, 0)                                          AS high, \n");
            varname1.Append("                (SELECT isnull(r.Lat,'') as lat \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lat, \n");
            varname1.Append("                (SELECT isnull( r.Lng,'') as lng \n");
            varname1.Append("                 FROM   Rol r \n");
            varname1.Append("                        INNER JOIN Loc l \n");
            varname1.Append("                                ON l.Rol = r.ID \n");
            varname1.Append("                 WHERE  l.Loc = t.LID)                                     AS lng, \n");
            varname1.Append("                CONVERT(VARCHAR(max), t.fdesc)                             AS fdesc, \n");
            varname1.Append("                Isnull((SELECT Isnull(dispalert, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS dispalert, \n");
            varname1.Append("                Isnull((SELECT Isnull(credit, 0) \n");
            varname1.Append("                        FROM   Loc l \n");
            varname1.Append("                        WHERE  l.Loc = t.LID \n");
            varname1.Append("                               AND t.LType = 0), 0)                        AS credithold \n");
            varname1.Append("FROM   TicketO t \n");
            varname1.Append("WHERE  Assigned <> 4 \n");
            varname1.Append("ORDER  BY EDate,assigned ");


            try
            {//"select distinct t.id, ldesc1,cdate ,edate,(LDesc3+', '+t.City+', '+t.State+', '+t.Zip ) as address,t.cat , r.Lat, r.Lng from TicketO t inner join Loc l on t.LID=l.Loc inner join Rol r on r.ID=l.Rol   where Assigned =0  order by EDate"
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// Assigned <>4  and DATEADD(DAY, DATEDIFF(DAY, 0, edate), 0)='" + objPropUser.Edate + "' and DWork='" + objPropUser.FieldEmp + "' and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomers(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetCustomers", DBNull.Value, DBNull.Value, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomers(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NULL ");

            //or LastUpdateDate >= (select QBLastSync from Control)
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomersMapping(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NULL and CreatedBy = 'MOM'");

            //or LastUpdateDate >= (select QBLastSync from Control)
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLocation(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.tag, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       o.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax t \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NULL ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLocationMapping(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.tag, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       o.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax t \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NULL and CreatedBy = 'MOM'");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBCustomers(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       o.QBCustomerID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       r.LastUpdateDate, \n");
            varname1.Append("       (SELECT QBCustomertypeID \n");
            varname1.Append("        FROM   OType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBCustomertypeID, \n");
            varname1.Append("       O.Type \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBCustomerID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control)");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getCustomersSageAdd(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       Substring(ownerID, 1, 10)                                        AS customer, \n");
            varname1.Append("       Substring(r.NAME, 1, 50)                                         AS NAME, \n");
            varname1.Append("       --Substring(r.Address, 1, 30)                                       AS Address1,   \n");
            varname1.Append("       --Substring(r.Address, 31, 30)                                      AS Address2,   \n");
            varname1.Append("       --Substring(r.Address, 61, 30)                                      AS Address3,   \n");
            varname1.Append("       --Substring(r.Address, 91, 30)                                      AS Address4,   \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 1), 1, 30)                            AS Address1, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 2), 1, 30)                            AS Address2, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 3), 1, 30)                            AS Address3, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 4), 1, 30)                            AS Address4, \n");
            varname1.Append("       Substring(r.City, 1, 30)                                         AS City, \n");
            varname1.Append("       Substring(r.Contact, 1, 15)                                      AS Contact, \n");
            varname1.Append("       Substring(r.EMail, 1, 50)                                        AS EMail, \n");
            varname1.Append("       Substring(r.Phone, 1, 15)                                        AS Phone, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       Substring(r.State, 1, 4)                                         AS State, \n");
            varname1.Append("       Substring(r.Zip, 1, 10)                                          AS Zip, \n");
            varname1.Append("       CASE o.Status \n");
            varname1.Append("         WHEN 0 THEN 'Active' \n");
            varname1.Append("         ELSE 'Inactive' \n");
            varname1.Append("       END                                                              AS Status, \n");
            varname1.Append("       CASE Isnull(o.type, '') \n");
            varname1.Append("         WHEN '' THEN 'Standard' \n");
            varname1.Append("         ELSE Substring(o.Type, 1, 20) \n");
            varname1.Append("       END                                                              AS Type \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  Isnull(SageID, 'NA') = 'NA' \n");
            varname1.Append("       AND Isnull(ownerID, '') <> '' ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update owner set sageid = '" + objPropUser.Custom1 + "' where id=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocSageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loc set sageid = '" + objPropUser.Custom1 + "' where loc=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomersSageUpdate(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT o.ID, \n");
            varname1.Append("       Isnull(LastUpdateDate, '01/01/1900')  AS LastUpdateDate, \n");
            varname1.Append("       Substring(r.NAME, 1, 50)              AS NAME, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 1), 1, 30) AS Address1, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 2), 1, 30) AS Address2, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 3), 1, 30) AS Address3, \n");
            varname1.Append("       Substring((SELECT items \n");
            varname1.Append("                  FROM   dbo.Splitsentence(r.Address, 30) spl \n");
            varname1.Append("                  WHERE  spl.id = 4), 1, 30) AS Address4, \n");
            varname1.Append("       Substring(r.City, 1, 30)              AS City, \n");
            varname1.Append("       Substring(r.Contact, 1, 15)           AS Contact, \n");
            varname1.Append("       Substring(r.EMail, 1, 50)             AS EMail, \n");
            varname1.Append("       Substring(r.Phone, 1, 15)             AS Phone, \n");
            varname1.Append("       Remarks, \n");
            varname1.Append("       Substring(r.State, 1, 4)              AS State, \n");
            varname1.Append("       Substring(r.Zip, 1, 10)               AS Zip, \n");
            varname1.Append("       CASE o.Status \n");
            varname1.Append("         WHEN 0 THEN 'Active' \n");
            varname1.Append("         ELSE 'Inactive' \n");
            varname1.Append("       END                                   AS Status, \n");
            varname1.Append("       CASE Isnull(o.type, '') \n");
            varname1.Append("         WHEN '' THEN 'Standard' \n");
            varname1.Append("         ELSE Substring(o.Type, 1, 20) \n");
            varname1.Append("       END                                   AS type \n");
            varname1.Append("FROM   Owner o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  SageID IS NOT NULL \n");
            varname1.Append("       AND LastUpdateDate >= (SELECT SageLastSync \n");
            varname1.Append("                              FROM   Control) ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomersForSageDelete(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select Id, SageID from owner where isnull(SageID,'') <> '' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsSageAdd(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spAddLocationsSage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsSageNA(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " SELECT Loc AS ID FROM Loc l WHERE SageID = 'NA' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet geCustomersSageNA(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " SELECT ID, OwnerID FROM owner WHERE SageID = 'NA' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsSageUpdate(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spUpdateLocationsSage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationsForSageDelete(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select loc, SageID from loc where isnull(SageID,'') <> '' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBLocation(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT qbcustomerid \n");
            varname1.Append("        FROM   Owner \n");
            varname1.Append("        WHERE  ID = o.Owner)   AS qbcustomerid, \n");
            varname1.Append("       o.Loc                   AS ID, \n");
            varname1.Append("       o.QBLocID, \n");
            varname1.Append("       r.Address, \n");
            varname1.Append("       o.Tag, \n");
            varname1.Append("       r.Cellular, \n");
            varname1.Append("       r.City, \n");
            varname1.Append("       r.Contact, \n");
            varname1.Append("       r.Country, \n");
            varname1.Append("       r.EMail, \n");
            varname1.Append("       r.Fax, \n");
            varname1.Append("       r.Name, \n");
            varname1.Append("       r.Phone, \n");
            varname1.Append("       r.Remarks, \n");
            varname1.Append("       r.State, \n");
            varname1.Append("       r.Zip, \n");
            varname1.Append("       o.Status, \n");
            varname1.Append("       isnull(o.balance,0) as balance , \n");
            varname1.Append("       r.LastUpdateDate, \n");
            varname1.Append("       o.Address               AS shipaddress, \n");
            varname1.Append("       o.City                  AS shipcity, \n");
            varname1.Append("       o.State                 AS shipstate, \n");
            varname1.Append("       o.Zip                   AS shipzip, \n");
            varname1.Append("       (SELECT QBStaxID \n");
            varname1.Append("        FROM   stax \n");
            varname1.Append("        WHERE  name = o.stax)AS QBstaxID, \n");
            varname1.Append("       (SELECT QBlocTypeID \n");
            varname1.Append("        FROM   LocType t \n");
            varname1.Append("        WHERE  t.Type = o.Type)AS QBlocTypeID \n");
            varname1.Append("FROM   Loc o \n");
            varname1.Append("       LEFT OUTER JOIN Rol r \n");
            varname1.Append("                    ON o.Rol = r.ID \n");
            varname1.Append("WHERE  QBLocID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) \n");
            varname1.Append("       AND QBLocID <> (SELECT qbcustomerid \n");/*For excluding the locations which doesnt exist in QB, which are same as parent.*/
            varname1.Append("                       FROM   Owner \n");
            varname1.Append("                       WHERE  ID = o.Owner) ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getLocations(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetLocations", DBNull.Value, DBNull.Value, objPropUser.DBName);
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocations");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetUsers", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName, objPropUser.IsSuper, objPropUser.Supervisor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetcustomers", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAuto(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select distinct o.ID,r.Name,r.Address from [Owner] o left outer join Rol r on o.Rol=r.ID where o.status=0 and ((NAME LIKE '" + objPropUser.SearchValue + "%') OR (Address LIKE '%" + objPropUser.SearchValue + "%') OR (Phone LIKE '" + objPropUser.SearchValue + "%') OR (City LIKE '" + objPropUser.SearchValue + "%') OR (EMail LIKE '%" + objPropUser.SearchValue + "%'))");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getAccountAuto(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select id,acct,fdesc, (acct+' : '+fdesc)account from chart  where status=0 and ((acct LIKE '" + objPropUser.SearchValue + "%') OR (fdesc LIKE '%" + objPropUser.SearchValue + "%'))");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAutojquery(User objPropUser)
        {
            //String strBuilder = "select distinct o.ID as value,r.Name as label,(r.Contact+', '+r.Address+', '+r.City+', '+r.[State]+', '+r.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from [Owner] o left outer join Rol r on o.Rol=r.ID left outer join Loc l on l.Owner=o.ID  where ((l.tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerSearch", objPropUser.SearchValue, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerProspectAutojquery(User objPropUser)
        {
            //String strBuilder = " select distinct 0 as prospect, o.ID as value,r.Name as label,(ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] from [Owner] o left outer join Rol r on o.Rol=r.ID left outer join Loc l on l.Owner=o.ID  left outer join Rol rl on l.Rol=rl.ID  ";
            //strBuilder += " where ((r.NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(r.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.state = '" + objPropUser.SearchValue.Replace("'", "''") + "') ";
            //strBuilder += "  OR (l.tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (l.ID LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (rl.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')   OR (rl.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(rl.Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (rl.EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (l.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (l.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')   OR (l.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (rl.state = '" + objPropUser.SearchValue.Replace("'", "''") + "') ) ";
            //strBuilder += " union ";
            //strBuilder += " select distinct 1 as prospect, o.ID as value,r.Name as label, (ISNULL( r.Contact,'') + ', ' +ISNULL( r.Address,'') + ', ' +ISNULL( r.City,'') + ', '+ISNULL( r.[State],'') + ', ' +ISNULL( r.Zip,'') + ', Phone: ' +ISNULL( r.Phone,'')+ ', Email: ' +ISNULL( r.EMail,'') ) AS [desc] from Prospect o left outer join Rol r on o.Rol=r.ID ";
            //strBuilder += " where ((NAME LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (state = '" + objPropUser.SearchValue.Replace("'", "''") + "')) order by r.name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustomerSearch", objPropUser.SearchValue, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTaskContactsSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTaskRolSearch", objPropUser.SearchValue);//"spGetTaskContactsSearch"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationAutojquery(User objPropUser)
        {
            try
            {
                //string str = "select distinct l.loc as value,l.tag as label,(r.Contact+', '+l.Address+', '+l.City+', '+l.[State]+', '+l.Zip+', Phone: '+r.Phone+', Email: '+r.EMail) as [desc] from loc l left outer join Rol r on l.Rol=r.ID  inner join owner o on o.id = l.owner where  r.type=4 and (((select top 1 name from rol where id=o.rol) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Tag LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (r.Address LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.City LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')  OR (r.Zip LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (dbo.RemoveSpecialChars(Phone) LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%') OR (EMail LIKE '%" + objPropUser.SearchValue.Replace("'", "''") + "%')) ";

                //if (objPropUser.CustomerID != 0)
                //{
                //    str += " and [owner]=" + objPropUser.CustomerID;
                //}

                //str += " order by tag ";

                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationSearch", objPropUser.SearchValue, objPropUser.CustomerID);//[owner]=" + objPropUser.CustomerID + " and
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationSearch(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetlocations", objPropUser.SearchBy, objPropUser.SearchValue, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet getCompany(User objPropUser)
        //{
        //    try
        //    {
        //        return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select name,dbname+':'+MSM as dbname from control");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet getCompany(User objPropUser)
        {
            string strCommandText = "select companyname,dbname+':'+type as dbname from tblcontrol ";

            if (!string.IsNullOrEmpty(objPropUser.DBName))
            {
                strCommandText += " where dbname in (" + objPropUser.DBName + ")";
            }

            strCommandText += " order by companyname ";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, strCommandText);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getUserByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetUserByID", objPropUser.UserID, objPropUser.TypeID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserLangByID(User objPropUser)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select lang from tbluser where fuser='" + objPropUser.Username + "'")).ToLower();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetCustomerByID", objPropUser.CustomerID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerAddress(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select r.Name,r.City,r.State,r.Zip,r.Phone, r.Fax,r.Contact,r.Address,r.EMail,r.Country from rol r inner join Owner o on o.Rol=r.ID where o.ID=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCustomerEmail(User objPropUser)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.EMail,'') as email from owner o inner join Rol r on r.ID=o.Rol where o.ID=" + objPropUser.CustomerID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByCustomerID(User objPropUser)
        {
            try
            {
                //return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetLocationByCustID", objPropUser.CustomerID, objPropUser.DBName, objPropUser.RoleID);
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetLocationByCustID", objPropUser.CustomerID, objPropUser.RoleID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, "spGetLocationByID", objPropUser.LocID, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByIDReport(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Tag as name,(l.Address+', '+l.City+', '+l.State+', '+l.Zip) as addressfull from Loc l where l.Loc=" + objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select type from category order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevUnit(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select unit,id from elev where loc =" + objPropUser.LocID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getequipByID(User objPropUser)
        {
            try
            {
                // return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select (select tag from Loc where Loc=e.Loc) as location, Loc, Owner, Unit, fDesc, Type, Cat, Manuf, Serial, State, Since, Last, Price, Status, Building, Remarks, fGroup, Template, InstallBy, install, category from Elev e where ID=" + objPropUser.EquipID + "  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.Elev= " + objPropUser.EquipID    );
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetEquipByID", objPropUser.EquipID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getequipREPDetails(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT (SELECT CASE \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketDPDA \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 2 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketO \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 0 \n");
            varname1.Append("                 WHEN EXISTS (SELECT 1 \n");
            varname1.Append("                              FROM   TicketD \n");
            varname1.Append("                              WHERE  ID = ticketID) THEN 1 \n");
            varname1.Append("                 ELSE 0 \n");
            varname1.Append("               END AS comp)     AS comp, \n");
            //varname1.Append("        ( CASE \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketDPDA \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketDPDA \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               WHEN EXISTS (SELECT 1 \n");
            //varname1.Append("                            FROM   TicketD \n");
            //varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
            //varname1.Append("                                                        FROM   TicketD \n");
            //varname1.Append("                                                        WHERE  ID = ticketID) \n");
            //varname1.Append("               ELSE 0 \n");
            //varname1.Append("             END ) as internet, ");
            varname1.Append("       (SELECT fDesc \n");
            varname1.Append("        FROM   tblWork w \n");
            varname1.Append("        WHERE  w.id = rd.fwork) AS fwork, \n");
            varname1.Append("       (SELECT fDesc \n");
            varname1.Append("        FROM   EquipTemp \n");
            varname1.Append("        WHERE  id = eti.EquipT) AS Template, \n");
            varname1.Append("       rd.Lastdate, \n");
            varname1.Append("       rd.NextDateDue, \n");
            varname1.Append("       rd.ticketID, \n");
            varname1.Append("       rd.Code, \n");
            varname1.Append("       eti.fDesc, \n");
            varname1.Append("       CASE eti.Frequency \n");
            varname1.Append("         WHEN 0 THEN 'Daily' \n");
            varname1.Append("         WHEN 1 THEN 'Weekly' \n");
            varname1.Append("         WHEN 2 THEN 'Bi-Weekly' \n");
            varname1.Append("         WHEN 3 THEN 'Monthly' \n");
            varname1.Append("         WHEN 4 THEN 'Bi-Monthly' \n");
            varname1.Append("         WHEN 5 THEN 'Quarterly' \n");
            varname1.Append("         WHEN 6 THEN 'Semi-Annually ' \n");
            varname1.Append("         WHEN 7 THEN 'Annually' \n");
            varname1.Append("         WHEN 8 THEN 'One Time' \n");
            varname1.Append("         WHEN 9 THEN '3 Times a Year' \n");
            varname1.Append("         WHEN 10 THEN 'Every 2 Year' \n");
            varname1.Append("         WHEN 11 THEN 'Every 3 Year' \n");
            varname1.Append("         WHEN 12 THEN 'Every 5 Year' \n");
            varname1.Append("         WHEN 13 THEN 'Every 7 Year' \n");
            varname1.Append("         WHEN 14 THEN 'On-Demand' \n");
            varname1.Append("       END                      AS freq, \n");
            varname1.Append("       (select unit from elev e where e.id= eti.elev ) as equip, status, comment, section \n");
            varname1.Append("FROM   RepDetail rd \n");
            varname1.Append("       left outer JOIN EquipTItem eti \n");
            //varname1.Append("               ON eti.ID = rd.EquipTItem ");
            varname1.Append("               ON eti.Elev = rd.Elev and eti.Code=rd.Code       ");
            varname1.Append("       Where rd.id is not null \n");

            if (objPropUser.EquipID != 0)
                varname1.Append(" and  rd.Elev = " + objPropUser.EquipID + " \n");

            if (!string.IsNullOrEmpty(objPropUser.StartDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(objPropUser.StartDate, out datetime))
                {
                    if (objPropUser.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue >= '" + objPropUser.StartDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate >= '" + objPropUser.StartDate + "'");
                    }
                }
            }
            if (!string.IsNullOrEmpty(objPropUser.EndDate))
            {
                DateTime datetime;
                if (DateTime.TryParse(objPropUser.EndDate, out datetime))
                {
                    if (objPropUser.Status == 1)
                    {
                        varname1.Append(" and rd.NextDateDue <= '" + objPropUser.EndDate + "'");
                    }
                    else
                    {
                        varname1.Append(" and rd.LastDate <= '" + objPropUser.EndDate + "'");
                    }
                }
            }

            if (!string.IsNullOrEmpty(objPropUser.SearchBy))
            {
                if (!string.IsNullOrEmpty(objPropUser.SearchValue))
                {
                    if (objPropUser.SearchBy.Trim() == "rd.ticketID" || objPropUser.SearchBy.Trim() == "eti.frequency")
                        varname1.Append("and  " + objPropUser.SearchBy + " = " + objPropUser.SearchValue.Trim() + " \n");
                    else if (objPropUser.SearchBy.Trim() == "eti.fDesc")
                        varname1.Append("and  " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue.Trim() + "%' \n");
                    else if (objPropUser.SearchBy.Trim() == "fwork")
                        varname1.Append("and ( SELECT fDesc FROM tblWork w WHERE  w.id = rd.fwork) like '" + objPropUser.SearchValue.Trim() + "%' \n");
                    else if (objPropUser.SearchBy.Trim() == "template")
                        varname1.Append("and eti.EquipT = " + objPropUser.SearchValue.Trim() + " \n");
                    else
                        varname1.Append("and  " + objPropUser.SearchBy + " like '" + objPropUser.SearchValue.Trim() + "%' \n");
                }
            }

            if (objPropUser.Cust != 0)
            {
                varname1.Append("       AND ( CASE \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketDPDA \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketDPDA \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(Internet,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
                varname1.Append("       AND ( CASE \n");
                varname1.Append("               WHEN EXISTS (SELECT 1 \n");
                varname1.Append("                            FROM   TicketD \n");
                varname1.Append("                            WHERE  ID = ticketID) THEN (SELECT isnull(ClearCheck,0) \n");
                varname1.Append("                                                        FROM   TicketD \n");
                varname1.Append("                                                        WHERE  ID = ticketID) \n");
                varname1.Append("               ELSE 0 \n");
                varname1.Append("             END ) = 1  ");
            }

            varname1.Append(" ORDER  BY rd.NextDateDue DESC ");

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElev(User objPropUser)
        {
            string str = "select distinct e.state, e.cat,e.category,e.manuf,e.price,e.last,e.since, e.id,e.unit,e.type,e.fdesc,e.status,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address, l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id WHERE e.id IS NOT NULL ";

            if (objPropUser.SearchBy != string.Empty)
            {
                if (objPropUser.SearchBy == "address")
                {
                    str += " and (l.address+', '+l.city+', '+l.state+', '+l.zip) like '%" + objPropUser.SearchValue + "%'";
                }
                else if (objPropUser.SearchBy == "r.name" || objPropUser.SearchBy == "l.id" || objPropUser.SearchBy == "l.tag")
                {
                    str += " and " + objPropUser.SearchBy + " like '%" + objPropUser.SearchValue + "%'";
                }
                else
                {
                    str += " and " + objPropUser.SearchBy + " like '" + objPropUser.SearchValue + "%'";
                }
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDate))
            {
                str += " and e.since='" + objPropUser.InstallDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.InstallDateString))
            {
                str += " and CONVERT(date,e.since) " + objPropUser.InstallDateString + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDate))
            {
                str += " and e.last ='" + objPropUser.ServiceDate + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.ServiceDateString))
            {
                str += " and CONVERT(date,e.last) " + objPropUser.ServiceDateString + "'";
            }
            if (objPropUser.Manufacturer != string.Empty)
            {
                str += " and e.manuf like '" + objPropUser.Manufacturer + "%'";
            }
            if (!string.IsNullOrEmpty(objPropUser.Price))
            {
                str += " and e.price='" + objPropUser.Price + "'";
            }
            if (!string.IsNullOrEmpty(objPropUser.PriceString))
            {
                str += " and e.price " + objPropUser.PriceString + "'";
            }
            if (objPropUser.LocID != 0)
            {
                str += " and e.loc=" + objPropUser.LocID + "";
            }
            if (objPropUser.CustomerID != 0)
            {
                str += " and e.owner=" + objPropUser.CustomerID + "";
            }

            if (objPropUser.RoleID != 0)
                str += " and isnull(l.roleid,0)=" + objPropUser.RoleID;

            if (!string.IsNullOrEmpty(objPropUser.Category))
                str += " and e.cat = '" + objPropUser.Category + "'";

            if (!string.IsNullOrEmpty(objPropUser.Type))
                str += " and e.type = '" + objPropUser.Type + "'";

            if (objPropUser.Status != -1)
                str += " and e.status = " + objPropUser.Status;

            str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getElevSearch(User objPropUser)
        {
            string str = "select e.unit as label, e.id as value, e.state, e.cat,e.category,e.manuf,e.price,e.last,e.since, e.id,e.unit,e.type,e.fdesc,e.status,l.tag ,l.ID as LID,e.Loc, l.Owner, (select top 1  name from rol where ID = (select top 1 rol from owner where ID = l.owner)) as custname FROM elev e INNER JOIN loc l ON l.Loc = e.Loc  WHERE e.id IS NOT NULL ";

            if (!string.IsNullOrEmpty(objPropUser.SearchValue))
            {
                str += " and e.state like '%" + objPropUser.SearchValue + "%'";
            }

            str += " order by e.unit";

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddUser(User objPropUser)
        {
            try
            {
                object objfire = DBNull.Value;
                object objhire = DBNull.Value;
                object objMerchant = DBNull.Value;
                object objSDate = DBNull.Value;
                object objEDate = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    objfire = objPropUser.DtFired;
                }
                if (objPropUser.DtHired != System.DateTime.MinValue)
                {
                    objhire = objPropUser.DtHired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }
                if (objPropUser.FStart != System.DateTime.MinValue)
                {
                    objSDate = objPropUser.FStart;
                }
                if (objPropUser.FEnd != System.DateTime.MinValue)
                {
                    objEDate = objPropUser.FEnd;
                }

                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spAddUser", objPropUser.Username, objPropUser.Password, objPropUser.PDA, objPropUser.Field, objPropUser.Status, objPropUser.FirstName, objPropUser.MiddleName, objPropUser.LastNAme, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Cell, objPropUser.Email, objhire, objfire, objPropUser.CreateTicket, objPropUser.WorkDate, objPropUser.LocationRemarks, objPropUser.ServiceHist, objPropUser.PurchaseOrd, objPropUser.Expenses, objPropUser.ProgFunctions, objPropUser.AccessUser, objPropUser.Remarks, objPropUser.Mapping, objPropUser.Schedule, objPropUser.DeviceID, objPropUser.Pager, objPropUser.Supervisor, objPropUser.Salesperson, objPropUser.UserLic, objPropUser.UserLicID, objPropUser.Lang, objMerchant, objPropUser.DefaultWorker, objPropUser.Dispatch, objPropUser.SalesMgr, objPropUser.MassReview, objPropUser.MOMUSer, objPropUser.MOMPASS, objPropUser.InServer, "POP3", objPropUser.InUsername, objPropUser.InPassword, objPropUser.InPort, objPropUser.OutServer, objPropUser.OutUsername, objPropUser.OutPassword, objPropUser.OutPort, objPropUser.SSL, objPropUser.EmailAccount, objPropUser.HourlyRate, objPropUser.EmpMaintenance, objPropUser.Timestampfix, objPropUser.PayMethod, objPropUser.PHours, objPropUser.Salary, objPropUser.Department, objPropUser.EmpRefID, objPropUser.PayPeriod, objPropUser.MileageRate, objPropUser.AddEquip, objPropUser.EditEquip, objPropUser.FChart, objPropUser.AddChart, objPropUser.EditChart, objPropUser.ViewChart, objPropUser.FGLAdj, objPropUser.AddGLAdj, objPropUser.EditGLAdj, objPropUser.ViewGLAdj, objPropUser.FDeposit, objPropUser.AddDeposit, objPropUser.EditDeposit, objPropUser.ViewDeposit, objPropUser.FCustomerPayment, objPropUser.AddCustomerPayment, objPropUser.EditCustomerPayment, objPropUser.ViewCustomerPayment, objPropUser.FinanStatement, objSDate, objEDate, objPropUser.APVendor, objPropUser.APBill, objPropUser.APBillPay, objPropUser.APBillSelect));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddQBUser(User objPropUser)
        {
            try
            {
                object objfire = DBNull.Value;
                object objhire = DBNull.Value;
                object objMerchant = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    objfire = objPropUser.DtFired;
                }
                if (objPropUser.DtHired != System.DateTime.MinValue)
                {
                    objhire = objPropUser.DtHired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }


                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spAddQBUser", objPropUser.Username, objPropUser.Password, objPropUser.PDA, objPropUser.Field, objPropUser.Status, objPropUser.FirstName, objPropUser.MiddleName, objPropUser.LastNAme, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Cell, objPropUser.Email, objhire, objfire, objPropUser.CreateTicket, objPropUser.WorkDate, objPropUser.LocationRemarks, objPropUser.ServiceHist, objPropUser.PurchaseOrd, objPropUser.Expenses, objPropUser.ProgFunctions, objPropUser.AccessUser, objPropUser.Remarks, objPropUser.Mapping, objPropUser.Schedule, objPropUser.DeviceID, objPropUser.Pager, objPropUser.Supervisor, objPropUser.Salesperson, objPropUser.UserLic, objPropUser.UserLicID, objPropUser.Lang, objMerchant, objPropUser.QBEmployeeID, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomer(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[32];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "ContactData";
            para[12].SqlDbType = SqlDbType.Structured;
            para[12].Value = objPropUser.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "contact";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.MainContact;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Phone";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Phone;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Website";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Website;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Email";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Email;

            para[18] = new SqlParameter();
            para[18].ParameterName = "cell";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Cell;

            para[19] = new SqlParameter();
            para[19].ParameterName = "Type";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.Type;

            para[20] = new SqlParameter();
            para[20].ParameterName = "returnval";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.EquipID;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "@grpbywo";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropUser.grpbyWO;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@openticket";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropUser.openticket;

            para[26] = new SqlParameter();
            para[26].ParameterName = "BillRate";
            para[26].SqlDbType = SqlDbType.Decimal;
            para[26].Value = objPropUser.BillRate;

            para[27] = new SqlParameter();
            para[27].ParameterName = "OT";
            para[27].SqlDbType = SqlDbType.Decimal;
            para[27].Value = objPropUser.RateOT;

            para[28] = new SqlParameter();
            para[28].ParameterName = "NT";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = objPropUser.RateNT;

            para[29] = new SqlParameter();
            para[29].ParameterName = "DT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = objPropUser.RateDT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "Travel";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = objPropUser.RateTravel;

            para[31] = new SqlParameter();
            para[31].ParameterName = "Mileage";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropUser.MileageRate;
            try
            {
                //custID = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para));                
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomer", para);
                custID = Convert.ToInt32(para[20].Value);
                objPropUser.CustomerID = custID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerQB(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[22];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddQBcustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerQBMapping(User objPropUser)
        {
            int custID;
            SqlParameter[] para = new SqlParameter[23];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustomerID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBacctID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.QBAccountNumber;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddQBcustomerMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddCustomerSage(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            //para[12] = new SqlParameter();
            //para[12].ParameterName = "ContactData";
            //para[12].SqlDbType = SqlDbType.Structured;
            //para[12].Value = objPropUser.ContactData;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Internet";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.Internet;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "SageKeyID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Balance";
            para[21].SqlDbType = SqlDbType.Money;
            para[21].Value = objPropUser.Balance;

            para[22] = new SqlParameter();
            para[22].ParameterName = "returnval";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Direction = ParameterDirection.ReturnValue;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Customer";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropUser.Custom1;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "SpAddSagecustomer", para);
                int custid = 0;
                if (para[22].Value != DBNull.Value)
                {
                    custid = Convert.ToInt32(para[22].Value);
                }
                return custid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomertest(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddLocationTest", objPropUser.Address, objPropUser.City, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSupervisorUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tblwork set super = '" + objPropUser.Supervisor + "' where id =" + objPropUser.WorkId);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipment(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[18];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@since";
            param[8].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }

            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }

            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@Remarks";
            param[12].SqlDbType = SqlDbType.Text;
            param[12].Value = objPropUser.Remarks;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Install";
            param[13].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[13].Value = DBNull.Value;
            }
            else
            {
                param[13].Value = objPropUser.InstallDateimport;
            }

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Category";
            param[14].SqlDbType = SqlDbType.VarChar;
            param[14].Value = objPropUser.Category;

            param[15] = new SqlParameter();
            param[15].ParameterName = "@template";
            param[15].SqlDbType = SqlDbType.Int;
            param[15].Value = objPropUser.CustomTemplateID;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = objPropUser.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@CustomItems";
            param[17].SqlDbType = SqlDbType.Structured;
            param[17].Value = objPropUser.dtcustom;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipmentImport(User objPropUser)
        {
            SqlParameter paraLastServ = new SqlParameter();
            paraLastServ.ParameterName = "Last";
            paraLastServ.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                paraLastServ.Value = DBNull.Value;
            }
            else
            {
                paraLastServ.Value = objPropUser.LastServiceDate;
            }

            SqlParameter paraSince = new SqlParameter();
            paraSince.ParameterName = "since";
            paraSince.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                paraSince.Value = DBNull.Value;
            }
            else
            {
                paraSince.Value = objPropUser.InstallDateTime;
            }

            SqlParameter paraInstalled = new SqlParameter();
            paraInstalled.ParameterName = "Install";
            paraInstalled.SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                paraInstalled.Value = DBNull.Value;
            }
            else
            {
                paraInstalled.Value = objPropUser.InstallDateimport;
            }



            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddEquipmentImport", objPropUser.Locationname, objPropUser.Unit, objPropUser.FirstName, objPropUser.Type, objPropUser.Cat, objPropUser.Manufacturer, objPropUser.Serial, objPropUser.UniqueID, paraSince, paraLastServ, objPropUser.EquipPrice, objPropUser.Status, objPropUser.Remarks, paraInstalled, objPropUser.Category);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateEquipment(User objPropUser)
        {
            SqlParameter[] param = new SqlParameter[20];
            param[0] = new SqlParameter();
            param[0].ParameterName = "@Loc";
            param[0].SqlDbType = SqlDbType.Int;
            param[0].Value = objPropUser.LocID;

            param[1] = new SqlParameter();
            param[1].ParameterName = "@Unit";
            param[1].SqlDbType = SqlDbType.VarChar;
            param[1].Value = objPropUser.Unit;

            param[2] = new SqlParameter();
            param[2].ParameterName = "@fDesc";
            param[2].SqlDbType = SqlDbType.VarChar;
            param[2].Value = objPropUser.Description;

            param[3] = new SqlParameter();
            param[3].ParameterName = "@Type";
            param[3].SqlDbType = SqlDbType.VarChar;
            param[3].Value = objPropUser.Type;

            param[4] = new SqlParameter();
            param[4].ParameterName = "@Cat";
            param[4].SqlDbType = SqlDbType.VarChar;
            param[4].Value = objPropUser.Cat;

            param[5] = new SqlParameter();
            param[5].ParameterName = "@Manuf";
            param[5].SqlDbType = SqlDbType.VarChar;
            param[5].Value = objPropUser.Manufacturer;

            param[6] = new SqlParameter();
            param[6].ParameterName = "@Serial";
            param[6].SqlDbType = SqlDbType.VarChar;
            param[6].Value = objPropUser.Serial;

            param[7] = new SqlParameter();
            param[7].ParameterName = "@State";
            param[7].SqlDbType = SqlDbType.VarChar;
            param[7].Value = objPropUser.UniqueID;

            param[8] = new SqlParameter();
            param[8].ParameterName = "@Since";
            param[8].SqlDbType = SqlDbType.DateTime;

            if (objPropUser.InstallDateTime == System.DateTime.MinValue)
            {
                param[8].Value = DBNull.Value;
            }
            else
            {
                param[8].Value = objPropUser.InstallDateTime;
            }


            param[9] = new SqlParameter();
            param[9].ParameterName = "@Last";
            param[9].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.LastServiceDate == System.DateTime.MinValue)
            {
                param[9].Value = DBNull.Value;
            }
            else
            {
                param[9].Value = objPropUser.LastServiceDate;
            }


            param[10] = new SqlParameter();
            param[10].ParameterName = "@Price";
            param[10].SqlDbType = SqlDbType.Decimal;
            param[10].Value = objPropUser.EquipPrice;

            param[11] = new SqlParameter();
            param[11].ParameterName = "@Status";
            param[11].SqlDbType = SqlDbType.TinyInt;
            param[11].Value = objPropUser.Status;

            param[12] = new SqlParameter();
            param[12].ParameterName = "@ID";
            param[12].SqlDbType = SqlDbType.Int;
            param[12].Value = objPropUser.EquipID;

            param[13] = new SqlParameter();
            param[13].ParameterName = "@Remarks";
            param[13].SqlDbType = SqlDbType.Text;
            param[13].Value = objPropUser.Remarks;

            param[14] = new SqlParameter();
            param[14].ParameterName = "@Install";
            param[14].SqlDbType = SqlDbType.DateTime;
            if (objPropUser.InstallDateimport == System.DateTime.MinValue)
            {
                param[14].Value = DBNull.Value;
            }
            else
            {
                param[14].Value = objPropUser.InstallDateimport;
            }

            param[15] = new SqlParameter();
            param[15].ParameterName = "@Category";
            param[15].SqlDbType = SqlDbType.VarChar;
            param[15].Value = objPropUser.Category;

            param[16] = new SqlParameter();
            param[16].ParameterName = "@items";
            param[16].SqlDbType = SqlDbType.Structured;
            param[16].Value = objPropUser.DtItems;

            param[17] = new SqlParameter();
            param[17].ParameterName = "@ItemsOnly";
            param[17].SqlDbType = SqlDbType.Int;
            param[17].Value = objPropUser.ItemsOnly;

            param[18] = new SqlParameter();
            param[18].ParameterName = "@template";
            param[18].SqlDbType = SqlDbType.Int;
            param[18].Value = objPropUser.CustomTemplateID;

            param[19] = new SqlParameter();
            param[19].ParameterName = "@CustomItems";
            param[19].SqlDbType = SqlDbType.Structured;
            param[19].Value = objPropUser.dtcustom;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateEquipment", param);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMassMCP(User objPropUser)
        {
            SqlParameter param = new SqlParameter()
            {
                ParameterName = "@items",
                SqlDbType = SqlDbType.Structured,
                Value = objPropUser.DtItems
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipmentMCPItems", param);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[45];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropUser.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = objPropUser.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            para[20].Value = objPropUser.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Owner";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.CustomerID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Stax";
            para[23].SqlDbType = SqlDbType.VarChar;
            para[23].Value = objPropUser.Stax;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Lat";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.Lat;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lng";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropUser.Lng;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Custom1";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropUser.Custom1;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom2";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropUser.Custom2;

            para[28] = new SqlParameter();
            para[28].ParameterName = "To";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = objPropUser.ToMail;

            para[28] = new SqlParameter();
            para[28].ParameterName = "To";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = objPropUser.ToMail;

            para[29] = new SqlParameter();
            para[29].ParameterName = "CC";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = objPropUser.ToMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "ToInv";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = objPropUser.MailToInv;

            para[31] = new SqlParameter();
            para[31].ParameterName = "CCInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = objPropUser.MailCCInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CreditHold";
            para[32].SqlDbType = SqlDbType.TinyInt;
            para[32].Value = objPropUser.CreditHold;

            para[33] = new SqlParameter();
            para[33].ParameterName = "DispAlert";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = objPropUser.DispAlert;

            para[34] = new SqlParameter();
            para[34].ParameterName = "CreditReason";
            para[34].SqlDbType = SqlDbType.VarChar;
            para[34].Value = objPropUser.CreditReason;

            para[35] = new SqlParameter();
            para[35].ParameterName = "prospectID";
            para[35].SqlDbType = SqlDbType.Int;
            para[35].Value = objPropUser.ProspectID;

            para[37] = new SqlParameter();                   //added by Mayuri 24th dec,15
            para[37].ParameterName = "ContractBill";
            para[37].SqlDbType = SqlDbType.TinyInt;
            para[37].Value = objPropUser.ContractBill;

            para[38] = new SqlParameter();
            para[38].ParameterName = "Terms";
            para[38].SqlDbType = SqlDbType.Int;
            para[38].Value = objPropUser.TermsID;

            //para[23] = new SqlParameter();
            //para[23].ParameterName = "MAPAddress";
            //para[23].SqlDbType = SqlDbType.VarChar;
            //para[23].Value = objPropUser.MAPAddress;
            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropUser.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = objPropUser.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = objPropUser.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = objPropUser.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = objPropUser.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = objPropUser.MileageRate;

            int locid = 0;
            try
            {
                locid = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLocation", para));
                objPropUser.LocID = locid;
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = objPropUser.Balance;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spQBAddLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBLocationMapping(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[25];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Owner";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.CustomerID;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "QBLocationID";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.QBlocationID;

            para[19] = new SqlParameter();
            para[19].ParameterName = "QBCustID";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.QBCustomerID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "LastUpdateDate";
            para[20].SqlDbType = SqlDbType.DateTime;
            para[20].Value = objPropUser.LastUpdateDate;

            para[21] = new SqlParameter();
            para[21].ParameterName = "type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "QBstax";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Stax;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Balance";
            para[23].SqlDbType = SqlDbType.Money;
            para[23].Value = objPropUser.Balance;

            para[24] = new SqlParameter();
            para[24].ParameterName = "QBacctID";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.QBAccountNumber;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spQBAddLocationMapping", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void AddCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDCompany", objPropUser.FirstName, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Fax, objPropUser.Email, objPropUser.Website, objPropUser.MSM, objPropUser.DSN, objPropUser.DBName, objPropUser.Username, objPropUser.Password, objPropUser.ContactName, objPropUser.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateCompany", objPropUser.FirstName, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Fax, objPropUser.Email, objPropUser.Website, objPropUser.ContactName, objPropUser.Remarks, objPropUser.Logo, objPropUser.CustWeb, objPropUser.QBPath, objPropUser.MultiLang, objPropUser.QBInteg, objPropUser.EmailMS, objPropUser.QBFirstSync, objPropUser.QBSalesTaxID, objPropUser.QbserviceItemlabor, objPropUser.QBserviceItemExp, objPropUser.YE, objPropUser.GSTReg);//,objPropUser.TransferTimeSheet, objPropUser.TransferInvoice);//,objPropUser.MerchantID,objPropUser.LoginID,objPropUser.PaymentUser,objPropUser.PaymentPass
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAnnualAmount(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update control set GrossInc =" + objPropUser.AnnualAmount + " , Month = " + objPropUser.Month + ", SalesAnnual = " + objPropUser.SalesAmount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateControl(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update control set msrep =" + objPropUser.REPtemplateID + " , tinternet = " + objPropUser.Internet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCompany(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "delete from tblcontrol where id='" + objPropUser.CtrlID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomerType(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into otype (type, remarks) values ('" + objPropUser.CustomerType + "','" + objPropUser.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDCusttype", objPropUser.CustomerType, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBCustomerType(User objPropUser)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   OType \n");
            varname1.Append("              WHERE  QBCustomerTypeID = '" + objPropUser.QBCustomerTypeID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO otype \n");
            varname1.Append("                  (type, \n");
            varname1.Append("                   remarks, \n");
            varname1.Append("                   QBCustomerTypeID) \n");
            //varname1.Append("                   LastUpdateDate) \n");
            varname1.Append("      VALUES      ('" + objPropUser.CustomerType + "', \n");
            varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            varname1.Append("                   '" + objPropUser.QBCustomerTypeID + "') \n");
            //varname1.Append("                   Getdate()) \n");
            varname1.Append("  END \n");
            //varname1.Append("ELSE \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      UPDATE OType \n");
            //varname1.Append("      SET    Remarks = '" + objPropUser.Remarks + "' \n");
            ////varname1.Append("             LastUpdateDate = Getdate() \n");
            //varname1.Append("      WHERE  QBCustomerTypeID = '" + objPropUser.QBCustomerTypeID + "' \n");
            //varname1.Append("  END ");
            ////Type = '" + objPropUser.CustomerType + "', \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBLocType(User objPropUser)
        {

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   LocType \n");
            //varname1.Append("              WHERE  QBlocTypeID = '" + objPropUser.QBCustomerTypeID + "') \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      INSERT INTO LocType \n");
            //varname1.Append("                  (type, \n");
            //varname1.Append("                   remarks, \n");
            //varname1.Append("                   QBlocTypeID) \n");
            ////varname1.Append("                   LastUpdateDate) \n");
            //varname1.Append("      VALUES      ('" + objPropUser.CustomerType + "', \n");
            //varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            //varname1.Append("                   '" + objPropUser.QBCustomerTypeID + "') \n");
            ////varname1.Append("                   Getdate()) \n");
            //varname1.Append("  END \n");
            ////varname1.Append("ELSE \n");
            ////varname1.Append("  BEGIN \n");
            ////varname1.Append("      UPDATE LocType \n");
            ////varname1.Append("      SET    Remarks = '" + objPropUser.Remarks + "', \n");
            ////varname1.Append("             LastUpdateDate = Getdate() \n");
            ////varname1.Append("      WHERE  QBlocTypeID = '" + objPropUser.QBCustomerTypeID + "' \n");
            ////varname1.Append("  END ");
            //////Type = '" + objPropUser.CustomerType + "', \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBjobtype", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.QBCustomerTypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "Spaddcategory", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.Logo, objPropUser.Chargeable, objPropUser.Default);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddEquipType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from ElevatorSpec where edesc ='" + objPropUser.EquipType + "' and ecat = 1) begin insert into ElevatorSpec (ecat, edesc) values (1,'" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('Equipment Type already exists, please use different equipment !',16,1)  RETURN END ");
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into ElevatorSpec (ecat, edesc) values (1,'" + objPropUser.EquipType + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipCateg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from ElevatorSpec where edesc ='" + objPropUser.EquipType + "' and ecat = 0) begin insert into ElevatorSpec (ecat, edesc) values (0,'" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('Equipment category already exists, please use different name !',16,1)  RETURN END ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddMCPS(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, " if not exists(select 1 from tblMCPStatus where name ='" + objPropUser.EquipType + "' ) begin insert into tblMCPStatus (name) values ('" + objPropUser.EquipType + "') End else BEGIN  RAISERROR ('MCP Status already exists, please use different name !',16,1)  RETURN END ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddServiceType(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into ltype (type, fdesc, remarks,Matcharge,free) values ('" + objPropUser.EquipType + "','" + objPropUser.Locationname + "','" + objPropUser.LocationRemarks + "',0,0)");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT 1 FROM ltype WHERE  type = '" + objPropUser.EquipType + "') Begin insert into ltype (type, fdesc, remarks,Matcharge,free,InvID) values ('" + objPropUser.EquipType + "','" + objPropUser.Locationname + "','" + objPropUser.LocationRemarks + "',0,0,'" + objPropUser.InvID + "') End Else Begin RAISERROR ('Service type already exists, please use different service  !',16,1) return End");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddLocationType(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into loctype (type, remarks) values ('" + objPropUser.CustomerType + "','" + objPropUser.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spADDLoctype", objPropUser.CustomerType, objPropUser.Remarks);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  otype set remarks='" + objPropUser.Remarks + "' where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateServiceType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  ltype set fdesc='" + objPropUser.Locationname + "', remarks='" + objPropUser.LocationRemarks + "', InvID='" + objPropUser.InvID + "' where type= '" + objPropUser.EquipType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCategory(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "Spupdatecategory", objPropUser.CustomerType, objPropUser.Remarks, objPropUser.Logo, objPropUser.Chargeable, objPropUser.Default);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateEquipType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  ElevatorSpec set edesc='" + objPropUser.EquipType + "' where edesc= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update  loctype set remarks='" + objPropUser.Remarks + "' where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void DeleteCustomerType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  otype where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerTypeByListID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  otype where isnull(qbcustomertypeid,'')<>'' and qbcustomertypeid= '" + objPropUser.QBCustomerTypeID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void DeleteCategory(User objPropUser)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  category where type= '" + objPropUser.CustomerType + "'");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void DeleteEquiptype(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where Type = '" + objPropUser.EquipType + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.EquipType + "' and ecat=1 end else begin RAISERROR ('Equipments are assigned to the selected equipment type, equipment type cannot be deleted!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEquipCateg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from Elev where cat = '" + objPropUser.EquipType + "' ) begin delete from ElevatorSpec where EDesc='" + objPropUser.EquipType + "' and ecat=0 end else begin RAISERROR ('Equipments are assigned to the selected equipment category, equipment category cannot be deleted!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteMCPS(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from repdetail where status = '" + objPropUser.EquipType + "' ) begin delete from tblmcpstatus where name='" + objPropUser.EquipType + "'  end else begin RAISERROR ('MCP Status is in use!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBillingCode(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   InvoiceI \n");
            varname1.Append("               WHERE  Acct = " + objPropUser.BillCode + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Inv \n");
            varname1.Append("      WHERE  ID = " + objPropUser.BillCode + " and name not in ('recurring', 'time spent', 'mileage', 'expenses') \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Billing code is in use!',16,1) \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteBillingCodebyListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   InvoiceI \n");
            varname1.Append("               WHERE  Acct =( select ID from inv where isnull(qbinvid,'')<>'' and  qbinvid = '" + objPropUser.QBInvID + "' )) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Inv \n");
            varname1.Append("      WHERE isnull(qbinvid,'')<>'' and  qbinvid = '" + objPropUser.QBInvID + "' \n");
            varname1.Append("  END \n");
            varname1.Append("else \n");
            varname1.Append("  BEGIN");
            varname1.Append("  UPDATE Inv SET Status=1 WHERE Isnull( qbinvid,'')<>'' AND  qbinvid = '" + objPropUser.QBInvID + "' \n");
            varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteServicetype(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Elev \n");
            varname1.Append("               WHERE  Cat = '" + objPropUser.EquipType + "' \n");
            varname1.Append("               UNION \n");
            varname1.Append("               SELECT 1 \n");
            varname1.Append("               FROM   Job \n");
            varname1.Append("               WHERE  CType = '" + objPropUser.EquipType + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM LType \n");
            varname1.Append("      WHERE  Type = '" + objPropUser.EquipType + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR ('Service type is in use!',16,1) \n");
            varname1.Append(" \n");
            varname1.Append("      RETURN \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSalesTax(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Loc \n");
            varname1.Append("               WHERE  STax = '" + objPropUser.SalesTax + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM STax \n");
            varname1.Append("      WHERE  Name = '" + objPropUser.SalesTax + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Sales tax is in use!',16,1) \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSalesTaxByListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS (SELECT 1 \n");
            varname1.Append("               FROM   Loc \n");
            varname1.Append("               WHERE  STax = (select Name from STax where  isnull(qbstaxid,'')<>'' and  qbstaxid = '" + objPropUser.QBSalesTaxID + "')) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM STax \n");
            varname1.Append("      WHERE isnull(qbstaxid,'')<>'' and  qbstaxid = '" + objPropUser.QBSalesTaxID + "' \n");
            varname1.Append("  END \n");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE STax SET Status=1 WHERE Isnull( qbstaxid,'')<>'' AND  qbstaxid = '" + objPropUser.QBSalesTaxID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteDepartment(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Type = " + objPropUser.DepartmentID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM JobType \n");
            varname1.Append("      WHERE  ID = " + objPropUser.DepartmentID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Department is in use!',16,1) \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteDepartmentByListID(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Type = (SELECT ID \n");
            varname1.Append("                             FROM   JobType \n");
            varname1.Append("                             WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("                                    AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "')) \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM JobType \n");
            varname1.Append("      WHERE  Isnull(QBJobTypeID, '') <> '' \n");
            varname1.Append("             AND QBJobTypeID = '" + objPropUser.QBJobtypeID + "' \n");
            varname1.Append("  END ");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE JobType SET Status=1 WHERE Isnull( QBJobTypeID,'')<>'' AND  QBJobTypeID = '" + objPropUser.QBJobtypeID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteCategory(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE  Cat = '" + objPropUser.Category + "' \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Cat = '" + objPropUser.Category + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM Category \n");
            varname1.Append("      WHERE  Type = '" + objPropUser.Category + "' \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR('Category is in use!',16,1) \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  tbluser where id= '" + objPropUser.UserID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomer(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   Loc \n");
            varname1.Append("              WHERE  Owner = " + objPropUser.CustomerID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      DELETE FROM rol \n");
            varname1.Append("      WHERE  id = (SELECT TOP 1 rol \n");
            varname1.Append("                   FROM   owner \n");
            varname1.Append("                   WHERE  id = " + objPropUser.CustomerID + ") \n");
            varname1.Append(" \n");
            varname1.Append("      DELETE FROM Owner \n");
            varname1.Append("      WHERE  ID = " + objPropUser.CustomerID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!',16,1) \n");
            varname1.Append(" \n");
            varname1.Append("      RETURN \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// "if not exists(select 1 from Loc where Owner = " + objPropUser.CustomerID + ") begin delete from rol where id=(select top 1 rol from owner where id =" + objPropUser.CustomerID + ") delete from Owner where ID=" + objPropUser.CustomerID + " end else begin RAISERROR ('Locations are assigned to the selected customer, customer cannot be deleted!', 16, 1) RETURN end");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerByListID(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   Loc \n");
            //varname1.Append("              WHERE  Owner = (SELECT TOP 1 ID \n");
            //varname1.Append("                   FROM   owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and  QBCustomerID = '" + objPropUser.QBCustomerID + "')) \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      DELETE FROM rol \n");
            //varname1.Append("      WHERE  id = (SELECT TOP 1 rol \n");
            //varname1.Append("                   FROM   owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and QBCustomerID = '" + objPropUser.QBCustomerID + "') \n");
            //varname1.Append(" \n");
            //varname1.Append("      DELETE FROM Owner \n");
            //varname1.Append("                   WHERE isnull( QBCustomerID,'')<>'' and QBCustomerID = '" + objPropUser.QBCustomerID + "' \n");
            //varname1.Append("  END \n");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE owner SET Status = 1 WHERE Isnull( QBCustomerID,'')<>'' AND  QBCustomerID = '" + objPropUser.QBCustomerID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteCustomerByListID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerBySageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteCustomerBySageID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocationBySageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteLocationBySageID", objPropUser.QBCustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEquipment(User objPropUser)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "if not exists (select 1 from TicketO where LElev = " + objPropUser.EquipID + " union select 1 from TicketD where Elev=" + objPropUser.EquipID + " union select 1 from tblJoinElevJob where elev=" + objPropUser.EquipID + " ) begin delete from Elev where ID=" + objPropUser.EquipID + " end else begin RAISERROR ('Selected equipment is in use, equipment cannot be deleted!', 16, 1) RETURN end");
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteEquipment", objPropUser.EquipID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocation(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   TicketO \n");
            varname1.Append("              WHERE ltype=0 and  LID = " + objPropUser.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   TicketD \n");
            varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   job \n");
            varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Elev \n");
            varname1.Append("              WHERE  Loc = " + objPropUser.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Lead ld INNER JOIN Loc l ON l.Rol=ld.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM ToDo t INNER JOIN Loc l ON l.Rol=t.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Done d INNER JOIN Loc l ON l.Rol=d.Rol WHERE l.Loc=" + objPropUser.LocID + " \n");
            varname1.Append("UNION SELECT 1 FROM Estimate e INNER JOIN Loc l ON l.Rol=e.RolID WHERE l.Loc=" + objPropUser.LocID + " \n");
            varname1.Append("              UNION \n");
            varname1.Append("              SELECT 1 \n");
            varname1.Append("              FROM   Invoice \n");
            varname1.Append("              WHERE  Loc = " + objPropUser.LocID + ") \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("    delete from rol where id=(select top 1 rol from loc where loc =" + objPropUser.LocID + ")   DELETE FROM Loc \n");
            varname1.Append("      WHERE  loc = " + objPropUser.LocID + " \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      RAISERROR ('Selected location is in use, location cannot be deleted!',16,1) \n");
            varname1.Append(" \n");
            varname1.Append("      RETURN \n");
            varname1.Append("  END ");


            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());// "if not exists(select 1 from TicketO where LID=" + objPropUser.LocID + " union select 1 from TicketD where Loc=" + objPropUser.LocID + "union select 1 from job where Loc=" + objPropUser.LocID + ") begin delete from Loc where loc=" + objPropUser.LocID + " end else begin RAISERROR ('Selected location is in use, location cannot be deleted!', 16, 1) RETURN end"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteLocationByListID(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   TicketO \n");
            //varname1.Append("              WHERE  LID = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   TicketD \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Job \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Elev \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append("              UNION \n");
            //varname1.Append("              SELECT 1 \n");
            //varname1.Append("              FROM   Invoice \n");
            //varname1.Append("              WHERE  Loc = (SELECT Loc \n");
            //varname1.Append("                            FROM   Loc \n");
            //varname1.Append("                            WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "')) \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      DELETE FROM Rol \n");
            //varname1.Append("      WHERE  ID = (SELECT TOP 1 rol \n");
            //varname1.Append("                   FROM   loc \n");
            //varname1.Append("                   WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "') \n");
            //varname1.Append(" \n");
            //varname1.Append("      DELETE FROM Loc \n");
            //varname1.Append("      WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "' \n");

            //varname1.Append("INSERT INTO tblSyncDeleted \n");
            //varname1.Append("            (Tbl, \n");
            //varname1.Append("             NAME, \n");
            //varname1.Append("             RefID, \n");
            //varname1.Append("             QBID) \n");
            //varname1.Append("VALUES      ('LOC', \n");
            //varname1.Append("             (select tag from loc where WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "'), \n");
            //varname1.Append("             (select loc from loc where WHERE isnull( QBLocID,'')<>'' and  QBLocID = '" + objPropUser.QBlocationID + "'), \n");
            //varname1.Append("             '" + objPropUser.QBlocationID + "' ) \n");

            //varname1.Append("  END ");
            //varname1.Append("else \n");
            //varname1.Append("  BEGIN");
            //varname1.Append("  UPDATE Loc SET Status=1 WHERE Isnull( QBLocID,'')<>'' AND  QBLocID = '" + objPropUser.QBlocationID + "' \n");
            //varname1.Append("  end");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spDeleteLocationByListID", objPropUser.QBlocationID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteEmployeeByListID(User objPropUser)
        {
            string strQuery = " update tbluser set status = 1 where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "' ";
            strQuery += " update tblWork set Status= 1 where fDesc = (select fuser from tblUser where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "') ";
            strQuery += " update Emp set Status= 1 where CallSign = (select fuser from tblUser where isnull(qbemployeeid,'')<>'' and qbemployeeid= '" + objPropUser.QBEmployeeID + "') ";

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocType(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from  loctype where type= '" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocTypeByListID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from loctype where isnull(qbloctypeid,'')<>'' and qbloctypeid= '" + objPropUser.QBJobtypeID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDatabase(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spCreateDB", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDatabaseName(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spAddControl", objPropUser.FirstName, objPropUser.DBName, objPropUser.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDatabaseName(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spUpdateControl", objPropUser.FirstName, objPropUser.DBName, objPropUser.CtrlID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateAdminPassword(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tbluser set username='" + objPropUser.Username + "', password='" + objPropUser.Password + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAdminPassword(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select username, password from tbluser");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[45];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Route";
            para[7].SqlDbType = SqlDbType.Int;
            para[7].Value = objPropUser.Route;

            para[8] = new SqlParameter();
            para[8].ParameterName = "Terr";
            para[8].SqlDbType = SqlDbType.Int;
            para[8].Value = objPropUser.Territory;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "contactname";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.MainContact;

            para[11] = new SqlParameter();
            para[11].ParameterName = "Phone";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Phone;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Fax";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Fax;

            para[13] = new SqlParameter();
            para[13].ParameterName = "cellular";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Cell;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Email";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Email;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolAddress";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolAddress;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolCity";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolCity;

            para[18] = new SqlParameter();
            para[18].ParameterName = "RolState";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.RolState;

            para[19] = new SqlParameter();
            para[19].ParameterName = "RolZip";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.RolZip;

            para[20] = new SqlParameter();
            para[20].ParameterName = "ContactData";
            para[20].SqlDbType = SqlDbType.Structured;
            para[20].Value = objPropUser.ContactData;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Type";
            para[21].SqlDbType = SqlDbType.VarChar;
            para[21].Value = objPropUser.Type;

            para[22] = new SqlParameter();
            para[22].ParameterName = "Locid";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.LocID;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Owner";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.CustomerID;

            para[24] = new SqlParameter();
            para[24].ParameterName = "stax";
            para[24].SqlDbType = SqlDbType.VarChar;
            para[24].Value = objPropUser.Stax;

            para[25] = new SqlParameter();
            para[25].ParameterName = "Lat";
            para[25].SqlDbType = SqlDbType.VarChar;
            para[25].Value = objPropUser.Lat;

            para[26] = new SqlParameter();
            para[26].ParameterName = "Lng";
            para[26].SqlDbType = SqlDbType.VarChar;
            para[26].Value = objPropUser.Lng;

            para[27] = new SqlParameter();
            para[27].ParameterName = "Custom1";
            para[27].SqlDbType = SqlDbType.VarChar;
            para[27].Value = objPropUser.Custom1;

            para[28] = new SqlParameter();
            para[28].ParameterName = "Custom2";
            para[28].SqlDbType = SqlDbType.VarChar;
            para[28].Value = objPropUser.Custom2;

            para[29] = new SqlParameter();
            para[29].ParameterName = "To";
            para[29].SqlDbType = SqlDbType.VarChar;
            para[29].Value = objPropUser.ToMail;

            para[30] = new SqlParameter();
            para[30].ParameterName = "CC";
            para[30].SqlDbType = SqlDbType.VarChar;
            para[30].Value = objPropUser.CCMail;

            para[31] = new SqlParameter();
            para[31].ParameterName = "ToInv";
            para[31].SqlDbType = SqlDbType.VarChar;
            para[31].Value = objPropUser.MailToInv;

            para[32] = new SqlParameter();
            para[32].ParameterName = "CCInv";
            para[32].SqlDbType = SqlDbType.VarChar;
            para[32].Value = objPropUser.MailCCInv;

            para[33] = new SqlParameter();
            para[33].ParameterName = "CreditHold";
            para[33].SqlDbType = SqlDbType.TinyInt;
            para[33].Value = objPropUser.CreditHold;

            para[34] = new SqlParameter();
            para[34].ParameterName = "DispAlert";
            para[34].SqlDbType = SqlDbType.TinyInt;
            para[34].Value = objPropUser.DispAlert;

            para[35] = new SqlParameter();
            para[35].ParameterName = "CreditReason";
            para[35].SqlDbType = SqlDbType.VarChar;
            para[35].Value = objPropUser.CreditReason;

            para[36] = new SqlParameter();                   //added by Mayuri 24th dec,15
            para[36].ParameterName = "ContractBill";
            para[36].SqlDbType = SqlDbType.TinyInt;
            para[36].Value = objPropUser.ContractBill;

            para[37] = new SqlParameter();
            para[37].ParameterName = "terms";
            para[37].SqlDbType = SqlDbType.Int;
            para[37].Value = objPropUser.TermsID;

            para[38] = new SqlParameter();
            para[38].ParameterName = "@Docs";
            para[38].SqlDbType = SqlDbType.Structured;
            para[38].Value = objPropUser.dtDocs;

            //para[24] = new SqlParameter();
            //para[24].ParameterName = "MAPAddress";
            //para[24].SqlDbType = SqlDbType.VarChar;
            //para[24].Value = objPropUser.MAPAddress;
            para[39] = new SqlParameter();
            para[39].ParameterName = "BillRate";
            para[39].SqlDbType = SqlDbType.Decimal;
            para[39].Value = objPropUser.BillRate;

            para[40] = new SqlParameter();
            para[40].ParameterName = "OT";
            para[40].SqlDbType = SqlDbType.Decimal;
            para[40].Value = objPropUser.RateOT;

            para[41] = new SqlParameter();
            para[41].ParameterName = "NT";
            para[41].SqlDbType = SqlDbType.Decimal;
            para[41].Value = objPropUser.RateNT;

            para[42] = new SqlParameter();
            para[42].ParameterName = "DT";
            para[42].SqlDbType = SqlDbType.Decimal;
            para[42].Value = objPropUser.RateDT;

            para[43] = new SqlParameter();
            para[43].ParameterName = "Travel";
            para[43].SqlDbType = SqlDbType.Decimal;
            para[43].Value = objPropUser.RateTravel;

            para[44] = new SqlParameter();
            para[44].ParameterName = "Mileage";
            para[44].SqlDbType = SqlDbType.Decimal;
            para[44].Value = objPropUser.MileageRate;
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateLocation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomer(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[34];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "ContactData";
            para[12].SqlDbType = SqlDbType.Structured;
            para[12].Value = objPropUser.ContactData;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Internet";
            para[13].SqlDbType = SqlDbType.Int;
            para[13].Value = objPropUser.Internet;

            para[14] = new SqlParameter();
            para[14].ParameterName = "CustomerId";
            para[14].SqlDbType = SqlDbType.Int;
            para[14].Value = objPropUser.CustomerID;

            para[15] = new SqlParameter();
            para[15].ParameterName = "contact";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.MainContact;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Phone";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Phone;

            para[17] = new SqlParameter();
            para[17].ParameterName = "Website";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Website;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Email";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Email;

            para[19] = new SqlParameter();
            para[19].ParameterName = "cell";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.Cell;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Type";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropUser.Type;

            para[21] = new SqlParameter();
            para[21].ParameterName = "Equipment";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.EquipID;

            //para[21] = new SqlParameter();
            //para[21].ParameterName = "TS";
            //para[21].SqlDbType = SqlDbType.SmallInt;
            //para[21].Value = objPropUser.IsTSDatabase;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageOwnerID";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.AccountNo;

            para[23] = new SqlParameter();
            para[23].ParameterName = "Billing";
            para[23].SqlDbType = SqlDbType.Int;
            para[23].Value = objPropUser.Billing;

            para[24] = new SqlParameter();
            para[24].ParameterName = "Central";
            para[24].SqlDbType = SqlDbType.Int;
            para[24].Value = objPropUser.Central;

            para[25] = new SqlParameter();
            para[25].ParameterName = "@grpbywo";
            para[25].SqlDbType = SqlDbType.Int;
            para[25].Value = objPropUser.grpbyWO;

            para[26] = new SqlParameter();
            para[26].ParameterName = "@Docs";
            para[26].SqlDbType = SqlDbType.Structured;
            para[26].Value = objPropUser.dtDocs;

            para[27] = new SqlParameter();
            para[27].ParameterName = "@openticket";
            para[27].SqlDbType = SqlDbType.Int;
            para[27].Value = objPropUser.openticket;

            para[28] = new SqlParameter();
            para[28].ParameterName = "BillRate";
            para[28].SqlDbType = SqlDbType.Decimal;
            para[28].Value = objPropUser.BillRate;

            para[29] = new SqlParameter();
            para[29].ParameterName = "OT";
            para[29].SqlDbType = SqlDbType.Decimal;
            para[29].Value = objPropUser.RateOT;

            para[30] = new SqlParameter();
            para[30].ParameterName = "NT";
            para[30].SqlDbType = SqlDbType.Decimal;
            para[30].Value = objPropUser.RateNT;

            para[31] = new SqlParameter();
            para[31].ParameterName = "DT";
            para[31].SqlDbType = SqlDbType.Decimal;
            para[31].Value = objPropUser.RateDT;

            para[32] = new SqlParameter();
            para[32].ParameterName = "Travel";
            para[32].SqlDbType = SqlDbType.Decimal;
            para[32].Value = objPropUser.RateTravel;

            para[33] = new SqlParameter();
            para[33].ParameterName = "Mileage";
            para[33].SqlDbType = SqlDbType.Decimal;
            para[33].Value = objPropUser.MileageRate;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomer", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerContact(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[2];

            para[0] = new SqlParameter();
            para[0].ParameterName = "ContactData";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.ContactData;

            para[1] = new SqlParameter();
            para[1].ParameterName = "rolid";
            para[1].SqlDbType = SqlDbType.Int;
            para[1].Value = objPropUser.RolId;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateContact", para[0], para[1]);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUser(User objPropUser)
        {
            try
            {
                object obj = DBNull.Value;
                object objMerchant = DBNull.Value;
                object objSDate = DBNull.Value;
                object objEDate = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    obj = objPropUser.DtFired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }
                if (objPropUser.FStart != System.DateTime.MinValue)
                {
                    objSDate = objPropUser.FStart;
                }
                if (objPropUser.FEnd != System.DateTime.MinValue)
                {
                    objEDate = objPropUser.FEnd;
                }

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spupdateUser", objPropUser.Username, objPropUser.PDA, objPropUser.Field, objPropUser.Status, objPropUser.FirstName, objPropUser.MiddleName, objPropUser.LastNAme, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Cell, objPropUser.Email, objPropUser.DtHired, obj, objPropUser.CreateTicket, objPropUser.WorkDate, objPropUser.LocationRemarks, objPropUser.ServiceHist, objPropUser.PurchaseOrd, objPropUser.Expenses, objPropUser.ProgFunctions, objPropUser.AccessUser, objPropUser.UserID, objPropUser.RolId, objPropUser.WorkId, objPropUser.EmpId, objPropUser.Mapping, objPropUser.Schedule, objPropUser.Password, objPropUser.DeviceID, objPropUser.Pager, objPropUser.Supervisor, objPropUser.Salesperson, objPropUser.UserLic, objPropUser.UserLicID, objPropUser.Remarks, objPropUser.Lang, objMerchant, objPropUser.DefaultWorker, objPropUser.Dispatch, objPropUser.SalesMgr, objPropUser.MassReview, objPropUser.MOMUSer, objPropUser.MOMPASS, objPropUser.InServer, "POP3", objPropUser.InUsername, objPropUser.InPassword, objPropUser.InPort, objPropUser.OutServer, objPropUser.OutUsername, objPropUser.OutPassword, objPropUser.OutPort, objPropUser.SSL, objPropUser.EmailAccount, objPropUser.HourlyRate, objPropUser.EmpMaintenance, objPropUser.Timestampfix, objPropUser.PayMethod, objPropUser.PHours, objPropUser.Salary, objPropUser.Department, objPropUser.EmpRefID, objPropUser.PayPeriod, objPropUser.MileageRate, objPropUser.AddEquip, objPropUser.EditEquip, objPropUser.FChart, objPropUser.FGLAdj, objPropUser.AddChart, objPropUser.EditChart, objPropUser.ViewChart, objPropUser.AddGLAdj, objPropUser.EditGLAdj, objPropUser.ViewGLAdj, objPropUser.FDeposit, objPropUser.AddDeposit, objPropUser.EditDeposit, objPropUser.ViewDeposit, objPropUser.FCustomerPayment, objPropUser.AddCustomerPayment, objPropUser.EditCustomerPayment, objPropUser.ViewCustomerPayment, objPropUser.FinanStatement, objSDate, objEDate, objPropUser.APVendor, objPropUser.APBill, objPropUser.APBillPay, objPropUser.APBillSelect);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateUserPermission(User objPropUser)
        {
            try
            {
                SqlParameter para = new SqlParameter();
                para.ParameterName = "@UserID";
                para.SqlDbType = SqlDbType.Int;
                para.Value = objPropUser.UserID;

                SqlParameter para1 = new SqlParameter();
                para1.ParameterName = "@Pages";
                para1.SqlDbType = SqlDbType.Structured;
                para1.Value = objPropUser.dtPageData;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddPagePermission", para, para1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTSUser(User objPropUser)
        {
            try
            {
                object obj = DBNull.Value;
                object objMerchant = DBNull.Value;

                if (objPropUser.DtFired != System.DateTime.MinValue)
                {
                    obj = objPropUser.DtFired;
                }
                if (objPropUser.MerchantInfoId != 0)
                {
                    objMerchant = objPropUser.MerchantInfoId;
                }

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spupdateTSUser", objPropUser.Username, objPropUser.PDA, objPropUser.Field, objPropUser.Status, objPropUser.FirstName, objPropUser.MiddleName, objPropUser.LastNAme, objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Tele, objPropUser.Cell, objPropUser.Email, objPropUser.DtHired, obj, objPropUser.CreateTicket, objPropUser.WorkDate, objPropUser.LocationRemarks, objPropUser.ServiceHist, objPropUser.PurchaseOrd, objPropUser.Expenses, objPropUser.ProgFunctions, objPropUser.AccessUser, objPropUser.UserID, objPropUser.RolId, objPropUser.WorkId, objPropUser.EmpId, objPropUser.Mapping, objPropUser.Schedule, objPropUser.Password, objPropUser.DeviceID, objPropUser.Pager, objPropUser.Supervisor, objPropUser.Salesperson, objPropUser.UserLic, objPropUser.UserLicID, objPropUser.Remarks, objPropUser.Lang, objMerchant, objPropUser.DefaultWorker, objPropUser.Dispatch, objPropUser.SalesMgr, objPropUser.MassReview);//, objPropUser.MOMUSer, objPropUser.MOMPASS, objPropUser.InServer, "POP3", objPropUser.InUsername, objPropUser.InPassword, objPropUser.InPort, objPropUser.OutServer, objPropUser.OutUsername, objPropUser.OutPassword, objPropUser.OutPort, objPropUser.SSL, objPropUser.EmailAccount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBCustomerID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update owner set QBCustomerID='" + objPropUser.QBCustomerID + "' where ID=" + objPropUser.CustomerID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBsalestaxID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update stax set QBstaxID='" + objPropUser.QBSalesTaxID + "' where name='" + objPropUser.SalesTax + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBDepartmentID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update jobtype set QBjobtypeID='" + objPropUser.QBJobtypeID + "' where ID='" + objPropUser.DepartmentID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBInvID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update inv set QBinvID='" + objPropUser.QBInvID + "' where ID='" + objPropUser.BillCode + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBTermsID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tblterms set QBtermsID='" + objPropUser.QBTermsID + "' where ID='" + objPropUser.TermsID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBAccountID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update chart set QBAccountID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBVendorID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update vendor set QBvendorID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBWageID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update prwage set QBwageID='" + objPropUser.QBAccountID + "' where ID='" + objPropUser.AccountID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateQBJobtypeID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loctype set QBloctypeID='" + objPropUser.QBCustomerTypeID + "' where type='" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBcustomertypeID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update otype set QBcustomertypeID='" + objPropUser.QBCustomerTypeID + "' where type='" + objPropUser.CustomerType + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBLocationID(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update loc set QBlocID='" + objPropUser.QBlocationID + "' where loc=" + objPropUser.LocID + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateDBObjects(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, objPropUser.Script);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Owner where Type= t.Type ) as Count from otype t");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getcategoryAll(User objPropUser)
        {
            string strCommandtext = "select Type,Remarks,icon, ((select Count(1)from TicketO where Cat= t.Type )+ (select COUNT(1)from TicketD where Cat=t.type)) as Count, isnull(Chargeable,0) as Chargeable, isnull(isdefault,0) as isdefault from Category t";

            if (!string.IsNullOrEmpty(objPropUser.Cat))
            {
                strCommandtext += " where type='" + objPropUser.Cat + "'";
            }
            strCommandtext += " order by type";
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strCommandtext);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEquiptype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select edesc,(select count(1) from elev where type=e.edesc) as Count from ElevatorSpec e  where ecat=1 order by edesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEquipmentCategory(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT ElevatorSpec.Edesc  FROM ElevatorSpec WHERE ECat=0 ORDER BY eDesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMCPS(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT ID,Name  FROM tblMCPStatus ORDER BY name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getServiceType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select l.type,l.fdesc,l.remarks,(select count(1) from elev where cat=l.type) as Count,l.InvID, isnull(i.Name,'') as Name from ltype l left join Inv i on l.InvID=i.ID order by fdesc");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBillCodes(User objPropUser)
        {
            string strQuery = "select (select top 1 acct+' : '+fdesc from chart where id = sacct)as account, sacct, case status when 0 then 'Active' else 'Inactive' end as Status,status as statusid, (select top 1 name from warehouse where id= warehouse) as warehousename,warehouse, id, Name,fDesc,Cat,Balance,Measure,Remarks, (select type from JobType where ID=Cat)as jobtype,  case type when 1 then Name +' : Service' when 0 then Name+ ' : Part' else Name end as BillType, Price1 from Inv ";//+ space(20-len(Name))

            if (!string.IsNullOrEmpty(objPropUser.Type))
            {
                strQuery += " where type=" + objPropUser.Type;
            }

            strQuery += " order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getBillCodesByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select id, Name,fDesc,Cat,Balance,Measure,Remarks, (select type from JobType where ID=Cat)as jobtype, Price1 from Inv where id=" + objPropUser.BillCode + " order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getSalesTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesTaxByTaxType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and  UType=" + objPropUser.UType + "  order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMSalesTax(User objPropUser)
        {
            string strQuery = "select isnull(IsTaxable,0) as IsTax,*, (select QBvendorID from vendor where acct='Mobile Service Manager' and QBvendorID is not null) as QBvendorID from stax  ";

            if (objPropUser.SearchValue == "1")
            {
                strQuery += " where QBStaxID is not null and LastUpdateDate >= (select QBLastSync from Control)";
            }
            else if (objPropUser.SearchValue == "0")
            {
                strQuery += " where QBStaxID is null";
            }
            strQuery += " order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBSalesTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from stax where QBStaxID is not null order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMLoctype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from loctype where QBloctypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype where QBjobtypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMBillcode(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetMSMBillcode", Convert.ToInt32(objPropUser.SearchValue));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMterms(User objPropUser)
        {
            string strQuery = "select *,(Name +' (' +CAST( ID as varchar(50)) + ')') as dupname from tblterms where QBtermsID is null ";

            if (!string.IsNullOrEmpty(objPropUser.SearchValue))
            {
                strQuery += "and ID in (" + objPropUser.SearchValue.Trim() + ")";
            }

            strQuery += " order by name";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMAccount(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetAccount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMPatrollWage(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetPayrollItem");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMVendor(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spQBGetVendor");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTerms(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblterms order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBDepartment(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from jobtype WHERE  QBjobtypeID IS NOT NULL and LastUpdateDate >= (select QBLastSync from Control) order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getMSMCustomertype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from otype where QBCustomerTypeID is null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBCustomertype(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from otype where QBCustomerTypeID is not null order by type");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesTaxByID(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from stax where name='" + objPropUser.Stax + "' order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddSalesTax(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("IF NOT Exists (select 1 from STax where Name = '" + objPropUser.SalesTax + "' )\n");
            QueryText.Append("BEGIN \n");
            QueryText.Append("INSERT INTO STax \n");
            QueryText.Append("            (Name, \n");
            QueryText.Append("             fDesc, \n");
            QueryText.Append("             Rate, \n");
            QueryText.Append("             State, \n");
            QueryText.Append("             GL, \n");
            QueryText.Append("             Type, \n");
            QueryText.Append("             UType, \n");
            QueryText.Append("             PstReg, \n");
            QueryText.Append("             lastupdatedate, \n");
            QueryText.Append("             Remarks) \n");
            QueryText.Append("VALUES      ( '" + objPropUser.SalesTax + "', \n");
            QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("              " + objPropUser.SalesRate + ", \n");
            QueryText.Append("              '" + objPropUser.State + "', \n");
            QueryText.Append("              '" + objPropUser.GLAccount + "', \n");
            QueryText.Append("              " + objPropUser.sType + ", \n");
            QueryText.Append("              '" + objPropUser.UType + "', \n");
            QueryText.Append("              '" + objPropUser.PSTReg + "', \n");
            QueryText.Append("              getdate(), \n");
            QueryText.Append("              '" + objPropUser.Remarks + "' ) \n");
            QueryText.Append("END \n");
            QueryText.Append("ELSE \n");
            QueryText.Append("BEGIN \n");
            QueryText.Append("RAISERROR ('Name already exists, please use different name !',16,1) \n");
            QueryText.Append("RETURN \n");
            QueryText.Append("END \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSalesTax(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("UPDATE STax \n");
            QueryText.Append("SET    fDesc = '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("       Rate = " + objPropUser.SalesRate + ", \n");
            QueryText.Append("       State = '" + objPropUser.State + "', \n");
            QueryText.Append("       lastupdatedate = getdate(), \n");
            QueryText.Append("       Remarks = '" + objPropUser.Remarks + "', \n");
            QueryText.Append("       Type = " + objPropUser.sType + ", \n");
            QueryText.Append("       PSTReg = '" + objPropUser.PSTReg + "', \n");
            QueryText.Append("       GL = " + objPropUser.GLAccount + " \n");
            QueryText.Append("WHERE  Name = '" + objPropUser.SalesTax + "' ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBSalesTax(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            varname1.Append("              FROM   STax \n");
            varname1.Append("              WHERE  QBStaxID = '" + objPropUser.QBSalesTaxID + "') \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      INSERT INTO STax \n");
            varname1.Append("                  (Name, \n");
            varname1.Append("                   fDesc, \n");
            varname1.Append("                   Rate, \n");
            varname1.Append("                   State, \n");
            varname1.Append("                   IsTaxable, \n");
            varname1.Append("                   GL, \n");
            varname1.Append("                   Type, \n");
            varname1.Append("                   UType, \n");
            varname1.Append("                   PstReg, \n");
            varname1.Append("                   QBStaxID, \n");
            varname1.Append("                   Remarks) \n");
            varname1.Append("VALUES      ( '" + objPropUser.SalesTax + "', \n");
            varname1.Append("              '" + objPropUser.SalesDescription + "', \n");
            varname1.Append("              " + objPropUser.SalesRate + ", \n");
            varname1.Append("              '" + objPropUser.State + "', \n");
            varname1.Append("              " + objPropUser.IsTaxable + ", \n");
            varname1.Append("              9, \n");
            varname1.Append("              0, \n");
            varname1.Append("              1, \n");
            varname1.Append("              '', \n");
            varname1.Append("              '" + objPropUser.QBSalesTaxID + "', \n");
            varname1.Append("              '" + objPropUser.Remarks + "' ) \n");
            varname1.Append("  END \n");
            varname1.Append("ELSE \n");
            varname1.Append("  BEGIN \n");
            varname1.Append("      UPDATE STax \n");
            varname1.Append("      SET    fDesc = '" + objPropUser.SalesDescription + "', \n");
            varname1.Append("             Rate = " + objPropUser.SalesRate + " \n");
            varname1.Append("      WHERE  QBStaxID = '" + objPropUser.QBSalesTaxID + "' AND Isnull(LastUpdateDate, '01/01/1900') < '" + objPropUser.LastUpdateDate + "' \n");
            varname1.Append("  END ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddDepartment(User objPropUser)
        {
            //StringBuilder QueryText = new StringBuilder();
            //QueryText.Append("update JobType set isdefault =0  INSERT INTO JobType \n");
            ////QueryText.Append("            (ID, \n");
            //QueryText.Append("            ( Type, \n");
            //QueryText.Append("              isdefault, \n");
            //QueryText.Append("             Remarks, \n");
            //QueryText.Append("             LastUpdateDate) \n");
            ////QueryText.Append("VALUES      ( 0, \n");
            //QueryText.Append("VALUES      ( '" + objPropUser.Type + "', \n");
            //QueryText.Append("       " + objPropUser.Default + ", \n");
            ////QueryText.Append("              '"+objPropUser.Type+"', \n");
            //QueryText.Append("              '" + objPropUser.Remarks + "' , ");
            //QueryText.Append("              GETDATE() ) ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddDepartment", objPropUser.Type, objPropUser.Default, objPropUser.Remarks);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddWage(Wage _objWage)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, "spAddWage", _objWage.Name, _objWage.Field, _objWage.Reg, _objWage.OT1, _objWage.OT2, _objWage.TT, _objWage.FIT, _objWage.FICA, _objWage.MEDI, _objWage.FUTA, _objWage.SIT, _objWage.Vac, _objWage.WC, _objWage.Uni, _objWage.GL, _objWage.NT, _objWage.MileageGL, _objWage.ReimGL, _objWage.ZoneGL, _objWage.Globe, _objWage.Status, _objWage.CReg, _objWage.COT, _objWage.CDT, _objWage.CNT, _objWage.CTT, _objWage.Remarks, _objWage.RegGL, _objWage.OTGL, _objWage.NTGL, _objWage.DTGL, _objWage.TTGL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBDepartment(User objPropUser)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("IF NOT EXISTS(SELECT 1 \n");
            //varname1.Append("              FROM   JobType \n");
            //varname1.Append("              WHERE  QBJobTypeID = '" + objPropUser.QBJobtypeID + "') \n");
            //varname1.Append("  BEGIN \n");
            //varname1.Append("      INSERT INTO JobType \n");
            //varname1.Append("                  (type, \n");
            //varname1.Append("                   remarks, \n");
            //varname1.Append("                   isdefault, \n");
            //varname1.Append("                   QBJobTypeID) \n");
            ////varname1.Append("                   LastUpdateDate) \n");
            //varname1.Append("      VALUES      ('" + objPropUser.Type + "', \n");
            //varname1.Append("                   '" + objPropUser.Remarks + "', \n");
            //varname1.Append("                   0, \n");
            //varname1.Append("                   '" + objPropUser.QBJobtypeID + "') \n");
            ////varname1.Append("                   Getdate()) \n");
            //varname1.Append("  END \n");           

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "AddQbJobType", objPropUser.QBJobtypeID, objPropUser.Type, objPropUser.Remarks, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBTerms(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQbTerms", objPropUser.QBTermsID, objPropUser.Type, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBPayrollWage(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBPayrollWage", objPropUser.QBWageID, objPropUser.Type, objPropUser.LastUpdateDate, objPropUser.QBAccountID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDepartment(User objPropUser)
        {
            ////StringBuilder QueryText = new StringBuilder();
            ////QueryText.Append("UPDATE JobType \n");
            ////QueryText.Append("SET    Remarks = '" + objPropUser.Remarks + "' \n");            
            ////QueryText.Append("WHERE  type = '" + objPropUser.Type + "' ");

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("update JobType set isdefault =0  UPDATE JobType \n");
            //varname1.Append("SET    Remarks = '" + objPropUser.Remarks + "', \n");
            //varname1.Append("    isdefault = " + objPropUser.Default + ", \n");
            //varname1.Append("       Type = '" + objPropUser.Type + "', \n");
            //varname1.Append("       LastUpdateDate = GETDATE() \n");
            //varname1.Append("WHERE  ID = " + objPropUser.JobtypeID + " ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateDepartment", objPropUser.Type, objPropUser.Default, objPropUser.Remarks, objPropUser.JobtypeID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateWage(Wage _objWage)
        {
            try
            {
                SqlParameter[] para = new SqlParameter[33];

                para[0] = new SqlParameter();
                para[0].ParameterName = "ID";
                para[0].SqlDbType = SqlDbType.Int;
                para[0].Value = _objWage.ID;

                para[1] = new SqlParameter();
                para[1].ParameterName = "Name";
                para[1].SqlDbType = SqlDbType.VarChar;
                para[1].Value = _objWage.Name;

                para[2] = new SqlParameter();
                para[2].ParameterName = "Field";
                para[2].SqlDbType = SqlDbType.Int;
                para[2].Value = _objWage.Field;

                para[3] = new SqlParameter();
                para[3].ParameterName = "Reg";
                para[3].SqlDbType = SqlDbType.Decimal;
                para[3].Value = _objWage.Reg;

                para[4] = new SqlParameter();
                para[4].ParameterName = "OT1";
                para[4].SqlDbType = SqlDbType.Decimal;
                para[4].Value = _objWage.OT1;

                para[5] = new SqlParameter();
                para[5].ParameterName = "OT2";
                para[5].SqlDbType = SqlDbType.Decimal;
                para[5].Value = _objWage.OT2;

                para[6] = new SqlParameter();
                para[6].ParameterName = "TT";
                para[6].SqlDbType = SqlDbType.Decimal;
                para[6].Value = _objWage.TT;

                para[7] = new SqlParameter();
                para[7].ParameterName = "FIT";
                para[7].SqlDbType = SqlDbType.SmallInt;
                para[7].Value = _objWage.FIT;

                para[8] = new SqlParameter();
                para[8].ParameterName = "FICA";
                para[8].SqlDbType = SqlDbType.SmallInt;
                para[8].Value = _objWage.FICA;

                para[9] = new SqlParameter();
                para[9].ParameterName = "MEDI";
                para[9].SqlDbType = SqlDbType.SmallInt;
                para[9].Value = _objWage.MEDI;

                para[10] = new SqlParameter();
                para[10].ParameterName = "FUTA";
                para[10].SqlDbType = SqlDbType.SmallInt;
                para[10].Value = _objWage.FUTA;

                para[11] = new SqlParameter();
                para[11].ParameterName = "SIT";
                para[11].SqlDbType = SqlDbType.SmallInt;
                para[11].Value = _objWage.SIT;

                para[12] = new SqlParameter();
                para[12].ParameterName = "Vac";
                para[12].SqlDbType = SqlDbType.SmallInt;
                para[12].Value = _objWage.Vac;

                para[13] = new SqlParameter();
                para[13].ParameterName = "WC";
                para[13].SqlDbType = SqlDbType.SmallInt;
                para[13].Value = _objWage.WC;

                para[14] = new SqlParameter();
                para[14].ParameterName = "Uni";
                para[14].SqlDbType = SqlDbType.SmallInt;
                para[14].Value = _objWage.Uni;

                para[15] = new SqlParameter();
                para[15].ParameterName = "GL";
                para[15].SqlDbType = SqlDbType.Int;
                para[15].Value = _objWage.GL;

                para[16] = new SqlParameter();
                para[16].ParameterName = "NT";
                para[16].SqlDbType = SqlDbType.Decimal;
                para[16].Value = _objWage.NT;

                para[17] = new SqlParameter();
                para[17].ParameterName = "MileageGL";
                para[17].SqlDbType = SqlDbType.Int;
                para[17].Value = _objWage.MileageGL;

                para[18] = new SqlParameter();
                para[18].ParameterName = "ReimGL";
                para[18].SqlDbType = SqlDbType.Int;
                para[18].Value = _objWage.ReimGL;

                para[19] = new SqlParameter();
                para[19].ParameterName = "ZoneGL";
                para[19].SqlDbType = SqlDbType.Int;
                para[19].Value = _objWage.ZoneGL;

                para[20] = new SqlParameter();
                para[20].ParameterName = "Globe";
                para[20].SqlDbType = SqlDbType.SmallInt;
                para[20].Value = _objWage.Globe;

                para[21] = new SqlParameter();
                para[21].ParameterName = "Status";
                para[21].SqlDbType = SqlDbType.SmallInt;
                para[21].Value = _objWage.Status;

                para[22] = new SqlParameter();
                para[22].ParameterName = "CReg";
                para[22].SqlDbType = SqlDbType.Decimal;
                para[22].Value = _objWage.CReg;

                para[23] = new SqlParameter();
                para[23].ParameterName = "COT";
                para[23].SqlDbType = SqlDbType.Decimal;
                para[23].Value = _objWage.COT;

                para[24] = new SqlParameter();
                para[24].ParameterName = "CDT";
                para[24].SqlDbType = SqlDbType.Decimal;
                para[24].Value = _objWage.CDT;

                para[25] = new SqlParameter();
                para[25].ParameterName = "CNT";
                para[25].SqlDbType = SqlDbType.Decimal;
                para[25].Value = _objWage.CNT;

                para[26] = new SqlParameter();
                para[26].ParameterName = "CTT";
                para[26].SqlDbType = SqlDbType.Decimal;
                para[26].Value = _objWage.CTT;

                para[27] = new SqlParameter();
                para[27].ParameterName = "Remarks";
                para[27].SqlDbType = SqlDbType.VarChar;
                para[27].Value = _objWage.Remarks;

                para[28] = new SqlParameter();
                para[28].ParameterName = "RegGL";
                para[28].SqlDbType = SqlDbType.Int;
                para[28].Value = _objWage.RegGL;

                para[29] = new SqlParameter();
                para[29].ParameterName = "OTGL";
                para[29].SqlDbType = SqlDbType.Int;
                para[29].Value = _objWage.OTGL;

                para[30] = new SqlParameter();
                para[30].ParameterName = "NTGL";
                para[30].SqlDbType = SqlDbType.Int;
                para[30].Value = _objWage.NTGL;

                para[31] = new SqlParameter();
                para[31].ParameterName = "DTGL";
                para[31].SqlDbType = SqlDbType.Int;
                para[31].Value = _objWage.DTGL;

                para[32] = new SqlParameter();
                para[32].ParameterName = "TTGL";
                para[32].SqlDbType = SqlDbType.Int;
                para[32].Value = _objWage.TTGL;

                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.StoredProcedure, "spUpdateWage", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddBillCode(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("INSERT INTO Inv \n");
            QueryText.Append("            (Name, \n");
            QueryText.Append("             fDesc, \n");
            QueryText.Append("             status, \n");
            //QueryText.Append("             Balance, \n");
            QueryText.Append("             Price1, \n");
            QueryText.Append("             Measure, \n");
            QueryText.Append("             tax, \n");
            QueryText.Append("             AllowZero, \n");
            QueryText.Append("             inuse, \n");
            QueryText.Append("             type, \n");
            QueryText.Append("             sacct, \n");
            QueryText.Append("             Remarks, \n");
            QueryText.Append("             lastupdatedate, \n");
            QueryText.Append("             cat, warehouse) \n");
            QueryText.Append("VALUES      ( '" + objPropUser.ContactName + "', \n");
            QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              " + objPropUser.Balance + ", \n");
            QueryText.Append("              '" + objPropUser.Measure + "', \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              " + objPropUser.Type + ", \n");
            QueryText.Append("              " + objPropUser.sAcct + ", \n");
            QueryText.Append("              '" + objPropUser.Remarks + " ', ");
            QueryText.Append("              getdate(), ");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              '" + objPropUser.WarehouseID + " ') ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBBillCode(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("IF NOT EXISTS(SELECT 1 \n");
            QueryText.Append("              FROM   Inv \n");
            QueryText.Append("              WHERE  QBInvID = '" + objPropUser.QBInvID + "') \n");
            QueryText.Append("  BEGIN \n");
            QueryText.Append("INSERT INTO Inv \n");
            QueryText.Append("            (Name, \n");
            QueryText.Append("             fDesc, \n");
            QueryText.Append("             Cat, \n");
            QueryText.Append("             status, \n");
            //QueryText.Append("             Balance, \n");
            QueryText.Append("             Price1, \n");
            QueryText.Append("             Measure, \n");
            QueryText.Append("             tax, \n");
            QueryText.Append("             AllowZero, \n");
            QueryText.Append("             inuse, \n");
            QueryText.Append("             type, \n");
            QueryText.Append("             sacct, \n");
            QueryText.Append("             Remarks, \n");
            QueryText.Append("             warehouse, \n");
            QueryText.Append("             QBAccountID, \n");
            QueryText.Append("                   QBInvID) \n");
            QueryText.Append("VALUES      ( '" + objPropUser.ContactName + "', \n");
            QueryText.Append("              '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              " + objPropUser.CatStatus + ", \n");
            QueryText.Append("              " + objPropUser.Balance + ", \n");
            QueryText.Append("              '" + objPropUser.Measure + "', \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              0, \n");
            QueryText.Append("              " + objPropUser.Type + ", \n");
            QueryText.Append("              10, \n");
            QueryText.Append("              '" + objPropUser.Remarks + " ', ");
            QueryText.Append("              '" + objPropUser.WarehouseID + " ', ");
            QueryText.Append("              '" + objPropUser.QBAccountID + " ', ");
            QueryText.Append("                   '" + objPropUser.QBInvID + "') \n");
            QueryText.Append("  END \n");

            QueryText.Append("  else \n");
            QueryText.Append("  begin \n");
            QueryText.Append("UPDATE Inv \n");
            QueryText.Append("SET Name = '" + objPropUser.ContactName + "', \n");
            QueryText.Append("    fDesc = '" + objPropUser.SalesDescription + "', \n");
            //QueryText.Append("    Balance = " + objPropUser.Balance + ", \n");
            QueryText.Append("    Price1 = " + objPropUser.Balance + ", \n");
            QueryText.Append("    QBAccountID = '" + objPropUser.QBAccountID + "', \n");
            QueryText.Append("    Remarks = '" + objPropUser.Remarks + "' \n");
            QueryText.Append("WHERE  QBInvID = '" + objPropUser.QBInvID + "' \n");
            QueryText.Append("       AND Isnull(LastUpdateDate, '01/01/1900') < '" + objPropUser.LastUpdateDate + "' ");
            QueryText.Append("  END \n");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spAddQBBillCode", objPropUser.QBInvID, objPropUser.ContactName, objPropUser.SalesDescription, objPropUser.CatStatus, objPropUser.Balance, objPropUser.Measure, objPropUser.Type, objPropUser.Remarks, objPropUser.WarehouseID, objPropUser.QBAccountID, objPropUser.LastUpdateDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public void UpdateBillCode(User objPropUser)
        {
            StringBuilder QueryText = new StringBuilder();
            QueryText.Append("UPDATE Inv \n");
            QueryText.Append("SET    Name = '" + objPropUser.ContactName + "', \n");
            QueryText.Append("       fDesc = '" + objPropUser.SalesDescription + "', \n");
            QueryText.Append("       status = " + objPropUser.CatStatus + ", \n");
            QueryText.Append("       cat = " + objPropUser.CatStatus + ", \n");
            //QueryText.Append("       Balance = " + objPropUser.Balance + ", \n");
            QueryText.Append("       Price1 = " + objPropUser.Balance + ", \n");
            QueryText.Append("       Measure = '" + objPropUser.Measure + "', \n");
            QueryText.Append("       sacct = " + objPropUser.sAcct + ", \n");
            QueryText.Append("       Remarks = '" + objPropUser.Remarks + " ', \n");
            QueryText.Append("       lastupdatedate = getdate(), \n");
            QueryText.Append("       warehouse = '" + objPropUser.WarehouseID + " ' \n");
            QueryText.Append("WHERE  ID = " + objPropUser.BillCode + " ");

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateWarehouse(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Warehouse set Name='" + objPropUser.WarehouseName + "' ,Remarks='" + objPropUser.Remarks + "' where ID='" + objPropUser.WarehouseID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertWareHouse(User objPropUser)
        {
            try
            {

                StringBuilder QueryText = new StringBuilder();
                QueryText.Append("IF NOT Exists (select 1 from Warehouse where ID = '" + objPropUser.WarehouseID + "' )\n");
                QueryText.Append("BEGIN \n");
                QueryText.Append("INSERT INTO Warehouse \n");
                QueryText.Append("            (ID, \n");
                QueryText.Append("             Name, \n");
                QueryText.Append("             Remarks) \n");

                QueryText.Append("VALUES      ( '" + objPropUser.WarehouseID + "', \n");
                QueryText.Append("              '" + objPropUser.WarehouseName + "', \n");
                QueryText.Append("              '" + objPropUser.Remarks + "') \n");
                QueryText.Append("END \n");
                QueryText.Append("ELSE \n");
                QueryText.Append("BEGIN \n");
                QueryText.Append("RAISERROR ('ID already exists, please use different id !',16,1) \n");
                QueryText.Append("RETURN \n");
                QueryText.Append("END \n");


                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, QueryText.ToString());
                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "insert into Warehouse(ID,Name,Remarks) values('" + objPropUser.WarehouseID + "','" + objPropUser.WarehouseName + "','" + objPropUser.Remarks + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void DeleteDiagnostic(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete Diagnostic where category='" + objPropUser.Category + "'  and type='" + objPropUser.DiagnosticType + "'  and fdesc='" + objPropUser.Remarks + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void DeleteCustomerQB(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "delete from Rol where ID=(select Rol from Owner where QBCustomerID='" + objPropUser.QBCustomerID + "') delete from Owner where QBCustomerID='" + objPropUser.QBCustomerID + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getlocationType(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Type,Remarks, (select Count(1)from Loc where Type= t.Type ) as Count from loctype t");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomerForReport(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Name, City, State, Zip, Address, GeoLock, Remarks, o.Type, Country, fLogin, Password, Status, TicketO, TicketD, Internet, Rol, Contact, Phone, Website,EMail, Cellular,(Address+', '+City+', '+State+', '+Zip) as addressfull from Owner o left outer join Rol r on o.Rol=r.ID where o.ID=" + objPropUser.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet gettrial(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select * from tblAuth");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet gettrialUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select ua.* from tbluserAuth ua inner join tblJoinAuth ja on ua.ID=ja.lid where ja.status=0 and ua.used=1 and ja.userid=" + objPropUser.UserID + " and ja.dbname='" + objPropUser.DBName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLicenseInfoUser(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "SELECT ID as lid,str,DBname,used,dateupdate FROM tblUserAuth where dbname='" + objPropUser.DBName + "' and used=0 order by dateupdate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateTrial(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set str='" + objPropUser.Reg + "', [date]=GETDATE() , [first]=1 where first=0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateReg(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set [date]=GETDATE(), str='" + objPropUser.Reg + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public void UpdateRegUser(User objPropUser)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "if not exists(select 1 from tblUserAuth where UserID=" + objPropUser.UserID + " and DBname='" + objPropUser.DBName + "') begin insert into tblUserAuth ( DBname, UserID, str ) values ( '" + objPropUser.DBName + "', " + objPropUser.UserID + ", '" + objPropUser.Reg + "' ) end else begin update tbluserAuth set str='" + objPropUser.Reg + "' where UserID=" + objPropUser.UserID + " and DBname='" + objPropUser.DBName + "' end");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void UpdateRegUser(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spCheck", objPropUser.UserID, objPropUser.Reg, objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRolCoordinates(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT column_name 'Column_Name' FROM information_schema.columns WHERE table_name = 'rol' AND column_name = 'lng') BEGIN ALTER TABLE rol ADD lat VARCHAR(50) NULL, lng VARCHAR(50) NULL END    update Rol set lat='" + objPropUser.Lat + "' , lng='" + objPropUser.Lng + "' where id=" + objPropUser.RolId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWarehouse(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from Warehouse");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserEmail(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(r.email,'') as email from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where fuser='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserPager(User objPropUser)
        {
            try
            {
                return objPropUser.Email = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select isnull(e.pager,'') as pager from tbluser u left outer join Emp e  on u.fUser=e.CallSign left outer join Rol r on e.Rol=r.ID where fuser='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getUserDeviceID(User objPropUser)
        {
            try
            {
                return objPropUser.DeviceID = Convert.ToString(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, "select deviceid from emp where callsign='" + objPropUser.Username + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDefaultWorkerLocation(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update Loc set Route =(select top 1 ID from Route where Name='" + objPropUser.Username + "' ) where Address like '%" + objPropUser.Address + "%' and City like '" + objPropUser.City + "%'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationAddress(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUpdateLocationAddress", objPropUser.Address, objPropUser.City, objPropUser.State, objPropUser.Zip, objPropUser.Lat, objPropUser.Lng, objPropUser.LocID, objPropUser.RolId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UserRegistrationTransfer(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, "spUserReg", objPropUser.UserLic, objPropUser.UserLicID, objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUserSyncStatus(User objPropUser)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropUser.ConnConfig, CommandType.Text, " select isnull(QBFirstSync,1) as QBFirstSync from control"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetSyncItems(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, " select isnull(QBFirstSync,1) as QBFirstSync, isnull(SyncInvoice,0) as SyncInvoice, isnull(SyncTimesheet,0) as SyncTimesheet from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEquipmentTemplate(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[5];

            para[0] = new SqlParameter();
            para[0].ParameterName = "fdesc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Lang;

            para[1] = new SqlParameter();
            para[1].ParameterName = "remarks";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "items";
            para[2].SqlDbType = SqlDbType.Structured;
            para[2].Value = objPropUser.DtItems;

            para[3] = new SqlParameter();
            para[3].ParameterName = "equipt";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropUser.REPtemplateID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "mode";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropUser.Mode;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddEquipTemplate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddCustomTemplate(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[7];

            para[0] = new SqlParameter();
            para[0].ParameterName = "fdesc";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Lang;

            para[1] = new SqlParameter();
            para[1].ParameterName = "remarks";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Remarks;

            para[2] = new SqlParameter();
            para[2].ParameterName = "items";
            para[2].SqlDbType = SqlDbType.Structured;
            para[2].Value = objPropUser.DtItems;

            para[3] = new SqlParameter();
            para[3].ParameterName = "equipt";
            para[3].SqlDbType = SqlDbType.Int;
            para[3].Value = objPropUser.REPtemplateID;

            para[4] = new SqlParameter();
            para[4].ParameterName = "mode";
            para[4].SqlDbType = SqlDbType.Int;
            para[4].Value = objPropUser.Mode;

            para[5] = new SqlParameter();
            para[5].ParameterName = "ItemsDeleted";
            para[5].SqlDbType = SqlDbType.Structured;
            para[5].Value = objPropUser.DtItemsDeleted;

            para[6] = new SqlParameter();
            para[6].ParameterName = "@CustomValues";
            para[6].SqlDbType = SqlDbType.Structured;
            para[6].Value = objPropUser.dtCustomValues;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddCustomTemplate", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesPerson(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select fuser,u.id from tbluser u inner join emp e on u.fuser = e.callsign where e.sales = 1 order by fuser");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTaskUsers(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select upper(fuser) as fuser,id from tbluser  order by fuser");//where status=0
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void UpdateCustomerUser(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[23];

            para[0] = new SqlParameter();
            para[0].ParameterName = "UserName";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.Username;

            para[1] = new SqlParameter();
            para[1].ParameterName = "Password";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Password;

            para[2] = new SqlParameter();
            para[2].ParameterName = "status";
            para[2].SqlDbType = SqlDbType.SmallInt;
            para[2].Value = objPropUser.Status;

            para[3] = new SqlParameter();
            para[3].ParameterName = "FName";
            para[3].SqlDbType = SqlDbType.VarChar;
            para[3].Value = objPropUser.FirstName;

            para[4] = new SqlParameter();
            para[4].ParameterName = "Address";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.Address;

            para[5] = new SqlParameter();
            para[5].ParameterName = "City";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.City;

            para[6] = new SqlParameter();
            para[6].ParameterName = "State";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.State;

            para[7] = new SqlParameter();
            para[7].ParameterName = "Zip";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Zip;

            para[8] = new SqlParameter();
            para[8].ParameterName = "country";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.Country;

            para[9] = new SqlParameter();
            para[9].ParameterName = "remarks";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Remarks;

            para[10] = new SqlParameter();
            para[10].ParameterName = "mapping";
            para[10].SqlDbType = SqlDbType.Int;
            para[10].Value = objPropUser.Mapping;

            para[11] = new SqlParameter();
            para[11].ParameterName = "schedule";
            para[11].SqlDbType = SqlDbType.Int;
            para[11].Value = objPropUser.Schedule;

            para[12] = new SqlParameter();
            para[12].ParameterName = "CustomerId";
            para[12].SqlDbType = SqlDbType.Int;
            para[12].Value = objPropUser.CustomerID;

            para[13] = new SqlParameter();
            para[13].ParameterName = "contact";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.MainContact;

            para[14] = new SqlParameter();
            para[14].ParameterName = "Phone";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.Phone;

            para[15] = new SqlParameter();
            para[15].ParameterName = "Website";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.Website;

            para[16] = new SqlParameter();
            para[16].ParameterName = "Email";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.Email;

            para[17] = new SqlParameter();
            para[17].ParameterName = "cell";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.Cell;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "GroupData";
            para[19].SqlDbType = SqlDbType.Structured;
            para[19].Value = objPropUser.dtGroupdata;

            para[20] = new SqlParameter();
            para[20].ParameterName = "Equipment";
            para[20].SqlDbType = SqlDbType.Int;
            para[20].Value = objPropUser.EquipID;

            para[21] = new SqlParameter();
            para[21].ParameterName = "@grpbywo";
            para[21].SqlDbType = SqlDbType.Int;
            para[21].Value = objPropUser.grpbyWO;

            para[22] = new SqlParameter();
            para[22].ParameterName = "@openticket";
            para[22].SqlDbType = SqlDbType.Int;
            para[22].Value = objPropUser.openticket;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerUser", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTimesheetEmp(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTimesheetEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID);//objPropUser.unsaved,
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getSavedTimesheetEmp(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetSavedTimesheet", objPropUser.Startdt, objPropUser.Enddt, objPropUser.Supervisor, objPropUser.DepartmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSavedTimesheet(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "Select isnull(processed, 0) as processed from tbltimesheet where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetTimesheetTicketsByEmp(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetTimesheetTicketsByEmp", objPropUser.Startdt, objPropUser.Enddt, objPropUser.EmpId, objPropUser.saved, objPropUser.unsaved);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessTimesheet(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, "update tbltimesheet set processed = 1 where startdate = '" + objPropUser.Startdt + "' and enddate= '" + objPropUser.Enddt + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTimesheet(User objPropUser)
        {
            try
            {
                SqlParameter[] paraEmpData = new SqlParameter[5];

                paraEmpData[0] = new SqlParameter();
                paraEmpData[0].ParameterName = "@StartDate";
                paraEmpData[0].SqlDbType = SqlDbType.DateTime;
                paraEmpData[0].Value = objPropUser.Startdt;

                paraEmpData[1] = new SqlParameter();
                paraEmpData[1].ParameterName = "@EndDate";
                paraEmpData[1].SqlDbType = SqlDbType.DateTime;
                paraEmpData[1].Value = objPropUser.Enddt;

                paraEmpData[2] = new SqlParameter();
                paraEmpData[2].ParameterName = "Processed";
                paraEmpData[2].SqlDbType = SqlDbType.Int;
                paraEmpData[2].Value = objPropUser.IsSuper;

                paraEmpData[3] = new SqlParameter();
                paraEmpData[3].ParameterName = "EmpData";
                paraEmpData[3].SqlDbType = SqlDbType.Structured;
                paraEmpData[3].Value = objPropUser.EmpData;

                paraEmpData[4] = new SqlParameter();
                paraEmpData[4].ParameterName = "TicketData";
                paraEmpData[4].SqlDbType = SqlDbType.Structured;
                paraEmpData[4].Value = objPropUser.dtTicketData;

                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddTimesheetEmp", paraEmpData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getScreens(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "SELECT p.id,p.pagename,p.url,isnull(pp.access,0)as access ,ISNULL( pp.edit,0) as edit, ISNULL( pp.[VIEW],0) as [VIEW], ISNULL( pp.[add],0) as [add], isnull (pp.[DELETE],0) as [DELETE] FROM tblPages p left OUTER JOIN tblpagepermissions pp ON p.id=pp.page AND pp.[USER]=" + objPropUser.UserID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getScreensByUser(User objPropUser)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Isnull(pp.access, 0)    AS access, \n");
            varname1.Append("       Isnull(pp.edit, 0)      AS edit, \n");
            varname1.Append("       Isnull(pp.[VIEW], 0)    AS [VIEW], \n");
            varname1.Append("       Isnull(pp.[add], 0)     AS [add], \n");
            varname1.Append("       Isnull (pp.[DELETE], 0) AS [DELETE] \n");
            varname1.Append("FROM   tblpagepermissions pp \n");
            varname1.Append("WHERE  pp.[USER] = (SELECT id \n");
            varname1.Append("                    FROM   tbluser \n");
            varname1.Append("                    WHERE  fuser = '" + objPropUser.Username + "') \n");
            varname1.Append("       AND Page = (SELECT id \n");
            varname1.Append("                   FROM   tblpages \n");
            varname1.Append("                   WHERE  url = '" + objPropUser.PageName + "') ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet AddSageLocation(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[24];

            para[0] = new SqlParameter();
            para[0].ParameterName = "Account";
            para[0].SqlDbType = SqlDbType.VarChar;
            para[0].Value = objPropUser.AccountNo;

            para[1] = new SqlParameter();
            para[1].ParameterName = "LocName";
            para[1].SqlDbType = SqlDbType.VarChar;
            para[1].Value = objPropUser.Locationname;

            para[2] = new SqlParameter();
            para[2].ParameterName = "Address";
            para[2].SqlDbType = SqlDbType.VarChar;
            para[2].Value = objPropUser.Address;

            para[3] = new SqlParameter();
            para[3].ParameterName = "status";
            para[3].SqlDbType = SqlDbType.SmallInt;
            para[3].Value = objPropUser.Status;

            para[4] = new SqlParameter();
            para[4].ParameterName = "City";
            para[4].SqlDbType = SqlDbType.VarChar;
            para[4].Value = objPropUser.City;

            para[5] = new SqlParameter();
            para[5].ParameterName = "State";
            para[5].SqlDbType = SqlDbType.VarChar;
            para[5].Value = objPropUser.State;

            para[6] = new SqlParameter();
            para[6].ParameterName = "Zip";
            para[6].SqlDbType = SqlDbType.VarChar;
            para[6].Value = objPropUser.Zip;

            para[7] = new SqlParameter();
            para[7].ParameterName = "remarks";
            para[7].SqlDbType = SqlDbType.VarChar;
            para[7].Value = objPropUser.Remarks;

            para[8] = new SqlParameter();
            para[8].ParameterName = "contactname";
            para[8].SqlDbType = SqlDbType.VarChar;
            para[8].Value = objPropUser.MainContact;

            para[9] = new SqlParameter();
            para[9].ParameterName = "Phone";
            para[9].SqlDbType = SqlDbType.VarChar;
            para[9].Value = objPropUser.Phone;

            para[10] = new SqlParameter();
            para[10].ParameterName = "Fax";
            para[10].SqlDbType = SqlDbType.VarChar;
            para[10].Value = objPropUser.Fax;

            para[11] = new SqlParameter();
            para[11].ParameterName = "cellular";
            para[11].SqlDbType = SqlDbType.VarChar;
            para[11].Value = objPropUser.Cell;

            para[12] = new SqlParameter();
            para[12].ParameterName = "Email";
            para[12].SqlDbType = SqlDbType.VarChar;
            para[12].Value = objPropUser.Email;

            para[13] = new SqlParameter();
            para[13].ParameterName = "Website";
            para[13].SqlDbType = SqlDbType.VarChar;
            para[13].Value = objPropUser.Website;

            para[14] = new SqlParameter();
            para[14].ParameterName = "RolAddress";
            para[14].SqlDbType = SqlDbType.VarChar;
            para[14].Value = objPropUser.RolAddress;

            para[15] = new SqlParameter();
            para[15].ParameterName = "RolCity";
            para[15].SqlDbType = SqlDbType.VarChar;
            para[15].Value = objPropUser.RolCity;

            para[16] = new SqlParameter();
            para[16].ParameterName = "RolState";
            para[16].SqlDbType = SqlDbType.VarChar;
            para[16].Value = objPropUser.RolState;

            para[17] = new SqlParameter();
            para[17].ParameterName = "RolZip";
            para[17].SqlDbType = SqlDbType.VarChar;
            para[17].Value = objPropUser.RolZip;

            para[18] = new SqlParameter();
            para[18].ParameterName = "Type";
            para[18].SqlDbType = SqlDbType.VarChar;
            para[18].Value = objPropUser.Type;

            para[19] = new SqlParameter();
            para[19].ParameterName = "SageOwner";
            para[19].SqlDbType = SqlDbType.VarChar;
            para[19].Value = objPropUser.SageCustID;

            para[20] = new SqlParameter();
            para[20].ParameterName = "SageKeyID";
            para[20].SqlDbType = SqlDbType.VarChar;
            para[20].Value = objPropUser.SageLocID;

            //para[21] = new SqlParameter();
            //para[21].ParameterName = "returnval";
            //para[21].SqlDbType = SqlDbType.Int;
            //para[21].Direction = ParameterDirection.ReturnValue;

            para[21] = new SqlParameter();
            para[21].ParameterName = "LastUpdateDate";
            para[21].SqlDbType = SqlDbType.DateTime;
            para[21].Value = objPropUser.LastUpdateDate;

            para[22] = new SqlParameter();
            para[22].ParameterName = "SageCustomer";
            para[22].SqlDbType = SqlDbType.VarChar;
            para[22].Value = objPropUser.Custom2;

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddSageLocation", para);

                //SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spAddSageLocation", para);
                //int locid = 0;
                //if (para[21].Value != DBNull.Value)
                //{
                //    locid = Convert.ToInt32(para[21].Value);
                //}
                //return locid;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getGetSageExportTickets(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetSageExportTickets", objPropUser.Startdt, objPropUser.Enddt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdatePeriodClosedDate(User objPropUser)
        {
            try
            {
                string query = "UPDATE tblUser SET fStart=@fStart, fEnd=@fEnd";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@fStart", objPropUser.FStart));
                parameters.Add(new SqlParameter("@fEnd", objPropUser.FEnd));

                int rowsAffected = SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUserAddress(User objPropUser)
        {
            try
            {
                StringBuilder varname = new StringBuilder();
                varname.Append("SELECT \n");
                varname.Append(" u.ID as userid,  \n");
                varname.Append(" r.ID as rolid,  \n");
                varname.Append(" r.City,");
                varname.Append(" r.State,");
                varname.Append(" r.Zip,");
                varname.Append(" r.Phone,");
                varname.Append(" r.Address,");
                varname.Append(" r.name as fFirst,");
                varname.Append(" r.name as Middle,");
                varname.Append(" r.name as Last,");
                varname.Append(" u.Status,");
                varname.Append(" r.Remarks");
                varname.Append(" FROM  Owner u 	");
                varname.Append(" LEFT OUTER JOIN Rol r ON u.Rol=r.ID ");
                varname.Append(" WHERE u.Status=0 AND u.ID=" + objPropUser.UserID);
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBillCodeSearch(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetBillingCodeSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetServiceTypeByType(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select l.type,l.fdesc,l.remarks,(select count(1) from elev where cat=l.type) as Count,l.InvID, isnull(i.Sacct,'') as Sacct, isnull((select c.fdesc from chart c where c.ID=i.sacct),'') as GLAcct from ltype l left join Inv i on l.InvID=i.ID where l.type = '" + objPropUser.Type + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWage(User objPropUser)
        {
            string strQuery = "select id,fdesc,remarks from PRWage"; // where Field = 1

            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getSTax(User objPropUser)
        {
            string strQuery = "select s.*,c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID and UType = 0 order by name";
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetWageByID(Wage _objWage)
        {
            try
            {
                return _objWage.Ds = SqlHelper.ExecuteDataset(_objWage.ConnConfig, "spGetWageByID", _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteWageByID(Wage _objWage)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objWage.ConnConfig, CommandType.Text, "DELETE FROM PRWage WHERE ID=" + _objWage.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllWage(Wage _objWage) // display all active wage details
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objWage.ConnConfig, CommandType.Text, "SELECT *, ID as value, fDesc, fDesc as label FROM PRWage WHERE Status = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDocInfo(User objPropUser)
        {
            SqlParameter[] para = new SqlParameter[1];
            para[0] = new SqlParameter();
            para[0].ParameterName = "@Docs";
            para[0].SqlDbType = SqlDbType.Structured;
            para[0].Value = objPropUser.dtDocs;

            try
            {
                SqlHelper.ExecuteNonQuery(objPropUser.ConnConfig, CommandType.StoredProcedure, "spUpdateDocInfo", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetUserSearch(User _objUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(_objUser.ConnConfig, "spGetUserSearch", _objUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getElevByLoc(User objPropUser)
        {
            string str = "select distinct e.state, e.cat,e.category,e.manuf,e.price,e.last,e.since, e.id,e.unit,e.type,e.fdesc,e.status,r.name,l.id as locid,l.tag ,(l.address+', '+l.city+', '+l.state+', '+l.zip) as address, l.Loc,e.ID as unitid FROM elev e INNER JOIN loc l ON l.Loc = e.Loc INNER JOIN owner o ON o.id = l.owner INNER JOIN rol r ON o.rol = r.id WHERE e.id IS NOT NULL ";
            str += " and e.loc=" + objPropUser.LocID + "";
            str += " order by e.unit";

            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllTc(User objPropUser)
        {
            string str = " SELECT t.*, p.PageName from [t&c] as t INNER JOIN [tblPages] as p ON t.tblPageID = p.ID ORDER BY ID ";
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetSearchPages(User objPropUser)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetPageSearch", objPropUser.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddTerms(User _objUser)
        {
            try
            {
                string query = "INSERT INTO [T&C] (tblPageID, TermsConditions) VALUES (@tblPageID, @TermsCondition) ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tblPageID", _objUser.PageID));
                parameters.Add(new SqlParameter("@TermsCondition", _objUser.TermsConditions));
                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTerms(User _objUser)
        {
            try
            {
                string query = "UPDATE [T&C] SET tblPageID=@tblPageID, TermsConditions=@TermsCondition WHERE ID='" + _objUser.TermsID + "' ";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tblPageID", _objUser.PageID));
                parameters.Add(new SqlParameter("@TermsCondition", _objUser.TermsConditions));
                SqlHelper.ExecuteDataset(_objUser.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPage(User _objUser)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objUser.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM [T&C] WHERE tblPageID = '" + _objUser.PageID + "')THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool IsExistPageForUpdate(User _objUser)
        {
            try
            {
                return Convert.ToBoolean(SqlHelper.ExecuteScalar(_objUser.ConnConfig, CommandType.Text, "SELECT CAST (CASE WHEN EXISTS(SELECT TOP 1 1 FROM [T&C] WHERE tblPageID = '" + _objUser.PageID + "' AND ID != '" + _objUser.TermsID + "')THEN 1 ELSE 0 END AS BIT)"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteTermsCondition(User _objUser)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objUser.ConnConfig, CommandType.Text, " DELETE FROM [T&C] WHERE ID='" + _objUser.TermsID + "'  ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateWarehouse(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spCreateWarehouse", objPropUser.WarehouseID, objPropUser.WarehouseName, objPropUser.Type, objPropUser.LocID, objPropUser.Remarks, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateInventoryWarehouse(User objPropUser)
        {
            try
            {
                SqlHelper.ExecuteScalar(objPropUser.ConnConfig, "spUpdateWarehouse", objPropUser.WarehouseID, objPropUser.WarehouseName, objPropUser.Type, objPropUser.LocID, objPropUser.Remarks, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetInventoryWarehouse(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.StoredProcedure, "spGetInventoryWarehouse");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllUseTax(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select s.*,case s.Utype when 0 then 'Sales tax' when 1 then 'Use tax' end as Utypename, c.fDesc as AcctDesc from stax as s, Chart as c Where s.GL=c.ID AND s.Utype = 1 order by name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetJobBillRatesById(User objPropUser)
        {
            try
            {
                return objPropUser.Ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select isnull(BillRate, 0) as BillRate, isnull(RateOT, 0) as RateOT, isnull(RateNT, 0) as RateNT, isnull(RateDT, 0) as RateDT, isnull(RateMileage, 0) as RateMileage, isnull(RateTravel,0) as RateTravel from Job where ID=" + objPropUser.JobId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}