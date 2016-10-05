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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

public partial class ControlPanel : System.Web.UI.Page
{
    User objPropUser = new User();
    BL_User objBL_User = new BL_User();

    General _objPropGeneral = new General();
    BL_General _objBLGeneral = new BL_General();

    Chart _objChart = new Chart();
    BL_Chart _objBLChart = new BL_Chart();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("login.aspx");
        }

        if (!IsPostBack)
        {
            FillYearEnd();
            GetBillcodesforTimeSheet();
            DataSet ds = new DataSet();
            objPropUser.ConnConfig = Session["config"].ToString();
            ds = objBL_User.getControl(objPropUser);
           
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["YE"].ToString()))
                {
                    ddlYearEnd.SelectedValue = ds.Tables[0].Rows[0]["YE"].ToString();
                }
                if(!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["GSTreg"].ToString()))    //change by dev for sales tax on 3rd feb, 16
                {
                    txtGSTReg.Text = ds.Tables[0].Rows[0]["GSTreg"].ToString();
                }
                _objPropGeneral.ConnConfig = Session["config"].ToString();
                DataSet _dsCustom = _objBLGeneral.getCustomFieldsControl(_objPropGeneral);

                if (_dsCustom.Tables[0].Rows.Count > 0)
                {
                    foreach(DataRow _dr in _dsCustom.Tables[0].Rows)
                    {
                        if(_dr["Name"].ToString().Equals("Country"))
                        {
                            ddlCountry.SelectedValue = _dr["Label"].ToString();
                        }
                        else if(_dr["Name"].ToString().Equals("GSTGL"))
                        {
                            if(!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                _objChart.ConnConfig = Session["config"].ToString();
                                _objChart.ID = Convert.ToInt32(_dr["Label"].ToString());
                                DataSet _dsChart = _objBLChart.GetChart(_objChart);

                                if(_dsChart.Tables[0].Rows.Count > 0)
                                {
                                    txtGSTGL.Text = _dsChart.Tables[0].Rows[0]["fDesc"].ToString();
                                    hdnGSTGL.Value = _dr["Label"].ToString();
                                }
                                
                            }
                        }
                        else if(_dr["Name"].ToString().Equals("GSTRate"))
                        {
                            if (!string.IsNullOrEmpty(_dr["Label"].ToString()))
                            {
                                txtGSTRate.Text = _dr["Label"].ToString();
                            }
                            else
                            {
                                txtGSTRate.Text = "0.00";
                            }
                        }
                    }
                }
                ddlService.SelectedValue = ds.Tables[0].Rows[0]["QBserviceItem"].ToString();
                ddlServiceExpense.SelectedValue = ds.Tables[0].Rows[0]["QBServiceItemExp"].ToString();
                ddlServicelabor.SelectedValue = ds.Tables[0].Rows[0]["QBServiceItemLabor"].ToString();
                //if (ds.Tables[0].Rows[0]["SyncTimesheet"]!=DBNull.Value)
                //chkSyncTimesheet.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SyncTimesheet"]);
                //if (ds.Tables[0].Rows[0]["SyncInvoice"] != DBNull.Value)
                //chkSyncInvoice.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["SyncInvoice"]);

                txtCompany.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                txtAddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtCity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                ddlState.SelectedValue = ds.Tables[0].Rows[0]["state"].ToString();
                txtZip.Text= ds.Tables[0].Rows[0]["zip"].ToString();
                txtTele.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                txtFax.Text = ds.Tables[0].Rows[0]["fax"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["email"].ToString();
                txtWebAdd.Text = ds.Tables[0].Rows[0]["webaddress"].ToString();
                ddlDBType.SelectedValue = ds.Tables[0].Rows[0]["msm"].ToString();
                txtDB.Text = ds.Tables[0].Rows[0]["dbname"].ToString();
                ddlDBType.Enabled = false;
                txtDB.Enabled = false;
                chkCustRegistrn.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["custweb"]);
                txtFilePath.Text = ds.Tables[0].Rows[0]["QBPath"].ToString();
                chkMultilang.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["multilang"]);
                chkMSEmail.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["msemailnull"]);
                chkSyncEmp.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["EmpSync"]);
                chkAcctIntegration.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["QBIntegration"]);
                chkAcctIntegration_CheckedChanged(sender, e);
                if (ds.Tables[0].Columns.Contains("Contact") && ds.Tables[0].Columns.Contains("Remarks"))
                {
                    txtRemarks.Text = ds.Tables[0].Rows[0]["remarks"].ToString();
                    txtContName.Text = ds.Tables[0].Rows[0]["Contact"].ToString();
                }
                Session["logo"]  = null;
                if (ds.Tables[0].Rows[0]["logo"] != DBNull.Value)
                {
                    byte[] myByteArray = (byte[])ds.Tables[0].Rows[0]["logo"];

                    MemoryStream ms = new MemoryStream(myByteArray, 0, myByteArray.Length);
                    ms.Write(myByteArray, 0, myByteArray.Length);
                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                    Session["logo"]  =ResizeImage( image);
                    string img = "data:image/png;base64," + Convert.ToBase64String((byte[])ds.Tables[0].Rows[0]["logo"]);
                    imgLogo.ImageUrl = img;
                }
                else
                {
                    imgLogo.ImageUrl = "images/blankimage.png";
                }               
            }
        }

        Permission();
    }

    private byte[] ResizeImage(System.Drawing.Image stImage)
    {
        byte[] bmpBytes = null;
        if (stImage != null)
        {
            // Create a bitmap of the content of the fileUpload control in memory
            Bitmap originalBMP = new Bitmap(stImage);
            double sngRatioraw = 0;
            int sngRatio = 0;
            int newWidth = 0;
            int newHeight = 0;
            // Calculate the new image dimensions
            int origWidth = originalBMP.Width;
            int origHeight = originalBMP.Height;
            if (origWidth > origHeight)
            {
                sngRatioraw = Convert.ToDouble( origWidth )/ Convert.ToDouble( origHeight);
                newWidth = 225;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newHeight = newWidth / sngRatio;
            }
            else
            {
                sngRatioraw =Convert.ToDouble(  origHeight) /Convert.ToDouble( origWidth);
                newHeight = 225;
                sngRatio = Convert.ToInt32(Math.Round(sngRatioraw));
                newWidth = newHeight / sngRatio;
            }

            // Create a new bitmap which will hold the previous resized bitmap
            Bitmap newBMP = new Bitmap(originalBMP, newWidth, newHeight);

            // Create a graphic based on the new bitmap
            Graphics oGraphics = Graphics.FromImage(newBMP);
            // Set the properties for the new graphic file
            oGraphics.SmoothingMode = SmoothingMode.AntiAlias; oGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the new graphic based on the resized bitmap
            oGraphics.DrawImage(originalBMP, 0, 0, newWidth, newHeight);

       

            bmpBytes = BmpToBytes_MemStream(newBMP);

            // Once finished with the bitmap objects, we deallocate them.
            originalBMP.Dispose();
            newBMP.Dispose();
            oGraphics.Dispose();
        }
        return bmpBytes;
    }

    private byte[] BmpToBytes_MemStream(Bitmap bmp)
    {
        MemoryStream ms = new MemoryStream();
        // Save to memory using the Jpeg format
        bmp.Save(ms, ImageFormat.Png);

        // read to end
        byte[] bmpBytes = ms.GetBuffer();
        bmp.Dispose();
        ms.Close();

        return bmpBytes;
    }       

    private void Permission()
    {
        HtmlGenericControl li = (HtmlGenericControl)Page.Master.FindControl("progMgr");
        //li.Style.Add("background", "url(images/active_menu_bg.png) repeat-x");
        li.Attributes.Add("class", "start active open");

        HyperLink a = (HyperLink)Page.Master.FindControl("userlink");
        //a.Style.Add("color", "#2382b2");

        HyperLink lnkUsersSmenu = (HyperLink)Page.Master.FindControl("lnkCntrlPnl");
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
            lblCustReg.Visible = false;
            chkCustRegistrn.Visible = false;

            DataTable dt = new DataTable();
            dt = (DataTable)Session["userinfo"];                

            string ProgFunc = dt.Rows[0]["Control"].ToString().Substring(0, 1);
            if (ProgFunc == "N")
            {
                Response.Redirect("home.aspx");
            }
        }

        if (Session["MSM"].ToString() == "TS")
        {
            //btnSubmit.Visible = false;
            Response.Redirect("home.aspx");
        }
        if (Convert.ToInt32(Session["ISsupervisor"]) == 1)
        {
            Response.Redirect("home.aspx");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.FirstName = txtCompany.Text;
            objPropUser.Address = txtAddress.Text;            
            objPropUser.City = txtCity.Text;
            objPropUser.State = ddlState.SelectedValue;
            objPropUser.Zip = txtZip.Text;
            objPropUser.Tele = txtTele.Text;
            objPropUser.Fax = txtFax.Text;            
            objPropUser.Email = txtEmail.Text;
            objPropUser.Website = txtWebAdd.Text;
            objPropUser.MSM = ddlDBType.SelectedValue;
            objPropUser.DSN = "";//txtDSN.Text.Trim();
            objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.Password = "";//txtDpass.Text.Trim();
            objPropUser.Username = "";// txtDuser.Text.Trim();
            objPropUser.ConnConfig = Session["config"].ToString();
            objPropUser.ContactName = txtContName.Text;
            objPropUser.CustWeb = Convert.ToInt16(chkCustRegistrn.Checked);
            objPropUser.QBPath = txtFilePath.Text;
            //System.Drawing.Image st = (System.Drawing.Image)Session["logo"] ;
            //objPropUser.Logo = ResizeImage(st);
            objPropUser.Logo = (byte[])Session["logo"];
            objPropUser.MultiLang = Convert.ToInt16(chkMultilang.Checked);
            objPropUser.QBInteg = Convert.ToInt16(chkAcctIntegration.Checked);
            objPropUser.EmailMS = Convert.ToInt16(chkMSEmail.Checked);
            objPropUser.QBFirstSync = Convert.ToInt16(chkSyncEmp.Checked);
            objPropUser.QBSalesTaxID = ddlService.SelectedValue;
            objPropUser.QbserviceItemlabor = ddlServicelabor.SelectedValue;
            objPropUser.QBserviceItemExp = ddlServiceExpense.SelectedValue;
            if (!ddlYearEnd.SelectedValue.Equals(":: Select ::"))
            {
                objPropUser.YE = Convert.ToInt32(ddlYearEnd.SelectedValue);
            }
            objPropUser.GSTReg = txtGSTReg.Text;            // change by dev 3rd Feb, 16

            //objPropUser.TransferInvoice = Convert.ToInt16(chkSyncInvoice.Checked);
            //objPropUser.TransferTimeSheet= Convert.ToInt16(chkSyncTimesheet.Checked);

            //objPropUser.MerchantID = txtMerchantID.Text.Trim();
            //objPropUser.LoginID = txtLoginID.Text.Trim();
            //objPropUser.PaymentUser = txtPayUser.Text.Trim();
            //objPropUser.PaymentPass = AES_Algo.Encrypt(txtPayPass.Text.Trim(), "MSMPAY", "4Bvq75DG", "SHA1", 1000, "pOWaTbO92LfXbh69JkYzfT7P465TNc0h", 256);

            //if (Convert.ToInt32(ViewState["mode"]) == 1)
            //{
                objBL_User.UpdateCompany(objPropUser);                

                ClientScript.RegisterStartupScript(Page.GetType(), "keySuccUp", "noty({text: 'Company info. updated successfully!',  type : 'success', layout:'topCenter',closeOnSelfClick:true, timeout : 5000,theme : 'noty_theme_default',  closable : false});", true);
                        
                //lblMsg.Text = "User updated successfully.";
                //ClearControls();
            //}
            //else
            //{
            //    objBL_User.AddCompany(objPropUser);
            //    ViewState["mode"] = 0;
            //    lblMsg.Text = "Company added successfully.";
            //    ClearControls();
            //}
            _objPropGeneral.ConnConfig = Session["config"].ToString();  // change by dev on 3rd feb, 16
            _objPropGeneral.CustomLabel = ddlCountry.SelectedValue;
            _objPropGeneral.CustomName = "Country";
            _objBLGeneral.UpdateCustom(_objPropGeneral);

            if(ddlCountry.SelectedValue.Equals("1"))
            {
                _objPropGeneral.CustomLabel = txtGSTRate.Text;
                _objPropGeneral.CustomName = "GSTRate";
                _objBLGeneral.UpdateCustom(_objPropGeneral);

                _objPropGeneral.CustomLabel = hdnGSTGL.Value;
                _objPropGeneral.CustomName = "GSTGL";
                _objBLGeneral.UpdateCustom(_objPropGeneral);
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
        ResetFormControlValues(this);
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
    
    protected void lnkClose_Click(object sender, EventArgs e)
    {
        Response.Redirect("home.aspx");
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            System.Drawing.Image imgfile = System.Drawing.Image.FromStream(FileUpload1.PostedFile.InputStream);
            Session["logo"]  = null;
            Session["logo"]  = ResizeImage( imgfile);                       
            string img = "data:image/png;base64," + Convert.ToBase64String(ResizeImage(imgfile));
            imgLogo.ImageUrl = img;            
        }
    }
    protected void chkAcctIntegration_CheckedChanged(object sender, EventArgs e)
    {
        if (chkAcctIntegration.Checked == true)
        {
            txtFilePath.Enabled = true;
        }
        else
        {
            txtFilePath.Enabled = false;
        }
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

        ddlServiceExpense.DataSource = ds.Tables[0];
        ddlServiceExpense.DataTextField = "billcode";
        ddlServiceExpense.DataValueField = "QBinvid";
        ddlServiceExpense.DataBind();

        ddlServiceExpense.Items.Insert(0, new ListItem(":: Select ::", ""));

        ddlServicelabor.DataSource = ds.Tables[0];
        ddlServicelabor.DataTextField = "billcode";
        ddlServicelabor.DataValueField = "QBinvid";
        ddlServicelabor.DataBind();

        ddlServicelabor.Items.Insert(0, new ListItem(":: Select ::", ""));
    }
    private void FillYearEnd()
    {
        try
        {
            var _varMonth = Enum.GetValues(typeof(CommonHelper.Months));
            var values = Enum.GetValues(typeof(CommonHelper.Months)).Cast<CommonHelper.Months>();
            
            ddlYearEnd.Items.Add(new ListItem(":: Select ::"));
            int i=0;
            foreach (var v in values)
            {
                ddlYearEnd.Items.Add(new ListItem(v.Description(), i.ToString()));
                i++;
            }

            //ddlYearEnd.Items.Add(new ListItem(":: Select ::"));
            //ddlYearEnd.AppendDataBoundItems = true;
            //ddlYearEnd.DataSource = _varMonth;
            //ddlYearEnd.DataBind();
        }
        catch (Exception ex)
        {
            string str = ex.Message.Replace("'", "\"").Replace("\r\n", string.Empty);
            ClientScript.RegisterStartupScript(Page.GetType(), "keyErr", "noty({text: '" + str + "',  type : 'error', layout:'topCenter',closeOnSelfClick:false, timeout : false,theme : 'noty_theme_default',  closable : true});", true);
        }
    }
   
}
