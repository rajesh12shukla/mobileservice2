using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using BusinessEntity;
using BusinessLayer;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Drawing;
using System.Web.UI.HtmlControls;

public partial class RouteBuilder : System.Web.UI.Page
{
    Customer objCustomer = new Customer();
    BL_Customer objBL_Customer = new BL_Customer();
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    GeneralFunctions objGeneralFunctions = new GeneralFunctions();
    Random randomGen = new Random();

    public char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            GetRouteTemplate();
            FillRoute();
            GetLoc(0);            
            GetWorkers();
        }
        Permission();
    }

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("schMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("lnkSchd");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkRouteBuilder");
        //lnkUsersSmenu.Style.Add("color", "#FF7A0A");
        lnkUsersSmenu.Style.Add("color", "#316b9d");
        lnkUsersSmenu.Style.Add("font-weight", "normal");
        lnkUsersSmenu.Style.Add("background-color", "#e5e5e5");
        if (Session["type"].ToString() == "c")
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void Page_PreRender(Object o, EventArgs e)
    {
        int count = 0;
        int pageindex = 0;
        int page = 0;
        if (ViewState["pageindex"] != null)
        {
            if (ViewState["pageindex"].ToString() != string.Empty)
            {
                pageindex = Convert.ToInt32(ViewState["pageindex"]);
                page = (pageindex * 10);
            }
        }
        foreach (GridViewRow gr in gvLocations.Rows)
        {
            //HiddenField hdnCoordinate = (HiddenField)gr.FindControl("hdnCoordinate");
            //Label lblLoc = (Label)gr.FindControl("lblLoc");
            //Label lblName = (Label)gr.FindControl("lblName");
            //Label lblAddress = (Label)gr.FindControl("lblAddress");
            //Label lblWorker = (Label)gr.FindControl("lblWorker");
            //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            //CheckBox chkGridOrigin = (CheckBox)gr.FindControl("chkGridOrigin");
            //CheckBox chkGridDest = (CheckBox)gr.FindControl("chkGridDest");
            HyperLink lnkLoc = (HyperLink)gr.FindControl("lnkLoc");
            //string[] coordinate = hdnCoordinate.Value.Split(',');
            //string lat = string.Empty;
            //string lng = string.Empty;
            //if (coordinate.Count() > 1)
            //{
            //    lat = coordinate[0];
            //    lng = coordinate[1];

            //}
            //chkGridOrigin.Attributes["onclick"] = "toggleSelectionGrid('" + gvLocations.ClientID + "','chkGridOrigin','" + chkGridOrigin.ClientID + "');";
            //chkGridDest.Attributes["onclick"] = "toggleSelectionGrid('" + gvLocations.ClientID + "','chkGridDest','" + chkGridDest.ClientID + "');";
            //chkSelect.Attributes["onclick"] = "MarkerToArray(" + lat + ", " + lng + "," + objGeneralFunctions.EncodeJsString(lblName.Text) + "," + objGeneralFunctions.EncodeJsString(lblAddress.Text) + ",'" + chkSelect.ClientID + "'," + lblLoc.Text + ",'" + gr.ClientID + "','" + lblWorker.Text + "'); SelectionLimit('" + gvLocations.ClientID + "','chkSelect','" + chkSelect.ClientID + "'); CheckOrigDest('" + chkSelect.ClientID + "','" + chkGridOrigin.ClientID + "','" + chkGridDest.ClientID + "')";

            gr.Attributes["onclick"] = "GridHover(" + (page + count) + ")";            
            
            if (Session["MSM"].ToString() == "TS")
            {
                lnkLoc.Enabled = false;
            }
            ////////chkSelect.Attributes["onclick"] = "AddMarkerForRoute();";      
            count++;
        }
        //////////ClientScript.RegisterStartupScript(Page.GetType(), "key1", "SelectedRowStyle('" + gvLocations.ClientID + "');", true);
    }

    protected void btnOptimize_Click(object sender, EventArgs e)
    {
        hdnSwitchMethods.Value = "1";
        string strLocIDs = string.Empty;
        int count = 0;
        foreach (GridViewRow gr in gvLocations.Rows)
        {
            CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            Label lblLoc = (Label)gr.FindControl("lblLoc");
            if (chkSelect.Checked == true)
            {
                if (count == 0)
                {
                    strLocIDs += lblLoc.Text;
                }
                else
                {
                    strLocIDs += "," + lblLoc.Text;
                }
                count++;
            }
        }

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.LocIDs = strLocIDs.Trim();
        DataSet ds = new DataSet();

        ds = objBL_Customer.getLocCoordinates(objCustomer);

        //gvOptimized.DataSource = ds.Tables[0];
        //gvOptimized.DataBind();
        rptWaypts.DataSource = ds.Tables[0];
        rptWaypts.DataBind();

        //OptimizeDirections(ds.Tables[0]);
    }

    protected void ddlRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSearchLoc.Text = string.Empty;
        GetLoc(Convert.ToInt32(ddlWorker.SelectedValue));
        ////txtTemplate.Text = string.Empty;
        ////ddlTemplates.SelectedIndex = -1;
        ////txtRemarks.Text = string.Empty;
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "JSON2Marker();", true);
        
    }

    protected void btnSaveRoute_Click(object sender, EventArgs e)
    {
        if (txtTemplate.Text.Trim() == string.Empty)
        {
            return;
        }
        int Mode = 1;
        string Msg = "Updated";
        if (ddlTemplates.SelectedValue == "0" || ddlTemplates.SelectedValue == "-1")
        {
            Mode = 0;
            Msg = "Added";
        }

        //DataTable dtLocChange = new DataTable();
        //dtLocChange.Columns.Add("TemplateID", typeof(int));
        //dtLocChange.Columns.Add("Loc", typeof(int));
        //dtLocChange.Columns.Add("Worker", typeof(int));
        //int worker = 0;
        //foreach (GridViewRow gr in gvLocChanges.Rows)
        //{
        ////    HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
        //    HiddenField hdnWorker = (HiddenField)gr.FindControl("hdnWorker");
        //    worker = Convert.ToInt32(hdnWorker.Value);
        ////    DataRow dr = dtLocChange.NewRow();

        ////    dr["TemplateID"] = Convert.ToInt32(ddlTemplates.SelectedValue);
        ////    dr["Loc"] = Convert.ToInt32(hdnID.Value);
        ////    dr["Worker"] = Convert.ToInt32(hdnWorker.Value);

        ////    dtLocChange.Rows.Add(dr);
        //}        

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Name = txtTemplate.Text.Trim();
        objCustomer.RouteSequence = hdnRouteSeq.Value;
        objCustomer.Remarks = txtRemarks.Text;
        objCustomer.Mode = Mode;
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);
        objCustomer.Worker = Convert.ToInt32(hdnAssignedWorker.Value.Split(';')[0]);
        //objCustomer.DtTemplateData = dtLocChange;
        objCustomer.Center = hdnCenter.Value;
        objCustomer.Radius = hdnRadius.Value;

        try
        {
            int TemplateID = objBL_Customer.AddRouteTemplate(objCustomer);
            GetRouteTemplate();
            ddlTemplates.SelectedValue = TemplateID.ToString();
            hdnEdited.Value = "0";
            lnkSaveTemplate.ImageUrl = "images/saveiconblack.png";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Template " + Msg + " Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void lnkDeleteRoute_Click(object sender, EventArgs e)
    {
        //DataTable dtLocChange = new DataTable();
        //dtLocChange.Columns.Add("TemplateID", typeof(int));
        //dtLocChange.Columns.Add("Loc", typeof(int));
        //dtLocChange.Columns.Add("Worker", typeof(int));

        objCustomer.ConnConfig = Session["config"].ToString();
        objCustomer.Mode = 2;
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);
        //objCustomer.DtTemplateData = dtLocChange;
        try
        {
            objBL_Customer.AddRouteTemplate(objCustomer);
            GetRouteTemplate();
            ddlTemplates.SelectedValue = "0";
            txtTemplate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            gvLocChanges.DataBind();

            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Template Deleted Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', dismissQueue: true, layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        hdnAssignedWorker.Value = lstWorker.SelectedValue + ";" + lstWorker.SelectedItem.Text;
        AssignWorker();
        ModalPopupExtender1.Hide();
        hdnEdited.Value = "1";
        lnkSaveTemplate.ImageUrl = "images/saveicon.png";
    }

    protected void gvWorkers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            txtSearchLoc.Text = string.Empty;
            GetLoc(id);
            ddlWorker.SelectedValue = id.ToString();
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "JSON2Marker();", true);

        }
    }

    protected void ddlTemplates_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTemplates.SelectedValue != "0" && ddlTemplates.SelectedValue != "-1")
        {
            //txtSearchLoc.Text = string.Empty;
            //ddlWorker.SelectedIndex = -1;
            //GetLoc(0);
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);

            GetTemplate();
            hdnEdited.Value = "0";
            lnkSaveTemplate.ImageUrl = "images/saveiconblack.png";
        }
        else
        {
            txtTemplate.Text = string.Empty;
            txtRemarks.Text = string.Empty;
            hdnEdited.Value = "1";
            lnkSaveTemplate.ImageUrl = "images/saveicon.png";
        }

        //GetTemplate();
        //hdnEdited.Value = "0";
        //lnkSaveTemplate.ImageUrl = "images/saveiconblack.png";
    }

    protected void lnkUpdateLocs_Click(object sender, EventArgs e)
    {
        DataTable dtLocChange = new DataTable();
        dtLocChange.Columns.Add("TemplateID", typeof(int));
        dtLocChange.Columns.Add("Loc", typeof(int));
        dtLocChange.Columns.Add("Worker", typeof(int));

        foreach (GridViewRow gr in gvLocChanges.Rows)
        {
            HiddenField hdnID = (HiddenField)gr.FindControl("hdnID");
            HiddenField hdnWorker = (HiddenField)gr.FindControl("hdnWorker");
            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
            DataRow dr = dtLocChange.NewRow();

            dr["TemplateID"] = Convert.ToInt32(ddlTemplates.SelectedValue);
            dr["Loc"] = Convert.ToInt32(hdnID.Value);
            dr["Worker"] = Convert.ToInt32(hdnWorker.Value);

            dtLocChange.Rows.Add(dr);
            //lblCurWorker.Text = lstWorker.SelectedItem.Text;
        }
        if (dtLocChange.Rows.Count > 0)
        {
            objCustomer.ConnConfig = Session["config"].ToString();
            objCustomer.DtTemplateData = dtLocChange;
            objBL_Customer.UpdateLocRoute(objCustomer);
            //GetTemplate();
            txtSearchLoc.Text = string.Empty;
            ddlWorker.SelectedIndex = -1;
            GetLoc(Convert.ToInt32(ddlWorker.SelectedValue));
            GetWorkers();
            AssignWorker();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "noty({text: 'Locations Updated Successfully!',  type : 'success', dismissQueue: true, layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false}); initialize();", true);
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyCircle", "AddCircleLatLng('" + hdnCenter.Value + "','" + hdnRadius.Value + "');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyAlertUpdateLoc", "alert('There are no locations to update.')", true);
        }
    }

    protected void btnSearch_Click(object sender, ImageClickEventArgs e)
    {
        GetLoc(Convert.ToInt32(ddlWorker.SelectedValue));
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "JSON2Marker();", true);

    }

    protected void btnClear_Click(object sender, ImageClickEventArgs e)
    {
        txtSearchLoc.Text = string.Empty;
        ddlWorker.SelectedIndex = -1;
        GetLoc(0);
        ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);
    }   



    private void GetRouteTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        ds = objBL_Customer.getRouteTemplate(objCustomer);
        
        ddlTemplates.DataSource = ds.Tables[0];
        ddlTemplates.DataTextField = "Name";
        ddlTemplates.DataValueField = "templateID";
        ddlTemplates.DataBind();

        //ddlTemplates.Items.Insert(0, new ListItem("- Template -", "-1"));
        ddlTemplates.Items.Insert(0, new ListItem("- Add New -", "0"));

        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        //Dictionary<string, object> row;
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    row = new Dictionary<string, object>();
        //    foreach (DataColumn col in ds.Tables[0].Columns)
        //    {
        //            row.Add(col.ColumnName, dr[col]);
        //    }
        //    rows.Add(row);
        //}
        //hdnTemplateData.Value = serializer.Serialize(rows);
    }

    private void GetTemplate()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        objCustomer.TemplateID = Convert.ToInt32(ddlTemplates.SelectedValue);
        ds = objBL_Customer.getTemplateByID(objCustomer);
        if(ds.Tables[0].Rows.Count>0)
        {
            txtTemplate.Text = ds.Tables[0].Rows[0]["name"].ToString();
            txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
            hdnCenter.Value = ds.Tables[0].Rows[0]["center"].ToString();
            hdnRadius.Value = ds.Tables[0].Rows[0]["radius"].ToString();
            lstWorker.SelectedValue = ds.Tables[0].Rows[0]["worker"].ToString();
            hdnAssignedWorker.Value = ds.Tables[0].Rows[0]["worker"].ToString() + ";" + ds.Tables[0].Rows[0]["workername"].ToString();
            AssignWorker();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyCircle", "AddCircleLatLng('" + hdnCenter.Value + "','" + hdnRadius.Value + "');", true);

            //if (ds.Tables[1].Rows.Count > 0)
            //{
            //    lstWorker.SelectedValue = ds.Tables[1].Rows[0]["workerid"].ToString();

                //AssignWorker();

                //gvLocChanges.DataSource = ds.Tables[1];

                //WorkerChanges(ds.Tables[1]);

            //    ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyCircle", "AddCircleLatLng('" + hdnCenter.Value + "','" + hdnRadius.Value + "');", true);

            //}
            //gvLocChanges.DataBind();
        }
    }

    private void GetLoc(int worker)
    {
        ViewState["pageindex"] = null;
        
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        objCustomer.Worker = worker;
        objCustomer.SearchValue = txtSearchLoc.Text.Trim();
        //objCustomer.Worker = Convert.ToInt32(ddlWorker.SelectedValue);
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        ds = objBL_Customer.getLocCoordinates(objCustomer);
        gvLocations.DataSource = ds.Tables[0];
        gvLocations.DataBind();
        CalculateTotal(ds.Tables[0]);
        gvLocations.PageIndex = 0;
        gvLocations.DataBind();
        //rptMarkers.DataSource = ds.Tables[0];
        //rptMarkers.DataBind();

        //if (!IsPostBack)
        //{
        //    AssignWorkerColor(ds.Tables[0]);
        //}
        //if (!IsPostBack)
        //{
            BindMarkers(ds.Tables[0],false);
        //}

        lblTotalRecLoc.Text = ds.Tables[0].Rows.Count.ToString() + " records found.";
    }

    private void CalculateTotal(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            Label lblTotalMonthBill = (Label)gvLocations.FooterRow.FindControl("lblTotalMonthBill");
            Label lblTotalPeriodBill = (Label)gvLocations.FooterRow.FindControl("lblTotalPeriodBill");
            Label lblGTotalPeriodHours = (Label)gvLocations.FooterRow.FindControl("lblGTotalPeriodHours");
            Label lblTotalMonthlyHours = (Label)gvLocations.FooterRow.FindControl("lblTotalMonthlyHours");
            Label lblTotalUnits = (Label)gvLocations.FooterRow.FindControl("lblUnitTotal");


            double TotalMonthBill = 0.00;
            double TotalPeriodBill = 0.00;
            double GTotalPeriodHours = 0.00;
            double TotalMonthlyHours = 0.00;
            int UnitTotal = 0;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["MonthlyBill"].ToString() != "" && dr["MonthlyBill"] != DBNull.Value)
                {
                    TotalMonthBill += Convert.ToDouble(dr["MonthlyBill"]);
                }
                if (dr["bamt"].ToString() != "" && dr["bamt"] != DBNull.Value)
                {
                    TotalPeriodBill += Convert.ToDouble(dr["bamt"]);
                }
                if (dr["monthlyhours"].ToString() != "" && dr["monthlyhours"] != DBNull.Value)
                {
                    TotalMonthlyHours += Convert.ToDouble(dr["monthlyhours"]);
                }
                if (dr["Hours"].ToString() != "" && dr["Hours"] != DBNull.Value)
                {
                    GTotalPeriodHours += Convert.ToDouble(dr["Hours"]);
                }
                if (dr["elevcount"].ToString() != "" && dr["elevcount"] != DBNull.Value)
                {
                    UnitTotal += Convert.ToInt32(dr["elevcount"]);
                }
            }

            lblTotalMonthBill.Text = string.Format("{0:c}", TotalMonthBill);
            lblTotalPeriodBill.Text = string.Format("{0:c}", TotalPeriodBill);
            lblGTotalPeriodHours.Text = string.Format("{0:n}", GTotalPeriodHours);
            lblTotalMonthlyHours.Text = string.Format("{0:n}", TotalMonthlyHours);
            lblTotalUnits.Text = UnitTotal.ToString();
        }
    }

    private void CalculateWorkerTotal(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            Label lblContractsTotal = (Label)gvWorkers.FooterRow.FindControl("lblContractsTotal");
            Label lblUnitTotal = (Label)gvWorkers.FooterRow.FindControl("lblUnitTotal");
            Label lblHoursTotal = (Label)gvWorkers.FooterRow.FindControl("lblHoursTotal");
            Label lblAmountTotal = (Label)gvWorkers.FooterRow.FindControl("lblAmountTotal");

            int ContractsTotal = 0;
            int UnitTotal = 0;
            double HoursTotal = 0.00;
            double AmountTotal = 0.00;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["contracts"].ToString() != "" && dr["contracts"] != DBNull.Value)
                {
                    ContractsTotal += Convert.ToInt32(dr["contracts"]);
                }
                if (dr["units"].ToString() != "" && dr["units"] != DBNull.Value)
                {
                    UnitTotal += Convert.ToInt32(dr["units"]);
                }
                if (dr["Hour"].ToString() != "" && dr["Hour"] != DBNull.Value)
                {
                    HoursTotal += Convert.ToDouble(dr["Hour"]);
                }
                if (dr["Amount"].ToString() != "" && dr["Amount"] != DBNull.Value)
                {
                    AmountTotal += Convert.ToDouble(dr["Amount"]);
                }
            }

            lblContractsTotal.Text = ContractsTotal.ToString();
            lblUnitTotal.Text = UnitTotal.ToString();
            lblHoursTotal.Text = string.Format("{0:n}", HoursTotal);
            lblAmountTotal.Text = string.Format("{0:c}", AmountTotal);
        }
    }
    
    private void GetWorkers()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet ds = new DataSet();
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        ds = objBL_Customer.getWorkers(objCustomer);
        gvWorkers.DataSource = ds.Tables[0];
        gvWorkers.DataBind();
        CalculateWorkerTotal(ds.Tables[0]);
        lblTotalRecWork.Text = ds.Tables[0].Rows.Count.ToString() + " records found.";
    }

    private void BindMarkers(DataTable dt, bool assignedOnly)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (DataRow dr in dt.Rows)
        {
            //if (dr["lat"] != DBNull.Value && dr["lat"].ToString().Trim() != string.Empty)
            //{
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.ColumnName == "title" || col.ColumnName == "lat" || col.ColumnName == "lng" || col.ColumnName == "description" || col.ColumnName == "worker")
                        row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            //}
        }
        string serialised = serializer.Serialize(rows);
        if (!assignedOnly)
            hdnMarkers.Value = serialised;
        else
            hdnAssignedMarkers.Value = serialised;
    }

    private void AssignWorkerColor(DataTable dtLocation)
    {
        List<string> lst = new List<string>();
        foreach (DataRow dr in dtLocation.Rows)
        {
            lst.Add(dr["name"].ToString());
        }

        var workers =
               (from worker in lst
                select worker).Distinct().ToList();

        WorkerColor(workers);
    }    

    private void WorkerColor(List<string> lst)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
        Dictionary<string, object> row;
        foreach (string str in lst)
        {
            row = new Dictionary<string, object>();

            row.Add("worker", str);
            row.Add("color", RandomColor());

            rows.Add(row);
        }
        hdnColor.Value = serializer.Serialize(rows);
        ViewState["workercol"] = rows;
    }
        
     public string getWorkerColor(object worker) {
         
         List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>((List<Dictionary<string, object>>)ViewState["workercol"]);
         string color=string.Empty;
         foreach (Dictionary<string, object> dict in rows)
         {
             if (dict["worker"].ToString().Trim().ToLower() == worker.ToString().Trim().ToLower())
             {
                 color = dict["color"].ToString();
             }
         }

        string url = "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=|" + color + "|ffffff";
        return url;
    }

    private string RandomColor()
    {
        string color = String.Format("{0:X6}", randomGen.Next(0x1000000));
        return color;
    }

    private void FillRoute()
    {
        DataSet ds = new DataSet();
        objPropUser.ConnConfig = Session["config"].ToString();
        ds = objBL_User.getRoute(objPropUser);

        ddlWorker.DataSource = ds.Tables[0];
        ddlWorker.DataTextField = "Name";
        ddlWorker.DataValueField = "ID";
        ddlWorker.DataBind();

        ddlWorker.Items.Insert(0, new ListItem("- Worker -", "0"));
        ddlWorker.Items.Insert(1, new ListItem("- All -", "0"));

        lstWorker.DataSource = ds.Tables[0];
        lstWorker.DataTextField = "Name";
        lstWorker.DataValueField = "ID";
        lstWorker.DataBind();

        if (!IsPostBack)
        {
            AssignWorkerColor(ds.Tables[0]);
        }
    }

    private void AssignWorker()
    {
        objCustomer.ConnConfig = Session["config"].ToString();
        //objCustomer.Worker =Convert.ToInt32( ddlWorker.SelectedValue);
        //objCustomer.SearchValue = txtSearchLoc.Text.Trim();
        objCustomer.SearchValue = string.Empty;
        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        DataSet ds = new DataSet();
        
        ds = objBL_Customer.getLocCoordinates(objCustomer);

        double lat1 = Convert.ToDouble(hdnCenter.Value.Split(',')[0]);
        double long1 = Convert.ToDouble(hdnCenter.Value.Split(',')[1]);
        double radius = Convert.ToDouble(hdnRadius.Value) / 1000;

        List<int> lstLoc = new List<int>();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr["lat"] != DBNull.Value)
            {
                if (dr["lat"].ToString().Trim() != string.Empty)
                {
                    double lat2 = Convert.ToDouble(dr["lat"]);
                    double long2 = Convert.ToDouble(dr["lng"]);
                    double dist = Distance(lat1, long1, lat2, long2);
                    if (dist <= radius)
                    {
                        lstLoc.Add(Convert.ToInt32(dr["loc"]));
                    }
                }
            }
        }

        //DataTable dtWorkerchanges = new DataTable();
        //dtWorkerchanges.Columns.Add("Route", typeof(int));
        //dtWorkerchanges.Columns.Add("Loc", typeof(int));

        DataTable dtChanges = ds.Tables[0].Clone();

        foreach (int loc in lstLoc)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (dr["loc"].ToString() == loc.ToString())
                {
                    dtChanges.ImportRow(dr);
                }
            }

            //DataRow drwork = dtWorkerchanges.NewRow();
            //drwork["Route"] =Convert.ToInt32( lstWorker.SelectedValue);
            //drwork["Loc"] = loc;
            //dtWorkerchanges.Rows.Add(drwork);
        }

        gvLocChanges.DataSource = dtChanges;
        gvLocChanges.DataBind();

        BindMarkers(dtChanges, true);

        WorkerChanges(dtChanges);

        //DataSet dsWorker = new DataSet();
        //objCustomer.ConnConfig = Session["config"].ToString();
        //objCustomer.dtWorkerData = dtWorkerchanges;
        //dsWorker = objBL_Customer.GetWorkerCalculations(objCustomer);
    }

    private void WorkerChanges(DataTable dt)
    {
        DataTable dtWorker = new DataTable();
        dtWorker.Columns.Add("Worker");
        DataRow drAssignedWor = dtWorker.NewRow();
        //drAssignedWor["Worker"] = lstWorker.SelectedItem.Text;
        drAssignedWor["Worker"] = hdnAssignedWorker.Value.Split(';')[1];

        dtWorker.Rows.Add(drAssignedWor);

        double totalhours = 0;
        double totalamt = 0;
        int totalcontract = 0;
        int totalunits = 0;
        foreach (DataRow dr in dt.Rows)
        {
            DataRow drWor = dtWorker.NewRow();
            drWor["Worker"] = dr["worker"];
            dtWorker.Rows.Add(drWor);

            if (dr["worker"].ToString() != hdnAssignedWorker.Value.Split(';')[1])
            {
                totalhours += Convert.ToDouble(dr["MonthlyHours"]);
                totalamt += Convert.ToDouble(dr["MonthlyBill"]);
                totalunits += Convert.ToInt32(dr["elevcount"]);
                totalcontract++;
            }
        }

        var distinctValues = (from row in dtWorker.AsEnumerable()
                              select row.Field<string>("Worker"))
                             .Distinct().ToArray();

        string strWorker = string.Join(",", Array.ConvertAll(distinctValues, x => SurroundWith(x.ToString(), "'")));

        if (Session["MSM"].ToString() == "TS")
        {
            objCustomer.Status = 1;
        }
        else
        {
            objCustomer.Status = 0;
        }
        objCustomer.Name = strWorker;
        objCustomer.ConnConfig = Session["config"].ToString();
        DataSet dsWorker = new DataSet();
        dsWorker = objBL_Customer.getWorkerMonthly(objCustomer);

        gvWorkerChanges.DataSource = dsWorker.Tables[0];
        gvWorkerChanges.DataBind();

        foreach (GridViewRow gr in gvWorkerChanges.Rows)
        {
            Label lblNewamt = (Label)gr.FindControl("lblNewamt");
            Label lblNewHours = (Label)gr.FindControl("lblNewHours");
            Label lblCurWorker = (Label)gr.FindControl("lblCurWorker");
            Label lblHours = (Label)gr.FindControl("lblHours");
            HiddenField lblAmt = (HiddenField)gr.FindControl("hdnAmt");
            Label lblContr = (Label)gr.FindControl("lblContr");
            Label lblUnits = (Label)gr.FindControl("lblUnits");
            Label lblNewContr = (Label)gr.FindControl("lblNewContr");
            Label lblNewUnits = (Label)gr.FindControl("lblNewUnits");

            double hours = 0;
            double totalWorkerhours = 0;
            double amount = 0;
            double totalWorkeramount = 0;
            int contr = 0;
            int totalWorkercontr = 0;
            int units = 0;
            int totalWorkerunits = 0;
            if (lblHours.Text.Trim() != string.Empty)
            {
                hours = Convert.ToDouble(lblHours.Text);
            }
            if (lblAmt.Value.Trim() != string.Empty)
            {
                amount = Convert.ToDouble(lblAmt.Value);
            }
            if (lblContr.Text.Trim() != string.Empty)
            {
                contr = Convert.ToInt32(lblContr.Text);
            }
            if (lblUnits.Text.Trim() != string.Empty)
            {
                units = Convert.ToInt32(lblUnits.Text);
            }
            if (hdnAssignedWorker.Value.Split(';')[1] == lblCurWorker.Text)
            {
                totalWorkerhours = hours + totalhours;
                lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                totalWorkeramount = amount + totalamt;
                lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                totalWorkercontr = contr + totalcontract;
                lblNewContr.Text = totalWorkercontr.ToString();

                totalWorkerunits = units + totalunits;
                lblNewUnits.Text = totalWorkerunits.ToString();
            }
            else
            {
                totalhours = 0;
                totalamt = 0;
                totalcontract = 0;
                totalunits = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["Worker"].ToString() == lblCurWorker.Text)
                    {
                        if (dr["MonthlyHours"].ToString().Trim() != string.Empty)
                            totalhours += Convert.ToDouble(dr["MonthlyHours"]);

                        if (dr["MonthlyBill"].ToString().Trim() != string.Empty)
                            totalamt += Convert.ToDouble(dr["MonthlyBill"]);

                        if (dr["elevcount"].ToString().Trim() != string.Empty)
                            totalunits += Convert.ToInt32(dr["elevcount"]);

                        totalcontract++;
                    }
                }
                totalWorkerhours = hours - totalhours;
                lblNewHours.Text = string.Format("{0:n}", totalWorkerhours);

                totalWorkeramount = amount - totalamt;
                lblNewamt.Text = string.Format("{0:c}", totalWorkeramount);

                totalWorkercontr = contr - totalcontract;
                lblNewContr.Text = totalWorkercontr.ToString();

                totalWorkerunits = units - totalunits;
                lblNewUnits.Text = totalWorkerunits.ToString();
            }
        }
    }

    private string SurroundWith(string text, string ends)
    {
        return ends + text + ends;
    }
    private string SurroundWith(string text, string starts, string ends)
    {
        return starts + text + ends;
    }


    /// <summary>
    /// Route Optimization Code 
    /// </summary>
    /// <param name="dt"></param>
    #region Route Optimization Algorithm

    private void OptimizeDirections(DataTable dt)
    {
        //objCustomer.ConnConfig = Session["config"].ToString();
        //DataSet ds = new DataSet();
        //ds = objBL_Customer.getLocCoordinates(objCustomer);

        string concatString = ConcatString(dt);
        string input = GetDistanceMatrix(concatString, concatString);
        var deserializer = new JavaScriptSerializer();
        RootObject distanceMatrix = deserializer.Deserialize<RootObject>(input);
        //RootObject distanceMatrix = getDistanceMatrixFromCode(dt);

        int nodeCount = distanceMatrix.rows.Count;
        //int nodeCount = distanceMatrix.rows.Length;
        List<short> arrayOfNodes = new List<short>();
        //for (short i = 1; i < nodeCount; i++)/* from A to any point*/
        for (short i = 1; i < nodeCount - 1; i++) /*for A to B*/
        {
            arrayOfNodes.Add(i);
        }
        List<List<short>> combinations = GenerateAllCombinations(arrayOfNodes);

        double shortestPath = double.MaxValue;
        //int shortestPath = Int32.MaxValue;
        List<short> shortestPathCombination = null;
        foreach (List<short> combination in combinations)
        {
            combination.Insert(0, 0);
            combination.Insert(nodeCount - 1, (short)(nodeCount - 1));/*for A to B*/

            double pathLength = GetPathLength(combination, distanceMatrix);
            if (pathLength < shortestPath)
            {
                shortestPathCombination = combination;
                shortestPath = pathLength;
            }
        }

        DataTable dtFinal = dt.Clone();
        foreach (var li in shortestPathCombination)
        {
            dtFinal.ImportRow(dt.Rows[li]);
        }

        //gvOptimized.DataSource = dtFinal;
        //gvOptimized.DataBind();
        rptWaypts.DataSource = dtFinal;
        rptWaypts.DataBind();
    }

    private string ConcatString(DataTable dt)
    {
        var concatString = "";
        int inc = 0;
        foreach (DataRow dr in dt.Rows)
        {
            concatString += dr["coordinates"].ToString();

            if (inc != dt.Rows.Count - 1)
            {
                concatString += "|";
            }

            inc++;
        }
        return concatString;
    }

    public string GetDistanceMatrix(string origins, string destinations)
    {
        WebRequest request = WebRequest.Create("http://maps.googleapis.com/maps/api/distancematrix/json?origins=" + origins + "&destinations=" + destinations + "&sensor=false&key=AIzaSyDedmuEC-1d__Jc1M3OLx1NIAnc_gMRbhE");
        request.Method = "GET";
        var response = request.GetResponse();
        string result;
        using (var stream = response.GetResponseStream())
        {
            using (var reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
        }
        //JavaScriptSerializer serializer = new JavaScriptSerializer();
        //return serializer.Deserialize<GeoJsonData>(result);
        return result;
    }

    private List<List<T>> GenerateAllCombinations<T>(List<T> array)
    {
        List<List<T>> allLists = new List<List<T>>();
        List<T> initialList = new List<T>();
        initialList.Add(array[0]);
        allLists.Add(initialList);

        //i is the next element to be inserted.  
        for (short i = 1; i < array.Count; i++)
        {
            List<List<T>> newAllLists = new List<List<T>>();

            foreach (List<T> existingList in allLists)
            {
                List<List<T>> tempAllLists = new List<List<T>>();
                for (int j = 0; j <= existingList.Count; j++)
                {
                    List<T> newList = new List<T>();
                    newList.AddRange(existingList);
                    newList.Insert(j, array[i]);
                    tempAllLists.Add(newList);
                }
                newAllLists.AddRange(tempAllLists);
                allLists = newAllLists;
            }

            GC.Collect();
        }
        return allLists;
    }

    private double GetPathLength(List<short> combination, RootObject distanceMatrix)
    {
        double length = 0;

        //for (int i = 0; i < combination.Count; i++)
        for (int i = 0; i < combination.Count - 1; i++)/*from A to any point*/
        {
            int source = combination[i];
            int destination = i + 1;
            destination = combination[i + 1];/*for A to any point*/
            length = length + distanceMatrix.rows[source].elements[destination].distance.value;

                        
            //if (destination == combination.Count)
            //{
            //    destination = combination[0];
            //}
            //else
            //{
            //    destination = combination[i + 1];
            //}
            
            //length = length + distanceMatrix.rows[source].elements[destination].distance.value;
           


            //int source = combination[i];
            ////int destination = -1;
            ////int inc = i + 1;
            //int destination = i + 1;
            //if (destination == combination.Count)
            ////if (destination == combination.Count - 1)
            //{
            //    destination = combination[0];
            //}
            ////else
            ////    if (inc < combination.Count)
            ////    {
            ////        destination = combination[inc];
            ////    }
            //if (destination != -1)
            //{
            //length = length + distanceMatrix.rows[source].elements[destination].distance.value;
            //}
            
        }

        return length;
    }

    /// <summary>
    /// Generate Distance Matrix for coordinates by mathematical claculations.
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    #region Generate Distance Matrix programatically
    private RootObject getDistanceMatrixFromCode(DataTable dt)
    {
        RootObject ro = new RootObject();
        List<Row> rowsList = new List<Row>();
        foreach (DataRow dr in dt.Rows)
        {
            Row r = new Row();
            List<Element> eleList = new List<Element>();
            foreach (DataRow drin in dt.Rows)
            {
                Element el = new Element();
                Distance dist = new Distance();
                double dis = Distance(Convert.ToDouble(dr["lat"].ToString()), Convert.ToDouble(dr["lng"].ToString()), Convert.ToDouble(drin["lat"].ToString()), Convert.ToDouble(drin["lng"].ToString()));
                //dist.value = Convert.ToInt32(Math.Round(dis));
                dist.value = dis;
                el.distance = dist;
                eleList.Add(el);
            }
            r.elements = eleList;
            rowsList.Add(r);
        }
        ro.rows = rowsList;

        return ro;
    }

    public double Distance(double Latitude1, double Longitude1, double Latitude2, double Longitude2)
    {
        //1- miles
        //double R = (type == 1) ? 3960 : 6371;          // R is earth radius.
        double R = 6371;  
        double dLat = this.toRadian(Latitude2 - Latitude1);
        double dLon = this.toRadian(Longitude2 - Longitude1);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(this.toRadian(Latitude1)) * Math.Cos(this.toRadian(Latitude2)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
        double d = R * c;

        return d;
    }

    private double toRadian(double val)
    {
        return (Math.PI / 180) * val;
    }
 
    #endregion   

    #endregion


    protected void gvLocations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {        
        GetLoc(Convert.ToInt32(ddlWorker.SelectedValue));
        ViewState["pageindex"] = e.NewPageIndex;
        //ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySuccUp", "initialize();", true);
        gvLocations.PageIndex = e.NewPageIndex;
        gvLocations.DataBind();
    }
}
