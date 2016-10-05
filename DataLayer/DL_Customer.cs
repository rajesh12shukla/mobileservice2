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
    public class DL_Customer
    {
        public DataSet getCustomers(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(Config.MS, "spGetCustomers", DBNull.Value, DBNull.Value, objPropCustomer.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspectByID(Customer objPropCustomer)
        {
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT p.ID, \n");
            //varname1.Append("       p.Rol, \n");
            //varname1.Append("       p.status, \n");
            //varname1.Append("       p.type, \n");
            //varname1.Append("       p.Address AS billaddress, \n");
            //varname1.Append("       p.City AS billcity, \n");
            //varname1.Append("       p.State AS billstate, \n");
            //varname1.Append("       p.Zip AS billzip, \n");
            //varname1.Append("       p.phone AS billphone, \n");
            //varname1.Append("       p.CustomerName, \n");
            //varname1.Append("       p.Terr, \n");
            //varname1.Append("       r.Name, \n");
            //varname1.Append("       r.Address , \n");
            //varname1.Append("       r.City    , \n");
            //varname1.Append("       r.State   , \n");
            //varname1.Append("       r.Zip     , \n");
            //varname1.Append("       r.Phone   , \n");
            //varname1.Append("       r.Cellular, \n");
            //varname1.Append("       r.email, \n");
            //varname1.Append("       r.Website, \n");
            //varname1.Append("       r.Fax, \n");
            //varname1.Append("       r.Contact, \n");
            //varname1.Append("       r.Remarks, \n");
            //varname1.Append("       r.lat, r.lng \n");
            //varname1.Append("FROM   Prospect p \n");
            //varname1.Append("       INNER JOIN Rol r \n");
            //varname1.Append("               ON r.ID = p.Rol ");
            //varname1.Append(" where p.ID=" + objPropCustomer.ProspectID);

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProspectByID", objPropCustomer.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspect(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProspects", objPropCustomer.SearchBy, objPropCustomer.SearchValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTasks(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetTasks", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTasksByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetTaskByID", objPropCustomer.TemplateID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunityByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunityByID", objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRecentProspect(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetRecentItems");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getProspectType(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from ptype");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getStages(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from stage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getRepTemplateName(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select ID,fdesc, 0 as CBcheckStatus from EquipTemp  order by ID");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getRepTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *,( SELECT Count(distinct Elev) FROM   dbo.EquipTItem WHERE  EquipT = EquipTemp.ID and Elev <>0) as TotalUnits from EquipTemp  order by ID ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getCustomTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from elevt order by fdesc ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getTemplateItemByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *, (select top 1 fdesc from equiptemp where id= ei.equipt)as name from EquipTItem ei where EquipT ='" + objPropCustomer.TemplateID + "' and Elev =0");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getCustTemplateItemByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select *,(select format from elevTItem eti where eti.ID=ei.customid)as formatMOM, (select top 1 fdesc from elevt where id= ei.elevt)as name from elevTItem ei where elevT =" + objPropCustomer.TemplateID + " and Elev =0 order by line select * from tblcustomvalues where elevt=" + objPropCustomer.TemplateID);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getCustomValues(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tblcustomvalues where itemid= " + objPropCustomer.ItemID + " order by value");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getTemplateItemCodes(Customer objPropCustomer)
        {
            string strQuery = "select distinct code from EquipTItem where isnull(code,'')<> ''";
            if (objPropCustomer.TemplateID != 0)
            {
                strQuery += "  and EquipT <> " + objPropCustomer.TemplateID;
            }
            if (!string.IsNullOrEmpty(objPropCustomer.SearchValue))
            {
                strQuery += "  and code like '" + objPropCustomer.SearchValue + "%'";
            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet getTemplateItemByMultipleID(Customer objPropCustomer, string id)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.EquipT in (" + id + ")");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public DataSet getTemplateItemByElevAndEquipT(Customer objPropCustomer, string EquipId, string Elev)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "if exists(select 1 from EquipTItem where EquipT = " + EquipId + " and Elev =" + Elev + ") Begin  select  et.fdesc as Name, et.Remarks, eti.EquipT,eti.fDesc,eti.Lastdate, eti.NextDateDue, eti.Frequency from EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID  where eti.EquipT = " + EquipId + " and eti.Elev=" + Elev + " End  Else Begin select	et.fdesc as Name,et.Remarks,eti.EquipT,	eti.fDesc,eti.Lastdate,eti.NextDateDue,	eti.Frequency from	EquipTItem eti inner join EquipTemp et on eti.EquipT = et.ID   where eti.EquipT = " + EquipId + " and eti.Elev=" + Elev + " End");

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public void AddProspect(Customer objPropCustomer)
        {
            var para = new SqlParameter[26];

            para[0] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@address",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Address
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@type",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Status
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@CustomerName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CustomerName
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@SalesPerson",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Terr
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@BillAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@BillCity",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCity
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@BillState",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillState
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Billzip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillZip
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@Billphone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillPhone
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Website
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Lat",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lat
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Lng",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lng
            };
            para[23] = new SqlParameter
            {
                ParameterName = "ContactData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.ContactData
            };
            para[24] = new SqlParameter
            {
                ParameterName = "@UpdateUser",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.LastUpdateUser
            };
            para[25] = new SqlParameter
            {
                ParameterName = "@Source",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Source
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProspect", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateProspect(Customer objPropCustomer)
        {
            var para = new SqlParameter[27];

            para[0] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@address",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Address
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@type",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Type
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Status
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@cell",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@CustomerName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.CustomerName
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@SalesPerson",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Terr
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@BillAddress",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Billaddress
            };
            para[15] = new SqlParameter
            {
                ParameterName = "@BillCity",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillCity
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@BillState",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillState
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@Billzip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillZip
            };
            para[18] = new SqlParameter
            {
                ParameterName = "@Billphone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.BillPhone
            };
            para[19] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[20] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Website
            };
            para[21] = new SqlParameter
            {
                ParameterName = "@Lat",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lat
            };
            para[22] = new SqlParameter
            {
                ParameterName = "@Lng",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Lng
            };
            para[23] = new SqlParameter
            {
                ParameterName = "@ContactData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.ContactData
            };
            para[24] = new SqlParameter
            {
                ParameterName = "@ProspectID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProspectID
            };
            para[25] = new SqlParameter
            {
                ParameterName = "@UpdateUser",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.LastUpdateUser
            };
            para[26] = new SqlParameter
            {
                ParameterName = "@Source",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Source
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateProspect", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProspect(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteProspect", objPropCustomer.ProspectID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddProspectType(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spAddProspectType", objPropCustomer.Type, objPropCustomer.Remarks, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateStages(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateStage", objPropCustomer.Stage.ID, objPropCustomer.Stage.Description, objPropCustomer.Stage.Count, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getLocCoordinates(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();
            if (objPropCustomer.Status == 0)
            {
                varname1.Append("SELECT (SELECT name \n");
                varname1.Append("        FROM   route \n");
                varname1.Append("        WHERE  id = l.route)   AS worker, \n");
                varname1.Append("       l.loc, \n");
                varname1.Append("        (ISNULL( r.lat,'') + ',' +ISNULL( r.lng,'') )      AS coordinates, \n");
                varname1.Append("       Replace(tag, '''', '`') AS tagrep, \n");
                varname1.Append("       tag, \n");
                varname1.Append("       l.address, \n");
                varname1.Append("       l.city, \n");
                varname1.Append("       lat, \n");
                varname1.Append("       lng, \n");
                varname1.Append("       tag                     AS title, \n");
                varname1.Append("       (l.address +' ,'+ l.city +' ,' +l.state)              AS description, \n");

                //if (objPropCustomer.Status == 0)
                //{
                varname1.Append("       isnull(Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.BAmt \n");
                varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
                varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
                varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
                varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyBill, \n");

                varname1.Append("       isnull(Round (CASE c.SCycle \n");
                varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
                varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
                //varname1.Append("                WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
                //varname1.Append("                WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyHours, \n");

                varname1.Append("       (SELECT Count(1) \n");
                varname1.Append("        FROM   tblJoinElevJob \n");
                varname1.Append("        WHERE  Job = c.job)    AS elevcount, \n");
                //}
                //else
                //{
                //    varname1.Append("       isnull(Round (CASE c.BCycle \n");
                //    varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
                //    varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
                //    varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
                //    varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
                //    varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
                //    varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
                //    varname1.Append("                WHEN 6 THEN 0 --never \n");
                //    varname1.Append("                else 0 \n");
                //    varname1.Append("              END, 2) ,0)         AS MonthlyBill, \n");

                //    varname1.Append("Isnull(Round (CASE c.SCycle WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
                //    varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
                //    varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
                //    varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
                //    varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
                //    varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
                //    varname1.Append("WHEN 6 THEN Hours -- monthly \n");
                //    varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
                //    varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
                //    varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
                //    varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
                //    varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
                //    varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
                //    varname1.Append("WHEN 13 THEN 0.00 --never \n");
                //    varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
                //    varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
                //    varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
                //    varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
                //    varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
                //    varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
                //    varname1.Append("else 0 \n");
                //    varname1.Append("END, 2), 0) AS MonthlyHours, ");

                //    varname1.Append(" (select count(1) from elev el where el.loc=l.loc and el.status=0 ) as elevcount, \n");
                //}

                varname1.Append("       isnull(c.BAmt,0) as bamt, \n");
                varname1.Append("       isnull(c.Hours,0) as Hours, \n");
                varname1.Append("       c.job \n");

                varname1.Append("FROM   loc l \n");
                varname1.Append("       INNER JOIN rol r \n");
                varname1.Append("               ON r.id = l.rol ");

                varname1.Append(" inner join Contract c on c.Loc=l.Loc \n");
                varname1.Append(" inner join job j on c.job=j.id \n");
                varname1.Append(" where l.id is not null \n");

                if (objPropCustomer.NullAddressOnly == true)
                {
                    varname1.Append(" and isnull(r.lat,'')= '' \n");
                }
                if (objPropCustomer.LocIDs != null)
                {
                    if (objPropCustomer.LocIDs != string.Empty)
                        varname1.Append(" and l.loc in(" + objPropCustomer.LocIDs + ") \n");
                    else
                        varname1.Append(" and l.loc = 0 \n");
                }
                if (objPropCustomer.Worker != 0)
                {
                    varname1.Append(" and l.route=" + objPropCustomer.Worker + "\n");
                }

                varname1.Append(
                    "  AND (((select top 1 name from rol where id=(select top 1 Rol from Owner o where o.ID=l.Owner)) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(Tag LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.ID LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.city LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.state LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (r.Address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')   OR (r.City LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.Zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(Phone) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')) \n");
                varname1.Append(" and c.status=0 \n");
                varname1.Append(" order by tag ");
            }
            else
            {
                varname1.Append("SELECT worker, \n");
                varname1.Append("       loc, \n");
                varname1.Append("       coordinates, \n");
                varname1.Append("       tagrep, \n");
                varname1.Append("       tag, \n");
                varname1.Append("       address, \n");
                varname1.Append("       city, \n");
                varname1.Append("       lat, \n");
                varname1.Append("       lng, \n");
                varname1.Append("       title, \n");
                varname1.Append("       description, \n");
                varname1.Append("       Route, \n");
                varname1.Append("       sum(elevcount) as elevcount, \n");
                varname1.Append("       Sum(MonthlyBill)  AS MonthlyBill, \n");
                varname1.Append("       Sum(MonthlyHours) AS MonthlyHours, \n");
                varname1.Append("       Sum(bamt)         AS bamt, \n");
                varname1.Append("       Sum(Hours)        AS Hours, \n");
                varname1.Append("       0        AS job \n");

                varname1.Append("FROM   (SELECT (SELECT name \n");
                varname1.Append("                FROM   route \n");
                varname1.Append("                WHERE  id = l.route)                             AS worker, \n");
                varname1.Append("               l.loc, \n");
                varname1.Append("               ( Isnull( r.lat, '') + ',' + Isnull( r.lng, '') ) AS coordinates, \n");
                varname1.Append("               Replace(tag, '''', '`')                           AS tagrep, \n");
                varname1.Append("               tag, \n");
                varname1.Append("               l.address, \n");
                varname1.Append("               l.city, \n");
                varname1.Append("               lat, \n");
                varname1.Append("               lng, \n");
                varname1.Append("               tag                                               AS title, \n");
                varname1.Append("               ( l.address + ' ,' + l.city + ' ,' + l.state )    AS description, \n");
                varname1.Append("       isnull(Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
                varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
                varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
                varname1.Append("                WHEN 6 THEN 0 --never \n");
                varname1.Append("                else 0 \n");
                varname1.Append("              END, 2) ,0)         AS MonthlyBill, \n");

                varname1.Append("Isnull(Round (CASE c.SCycle WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
                varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
                varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
                varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
                varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
                varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
                varname1.Append("WHEN 6 THEN Hours -- monthly \n");
                varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
                varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
                varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
                varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
                varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
                varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
                varname1.Append("WHEN 13 THEN 0.00 --never \n");
                varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
                varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
                varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
                varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
                varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
                varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
                varname1.Append("else 0 \n");
                varname1.Append("END, 2), 0) AS MonthlyHours, ");

                //varname1.Append(" (select count(1) from elev el where el.loc=l.loc and el.status=0 ) as elevcount, \n");
                varname1.Append(" (case  when j.fgroup is not null then( select count(1)from elev el where el.loc=l.loc and el.status=0 and el.fGroup=j.fGroup ) else 1 end) as elevcount, \n");

                varname1.Append("               Isnull(c.BAmt, 0)                                 AS bamt, \n");
                varname1.Append("               Isnull(c.Hours, 0)                                AS Hours, \n");
                varname1.Append("               l.route \n");
                varname1.Append("        FROM   loc l \n");
                varname1.Append("               INNER JOIN rol r \n");
                varname1.Append("                       ON r.id = l.rol \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("               INNER JOIN job j \n");
                varname1.Append("                       ON c.job = j.id \n");
                varname1.Append("        WHERE  l.id IS NOT NULL \n");

                if (objPropCustomer.NullAddressOnly == true)
                {
                    varname1.Append(" and isnull(r.lat,'')= '' \n");
                }
                if (objPropCustomer.LocIDs != null)
                {
                    if (objPropCustomer.LocIDs != string.Empty)
                        varname1.Append(" and l.loc in(" + objPropCustomer.LocIDs + ") \n");
                    else
                        varname1.Append(" and l.loc = 0 \n");
                }
                if (objPropCustomer.Worker != 0)
                {
                    varname1.Append(" and l.route=" + objPropCustomer.Worker + " \n");
                }

                varname1.Append(
                    "  AND (((select top 1 name from rol where id=(select top 1 Rol from Owner o where o.ID=l.Owner)) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(Tag LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.ID LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or(l.address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.city LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.state LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') or (l.zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (Contact LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (r.Address LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')   OR (r.City LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.Zip LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')  OR (dbo.RemoveSpecialChars(Phone) LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%') OR (r.EMail LIKE '%" +
                    objPropCustomer.SearchValue.Replace("'", "''") + "%')) \n");
                varname1.Append("               AND c.status = 0) t \n");

                varname1.Append("GROUP  BY worker, \n");
                varname1.Append("          loc, \n");
                varname1.Append("          coordinates, \n");
                varname1.Append("          tagrep, \n");
                varname1.Append("          tag, \n");
                varname1.Append("          address, \n");
                varname1.Append("          city, \n");
                varname1.Append("          lat, \n");
                varname1.Append("          lng, \n");
                //varname1.Append("          elevcount, \n");
                varname1.Append("          title, \n");
                varname1.Append("          description, \n");
                varname1.Append("          Route \n");
                varname1.Append("ORDER  BY tag ");

            }
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getWorkers(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT Name, \n");
            varname1.Append("       id, \n");

            if (objPropCustomer.Status == 0)
            {
                varname1.Append("       (SELECT Count (1) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("               ) AS contr, \n");

                varname1.Append("       (SELECT Count(1) \n");
                varname1.Append("        FROM   tblJoinElevJob ej \n");
                varname1.Append("               INNER JOIN Contract cc \n");
                varname1.Append("                       ON cc.Job = ej.Job \n");
                varname1.Append("               INNER JOIN job jj \n");
                varname1.Append("                       ON jj.ID = cc.Job \n");
                varname1.Append("        WHERE  jj.Custom20 = r.ID and cc.status =0)AS units, \n");

                varname1.Append(" Isnull((SELECT Sum (Isnull(Round (CASE c.sCycle \n");
                varname1.Append("                                    WHEN 0 THEN c.Hours \n");
                varname1.Append("                                    WHEN 1 THEN c.Hours / 2 \n");
                varname1.Append("                                    WHEN 2 THEN c.Hours / 3 \n");
                varname1.Append("                                    WHEN 3 THEN c.Hours / 6 \n");
                varname1.Append("                                    WHEN 4 THEN c.Hours / 12 \n");
                varname1.Append("                                    ELSE 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("              ),0)  AS MonthlyHours, \n");

                varname1.Append(" Isnull((SELECT Sum (Isnull(Round (CASE c.BCycle \n");
                varname1.Append("                                    WHEN 0 THEN c.BAmt \n");
                varname1.Append("                                    WHEN 1 THEN c.BAmt / 2 \n");
                varname1.Append("                                    WHEN 2 THEN c.BAmt / 3 \n");
                varname1.Append("                                    WHEN 3 THEN c.BAmt / 6 \n");
                varname1.Append("                                    WHEN 4 THEN c.BAmt / 12 \n");
                varname1.Append("                                    ELSE 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID \n");
                varname1.Append("               AND c.status = 0) , 0) AS MonthlyBill ");
            }
            else
            {
                varname1.Append("       (SELECT  Count (distinct l.loc) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("               ) AS contr, \n");

                //varname1.Append("  (select dbo.CalculateRouteUnits(r.id)) as units, \n");
                //varname1.Append("   (select count(1) from Elev el where el.status=0 and el.loc in ( select l.loc from loc l inner join contract c on l.loc=c.loc where l.route=r.id and c.status=0 )  ) as units, \n");
                varname1.Append("(SELECT isnull( Sum (elevcount),0) \n");
                varname1.Append(" FROM   (SELECT ( CASE \n");
                varname1.Append("                    WHEN j.fgroup IS NOT NULL THEN(SELECT Count(1) \n");
                varname1.Append("                                                   FROM   elev el \n");
                varname1.Append("                                                   WHERE  el.loc = l.loc \n");
                varname1.Append("                                                          AND el.status = 0 \n");
                varname1.Append("                                                          AND el.fGroup = j.fGroup) \n");
                varname1.Append("                    ELSE 1 \n");
                varname1.Append("                  END ) AS elevcount \n");
                varname1.Append("         FROM   Loc l \n");
                varname1.Append("                INNER JOIN Contract c \n");
                varname1.Append("                        ON c.Loc = l.Loc \n");
                varname1.Append("                INNER JOIN Job j \n");
                varname1.Append("                        ON c.Job = j.ID \n");
                varname1.Append("         WHERE  c.Status = 0 \n");
                varname1.Append("                AND l.Route = r.ID) t)  as units,");

                varname1.Append("isnull( (SELECT Sum (Isnull(Round (CASE c.sCycle \n");
                varname1.Append("WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
                varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
                varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
                varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
                varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
                varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
                varname1.Append("WHEN 6 THEN Hours -- monthly \n");
                varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
                varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
                varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
                varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
                varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
                varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
                varname1.Append("WHEN 13 THEN 0.00 --never \n");
                varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
                varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
                varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
                varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
                varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
                varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
                varname1.Append("else 0 \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID and c.status =0 \n");
                varname1.Append("              ),0)  AS MonthlyHours, \n");

                varname1.Append("isnull((SELECT Sum (Isnull(Round (CASE c.BCycle \n");
                varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
                varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
                varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
                varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
                varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
                varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
                varname1.Append("                WHEN 6 THEN 0 --never \n");
                varname1.Append("                else 0 --never \n");
                varname1.Append("                                  END, 2), 0)) \n");
                varname1.Append("        FROM   Loc l \n");
                varname1.Append("               INNER JOIN Contract c \n");
                varname1.Append("                       ON c.Loc = l.Loc \n");
                varname1.Append("        WHERE  Route = r.ID \n");
                varname1.Append("               AND c.status = 0),0) AS MonthlyBill ");
            }

            varname1.Append("FROM   Route r \n ");
            if (!string.IsNullOrEmpty(objPropCustomer.Name))
            {
                varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n");
            }
            varname1.Append("order by name");

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getWorkerMonthly(Customer objPropCustomer)
        {
            StringBuilder varname1 = new StringBuilder();

            //if (objPropCustomer.Status == 0)
            //{
            //    varname1.Append("SELECT r.Name, \n");
            //    varname1.Append("      isnull( Sum (c.BAmt),0)        BAmt, \n");
            //    varname1.Append("      isnull( Sum(c.Hours),0)        Hours, \n");
            //    varname1.Append("      isnull( Sum(Round (CASE c.BCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.BAmt \n");
            //    varname1.Append("                WHEN 1 THEN c.BAmt / 2 \n");
            //    varname1.Append("                WHEN 2 THEN c.BAmt / 3 \n");
            //    varname1.Append("                WHEN 3 THEN c.BAmt / 6 \n");
            //    varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
            //    varname1.Append("                    WHEN 6 THEN 0 \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyBill, \n");
            //    varname1.Append("      isnull( Sum(Round (CASE c.SCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            //    varname1.Append("                WHEN 1 THEN c.Hours / 2 --Bi-Monthly \n");
            //    varname1.Append("                WHEN 2 THEN c.Hours / 3 --Quarterly \n");
            //    varname1.Append("                WHEN 3 THEN c.Hours / 6 --Semi-Anually \n");
            //    varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            //    //varname1.Append("                WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
            //    //varname1.Append("                WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyHours, \n");

            //    varname1.Append("       Count(c.job)                   AS contr, \n");

            //    varname1.Append("       (SELECT Count(1) \n");
            //    varname1.Append("        FROM   tblJoinElevJob ej \n");
            //    varname1.Append("               INNER JOIN Contract cc \n");
            //    varname1.Append("                       ON cc.Job = ej.Job \n");
            //    varname1.Append("               INNER JOIN job jj \n");
            //    varname1.Append("                       ON jj.ID = cc.Job \n");
            //    varname1.Append("        WHERE  jj.Custom20 = r.ID and cc.status=0)AS units \n");

            //    varname1.Append("FROM   route r \n");
            //    varname1.Append("       LEFT OUTER JOIN Loc l \n");
            //    varname1.Append("                    ON r.ID = l.Route \n");
            //    varname1.Append("       LEFT OUTER JOIN Contract c \n");
            //    varname1.Append("                    ON l.Loc = c.Loc \n");
            //    varname1.Append("                       AND c.Status = 0 \n");
            //    varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n"); 
            //    varname1.Append("GROUP  BY r.Name, r.id ");
            //}
            //else
            //{
            //    varname1.Append("SELECT r.Name, \n");
            //    varname1.Append("      isnull( Sum (c.BAmt),0)        BAmt, \n");
            //    varname1.Append("      isnull( Sum(c.Hours),0)        Hours, \n");

            //    varname1.Append("      isnull( Sum(Round (CASE c.BCycle \n");
            //    varname1.Append("                WHEN 0 THEN c.bamt --Monthly \n");
            //    varname1.Append("                WHEN 1 THEN c.bamt / 2 --Bi-Monthly \n");
            //    varname1.Append("                WHEN 2 THEN c.bamt / 3 --Quarterly \n");
            //    varname1.Append("                WHEN 3 THEN c.bamt / 4 --3timesyr \n");
            //    varname1.Append("                WHEN 4 THEN c.bamt / 6 --semiannual \n");
            //    varname1.Append("                WHEN 5 THEN c.bamt / 12 --annual \n");
            //    varname1.Append("                WHEN 6 THEN 0 --never \n");
            //    varname1.Append("                else 0  \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyBill, \n");

            //    varname1.Append("      isnull( Sum(Round (CASE c.SCycle \n");
            //    varname1.Append("WHEN 0 THEN ( CASE SWE WHEN 1 THEN Hours * 30 ELSE Hours * 21.66 END ) --daily, \n");
            //    varname1.Append("WHEN 1 THEN ( Hours * 12.99 ) -- threeXweek \n");
            //    varname1.Append("WHEN 2 THEN ( Hours * 8.60 ) -- twoXweek \n");
            //    varname1.Append("WHEN 3 THEN ( Hours * 4.30 ) -- weekly \n");
            //    varname1.Append("WHEN 4 THEN ( Hours * 2.17 ) -- biweekly \n");
            //    varname1.Append("WHEN 5 THEN ( Hours * 2 ) -- semimonthly \n");
            //    varname1.Append("WHEN 6 THEN Hours -- monthly \n");
            //    varname1.Append("WHEN 7 THEN ( Hours / 1.37991 ) -- evry6weeks \n");
            //    varname1.Append("WHEN 8 THEN ( Hours / 2.00 )-- bimonyhly \n");
            //    varname1.Append("WHEN 9 THEN ( Hours / 3.00 )-- quart \n");
            //    varname1.Append("WHEN 10 THEN ( Hours / 4.00 )-- threetimeperyr \n");
            //    varname1.Append("WHEN 11 THEN ( Hours / 6.00 )-- semiannualy \n");
            //    varname1.Append("WHEN 12 THEN ( Hours / 12.00 )-- annualy \n");
            //    varname1.Append("WHEN 13 THEN 0.00 --never \n");
            //    varname1.Append("WHEN 14 THEN ( Hours * 1.44 )-- every4w \n");
            //    varname1.Append("WHEN 15 THEN ( Hours * 1.08 )-- every5w \n");
            //    varname1.Append("WHEN 16 THEN ( Hours * 0.87 )-- every3w \n");
            //    varname1.Append("WHEN 17 THEN ( Hours / 1.83988 )-- every8w \n");
            //    varname1.Append("WHEN 18 THEN ( Hours / 2.9898 )-- every13w \n");
            //    varname1.Append("WHEN 19 THEN ( Hours / 2.29984 )-- every10w \n");
            //    varname1.Append("else 0 \n");
            //    varname1.Append("                  END, 2)),0) AS MonthlyHours, \n");

            //    varname1.Append("       Count( distinct l.loc)                   AS contr, \n");

            //    //varname1.Append("   (select count(1) from Elev el where el.status=0 and el.loc in ( select l.loc from loc l inner join contract c on l.loc=c.loc where l.route=r.id and c.status=0 )  ) as units \n");
            //    //varname1.Append("  (select dbo.CalculateRouteUnits(r.id)) as units \n");
            //    varname1.Append("(SELECT isnull( Sum (elevcount),0) \n");
            //    varname1.Append(" FROM   (SELECT ( CASE \n");
            //    varname1.Append("                    WHEN j.fgroup IS NOT NULL THEN(SELECT Count(1) \n");
            //    varname1.Append("                                                   FROM   elev el \n");
            //    varname1.Append("                                                   WHERE  el.loc = l.loc \n");
            //    varname1.Append("                                                          AND el.status = 0 \n");
            //    varname1.Append("                                                          AND el.fGroup = j.fGroup) \n");
            //    varname1.Append("                    ELSE 1 \n");
            //    varname1.Append("                  END ) AS elevcount \n");
            //    varname1.Append("         FROM   Loc l \n");
            //    varname1.Append("                INNER JOIN Contract c \n");
            //    varname1.Append("                        ON c.Loc = l.Loc \n");
            //    varname1.Append("                INNER JOIN Job j \n");
            //    varname1.Append("                        ON c.Job = j.ID \n");
            //    varname1.Append("         WHERE  c.Status = 0 \n");
            //    varname1.Append("                AND l.Route = r.ID) t)  as units");

            //    varname1.Append("FROM   route r \n");
            //    varname1.Append("       LEFT OUTER JOIN Loc l \n");
            //    varname1.Append("                    ON r.ID = l.Route \n");
            //    varname1.Append("       LEFT OUTER JOIN Contract c \n");
            //    varname1.Append("                    ON l.Loc = c.Loc \n");
            //    varname1.Append("                       AND c.Status = 0 \n");
            //    varname1.Append("WHERE  r.Name IN ( " + objPropCustomer.Name + " ) \n"); 
            //    varname1.Append("GROUP  BY r.Name, r.id ");
            //}

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocRoute(Customer objPropCustomer)
        {
            SqlParameter para;
            para = new SqlParameter
            {
                ParameterName = "Locations",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtTemplateData
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spUpdateLocRoute", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet GetWorkerCalculations(Customer objPropCustomer)
        {
            SqlParameter para;
            para = new SqlParameter
            {
                ParameterName = "WorkerData",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.dtWorkerData
            };

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spWorkerChangeCalculation", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public int AddRouteTemplate(Customer objPropCustomer)
        {
            var para = new SqlParameter[10];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };

            para[1] = new SqlParameter
            {
                ParameterName = "sequence",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RouteSequence
            };

            para[2] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter { ParameterName = "Mode", SqlDbType = SqlDbType.Int, Value = objPropCustomer.Mode };

            para[4] = new SqlParameter
            {
                ParameterName = "TemplateID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };

            //para[5] = new SqlParameter();
            //para[5].ParameterName = "TemplateData";
            //para[5].SqlDbType = SqlDbType.Structured;
            //para[5].Value = objPropCustomer.DtTemplateData;

            para[5] = new SqlParameter { ParameterName = "worker", SqlDbType = SqlDbType.Int, Value = DBNull.Value };

            para[6] = new SqlParameter
            {
                ParameterName = "Center",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Center
            };

            para[7] = new SqlParameter
            {
                ParameterName = "Radius",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Radius
            };

            para[8] = new SqlParameter
            {
                ParameterName = "Overlay",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Overlay
            };

            para[9] = new SqlParameter
            {
                ParameterName = "PolygonCoord",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.PolygonCoord
            };

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spRouteTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getRouteTemplate(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tblroutetemplate");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getTemplateByID(Customer objPropCustomer)
        {
            string str = "select (select name from route where id=t.worker) as workername, t.* from tblroutetemplate t where templateid=" + objPropCustomer.TemplateID + "";

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT ID, \n");
            //varname1.Append("       TemplateID, \n");
            //varname1.Append("       t.Loc, \n");
            //varname1.Append("       Worker                            AS workerid, \n");
            //varname1.Append("       (SELECT name \n");
            //varname1.Append("        FROM   route \n");
            //varname1.Append("        WHERE  id = (SELECT Route \n");
            //varname1.Append("                     FROM   Loc \n");
            //varname1.Append("                     WHERE  Loc = t.Loc))AS worker, \n");
            //varname1.Append("       (SELECT Tag \n");
            //varname1.Append("        FROM   Loc \n");
            //varname1.Append("        WHERE  Loc = t.Loc)              AS tag, \n");
            //varname1.Append("      isnull( Round (CASE c.BCycle \n");
            //varname1.Append("                WHEN 0 THEN c.BAmt \n");
            //varname1.Append("                WHEN 1 THEN c.BAmt / 6 \n");
            //varname1.Append("                WHEN 2 THEN c.BAmt / 4 \n");
            //varname1.Append("                WHEN 3 THEN c.BAmt / 2 \n");
            //varname1.Append("                WHEN 4 THEN c.BAmt / 12 \n");
            //varname1.Append("                    WHEN 6 THEN 0 \n");
            //varname1.Append("              END, 2)  ,0)                  AS MonthlyBill, \n");
            //varname1.Append("      isnull( Round (CASE c.SCycle \n");
            //varname1.Append("                WHEN 0 THEN c.Hours --Monthly \n");
            //varname1.Append("                WHEN 1 THEN c.Hours / 6 --Bi-Monthly \n");
            //varname1.Append("                WHEN 2 THEN c.Hours / 4 --Quarterly \n");
            //varname1.Append("                WHEN 3 THEN c.Hours / 2 --Semi-Anually \n");
            //varname1.Append("                WHEN 4 THEN c.Hours / 12 --Anually \n");
            //varname1.Append("                WHEN 5 THEN c.Hours * 4.3 / 12 --Weekly \n");
            //varname1.Append("                WHEN 6 THEN c.Hours * 2.15 / 12 --Bi-Weekly \n");
            //varname1.Append("              END, 2) ,0)                   AS MonthlyHours \n");

            //if (objPropCustomer.Status == 0)
            //{
            //    varname1.Append("       ,(SELECT Count(1) \n");
            //    varname1.Append("        FROM   tblJoinElevJob \n");
            //    varname1.Append("        WHERE  Job = c.job)    AS elevcount \n");
            //}
            //else
            //{
            //    varname1.Append("        ,0    AS elevcount \n");
            //}


            //varname1.Append("FROM   tblTemplateDetails t \n");
            //varname1.Append("       INNER JOIN Contract c \n");
            //varname1.Append("               ON c.Loc = t.Loc ");

            //str += varname1.ToString();
            //str += " where templateid=" + objPropCustomer.TemplateID + "";


            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, str);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddTask(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spAddTask", objPropCustomer.ROL, objPropCustomer.DueDate, objPropCustomer.TimeDue, objPropCustomer.Subject, objPropCustomer.Remarks, objPropCustomer.AssignedTo, objPropCustomer.Name, objPropCustomer.Contact, objPropCustomer.Mode, objPropCustomer.TaskID, objPropCustomer.Status, objPropCustomer.Resolution, objPropCustomer.LastUpdateUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getOpportunity(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetOpportunity", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate, objPropCustomer.ROL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddOpportunity(Customer objPropCustomer)
        {
            try
            {
                int oppid = 0;
                oppid = Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spAddOpportunity", objPropCustomer.OpportunityID, objPropCustomer.Name, objPropCustomer.ROL, objPropCustomer.Probability, objPropCustomer.Status, objPropCustomer.Remarks, objPropCustomer.EndDate, objPropCustomer.Mode, objPropCustomer.ProspectID, objPropCustomer.NextStep, objPropCustomer.Description, objPropCustomer.Source, objPropCustomer.Amount, objPropCustomer.Fuser, objPropCustomer.LastUpdateUser, objPropCustomer.Close, objPropCustomer.ticketID));
                return oppid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteOpportunity(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.Text, "delete from lead where ID=" + objPropCustomer.OpportunityID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTask(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.Text, "delete from todo where ID=" + objPropCustomer.TaskID + " delete from done where ID=" + objPropCustomer.TaskID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getContactByRolID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "Spgetcontactbyrol", objPropCustomer.ROL);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getSalesDashboard(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spSalesDashboard");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationRole(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select * from tbllocationrole where owner=" + objPropCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getLocationByRoleID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select loc from loc where roleID=" + objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddLocationRole(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spAddLocationRole", objPropCustomer.LocationRole, objPropCustomer.Username, objPropCustomer.Password, objPropCustomer.CustomerID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateLocationRole(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spUpdateLocationRole", objPropCustomer.LocationRole, objPropCustomer.Username, objPropCustomer.Password, objPropCustomer.CustomerID, objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteLocationRole(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spDeleteLocationRole", objPropCustomer.RoleID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLabor(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select id, item, amount from tblestimatelabour");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet GetEstimateLaborForEstimate(Customer objPropCustomer)
        {
            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, "select itemid as id, item, amount from tblEstimateLabourItems where EstimateID = " + objPropCustomer.TemplateID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEstimateTemplate(Customer objPropCustomer)
        {
            var para = new SqlParameter[7];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[4] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };

            if (objPropCustomer.dtItems != null)
            {
                para[5] = new SqlParameter
                {
                    ParameterName = "items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
                para[6] = new SqlParameter
                {
                    ParameterName = "LaborItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtLaborItems
                };
            }

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimateTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateTemplate(Customer objPropCustomer)
        {
            string strQuery = "Select ID,Name,fdesc,remarks from Estimate where EstTemplate=1";

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimate(Customer objPropCustomer)
        {

            StringBuilder varname1 = new StringBuilder();
            varname1.Append("SELECT ID, \n");
            varname1.Append("       NAME, \n");
            varname1.Append("       fdesc, \n");
            varname1.Append("       remarks, \n");
            varname1.Append("       job, \n");
            varname1.Append("       (SELECT NAME \n");
            varname1.Append("        FROM   Rol \n");
            varname1.Append("        WHERE  ID = e.RolID) AS contact, \n");
            varname1.Append("       ffor, \n");
            varname1.Append("       CASE status \n");
            varname1.Append("         WHEN 0 THEN 'Open' \n");
            varname1.Append("         WHEN 1 THEN 'Canceled' \n");
            varname1.Append("         WHEN 2 THEN 'Withdrawn' \n");
            varname1.Append("         WHEN 3 THEN 'Disqualified' \n");
            varname1.Append("         WHEN 4 THEN 'Sold' \n");
            varname1.Append("         WHEN 5 THEN 'Competitor' \n");
            varname1.Append("       END                   AS status \n");
            varname1.Append("FROM   Estimate e \n");
            varname1.Append("WHERE  EstTemplate = 0 ");

            if (objPropCustomer.Close == 1)
            {
                varname1.Append(" and ffor ='ACCOUNT' ");
                if (objPropCustomer.Center == "1")
                    varname1.Append(" and   job is null ");

            }

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet getEstimateTemplateByID(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name ,fDesc,Remarks, rolid, locid,( case ffor when 'ACCOUNT' then (select tag from loc where loc = locid) when 'PROSPECT' then (select name from rol where id=rolid) end )as contact, isnull(cadexchange,0) as cadexchange, status, job from Estimate where ID=" + objPropCustomer.TemplateID;
            strQuery += " select ID, Estimate , Line , fDesc as scope, Quan as quantity,Cost,Price as amount,Amount as total , vendor, isnull( currency ,'') as currency,isnull( measure,1) measure, code,fdesc, amount as budget from EstimateI where Estimate=" + objPropCustomer.TemplateID;
            strQuery += " select ID, Line, TemplateID, LabourID, Amount from tblJoinLaborTemplate where TemplateID = " + objPropCustomer.TemplateID;
            strQuery+= "SELECT j.JobT,j.Job,m.JobTItemID as JobTItem,j.Type as jtype,j.fDesc, j.Code as jcode, j.Line, m.MilestoneName as MilesName, m.RequiredBy as RequiredBy,j.ETCMod as LeadTime,ProjAcquistDate,ActAcquistDate, Comments, isnull(m.Type,0) as Type, isnull(o.Department,'') AS Department, isnull(m.Amount, 0) as Amount FROM jobtitem j INNER JOIN Milestone m ON m.JobtItemId = j.ID LEFT JOIN OrgDep o ON o.ID = m.Type WHERE (j.job=0 or j.job is null) AND j.jobT= " + objPropCustomer.TemplateID;
            strQuery += "select  j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type  from JobT j where j.ID = " + objPropCustomer.TemplateID + "order by j.ID ";
            strQuery += "SELECT j.Code as jcode, j.fDesc, j.Type as jtype, b.Type as Btype, b.Item as BItem,b.QtyRequired as QtyReq, b.UM as UM, b.ScrapFactor as ScrapFact, b.BudgetUnit as BudgetUnit,b.BudgetExt, j.Line,b.Vendor,b.TotalPrice,LTRIM(RTRIM(b.Currency)) as currency,b.AmountDollars as Amount,b.Percentage as jPercent FROM jobtitem j INNER JOIN Bom b ON b.JobtItemId = j.ID WHERE (j.job=0 or j.job is null) AND j.jobT=" + objPropCustomer.TemplateID + "";
            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucket(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name , [Desc] from tblEstimateBucket";

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucketItems(Customer objPropCustomer)
        {
            string strQuery = "select ID, line, item, vendor, unit, cost,isnull( measure,1) measure,code from tblEstimateBucketItems where bucketid=" + objPropCustomer.BucketID;

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEstimateBucket(Customer objPropCustomer)
        {
            var para = new SqlParameter[5];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "desc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "bucketID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.BucketID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };

            if (objPropCustomer.dtItems != null)
            {
                para[4] = new SqlParameter
                {
                    ParameterName = "items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
            }

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimateBucket", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getEstimateBucketByID(Customer objPropCustomer)
        {
            string strQuery = "select ID, Name , [Desc] from tblEstimateBucket where ID=" + objPropCustomer.BucketID;
            strQuery += " select ID, Line , item as scope, vendor, code, cost, unit,isnull( measure,1) measure  from tblEstimateBucketItems where BucketID=" + objPropCustomer.BucketID;

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddEstimateLabor(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "AddLaborItem", objPropCustomer.Name, objPropCustomer.Amount, objPropCustomer.BucketID, objPropCustomer.Mode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddEstimate(Customer objPropCustomer)
        {
            var para = new SqlParameter[19];

            para[0] = new SqlParameter
            {
                ParameterName = "Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Description
            };
            para[2] = new SqlParameter
            {
                ParameterName = "remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };

            para[3] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[4] = new SqlParameter
            {
                ParameterName = "mode",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Mode
            };
            para[5] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };

            para[6] = new SqlParameter
            {
                ParameterName = "rol",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ROL
            };

            para[7] = new SqlParameter
            {
                ParameterName = "CADExchange",
                SqlDbType = SqlDbType.Money,
                Value = objPropCustomer.CADExchange
            };

            para[8] = new SqlParameter
            {
                ParameterName = "Edited",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.IsItemEdited
            };

            para[12] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };


            if (objPropCustomer.dtItems != null)
            {
                para[9] = new SqlParameter
                {
                    ParameterName = "items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
                para[10] = new SqlParameter
                {
                    ParameterName = "LaborItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtLaborItems
                };
                para[11] = new SqlParameter
                {
                    ParameterName = "LaborColumnItems",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtLaborItemsEstimate
                };
                           
            }
            if (objPropCustomer.DtMilestone != null) 
            {
                para[13] = new SqlParameter
                {
                    ParameterName = "MilestonItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtMilestone
                };    
            }
            if (objPropCustomer._dtBomEstimate != null)
            {
                para[14] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer._dtBomEstimate
                };
            }
            para[15] = new SqlParameter
            {
                ParameterName = "Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[16] = new SqlParameter
            {
                ParameterName = "estDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.date
            };
            para[17] = new SqlParameter
            {
                ParameterName = "EstimateNo",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.estimateno
            };
            para[18] = new SqlParameter
            {
                ParameterName = "Jobtype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.type
            };

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddEstimate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       

        public int AddProject(Customer objPropCustomer)
        {
            var para = new SqlParameter[51];

            para[0] = new SqlParameter
            {
                ParameterName = "job",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.ProjectJobID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "owner",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.CustomerID
            };
            para[2] = new SqlParameter
            {
                ParameterName = "loc",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.LocID
            };
            para[3] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Name
            };
            para[4] = new SqlParameter
            {
                ParameterName = "status",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Status
            };
            para[5] = new SqlParameter
            {
                ParameterName = "type",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Type
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "ctype",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.ctypeName
            };
            para[8] = new SqlParameter
            {
                ParameterName = "ProjCreationDate",
                SqlDbType = SqlDbType.DateTime,
                Value = objPropCustomer.ProjectCreationDate
            };
            para[9] = new SqlParameter
            {
                ParameterName = "PO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.PO
            };
            para[10] = new SqlParameter
            {
                ParameterName = "SO",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.SO
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Certified",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Certified
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Custom1",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom1
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Custom2",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom2
            };
            para[14] = new SqlParameter
            {
                ParameterName = "Custom3",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom3
            };
            para[15] = new SqlParameter
            {
                ParameterName = "Custom4",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Custom4
            };
            if (objPropCustomer.Custom5 != DateTime.MinValue)
            {
                para[16] = new SqlParameter
                {
                    ParameterName = "Custom5",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objPropCustomer.Custom5
                };
            }
            para[17] = new SqlParameter
            {
                ParameterName = "template",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.TemplateID
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RolName",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolName
            };
            para[19] = new SqlParameter
            {
                ParameterName = "city",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.City
            };
            para[20] = new SqlParameter
            {
                ParameterName = "state",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.State
            };
            para[21] = new SqlParameter
            {
                ParameterName = "zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Zip
            };
            para[22] = new SqlParameter
            {
                ParameterName = "country",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Country
            };
            para[23] = new SqlParameter
            {
                ParameterName = "phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Phone
            };
            para[24] = new SqlParameter
            {
                ParameterName = "cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Cellular
            };
            para[25] = new SqlParameter
            {
                ParameterName = "fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Fax
            };
            para[26] = new SqlParameter
            {
                ParameterName = "contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Contact
            };
            para[27] = new SqlParameter
            {
                ParameterName = "email",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.Email
            };
            para[28] = new SqlParameter
            {
                ParameterName = "rolRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.RolRemarks
            };
            para[29] = new SqlParameter
            {
                ParameterName = "rolType",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.RolType
            };
            para[30] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvExp
            };
            para[31] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.InvServ
            };
            para[32] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Wage
            };
            para[33] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.GLInt
            };
            para[34] = new SqlParameter
            {
                ParameterName = "jobtCType",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.JobTempCtype
            };
            para[35] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Post
            };
            para[36] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.Charge
            };
            para[37] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.JobClose
            };
            para[38] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = objPropCustomer.fInt
            };
            para[39] = new SqlParameter
            {
                ParameterName = "TeamItems",
                SqlDbType = SqlDbType.Structured,
                Value = objPropCustomer.DtTeam
            };
            if (objPropCustomer.DtBOM != null)
            {
                para[40] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.DtBOM
                };
            }
            if (objPropCustomer.dtItems != null)
            {
                para[41] = new SqlParameter
                {
                    ParameterName = "Items",
                    SqlDbType = SqlDbType.Structured,
                    Value = objPropCustomer.dtItems
                };
            }
            para[42] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };
            if (objPropCustomer.DtMilestone != null)
            {
                if (objPropCustomer.DtMilestone.Rows.Count > 0)
                {
                    para[43] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtMilestone
                    };
                }
            }
            if (objPropCustomer.DtCustom != null)
            {
                if (objPropCustomer.DtCustom.Rows.Count > 0)
                {
                    para[44] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = objPropCustomer.DtCustom
                    };
                }
            }
            para[45] = new SqlParameter
            {
                ParameterName = "BillRate",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.BillRate
            };
            para[46] = new SqlParameter
            {
                ParameterName = "RateOT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateOT
            };
            para[47] = new SqlParameter
            {
                ParameterName = "RateNT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateNT
            };
            para[48] = new SqlParameter
            {
                ParameterName = "RateDT",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateDT
            };
            para[49] = new SqlParameter
            {
                ParameterName = "RateTravel",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.RateTravel
            };
            para[50] = new SqlParameter
            {
                ParameterName = "Mileage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Mileage
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddProject", para);
                return Convert.ToInt32(para[42].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int AddProjectTemplate(JobT _objJob)
        {
            var para = new SqlParameter[26];

            para[0] = new SqlParameter
            {
                ParameterName = "jobT",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Type
            };
            para[3] = new SqlParameter
            {
                ParameterName = "NRev",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NRev
            };
            para[4] = new SqlParameter
            {
                ParameterName = "NDed",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NDed
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Count",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Count
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvExp
            };
            para[8] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvServ
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Wage
            };
            para[10] = new SqlParameter
            {
                ParameterName = "CType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.CType
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Charge
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Post
            };
            para[14] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.fInt
            };
            para[15] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.GLInt
            };
            para[16] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.JobClose
            };
            para[17] = new SqlParameter
            {
                ParameterName = "tempRev",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.TemplateRev
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RevRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.RevRemarks
            };
            para[19] = new SqlParameter
            {
                ParameterName = "alertType",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.AlertType
            };
            para[20] = new SqlParameter
            {
                ParameterName = "alertMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.AlertMgr
            };
            para[21] = new SqlParameter
            {
                ParameterName = "MilestoneMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.MilestoneMgr
            };
            if (_objJob.ProjectDt.Rows.Count > 0)
            {
                para[22] = new SqlParameter
                {
                    ParameterName = "BomItem",
                    SqlDbType = SqlDbType.Structured,
                    Value = _objJob.ProjectDt
                };
            }
            if(_objJob.MilestoneDt != null)
            {
                if (_objJob.MilestoneDt.Rows.Count > 0)
                {
                    para[23] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.MilestoneDt
                    };
                }
            }
            if(_objJob.CustomTabItem != null)
            {
                if(_objJob.CustomTabItem.Rows.Count > 0)
                {
                    para[24] = new SqlParameter
                    {
                        ParameterName = "CustomTabItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomTabItem
                    };
                }
            }
            if (_objJob.CustomItem != null)
            {
                if (_objJob.CustomItem.Rows.Count > 0)
                {
                    para[25] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItem
                    };
                }
            }
            //if (_objJob.EstimateData != null)
            //{
            //    if (_objJob.EstimateData.Rows.Count > 0)
            //    {
            //        para[26] = new SqlParameter
            //        {
            //            ParameterName = "EstimateData",
            //            SqlDbType = SqlDbType.Structured,
            //            Value = _objJob.EstimateData
            //        };
            //    }
            //}
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.StoredProcedure, "spAddProjectTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBomt(Customer objPropCustomer)
        {
            string strQuery = "SELECT ID,Type FROM BOMT";

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int ConvertEstimateToProject(Customer objPropCustomer)
        {
            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, "spConvertEstimateToProject", objPropCustomer.TemplateID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataSet getJobEstimate(Customer objPropCustomer)
        {
            string strQuery = "select j.* from Job j  where j.status=0 and Loc =" + objPropCustomer.LocID + " order by Fdate desc";//inner join Estimate e on e.Job = j.ID

            try
            {
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobProject(Customer objPropCustomer)
        {
            //string strQuery = "select (select id from estimate where job = j.id)as estimateid, j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status ,(select tag from loc where loc = j.loc)as locname from Job j  ";
            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT (SELECT id \n");
            //varname1.Append("        FROM   estimate \n");
            //varname1.Append("        WHERE  job = j.id)       AS estimateid, \n");
            //varname1.Append("       j.id, \n");
            //varname1.Append("       j.fdesc, \n");
            //varname1.Append("       CASE j.status \n");
            //varname1.Append("         WHEN 0 THEN 'Active' \n");
            //varname1.Append("         WHEN 1 THEN 'Inactive' \n");
            //varname1.Append("       END                       AS status, \n");
            //varname1.Append("       (SELECT tag \n");
            //varname1.Append("        FROM   loc \n");
            //varname1.Append("        WHERE  loc = j.loc)      AS locname, \n");
            //varname1.Append("       fDate, \n");
            //varname1.Append("       Rev                       AS TotalBilled, \n");
            //varname1.Append("       Hour, \n");
            //varname1.Append("       PO                        AS TotalOnOreder, \n");
            //varname1.Append("       ( Mat + Labor )           AS TotalExp, \n");
            //varname1.Append("       ( Rev - ( Mat + Labor ) ) AS net \n");
            //varname1.Append("FROM   Job j ");

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("SELECT (SELECT id \n");
            //varname1.Append("        FROM   estimate \n");
            //varname1.Append("        WHERE  job = j.id)                                                                                                                                                                                         AS estimateid, \n");
            //varname1.Append("       j.id, \n");
            //varname1.Append("       j.fdesc, \n");
            //varname1.Append("       CASE j.status \n");
            //varname1.Append("         WHEN 0 THEN 'Active' \n");
            //varname1.Append("         WHEN 1 THEN 'Inactive' \n");
            //varname1.Append("       END                                                                                                                                                                                                         AS status, \n");
            //varname1.Append("       (SELECT tag \n");
            //varname1.Append("        FROM   loc \n");
            //varname1.Append("        WHERE  loc = j.loc)                                                                                                                                                                                        AS locname, \n");
            //varname1.Append("       j.fDate, \n");
            //varname1.Append("       j.PO                                                                                                                                                                                                          AS TotalOnOreder, \n");
            //varname1.Append("       ( Sum(d.Reg) + Sum(d.OT) + Sum(d.DT) + Sum(d.TT) \n");
            //varname1.Append("         + Sum(d.NT) )                                                                                                                                                                                             AS Hour, \n");
            //varname1.Append("       isnull((select sum(isnull(Price,0) * isnull(Quan,0)) as amount FROM InvoiceI where job = j.ID),0) AS TotalBilled,  \n");
            //varname1.Append("       CONVERT(NUMERIC(30, 2), Sum(( Isnull(d.Reg, 0) + ( Isnull(d.OT, 0) * 1.5 ) + ( Isnull(d.DT, 0) * 2 ) + ( Isnull(d.NT, 0) * 1.7 ) + Isnull(d.TT, 0) ) * Isnull(w.HourlyRate, 0)))                            AS TotalExp, \n");
            //varname1.Append("       ( isnull((select sum(isnull(Price,0) * isnull(Quan,0)) as amount FROM InvoiceI where job = j.ID),0) \n");
            //varname1.Append("          - CONVERT(NUMERIC(30, 2), Sum(( Isnull(d.Reg, 0) + ( Isnull(d.OT, 0) * 1.5 ) + ( Isnull(d.DT, 0) * 2 ) + ( Isnull(d.NT, 0) * 1.7 ) + Isnull(d.TT, 0) ) * Isnull(w.HourlyRate, 0))) )AS net \n");
            //varname1.Append("--( Mat + Labor )           AS TotalExp,  \n");
            //varname1.Append("--( Rev - ( Mat + Labor ) ) AS net  \n");
            //varname1.Append("FROM   Job j \n");
            //varname1.Append("       left outer JOIN TicketD d \n");
            //varname1.Append("               ON d.Job = j.ID \n");
            //if (!string.IsNullOrEmpty(objPropCustomer.StartDate))
            //{
            //    varname1.Append(" and d.edate >='" + objPropCustomer.StartDate + "'\n");
            //}
            //if (!string.IsNullOrEmpty(objPropCustomer.EndDate))
            //{
            //    varname1.Append(" and d.edate <'" + Convert.ToDateTime(objPropCustomer.EndDate).AddDays(1) + "'");
            //}
            ////varname1.Append("       left outer JOIN invoice i \n");
            ////varname1.Append("               ON i.Job = j.ID \n");
            ////if (objPropCustomer.StartDate != string.Empty)
            ////{
            ////    varname1.Append(" and i.fdate >='" + objPropCustomer.StartDate + "'\n");
            ////}
            ////if (objPropCustomer.EndDate != string.Empty)
            ////{
            ////    varname1.Append(" and i.fdate <'" + Convert.ToDateTime(objPropCustomer.EndDate).AddDays(1) + "'");
            ////}
            //varname1.Append("       LEFT OUTER JOIN tblWork w \n");
            //varname1.Append("                    ON w.ID = d.fWork \n");
            //varname1.Append("                    where j.id is not null  \n");

            //if (objPropCustomer.SearchBy != string.Empty && objPropCustomer.SearchBy != null)
            //{
            //    if (objPropCustomer.SearchBy == "j.fdate")
            //    {
            //        varname1.Append(" and " + objPropCustomer.SearchBy + " = '" + objPropCustomer.SearchValue + "' \n");
            //    }
            //    else if (objPropCustomer.SearchBy == "l.loc")
            //    {
            //        varname1.Append(" and " + objPropCustomer.SearchBy + " = " + objPropCustomer.SearchValue + " \n");
            //    }
            //    else if (objPropCustomer.SearchBy == "j.status")
            //    {
            //        varname1.Append(" and " + objPropCustomer.SearchBy + " = " + objPropCustomer.SearchValue + " \n");
            //    }
            //    else
            //    {
            //        varname1.Append(" and " + objPropCustomer.SearchBy + " like '" + objPropCustomer.SearchValue + "%' \n");
            //    }
            //}
            //if (objPropCustomer.StartDate != string.Empty)
            //{
            //    varname1.Append(" and d.edate >='" + objPropCustomer.StartDate + "'\n");
            //    //varname1.Append(" and i.fdate >='" + objPropCustomer.StartDate + "'\n");
            //}
            //if (objPropCustomer.EndDate != string.Empty)
            //{
            //    varname1.Append(" and d.edate <'" +objPropCustomer.EndDate+ "'");
            //   // varname1.Append(" and i.fdate <'" + objPropCustomer.EndDate + "'");
            //}

            //varname1.Append("GROUP  BY j.ID, \n");
            //varname1.Append("          j.fdesc, \n");
            //varname1.Append("          j.Status, \n");
            //varname1.Append("          j.Loc, \n");
            //varname1.Append("          j.fDate, \n");
            //varname1.Append("          j.Rev, \n");
            //varname1.Append("          j.PO, \n");
            //varname1.Append("          j.Mat, \n");
            //varname1.Append("          j.Labor ");

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetJobProject", objPropCustomer.SearchBy, objPropCustomer.SearchValue, objPropCustomer.StartDate, objPropCustomer.EndDate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddCustomBomT(Customer objPropCustomer)
        {
            var para = new SqlParameter[4];

            para[0] = new SqlParameter
            {
                ParameterName = "@Label",
                SqlDbType = SqlDbType.VarChar,
                Value = objPropCustomer.label
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@TabID",
                SqlDbType = SqlDbType.Int,
                Value = objPropCustomer.Tab
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Percentage",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.Percentage
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@Amount",
                SqlDbType = SqlDbType.Decimal,
                Value = objPropCustomer.PerAmount
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, CommandType.StoredProcedure, "spAddCustomEstimate", para);
                //SqlHelper.ExecuteNonQuery(_objJob.ConnConfig, "spDeleteProjectTemplate", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectTemplate(Customer objPropCustomer)
        {
            string strQuery = "select  j.id,jt.Type as Dept, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count, j.Type from JobT j, JobType jt where j.Type=jt.ID order by j.ID ";

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getJobProjectTemp(Customer objPropCustomer)
        {
            string strQuery = "select  j.id, j.fdesc, case j.status when 0 then 'Active' when 1 then 'Inactive' end as status, j.status as jStatus, isnull(j.TemplateRev,'') as TemplateRev, isnull(j.Count,0) as Count  from JobT j order by j.ID ";

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getWage(Customer objPropCustomer)
        {
            string strQuery = "select id,fdesc,remarks from PRWage where Field = 1 ";

            try
            {
                return SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobProjectByJobID(Customer objPropCustomer)
        {

            //StringBuilder varname1 = new StringBuilder();
            //varname1.Append("   SELECT j.fdesc, \n");
            //varname1.Append("       j.remarks, \n");
            //varname1.Append("       isnull(j.owner,0) as owner, \n");
            //varname1.Append("       j.Loc, \n");
            //varname1.Append("       j.fdate, \n");
            //varname1.Append("       isnull(j.Status,0) as status, \n");
            //varname1.Append("       isnull(j.Type,0) as Type, \n");
            //varname1.Append("       j.Ctype, \n");
            //varname1.Append("       j.PO, \n");
            //varname1.Append("       j.SO, \n");
            //varname1.Append("       j.Certified, \n");
            //varname1.Append("       j.ProjCreationDate, \n");
            //varname1.Append("       j.Custom21, \n");
            //varname1.Append("       j.Custom22, \n");
            //varname1.Append("       j.Custom23, \n");
            //varname1.Append("       j.Custom24, \n");
            //varname1.Append("       j.Custom25, \n");
            //varname1.Append("       j.template, \n");
            //varname1.Append("       r.address, \n");
            //varname1.Append("       r.city, \n");
            //varname1.Append("       r.state, \n");
            //varname1.Append("       r.zip, \n");
            //varname1.Append("       (SELECT tag \n");
            //varname1.Append("        FROM   Loc \n");
            //varname1.Append("        WHERE  Loc = j.Loc) AS locname, \n");
            //varname1.Append("        isnull(r.Name,'') as customerName, \n");
            //varname1.Append("       j.id, \n");
            //varname1.Append("       (SELECT id \n");
            //varname1.Append("        FROM   estimate \n");
            //varname1.Append("        WHERE  job = j.id)  AS estimateid, \n");
            //varname1.Append("       (SELECT NAME \n");
            //varname1.Append("        FROM   estimate \n");
            //varname1.Append("        WHERE  job = j.id)  AS estimate \n");
            //varname1.Append("   FROM   Job j LEFT JOIN Owner o ON o.ID = j.Owner \n");
            //varname1.Append("   LEFT JOIN Rol r ON r.ID = o.Rol \n");
            //varname1.Append("   WHERE  j.id = " + objPropCustomer.ProjectJobID);
            //varname1.Append("   \n");
            //varname1.Append("   SELECT *, \n");
            //varname1.Append("    case when isnull(Actual,0) <> 0 then CONVERT(NUMERIC(30, 2), ( ( budget - Actual ) / Actual ) * 100) else 0 end AS [percent] \n");
            //varname1.Append("   FROM   (SELECT j.ID, \n");
            //varname1.Append("               j.code+' : '+j.fdesc as billtype, \n");
            //varname1.Append("               j.JobT, \n");
            //varname1.Append("               j.Job, \n");
            //varname1.Append("               j.Type, \n");
            //varname1.Append("               j.fDesc, \n");
            //varname1.Append("               j.Code, \n");
            //varname1.Append("               j.Line, \n");
            //varname1.Append("               Isnull(j.budget, 0) AS budget, \n");
            //varname1.Append("               CASE \n");
            //varname1.Append("                 WHEN Isnull(j.Actual, 0) = 0 THEN ( CONVERT(NUMERIC(30, 2), (SELECT ( Isnull(d.Reg, 0) + ( Isnull(d.OT, 0) * 1.5 ) + ( Isnull(d.DT, 0) * 2 ) + ( Isnull(d.NT, 0) * 1.7 ) + Isnull(d.TT, 0) ) * (SELECT Isnull(w.HourlyRate, 0) \n");
            //varname1.Append("                                                                                                                                                                                                                 FROM   tblWork w \n");
            //varname1.Append("                                                                                                                                                                                                                 WHERE  w.ID = d.fWork) \n");
            //varname1.Append("                                                                              FROM   TicketD d \n");
            //varname1.Append("                                                                              WHERE  Isnull(d.JobCode, 0) = j.Code \n");
            //varname1.Append("                                                                                     AND Isnull(d.Job, 0) = j.Job)) ) \n");
            //varname1.Append("                 ELSE j.Actual \n");
            //varname1.Append("               END                 AS Actual \n");
            //varname1.Append("        FROM   jobtitem j ");
            //varname1.Append("        WHERE  j.job = " + objPropCustomer.ProjectJobID );
            //if (objPropCustomer.Type != string.Empty && objPropCustomer.Type != null)
            //    varname1.Append(" and j.type =" + objPropCustomer.Type);
            //varname1.Append( " ) AS tab ");


            //string strQuery = "select j.fdesc, j.remarks, j.Loc,j.template,(select tag from Loc where Loc = j.Loc) as locname,j.id, (select id from estimate where job = j.id)as estimateid,(select name from estimate where job = j.id)as estimate   from Job j where j.id=" + objPropCustomer.ProjectJobID;
            //strQuery += " select * from jobtitem j where j.job=" + objPropCustomer.ProjectJobID;

            try
            {
                //return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, varname1.ToString());
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectByJobID", objPropCustomer.ProjectJobID, objPropCustomer.Type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataSet getJobTemplateByID(Customer objPropCustomer)
        {
            //string strQuery = "declare @GLIntName varchar(75) select @GLIntName=c.fdesc from JobT j left join Chart c on j.GLInt = c.ID where j.ID=" + objPropCustomer.ProjectJobID;
            //strQuery += " select j.*,p.fDesc as WageName, i.Name as InvServiceName, c.fDesc as InvExpName, @GLIntName as GLName from JobT j left join PRWage p on j.Wage = p.ID left join Inv i on j.InvServ = i.ID left join Chart c on j.InvExp = c.ID where j.id=" + objPropCustomer.ProjectJobID;
            //strQuery += " select * from jobtitem j where j.job is null and j.jobT=" + objPropCustomer.ProjectJobID;

            try
            {
                //return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, CommandType.Text, strQuery);
                return objPropCustomer.DsCustomer = SqlHelper.ExecuteDataset(objPropCustomer.ConnConfig, "spGetProjectTemplateByID", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAllCustomers(Loc objLoc)
        {
            try
            {
                return objLoc.DsLoc = SqlHelper.ExecuteDataset(objLoc.ConnConfig, CommandType.Text, "SELECT Loc,ID,Tag FROM Loc Order by Tag ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet getAllLocationOnCustomer(Loc objLoc, int _ownerId)
        {
            try
            {
                return objLoc.DsLoc = SqlHelper.ExecuteDataset(objLoc.ConnConfig, CommandType.Text, "SELECT Loc,ID,Tag FROM Loc where Owner =" + _ownerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteProject(Customer objPropCustomer)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objPropCustomer.ConnConfig, "spdeleteproject", objPropCustomer.ProjectJobID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateCustomerBalance(Owner _objOwner)
        {
            try
            {
                string query = "UPDATE Owner SET Balance = @Balance WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objOwner.ID));
                parameters.Add(new SqlParameter("@Balance", _objOwner.Balance));
                SqlHelper.ExecuteNonQuery(_objOwner.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOwnerByID(Owner _objOwner)
        {
            try
            {
                return _objOwner.Ds = SqlHelper.ExecuteDataset(_objOwner.ConnConfig, CommandType.Text, "SELECT ID,Status,Locs,Elevs,Balance,Type,Billing,Central,Rol,Internet,TicketO,TicketD,Ledger,Request,Password,fLogin,Statement,Custom1,Custom2,NeedsFullSync,MerchantServicesId,idCreditCardDefault,QBCustomerID,msmuser,msmpass,SageID,CPEquipment,OwnerID FROM Owner WHERE ID="+_objOwner.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetOwnerByLoc(Owner _objOwner)
        {
            try
            {
                return _objOwner.Ds = SqlHelper.ExecuteDataset(_objOwner.ConnConfig, CommandType.Text, "SELECT l.Loc,o.ID,o.Status,o.Locs,o.Elevs,o.Balance,o.Type,o.Billing,o.Central,o.Rol,o.Internet,o.TicketO,o.TicketD,o.Ledger,o.Request,o.Password,o.fLogin,o.Statement,o.Custom1,o.Custom2,o.NeedsFullSync,o.MerchantServicesId,o.idCreditCardDefault,o.QBCustomerID,o.msmuser,o.msmpass,o.SageID,o.CPEquipment,o.OwnerID FROM Owner o, Loc l WHERE l.Owner=o.ID AND l.Loc=" + _objOwner.Locs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void DeleteProjectTemplate(JobT _objJob)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(_objJob.ConnConfig, "spDeleteProjectTemplate", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public double GetCustomerBalanceByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, "SELECT isnull(Balance,0) as Balance FROM Owner where ID=" + objPropCustomer.CustomerID));
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public double GetLocBalanceByID(Customer objPropCustomer)
        {
            try
            {
                return objPropCustomer.Balance = Convert.ToDouble(SqlHelper.ExecuteScalar(objPropCustomer.ConnConfig, CommandType.Text, "SELECT isnull(Balance,0) as Balance FROM Loc where Loc=" + objPropCustomer.CustomerID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int UpdateProjectTemplate(JobT _objJob)
        {
            var para = new SqlParameter[27];

            para[0] = new SqlParameter
            {
                ParameterName = "jobT",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "fdesc",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Type
            };
            para[3] = new SqlParameter
            {
                ParameterName = "NRev",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NRev
            };
            para[4] = new SqlParameter
            {
                ParameterName = "NDed",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.NDed
            };
            para[5] = new SqlParameter
            {
                ParameterName = "Count",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Count
            };
            para[6] = new SqlParameter
            {
                ParameterName = "Remarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.Remarks
            };
            para[7] = new SqlParameter
            {
                ParameterName = "InvExp",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvExp
            };
            para[8] = new SqlParameter
            {
                ParameterName = "InvServ",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.InvServ
            };
            para[9] = new SqlParameter
            {
                ParameterName = "Wage",
                SqlDbType = SqlDbType.Int,
                Value = _objJob.Wage
            };
            para[10] = new SqlParameter
            {
                ParameterName = "CType",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.CType
            };
            para[11] = new SqlParameter
            {
                ParameterName = "Status",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "Charge",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Charge
            };
            para[13] = new SqlParameter
            {
                ParameterName = "Post",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.Post
            };
            para[14] = new SqlParameter
            {
                ParameterName = "fInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.fInt
            };
            para[15] = new SqlParameter
            {
                ParameterName = "GLInt",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.GLInt
            };
            para[16] = new SqlParameter
            {
                ParameterName = "JobClose",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.JobClose
            };
            para[17] = new SqlParameter
            {
                ParameterName = "tempRev",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.TemplateRev
            };
            para[18] = new SqlParameter
            {
                ParameterName = "RevRemarks",
                SqlDbType = SqlDbType.VarChar,
                Value = _objJob.RevRemarks
            };
            para[19] = new SqlParameter
            {
                ParameterName = "alertType",
                SqlDbType = SqlDbType.SmallInt,
                Value = _objJob.AlertType
            };
            para[20] = new SqlParameter
            {
                ParameterName = "alertMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.AlertMgr
            };
            para[21] = new SqlParameter
            {
                ParameterName = "MilestoneMgr",
                SqlDbType = SqlDbType.Bit,
                Value = _objJob.MilestoneMgr
            };
            para[22] = new SqlParameter
            {
                ParameterName = "BomItem",
                SqlDbType = SqlDbType.Structured,
                Value = _objJob.ProjectDt
            };
            if(_objJob.MilestoneDt != null)
            {
                if (_objJob.MilestoneDt.Rows.Count > 0)
                {
                    para[23] = new SqlParameter
                    {
                        ParameterName = "MilestonItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.MilestoneDt
                    };
                }
            }
            if(_objJob.CustomTabItem != null)
            {
                if(_objJob.CustomTabItem.Rows.Count > 0)
                {
                    para[24] = new SqlParameter
                    {
                        ParameterName = "CustomTabItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomTabItem
                    };
                }
            }
            if (_objJob.CustomItem != null)
            {
                if (_objJob.CustomItem.Rows.Count > 0)
                {
                    para[25] = new SqlParameter
                    {
                        ParameterName = "CustomItem",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItem
                    };
                }
            }
            if (_objJob.CustomItemDelete != null)
            {
                if (_objJob.CustomItemDelete.Rows.Count > 0)
                {
                    para[26] = new SqlParameter
                    {
                        ParameterName = "CustomItemDelete",
                        SqlDbType = SqlDbType.Structured,
                        Value = _objJob.CustomItemDelete
                    };
                }
            }

            try
            {
                return Convert.ToInt32(SqlHelper.ExecuteScalar(_objJob.ConnConfig, CommandType.StoredProcedure, "spUpdateProjectTemplate", para));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateTemplateStatus(JobT _objJob)
        {
            try
            {
                SqlHelper.ExecuteScalar(_objJob.ConnConfig, "spUpdateProjectStatus", _objJob.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
    }
}
