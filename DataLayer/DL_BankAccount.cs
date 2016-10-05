using BusinessEntity;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DL_BankAccount
    {
        public int AddRol(Rol objRol)
        {
            var para = new SqlParameter[21];

            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Cellular
            };
            para[14] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.SmallInt,
                Value = objRol.Type
            };
            para[17] = new SqlParameter
            {
                ParameterName = "@GeoLock",
                SqlDbType = SqlDbType.Int,
                Value = objRol.GeoLock
            };
            if (objRol.Since != DateTime.MinValue)
            {
                para[18] = new SqlParameter
                {
                    ParameterName = "@Since",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objRol.Since
                };
            }
            if (objRol.Last != DateTime.MinValue)
            {
                para[19] = new SqlParameter
                {
                    ParameterName = "@Last",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objRol.Last
                };
            }
            para[20] = new SqlParameter
            {
                ParameterName = "returnval",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.ReturnValue
            };

            try
            {
                SqlHelper.ExecuteNonQuery(objRol.ConnConfig, CommandType.StoredProcedure,"spAddRolDetails", para);
                return Convert.ToInt32(para[20].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void AddBank(Bank objBank)
        {
            var para = new SqlParameter[17];

            para[1] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.fDesc
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@Rol",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Rol
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@NBranch",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NBranch
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@NAcct",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NAcct
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@NRoute",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NRoute
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@NextC",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextC
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@NextD",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextD
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@NextE",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextE
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Rate",
                SqlDbType = SqlDbType.Float,
                Value = objBank.Rate
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@CLimit",
                SqlDbType = SqlDbType.Float,
                Value = objBank.CLimit
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Warn",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Warn
            };

            para[14] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Status
            };
            para[16] = new SqlParameter
            {
                ParameterName = "@Chart",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Chart
            };
            try
            {
                SqlHelper.ExecuteNonQuery(objBank.ConnConfig, CommandType.StoredProcedure, "spAddBankDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetStates(State _objState)
        {
            try
            {
                return _objState.DsState = SqlHelper.ExecuteDataset(_objState.ConnConfig, CommandType.Text, "SELECT Name, fDesc, Country FROM State");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateRol(Rol objRol)
        {
            var para = new SqlParameter[14];

            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objRol.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@Name",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Name
            };
            para[2] = new SqlParameter
            {
                ParameterName = "@City",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.City
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@State",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.State
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@Zip",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Zip
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@Phone",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Phone
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@Fax",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Fax
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@Contact",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Contact
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Address",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Address
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@Email",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.EMail
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Website",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Website
            };
            para[11] = new SqlParameter
            {
                ParameterName = "@Country",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Country
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Cellular",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Cellular
            };
            para[13] = new SqlParameter
            {
                ParameterName = "@Type",
                SqlDbType = SqlDbType.VarChar,
                Value = objRol.Type
            };
            //para[14] = new SqlParameter
            //{
            //    ParameterName = "@Remarks",
            //    SqlDbType = SqlDbType.VarChar,
            //    Value = objRol.Remarks
            //};

            //para[17] = new SqlParameter
            //{
            //    ParameterName = "@GeoLock",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objRol.GeoLock
            //};
            try
            {
                SqlHelper.ExecuteDataset(objRol.ConnConfig, "spUpdateRolDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBank(Bank objBank)
        {
            var para = new SqlParameter[13];

            para[0] = new SqlParameter
            {
                ParameterName = "@ID",
                SqlDbType = SqlDbType.Int,
                Value = objBank.ID
            };
            para[1] = new SqlParameter
            {
                ParameterName = "@fDesc",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.fDesc
            };
            //para[2] = new SqlParameter
            //{
            //    ParameterName = "@Rol",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objBank.Rol
            //};
            para[2] = new SqlParameter
            {
                ParameterName = "@NBranch",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NBranch
            };
            para[3] = new SqlParameter
            {
                ParameterName = "@NAcct",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NAcct
            };
            para[4] = new SqlParameter
            {
                ParameterName = "@NRoute",
                SqlDbType = SqlDbType.VarChar,
                Value = objBank.NRoute
            };
            para[5] = new SqlParameter
            {
                ParameterName = "@NextC",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextC
            };
            para[6] = new SqlParameter
            {
                ParameterName = "@NextD",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextD
            };
            para[7] = new SqlParameter
            {
                ParameterName = "@NextE",
                SqlDbType = SqlDbType.Int,
                Value = objBank.NextE
            };
            para[8] = new SqlParameter
            {
                ParameterName = "@Rate",
                SqlDbType = SqlDbType.Float,
                Value = objBank.Rate
            };
            para[9] = new SqlParameter
            {
                ParameterName = "@CLimit",
                SqlDbType = SqlDbType.Float,
                Value = objBank.CLimit
            };
            para[10] = new SqlParameter
            {
                ParameterName = "@Warn",
                SqlDbType = SqlDbType.TinyInt,
                Value = objBank.Warn
            };

            para[11] = new SqlParameter
            {
                ParameterName = "@Status",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Status
            };
            para[12] = new SqlParameter
            {
                ParameterName = "@Rol",
                SqlDbType = SqlDbType.Int,
                Value = objBank.Rol
            };
            //para[12] = new SqlParameter
            //{
            //    ParameterName = "@Chart",
            //    SqlDbType = SqlDbType.Int,
            //    Value = objBank.Chart
            //};

            try
            {
                SqlHelper.ExecuteNonQuery(objBank.ConnConfig, CommandType.StoredProcedure, "spUpdateBankDetails", para);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankByChart(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT ID,fDesc,Rol,NBranch,NAcct,NRoute,NextC,NextD,NextE,Rate,CLimit,Warn,Recon,Balance,Status,InUse,Chart FROM Bank WHERE Chart=" + objBank.Chart);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetRolByID(Rol objRol)
        {
            try
            {
                return objRol.DsRol = SqlHelper.ExecuteDataset(objRol.ConnConfig, CommandType.Text, "SELECT ID,Name,City,State,Zip,Phone,Fax,Contact,Address,EMail,Website,Cellular,Country FROM Rol WHERE ID='" + objRol.ID + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet IsExistBankAcct(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT COUNT(*) AS CBANK FROM Bank WHERE Chart='" + objBank.Chart + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetAllBankNames(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT ID, fDesc FROM Bank");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataSet GetBankByID(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.*, c.Balance AS BankBalance FROM Bank b INNER JOIN Chart c ON b.Chart = c.ID WHERE b.ID = " + objBank.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalance(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Balance = @Balance WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@Balance", _objBank.Balance));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankBalanceNcheck(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Balance = @Balance, NextC = @NextC WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@NextC", _objBank.NextC));
                parameters.Add(new SqlParameter("@Balance", _objBank.Balance));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateBankRecon(Bank _objBank)
        {
            try
            {
                string query = "UPDATE Bank SET Recon = @Recon, LastReconDate=@LastReconDate WHERE ID = @ID";
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@ID", _objBank.ID));
                parameters.Add(new SqlParameter("@Recon", _objBank.Recon));
                parameters.Add(new SqlParameter("@LastReconDate", _objBank.LastReconDate));
                int rowsAffected = SqlHelper.ExecuteNonQuery(_objBank.ConnConfig, CommandType.Text, query, parameters.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetChartByBank(Bank _objBank)
        {
            try
            {
                return _objBank.Chart = Convert.ToInt32(SqlHelper.ExecuteScalar(_objBank.ConnConfig, CommandType.Text, "SELECT Isnull(Chart,0) FROM Bank WHERE ID=" + _objBank.ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int GetBankIDByChart(Bank _objBank)
        {
            try
            {
                return _objBank.ID = Convert.ToInt32(SqlHelper.ExecuteScalar(_objBank.ConnConfig, CommandType.Text, "SELECT Isnull(ID,0) FROM Bank WHERE Chart=" + _objBank.Chart));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public DataSet GetBankRolByID(Bank objBank)
        //{
        //    try
        //    {
        //        return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.ID,b.fDesc,b.Rol,b.NBranch,b.NAcct,b.NRoute,b.NextC,b.NextD,b.NextE,b.Rate,b.CLimit,b.Warn,b.Recon,b.Balance,b.Status,b.InUse,b.Chart,r. FROM Bank as b, Rol as r WHERE ID=" + objBank.ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public DataSet GetBankRolByID(Bank objBank)
        {
            try
            {
                return objBank.DsBank = SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.Text, "SELECT b.ID,b.fDesc,b.Rol,b.NBranch,b.NAcct,b.NRoute,b.NextC,b.NextD,b.NextE,b.Rate,b.CLimit,b.Warn,b.Recon,b.Balance,b.Status,b.InUse,b.Chart, r.Name as BankName FROM Bank as b, Rol as r WHERE b.Rol=r.ID and b.ID=" + objBank.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public void DeleteRolByID(Rol objRol)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(objRol.ConnConfig, CommandType.Text, " DELETE FROM Rol WHERE ID = " + objRol.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void BankRecon(Bank objBank)
        {
            try
            {
                var para = new SqlParameter[10];

                para[0] = new SqlParameter
                {
                    ParameterName = "@bank",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ID
                };
                para[1] = new SqlParameter
                {
                    ParameterName = "@endbalance",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.Balance
                };
                para[2] = new SqlParameter
                {
                    ParameterName = "@ReconDate",
                    SqlDbType = SqlDbType.DateTime,
                    Value = objBank.LastReconDate
                };
                para[3] = new SqlParameter
                {
                    ParameterName = "@ServiceChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.ServiceCharge
                };
                 para[4] = new SqlParameter
                {
                    ParameterName = "@ServiceAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.ServiceAcct
                };
                if(objBank.ServiceDate != System.DateTime.MinValue)
                {
                    para[5] = new SqlParameter
                    {
                        ParameterName = "@ServiceDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.ServiceDate
                    };
                }
                para[6] = new SqlParameter
                {
                    ParameterName = "@InterestChrg",
                    SqlDbType = SqlDbType.Decimal,
                    Value = objBank.InterestCharge
                };
                para[7] = new SqlParameter
                {
                    ParameterName = "@InterestAcct",
                    SqlDbType = SqlDbType.Int,
                    Value = objBank.InterestAcct
                };
                if(objBank.InterestDate != System.DateTime.MinValue)
                {
                    para[8] = new SqlParameter
                    {
                        ParameterName = "@InterestDate",
                        SqlDbType = SqlDbType.DateTime,
                        Value = objBank.InterestDate
                    };
                }
                para[9] = new SqlParameter
                {
                    ParameterName = "@BankRecon",
                    SqlDbType = SqlDbType.Structured,
                    Value = objBank.DtBank
                };

                SqlHelper.ExecuteDataset(objBank.ConnConfig, CommandType.StoredProcedure, "spBankRecon", para);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
