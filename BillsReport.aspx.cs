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

public partial class BillsReport : System.Web.UI.Page
{
    #region Variables
    PJ _objPJ = new PJ();
    BL_Bills _objBLBills = new BL_Bills();
    #endregion

    #region Events

    #region PAGELOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (!IsPostBack)
        {
            DateTime _now = DateTime.Now;
            var _startDate = new DateTime(_now.Year, _now.Month, 1);
            var _endDate = _startDate.AddMonths(1).AddDays(-1);
            txtStartDate.Text = _startDate.ToShortDateString();
            txtEndDate.Text = _endDate.ToShortDateString();

            GetBillsReport();
            txtSearchDate.Visible = false;
        }
    }
    #endregion

    protected void rvBills_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetBillsReport();
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
                mail.Title = "AP Bills Report";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the Bill Report attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "BillsList.pdf";

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
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetBillsReport();
    }
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
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

        GetBillsReport();
    }
    #endregion

    #region Custom Functions
    private void GetBillsReport()
    {
        try
        {
            _objPJ.ConnConfig = Session["config"].ToString();
            _objPJ.StartDate = Convert.ToDateTime(txtStartDate.Text);
            _objPJ.EndDate = Convert.ToDateTime(txtEndDate.Text);
            _objPJ.SearchValue = Convert.ToInt16(ddlInvoice.SelectedValue);
      
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
                }
            }

            DataSet _ds = _objBLBills.GetAllPJDetails(_objPJ);

            ReportParameter rpStartDate = new ReportParameter("paramStartDate", _objPJ.StartDate.ToShortDateString());
            ReportParameter rpEndDate = new ReportParameter("paramEndDate", _objPJ.EndDate.ToShortDateString());

            rvBills.LocalReport.DataSources.Clear();
            rvBills.LocalReport.DataSources.Add(new ReportDataSource("dsBills", _ds.Tables[0]));
            rvBills.LocalReport.ReportPath = "Reports/APInvoiceList.rdlc";
            rvBills.LocalReport.EnableExternalImages = true;
            rvBills.LocalReport.SetParameters(new ReportParameter[] { rpStartDate, rpEndDate });
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
    #endregion
    
}