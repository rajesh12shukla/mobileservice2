using BusinessEntity;
using BusinessLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class AddVendor : System.Web.UI.Page
{
    #region Variables
    BL_Vendor objBL_Vendor = new BL_Vendor();
    Vendor _objvendor = new Vendor();
   
    BL_BankAccount _objBLBank = new BL_BankAccount();
    Rol _objRol = new Rol();

    User _objPropUser = new User();
    BL_User _objBLUser = new BL_User();
    BL_Bills objBLBill = new BL_Bills();
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
            string _connectionString = Session["config"].ToString();

            if (!IsPostBack)
            {
                userpermissions();
                FillState();
                FillTerms();
                //FillCountry();
                if (Request.QueryString["id"] != null)  //Edit COA
                {
                    _objvendor.ConnConfig = _connectionString;
                    SetDataForEdit();
                    //lblAcctType.Text = ddlType.SelectedItem.Text;
                    //lblAcctNum.Text = txtAcctNum.Text;
                    //lblAcctName.Text = txtAcName.Text;

                    _objvendor.ConnConfig = Session["config"].ToString();
                    _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);
                    DataSet ds = objBLBill.GetAPExpenses(_objvendor);
                    gvTrans.DataSource = ds.Tables[0];
                    gvTrans.DataBind();
                    
                    
                    //foreach(DataListItem item in gvTrans.Items)
                    //{
                    //    if(item.ItemType == ListItemType.Footer)
                    //    {
                    //        Label lblFooterAmount = (Label)item.FindControl("lblFooterAmount");
                    //        if(ds.Tables[0].Rows.Count > 0)
                    //        {
                    //            lblFooterAmount.Text = string.Format("{0:c}", ds.Tables[0].Compute("sum(Amount)", string.Empty));
                    //        }
                    //    }
                    //}
                    
                }
                Permission();
            }
            txtGeolock.Visible = false;
            txtSince.Visible = false;
            txtLast.Visible = false;
            txtInuse.Visible = false;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
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
                _objPropUser.PageName = "addvendor.aspx";
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
    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("financeMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkFinanceMgr");
        //a.Style.Add("color", "#2382b2"); 

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkVendors");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderCstm");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("cstmMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
            //Response.Redirect("addcustomer.aspx?uid=" + Session["userid"].ToString());
        }

        if (Session["MSM"].ToString() == "TS")
        {
            Response.Redirect("home.aspx");
            //pnlGridButtons.Visible = false;
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }

        //DataTable dt = new DataTable();
        //dt = (DataTable)Session["userinfo"];

        //string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
        //if (ProgFunc == "N")
        //{
        //    Response.Redirect("home.aspx");
        //}
    }
    #endregion
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["id"] != null)
            {
                #region "EDIT Vendor"

                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.ID = Convert.ToInt32(hdnRolID.Value);         //Update Vandor
                _objRol.Name = txtName.Text;
                _objRol.Address = txtAddress.Text;
                _objRol.City = txtCity.Text;
                _objRol.State = ddlState.SelectedValue;
                _objRol.Country = txtCountry.Text;
                _objRol.EMail = txtEmailid.Text;
                _objRol.Website = txtWebsite.Text;
                _objRol.Zip = txtZip.Text;
                _objRol.Phone = txtPhone.Text;
                _objRol.Fax = txtFax.Text;
                _objRol.Contact = txtContact.Text;
                _objRol.Cellular = txtCellular.Text;
                _objRol.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _objRol.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Type = 1;
                
                _objBLBank.UpdateRol(_objRol);

                _objvendor.ConnConfig = Session["config"].ToString();
                _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);
                DataSet _dsIsAcctExitForEdit = objBL_Vendor.IsExistForUpdateVendor(_objvendor);
                int _count = Convert.ToInt32(_dsIsAcctExitForEdit.Tables[0].Rows[0]["CountVendor"]);
                if (_count.Equals(0))
                {
                    _objvendor.ConnConfig = Session["config"].ToString();
                    _objvendor.Acct = txtAccountid.Text;
                    DataSet _dsIsAcctExit = objBL_Vendor.IsExistForUpdateVendor(_objvendor);
                    int _count1 = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountVendor"]);
                    if (_count1.Equals(0))
                    {
                        GetVenderData();
                        if (ddlStatus.SelectedIndex != 0)
                        {
                            _objvendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                        }
                        else
                        {
                            _objvendor.Status = 0;
                        }
                        _objvendor.ShipVia = txtShipvia.Text;
                        _objvendor.Balance = Convert.ToDouble(txtBalance.Text);
                        
                        if (ddlTerms.SelectedIndex != 0)
                        {
                            _objvendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                        }
                        else
                        {
                            _objvendor.Terms = 1;
                        }
                        if (!string.IsNullOrEmpty(txtDays.Text))
                        {
                            _objvendor.Days = Convert.ToInt16(txtDays.Text);
                        }
                        _objvendor.Vendor1099 = Convert.ToInt16(txt1099.Text);
                        _objvendor.InUse = Convert.ToInt16(txtInuse.Text);
                        if (!string.IsNullOrEmpty(txtCreditlimit.Text))
                        {
                            _objvendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                        }
                        if (!string.IsNullOrEmpty(hdnAcctID.Value))
                        {
                            _objvendor.DA = Convert.ToInt32(hdnAcctID.Value);
                        }
                        _objvendor.Type = ddlType.SelectedItem.Text;             // change by Mayuri 9th May, 16
                        objBL_Vendor.UpdateVendor(_objvendor);

                        ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Vendor added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                        Response.Redirect("~/Vendors.aspx");
                    }
                    else
                    {
                        if (ddlType.SelectedItem.Text.Equals("Bank"))
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "resize controls", "ResizeControls('true');", true);
                        }
                        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This Acct# Number already exist.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                    }
                }
                #endregion

            }
            else
            {
                _objRol.ConnConfig = Session["config"].ToString();
                _objRol.Name = txtName.Text;
                _objRol.Address = txtAddress.Text;
                _objRol.City = txtCity.Text;
                _objRol.State = ddlState.SelectedItem.Value;
                _objRol.Country = txtCountry.Text;
                _objRol.EMail = txtEmailid.Text;
                _objRol.Website = txtWebsite.Text;
                _objRol.Zip = txtZip.Text;
                _objRol.Phone = txtPhone.Text;
                _objRol.Fax = txtFax.Text;
                _objRol.Contact = txtContact.Text;
                _objRol.Cellular = txtCellular.Text;
                _objRol.GeoLock = Convert.ToInt32(txtGeolock.Text);
                _objRol.Since = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Last = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                _objRol.Type = 1;                                           // change by Mayuri 9th May, 16

                _objvendor.Rol = _objBLBank.AddRol(_objRol);
                //_objvendor.Rol = Convert.ToInt32(_dsRol.Tables[0].Rows[0]["RolID"]);

                _objvendor.ConnConfig = Session["config"].ToString();
                _objvendor.Acct = txtAccountid.Text;
                DataSet _dsIsAcctExit = objBL_Vendor.IsExistsForInsertVendor(_objvendor);
                int _count1 = Convert.ToInt32(_dsIsAcctExit.Tables[0].Rows[0]["CountVendor"]);
                if (_count1.Equals(0))
                {
                    GetVenderData();
                    if (ddlStatus.SelectedIndex != 0)
                    {
                        _objvendor.Status = Convert.ToInt16(ddlStatus.SelectedValue);
                    }
                    else
                    {
                        _objvendor.Status = 0;
                    }
                    _objvendor.ShipVia = txtShipvia.Text;
                    if (string.IsNullOrEmpty(txtBalance.Text))
                        txtBalance.Text = "0.00";
                    _objvendor.Balance = Convert.ToDouble(txtBalance.Text);
                    if (!string.IsNullOrEmpty(txtCreditlimit.Text))
                    {
                        _objvendor.CLimit = Convert.ToDouble(txtCreditlimit.Text);
                    }
                    
                    if (ddlTerms.SelectedIndex != 0)
                    {
                        _objvendor.Terms = Convert.ToInt16(ddlTerms.SelectedValue);
                    }
                    else
                    {
                        _objvendor.Terms = 1;
                    }
                    if (!string.IsNullOrEmpty(txtDays.Text))
                    {
                        _objvendor.Days = Convert.ToInt16(txtDays.Text);
                    }
                    
                    _objvendor.Vendor1099 = Convert.ToInt16(txt1099.Text);
                    _objvendor.InUse = Convert.ToInt16(txtInuse.Text);
                    if(!string.IsNullOrEmpty(hdnAcctID.Value))
                    {
                        _objvendor.DA = Convert.ToInt32(hdnAcctID.Value);
                    }
                    _objvendor.Type = ddlType.SelectedItem.Text;     // change by Mayuri 9th May, 16
                    objBL_Vendor.AddVendor(_objvendor);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: 'Vendor added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //Response.Write("Insert Data....");
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: 'This Acct# Number already exist.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("Vendors.aspx");
    }
    #endregion

    #region Custom Functions
    public void SetDataForEdit()  //EDIT Vendor
    {
        try
        {
            lblHeader.Text = "Edit Vendor";

            DataSet _dsVender = new DataSet();

            _objvendor.ID = Convert.ToInt32(Request.QueryString["id"]);

            _dsVender = objBL_Vendor.GetVendorEdit(_objvendor);

            txtAccountid.Text = _dsVender.Tables[0].Rows[0]["Acct"].ToString();
            txtName.Text = _dsVender.Tables[0].Rows[0]["Name"].ToString();
            txtAddress.Text = _dsVender.Tables[0].Rows[0]["Address"].ToString();
            txtCity.Text = _dsVender.Tables[0].Rows[0]["City"].ToString();
            ddlState.SelectedValue = _dsVender.Tables[0].Rows[0]["State"].ToString();
            txtCountry.Text = _dsVender.Tables[0].Rows[0]["Country"].ToString();
            txtEmailid.Text = _dsVender.Tables[0].Rows[0]["EMail"].ToString();
            txtWebsite.Text = _dsVender.Tables[0].Rows[0]["Website"].ToString();
            txtZip.Text = _dsVender.Tables[0].Rows[0]["Zip"].ToString();
            txtPhone.Text = _dsVender.Tables[0].Rows[0]["Phone"].ToString();
            txtFax.Text = _dsVender.Tables[0].Rows[0]["Fax"].ToString();
            txtContact.Text = _dsVender.Tables[0].Rows[0]["Contact"].ToString();
            txtCellular.Text = _dsVender.Tables[0].Rows[0]["Cellular"].ToString();
            txtGeolock.Text = _dsVender.Tables[0].Rows[0]["GeoLock"].ToString();
            txtSince.Text = _dsVender.Tables[0].Rows[0]["Since"].ToString();
            txtLast.Text = _dsVender.Tables[0].Rows[0]["Last"].ToString();
            if(!string.IsNullOrEmpty(_dsVender.Tables[0].Rows[0]["Type"].ToString()))
            {
                ddlType.SelectedValue = _dsVender.Tables[0].Rows[0]["Type"].ToString();
            }
            
            ddlStatus.SelectedValue = _dsVender.Tables[0].Rows[0]["Status"].ToString();
            txtShipvia.Text = _dsVender.Tables[0].Rows[0]["ShipVia"].ToString();
            txtBalance.Text = _dsVender.Tables[0].Rows[0]["Balance"].ToString();
            txtCreditlimit.Text = _dsVender.Tables[0].Rows[0]["CLimit"].ToString();
            ddlTerms.SelectedValue = _dsVender.Tables[0].Rows[0]["Terms"].ToString();
            txtDays.Text = _dsVender.Tables[0].Rows[0]["Days"].ToString();
            txt1099.Text = _dsVender.Tables[0].Rows[0]["1099"].ToString();
            txtInuse.Text = _dsVender.Tables[0].Rows[0]["InUse"].ToString();
            txtDefaultAcct.Text = _dsVender.Tables[0].Rows[0]["DefaultAcct"].ToString();
            hdnAcctID.Value = _dsVender.Tables[0].Rows[0]["DA"].ToString();
            hdnRolID.Value = _dsVender.Tables[0].Rows[0]["Rol"].ToString();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void GetVenderData()
    {
        try
        {
            _objvendor.Acct = txtAccountid.Text;
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillState()
    {
        try
        {
            if (ddlState.SelectedIndex != 0)
            {
                DataSet _dsState = new DataSet();
                State _objState = new State();

                _objState.ConnConfig = Session["config"].ToString();

                _dsState = _objBLBank.GetStates(_objState);

                ddlState.Items.Add(new ListItem("Select State"));
                ddlState.AppendDataBoundItems = true;
                ddlState.DataSource = _dsState;
                ddlState.DataValueField = "Name";
                ddlState.DataTextField = "fDesc";
                ddlState.DataBind();
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private void FillTerms()
    {
        try
        {
            DataSet ds = new DataSet();
            _objPropUser.ConnConfig = Session["config"].ToString();

            ds = _objBLUser.getTerms(_objPropUser);

            ddlTerms.DataSource = ds.Tables[0];
            ddlTerms.DataTextField = "name";
            ddlTerms.DataValueField = "id";
            ddlTerms.DataBind();

            ddlTerms.Items.Insert(0, new ListItem(":: Select ::", ""));
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    //private void FillCountry()
    //{
    //    try
    //    {
    //        if (ddlCountry.SelectedIndex != 0)
    //        {
    //            //DataSet _dsState = new DataSet();
    //            //State _objState = new State();

    //            //_objState.ConnConfig = Session["config"].ToString();

    //            //_dsState = _objBLBank.GetStates(_objState);

    //            //ddlState.Items.Add(new ListItem(" "));
    //            //ddlState.AppendDataBoundItems = true;

    //            //ddlState.DataSource = _dsState;
    //            //ddlState.DataValueField = "Name";
    //            //ddlState.DataTextField = "fDesc";
    //            ddlCountry.Items.Add(new ListItem("United States"));
    //            ddlCountry.DataBind();
    //        }
    //        else
    //        {

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
    //    }
    //}
    protected void gvTrans_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblId = (Label)e.Item.FindControl("lblId");
            Label lblType = (Label)e.Item.FindControl("lblType");
            LinkButton lnkId = (LinkButton)e.Item.FindControl("lnkId");
            if (lblType.Text.Equals("Bill"))
            {
                //lnkId.Click = Response.Redirect();
                lnkId.OnClientClick = "window.open('addbills.aspx?id=" + lblId.Text + "');";
            }
            else
            {
                lnkId.OnClientClick = "window.open('editcheck.aspx?id=" + lblId.Text + "');";
            }
        }
    }
}