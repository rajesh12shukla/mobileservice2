using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DataLayer;
using BusinessEntity;
using System.Data;

namespace BusinessLayer
{
   public class BL_General
    {
       DL_General objDL_General = new DL_General();

       public void RegisterDevice(General objGeneral)
       {
           objDL_General.RegisterDevice(objGeneral);
       }

       public void PingResponse(General objGeneral)
       {
           objDL_General.PingResponse(objGeneral);
       }

       public string GetRegID(General objGeneral)
       {
           return objDL_General.GetRegID(objGeneral);
       }

       public int GetUpdateTicketSP(General objGeneral)
       {
           return objDL_General.GetUpdateTicketSP(objGeneral);
       }

       public int GetFunctionSpecialChars(General objGeneral)
       {
           return objDL_General.GetFunctionSpecialChars(objGeneral);
       }

       public void InsetLatLngRole(General objGeneral)
       {
           objDL_General.InsetLatLngRole(objGeneral);
       }

       public void UpdatePDAField(General objGeneral)
       {
           objDL_General.UpdatePDAField(objGeneral);
       }

       public void UpdateCustom(General objGeneral)
       {
           objDL_General.UpdateCustom(objGeneral);
       }

       public DataSet getCustomFields(General objGeneral)
       {
          return objDL_General.getCustomFields(objGeneral);
       }

       public string getCode(General objGeneral)
       {
           return objDL_General.getCode(objGeneral);
       }

       public DataSet getDiagnosticCategory(General objGeneral)
       {
           return objDL_General.getDiagnosticCategory(objGeneral);
       }

       public DataSet getDiagnostic(General objGeneral)
       {
           return objDL_General.getDiagnostic(objGeneral);
       }

       public DataSet getDiagnosticAll(General objGeneral)
       {
           return objDL_General.getDiagnosticAll(objGeneral);
       }

       public DataSet getCodesAll(General objGeneral)
       {
           return objDL_General.getCodesAll(objGeneral);
       }

       public DataSet getPing(General objGeneral)
       {
           return objDL_General.getPing(objGeneral);
       }

       public void InsertDiagnostic(General objGeneral)
       {
           objDL_General.InsertDiagnostic(objGeneral);
       }

       public void UpdateDiagnostic(General objGeneral)
       {
           objDL_General.UpdateDiagnostic(objGeneral);
       }

       public void InsertQuickCodes(General objGeneral)
       {
           objDL_General.InsertQuickCodes(objGeneral);
       }

       public void UpdateQuickCodes(General objGeneral)
       {
           objDL_General.UpdateQuickCodes(objGeneral);
       }

       public void DeleteQuickCodes(General objGeneral)
       {
           objDL_General.DeleteQuickCodes(objGeneral);
       }

       public void InsertGPSInterval(General objGeneral)
       {
           objDL_General.InsertGPSInterval(objGeneral);
       }

       public string GetGPSInterval(General objGeneral)
       {
         return  objDL_General.GetGPSInterval(objGeneral);
       }

       public string GetDeviceTokenID(General objGeneral)
       {
           return objDL_General.GetDeviceTokenID(objGeneral);
       }

       public void LogError(General objGeneral)
       {
           objDL_General.LogError(objGeneral);
       }

       public void UpdateQBLastSync(General objGeneral)
       {
           objDL_General.UpdateQBLastSync(objGeneral);
       }

       public void UpdateSageLastSync(General objGeneral)
       {
           objDL_General.UpdateSageLastSync(objGeneral);
       }

       public void AddQBErrorLog(General objGeneral)
       {
           objDL_General.AddQBErrorLog(objGeneral);
       }

       public DataSet getQBlatsync(General objGeneral)
       {
           return objDL_General.getQBlatsync(objGeneral);
       }

       public DataSet getSagelatsync(General objGeneral)
       {
           return objDL_General.getSagelatsync(objGeneral);
       }

       public DataSet GetMails(General objGeneral)
       {
           return objDL_General.GetMails(objGeneral);
       }

       public int GetMailsCount(General objGeneral)
       {
           return objDL_General.GetMailsCount(objGeneral);
       }

       public int AddEmails(General objGeneral)
       {
          return objDL_General.AddEmails(objGeneral);
       }

       public DataSet GetEmailAcc(General objGeneral)
       {
           return objDL_General.GetEmailAcc(objGeneral);
       }

       public DataSet GetEmailAccounts(General objGeneral)
       {
           return objDL_General.GetEmailAccounts(objGeneral);
       }

       public int GetMAXEmailUID(General objGeneral)
       {
           return objDL_General.GetMAXEmailUID(objGeneral);
       }

       public DataSet GetMsgUID(General objGeneral)
       {
           return objDL_General.GetMsgUID(objGeneral);
       }

       public DataSet getCRMEmails(General objGeneral)
       {
           return objDL_General.getCRMEmails(objGeneral);
       }

       public DataSet ExecQuery(General objGeneral)
       {
           return objDL_General.ExecQuery(objGeneral);
       }
       public DataSet getCustomFieldsControl(General objPropGeneral)
       {
           return objDL_General.getCustomFieldsControl(objPropGeneral);
       }
    }
}
