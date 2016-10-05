using BusinessEntity;
using BusinessLayer;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class APAgingReport : System.Web.UI.Page
{
    #region Variables
    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();

    User _objUser = new User();
    BL_User _objBLUser = new BL_User();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            GetAPAgingReport();
            txtSearchDate.Visible = false;
        }
    }
    protected void hideModalPopupViaServerConfirm_Click(object sender, EventArgs e)
    {
        if (txtTo.Text.Trim() != string.Empty)
        {
            try
            {
                Mail mail = new Mail();
                mail.From = txtFrom.Text.Trim();
                mail.To = txtTo.Text.Split(';', ',').OfType<string>().ToList();
                if (txtCC.Text.Trim() != string.Empty)
                {
                    mail.Cc = txtCC.Text.Split(';', ',').OfType<string>().ToList();
                }
                mail.Title = "AP Aging Report By Due Date";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the AP Aging Report By Due Date attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "APAging.pdf";

                mail.DeleteFilesAfterSend = true;
                mail.RequireAutentication = false;

                mail.Send();
                //this.programmaticModalPopup.Hide();
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keySucc", "noty({text: 'Mail sent successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);

            }
            catch (Exception ex)
            {
                string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
                ScriptManager.RegisterStartupScript(this, Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);

            }
        }
    }
    protected void rvBills_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetAPAgingReport();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    private void GetAPAgingReport()
    {
        try
        {
            _objPJ.ConnConfig = Session["config"].ToString();
            //_objPJ.StartDate = Convert.ToDateTime(txtStartDate.Text);
            //_objPJ.EndDate = Convert.ToDateTime(txtEndDate.Text);
            //_objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);

            //if (_objPJ.SearchValue.Equals(2))
            //{
            //    if (string.IsNullOrEmpty(txtSearchDate.Text))
            //    {
            //        _objPJ.SearchDate = DateTime.Now;
            //        txtSearchDate.Text = DateTime.Now.ToShortDateString();
            //    }
            //    else
            //    {
            //        _objPJ.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
            //    }
            //}
            DataSet dsC = new DataSet();
            _objUser.ConnConfig = Session["config"].ToString();
            dsC = _objBLUser.getControl(_objUser);
            if (dsC.Tables[0].Rows.Count > 0)
            {
                if (Session["MSM"].ToString() != "TS")
                {
                    if (txtFrom.Text.Trim() == string.Empty)
                    {
                        txtFrom.Text = dsC.Tables[0].Rows[0]["Email"].ToString();
                    }
                }

                string address = dsC.Tables[0].Rows[0]["name"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["Address"].ToString() + Environment.NewLine;
                address += dsC.Tables[0].Rows[0]["city"].ToString() + ", " + dsC.Tables[0].Rows[0]["state"].ToString() + ", " + dsC.Tables[0].Rows[0]["zip"].ToString() + Environment.NewLine;
                address += "Phone: " + dsC.Tables[0].Rows[0]["Phone"].ToString() + Environment.NewLine;
                address += "Fax: " + dsC.Tables[0].Rows[0]["fax"].ToString() + Environment.NewLine;
                address += "Email: " + dsC.Tables[0].Rows[0]["email"].ToString() + Environment.NewLine;
                address = "Please review the attached invoice from: " + Environment.NewLine + Environment.NewLine + address;
                ViewState["company"] = address;
                txtBody.Text = address;
            }
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
            DateTime _dueDate = DateTime.Now;
            if (_objPJ.SearchValue.Equals(2))
            {
                if (string.IsNullOrEmpty(txtSearchDate.Text))
                {
                    _objPJ.SearchDate = DateTime.Now;
                    txtSearchDate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    _objPJ.SearchDate = Convert.ToDateTime(txtSearchDate.Text);
                    _dueDate = _objPJ.SearchDate;
                }
            }
            
            DataSet _ds = _objBLBills.GetBillsDetailsByDue(_objPJ);


            //ReportParameter rpStartDate = new ReportParameter("paramStartDate", _objPJ.StartDate.ToShortDateString());
            int valExpCol = 0;
            if(rdExpandAll.Checked.Equals(true))
            {
                valExpCol = 1;
            }
            rvBills.LocalReport.DataSources.Clear();
            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpExpColl = new ReportParameter("paramExpCllAll", valExpCol.ToString());
            ReportParameter rpDueDate = new ReportParameter("paramDueDate", _dueDate.ToString());

            rvBills.LocalReport.DataSources.Add(new ReportDataSource("dsAPAging", _ds.Tables[0]));
            rvBills.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvBills.LocalReport.ReportPath = "Reports/APAging.rdlc";
            rvBills.LocalReport.EnableExternalImages = true;
            rvBills.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpExpColl, rpDueDate }));
            //rvBills.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
            rvBills.LocalReport.Refresh();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
    private byte[] ExportReportToPDF(string reportName)
    {
        Warning[] warnings;
        string[] streamids;
        string mimeType;
        string encoding;
        string filenameExtension;
        byte[] bytes = rvBills.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetAPAgingReport();
    }
    //protected void rdExpandAll_CheckedChanged(object sender, EventArgs e)
    //{
    //    GetAPAgingReport();
    //}
    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        GetAPAgingReport();
    }
    protected void ddlInvoice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlInvoice.SelectedValue.Equals("2"))
            txtSearchDate.Visible = true;
        else
        {
            txtSearchDate.Visible = false;
            txtSearchDate.Text = "";
        }

        GetAPAgingReport();
    }
}