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
using System.Collections.Generic;
using System.Web.Script.Serialization;

public partial class AddEstimateBucket : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["edit"] = 0;
            CreateTable();
            GetBucket();
        }
    }
    private void GetBucket()
    {
        if (Request.QueryString["uid"] != null)
        {
            ViewState["edit"] = 1;
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.BucketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Customer.getEstimateBucketByID(objProp_Customer);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                DataTable dtitems = ds.Tables[1].Copy();
                gvBucketItems.DataSource = dtitems;
                gvBucketItems.DataBind();
            }
        }
    }
    protected void lnkContactSave_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.Name = txtName.Text.Trim();
            objProp_Customer.dtItems = GetItemsfromGrid();
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_Customer.Mode = 0;
                objBL_Customer.AddEstimateBucket(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Bucket Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                txtName.Text = string.Empty;
                CreateTable();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.Mode = 1;
                objProp_Customer.BucketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objBL_Customer.AddEstimateBucket(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Bucket Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                BindGrid(RestoreGrid());
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter', theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void CreateTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("unit", typeof(double));
        dt.Columns.Add("cost", typeof(double));
        dt.Columns.Add("measure", typeof(int));

        DataRow dr = dt.NewRow();
        dr["Scope"] = DBNull.Value;
        dr["vendor"] = DBNull.Value;
        dr["measure"] = 1;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Scope"] = DBNull.Value;
        dr1["vendor"] = DBNull.Value;
        dr1["measure"] = 1;
        dt.Rows.Add(dr1);

        BindGrid(dt);
    }

    private void BindGrid(DataTable dt)
    {
        gvBucketItems.DataSource = dt;
        gvBucketItems.DataBind();
    }

    private DataTable GetItemsfromGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("BucketID", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("Item", typeof(string));
        dt.Columns.Add("Vendor", typeof(string));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("unit", typeof(double));
        dt.Columns.Add("cost", typeof(double));
        dt.Columns.Add("measure", typeof(int));

        string strItems = hdnItemJSON.Value.Trim();
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() == string.Empty)
                {
                    return dt;
                }
                i++;
                DataRow dr = dt.NewRow();
                dr["BucketID"] = 0;
                dr["Line"] = i;
                dr["Item"] = dict["txtScope"].ToString().Trim();
                dr["Vendor"] = dict["txtVendor"].ToString().Trim();
                dr["code"] = dict["txtCode"].ToString().Trim();
                if (dict["txtUnit"].ToString().Trim() != string.Empty)
                    dr["unit"] = Convert.ToDouble(dict["txtUnit"]);
                if (dict["txtCost"].ToString().Trim() != string.Empty)
                    dr["cost"] = Convert.ToDouble(dict["txtCost"]);
                if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                    dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                dt.Rows.Add(dr);
            }
        }

        return dt;
    }

    private DataTable RestoreGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("unit", typeof(double));
        dt.Columns.Add("cost", typeof(double));
        dt.Columns.Add("measure", typeof(int));

        string strItems = hdnItemJSON.Value.Trim();
        if (strItems != string.Empty)
        {
            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            int i = 0;
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                i++;
                DataRow dr = dt.NewRow();
                dr["Scope"] = dict["txtScope"];
                dr["vendor"] = dict["txtVendor"];
                dr["code"] = dict["txtCode"].ToString().Trim();
                if (dict["txtUnit"].ToString().Trim() != string.Empty)
                    dr["unit"] = Convert.ToDouble(dict["txtUnit"]);
                if (dict["txtCost"].ToString().Trim() != string.Empty)
                    dr["cost"] = Convert.ToDouble(dict["txtCost"]);
                if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                    dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }
}
