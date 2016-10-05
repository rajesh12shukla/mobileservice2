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

public partial class ARAgingReport : System.Web.UI.Page
{
    #region Variables
    Contracts objContract = new Contracts();
    BL_Contracts objBLContracts = new BL_Contracts();

    User _objUser = new User();
    BL_User _objBLUser = new BL_User();

    #endregion

    #region events
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

            GetARAgingReport();
            //txtSearchDate.Visible = false;
        }
    }
    #endregion
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }
    protected void lnkSearch_Click(object sender, EventArgs e)
    {
        GetARAgingReport();
    }
    protected void rvInvoices_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
    {
        GetARAgingReport();
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
                mail.Title = "AR Aging Report By Due Date";
                if (txtBody.Text.Trim() != string.Empty)
                {
                    mail.Text = txtBody.Text.Replace(Environment.NewLine, "<BR/>");
                }
                else
                {
                    mail.Text = "This is report email sent from Mobile Office Manager. Please find the AR Aging Report By Due Date attached.";
                }
                //mail.AttachmentFiles.Add(ExportReportToPDF("Report_" + objGen.generateRandomString(10) + ".pdf"));
                mail.attachmentBytes = ExportReportToPDF("");
                mail.FileName = "ARAging.pdf";

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
    protected void rdExpCollAll_CheckedChanged(object sender, EventArgs e)
    {
        GetARAgingReport();
    }
    #endregion

    #region Custom function
    private void GetARAgingReport()
    {
        try
        {
            objContract.ConnConfig = Session["config"].ToString();
            objContract.StartDate = Convert.ToDateTime(txtStartDate.Text);
            objContract.EndDate = Convert.ToDateTime(txtEndDate.Text);
            bool IsCustomer = false;
            if (Request.QueryString["uid"] != null)
            {
                objContract.SearchBy = "l.owner";
                objContract.SearchValue = Request.QueryString["uid"].ToString();
                IsCustomer = true;
            }
            DataSet _ds = objBLContracts.GetARInvoices(objContract);
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

            //ReportParameter rpStartDate = new ReportParameter("paramStartDate", objContract.StartDate.ToShortDateString());
            //ReportParameter rpEndDate = new ReportParameter("paramEndDate", objContract.EndDate.ToShortDateString());
            int valExpCol = 0;
            if (rdExpandAll.Checked.Equals(true))
            {
                valExpCol = 1;
            }

            ReportParameter rpUser = new ReportParameter("paramUser", Session["User"].ToString());
            ReportParameter rpExpColl = new ReportParameter("paramExpCllAll", valExpCol.ToString());
            ReportParameter rpByCust = new ReportParameter("paramByCustomer", IsCustomer.ToString());
            rvInvoices.LocalReport.DataSources.Clear();
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dsARAging", _ds.Tables[0]));
            rvInvoices.LocalReport.DataSources.Add(new ReportDataSource("dsCompany", dsC.Tables[0]));
            rvInvoices.LocalReport.ReportPath = "Reports/ARAging.rdlc";
            rvInvoices.LocalReport.EnableExternalImages = true;
            rvInvoices.LocalReport.SetParameters((new ReportParameter[] { rpUser, rpExpColl, rpByCust }));
            rvInvoices.LocalReport.Refresh();
            
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
        byte[] bytes = rvInvoices.LocalReport.Render(
            "PDF", null, out mimeType, out encoding, out filenameExtension,
             out streamids, out warnings);

        return bytes;
    }
    #endregion

}