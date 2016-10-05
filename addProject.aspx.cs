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
using System.IO;
using System.Text;
using AjaxControlToolkit;
using System.Globalization;

public partial class addProject : System.Web.UI.Page
{
    #region "Variables"
    BL_Job objBL_Job = new BL_Job();
    JobT objJob = new JobT();
    protected DataTable dtBomType = new DataTable();
    protected DataTable dtBomItem = new DataTable();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objProp_Customer = new Customer();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    GeneralFunctions objgn = new GeneralFunctions();
    BL_BankAccount objBL_Bank = new BL_BankAccount();

    State objState = new State();

    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    protected DataTable dtCustomField = new DataTable();

    Wage objWage = new Wage();
    
    #endregion

    #region Events
    public bool IsCustomExist
    {
        get
        {
            if (ViewState["IsCustomExist"] == null)
                ViewState["IsCustomExist"] = false;
            return (bool)ViewState["IsCustomExist"];
        }
        set
        {
            ViewState["IsCustomExist"] = value;
        }
    }
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
            if (!IsPostBack)
            {   
                FillProjectsTemplate();
                FillState();
                FillContractType();
                FillJobType();
                FillJobStatus();
                FillPosting();

                txtProjCreationDate.Text = DateTime.Now.ToShortDateString();
                DisableControl();
                Initialize();

                if (Request.QueryString["locid"] != null)
                {
                    hdnLocID.Value = Request.QueryString["locid"].ToString();
                    btnSelectLoc_Click(sender,e);                
                }

                if (Request.QueryString["uid"] != null)
                {
                    //pnlNext.Visible = true;
                    lblHeader.Text = "Edit Project";
                    GetData();
                    BindBudget();
                    if (!ddlTemplate.SelectedValue.Equals("Select Template"))
                    {
                        ddlTemplate.Enabled = false;
                    }
                    objJob.ConnConfig = Session["config"].ToString();
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                    DataSet ds = objBL_Job.GetProfitLossValues(objJob);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drNet = ds.Tables[0].Rows[0];
                        string strVal = "";
                        strVal = " Revenues : " + string.Format("{0:c}", Convert.ToDouble(drNet["TotalBilled"]));
                        strVal += " ~ Labor expense : " + string.Format("{0:c}", Convert.ToDouble(drNet["LaborExp"]));
                        strVal += " ~ Material expense : " + string.Format("{0:c}", Convert.ToDouble(drNet["MaterialExp"]));
                        strVal += " ~ Other expense : " + string.Format("{0:c}", Convert.ToDouble(drNet["Expenses"]));
                        strVal += " ~ Profit Amount : " + string.Format("{0:c}", Convert.ToDouble(drNet["Net"]));
                        strVal += " ~ Actual Hours : " + Convert.ToDouble(drNet["Hour"]).ToString("0.00", CultureInfo.InvariantCulture);

                        //lblNetAmount.Visible = true;
                        lblNetAmountVal.Visible = true;
                        lblNetAmountVal.ForeColor = System.Drawing.Color.White;
                        //lblNetAmount.ForeColor = System.Drawing.Color.White;

                        lblNetAmountVal.Text = strVal;
                        //lblNetAmount.Text = strVal;
                        if (Convert.ToDouble(drNet["TotalBilled"]) < 0)
                        {
                            lblRevenue.ForeColor = System.Drawing.Color.Red;
                        }
                        if (Convert.ToDouble(drNet["LaborExp"]) < 0)
                        {
                            lblLabor.ForeColor = System.Drawing.Color.Red;
                        }
                        if (Convert.ToDouble(drNet["MaterialExp"]) < 0)
                        {
                            lblMaterialExp.ForeColor = System.Drawing.Color.Red;
                        }
                        if (Convert.ToDouble(drNet["Expenses"]) < 0)
                        {
                            lblExpense.ForeColor = System.Drawing.Color.Red;
                        }
                        if (Convert.ToDouble(drNet["Net"]) < 0)
                        {
                            lblProfitAmt.ForeColor = System.Drawing.Color.Red;
                        }
                        if (Convert.ToDouble(drNet["Hour"]) < 0)
                        {
                            lblActualHr.ForeColor = System.Drawing.Color.Red;
                        }

                        lblRevenue.Text = string.Format("{0:c}", Convert.ToDouble(drNet["TotalBilled"]));
                        lblLabor.Text = string.Format("{0:c}", Convert.ToDouble(drNet["LaborExp"]));
                        lblMaterialExp.Text = string.Format("{0:c}", Convert.ToDouble(drNet["MaterialExp"]));
                        lblExpense.Text = string.Format("{0:c}", Convert.ToDouble(drNet["Expenses"]));
                        lblProfitAmt.Text = string.Format("{0:c}", Convert.ToDouble(drNet["Net"]));
                        lblActualHr.Text = string.Format("{0:n}", Convert.ToDouble(drNet["Hour"]));
                        lblTotalExpense.Text = string.Format("{0:c}", Convert.ToDouble(drNet["TotalExp"]));
                        lblPercentProfit.Text = string.Format("{0:n}", Convert.ToDouble(drNet["NetPercent"]));
                        lblTotalOrder.Text = string.Format("{0:c}", Convert.ToDouble(drNet["TotalOnOrder"]));
                    }
                }
                else
                {
                    ddlJobStatus.SelectedValue = "0";
                    divAP.Visible = false;
                    divTickets.Visible = false;
                    divInvoices.Visible = false;
                    divJC.Visible = false;
                }

                BindEquip();
                divJC.Visible = false;

                ddlJobType.Enabled = false;
                Permission();
            }
            if (Page.IsPostBack)                // comment 8.16.16
            {
                #region rebind custom fields
                if (IsCustomExist)                    // commented by Mayuri on 28th july, 16 incomplete custom field func.
                {
                    bool IsTemplateDdl = false;
                    Control ctrl = null;

                    string ctrlName = Page.Request.Params.Get("__EVENTTARGET");
                
                    if (!String.IsNullOrEmpty(ctrlName))
                    {
                        ctrl = Page.FindControl(ctrlName);
                        if (ctrl.ID == ddlTemplate.ID)       //if ddltemplate caused postback then do not update previous template's custom fields
                        {
                            IsTemplateDdl = true;
                        }
                        if(ctrl.ID == lnkSaveTemplate.ID)
                        {
                            IsTemplateDdl = true;
                        }
                    }
                    if (!IsTemplateDdl)
                    {
                        objJob.ConnConfig = Session["config"].ToString();
                        if (ddlTemplate.SelectedValue.ToString() != "Select Template")
                        {
                            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                        }
                        if (Request.QueryString["uid"] != null)
                        {
                            objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                        }
                        DataSet dsTemp = new DataSet();
                        dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
                        DataTable dtCustom = GetCustomItems();
                        if (dtCustom == null)
                            dtCustom = dsTemp.Tables[0];
                        if (dsTemp.Tables[0].Rows.Count > 0)
                        {
                            CreateCustomTable();
                            DisplayCustomByTab(dtCustom, dsTemp.Tables[1], objJob.ID);
                        }
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
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
                //TextBox txtScrapFactor = (e.Row.FindControl("txtScrapFactor") as TextBox);
                Label lblBudgetExt = (e.Row.FindControl("lblBudgetExt") as Label);
                TextBox txtBudgetUnit = (e.Row.FindControl("txtBudgetUnit") as TextBox);
                TextBox txtQtyReq = (e.Row.FindControl("txtQtyReq") as TextBox);
                HiddenField hdnItem = (e.Row.FindControl("hdnItem") as HiddenField);

                double _budgetExt = 0.0;
                double _qtyReq = 0.0;
                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    FillInventory(ddlItem);
                    //txtScrapFactor.Enabled = true;

                    //if (!string.IsNullOrEmpty(txtScrapFactor.Text) && !string.IsNullOrEmpty(txtQtyReq.Text))
                    //{
                    //    _qtyReq = Convert.ToDouble(txtQtyReq.Text) + Convert.ToDouble(txtScrapFactor.Text);
                    //}
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    FillWage(ddlItem);
                    //txtScrapFactor.Enabled = false;
                }
                else
                {
                    FillItems(ddlItem);
                    //txtScrapFactor.Enabled = false;
                }

                if (!string.IsNullOrEmpty(hdnItem.Value))
                {
                    ddlItem.SelectedValue = hdnItem.Value;
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
                //lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);

                //if (ViewState["TempBOM"] != null)
                //{
                //    DataTable _dtBom = (DataTable)ViewState["TempBOM"];
                //    if (_dtBom.Rows.Count > 0)
                //    {
                //        foreach (var item in _dtBom.Rows)
                //        {
                //            var itemVal = _dtBom.Rows[0]["BItem"].ToString();
                //            if (!string.IsNullOrEmpty(itemVal))
                //            {
                //                if (e.Row.RowType == DataControlRowType.DataRow)
                //                {
                //                    itemVal = DataBinder.Eval(e.Row.DataItem, "BItem").ToString();
                //                    ddlItem.SelectedValue = itemVal.ToString();
                //                }
                //            }
                //        }
                //    }
                //}

                break;
        }
    }
    protected void gvBOM_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddProject"))
        {
            int rowIndex = gvBOM.Rows.Count - 1;
            GridViewRow row = gvBOM.Rows[rowIndex];
            HiddenField hdnIndex = row.FindControl("hdnIndex") as HiddenField;

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

            ViewState["ProjectTemplate"] = dt;
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
                //TextBox txtScrapFactor = (TextBox)gr.FindControl("txtScrapFactor");
                TextBox txtQtyReq = (TextBox)gr.FindControl("txtQtyReq");
                Label lblBudgetExt = (Label)gr.FindControl("lblBudgetExt");
                HiddenField hdnBudgetExt = (HiddenField)gr.FindControl("hdnBudgetExt");

                if (ddlBType.SelectedValue.Equals("1"))       // on select of Materials, fill inv data
                {
                    //txtScrapFactor.Enabled = true;
                    FillInventory(ddlItem);
                }
                else if (ddlBType.SelectedValue.Equals("2"))  // on select of Labor, fill wage data
                {
                    //txtScrapFactor.Enabled = false;
                    FillWage(ddlItem);
                }
                else
                {
                    //txtScrapFactor.Enabled = false;
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
                        //if (!string.IsNullOrEmpty(txtScrapFactor.Text))
                        //{
                        //    //_budgetExt = _budgetExt * Convert.ToDouble(txtScrapFactor.Text);
                        //    _qtyReq = Convert.ToDouble(txtQtyReq.Text) + Convert.ToDouble(txtScrapFactor.Text);
                        //}
                        //else
                        _qtyReq = Convert.ToDouble(txtQtyReq.Text);
                        _budgetExt = _qtyReq * Convert.ToDouble(txtBudgetUnit.Text);
                        //_budgetExt = Convert.ToDouble(txtBudgetUnit.Text) * Convert.ToDouble(txtQtyReq.Text);

                    }
                    lblBudgetExt.Text = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                    hdnBudgetExt.Value = _budgetExt.ToString("0.00", CultureInfo.InvariantCulture);
                }
                else
                {
                    //txtScrapFactor.Text = "";
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
    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            //objProp_Customer.TemplateID = Convert.ToInt32(ddlEstimates.SelectedValue);
            //objProp_Customer.Description = txtREPdesc.Text.Trim();
            //objProp_Customer.DtCustom = GetCustomItems();
            objProp_Customer.ConnConfig = Session["config"].ToString();

            #region add custom fields
            DataTable dtCustom = GetCustomItems();                // comment 8.16.16
            if(dtCustom != null)
            {
                if (dtCustom.Rows.Count > 0)
                {
            dtCustom.Columns.Remove("ControlID");
            dtCustom.Select("Format = 5 and Value = 'on'")
                .AsEnumerable().ToList()
                .ForEach(t => t["Value"] = true);
            dtCustom.AcceptChanges();

            dtCustom.Select("Format = 5 and Value = 'true'")
               .AsEnumerable().ToList()
               .ForEach(t => t["UpdatedDate"] = DateTime.Now);
            dtCustom.AcceptChanges();

            dtCustom.Select("Format = 5 and Value = 'true'")
               .AsEnumerable().ToList()
               .ForEach(t => t["Username"] = Session["username"].ToString());
            dtCustom.AcceptChanges();

            dtCustom.Select("Format = 5 and Value = 'off'")
                .AsEnumerable().ToList()
                .ForEach(t => t["Value"] = false);
            dtCustom.AcceptChanges();

            dtCustom.Select("Format = 5 and Value = ''")
              .AsEnumerable().ToList()
              .ForEach(t => t["Value"] = false);
            dtCustom.AcceptChanges();

            objProp_Customer.DtCustom = dtCustom;
                }
            }
            #endregion


            objProp_Customer.DtTeam = GetTeamItems();
            if (!string.IsNullOrEmpty(hdnCustID.Value))
            {
                objProp_Customer.CustomerID = Convert.ToInt32(hdnCustID.Value);
            }
            if (!string.IsNullOrEmpty(hdnLocID.Value))
            {
                objProp_Customer.LocID = Convert.ToInt32(hdnLocID.Value);
            }
            objProp_Customer.Name = txtREPdesc.Text.Trim();
            if (!ddlJobStatus.SelectedValue.Equals("Select Status"))
            {
                objProp_Customer.Status = Convert.ToInt32(ddlJobStatus.SelectedValue);
            }
            if (!ddlJobType.SelectedValue.Equals("Select Type"))
            {
                objProp_Customer.Type = ddlJobType.SelectedValue;
            }
            objProp_Customer.Remarks = txtREPremarks.Text.Trim();
            //add job.address

            if (!ddlTemplate.SelectedValue.Equals("Select Template"))
            {
                objProp_Customer.TemplateID = Convert.ToInt32(ddlTemplate.SelectedValue);
            }

            if (!string.IsNullOrEmpty(txtProjCreationDate.Text))
            {
                objProp_Customer.ProjectCreationDate = Convert.ToDateTime(txtProjCreationDate.Text);
            }
            else
            {
                objProp_Customer.ProjectCreationDate = DateTime.Now;
            }

            objProp_Customer.PO = txtPO.Text;
            objProp_Customer.SO = txtSalesOrder.Text;
            //add attach PO
            objProp_Customer.Certified = Convert.ToInt16(chkCertifiedJob.Checked);
            objProp_Customer.Custom1 = txtCustom1.Text;
            objProp_Customer.Custom2 = txtCustom2.Text;
            objProp_Customer.Custom3 = txtCustom3.Text;
            objProp_Customer.Custom4 = txtCustom4.Text;
            if (!string.IsNullOrEmpty(txtCustom5.Text))
            {
                objProp_Customer.Custom5 = Convert.ToDateTime(txtCustom5.Text);
            }

            #region rol detail
            objProp_Customer.RolName = txtName.Text;
            objProp_Customer.City = txtCity.Text;

            if (!ddlState.SelectedValue.Equals("Select State"))
            {
                objProp_Customer.State = ddlState.SelectedValue;
            }
            objProp_Customer.Zip = txtPostalCode.Text;
            objProp_Customer.Country = txtCountry.Text;
            objProp_Customer.Phone = txtPhone.Text;
            objProp_Customer.Cellular = txtMobile.Text;
            objProp_Customer.Fax = txtFax.Text;
            objProp_Customer.Contact = txtContactName.Text;
            objProp_Customer.Email = txtEmailWeb.Text;
            objProp_Customer.RolRemarks = txtRemarks.Text;
            objProp_Customer.RolType = 8;
            #endregion

            #region finance-general

            if (!string.IsNullOrEmpty(uc_InvExpGL._hdnAcctID.Value))
            {
                objProp_Customer.InvExp = Convert.ToInt32(uc_InvExpGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(uc_InterestGL._hdnAcctID.Value))
            {
                objProp_Customer.GLInt = Convert.ToInt32(uc_InterestGL._hdnAcctID.Value);
            }
            if (!string.IsNullOrEmpty(hdnInvServiceID.Value))
            {
                objProp_Customer.InvServ = Convert.ToInt32(hdnInvServiceID.Value);
            }
            if (!string.IsNullOrEmpty(hdnPrevilWageID.Value))
            {
                objProp_Customer.Wage = Convert.ToInt32(hdnPrevilWageID.Value);
            }
            if (!ddlContractType1.SelectedValue.Equals("0"))
            {
                objProp_Customer.JobTempCtype = ddlContractType1.SelectedValue;
            }
            if (!ddlContractType1.SelectedValue.Equals("0"))
            {
                objProp_Customer.ctypeName = ddlContractType1.SelectedValue;
            }
            if (!ddlPostingMethod.SelectedValue.Equals("0"))
            {
                objProp_Customer.Post = Convert.ToInt16(ddlPostingMethod.SelectedValue);
            }
            if (chkChargeInt.Checked.Equals(true))
            {
                objProp_Customer.fInt = 1;
            }
            else
                objProp_Customer.fInt = 0;

            if (chkInvoicing.Checked.Equals(true))
            {
                objProp_Customer.JobClose = 1;
            }
            else
                objProp_Customer.JobClose = 0;

            if (chkChargeable.Checked.Equals(true))
            {
                objProp_Customer.Charge = 1;
            }
            else
                objProp_Customer.Charge = 0;

            #endregion

            #region Milestones
            DataTable dtMilestone = new DataTable();
            dtMilestone = GetMilestoneItems();
            dtMilestone.Columns.Remove("Department");

            int mline = 1;

            if (ViewState["mLine"] == null)
            {
                dtMilestone.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }
            else
            {
                mline = (Int16)ViewState["mLine"];
                mline++;
                dtMilestone.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = mline++);
                dtMilestone.AcceptChanges();
            }
            #endregion

            #region BOM

            DataTable dtBom = GetBomItems();

            int bline = 1;

            if (ViewState["bLine"] == null)
            {
                dtBom.AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }
            else
            {
                bline = (Int16)ViewState["bLine"];
                bline++;
                dtBom.Select("Line = 0")
                    .AsEnumerable().ToList()
                    .ForEach(t => t["Line"] = bline++);
                dtBom.AcceptChanges();
            }
            #endregion

            if (Request.QueryString["uid"] != null)
            {
                objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            }
            
            objProp_Customer.DtBOM = dtBom;
            objProp_Customer.DtMilestone = dtMilestone;
            if (!string.IsNullOrEmpty(txtBillRate.Text))
            {
                objProp_Customer.BillRate = Convert.ToDouble(txtBillRate.Text);
            }
            if (!string.IsNullOrEmpty(txtOt.Text))
            {
                objProp_Customer.RateOT = Convert.ToDouble(txtOt.Text);
            }
            if (!string.IsNullOrEmpty(txtNt.Text))
            {
                objProp_Customer.RateNT = Convert.ToDouble(txtNt.Text);
            }
            if (!string.IsNullOrEmpty(txtDt.Text))
            {
                objProp_Customer.RateDT = Convert.ToDouble(txtDt.Text);
            }
            if (!string.IsNullOrEmpty(txtTravel.Text))
            {
                objProp_Customer.RateTravel = Convert.ToDouble(txtTravel.Text);
            }
            if (!string.IsNullOrEmpty(txtMileage.Text))
            {
                objProp_Customer.Mileage = Convert.ToDouble(txtMileage.Text);
            }

            int jobid = objBL_Customer.AddProject(objProp_Customer);

            if (Request.QueryString["uid"] != null)
            {
                GetData();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Updated Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                objgn.ResetFormControlValues(this);
                //Response.Redirect(Page.Request.RawUrl, false);
                Initialize();

                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyProj", "noty({text: 'Project Added Successfully! <BR/>Project# " + jobid.ToString() + "', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("project.aspx", false);
    }
    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        FillLoc();
    }
    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillAddress();
        BindEquip();
    }
    protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!ddlTemplate.SelectedValue.Equals("Select Template"))
        {
            EnableControl();
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
            GetJobTemplate(objJob.ID);
            //ddlTemplate.SelectedValue = ddlTemplate.SelectedValue;
            objProp_Customer.ProjectJobID = Convert.ToInt32(ddlTemplate.SelectedValue);
            objProp_Customer.ConnConfig = Session["config"].ToString();
            DataSet ds = objBL_Customer.getJobTemplateByID(objProp_Customer);

            //GetTemplateData(ddlTemplate.SelectedValue); // Fill details into BOM tab
            bool IsExistsb = IsExistsBOM();
            bool IsExistsm = IsExistsMilestone();

            if (!IsExistsb)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    gvBOM.DataSource = ds.Tables[1];
                    gvBOM.DataBind();
                }
            }

            if (!IsExistsm)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    gvMilestones.DataSource = ds.Tables[2];
                    gvMilestones.DataBind();
                }
            }

            #region Bind Custom fields
            // comment 8.16.16
            DataSet dsTemp = new DataSet();                   //commented by Mayuri on 28th july,16 incomplete custom field functionality
            dsTemp = objBL_Job.GetProjectTemplateCustomFields(objJob);
            if (dsTemp.Tables[0].Rows.Count > 0)
            {
                ViewState["IsCustomExist"] = true;
                CreateCustomTable();
                DisplayCustomByTab(dsTemp.Tables[0], dsTemp.Tables[1], objJob.ID);
            }

            #endregion
            DataSet dsJ = objBL_Job.GetJobTById(objJob);

            if (dsJ.Tables[0].Rows.Count > 0)
            {
                ddlJobType.SelectedValue = dsJ.Tables[0].Rows[0]["Type"].ToString();
            }
            else
            {
                ddlJobType.SelectedValue = "Select Type";
            }
            if (IsExistsb || IsExistsm)
            {
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "WarnTemplate", "WarningTemplate();", true);
            }
        }
        else
        {
            DisableControl();
        }
    }

    #region Budget

    #region Tickets
    protected void ddlPagesOpenCall_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvTickets.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvTickets.PageIndex = ddlPages.SelectedIndex;
        GetOpenCalls();
    }
    protected void gvOpenCalls_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvTickets.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvTickets.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvTickets.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvTickets.PageCount.ToString();
    }
    protected void PaginateOpencalls(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvTickets.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvTickets.PageIndex = 0;
                break;
            case "prev":
                gvTickets.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvTickets.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvTickets.PageIndex = gvTickets.PageCount;
                break;
        }

        // popultate the gridview control
        GetOpenCalls();
    }
    protected void gvOpenCalls_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateOpencalls(sender, e);
    }
    public string ShowHoverText(object desc, object reason)
    {
        string result = string.Empty;
        result = "<B>Reason</B>: " + Convert.ToString(reason).Replace("\n", "<br/>");

        if (!string.IsNullOrEmpty(Convert.ToString(desc)))
            result += "<br/><br/><B>Resolution</B>: " + Convert.ToString(desc).Replace("\n", "<br/>");

        return result;
    }
    protected void gvOpenCalls_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            AjaxControlToolkit.HoverMenuExtender ajxHover = (AjaxControlToolkit.HoverMenuExtender)e.Row.FindControl("hmeRes");
            e.Row.ID = e.Row.RowIndex.ToString();
            ajxHover.TargetControlID = e.Row.ID;
        }
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetOpenCalls();
    }
    #endregion

    #region Invoice
    protected void ddlPagesInvoice_SelectedIndexChanged(Object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvInvoice.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        gvInvoice.PageIndex = ddlPages.SelectedIndex;
        GetInvoices();
    }
    //protected void ddlArPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GridViewRow gvrPager = gvArInvoice.BottomPagerRow;
    //    DropDownList ddlArPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlArPages");
    //    gvArInvoice.PageIndex = ddlArPages.SelectedIndex;
    //    GetInvoices();
    //}
    protected void gvInvoice_DataBound(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvInvoice.BottomPagerRow;

        if (gvrPager == null) return;

        // get your controls from the gridview
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");
        Label lblPageCount = (Label)gvrPager.Cells[0].FindControl("lblPageCount");

        if (ddlPages != null)
        {
            // populate pager
            for (int i = 0; i < gvInvoice.PageCount; i++)
            {

                int intPageNumber = i + 1;
                ListItem lstItem = new ListItem(intPageNumber.ToString());

                if (i == gvInvoice.PageIndex)
                    lstItem.Selected = true;

                ddlPages.Items.Add(lstItem);
            }
        }

        // populate page count
        if (lblPageCount != null)
            lblPageCount.Text = gvInvoice.PageCount.ToString();
    }
    protected void PaginateInvoice(object sender, CommandEventArgs e)
    {
        // get the current page selected
        int intCurIndex = gvInvoice.PageIndex;

        switch (e.CommandArgument.ToString().ToLower())
        {
            case "first":
                gvInvoice.PageIndex = 0;
                break;
            case "prev":
                gvInvoice.PageIndex = intCurIndex - 1;
                break;
            case "next":
                gvInvoice.PageIndex = intCurIndex + 1;
                break;
            case "last":
                gvInvoice.PageIndex = gvInvoice.PageCount;
                break;
        }

        // popultate the gridview control
        GetInvoices();
    }
    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        PaginateInvoice(sender, e);
    }

    #endregion

    protected void ddlInvoiceStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetInvoices();
    }

    #endregion

    #region Attributes
    protected void gvTeamItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddTeam"))
        {
            int rowIndex = gvTeamItems.Rows.Count - 1;

            HiddenField hdnIndex = gvTeamItems.Rows[rowIndex].Cells[0].FindControl("hdnIndex") as HiddenField;

            DataTable dt = GetTeamItems();
            if (dt.Rows.Count < 1)
            {
                for (int j = 0; j <= rowIndex; j++)
                {
                    DataRow dr = dt.NewRow();
                    dt.Rows.Add(dr);
                }
            }
            DataRow dr2 = dt.NewRow();
            dt.Rows.Add(dr2);

            gvTeamItems.DataSource = dt;
            gvTeamItems.DataBind();
        }
    }
    #endregion

    #region Finance
    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        try
        {
            string filename = string.Empty;
            string fullpath = string.Empty;
            string MIME = string.Empty;
            if (fuAttachPO.HasFile)
            {
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\ld_" + Request.QueryString["uid"].ToString() + @"\";
                filename = fuAttachPO.FileName;
                fullpath = savepath + filename;
                MIME = System.IO.Path.GetExtension(fuAttachPO.PostedFile.FileName).Substring(1);

                if (File.Exists(fullpath))
                {
                    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                fuAttachPO.SaveAs(fullpath);
            }

            objMapData.Screen = "Project";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            objMapData.TempId = "0";
            objMapData.FileName = filename;
            objMapData.DocTypeMIME = MIME;
            objMapData.FilePath = fullpath;
            objMapData.DocID = 0;
            objMapData.Mode = 0;
            objMapData.ConnConfig = Session["config"].ToString();
            objBL_MapData.AddFile(objMapData);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }


    #endregion

    protected void gvArInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //PaginateInvoice(sender, e);
    }
    protected void ddlJobType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!ddlJobType.SelectedValue.Equals("Select Template"))
            {
                objJob.ConnConfig = Session["config"].ToString();

                objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);
                objJob.Type = Convert.ToInt16(ddlJobType.SelectedValue);
                objJob.IsExist = objBL_Job.IsExistProjectTempByType(objJob);
                if (!objJob.IsExist.Equals(true))
                {
                    ddlTemplate.SelectedValue = "0";
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMilestones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("AddMilestone"))
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

            gvMilestones.DataSource = dt;
            gvMilestones.DataBind();
        }
    }
    protected void ibDeleteBom_Click(object sender, ImageClickEventArgs e)
    {
        //DataTable dt = GetBOMGridItems();
        foreach (GridViewRow gr in gvBOM.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked.Equals(true))
            {
                Label lblLine = gr.FindControl("lblLine") as Label;

                if (Request.QueryString["uid"] != null)
                {
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objJob.Phase = Convert.ToInt32(lblLine.Text);
                    bool IsExist = objBL_Job.IsExistExpJobItemByJob(objJob);
                    if (IsExist.Equals(true))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvBOM.ClientID + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvBOM.ClientID + "')", true);
                }
            }
        }
    }
    protected void ibDeleteMilestone_Click(object sender, ImageClickEventArgs e)
    {
        //DataTable dt = GetMilestoneGridItems();

        foreach (GridViewRow gr in gvMilestones.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (chkSelect.Checked.Equals(true))
            {
                Label lblLine = gr.FindControl("lblLine") as Label;

                if (Request.QueryString["uid"] != null)
                {
                    objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                    objJob.Phase = Convert.ToInt32(lblLine.Text);
                    bool IsExist = objBL_Job.IsExistRevJobItemByJob(objJob);
                    if (IsExist.Equals(true))
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "noty({text: 'Selected job item is in use, it cannot be deleted!', dismissQueue: true,  type : 'warning', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvMilestones.ClientID + "')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "removebomLine", "removeLine('" + gvMilestones.ClientID + "')", true);
                }
            }
        }
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
                    objJob.ConnConfig = Session["config"].ToString();
                    ds = objBL_Job.GetInventoryItem(objJob);
                    dtBomItem = ds.Tables[0];
                }
                else if (dtBomItem.Rows.Count.Equals(0) && ddlBType.SelectedValue == "2")
                {
                    objWage.ConnConfig = Session["config"].ToString();
                    ds = objBL_User.GetAllWage(objWage);
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
    private void GetData()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        objProp_Customer.ProjectJobID = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objProp_Customer.Type = string.Empty;
        DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //ddlEstimates.SelectedValue = ds.Tables[0].Rows[0]["estimateid"].ToString();
            //txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
            lblProjectNo.Text = "# " + ds.Tables[0].Rows[0]["ID"].ToString();
            txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
            txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
                ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();
            //uc_LocationSearch1._txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
            //uc_LocationSearch1._hdnLocId.Value = ds.Tables[0].Rows[0]["loc"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
            hdnLocID.Value = ds.Tables[0].Rows[0]["loc"].ToString();
            hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
            txtCustomer.Text = ds.Tables[0].Rows[0]["customerName"].ToString();
            //ddlTemplate.SelectedValue = ds.Tables[0].Rows[0]["template"].ToString();

            ddlJobType.SelectedValue = ds.Tables[0].Rows[0]["Type"].ToString();
            ddlJobStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ctype"].ToString()))
            {
                ddlContractType1.SelectedValue = ds.Tables[0].Rows[0]["ctype"].ToString();
            }

            if (Convert.ToBoolean(ds.Tables[0].Rows[0]["Certified"]))
            {
                chkCertifiedJob.Checked = true;
            }
            txtPO.Text = ds.Tables[0].Rows[0]["PO"].ToString();
            txtSalesOrder.Text = ds.Tables[0].Rows[0]["SO"].ToString();
            txtCustom1.Text = ds.Tables[0].Rows[0]["Custom21"].ToString();
            txtCustom2.Text = ds.Tables[0].Rows[0]["Custom22"].ToString();
            txtCustom3.Text = ds.Tables[0].Rows[0]["Custom23"].ToString();
            txtCustom4.Text = ds.Tables[0].Rows[0]["Custom24"].ToString();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Custom25"].ToString()))
            {
                txtCustom5.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["Custom25"]).ToString("MM/dd/yyyy");
            }

            txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ProjCreationDate"].ToString()))
            {
                txtProjCreationDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["ProjCreationDate"]).ToString("MM/dd/yyyy");
            }

            #region gc information

            txtName.Text = ds.Tables[0].Rows[0]["gcName"].ToString();
            txtCity.Text = ds.Tables[0].Rows[0]["gcCity"].ToString();
            if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["gcState"].ToString()))
            {
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["gcState"].ToString();
            }

            txtPostalCode.Text = ds.Tables[0].Rows[0]["gcZip"].ToString();
            txtCountry.Text = ds.Tables[0].Rows[0]["gcCountry"].ToString();
            txtPhone.Text = ds.Tables[0].Rows[0]["gcPhone"].ToString();
            txtMobile.Text = ds.Tables[0].Rows[0]["gcCellular"].ToString();
            txtFax.Text = ds.Tables[0].Rows[0]["gcFax"].ToString();
            txtEmailWeb.Text = ds.Tables[0].Rows[0]["gcEmail"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["gcRemarks"].ToString();
            txtContactName.Text = ds.Tables[0].Rows[0]["gcContact"].ToString();

            #endregion

            #region finance-general


            //if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CType"].ToString()))
            if (!Convert.ToInt32(ds.Tables[0].Rows[0]["Template"]).Equals(0))
            {
                EnableControl();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["jobTempCtype"].ToString()))
                {
                    ddlContractType1.SelectedValue = ds.Tables[0].Rows[0]["jobTempCtype"].ToString();
                }

                hdnInvServiceID.Value = ds.Tables[0].Rows[0]["InvServ"].ToString();
                txtInvService.Text = ds.Tables[0].Rows[0]["InvServiceName"].ToString();
                hdnPrevilWageID.Value = ds.Tables[0].Rows[0]["Wage"].ToString();
                txtPrevilWage.Text = ds.Tables[0].Rows[0]["WageName"].ToString();
                uc_InterestGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["GLName"].ToString();
                uc_InterestGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["GLInt"].ToString();
                uc_InvExpGL._txtGLAcct.Text = ds.Tables[0].Rows[0]["InvExpName"].ToString();
                uc_InvExpGL._hdnAcctID.Value = ds.Tables[0].Rows[0]["InvExp"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Post"].ToString()))
                {
                    ddlPostingMethod.SelectedValue = ds.Tables[0].Rows[0]["Post"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Charge"].ToString().Equals("1"))
                {
                    chkChargeable.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["fInt"].ToString().Equals("1"))
                {
                    chkChargeInt.Checked = true;
                }
                if (ds.Tables[0].Rows[0]["JobClose"].ToString().Equals("1"))
                {
                    chkInvoicing.Checked = true;
                }

            }
            else
            {
                DisableControl();
            }

            #endregion

            //ddlEstimates_SelectedIndexChanged(sender, e);
            if (ds.Tables[0].Rows[0]["estimateid"].ToString() != "")
            {
                trEstimate.Visible = true;
                lnkEstimate.Text = ds.Tables[0].Rows[0]["estimateid"].ToString() + " - " + ds.Tables[0].Rows[0]["estimate"].ToString();
                lnkEstimate.NavigateUrl = "addestimate.aspx?uid=" + ds.Tables[0].Rows[0]["estimateid"].ToString();
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                gvBOM.DataSource = ds.Tables[2];
                gvBOM.DataBind();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                gvTeamItems.DataSource = ds.Tables[3];
                gvTeamItems.DataBind();
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                gvMilestones.DataSource = ds.Tables[4];
                gvMilestones.DataBind();
            }

            GetOpenCalls();
            GetInvoices();
            GetAPInvoices();
            if (ds.Tables[0].Rows[0]["template"].ToString() != string.Empty)
            {
                objJob.ID = Convert.ToInt32(ds.Tables[0].Rows[0]["template"].ToString());
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
                DataSet dsCustom = objBL_Job.GetProjectTemplateCustomFields(objJob);
                if (dsCustom.Tables[0].Rows.Count > 0)
                {
                    ViewState["IsCustomExist"] = true;
                    CreateCustomTable();
                    DisplayCustomByTab(dsCustom.Tables[0], dsCustom.Tables[1], objJob.ID);
                }
            }

            GetJobCost();

            if (ds.Tables[5].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[5].Rows[0]["bLine"].ToString()))
                {
                    ViewState["bLine"] = Convert.ToInt16(ds.Tables[5].Rows[0]["bLine"].ToString());
                }
            }
            if (ds.Tables[6].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[6].Rows[0]["mLine"].ToString()))
                {
                    ViewState["mLine"] = Convert.ToInt16(ds.Tables[6].Rows[0]["mLine"].ToString());
                }
            }
            txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
            txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
            txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
            txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
            txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
            txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
        }
    }
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("ProjectMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("ProjectLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkProject");
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
    private void Initialize()
    {
        CreateBOMTable();
        CreateMilestoneTable();
        CreateTeamTable();
        BindEquip();
    }

    #region Milestones
    private void CreateMilestoneTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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

        ViewState["MProjectTemplate"] = dt;
        gvMilestones.DataSource = dt;
        gvMilestones.DataBind();
    }
    private bool IsExistsMilestone()
    {
        string strItems = hdnMilestone.Value.Trim();
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
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return false;
                    }
                    i++;
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        if (Convert.ToInt16(dict["hdnID"].ToString()) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return false;
    }
    private DataTable GetMilestoneItems()
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
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
                    //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    //if (!string.IsNullOrEmpty(dict["txtActAcquiDate"].ToString()))
                    //{
                    //    dr["ActAcquDate"] = dict["txtActAcquiDate"].ToString();
                    //}
                    //dr["Comments"] = dict["txtComments"].ToString();

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
    private DataTable GetMilestoneGridItems()       //get all items in milestone grid
    {
        DataTable dt = new DataTable();

        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
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
                    //dr["LeadTime"] = dict["txtLeadTime"].ToString();
                    if (!string.IsNullOrEmpty(dict["hdnType"].ToString()))
                    {
                        dr["Type"] = dict["hdnType"].ToString();
                        dr["Department"] = dict["txtSType"].ToString();
                    }
                    //if (!string.IsNullOrEmpty(dict["txtActAcquiDate"].ToString()))
                    //{
                    //    dr["ActAcquDate"] = dict["txtActAcquiDate"].ToString();
                    //}
                    //dr["Comments"] = dict["txtComments"].ToString();

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

    #region BOM
    private void CreateBOMTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
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
        dr["ID"] = 0;
        dr["Line"] = 0;
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["ID"] = 0;
        dr1["Line"] = 0;
        dt.Rows.Add(dr1);

        ViewState["ProjectTemplate"] = dt;
        gvBOM.DataSource = dt;
        gvBOM.DataBind();
    }
    private DataTable GetBomItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    if (dict["hdnLine"].ToString().Trim() != string.Empty)
                    {
                        dr["Line"] = Convert.ToInt32(dict["hdnLine"].ToString());
                    }
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
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
                    //if (dict["ddlBType"].ToString() != "0" && dict["ddlBType"].ToString() != "2")
                    //if (dict["ddlBType"].ToString() == "1")
                    //{
                    //if (dict["txtScrapFactor"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["ScrapFact"] = Convert.ToDouble(dict["txtScrapFactor"]);
                    //    if (!string.IsNullOrEmpty(dict["txtQtyReq"].ToString().Trim()))
                    //    {
                    //        _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim()) + Convert.ToDouble(dict["txtScrapFactor"]);
                    //    }
                    //}
                    //}
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
    private bool IsExistsBOM()
    {
        string strItems = hdnItemJSON.Value.Trim();
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
                    if (dict["txtScope"].ToString().Trim() == string.Empty)
                    {
                        return false;
                    }
                    i++;
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        if (Convert.ToInt16(dict["hdnID"].ToString()) > 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return false;
    }
    private DataTable GetBOMGridItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("JobT", typeof(int));
        dt.Columns.Add("Job", typeof(int));
        dt.Columns.Add("ID", typeof(int));
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
                    if (dict["hdnID"].ToString().Trim() != string.Empty)
                    {
                        dr["ID"] = Convert.ToInt32(dict["hdnID"].ToString());
                    }
                    else
                    {
                        dr["ID"] = 0;
                    }
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
                    //if (dict["ddlBType"].ToString() != "0" && dict["ddlBType"].ToString() != "2")
                    //if (dict["ddlBType"].ToString() == "1")
                    //{
                    //if (dict["txtScrapFactor"].ToString().Trim() != string.Empty)
                    //{
                    //    dr["ScrapFact"] = Convert.ToDouble(dict["txtScrapFactor"]);
                    //    if (!string.IsNullOrEmpty(dict["txtQtyReq"].ToString().Trim()))
                    //    {
                    //        _qtyReq = Convert.ToDouble(dict["txtQtyReq"].ToString().Trim()) + Convert.ToDouble(dict["txtScrapFactor"]);
                    //    }
                    //}
                    //}
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
    private void FillBomType()
    {
        try
        {
            DataSet ds = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            ds = objBL_Job.GetBomType(objJob);

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
    private void FillInventory(DropDownList ddlItem)
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();
            DataSet _dsInv = objBL_Job.GetInventoryItem(objJob);
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
            objWage.ConnConfig = Session["config"].ToString();
            DataSet _dsWage = objBL_User.GetAllWage(objWage);
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
    private void FillItems(DropDownList ddlItem)
    {
        ddlItem.Items.Clear();
        ddlItem.Items.Add(new ListItem("No data found", "0"));
        ddlItem.DataBind();
    }
    #endregion

    private void FillLoc()
    {
        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnCustID.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser);

        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocID.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillAddress();
            BindEquip();
        }
    }
    private void FillJobStatus()
    {
        objJob.ConnConfig = Session["config"].ToString();
        DataSet _ds = objBL_Job.GetJobStatus(objJob);
        if (_ds.Tables[0].Rows.Count > 0)
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("Select Status"));
            ddlJobStatus.AppendDataBoundItems = true;
            ddlJobStatus.DataSource = _ds;
            ddlJobStatus.DataValueField = "ID";
            ddlJobStatus.DataTextField = "Status";
            ddlJobStatus.DataBind();
        }
        else
        {
            ddlJobStatus.Items.Clear();
            ddlJobStatus.Items.Add(new ListItem("No data found", "Select Status"));
        }
    }
    private void FillAddress()
    {
        if (!string.IsNullOrEmpty(hdnLocID.Value))
        {
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
                if (string.IsNullOrEmpty(hdnCustID.Value))
                {
                    txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                    hdnCustID.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                }
                txtBillRate.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["BillRate"].ToString()));
                txtOt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateOT"].ToString()));
                txtNt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateNT"].ToString()));
                txtDt.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateDT"].ToString()));
                txtMileage.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateMileage"].ToString()));
                txtTravel.Text = string.Format("{0:n}", Convert.ToDouble(ds.Tables[0].Rows[0]["RateTravel"].ToString()));
            }
            else
            {
                txtAddress.Text = "0.00";
                txtBillRate.Text = "0.00";
                txtOt.Text = "0.00";
                txtNt.Text = "0.00";
                txtDt.Text = "0.00";
                txtMileage.Text = "0.00";
                txtTravel.Text = "0.00";
            }
        }
    }
    private void FillJobType()
    {
        try
        {
            DataSet _dsJob = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsJob = objBL_Job.GetAllJobType(objJob);
            if (_dsJob.Tables[0].Rows.Count > 0)
            {
                ddlJobType.Items.Add(new ListItem("Select Type", "Select Type"));
                ddlJobType.AppendDataBoundItems = true;
                ddlJobType.DataSource = _dsJob;
                ddlJobType.DataValueField = "ID";
                ddlJobType.DataTextField = "Type";
                ddlJobType.DataBind();
            }
            else
            {
                ddlJobType.Items.Add(new ListItem("No data found", "Select Type"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillProjectsTemplate()
    {
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemp(objProp_Customer);
        if (ds.Tables[0].Rows.Count > 0)
        {
            rfvTemplateType.InitialValue = "Select Template";
            if (Request.QueryString["uid"] == null)
            {
                DataRow dr = ds.Tables[0].Select("id = 0").FirstOrDefault();
                if (dr != null)
                {
                    ds.Tables[0].Rows.Remove(dr);
                }
            }
            ddlTemplate.DataSource = ds.Tables[0];
            ddlTemplate.DataTextField = "Fdesc";
            ddlTemplate.DataValueField = "id";
            ddlTemplate.DataBind();
            ddlTemplate.Items.Insert(0, new ListItem("Select Template"));
        }
        else
        {
            rfvTemplateType.InitialValue = "No data found";
            ddlTemplate.Items.Insert(0, new ListItem("No data found", "0"));
        }
    }

    #region Attribute
    private DataTable GetTeamItems()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("JobID", typeof(int));
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        try
        {
            string strItems = hdnItemTeamJSON.Value.Trim();

            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
                objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                int i = 0;
                //if(objEstimateItemData.Count > 0)
                //{
                foreach (Dictionary<object, object> dict in objEstimateItemData)
                {
                    if (dict["txtTitle"].ToString().Trim() == string.Empty)
                    {
                        return dt;
                    }
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Line"] = i;
                    if (Request.QueryString["uid"] != null)
                    {
                        dr["JobID"] = Convert.ToInt32(Request.QueryString["uid"]);
                    }
                    else
                    {
                        dr["JobID"] = 0;
                    }
                    dr["Title"] = dict["txtTitle"].ToString().Trim();
                    dr["UserID"] = dict["txtUserID"].ToString().Trim();
                    dr["FirstName"] = dict["txtFirstName"].ToString().Trim();
                    dr["LastName"] = dict["txtLastName"].ToString().Trim();
                    dr["Email"] = dict["txtEmail"].ToString().Trim();
                    dr["Mobile"] = dict["txtMobile"].ToString().Trim();
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
    private void FillState()
    {
        try
        {
            DataSet dsState = new DataSet();

            objState.ConnConfig = Session["config"].ToString();
            dsState = objBL_Bank.GetStates(objState);
            if (dsState.Tables[0].Rows.Count > 0)
            {
                ddlState.Items.Add(new ListItem("Select State"));
                ddlState.AppendDataBoundItems = true;

                ddlState.DataSource = dsState;
                ddlState.DataValueField = "Name";
                ddlState.DataTextField = "fDesc";
                ddlState.DataBind();
            }
            else
            {
                ddlState.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void CreateTeamTable()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Title", typeof(string));
        dt.Columns.Add("UserID", typeof(string));
        dt.Columns.Add("FirstName", typeof(string));
        dt.Columns.Add("LastName", typeof(string));
        dt.Columns.Add("Email", typeof(string));
        dt.Columns.Add("Mobile", typeof(string));

        DataRow dr = dt.NewRow();
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dt.Rows.Add(dr1);

        ViewState["TeamItems"] = dt;
        gvTeamItems.DataSource = dt;
        gvTeamItems.DataBind();
    }
    private void FillContractType()
    {
        try
        {
            DataSet _dsContract = new DataSet();
            objJob.ConnConfig = Session["config"].ToString();
            _dsContract = objBL_Job.GetContractType(objJob);
            if (_dsContract.Tables[0].Rows.Count > 0)
            {
                //ddlContractType.Items.Add(new ListItem("Select Service Type", "0"));
                //ddlContractType.AppendDataBoundItems = true;
                //ddlContractType.DataSource = _dsContract;
                //ddlContractType.DataValueField = "Type";
                //ddlContractType.DataTextField = "Type";
                //ddlContractType.DataBind();

                ddlContractType1.Items.Add(new ListItem("Select Service Type", "0"));
                ddlContractType1.AppendDataBoundItems = true;
                ddlContractType1.DataSource = _dsContract;
                ddlContractType1.DataValueField = "Type";
                ddlContractType1.DataTextField = "Type";
                ddlContractType1.DataBind();
            }
            else
            {
                //ddlContractType.Items.Add(new ListItem("No data found", "0"));
                ddlContractType1.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void BindEquip()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        if (string.IsNullOrEmpty(hdnLocID.Value))
        {
            objPropUser.LocID = 0;
        }
        else
        {
            objPropUser.LocID = Convert.ToInt32(hdnLocID.Value);
        }

        DataSet ds = objBL_User.getElevByLoc(objPropUser);
        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();
    }
    #endregion

    #region Budget
    private void GetOpenCalls()
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
        objMapData.OrderBy = "edate desc";
        objMapData.Department = -1;
        ds = objBL_MapData.getCallHistory(objMapData);
        gvTickets.DataSource = ds.Tables[0];
        gvTickets.DataBind();
        CalculateBalance(ds.Tables[0]);
        lblTicketCount.Text = ds.Tables[0].Rows.Count + " ticket(s) found.";
    }
    private void GetInvoices()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());
        if (ddlInvoiceStatus.SelectedValue != "-1")
        {
            objProp_Contracts.SearchBy = "i.Status";
            objProp_Contracts.SearchValue = ddlInvoiceStatus.SelectedValue;
        }
        ds = objBL_Contracts.GetInvoices(objProp_Contracts);
        gvInvoice.DataSource = ds.Tables[0];
        gvInvoice.DataBind();
        calculateInvoice(ds.Tables[0]);
        //gvArInvoice.DataSource = ds.Tables[0];
        //gvArInvoice.DataBind();
    }
    private void GetAPInvoices()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

        ds = objBL_Contracts.GetAPInvoices(objProp_Contracts);
        gvAPInvoices.DataSource = ds.Tables[0];
        gvAPInvoices.DataBind();
    }
    private void GetJobCost()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        objProp_Contracts.jobid = Convert.ToInt32(Request.QueryString["uid"].ToString());

        ds = objBL_Contracts.GetJobCostItems(objProp_Contracts);
        gvJOBC.DataSource = ds.Tables[0];
        gvJOBC.DataBind();
    }
    private void CalculateBalance(DataTable dt)
    {
        double dblBalTotal = 0;
        double dblExpenses = 0;
        double dblEST = 0;
        double dblLabExpenses = 0;
        double dblRT = 0;
        double dblOT = 0;
        double dblDT = 0;
        double dblTT = 0;
        double dblNT = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["tottime"].ToString() != string.Empty)
                {
                    dblBalTotal += Convert.ToDouble(dr["tottime"].ToString());
                }
                if (dr["expenses"].ToString() != string.Empty)
                {
                    dblExpenses += Convert.ToDouble(dr["expenses"].ToString());
                }
                if (dr["est"].ToString() != string.Empty)
                {
                    dblEST += Convert.ToDouble(dr["est"].ToString());
                }
                if (dr["laborexp"].ToString() != string.Empty)
                {
                    dblLabExpenses += Convert.ToDouble(dr["laborexp"].ToString());
                }
                if (dr["reg"].ToString() != string.Empty)
                {
                    dblRT += Convert.ToDouble(dr["reg"].ToString());
                }
                if (dr["ot"].ToString() != string.Empty)
                {
                    dblOT += Convert.ToDouble(dr["ot"].ToString());
                }
                if (dr["nt"].ToString() != string.Empty)
                {
                    dblNT += Convert.ToDouble(dr["nt"].ToString());
                }
                if (dr["dt"].ToString() != string.Empty)
                {
                    dblDT += Convert.ToDouble(dr["dt"].ToString());
                }
                if (dr["tt"].ToString() != string.Empty)
                {
                    dblTT += Convert.ToDouble(dr["tt"].ToString());
                }
            }

            Label lblTotalFooter = (Label)gvTickets.FooterRow.FindControl("lblTotalFooter");
            Label lblExpenseFooter = (Label)gvTickets.FooterRow.FindControl("lblExpenseFooter");
            Label lblESTFooter = (Label)gvTickets.FooterRow.FindControl("lblESTFooter");
            Label lblLabExpenseFooter = (Label)gvTickets.FooterRow.FindControl("lblLabExpenseFooter");
            Label lblRTFooter = (Label)gvTickets.FooterRow.FindControl("lblRTFooter");
            Label lblOTFooter = (Label)gvTickets.FooterRow.FindControl("lblOTFooter");
            Label lblNTFooter = (Label)gvTickets.FooterRow.FindControl("lblNTFooter");
            Label lblDTFooter = (Label)gvTickets.FooterRow.FindControl("lblDTFooter");
            Label lblTTFooter = (Label)gvTickets.FooterRow.FindControl("lblTTFooter");

            lblTotalFooter.Text = string.Format("{0:n}", dblBalTotal);
            lblExpenseFooter.Text = string.Format("{0:c}", dblExpenses);
            lblESTFooter.Text = string.Format("{0:n}", dblEST);
            lblLabExpenseFooter.Text = string.Format("{0:c}", dblLabExpenses);
            lblRTFooter.Text = string.Format("{0:n}", dblRT);
            lblOTFooter.Text = string.Format("{0:n}", dblOT);
            lblNTFooter.Text = string.Format("{0:n}", dblNT);
            lblDTFooter.Text = string.Format("{0:n}", dblDT);
            lblTTFooter.Text = string.Format("{0:n}", dblTT);
        }
    }
    private void CalculateItems(DataTable dt)
    {
        double dblActual = 0;
        double dblBudget = 0;
        double dblPercent = 0;

        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["actual"].ToString() != string.Empty)
                {
                    dblActual += Convert.ToDouble(dr["actual"].ToString());
                }
                if (dr["budget"].ToString() != string.Empty)
                {
                    dblBudget += Convert.ToDouble(dr["budget"].ToString());
                }
                if (dr["Percent"].ToString() != string.Empty)
                {
                    dblPercent += Convert.ToDouble(dr["Percent"].ToString());
                }
            }

            TextBox txtTActual = (TextBox)gvBOM.FooterRow.FindControl("txtTActual");
            TextBox txtTBudget = (TextBox)gvBOM.FooterRow.FindControl("txtTBudget");
            TextBox txtTPercent = (TextBox)gvBOM.FooterRow.FindControl("txtTPercent");

            txtTActual.Text = string.Format("{0:n}", dblActual);
            txtTBudget.Text = string.Format("{0:n}", dblBudget);
            txtTPercent.Text = string.Format("{0:n}", dblPercent);
        }
    }
    private void calculateInvoice(DataTable dt)
    {
        lblCountInvoice.Text = dt.Rows.Count.ToString() + " Record(s) Found.";

        if (dt.Rows.Count > 0)
        {
            Label lblTotalPretaxAmt = (Label)gvInvoice.FooterRow.FindControl("lblTotalPretaxAmt");
            Label lblTotalSalesTax = (Label)gvInvoice.FooterRow.FindControl("lblTotalSalesTax");
            Label lblTotalInvoice = (Label)gvInvoice.FooterRow.FindControl("InvTotalInvoice");
            Label lblTotalDue = (Label)gvInvoice.FooterRow.FindControl("InvTotalDue");

            double TotalPretaxAmt = 0;
            double TotalSalesTax = 0;
            double TotalInvoice = 0;
            double TotalDue = 0;

            double PretaxAmt = 0;
            double SalesTax = 0;
            double Invoice = 0;
            double Due = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["amount"] != DBNull.Value && dr["amount"].ToString() != "")
                {
                    PretaxAmt = Convert.ToDouble(dr["amount"]);
                }

                if (dr["stax"] != DBNull.Value && dr["stax"].ToString() != "")
                {
                    SalesTax = Convert.ToDouble(dr["stax"]);
                }

                if (dr["total"] != DBNull.Value && dr["total"].ToString() != "")
                {
                    Invoice = Convert.ToDouble(dr["total"]);
                }

                Due = Convert.ToDouble(dr["balance"]);

                TotalPretaxAmt += PretaxAmt;
                TotalSalesTax += SalesTax;
                TotalInvoice += Invoice;
                TotalDue += Due;
            }

            lblTotalPretaxAmt.Text = string.Format("{0:c}", TotalPretaxAmt);
            lblTotalSalesTax.Text = string.Format("{0:c}", TotalSalesTax);
            lblTotalInvoice.Text = string.Format("{0:c}", TotalInvoice);
            lblTotalDue.Text = string.Format("{0:c}", TotalDue);
        }
    }
    #endregion

    #region Finance

    #region Budget
    private void BindBudget()
    {
        try
        {
            DataSet ds = new DataSet();
            objJob.Job = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Job.GetJobCostByJob(objJob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvBudget.DataSource = ds.Tables[0];
                gvBudget.DataBind();
            }
            ViewState["budget"] = ds.Tables[0];
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvBudget_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                GridView gv = (GridView)e.Row.FindControl("gvChildGrid");
                HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
                objJob.Type = Convert.ToInt16(hdnType.Value);
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);

                DataSet ds = new DataSet();
                ds = objBL_Job.GetJobCostCodeByJob(objJob);

                gv.DataSource = ds.Tables[0];
                gv.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvChildGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                GridView gv = (GridView)e.Row.FindControl("gvChildItemGrid");
                HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
                Label lblOpSeq = (Label)e.Row.FindControl("lblOpSeq");
                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                objJob.Type = Convert.ToInt16(hdnType.Value);
                objJob.Code = lblOpSeq.Text;

                DataSet ds = new DataSet();
                ds = objBL_Job.GetJobCostTypeByJob(objJob);

                gv.DataSource = ds.Tables[0];
                gv.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void gvChildItemGrid_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                GridView gv = (GridView)e.Row.FindControl("gvInnerChildItemGrid");
                GridView gvInnerTicket = (GridView)e.Row.FindControl("gvInnerChildTicket");

                HiddenField hdnType = (HiddenField)e.Row.FindControl("hdnType");
                Label lblOpSeq = (Label)e.Row.FindControl("lblOpSeq");
                HiddenField hdnItemType = (HiddenField)e.Row.FindControl("hdnItemType");

                objJob.Job = Convert.ToInt32(Request.QueryString["uid"]);
                objJob.Type = Convert.ToInt16(hdnType.Value);
                objJob.Code = lblOpSeq.Text;
                objJob.TypeId = Convert.ToInt16(hdnItemType.Value);

                DataSet ds = new DataSet();
                ds = objBL_Job.GetJobCostInvoicesByJob(objJob);
                
                if (objJob.Type.Equals(1) && objJob.TypeId.Equals(2))
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        gv.DataSource = ds.Tables[0];
                        gv.DataBind();

                        if(ds.Tables[0].Rows.Count > 0)
                        {
                            Label lblTotalAmount = (Label)gv.FooterRow.FindControl("lblTotalAmount");
                            Label lblTotalBudgetAmt = (Label)gv.FooterRow.FindControl("lblTotalBudgetAmt");
                            Label lblTotalActualAmt = (Label)gv.FooterRow.FindControl("lblTotalActualAmt");

                            lblTotalAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Amount)", string.Empty));
                            lblTotalBudgetAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Budget)", string.Empty));
                            lblTotalActualAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Actual)", string.Empty));
                        }
                    }
                
                    DataSet dsTicket = new DataSet();
                    dsTicket = objBL_Job.GetJobCostTicketsByJob(objJob);
                    if (dsTicket.Tables[0].Rows.Count > 0)
                    {
                        gvInnerTicket.DataSource = dsTicket.Tables[0];
                        gvInnerTicket.DataBind();

                        if(dsTicket.Tables[0].Rows.Count > 0)
                        {
                            DataTable dtTicket = dsTicket.Tables[0];
                            Label lblTotalBudgetHr = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalBudgetHr");
                            Label lblTotalActualHr = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalActualHr");
                            Label lblTotalHourlyWage = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalHourlyWage");
                            Label lblTotalActualCost = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalActualCost");
                            Label lblTotalLaborExp = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalLaborExp");
                            Label lblTotalOtherExp = (Label)gvInnerTicket.FooterRow.FindControl("lblTotalOtherExp");

                            lblTotalBudgetHr.Text = string.Format("{0:n}", dtTicket.Compute("SUM(Est)", string.Empty));
                            lblTotalActualHr.Text = string.Format("{0:n}", dtTicket.Compute("SUM(ActualHr)", string.Empty));
                            lblTotalHourlyWage.Text = string.Format("{0:c}", dtTicket.Compute("SUM(HourlyWage)", string.Empty));
                            lblTotalActualCost.Text = string.Format("{0:c}", dtTicket.Compute("SUM(ActualCostIncur)", string.Empty));
                            lblTotalLaborExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(LaborExp)", string.Empty));
                            lblTotalOtherExp.Text = string.Format("{0:c}", dtTicket.Compute("SUM(Expenses)", string.Empty));
                        }
                    }
                }
                else
                {
                    gv.DataSource = ds.Tables[0];
                    gv.DataBind();

                    if(ds.Tables[0].Rows.Count > 0)
                    {
                        Label lblTotalAmount = (Label)gv.FooterRow.FindControl("lblTotalAmount");
                        Label lblTotalBudgetAmt = (Label)gv.FooterRow.FindControl("lblTotalBudgetAmt");
                        Label lblTotalActualAmt = (Label)gv.FooterRow.FindControl("lblTotalActualAmt");

                        lblTotalAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Amount)", string.Empty));
                        lblTotalBudgetAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Budget)", string.Empty));
                        lblTotalActualAmt.Text = string.Format("{0:c}", ds.Tables[0].Compute("SUM(Actual)", string.Empty));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrProj", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    #endregion

    private void GetJobTemplate(int tid)
    {
        try
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = tid;
            DataSet ds = objBL_Job.GetJobTFinanceByID(objJob);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow _dr = ds.Tables[0].Rows[0];
                hdnInvServiceID.Value = _dr["InvServ"].ToString();
                txtInvService.Text = _dr["InvServiceName"].ToString();
                hdnPrevilWageID.Value = _dr["Wage"].ToString();
                txtPrevilWage.Text = _dr["WageName"].ToString();
                uc_InterestGL._txtGLAcct.Text = _dr["GLName"].ToString();
                uc_InterestGL._hdnAcctID.Value = _dr["GLInt"].ToString();
                uc_InvExpGL._txtGLAcct.Text = _dr["InvExpName"].ToString();
                uc_InvExpGL._hdnAcctID.Value = _dr["InvExp"].ToString();
                if (!string.IsNullOrEmpty(_dr["CType"].ToString()))
                {
                    ddlContractType1.SelectedValue = _dr["CType"].ToString();
                }
                if (!string.IsNullOrEmpty(_dr["Post"].ToString()))
                {
                    ddlPostingMethod.SelectedValue = _dr["Post"].ToString();
                }
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
            objJob.ConnConfig = Session["config"].ToString();
            _dsPost = objBL_Job.GetPosting(objJob);
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
                ddlPostingMethod.Items.Add(new ListItem("No data found", "0"));
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void DisableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = false;
        uc_InterestGL._txtGLAcct.Enabled = false;
        txtInvService.Enabled = false;
        txtPrevilWage.Enabled = false;
        ddlPostingMethod.Enabled = false;
        ddlContractType1.Enabled = false;
        chkChargeInt.Enabled = false;
        chkInvoicing.Enabled = false;
        chkChargeable.Enabled = false;
    }
    private void ClearTempControl()     //clear project template field's controls. Finance > General
    {
        uc_InvExpGL._txtGLAcct.Text = "";
        uc_InterestGL._txtGLAcct.Text = "";
        txtInvService.Text = "";
        txtPrevilWage.Text = "";
        ddlPostingMethod.SelectedIndex = 0;
        ddlContractType1.SelectedIndex = 0;
        chkChargeInt.Checked = false;
        chkInvoicing.Checked = false;
        chkChargeable.Checked = false;
    }
    private void EnableControl()       // Finance > General
    {
        ClearTempControl();

        uc_InvExpGL._txtGLAcct.Enabled = true;
        uc_InterestGL._txtGLAcct.Enabled = true;
        txtInvService.Enabled = true;
        txtPrevilWage.Enabled = true;
        ddlPostingMethod.Enabled = true;
        ddlContractType1.Enabled = true;
        chkChargeInt.Enabled = true;
        chkInvoicing.Enabled = true;
        chkChargeable.Enabled = true;
    }
    #endregion

    #endregion

    #region Custom Field

    protected override void OnInit(EventArgs e)       // comment 8.16.16
    {
        base.OnInit(e);
        // dt = (DataTable)ViewState["CustomFields"];
    }
    protected override object SaveViewState()
    {
        TrackViewState();
        object viewState = base.SaveViewState();
        return viewState;
    }
    protected override void LoadViewState(object savedState)
    {
        TrackViewState();
        base.LoadViewState(savedState);

        if (!ddlTemplate.SelectedValue.Equals("0") && !ddlTemplate.SelectedValue.Equals(""))      // comment 8.16.16
        {
            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = Convert.ToInt32(ddlTemplate.SelectedValue);

            DataSet ds = new DataSet();
            ds = objBL_Job.GetProjectTemplateCustomFields(objJob);
            DisplayCustomByTab(ds.Tables[0], ds.Tables[1], objJob.ID);
        }
    }
    private void CreateCustomTable()
    {
        dtCustomField = new DataTable();
        dtCustomField.Columns.Add("ID", typeof(int));
        dtCustomField.Columns.Add("tblTabID", typeof(int));
        dtCustomField.Columns.Add("Label", typeof(string));
        dtCustomField.Columns.Add("Line", typeof(Int16));
        dtCustomField.Columns.Add("Value", typeof(string));
        dtCustomField.Columns.Add("Format", typeof(Int16));
        dtCustomField.Columns.Add("ControlID", typeof(string));
        dtCustomField.Columns.Add("UpdatedDate", typeof(DateTime));
        dtCustomField.Columns.Add("Username", typeof(string));
    }
    private string DisplayCustomField(DataTable dtCust, DataTable dtValue, PlaceHolder tbContainer)
    {
        try
        {
            StringBuilder html = new StringBuilder();
            string varControl = "";
            DataTable dtItem = new DataTable();
            DataTable dtc = dtCust;
            DataTable dtCustomValue = dtValue;

            int rowCount = dtc.Rows.Count;
            int rowcount1 = rowCount / 2;
            int count = 0;

            if (ViewState["TabId"].ToString().Equals("1"))
            {
                html.Append("   <div class='col-md-6 col-lg-6'> ");
            }
            else
            {
                html.Append("   <div class='col-md-6 col-lg-6'> ");
            }
            int i = 0;
            foreach (DataRow drc in dtc.Rows)
            {
                if (rowcount1.Equals(count))
                {
                    html.Append("   </div>                          \n");
                    if (ViewState["TabId"].ToString().Equals("1"))
                    {
                        html.Append("   <div class='col-md-4 col-lg-4'> \n");
                    }
                    else
                    {
                        html.Append("   <div class='col-md-6 col-lg-6'> \n");
                    }
                }

                string ctrlValue = drc["Value"].ToString();
                string ctrlName = drc["Line"].ToString();
                html.Append(" <div class='form-group'>      \n");
                html.Append("   <div class='form-col'>      \n");
                html.Append("      <div class='fc-label'>   \n");
                html.Append(drc["Label"].ToString());
                html.Append("      </div> \n");
                html.Append("      <div class='fc-input'>   \n");

                StringWriter sw = new StringWriter(html);
                HtmlTextWriter writer = new HtmlTextWriter(sw);

                CustomTextBox txt = new CustomTextBox();

                #region append with control

                switch (Convert.ToInt16(drc["Format"]))
                {
                    case 1:                                 ////////////////////////// Currency

                        txt = new CustomTextBox();
                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom currency";
                        txt.MaxLength = 14;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        break;

                    case 2:                                 ////////////////////////// Date
                        txt = new CustomTextBox();

                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom date-picker";
                        txt.MaxLength = 12;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        //CalendarExtender ce = new CalendarExtender();
                        //ce.Enabled = true;
                        //ce.TargetControlID = txt.ID;
                        //ce.ID = varControl + "_CalendarExtender";

                        #region comment
                        //writer = new HtmlTextWriter(sw);
                        //sw = new StringWriter(html);

                        //uc_Datepicker ucDate = (uc_Datepicker)LoadControl("uc_Datepicker.ascx");

                        //ucDate.ID = "txt" + ctrlName;
                        //ucDate._txtDate.Text = ctrlValue;
                        //ucDate.RenderControl(writer);

                        #endregion

                        break;

                    case 3:                                 ////////////////////////// Text

                        txt = new CustomTextBox();
                        txt.ID = "txt" + ctrlName;
                        txt.Text = ctrlValue;
                        txt.CssClass = "form-control custom";
                        txt.MaxLength = 50;
                        varControl = "txt" + ctrlName;

                        txt.RenderControl(writer);

                        break;

                    case 4:                                 ////////////////////////// Dropdown

                        CustomDropDownList ddl = new CustomDropDownList();
                        ddl.ID = "ddl" + ctrlName;
                        ddl.SelectedValue = drc["Value"].ToString();
                        ddl.CssClass = "form-control custom";
                        if (!string.IsNullOrEmpty(drc["Value"].ToString()))
                        {
                            ddl.SelectedValue = drc["Value"].ToString();
                        }
                        dtItem = new DataTable();
                        var rows = dtCustomValue.AsEnumerable().Where(x => ((Int16)x["Line"]) == Convert.ToInt16(drc["Line"]));
                        if (rows.Any())
                            dtItem = rows.CopyToDataTable();
                        //dtItem = dtCustomValue.Select("Line="+drc["Format"].ToString()).CopyToDataTable();
                        foreach (DataRow drItem in dtItem.Rows)
                        {
                            ddl.Items.Add(drItem["Value"].ToString());
                        }
                        varControl = "ddl" + ctrlName;

                        ddl.RenderControl(writer);

                        break;

                    case 5:                                 ////////////////////////// Checkbox
                        CustomCheckBox chk = new CustomCheckBox();
                        chk.CssClass = "custom";
                        chk.ID = "chk" + ctrlName;

                        if (!string.IsNullOrEmpty(drc["Value"].ToString()))
                        {
                            if (drc["Value"].ToString().ToLower().Equals("true"))
                            {
                                chk.Checked = true;
                            }
                            else if (drc["Value"].ToString().ToLower().Equals("on"))
                            {
                                chk.Checked = true;
                            }
                        }
                        varControl = "chk" + ctrlName;

                        chk.RenderControl(writer);

                        if (drc["Value"].ToString().ToLower().Equals("true"))
                        {
                            if (drc["UpdatedDate"] != DBNull.Value && drc["Username"] != DBNull.Value)
                            {
                                string username = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; " + Convert.ToDateTime(drc["UpdatedDate"]).ToString("MM/dd/yy hh:mm tt") + " " + drc["Username"].ToString();
                                Label lbl = new Label();
                                lbl.ID = "custom";
                                lbl.ID = "lbl" + ctrlName;
                                lbl.Text = username;

                                sw = new StringWriter(html);
                                writer = new HtmlTextWriter(sw);
                                lbl.RenderControl(writer);
                            }
                        }
                        break;

                }

                DataRow dr = dtCustomField.NewRow();
                dr["ID"] = Convert.ToInt32(drc["ID"]);
                dr["tblTabID"] = Convert.ToInt32(drc["tblTabID"]);
                dr["Line"] = Convert.ToInt16(drc["Line"]);
                dr["Label"] = drc["Label"].ToString();
                dr["Value"] = drc["Value"].ToString();
                dr["Format"] = Convert.ToInt16(drc["Format"]);
                dr["ControlID"] = varControl;

                dtCustomField.Rows.Add(dr);

                #endregion

                html.Append("      </div>                        \n");
                html.Append("   </div>                           \n");
                html.Append(" </div>                             \n");

                count++;
                i++;
            }
            html.Append("   </div>                               \n");

            ViewState["CustomFields"] = dtCust;
            return html.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void DisplayCustomByTab(DataTable dtCustom, DataTable dtVal, int jobId)
    {
        try
        {
            DataTable dtCustomTab = new DataTable();
            DataTable dtItem = new DataTable();
            //fetch all custom field from tblCustomTab and tblCustom
            DataTable dtCust = new DataTable();
            DataTable dtValue = new DataTable();

            DataTable dtc = dtCustom;
            DataTable dtv = dtVal;
            objJob.ConnConfig = Session["config"].ToString();
            objJob.PageUrl = "addproject.aspx";

            DataSet _ds = objBL_Job.GetTabByPageUrl(objJob);

            objJob.ConnConfig = Session["config"].ToString();
            objJob.ID = jobId;
            DataSet dsTab = objBL_Job.GetProjectCustomTab(objJob);
            string html = string.Empty;
            ViewState["TabId"] = 0;
            foreach (DataRow drCus in dsTab.Tables[0].Rows)
            {
                ViewState["TabId"] = 0;
                html = string.Empty;
                switch (drCus["tblTabID"].ToString())
                {
                    case "1":                           //  Header

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 1);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }
                        ViewState["TabId"] = 1;

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderHeader);


                        PlaceHolderHeader.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "2":                           //  Attributes - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 2);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttrGeneral);
                        PlaceHolderAttrGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "3":                           //  Attributes - GC Info

                        dtCust = new DataTable();
                        dtValue = new DataTable();

                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 3);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriGC);
                        PlaceHolderAttriGC.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "4":                           //  Attributes - Equipment

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 4);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderAttriEquip);
                        PlaceHolderAttriEquip.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "5":                           //  Finance - General

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 5);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceGeneral);
                        PlaceHolderFinceGeneral.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "6":                           //  Finance - Billing

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 6);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBill);
                        PlaceHolderFinceBill.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "7":                           //  Finance - Budgets

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 7);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderFinceBudget);
                        PlaceHolderFinceBudget.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "8":                           //  Ticketlist

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();

                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 8);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderTicket);
                        PlaceHolderTicket.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "9":                           //  BOM

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 9);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }

                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderBOM);
                        PlaceHolderBOM.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                    case "10":                           //  Milestones

                        dtCust = new DataTable();
                        dtValue = new DataTable();
                        if (dtCustom.Rows.Count > 0)
                        {
                            var rowsdt = dtCustom.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                            if (rowsdt.Any())
                            {
                                dtCust = rowsdt.CopyToDataTable();
                                var rows = dtVal.AsEnumerable().Where(x => ((int)x["tblTabID"]) == 10);
                                if (rows.Any())
                                {
                                    dtValue = rows.CopyToDataTable();
                                }
                            }
                        }

                        html = DisplayCustomField(dtCust, dtValue, PlaceHolderMilestone);
                        PlaceHolderMilestone.Controls.Add(new Literal { Text = html.ToString() });

                        break;
                }
            }
            ViewState["CustomFields"] = dtCustomField;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DataTable GetCustomItems()
    {
        string strItems = hdnCustomJSON.Value;
        DataTable dt = (DataTable)ViewState["CustomFields"];
        try
        {
            if (strItems != string.Empty)
            {
                JavaScriptSerializer sr = new JavaScriptSerializer();
                List<Dictionary<object, object>> objItemData = new List<Dictionary<object, object>>();
                objItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
                if (objItemData.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        var customDs = objItemData.Where(y => y.Keys.Contains(dr["ControlID"].ToString())).FirstOrDefault();
                        if (customDs != null)
                        {
                            if (customDs.Count > 0)
                            {
                                dr["Value"] = customDs.Values.First();
                            }
                        }
                    }

                    //dt.Columns.Remove("ControlID");
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

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index + 1]["id"], false);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addproject.aspx?uid=" + dt.Rows[index - 1]["id"], false);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        Response.Redirect("addproject.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["id"], false);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["projids"];
        Response.Redirect("addproject.aspx?uid=" + dt.Rows[0]["id"], false);
    }
}

