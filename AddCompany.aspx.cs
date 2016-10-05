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
//using Microsoft.SqlServer.Management.Smo;
//using Microsoft.SqlServer.Management.Common;
using System.Data.SqlClient;
using System.IO;
using Microsoft.ApplicationBlocks.Data;

public partial class AddCompany : System.Web.UI.Page
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
            objPropUser.DSN = txtDSN.Text.Trim();
            objPropUser.DBName = txtDB.Text.Trim();
            objPropUser.Password = txtDpass.Text.Trim();
            objPropUser.Username = txtDuser.Text.Trim();
            objPropUser.Type = ddlDBType.SelectedValue;
            objPropUser.ConnConfig = Connectionstr(txtDB.Text.Trim());
            objPropUser.ContactName = txtContName.Text;
            objPropUser.Remarks = txtRemarks.Text;
            
            //DataSet dsDbname = new DataSet();
            //dsDbname = objBL_User.getDatabases(objPropUser);
                       
            //if (dsDbname.Tables[0].Rows.Count == 0)
            //{
                //objBL_User.CreateDatabase(objPropUser);
                //CreateDatabaseObjects(txtDB.Text.Trim());
                objBL_User.AddCompany(objPropUser);
                objBL_User.AddDatabaseName(objPropUser);
                lblMsg.Text = "Database created successfully.";
                ClearControls();               
            //}
            //else
            //{
            //    lblMsg.Text = "Database already exists, please use different database name.";
            //    return;
            //}
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

    private void CreateDatabaseObjects(string dbname)
    {
        FileInfo fileInfo = new FileInfo(Server.MapPath(Request.ApplicationPath) + "/scripts/CreateDBObjects.sql");

        string script = fileInfo.OpenText().ReadToEnd();

        string constr = Connectionstr(dbname);

        SqlConnection connection = new SqlConnection(constr);
        //Server serversql = new Server(new ServerConnection(connection));
        //serversql.ConnectionContext.ExecuteNonQuery(script);
        ////serversql.ConnectionContext.Disconnect();
        ////connection.Close();
    }

    ////private void DropDatabase(string dbname)
    ////{
    ////    string constr = Connectionstr(dbname);
    ////    SqlConnection connection = new SqlConnection(constr);
    ////    Server serversql = new Server(new ServerConnection(connection));
    ////    serversql.Databases[dbname].Drop();
    ////    //connection.Close();
    ////}

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
