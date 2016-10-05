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
using OpenPop.Pop3;
using OpenPop.Pop3.Exceptions;
using ImapX;

public partial class AddUser : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    BL_Contracts objBL_Contracts = new BL_Contracts();
    Contracts objProp_Contracts = new Contracts();

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["sup"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            userpermissions();
            ViewState["mode"] = 0;
            ViewState["super"] = 0;

            if (Request.QueryString["sup"] != null)
            {
                ViewState["super"] = 1;
                lblHeader.Text = "Add Supervisor";
                ddlSuper.Enabled = false;
                lnkClose.Visible = false;
                lnkCancelContact.Visible = true;
                btnSubmit.Visible = true;
                ddlUserType.SelectedValue = "1";
                ddlUserType_SelectedIndexChanged(sender, e);
                ddlUserType.Enabled = false;
                chkSuper.Checked = true;
                chkSuper.Enabled = false;
            }

            FillDepartment();
            FillSupervisor();
            GetControl();
            GetMerchantID();
            FillPages();

            if (Request.QueryString["uid"] != null)
            {
                if (Request.QueryString["t"] != null)
                {
                    ViewState["mode"] = 0;
                }
                else
                {
                    //lblAddEditUser.Text = "Edit User";
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit User";
                    txtUserName.ReadOnly = true;
                }
                pnlNext.Visible = true;
                objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
                objPropUser.TypeID = Convert.ToInt32(Request.QueryString["type"]);
                objPropUser.DBName = Session["dbname"].ToString();
                DataSet ds = new DataSet();
                ds = objBL_User.getUserByID(objPropUser);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Request.QueryString["type"].ToString() == "2" && Request.QueryString["t"] == null)
                    {
                        ddlUserType.Items.Insert(2, new ListItem("Customer", "2"));
                        tblPermission.Visible = false;
                        txtHireDt.Visible = false;
                        txtTerminationDt.Visible = false;
                        //hire.Visible = false;
                        //fire.Visible = false;
                        ddlUserType.SelectedIndex = 2;
                        ddlUserType.Enabled = false;
                        chkMap.AutoPostBack = false;
                        btnSubmit.Visible = false;
                        chkSalesperson.Visible = false;
                        lblSales.Visible = false;
                        if (ds.Tables[0].Rows[0]["ticketo"].ToString() == "1")
                        {
                            chkScheduleBrd.Checked = true;
                        }
                        if (ds.Tables[0].Rows[0]["ticketd"].ToString() == "1")
                        {
                            chkMap.Checked = true;
                        }
                        ddlLang.Visible = false;
                        lblMultiLang.Visible = false;
                    }
                    else
                    {
                        string map = ds.Tables[0].Rows[0]["ticket"].ToString().Substring(3, 1);
                        string sch = ds.Tables[0].Rows[0]["ticket"].ToString().Substring(0, 1);

                        if (map == "Y")
                        {
                            chkMap.Checked = true;
                        }
                        //if (sch == "Y")
                        //{
                        //    chkScheduleBrd.Checked = true;
                        //}
                        //if (Session["MSM"].ToString() == "TS")
                        //{
                        if (ds.Tables[0].Rows[0]["dboard"] != DBNull.Value)
                        {
                            if (ds.Tables[0].Rows[0]["dboard"].ToString().Trim() != string.Empty)
                            {
                                int schTS = Convert.ToInt32(ds.Tables[0].Rows[0]["dboard"]);
                                if (schTS == 1)
                                {
                                    chkScheduleBrd.Checked = true;
                                }
                                else
                                {
                                    chkScheduleBrd.Checked = false;
                                }
                            }
                        }
                        //}
                        txtMsg.Text = ds.Tables[0].Rows[0]["pager"].ToString();
                        string lang = "english";
                        if (ds.Tables[0].Rows[0]["Lang"].ToString().ToLower() != "none")
                        {
                            lang = ds.Tables[0].Rows[0]["Lang"].ToString().ToLower();
                        }
                        ddlLang.SelectedValue = lang;
                        ddlMerchantID.SelectedValue = ds.Tables[0].Rows[0]["merchantinfoid"].ToString();
                        if (Session["MSM"].ToString() != "TS")
                        {
                            if (ds.Tables[0].Rows[0]["sales"].ToString() == "1")
                            {
                                chkSalesperson.Checked = true;
                                chkEmailAcc.Enabled = true;
                                if (Convert.ToInt16(ds.Tables[0].Rows[0]["emailaccount"]) == 1)
                                {
                                    chkEmailAcc.Checked = true;
                                    pnlEmailAccount.Visible = true;
                                }
                            }
                            else
                            {
                                chkEmailAcc.Enabled = false;
                            }
                        }
                    }

                    ViewState["userid"] = ds.Tables[0].Rows[0]["userid"].ToString();
                    ViewState["rolid"] = ds.Tables[0].Rows[0]["rolid"].ToString();
                    ViewState["empid"] = ds.Tables[0].Rows[0]["empid"].ToString();
                    ViewState["workid"] = ds.Tables[0].Rows[0]["workid"].ToString();

                    txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtCell.Text = ds.Tables[0].Rows[0]["Cellular"].ToString();
                    txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    if (ds.Tables[0].Rows[0]["DFired"].ToString() != string.Empty)
                    {
                        txtTerminationDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DFired"].ToString()).ToShortDateString();
                    }
                    if (ds.Tables[0].Rows[0]["DHired"].ToString() != string.Empty)
                    {
                        txtHireDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["DHired"].ToString()).ToShortDateString();
                    }
                    txtEmail.Text = ds.Tables[0].Rows[0]["EMail"].ToString();
                    txtFName.Text = ds.Tables[0].Rows[0]["fFirst"].ToString();
                    txtLName.Text = ds.Tables[0].Rows[0]["Last"].ToString();
                    if (ViewState["mode"].ToString() != "0")
                        lblUserName.Text = ds.Tables[0].Rows[0]["fUser"].ToString();
                    txtMName.Text = ds.Tables[0].Rows[0]["Middle"].ToString();
                    txtPassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    ddlState.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
                    rbStatus.SelectedValue = ds.Tables[0].Rows[0]["Status"].ToString();
                    txtTelephone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtUserName.Text = ds.Tables[0].Rows[0]["fUser"].ToString();
                    txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    ddlUserType.SelectedValue = ds.Tables[0].Rows[0]["Field"].ToString();
                    txtRemarks.Text = ds.Tables[0].Rows[0]["Remarks"].ToString();
                    txtDeviceID.Text = ds.Tables[0].Rows[0]["PDASerialNumber"].ToString();
                    chkDefaultWorker.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DefaultWorker"]);
                    chkMassReview.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["massreview"]);
                    txtHourlyRate.Text = ds.Tables[0].Rows[0]["hourlyrate"].ToString();
                    //txtMOMUserName.Text = ds.Tables[0].Rows[0]["msmuser"].ToString();
                    //txtMOMPassword.Text = ds.Tables[0].Rows[0]["msmpass"].ToString();
                    ddlPayMethod.SelectedValue = ds.Tables[0].Rows[0]["pmethod"].ToString();
                    ddlPayMethod_SelectedIndexChanged1(sender, e);
                    txtHours.Text = ds.Tables[0].Rows[0]["phour"].ToString();
                    txtAmount.Text = ds.Tables[0].Rows[0]["salary"].ToString();
                    txtMileageRate.Text = ds.Tables[0].Rows[0]["mileagerate"].ToString();
                    txtEmpID.Text = ds.Tables[0].Rows[0]["ref"].ToString();
                    ddlPayPeriod.SelectedValue = ds.Tables[0].Rows[0]["payperiod"].ToString();
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fStart"].ToString()))
                    {
                        txtStartDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fStart"]).ToString("MM/dd/yyyy");
                    }
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["fEnd"].ToString()))
                    {
                        txtEndDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["fEnd"]).ToString("MM/dd/yyyy");
                    }

                    if (ds.Tables[1] != null)
                    {
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            txtInServer.Text = ds.Tables[1].Rows[0]["InServer"].ToString();
                            txtInUSername.Text = ds.Tables[1].Rows[0]["InUsername"].ToString();
                            txtInPassword.Text = ds.Tables[1].Rows[0]["InPassword"].ToString();
                            txtinPort.Text = ds.Tables[1].Rows[0]["InPort"].ToString();
                            txtOutServer.Text = ds.Tables[1].Rows[0]["OutServer"].ToString();
                            txtOutUsername.Text = ds.Tables[1].Rows[0]["OutUsername"].ToString();
                            txtOutPassword.Text = ds.Tables[1].Rows[0]["OutPassword"].ToString();
                            txtOutPort.Text = ds.Tables[1].Rows[0]["OutPort"].ToString();
                            chkSSL.Checked = Convert.ToBoolean(ds.Tables[1].Rows[0]["SSL"].ToString());
                        }
                    }
                    if (ds.Tables[2] != null)
                    {

                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            foreach (ListItem li in ddlDepartment.Items)
                            {
                                if (dr["department"].ToString().Equals(li.Value))
                                    li.Selected = true;
                            }
                        }

                    }

                    if (ddlUserType.SelectedValue == "1")
                    {
                        pnlWorker.Enabled = true;
                        chkMap.Enabled = false;
                        chkMap.Checked = true;
                        chkMap_CheckedChanged(sender, e);

                        int sup = 0;
                        objPropUser.ConnConfig = Session["config"].ToString();
                        objPropUser.Username = ds.Tables[0].Rows[0]["fuser"].ToString();
                        sup = objBL_User.getLoginSuper(objPropUser);
                        if (sup == 1)
                        //if (ds.Tables[0].Rows[0]["fuser"].ToString() == ds.Tables[0].Rows[0]["super"].ToString())
                        //if (ds.Tables[0].Rows[0]["fuser"].ToString() == Session["username"].ToString() && Convert.ToInt32(Session["ISsupervisor"]) == 1)
                        {
                            ViewState["super"] = 1;
                            lblSuper.Enabled = false;
                            ddlSuper.Enabled = false;
                            pnlGrid.Visible = true;
                            chkSuper.Checked = true;
                            objPropUser.Username = ds.Tables[0].Rows[0]["fuser"].ToString();
                            int superv = objBL_User.getISSuper(objPropUser);
                            if (superv != 0)
                                chkSuper.Enabled = false;
                            else
                                chkSuper.Enabled = true;
                        }
                        else
                        {
                            ViewState["super"] = 0;
                            lblSuper.Enabled = true;
                            ddlSuper.Enabled = true;
                        }

                        ddlSuper.SelectedValue = ds.Tables[0].Rows[0]["super"].ToString().ToUpper();
                        //if (ds.Tables[0].Rows[0]["sales"].ToString() == "1")
                        //{
                        //    chkSalesperson.Checked = true;
                        //}
                        //else
                        //{
                        //    chkSalesperson.Checked = false;
                        //}
                    }

                    //if (ds.Tables[0].Rows[0]["PDA"].ToString() == "0")
                    //{
                    //    chkPDA.Checked = false;
                    //}
                    //else
                    //{
                    //    chkPDA.Checked = true;
                    //}

                    string crtTckt = ds.Tables[0].Rows[0]["Dispatch"].ToString().Substring(0, 1);
                    string wrkDt = ds.Tables[0].Rows[0]["Dispatch"].ToString().Substring(1, 1);
                    string LocnRemark = ds.Tables[0].Rows[0]["Location"].ToString().Substring(3, 1);
                    string ServHist = ds.Tables[0].Rows[0]["Dispatch"].ToString().Substring(3, 1);
                    string PurcOrders = ds.Tables[0].Rows[0]["PO"].ToString().Substring(0, 1);
                    string Exp = ds.Tables[0].Rows[0]["PO"].ToString().Substring(1, 1);
                    string ProgFunc = string.Empty;
                    string AccessUser = string.Empty;
                    string dispatch = ds.Tables[0].Rows[0]["Dispatch"].ToString().Substring(5, 1);
                    string Sales = ds.Tables[0].Rows[0]["UserSales"].ToString().Substring(0, 1);
                    string EmpMaintenace = ds.Tables[0].Rows[0]["employeeMaint"].ToString().Substring(3, 1);
                    string TCTimeFix = ds.Tables[0].Rows[0]["TC"].ToString().Substring(1, 1);
                    string Addequip = ds.Tables[0].Rows[0]["elevator"].ToString().Substring(0, 1);
                    string Editequip = ds.Tables[0].Rows[0]["elevator"].ToString().Substring(1, 1);
                    string _chart = ds.Tables[0].Rows[0]["Chart"].ToString();
                    string _glAdj = ds.Tables[0].Rows[0]["GLAdj"].ToString();
                    string _financeState = ds.Tables[0].Rows[0]["Financial"].ToString().Substring(5, 1);
                    
                    string _apVendor = ds.Tables[0].Rows[0]["Vendor"].ToString(); //check Account payable permission
                    string _apBill = ds.Tables[0].Rows[0]["Bill"].ToString();
                    string _apBillSelect = ds.Tables[0].Rows[0]["BillSelect"].ToString();
                    string _apBillPay = ds.Tables[0].Rows[0]["BillPay"].ToString();

                    //string _addChart = _chart.Substring(0, 1);
                    //string _editChart = _chart.Substring(1, 1);
                    //string _viewChart = _chart.Substring(3, 1);
                    //string _addglAdj = _glAdj.Substring(0, 1);
                    //string _editglAdj = _glAdj.Substring(1, 1);
                    //string _viewglAdj = _glAdj.Substring(3, 1);
                    
                    if (_chart.Equals("YYYYYY") && _glAdj.Equals("YYYYYY"))
                    {
                        chkFinanceMgr.Checked = true;
                    }
                    if (_financeState.Equals("Y"))
                    {
                        chkFinanceStatement.Checked = true;
                    }
                    else
                    {
                        chkFinanceStatement.Checked = false;
                    }
                    if(_apVendor.Contains("Y") || _apBill.Contains("Y") || _apBillSelect.Contains("Y") || _apBillPay.Contains("Y"))
                    {
                        chkAccountPayable.Checked = true;
                    }
                    else
                    {
                        chkAccountPayable.Checked = false;
                    }
                    //if (_addChart.Equals("Y") && _addglAdj.Equals("Y"))
                    //{
                    //    chkAddFinance.Checked = true;
                    //}
                    //if (_editChart.Equals("Y") && _editglAdj.Equals("Y"))
                    //{
                    //    chkEditFinance.Checked = true;
                    //}
                    //if (_viewChart.Equals("Y") && _viewglAdj.Equals("Y"))
                    //{
                    //    chkViewFinance.Checked = true;
                    //}

                    if (Addequip == "Y")
                    {
                        chkAddEquipments.Checked = true;
                    }
                    else
                    {
                        chkAddEquipments.Checked = false;
                    }

                    if (Editequip == "Y")
                    {
                        chkEditEquipment.Checked = true;
                    }
                    else
                    {
                        chkEditEquipment.Checked = false;
                    }

                    if (ds.Tables[0].Rows[0]["Control"].ToString() != string.Empty)
                    {
                        ProgFunc = ds.Tables[0].Rows[0]["Control"].ToString().Substring(0, 1);
                    }
                    if (ds.Tables[0].Rows[0]["users"].ToString() != string.Empty)
                    {
                        AccessUser = ds.Tables[0].Rows[0]["users"].ToString().Substring(0, 1);
                    }

                    if (EmpMaintenace == "Y")
                    {
                        chkEmpMainten.Checked = true;
                        //txtHourlyRate.Visible = true;
                        //lblHourlyR.Visible = true;
                    }
                    else
                    {
                        chkEmpMainten.Checked = false;
                        //txtHourlyRate.Visible = false;
                        //lblHourlyR.Visible = false;
                    }

                    if (TCTimeFix == "Y")
                        chkTimestampFix.Checked = true;
                    else
                        chkTimestampFix.Checked = false;


                    if (crtTckt == "Y")
                    {
                        chkTicket.Checked = true;
                    }
                    else
                    {
                        chkTicket.Checked = false;
                    }

                    if (wrkDt == "Y")
                    {
                        chkWorkDt.Checked = true;
                    }
                    else
                    {
                        chkWorkDt.Checked = false;
                    }

                    if (LocnRemark == "Y")
                    {
                        chkLocation.Checked = true;
                    }
                    else
                    {
                        chkLocation.Checked = false;
                    }

                    if (ServHist == "Y")
                    {
                        chkServiceHist.Checked = true;
                    }
                    else
                    {
                        chkServiceHist.Checked = false;
                    }

                    if (PurcOrders == "Y")
                    {
                        chkPurchaseOrd.Checked = true;
                    }
                    else
                    {
                        chkPurchaseOrd.Checked = false;
                    }

                    if (Exp == "Y")
                    {
                        chkExpenses.Checked = true;
                    }
                    else
                    {
                        chkExpenses.Checked = false;
                    }

                    if (ProgFunc == "Y")
                    {
                        chkProgram.Checked = true;
                    }
                    else
                    {
                        chkProgram.Checked = false;
                    }

                    if (AccessUser == "Y")
                    {
                        chkAccessUser.Checked = true;
                    }
                    else
                    {
                        chkAccessUser.Checked = false;
                    }

                    if (dispatch == "Y")
                    {
                        chkDispatch.Checked = true;
                    }
                    else
                    {
                        chkDispatch.Checked = false;
                    }

                    if (Sales == "Y")
                    {
                        chkSalesMgr.Checked = true;
                    }
                    else
                    {
                        chkSalesMgr.Checked = false;
                    }

                    objPropUser.ConnConfig = Session["config"].ToString();
                    objPropUser.Supervisor = txtUserName.Text;
                    DataSet dsSupersUsers = new DataSet();
                    dsSupersUsers = objBL_User.getUserForSupervisor(objPropUser);
                    ViewState["superusers"] = dsSupersUsers.Tables[0];
                    ViewState["supersaved"] = dsSupersUsers.Tables[0];

                    GetUserunderSuper();
                }
            }
        }
        if (Request.QueryString["sup"] == null)
        {
            Permission();
        }
    }

    private void GetControl()
    {
        int Multilang = Convert.ToInt16(Session["IsMultiLang"]);
        if (Multilang == 0)
        {
            ddlLang.Visible = false;
            lblMultiLang.Visible = false;
        }
    }


    private void FillSupervisor()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getSupervisor(objPropUser);

        ddlSuper.DataSource = ds.Tables[0];
        ddlSuper.DataTextField = "fuser";
        ddlSuper.DataValueField = "fuser";
        ddlSuper.DataBind();

        ddlSuper.Items.Insert(0, new ListItem("-- Select --", ""));
        ddlSuper.Items.Insert(1, new ListItem("-- Add New --", "-1"));
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("progMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("userlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkUsersSmenu");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderProg");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("progMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");

        if (Session["type"].ToString() != "am")
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            string AccessUser = dt.Rows[0]["users"].ToString().Substring(0, 1);
            string Maintenance = dt.Rows[0]["EmployeeMaint"].ToString().Substring(3, 1);

            if (ProgFunc == "N")
            {
                Response.Redirect("home.aspx");
            }
            if (AccessUser == "N")
            {
                Response.Redirect("home.aspx");
            }
            if (Maintenance == "N")
            {
                txtHourlyRate.Visible = false;
                lblHourlyR.Visible = false;
            }
        }

        if (Session["MSM"].ToString() == "TS")
        {
            chkEmailAcc.Enabled = false;
            ddlUserType.Enabled = false;
            if (Convert.ToInt32(ViewState["super"]) != 1)
            {
                btnSubmit.Visible = false;
            }
            if (Session["type"].ToString() == "am" || Session["Username"].ToString() == "ADMIN")
            {
                btnSubmit.Visible = true;
            }
        }
        if (Request.QueryString["type"] != null)
        {
            if (Request.QueryString["type"].ToString() == "2")
            {
                btnSubmit.Visible = false;
            }
        }
    }

    private void userpermissions()
    {
        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                objPropUser.PageName = "adduser.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objPropUser);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.Address = txtAddress.Text;
            objPropUser.Cell = txtCell.Text;
            objPropUser.City = txtCity.Text;

            if (txtTerminationDt.Text.Trim() != string.Empty)
            {
                objPropUser.DtFired = Convert.ToDateTime(txtTerminationDt.Text);
            }

            if (txtHireDt.Text.Trim() != string.Empty)
            {
                objPropUser.DtHired = Convert.ToDateTime(txtHireDt.Text);
            }

            //objPropUser.DtHired =Convert.ToDateTime( txtHireDt.Text);
            objPropUser.Email = txtEmail.Text;
            objPropUser.FirstName = txtFName.Text;
            objPropUser.LastNAme = txtLName.Text;
            objPropUser.MiddleName = txtMName.Text.Trim();
            objPropUser.Password = txtPassword.Text.Trim();
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Status = Convert.ToInt32(rbStatus.SelectedValue);
            objPropUser.Tele = txtTelephone.Text;
            objPropUser.Username = txtUserName.Text.Trim();
            objPropUser.Zip = txtZip.Text;
            objPropUser.Remarks = txtRemarks.Text;
            objPropUser.DeviceID = txtDeviceID.Text.Trim();
            objPropUser.Pager = txtMsg.Text;
            GeneralFunctions objgn = new GeneralFunctions();
            objPropUser.InServer = txtInServer.Text.Trim();
            objPropUser.InUsername = txtInUSername.Text.Trim();
            objPropUser.InPassword = txtInPassword.Text.Trim();
            objPropUser.InPort = Convert.ToInt32(objgn.IsNull(txtinPort.Text.Trim(), "0"));
            objPropUser.OutServer = txtOutServer.Text.Trim();
            if (chkSame.Checked == true)
            {
                objPropUser.OutUsername = txtInUSername.Text.Trim();
                objPropUser.OutPassword = txtInPassword.Text.Trim();
            }
            else
            {
                objPropUser.OutUsername = txtOutUsername.Text.Trim();
                objPropUser.OutPassword = txtOutPassword.Text.Trim();
            }
            objPropUser.OutPort = Convert.ToInt32(objgn.IsNull(txtOutPort.Text.Trim(), "0"));
            objPropUser.SSL = chkSSL.Checked;
            if(!string.IsNullOrEmpty(txtStartDate.Text))
                objPropUser.FStart = Convert.ToDateTime(txtStartDate.Text);
            if (!string.IsNullOrEmpty(txtEndDate.Text))
                objPropUser.FEnd = Convert.ToDateTime(txtEndDate.Text);
            if (chkSalesperson.Checked == true)
            {
                objPropUser.Salesperson = 1;

                objPropUser.EmailAccount = Convert.ToInt16(chkEmailAcc.Checked);

            }
            else
            {
                objPropUser.Salesperson = 0;
            }

            if (chkScheduleBrd.Checked == true)
            {
                objPropUser.Schedule = 1;
            }
            else
            {
                objPropUser.Schedule = 0;
            }

            if (chkMap.Checked == true)
            {
                objPropUser.Mapping = 1;
            }
            else
            {
                objPropUser.Mapping = 0;
            }

            objPropUser.Field = Convert.ToInt32(ddlUserType.SelectedValue);

            //if (chkPDA.Checked == true)
            //{
            //    objPropUser.PDA = 1;
            //}
            //else
            //{
            //    objPropUser.PDA = 0;
            //}

            if (chkAccessUser.Checked == true)
            {
                objPropUser.AccessUser = "Y";
            }
            else
            {
                objPropUser.AccessUser = "N";
            }


            if (chkExpenses.Checked == true)
            {
                objPropUser.Expenses = "Y";
            }
            else
            {
                objPropUser.Expenses = "N";
            }

            if (chkLocation.Checked == true)
            {
                objPropUser.LocationRemarks = "Y";
            }
            else
            {
                objPropUser.LocationRemarks = "N";
            }

            if (chkProgram.Checked == true)
            {
                objPropUser.ProgFunctions = "Y";
            }
            else
            {
                objPropUser.ProgFunctions = "N";
            }

            if (chkPurchaseOrd.Checked == true)
            {
                objPropUser.PurchaseOrd = "Y";
            }
            else
            {
                objPropUser.PurchaseOrd = "N";
            }

            if (chkServiceHist.Checked == true)
            {
                objPropUser.ServiceHist = "Y";
            }
            else
            {
                objPropUser.ServiceHist = "N";
            }

            if (chkTicket.Checked == true)
            {
                objPropUser.CreateTicket = "Y";
            }
            else
            {
                objPropUser.CreateTicket = "N";
            }

            if (chkWorkDt.Checked == true)
            {
                objPropUser.WorkDate = "Y";
            }
            else
            {
                objPropUser.WorkDate = "N";
            }

            if (chkDispatch.Checked == true)
            {
                objPropUser.Dispatch = "Y";
            }
            else
            {
                objPropUser.Dispatch = "N";
            }
            if (chkFinanceMgr.Checked == true)
            {
                objPropUser.FChart = 1;
                objPropUser.FGLAdj = 1;
            }
            else
            {
                objPropUser.FChart = 0;
                objPropUser.FGLAdj = 0;
            }
            if(chkFinanceStatement.Checked ==true)
            {
                objPropUser.FinanStatement = 1;
            }
            else
            {
                objPropUser.FinanStatement = 0;
            }
            if (chkAccountPayable.Checked == true)
            {
                objPropUser.APVendor = 1;
                objPropUser.APBill = 1;
                objPropUser.APBillPay = 1;
                objPropUser.APBillSelect = 1;
            }
            else
            {
                objPropUser.APVendor = 0;
                objPropUser.APBill = 0;
                objPropUser.APBillPay = 0;
                objPropUser.APBillSelect = 0;
            }

            //if (chkAddFinance.Checked == true)
            //{
            //    objPropUser.AddChart = 1;
            //    objPropUser.AddGLAdj = 1;
            //}
            //else
            //{
            //    objPropUser.AddChart = 0;
            //    objPropUser.AddGLAdj = 0;
            //}
            //if (chkEditFinance.Checked == true)
            //{
            //    objPropUser.EditChart = 1;
            //    objPropUser.EditGLAdj = 1;
            //}
            //else
            //{
            //    objPropUser.EditChart = 0;
            //    objPropUser.EditGLAdj = 0;
            //}
            //if (chkViewFinance.Checked == true)
            //{
            //    objPropUser.ViewChart = 1;
            //    objPropUser.ViewGLAdj = 1;
            //}
            //else
            //{
            //    objPropUser.ViewChart = 0;
            //    objPropUser.ViewGLAdj = 0;
            //}
            objPropUser.EmpMaintenance = Convert.ToInt16(chkEmpMainten.Checked);

            objPropUser.SalesMgr = Convert.ToInt16(chkSalesMgr.Checked);

            objPropUser.DefaultWorker = Convert.ToInt32(chkDefaultWorker.Checked);

            objPropUser.MassReview = Convert.ToInt32(chkMassReview.Checked);

            objPropUser.HourlyRate = Convert.ToDouble(objgn.IsNull(txtHourlyRate.Text.Trim(), "0"));

            objPropUser.Timestampfix = Convert.ToInt32(chkTimestampFix.Checked);

            objPropUser.AddEquip = Convert.ToInt32(chkAddEquipments.Checked);

            objPropUser.EditEquip = Convert.ToInt32(chkEditEquipment.Checked);

            objPropUser.PayMethod = Convert.ToInt16(ddlPayMethod.SelectedValue);
            objPropUser.PHours = txtHours.Text.Trim() != string.Empty ? Convert.ToDouble(txtHours.Text.Trim()) : 0;
            objPropUser.Salary = txtAmount.Text.Trim() != string.Empty ? Convert.ToDouble(txtAmount.Text.Trim()) : 0;
            objPropUser.EmpRefID = txtEmpID.Text.Trim();
            objPropUser.MileageRate = txtMileageRate.Text.Trim() != string.Empty ? Convert.ToDouble(txtMileageRate.Text.Trim()) : 0;
            objPropUser.PayPeriod = Convert.ToInt16(ddlPayPeriod.SelectedValue);

            List<string> Departmentids = new List<string>();
            foreach (ListItem li in ddlDepartment.Items)
            {
                if (li.Selected)
                    Departmentids.Add(li.Value);
            }
            string strDepartment = string.Join(",", Departmentids.ToArray());
            objPropUser.Department = strDepartment;
            //objPropUser.MOMUSer = txtMOMUserName.Text.Trim();
            //objPropUser.MOMPASS = txtMOMPassword.Text.Trim();

            objPropUser.ConnConfig = Session["config"].ToString();


            if (chkSuper.Checked == true)
            {
                objPropUser.Supervisor = txtUserName.Text.Trim();

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    UpdateUsers(1);
                }
            }
            else
            {
                objPropUser.Supervisor = ddlSuper.SelectedValue;

                if (Convert.ToInt32(ViewState["mode"]) == 1)
                {
                    UpdateUsers(0);
                }

            }

            string strLic = "0";
            string strDay = "30";
            string strDate = System.DateTime.Now.ToShortDateString();
            string strUsername = txtUserName.Text;

            objPropUser.DBName = Session["dbname"].ToString();
            DataSet dsinfo = new DataSet();
            dsinfo = objBL_User.getLicenseInfoUser(objPropUser);

            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                string strRegDecr = SSTCryptographer.Decrypt(dsinfo.Tables[0].Rows[0]["str"].ToString(), "regu");
                string[] strRegItems = strRegDecr.Split('&');
                strLic = strRegItems[0];
                strDay = strRegItems[1];
                strDate = strRegItems[2];
                objPropUser.UserLicID = Convert.ToInt32(dsinfo.Tables[0].Rows[0]["lid"]);
            }

            string strReg = strLic + "&" + strDay + "&" + strDate + "&" + strUsername;
            string strRegEncr = SSTCryptographer.Encrypt(strReg, "regu");
            objPropUser.UserLic = strRegEncr;

            objPropUser.Lang = ddlLang.SelectedValue;

            if (ddlMerchantID.SelectedValue != string.Empty)
            {
                objPropUser.MerchantInfoId = Convert.ToInt32(ddlMerchantID.SelectedValue);
            }
            objPropUser.dtPageData = PagePermissionData();
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                objPropUser.UserID = Convert.ToInt32(ViewState["userid"]);
                if (!string.IsNullOrEmpty(ViewState["empid"].ToString()))
                {
                    objPropUser.EmpId = Convert.ToInt32(ViewState["empid"]);
                }
                if (!string.IsNullOrEmpty(ViewState["rolid"].ToString()))
                {
                    objPropUser.RolId = Convert.ToInt32(ViewState["rolid"]);
                }
                if (ViewState["workid"].ToString() != string.Empty)
                {
                    objPropUser.WorkId = Convert.ToInt32(ViewState["workid"]);
                }
                else
                {
                    objPropUser.WorkId = 0;
                }

                if (Session["MSM"].ToString() == "TS")
                {
                    if (Session["type"].ToString() == "am" || Session["Username"].ToString() == "ADMIN")
                    {
                        objBL_User.UpdateTSUser(objPropUser);
                    }
                }
                else
                {
                    objBL_User.UpdateUser(objPropUser);
                }

                objBL_User.UpdateUserPermission(objPropUser);

                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'User updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                //ViewState["mode"] = 0;
                //lblMsg.Text = "User updated successfully.";
                //ClearControls();                
            }
            else
            {
                objPropUser.UserID = objBL_User.AddUser(objPropUser);
                objBL_User.UpdateUserPermission(objPropUser);

                ViewState["mode"] = 0;
                //lblMsg.Text = "User added successfully.";

                string strsuper = "User";
                if (Request.QueryString["sup"] != null)
                {
                    strsuper = "Supervisor";
                }

                ClientScript.RegisterStartupScript(Page.GetType(), "keySucc", "noty({text: '" + strsuper + " added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                ClearControls();
                ResetFormControlValues(this);
            }
        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;    
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

        }
    }

    private void ClearControls()
    {
        txtAddress.Text = string.Empty;
        txtCell.Text = string.Empty;
        txtCity.Text = string.Empty;
        txtTerminationDt.Text = string.Empty;
        txtHireDt.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtFName.Text = string.Empty;
        txtLName.Text = string.Empty;
        //txtMName.Text=string.Empty;
        txtPassword.Text = string.Empty;
        ddlState.SelectedIndex = -1;
        rbStatus.SelectedIndex = -1;
        txtTelephone.Text = string.Empty;
        txtUserName.Text = string.Empty;
        txtZip.Text = string.Empty;
        //chkPDA.Checked = false;
        chkScheduleBrd.Checked = false;
        ddlUserType.SelectedIndex = -1;
        chkAccessUser.Checked = false;
        chkExpenses.Checked = false;
        chkLocation.Checked = false;
        chkProgram.Checked = false;
        chkPurchaseOrd.Checked = false;
        chkScheduleBrd.Checked = false;
        chkServiceHist.Checked = false;
        chkTicket.Checked = false;
        chkWorkDt.Checked = false;
    }

    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("users.aspx");
    }

    private DataTable GetUser()
    {
        objPropUser.UserID = Convert.ToInt32(Session["userid"]);
        DataSet ds = new DataSet();
        ds = objBL_User.getUserByID(objPropUser);
        return ds.Tables[0];
    }

    private void UpdateUsers(int updatenew)
    {

        DataTable dtSaved = new DataTable();
        dtSaved = (DataTable)ViewState["supersaved"];

        foreach (DataRow dr in dtSaved.Rows)
        {
            UpdateSupervisorUser(Convert.ToInt32(dr["UserID"]), "");
        }

        if (updatenew == 1)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)ViewState["superusers"];

            foreach (DataRow dr in dt.Rows)
            {
                UpdateSupervisorUser(Convert.ToInt32(dr["UserID"]), txtUserName.Text);
            }
        }

        //foreach (GridViewRow gr in gvUsers.Rows)
        //{
        //CheckBox chkSelected = (CheckBox)gr.FindControl("chkSelect");
        //Label lblUserID = (Label)gr.FindControl("lblId");

        //if (chkSelected.Checked == true)
        //{
        //UpdateSupervisorUser(Convert.ToInt32(lblUserID.Text));               
        //}
        //}
        //GetUserunderSuper();
    }

    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlUserType.SelectedValue == "2")
        //{
        //    tblPermission.Visible = false;
        //    txtHireDt.Visible = false;
        //    txtTerminationDt.Visible= false;
        //    hire.Visible = false;
        //    fire.Visible = false;
        //}
        //else
        //{
        //    tblPermission.Visible = true;
        //    txtHireDt.Visible = true;
        //    txtTerminationDt.Visible= true;
        //    hire.Visible = true;
        //    fire.Visible = true;
        //}

        if (ddlUserType.SelectedValue == "1")
        {
            pnlWorker.Enabled = true;
            chkMap.Enabled = false;
            chkMap.Checked = true;
            chkMap_CheckedChanged(sender, e);
            //if (Convert.ToInt32( ViewState["super"] )== 0)
            //{
            //    lblSuper.Enabled = true;
            //    ddlSuper.Enabled = true;
            //}
            //lblDefwork.Visible = true;
            //chkDefaultWorker.Visible = true;
        }
        else
        {
            pnlWorker.Enabled = false;
            //chkMap.Enabled = true;
            ////if (txtDeviceID.Text == string.Empty)
            ////{
            //chkMap.Checked = false;
            //chkMap_CheckedChanged(sender, e);
            //lblSuper.Enabled = false;
            //ddlSuper.Enabled = false;
            ////}
            //lblDefwork.Visible = false;
            //chkDefaultWorker.Visible = false;
        }
    }


    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;

        if (index < c)
        {
            if (Convert.ToInt16(dt.Rows[index + 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index + 1]["userid"] + "&type=" + dt.Rows[index + 1]["usertypeid"]);
        }
    }

    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["userkey"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["type"].ToString() + "_" + Request.QueryString["uid"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            if (Convert.ToInt16(dt.Rows[index - 1]["usertypeid"].ToString()) == 2)
                Response.Redirect("customeruser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
            else
                Response.Redirect("adduser.aspx?uid=" + dt.Rows[index - 1]["userid"] + "&type=" + dt.Rows[index - 1]["usertypeid"]);
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[dt.Rows.Count - 1]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[dt.Rows.Count - 1]["userid"] + "&type=" + dt.Rows[dt.Rows.Count - 1]["usertypeid"]);
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["usersdata"];
        if (Convert.ToInt16(dt.Rows[0]["usertypeid"].ToString()) == 2)
            Response.Redirect("customeruser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
        else
            Response.Redirect("adduser.aspx?uid=" + dt.Rows[0]["userid"] + "&type=" + dt.Rows[0]["usertypeid"]);
    }
    protected void chkMap_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMap.Checked == true)
        {
            if (ddlUserType.SelectedValue != "0")
            {
                lblDeviceID.Visible = true;
                txtDeviceID.Visible = true;
            }
        }
        else
        {
            lblDeviceID.Visible = false;
            txtDeviceID.Visible = false;
        }
    }

    protected void ddlSuper_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSuper.SelectedIndex == 1)
        {
            if (Session["MSM"].ToString() == "TS")
            {
                ddlSuper.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuperAlert", "noty({text: 'Please add Supervisor and User from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
                return;
            }

            this.programmaticModalPopup.Show();
            iframeTicket.Attributes["src"] = "adduser.aspx?sup=1";
        }
    }

    protected void hideModalPopupViaServer_Click(object sender, EventArgs e)
    {
        this.programmaticModalPopup.Hide();
        iframeTicket.Attributes["src"] = "";
        FillSupervisor();
    }

    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;

            return (SortDirection)ViewState["sortDirection"];
        }
        set { ViewState["sortDirection"] = value; }
    }

    private void SortGridView(string sortExpression, string direction)
    {
        //DataTable dt = PageSortData();

        //DataView dv = new DataView(dt);
        //dv.Sort = sortExpression + direction;

        //gvUsers.DataSource = dv.ToTable();
        //gvUsers.DataBind();
    }

    private void GetUsers()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Supervisor = txtUserName.Text;
        DataSet ds = new DataSet();
        ds = objBL_User.getUsersSuper(objPropUser);
        gvUsers.DataSource = ds.Tables[0];
        gvUsers.DataBind();

        DataTable dtSupersUsers = new DataTable();
        dtSupersUsers = (DataTable)ViewState["superusers"];
        //dsSupersUsers=objBL_User.getUserForSupervisor(objPropUser);

        foreach (GridViewRow gr in gvUsers.Rows)
        {
            CheckBox chkSelected = (CheckBox)gr.FindControl("chkSelect");
            Label lblUserID = (Label)gr.FindControl("lblId");

            foreach (DataRow dr in dtSupersUsers.Rows)
            {
                if (lblUserID.Text == dr["userid"].ToString())
                {
                    chkSelected.Checked = true;
                }
            }
        }
    }

    private void GetUserunderSuper()
    {
        DataTable dt = new DataTable();
        dt = (DataTable)ViewState["superusers"];

        gvUsers.DataSource = dt;
        gvUsers.DataBind();
    }

    private void UpdateSupervisorUser(int workid, string username)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.WorkId = workid;
        objPropUser.Supervisor = username;
        objBL_User.UpdateSupervisorUser(objPropUser);
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        GetUsers();
        lnkDone.Visible = true;
        btnEdit.Visible = false;
        gvUsers.Columns[0].Visible = true;
    }

    protected void lnkDone_Click(object sender, EventArgs e)
    {
        //GetUserunderSuper();
        int first = 0;
        string str = string.Empty;
        foreach (GridViewRow gr in gvUsers.Rows)
        {
            CheckBox chkSelected = (CheckBox)gr.FindControl("chkSelect");
            Label lblUserID = (Label)gr.FindControl("lblId");

            if (chkSelected.Checked == true)
            {
                if (first == 0)
                {
                    str = lblUserID.Text;
                    first = 1;
                }
                else
                {
                    str = str + "," + lblUserID.Text;
                }
            }
        }


        if (str != string.Empty)
        {
            DataSet ds = new DataSet();

            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.Address = str;
            ds = objBL_User.getSelectedUser(objPropUser);

            ViewState["superusers"] = ds.Tables[0];
            GetUserunderSuper();
        }
        else
        {
            DataTable dt = (DataTable)ViewState["superusers"];
            DataTable dtSup = dt.Clone();
            ViewState["superusers"] = dtSup;
            GetUserunderSuper();
        }

        lnkDone.Visible = false;
        btnEdit.Visible = true;
        gvUsers.Columns[0].Visible = false;

        DataTable dtsupr = (DataTable)ViewState["superusers"];
        if (dtsupr.Rows.Count == 0)
        {
            chkSuper.Enabled = true;
            ddlSuper.SelectedIndex = 0;
        }
    }

    private void ResetFormControlValues(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c.Controls.Count > 0)
            {
                ResetFormControlValues(c);
            }
            else
            {
                switch (c.GetType().ToString())
                {
                    case "System.Web.UI.WebControls.DropDownList":
                        ((DropDownList)c).SelectedIndex = -1;
                        break;
                    case "System.Web.UI.WebControls.TextBox":
                        ((TextBox)c).Text = "";
                        break;
                    case "System.Web.UI.WebControls.CheckBox":
                        ((CheckBox)c).Checked = false;
                        break;
                    case "System.Web.UI.WebControls.RadioButton":
                        ((RadioButton)c).Checked = false;
                        break;
                }
            }
        }
    }

    protected void imgbtnMerchant_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlMerchantID.SelectedIndex > 1)
        {
            //TogglePopUp();
            this.programmaticModalPopup.Show();
            DataSet ds = new DataSet();
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantID = ddlMerchantID.SelectedValue;
            ds = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgbtnDelete.Visible = true;
                txtMerchantID.Enabled = false;
                txtMerchantID.Text = ds.Tables[0].Rows[0]["MerchantID"].ToString();
                txtLoginID.Text = ds.Tables[0].Rows[0]["LoginID"].ToString();
                txtMerchantUsername.Text = ds.Tables[0].Rows[0]["Username"].ToString();
                txtMerchantPassword.Text = AES_Algo.Decrypt(ds.Tables[0].Rows[0]["Password"].ToString(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256).TrimEnd('\0');
                hdnMerchantInfoID.Value = ds.Tables[0].Rows[0]["id"].ToString();
            }
        }
    }

    private void GetMerchantID()
    {
        DataSet ds = new DataSet();
        objProp_Contracts.ConnConfig = Session["config"].ToString();
        ds = objBL_Contracts.getPaymentGatewayInfo(objProp_Contracts);
        ddlMerchantID.DataSource = ds.Tables[0];
        ddlMerchantID.DataTextField = "merchantid";
        ddlMerchantID.DataValueField = "id";
        ddlMerchantID.DataBind();

        ddlMerchantID.Items.Insert(0, new ListItem("-- Select --", ""));
        ddlMerchantID.Items.Insert(1, new ListItem("-- Add New --", ""));
    }

    protected void lnkCancelMerchant_Click(object sender, EventArgs e)
    {
        ClearMerchant();
    }

    private void ClearMerchant()
    {
        txtMerchantID.Text = string.Empty;
        txtLoginID.Text = string.Empty;
        txtMerchantUsername.Text = string.Empty;
        txtMerchantPassword.Text = string.Empty;
        hdnMerchantInfoID.Value = "0";
        txtMerchantID.Enabled = true;
        //imgbtnDelete.Visible = false;

        GetMerchantID();
        //TogglePopUp();
        this.programmaticModalPopup.Hide();
    }

    private void TogglePopUp()
    {
        //string strScript = "TogglePopUp();";
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "keyDisplay", strScript, true);
        this.programmaticModalPopup.Show();
    }

    protected void lnkSaveMerchant_Click(object sender, EventArgs e)
    {
        try
        {
            string strMessage = string.Empty;

            if (Convert.ToInt32(hdnMerchantInfoID.Value) == 0)
            {
                strMessage = "Merchant added successfully!";
            }
            else
            {
                strMessage = "Merchant updated successfully!";
            }

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantID = txtMerchantID.Text.Trim();
            objProp_Contracts.LoginID = txtLoginID.Text.Trim();
            objProp_Contracts.PaymentUser = txtMerchantUsername.Text.Trim();
            objProp_Contracts.MerchantInfoID = Convert.ToInt32(hdnMerchantInfoID.Value);
            objProp_Contracts.PaymentPass = AES_Algo.Encrypt(txtMerchantPassword.Text.Trim(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);

            objBL_Contracts.AddMerchant(objProp_Contracts);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccMerchant", "noty({text: '" + strMessage + "',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);
            ScriptManager.RegisterStartupScript(this, GetType(), "hideModalPopup", "hideModalPopup();", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrMerchant", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }

    protected void imgbtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.MerchantInfoID = Convert.ToInt32(hdnMerchantInfoID.Value);

            objBL_Contracts.DeleteMerchant(objProp_Contracts);

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccMerchantDel", "noty({text: 'Merchant deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false,dismissQueue: true});", true);

            ClearMerchant();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErrMerchantDel", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true,dismissQueue: true});", true);
        }
    }
    protected void chkSuper_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSuper.Checked)
        {
            ddlSuper.Enabled = false;
            ddlSuper.SelectedIndex = 0;
            pnlGrid.Visible = true;
        }
        else
        {
            ddlSuper.Enabled = true;
            ddlSuper.SelectedIndex = 0;
            pnlGrid.Visible = false;
        }
    }

    protected void chkSalesperson_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSalesperson.Checked == true)
        {
            if (Session["MSM"].ToString() != "TS")
            {
                chkEmailAcc.Enabled = true;
            }
        }
        else
        {
            chkEmailAcc.Enabled = false;
            chkEmailAcc.Checked = false;
            pnlEmailAccount.Visible = false;
        }
    }

    protected void chkSame_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSame.Checked == true)
        {
            txtOutUsername.Text = txtInUSername.Text.Trim();
            txtOutPassword.Text = txtInPassword.Text.Trim();
            txtOutPassword.Enabled = false;
            txtOutUsername.Enabled = false;
        }
        else
        {
            txtOutPassword.Enabled = true;
            txtOutUsername.Enabled = true;
        }
    }
    protected void chkEmailAcc_CheckedChanged(object sender, EventArgs e)
    {
        if (chkEmailAcc.Checked == true)
            pnlEmailAccount.Visible = true;
        else
            pnlEmailAccount.Visible = false;
    }

    protected void btnTestOut_Click(object sender, EventArgs e)
    {
        Mail mail = new Mail();
        try
        {
            mail.Username = txtOutUsername.Text.Trim();
            mail.Password = txtOutPassword.Text.Trim();
            mail.SMTPHost = txtOutServer.Text.Trim();
            mail.SMTPPort = Convert.ToInt32(txtOutPort.Text.Trim());

            mail.From = txtOutUsername.Text.Trim();
            mail.To = txtOutUsername.Text.Split(';', ',').OfType<string>().ToList();
            mail.Title = "Test Email";
            mail.Text = "Test Email from Mobile Office Manager.";
            mail.RequireAutentication = true;
            mail.Send();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Mail sent successfully.');", true);

        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    protected void btnTestIncoming_Click(object sender, EventArgs e)
    {
        try
        {
            //Pop3Client pop3Client = new Pop3Client();
            //if (pop3Client.Connected)
            //    pop3Client.Disconnect();

            //pop3Client.Connect(txtInServer.Text.Trim(), int.Parse(txtinPort.Text.Trim()), true);
            //pop3Client.Authenticate(txtInUSername.Text.Trim(), txtInPassword.Text.Trim());

            //int count = pop3Client.GetMessageCount();

            using (ImapClient client = new ImapClient())
            {
                if (client.Connect())
                    client.Disconnect();

                try
                {
                    if (client.Connect(txtInServer.Text.Trim(), Convert.ToInt32(txtinPort.Text.Trim()), true, false))
                    {
                        if (client.Login(txtInUSername.Text.Trim(), txtInPassword.Text.Trim()))
                        {
                            int count = client.Folders.Inbox.Messages.Count();
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Successful');", true);
                            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('" + count.ToString() + " emails found.');", true);
                            client.Disconnect();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Invalid Credentials');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Connection Failed');", true);
                    }
                }
                catch (ImapX.Exceptions.ServerAlertException ex)
                {
                    throw ex;
                }
                catch (ImapX.Exceptions.OperationFailedException ex)
                {
                    throw ex;
                }
                catch (ImapX.Exceptions.InvalidStateException ex)
                {
                    throw ex;
                }
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        }

        //catch (InvalidLoginException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The server did not accept the user credentials!');", true);
        //}
        //catch (PopServerNotFoundException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The server could not be found');", true);
        //}
        //catch (PopServerLockedException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('The mailbox is locked. It might be in use or under maintenance. Are you connected elsewhere?');", true);
        //}
        //catch (LoginDelayException)
        //{
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "alert('Login not allowed. Server enforces delay between logins. Have you connected recently?');", true);
        //}
        //catch (Exception ex)
        //{
        //    string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
        //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, theme : 'noty_theme_default',  closable : true});", true);
        //}
    }
    //protected void chkEmpMainten_CheckedChanged(object sender, EventArgs e)
    //{
    //    txtHourlyRate.Visible = chkEmpMainten.Checked;
    //    lblHourlyR.Visible = chkEmpMainten.Checked;
    //}
    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();
    }
    protected void ddlPayMethod_SelectedIndexChanged1(object sender, EventArgs e)
    {
        if (ddlPayMethod.SelectedValue == "2")
        {
            txtHours.Enabled = true;
            txtAmount.Enabled = false;
        }
        else if (ddlPayMethod.SelectedValue == "0")
        {
            txtHours.Enabled = false;
            txtAmount.Enabled = true;
        }
        else
        {
            txtAmount.Enabled = false;
            txtHours.Enabled = false;
        }
    }

    private void FillPages()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        if (Request.QueryString["uid"] != null)
            objPropUser.UserID = Convert.ToInt32(Request.QueryString["uid"]);
        ds = objBL_User.getScreens(objPropUser);

        gvPages.DataSource = ds.Tables[0];
        gvPages.DataBind();
    }

    private DataTable PagePermissionData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("pageid", typeof(int));
        dt.Columns.Add("access", typeof(int));
        dt.Columns.Add("view", typeof(int));
        dt.Columns.Add("add", typeof(int));
        dt.Columns.Add("edit", typeof(int));
        dt.Columns.Add("delete", typeof(int));

        foreach (GridViewRow gr in gvPages.Rows)
        {
            Label lblId = (Label)gr.FindControl("lblId");
            CheckBox chkAccess = (CheckBox)gr.FindControl("chkAccess");
            CheckBox chkAdd = (CheckBox)gr.FindControl("chkAdd");
            CheckBox chkEdit = (CheckBox)gr.FindControl("chkEdit");
            CheckBox chkDelete = (CheckBox)gr.FindControl("chkDelete");
            CheckBox chkView = (CheckBox)gr.FindControl("chkView");

            DataRow dr = dt.NewRow();
            dr["pageid"] = Convert.ToInt32(lblId.Text);
            dr["access"] = Convert.ToInt32(chkAccess.Checked);
            dr["add"] = Convert.ToInt32(chkAdd.Checked);
            dr["edit"] = Convert.ToInt32(chkEdit.Checked);
            dr["delete"] = Convert.ToInt32(chkDelete.Checked);
            dr["view"] = Convert.ToInt32(chkView.Checked); ;

            dt.Rows.Add(dr);
        }

        return dt;
    }
}
