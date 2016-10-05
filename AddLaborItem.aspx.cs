using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using BusinessLayer;
using BusinessEntity;

public partial class AddLaborItem : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetEstimateLabor();
        }
    }
    private void GetEstimateLabor()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

        if (dsLabor.Tables[0].Rows.Count == 0)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("item");
            dt.Columns.Add("amount");
            DataRow dr = dt.NewRow();
            dt.Rows.Add(dr);
            gvLaborItems.DataSource = dt;
            gvLaborItems.DataBind();
            gvLaborItems.Rows[0].Visible = false;
        }
        else
        {
            gvLaborItems.DataSource = dsLabor.Tables[0];
            gvLaborItems.DataBind();
            gvLaborItems.Rows[0].Visible = true;
        }
    }

    private int AddItem(int mode, string name, string amount, int ID)
    {
        int success = 0;
        try
        {
            if (name.Trim() != string.Empty)
            {
                if (amount == string.Empty)
                    amount = "0";

                objProp_Customer.ConnConfig = Session["config"].ToString();
                objProp_Customer.Name = name.Trim();
                objProp_Customer.Amount = Convert.ToDouble(amount);
                objProp_Customer.BucketID = ID;
                objProp_Customer.Mode = mode;

                objBL_Customer.AddEstimateLabor(objProp_Customer);

                GetEstimateLabor();
                success = 1;
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter', theme : 'noty_theme_default',  closable : false});", true);
        }
        return success;
    }
    protected void imgAdd_Click(object sender, EventArgs e)
    {
        LinkButton btna = sender as LinkButton;
        GridViewRow row = (GridViewRow)btna.NamingContainer;
        TextBox txtNAme = (TextBox)row.FindControl("txtName");
        TextBox txtAmount = (TextBox)row.FindControl("txtAmount");

        int success = AddItem(0, txtNAme.Text.Trim(), txtAmount.Text.Trim(), 0);

        if (success == 1)
        {
            txtNAme.Text = string.Empty;
            txtAmount.Text = string.Empty;
        }
    }
    protected void imgSave_Click(object sender, EventArgs e)
    {
        LinkButton btna = sender as LinkButton;
        GridViewRow row = (GridViewRow)btna.NamingContainer;
        TextBox txtNAme = (TextBox)row.FindControl("txtName");
        TextBox txtAmount = (TextBox)row.FindControl("txtAmount");
        Label lblID = (Label)row.FindControl("lblID");
        AddItem(1, txtNAme.Text.Trim(), txtAmount.Text.Trim(), Convert.ToInt32(lblID.Text.Trim()));
    }
    protected void imgDel_Click(object sender, EventArgs e)
    {
        LinkButton btna = sender as LinkButton;
        GridViewRow row = (GridViewRow)btna.NamingContainer;
        Label lblID = (Label)row.FindControl("lblID");
        AddItem(2, "0", "0", Convert.ToInt32(lblID.Text.Trim()));
    }
}
