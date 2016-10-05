<%@ WebHandler Language="C#" Class="CKimageupload" %>

using System;
using System.Web;

public class CKimageupload : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        HttpPostedFile uploads = context.Request.Files["upload"];
        string CKEditorFuncNum = context.Request["CKEditorFuncNum"];        
        string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
        string savepath = savepathconfig + @"\Email_Embedded\";
        var ImgAID = System.Guid.NewGuid();
        string filename = ImgAID.ToString() + ".jpg";
        string fullpath = savepath + filename;
        uploads.SaveAs(fullpath);

        string imageurladdress = System.Web.Configuration.WebConfigurationManager.AppSettings["imageurladdress"].Trim();
        string url = "http://" + imageurladdress + "/MobileServiceDocs/Email_Embedded/" + filename;
        
        context.Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}