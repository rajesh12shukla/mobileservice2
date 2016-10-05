/*#cc01:11-09-2016*/

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
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Globalization;

public partial class AddProjectTemp : System.Web.UI.Page
{
    #region Variables
    protected DataTable dtBomType = new DataTable();
    protected DataTable dtFormat = new DataTable();
    protected DataTable dtTab = new DataTable();
    protected DataTable dtBomItem = new DataTable();
    protected DataTable dtTabCalculation = new DataTable();/*#cc01:Added by rajesh*/
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();

    JobT _objJob = new JobT();
    BL_Job objBL_Job = new BL_Job();

    Wage _objWage = new Wage();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneral = new GeneralFunctions();

    #endregion

    #region Events

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            FillBomType();
            FillFormat();
            FillTab();
            if (!IsPostBack)
            {
                FillJobType();
                FillContractType();
                FillPosting();
                CreateBOMTable();
                CreateMilestoneTable();
                CreateCustomTable();
                BindCustomGrid();
                
                CustTempGridData2();/*#cc01:Added by rajesh*/
                //if (Session["PageEstimateTemplate"].ToString() == "1")
                //{
                //lblHeader.Text = "Add Template";
                //}

                if (Request.QueryString["uid"] != null)
                {
                    //if (Session["PageEstimateTemplate"].ToString() == "1")
                    //{
                        lblHeader.Text = "Edit Template";
                    //}
                    //else
                    //{
                    //    lblHeader.Text = "Edit Project Template";
                    //}
                    GetData();
                    pnlNext.Visible = true;
                }
                else
                {
                    tempRev.Visible = false;
                    tempRemarks.Visible = false;
                }

                Permission();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ValidateJobItem())
            //{
            _objJob.ConnConfig = Session["config"].ToString();

            #region Header

            if (Request.QueryString["uid"] != null)
            {
                _objJob.ID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                _objJob.TemplateRev = txtTempRev.Text;
                _objJob.RevRemarks = txtTempRemarks.Text;
            }

            _objJob.Remarks = txtREPremarks.Text.Trim();
            _objJob.fDesc = txtREPdesc.Text;
            if (!ddlJobType.SelectedValue.Equals("Select Department"))
            {
                _objJob.Type = Convert.ToInt16(ddlJobType.SelectedValue);
            }
            _objJob.Status = Convert.ToInt16(ddlStatus.SelectedValue);

            if (!ddlAlertType.SelectedValue.Equals("Select Type"))
            {
                _objJob.AlertType = Convert.ToInt16(ddlAlertType.SelectedValue);
            }
            if (chkAlert.Checked.Equals(true))
            {
                _objJob.AlertMgr = true;
            }
            else
            {
                _objJob.AlertMgr = false;
            }
            _objJob.MilestoneMgr = true;
            //if (chkMilestone.Checked.Equals(true))
            //{
            //    _objJob.MilestoneMgr = true;
            //}
            //else
            //    _objJob.MilestoneMgr = false;

            #endregion

            #region Finance
            if (!string.IsNullOrEmpty(uc_InvExpGL._hdnAcctID.Value))
            {
                _objJob.InvExp = Convert.ToInt32(uc_InvExpGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(uc_InterestGL._hdnAcctID.Value))
            {
                _objJob.GLInt = Convert.ToInt32(uc_InterestGL._hdnAcctID.Value);
            }
            //if (!string.IsNullOrEmpty(hdnInvExpGLID.Value))
            //    _objJob.InvExp = Convert.ToInt32(hdnInvExpGLID.Value);
            //if (!string.IsNullOrEmpty(hdnInterestGLID.Value))
            //    _objJob.GLInt = Convert.ToInt32(hdnInterestGLID.Value);
            if (!string.IsNullOrEmpty(hdnInvServiceID.Value))
                _objJob.InvServ = Convert.ToInt32(hdnInvServiceID.Value);
            if (!string.IsNullOrEmpty(hdnPrevilWageID.Value))
                _objJob.Wage = Convert.ToInt32(hdnPrevilWageID.Value);

            if (!ddlContractType.SelectedValue.Equals("0"))
            {
                _objJob.CType = ddlContractType.SelectedValue;
            }
            if (!ddlPostingMethod.SelectedValue.Equals("No data found"))
            {
                _objJob.Post = Convert.ToInt16(ddlPostingMethod.SelectedValue);
            }


            if (chkChargeInt.Checked.Equals(true))
            {
                _objJob.fInt = 1;
            }
            else
                _objJob.fInt = 0;

            if (chkInvoicing.Checked.Equals(true))
            {
                _objJob.JobClose = 1;
            }
            else
                _objJob.JobClose = 0;

            if (chkChargeable.Checked.Equals(true))
            {
                _objJob.Charge = 1;
            }
            else
                _objJob.Charge = 0;
            #endregion

            #region BOM
            DataTable dtB = GetBOMItems();

            int bline = 1;

            if (ViewState["bLine"] == null)
            {
                dtB.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtB.AcceptChanges();
            }
            else
            {
                bline = (Int16)ViewState["bLine"];
                bline++;
                dtB.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtB.AcceptChanges();
            }

            #endregion

            #region Milestones

            DataTable dtM = GetMilestoneItems();
            dtM.Columns.Remove("Department");

            int mline = 1;

            if (ViewState["mLine"] == null)
            {
                dtM.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();
            }
            else
            {
                mline = (Int16)ViewState["mLine"];
                mline++;
                dtM.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtM.AcceptChanges();
            }

            #endregion

            #region Custom

            CustTempGridData();
            DataTable dtCustom = (DataTable)ViewState["CustomTable"];

            DataTable dtCustomVal = (DataTable)ViewState["CustomValues"];
            DataTable dtEstimateData = (DataTable)ViewState["EstimateTable"];/*#cc01:Added by rajesh*/
            dtCustom.Columns.Add("UpdatedDate", typeof(DateTime));
            dtCustom.Columns.Add("Username", typeof(string));

            _objJob.CustomTabItem = dtCustom;
            _objJob.CustomItem = dtCustomVal;
            /*#cc01:Added by rajesh start*/
            for (int i = dtEstimateData.Rows.Count - 1; i >= 0; i--)
            {
                if (dtEstimateData.Rows[i][1] == DBNull.Value)
                    dtEstimateData.Rows[i].Delete();
            }
            dtEstimateData.AcceptChanges();

            _objJob.EstimateData = dtEstimateData;
            /*#cc01:Added by rajesh end*/
            #endregion

            if (dtM != null)
            {
                if (dtM.Rows.Count > 0)
                {
                    _objJob.NRev = Convert.ToInt16(dtM.Select("jType = 0").Count());
                    _objJob.NDed = Convert.ToInt16(dtM.Select("jType = 1").Count());
                }
            }
            //GetChecklist();
            int jobid = 0;
            if (Request.QueryString["uid"] != null)
            {
                DataTable dtDeleted = dtCustom.Clone();

                if (ViewState["CustomDeletedRows"] != null)
                    dtDeleted = (DataTable)ViewState["CustomDeletedRows"];
                _objJob.CustomItemDelete = dtDeleted;

                _objJob.ProjectDt = dtB;
                _objJob.MilestoneDt = dtM;

                jobid = objBL_Customer.UpdateProjectTemplate(_objJob);
            }
            else
            {
                _objJob.ProjectDt = dtB;
                _objJob.MilestoneDt = dtM;
                jobid = objBL_Customer.AddProjectTemplate(_objJob);
            }

            if (Request.QueryString["uid"] != null)
            {
                //GetData();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Template Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                //objGeneral.ResetFormControlValues(this);
                //gvTemplateItems.DataBind();
                //CreateCustomTable();
                //BindCustomGrid();
                //CreateTable();

                //Response.Redirect(Request.RawUrl);

                Response.Redirect(Page.Request.RawUrl, false);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Template Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            Response.Redirect(Page.Request.RawUrl, false);
            //}
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);
            int c = dt.Rows.Count - 1;
            if (index < c)
            {
                Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[index + 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            DataColumn[] keyColumns = new DataColumn[1];
            keyColumns[0] = dt.Columns["ID"];
            dt.PrimaryKey = keyColumns;

            DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
            int index = dt.Rows.IndexOf(d);

            if (index > 0)
            {
                Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[index - 1]["ID"]);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)Session["ProjectTemp"];
            Response.Redirect("addprojecttemp.aspx?uid=" + dt.Rows[0]["ID"]);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("projecttemplate.aspx");
    }

    #region Header
    //protected void chkMilestone_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (chkMilestone.Checked.Equals(true))
    //    {
    //        tbpnlMilestone.Enabled = true;
    //    }
    //    else
    //    {
    //        tbpnlMilestone.Enabled = false;
    //    }
    //}
    //protected void txtDepartmentQty_TextChanged(object sender, EventArgs e)
    //{
    //    if (!string.IsNullOrEmpty(txtDepartmentQty.Text))
    //    {
    //        _depQty = Convert.ToInt32(txtDepartmentQty.Text);
    //        OnInit(e);
    //        UpdatePanel3.Update();
    //    }
    //}
    #endregion

    #region BOM
    protected void gvBOM_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //BOM Item list
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                DropDownList ddlBType = (e.Row.FindControl("ddlBType") as DropDownList);
                DropDownList ddlItem = (e.Row.FindControl("ddlItem") as DropDownList);
                Label lblBudgetExt = (e.Row.FindControl("lblBudgetExt") as Label);
                TextBox txtBudgetUnit = (e.Row.FindControl("txtBudgetUnit") as TextBox);
                TextBox txtQtyReq = (e.Row.FindControl("txtQtyReq") as TextBox);

                double _budgetExt = 0.0;
                double _qtyReq = 0.0;
                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    FillInv(ddlItem);

                    if (!string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    }
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    FillWage(ddlItem);
                }
                else
                {
                    FillItems(ddlItem);
                }
                if (!string.IsNullOrEmpty(txtBudgetUnit.Text))
                {
                    if (!string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        if (_qtyReq.Equals(0))
                            _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                    }
                    _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                }

                if (ViewState["TempBOM"] != null)
                {
                    DataTable _dtBom = (DataTable)ViewState["TempBOM"];
                    if (_dtBom.Rows.Count > 0)
                    {
                        foreach (var item in _dtBom.Rows)
                        {
                            var itemVal = _dtBom.Rows[0]["BItem"].ToString();
                            if (!string.IsNullOrEmpty(itemVal))
                            {
                                if (e.Row.RowType == DataControlRowType.DataRow)
                                {
                                    itemVal = DataBinder.Eval(e.Row.DataItem, "BItem").ToString();
                                    ddlItem.SelectedValue = itemVal.ToString();
                                }
                            }
                        }
                    }
                }

                break;
        }
    }
    protected void gvBOM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddBOMItem"))
        {
            int rowIndex = gvBOM.Rows.Count - 1;
            GridViewRow row = gvBOM.Rows[rowIndex];
            HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;
            HiddenField hdnLine = row.FindControl("hdnLine") as HiddenField;

            DataTable dt = GetBOMGridItems();

            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Line"] = 0;
                    dt.Rows.Add(dr);
                }
            }

            DataRow dr2 = dt.NewRow();
            dr2["Line"] = 0;
            dt.Rows.Add(dr2);

            ViewState["TempBOM"] = dt;
            gvBOM.DataSource = dt;
            gvBOM.DataBind();
        }
    }
    protected void ddlBType_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlBomType = (DropDownList)sender;
        GridViewRow gridrow = (GridViewRow)ddlBomType.NamingContainer;
        int rowIndex = gridrow.RowIndex;

        foreach (GridViewRow gr in gvBOM.Rows)
        {
            if (gr.RowIndex == rowIndex)
            {
                DropDownList ddlBType = (DropDownList)gr.FindControl("ddlBType");
                DropDownList ddlItem = (DropDownList)gr.FindControl("ddlItem");
                TextBox txtBudgetUnit = (TextBox)gr.FindControl("txtBudgetUnit");
                TextBox txtQtyReq = (TextBox)gr.FindControl("txtQtyReq");
                Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
                HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    FillInv(ddlItem);
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    FillWage(ddlItem);
                }
                else
                {
                    FillItems(ddlItem);
                }
                double _budgetExt = 0;
                if (ddlBType.SelectedValue.Equals("1")) // if Materials
                {
                    double _qtyReq = 0.0;
                    //double _budgetUnit = 0.0;
                    //double _scrapFact = 0.0;


                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
                    {

                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                        _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                        //_budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);

                    }
                    lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                    hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtBudgetUnit.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
                    {
                        _budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);
                    }
                    lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                    hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                }

            }
        }

        foreach (GridViewRow gr in gvBOM.Rows)
        {
            Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
            HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

            lblBudgetExt.Text = hdnBudgetExt.Value;
        }
    }
    #endregion

    #region Milestone
    protected void gvMilestones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestoneItem"))
        {
            int rowIndex = gvMilestones.Rows.Count - 1;
            GridViewRow row = gvMilestones.Rows[rowIndex];
            HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

            DataTable dt = GetMilestoneGridItems();
            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dr["Line"] = 0;
                    dt.Rows.Add(dr);
                }
            }

            DataRow dr2 = dt.NewRow();
            dr2["Line"] = 0;
            dt.Rows.Add(dr2);

            ViewState["TempMilestone"] = dt;
            gvMilestones.DataSource = dt;
            gvMilestones.DataBind();

        }
    }
    #endregion

    #region Custom

    protected void gvCustom_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCustom.Rows[index];

            if (e.CommandName.Equals("AddCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
                LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");

                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    txtCustomValue.Text = string.Empty;
                }
            }
            else if (e.CommandName.Equals("UpdateCustomValue"))
            {
                TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                if (txtCustomValue.Text.Trim() != string.Empty)
                {
                    ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                    ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                    ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
                }
            }
            else if (e.CommandName.Equals("DeleteCustomValue"))
            {
                LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
                TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
                DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
                LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
                LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");

                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.SelectedIndex = 0;
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvCustom_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlFormat = (DropDownList)e.Row.FindControl("ddlFormat");
            DropDownList ddlTab = (DropDownList)e.Row.FindControl("ddlTab");

            Panel pnlCustomValue = (Panel)e.Row.FindControl("pnlCustomValue");
            if (ddlFormat.SelectedItem.Text == "Dropdown")
                pnlCustomValue.Visible = true;
            else
                pnlCustomValue.Visible = false;

            DropDownList ddlCustomValue = (DropDownList)e.Row.FindControl("ddlCustomValue");
            Label lblID = (Label)e.Row.FindControl("lblID");

            if (ViewState["CustomValues"] != null)
            {
                DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
                DataTable dt = dtCustomval.Clone();
                //tblCustomTabID = " + Convert.ToInt32(lblID.Text) + " AND
                DataRow[] result = dtCustomval.Select("Line = " + (e.Row.RowIndex + 1) + "");
                foreach (DataRow row in result)
                {
                    dt.ImportRow(row);
                }

                if (dt.Rows.Count > 0)
                {
                    dt.DefaultView.Sort = "Value  ASC";
                    dt = dt.DefaultView.ToTable();
                }

                ddlCustomValue.DataSource = dt;
                ddlCustomValue.DataTextField = "Value";
                ddlCustomValue.DataValueField = "Value";
                ddlCustomValue.DataBind();
                ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
            }

            if (ddlJobType.SelectedValue.Equals("0"))
            {
                ddlTab.Enabled = false;
                ddlTab.SelectedValue = "0";
            }
        }
    }
    protected void lnkDelCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkDelete = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkDelete.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");

            ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
            ddlCustomValue.SelectedIndex = 0;
            lnkAddCustomValue.Visible = true;
            lnkUpdateCustomValue.Visible = false;
            lnkDelete.Visible = false;
            txtCustomValue.Text = string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkUpdateCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkUpdate = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkUpdate.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            if (txtCustomValue.Text.Trim() != string.Empty)
            {
                ddlCustomValue.Items.Remove(new ListItem(ddlCustomValue.SelectedValue, ddlCustomValue.SelectedValue));
                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                ddlCustomValue.SelectedValue = txtCustomValue.Text.Trim();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlFormat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            Panel pnlCustomValue = (Panel)row.FindControl("pnlCustomValue");
            if (row != null)
            {
                if (ddl.SelectedItem.Text == "Dropdown")
                    pnlCustomValue.Visible = true;
                else
                    pnlCustomValue.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkAddCustomValue_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lnkAdd = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lnkAdd.NamingContainer;
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            DropDownList ddlCustomValue = (DropDownList)row.FindControl("ddlCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
            if (txtCustomValue.Text.Trim() != string.Empty)
            {
                ddlCustomValue.Items.Add(new ListItem(txtCustomValue.Text.Trim(), txtCustomValue.Text.Trim()));
                txtCustomValue.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lnkAddnewRow_Click(object sender, ImageClickEventArgs e)
    {
        CustTempGridData();
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["CustomTable"];
        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["tblTabID"] = 0;
        dr["Label"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Value"] = DBNull.Value;
        dr["Format"] = 0;

        dt.Rows.Add(dr);

        ViewState["CustomTable"] = dt;
        BindCustomGrid();
        //BindCustomDropDown();
    }
    protected void ibtnDeleteCItem_Click(object sender, ImageClickEventArgs e)
    {
        DeleteCustItem();
    }
    protected void ddlCustomValue_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DropDownList ddl = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddl.NamingContainer;
            LinkButton lnkAddCustomValue = (LinkButton)row.FindControl("lnkAddCustomValue");
            LinkButton lnkUpdateCustomValue = (LinkButton)row.FindControl("lnkUpdateCustomValue");
            LinkButton lnkDelCustomValue = (LinkButton)row.FindControl("lnkDelCustomValue");
            TextBox txtCustomValue = (TextBox)row.FindControl("txtCustomValue");
            if (ddl.SelectedIndex == 0)
            {
                lnkAddCustomValue.Visible = true;
                lnkUpdateCustomValue.Visible = false;
                lnkDelCustomValue.Visible = false;
                txtCustomValue.Text = string.Empty;
            }
            else
            {
                lnkAddCustomValue.Visible = false;
                lnkUpdateCustomValue.Visible = true;
                lnkDelCustomValue.Visible = true;
                txtCustomValue.Text = ddl.SelectedValue;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    protected void lbtnCodeSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlItem = (DropDownList)sender;
        GridViewRow row = (GridViewRow)ddlItem.NamingContainer;
        TextBox txtScope = (TextBox)row.FindControl("txtScope");
        DropDownList ddlBType = (DropDownList)row.FindControl("ddlBType");

        DataSet ds = new DataSet();
        if (ddlItem.SelectedValue != "0")
        {
            if (ddlBType.SelectedValue == "1" || ddlBType.SelectedValue == "2")       // select bom type Material
            {
                if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "1")
                {
                    _objJob.ConnConfig = Session["config"].ToString();
                    ds = objBL_Job.GetInventoryItem(_objJob);
                    dtBomItem = ds.Tables[0];
                }
                else if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "2")
                {
                    _objWage.ConnConfig = Session["config"].ToString();
                    ds = objBL_User.GetAllWage(_objWage);
                    dtBomItem = ds.Tables[0];
                }
                if (dtBomItem.Rows.Count > 0)
                {
                    DataRow dr = dtBomItem.Select("Value =" + ddlItem.SelectedValue).FirstOrDefault();
                    if (dr != null)
                    {
                        txtScope.Text = dr["fDesc"].ToString();
                    }
                    else
                    {
                        txtScope.Text = "";
                    }
                }
            }
        }
    }
    #endregion

    #region Custom Functions
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProjectTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("SalesMgrSub");
        ////ul.Attributes.Remove("class");
        //ul.Style.Add("display", "block");
    }
    private void GetData()
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            DataSet ds = objBL_Customer.getJobTemplateByID(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = ds.Tables[0].Rows[0];
                lblProjectNo.Text = _dr["ID"].ToString();
                txtREPdesc.Text = _dr["fdesc"].ToString();
                txtREPremarks.Text = _dr["remarks"].ToString();
                ddlJobType.SelectedValue = _dr["Type"].ToString();
                ddlStatus.SelectedValue = _dr["Status"].ToString();
                hdnInvServiceID.Value = _dr["InvServ"].ToString();
                txtInvService.Text = _dr["InvServiceName"].ToString();
                hdnPrevilWageID.Value = _dr["Wage"].ToString();
                txtPrevilWage.Text = _dr["WageName"].ToString();

                uc_InterestGL._txtGLAcct.Text = _dr["GLName"].ToString();
                uc_InterestGL._hdnAcctID.Value = _dr["GLInt"].ToString();
                uc_InvExpGL._txtGLAcct.Text = _dr["InvExpName"].ToString();
                uc_InvExpGL._hdnAcctID.Value = _dr["InvExp"].ToString();
                //hdnInvExpGLID.Value = _dr["InvExp"].ToString();
                //txtInvExpGL.Text = _dr["InvExpName"].ToString();
                //hdnInterestGLID.Value = _dr["GLInt"].ToString();
                //txtInterestGL.Text = _dr["GLName"].ToString();
                if (!string.IsNullOrEmpty(_dr["CType"].ToString()))
                {
                    ddlContractType.SelectedValue = _dr["CType"].ToString();
                }
                ddlPostingMethod.SelectedValue = _dr["Post"].ToString();

                txtTempRev.Text = _dr["TemplateRev"].ToString();
                txtTempRemarks.Text = _dr["RevRemarks"].ToString();


                if (_dr["Charge"].ToString().Equals("1"))
                {
                    chkChargeable.Checked = true;
                }
                if (_dr["fInt"].ToString().Equals("1"))
                {
                    chkChargeInt.Checked = true;
                }
                if (_dr["JobClose"].ToString().Equals("1"))
                {
                    chkInvoicing.Checked = true;
                }
                if (Convert.ToBoolean(_dr["AlertMgr"]).Equals(true))
                {
                    chkAlert.Checked = true;
                }
                if (!_dr["AlertType"].ToString().Equals("Select Type"))
                {
                    ddlAlertType.SelectedValue = _dr["AlertType"].ToString();
                }
                ViewState["TempBOM"] = ds.Tables[1];
                ViewState["TempMilestone"] = ds.Tables[2];

                if (ds.Tables[1].Rows.Count > 0)
                {
                    gvBOM.DataSource = ds.Tables[1];
                    gvBOM.DataBind();
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvMilestones.DataSource = ds.Tables[2];
                    gvMilestones.DataBind();
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    ViewState["CustomTable"] = (DataTable)ds.Tables[3];
                    ViewState["CustomValues"] = (DataTable)ds.Tables[4];

                    gvCustom.DataSource = ds.Tables[3];
                    gvCustom.DataBind();
                }
                //if (ds.Tables[5].Rows.Count > 0)
                //{
                //    if (!string.IsNullOrEmpty(ds.Tables[5].Rows[0]["bLine"].ToString()))
                //    {
                //        ViewState["bLine"] = Convert.ToInt16(ds.Tables[5].Rows[0]["bLine"].ToString());
                //    }
                //}
                //if (ds.Tables[6].Rows.Count > 0)
                //{
                //    if (!string.IsNullOrEmpty(ds.Tables[6].Rows[0]["mLine"].ToString()))
                //    {
                //        ViewState["mLine"] = Convert.ToInt16(ds.Tables[6].Rows[0]["mLine"].ToString());
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region Header
    private void FillJobType()
    {
        try
        {
            // is recurring job template exist or not
            if (Request.QueryString["uid"] != null)
            {
                _objJob.ID = Convert.ToInt32(Request.QueryString["uid"]);
            }

            _objJob.IsExistRecurr = objBL_Job.IsExistRecurrJobT(_objJob);

            DataSet _dsJob = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            _dsJob = objBL_Job.GetAllJobType(_objJob);
            DataTable jobDt = new DataTable();

            //ViewState["RecurrJobType"] = _dsJob.Tables[0];
            if (_objJob.IsExistRecurr.Equals(true))
            {
                jobDt = _dsJob.Tables[0].Select("ID <> 0").CopyToDataTable();
            }
            else
            {
                jobDt = _dsJob.Tables[0];
            }

            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlJobType.Items.Clear();
                ddlJobType.Items.Add(new ListItem("Select Department", "Select Department"));
                ddlJobType.AppendDataBoundItems = true;
                ddlJobType.DataSource = jobDt;
                ddlJobType.DataValueField = "ID";
                ddlJobType.DataTextField = "Type";
                ddlJobType.DataBind();
            }
            else
            {
                ddlJobType.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Finance
    private void FillContractType()
    {
        try
        {
            DataSet _dsContract = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            _dsContract = objBL_Job.GetContractType(_objJob);
            if (_dsContract.Tables[0].Rows.Count > 0)
            {
                ddlContractType.Items.Add(new ListItem("Select Service Type", "0"));
                ddlContractType.AppendDataBoundItems = true;
                ddlContractType.DataSource = _dsContract;
                ddlContractType.DataValueField = "Type";
                ddlContractType.DataTextField = "Type";
                ddlContractType.DataBind();
            }
            else
            {
                ddlContractType.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillPosting()
    {
        try
        {
            DataSet _dsPost = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            _dsPost = objBL_Job.GetPosting(_objJob);
            if (_dsPost.Tables[0].Rows.Count > 0)
            {
                ddlPostingMethod.DataSource = _dsPost;
                ddlPostingMethod.DataBind();
                ddlPostingMethod.DataValueField = "ID";
                ddlPostingMethod.DataTextField = "Post";
                ddlPostingMethod.DataBind();
            }
            else
            {
                ddlPostingMethod.Items.Add(new ListItem("No data found"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region BOM
    private void CreateBOMTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Actual", typeof(double));
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("jPercent", typeof(double));
        dt.Columns.Add("btype", typeof(int));
        //dt.Columns.Add("Btype", typeof(string));
        dt.Columns.Add("BItem", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));

        DataRow dr = dt.NewRow();
        dr["Line"] = 0;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Line"] = 0;
        dt.Rows.Add(dr1);

        gvBOM.DataSource = dt;
        gvBOM.DataBind();
    }
    private DataTable GetBOMGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("btype", typeof(int));
        dt.Columns.Add("BItem", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("Actual", typeof(double));
        dt.Columns.Add("jPercent", typeof(double));

        string strItems = hdnItemJSON.Value.Trim();
        double _budgetExt = 0;
        double _qtyReq = 0;
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    _qtyReq = 0;
                    _budgetExt = 0;
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();

                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    //dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    if (dict["ddlBType"].ToString() != "0")
                    {
                        dr["btype"] = Convert.ToInt32(dict["ddlBType"]);
                    }

                    if (!Convert.ToInt32(dict["ddlItem"]).Equals(0))
                    {
                        dr["BItem"] = Convert.ToInt32(dict["ddlItem"]);
                    }

                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty)
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                        dr["QtyReq"] = 0;
                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        _budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = _budgetExt;
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetBOMItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("jBudget", typeof(double));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("btype", typeof(int));
        dt.Columns.Add("BItem", typeof(string));
        dt.Columns.Add("QtyReq", typeof(double));
        dt.Columns.Add("UM", typeof(string));
        dt.Columns.Add("ScrapFact", typeof(string));
        dt.Columns.Add("BudgetUnit", typeof(double));
        dt.Columns.Add("BudgetExt", typeof(double));
        dt.Columns.Add("Actual", typeof(double));
        dt.Columns.Add("jPercent", typeof(double));

        string strItems = hdnItemJSON.Value.Trim();
        double _budgetExt = 0;
        double _qtyReq = 0;
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    _qtyReq = 0;
                    _budgetExt = 0;
                    if ((dict["txtScope"].ToString().Trim() == string.Empty) && (dict["ddlBType"].ToString() != "0"))
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = dict["hdnLine"].ToString().Trim();
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    //dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    if (dict["ddlBType"].ToString() != "0")
                    {
                        dr["btype"] = Convert.ToInt32(dict["ddlBType"]);
                    }

                    if (!Convert.ToInt32(dict["ddlItem"]).Equals(0))
                    {
                        dr["BItem"] = Convert.ToInt32(dict["ddlItem"]);
                    }

                    if (dict["txtQtyReq"].ToString().Trim() != string.Empty)
                    {
                        dr["QtyReq"] = Convert.ToDouble(dict["txtQtyReq"]);
                    }
                    else
                        dr["QtyReq"] = 0;
                    if (dict["txtUM"].ToString().Trim() != string.Empty)
                    {
                        dr["UM"] = dict["txtUM"].ToString().Trim();
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        dr["BudgetUnit"] = Convert.ToDouble(dict["txtBudgetUnit"]);
                    }
                    if (dict["txtBudgetUnit"].ToString().Trim() != string.Empty)
                    {
                        if (_qtyReq.Equals(0))
                        {
                            _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim());
                        }
                        _budgetExt = _qtyReq * Convert.ToDouble(dict["txtBudgetUnit"].ToString());
                        dr["BudgetExt"] = _budgetExt;
                    }
                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    /// <summary>
    /// FillItems method is to fill drop down if none data available
    /// </summary>
    /// <param name="ddlItem"> A drop down control of gvBOM gridview</param>
    private void FillItems(DropDownList ddlItem)
    {
        ddlItem.Items.Clear();
        ddlItem.Items.Add(new ListItem("No data found", "0"));
        ddlItem.DataBind();
    }
    /// <summary>
    /// Fill Inventory details
    /// </summary>
    /// <param name="ddlItem"></param>
    private void FillInv(DropDownList ddlItem)
    {
        try
        {
            _objJob.ConnConfig = Session["config"].ToString();
            DataSet _dsInv = objBL_Job.GetInventoryItem(_objJob);
            dtBomItem = _dsInv.Tables[0];

            if (_dsInv.Tables[0].Rows.Count > 0)
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("Select Item", "0"));
                ddlItem.AppendDataBoundItems = true;
                ddlItem.DataSource = _dsInv;

                ddlItem.DataValueField = "value";
                ddlItem.DataTextField = "label";
                ddlItem.DataBind();
            }
            else
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillWage(DropDownList ddlItem)
    {
        try
        {
            _objWage.ConnConfig = Session["config"].ToString();
            DataSet _dsWage = objBL_User.GetAllWage(_objWage);
            dtBomItem = _dsWage.Tables[0];

            if (_dsWage.Tables[0].Rows.Count > 0)
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("Select Item", "0"));
                ddlItem.AppendDataBoundItems = true;
                ddlItem.DataSource = _dsWage;

                ddlItem.DataValueField = "value";
                ddlItem.DataTextField = "label";
                ddlItem.DataBind();
            }
            else
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            _objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(_objJob);

            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = 0;
            dr["Type"] = "Select Type";
            ds.Tables[0].Rows.InsertAt(dr, 0);

            dtBomType = ds.Tables[0];

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    #region Milestones
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquistDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));

        DataRow dr = dt.NewRow();
        dr["Line"] = 0;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Line"] = 0;
        dt.Rows.Add(dr1);

        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();

    }
    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));
        try
        {
            string strItems = hdnMilestone.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;

                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["hdnLine"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);

                    dr["MilesName"] = dict["txtName"].ToString().Trim();
                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("JobTItem", typeof(int));
        dt.Columns.Add("jType", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("jcode", typeof(string));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("MilesName", typeof(string));
        dt.Columns.Add("RequiredBy", typeof(DateTime));
        dt.Columns.Add("LeadTime", typeof(double));
        dt.Columns.Add("ProjAcquistDate", typeof(string));
        dt.Columns.Add("ActAcquDate", typeof(string));
        dt.Columns.Add("Comments", typeof(string));
        dt.Columns.Add("Type", typeof(int));
        dt.Columns.Add("Department", typeof(string));
        dt.Columns.Add("Amount", typeof(double));

        try
        {
            string strItems = hdnMilestone.Value.Trim();

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
                    dr["Line"] = dict["hdnLine"].ToString().Trim();
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();
                    dr["jcode"] = dict["txtCode"].ToString().Trim();
                    dr["jtype"] = Convert.ToInt16(dict["ddlType"]);
                    dr["MilesName"] = dict["txtName"].ToString().Trim();
                    if (dict["txtRequiredBy"].ToString() != string.Empty)
                    {
                        dr["RequiredBy"] = Convert.ToDateTime(dict["txtRequiredBy"]);
                    }
                    if (dict["txtAmount"].ToString() != string.Empty)
                    {
                        dr["Amount"] = Convert.ToDouble(dict["txtAmount"]);
                    }
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }

                    dt.Rows.Add(dr);
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }
    #endregion

    #region Checklist
    private void GetChecklist()
    {
        //GridView gvDepartment = (GridView)Page.FindControl("gvDepartment");
        //foreach (var uc in this.FindControl("PlaceHolder1").Controls.OfType<UserControl>())
        //{
        //    GridView gv1 = new GridView();
        //    GetDepartment(gv1);
        //}
        try
        {
            PlaceHolder p = (PlaceHolder)this.FindControl("PlaceHolder1");

            foreach (var uc in p.Controls)
            {
                GridView gv1 = new GridView();
                GetDepartment(gv1);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #region generate code for delegate
    protected override void OnInit(EventArgs e)
    {
        //InitializeComponent(_depQty);     //uncomment for checklist
        //base.OnInit(e);
    }
    private void InitializeComponent(int _dQty)
    {
        this.Load += new System.EventHandler(this.Page_Load);
        //_dQty = 0;        //uncomment for checklist
        //for (int i = 0; i < _dQty; i++)
        //{
        //    uc_gvChecklist ucGvChecklist = LoadControl("~/uc_gvChecklist.ascx") as uc_gvChecklist;
        //    PlaceHolder1.Controls.Add(ucGvChecklist);
        //    ucGvChecklist.GridRowCommand += new uc_gvChecklist.RowCommand(ucGvDepartment_GridRowCommand);
        //}
    }

    protected void ucGvDepartment_GridRowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        DataTable dtnew = new DataTable();
        GridView GvDepartment = (GridView)sender;

        switch (e.CommandName)
        {
            case "UpArr":
                dt = GetDepartment(GvDepartment);

                #region Up Row

                foreach (GridViewRow gr in GvDepartment.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked.Equals(true))
                    {
                        int rowIndex = gr.RowIndex;
                        if (rowIndex == 0)
                        {
                            continue;
                        }
                        else
                        {
                            string fdesc = dt.Rows[rowIndex]["fdesc"].ToString();
                            string _format = dt.Rows[rowIndex]["Format"].ToString();
                            string _refFormat = dt.Rows[rowIndex]["RefFormat"].ToString();

                            dt.Rows[rowIndex]["fdesc"] = dt.Rows[rowIndex - 1]["fdesc"];
                            dt.Rows[rowIndex]["Format"] = dt.Rows[rowIndex - 1]["Format"];
                            dt.Rows[rowIndex]["RefFormat"] = dt.Rows[rowIndex - 1]["RefFormat"];

                            dt.Rows[rowIndex - 1]["fdesc"] = fdesc;
                            dt.Rows[rowIndex - 1]["Format"] = _format;
                            dt.Rows[rowIndex - 1]["RefFormat"] = _refFormat;
                            dt.DefaultView.Sort = "Line";
                            dt.AcceptChanges();
                            dtnew = dt.Copy();
                            dt.AcceptChanges();
                            ViewState["Dep1"] = dtnew;
                        }
                    }
                }
                GvDepartment.DataSource = dt;
                GvDepartment.DataBind();
                #endregion

                break;
            case "DownArr":

                int lastRow = GvDepartment.Rows.Count - 1;
                dt = GetDepartment(GvDepartment);

                #region Down Row

                foreach (GridViewRow gr in GvDepartment.Rows)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked.Equals(true))
                    {
                        int rowIndex = gr.RowIndex;
                        if (rowIndex == lastRow)
                        {
                            continue;
                        }
                        else
                        {
                            string fdesc = dt.Rows[rowIndex]["fdesc"].ToString();
                            string _format = dt.Rows[rowIndex]["Format"].ToString();
                            string _refFormat = dt.Rows[rowIndex]["Format"].ToString();

                            dt.Rows[rowIndex]["fdesc"] = dt.Rows[rowIndex + 1]["fdesc"];
                            dt.Rows[rowIndex]["Format"] = dt.Rows[rowIndex + 1]["Format"];
                            dt.Rows[rowIndex]["RefFormat"] = dt.Rows[rowIndex + 1]["RefFormat"];

                            dt.Rows[rowIndex + 1]["fdesc"] = fdesc;
                            dt.Rows[rowIndex + 1]["Format"] = _format;
                            dt.Rows[rowIndex + 1]["RefFormat"] = _refFormat;
                            dt.DefaultView.Sort = "Line";
                            dt.AcceptChanges();
                            dtnew = dt.Copy();
                            dt.AcceptChanges();

                            ViewState["Dep"] = dtnew;
                        }
                    }
                }
                GvDepartment.DataSource = dt;
                GvDepartment.DataBind();
                #endregion

                break;
        }
    }
    #endregion

    private DataTable GetDepartment(GridView gvDepartment)
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("line", typeof(int));
        dt.Columns.Add("fdesc", typeof(string));
        dt.Columns.Add("Format", typeof(string));
        dt.Columns.Add("RefFormat", typeof(string));

        DataTable dtDetails = dt.Clone();
        int i = 0;
        try
        {
            foreach (GridViewRow gr in gvDepartment.Rows)
            {
                DataRow dr = dt.NewRow();
                i++;
                dr["Line"] = i;

                TextBox txtDescription = (TextBox)gr.FindControl("txtDescription");
                DropDownList ddlControl = (DropDownList)gr.FindControl("ddlControl");
                DropDownList ddlRefControl = (DropDownList)gr.FindControl("ddlRefControl");
                if (i <= 10)
                {
                    if (!string.IsNullOrEmpty(txtDescription.Text))
                    {
                        dr["fdesc"] = txtDescription.Text;
                        dr["Format"] = Convert.ToInt32(ddlControl.SelectedValue);
                        dr["RefFormat"] = Convert.ToInt32(ddlRefControl.SelectedValue);
                    }
                    dt.Rows.Add(dr);
                }
                else
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    #endregion

    #region Custom
    /// <summary>
    /// Fill Format drop down in Custom tab which contain list of controls.
    /// </summary>
    private void FillFormat()
    {
        try
        {
            dtFormat = new DataTable();
            dtFormat.Columns.Add("value", typeof(string));
            dtFormat.Columns.Add("format", typeof(string));

            DataRow drCustom = dtFormat.NewRow();
            drCustom["value"] = 0;
            drCustom["format"] = "";
            dtFormat.Rows.Add(drCustom);

            List<string> lstCustom = System.Enum.GetNames(typeof(CommonHelper.CustomField)).ToList();

            int i = 0;
            foreach (var lst in lstCustom)
            {
                i = i + 1;
                drCustom = dtFormat.NewRow();
                drCustom["value"] = i;
                drCustom["format"] = lst;

                dtFormat.Rows.Add(drCustom);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    /// <summary>
    /// Fill tab details of add/edit project page
    /// </summary>
    private void FillTab()
    {
        try
        {
            _objJob.ConnConfig = Session["config"].ToString();
            _objJob.PageUrl = "addproject.aspx";
            DataSet _ds = objBL_Job.GetTabByPageUrl(_objJob);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                dtTab = _ds.Tables[0];
                DataRow _drTab = dtTab.NewRow();
                _drTab["ID"] = 0;
                _drTab["TabName"] = "";
                dtTab.Rows.InsertAt(_drTab, 0);
            }
            else
            {
                dtTab = _ds.Tables[0];
                DataRow _drTab = dtTab.NewRow();
                _drTab["ID"] = 0;
                _drTab["TabName"] = "No Data Found";
                dtTab.Rows.Add(_drTab);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    /// <summary>
    /// Bind Custom drop down
    /// </summary>
    private void BindCustomDropDown()
    {
        int rowIndex = 1;
        try
        {
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                Label lblLine = (Label)gr.FindControl("lblLine");
                Panel pnlCustomValue = (Panel)gr.FindControl("pnlCustomValue");
                if (ddlFormat.SelectedItem.Text == "Dropdown")
                {
                    pnlCustomValue.Visible = true;
                }
                else
                    pnlCustomValue.Visible = false;

                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");
                Label lblID = (Label)gr.FindControl("lblID");

                if (ViewState["CustomValues"] != null)
                {
                    DataTable dtCustomval = (DataTable)ViewState["CustomValues"];
                    DataTable dt = dtCustomval.Clone();
                    //DataRow[] result = dtCustomval.Select("ItemID = " + Convert.ToInt32(lblID.Text) + "");
                    DataRow[] result = dtCustomval.Select("Line = " + Convert.ToInt32(lblLine.Text) + "");
                    foreach (DataRow row in result)
                    {
                        dt.ImportRow(row);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        dt.DefaultView.Sort = "Value  ASC";
                        dt = dt.DefaultView.ToTable();
                    }

                    ddlCustomValue.DataSource = dt;
                    ddlCustomValue.DataTextField = "Value";
                    ddlCustomValue.DataValueField = "Value";
                    ddlCustomValue.DataBind();
                    ddlCustomValue.Items.Insert(0, (new ListItem("--Add New--", "")));
                    if (ddlFormat.SelectedItem.Text == "Dropdown")
                    {
                        rowIndex++;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Fetch Custom field and Custom drop down details from gridview
    /// </summary>
    private void CustTempGridData()
    {
        try
        {
            DataTable dt = (DataTable)ViewState["CustomTable"];
            DataTable dtDetails = dt.Clone();
            DataTable dtCustomValues = new DataTable();
            dtCustomValues.Columns.Add("ID", typeof(int));
            dtCustomValues.Columns.Add("tblTabID", typeof(int));
            dtCustomValues.Columns.Add("Label", typeof(string));
            dtCustomValues.Columns.Add("Line", typeof(Int16));
            dtCustomValues.Columns.Add("Value", typeof(string));
            dtCustomValues.Columns.Add("Format", typeof(Int16));

            int line = 1;
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
                DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
                DropDownList ddlFormat = (DropDownList)gr.FindControl("ddlFormat");
                DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");

                if (!string.IsNullOrEmpty(lblDesc.Text.ToString()) && !ddlFormat.SelectedValue.Equals(0))
                {
                    foreach (ListItem li in ddlCustomValue.Items)
                    {
                        if (li.Value != string.Empty)
                        {
                            DataRow drCustomVal = dtCustomValues.NewRow();
                            drCustomVal["tblTabID"] = ddlTab.SelectedValue;
                            drCustomVal["Label"] = lblDesc.Text;
                            drCustomVal["Line"] = line;
                            drCustomVal["Value"] = li.Value;
                            drCustomVal["Format"] = ddlFormat.SelectedValue;
                            dtCustomValues.Rows.Add(drCustomVal);
                        }
                    }

                    DataRow dr = dtDetails.NewRow();
                    dr["ID"] = Convert.ToInt32(lblID.Text);
                    dr["tblTabID"] = ddlTab.SelectedValue;
                    dr["Label"] = lblDesc.Text.Trim();
                    dr["Line"] = lblIndex.Text;
                    dr["Format"] = ddlFormat.SelectedValue;
                    dtDetails.Rows.Add(dr);
                    line++;
                }
            }
            ViewState["CustomTable"] = dtDetails;
            ViewState["CustomValues"] = dtCustomValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Delete Custom field item from Custom gridview
    /// </summary>
    private void DeleteCustItem()
    {
        try
        {
            CustTempGridData();

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CustomTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridViewRow gr in gvCustom.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }

                    count++;
                }
            }

            ViewState["CustomDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = 0;
                dr["tblTabID"] = 0;
                dr["Label"] = DBNull.Value;
                dr["Line"] = dt.Rows.Count + 1;
                dr["Value"] = DBNull.Value;
                dr["Format"] = 0;
                dt.Rows.Add(dr);
            }

            ViewState["CustomTable"] = dt;
            BindCustomGrid();
            BindCustomDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /// <summary>
    /// Bind Custom gridview
    /// </summary>
    private void BindCustomGrid()
    {
        try
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["CustomTable"];

            gvCustom.DataSource = dt;
            gvCustom.DataBind();

            ((Label)gvCustom.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    //private void BindCustomGrid2()
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt = (DataTable)ViewState["CustomTable"];

    //        gvCustom2.DataSource = dt;
    //        gvCustom2.DataBind();

    //        ((Label)gvCustom2.FooterRow.FindControl("lblRowCount")).Text = "Total Line Items: " + Convert.ToString(dt.Rows.Count);
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    private void CreateCustomTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("tblTabID", typeof(int));
        dt.Columns.Add("Label", typeof(string));
        dt.Columns.Add("Line", typeof(Int16));
        dt.Columns.Add("Value", typeof(string));
        dt.Columns.Add("Format", typeof(Int16));

        DataRow dr = dt.NewRow();

        dr["ID"] = 0;
        dr["tblTabID"] = 0;
        dr["Label"] = DBNull.Value;
        dr["Line"] = dt.Rows.Count + 1;
        dr["Value"] = DBNull.Value;
        dr["Format"] = 0;

        dt.Rows.Add(dr);

        ViewState["CustomTable"] = dt;
    }
    #endregion
    #endregion

    protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlJobType.SelectedValue.Equals("0"))
        {
            DisableTab();
        }
        else
        {
            EnableTab();
        }
    }
    private void DisableTab()
    {
        foreach (GridViewRow gr in gvCustom.Rows)
        {
            DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
            ddlTab.Enabled = false;
            ddlTab.SelectedValue = "0";
        }
    }
    private void EnableTab()
    {
        foreach (GridViewRow gr in gvCustom.Rows)
        {
            DropDownList ddlTab = (DropDownList)gr.FindControl("ddlTab");
            ddlTab.Enabled = true;
        }
    }
    //private bool ValidateJobItem()
    //{
    //    bool IsValid = false;
    //    string warnmsg = string.Empty;
    //    DataTable dtB = GetItemsfromGrid();
    //    _objJob.ProjectDt = dtB;

    //    DataTable dtM = new DataTable();
    //    dtM = GetMilestoneItems();

    //    if(dtB.Rows.Count > 0)
    //    {
    //        int count_mat = dtB.Select("bType = 1").Count();     // Material count
    //        int count_labor = dtB.Select("bType = 2").Count();   // Labor count
    //        if(count_mat > 0 && count_labor > 0)
    //        {
    //            IsValid = true;
    //        }
    //    }

    //    if(IsValid)
    //    {
    //        if (dtM.Rows.Count > 0)
    //        {
    //            int countm = dtM.Select("Type = 1").Count();
    //            if (countm > 0)
    //            {
    //                IsValid = true;
    //            }   
    //        }

    //        if(!IsValid)
    //        {
    //            warnmsg = "Please add milestone item with type 'Finance'.";
    //        }
    //    }
    //    else
    //    {
    //        warnmsg = "Please add BOM item with type 'Labor' and 'Materials'.";
    //    }
    //    TabContainer2.ActiveTabIndex = 0;

    //    if (!IsValid)
    //    {
    //        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: '" + warnmsg + "', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //    return IsValid;
    //    //return false;
    //}

    /*#cc01:Added by rajesh start*/
    protected void gvCustom2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlFormat1 = (DropDownList)e.Row.FindControl("ddlFormat1");
            DropDownList ddlTab1 = (DropDownList)e.Row.FindControl("ddlTab1");
            objProp_Customer.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Customer.GetCustomerBomT(objProp_Customer);
            ddlTab1.DataSource = ds.Tables[0];
            ddlTab1.DataValueField = "ID";
            ddlTab1.DataTextField = "Type";
            ddlTab1.DataBind();


        }
    }
    protected void ibtnDeleteCItem2_Click(object sender, ImageClickEventArgs e)
    {
        DeleteEstimateItem();
    }
    protected void lnkAddnewRow1_Click(object sender, ImageClickEventArgs e)
    {
        int rowIndex = 0;

        if (ViewState["EstimateTable"] != null)
        {
            DataTable dtCurrentTable = (DataTable)ViewState["EstimateTable"];
            DataRow drCurrentRow = null;
            if (dtCurrentTable.Rows.Count > 0)
            {
                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    //extract the TextBox values
                    TextBox lblDesc = (TextBox)gvCustom2.Rows[rowIndex].Cells[1].FindControl("lblDesc");
                    DropDownList ddlTab1 = (DropDownList)gvCustom2.Rows[rowIndex].Cells[2].FindControl("ddlTab1");
                    TextBox txtPercentage = (TextBox)gvCustom2.Rows[rowIndex].Cells[3].FindControl("txtPercentage");
                    TextBox txtAmount = (TextBox)gvCustom2.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                    drCurrentRow = dtCurrentTable.NewRow();
                    //drCurrentRow["RowNumber"] = i + 1;

                    dtCurrentTable.Rows[i - 1]["Label"] = lblDesc.Text;
                    dtCurrentTable.Rows[i - 1]["TabCalculation"] = Convert.ToInt32(ddlTab1.SelectedValue);
                    dtCurrentTable.Rows[i - 1]["Percentage"] = Convert.ToDecimal(txtPercentage.Text);
                    dtCurrentTable.Rows[i - 1]["Amount"] = Convert.ToDecimal(txtAmount.Text);

                    rowIndex++;
                }
                dtCurrentTable.Rows.Add(drCurrentRow);
                ViewState["EstimateTable"] = dtCurrentTable;

                gvCustom2.DataSource = dtCurrentTable;
                gvCustom2.DataBind();
            }
        }
        else
        {
            Response.Write("ViewState is null");
        }

        //Set Previous Data on Postbacks
        SetPreviousData(); ;
        AddLabelText();
    }


    private void AddLabelText()
    {
        int rowIndex = 0;
        TextBox lblDesc = (TextBox)gvCustom2.Rows[rowIndex].Cells[1].FindControl("lblDesc");
        DropDownList ddlTab1 = (DropDownList)gvCustom2.Rows[rowIndex].Cells[2].FindControl("ddlTab1");
        string Label = lblDesc.Text.Trim();
        if (!string.IsNullOrEmpty(Label))
        {
            ddlTab1.Items.Add(new ListItem(Label, Label));
            rowIndex++;
        }
    }
    private void SetPreviousData()
    {
        int rowIndex = 0;
        if (ViewState["EstimateTable"] != null)
        {
            DataTable dt = (DataTable)ViewState["EstimateTable"];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TextBox lblDesc = (TextBox)gvCustom2.Rows[rowIndex].Cells[1].FindControl("lblDesc");
                    DropDownList ddlTab1 = (DropDownList)gvCustom2.Rows[rowIndex].Cells[2].FindControl("ddlTab1");
                    TextBox txtPercentage = (TextBox)gvCustom2.Rows[rowIndex].Cells[3].FindControl("txtPercentage");
                    TextBox txtAmount = (TextBox)gvCustom2.Rows[rowIndex].Cells[4].FindControl("txtAmount");
                    //TextBox box1 = (TextBox)gvCustom2.Rows[rowIndex].Cells[1].FindControl("TextBox1");
                    //TextBox box2 = (TextBox)gvCustom2.Rows[rowIndex].Cells[2].FindControl("TextBox2");
                    //TextBox box3 = (TextBox)gvCustom2.Rows[rowIndex].Cells[3].FindControl("TextBox3");

                    lblDesc.Text = dt.Rows[i]["Label"].ToString();
                    ddlTab1.SelectedValue = dt.Rows[i]["TabCalculation"].ToString();
                    txtPercentage.Text = dt.Rows[i]["Percentage"].ToString();
                    txtAmount.Text = dt.Rows[i]["Amount"].ToString();

                    rowIndex++;
                }
            }
        }
    }

    private void DeleteEstimateItem()
    {
        try
        {
            //CustTempGridData();

            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["EstimateTable"];
            DataTable dtdeleted = dt.Clone();
            int count = 0;
            foreach (GridViewRow gr in gvCustom2.Rows)
            {
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblIndex = (Label)gr.FindControl("lblIndex");
                int index = Convert.ToInt32(lblIndex.Text) - 1;
                if (chkSelect.Checked == true)
                {
                    if (dt.Rows.Count > 0)
                    {
                        dtdeleted.ImportRow(dt.Rows[index - count]);
                        dt.Rows.RemoveAt(index - count);
                    }

                    count++;
                }
            }

            ViewState["CustomDeletedRows"] = dtdeleted;

            if (dt.Rows.Count == 0)
            {
                DataRow dr = dt.NewRow();
                dr["Label"] = string.Empty;
                dr["TabCalculation"] = 0;
                dr["Percentage"] = 0;
                dr["Amount"] = 0;
                dt.Rows.Add(dr);
            }

            ViewState["EstimateTable"] = dt;
            gvCustom2.DataSource = (DataTable)ViewState["CustomDeletedRows"];
            gvCustom2.DataBind();
            SetPreviousData();
            //BindCustomDropDown();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void CustTempGridData2()
    {
        try
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //dt.Columns.Add(new DataColumn("RowNumber", typeof(string)));

            dt.Columns.Add("Label", typeof(string));
            dt.Columns.Add("TabCalculation", typeof(int));
            dt.Columns.Add("Percentage", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dr = dt.NewRow();
            dr["Label"] = string.Empty;
            dr["TabCalculation"] = 0;
            dr["Percentage"] = 0;
            dr["Amount"] = 0;
            dt.Rows.Add(dr);   //Store the DataTable in ViewState
            ViewState["EstimateTable"] = dt;

            gvCustom2.DataSource = dt;
            gvCustom2.DataBind();
            //DataTable dt = (DataTable)ViewState["CustomTable"];
            //DataTable dtDetails = dt.Clone();
            //DataTable dtEstimateTable = new DataTable();
            //dtEstimateTable.Columns.Add("Label", typeof(string));
            //dtEstimateTable.Columns.Add("TabCalculation", typeof(int));
            //dtEstimateTable.Columns.Add("Percentage", typeof(decimal));
            //dtEstimateTable.Columns.Add("Amount", typeof(decimal));


            //int line = 1;
            //foreach (GridViewRow gr in gvCustom2.Rows)
            //{

            //    TextBox lblDesc = (TextBox)gr.FindControl("lblDesc");
            //    DropDownList ddlTab1 = (DropDownList)gr.FindControl("ddlTab1");
            //    TextBox txtPercentage = (TextBox)gr.FindControl("txtPercentage");
            //    TextBox txtAmount = (TextBox)gr.FindControl("txtAmount");
            //   // DropDownList ddlCustomValue = (DropDownList)gr.FindControl("ddlCustomValue");

            //    if (lblDesc.Text.ToString()!="" && txtPercentage.Text.ToString()!="")
            //    {

            //        DataRow dr = dtEstimateTable.NewRow();
            //        dr["Label"] = Convert.ToString(lblDesc.Text.Trim());
            //        dr["TabCalculation"] = Convert.ToInt32(ddlTab1.SelectedValue);
            //        dr["Percentage"] = Convert.ToDecimal(txtPercentage.Text.Trim());

            //        dr["Amount"] = Convert.ToDecimal(txtAmount.Text.Trim());
            //        dtEstimateTable.Rows.Add(dr);
            //        line++;
            //    }
            //}
            //ViewState["EstimateTable"] = dtEstimateTable;
            //ViewState["CustomValues"] = dtCustomValues;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    /*#cc01:Added by rajesh end*/
}
