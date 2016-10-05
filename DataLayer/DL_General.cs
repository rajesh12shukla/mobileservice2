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
    public class DL_General
    {
        public void RegisterDevice(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "spDeviceRegistration", objPropGeneral.DeviceID, objPropGeneral.RegID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PingResponse(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, "Sppingdevice", objPropGeneral.DeviceID, objPropGeneral.RegID, objPropGeneral.IsRunning, objPropGeneral.IsGPSEnabled);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetRegID(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.RegID = Convert.ToString(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "select m.reg from [MSM2_Admin].dbo.tbldeviceregistration m inner join emp e on m.deviceId=e.deviceid where m.callsign=" + objPropGeneral.EmpID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetUpdateTicketSP(General objPropGeneral)
        {   //spUpdateTicket
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "SELECT 1 FROM sys.procedures where name='" + objPropGeneral.FunctionName + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetFunctionSpecialChars(General objPropGeneral)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "SELECT 1 AS function_name FROM sys.objects WHERE type_desc LIKE '%FUNCTION%' and name='" + objPropGeneral.FunctionName + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsetLatLngRole(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT column_name 'Column_Name' FROM information_schema.columns WHERE table_name = 'rol' AND column_name = 'lng') BEGIN ALTER TABLE rol ADD lat VARCHAR(50) NULL, lng VARCHAR(50) NULL END");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePDAField(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, "ALTER TABLE emp ALTER COLUMN PDASerialNumber varchar(100) null");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustom(General objPropGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropGeneral.ConnConfig, CommandType.Text, "update custom set label ='" + objPropGeneral.CustomLabel + "' where name ='" + objPropGeneral.CustomName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomFields(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select * from custom where name = '" + objPropGeneral.CustomName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getCode(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.CodeDesc = Convert.ToString(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, "select text from codes where code = '" + objPropGeneral.Code + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCodesAll(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select code,text from codes");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getPing(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(Config.MS, CommandType.Text, "select isnull(isrunning,0) as isrunning,  isnull( IsGPSEnabled,0)  as IsGPSEnabled  from tblpingdevice where deviceID='" + objPropGeneral.DeviceID + "' and randomid='" + objPropGeneral.RegID + "' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getDiagnosticCategory(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select distinct Category from Diagnostic ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDiagnostic(General objPropGeneral)
        {
            string query = "select fdesc from Diagnostic where Type = " + objPropGeneral.CodeType + "";

            if (objPropGeneral.CodeCategory != "ALL")
            {
                query += "and Category='" + objPropGeneral.CodeCategory + "'";
            }

            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getDiagnosticAll(General objPropGeneral)
        {
            string query = "  SELECT *, CASE type WHEN  1 THEN 'Resolution' WHEN 0 THEN 'Reason' END AS typecode FROM Diagnostic";

            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertDiagnostic(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "insert into Diagnostic(category,type,fdesc) values('" + objGeneral.Category + "','" + objGeneral.DiagnosticType + "','" + objGeneral.Remarks + "')");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddDiagnostic", objGeneral.Remarks, objGeneral.Category, objGeneral.DiagnosticType, 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateDiagnostic(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Diagnostic set category='" + objGeneral.Category + "' ,type='" + objGeneral.DiagnosticType + "' where fdesc='" + objGeneral.Remarks + "'");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddDiagnostic", objGeneral.Remarks, objGeneral.Category, objGeneral.DiagnosticType, 1);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertQuickCodes(General objGeneral)
        {
            try
            {
                //SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "insert into codes(code,text) values('" + objGeneral.Code + "','" + objGeneral.CodeDesc + "')");
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "IF NOT EXISTS(SELECT 1 FROM codes WHERE  code = '" + objGeneral.Code + "')  BEGIN insert into codes(code,text) values('" + objGeneral.Code + "','" + objGeneral.CodeDesc + "') End ELSE BEGIN RAISERROR ('Code already exists, please use different code !',16,1) RETURN END   ");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQuickCodes(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update codes set Text='" + objGeneral.CodeDesc + "'  where code='" + objGeneral.Code + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteQuickCodes(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "delete from codes  where code='" + objGeneral.Code + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertGPSInterval(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "update tblauth set GPSInterval=" + objGeneral.GPSInterval);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetGPSInterval(General objGeneral)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "select gpsinterval from tblauth"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetDeviceTokenID(General objGeneral)
        {
            try
            {
                return Convert.ToString(SqlHelper.ExecuteScalar(Config.MS, CommandType.Text, "select tokenid from PushNotifications where deviceID='" + objGeneral.DeviceID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LogError(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(Config.MS, CommandType.Text, "insert into tblServiceErrorLog (ServiceName,Error)values('" + objGeneral.ServiceName + "','" + objGeneral.Error + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateQBLastSync(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Control set QBLastSync=GETDATE()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSageLastSync(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, CommandType.Text, "update Control set SageLastSync=GETDATE()");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddQBErrorLog(General objGeneral)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objGeneral.ConnConfig, "spAddQBErrorLog", objGeneral.QBapi, objGeneral.QBRequestID, objGeneral.QBStatusCode, objGeneral.QBStatusSeverity, objGeneral.QBStatusMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getQBlatsync(General objPropGeneral)
        {
            string query = "select isnull(QBLastSync,'')QBLastSync ,isnull(qbintegration,0)qbintegration  from Control";

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSagelatsync(General objPropGeneral)
        {
            string query = "select isnull(SageLastSync,'')SageLastSync ,isnull(sageintegration,0)sageintegration  from Control";

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetMails(General objPropGeneral)
        {
            string strQuery = "select * from tblemail where ID is not null";
            if (objPropGeneral.type != -1 && objPropGeneral.type != -2)
            {
                strQuery += " and isnull(type,0)= " + objPropGeneral.type;
            }
            if (objPropGeneral.rol != 0)
            {
                ////strQuery += " and isnull(rol,0)= " + objPropGeneral.rol;
                strQuery += " and ID in (select Email From tblEmailRol where Rol = " + objPropGeneral.rol + ")";
            }
            if (!string.IsNullOrEmpty(objPropGeneral.RegID))
            {
                strQuery += " and CHARINDEX('" + objPropGeneral.RegID + "', [Subject] ) > 0";
            }
            ////if (objPropGeneral.userid != 0)
            ////{
            ////    strQuery += " and isnull([user],0)= " + objPropGeneral.userid;
            ////}
            

            if (!string.IsNullOrEmpty(objPropGeneral.OrderBy))
            {                
                strQuery += " order by " + objPropGeneral.OrderBy;
            }
            else
            {
                strQuery += " order by recdate desc";
            }

            try
            {
                return SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMailsCount(General objPropGeneral)
        {
            string strQuery = "select count(1) from tblemail where ID is not null";
            if (objPropGeneral.type != -1 && objPropGeneral.type != -2)
            {
                strQuery += " and isnull(type,0)= " + objPropGeneral.type;
            }
            if (objPropGeneral.rol != 0)
            {
                strQuery += " and ID in (select Email From tblEmailRol where Rol = " + objPropGeneral.rol + ")";
            }
            if (!string.IsNullOrEmpty(objPropGeneral.RegID))
            {
                strQuery += " and CHARINDEX('" + objPropGeneral.RegID + "', [Subject] ) > 0";
            }
            ////if (objPropGeneral.userid != 0)
            ////{
            ////    strQuery += " and isnull([user],0)= " + objPropGeneral.userid;
            ////}

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropGeneral.ConnConfig, CommandType.Text, strQuery));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEmails(General objGeneral)
        {
            try
            {
                return Convert.ToInt16(SqlHelper.ExecuteScalar(objGeneral.ConnConfig, "spADDEmail", objGeneral.From, objGeneral.to, objGeneral.cc, objGeneral.bcc, objGeneral.subject, objGeneral.sentdate, objGeneral.Attachments, objGeneral.msgid, objGeneral.uid, objGeneral.GUID, objGeneral.type, objGeneral.userid, objGeneral.AccountID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailAcc(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select * from tblEmailAccounts where UserId=" + objGeneral.userid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEmailAccounts(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select ea.* from tblEmailAccounts ea inner join tbluser u on u.id=ea.userid where u.status=0 and u.emailaccount=1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetMAXEmailUID(General objGeneral)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objGeneral.ConnConfig, CommandType.Text, "select isnull(MAX(uid),0) from tblEmail where AccountID='" + objGeneral.AccountID + "'"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetMsgUID(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, "select UID from tblemail where accountid = '" + objGeneral.AccountID + "' and type = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCRMEmails(General objGeneral)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   Rol \n");
            varname1.Append("WHERE  Type IN ( 0, 3, 4 ) \n");
            varname1.Append("       AND Isnull(Rtrim(Ltrim(EMail)), '') <> '' \n");
            varname1.Append("       AND email LIKE '%_@__%.__%' \n");
            varname1.Append("UNION \n");
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   Phone \n");
            varname1.Append("WHERE  Rol IN (SELECT DISTINCT Rol \n");
            varname1.Append("               FROM   Rol \n");
            varname1.Append("               WHERE  Type IN ( 0, 3, 4 )) \n");
            varname1.Append("       AND Isnull(Rtrim(Ltrim(EMail)), '') <> '' \n");
            varname1.Append("       AND email LIKE '%_@__%.__%' \n");
            varname1.Append("UNION \n");
            varname1.Append("SELECT DISTINCT Rtrim(Ltrim(EMail)) AS email \n");
            varname1.Append("FROM   tblEmailAddresses \n");
            varname1.Append("WHERE  email LIKE '%_@__%.__%' ");


            try
            {
                return objGeneral.Ds = SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, varname1.ToString()); //"select distinct EMail from Rol where Type in (0,3,4)  and ISNULL(EMail,'')<> ''"
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet ExecQuery(General objGeneral)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objGeneral.ConnConfig, CommandType.Text, objGeneral.TextQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getCustomFieldsControl(General objPropGeneral)
        {
            try
            {
                return objPropGeneral.Ds = SqlHelper.ExecuteDataset(objPropGeneral.ConnConfig, CommandType.Text, "select * from custom where Name='GSTRate' or Name ='GSTGL' or Name='Country'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
