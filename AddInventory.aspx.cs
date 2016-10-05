using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.IO;
using System.Data.Odbc;
using BusinessEntity;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.Script.Serialization;
using System.Net.Mail;
public partial class AddInventory : System.Web.UI.Page
{


    #region ::Declarartion::
    BL_Inventory objBL_Inventory = new BL_Inventory();
    User objProp_User = new User();
    Inventory objProp_Inventory = new Inventory();
    Itype objProp_Intype = new BusinessEntity.Itype();
    Chart objProp_Chart = new BusinessEntity.Chart();
    private enum chartType
    {
        Assets = 0,
        Liabilities = 1,
        Equity = 2,
        Revenue = 3,
        CostofSales = 4,
        Expenses = 5,
        CashinBank = 6,
        PurchaseOrders = 7
    }

    public string invID = string.Empty;
    #endregion

    #region ::Page Events::
    protected void Page_Load(object sender, EventArgs e)
    {


        invID = !string.IsNullOrEmpty(Request.QueryString["id"]) ? Request.QueryString["id"] : string.Empty;

        if (!IsPostBack)
        {
            if (Session["config"] != null)
            {
                BindControls();
                if (!string.IsNullOrEmpty(invID))
                {
                    FillData(invID);
                }
            }


        }


    }


    protected void Page_PreRender(Object o, EventArgs e)
    {
    }






    #endregion

    #region ::Events::
    protected void btneditVendorInfo_Click(object sender, EventArgs e)
    {
        clear();
        string itemname = txtItemHeaderName.Text;
        string itemdecs = txtDes.Text;

        txtEngineeringName.Text = itemname;
        txtEngineeringDescription.Text = itemdecs;
        txtFinanceName.Text = itemname;
        txtFinanceDescription.Text = itemdecs;
        txtPurchasingName.Text = itemname;
        txtPurchasingDescription.Text = itemdecs;
        txtInventoryName.Text = itemname;
        txtInventoryDescription.Text = itemdecs;
        txtSalesName.Text = itemname;
        txtSalesDescription.Text = itemdecs;

        foreach (DataListItem dt in dtlVendors.Items)
        {
            InventoryManufacturerInformation item = new BusinessEntity.InventoryManufacturerInformation();
            CheckBox chkinvi = dt.FindControl("chkvenitem") as CheckBox;
            HiddenField id = dt.FindControl("hdnid") as HiddenField;
            Label mpn = dt.FindControl("lblmpn") as Label;
            Label apprivedmanfacturer = dt.FindControl("lblName") as Label;
            Label apprivedorid = dt.FindControl("lblvendorid") as Label;
            if (chkinvi.Checked)
            {

                txtInventoryMPN.Text = Convert.ToString(mpn.Text);
                txtInventoryApprovedManufacturer.Text = Convert.ToString(apprivedmanfacturer.Text);
                ddlInventoryApprovedVendor.SelectedValue = Convert.ToString(apprivedorid.Text);
                hdninvvendinfo.Value = id.Value;


            }


        }


        //this.programmaticModalPopup.Show();
        //this.pnlInventoryWarehouse.Visible = true;
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                objProp_Inventory = new BusinessEntity.Inventory();
                #region ItemHeader
                objProp_Inventory.Measure = ddlUOM.SelectedValue;
                objProp_Inventory.Status = Convert.ToInt32(ddlInvStatus.SelectedValue);
                objProp_Inventory.Description2 = txtDes2.Text;
                objProp_Inventory.Description3 = txtDes3.Text;
                objProp_Inventory.Description4 = txtDes4.Text;
                objProp_Inventory.DateCreated = Convert.ToDateTime(txtDateCreated.Text);
                objProp_Inventory.Cat = ddlCategory.SelectedValue != "" ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
                objProp_Inventory.Remarks = txtRemarks.Text;
                #endregion
                #region Eng
                objProp_Inventory.Specification = txtSpecification.Text;
                objProp_Inventory.Specification2 = txtSpecification2.Text;
                objProp_Inventory.Specification3 = txtSpecification3.Text;
                objProp_Inventory.Specification4 = txtSpecification4.Text;
                objProp_Inventory.Revision = txtRevision.Text;
                objProp_Inventory.LastRevisionDate = string.IsNullOrEmpty(txtRevisionDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtRevisionDate.Text);
                objProp_Inventory.Eco = txtECO.Text;
                objProp_Inventory.Drawing = txtDrawing.Text;
                objProp_Inventory.Reference = txtReference.Text;
                objProp_Inventory.Length = txtLength.Text;
                objProp_Inventory.Width = txtWidth.Text;
                objProp_Inventory.Height = txtHeight.Text;
                objProp_Inventory.Weight = txtWeight.Text;
                objProp_Inventory.ShelfLife = Convert.ToDecimal(txtshelflife.Text);
                objProp_Inventory.InspectionRequired = chkInspRequired.Checked;
                objProp_Inventory.CoCRequired = chkCoCpRequired.Checked;
                objProp_Inventory.SerializationRequired = chkSerializationRequired.Checked;
                ProcessPostedDrawing(objProp_Inventory);


                #endregion
                #region Finance
                objProp_Inventory.UnitCost = 0;
                objProp_Inventory.LCost = 0;
                objProp_Inventory.GLSales = (string)ddlglsales.SelectedValue;
                objProp_Inventory.GLcogs = (string)ddlglcogs.SelectedValue;
                objProp_Inventory.LVendor = Convert.ToInt32(ddlLastPurchaseFromVendor.SelectedValue);
                //Last Purchase date Once confirmed put here
                objProp_Inventory.OHValue = 0;
                objProp_Inventory.OOValue = 0;
                objProp_Inventory.Committed = 0;
                objProp_Inventory.ABCClass = Convert.ToString(ddlABC.SelectedValue);
                objProp_Inventory.Tax = Convert.ToInt32(ckhTaxable.Checked);
                objProp_Inventory.InventoryTurns = 0;
                #endregion
                #region Purchasing
                //objProp_Inventory.nextPOdate = Convert.ToDateTime(txtNextPoDate.Text);
                //objProp_Inventory.lastpurchasedate = Convert.ToDateTime(txtLastPODate.Text);
                objProp_Inventory.LCost = 0;
                //objProp_Inventory.LVendor = Convert.ToInt32(txtLastVendor.Text);
                //objProp_Inventory.LastReceiptDate = string.IsNullOrEmpty(txtLastReceiptDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtLastReceiptDate.Text); //Convert.ToDateTime(txtLastReceiptDate.Text);
                objProp_Inventory.Commodity = Convert.ToString(ddlCommodity.SelectedValue);
                objProp_Inventory.EAU = 0; //Convert.ToDecimal(txtEAU.Text);
                objProp_Inventory.EOLDate = string.IsNullOrEmpty(txtEOLDate.Text) ? DateTime.MinValue : Convert.ToDateTime(txtEOLDate.Text);//Convert.ToDateTime(txtEOLDate.Text);
                objProp_Inventory.Weight = Convert.ToString(txtWeight.Text);
                objProp_Inventory.WarrantyPeriod = string.IsNullOrEmpty(txtWarrantyPeriod.Text) ? 0 : Convert.ToInt32(txtWarrantyPeriod.Text); //Convert.ToDateTime(txtWarrantyPeriod.Text);
                // objProp_Inventory.MPN = Convert.ToString(txtMPN.Text);
                // objProp_Inventory.ApprovedManufacturer = Convert.ToString(txtAppManuFacturer.Text);
                //objProp_Inventory.ApprovedVendor = Convert.ToString(ddlApprovedVendor.SelectedValue);
                objProp_Inventory.LeadTime = (int)Convert.ToDouble(txtLeadTime.Text);
                objProp_Inventory.MOQ = Convert.ToDecimal(txtMOQ.Text);
                objProp_Inventory.EOQ = Convert.ToDecimal(txtEOQ.Text);
                #endregion
                #region Inventory
                objProp_Inventory.Warehouse = Convert.ToString(ddlWareHouse.SelectedValue);
                objProp_Inventory.Aisle = Convert.ToString(txtAisle.Text);
                objProp_Inventory.Shelf = Convert.ToString(txtShelf.Text);
                objProp_Inventory.Bin = Convert.ToString(txtBin.Text);
                //objProp_Inventory.DateLastUsed = Convert.ToString(txtDateLastUsed.Text);
                //objProp_Inventory.ShelfLife = Convert.ToDecimal(txtshelflife.Text);
                #endregion
                #region Sales
                objProp_Inventory.Price1 = Convert.ToDecimal(txtPrice1.Text);
                objProp_Inventory.Price2 = Convert.ToDecimal(txtPrice2.Text);
                objProp_Inventory.Price3 = Convert.ToDecimal(txtPrice3.Text);
                objProp_Inventory.Price4 = Convert.ToDecimal(txtPrice4.Text);
                objProp_Inventory.Price5 = Convert.ToDecimal(txtPrice5.Text);
                objProp_Inventory.Price6 = Convert.ToDecimal(txtPrice6.Text);
                //objProp_Inventory.AnnualSalesQty = Convert.ToDecimal(txtAnnualSalesQuantity.Text);
                //objProp_Inventory.AnnualSalesAmt = Convert.ToDecimal(txtAnnualSales.Text);
                objProp_Inventory.MaxDiscountPercentage = Convert.ToDecimal(txtMaxDiscount.Text);
                #endregion
                #region Inventory Vendor
                //DataTable dt = ViewState["POVendors"] as DataTable;
                //List<InventoryManufacturerInformation> lstvendorinfo = new List<InventoryManufacturerInformation>();
                //if (dt != null)
                //{
                //    if (dt.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dt.Rows.Count; i++)
                //        {
                //            InventoryManufacturerInformation item = new BusinessEntity.InventoryManufacturerInformation();
                //            item.ID = dt.Rows[i]["ID"] != DBNull.Value ? (int)dt.Rows[i]["ID"] : 0;
                //            item.InventoryID = dt.Rows[i]["InvID"] != DBNull.Value ? (int)dt.Rows[i]["InvID"] : 0;
                //            item.ApprovedVendor = (string)dt.Rows[i]["ApprovedVendorId"];
                //            item.ApprovedManufacturer = (string)dt.Rows[i]["ApprovedManufacturer"];
                //            item.MPN = (string)dt.Rows[i]["MPN"];

                //            lstvendorinfo.Add(item);
                //        }
                //    }
                //}

                #endregion

                objProp_Inventory.ApprovedVendors = Getvendors().ToArray();
                if (string.IsNullOrEmpty(invID))
                {
                    objProp_Inventory.Name = txtItemHeaderName.Text;
                    objProp_Inventory.fDesc = txtDes.Text;


                    Inventory createdinv = objBL_Inventory.CreateInventory(objProp_Inventory);

                    if (createdinv != null)
                    {
                        clear();

                        Response.Redirect("AddInventory.aspx?Id=" + createdinv.ID);
                        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Item Created Successfully! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'Item could not be created! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    objProp_Inventory.ID = Convert.ToInt32(invID);
                    objProp_Inventory.fDesc = txtDes.Text;

                    objBL_Inventory.UpdateInventory(objProp_Inventory);

                    clear();

                    Response.Redirect("AddInventory.aspx?Id=" + objProp_Inventory.ID);

                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccadd", "noty({text: 'Item Updated Successfully! </br> <b> Inventory P/N# : " + txtItemHeaderName.Text + "</b>',  type : 'success', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }


        }
    }
    protected void btnaddVendorInfo_Click(object sender, EventArgs e)
    {
        clear();
        string itemname = txtItemHeaderName.Text;
        string itemdecs = txtDes.Text;
        txtEngineeringName.Text = itemname;
        txtEngineeringDescription.Text = itemdecs;
        txtFinanceName.Text = itemname;
        txtFinanceDescription.Text = itemdecs;
        txtPurchasingName.Text = itemname;
        txtPurchasingDescription.Text = itemdecs;
        txtInventoryName.Text = itemname;
        txtInventoryDescription.Text = itemdecs;
        txtSalesName.Text = itemname;
        txtSalesDescription.Text = itemdecs;
        // this.programmaticModalPopup.Show();
        //this.pnlInventoryWarehouse.Visible = true;

    }

    protected void lnkSaveInventoryWarehouse_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["POVendors"];

        if (dtlVendors.Items.Count > 0)
        {
            if (hdninvvendinfo.Value != "")
            {
                int index = 0;
                foreach (DataListItem dtitemm in dtlVendors.Items)
                {
                    InventoryManufacturerInformation item = new BusinessEntity.InventoryManufacturerInformation();
                    CheckBox chkinvi = dtitemm.FindControl("chkvenitem") as CheckBox;
                    HiddenField id = dtitemm.FindControl("hdnid") as HiddenField;
                    Label lblmpn = dtitemm.FindControl("lblmpn") as Label;
                    Label lblName = dtitemm.FindControl("lblName") as Label;
                    Label lblvendorid = dtitemm.FindControl("lblvendorid") as Label;
                    Label lblvendorname = dtitemm.FindControl("lblvendorname") as Label;
                    if (chkinvi.Checked)
                    {
                        id.Value = hdninvvendinfo.Value;
                        lblmpn.Text = txtInventoryMPN.Text;
                        lblName.Text = txtInventoryApprovedManufacturer.Text;
                        lblvendorid.Text = ddlInventoryApprovedVendor.SelectedItem.Value;
                        lblvendorname.Text = ddlInventoryApprovedVendor.SelectedItem.Text;
                        if (dt != null)
                        {
                            if (dt.Rows.Count > 0)
                            {


                                //for (int i = 0; i < dt.Rows.Count; i++)
                                //{

                                if (id.Value == Convert.ToString(dt.Rows[index]["ID"]))
                                {
                                    // dt.Rows[i]["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                                    dt.Rows[index]["InvID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                                    dt.Rows[index]["MPN"] = (txtInventoryMPN.Text);
                                    dt.Rows[index]["ApprovedManufacturer"] = (txtInventoryApprovedManufacturer.Text);
                                    dt.Rows[index]["ApprovedVendor"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
                                    dt.Rows[index]["ApprovedVendorId"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
                                }
                                // }
                            }
                        }

                        ViewState["POVendors"] = dt;
                    }

                    index++;


                }
            }
            else
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
                dr["InvID"] = invID != "" ? Convert.ToInt32(invID) : 0;
                dr["MPN"] = (txtInventoryMPN.Text);
                dr["ApprovedManufacturer"] = (txtInventoryApprovedManufacturer.Text);
                dr["ApprovedVendor"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
                dr["ApprovedVendorId"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
                dt.Rows.Add(dr);
                dt.AcceptChanges();
                dtlVendors.DataSource = dt;
                dtlVendors.DataBind();
                ViewState["POVendors"] = dt;


            }
        }
        else
        {
            dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("InvID", typeof(int));
            dt.Columns.Add("MPN", typeof(string));
            dt.Columns.Add("ApprovedManufacturer", typeof(string));
            dt.Columns.Add("ApprovedVendor", typeof(string));
            dt.Columns.Add("ApprovedVendorId", typeof(string));
            dt.AcceptChanges();


            DataRow dr = dt.NewRow();
            dr["ID"] = hdninvvendinfo.Value != "" ? Convert.ToInt32(hdninvvendinfo.Value) : 0;
            dr["InvID"] = invID != "" ? Convert.ToInt32(invID) : 0;
            dr["MPN"] = (txtInventoryMPN.Text);
            dr["ApprovedManufacturer"] = (txtInventoryApprovedManufacturer.Text);
            dr["ApprovedVendor"] = (ddlInventoryApprovedVendor.SelectedItem.Text);
            dr["ApprovedVendorId"] = (ddlInventoryApprovedVendor.SelectedItem.Value);
            dt.Rows.Add(dr);
            dt.AcceptChanges();
            dtlVendors.DataSource = dt;
            dtlVendors.DataBind();



            ViewState["POVendors"] = dt;
        }

        //this.programmaticModalPopup.Hide();
        //pnlInventoryWarehouse.Visible = false;


        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "modalclose();", true);

        clear();
    }

    protected void lnkCloseInventoryWarehouse_Click(object sender, EventArgs e)
    {
        //this.programmaticModalPopup.Hide();
        //this.pnlInventoryWarehouse.Visible = false;
        clear();
    }
    public void ddlHeaderNameName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlHeaderNameName.SelectedValue != "0")
        {
            FillData(ddlHeaderNameName.SelectedValue);
        }
    }

    protected void btndekVendorInfo_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)ViewState["POVendors"];

        int itemIndex = 0;


        // Check each box and see if the item should be deleted.
        foreach (DataListItem dtitemm in dtlVendors.Items)
        {
            CheckBox chkinvi = dtitemm.FindControl("chkvenitem") as CheckBox;
            if (chkinvi.Checked)
            {
                //itemIndex = dtlVendors.DataKeys[itemIndex].ToString();
                if (dt.Rows.Count > 0)
                {
                    dt.Rows[itemIndex].Delete();
                    dt.AcceptChanges();
                }

            }
            itemIndex++;
        }


        dtlVendors.DataSource = dt;
        dtlVendors.DataBind();

    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Inventory.aspx");
    }
    #endregion


    #region ::Methods::
    private void BindControls()
    {
        #region Unit Of Measure
        objBL_Inventory = new BL_Inventory();
        List<UnitOfMeasure> dsuom = objBL_Inventory.GetALLUnitOfMeasure();

        if (dsuom != null)
        {
            if (dsuom.Count > 0)
            {

                ddlUOM.DataSource = dsuom;
                ddlUOM.DataValueField = "ID";
                ddlUOM.DataTextField = "Description";
                // ddlUOM.DataTextFormatString = "[(Code)]15[|(Description)]32";
                ddlUOM.DataBind();

                ddlUOM.Items.Add(new ListItem("Select UOM", "0"));

                ddlUOM.SelectedValue = "0";




            }
        }
        #endregion

        #region Status
        ddlInvStatus.Items.Add(new ListItem("Active", "0"));
        ddlInvStatus.Items.Add(new ListItem("Inactive", "1"));
        ddlInvStatus.Items.Add(new ListItem("On Hold", "2"));

        #endregion

        #region Category
        objBL_Inventory = new BL_Inventory();
        List<Itype> itypes = objBL_Inventory.GetALLItype();

        if (itypes != null)
        {
            if (itypes.Count > 0)
            {

                ddlCategory.DataSource = itypes;
                ddlCategory.DataValueField = "ID";
                ddlCategory.DataTextField = "Type";
                // ddlUOM.DataTextFormatString = "[(Code)]15[|(Description)]32";
                ddlCategory.DataBind();

                ddlCategory.Items.Add(new ListItem("Select Category", "0"));

                ddlCategory.SelectedValue = "0";




            }
        }
        #endregion

        #region Gl

        List<Chart> chart1 = objBL_Inventory.GetChartByType((int)chartType.Revenue);
        if (chart1 != null)
        {
            if (chart1.Count > 0)
            {
                //ddlglsales.DataSource = chart1;
                //ddlglsales.DataValueField = "ID";
                // ddlglsales.DataTextField = string.Format("{0}-{1}", "Acct", "fDesc");

                ddlglsales.Items.Add(new ListItem("Select GL Sales", "0"));

                foreach (Chart chart in chart1)
                {
                    ddlglsales.Items.Add(new ListItem(string.Format("{0}-{1}", chart.Acct, chart.fDesc), chart.ID.ToString()));

                }
                ddlglsales.DataBind();




                ddlglsales.SelectedValue = "0";
            }
        }

        List<Chart> chart2 = objBL_Inventory.GetChartByType((int)chartType.CostofSales);
        if (chart2 != null)
        {


            if (chart2.Count > 0)
            {
                ddlglcogs.Items.Add(new ListItem("Select GL COGS", "0"));

                foreach (Chart chart in chart2)
                {
                    ddlglcogs.Items.Add(new ListItem(string.Format("{0}-{1}", chart.ID, chart.fDesc), chart.ID.ToString()));

                }
                ddlglcogs.DataBind();
                ddlglcogs.SelectedValue = "0";

                //ddlglcogs.DataSource = chart2;
                //ddlglcogs.DataValueField = "ID";
                //ddlglcogs.DataTextField = "fDesc";
                //ddlglcogs.DataBind();

                //ddlglcogs.Items.Add(new ListItem("Select GL COGS", "0"));

                //ddlglcogs.SelectedValue = "0";
            }
        }





        #endregion

        #region ABC Class
        ddlABC.Items.Add(new ListItem("None", "0"));
        ddlABC.Items.Add(new ListItem("Class A", "A"));
        ddlABC.Items.Add(new ListItem("Class B", "B"));
        ddlABC.Items.Add(new ListItem("Class C", "C"));
        #endregion

        #region WareHouse
        List<string> strwarehouse = new List<string>();
        objProp_User.ConnConfig = Session["config"].ToString();
        strwarehouse = objBL_Inventory.GetInventoryWarehouse(objProp_User);
        ddlWareHouse.DataSource = strwarehouse;
        ddlWareHouse.DataBind();
        #endregion

        #region Vendor Information
        DataTable dt = new DataTable();
        dt.Columns.Add("ID", typeof(int));
        dt.Columns.Add("InvID", typeof(int));
        dt.Columns.Add("MPN", typeof(string));
        dt.Columns.Add("ApprovedManufacturer", typeof(string));
        dt.Columns.Add("ApprovedVendor", typeof(string));
        dt.Columns.Add("ApprovedVendorId", typeof(string));
        dt.AcceptChanges();
        dtlVendors.DataSource = dt;
        dtlVendors.DataBind();

        txtDateCreated.Text = DateTime.Today.ToString("MM/dd/yyyy");

        objBL_Inventory = new BL_Inventory();
        Dictionary<string, string> lstvendor = objBL_Inventory.GetAllVendor(Session["config"].ToString());
        ddlInventoryApprovedVendor.DataValueField = "Key";
        ddlInventoryApprovedVendor.DataTextField = "Value";
        ddlInventoryApprovedVendor.DataSource = lstvendor;
        ddlInventoryApprovedVendor.DataBind();

        ddlLastPurchaseFromVendor.DataValueField = "Key";
        ddlLastPurchaseFromVendor.DataTextField = "Value";
        ddlLastPurchaseFromVendor.DataSource = lstvendor;
        ddlLastPurchaseFromVendor.DataBind();

        ddlApprovedVendorrequestquote.DataValueField = "Key";
        ddlApprovedVendorrequestquote.DataTextField = "Value";
        ddlApprovedVendorrequestquote.DataSource = lstvendor;
        ddlApprovedVendorrequestquote.DataBind();
        linkquoterequestid.Visible = false;
        #endregion

        #region Name DDl Binding
        objBL_Inventory = new BL_Inventory();
        objProp_Inventory.ConnConfig = Session["config"].ToString();
        DataSet dsinv = objBL_Inventory.GetAllInventory(objProp_Inventory);

        if (dsinv != null)
        {
            if (dsinv.Tables.Count > 0)
            {
                if (dsinv.Tables[0].Rows.Count > 0)
                {
                    var dtinv = from c in dsinv.Tables[0].Select() select new { ID = c[0], Name = c[1] };



                    ddlHeaderNameName.DataSource = dtinv;
                    ddlHeaderNameName.DataTextField = "Name";
                    ddlHeaderNameName.DataValueField = "ID";
                    ddlHeaderNameName.DataBind();
                    ddlHeaderNameName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlHeaderNameName.SelectedValue = "0";

                    ddlEngineeringName.DataSource = dtinv;
                    ddlEngineeringName.DataTextField = "Name";
                    ddlEngineeringName.DataValueField = "ID";
                    ddlEngineeringName.DataBind();
                    ddlEngineeringName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlEngineeringName.SelectedValue = "0";

                    ddlFinanceName.DataSource = dtinv;
                    ddlFinanceName.DataTextField = "Name";
                    ddlFinanceName.DataValueField = "ID";
                    ddlFinanceName.DataBind();
                    ddlFinanceName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlFinanceName.SelectedValue = "0";

                    ddlPurchasingName.DataSource = dtinv;
                    ddlPurchasingName.DataTextField = "Name";
                    ddlPurchasingName.DataValueField = "ID";
                    ddlPurchasingName.DataBind();
                    ddlPurchasingName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlPurchasingName.SelectedValue = "0";

                    ddlSalesName.DataSource = dtinv;
                    ddlSalesName.DataTextField = "Name";
                    ddlSalesName.DataValueField = "ID";
                    ddlSalesName.DataBind();
                    ddlSalesName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlSalesName.SelectedValue = "0";

                    ddlInventoryName.DataSource = dtinv;
                    ddlInventoryName.DataTextField = "Name";
                    ddlInventoryName.DataValueField = "ID";
                    ddlInventoryName.DataBind();
                    ddlInventoryName.Items.Add(new ListItem("Select P/N", "0"));
                    ddlInventoryName.SelectedValue = "0";



                }
            }
        }
        #endregion

        #region commodity
        Commodity objBL_commodity = new Commodity();
        objBL_commodity.ConnConfig = Session["config"].ToString();
        DataSet dscommodity = objBL_Inventory.ReadAllCommodity(objBL_commodity);
        if (dscommodity != null)
        {
            if (dscommodity.Tables.Count > 0)
            {
                if (dscommodity.Tables[0].Rows.Count > 0)
                {
                    var dtcommodity = from c in dscommodity.Tables[0].Select() select new { Id = c[0], DisplayVal = c[1] + " - " + c[2] };

                    ddlCommodity.DataSource = dtcommodity;
                    ddlCommodity.DataTextField = "DisplayVal";
                    ddlCommodity.DataValueField = "Id";
                    ddlCommodity.DataBind();
                    ddlCommodity.Items.Add(new ListItem("Select Commodity", "0"));
                    ddlCommodity.SelectedValue = "0";
                }
            }
        }
        #endregion




    }

    private void clear()
    {
        txtInventoryMPN.Text = string.Empty;
        txtInventoryApprovedManufacturer.Text = string.Empty;
        ddlInventoryApprovedVendor.ClearSelection();
        hdninvvendinfo.Value = "";
    }

    private void FillData(string Id)
    {
        objProp_Inventory = new BusinessEntity.Inventory();
        objProp_Inventory = GetInventoryById(Id).ReponseObject;

        if (objProp_Inventory != null)
        {
            DataSet dsprchaseinfo = objBL_Inventory.GetItemPurchaseOrder(objProp_Inventory.ID);

            #region ItemHeader
            txtItemHeaderName.Text = objProp_Inventory.Name;
            ddlHeaderNameName.SelectedValue = objProp_Inventory.ID.ToString();
            txtDes.Text = objProp_Inventory.fDesc;
            // ddlUOM.SelectedValue = objProp_Inventory.Measure;
            ddlInvStatus.SelectedValue = Convert.ToString(objProp_Inventory.Status);
            txtDes2.Text = objProp_Inventory.Description2;
            txtDes3.Text = objProp_Inventory.Description3;
            txtDes4.Text = objProp_Inventory.Description4;
            txtDateCreated.Text = Convert.ToString(objProp_Inventory.DateCreated);
            ddlCategory.SelectedValue = Convert.ToString(objProp_Inventory.Cat);
            txtRemarks.Text = objProp_Inventory.Remarks;
            ddlUOM.SelectedValue = Convert.ToString(objProp_Inventory.Measure);
            #endregion
            #region Eng
            ddlEngineeringName.SelectedValue = objProp_Inventory.ID.ToString();
            txtEngineeringDescription.Text = objProp_Inventory.fDesc;
            txtSpecification.Text = objProp_Inventory.Specification;
            txtSpecification2.Text = objProp_Inventory.Specification2;
            txtSpecification3.Text = objProp_Inventory.Specification3;
            txtSpecification4.Text = objProp_Inventory.Specification4;
            txtRevision.Text = objProp_Inventory.Revision;
            txtRevisionDate.Text = Convert.ToString(objProp_Inventory.LastRevisionDate);
            txtECO.Text = objProp_Inventory.Eco;
            txtDrawing.Text = objProp_Inventory.Drawing;
            txtReference.Text = objProp_Inventory.Reference;
            txtLength.Text = objProp_Inventory.Length;
            txtWidth.Text = objProp_Inventory.Width;
            txtHeight.Text = objProp_Inventory.Height;
            txtWeight.Text = objProp_Inventory.Weight;
            txtshelflife.Text = Convert.ToString(objProp_Inventory.ShelfLife);
            chkInspRequired.Checked = objProp_Inventory.InspectionRequired;
            chkCoCpRequired.Checked = objProp_Inventory.CoCRequired;
            chkSerializationRequired.Checked = objProp_Inventory.SerializationRequired;

            #endregion
            #region Finance
            ddlFinanceName.SelectedValue = objProp_Inventory.ID.ToString();
            txtFinanceDescription.Text = objProp_Inventory.fDesc;
            txtunitCost.Text = Convert.ToString(objProp_Inventory.UnitCost);
            txtLastPurchaseCost.Text = Convert.ToString(objProp_Inventory.LCost);
            ddlglsales.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLSales) ? objProp_Inventory.GLSales : "0";
            ddlglcogs.SelectedValue = !string.IsNullOrEmpty(objProp_Inventory.GLcogs) ? objProp_Inventory.GLcogs : "0";
            if (objProp_Inventory.LVendor != 0)
                ddlLastPurchaseFromVendor.SelectedValue = Convert.ToString(objProp_Inventory.LVendor);
            //Last Purchase date Once confirmed put here
            txtOHVal.Text = Convert.ToString(objProp_Inventory.OHValue);
            txtOOVal.Text = Convert.ToString(objProp_Inventory.OOValue);
            txtComittedValue.Text = Convert.ToString(objProp_Inventory.OOValue);
            objProp_Inventory.Committed = Convert.ToDecimal(objProp_Inventory.Committed);
            if (objProp_Inventory.ABCClass != "")
                ddlABC.SelectedValue = objProp_Inventory.ABCClass;
            ckhTaxable.Checked = Convert.ToBoolean(objProp_Inventory.Tax);
            txtInventoryTurns.Text = Convert.ToString(objProp_Inventory.InventoryTurns);

            #endregion
            #region Purchasing
            ddlPurchasingName.SelectedValue = objProp_Inventory.ID.ToString();
            txtPurchasingDescription.Text = objProp_Inventory.fDesc;
            //objProp_Inventory.nextPOdate = Convert.ToDateTime(txtNextPoDate.Text);
            txtLastPODate.Text = Convert.ToString(objProp_Inventory.DateLastPurchase);
            txtLastUnitCost.Text = Convert.ToString(objProp_Inventory.LCost);
            txtLastVendor.Text = ddlLastPurchaseFromVendor.SelectedItem.Text;
            //objProp_Inventory.LVendor = Convert.ToInt32(txtLastVendor.Text);
            txtLastReceiptDate.Text = Convert.ToString(objProp_Inventory.LastReceiptDate);
            ddlCommodity.SelectedValue = objProp_Inventory.Commodity != "" ? objProp_Inventory.Commodity : "0";
            txtEAU.Text = Convert.ToString(objProp_Inventory.EAU);
            txtEOLDate.Text = Convert.ToString(objProp_Inventory.EOLDate);
            txtWeight.Text = objProp_Inventory.Weight;
            txtWarrantyPeriod.Text = Convert.ToString(objProp_Inventory.WarrantyPeriod);
            txtLeadTime.Text = Convert.ToString(objProp_Inventory.LeadTime);
            txtMOQ.Text = Convert.ToString(objProp_Inventory.MOQ);
            txtEOQ.Text = Convert.ToString(objProp_Inventory.EOQ);
            #endregion
            #region Inventory
            ddlInventoryName.SelectedValue = objProp_Inventory.ID.ToString();
            txtInventoryDescription.Text = objProp_Inventory.fDesc;
            if (objProp_Inventory.Warehouse != "")
                ddlWareHouse.SelectedValue = objProp_Inventory.Warehouse;
            txtAisle.Text = objProp_Inventory.Aisle;
            txtShelf.Text = objProp_Inventory.Shelf;
            txtBin.Text = objProp_Inventory.Bin;
            txtDateLastUsed.Text = Convert.ToString(objProp_Inventory.LastRevisionDate);
            //objProp_Inventory.DateLastUsed = Convert.ToString(txtDateLastUsed.Text);
            txtshelflife.Text = Convert.ToString(objProp_Inventory.ShelfLife);
            #endregion
            #region Sales
            ddlSalesName.SelectedValue = objProp_Inventory.ID.ToString();
            txtSalesDescription.Text = objProp_Inventory.fDesc;
            txtPrice1.Text = Convert.ToString(objProp_Inventory.Price1);
            txtPrice2.Text = Convert.ToString(objProp_Inventory.Price2);
            txtPrice3.Text = Convert.ToString(objProp_Inventory.Price3);
            txtPrice4.Text = Convert.ToString(objProp_Inventory.Price4);
            txtPrice5.Text = Convert.ToString(objProp_Inventory.Price5);
            txtPrice6.Text = Convert.ToString(objProp_Inventory.Price6);
            txtAnnualSalesQuantity.Text = Convert.ToString(objProp_Inventory.AnnualSalesQty);
            txtAnnualSales.Text = Convert.ToString(objProp_Inventory.AnnualSalesAmt);
            txtMaxDiscount.Text = Convert.ToString(objProp_Inventory.MaxDiscountPercentage);
            #endregion

            #region Inventory Vendor
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("InvID", typeof(int));
            dt.Columns.Add("MPN", typeof(string));
            dt.Columns.Add("ApprovedManufacturer", typeof(string));
            dt.Columns.Add("ApprovedVendor", typeof(string));
            dt.Columns.Add("ApprovedVendorId", typeof(string));
            dt.AcceptChanges();



            List<InventoryManufacturerInformation> lstvendorinfo = new List<InventoryManufacturerInformation>();
            if (objProp_Inventory.ApprovedVendors != null)
            {
                foreach (InventoryManufacturerInformation invman in objProp_Inventory.ApprovedVendors)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = invman.ID;
                    dr["InvID"] = invman.InventoryID;
                    dr["ApprovedVendorId"] = invman.ApprovedVendorId;
                    dr["ApprovedVendor"] = invman.ApprovedVendor;
                    dr["ApprovedManufacturer"] = invman.ApprovedManufacturer;
                    dr["MPN"] = invman.MPN;
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();

                }
            }
            dtlVendors.DataSource = dt;
            dtlVendors.DataBind();

            ViewState["POVendors"] = dt;

            #endregion

            #region PO Info
            if (dsprchaseinfo != null)
            {
                if (dsprchaseinfo.Tables.Count > 0)
                {
                    if (dsprchaseinfo.Tables[0].Rows.Count > 0)
                    {
                        if (dsprchaseinfo.Tables[1].Rows.Count > 0)
                        {
                            txtNextPoDate.Text = dsprchaseinfo.Tables[1].Rows[0]["Due"] != DBNull.Value ? Convert.ToString(dsprchaseinfo.Tables[1].Rows[0]["Due"]) : "";
                        }
                    }
                }
            }
            #endregion


            #region Inventory Stats
            objBL_Inventory = new BL_Inventory();
            DataSet dsquantity = objBL_Inventory.GetItemQuantity();
            if (dsquantity != null)
            {
                if (dsquantity.Tables.Count > 0)
                {
                    if (dsquantity.Tables[0].Rows.Count > 0)
                    {
                        txtOnHnad.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["OnHand"]);
                        txtOnOrder.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["OnOrder"]);
                        txtOnComitted.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["Comitted"]);
                        txtOnAvaliable.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["Avaliable"]);
                        txtIssuedtoOpenjobs.Text = Convert.ToString(dsquantity.Tables[0].Rows[0]["IssuesToOpenJobs"]);
                    }
                }
            }
            #endregion

            linkquoterequestid.Visible = true;
        }
        else
        {

        }


    }


    private void ProcessPostedDrawing(Inventory obj_inv)
    {
        if (!string.IsNullOrEmpty(txtDrawing.Text))
        {
            // Read the file and convert it to Byte Array

            string filePath = flDrawing.PostedFile.FileName;

            string filename = Path.GetFileName(filePath);

            string ext = Path.GetExtension(filename);

            string contenttype = String.Empty;

            #region Extension Check

            //Set the contenttype based on File Extension
            switch (ext)
            {

                case ".doc":

                    contenttype = "application/vnd.ms-word";

                    break;

                case ".docx":

                    contenttype = "application/vnd.ms-word";

                    break;

                case ".xls":

                    contenttype = "application/vnd.ms-excel";

                    break;

                case ".xlsx":

                    contenttype = "application/vnd.ms-excel";

                    break;

                case ".jpg":

                    contenttype = "image/jpg";

                    break;

                case ".png":

                    contenttype = "image/png";

                    break;

                case ".gif":

                    contenttype = "image/gif";

                    break;

                case ".pdf":

                    contenttype = "application/pdf";

                    break;

            }
            #endregion

            if (contenttype != String.Empty)
            {



                Stream fs = flDrawing.PostedFile.InputStream;

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                if (bytes != null)
                {

                    string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                    string savepath = savepathconfig + @"\" + Session["dbname"] + @"\Inventory\In_" + invID + @"\";
                    string fullpath = savepath + filename;

                    if (!Directory.Exists(savepath))
                    {
                        Directory.CreateDirectory(savepath);
                    }
                    if (File.Exists(fullpath))
                    {
                        GeneralFunctions objGeneralFunctions = new GeneralFunctions();
                        filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                        fullpath = savepath + filename;
                    }


                    flDrawing.SaveAs(fullpath);

                    obj_inv.Drawing = fullpath;

                }




            }

            else
            {



            }

        }
    }



    private List<InventoryManufacturerInformation> Getvendors()
    {
        List<InventoryManufacturerInformation> items = new List<BusinessEntity.InventoryManufacturerInformation>();

        if (dtlVendors != null)
        {
            foreach (DataListItem dt in dtlVendors.Items)
            {
                InventoryManufacturerInformation item = new BusinessEntity.InventoryManufacturerInformation();
                HiddenField id = dt.FindControl("hdnid") as HiddenField;
                Label mpn = dt.FindControl("lblmpn") as Label;
                Label apprivedmanfacturer = dt.FindControl("lblName") as Label;
                Label apprivedorid = dt.FindControl("lblvendorid") as Label;
                if (id.Value != "")
                {
                    item.ID = Convert.ToInt32(id.Value);
                    item.MPN = Convert.ToString(mpn.Text);
                    item.ApprovedManufacturer = Convert.ToString(apprivedmanfacturer.Text);
                    item.ApprovedVendorId = Convert.ToString(apprivedorid.Text);

                    items.Add(item);
                }


            }
        }

        return items;
    }
    #endregion

    #region ::WebMethods::
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<Inventory> GetInventoryById(string EditID)
    {
        WebMethodResponse<Inventory> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<BusinessEntity.Inventory>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();


        BL_Inventory objBL_JsonInventory = new BL_Inventory();
        BusinessEntity.Inventory objProp_JsonInventory = new BusinessEntity.Inventory();
        objProp_JsonInventory.ID = Convert.ToInt32(EditID);


        try
        {
            objProp_JsonInventory = objBL_JsonInventory.GetInventoryByID(objProp_JsonInventory);



            if (objProp_JsonInventory != null)
            {


                jsonInventoryInformation.Header.HasError = false;
                jsonInventoryInformation.ReponseObject = objProp_JsonInventory;
            }
            else
            {
                jsonInventoryInformation.Header.HasError = true;

            }
        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.Header.ErrorMessages = strmsg;
        }
        return jsonInventoryInformation;
    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<RequestQuoteVendorJSON> GetApprovedVendorInfo(string strinvID, string Vendor)
    {
        WebMethodResponse<RequestQuoteVendorJSON> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<RequestQuoteVendorJSON>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();


        BL_Inventory objBL_JsonInventory = new BL_Inventory();

        int intinvID, vendorid = 0;
        Int32.TryParse(strinvID, out intinvID);
        Int32.TryParse(Vendor, out vendorid);



        RequestQuoteVendorJSON jsonitem = new RequestQuoteVendorJSON();

        try
        {
            jsonitem = JSONMappingUtility.RequestQuoteVendorMappingJSON(objBL_JsonInventory.GetInvManufacturerInfoByInvAndVendorId(intinvID, vendorid));
            jsonInventoryInformation.Header.HasError = false;
            jsonInventoryInformation.ReponseObject = jsonitem;

        }
        catch (Exception ex)
        {
            string errormsg = ex.Message;
            List<string> strmsg = new List<string>();
            strmsg.Add(errormsg);

            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.Header.ErrorMessages = strmsg;
        }
        return jsonInventoryInformation;
    }




    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static WebMethodResponse<string> SendMail(string ToEmail, string Quantity, string body)
    {
        WebMethodResponse<string> jsonInventoryInformation = new BusinessEntity.WebMethodResponse<string>();
        jsonInventoryInformation.Header = new BusinessEntity.WebMethodHeader();
        string response = string.Empty;


        string senderID = "rasmi.das11@gmail.com";// use sender’s email id here..
        const string senderPassword = "asspd1600m"; // sender password here…

        try
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com", // smtp server address here…
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                Timeout = 30000,
                UseDefaultCredentials = false,


            };
            MailMessage message = new MailMessage(senderID, "rasmi.das11@gmail.com", "hiii", "opopoiopio");
            smtp.Send(message);

            response = "Quote sent successfully!";
            jsonInventoryInformation.Header.HasError = false;
            jsonInventoryInformation.ReponseObject = response;

        }
        catch (Exception ex)
        {



            response = "Quote could not be sent!";
            jsonInventoryInformation.Header.HasError = true;
            jsonInventoryInformation.ReponseObject = response;

        }
        return jsonInventoryInformation;
    }
    #endregion





}



