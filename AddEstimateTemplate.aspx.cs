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

public partial class AddEstimateTemplate : System.Web.UI.Page
{
    Customer objProp_Customer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            ViewState["edit"] = 0;
            FillBucketDropdown();
            CreateTable();
            //BindGrid();
            GetTemplate();
            Permission();
        }
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.Master.FindControl("SalesLink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.Master.FindControl("lnkEstimateTempl");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.Master.FindControl("HoverMenuExtenderSales");
        //hm.Enabled = false;
        HtmlGenericControl ul = (HtmlGenericControl)Page.Master.Master.FindControl("SalesMgrSub");
        //ul.Attributes.Remove("class");
        ul.Style.Add("display", "block");     
    }
    private void GetTemplate()
    {
        if (Request.QueryString["uid"] != null)
        {
            Label13.Text = "Edit Estimate Template";

            ViewState["edit"] = 1;
            DataSet ds = new DataSet();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
            ds = objBL_Customer.getEstimateTemplateByID(objProp_Customer);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtREPdesc.Text = ds.Tables[0].Rows[0]["fdesc"].ToString();
                txtREPremarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();

                DataTable dtitems = ds.Tables[1].Copy();

                objProp_Customer.ConnConfig = Session["config"].ToString();
                DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
                foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                {
                    dtitems.Columns.Add(drLabor["item"].ToString(), typeof(double));
                }

                foreach (DataRow drl in dtitems.Rows)
                {
                    foreach (DataRow drldata in ds.Tables[2].Rows)
                    {
                        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                        {
                            if (Convert.ToInt32(drLabor["ID"].ToString()) == Convert.ToInt32(drldata["labourID"].ToString()) && Convert.ToUInt32(drl["line"].ToString()) == Convert.ToUInt32(drldata["line"].ToString()))
                                drl[drLabor["item"].ToString()] = drldata["amount"];
                        }
                    }
                }

                //DataRow[] drselect = dtitems.Select("measure <> 3");
                //DataRow[] drselectPercent = dtitems.Select("measure = 3");

                var drselect = from myRow in dtitems.AsEnumerable()
                               where myRow.Field<short>("Measure") != 3
                               select myRow;
                DataTable dtselect = new DataTable();
                if (drselect.Count() > 0)
                    dtselect = drselect.CopyToDataTable<DataRow>();

                var drselectPercent = from myRow in dtitems.AsEnumerable()
                                      where myRow.Field<short>("Measure") == 3
                                      select myRow;

                DataTable dtselectPercent = new DataTable();
                if (drselectPercent.Count() > 0)
                    dtselectPercent = drselectPercent.CopyToDataTable<DataRow>();

                gvTemplateItems.DataSource = dtselect;
                gvTemplateItems.DataBind();

                gvTemplateItemsPercentage.DataSource = dtselectPercent;
                gvTemplateItemsPercentage.DataBind();

            }
        }
    }

    private void CreateTable()
    {
        //Session["esttempltable"] = null;

        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("cost", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("amount", typeof(double));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("measure", typeof(int));

        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        {
            dt.Columns.Add(drLabor["item"].ToString(), typeof(double));
        }

        DataRow dr = dt.NewRow();
        dr["Scope"] = DBNull.Value;
        dr["vendor"] = DBNull.Value;
        //dr["measure"] = 1;
        //dr["Quantity"] = 0;
        //dr["cost"] = 0;
        dr["currency"] = DBNull.Value;
        //dr["amount"] = 0;
        //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //{
        //    dr[drLabor["item"].ToString()] = 0;
        //}
        dt.Rows.Add(dr);

        DataRow dr1 = dt.NewRow();
        dr1["Scope"] = DBNull.Value;
        dr1["vendor"] = DBNull.Value;
        //dr1["measure"] = 1;
        //dr1["Quantity"] = 0;
        //dr1["cost"] = 0;
        dr1["currency"] = DBNull.Value;
        //dr1["amount"] = 0;
        //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        //{
        //    dr1[drLabor["item"].ToString()] = 0;
        //}
        dt.Rows.Add(dr1);

        //Session["esttempltable"] = dt;
        //return dt;
        BindGrid(dt);
        BindGridPerc(dt);
    }

    private void BindGrid(DataTable dt)
    {
        gvTemplateItems.DataSource = dt;
        gvTemplateItems.DataBind();

        AddColumns();
        gvTemplateItems.DataBind();
    }

    private void BindGridPerc(DataTable dt)
    {
        gvTemplateItemsPercentage.DataSource = dt;
        gvTemplateItemsPercentage.DataBind();

        //AddColumns();
        //gvTemplateItemsPercentage.DataBind();
    }

    private void AddColumns()
    {
        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

        if (!IsPostBack)
        {
            foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
            {
                TemplateField temp1 = new TemplateField();
                temp1.HeaderText = drLabor["item"].ToString() + ":" + drLabor["amount"].ToString();

                DynamicTemplateField dyntmp = new DynamicTemplateField(drLabor["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
                temp1.ItemTemplate = dyntmp;
                DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(drLabor["item"].ToString(), drLabor["amount"].ToString(), Convert.ToInt32(drLabor["ID"].ToString()));
                temp1.FooterTemplate = dyntmpfooter;
                gvTemplateItems.Columns.Add(temp1);
                //gvTemplateItemsPercentage.Columns.Add(temp1);
            }
        }
        else
        {
            List<TemplateField> strcol = new List<TemplateField>();
            List<DataRow> rowlab = new List<DataRow>();
            List<DataRow> rowlabgrid = new List<DataRow>();
            foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
            {
                rowlab.Add(drLabor);
                int dtfields = 0;
                foreach (TemplateField tf in gvTemplateItems.Columns)
                {
                    if (dtfields > 9)
                    {
                        if (tf.HeaderText.Split(':').ToString() == drLabor["item"].ToString())//+ " " + drLabor["amount"].ToString())
                        {
                            tf.HeaderText = drLabor["item"].ToString() + ":" + drLabor["amount"].ToString();
                            DynamicTemplateField dyntmp = new DynamicTemplateField(drLabor["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
                            tf.ItemTemplate = dyntmp;
                            DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(drLabor["item"].ToString(), drLabor["amount"].ToString(), Convert.ToInt32(drLabor["ID"].ToString()));
                            tf.FooterTemplate = dyntmpfooter;

                            strcol.Add(tf);
                            rowlabgrid.Add(drLabor);
                        }
                    }
                    dtfields++;
                }

                //dtfields = 0;
                //foreach (TemplateField tf in gvTemplateItemsPercentage.Columns)
                //{
                //    if (dtfields > 9)
                //    {
                //        if (tf.HeaderText == drLabor["item"].ToString() + " " + drLabor["amount"].ToString())
                //        {
                //            DynamicTemplateField dyntmp = new DynamicTemplateField(drLabor["item"].ToString());
                //            tf.ItemTemplate = dyntmp;
                //            DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(drLabor["item"].ToString(), drLabor["amount"].ToString());
                //            tf.FooterTemplate = dyntmpfooter;

                //            //strcol.Add(tf);
                //            //rowlabgrid.Add(drLabor);
                //        }
                //    }
                //    dtfields++;
                //}
            }

            int dtfields1 = 0;
            List<TemplateField> strcol1 = new List<TemplateField>();
            foreach (TemplateField tf in gvTemplateItems.Columns)
            {
                if (dtfields1 > 9)
                {
                    strcol1.Add(tf);
                }
                dtfields1++;
            }

            foreach (TemplateField s in strcol)
            {
                strcol1.Remove(s);
            }

            foreach (TemplateField s in strcol1)
            {
                //gvTemplateItems.Columns.Remove(s);
            }

            foreach (DataRow d in rowlabgrid)
            {
                rowlab.Remove(d);
            }
            foreach (DataRow d in rowlab)
            {
                TemplateField temp1 = new TemplateField();
                temp1.HeaderText = d["item"].ToString() + ":" + d["amount"].ToString();

                DynamicTemplateField dyntmp = new DynamicTemplateField(d["item"].ToString(), gvTemplateItems.ClientID, gvTemplateItemsPercentage.ClientID);
                temp1.ItemTemplate = dyntmp;
                DynamicTemplateFieldFooter dyntmpfooter = new DynamicTemplateFieldFooter(d["item"].ToString(), d["amount"].ToString(), Convert.ToInt32(d["ID"].ToString()));
                temp1.FooterTemplate = dyntmpfooter;
                gvTemplateItems.Columns.Add(temp1);
                //gvTemplateItemsPercentage.Columns.Add(temp1);
            }
        }
    }

    //private void AddNewRow()
    //{
    //    //GridData();
    //    ViewState["first"] = 2;
    //    //DataTable dt = new DataTable();
    //    //dt = (DataTable)Session["esttempltable"];

    //    //DataRow dr = dt.NewRow();
    //    //dr["Scope"] = DBNull.Value;
    //    //dr["vendor"] = DBNull.Value;
    //    //dr["Quantity"] = 0;
    //    //dr["cost"] = 0;
    //    //dr["currency"] = DBNull.Value;
    //    //dr["amount"] = 0;
    //    //dt.Rows.Add(dr);

    //    //Session["esttempltable"] = dt;

    //    //BindGrid();
    //}
    //protected void lnkAddnewRow_Click(object sender, EventArgs e)
    //{
    //    AddNewRow();
    //}

    //private void DeleteREPItem()
    //{

    //    GridData();

    //    DataTable dt = new DataTable();
    //    dt = (DataTable)Session["esttempltable"];

    //    int count = 0;
    //    foreach (GridViewRow gr in gvTemplateItems.Rows)
    //    {
    //        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //        Label lblIndex = (Label)gr.FindControl("lblIndex");
    //        int index = Convert.ToInt32(lblIndex.Text) - 1;

    //        if (chkSelect.Checked == true)
    //        {
    //            dt.Rows.RemoveAt(index - count);
    //            count++;
    //        }
    //    }

    //    if (dt.Rows.Count == 0)
    //    {
    //        DataRow dr = dt.NewRow();
    //        dr["Code"] = DBNull.Value;
    //        dr["EquipT"] = DBNull.Value;
    //        dr["Elev"] = 0;
    //        dr["fDesc"] = DBNull.Value;
    //        dr["Line"] = dt.Rows.Count;
    //        dr["Lastdate"] = DBNull.Value;
    //        dr["NextDateDue"] = DBNull.Value;
    //        dr["Frequency"] = -1;
    //        dt.Rows.Add(dr);
    //    }

    //    Session["esttempltable"] = dt;
    //    BindGrid();
    //}

    //private void GridData()
    //{
    //    DataTable dt = (DataTable)Session["esttempltable"];

    //    DataTable dtDetails = dt.Clone();

    //    foreach (GridViewRow gr in gvTemplateItems.Rows)
    //    {
    //        Label lblIndex = (Label)gr.FindControl("lblIndex");
    //        TextBox txtScope = (TextBox)gr.FindControl("txtScope");
    //        TextBox txtvendor = (TextBox)gr.FindControl("txtvendor");
    //        TextBox txtQuan = (TextBox)gr.FindControl("txtQuan");
    //        TextBox txtUnitCost = (TextBox)gr.FindControl("txtUnitCost");
    //        DropDownList ddlCurrency = (DropDownList)gr.FindControl("ddlCurrency");
    //        TextBox txtTotal = (TextBox)gr.FindControl("txtTotal");

    //        objProp_Customer.ConnConfig = Session["config"].ToString();
    //        DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

    //        DataRow dr = dtDetails.NewRow();
    //        dr["Scope"] = txtScope.Text.Trim();
    //        dr["vendor"] = txtvendor.Text.Trim();
    //        dr["Quantity"] = Convert.ToDouble(txtQuan.Text.Trim());
    //        dr["cost"] = Convert.ToDouble(txtUnitCost.Text.Trim());
    //        dr["currency"] = ddlCurrency.SelectedValue;
    //        dr["amount"] = Convert.ToDouble(txtTotal.Text.Trim());

    //        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
    //        {
    //            string strCol = drLabor["item"].ToString();
    //            TextBox txtitem1 = (TextBox)gr.FindControl("txt" + strCol);

    //            if (txtitem1.Text.Trim() != string.Empty)
    //                dr[strCol] = Convert.ToDouble(txtitem1.Text.Trim());
    //        }
    //        dtDetails.Rows.Add(dr);
    //    }
    //    Session["esttempltable"] = dtDetails;
    //}
    //protected void Page_Init(object sender, EventArgs e)
    //{
    //    if (IsPostBack)
    //    {
    //        if (ViewState["first"].ToString() == "2")
    //        {
    //            GridData();
    //            DataTable dt = new DataTable();
    //            dt = (DataTable)Session["esttempltable"];

    //            DataRow dr = dt.NewRow();
    //            dr["Scope"] = DBNull.Value;
    //            dr["vendor"] = DBNull.Value;
    //            dr["Quantity"] = 0;
    //            dr["cost"] = 0;
    //            dr["currency"] = DBNull.Value;
    //            dr["amount"] = 0;
    //            dt.Rows.Add(dr);

    //            Session["esttempltable"] = dt;

    //            BindGrid();
    //        }
    //    }

    //}

    protected void lnkSaveTemplate_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.Name = txtName.Text.Trim();
            objProp_Customer.Description = txtREPdesc.Text.Trim();
            objProp_Customer.Remarks = txtREPremarks.Text.Trim();
            DataSet ds = GetItemsfromGrid();
            objProp_Customer.dtLaborItems = ds.Tables[1];
            objProp_Customer.dtItems = ds.Tables[0];
            if (ViewState["edit"].ToString() == "0")
            {
                objProp_Customer.Mode = 0;
                objBL_Customer.AddEstimateTemplate(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Template Added Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                IntializeControls();
            }
            else if (ViewState["edit"].ToString() == "1")
            {
                objProp_Customer.Mode = 1;
                objProp_Customer.TemplateID = Convert.ToInt32(Request.QueryString["uid"].ToString());
                objBL_Customer.AddEstimateTemplate(objProp_Customer);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keysuccess", "noty({text: 'Template Updated Successfully!', dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                DataSet dsGrid = RestoreGrid();
                BindGrid(dsGrid.Tables[0]);
                if (dsGrid.Tables.Count > 1)
                    BindGridPerc(dsGrid.Tables[1]);
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrDelProspect", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter', theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void lnkCloseTemplate_Click(object sender, EventArgs e)
    {
        Response.Redirect("estimatetemplate.aspx");
    }

    private void IntializeControls()
    {
        GeneralFunctions objgn = new GeneralFunctions();
        objgn.ResetFormControlValues(this);
        CreateTable();
        //BindGrid();
    }

    private DataSet GetItemsfromGrid()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        dt.Columns.Add("Estimate", typeof(int));
        dt.Columns.Add("Line", typeof(int));
        dt.Columns.Add("fDesc", typeof(string));
        dt.Columns.Add("Quan", typeof(double));
        dt.Columns.Add("Cost", typeof(double));
        dt.Columns.Add("Price", typeof(double));
        dt.Columns.Add("Hours", typeof(double));
        dt.Columns.Add("Rate", typeof(double));
        dt.Columns.Add("Labor", typeof(double));
        dt.Columns.Add("Amount", typeof(double));
        dt.Columns.Add("STax", typeof(int));
        dt.Columns.Add("Code", typeof(string));
        dt.Columns.Add("Vendor", typeof(string));
        dt.Columns.Add("Currency", typeof(string));

        dt.Columns.Add("measure", typeof(int));

        DataTable dtLaborItems = new DataTable();
        dtLaborItems.Columns.Add("Line", typeof(int));
        dtLaborItems.Columns.Add("LabourID", typeof(int));
        dtLaborItems.Columns.Add("TemplateID", typeof(int));
        dtLaborItems.Columns.Add("Amount", typeof(double));


        DataSet ds1 = JSONToDatatableGridItems(hdnItemJSON.Value.Trim(), dt, dtLaborItems, 0, true);
        DataSet ds2 = JSONToDatatableGridItems(hdnItemJSONPerc.Value.Trim(), dt, dtLaborItems, ds1.Tables[0].Rows.Count, false);

        ds1.Tables[0].Merge(ds2.Tables[0]);
        ds1.Tables[1].Merge(ds2.Tables[1]);

        dt = ds1.Tables[0].Copy();
        dtLaborItems = ds1.Tables[1].Copy();

        ds.Tables.Add(dt);
        ds.Tables.Add(dtLaborItems);

        return ds;
    }

    private DataSet JSONToDatatableGridItems(string strItems, DataTable dtnew, DataTable dtLaborItemsNew, int i, bool includeLaborItems)
    {
        DataTable dt = dtnew.Copy();
        DataTable dtLaborItems = dtLaborItemsNew.Copy();

        DataSet ds = new DataSet();
        //string strItems = hdnItemJSON.Value.Trim();
        if (strItems != string.Empty)
        {
            objProp_Customer.ConnConfig = Session["config"].ToString();
            DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

            JavaScriptSerializer sr = new JavaScriptSerializer();
            List<Dictionary<object, object>> objEstimateItemData = new List<Dictionary<object, object>>();
            objEstimateItemData = sr.Deserialize<List<Dictionary<object, object>>>(strItems);
            //List<EstimateItemData> objEstimateItemData = new List<EstimateItemData>();
            //objEstimateItemData = sr.Deserialize<List<EstimateItemData>>(strItems);
            //int i = 0;
            //foreach (EstimateItemData eid in objEstimateItemData)
            foreach (Dictionary<object, object> dict in objEstimateItemData)
            {
                if (dict["txtScope"].ToString().Trim() != string.Empty)// || dict["txtUnitCost"].ToString().Trim() == string.Empty|| dict["txtQuan"].ToString().Trim() == string.Empty ||  dict["txtTotal"].ToString().Trim() == string.Empty
                {
                    i++;
                    DataRow dr = dt.NewRow();
                    dr["Estimate"] = 0;
                    dr["Line"] = i;
                    dr["fDesc"] = dict["txtScope"].ToString().Trim();

                    if (dict["txtQuan"].ToString().Trim() != string.Empty)
                        dr["Quan"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                    if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                        dr["Cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Price"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["Hours"] = 0;
                    dr["Rate"] = 0;
                    dr["Labor"] = 0;

                    if (dict["txtTotal"].ToString().Trim() != string.Empty)
                        dr["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                    dr["STax"] = 0;
                    dr["code"] = string.Empty;
                    dr["Vendor"] = dict["txtVendor"].ToString().Trim();
                    dr["Currency"] = dict["ddlCurrency"].ToString().Trim();
                    dr["code"] = dict["txtCode"];
                    if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                        dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                    dt.Rows.Add(dr);

                    if (includeLaborItems)
                    {
                        foreach (DataRow drlab in dsLabor.Tables[0].Rows)
                        {
                            if (dict["txt" + drlab["Item"]].ToString().Trim() != string.Empty)
                            {
                                DataRow drLaborItems = dtLaborItems.NewRow();
                                drLaborItems["Line"] = i;
                                drLaborItems["LabourID"] = drlab["ID"];
                                drLaborItems["Amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drlab["Item"]].ToString(), "0"));
                                dtLaborItems.Rows.Add(drLaborItems);
                            }
                        }
                    }
                }
            }
        }

        ds.Tables.Add(dt);
        ds.Tables.Add(dtLaborItems);

        return ds;
    }

    private DataSet RestoreGrid()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Scope", typeof(string));
        dt.Columns.Add("vendor", typeof(string));
        dt.Columns.Add("Quantity", typeof(double));
        dt.Columns.Add("cost", typeof(string));
        dt.Columns.Add("currency", typeof(string));
        dt.Columns.Add("amount", typeof(double));

        dt.Columns.Add("code", typeof(string));
        dt.Columns.Add("measure", typeof(int));

        objProp_Customer.ConnConfig = Session["config"].ToString();
        DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);
        foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
        {
            dt.Columns.Add(drLabor["item"].ToString(), typeof(double));
        }

        DataTable dtPerc = dt.Clone();
        DataSet ds = new DataSet();
        ds.Tables.Add(dt);
        ds.Tables.Add(dtPerc);

        dt = JSONtoTableRestore(hdnItemJSON.Value.Trim(), dt, dsLabor, true);
        dtPerc = JSONtoTableRestore(hdnItemJSONPerc.Value.Trim(), dtPerc, dsLabor, false);

        return ds;
    }

    private DataTable JSONtoTableRestore(string strItems, DataTable dt, DataSet dsLabor, bool includeLaborItems)
    {
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
                dr["code"] = dict["txtCode"];
                if (dict["ddlMeasure"].ToString().Trim() != string.Empty)
                    dr["measure"] = Convert.ToInt32(dict["ddlMeasure"]);

                if (dict["txtQuan"].ToString().Trim() != string.Empty)
                    dr["Quantity"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtQuan"].ToString(), "0"));

                if (dict["txtUnitCost"].ToString().Trim() != string.Empty)
                    dr["cost"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtUnitCost"].ToString(), "0"));

                dr["currency"] = dict["ddlCurrency"];

                if (dict["txtTotal"].ToString().Trim() != string.Empty)
                    dr["amount"] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txtTotal"].ToString(), "0"));

                if (includeLaborItems)
                {
                    foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    {
                        if (dict.ContainsKey("txt" + drLabor["item"].ToString()))
                        {
                            if (dict["txt" + drLabor["item"].ToString()].ToString().Trim() != string.Empty)
                                dr[drLabor["item"].ToString()] = Convert.ToDouble(objGeneralFunctions.IsNull(dict["txt" + drLabor["item"].ToString()].ToString(), "0"));
                        }
                    }
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }

    //protected void gvTemplateItems_DataBound(object sender, EventArgs e)
    //{
    //    FillBucketDropdown();
    //}

    protected void btnInsertBuck_Click(object sender, EventArgs e)
    {
        //DropDownList ddlBucket = gvTemplateItems.FooterRow.FindControl("ddlBucket") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.BucketID = Convert.ToInt32(ddlBucket.SelectedValue);
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getEstimateBucketItems(objProp_Customer);

        //objProp_Customer.ConnConfig = Session["config"].ToString();
        //DataSet dsLabor = objBL_Customer.GetEstimateLabor(objProp_Customer);

        DataSet dsGrid = RestoreGrid();
        DataTable dt = dsGrid.Tables[0];
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["measure"].ToString() != "3")
            {
                DataRow drItems = dt.NewRow();
                drItems["Scope"] = dr["item"];
                drItems["vendor"] = dr["vendor"];
                drItems["Quantity"] = dr["unit"];
                drItems["cost"] = dr["cost"];
                drItems["currency"] = string.Empty;
                drItems["measure"] = dr["measure"];
                drItems["code"] = dr["code"];
                //drItems["amount"] = 0;
                //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                //{
                //    drItems[drLabor["item"].ToString()] = 0;
                //}
                dt.Rows.Add(drItems);
            }
        }
        BindGrid(dt);

        if (dsGrid.Tables.Count > 1)
        {
            DataTable dtPerc = dsGrid.Tables[1];
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["measure"].ToString() == "3")
                {
                    DataRow drItems = dtPerc.NewRow();
                    drItems["Scope"] = dr["item"];
                    drItems["vendor"] = dr["vendor"];
                    drItems["Quantity"] = dr["unit"];
                    drItems["cost"] = dr["cost"];
                    drItems["currency"] = string.Empty;
                    drItems["measure"] = dr["measure"];
                    drItems["code"] = dr["code"];
                    //drItems["amount"] = 0;
                    //foreach (DataRow drLabor in dsLabor.Tables[0].Rows)
                    //{
                    //    drItems[drLabor["item"].ToString()] = 0;
                    //}
                    dtPerc.Rows.Add(drItems);
                }
            }
            BindGridPerc(dtPerc);
        }
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        DataSet ds = RestoreGrid();
        FillBucketDropdown();
        BindGrid(ds.Tables[0]);
        if (ds.Tables.Count > 1)
            BindGridPerc(ds.Tables[1]);
        ModalPopupExtender1.Hide();
        ModalPopupExtender2.Hide();
    }

    private void FillBucketDropdown()
    {
        // DropDownList ddlBucket = gvTemplateItems.FooterRow.FindControl("ddlBucket") as DropDownList;
        DataSet ds = new DataSet();
        objProp_Customer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getEstimateBucket(objProp_Customer);
        ddlBucket.DataSource = ds.Tables[0];
        ddlBucket.DataTextField = "name";
        ddlBucket.DataValueField = "id";
        ddlBucket.DataBind();

        ddlBucket.Items.Insert(0, new ListItem("-- Select Bucket --", "0"));
        ddlBucket.Items.Insert(1, new ListItem("-- Add New --", "0"));
    }

    //protected void gvTemplateItems_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.Header)
    //    {
    //        GridView HeaderGrid = (GridView)sender;
    //        GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

    //        TableCell HeaderCell = new TableCell();
    //        HeaderCell.Text = "";
    //        HeaderCell.ColumnSpan = 3;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Material";
    //        HeaderCell.ColumnSpan = 4;
    //        HeaderCell.
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        HeaderCell = new TableCell();
    //        HeaderCell.Text = "Labor";
    //        //HeaderCell.ColumnSpan = 2;
    //        HeaderGridRow.Cells.Add(HeaderCell);

    //        gvTemplateItems.Controls[0].Controls.AddAt(0, HeaderGridRow);

    //    }
    //}



    protected void lnkUploadTemplate_Click(object sender, EventArgs e)
    {

    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

    }
}

//public class DynamicTemplateField : ITemplate
//{
//    string strTextBoxName;
//    public DynamicTemplateField(string name)
//    {
//        this.strTextBoxName = name;
//    }

//    public void InstantiateIn(Control container)
//    {
//        TextBox txt1 = new TextBox();
//        txt1.ID = "txt" + strTextBoxName;
//        txt1.Width = 50;
//        txt1.Text = "0";
//        txt1.Attributes.Add("onkeydown", "NumericValid(event);");
//        txt1.Attributes.Add("onblur", "calculateDynamic('" + strTextBoxName + "');");
//        txt1.DataBinding += new EventHandler(txt1_DataBinding);
//        container.Controls.Add(txt1);
//    }
//    private void txt1_DataBinding(object sender, EventArgs e)
//    {
//        TextBox target = (TextBox)sender;
//        GridViewRow container = (GridViewRow)target.NamingContainer;
//        target.Text = ((DataRowView)container.DataItem)[strTextBoxName].ToString();
//    }
//}
//public class DynamicTemplateFieldFooter : ITemplate
//{
//    string strTextBoxName;
//    string strvalue;
//    public DynamicTemplateFieldFooter(string name, string value)
//    {
//        this.strTextBoxName = name;
//        this.strvalue = value;
//    }

//    public void InstantiateIn(Control container)
//    {
//        TextBox txt1 = new TextBox();
//        txt1.ID = "txt" + strTextBoxName + "T";
//        txt1.Width = 50;
//        txt1.CssClass = "texttransparent";
//        txt1.Attributes.Add("onfocus", "this.blur();");
//        container.Controls.Add(txt1);

//        HiddenField hdn1 = new HiddenField();
//        hdn1.ID = "hdn" + strTextBoxName + "T";
//        hdn1.Value = strvalue;
//        container.Controls.Add(hdn1);
//    }
//}

//public class EstimateItemData
//{
//    public string txtScope { get; set; }
//    public string txtVendor { get; set; }
//    public string txtQuan { get; set; }
//    public string txtUnitCost { get; set; }
//    public string ddlCurrency { get; set; }
//    public string txtTotal { get; set; }
//    //public string txtMechanic { get; set; }
//    //public string txtHelper { get; set; }
//    //public string txtAdjustor { get; set; }
//    //public string txtTEST { get; set; }
//}
