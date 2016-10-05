using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessEntity;
using BusinessLayer;

public partial class ConvertProspect : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillCustomerType();
            FillLocationType();
            FillSalesTax();
            Fillterritory();
            FillRoute();

            if (Request.QueryString["uid"] != null)
            {
                txtProspect.Text = Request.QueryString["pname"].ToString();
                txtCustName.Text = Request.QueryString["pname"].ToString();
                hdnProspectID.Value = Request.QueryString["uid"].ToString();
            }
        }
    }

    private void FillCustomerType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCustomerType(objPropUser);
        ddlCustomerType.DataSource = ds.Tables[0];
        ddlCustomerType.DataTextField = "Type";
        ddlCustomerType.DataValueField = "Type";
        ddlCustomerType.DataBind();

        ddlCustomerType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void FillLocationType()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getlocationType(objPropUser);
        ddlLocType.DataSource = ds.Tables[0];
        ddlLocType.DataTextField = "Type";
        ddlLocType.DataValueField = "Type";
        ddlLocType.DataBind();

        ddlLocType.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void FillSalesTax()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getSalesTax(objPropUser);

        ddlSTax.DataSource = ds.Tables[0];
        ddlSTax.DataTextField = "Name";
        ddlSTax.DataValueField = "Name";
        ddlSTax.DataBind();

        ddlSTax.Items.Insert(0, new ListItem(":: Select ::", ""));

    }

    private void Fillterritory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getTerritory(objPropUser);
        ddlTerr.DataSource = ds.Tables[0];
        ddlTerr.DataTextField = "Name";
        ddlTerr.DataValueField = "ID";
        ddlTerr.DataBind();

        ddlTerr.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "Name";
        ddlRoute.DataValueField = "ID";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));

    }

    protected void lnkConvert_Click(object sender, EventArgs e)
    {

    }
}
