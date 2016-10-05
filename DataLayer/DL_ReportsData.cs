using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using BusinessEntity;
using System.IO;


namespace DataLayer
{
    public class DL_ReportsData
    {
        public DataSet GetCustomerDetails(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "sp_GetCustomerDetails_Report", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetCustomerDetailsTest(User objPropUser)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                //varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                //varname1.Append("o.[Type], o.Balance, o.Status, \n");
                //varname1.Append("l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                //varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                //varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, e.Unit AS EquipmentName, \n");
                //varname1.Append("e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType, \n");
                //varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                //varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                //varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                //varname1.Append("from Rol r inner join owner o on r.Id = o.Rol ");
                //varname1.Append("INNER JOIN Loc AS l ON o.ID = l.Owner ");
                //varname1.Append("INNER JOIN Terr AS t ON t.ID = l.Terr ");
                //varname1.Append("INNER JOIN Elev AS e ON l.Loc = e.Loc ");
                //varname1.Append("order by r.name");

                varname1.Append("select r.Name, r.City, r.State, r.Zip, r.Phone, r.Fax, r.Contact, r.Address, r.Email, r.Country, r.Website, r.Cellular, \n");
                varname1.Append("o.[Type], o.Balance, o.Status, \n");
                varname1.Append("(select count(1) from loc where owner=o.id) as loc, \n");
                varname1.Append("(select count(1) from elev where owner=o.id) as equip, \n");
                varname1.Append("(select count(1) from ticketo where owner=o.id) as opencall \n");
                varname1.Append("from Rol r inner join owner o on r.Id = o.Rol \n");
                varname1.Append("select l.ID AS LocationId, l.Tag AS LocationName, l.Address AS LocationAddress, l.City AS LocationCity, l.State AS LocationState, l.Zip AS LocationZip, l.Type AS LocationType, \n");
                varname1.Append("l.STax AS LocationSTax, l.Elevs AS EquipmentCounts, l.Status AS LocationStatus, \n");
                varname1.Append("l.Balance AS LocationBalance, t.Name AS DefaultSalesPerson, l.prospect AS LocationProspect, ro.Name as Route \n");
                varname1.Append("from Terr AS t INNER JOIN Loc AS l ON t.ID = l.Terr \n");
                varname1.Append("INNER JOIN route AS ro ON ro.ID = l.Route \n");
                varname1.Append("select e.Unit AS EquipmentName, e.Manuf, e.Type AS EquipmentType, e.Cat AS ServiceType, e.Price AS EquipmentPrice, e.Install AS InstalledOn, e.State AS EquipmentState, e.Building AS BuildingType \n");
                varname1.Append("from Elev as e \n");

                varname1.Append("select v.ID VendorID, v.Acct AS VendorName, v.Type AS VendorType, v.Status As VendorStatus, v.Balance As VBalance, v.CLimit, v.DA, v.Terms,v.Days, v.InUse \n");
                varname1.Append("from Rol r inner join Vendor v on r.Id = v.Rol and r.Type = 1");


                //varname1.Append("from loc as l ");
                //varname1.Append("inner join rol as r ");
                //varname1.Append("on r.ID = l.Rol ");
                //varname1.Append("inner join route as ro ");
                //varname1.Append("on ro.ID = l.route ");
                

                //if (!(string.IsNullOrEmpty(LocName)))
                //{
                //    varname1.Append("where l.type like '%" + LocName + "%'");
                //}



                ds = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get customer summary listing details. By Yashasvi Jadav
        /// </summary>
        /// <param name="cstData"></param>
        /// <param name="LocName"></param>
        /// <returns></returns>
        public DataSet GetAccSummaryDetail(User cstData)
        {
            try
            {
                DataSet ds = new DataSet();
                StringBuilder varname1 = new StringBuilder();
                varname1.Append("select \n");
                varname1.Append("Name as Route \n");
                //varname1.Append("from loc as l ");
                //varname1.Append("inner join rol as r ");
                //varname1.Append("on r.ID = l.Rol ");
                //varname1.Append("inner join route as ro ");
                //varname1.Append("on ro.ID = l.route ");
                varname1.Append("from route");

                //if (!(string.IsNullOrEmpty(LocName)))
                //{
                //    varname1.Append("where l.type like '%" + LocName + "%'");
                //}

                ds = SqlHelper.ExecuteDataset(cstData.ConnConfig, CommandType.Text, varname1.ToString());

                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataSet InsertCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[22];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportName
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportType
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.UserId
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsGlobal
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsAscending
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SortBy
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterColumns
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterValues
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.CompanyName
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportTitle
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SubTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.DatePrepared
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.TimePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PageNumber
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ExtraFooterLine
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Alignment
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.MainHeader
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PDFSize
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsStock
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Module",
                SqlDbType = SqlDbType.VarChar,
                Value = objCustReport.Module
            };

            try
            {
                // SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spAddCustomerReportDetails", para);
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, "spAddCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool CheckForDelete(CustomerReport objCustReport)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objCustReport.UserId + " and Id = " + objCustReport.ReportId + "");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[1];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spDeleteCustomerReport", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool CheckExistingReport(CustomerReport objCustReport, string reportAction)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where ReportName = '" + objCustReport.ReportName + "'");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    if (reportAction != "Save")
                    {
                        if (objCustReport.ReportId == Convert.ToInt32(objCustReport.DsCustomer.Tables[0].Rows[0]["Id"]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsStockReportExist(CustomerReport objCustReport, string reportAction)
        {
            try
            {
                objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where ReportName = '" + objCustReport.ReportName + "' and IsStock='true'");
                if (objCustReport.DsCustomer.Tables[0].Rows.Count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerReport(CustomerReport objCustReport)
        {
            var para = new SqlParameter[22];

            para[0] = new SqlParameter
            {
                ParameterName = "@RptId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ReportName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ReportType",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportType
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@UserId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.UserId
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@IsGlobal",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsGlobal
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@IsAscendingOrder",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsAscending
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@SortBy",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SortBy
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@FilterColumns",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterColumns
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@FilterValues",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.FilterValues
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@CompanyName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.CompanyName
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@ReportTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ReportTitle
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@SubTitle",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.SubTitle
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@DatePrepared",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.DatePrepared
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@TimePrepared",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.TimePrepared
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@PageNumber",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PageNumber
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@ExtraFooterLine",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ExtraFooterLine
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Alignment",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.Alignment
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@MainHeader",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.MainHeader
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@PDFSize",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.PDFSize
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@IsStock",
                SqlDbType = SqlDbType.Bit,
                Value = objCustReport.IsStock
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spUpdateCustomerReportDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReports(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " union select * from tblReports where IsGlobal = 'true'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetStockReports(User objPropUser)
        {
            try
            {
                if (objPropUser.UserID == 0)
                {
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where ReportType = '" + objPropUser.Type + "' and IsStock = 'true' and IsGlobal = 'true'");
                }
                else
                {
                    return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select * from tblReports where UserId = " + objPropUser.UserID + " and ReportType = '" + objPropUser.Type + "' and IsStock = 'true' and IsGlobal = 'true'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportColByRepId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select ColumnName from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportFiltersByRepId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select FilterColumn, FilterSet from tblReportFilters where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetOwners(string query, User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetReportDetailById(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReports where Id = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetControlForReports(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, CommandType.Text, "select Name, Address, City, State, Zip, Phone, Fax, Email, WebAddress, Logo ,dbname from control");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerType(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Type from OType");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetGroupedCustomersLocation(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetGroupedCustomersLocation", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerName(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Name from CustomerReportDetails where Name != '' order by Name");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerAddress(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct Address from CustomerReportDetails where Address != '' order by Address");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustomerCity(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select distinct City from CustomerReportDetails where City != '' order by City");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetHeaderFooterDetail(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select * from tblReportHeaderFooterDetail where ReportId = " + objCustReport.ReportId + " ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetColumnWidthByReportId(CustomerReport objCustReport)
        {
            try
            {
                return objCustReport.DsCustomer = SqlHelper.ExecuteDataset(objCustReport.ConnConfig, CommandType.Text, "select ColumnWidth from tblReportColumnsMapping where ReportId = " + objCustReport.ReportId + "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateCustomerReportResizedWidth(CustomerReport objCustReport)
        {
            var para = new SqlParameter[3];

            para[0] = new SqlParameter
            {
                ParameterName = "@ReportId",
                SqlDbType = SqlDbType.Int,
                Value = objCustReport.ReportId
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@ColumnName",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnName
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@ColumnWidth",
                SqlDbType = SqlDbType.NVarChar,
                Value = objCustReport.ColumnWidth
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objCustReport.ConnConfig, CommandType.StoredProcedure, "spUpdateCustReportResizedWidth", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetCustReportFiltersValue(User objPropUser)
        {
            try
            {
                return objPropUser.DsUserAuthorization = SqlHelper.ExecuteDataset(objPropUser.ConnConfig, "spGetCustReportFiltersValue", objPropUser.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
