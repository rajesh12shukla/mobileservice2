﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessEntity;
using System.Data;
using AjaxControlToolkit;
using System.Web.Script.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel;
using MicrosoftTranslatorSdk.SoapSamples;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Web.UI.HtmlControls;
//using PushNotification;

public partial class AddTicket : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();

    BL_MapData objBL_MapData = new BL_MapData();
    MapData objMapData = new MapData();

    BL_General objBL_General = new BL_General();
    General objGeneral = new General();

    BL_Customer objBL_Customer = new BL_Customer();
    Customer objCustomer = new Customer();

    public GeneralFunctions objGeneralFunctions = new GeneralFunctions();

    bool success;
    //AndroidPushNotification obj_PushNotification = new AndroidPushNotification(); 

    string defaultDate = "12/30/1899";

    protected void Page_PreInit(object sender, System.EventArgs e)
    {
        if (Request.QueryString["popup"] != null)
        {
            Page.MasterPageFile = "popup.master";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            //Response.Redirect("timeout.htm");
            Response.Redirect("login.aspx");
            return;
        }

        GetControl();

        if (!IsPostBack)
        {
            GetQBInt();
            userpermissions();
            Permission1();
            #region FillControls
            ViewState["comp"] = "0";
            ViewState["convert"] = "0";
            hdnFormId.Value = objGeneralFunctions.generateRandomString(10);/****hdnformid is used for the randomid (tempid) for document upload feature.*********/
            FillCustomFields();
            getDiagnosticCategory();
            //lblReviewed.Visible = false;
            //chkReviewed.Visible = false;
            //FillLoc();
            FillDefaultRoute();
            FillWorker(string.Empty);
            FillCategory();
            FillElevUnit();
            FillDepartment();
            FillWage();
            GetBillcodesforTimeSheet();
            GetPayrollforTimeSheet();
            ViewState["mode"] = 0;
            txtCallDt.Text = System.DateTime.Now.ToShortDateString();
            txtCallTime.Text = System.DateTime.Now.ToShortTimeString();
            ChangeStatus();
            ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";
            FillProjectsTemplate();
            txtFby.Text = Session["username"].ToString();
            #endregion

            /**********When start date passed from schedule board to add new ticket***********/
            if (Request.QueryString["timer"] != null)
            {
                txtSchDt.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortDateString();
                txtSchTime.Text = Convert.ToDateTime(Request.QueryString["start"]).ToShortTimeString();
                ddlRoute.SelectedValue = Request.QueryString["r"].ToString().ToUpper();
                ddlRoute_SelectedIndexChanged(sender, e);
                ddlStatus.SelectedValue = "1";
                ChangeStatus();
                TimeSpan ts = Convert.ToDateTime(Request.QueryString["end"]) - Convert.ToDateTime(Request.QueryString["start"]);
                txtEST.Text = string.Format("{0:00.00}", ts.TotalHours);
            }

            if (Request.QueryString["locid"] != null)
            {
                hdnLocId.Value = Request.QueryString["locid"].ToString();
                FillLocInfo();
                if (Request.QueryString["unitid"] != null)
                {
                    hdnUnitID.Value = Request.QueryString["unitid"].ToString();
                    txtUnit.Text = Request.QueryString["unit"].ToString();
                }
            }

            /******* If existing ticket is opened *********/
            if (Request.QueryString["id"] != null)
            {
                /********* Check whether ticket is copy mode or edit mode***********/
                if (Request.QueryString["copy"] != null)
                {
                    lblHeader.Text = "Copy Ticket";
                    tblTicketID.Visible = false;
                }
                else
                {
                    ViewState["mode"] = 1;
                    lblHeader.Text = "Edit Ticket";
                    ddlStatus.Items[5].Enabled = true;
                    tblTicketID.Visible = true;
                    //tblWO.Attributes.Add("style", "float:right; margin-right:0px;");
                }
                ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";

                /**********check whether ticket is completed*************/
                if (Request.QueryString["comp"] != null)
                {
                    hdnComp.Value = Request.QueryString["comp"].ToString();
                    ViewState["comp"] = Request.QueryString["comp"].ToString();
                    objMapData.ISTicketD = Convert.ToInt32(Request.QueryString["comp"]);
                    if (objMapData.ISTicketD == 2 || objMapData.ISTicketD == 1)
                    {
                        ddlStatus.Items[5].Enabled = true;
                        ////lblReviewed.Visible = true;
                        //chkReviewed.Visible = true;
                        if (Request.QueryString["copy"] == null)
                        {
                            ddlStatus.Enabled = false;
                            btnEnroute.Enabled = false;
                            btnOnsite.Enabled = false;
                            btnComplete.Enabled = false;
                        }
                    }

                    ////if (objMapData.ISTicketD == 1)
                    ////{
                    ////    //chkReviewed.Checked = true;
                    ////    lblReviewed.Visible = true;
                    ////    chkReviewed.Visible = true;
                    ////}

                    ///******* check whether ticket is completed and is TS login **********/
                    //if (Session["MSM"].ToString() == "TS")//&& Convert.ToInt32(Request.QueryString["comp"]) != 1
                    //{
                    //    if (Request.QueryString["copy"] == null)
                    //    {                            
                    //        lnkSave.Visible = false;
                    //        lnkPrint.Visible = false;
                    //        pnlDocumentButtons.Visible = false;
                    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Readonly mode. Please complete ticket from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                    //    }
                    //}
                }


                #region Fill data for edit ticket
                DataSet ds = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                ds = objBL_MapData.GetTicketByID(objMapData);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pnlNext.Visible = true;
                    lnkPDF.Visible = true;
                    lnkPDF.NavigateUrl = "ticketpdf.ashx?id=" + Request.QueryString["id"].ToString() + "&c=" + Request.QueryString["comp"].ToString();
                    GetOpportTicket(objMapData.TicketID);
                    fillREPHistory();
                    GetDocuments();
                    chkWorkComp.Checked = Convert.ToBoolean(Convert.ToInt32(ds.Tables[0].Rows[0]["workcmpl"].ToString()));
                    txtWO.Text = ds.Tables[0].Rows[0]["workorder"].ToString();
                    txtCustomer.Text = ds.Tables[0].Rows[0]["customername"].ToString();
                    hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                    if (hdnPatientId.Value == string.Empty)
                    {
                        hdnProspect.Value = "1";
                    }

                    //FillLoc();
                    //ddlLoc.SelectedValue = ds.Tables[0].Rows[0]["lid"].ToString();
                    txtLocation.Text = ds.Tables[0].Rows[0]["locname"].ToString();
                    hdnLocId.Value = ds.Tables[0].Rows[0]["lid"].ToString();
                    FillLocInfo();
                    //txtAddress.Text = ds.Tables[0].Rows[0]["ldesc3"].ToString();
                    //txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    //ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                    //txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                    ddlCategory.SelectedValue = ds.Tables[0].Rows[0]["cat"].ToString();
                    //ddlUnit.SelectedValue = ds.Tables[0].Rows[0]["lelev"].ToString();
                    hdnUnitID.Value = ds.Tables[0].Rows[0]["lelev"].ToString();
                    FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
                    txtUnit.Text = ds.Tables[0].Rows[0]["unitname"].ToString();
                    hdnProjectId.Value = ds.Tables[0].Rows[0]["job"].ToString();
                    GetJobCode();
                    txtProject.Text = ds.Tables[0].Rows[0]["jobdesc"].ToString();
                    if (ds.Tables[0].Rows[0]["jobcode"].ToString() == string.Empty && ds.Tables[0].Rows[0]["phase"].ToString() == "0")
                        txtJobCode.Text = string.Empty;
                    else
                        txtJobCode.Text = objGeneralFunctions.IsNull(ds.Tables[0].Rows[0]["jobcode"].ToString(), "NA") + "/" + ds.Tables[0].Rows[0]["phase"].ToString() + "/" + ds.Tables[0].Rows[0]["jobitemdesc"].ToString();

                    hdnProjectCode.Value = ds.Tables[0].Rows[0]["phase"].ToString() + ":" + ds.Tables[0].Rows[0]["jobcode"].ToString() + ":" + ds.Tables[0].Rows[0]["jobitemdesc"].ToString();

                    txtReason.Text = ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|')[0];
                    if (ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|').Count() > 1)
                    {
                        txtTranslate.Text = ds.Tables[0].Rows[0]["fdesc"].ToString().Split('|')[1];
                        if (txtTranslate.Text.Trim() != string.Empty)
                        {
                            pnlTranslate.Attributes.Add("style", " display: block;");
                        }
                    }
                    txtWorkCompl.Text = ds.Tables[0].Rows[0]["descres"].ToString().Split('|')[0];
                    if (ds.Tables[0].Rows[0]["descres"].ToString().Split('|').Count() > 1)
                    {
                        txtTransDesc.Text = ds.Tables[0].Rows[0]["descres"].ToString().Split('|')[1];
                        //if (Request.QueryString["comp"].ToString() == "2" && txtTransDesc.Text.Trim() != string.Empty)
                        if (txtTransDesc.Text.Trim() != string.Empty)
                        {
                            //txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtTransDesc.Text, "es", "en");
                            pnlTransDesc.Attributes.Add("style", "display: block;");
                        }
                    }
                    if (ds.Tables[0].Rows[0]["phone"].ToString().Trim() != string.Empty)
                        txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    if (ds.Tables[0].Rows[0]["cphone"].ToString().Trim() != string.Empty)
                        txtCell.Text = ds.Tables[0].Rows[0]["cphone"].ToString();
                    FillWorker(ds.Tables[0].Rows[0]["dworkup"].ToString());
                    ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["dworkup"].ToString();
                    ddlRoute_SelectedIndexChanged(sender, e);
                    ViewState["workid"] = ds.Tables[0].Rows[0]["dworkup"].ToString();
                    txtCallDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["cdate"]).ToShortDateString();
                    txtCallTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["cdate"]).ToShortTimeString().Replace("12:00 AM", "");
                    if (ds.Tables[0].Rows[0]["edate"] != DBNull.Value)
                    {
                        txtSchDt.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"]).ToShortDateString();
                        txtSchTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["edate"]).ToShortTimeString().Replace("12:00 AM", "");
                    }
                    ddlStatus.SelectedValue = ds.Tables[0].Rows[0]["assigned"].ToString();
                    ChangeStatus();
                    txtEST.Text = ds.Tables[0].Rows[0]["est"].ToString();
                    lblTicketnumber.Text = ds.Tables[0].Rows[0]["id"].ToString();
                    lblTicketnumber.Visible = true;
                    lblTicketLabel.Visible = true;
                    txtRT.Text = ds.Tables[0].Rows[0]["Reg"].ToString();
                    txtOT.Text = ds.Tables[0].Rows[0]["ot"].ToString();
                    txtNT.Text = ds.Tables[0].Rows[0]["nt"].ToString();
                    txtDT.Text = ds.Tables[0].Rows[0]["dt"].ToString();
                    txtTT.Text = ds.Tables[0].Rows[0]["tt"].ToString();
                    txtTotal.Text = ds.Tables[0].Rows[0]["total"].ToString();
                    txtExpMisc.Text = ds.Tables[0].Rows[0]["othere"].ToString();
                    txtExpToll.Text = ds.Tables[0].Rows[0]["toll"].ToString();
                    txtExpZone.Text = ds.Tables[0].Rows[0]["zone"].ToString();
                    txtMileStart.Text = ds.Tables[0].Rows[0]["Smile"].ToString();
                    txtMileEnd.Text = ds.Tables[0].Rows[0]["emile"].ToString();
                    chkChargeable.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Chargen"]);
                    chkReviewed.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ClearCheck1"]);
                    hdnReviewed.Value = ds.Tables[0].Rows[0]["ClearCheck1"].ToString();
                    chkTimeTrans.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["TimeTransfer"]);
                    //txtInvoiceNo.Text = ds.Tables[0].Rows[0]["invoice"].ToString();
                    txtInvoiceNo.Text = ds.Tables[0].Rows[0]["manualinvoice"].ToString();
                    if (ds.Tables[0].Rows[0]["invoice"].ToString() == "0")
                    {
                        txtInvoiceNo.Text = string.Empty;
                    }
                    if (ds.Tables[0].Rows[0]["internet"] != DBNull.Value)
                    {
                        chkInternet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["internet"]);
                    }
                    if (ViewState["internetdefault"].ToString() == "1" && chkReviewed.Checked == false && ddlStatus.SelectedValue == "4")
                        chkInternet.Checked = true;
                    txtNameWho.Text = ds.Tables[0].Rows[0]["who"].ToString();
                    //txtRemarks.Text = ds.Tables[0].Rows[0]["bremarks"].ToString();
                    txtRecommendation.Text = ds.Tables[0].Rows[0]["bremarks"].ToString();
                    ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    txtCst1.Text = ds.Tables[0].Rows[0]["custom1"].ToString();
                    txtCst2.Text = ds.Tables[0].Rows[0]["custom2"].ToString();
                    txtCst3.Text = ds.Tables[0].Rows[0]["custom3"].ToString();
                    txtCst4.Text = ds.Tables[0].Rows[0]["custom4"].ToString();
                    txtCst5.Text = ds.Tables[0].Rows[0]["custom5"].ToString();
                    txtTickCustom1.Text = ds.Tables[0].Rows[0]["Customtick1"].ToString();
                    txtTickCustom2.Text = ds.Tables[0].Rows[0]["Customtick2"].ToString();
                    txtTickCustom3.Text = ds.Tables[0].Rows[0]["Customtick5"].ToString();
                    chkTickCustom1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Customticket3"]);
                    chkTickCustom2.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Customticket4"]);

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["highdecline"].ToString()))
                    {
                        imgHigh.Visible = Convert.ToBoolean(ds.Tables[0].Rows[0]["highdecline"]);
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["custom6"].ToString()))
                    {
                        chkCst1.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custom6"]);
                    }

                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["custom7"].ToString()))
                    {
                        chkCst2.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custom7"]);
                    }

                    if (ds.Tables[0].Rows[0]["timeroute"] != DBNull.Value)
                    {
                        txtEnrTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timeroute"]).ToShortTimeString();
                    }
                    if (ds.Tables[0].Rows[0]["timesite"] != DBNull.Value)
                    {
                        txtOnsitetime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timesite"]).ToShortTimeString();
                    }
                    if (ds.Tables[0].Rows[0]["timecomp"] != DBNull.Value)
                    {
                        txtComplTime.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["timecomp"]).ToShortTimeString();
                    }
                    //if (ds.Tables[0].Rows[0]["signature"] != DBNull.Value)
                    //{
                    //    string img = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["signature"]);
                    //    imgSign.ImageUrl = img;
                    //    hdnImg.Value = img;
                    //}
                    ddlPayroll.SelectedValue = ds.Tables[0].Rows[0]["qbpayrollitem"].ToString();
                    ddlService.SelectedValue = ds.Tables[0].Rows[0]["qbserviceitem"].ToString();
                    ddlWage.SelectedValue = ds.Tables[0].Rows[0]["wagec"].ToString();
                    //if (ds.Tables[0].Rows[0]["fby"].ToString().Trim() != string.Empty)
                    txtFby.Text = ds.Tables[0].Rows[0]["fby"].ToString();

                    string signature = GetTicketSignature(ds.Tables[0].Rows[0]["id"].ToString(), ds.Tables[0].Rows[0]["fwork"].ToString()).Trim();
                    if (signature != string.Empty)
                    {
                        imgSign.ImageUrl = signature;
                        hdnImg.Value = signature;
                    }

                    selectEquip(Convert.ToInt32(Request.QueryString["id"].ToString()));

                    /******** Prevent invoice listing on copy ticket *********/
                    /////
                    if (Request.QueryString["copy"] == null)
                    {
                        if (ds.Tables[0].Rows[0]["invoice"] != DBNull.Value)
                        {
                            if (ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != string.Empty && ds.Tables[0].Rows[0]["invoice"].ToString().Trim() != "0")
                            {
                                txtInvoiceNo.Text = ds.Tables[0].Rows[0]["invoice"].ToString();
                                txtInvoiceNo.Enabled = false;
                                chkChargeable.Enabled = false;
                                chkInvoice.Enabled = false;

                                if (Session["MSM"].ToString() != "TS")
                                {
                                    lnkInvoice.Visible = true;
                                    lnkInvoice.Attributes["onclick"] = "window.open('addinvoice.aspx?o=1&uid=" + ds.Tables[0].Rows[0]["invoice"].ToString() + "', 'Invoice', 'height=768,width=1280,scrollbars=yes');";

                                    if (ds.Tables[0].Rows[0]["qbinvoiceid"].ToString() != "")
                                    {
                                        imgInv.ImageUrl = "images/QB_invoice.png";
                                        imgInv.ToolTip = "Invoice created in QuickBooks";
                                    }
                                }
                                //lnkInvoice.Attributes["href"] = "addinvoice.aspx?o=1&uid=" + ds.Tables[0].Rows[0]["invoice"].ToString();//, 'Invoice', 'height=768,width=1280,scrollbars=yes'
                            }
                        }
                        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
                        {
                            if (ds.Tables[0].Rows[0]["dworkup"].ToString().Trim() != string.Empty)
                            {
                                if (Session["username"].ToString().ToUpper() != ds.Tables[0].Rows[0]["superv"].ToString().ToUpper())
                                {
                                    lnkSave.Visible = false;
                                    lnkPrint.Visible = false;
                                    pnlDocumentButtons.Visible = false;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Readonly Mode.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                                }
                            }
                        }
                        ViewState["title"] = "Ticket# " + Request.QueryString["id"].ToString() + " : Mobile Office Manager";

                    }

                    GetDocuments();

                }
                #endregion


                if (Request.QueryString["comp"] != null)
                {
                    /******** check whether ticket is completed and is TS login ********* IntegrateIntegrate */
                    if (ViewState["tsint"].ToString() != "1")
                    {
                        if (Session["MSM"].ToString() == "TS")//&& Convert.ToInt32(Request.QueryString["comp"]) != 1
                        {
                            if (Request.QueryString["copy"] != null)
                            {
                                ddlStatus.SelectedValue = "1";
                                ChangeStatus();
                            }
                            else
                            {
                                if (ddlStatus.SelectedValue == "4")
                                {
                                    lnkPrint.OnClientClick = "javascript:Page_ValidationActive = false;";
                                    lnkPrint.CausesValidation = false;
                                    lnkSave.Visible = false;
                                    //lnkPrint.Visible = false;
                                    ViewState["readonly"] = "1";
                                    pnlDocumentButtons.Visible = false;
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "keySuccUp", "noty({text: 'Readonly mode. Please complete ticket from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                                }
                            }
                        }
                    }
                }

                if (Request.QueryString["st"] != null)
                {
                    ddlStatus.SelectedValue = Request.QueryString["st"].ToString();
                    ChangeStatus();
                }

                /********Check for follow up tikcet ********/
                if (Request.QueryString["follow"] != null)
                {
                    lblHeader.Text = "Follow-up Ticket";
                    ViewState["title"] = lblHeader.Text + " : Mobile Office Manager";
                    txtReason.Text = "Follow-up ticket on ticket# " + Request.QueryString["id"].ToString() + "\n" + txtReason.Text;
                    if (Convert.ToInt16(Session["IsMultiLang"]) != 0)
                    {
                        txtTranslate.Text = TranslateMethod(GetAccessToken(), "Follow-up ticket on ticket#", "en", "es") + " " + Request.QueryString["id"].ToString() + "\n" + txtTranslate.Text;
                    }
                    chkWorkComp.Checked = true;
                    chkReviewed.Checked = false;
                    ddlStatus.SelectedValue = "1";
                    ChangeStatus();
                    txtInvoiceNo.Text = string.Empty;
                    //txtWO.Text = string.Empty;

                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuucessFollowupMSg", "  noty({text: 'Ticket# " + Request.QueryString["id"].ToString() + " saved successfully!',dismissQueue: true,  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); ", true);
                    ClientScript.RegisterStartupScript(Page.GetType(), "keyFollowUpMSg", "  noty({text: 'You can now create follow-up ticket below.',dismissQueue: true,  type : 'information', layout:'topCenter',closeOnSelfClick:true, timeout : false,theme : 'noty_theme_default',  closable : false}); ", true);
                }

                /***********fill related tickets (tickets with same WO)************/
                ////FillRelatedTickets();
            }

            /******** Check for TS login**********IntegrateIntegrate*/
            if (ViewState["tsint"].ToString() != "1")
            {
                if (Session["MSM"].ToString() == "TS")
                {
                    //ddlStatus.Enabled = false;
                    btnEnroute.Enabled = false;
                    btnOnsite.Enabled = false;
                    btnComplete.Enabled = false;
                }
            }

            //GetQBInt();
            ShowQBSyncControls();

            /*******set focus on controls using hidden variable. it was needed because ajax toolkit tab control causes default focus to be set at the very first tab. so we need to set it this way.*********/
            hdnFocus.Value = txtLocation.ClientID;

        }

    }

    private void Permission1()
    {
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("toggleMenu");
        //ul.Attributes.Add("class", "page-sidebar-menu page-sidebar-menu-closed");

        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("schMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkSchd");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkListView");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");

        //AjaxControlToolkit.HoverMenuExtender hm = (AjaxControlToolkit.HoverMenuExtender)Page.Master.FindControl("HoverMenuExtenderSchd");
        //hm.Enabled = false;
        //HtmlGenericControl ul = (HtmlGenericControl)Page.Master.FindControl("schdMgrSub");
        //ul.Style.Add("display", "block");
        //ul.Style.Add("visibility", "visible");
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
        if (Session["MSM"].ToString() == "TS")
        {
            //lnkCopy.Visible = false;
            //lnkDelete.Visible = false;
            //lnkAddticket.Visible = false;
        }
    }
    private void Permission()
    {
        #region Permission for location remarks
        string strLocRemarks = string.Empty;
        string strTCFix = string.Empty;
        if (Session["type"].ToString() != "am")
        {
            DataTable dtLocrem = new DataTable();
            dtLocrem = (DataTable)Session["userinfo"];

            strLocRemarks = dtLocrem.Rows[0]["Location"].ToString().Substring(3, 1);
            strTCFix = dtLocrem.Rows[0]["TC"].ToString().Substring(1, 1);
            if (strTCFix == "Y")
            {
                txtEnrTime.Enabled = false;
                txtOnsitetime.Enabled = false;
                txtComplTime.Enabled = false;
                MaskedEditValidator1.Enabled = false;
                MaskedEditValidator2.Enabled = false;
                MaskedEditValidator4.Enabled = false;
                //btnEnroute.Enabled = false;
                //btnOnsite.Enabled = false;
                //btnComplete.Enabled = false;
                //ddlStatus.Items[2].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[3].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[4].Attributes["disabled"] = "disabled";
                //ddlStatus.Items[2].Attributes["style"] = "background-color:silver";
                //ddlStatus.Items[3].Attributes["style"] = "background-color:silver";
                //ddlStatus.Items[4].Attributes["style"] = "background-color:silver";
            }
        }
        else
        {
            strLocRemarks = "Y";
        }
        if (strLocRemarks == "Y")
        {
            txtRemarks.Visible = true;
            lblRemarks.Visible = true;
        }
        else
        {
            txtRemarks.Visible = false;
            lblRemarks.Visible = false;
        }
        #endregion

        if (ViewState["title"] != null)
        {
            if (ViewState["title"].ToString() != string.Empty)
                Page.Title = ViewState["title"].ToString();
        }
    }

    private void userpermissions()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        DataSet dsLastSync = objBL_General.getSagelatsync(objGeneral);
        int intintegration = Convert.ToInt32(dsLastSync.Tables[0].Rows[0]["sageintegration"]);
        if (intintegration == 1)
        { txtCustSageID.Visible = true; lblSageid.Visible = true; hdnSageInt.Value = "1"; }
        else
        { txtCustSageID.Visible = false; lblSageid.Visible = false; hdnSageInt.Value = "0"; }

        if (Session["type"].ToString() != "c")
        {
            if (Session["type"].ToString() != "am")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Username = Session["username"].ToString();
                //objPropUser.PageName = "ticketlistview.aspx";
                objPropUser.PageName = "addticket.aspx";
                DataSet dspage = objBL_User.getScreensByUser(objPropUser);
                if (dspage.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["access"].ToString()) == false)
                    {
                        Response.Redirect("home.aspx");
                    }
                    //if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["view"].ToString()) == false)
                    //{
                    //    if (Request.QueryString["id"] != null)
                    //    {
                    //        Response.Redirect("home.aspx");
                    //    }
                    //}
                    //if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["edit"].ToString()) == false)
                    //{
                    //    if (Request.QueryString["id"] != null)
                    //    {
                    //        lnkSave.Visible = false;
                    //        lnkPrint.Visible = false;
                    //        lnkConvert.Enabled = false;
                    //    }
                    //}
                    //if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["add"].ToString()) == false)
                    //{
                    //    if (Request.QueryString["id"] == null)
                    //    {
                    //        lnkSave.Visible = false;
                    //        lnkPrint.Visible = false;
                    //        lnkConvert.Enabled = false;
                    //    }
                    //}                
                    //if (Convert.ToBoolean(dspage.Tables[0].Rows[0]["delete"].ToString()) == false)
                    //{

                    //}
                }
                else
                {
                    Response.Redirect("home.aspx");
                }
            }
        }
    }


    private void FillDefaultRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);
        ddlDefRoute.DataSource = ds.Tables[0];
        ddlDefRoute.DataTextField = "Name";
        ddlDefRoute.DataValueField = "ID";
        ddlDefRoute.DataBind();
        ddlDefRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void GetOpportTicket(int TicketID)
    {
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = TicketID;
        string opportID = objBL_MapData.GetopportunityTicket(objMapData);

        if (!string.IsNullOrEmpty(opportID.Trim()))
        {
            lnkOpport.Visible = true;
            lnkOpport.Text = "Opportunity# " + opportID;
            lnkOpport.NavigateUrl = "addopprt.aspx?uid=" + opportID;

            if (Session["MSM"].ToString() == "TS")
            {
                lnkOpport.Enabled = false;
            }
        }
    }

    /// <summary>
    /// Fill data to custom fields tab
    /// </summary>
    private void FillCustomFields()
    {
        DataSet dscstm = new DataSet();

        dscstm = GetCustomFields("Ticket2");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket3");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom3.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket1");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket4");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom4.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket5");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom5.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket6");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom6.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("Ticket7");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustom7.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst1");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick1.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst2");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick2.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst3");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick3.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst4");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick4.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
        dscstm = GetCustomFields("TicketCst5");
        if (dscstm.Tables[0].Rows.Count > 0)
        {
            lblCustomTick5.Text = dscstm.Tables[0].Rows[0]["label"].ToString();
        }
    }

    /// <summary>
    /// operation related to gridview row javascript.
    /// </summary>
    /// <param name="o"></param>
    /// <param name="e"></param>
    protected void Page_PreRender(Object o, EventArgs e)
    {
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblUnit");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            TextBox txtPrice = (TextBox)gr.FindControl("txtPrice");
            TextBox txtHours = (TextBox)gr.FindControl("txtHours");

            chkSelect.Attributes["onclick"] = "SelectRows('" + gvEquip.ClientID + "','" + txtUnit.ClientID + "','" + hdnUnitID.ClientID + "');";
            //Label lblID = (Label)gr.FindControl("lblID");
            //Label lblname = (Label)gr.FindControl("lblUnit");
            //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            //gr.Attributes["onclick"] = "SelectRowFill('" + gvEquip.ClientID + "','" + lblID.ClientID + "','" + lblname.ClientID + "','" + hdnUnitID.ClientID + "','" + txtUnit.ClientID + "','divequip'); document.getElementById('" + btnEquip.ClientID + "').click();";
        }

        foreach (GridViewRow gr in gvProject.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblname = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowFill('" + gvProject.ClientID + "','" + lblID.ClientID + "','" + lblname.ClientID + "','" + hdnProjectId.ClientID + "','" + txtProject.ClientID + "','divproject');  document.getElementById('" + btnGetCode.ClientID + "').click();";
        }

        foreach (GridViewRow gr in gvProjectCode.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            Label lblIDname = (Label)gr.FindControl("lblIDname");
            Label lblIDname1 = (Label)gr.FindControl("lblIDname1");
            Label lblname = (Label)gr.FindControl("lblDesc");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowFill('" + gvProjectCode.ClientID + "','" + lblIDname.ClientID + "','" + lblIDname1.ClientID + "','" + hdnProjectCode.ClientID + "','" + txtJobCode.ClientID + "','divProjectCode');";
        }

        foreach (GridViewRow gr in gvDocuments.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

            gr.Attributes["onclick"] = "SelectRowChk('" + gr.ClientID + "','" + chkSelect.ClientID + "','" + gvDocuments.ClientID + "',event);";
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "alertScript", "SelectedRowStyle('" + gvDocuments.ClientID + "');", true);
        Permission();
    }

    /// <summary>
    /// Get data from Control table to show hide controls. Currently it handles the multilanguage option controls.
    /// </summary>
    private void GetControl()
    {
        int Multilang = Convert.ToInt16(Session["IsMultiLang"]);
        hdnMultiLang.Value = Multilang.ToString();
        if (Multilang == 0)
        {
            divTransicon.Visible = false;
            divTransIconReason.Visible = false;
        }
    }

    private void getDiagnosticCategory()
    {
        DataSet ds = new DataSet();
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getDiagnosticCategory(objGeneral);
        ddlCodeCat.DataSource = ds.Tables[0];
        ddlCodeCat.DataTextField = "category";
        ddlCodeCat.DataValueField = "category";
        ddlCodeCat.DataBind();

        ddlCodeCat.Items.Insert(0, new ListItem("ALL", "ALL"));
    }

    /// <summary>
    /// Fill custom fields data according to the field name paased.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private DataSet GetCustomFields(string name)
    {
        DataSet ds = new DataSet();
        objGeneral.CustomName = name;
        objGeneral.ConnConfig = Session["config"].ToString();
        ds = objBL_General.getCustomFields(objGeneral);
        return ds;
    }

    private void GetDataEquip()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.SearchBy = string.Empty;
        objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        //objPropUser.SearchBy = "e.loc";
        //objPropUser.SearchValue = hdnLocId.Value;
        objPropUser.InstallDate = string.Empty;
        objPropUser.ServiceDate = string.Empty;
        objPropUser.Price = string.Empty;
        objPropUser.Manufacturer = string.Empty;
        objPropUser.Status = -1;
        ds = objBL_User.getElev(objPropUser);
        gvEquip.DataSource = ds.Tables[0];
        gvEquip.DataBind();
    }

    private void selectEquip(int ticketID)
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.TicketID = ticketID;
        ds = objBL_MapData.getElevByTicket(objMapData);
        txtUnit.Text = string.Empty;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            foreach (GridViewRow gr in gvEquip.Rows)
            {
                Label lblID = (Label)gr.FindControl("lblID");
                Label lblname = (Label)gr.FindControl("lblunit");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                TextBox txtHours = (TextBox)gr.FindControl("txtHours");
                if (dr["elev_id"].ToString() == lblID.Text)
                {
                    chkSelect.Checked = true;
                    txtHours.Text = dr["labor_percentage"].ToString();

                    if (txtUnit.Text != string.Empty)
                    {
                        txtUnit.Text = txtUnit.Text + ", " + lblname.Text;
                    }
                    else
                    {
                        txtUnit.Text = lblname.Text;
                    }
                }
            }
        }
    }

    private void GetDataProject()
    {
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.LocID = Convert.ToInt32(hdnLocId.Value);

        ds = objBL_Customer.getJobEstimate(objCustomer);
        gvProject.DataSource = ds.Tables[0];
        gvProject.DataBind();
    }

    private void FillWorker(string Worker)
    {
        ddlRoute.Items.Clear();
        ddlRoute.SelectedIndex = -1;
        ddlRoute.SelectedValue = null;
        ddlRoute.ClearSelection();
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Status = 0;
        objPropUser.Username = Worker;
        ds = objBL_User.getEMP(objPropUser);
        ddlRoute.DataSource = ds.Tables[0];
        ddlRoute.DataTextField = "fDesc";
        ddlRoute.DataValueField = "fDesc";
        ddlRoute.DataBind();

        ddlRoute.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void FillCategory()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getCategory(objPropUser);
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataTextField = "type";
        ddlCategory.DataValueField = "type";
        ddlCategory.DataBind();

        ddlCategory.Items.Insert(0, new ListItem(":: Select ::", ""));
        ddlCategory.Items.Insert(1, new ListItem("None", "None"));
    }

    private void FillDepartment()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();

        ds = objBL_User.getDepartment(objPropUser);

        ddlDepartment.DataSource = ds.Tables[0];
        ddlDepartment.DataTextField = "type";
        ddlDepartment.DataValueField = "id";
        ddlDepartment.DataBind();

        ddlDepartment.Items.Insert(0, new ListItem(":: Select ::", ""));

        if (Session["MSM"].ToString() != "TS")
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["isdefault"].ToString() == "1")
                {
                    ddlDepartment.SelectedValue = dr["id"].ToString();
                }
            }
        }
    }

    private void FillElevUnit()
    {
        //if (hdnLocId.Value != "")
        //{
        //    DataSet ds = new DataSet();
        //    objPropUser.ConnConfig = Session["config"].ToString();
        //    objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
        //    ds = objBL_User.getElevUnit(objPropUser);
        //    ddlUnit.DataSource = ds.Tables[0];
        //    ddlUnit.DataTextField = "unit";
        //    ddlUnit.DataValueField = "id";
        //    ddlUnit.DataBind();
        //}
        //ddlUnit.Items.Insert(0, new ListItem("None", "0"));
    }

    /// <summary>
    /// fills location accoding to the customer selected.
    /// </summary>
    public void FillLoc()
    {
        //if (hdnPatientId.Value != string.Empty)
        //{
        //    objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        //    objPropUser.DBName = Session["dbname"].ToString();
        //    DataSet ds = new DataSet();
        //    ds = objBL_User.getLocationByCustomerID(objPropUser);

        //    ddlLoc.DataSource = ds.Tables[0];
        //    ddlLoc.DataTextField = "Name";
        //    ddlLoc.DataValueField = "loc";
        //    ddlLoc.DataBind();
        //}
        //ddlLoc.Items.Insert(0,new ListItem(":: Select ::",""));

        DataSet ds = new DataSet();
        objPropUser.SearchValue = "";
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);
        ds = objBL_User.getLocationAutojquery(objPropUser);
        SetProspect();
        if (ds.Tables[0].Rows.Count == 1)
        {
            hdnLocId.Value = ds.Tables[0].Rows[0]["value"].ToString();
            txtLocation.Text = ds.Tables[0].Rows[0]["label"].ToString();
            FillLocInfo();
            FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
        }
        else if (ds.Tables[0].Rows.Count > 1)
        { txtCustSageID.Text = ds.Tables[0].Rows[0]["custsageid"].ToString(); }
        else
        { txtCustSageID.Text = string.Empty; }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCustomers(string prefixText, int count, string contextKey)
    {
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        //objPropUser.DBName = HttpContext.Current.Session["dbname"].ToString();
        //objPropUser.SearchBy = "Name";
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        ds = objBL_User.getCustomerAuto(objPropUser);
        //ds = objBL_User.getCustomerSearch(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["id"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] Getlocation(string prefixText, int count, string contextKey)
    {
        BusinessEntity.User objPropUser = new BusinessEntity.User();
        BL_User objBL_User = new BL_User();

        DataSet ds = new DataSet();
        objPropUser.SearchValue = prefixText;
        objPropUser.ConnConfig = HttpContext.Current.Session["config"].ToString();
        //objPropUser.CustomerID = Convert.ToInt32(hdnPatientId.Value);           
        ds = objBL_User.getLocationByCustomerID(objPropUser);

        DataTable dt = ds.Tables[0];

        List<string> txtItems = new List<string>();
        String dbValues;

        foreach (DataRow row in dt.Rows)
        {
            dbValues = AutoCompleteExtender.CreateAutoCompleteItem(row["Name"].ToString(), row["loc"].ToString());
            txtItems.Add(dbValues);
        }

        return txtItems.ToArray();
    }

    protected void btnSelectCustomer_Click(object sender, EventArgs e)
    {
        lnkConvert.Visible = false;
        FillLoc();
        hdnFocus.Value = txtLocation.ClientID;
    }

    //protected void ddlLoc_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    //ResetFormControlValues(this);
    //    FillLocInfo();
    //}


    /// <summary>
    /// Fills location data for the location/customer selected.
    /// </summary>
    private void FillLocInfo()
    {
        if (hdnLocId.Value == "")
        {
            return;
        }
        if (hdnProspect.Value != "1")//hdnPatientId.Value != string.Empty
        {
            RequiredFieldValidator1.Enabled = true;
            objPropUser.DBName = Session["dbname"].ToString();
            objPropUser.LocID = Convert.ToInt32(hdnLocId.Value);
            DataSet ds = new DataSet();
            ds = objBL_User.getLocationByID(objPropUser);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtLocation.Text = ds.Tables[0].Rows[0]["tag"].ToString();
                txtAcctno.Text = ds.Tables[0].Rows[0]["id"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["LocAddress"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["LocCity"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["locstate"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["locZip"].ToString();
                //ddlRoute.SelectedValue = ds.Tables[0].Rows[0]["Route"].ToString();             
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["name"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtCustSageID.Text = ds.Tables[0].Rows[0]["custsageid"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["custname"].ToString();
                hdnPatientId.Value = ds.Tables[0].Rows[0]["owner"].ToString();
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                hdnRolID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                //lblDefwork.Text = ds.Tables[0].Rows[0]["defwork"].ToString();
                ddlDefRoute.SelectedValue = ds.Tables[0].Rows[0]["route"].ToString();
                txtCreditReason.Text = ds.Tables[0].Rows[0]["creditreason"].ToString();
                chkDispAlert.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["DispAlert"]);
                chkCreditHold.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Credit"]);
                //hdnUnitID.Value = string.Empty;
                //txtUnit.Text = string.Empty;
                txtProject.Text = string.Empty;
                hdnProjectId.Value = string.Empty;
                if (Session["MSM"].ToString() != "TS")
                    lnkConvert.Visible = false;
                ddlDefRoute.Enabled = true;
                RequiredFieldValidator34.Enabled = true;
            }
            GetDataEquip();
            FillElevUnit();
            GetDataProject();
        }
        else
        {
            SetProspect();
            RequiredFieldValidator1.Enabled = false;
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.ProspectID = Convert.ToInt32(hdnLocId.Value);
            DataSet ds = new DataSet();
            ds = objBL_Customer.getProspectByID(objCustomer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtAcctno.Text = "--";
                txtCustSageID.Text = "--";
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text = ds.Tables[0].Rows[0]["Zip"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                txtMaincontact.Text = ds.Tables[0].Rows[0]["contact"].ToString();
                txtPhoneCust.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtCell.Text = ds.Tables[0].Rows[0]["cellular"].ToString();
                txtCustomer.Text = ds.Tables[0].Rows[0]["name"].ToString();
                hdnRolID.Value = ds.Tables[0].Rows[0]["rol"].ToString();
                txtLocation.Text = string.Empty;
                //hdnUnitID.Value = string.Empty;
                //txtUnit.Text = string.Empty;
                txtProject.Text = string.Empty;
                hdnProjectId.Value = string.Empty;
                if (Session["MSM"].ToString() != "TS")
                    lnkConvert.Visible = true;
                lat.Value = ds.Tables[0].Rows[0]["lat"].ToString();
                lng.Value = ds.Tables[0].Rows[0]["lng"].ToString();
                ddlDefRoute.Enabled = false;
                ddlDefRoute.SelectedIndex = 0;
                RequiredFieldValidator34.Enabled = false;
            }
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

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        //if (!Page.IsValid)
        //{
        //    ClientScript.RegisterStartupScript(Page.GetType(), "keyValid", "noty({text: 'Fill all the fields.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        //    return;
        //}
        if (hdnProspect.Value == string.Empty)
        {
            RequiredFieldValidator1.Enabled = true;
        }
        else
        {
            RequiredFieldValidator1.Enabled = false;
        }

        /*****************
         validation for prospect ticket completion. prospect need to be converted to customer before ticket completion. 
         If prospect ticket is completed from device (viewstate["comp"]=2) then it need to be redirect to convert process without saving the ticket data. 
         *****************/
        int deviceCompletedConvert = 0;
        if (ViewState["comp"].ToString() == "2" && ViewState["convert"].ToString() == "1")
        {
            deviceCompletedConvert = 1;
        }

        if (hdnProspect.Value != string.Empty && ddlStatus.SelectedValue == "4" && deviceCompletedConvert == 0)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keyProspectComplete", "noty({text: 'Ticket can be completed only for customers. Please convert the selected Lead to Customer first.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            return;
        }


        try
        {
            if (txtEnrTime.Text.Trim() == string.Empty)
            {
                objMapData.EnrouteTime = System.DateTime.MinValue;
                //objMapData.EnrouteTime =Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.EnrouteTime = Convert.ToDateTime(txtSchDt.Text + " " + txtEnrTime.Text);
                objMapData.EnrouteTime = Convert.ToDateTime(defaultDate + " " + txtEnrTime.Text);
            }

            if (txtOnsitetime.Text.Trim() == string.Empty)
            {
                objMapData.OnsiteTime = System.DateTime.MinValue;
                //objMapData.OnsiteTime = Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.OnsiteTime = Convert.ToDateTime(txtSchDt.Text + " " + txtOnsitetime.Text);
                objMapData.OnsiteTime = Convert.ToDateTime(defaultDate + " " + txtOnsitetime.Text);
            }

            if (txtComplTime.Text.Trim() == string.Empty)
            {
                objMapData.ComplTime = System.DateTime.MinValue;
                //objMapData.ComplTime = Convert.ToDateTime(defaultDate);
            }
            else
            {
                //objMapData.ComplTime = Convert.ToDateTime(txtSchDt.Text + " " + txtComplTime.Text);
                objMapData.ComplTime = Convert.ToDateTime(defaultDate + " " + txtComplTime.Text);
            }
            if (hdnLocId.Value == string.Empty)
            {
                objMapData.LocID = 0;
            }
            else
            {
                objMapData.LocID = Convert.ToInt32(hdnLocId.Value);
            }
            objMapData.CustomerName = txtCustomer.Text;
            if (hdnPatientId.Value == string.Empty)
            {
                objMapData.CustID = 0;
            }
            else
            {
                objMapData.CustID = Convert.ToInt32(hdnPatientId.Value);
            }
            objMapData.LocTag = txtLocation.Text;
            objMapData.LocAddress = txtAddress.Text;
            objMapData.City = txtCity.Text;
            objMapData.State = ddlState.SelectedValue;
            objMapData.Zip = txtZip.Text;
            objMapData.Phone = txtPhoneCust.Text;
            objMapData.Cell = txtCell.Text;
            objMapData.Worker = ddlRoute.SelectedValue;
            objMapData.CallDate = Convert.ToDateTime(txtCallDt.Text + " " + txtCallTime.Text);

            if (txtSchDt.Text.Trim() != string.Empty)
            {
                objMapData.SchDate = Convert.ToDateTime(txtSchDt.Text + " " + txtSchTime.Text);
            }
            else
            {
                objMapData.SchDate = System.DateTime.MinValue;
            }

            objMapData.Assigned = Convert.ToInt32(ddlStatus.SelectedValue);
            objMapData.Category = ddlCategory.SelectedValue;
            if (hdnUnitID.Value != "")
            {
                objMapData.Unit = Convert.ToInt32(hdnUnitID.Value);
            }

            if (hdnProjectId.Value != "")
            {
                objMapData.jobid = Convert.ToInt32(hdnProjectId.Value);
            }



            if (txtTranslate.Text.Trim() != string.Empty)
            {
                objMapData.Reason = txtReason.Text.Replace('|', ' ') + "|" + txtTranslate.Text.Replace('|', ' ');
            }
            else
            {
                objMapData.Reason = txtReason.Text;
            }

            if (txtEST.Text.Trim() != string.Empty)
            {
                objMapData.EST = Convert.ToDouble(txtEST.Text);
            }
            else
            {
                objMapData.EST = 00.50;
            }
            objMapData.ConnConfig = Session["config"].ToString();

            if (hdnProjectCode.Value != "")
            {
                objMapData.jobcode = hdnProjectCode.Value;
            }


            //Separate the english and spanish text by '|'
            if (txtTransDesc.Text.Trim() != string.Empty)
            {
                objMapData.CompDescription = txtWorkCompl.Text.Replace('|', ' ') + "|" + txtTransDesc.Text.Replace('|', ' ');
            }
            else
            {
                objMapData.CompDescription = txtWorkCompl.Text;
            }

            double RT = Convert.ToDouble(objGeneralFunctions.IsNull(txtRT.Text, "0"));
            double OT = Convert.ToDouble(objGeneralFunctions.IsNull(txtOT.Text, "0"));
            double NT = Convert.ToDouble(objGeneralFunctions.IsNull(txtNT.Text, "0"));
            double TT = Convert.ToDouble(objGeneralFunctions.IsNull(txtTT.Text, "0"));
            double DT = Convert.ToDouble(objGeneralFunctions.IsNull(txtDT.Text, "0"));

            //if (txtOnsitetime.Text != "" && txtEnrTime.Text != "" && txtOnsitetime.Text != "__:__ AM" && txtOnsitetime.Text != "__:__ PM" && txtEnrTime.Text != "__:__ AM" && txtEnrTime.Text != "__:__ PM")
            //{
            //    if (TT == 0)
            //    {
            //        TimeSpan diff = Convert.ToDateTime("01/01/2009 " + txtOnsitetime.Text) - Convert.ToDateTime("01/01/2009 " + txtEnrTime.Text);
            //        TT = diff.TotalHours;
            //    }
            //}

            //if (txtOnsitetime.Text != "" && txtComplTime.Text != "" && txtOnsitetime.Text != "__:__ AM" && txtOnsitetime.Text != "__:__ PM" && txtComplTime.Text != "__:__ AM" && txtComplTime.Text != "__:__ PM") {
            //    if (RT == 0)
            //    {
            //        TimeSpan diff = Convert.ToDateTime("01/01/2009 " + txtComplTime.Text) - Convert.ToDateTime("01/01/2009 " + txtOnsitetime.Text);
            //        RT = diff.TotalHours;
            //    }
            //}

            double ZoneExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpZone.Text, "0"));
            double TollExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpToll.Text, "0"));
            double MiscExp = Convert.ToDouble(objGeneralFunctions.IsNull(txtExpMisc.Text, "0"));

            objMapData.ZoneExpense = ZoneExp;
            objMapData.TollExpense = TollExp;
            objMapData.MiscExpense = MiscExp;

            objMapData.RT = RT;
            objMapData.OT = OT;
            objMapData.NT = NT;
            objMapData.TT = TT;
            objMapData.DT = DT;
            //objMapData.Total = Convert.ToDouble(txtTotal.Text);
            objMapData.Total = RT + OT + NT + TT + DT;
            objMapData.Charge = Convert.ToInt32(chkChargeable.Checked);
            objMapData.Review = Convert.ToInt32(chkReviewed.Checked);
            objMapData.Who = txtNameWho.Text;
            objMapData.Remarks = txtRemarks.Text;
            objMapData.Level = 1;
            objMapData.Department = Convert.ToInt32(ddlDepartment.SelectedValue);
            objMapData.Custom2 = txtCst2.Text;
            objMapData.Custom3 = txtCst3.Text;
            objMapData.Custom1 = txtCst1.Text;
            objMapData.Custom4 = txtCst4.Text;
            objMapData.Custom5 = txtCst5.Text;
            objMapData.Custom6 = Convert.ToInt32(chkCst1.Checked);
            objMapData.Custom7 = Convert.ToInt32(chkCst2.Checked);
            objMapData.Workorder = txtWO.Text.Trim();
            objMapData.WorkComplete = Convert.ToInt16(chkWorkComp.Checked);
            objMapData.MileStart = Convert.ToInt32(objGeneralFunctions.IsNull(txtMileStart.Text.ToString(), "0"));
            objMapData.MileEnd = Convert.ToInt32(objGeneralFunctions.IsNull(txtMileEnd.Text.ToString(), "0"));
            objMapData.Internet = Convert.ToInt32(chkInternet.Checked);
            objMapData.ManualInvoiceID = txtInvoiceNo.Text.Trim();
            objMapData.TimeTransfer = Convert.ToInt32(chkTimeTrans.Checked);
            objMapData.DispAlert = Convert.ToInt16(chkDispAlert.Checked);
            objMapData.CreditHold = Convert.ToInt16(chkCreditHold.Checked);
            objMapData.CreditReason = txtCreditReason.Text.Trim();
            objMapData.IsRecurring = 0;
            //objMapData.InvoiceID = Convert.ToInt32(objGeneralFunctions.IsNull(txtInvoiceNo.Text.Trim(),"0"));
            objMapData.QBServiceID = ddlService.SelectedValue;
            objMapData.QBPayrollID = ddlPayroll.SelectedValue;
            objMapData.LastUpdatedBy = Session["username"].ToString();
            objMapData.MainContact = txtMaincontact.Text.Trim();
            objMapData.Recommendation = txtRecommendation.Text.Trim();
            objMapData.CustomTick1 = txtTickCustom1.Text.Trim();
            objMapData.CustomTick2 = txtTickCustom2.Text.Trim();
            objMapData.CustomTick3 = Convert.ToInt16(chkTickCustom1.Checked);
            objMapData.CustomTick4 = Convert.ToInt16(chkTickCustom2.Checked);
            objMapData.CustomTick5 = Convert.ToDouble(objGeneralFunctions.IsNull(txtTickCustom3.Text.Trim(), "0"));
            objMapData.Lat = lat.Value;
            objMapData.Lng = lng.Value;
            if (ddlDefRoute.SelectedValue == string.Empty && hdnPatientId.Value == string.Empty)
                objMapData.DefaultWorker = 0;
            else
                objMapData.DefaultWorker = Convert.ToInt32(ddlDefRoute.SelectedValue);
            if (ddlTemplate.SelectedValue != string.Empty)
                objMapData.JobTemplateID = Convert.ToInt32(ddlTemplate.SelectedValue);
            if (ddlWage.SelectedValue != string.Empty)
                objMapData.WageID = Convert.ToInt32(ddlWage.SelectedValue);

            objMapData.fBy = txtFby.Text.Trim();
            objMapData.dtEquips = GetElevData();
            if (hdnImg.Value != "")
            {
                string str = hdnImg.Value;
                string last = str.Substring(str.LastIndexOf(',') + 1);
                objMapData.Signature = Convert.FromBase64String(last);
            }

            /****** if mode is to update existing record********/
            if (Convert.ToInt32(ViewState["mode"]) == 1)
            {
                int projectID = 0;
                objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"]);
                if (deviceCompletedConvert == 0)
                {
                    if (ViewState["tsint"].ToString() != "1")/*******For TS integration to work TS and MOM together with MOM while the TS database is converted to MOM*********/
                        projectID = objBL_MapData.UpdateTicketInfo(objMapData);
                    else
                        projectID = objBL_MapData.UpdateTicketInfoTS(objMapData);
                }
                if (hdnProjectId.Value == string.Empty || hdnProjectId.Value == "0")
                {
                    hdnProjectId.Value = projectID.ToString();
                    txtProject.Text = projectID.ToString();
                    GetDataProject();
                    GetJobCode();
                }
                hdnReviewed.Value = Convert.ToInt16(chkReviewed.Checked).ToString();
                ViewState["ticid"] = objMapData.TicketID;
                ViewState["assign"] = ddlStatus.SelectedValue;

                if (ViewState["convert"].ToString() == "1")
                {
                    ConvertProspectWizard();
                }
                else
                {
                    string comp = "0";
                    /***** if status is completed********/
                    if (ddlStatus.SelectedValue == "4")
                    {
                        comp = "1";
                        ddlStatus.Enabled = false;
                        btnEnroute.Enabled = false;
                        btnOnsite.Enabled = false;
                        btnComplete.Enabled = false;
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWC", "CheckWorkComplete('" + ViewState["ticid"].ToString() + "'," + Convert.ToInt16(chkWorkComp.Checked) + ",'Ticket updated successfully!','" + comp + "'," + Convert.ToInt16(chkInvoice.Checked) + ");", true);
                    if (ddlStatus.SelectedValue != "4" && ddlStatus.SelectedValue != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm();", true);
                    }
                    if (txtWO.Text.Trim() == string.Empty)
                    {
                        txtWO.Text = lblTicketnumber.Text;
                    }

                    /******** Fill related tickets list**********/
                    FillRelatedTickets();

                    //if (ViewState["workid"].ToString() != ddlRoute.SelectedValue)
                    //{
                    //SendPushNotification();
                    //}        
                }
            }
            /******** when mode is add new record **********/
            else
            {
                if (ViewState["tsint"].ToString() != "1")
                    objBL_MapData.AddTicket(objMapData);
                else
                    objBL_MapData.AddTicketTS(objMapData);


                ViewState["ticid"] = objMapData.TicketID;
                ViewState["assign"] = ddlStatus.SelectedValue;

                /***********Update temp document in documents table with the ticket id created***********/
                UpdateDoc();

                if (ViewState["convert"].ToString() == "1")
                {
                    ConvertProspectWizard();
                }
                else
                {
                    string comp = "0";
                    if (ddlStatus.SelectedValue == "4")
                    {
                        comp = "1";
                    }
                    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyWC", "CheckWorkComplete('" + ViewState["ticid"].ToString() + "'," + Convert.ToInt16(chkWorkComp.Checked) + ",'Ticket added successfully! Ticket# : " + ViewState["ticid"].ToString() + "','" + comp + "',0);", true);
                    if (ddlStatus.SelectedValue != "4" && ddlStatus.SelectedValue != "0")
                    {
                        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyNotyConfirm", "notyConfirm();", true);
                    }

                    ResetFormControlValues(this);
                    txtCallDt.Text = System.DateTime.Now.ToShortDateString();
                    txtCallTime.Text = System.DateTime.Now.ToShortTimeString();
                    txtSchDt.Text = System.DateTime.Now.ToShortDateString();
                    txtSchTime.Text = System.DateTime.Now.ToShortTimeString();
                    lblHeader.Text = "Add Ticket";
                    //SendPushNotification();
                }
            }
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "updateparent", "if(window.opener && !window.opener.closed) { if(window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch')) window.opener.document.getElementById('ctl00_ContentPlaceHolder1_btnSearch').click();}", true);
            success = true;

        }
        catch (Exception ex)
        {
            //lblMsg.Text = ex.Message;
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            success = false;
        }
    }

    private DataTable GetElevData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("ticket_id", typeof(int));
        dt.Columns.Add("elev_id", typeof(int));
        dt.Columns.Add("labor_percentage", typeof(int));

        foreach (GridViewRow gvr in gvEquip.Rows)
        {
            CheckBox chkSelect = (CheckBox)gvr.FindControl("chkSelect");

            if (chkSelect.Checked == true)
            {
                DataRow dr = dt.NewRow();
                Label lblUnit = (Label)gvr.FindControl("lblID");
                TextBox txtHours = (TextBox)gvr.FindControl("txtHours");
                dr["ticket_id"] = 0;
                dr["elev_id"] = Convert.ToInt32(lblUnit.Text);
                if (txtHours.Text.Trim() != string.Empty)
                {
                    dr["labor_percentage"] = Convert.ToInt32(txtHours.Text);
                }
                dt.Rows.Add(dr);
            }
        }
        return dt;
    }


    /// <summary>
    /// update location coordinates for the seleted address for location
    /// </summary>
    private void updateCoordinates()
    {
        if (hdnRolID.Value != string.Empty)
        {
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.RolId = Convert.ToInt32(hdnRolID.Value);
            objPropUser.Lat = lat.Value;
            objPropUser.Lng = lng.Value;

            objBL_User.UpdateRolCoordinates(objPropUser);
        }
    }

    private void SendPushNotification()
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.EmpID = ddlRoute.SelectedValue;
        string str = objBL_General.GetRegID(objGeneral);
        if (str != string.Empty && str != null)
        {
            //obj_PushNotification.Android();
        }
    }

    private void ChangeStatus()
    {
        /*****unassigned **********/
        if (ddlStatus.SelectedValue == "0")
        {
            ddlRoute.SelectedValue = "";
            ChangeRoute();

            txtEnrTime.Text = string.Empty;
            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;


            ddlRoute.Enabled = false;
            txtEnrTime.Enabled = false;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            MaskedEditValidator3.Enabled = false;
            RequiredFieldValidator24.Enabled = false;
            RequiredFieldValidator25.Enabled = false;
            RequiredFieldValidator26.Enabled = false;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;

            txtWorkCompl.Enabled = false;
            txtRecommendation.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Enabled = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false;
        }
        /***********assigned********/
        else if (ddlStatus.SelectedValue == "1")
        {
            txtEnrTime.Text = string.Empty;
            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;

            ddlRoute.Enabled = true;
            txtEnrTime.Enabled = false;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = false;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;

            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Enabled = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false;
        }
        /*********enroute***********/
        else if (ddlStatus.SelectedValue == "2")
        {
            txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            //txtEnrTime.Focus();

            txtComplTime.Text = string.Empty;
            txtOnsitetime.Text = string.Empty;

            ddlRoute.Enabled = true;
            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = false;
            txtComplTime.Enabled = false;

            MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = false;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;

            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Enabled = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false;
        }
        /***********onsite************/
        else if (ddlStatus.SelectedValue == "3")
        {

            txtOnsitetime.Text = System.DateTime.Now.ToShortTimeString();
            //txtOnsitetime.Focus();
            if (txtEnrTime.Text == string.Empty)
            {
                txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            }
            txtComplTime.Text = string.Empty;

            ddlRoute.Enabled = true;
            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = true;
            txtComplTime.Enabled = false;

            MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = true;
            RequiredFieldValidator27.Enabled = false;
            RequiredFieldValidator28.Enabled = false;

            txtRecommendation.Enabled = false;
            txtWorkCompl.Enabled = false;
            txtWorkCompl.Text = string.Empty;
            lblWCD.Enabled = false;
            btnCodesCmpl.Enabled = false;
            divTransicon.Visible = false;

            txtRT.Enabled = false;
            txtDT.Enabled = false;
            txtOT.Enabled = false;
            txtNT.Enabled = false;
            txtTT.Enabled = false;

            txtRT.Text = "0.00";
            txtDT.Text = "0.00";
            txtOT.Text = "0.00";
            txtNT.Text = "0.00";
            txtTT.Text = "0.00";

            txtExpMisc.Enabled = false;
            txtExpZone.Enabled = false;
            txtExpToll.Enabled = false;

            txtMileEnd.Enabled = false;
            txtMileStart.Enabled = false;

            chkInternet.Enabled = false;
            chkWorkComp.Enabled = false;
            chkReviewed.Enabled = false;
            chkInvoice.Enabled = false;
            txtInvoiceNo.Enabled = false;
            chkChargeable.Enabled = false;
            chkTimeTrans.Enabled = false;
            //ddlService.Enabled = false;
            //ddlPayroll.Enabled = false;
        }
        /********completed***********/
        else if (ddlStatus.SelectedValue == "4")
        {
            //txtComplTime.Text = System.DateTime.Now.ToShortTimeString();
            //txtComplTime.Focus();
            //if (txtOnsitetime.Text == string.Empty)
            //{
            //    txtOnsitetime.Text = System.DateTime.Now.ToShortTimeString();
            //}
            //if (txtEnrTime.Text == string.Empty)
            //{
            //    txtEnrTime.Text = System.DateTime.Now.ToShortTimeString();
            //}

            ddlRoute.Enabled = true;
            txtEnrTime.Enabled = true;
            txtOnsitetime.Enabled = true;
            txtComplTime.Enabled = true;

            MaskedEditValidator3.Enabled = true;
            RequiredFieldValidator24.Enabled = true;
            RequiredFieldValidator25.Enabled = true;
            RequiredFieldValidator26.Enabled = true;
            RequiredFieldValidator29.Enabled = true;
            RequiredFieldValidator27.Enabled = true;
            RequiredFieldValidator28.Enabled = true;


            txtRecommendation.Enabled = true;
            txtWorkCompl.Enabled = true;
            lblWCD.Enabled = true;
            btnCodesCmpl.Enabled = true;
            int Multilang = Convert.ToInt16(Session["IsMultiLang"]);
            if (Multilang != 0)
            {
                divTransicon.Visible = true;
            }

            txtRT.Enabled = true;
            txtDT.Enabled = true;
            txtOT.Enabled = true;
            txtNT.Enabled = true;
            txtTT.Enabled = true;

            txtExpMisc.Enabled = true;
            txtExpZone.Enabled = true;
            txtExpToll.Enabled = true;

            txtMileEnd.Enabled = true;
            txtMileStart.Enabled = true;

            chkInternet.Enabled = true;
            chkWorkComp.Enabled = true;
            chkReviewed.Enabled = true;
            chkInvoice.Enabled = true;
            txtInvoiceNo.Enabled = true;
            chkChargeable.Enabled = true;
            chkTimeTrans.Enabled = true;
            ////ddlService.Enabled = true;
            ////ddlPayroll.Enabled = true;
            //FillChargeableFromCategory();
            //if (ViewState["internetdefault"].ToString() == "1" && chkReviewed.Checked == false && ddlStatus.SelectedValue == "4")
            //    chkInternet.Checked = true;
        }
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {/*******Integrate*****/
        if (ViewState["tsint"].ToString() != "1")
        {
            if (ddlStatus.SelectedValue == "4" && Session["MSM"].ToString() == "TS")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCompleteDiable", "noty({text: 'Ticket can be completed only from Total Service.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
                ddlStatus.SelectedValue = "0";
                ChangeStatus();
                return;
            }
        }

        if (hdnProspect.Value != string.Empty && ddlStatus.SelectedValue == "4")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCompleteDisable", "noty({text: 'Ticket can be completed only from Customers.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            ddlStatus.SelectedValue = "1";
            ChangeStatus();
            return;
        }

        ChangeStatus();
        ShowQBSyncControls();
        FillChargeableFromCategory();
        /********set focus***********/
        hdnFocus.Value = ddlStatus.ClientID;
    }

    protected void btnSelectLoc_Click(object sender, EventArgs e)
    {
        FillLocInfo();
        FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
        hdnFocus.Value = txtAddress.ClientID;
        selectEquip();
    }

    private void selectEquip()
    {
        foreach (GridViewRow gr in gvEquip.Rows)
        {
            Label lblID = (Label)gr.FindControl("lblID");
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            if (lblID.Text == hdnUnitID.Value)
            {
                chkSelect.Checked = true;
            }
        }
    }

    protected void lnkPrint_Click(object sender, EventArgs e)
    {
        string stc = string.Empty;
        bool doRedirect = false;
        int TicketID = 0;

        if (ViewState["readonly"] != null)
        {
            if (Request.QueryString["comp"] != null)
            {
                stc = Request.QueryString["comp"].ToString();
            }
            doRedirect = true;
            TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        }
        else
        {
            lnkSave_Click(sender, e);

            if (success == true)
            {
                if (Request.QueryString["comp"] != null)
                {
                    stc = Request.QueryString["comp"].ToString();
                }

                if (ViewState["assign"].ToString() == "4")
                {
                    stc = "1";
                }
                else
                {
                    stc = "0";
                }
                doRedirect = true;
                TicketID = objMapData.TicketID;
            }
        }

        if (doRedirect)
        {
            //if (Request.QueryString["pop"] == null)
            Response.Redirect("Printticket.aspx?id=" + TicketID + "&c=" + stc);
            //else
            //    Response.Redirect("Printticket.aspx?id=" + TicketID + "&c=" + stc + "&pop=1");
        }
    }

    protected void btnEnroute_Click(object sender, EventArgs e)
    {
        ddlStatus.SelectedValue = "2";
        ChangeStatus();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ERPress", " calculate_Time();", true);
        hdnFocus.Value = txtEnrTime.ClientID;

    }
    protected void btnOnsite_Click(object sender, EventArgs e)
    {
        ddlStatus.SelectedValue = "3";
        ChangeStatus();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ERPress", " calculate_Time();", true);
        hdnFocus.Value = txtOnsitetime.ClientID;
    }
    protected void btnComplete_Click(object sender, EventArgs e)
    {
        if (hdnProspect.Value != string.Empty)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "keyCompleteDisable", "noty({text: 'Ticket can be completed only from Customers.',  type : 'information', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : false});", true);
            return;
        }

        ddlStatus.SelectedValue = "4";
        ChangeStatus();
        FillChargeableFromCategory();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "ERPress", " calculate_Time();", true);
        hdnFocus.Value = txtComplTime.ClientID;
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        lnkSave_Click(sender, e);
        string stc = "";
        stc = Request.QueryString["comp"].ToString();

        if (ddlStatus.SelectedValue == "4")
        {
            stc = "1";
        }
        Response.Redirect("PrintTicketEIR.aspx?id=" + objMapData.TicketID + "&c=" + stc);
    }

    protected void ddlCodeCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        objGeneral.ConnConfig = Session["config"].ToString();
        objGeneral.CodeCategory = ddlCodeCat.SelectedValue;

        if (ViewState["codes"].ToString() == "0")
        {
            objGeneral.CodeType = 0;
        }
        else
        {
            objGeneral.CodeType = 1;
        }

        DataSet ds = new DataSet();
        ds = objBL_General.getDiagnostic(objGeneral);
        chklstCodes.DataSource = ds.Tables[0];
        chklstCodes.DataTextField = "fdesc";
        chklstCodes.DataValueField = "fdesc";
        chklstCodes.DataBind();

        //ddlCodeCat.Focus();
    }

    protected void btnDone_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        if (ViewState["codes"].ToString() == "0")
        {
            str = txtReason.Text;
        }
        else
        {
            str = txtWorkCompl.Text;
        }

        for (int item = 0; item < chklstCodes.Items.Count; item++)
        {
            if (chklstCodes.Items[item].Selected == true)
            {
                //if (item == 0)
                //{
                if (str != string.Empty)
                {
                    str += ", ";
                }
                //}
                //if (item != chklstCodes.Items.Count - 1)
                //{
                //    str += chklstCodes.Items[item].Text ;
                //}
                //else
                //{
                str += chklstCodes.Items[item].Text;
                //}
            }
        }
        if (ViewState["codes"].ToString() == "0")
        {
            txtReason.Text = str;
            //txtReason.Focus();
            hdnFocus.Value = txtReason.ClientID;

        }
        else
        {
            txtWorkCompl.Text = str;
            //txtWorkCompl.Focus();
            hdnFocus.Value = txtWorkCompl.ClientID;
        }
        pnlCodes.Visible = false;
        chklstCodes.SelectedIndex = -1;

    }
    protected void btnCodes_Click(object sender, EventArgs e)
    {
        ViewState["codes"] = "0";
        pnlCodes.Visible = true;
        ddlCodeCat.SelectedIndex = 0;
        lblCodeHeader.Text = "Call Codes";
        ddlCodeCat_SelectedIndexChanged(sender, e);
        //ddlCodeCat.Focus();
        hdnFocus.Value = ddlCodeCat.ClientID;
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlCodes.Visible = false;
        chklstCodes.SelectedIndex = -1;
        if (ViewState["codes"].ToString() == "0")
        {
            //txtReason.Focus();
            hdnFocus.Value = txtReason.ClientID;
        }
        else
        {
            //txtWorkCompl.Focus();
            hdnFocus.Value = txtWorkCompl.ClientID;
        }
    }
    protected void btnCodesCmpl_Click(object sender, EventArgs e)
    {
        ViewState["codes"] = "1";
        pnlCodes.Visible = true;
        ddlCodeCat.SelectedIndex = 0;
        lblCodeHeader.Text = "Resolution Codes";
        ddlCodeCat_SelectedIndexChanged(sender, e);
        hdnFocus.Value = ddlCodeCat.ClientID;
    }
    protected void btnMail_Click(object sender, EventArgs e)
    {
        string stc = "";
        if (Request.QueryString["comp"] != null)
        {
            stc = Request.QueryString["comp"].ToString();
        }

        if (ViewState["assign"].ToString() == "4")
        {
            stc = "1";
        }
        else
        {
            stc = "0";
        }

        //if (Request.QueryString["pop"] == null)
        Response.Redirect("mailticket.aspx?id=" + ViewState["ticid"].ToString() + "&c=" + stc);
        //else
        //    Response.Redirect("mailticket.aspx?id=" + ViewState["ticid"].ToString() + "&c=" + stc + "pop=1");
    }

    /// <summary>
    /// Get access token from Bing translate API
    /// </summary>
    /// <returns></returns>
    public string GetAccessToken()
    {
        AdmAccessToken admToken;
        string headerValue;
        //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
        AdmAuthentication admAuth = new AdmAuthentication("MobileServiceManager", "fXzR98jYu/w2rXPvw8WTU3Zb2V7/WQkSiCS7Z1tSA6I=");

        admToken = admAuth.GetAccessToken();
        // Create a header with the access_token property of the returned token
        headerValue = "Bearer" + " " + HttpUtility.UrlEncode(admToken.access_token);

        return admToken.access_token;
    }

    /// <summary>
    /// Translation method to tranlate language from BING API
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="sourceText"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    private string TranslateMethod(string authToken, string sourceText, string from, string to)
    {
        // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
        BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();
        //Set Authorization header before sending the request
        HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
        httpRequestProperty.Method = "POST";
        httpRequestProperty.Headers.Add("Authorization", authToken);

        // Creates a block within which an OperationContext object is in scope.
        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            // string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";

            string translationResult;
            //Keep appId parameter blank as we are sending access token in authorization header.
            translationResult = client.Translate("Bearer" + " " + authToken, sourceText, from, to, "text/html", "general");
            return translationResult;
        }
    }

    /// <summary>
    /// Detects the language from BING API
    /// </summary>
    /// <param name="authToken"></param>
    /// <param name="sourceText"></param>
    /// <returns></returns>
    private string DetectLanguageMethod(string authToken, string sourceText)
    {
        // Add TranslatorService as a service reference, Address:http://api.microsofttranslator.com/V2/Soap.svc
        BingTranslatorService.LanguageServiceClient client = new BingTranslatorService.LanguageServiceClient();
        //Set Authorization header before sending the request
        HttpRequestMessageProperty httpRequestProperty = new HttpRequestMessageProperty();
        httpRequestProperty.Method = "POST";
        httpRequestProperty.Headers.Add("Authorization", authToken);

        // Creates a block within which an OperationContext object is in scope.
        using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
        {
            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;
            // string sourceText = "<UL><LI>Use generic class names. <LI>Use pixels to express measurements for padding and margins. <LI>Use percentages to specify font size and line height. <LI>Use either percentages or pixels to specify table and container width.   <LI>When selecting font families, choose browser-independent alternatives.   </LI></UL>";

            string translationResult;
            //Keep appId parameter blank as we are sending access token in authorization header.
            translationResult = client.Detect("Bearer" + " " + authToken, sourceText);
            return translationResult;
        }
    }

    protected void lnkTranslate_Click(object sender, EventArgs e)
    {
        if (txtReason.Text.Trim() == string.Empty)
        {
            pnlTranslate.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            string sourceLang = DetectLanguageMethod(GetAccessToken(), txtReason.Text);
            string reason = txtReason.Text;

            if (sourceLang == "es")
            {
                txtReason.Text = TranslateMethod(GetAccessToken(), txtReason.Text, sourceLang, "en");
                txtTranslate.Text = reason;
            }
            else if (sourceLang == "en")
            {
                txtTranslate.Text = TranslateMethod(GetAccessToken(), txtReason.Text, sourceLang, "es");
            }

            hdnIsEdited.Value = "0";
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTranslate.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransEnglish_Click(object sender, EventArgs e)
    {
        if (txtWorkCompl.Text.Trim() == string.Empty)
        {
            pnlTransDesc.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            string sourceLang = DetectLanguageMethod(GetAccessToken(), txtWorkCompl.Text);
            string workCompl = txtWorkCompl.Text;

            if (sourceLang == "es")
            {
                txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtWorkCompl.Text, sourceLang, "en");
                txtTransDesc.Text = workCompl;
            }
            else if (sourceLang == "en")
            {
                txtTransDesc.Text = TranslateMethod(GetAccessToken(), txtWorkCompl.Text, sourceLang, "es");
            }
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        //finally
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keytxtscript", "$('#" + txtWorkCompl.ClientID + "').keyup(function(event) {replaceQuickCodes(event, '" + txtWorkCompl.ClientID + "', $('#" + hdnCon.ClientID + "').val());}); $('#" + txtWorkCompl.ClientID + "').focus(function() {$(this).animate({width: '520px', height: '75px'}, 500, function() {});});            $('#" + txtWorkCompl.ClientID + "').blur(function() {$(this).animate({width: '188px', height: '63px'}, 500, function() {});});", true);

        //}
        pnlTransDesc.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransReasonToEnglish_Click(object sender, EventArgs e)
    {
        if (txtTranslate.Text.Trim() == string.Empty)
        {
            pnlTranslate.Attributes.Add("style", " display: block;");
            return;
        }

        try
        {
            txtReason.Text = TranslateMethod(GetAccessToken(), txtTranslate.Text, "es", "en");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr2", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTranslate.Attributes.Add("style", " display: block;");
    }

    protected void lnkTransDescToEnglish_Click(object sender, EventArgs e)
    {
        if (txtTransDesc.Text.Trim() == string.Empty)
        {
            pnlTransDesc.Attributes.Add("style", "display: block;");
            return;
        }

        try
        {
            txtWorkCompl.Text = TranslateMethod(GetAccessToken(), txtTransDesc.Text, "es", "en");
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "keyTransErr1", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
        pnlTransDesc.Attributes.Add("style", "display: block;");
    }

    private void ChangeRoute()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = ddlRoute.SelectedValue;

        string Lang = objBL_User.getUserLangByID(objPropUser);
        if (!string.IsNullOrEmpty(Lang))
        {
            if (Lang != "none")
            {
                hdnLang.Value = Lang;
            }
            else
            {
                hdnLang.Value = "english";
            }
        }
        else
        {
            hdnLang.Value = "english";
        }
    }

    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.Username = ddlRoute.SelectedValue;
        int status = objBL_User.getEMPStatus(objPropUser);
        if (status == 1)
            lblWorkStatus.Text = "Inactive";
        else
            lblWorkStatus.Text = "";
        ChangeRoute();
        hdnFocus.Value = ddlRoute.ClientID;
    }

    protected void lnkUploadDoc_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
            try
            {
                /****** Get path from web.config***********/
                string savepathconfig = System.Web.Configuration.WebConfigurationManager.AppSettings["AttachmentPath"].Trim();
                string filename = "";
                string savepath = savepathconfig + @"\" + Session["dbname"] + @"\";
                string fullpath = "";

                /****** When mode 0 (add new ticket) creates a temp id instead if the ticket id. creates directory with temp id name. when mode is 1 (update record) it creates directory with ticketID.
                 Ihe temp concept is just for uploading the doc before the ticket is saved as ticket id is not created yet.
                 If ticket is not created and documents are uploaded then they are automaticklly deleted next day on Tickelistview.aspx load event.
                 In documents table the screen field is set 'temp' in case of mode=0. In case of mode =1 screen is set to 'ticket'.**********/
                if (Convert.ToInt32(ViewState["mode"]) == 0)
                {
                    savepath += hdnFormId.Value + @"\";
                    //filename = hdnFormId.Value + "_" + FileUpload1.FileName;
                    filename = FileUpload1.FileName;
                    objMapData.Screen = "Temp";
                    objMapData.TicketID = 0;
                    objMapData.TempId = hdnFormId.Value;
                }
                else
                {
                    savepath += Request.QueryString["id"].ToString() + @"\";
                    //filename = Convert.ToInt32(Request.QueryString["id"].ToString()) + "_" + FileUpload1.FileName;
                    filename = FileUpload1.FileName;
                    objMapData.Screen = "Ticket";
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                    objMapData.TempId = "0";
                }

                fullpath = savepath + filename;
                if (File.Exists(fullpath))
                {
                    filename = objGeneralFunctions.generateRandomString(4) + "_" + filename;
                    fullpath = savepath + filename;
                }

                objMapData.FileName = filename;
                objMapData.DocTypeMIME = System.IO.Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);
                objMapData.FilePath = fullpath;
                objMapData.ConnConfig = Session["config"].ToString();

                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }

                FileUpload1.SaveAs(fullpath);
                objMapData.Mode = 0;
                objMapData.DocID = 0;
                objBL_MapData.AddFile(objMapData);
                GetDocuments();
            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

                ClientScript.RegisterStartupScript(Page.GetType(), "keyUploadErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
            }
    }

    private void GetDocuments()
    {
        if (Convert.ToInt32(ViewState["mode"]) == 0)
        {
            objMapData.Screen = "Temp";
            objMapData.TempId = hdnFormId.Value;
            objMapData.TicketID = 0;
        }
        else
        {
            objMapData.Screen = "Ticket";
            objMapData.TempId = "0";
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
        }

        objMapData.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_MapData.GetDocuments(objMapData);
        gvDocuments.DataSource = ds.Tables[0];
        gvDocuments.DataBind();
    }

    /// <summary>
    /// Update document data when new ticket is saved and docs are already uploaded. hdnFormID stores the temp id. 
    /// it actually updates the screen and screenid fields. updated 'temp' with 'ticket' in screen document.screen field. updates document.screenid field with 'null' to ticketID created.
    /// </summary>
    private void UpdateDoc()
    {
        objMapData.Screen = "Ticket";
        objMapData.TicketID = Convert.ToInt32(ViewState["ticid"]);
        objMapData.TempId = hdnFormId.Value;
        objMapData.ConnConfig = Session["config"].ToString();
        objBL_MapData.UpdateFile(objMapData);
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        HttpPostedFile file;
        Session["files"] = Request.Files;
        foreach (string key in Request.Files.Keys)
        {
            file = Request.Files[key];

            if (file != null && file.ContentLength > 0)
            {
                file.SaveAs("C:\\UploadedUserFiles\\" + file.FileName);
            }
        }
    }

    private void DownloadDocument(string filePath, string DownloadFileName)
    {
        try
        {
            System.IO.FileInfo FileName = new System.IO.FileInfo(filePath);
            FileStream myFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader _BinaryReader = new BinaryReader(myFile);

            try
            {
                long startBytes = 0;
                string lastUpdateTiemStamp = File.GetLastWriteTimeUtc(filePath).ToString("r");
                string _EncodedData = HttpUtility.UrlEncode(DownloadFileName, Encoding.UTF8) + lastUpdateTiemStamp;

                Response.Clear();
                Response.Buffer = false;
                Response.AddHeader("Accept-Ranges", "bytes");
                Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(DownloadFileName));
                Response.AddHeader("Content-Length", (FileName.Length - startBytes).ToString());
                Response.AddHeader("Connection", "Keep-Alive");
                Response.ContentEncoding = Encoding.UTF8;

                //Send data
                _BinaryReader.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //Dividing the data in 1024 bytes package
                int maxCount = (int)Math.Ceiling((FileName.Length - startBytes + 0.0) / 1024);

                //Download in block of 1024 bytes
                int i;
                for (i = 0; i < maxCount && Response.IsClientConnected; i++)
                {
                    Response.BinaryWrite(_BinaryReader.ReadBytes(1024));
                    Response.Flush();
                }
                ////if blocks transfered not equals total number of blocks
                //if (i < maxCount)
                //    return false;
                //return true; 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Response.End();
                _BinaryReader.Close();
                myFile.Close();
            }
        }
        catch (FileNotFoundException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('File not found.');", true);
        }
        catch (UnauthorizedAccessException ex)
        {
            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileaccessWarning", "alert('Please provide access permissions to the file path.');", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(),
            "FileerrorWarning", "alert('" + str + "');", true);
        }
    }

    protected void lblName_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;

        string[] CommandArgument = btn.CommandArgument.Split(',');

        string FileName = CommandArgument[0];

        string FilePath = CommandArgument[1];

        DownloadDocument(FilePath, FileName);
    }

    public void DeleteFileFromFolder(string StrFilename, int DocumentID)
    {
        try
        {
            //File.Delete(StrFilename);
            DeleteFile(DocumentID);
        }
        catch (FileNotFoundException ex)
        {
            DeleteFile(DocumentID);
        }
        catch (UnauthorizedAccessException ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteAccessWarning", "noty({text: 'Please provide delete permissions to the file path.',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);

            ScriptManager.RegisterStartupScript(this, GetType(),
            "FileDeleteErrorWarning", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteDoc_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow di in gvDocuments.Rows)
        {
            CheckBox chkSelected = (CheckBox)di.FindControl("chkSelect");
            Label lblID = (Label)di.FindControl("lblId");
            Label lblPath = (Label)di.FindControl("lblPath");

            if (chkSelected.Checked == true)
            {
                DeleteFileFromFolder(lblPath.Text, Convert.ToInt32(lblID.Text));
            }
        }
    }

    private void DeleteFile(int DocumentID)
    {
        try
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.DocumentID = DocumentID;
            objBL_MapData.DeleteFile(objMapData);
            GetDocuments();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErrdelete", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : 3000,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    private void FillRelatedTickets()
    {
        if (txtWO.Text.Trim() != string.Empty)
        {
            if (!txtWO.Text.Trim().Equals(hdnWO.Value, StringComparison.CurrentCultureIgnoreCase))
            {
                DataSet ds = new DataSet();
                objMapData.ConnConfig = Session["config"].ToString();
                objMapData.Workorder = txtWO.Text.Trim();

                if (Request.QueryString["id"] != null && Request.QueryString["copy"] == null)
                    objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"].ToString());
                else
                    objMapData.TicketID = 0;

                ds = objBL_MapData.GetTicketsByWorkorder(objMapData);

                lstRelatedTickets.DataSource = ds.Tables[0];
                lstRelatedTickets.DataBind();

                hdnWO.Value = txtWO.Text.Trim();
            }
        }

    }

    /// <summary>
    /// Get Signature image.
    /// </summary>
    /// <param name="ticketid"></param>
    /// <param name="workerid"></param>
    /// <returns></returns>
    public string GetTicketSignature(string ticketid, string workerid)
    {
        DataSet ds = new DataSet();
        string image = string.Empty;
        if (workerid.Trim() != string.Empty)
        {
            objMapData.ConnConfig = Session["config"].ToString();
            objMapData.TicketID = Convert.ToInt32(ticketid);
            objMapData.WorkID = Convert.ToInt32(workerid);
            ds = objBL_MapData.GetTicketSignature(objMapData);

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["signature"] != DBNull.Value)
                {
                    image = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["signature"]);
                }
            }
        }
        return image;
    }


    private void fillREPHistory()
    {
        objPropUser.ConnConfig = Session["config"].ToString();
        objPropUser.EquipID = 0;
        objPropUser.SearchBy = "rd.ticketID";
        objPropUser.SearchValue = Request.QueryString["id"].ToString();

        DataSet ds = new DataSet();
        ds = objBL_User.getequipREPDetails(objPropUser);

        gvRepDetails.DataSource = ds.Tables[0];
        gvRepDetails.DataBind();
    }

    private void GetBillcodesforTimeSheet()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetBillcodesforTimeSheet(objProp_Contracts);

        ddlService.DataSource = ds.Tables[0];
        ddlService.DataTextField = "billcode";
        ddlService.DataValueField = "QBinvid";
        ddlService.DataBind();

        ddlService.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void GetPayrollforTimeSheet()
    {
        BL_Contracts objBL_Contracts = new BL_Contracts();
        Contracts objProp_Contracts = new Contracts();

        objProp_Contracts.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Contracts.GetPayrollforTimeSheet(objProp_Contracts);

        ddlPayroll.DataSource = ds.Tables[0];
        ddlPayroll.DataTextField = "fdesc";
        ddlPayroll.DataValueField = "QBwageid";
        ddlPayroll.DataBind();

        ddlPayroll.Items.Insert(0, new ListItem(":: Select ::", ""));
    }

    private void GetQBInt()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getControl(objPropUser);

        ViewState["qbint"] = "0";
        ViewState["tsint"] = "0";
        ViewState["internetdefault"] = "0";
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["QBIntegration"].ToString() == "1")
                ViewState["qbint"] = "1";
            if (ds.Tables[0].Rows[0]["TsIntegration"].ToString() == "1")
                ViewState["tsint"] = "1";
            if (ds.Tables[0].Rows[0]["tinternett"].ToString() == "1")
                ViewState["internetdefault"] = "1";
        }
    }

    private void ShowQBSyncControls()
    {
        if (ViewState["qbint"].ToString() == "1")//&& ddlStatus.SelectedValue=="4"
        {
            chkTimeTrans.Enabled = true;
            ddlService.Enabled = true;
            ddlPayroll.Enabled = true;
        }
        else
        {
            chkTimeTrans.Enabled = false;
            ddlService.Enabled = false;
            ddlPayroll.Enabled = false;
        }
    }

    protected void ddlService_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlService.SelectedValue != string.Empty)
        {
            BL_Contracts objBL_Contracts = new BL_Contracts();
            Contracts objProp_Contracts = new Contracts();

            objProp_Contracts.ConnConfig = Session["config"].ToString();
            objProp_Contracts.QBInvID = ddlService.SelectedValue;
            DataSet ds = new DataSet();
            ds = objBL_Contracts.GetPayrollByAccount(objProp_Contracts);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlPayroll.SelectedValue = ds.Tables[0].Rows[0]["QBPayrollItem"].ToString();
            }
            else
            {
                ddlPayroll.SelectedValue = string.Empty;
            }
        }
    }
    protected void lblRelatedTickets_Click(object sender, ImageClickEventArgs e)
    {
        divRelated.Visible = true;
        FillRelatedTickets();
    }
    protected void lnkCloseRelated_Click(object sender, EventArgs e)
    {
        divRelated.Visible = false;
    }
    protected void txtCustomer_TextChanged(object sender, EventArgs e)
    {
        //SetProspect();
    }
    private void SetProspect()
    {
        if (hdnProspect.Value != "1")
        {
            RequiredFieldValidator1.Enabled = true;
            //txtLocation.Enabled = true;
            //txtLocation.Text = "";
            //hdnProspect.Value = "";
            //txtCustomer.ForeColor = System.Drawing.ColorTranslator.FromHtml("#686767");
        }
        else
        {
            RequiredFieldValidator1.Enabled = false;
            //hdnFocus.Value = txtAddress.ClientID;
            //txtLocation.Enabled = false;
            //txtLocation.Text = "";
            //hdnProspect.Value = "1";
            //txtCustomer.ForeColor = System.Drawing.Color.Brown;
        }
    }
    protected void lnkConvert_Click(object sender, EventArgs e)
    {
        pnlCustomer.Visible = true;
        uc_CustomerSearch1._txtCustomer.Focus();
        lnkConvert.Visible = false;
        lnkPrint.Visible = false;
        ViewState["convert"] = "1";
    }

    private void ConvertProspectWizard()
    {
        string ProspectID = hdnLocId.Value;
        string ticketid = ViewState["ticid"].ToString();
        string comp = "0";
        if (Request.QueryString["comp"] == null)
        {
            if (ViewState["assign"] != null)
            {
                if (ViewState["assign"].ToString() == "4")
                    comp = "1";
            }
        }
        else
        {
            comp = Request.QueryString["comp"].ToString();
        }

        if (ViewState["convert"].ToString() == "1")
        {
            if (uc_CustomerSearch1._hdnCustID.Value == string.Empty)
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addcustomer.aspx?cpw=1&prospectid=" + ProspectID + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
            else
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "key1", "alert('Continue to Convert Lead Wizard.'); window.location.href='addlocation.aspx?cpw=1&prospectid=" + ProspectID + "&customerid=" + uc_CustomerSearch1._hdnCustID.Value + "&Ticketid=" + ticketid + "&comp=" + comp + "';", true);
        }
    }

    private void GetJobCode()
    {
        if (hdnProjectId.Value != string.Empty)
        {
            Customer objProp_Customer = new Customer();
            BL_Customer objBL_Customer = new BL_Customer();
            objProp_Customer.ConnConfig = Session["config"].ToString();
            objProp_Customer.ProjectJobID = Convert.ToInt32(hdnProjectId.Value);
            objProp_Customer.Type = "1";
            DataSet ds = objBL_Customer.getJobProjectByJobID(objProp_Customer);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtProject.Text = ds.Tables[0].Rows[0]["ID"].ToString() + "-" + ds.Tables[0].Rows[0]["fdesc"].ToString();
                ////hdnProjectId.Value = ds.Tables[0].Rows[0]["ID"].ToString();
                //if (ds.Tables[1].Rows.Count > 0)
                //{
                gvProjectCode.DataSource = ds.Tables[1];
                gvProjectCode.DataBind();
                txtJobCode.Text = string.Empty;
                hdnProjectCode.Value = string.Empty;
                //}
                ddlDepartment.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
            }
        }
    }
    protected void btnGetCode_Click(object sender, EventArgs e)
    {
        GetJobCode();
    }

    protected void btnEquip_Click(object sender, EventArgs e)
    {
        FillRecentCalls(Convert.ToInt32(hdnLocId.Value));
    }

    private void FillProjectsTemplate()
    {
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getJobProjectTemplate(objCustomer);
        ddlTemplate.DataSource = ds.Tables[0];
        ddlTemplate.DataTextField = "Fdesc";
        ddlTemplate.DataValueField = "id";
        ddlTemplate.DataBind();

        ddlTemplate.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillWage()
    {
        DataSet ds = new DataSet();
        objCustomer.ConnConfig = Session["config"].ToString();
        ds = objBL_Customer.getWage(objCustomer);
        ddlWage.DataSource = ds.Tables[0];
        ddlWage.DataTextField = "Fdesc";
        ddlWage.DataValueField = "id";
        ddlWage.DataBind();

        ddlWage.Items.Insert(0, new ListItem(":: Select ::", "0"));
    }

    private void FillRecentCalls(int loc)
    {
        DataSet ds = new DataSet();
        objMapData.ConnConfig = Session["config"].ToString();
        objMapData.LocID = loc;
        if (Request.QueryString["id"] != null)
            objMapData.TicketID = Convert.ToInt32(Request.QueryString["id"]);
        ds = objBL_MapData.GetRecentCallsLoc(objMapData);
        lstRecentCalls.DataSource = ds.Tables[0];
        lstRecentCalls.DataBind();
    }

    public string RecentCallsDetails(string assigned, string worker, string cat, string elev)
    {
        string str = string.Empty;
        if (assigned.ToLower() == "assigned")
            str = assigned + " to <strong>" + worker + "</strong>";
        else
            str = assigned + " by <strong>" + worker + "</strong>";

        if (cat != string.Empty)
            str += "<BR/>Category - " + cat;
        if (elev != string.Empty)
            str += "<BR/>Equipment - " + elev;

        return str;
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillChargeableFromCategory();
    }

    private void FillChargeableFromCategory()
    {
        if (hdnReviewed.Value != "")
        {
            if (Convert.ToInt16(hdnReviewed.Value) == 0 && ddlStatus.SelectedValue == "4")
            {
                objPropUser.ConnConfig = Session["config"].ToString();
                objPropUser.Cat = ddlCategory.SelectedValue;
                DataSet ds = objBL_User.getcategoryAll(objPropUser);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    bool charge = Convert.ToBoolean(ds.Tables[0].Rows[0]["chargeable"]);
                    chkChargeable.Checked = charge;
                }
                else
                {
                    chkChargeable.Checked = false;
                }
            }
        }
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ticketids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
        int index = dt.Rows.IndexOf(d);
        int c = dt.Rows.Count - 1;
        if (index < c)
        {
            Response.Redirect("addticket.aspx?comp=" + dt.Rows[index + 1]["comp"] + "&id=" + dt.Rows[index + 1]["id"]);
        }
    }
    protected void lnkPrevious_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ticketids"];
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Columns["ID"];
        dt.PrimaryKey = keyColumns;

        DataRow d = dt.Rows.Find(Request.QueryString["id"].ToString());
        int index = dt.Rows.IndexOf(d);

        if (index > 0)
        {
            Response.Redirect("addticket.aspx?comp=" + dt.Rows[index - 1]["comp"] + "&id=" + dt.Rows[index - 1]["id"]);
        }
    }
    protected void lnkLast_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ticketids"];
        Response.Redirect("addticket.aspx?comp=" + dt.Rows[dt.Rows.Count - 1]["comp"] + "&id=" + dt.Rows[dt.Rows.Count - 1]["id"]);
    }
    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        DataTable dt = (DataTable)Session["ticketids"];
        Response.Redirect("addticket.aspx?comp=" + dt.Rows[0]["comp"] + "&id=" + dt.Rows[0]["id"]);
    }

}




