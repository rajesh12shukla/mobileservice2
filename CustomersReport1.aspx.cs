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
using BusinessEntity;
using BusinessLayer;
using System.Collections.Generic;
//using CrystalDecisions.CrystalReports.Engine;
using System.Web.Script.Serialization;
//using CrystalDecisions.Shared;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Net.Configuration;


public partial class Default2 : System.Web.UI.Page
{
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    CustomerReport objCustReport = new CustomerReport();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();
    public static int pubReportId = 0;
    public static string sortBy = string.Empty;
    // public static string getQuery = string.Empty;
    public static string getPrintData = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                DeleteExcelFiles();
                DeletePDFFiles();
            }
            catch
            {
                //
            }
            dvSaveReport.Attributes.Add("style", "display:none");
            if (Session["userid"] == null)
            {
                Response.Redirect("login.aspx");
            }

            if (Request.QueryString["reportId"] != null || Convert.ToInt32(Request.QueryString["reportId"]) != 0)
            {
                objCustReport.ReportId = Convert.ToInt32(Request.QueryString["reportId"]);
            }
            else
            {
                Response.Redirect("customers.aspx", false);
                return;
            }
            if (Request.QueryString["reportName"] != null)
            {
                objCustReport.ReportName = Request.QueryString["reportName"];
                hdnCustomizeReportName.Value = objCustReport.ReportName;
            }
            pubReportId = objCustReport.ReportId;

            sortBy = string.Empty;
            GetCustomerDetails();
            GetReportsName();
            if (pubReportId != 0)
            {
                // dvGridReport.Attributes.Add("style", "display:block");
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }
            else
            {
                GetGroupedCustomersLocation();
                //grdCustomerReportData.DataSource = null;
                // dvGridReport.Attributes.Add("style", "display:none");
            }

            GetCustReportFiltersValue();
            // GetCustomerName();
            //  GetCustomerCity();
            // GetCustomerAddress();
            // GetCustomerType();
            ConvertToJSON();
            GetUserEmail();
        }
        else
        {

            //ReportDocument doc = (ReportDocument)Session["ReportDocument"];
            //CrystalReportViewer1.ReportSource = doc;

            ////GetReportsName();
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void GetUserEmail()
    {
        objProp_User.ConnConfig = Session["config"].ToString();
        objProp_User.Username = Session["username"].ToString();
        txtFrom.Text = objBL_User.getUserEmail(objProp_User);

        DataSet dsC = new DataSet();

        dsC = objBL_User.getControl(objProp_User);

        if (dsC.Tables[0].Rows.Count > 0)
        {
            if (txtFrom.Text.Trim() == string.Empty)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                }
            }
        }

        if (txtFrom.Text.Trim() == string.Empty)
        {
            System.Configuration.Configuration configurationFile = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
            string username = mailSettings.Smtp.Network.UserName;
            txtFrom.Text = username;
        }
    }

    private void GetReportDetailByRptId()
    {
        try
        {
            DataSet dsGetRptDetails = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                dsGetRptDetails = objBL_ReportsData.GetReportDetailById(objCustReport);
                if (dsGetRptDetails.Tables.Count > 0)
                {
                    bool isGlobal = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsGlobal"]);
                    bool isAscending = Convert.ToBoolean(dsGetRptDetails.Tables[0].Rows[0]["IsAscendingOrder"]);

                    if (isGlobal)
                    {
                        chkIsGlobal.Checked = true;
                    }
                    else
                    {
                        chkIsGlobal.Checked = false;
                    }

                    if (isAscending)
                    {
                        rdbOrders.SelectedValue = "1";
                    }
                    else
                    {
                        rdbOrders.SelectedValue = "2";
                    }

                    hdnDrpSortBy.Value = dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString();
                    sortBy = dsGetRptDetails.Tables[0].Rows[0]["SortBy"].ToString() + " " + (isAscending == true ? "Asc" : "Desc");
                }
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerType()
    {
        try
        {
            DataSet dsGetCustType = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustType = objBL_ReportsData.GetCustomerType(objCustReport);
            if (dsGetCustType.Tables[0].Rows.Count > 0)
            {
                drpType.DataSource = dsGetCustType.Tables[0];
                drpType.DataTextField = "Type";
                drpType.DataValueField = "Type";
                drpType.DataBind();

                // drpType.Items.Insert(0, "All");
                // drpType.SelectedIndex = 0;
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustReportFiltersValue()
    {
        DataSet dsGetCustReportFiltersValue = new DataSet();
        objProp_User.DBName = Session["dbname"].ToString();
        objProp_User.ConnConfig = Session["config"].ToString();
        dsGetCustReportFiltersValue = objBL_ReportsData.GetCustReportFiltersValue(objProp_User);

        if (dsGetCustReportFiltersValue.Tables[0].Rows.Count > 0)
        {
            drpName.DataSource = dsGetCustReportFiltersValue.Tables[0];
            drpName.DataTextField = "Name";
            drpName.DataValueField = "Name";
            drpName.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[1].Rows.Count > 0)
        {
            drpAddress.DataSource = dsGetCustReportFiltersValue.Tables[1];
            drpAddress.DataTextField = "Address";
            drpAddress.DataValueField = "Address";
            drpAddress.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[2].Rows.Count > 0)
        {
            drpCity.DataSource = dsGetCustReportFiltersValue.Tables[2];
            drpCity.DataTextField = "City";
            drpCity.DataValueField = "City";
            drpCity.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[3].Rows.Count > 0)
        {
            drpLocationId.DataSource = dsGetCustReportFiltersValue.Tables[3];
            drpLocationId.DataTextField = "LocationId";
            drpLocationId.DataValueField = "LocationId";
            drpLocationId.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[4].Rows.Count > 0)
        {
            drpLocationName.DataSource = dsGetCustReportFiltersValue.Tables[4];
            drpLocationName.DataTextField = "LocationName";
            drpLocationName.DataValueField = "LocationName";
            drpLocationName.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[5].Rows.Count > 0)
        {
            drpLocationAddress.DataSource = dsGetCustReportFiltersValue.Tables[5];
            drpLocationAddress.DataTextField = "LocationAddress";
            drpLocationAddress.DataValueField = "LocationAddress";
            drpLocationAddress.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[6].Rows.Count > 0)
        {
            drpLocationCity.DataSource = dsGetCustReportFiltersValue.Tables[6];
            drpLocationCity.DataTextField = "LocationCity";
            drpLocationCity.DataValueField = "LocationCity";
            drpLocationCity.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[7].Rows.Count > 0)
        {
            drpLocationType.DataSource = dsGetCustReportFiltersValue.Tables[7];
            drpLocationType.DataTextField = "Type";
            drpLocationType.DataValueField = "Type";
            drpLocationType.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[8].Rows.Count > 0)
        {
            drpEquipmentName.DataSource = dsGetCustReportFiltersValue.Tables[8];
            drpEquipmentName.DataTextField = "EquipmentName";
            drpEquipmentName.DataValueField = "EquipmentName";
            drpEquipmentName.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[9].Rows.Count > 0)
        {
            drpManuf.DataSource = dsGetCustReportFiltersValue.Tables[9];
            drpManuf.DataTextField = "Manuf";
            drpManuf.DataValueField = "Manuf";
            drpManuf.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[10].Rows.Count > 0)
        {
            drpEquipmentType.DataSource = dsGetCustReportFiltersValue.Tables[10];
            drpEquipmentType.DataTextField = "EquipmentType";
            drpEquipmentType.DataValueField = "EquipmentType";
            drpEquipmentType.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[11].Rows.Count > 0)
        {
            drpServiceType.DataSource = dsGetCustReportFiltersValue.Tables[11];
            drpServiceType.DataTextField = "ServiceType";
            drpServiceType.DataValueField = "ServiceType";
            drpServiceType.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[12].Rows.Count > 0)
        {
            drpType.DataSource = dsGetCustReportFiltersValue.Tables[12];
            drpType.DataTextField = "Type";
            drpType.DataValueField = "Type";
            drpType.DataBind();
        }

        if (dsGetCustReportFiltersValue.Tables[13].Rows.Count > 0)
        {
            drpLocationZip.DataSource = dsGetCustReportFiltersValue.Tables[13];
            drpLocationZip.DataTextField = "LocationZip";
            drpLocationZip.DataValueField = "LocationZip";
            drpLocationZip.DataBind();
        }
    }

    private void GetCustomerName()
    {
        try
        {
            DataSet dsGetCustName = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustName = objBL_ReportsData.GetCustomerName(objCustReport);
            if (dsGetCustName.Tables[0].Rows.Count > 0)
            {
                drpName.DataSource = dsGetCustName.Tables[0];
                drpName.DataTextField = "Name";
                drpName.DataValueField = "Name";
                drpName.DataBind();

                // drpName.Items.Insert(0, "All");
                // drpName.SelectedIndex = 0;
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerAddress()
    {
        try
        {
            DataSet dsGetCustAddress = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustAddress = objBL_ReportsData.GetCustomerAddress(objCustReport);
            if (dsGetCustAddress.Tables[0].Rows.Count > 0)
            {
                drpAddress.DataSource = dsGetCustAddress.Tables[0];
                drpAddress.DataTextField = "Address";
                drpAddress.DataValueField = "Address";
                drpAddress.DataBind();

                // drpName.Items.Insert(0, "All");
                // drpName.SelectedIndex = 0;
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetCustomerCity()
    {
        try
        {
            DataSet dsGetCustCity = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            dsGetCustCity = objBL_ReportsData.GetCustomerCity(objCustReport);
            if (dsGetCustCity.Tables[0].Rows.Count > 0)
            {
                drpCity.DataSource = dsGetCustCity.Tables[0];
                drpCity.DataTextField = "City";
                drpCity.DataValueField = "City";
                drpCity.DataBind();

                // drpName.Items.Insert(0, "All");
                // drpName.SelectedIndex = 0;
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    private void GetReportsName()
    {
        try
        {
            string globalImageURL = "images/Globel_Report.png";
            string privateImageURL = "images/Private_Report.png";

            DataSet dsGetReports = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            objProp_User.UserID = Convert.ToInt32(Session["UserID"].ToString());
            dsGetReports = objBL_ReportsData.GetReports(objProp_User);
            if (dsGetReports.Tables.Count > 0)
            {
                drpReports.DataSource = dsGetReports.Tables[0];
                drpReports.DataTextField = "ReportName";
                drpReports.DataValueField = "Id";
                drpReports.DataBind();
                drpReports.Items.Insert(0, new System.Web.UI.WebControls.ListItem { Text = "Customer Detail", Value = "0" });
                System.Web.UI.WebControls.ListItem itemCD = drpReports.Items[0];
                itemCD.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                drpReports.SelectedValue = pubReportId.ToString();

                if (dsGetReports.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGetReports.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(dsGetReports.Tables[0].Rows[i]["IsGlobal"].ToString()) == true)
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + globalImageURL + ");background-repeat:no-repeat;";
                        }
                        else
                        {
                            System.Web.UI.WebControls.ListItem item = drpReports.Items[i + 1];
                            item.Attributes["style"] = "background: url(" + privateImageURL + ");background-repeat:no-repeat;";
                        }
                    }
                }

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetReportColumnsByRepId()
    {
        try
        {
            DataSet dsGetColumns = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            if (pubReportId != 0)
            {
                dsGetColumns = objBL_ReportsData.GetReportColByRepId(objCustReport);
                string[] checkedColumns = null;
                string[] selectedFiltersColumns = null;
                string[] selectedFiltersValues = null;
                if (dsGetColumns.Tables[0].Rows.Count > 0)
                {
                    checkedColumns = dsGetColumns.Tables[0].AsEnumerable().Select(s => s.Field<string>("ColumnName")).ToArray<string>();
                    hdnColumnList.Value = string.Join(",", checkedColumns);
                }

                DataSet dsSelectedFilters = new DataSet();
                dsSelectedFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
                if (dsSelectedFilters.Tables[0].Rows.Count > 0)
                {
                    selectedFiltersColumns = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterColumn")).ToArray<string>();
                    selectedFiltersValues = dsSelectedFilters.Tables[0].AsEnumerable().Select(s => s.Field<string>("FilterSet")).ToArray<string>();
                }

                if (drpReports.SelectedItem.ToString() != "Resize And Reorder")
                {
                    BindReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                    dvGridReport.Attributes.Add("style", "display:none");
                }
                else
                {
                    BindGridReport(checkedColumns, selectedFiltersColumns, selectedFiltersValues, sortBy);
                    dvGridReport.Attributes.Add("style", "display:block;height:350px;overflow:auto;");
                }
            }
            else
            {
                GetGroupedCustomersLocation();
                dvGridReport.Attributes.Add("style", "display:none");
            }

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private List<CustomerReport> GetReportFilters()
    {
        List<CustomerReport> lstCustomerReport = new List<CustomerReport>();
        try
        {
            DataSet dsGetFilters = new DataSet();
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            dsGetFilters = objBL_ReportsData.GetReportFiltersByRepId(objCustReport);
            for (int i = 0; i <= dsGetFilters.Tables[0].Rows.Count - 1; i++)
            {
                CustomerReport objCustmerReport = new CustomerReport();
                objCustmerReport.FilterColumns = dsGetFilters.Tables[0].Rows[i]["FilterColumn"].ToString();
                objCustmerReport.FilterValues = dsGetFilters.Tables[0].Rows[i]["FilterSet"].ToString();

                lstCustomerReport.Add(objCustmerReport);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        return lstCustomerReport;
    }

    public void ConvertToJSON()
    {
        JavaScriptSerializer jss1 = new JavaScriptSerializer();
        string _myJSONstring = jss1.Serialize(GetReportFilters());
        string filters = "var filters=" + _myJSONstring + ";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "reportsr123", filters, true);
    }


    private void GetCustomerDetails()
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.getCustomerDetails(objProp_User);

            if (dsGetCustDetails.Tables.Count > 0)
            {
                List<string> lstHeaders = new List<string>();
                //string[] columnNames = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                //                        select dc.ColumnName).ToArray();

                lstHeaders = (from dc in dsGetCustDetails.Tables[0].Columns.Cast<DataColumn>()
                              select dc.ColumnName).ToList();

                chkColumnList.DataSource = lstHeaders;
                chkColumnList.DataBind();

                lstFilter.DataSource = lstHeaders;
                lstFilter.DataBind();

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnSaveReport_Click(object sender, EventArgs e)
    {
        try
        {
            //string[] checkedColumns = chkColumnList.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToArray();

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            //  objCustReport.ReportId = Convert.ToInt32(ViewState["ReportId"]);
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            objCustReport.ReportName = txtReportName.Text;
            objCustReport.ReportType = "Customer";
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.IsGlobal = chkIsGlobal.Checked ? true : false;
            // objCustReport.ColumnName = string.Join(",", checkedColumns);
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            objCustReport.FilterColumns = hdnFilterColumns.Value.TrimEnd('^');
            objCustReport.FilterValues = HttpUtility.HtmlDecode(hdnFilterValues.Value.Trim().TrimEnd('^').TrimEnd('|'));
            hdnCustomizeReportName.Value = objCustReport.ReportName;
            objCustReport.IsAscending = rdbOrders.SelectedItem.Value == "1" ? true : false;
            //objCustReport.SortBy = drpSortBy.Text;
            objCustReport.SortBy = hdnDrpSortBy.Value;
            objCustReport.MainHeader = chkMainHeader.Checked ? true : false;
            objCustReport.CompanyName = txtCompanyName.Text;
            objCustReport.ReportTitle = txtReportTitle.Text;
            objCustReport.SubTitle = txtSubtitle.Text;
            if (chkDatePrepared.Checked)
            {
                objCustReport.DatePrepared = drpDatePrepared.SelectedValue.ToString();
            }
            else
            {
                objCustReport.DatePrepared = "";
            }
            objCustReport.TimePrepared = chkTimePrepared.Checked ? true : false;
            if (chkPageNumber.Checked)
            {
                objCustReport.PageNumber = drpPageNumber.SelectedValue.ToString();
            }
            else
            {
                objCustReport.PageNumber = "";
            }
            objCustReport.ExtraFooterLine = txtExtraFooterLine.Text;
            objCustReport.Alignment = drpAlignment.SelectedValue.ToString();
            objCustReport.PDFSize = drpPDFPageSize.SelectedValue.ToString();

            if (objBL_ReportsData.CheckExistingReport(objCustReport, hdnReportAction.Value) == true)
            {
                dvSaveReport.Attributes.Add("style", "display:block");
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report with this name already exists!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            DataSet ds = new DataSet();
            if (hdnReportAction.Value == "Save")
            {
                ds = objBL_ReportsData.InsertCustomerReport(objCustReport);
                pubReportId = Convert.ToInt32(ds.Tables[0].Rows[0]["ReportId"]);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report added successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            else
            {
                // if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
                if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
                {
                    objBL_ReportsData.UpdateCustomerReport(objCustReport);
                    pubReportId = objCustReport.ReportId;
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report customized successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                    //}
                    //  else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
                    //{
                    //  ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be updated!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                }
            }
            dvSaveReport.Attributes.Add("style", "display:none");
            GetReportDetailByRptId();
            GetReportsName();
            GetReportColumnsByRepId();
            ConvertToJSON();
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    protected void btnDeleteReport_Click(object sender, EventArgs e)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            if (drpReports.Items.Count > 0)
            {
                objCustReport.ReportId = Convert.ToInt32(drpReports.SelectedValue);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Please select report to delete.',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                return;
            }
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            //if (objBL_ReportsData.CheckForDelete(objCustReport) == true && drpReports.SelectedItem.ToString().ToLower() != "default report")
            if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            {
                objBL_ReportsData.DeleteCustomerReport(objCustReport);
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report deleted successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

                GetReportsName();
                if (drpReports.Items.Count > 0)
                {
                    drpReports.SelectedIndex = 0;
                    pubReportId = Convert.ToInt32(drpReports.SelectedValue);
                    hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();

                    GetReportDetailByRptId();
                    GetReportsName();
                    GetReportColumnsByRepId();
                    ConvertToJSON();
                }
                else
                {
                    // CrystalReportViewer1.ReportSource = null;
                }
                // return;
            }
            //else if (drpReports.SelectedItem.ToString().ToLower() == "default report")
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Default Report can not be deleted!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            //}
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to delete this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            // GetReportsName();
            //drpReports.SelectedValue = pubReportId.ToString();

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void BindReport(string[] checkedColumns, string[] selectedFiltersColumns, string[] selectedFiltersValues, string sortBy)
    {
        string query = "SELECT ";
        //bool isSelected = chkColumnList.Items.Cast<ListItem>().Count(i => i.Selected == true) > 0;
        //if (!isSelected)
        //{
        //    chkColumnList.Items[0].Selected = true;
        //}
        //foreach (ListItem item in chkColumnList.Items)
        //{
        //    if (item.Selected)
        //    {
        //        query += item.Value + ",";
        //        isSelected = true;
        //    }
        //}

        foreach (var item in checkedColumns)
        {
            query += item + ",";
        }

        query = query.Substring(0, query.Length - 1);
        if (selectedFiltersColumns == null)
        {
            query += " FROM CustomerReportDetails order by " + sortBy;
        }
        else
        {
            string filters = string.Empty;
            if (selectedFiltersColumns != null)
            {
                for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                {
                    if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice")
                    {
                        if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        {
                            filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        }
                        else
                        {
                            int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                            if (indexOfSingleQuote == 0)
                            {
                                filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                        }
                    }
                    else
                    {
                        if (selectedFiltersValues[i].Contains("and"))
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        }
                        else
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        }
                    }
                }
            }
            filters = filters.Substring(0, filters.Length - 4);
            query += " FROM CustomerReportDetails where " + filters + " order by " + sortBy;
        }


        //ReportDocument crystalReport = new ReportDocument();
        //CrystalReportViewer1.DisplayGroupTree = false;
        //crystalReport.Load(Server.MapPath("~/Reports/test1.rpt"));

        //DataSet dsCustomers = GetData(query, crystalReport);

        //crystalReport.SetDataSource(dsCustomers);

        //// var field = crystalReport.ReportDefinition.ReportObjects["CompanyName"];
        ////  field.ObjectFormat.HorizontalAlignment = Alignment.RightAlign;         


        ////  crystalReport.SetParameterValue("CompanyName", txtCompanyName.Text);             

        //crystalReport.SetParameterValue("ReportTitle", txtReportTitle.Text);
        //crystalReport.SetParameterValue("SubTitle", txtSubtitle.Text);
        //CrystalReportViewer1.ReportSource = crystalReport;


        //Session["ReportDocument"] = crystalReport;

    }

    //private DataSet GetData(string query, ReportDocument crystalReport)
    //{
    //    DataSet dsGetCustDetails = new DataSet();
    //    objProp_User.DBName = Session["dbname"].ToString();
    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);


    //    TestDS ds = new TestDS();

    //    // List<TextObject> textObjectsHead = crystalReport.ReportDefinition.Sections["Section1"].ReportObjects.OfType<TextObject>().ToList();
    //    // textObjectsHead[0].Text = "Test Company welcome";

    //    //Get the List of all TextObjects in Section2.
    //    List<TextObject> textObjects = crystalReport.ReportDefinition.Sections["Section2"].ReportObjects.OfType<TextObject>().ToList();
    //    List<TextObject> footerTextObjects = crystalReport.ReportDefinition.Sections["Section4"].ReportObjects.OfType<TextObject>().ToList();

    //    for (int i = 0; i < textObjects.Count; i++)
    //    {
    //        //Set the name of Column in TextObject.
    //        textObjects[i].Text = string.Empty;
    //        footerTextObjects[i].Text = string.Empty;
    //        if (dsGetCustDetails.Tables[0].Columns.Count > i)
    //        {
    //            textObjects[i].Text = dsGetCustDetails.Tables[0].Columns[i].ToString();

    //            if (textObjects[i].Text.ToLower() == "balance")
    //            {
    //                if (i == 0)
    //                {
    //                    footerTextObjects[0].Text = string.Empty; ;
    //                }
    //                else
    //                {
    //                    footerTextObjects[0].Text = "Total";
    //                }
    //                footerTextObjects[i].Text = (dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty)).ToString();
    //            }
    //            if (textObjects[i].Text.ToLower() == "loc")
    //            {
    //                if (i == 0)
    //                {
    //                    footerTextObjects[0].Text = string.Empty; ;
    //                }
    //                else
    //                {
    //                    footerTextObjects[0].Text = "Total";
    //                }
    //                footerTextObjects[i].Text = (dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty)).ToString();
    //            }
    //            if (textObjects[i].Text.ToLower() == "equip")
    //            {
    //                if (i == 0)
    //                {
    //                    footerTextObjects[0].Text = string.Empty; ;
    //                }
    //                else
    //                {
    //                    footerTextObjects[0].Text = "Total";
    //                }
    //                footerTextObjects[i].Text = (dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty)).ToString();
    //            }
    //            if (textObjects[i].Text.ToLower() == "opencall")
    //            {
    //                if (i == 0)
    //                {
    //                    footerTextObjects[0].Text = string.Empty; ;
    //                }
    //                else
    //                {
    //                    footerTextObjects[0].Text = "Total";
    //                }
    //                footerTextObjects[i].Text = (dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty)).ToString();
    //            }
    //        }
    //    }


    //    for (int i = 0; i < dsGetCustDetails.Tables[0].Rows.Count; i++)
    //    {
    //        DataRow dr = ds.Tables[0].Rows.Add();
    //        for (int j = 0; j < dsGetCustDetails.Tables[0].Columns.Count; j++)
    //        {
    //            dr[j] = dsGetCustDetails.Tables[0].Rows[i][j];
    //        }
    //    }

    //    DataSet dsC = new DataSet();
    //    objProp_User.DBName = Session["dbname"].ToString();
    //    objProp_User.ConnConfig = Session["config"].ToString();
    //    dsC = objBL_ReportsData.GetControlForReports(objProp_User);
    //    for (int i = 0; i < dsC.Tables[0].Rows.Count; i++)
    //    {
    //        DataRow dr = ds.Tables[1].Rows.Add();
    //        for (int j = 0; j < dsC.Tables[0].Columns.Count; j++)
    //        {
    //            dr[j] = dsC.Tables[0].Rows[i][j];
    //        }
    //    }


    //    return ds;
    //    //  }
    //}

    protected void drpReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // if (drpReports.SelectedIndex != 0)
            // {
            pubReportId = Convert.ToInt32(drpReports.SelectedValue);
            hdnCustomizeReportName.Value = drpReports.SelectedItem.ToString();
            GetReportsName();
            drpReports.SelectedValue = pubReportId.ToString();

            if (pubReportId != 0)
            {
                //grdCustomerReportData.PageIndex = 0;
                // dvGridReport.Attributes.Add("style", "display:block");
                GetReportDetailByRptId();
                GetReportColumnsByRepId();
            }
            else
            {
                GetGroupedCustomersLocation();
                // grdCustomerReportData.DataSource = null;
                dvGridReport.Attributes.Add("style", "display:none");
            }


            //  GetReportDetailByRptId();
            // GetReportColumnsByRepId();
            ConvertToJSON();
            // }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Customers.aspx", false);
        }
        catch
        {
            //
        }
    }

    private void GetGroupedCustomersLocation()
    {
        try
        {
            GroupedCustomerLocations dsGpdCustLoc = new GroupedCustomerLocations();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            DataSet ds = new DataSet();
            ds = objBL_ReportsData.GetGroupedCustomersLocation(objProp_User);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsGpdCustLoc.Tables[0].Rows.Add();
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        dr[j] = ds.Tables[0].Rows[i][j];
                    }
                }

                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    DataRow dr = dsGpdCustLoc.Tables[1].Rows.Add();
                    for (int j = 0; j < ds.Tables[1].Columns.Count; j++)
                    {
                        dr[j] = ds.Tables[1].Rows[i][j];
                    }
                }

                DataSet dsCompany = new DataSet();
                dsCompany = objBL_ReportsData.GetControlForReports(objProp_User);
                for (int i = 0; i < dsCompany.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsGpdCustLoc.Tables[2].Rows.Add();
                    for (int j = 0; j < dsCompany.Tables[0].Columns.Count; j++)
                    {
                        dr[j] = dsCompany.Tables[0].Rows[i][j];
                    }
                }


                //ReportDocument crystalReport = new ReportDocument();
                //CrystalReportViewer1.DisplayGroupTree = false;
                //crystalReport.Load(Server.MapPath("~/Reports/GroupedCustomerLocations.rpt"));

                //crystalReport.SetDataSource(dsGpdCustLoc);
                //CrystalReportViewer1.ReportSource = crystalReport;
                //CrystalReportViewer1.EnableDatabaseLogonPrompt = false;
                //crystalReport.Subreports["EquipmentDetails.rpt"].SetDataSource(dsGpdCustLoc.Tables[1]);
                //Session["ReportDocument"] = crystalReport;

                dvGridReport.Attributes.Add("style", "display:none");
                //grdCustomerReportData.DataSource = null;

            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    private void BindGridReport(string[] checkedColumns, string[] selectedFiltersColumns, string[] selectedFiltersValues, string sortBy)
    {
        string query = "SELECT distinct ";

        foreach (var item in checkedColumns)
        {
            query += item + ",";
        }

        query = query.Substring(0, query.Length - 1);
        if (selectedFiltersColumns == null)
        {
            query += " FROM CustomerReportDetails order by " + sortBy;
        }
        else
        {
            string filters = string.Empty;
            if (selectedFiltersColumns != null)
            {
                for (int i = 0; i <= selectedFiltersColumns.Count() - 1; i++)
                {
                    if (selectedFiltersColumns[i].ToLower() != "balance" && selectedFiltersColumns[i].ToLower() != "loc" && selectedFiltersColumns[i].ToLower() != "equip" && selectedFiltersColumns[i].ToLower() != "opencall" && selectedFiltersColumns[i].ToLower() != "equipmentprice")
                    {
                        if (!selectedFiltersValues[i].Contains("'") && !selectedFiltersValues[i].Contains("|"))
                        {
                            filters += selectedFiltersColumns[i] + "=" + "'" + selectedFiltersValues[i] + "'" + " AND ";
                        }
                        else
                        {
                            int indexOfSingleQuote = selectedFiltersValues[i].IndexOf("'");
                            if (indexOfSingleQuote == 0)
                            {
                                filters += selectedFiltersColumns[i] + " in (" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                            else
                            {
                                filters += selectedFiltersColumns[i] + " in ('" + selectedFiltersValues[i].Replace('|', ',') + ")" + " AND ";
                            }
                        }
                    }
                    else
                    {
                        if (selectedFiltersValues[i].Contains("and"))
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i].Replace("and", "and " + selectedFiltersColumns[i] + "") + " AND ";
                        }
                        else
                        {
                            filters += selectedFiltersColumns[i] + selectedFiltersValues[i] + " AND ";
                        }
                    }
                }
            }
            filters = filters.Substring(0, filters.Length - 4);
            query += " FROM CustomerReportDetails where " + filters + " order by " + sortBy;
        }


        BindHeaderDetails();
        GetGridData(query);

    }

    //public static Image resizeImage(Image imgToResize, Size size)
    //{
    //    return (System.Drawing.Image)(new System.Drawing.Bitmap(imgToResize, size));
    //}

    private void BindHeaderDetails()
    {
        try
        {
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);
            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];
                imgLogo.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(compLogo);

                MemoryStream ms = new MemoryStream(compLogo, 0, compLogo.Length);
                // Convert byte[] to Image
                ms.Write(compLogo, 0, compLogo.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                string imagePathToSave = Server.MapPath("ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png");
                string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";

                System.Drawing.Image resizedImage = image.GetThumbnailImage(130, 130, null, IntPtr.Zero);
                resizedImage.Save(imagePathToSave, System.Drawing.Imaging.ImageFormat.Png);
                // imgLogo.ImageUrl = imagePathToShow;

                lblCompanyName.Text = dsCompDetail.Tables[0].Rows[0]["Name"].ToString();
                lblCompAddress.Text = dsCompDetail.Tables[0].Rows[0]["Address"].ToString();
                lblCompEmail.Text = dsCompDetail.Tables[0].Rows[0]["Email"].ToString();
            }

            objCustReport.ReportId = pubReportId;
            DataSet dsGetHeaderFooterDetail = new DataSet();
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                hdnMainHeader.Value = dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString();
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {
                    chkMainHeader.Checked = true;
                }
                else
                {

                    chkMainHeader.Checked = false;
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    txtCompanyName.Enabled = true;
                    txtCompanyName.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    lblCompanyName2.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString();
                    chkCompanyName.Checked = true;
                }
                else
                {
                    txtCompanyName.Enabled = false;
                    txtCompanyName.Text = "";
                    lblCompanyName2.Text = "";
                    chkCompanyName.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    txtReportTitle.Enabled = true;
                    txtReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    lblReportTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString();
                    chkReportTitle.Checked = true;
                }
                else
                {
                    txtReportTitle.Enabled = false;
                    txtReportTitle.Text = "";
                    lblReportTitle.Text = "";
                    chkReportTitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    txtSubtitle.Enabled = true;
                    txtSubtitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    lblSubTitle.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString();
                    chkSubtitle.Checked = true;
                }
                else
                {
                    txtSubtitle.Enabled = false;
                    txtSubtitle.Text = "";
                    lblSubTitle.Text = "";
                    chkSubtitle.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "12/31/01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 01")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "Dec 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMM dd, yyyy");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() == "December 31, 2001")
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MMMM dd, yyyy");
                    }
                    else
                    {
                        lblDate.Text = DateTime.Now.Date.ToString("MM/dd/yyyy");
                    }

                    drpDatePrepared.Enabled = true;
                    drpDatePrepared.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString();
                    chkDatePrepared.Checked = true;
                }
                else
                {
                    lblDate.Text = "";
                    drpDatePrepared.Enabled = false;
                    drpDatePrepared.SelectedIndex = 0;
                    chkDatePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    lblTime.Text = DateTime.Now.ToString("hh:mm tt");
                    chkTimePrepared.Checked = true;
                }
                else
                {
                    lblTime.Text = "";
                    chkTimePrepared.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString() != "")
                {
                    drpPageNumber.Enabled = true;
                    drpPageNumber.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PageNumber"].ToString();
                    chkPageNumber.Checked = true;
                }
                else
                {
                    drpPageNumber.Enabled = false;
                    drpPageNumber.SelectedIndex = 0;
                    chkPageNumber.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    txtExtraFooterLine.Enabled = true;
                    txtExtraFooterLine.Text = dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString();
                    chkExtraFootLine.Checked = true;
                }
                else
                {
                    txtExtraFooterLine.Enabled = false;
                    txtExtraFooterLine.Text = "";
                    chkExtraFootLine.Checked = false;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() != "")
                {
                    drpAlignment.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-left");
                        dvSubHeader.Attributes.Add("align", "left");
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-right");
                        dvSubHeader.Attributes.Add("align", "right");
                    }
                    else
                    {
                        dvSubHeader.Attributes.Add("Style", "text-align:-moz-center");
                        dvSubHeader.Attributes.Add("align", "center");
                    }
                }
                else
                {
                    drpDatePrepared.SelectedIndex = 0;
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString() != "")
                {
                    drpPDFPageSize.SelectedValue = dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString();
                }
                Session["ReportDate"] = lblDate.Text;
                Session["ReportTime"] = lblTime.Text;
            }
        }
        catch (Exception exp)
        {
            //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GetGridData(string query)
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetOwners(query, objProp_User);
            Session["DsGetCustomerDetails"] = dsGetCustDetails;
            Session["Query"] = query;
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                // grdCustomerReportData.DataSource = dsGetCustDetails.Tables[0];
                // grdCustomerReportData.DataBind();

                BindReportTable(dsGetCustDetails);

            }
            else
            {
                // grdCustomerReportData.DataSource = null;
                //  grdCustomerReportData.DataBind();
            }


            // dvGridReport.Attributes.Add("style", "display:block");
            //CrystalReportViewer1.ReportSource = null;
            // getQuery = query;
            ConvertToJSON();

        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    //protected void btnNext_Click(object sender, EventArgs e)
    //{
    //    int i = grdCustomerReportData.PageIndex + 1;

    //    if (i <= grdCustomerReportData.PageCount)
    //    {

    //        grdCustomerReportData.PageIndex = i;

    //      //  btnlast.Enabled = true;

    //       // btnprevious.Enabled = true;

    //      //  btnfirst.Enabled = true;

    //    }



    //    if (grdCustomerReportData.PageCount - 1 == grdCustomerReportData.PageIndex)
    //    {

    //       // btnnext.Enabled = false;

    //       // btnlast.Enabled = false;

    //    }
    //}



    //protected void grdCustomerReportData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.Width = (e.Row.Cells.Count) * 120;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            // int i = e.Row.Cells.Count;
    //            for (int i = 0; i <= e.Row.Cells.Count - 1; i++)
    //            {
    //                e.Row.Cells[i].Width = new Unit("120px");
    //            }
    //        }
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}
    //protected void grdCustomerReportData_DataBound(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        ddlPages.Items.Clear();
    //        for (int i = 0; i < grdCustomerReportData.PageCount; i++)
    //        {

    //            int intPageNumber = i + 1;
    //            System.Web.UI.WebControls.ListItem lstItem = new System.Web.UI.WebControls.ListItem(intPageNumber.ToString());

    //            if (i == grdCustomerReportData.PageIndex)
    //                lstItem.Selected = true;

    //            ddlPages.Items.Add(lstItem);
    //        }

    //        lblPageCount.Text = grdCustomerReportData.PageCount.ToString();
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void drpGridRow_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.PageSize = Convert.ToInt32(drpGridRow.SelectedValue);
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void ddlPages_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.PageIndex = ddlPages.SelectedIndex;
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void btnFirst_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.PageIndex = 0;
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void btnForward_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        int i = grdCustomerReportData.PageIndex + 1;
    //        if (i <= grdCustomerReportData.PageCount)
    //        {
    //            grdCustomerReportData.PageIndex = i;
    //        }

    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }

    //}

    //protected void btnPrev_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        int i = grdCustomerReportData.PageCount;

    //        if (grdCustomerReportData.PageIndex > 0)
    //        {
    //            grdCustomerReportData.PageIndex = grdCustomerReportData.PageIndex - 1;
    //        }
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}

    //protected void btnLast_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.PageIndex = grdCustomerReportData.PageCount;
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}
    //protected void grdCustomerReportData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        grdCustomerReportData.PageIndex = e.NewPageIndex;
    //        GetGridData(getQuery);
    //    }
    //    catch (Exception exp)
    //    {
    //        ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
    //    }
    //}


    private void BindReportTable(DataSet dsGetCustDetails)
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            DataSet dsGetColumnWidth = new DataSet();
            objCustReport.ReportId = pubReportId;
            dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
            hdnColumnWidth.Value = "";
            if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
                {
                    hdnColumnWidth.Value += dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString() + ",";
                }

                hdnColumnWidth.Value = hdnColumnWidth.Value.TrimEnd(',');
            }

            //Building an HTML string.
            StringBuilder html = new StringBuilder();
            string footer = string.Empty;
            //Table start.
            html.Append("<table id='tblResize' border = '0'>");

            //Building the Header row.
            html.Append("<thead><tr>");

            foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
            {
                html.Append("<th class='resize-header'>");
                //html.Append("<th style='border:13px solid transparent;color:black;font-size:11px;width:150px; border-image: url(images/icons_big/list-bullet2.PNG) " + b + " '>");
                //html.Append("<th style='border:1;color:black;font-size:11px;'>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr></thead>");


            //Building the Data rows.
            foreach (DataRow row in dsGetCustDetails.Tables[0].Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in dsGetCustDetails.Tables[0].Columns)
                {
                    html.Append("<td style='border:0;padding:10px 20px 3px 10px;color:black;'>");
                    html.Append(row[column.ColumnName]);
                    html.Append("</td>");
                }
                html.Append("</tr>");

            }

            // html.Append("<tr><td>&nbsp;</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td><td>5000</td></tr>");

            for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
            {
                if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString() + "</td>";
                }
                else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                {
                    footer += "<td style='border:0;padding:20px 20px 3px 10px;color:black;font-weight:bold;'>" + dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString() + "</td>";
                }
                else
                {
                    footer += "<td style='border:0;padding:10px 20px 3px 10px;color:black;'>&nbsp;</td>";
                }
            }
            if (footer != "")
            {
                html.Append("<tr>" + footer + "</tr>");
            }

            //Table end.
            html.Append("</table>");

            //Append the HTML string to Placeholder.
            PlaceHolder1.Controls.Add(new Literal { Text = html.ToString() });

            // getPrintData = html.ToString();

            //try
            //{
            //    GeneratePdfTable(dsGetCustDetails);
            //}
            //catch (Exception exp)
            //{
            //    ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            //}
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }


    // protected void btnPrintReport_Click(object sender, EventArgs e)
    // {
    //Response.ContentType = "application/pdf";
    //Response.AddHeader("content-disposition", "attachment;filename=UserDetails.pdf");
    //Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //StringWriter sw = new StringWriter();
    //HtmlTextWriter hw = new HtmlTextWriter(sw);
    //// this.Page.RenderControl(hw);
    //dvGridReport.RenderControl(hw);
    ////StringReader sr = new StringReader(sw.ToString());
    //StringReader sr = new StringReader(getPrintData);
    //Document pdfDoc = new Document(PageSize.LEGAL, 10f, 10f, 100f, 0.0f);
    //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
    //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
    //pdfDoc.Open();
    //htmlparser.Parse(sr);
    //pdfDoc.Close();
    //Response.Write(pdfDoc);
    //Response.End();

    //  }

    //protected void ExportToImage(object sender, EventArgs e)
    //{
    //    string base64 = Request.Form[hfImageData.UniqueID].Split(',')[1];
    //    byte[] bytes = Convert.FromBase64String(base64);
    //    Response.Clear();
    //    Response.ContentType = "image/png";
    //    Response.AddHeader("Content-Disposition", "attachment; filename=HTML.png");
    //    Response.Buffer = true;
    //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //    Response.BinaryWrite(bytes);
    //    Response.End();
    //}


    private void GeneratePdfTable(DataSet dsGetCustDetails)
    {
        try
        {
            DeletePDFFiles();
        }
        catch
        {
            //
        }
        //server folder path which is stored your PDF documents         
        string serverPath = Server.MapPath("ReportFiles/PDF");
        //string filename = path + "/Doc1.pdf";
        int userId = Convert.ToInt32(Session["UserID"].ToString());
        string fileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".pdf";
        string filePath = serverPath + "/" + fileName;
        //File.Delete(filePath);
        Session["FilePath"] = filePath;
        // File.Create(filePath);
        // lblAttachedFile.Text = fileName;


        objCustReport.DBName = Session["dbname"].ToString();
        objCustReport.ConnConfig = Session["config"].ToString();

        int[] getColumnsWidth = new int[dsGetCustDetails.Tables[0].Columns.Count];
        int countTotalWidth = 0;
        DataSet dsGetColumnWidth = new DataSet();
        objCustReport.ReportId = pubReportId;
        dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
        if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
            {
                getColumnsWidth[i] = Convert.ToInt32(dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString().Replace("px", ""));
                countTotalWidth = countTotalWidth + getColumnsWidth[i];
            }
        }

        DataSet dsGetHeaderFooterDetail = new DataSet();
        dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);

        Rectangle rcPageSize = PageSize.A4;
        //Create new PDF document  
        if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
        {
            rcPageSize = PageSize.GetRectangle(dsGetHeaderFooterDetail.Tables[0].Rows[0]["PDFSize"].ToString());
        }
        // Rectangle rcPageSize = PageSize.GetRectangle("A1");
        //Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);
        Document document = new Document(rcPageSize, 20f, 20f, 20f, 20f);

        try
        {
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create, FileAccess.Write));

            PdfPTable headerTable = new PdfPTable(2);
            PdfPTable tblDateTime = new PdfPTable(1);
            PdfPTable tblExtraFooter = new PdfPTable(1);
            tblExtraFooter.TotalWidth = countTotalWidth;
            tblExtraFooter.LockedWidth = true;
            tblExtraFooter.SpacingBefore = 30f;

            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                {

                    headerTable.TotalWidth = 300;
                    headerTable.LockedWidth = true;

                    //  headerTable.SpacingBefore = 20f;
                    //  headerTable.SpacingAfter = 30f;
                    headerTable.HorizontalAlignment = 0;

                    tblDateTime.TotalWidth = 200;
                    tblDateTime.LockedWidth = true;
                    tblDateTime.HorizontalAlignment = 2;

                    DataSet dsCompDetail = new DataSet();
                    objProp_User.DBName = Session["dbname"].ToString();
                    objProp_User.ConnConfig = Session["config"].ToString();
                    dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

                    byte[] compLogo = (byte[])dsCompDetail.Tables[0].Rows[0]["Logo"];

                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(compLogo);
                    logo.ScaleAbsolute(110, 110);
                    PdfPCell imageCell = new PdfPCell(logo);
                    imageCell.Border = 0;
                    headerTable.AddCell(imageCell);

                    Font compNameStyle = FontFactory.GetFont("Arial", 13, iTextSharp.text.Font.BOLD);
                    Font compStyle = FontFactory.GetFont("Arial", 11, iTextSharp.text.Font.NORMAL);

                    PdfPTable tblCompDetails = new PdfPTable(1);

                    PdfPCell companyName = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Name"].ToString(), compNameStyle));
                    companyName.Border = 0;
                    companyName.PaddingTop = 20;
                    companyName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(companyName);

                    PdfPCell compAddress = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Address"].ToString(), compStyle));
                    compAddress.Border = 0;
                    compAddress.PaddingTop = 10;
                    compAddress.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compAddress);

                    PdfPCell compEmail = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Email"].ToString(), compStyle));
                    compEmail.Border = 0;
                    compEmail.PaddingTop = 10;
                    compEmail.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    tblCompDetails.AddCell(compEmail);

                    headerTable.DefaultCell.Border = Rectangle.NO_BORDER;
                    headerTable.AddCell(tblCompDetails);
                }
            }
            Font header2ompNameStyle = FontFactory.GetFont("Arial", 12, iTextSharp.text.Font.BOLD);
            Font header2ReportTitleStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            Font header2SubTitleStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);
            PdfPTable tblCompDetails2 = new PdfPTable(1);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                tblCompDetails2.DefaultCell.Border = Rectangle.NO_BORDER;
                tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                tblCompDetails2.SpacingBefore = 15;
                tblCompDetails2.SpacingAfter = 10;
                //   tblCompDetails2.WidthPercentage = 50.0f;

                //  int[] compDetailWidth = new int[] { 1000 };
                // tblCompDetails2.SetWidths(compDetailWidth);
                //if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                //{
                //    tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                //}
                //else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                //{
                //    tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //}
                //else
                //{
                //    tblCompDetails2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                //}

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                {
                    PdfPCell compName = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString(), header2ompNameStyle));
                    compName.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        compName.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(compName);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                {
                    PdfPCell reportTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString(), header2ReportTitleStyle));
                    reportTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        reportTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(reportTitle);
                }
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                {
                    PdfPCell subTitle = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString(), header2SubTitleStyle));
                    subTitle.Border = 0;
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Right")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                    }
                    else if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString() == "Left")
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_LEFT;
                    }
                    else
                    {
                        subTitle.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    }
                    tblCompDetails2.AddCell(subTitle);
                }
                Font dateTimeStyle = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD);
                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["TimePrepared"].ToString() == "True")
                {
                    if (Session["ReportTime"] != null)
                    {
                        PdfPCell time = new PdfPCell(new Phrase(Session["ReportTime"].ToString(), dateTimeStyle));
                        time.Border = 0;
                        time.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(time);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["DatePrepared"].ToString() != "")
                {
                    if (Session["ReportDate"] != null)
                    {
                        PdfPCell date = new PdfPCell(new Phrase(Session["ReportDate"].ToString(), dateTimeStyle));
                        date.Border = 0;
                        date.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                        tblDateTime.AddCell(date);
                    }
                }

                if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                {
                    PdfPCell extraFooter = new PdfPCell(new Phrase(dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString(), header2ReportTitleStyle));
                    extraFooter.Border = 0;
                    extraFooter.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                    tblExtraFooter.AddCell(extraFooter);
                }
            }

            PdfPTable table = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            table.TotalWidth = countTotalWidth;

            //fix the absolute width of the table
            table.LockedWidth = true;

            //relative col widths in proportions - 1/3 and 2/3
            //float[] widths = new float[] { 20f, 4f, 6f };
            // int[] widths2 = new int[] { 350, 200, 50, 50, 50, 50, 50, 50, 50, 50, 50, 50 };
            table.SetWidths(getColumnsWidth);
            table.HorizontalAlignment = 0;
            table.DefaultCell.Border = Rectangle.NO_BORDER;
            //leave a gap before and after the table
            table.SpacingBefore = 20f;
            table.SpacingAfter = 30f;
            Font headerStyle = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD);
            Font rowsStyle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL);
            PdfPTable footer = new PdfPTable(dsGetCustDetails.Tables[0].Columns.Count);
            if (dsGetCustDetails.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j <= dsGetCustDetails.Tables[0].Columns.Count - 1; j++)
                {
                    PdfPCell columns = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Columns[j].ToString(), headerStyle));
                    columns.Border = 0;
                    table.AddCell(columns);
                }

                for (int j = 0; j <= dsGetCustDetails.Tables[0].Rows.Count - 1; j++)
                {
                    for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Rows[j][i].ToString().Trim(), rowsStyle));
                        rows.Border = 0;
                        table.AddCell(rows);
                    }
                }

                Font footerStyle = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
                footer.TotalWidth = countTotalWidth;
                footer.LockedWidth = true;
                footer.SetWidths(getColumnsWidth);
                footer.HorizontalAlignment = 0;
                footer.DefaultCell.Border = Rectangle.NO_BORDER;

                //leave a gap before and after the table
                //footer.SpacingBefore = 5f;
                //footer.SpacingAfter = 30f;


                for (int i = 0; i <= dsGetCustDetails.Tables[0].Columns.Count - 1; i++)
                {
                    if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "Balance")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(Balance)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "loc")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(loc)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "equip")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(equip)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else if (dsGetCustDetails.Tables[0].Columns[i].ToString() == "opencall")
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(dsGetCustDetails.Tables[0].Compute("SUM(opencall)", string.Empty).ToString(), footerStyle));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                    else
                    {
                        PdfPCell rows = new PdfPCell(new Phrase(""));
                        rows.Border = 0;
                        footer.AddCell(rows);
                    }
                }
            }

            //PdfPCell cell1 = new PdfPCell(new Phrase("Header1", fontTitulosDeptos));
            //PdfPCell cell2 = new PdfPCell(new Phrase("Header2", fontTitulosDeptos));
            //PdfPCell cell3 = new PdfPCell(new Phrase("Header3", fontTitulosDeptos));
            ////cell.Colspan = 3;            

            //cell1.Border = 0;
            //cell2.Border = 0;
            //cell3.Border = 0;
            //cell1.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right            
            //table.AddCell(cell1);
            //cell2.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell2);
            //cell3.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            //table.AddCell(cell3);

            //PdfPCell cell4 = new PdfPCell(new Phrase("Col 1 Row 1", fontTitulosDeptos));
            //cell4.Border = 0;
            //table.AddCell(cell4);
            //PdfPCell cell5 = new PdfPCell(new Phrase("Col 2 Row 1", fontTitulosDeptos));
            //cell5.Border = 0;
            //table.AddCell(cell5);
            //PdfPCell cell6 = new PdfPCell(new Phrase("Col 3 Row 1", fontTitulosDeptos));
            //cell6.Border = 0;
            //table.AddCell(cell6);
            //PdfPCell cell7 = new PdfPCell(new Phrase("Col 1 Row 2", fontTitulosDeptos));
            //cell7.Border = 0;
            //table.AddCell(cell7);
            //PdfPCell cell8 = new PdfPCell(new Phrase("Col 1 Row 2", fontTitulosDeptos));
            //cell8.Border = 0;
            //table.AddCell(cell8);
            //PdfPCell cell9 = new PdfPCell(new Phrase("Col 1 Row 2", fontTitulosDeptos));
            //cell9.Border = 0;
            //table.AddCell(cell9);           

            document.Open();
            document.Add(tblDateTime);
            document.Add(headerTable);
            if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
            {
                document.Add(tblCompDetails2);
            }
            document.Add(table);
            document.Add(footer);
            document.Add(tblExtraFooter);
        }
        catch (Exception ex)
        {
            document.Close();
        }
        finally
        {
            document.Close();
            //ShowPdf(filename);
        }
    }

    public void ShowPdf(string filename)
    {
        //Clears all content output from Buffer Stream
        Response.ClearContent();
        //Clears all headers from Buffer Stream
        Response.ClearHeaders();
        //Adds an HTTP header to the output stream
        Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
        //Gets or Sets the HTTP MIME type of the output stream
        Response.ContentType = "application/pdf";
        //Writes the content of the specified file directory to an HTTP response output stream as a file block
        Response.WriteFile(filename);
        //sends all currently buffered output to the client
        Response.Flush();
        //Clears all content output from Buffer Stream
        Response.Clear();
    }

    private void SaveResizedAndReorderReport()
    {
        try
        {
            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.UserId = Convert.ToInt32(Session["UserID"].ToString());
            objCustReport.ReportId = pubReportId;
            objCustReport.ColumnName = hdnLstColumns.Value.TrimEnd('^');
            objCustReport.ColumnWidth = hdnColumnWidth.Value.TrimEnd('^');
            if (objBL_ReportsData.CheckForDelete(objCustReport) == true)
            {
                objBL_ReportsData.UpdateCustomerReportResizedWidth(objCustReport);
                //ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Report updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'You dont have permission to update this report!',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        GetReportsName();
        if (pubReportId != 0)
        {
            drpReports.SelectedValue = pubReportId.ToString();
        }
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
        GeneratePdfTable(dsGetCustomerDetails);
        Session["ReportId"] = pubReportId;

        string script = String.Format("window.open('CustomerReportPreview.aspx');");
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
    }


    public void GetPDFData()
    {
        GetReportColumnsByRepId();
        DataSet dsGetCustomerDetails = new DataSet();
        dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
        Session["ReportId"] = pubReportId;
        GeneratePdfTable(dsGetCustomerDetails);
        //BindReportTable(dsGetCustomerDetails);
    }

    protected void btnSendReport_Click(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();
        if (hdnSendReportType.Value == "btnSendPDFReport")
        {
            DataSet dsGetCustomerDetails = new DataSet();
            dsGetCustomerDetails = (DataSet)Session["DsGetCustomerDetails"];
            GeneratePdfTable(dsGetCustomerDetails);
        }
        else
        {
            GenerateExcelFile();
        }
        if (txtTo.Text.Trim() != string.Empty)
        {
            Mail mail = new Mail();
            try
            {
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCc.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCc.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = txtSubject.Text;
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Report attached.";
                }

                string filePath = Session["FilePath"].ToString();
                mail.AttachmentFiles.Add(filePath);
                // mail.attachmentBytes = ExportReportToPDF("");                    

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                GetReportsName();
                if (pubReportId != 0)
                {
                    drpReports.SelectedValue = pubReportId.ToString();
                }
                //  this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }      
    }


    private void GenerateExcelFile()
    {
        try
        {
            try
            {
                DeleteExcelFiles();
            }
            catch
            {
                //
            }
            string htmlToExport = string.Empty;
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbExtraFooter = new StringBuilder();
            DataSet dsCompDetail = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsCompDetail = objBL_ReportsData.GetControlForReports(objProp_User);

            objCustReport.DBName = Session["dbname"].ToString();
            objCustReport.ConnConfig = Session["config"].ToString();
            objCustReport.ReportId = pubReportId;
            DataSet dsGetHeaderFooterDetail = new DataSet();
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);


            if (dsCompDetail.Tables[0].Rows.Count > 0)
            {
                string imagePathToShow = ConfigurationManager.AppSettings["ReportImagePath"].ToString() + "ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png";

                //  sbHeader.Append("<table  border = '0'>");
                //sbHeader.Append("<tr><td style='width:300px;text-align:left;height:200px'><img src = '" + imagePathToShow + "'></img></td>");
                // sbHeader.Append("<tr><td>");
                sbHeader.Append(@"<html xmlns:o='urn:schemas-microsoft-com:office:office' xmlns:w='urn:schemas-microsoft-com:office:excel' xmlns='http://www.w3.org/TR/REC-html40'><head><title>Time</title>");
                sbHeader.Append(@"<body lang=EN-US style='mso-element:header' id=h1><span style='mso--code:DATE'></span><div>");
                sbHeader.Append("<table border = '0'>");
                //sbHeader.Append("<tr><td rowspan='5' style='vertical-align:top'><img src=" + imagePathToShow + "></img></td><td style='padding-top:20px;'>&nbsp;</td></tr>");
                //sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
                //sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
                //sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
                //sbHeader.Append("<tr><td style='height:50px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>&nbsp;</td></tr>");
                //sbHeader.Append("<tr><td colspan='2'>"); 

                if (dsGetHeaderFooterDetail.Tables[0].Rows.Count > 0)
                {
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["MainHeader"].ToString() == "True")
                    {
                        sbHeader.Append("<tr><td><table><tr><td rowspan='4' style='height:150px;vertical-align:center;text-align:center;'><img src=" + imagePathToShow + "></img></td><td>&nbsp;</td></tr><tr><td style='height:20px;text-align:left;color:Black;font-size:18px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Name"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:bold;'>" + dsCompDetail.Tables[0].Rows[0]["Address"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:20px;text-align:left;color:Black;font-size:15px;font-weight:normal;'>" + dsCompDetail.Tables[0].Rows[0]["Email"].ToString() + "</td></tr>");
                        sbHeader.Append("<tr><td style='height:60px;'>&nbsp;</td></tr>");
                        sbHeader.Append("</table></td><td style='vertical-align:top;font-weight:bold;font-size:10px;color:black;'>" + lblTime.Text + " <br/> " + lblDate.Text + "</td></tr>");
                    }
                    string alignment = string.Empty;
                    alignment = dsGetHeaderFooterDetail.Tables[0].Rows[0]["Alignment"].ToString();
                    if (alignment.ToLower() == "standard" || alignment.ToLower() == "centered")
                    {
                        alignment = "center";
                    }

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["CompanyName"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:16px;font-weight:bold;'>" + lblCompanyName2.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ReportTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:14px;font-weight:bold;'>" + lblReportTitle.Text + "</td></tr>");
                    }
                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["SubTitle"].ToString() != "")
                    {
                        sbHeader.Append("<tr><td colspan='2' style='height:20px;text-align:" + alignment + ";color:Black;font-size:12px;font-weight:bold;'>" + lblSubTitle.Text + "</td></tr>");
                    }
                    sbHeader.Append("<tr><td colspan='2' style='height:15px;'>&nbsp;</td></tr>");
                    sbHeader.Append("<tr><td colspan='2'>");

                    if (dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() != "")
                    {
                        sbExtraFooter.Append("<tr><td colspan='2' style='height:80px;text-align:center;color:Black;font-size:14px;font-weight:bold;'>" + dsGetHeaderFooterDetail.Tables[0].Rows[0]["ExtraFooterLine"].ToString() + "</td></tr>");
                    }
                }
            }

            string html = hdnDivToExport.Value;
            // ExportToExcel(ref html, "MyReport");

            html = html.Replace("&gt;", ">");
            html = html.Replace("&lt;", "<");

            htmlToExport = sbHeader + "<br /> " + html + "</td></tr>" + sbExtraFooter + "</table></div></body></html>";

            //HttpContext.Current.Response.ClearContent();
            //HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + "_" + DateTime.Now.ToString("M_dd_yyyy_H_M_s") + ".xls");
            //HttpContext.Current.Response.ContentType = "application/xls";
            //HttpContext.Current.Response.Write(html);
            //HttpContext.Current.Response.End();

            //StringWriter sWriter = new StringWriter();
            //HtmlTextWriter hWriter = new HtmlTextWriter(sWriter);
            //dvGridReport.RenderControl(hWriter);
            //string HtmlInfo = sWriter.ToString().Trim();
            string excelServerPath = Server.MapPath("ReportFiles/Excel");
            int userId = Convert.ToInt32(Session["UserID"].ToString());
            string excelFileName = hdnCustomizeReportName.Value.Replace(" ", "") + "-" + userId + "-" + DateTime.Now.Ticks + ".xls";
            string filePath = excelServerPath + "/" + excelFileName;
            FileStream fStream = new FileStream(filePath, FileMode.Create);
            BinaryWriter BWriter = new BinaryWriter(fStream);
            //BWriter.Write(html);
            BWriter.Write(htmlToExport);
            BWriter.Close();
            fStream.Close();

            Session["FilePath"] = filePath;
        }
        catch
        {

        }
    }

    protected void ExportToExcel(object sender, EventArgs e)
    {
        SaveResizedAndReorderReport();
        GetReportColumnsByRepId();

        GetReportsName();
        if (pubReportId != 0)
        {
            drpReports.SelectedValue = pubReportId.ToString();
        }

        GenerateExcelFile();

        string script = String.Format("window.open('CustomerReportPreview.aspx');");
        this.Page.ClientScript.RegisterStartupScript(this.GetType(), "123", script, true);
    }

    private void DeletePDFFiles()
    {
        string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/PDF"));
        foreach (string filePath in filePaths)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                //
            }
        }
    }

    private void DeleteExcelFiles()
    {
        string[] filePaths = Directory.GetFiles(Server.MapPath("ReportFiles/Excel"));
        string[] imagesPaths = Directory.GetFiles(Server.MapPath("ReportFiles/Images"));
        foreach (string filePath in filePaths)
        {
            try
            {
                File.Delete(filePath);
            }
            catch
            {
                //
            }
        }

        //foreach (string imagePath in imagesPaths)
        //{
        //    try
        //    {
        //        File.Delete(imagePath);
        //    }
        //    catch
        //    {
        //        //
        //    }

        //}

        //try
        //{
        //    File.Delete(Server.MapPath("ReportFiles/Images/logo" + Convert.ToInt32(Session["UserID"].ToString()) + ".png"));
        //}
        //catch
        //{
        //    //
        //}

    }
    protected void btnSaveReport2_Click(object sender, EventArgs e)
    {
        try
        {
            SaveResizedAndReorderReport();
            GetReportColumnsByRepId();
            if (pubReportId != 0)
            {
                drpReports.SelectedValue = pubReportId.ToString();
            }
            //  this.programmaticModalPopup.Hide();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Report updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }
}


