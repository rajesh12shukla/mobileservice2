using BusinessEntity;
using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AddReceivePO : System.Web.UI.Page
{
    #region Variable
    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();

    Vendor _objVendor = new Vendor();
    BL_Vendor _objBLVendor = new BL_Vendor();

    PO _objPO = new PO();
    BL_Bills _objBLBills = new BL_Bills();

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
            if (!IsPostBack)
            {
                divSuccess.Visible = false;
                txtDate_CalendarExtender.StartDate = DateTime.Now;
                lblTotal.Text = "$0.00";
                if (Request.QueryString["id"] != null)
                {   
                    lblHeader.Text = "PO Reception";
                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.RID = Convert.ToInt32(Request.QueryString["id"]);
                    DataSet ds = _objBLBills.GetReceivePoById(_objPO);
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtReception.Text = dr["ID"].ToString();
                    txtPO.Text = dr["PO"].ToString();
                    txtDueDate.Text = Convert.ToDateTime(dr["Due"]).ToString("MM/dd/yyyy");
                    if (!string.IsNullOrEmpty(dr["ReceiveDate"].ToString()))
                    {
                        txtDate.Text = Convert.ToDateTime(dr["ReceiveDate"]).ToString("MM/dd/yyyy");
                    }
                    txtRef.Text = dr["Ref"].ToString();
                    txtVendor.Text = dr["VendorName"].ToString();
                    hdnVendorID.Value = dr["Vendor"].ToString();
                    txtTrkWB.Text = dr["WB"].ToString();
                    txtShipTo.Text = dr["ShipTo"].ToString();
                    txtCreatedBy.Text = dr["fby"].ToString();
                    txtRcomments.Text = dr["Comments"].ToString();
                    txtComments.Text = dr["fDesc"].ToString();
                    txtAddress.Text = dr["Address"].ToString();
                    
                    lblTotal.Text = "$"+string.Format("{0:c}", dr["ReceivedAmount"].ToString());
                    
                    string status = "New";
                    switch (Convert.ToInt16(dr["Status"]))
                    {
                        case 0:
                            status = "New";
                            break;
                        case 1:
                            status = "Closed";
                            break;
                        case 2:
                            status = "Cancelled";
                            break;
                        case 3:
                            status = "Partial-Quantity";
                            break;
                        case 4:
                            status = "Partial-Amount";
                            break;
                    }
                    txtStatus.Text = status;
                    ViewState["PO"] = ds.Tables[0];

                    txtReception.Enabled = false;
                    txtPO.Enabled = false;
                    txtDate.Enabled = false;
                    txtRef.Enabled = false;
                    txtVendor.Enabled = false;
                    txtTrkWB.Enabled = false;
                    txtShipTo.Enabled = false;
                    txtCreatedBy.Enabled = false;
                    txtComments.Enabled = false;
                    txtAddress.Enabled = false;
                    txtStatus.Enabled = false;
                    txtRcomments.Enabled = false;

                    gvPO.DataSource = ds.Tables[0];
                    gvPO.DataBind();
                    foreach (GridViewRow gr in gvPO.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                        
                        chkSelect.Checked = true;
                    }

                    gvPOItems.DataSource = ds.Tables[1];
                    gvPOItems.DataBind();
                    if(dr["Status"].ToString().Equals(1))
                    {
                        btnSubmit.Visible = false;
                    }
                    
                    foreach (GridViewRow gr in gvPOItems.Rows)
                    {
                        CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
                        TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");
                        TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");

                        chkSelect.Checked = true;
                        txtReceive.Enabled = false;
                        txtReceiveQty.Enabled = false;
                    }

                }
                else
                {
                    txtCreatedBy.Text = Session["Username"].ToString();
                    _objPO.ConnConfig = Session["config"].ToString();
                    int id = _objBLBills.GetMaxReceivePOId(_objPO);

                    txtDate.Text = DateTime.Now.ToShortDateString();
                    txtReception.Text = id.ToString();
                    txtReception.Enabled = false;
                    //txtDueDate.Enabled = false;
                    txtCreatedBy.Enabled = false;
                    txtAddress.Enabled = false;
                    txtShipTo.Enabled = false;
                    txtStatus.Enabled = false;
                    txtComments.Enabled = false;

                    _objPO.ConnConfig = Session["config"].ToString();
                    DataSet ds = _objBLBills.GetAllPOByDue(_objPO);
                    ViewState["PO"] = ds.Tables[0];

                    gvPO.DataSource = ds.Tables[0];
                    gvPO.DataBind();

                    gvPOItems.DataSource = "";
                    gvPOItems.DataBind();
                }
                
                FillUserAddress();
            }
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool _flag = false;
            GetPeriodDetails(Convert.ToDateTime(txtDate.Text));
            _flag = (bool)ViewState["FlagPeriodClose"];
            bool IsExist;

            if (_flag)
            {
                if (Request.QueryString["id"] == null)
                {
                    if (ValidateGrid())
                    {
                        _objPO.ConnConfig = Session["config"].ToString();
                        _objPO.Ref = txtRef.Text;
                        IsExist = _objBLBills.IsExistRPOForInsert(_objPO);
                        if (IsExist.Equals(false))
                        {
                            double amount = 0;
                            double poAmount = 0;
                            double poQty = 0;
                            double qty = 0;

                            foreach (GridViewRow gv in gvPOItems.Rows)
                            {
                                Label lblBOQty = (Label)gv.FindControl("lblBOQty");
                                Label lblOutstand = (Label)gv.FindControl("lblOutstand");

                                poQty = poQty + double.Parse(lblBOQty.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowThousands |
                                                    NumberStyles.AllowDecimalPoint);

                                poAmount = poAmount + double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowThousands |
                                                    NumberStyles.AllowDecimalPoint);

                            }
                            bool IsAmount = false;
                            int countItem = 0;
                            #region Update PO balance
                            foreach (GridViewRow gr in gvPOItems.Rows)
                            {
                                Label lblOutstand = (Label)gr.FindControl("lblOutstand");
                                TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");
                                TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");
                                Label lblLine = (Label)gr.FindControl("lblLine");
                                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");

                                if (chkSelect.Checked.Equals(true))
                                {
                                    _objPO.POID = Convert.ToInt32(txtPO.Text);
                                    _objPO.Line = Convert.ToInt16(lblLine.Text);
                                    _objPO.ConnConfig = Session["config"].ToString();
                                    if (!Convert.ToDouble(txtReceive.Text).Equals(0))
                                    {
                                        _objPO.Balance = double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowThousands |
                                                    NumberStyles.AllowDecimalPoint) - Convert.ToDouble(txtReceive.Text);
                                        _objPO.Selected = Convert.ToDouble(txtReceive.Text);

                                        _objBLBills.UpdatePOItemBalance(_objPO);

                                        amount = amount + Convert.ToDouble(txtReceive.Text);
                                        IsAmount = true;
                                    }
                                    else if (!Convert.ToDouble(txtReceiveQty.Text).Equals(0))
                                    {

                                        Label lblBOQty = (Label)gr.FindControl("lblBOQty");

                                        _objPO.BalanceQuan = double.Parse(lblBOQty.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
                                                    NumberStyles.AllowThousands |
                                                    NumberStyles.AllowDecimalPoint) - Convert.ToDouble(txtReceiveQty.Text);
                                        _objPO.SelectedQuan = Convert.ToDouble(txtReceiveQty.Text);

                                        _objBLBills.UpdatePOItemQuan(_objPO);

                                        qty = qty + Convert.ToDouble(txtReceiveQty.Text);
                                    }
                                    _objPO.ConnConfig = Session["config"].ToString();
                                    _objPO.Quan = Convert.ToDouble(txtReceiveQty.Text);
                                    _objPO.Amount = Convert.ToDouble(txtReceive.Text);
                                    _objPO.Line = Convert.ToInt16(lblLine.Text);
                                    _objPO.ReceivePOId = Convert.ToInt32(txtReception.Text);

                                    _objBLBills.AddReceivePOItem(_objPO);
                                }
                                countItem++;
                            }
                            #endregion

                            #region Update PO status
                            _objPO.POID = Convert.ToInt32(txtPO.Text);

                            _objPO.Status = 1;

                            if (IsAmount)
                            {
                                if (amount < poAmount)
                                {
                                    _objPO.Status = 4;
                                }
                                else if (amount.Equals(poAmount))
                                {
                                    _objPO.Status = 5;      // closed at receive po level
                                }
                            }
                            else
                            {
                                if (qty < poQty)
                                {
                                    _objPO.Status = 3;
                                }
                                else if (qty.Equals(poQty))
                                {
                                    _objPO.Status = 5;      // closed at receive po level
                                }
                            }
                            _objBLBills.UpdatePOStatus(_objPO);
                            #endregion

                            #region Add Reception PO

                            _objPO.ConnConfig = Session["config"].ToString();
                            _objPO.RID = Convert.ToInt32(txtReception.Text);
                            _objPO.POID = Convert.ToInt32(txtPO.Text);
                            _objPO.Ref = txtRef.Text;
                            _objPO.WB = txtTrkWB.Text;
                            _objPO.Comments = txtRcomments.Text;
                            _objPO.Amount = amount;
                            _objPO.fDate = Convert.ToDateTime(txtDate.Text);
                            _objBLBills.AddReceivePO(_objPO);

                            _objPO.ConnConfig = Session["config"].ToString();
                            _objPO.POID = Convert.ToInt32(txtPO.Text);
                            _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                            _objBLBills.UpdatePODue(_objPO);
                            #endregion

                            Response.Redirect(Request.RawUrl, false);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "warningRef", "noty({text: 'Ref number already exists, Please use different Ref number!',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyedit", "noty({text: 'You must fill out atleast one line.',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                else
                {
                    _objPO.ConnConfig = Session["config"].ToString();
                    _objPO.POID = Convert.ToInt32(txtPO.Text);
                    _objPO.Due = Convert.ToDateTime(txtDueDate.Text);
                    _objBLBills.UpdatePODue(_objPO);
                    Response.Redirect(Request.RawUrl, false);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'warning', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("managereceivepo.aspx");
    }
    protected void btnSelectVendor_Click(object sender, EventArgs e)
    {
        FillAddress();
    }
    protected void txtVendor_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if(!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                DataSet ds = new DataSet();
                _objPO.ConnConfig = Session["config"].ToString();
                _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);

                ds = _objBLBills.GetPOByVendor(_objPO);
                
                ViewState["PO"] = ds.Tables[0];
                gvPO.DataSource = ds.Tables[0];
                gvPO.DataBind();

                gvPOItems.DataSource = ds.Tables[1];
                gvPOItems.DataBind();
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkSelect = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chkSelect.NamingContainer;
            Label lblID = (Label)row.FindControl("lblID");
            Label lblDue = (Label)row.FindControl("lblDue");
            Label lblfDate = (Label)row.FindControl("lblfDate");
            Label lblAmount = (Label)row.FindControl("lblAmount");
            Label lblComment = (Label)row.FindControl("lblComment");
            Label lblVendorName = (Label)row.FindControl("lblVendorName");
            Label lblVendor = (Label)row.FindControl("lblVendor");
            Label lblAddress = (Label)row.FindControl("lblAddress");
            Label lblStatus = (Label)row.FindControl("lblStatus");

            if (chkSelect.Checked.Equals(true))
            {
                _objPO.ConnConfig = Session["config"].ToString();
                _objPO.POID = Convert.ToInt32(lblID.Text);
                DataSet ds = _objBLBills.GetPOItemByPO(_objPO);
                gvPOItems.DataSource = ds.Tables[0];
                gvPOItems.DataBind();
            }

            foreach (GridViewRow gr in gvPO.Rows)
            {
                CheckBox chk = (CheckBox)gr.FindControl("chkSelect");
                chk.Checked = false;
            }

            chkSelect.Checked = true;
            txtVendor.Text = lblVendorName.Text;
            hdnVendorID.Value = lblVendor.Text;
            txtAddress.Text = lblAddress.Text;
            txtPO.Text = lblID.Text;
            txtDueDate.Text = lblDue.Text;
            //txtDate.Text = lblfDate.Text;
            hdnAmount.Value = lblAmount.Text;
            txtComments.Text = lblComment.Text;
            string status = "New";
            switch (Convert.ToInt16(lblStatus.Text))
            {
                case 0:
                    status = "New";
                    break;
                case 1:
                    status = "Closed";
                    break;
                case 2:
                    status = "Cancelled";
                    break;
                case 3:
                    status = "Partial-Quantity";
                    break;
                case 4:
                    status = "Partial-Amount";
                    break;
            }
            txtStatus.Text = status;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //protected void chkSelect_CheckedChanged1(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        CheckBox chkSelect = (CheckBox)sender;
    //        GridViewRow row = (GridViewRow)chkSelect.NamingContainer;
    //        Label lblOrdered = (Label)row.FindControl("lblOrdered");
    //        Label lblPrvIn = (Label)row.FindControl("lblPrvIn");
    //        Label lblOutstand = (Label)row.FindControl("lblOutstand");
    //        TextBox txtReceive = (TextBox)row.FindControl("txtReceive");
    //        TextBox txtReceiveQty = (TextBox)row.FindControl("txtReceiveQty");
    //        bool IsAmount = false;

    //        if(!string.IsNullOrEmpty(txtReceiveQty.Text))
    //        {
    //            if(Convert.ToDouble(txtReceiveQty.Text).Equals(0))
    //            {
    //                IsAmount = true;
    //            }
    //        }
    //        else
    //            IsAmount = true;

    //        if(IsAmount)
    //        {
    //            double _dueBalance = double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                                               NumberStyles.AllowThousands |
    //                                               NumberStyles.AllowDecimalPoint);

    //            txtReceive.Text = _dueBalance.ToString("0.00", CultureInfo.InvariantCulture);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow gvrPager = gvPOItems.BottomPagerRow;
        DropDownList ddlPages = (DropDownList)gvrPager.Cells[0].FindControl("ddlPages");

        gvPO.PageIndex = ddlPages.SelectedIndex;

        // a method to populate your grid

    }
    protected void btnSelectPO_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                DataSet ds = new DataSet();
                _objPO.ConnConfig = Session["config"].ToString();
                _objPO.Vendor = Convert.ToInt32(hdnVendorID.Value);

                ds = _objBLBills.GetPOByVendor(_objPO);

                ViewState["PO"] = ds.Tables[0];
                gvPO.DataSource = ds.Tables[0];
                gvPO.DataBind();
            }

            _objPO.ConnConfig = Session["config"].ToString();
            _objPO.POID = Convert.ToInt32(txtPO.Text);
            DataSet dsItem = _objBLBills.GetPOItemByPO(_objPO);
            gvPOItems.DataSource = dsItem.Tables[0];
            gvPOItems.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    //protected void txtReceive_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtReceive = (TextBox)sender;
    //        GridViewRow row = (GridViewRow)txtReceive.NamingContainer;
    //        TextBox txtReceiveQty = (TextBox)row.FindControl("txtReceiveQty");
    //        Label lblOutstand = (Label)row.FindControl("lblOutstand");
    //        CheckBox chk = (CheckBox)row.FindControl("chkSelectItem");

    //        if (!string.IsNullOrEmpty(txtReceive.Text))
    //        {
    //            if (Convert.ToDouble(txtReceive.Text) > 0)
    //            {
    //                txtReceiveQty.Text = "0.00";
    //                double outstandVal = double.Parse(lblOutstand.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //                if (Convert.ToDouble(txtReceive.Text) > outstandVal)
    //                {
    //                    txtReceive.Text = outstandVal.ToString("0.00", CultureInfo.InvariantCulture);
    //                }
                    
    //                foreach(GridViewRow gr in gvPOItems.Rows)
    //                {
    //                    TextBox txtReceiveQty1 = (TextBox)gr.FindControl("txtReceiveQty");
    //                    TextBox txtReceive1 = (TextBox)gr.FindControl("txtReceive");
    //                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
    //                    txtReceiveQty1.Text = "0.00";
    //                    if (chkSelect.Checked.Equals(true))
    //                    {
    //                        if (Convert.ToDouble(txtReceive1.Text).Equals(0))
    //                        {
    //                            chkSelect.Checked = false;
    //                        }
    //                    }
    //                }
    //                chk.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    //protected void txtReceiveQty_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        TextBox txtReceiveQty = (TextBox)sender;
    //        GridViewRow row = (GridViewRow)txtReceiveQty.NamingContainer;
    //        TextBox txtReceive = (TextBox)row.FindControl("txtReceive");
    //        Label lblBOQty = (Label)row.FindControl("lblBOQty");
    //        CheckBox chk = (CheckBox)row.FindControl("chkSelect");

    //        if (!(string.IsNullOrEmpty(txtReceiveQty.Text)))
    //        {
    //            if (Convert.ToDouble(txtReceiveQty.Text) > 0)
    //            {
    //                txtReceive.Text = "0.00";
    //                double outstandQty = double.Parse(lblBOQty.Text.Replace('$', '0'), NumberStyles.AllowParentheses |
    //                              NumberStyles.AllowThousands |
    //                              NumberStyles.AllowDecimalPoint);
    //                if (Convert.ToDouble(txtReceiveQty.Text) > outstandQty)
    //                {
    //                    txtReceiveQty.Text = outstandQty.ToString("0.00", CultureInfo.InvariantCulture);
    //                }
                    
    //                foreach (GridViewRow gr in gvPOItems.Rows)
    //                {
    //                    TextBox txtReceive1 = (TextBox)gr.FindControl("txtReceive");
    //                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
    //                    TextBox txtReceiveQty1 = (TextBox)gr.FindControl("txtReceiveQty");
    //                    txtReceive1.Text = "0.00";
    //                    if(chkSelect.Checked.Equals(true))
    //                    {
    //                        if(Convert.ToDouble(txtReceiveQty1.Text).Equals(0))
    //                        {
    //                            chkSelect.Checked = false;
    //                        }
    //                    }
    //                }
    //                chk.Checked = true;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
    #endregion

    #region Custom function
    private void FillAddress()
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnVendorID.Value))
            {
                _objVendor.ConnConfig = Session["config"].ToString();
                _objVendor.ID = Convert.ToInt32(hdnVendorID.Value);
                DataSet ds = new DataSet();
                ds = _objBLVendor.GetVendorRolDetails(_objVendor);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["city"].ToString() + ", " + ds.Tables[0].Rows[0]["State"].ToString() + ", " + ds.Tables[0].Rows[0]["Zip"].ToString();
                }
                else
                {
                    txtAddress.Text = "";
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillUserAddress()
    {
        try
        {
            DataSet dsC = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();
            dsC = _objBLUser.getControl(_objPropUser);

            string address;
            address = dsC.Tables[0].Rows[0]["Address"].ToString() + ", " + Environment.NewLine;
            address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
            txtShipTo.Text = address;
        }
        catch(Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetPeriodDetails(DateTime _transDate)
    {
        bool _flag = CommonHelper.GetPeriodDetails(_transDate);
        ViewState["FlagPeriodClose"] = _flag;
        if (!_flag)
        {
            divSuccess.Visible = true;
        }
    }
    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                _objPropUser.ConnConfig = Session["config"].ToString();
                _objPropUser.Username = Session["username"].ToString();
                _objPropUser.PageName = "addreceivepo.aspx";
                DataSet dspage = _objBLUser.getScreensByUser(_objPropUser);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }
    private bool ValidateGrid()
    {
        bool IsValid = false;
        int count = 0;
        foreach (GridViewRow gr in gvPOItems.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelectItem");
            TextBox txtReceive = (TextBox)gr.FindControl("txtReceive");
            TextBox txtReceiveQty = (TextBox)gr.FindControl("txtReceiveQty");

            if(chkSelect.Checked.Equals(true))
            {
                if(!string.IsNullOrEmpty(txtReceive.Text))
                {
                    if(Convert.ToDouble(txtReceive.Text) > 0)
                    {
                        count++;
                    }
                }
                if(!string.IsNullOrEmpty(txtReceiveQty.Text))
                {
                    if(Convert.ToDouble(txtReceiveQty.Text) > 0)
                    {
                        count++;
                    }
                }
            }
        }
        if(count > 0)
        {
            IsValid = true;
        }
        return IsValid;
    }
    #endregion
}