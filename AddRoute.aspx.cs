using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddRoute : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            FillWorker();
        }
    }

    private void FillWorker()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "fDesc";
        ddlRoute.DataBind();
        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("routes.aspx");
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        
    }
}