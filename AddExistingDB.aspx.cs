using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessEntity;
using BusinessLayer;
using System.Data;
using Microsoft.ApplicationBlocks.Data;

public partial class AddExistingDB : System.Web.UI.Page
{
    BusinessEntity.User objPropUser = new BusinessEntity.User();
    BL_User objBL_User = new BL_User();



    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MSM"] == null)
        {
            Response.Redirect("login.aspx");
        }
        if (Session["MSM"].ToString() != "ADMIN")
        {
            Response.Redirect("home.aspx");
        }

        if (!IsPostBack)
        {
            ViewState["mode"] = 0;

            if (Request.QueryString["ID"] != null)
            {
                ViewState["mode"] = 1;
                DataSet ds = new DataSet();
                objPropUser.CtrlID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                ds = objBL_User.getAdminControlByID(objPropUser);
                //ViewState["mode"] = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtCompany.Text = ds.Tables[0].Rows[0]["companyname"].ToString();
                    txtDB.Text = ds.Tables[0].Rows[0]["dbname"].ToString();
                    ddlDBType.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    ddlDBType.Enabled = false;
                }
            }
            else
            {
                //lblAddEditUser.Text = "Add Existing Database";
               // ViewState["mode"] = 1;
                lblHeader.Text = "Add Existing Database";
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            objPropUser.FirstName = txtCompany.Text;
            objPropUser.MSM = ddlDBType.SelectedValue;
            objPropUser.DSN = txtDSN.Text.Trim();
            objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.Password = txtDpass.Text.Trim();
            objPropUser.Username = txtDuser.Text.Trim();
            objPropUser.Type = ddlDBType.SelectedValue;
            objPropUser.ConnConfig = Connectionstr(txtDB.Text.Trim());

            if (txtDB.Text != string.Empty)
            {
                objPropUser.DBName = txtDB.Text.Trim();
                DataSet dsDB = new DataSet();
                dsDB = objBL_User.getDatabases(objPropUser);

                if (dsDB.Tables[0].Rows.Count > 0)
                {
                    objPropUser.ConnConfig = Connectionstr(txtDB.Text.Trim());
                    DataSet dsChk = new DataSet();
                    dsChk = objBL_User.CheckDB(objPropUser);

                    if (ddlDBType.SelectedValue == "TS")
                    {
                        if (dsChk.Tables[0].Rows.Count > 0 && dsChk.Tables[1].Rows.Count == 0)
                        {
                            if (Convert.ToInt32(ViewState["mode"]) == 1)
                            {
                                objPropUser.CtrlID = Convert.ToInt32(Request.QueryString["ID"].ToString());
                                objBL_User.UpdateDatabaseName(objPropUser);

                                lblMsg.Text = "Database updated successfully.";
                            }
                            else
                            {
                                objBL_User.AddDatabaseName(objPropUser);
                                lblMsg.Text = "Database added successfully.";
                                ClearControls();
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Not a valid TS-9 database.";
                            return;
                        }
                    }
                    else if (ddlDBType.SelectedValue == "MSM")
                    {
                        if (dsChk.Tables[0].Rows.Count > 0 && dsChk.Tables[1].Rows.Count == 1)
                        {
                            if (Convert.ToInt32(ViewState["mode"]) == 1)
                            {
                                objPropUser.CtrlID = Convert.ToInt32(Request.QueryString["ID"].ToString());

                                objBL_User.UpdateDatabaseName(objPropUser);
                                lblMsg.Text = "Database updated successfully.";
                            }
                            else
                            {
                                objBL_User.AddDatabaseName(objPropUser);
                                lblMsg.Text = "Database added successfully.";
                                ClearControls();
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Not a valid MSM database.";
                            return;
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "Database does not exists.";
                    return;
                }
            }
            else
            {
                lblMsg.Text = "Please enter database name.";
                return;
            }


            ////DataSet dsDbname = new DataSet();
            ////dsDbname = objBL_User.getDatabases(objPropUser);

            ////if (dsDbname.Tables[0].Rows.Count == 0)
            ////{                
            //    objBL_User.AddDatabaseName(objPropUser);
            //    //ViewState["mode"] = 0;
            //    lblMsg.Text = "Database added successfully.";
            //    ClearControls();
            ////}
            ////else
            ////{
            ////    lblMsg.Text = "Database already exists, please use different database name.";
            ////    return;
            ////}
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
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
        Response.Redirect("adminpanel.aspx");
    }

    private string Connectionstr(string dbname)
    {
        string server = Config.MS.Split(';')[0].Split('=')[1];
        string database = dbname;
        string user = Config.MS.Split(';')[2].Split('=')[1];
        string pass = Config.MS.Split(';')[3].Split('=')[1];

        string constr = "server=" + server + ";database=" + database + ";user=" + user + ";password=" + pass + "";
        return constr;
    }
}
