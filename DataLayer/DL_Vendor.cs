using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DataLayer
{
    public class DL_Vendor
    {
        public DataSet AddVendor(Vendor objVendor)
        {
            try
            {
                string query = "INSERT INTO Vendor ([Rol],[Acct], [Type], [Status], [ShipVia], [Balance], [CLimit],[Terms],[Days],[1099],[InUse],[DA]) OUTPUT INSERTED.ID VALUES (@Rol,@Acct,@Type, @Status, @ShipVia, @Balance, @CLimit, @Terms, @Days, @Vendor1099,@Inuse,@DA)";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@Rol", objVendor.Rol));
                parameters.Add(new SqlParameter("@Acct", objVendor.Acct));
                parameters.Add(new SqlParameter("@Type", objVendor.Type));
                parameters.Add(new SqlParameter("@Status", objVendor.Status));
                parameters.Add(new SqlParameter("@ShipVia", objVendor.ShipVia));
                parameters.Add(new SqlParameter("@Balance", objVendor.Balance));
                parameters.Add(new SqlParameter("@CLimit", objVendor.CLimit));
                parameters.Add(new SqlParameter("@Terms", objVendor.Terms));
                parameters.Add(new SqlParameter("@Days", objVendor.Days));
                parameters.Add(new SqlParameter("@Vendor1099", objVendor.Vendor1099));
                parameters.Add(new SqlParameter("@Inuse", objVendor.InUse));
                parameters.Add(new SqlParameter("@DA", objVendor.DA));
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetAllVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT ID,Rol, Acct, Type, Status, ShipVia, Balance, CLimit, Terms, Days, 1099 FROM Vendor Order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateVendor(Vendor objVendor)
        {
            try
            {
                string query = "UPDATE Vendor SET  [Acct] = @Acct, [Type] = @Type, [Status] = @Status, [ShipVia] = @ShipVia, [Balance] = @Balance, [CLimit]=@CLimit, [Terms] = @Terms, [Days] = @Days, [1099] = @Vendor1099, [DA] = @DA WHERE [ID] = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", objVendor.ID));
                //parameters.Add(new SqlParameter("@Rol", objVendor.Rol));
                parameters.Add(new SqlParameter("@Acct", objVendor.Acct));
                parameters.Add(new SqlParameter("@Type", objVendor.Type));
                parameters.Add(new SqlParameter("@Status", objVendor.Status));
                parameters.Add(new SqlParameter("@ShipVia", objVendor.ShipVia));
                parameters.Add(new SqlParameter("@Balance", objVendor.Balance));
                parameters.Add(new SqlParameter("@CLimit", objVendor.CLimit));
                parameters.Add(new SqlParameter("@Terms", objVendor.Terms));
                parameters.Add(new SqlParameter("@Days", objVendor.Days));
                parameters.Add(new SqlParameter("@Vendor1099", objVendor.Vendor1099));
                parameters.Add(new SqlParameter("@DA", objVendor.DA));
                int rowsAffected = SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT ID,Rol,Acct,Type,Status,isnull(Balance,0) as Balance,CLimit,[1099],FID,DA,[Acct#],Terms,Disc,Days,InUse,Remit,OnePer,DBank,Custom1,Custom2,Custom3,Custom4,Custom5,Custom6,Custom7,Custom8,Custom9,Custom10 FROM Vendor WHERE ID = " + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteVendor(Vendor objVendor)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objVendor.ConnConfig, CommandType.Text, " DELETE FROM Vendor WHERE ID = " + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistsForInsertVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.DsIsExist = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + objVendor.Acct + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistForUpdateVendor(Vendor objVendor)
        {
            try
            {
                return objVendor.DsIsExist = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "Select Count(*) as CountVendor FROM Vendor Where Acct='" + objVendor.Acct + "' AND ID !=" + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorGridview(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID,Vendor.Rol,Rol.Name,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct, Rol.Type, Vendor.Status, Vendor.ShipVia, (isnull(Vendor.Balance,0)*-1) as Balance ,Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by Rol.Name");
                //SELECT Vendor.ID,Rol.Name,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct, Vendor.Type, Vendor.Status, Vendor.ShipVia, Vendor.Balance,Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by ID);
                //"SELECT Vendor.ID,Rol.Name, Vendor.Acct, Vendor.Type, Vendor.Status, Vendor.ShipVia, Vendor.Balance, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099] FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by ID;");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorDetails(Vendor objVendor) // Get Vendors details who's expense is exists in PJ table.
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,isnull(v.Balance,0) as Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r, PJ p where v.ID=p.Vendor AND v.Rol=r.ID Order by r.Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendorEdit(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID, Vendor.Rol,Rol.Name,Rol.Address,Rol.City,Rol.State,Rol.Country,Rol.EMail,Rol.Website,Rol.Cellular,Rol.Zip,Rol.Phone,Rol.Fax,Rol.Contact,Rol.GeoLock,Rol.Since,Rol.Last, Vendor.Acct, Rol.Type, Vendor.Status, Vendor.ShipVia, isnull(Vendor.Balance,0) as Balance, Vendor.InUse, Vendor.CLimit, Vendor.Terms, Vendor.Days, Vendor.[1099], isnull(Vendor.DA,0) as DA, isnull(chart.fDesc,'') as DefaultAcct FROM Vendor Left Join Rol on Vendor.Rol=Rol.ID left join Chart on Vendor.DA=Chart.ID where Vendor.ID=" + objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllVendors(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,v.Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r where v.Rol=r.ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorSearch(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, "spGetVendorSearch", objVendor.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool IsExistVendorDetails(Vendor _objVendor)
        {
            try
            {
                return _objVendor.IsExist = Convert.ToBoolean(SqlHelper.ExecuteScalar(_objVendor.ConnConfig, "spIsVendorExist", _objVendor.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorRolDetails(Vendor _objVendor)
        {
            try
            {
                return _objVendor.Ds = SqlHelper.ExecuteDataset(_objVendor.ConnConfig, CommandType.Text, "SELECT v.*,r.Name,r.State,r.City,r.Zip,r.Address FROM Vendor as v, Rol as r WHERE v.Rol = r.ID and v.ID = " + _objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetVendorGLById(Vendor objVendor)
        {
            try
            {
                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID, isnull(Vendor.DA,0) as DA, chart.Acct ,isnull(chart.fDesc,'') as DefaultAcct FROM Vendor LEFT JOIN Rol ON Vendor.Rol=Rol.ID LEFT JOIN Chart ON Vendor.DA=Chart.ID WHERE (Vendor.DA <> 0 OR Vendor.DA <> NULL) AND Vendor.ID = '" + objVendor.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorListDetails(Vendor objVendor)
        {
            try
            {
                //return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT DISTINCT v.ID,v.Rol,r.Name,v.Acct,v.Type,v.Status,v.Balance,v.CLimit,v.FID,v.DA,v.Acct#,v.Terms,v.Disc,v.Days,v.InUse,v.Remit,v.OnePer,v.DBank,v.Custom1,v.Custom2,v.Custom3,v.Custom4,v.Custom5,v.Custom6,v.Custom7,v.Custom8,v.Custom9,v.Custom10,v.ShipVia,v.QBVendorID FROM Vendor v, Rol r where v.Rol=r.ID");

                return objVendor.Ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.StoredProcedure, "SpVendorList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetVendorAcct(Vendor _objVendor)
        {
            try
            {
                return _objVendor.Ds = SqlHelper.ExecuteDataset(_objVendor.ConnConfig, CommandType.Text, "SELECT v.ID,v.Acct# FROM Vendor as v WHERE v.ID = " + _objVendor.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      


        public DataSet GetAllVenderAjaxSearch(Vendor objVendor)
        {
            DataSet ds = new DataSet();
            try
            {

                if (HttpContext.Current.Session["Vendors"] != null)
                {
                    DataTable dtpo = ((DataTable)HttpContext.Current.Session["Vendors"]).Copy();

                    ds.Tables.Add(dtpo);


                }
                else
                {
                    objVendor.ConnConfig = HttpContext.Current.Session["config"].ToString();

                    ds = SqlHelper.ExecuteDataset(objVendor.ConnConfig, CommandType.Text, "SELECT Vendor.ID,Vendor.Rol,Rol.Name,Vendor.Acct,case when Rol.Type=1 then 'Cost Of Sales' else 'Overhead' end as Type,case when Vendor.Status=1 then 'InActive' when Vendor.Status=0 then 'Active' else 'Hold' end as Status,(isnull(Vendor.Balance,0)*-1) as Balance FROM Vendor Join Rol on Vendor.Rol=Rol.ID Order by Rol.Name");

                }

                objVendor.Ds = ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }
    }
}
