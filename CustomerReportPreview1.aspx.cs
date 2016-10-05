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
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

public partial class CustomerReportPreview : System.Web.UI.Page
{
    CustomerReport objCustReport = new CustomerReport();
    BL_ReportsData objBL_ReportsData = new BL_ReportsData();
    BL_User objBL_User = new BL_User();
    BusinessEntity.User objProp_User = new BusinessEntity.User();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                //if (Session["DsGetCustomerDetails"] != null && Session["ReportId"] != null)
                //{
                //    GeneratePdfTable((DataSet)Session["DsGetCustomerDetails"]);
                //}
                //if (Session["Query"] != null)
                //{
                //    GetGridData();
                //}

                ShowFile(Session["FilePath"].ToString());
            }
            catch (Exception exp)
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
            }
        }
    }

    private void GetGridData()
    {
        try
        {
            DataSet dsGetCustDetails = new DataSet();
            objProp_User.DBName = Session["dbname"].ToString();
            objProp_User.ConnConfig = Session["config"].ToString();
            dsGetCustDetails = objBL_ReportsData.GetOwners(Session["Query"].ToString(), objProp_User);


            GeneratePdfTable(dsGetCustDetails);


        }
        catch (Exception exp)
        {
            ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: '" + exp.Message.ToString() + "',  type : 'error', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
        }
    }

    private void GeneratePdfTable(DataSet dsGetCustDetails)
    {

        objCustReport.DBName = Session["dbname"].ToString();
        objCustReport.ConnConfig = Session["config"].ToString();

        int[] getColumnsWidth = new int[dsGetCustDetails.Tables[0].Columns.Count];
        int countTotalWidth = 0;
        DataSet dsGetColumnWidth = new DataSet();
        objCustReport.ReportId = Convert.ToInt32(Session["ReportId"]);
        dsGetColumnWidth = objBL_ReportsData.GetColumnWidthByReportId(objCustReport);
        //  hdnColumnWidth.Value = "";
        if (dsGetColumnWidth.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i <= dsGetColumnWidth.Tables[0].Rows.Count - 1; i++)
            {
                //hdnColumnWidth.Value += dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString() + ",";
                getColumnsWidth[i] = Convert.ToInt32(dsGetColumnWidth.Tables[0].Rows[i]["ColumnWidth"].ToString().Replace("px", ""));
                countTotalWidth = countTotalWidth + getColumnsWidth[i];
            }

            //  hdnColumnWidth.Value = hdnColumnWidth.Value.Replace("px", "").TrimEnd(',');

        }

        //server folder path which is stored your PDF documents        
        //  string path = Server.MapPath("PDF-Files");
        //  string filename = path + "/Doc1.pdf";

        string filePath = Session["FilePath"].ToString();

        //Create new PDF document 
        Rectangle rec = PageSize.A4;
        Document document = new Document(PageSize.A4, 20f, 20f, 20f, 20f);

        try
        {
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create, FileAccess.Write));
            PdfPTable headerTable = new PdfPTable(2);
            PdfPTable tblDateTime = new PdfPTable(1);
            DataSet dsGetHeaderFooterDetail = new DataSet();
            dsGetHeaderFooterDetail = objBL_ReportsData.GetHeaderFooterDetail(objCustReport);
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
                    tblCompDetails.AddCell(companyName);

                    PdfPCell compAddress = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Address"].ToString(), compStyle));
                    compAddress.Border = 0;
                    compAddress.PaddingTop = 10;
                    tblCompDetails.AddCell(compAddress);

                    PdfPCell compEmail = new PdfPCell(new Phrase(dsCompDetail.Tables[0].Rows[0]["Email"].ToString(), compStyle));
                    compEmail.Border = 0;
                    compEmail.PaddingTop = 10;
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
                tblCompDetails2.HorizontalAlignment = 0;
                tblCompDetails2.SpacingBefore = 20;
                tblCompDetails2.SpacingAfter = 20;
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
        }
        catch (Exception ex)
        {
            document.Close();
        }
        finally
        {
            document.Close();
            ShowFile(filePath);
        }
    }

    public void ShowFile(string filename)
    {
        //Clears all content output from Buffer Stream
        Response.ClearContent();
        //Clears all headers from Buffer Stream
        Response.ClearHeaders();
        //Adds an HTTP header to the output stream
        Response.AddHeader("Content-Disposition", "inline;filename=" + filename);
        //Gets or Sets the HTTP MIME type of the output stream
        if (filename.Contains(".xls"))
        {
            Response.ContentType = "application/xls";
        }
        else
        {
            Response.ContentType = "application/pdf";
        }
        //Writes the content of the specified file directory to an HTTP response output stream as a file block
        Response.WriteFile(filename);
        //sends all currently buffered output to the client
        Response.Flush();
        //Clears all content output from Buffer Stream
        Response.Clear();
    }

}
