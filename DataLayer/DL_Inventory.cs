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
    public class DL_Inventory
    {
        public DataSet GetInventory(Inventory _objInv)
        {
            DataSet ds = new DataSet();
            try
            {
                if (HttpContext.Current.Session["Inventory"] != null)
                {

                    ds = ((DataSet)HttpContext.Current.Session["Inventory"]);

                }
                else
                {

                    ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.StoredProcedure, Inventory.GET_ALL_INVENTORY);

                }

                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetALLInventory(Inventory _objInv)
        {
            DataSet ds = null;
            try
            {


                ds = SqlHelper.ExecuteDataset(_objInv.ConnConfig, CommandType.StoredProcedure, Inventory.GET_ALL_INVENTORY);



                _objInv.Ds = ds;

                HttpContext.Current.Session["Inventory"] = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet GetInventoryByID(Inventory _objInv)
        {
            DataSet ds = null;
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_ALL_INVENTORY_BY_ID, _objInv.ID);



                _objInv.Ds = ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }


        public DataSet DeleteInventory(string inventoryxml)
        {
            DataSet ds = null;
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_DELETE_INVENTORY_BULK, constring);


                Inventory _objInv = new Inventory();
                _objInv.ConnConfig = constring;
                HttpContext.Current.Session["Inventory"] = GetALLInventory(_objInv);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;

        }


        public Inventory CreateInventory(string xml, Inventory inv)
        {
            Inventory success = inv;


            try
            {


                string constring = HttpContext.Current.Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();
                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventoryCreate"))
                    {
                        try
                        {

                            int id = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_XML, xml);
                            success.ID = id;
                            if (inv.ApprovedVendors != null)
                            {
                                foreach (InventoryManufacturerInformation _objManInv in inv.ApprovedVendors)
                                {
                                    _objManInv.InventoryID = inv.ID;



                                    _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_MANUFACTURER_INFORMATION, _objManInv.InventoryID, _objManInv.MPN, _objManInv.ApprovedManufacturer, _objManInv.ApprovedVendor);
                                }
                            }

                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback("TransactionInventoryCreate");
                        }
                    }

                }










            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }



        public void UpdateInventory(string xml, Inventory inv)
        {



            try
            {

                DataSet dsAppManuinfo = GetInventoryManufactureInfo(inv.ID);

                string constring = HttpContext.Current.Session["config"].ToString();
                using (SqlConnection con = new SqlConnection(constring))
                {
                    con.Open();

                    using (SqlTransaction tans = con.BeginTransaction("TransactionInventory"))
                    {

                        try
                        {

                            SqlHelper.ExecuteScalar(tans, Inventory.UPDATE_INVENTORY_XML, xml);




                            if (inv.ApprovedVendors != null)
                            {
                                foreach (InventoryManufacturerInformation _objManInv in inv.ApprovedVendors)
                                {
                                    _objManInv.InventoryID = inv.ID;

                                    if (_objManInv.ID != 0)//Update when you have and ID associated with the vendor information
                                        SqlHelper.ExecuteNonQuery(tans, Inventory.UPDATE_INVENTORY_MANUFACTURER_INFORMATION, _objManInv.ID, _objManInv.InventoryID, _objManInv.MPN, _objManInv.ApprovedManufacturer, _objManInv.ApprovedVendorId);
                                    else//create vendors
                                        _objManInv.ID = (int)SqlHelper.ExecuteScalar(tans, Inventory.CREATE_INVENTORY_MANUFACTURER_INFORMATION, _objManInv.InventoryID, _objManInv.MPN, _objManInv.ApprovedManufacturer, _objManInv.ApprovedVendorId);
                                }

                                //check for the list to be deleted
                                if (dsAppManuinfo != null)
                                {
                                    if (dsAppManuinfo.Tables.Count > 0)
                                    {
                                        if (dsAppManuinfo.Tables[0].Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dsAppManuinfo.Tables[0].Rows.Count; i++)
                                            {
                                                int ID = Convert.ToInt32(dsAppManuinfo.Tables[0].Rows[i]["ID"]);

                                                if (!inv.ApprovedVendors.Any(x => x.ID != 0 & x.ID == ID))
                                                {
                                                    SqlHelper.ExecuteNonQuery(tans, Inventory.DELETE_INVENTORY_MANUFACTURER_INFORMATION, ID);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            tans.Commit();


                        }
                        catch (Exception ex)
                        {

                            tans.Rollback("TransactionInventory");

                            throw ex;
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }




        }

        public int CreateInventoryManufacturerInformation(InventoryManufacturerInformation _objManInv)
        {
            int success = 0;


            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();


                // SqlHelper.ExecuteScalar(transaction, Inventory.CREATE_INVENTORY_XML, xml);

                SqlHelper.ExecuteScalar(constring, Inventory.CREATE_INVENTORY_MANUFACTURER_INFORMATION, _objManInv.InventoryID, _objManInv.MPN, _objManInv.ApprovedManufacturer, _objManInv.ApprovedVendor);


            }
            catch (Exception ex)
            {

                throw ex;
            }


            return success;

        }


        public DataSet GetInventoryManufactureInfo(int _objInv)
        {
            DataSet ds = new DataSet();
            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID, _objInv);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public void DeleteInventoryManufactureInfo(int _objManInvid)
        {

            try
            {

                string constring = HttpContext.Current.Session["config"].ToString();
                SqlHelper.ExecuteNonQuery(constring, Inventory.DELETE_INVENTORY_MANUFACTURER_INFORMATION, _objManInvid);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataSet GetItemQuantity()
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, CommandType.StoredProcedure, Inventory.GET_ALL_ITEM_QUANTITY);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetItemPurchaseOrder(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_ALL_ITEM_PURCHASE_ORDER, ID);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public DataSet GetInvManufacturerInfoByInvAndVendorId(int InventoryId, int ApprovedVendorId)
        {
            DataSet ds = new DataSet();
            try
            {
                string constring = HttpContext.Current.Session["config"].ToString();

                ds = SqlHelper.ExecuteDataset(constring, Inventory.GET_INVENTORY_MANUFACTURER_INFORMATION_BY_INVENTORYID_APPROVEDVENDOR, InventoryId, ApprovedVendorId);



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ds;
        }

        public Commodity CreateCommodity(Commodity com)
        {
            Commodity comdt = com;
            try
            {
                comdt.ID = (int)SqlHelper.ExecuteScalar(com.ConnConfig, Commodity.CREATE_COMMODITY, com.Code, com.Desc);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public DataSet ReadCommodityById(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(com.ConnConfig, Commodity.GET_ALL_COMMODITY_BY_ID, com.ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public DataSet ReadAllCommodity(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = SqlHelper.ExecuteDataset(com.ConnConfig, Commodity.GET_ALL_COMMODITY_BY_ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        public Commodity UpdateCommodity(Commodity com)
        {
            Commodity comdt = com;
            try
            {
                SqlHelper.ExecuteScalar(com.ConnConfig, Commodity.UPDATE_COMMODITY, com.ID, com.Code, com.Desc, com.IsActive);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        //public void DeleteTransInvoiceByRef(Transaction _objTrans)
        //{
        //    try
        //    {
        //        SqlHelper.ExecuteNonQuery(_objTrans.ConnConfig, CommandType.Text, " DELETE FROM Trans WHERE Ref = " + _objTrans.Ref + " AND Batch = " + _objTrans.BatchID);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public DataSet GetInvoiceByID(Invoices _objInvoice)
        //{
        //    try
        //    {
        //        return _objInvoice.Ds = SqlHelper.ExecuteDataset(_objInvoice.ConnConfig, CommandType.Text, "SELECT fDate,Ref,Batch,TransID FROM Invoice WHERE Ref=" + _objInvoice.Ref);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }

}
