<%@ WebHandler Language="C#" Class="CompanyLogo" %>

using System;
using System.Web;
using System.Data;
using BusinessEntity;
using BusinessLayer;
using System.Web.SessionState;

public class CompanyLogo : IHttpHandler, IReadOnlySessionState
{

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    
    public void ProcessRequest (HttpContext context) {
        try
        {
            
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = context.Session["config"].ToString();
            ds = objBL_User.getControl(objPropUser);
                        
            context.Response.ContentType = "image/jpg";
            if (ds.Tables[0].Rows[0]["logo"] != DBNull.Value)
            {
                context.Response.BinaryWrite((byte[])ds.Tables[0].Rows[0]["logo"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}