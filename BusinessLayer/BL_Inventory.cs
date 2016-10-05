using BusinessEntity;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Reflection;


namespace BusinessLayer
{
    public class BL_Inventory
    {
        DL_Inventory _objDLInventory = new DL_Inventory();
        DL_UnitOfMeasure _objUom = new DL_UnitOfMeasure();
        DL_Itype _objItype = new DL_Itype();
        DL_Chart _objChart = new DL_Chart();
        DL_User _objUser = new DL_User();
        DL_Vendor _objVendor = new DL_Vendor();


        public DataSet GetInventory(Inventory _objInv)
        {


            return _objDLInventory.GetInventory(_objInv);
        }
        public Inventory GetInventoryByID(Inventory _objInv)
        {
            DataSet ds = _objDLInventory.GetInventoryByID(_objInv);

            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            #region Inventory Info
                            _objInv.Name = ds.Tables[0].Rows[i]["Name"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Name"] : string.Empty;
                            _objInv.fDesc = ds.Tables[0].Rows[i]["fDesc"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["fDesc"] : string.Empty;
                            _objInv.Part = ds.Tables[0].Rows[i]["Part"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Part"] : string.Empty;
                            _objInv.Status = ds.Tables[0].Rows[i]["Status"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Status"]) : 0;
                            _objInv.SAcct = ds.Tables[0].Rows[i]["SAcct"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["SAcct"]) : 0;
                            _objInv.Measure = ds.Tables[0].Rows[i]["Measure"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Measure"] : "0";
                            _objInv.Tax = ds.Tables[0].Rows[i]["Tax"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Tax"]) : 0;
                            _objInv.Balance = ds.Tables[0].Rows[i]["Balance"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Balance"]) : 0;
                            _objInv.Price1 = ds.Tables[0].Rows[i]["Price1"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price1"]) : 0;
                            _objInv.Price2 = ds.Tables[0].Rows[i]["Price2"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price2"]) : 0;
                            _objInv.Price3 = ds.Tables[0].Rows[i]["Price3"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price3"]) : 0;
                            _objInv.Price4 = ds.Tables[0].Rows[i]["Price4"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price4"]) : 0;
                            _objInv.Price5 = ds.Tables[0].Rows[i]["Price5"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price5"]) : 0;
                            _objInv.Remarks = ds.Tables[0].Rows[i]["Remarks"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Remarks"] : string.Empty;
                            _objInv.Cat = ds.Tables[0].Rows[i]["Cat"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Cat"]) : 0;
                            _objInv.LVendor = ds.Tables[0].Rows[i]["LVendor"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["LVendor"]) : 0;
                            _objInv.LCost = ds.Tables[0].Rows[i]["LCost"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["LCost"]) : 0;
                            _objInv.AllowZero = ds.Tables[0].Rows[i]["AllowZero"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["AllowZero"]) : 0;
                            //_objInv.Type = ds.Tables[0].Rows[i]["Type"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Type"]) : 0;
                            _objInv.InUse = ds.Tables[0].Rows[i]["InUse"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["InUse"]) : 0;
                            _objInv.EN = ds.Tables[0].Rows[i]["EN"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["EN"]) : 0;
                            _objInv.Hand = ds.Tables[0].Rows[i]["Hand"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["Hand"]) : 0;
                            _objInv.Aisle = ds.Tables[0].Rows[i]["Aisle"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Aisle"] : string.Empty;
                            _objInv.fOrder = ds.Tables[0].Rows[i]["fOrder"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["fOrder"]) : 0;
                            _objInv.Min = ds.Tables[0].Rows[i]["Min"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Min"]) : 0;
                            _objInv.Shelf = ds.Tables[0].Rows[i]["Shelf"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Shelf"] : string.Empty;
                            _objInv.Bin = ds.Tables[0].Rows[i]["Bin"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Bin"] : string.Empty;
                            _objInv.Requ = ds.Tables[0].Rows[i]["Requ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Requ"]) : 0;
                            _objInv.Warehouse = ds.Tables[0].Rows[i]["Warehouse"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Warehouse"] : string.Empty;
                            _objInv.Price6 = ds.Tables[0].Rows[i]["Price6"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Price6"]) : 0;
                            _objInv.Committed = ds.Tables[0].Rows[i]["Committed"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Committed"]) : 0;

                            _objInv.QBInvID = ds.Tables[0].Rows[i]["QBInvID"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["QBInvID"] : string.Empty;
                            _objInv.LastUpdateDate = ds.Tables[0].Rows[i]["LastUpdateDate"] != DBNull.Value ? (DateTime)ds.Tables[0].Rows[i]["LastUpdateDate"] : (Nullable<DateTime>)null;
                            _objInv.QBAccountID = ds.Tables[0].Rows[i]["QBAccountID"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["QBAccountID"] : string.Empty;
                            _objInv.Available = ds.Tables[0].Rows[i]["Available"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["Available"]) : 0;
                            _objInv.IssuedOpenJobs = ds.Tables[0].Rows[i]["IssuedOpenJobs"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["IssuedOpenJobs"]) : 0;
                            _objInv.Description2 = ds.Tables[0].Rows[i]["Description2"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description2"] : string.Empty;
                            _objInv.Description3 = ds.Tables[0].Rows[i]["Description3"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description3"] : string.Empty;
                            _objInv.Description4 = ds.Tables[0].Rows[i]["Description4"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Description4"] : string.Empty;
                            _objInv.DateCreated = ds.Tables[0].Rows[i]["DateCreated"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["DateCreated"]) : (Nullable<DateTime>)null;
                            _objInv.Class = ds.Tables[0].Rows[i]["Class"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Class"] : string.Empty;
                            _objInv.Specification = ds.Tables[0].Rows[i]["Specification"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification"] : string.Empty;
                            _objInv.Specification2 = ds.Tables[0].Rows[i]["Specification2"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification2"] : string.Empty;
                            _objInv.Specification3 = ds.Tables[0].Rows[i]["Specification3"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification3"] : string.Empty;
                            _objInv.Specification4 = ds.Tables[0].Rows[i]["Specification4"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Specification4"] : string.Empty;
                            _objInv.Revision = ds.Tables[0].Rows[i]["Revision"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Revision"] : string.Empty;
                            _objInv.LastRevisionDate = ds.Tables[0].Rows[i]["LastRevisionDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["LastRevisionDate"]) : (Nullable<DateTime>)null;
                            _objInv.Eco = ds.Tables[0].Rows[i]["Eco"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Eco"] : string.Empty;
                            _objInv.Drawing = ds.Tables[0].Rows[i]["Drawing"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Drawing"] : string.Empty;
                            _objInv.Reference = ds.Tables[0].Rows[i]["Reference"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Reference"] : string.Empty;
                            _objInv.Length = ds.Tables[0].Rows[i]["Length"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Length"] : string.Empty;
                            _objInv.Width = ds.Tables[0].Rows[i]["Width"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Width"] : string.Empty;
                            _objInv.Weight = ds.Tables[0].Rows[i]["Weight"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Weight"] : string.Empty;
                            _objInv.InspectionRequired = ds.Tables[0].Rows[i]["InspectionRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["InspectionRequired"] : false;
                            _objInv.CoCRequired = ds.Tables[0].Rows[i]["CoCRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["CoCRequired"] : false;
                            _objInv.ShelfLife = ds.Tables[0].Rows[i]["ShelfLife"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["ShelfLife"]) : 0;
                            _objInv.SerializationRequired = ds.Tables[0].Rows[i]["SerializationRequired"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["SerializationRequired"] : false;
                            _objInv.GLcogs = ds.Tables[0].Rows[i]["GLcogs"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["GLcogs"] : string.Empty;
                            _objInv.GLPurchases = ds.Tables[0].Rows[i]["GLPurchases"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["GLPurchases"] : string.Empty;
                            _objInv.ABCClass = ds.Tables[0].Rows[i]["ABCClass"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["ABCClass"] : string.Empty;
                            _objInv.OHValue = ds.Tables[0].Rows[i]["OHValue"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OHValue"]) : 0;
                            _objInv.OOValue = ds.Tables[0].Rows[i]["OOValue"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["OOValue"]) : 0;
                            _objInv.OverIssueAllowance = ds.Tables[0].Rows[i]["OverIssueAllowance"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["OverIssueAllowance"] : false;
                            _objInv.UnderIssueAllowance = ds.Tables[0].Rows[i]["UnderIssueAllowance"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["UnderIssueAllowance"] : false;
                            _objInv.InventoryTurns = ds.Tables[0].Rows[i]["InventoryTurns"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["InventoryTurns"]) : 0;
                            _objInv.MOQ = ds.Tables[0].Rows[i]["MOQ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MOQ"]) : 0;
                            _objInv.EOQ = ds.Tables[0].Rows[i]["EOQ"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EOQ"]) : 0;
                            _objInv.MinInvQty = ds.Tables[0].Rows[i]["MinInvQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MinInvQty"]) : 0;
                            _objInv.MaxInvQty = ds.Tables[0].Rows[i]["MaxInvQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MaxInvQty"]) : 0;
                            _objInv.Commodity = ds.Tables[0].Rows[i]["Commodity"] != DBNull.Value ? (string)ds.Tables[0].Rows[i]["Commodity"] : string.Empty;
                            _objInv.LastReceiptDate = ds.Tables[0].Rows[i]["LastReceiptDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["LastReceiptDate"]) : (Nullable<DateTime>)null;
                            _objInv.EAU = ds.Tables[0].Rows[i]["EAU"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["EAU"]) : 0;
                            _objInv.EOLDate = ds.Tables[0].Rows[i]["EOLDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["EOLDate"]) : (Nullable<DateTime>)null;
                            _objInv.WarrantyPeriod = ds.Tables[0].Rows[i]["WarrantyPeriod"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["WarrantyPeriod"]) : (Nullable<int>)null;
                            _objInv.PODueDate = ds.Tables[0].Rows[i]["PODueDate"] != DBNull.Value ? Convert.ToDateTime(ds.Tables[0].Rows[i]["PODueDate"]) : (Nullable<DateTime>)null;
                            _objInv.DefaultReceivingLocation = ds.Tables[0].Rows[i]["DefaultReceivingLocation"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["DefaultReceivingLocation"] : false;
                            _objInv.DefaultInspectionLocation = ds.Tables[0].Rows[i]["DefaultInspectionLocation"] != DBNull.Value ? (bool)ds.Tables[0].Rows[i]["DefaultInspectionLocation"] : false;
                            _objInv.LastSalePrice = ds.Tables[0].Rows[i]["LastSalePrice"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["LastSalePrice"]) : 0;
                            _objInv.AnnualSalesQty = ds.Tables[0].Rows[i]["AnnualSalesQty"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AnnualSalesQty"]) : 0;
                            _objInv.AnnualSalesAmt = ds.Tables[0].Rows[i]["AnnualSalesAmt"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["AnnualSalesAmt"]) : 0;
                            _objInv.QtyAllocatedToSO = ds.Tables[0].Rows[i]["QtyAllocatedToSO"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["QtyAllocatedToSO"]) : 0;
                            _objInv.MaxDiscountPercentage = ds.Tables[0].Rows[i]["MaxDiscountPercentage"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["MaxDiscountPercentage"]) : 0;
                            _objInv.Height = ds.Tables[0].Rows[i]["Height"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["Height"]) : string.Empty;
                            _objInv.UnitCost = ds.Tables[0].Rows[i]["UnitCost"] != DBNull.Value ? Convert.ToDecimal(ds.Tables[0].Rows[i]["UnitCost"]) : 0;
                            _objInv.GLSales = ds.Tables[0].Rows[i]["GLSales"] != DBNull.Value ? Convert.ToString(ds.Tables[0].Rows[i]["GLSales"]) : string.Empty;
                            _objInv.LeadTime = ds.Tables[0].Rows[i]["LeadTime"] != DBNull.Value ? Convert.ToInt32(ds.Tables[0].Rows[i]["LeadTime"]) : 0;
                            #endregion
                        }

                    }
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<InventoryManufacturerInformation> invVendor = new List<InventoryManufacturerInformation>();

                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            #region Vendor Info
                            InventoryManufacturerInformation vendorinfo = new InventoryManufacturerInformation();
                            vendorinfo.ID = ds.Tables[1].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["ID"]) : 0;
                            vendorinfo.InventoryID = ds.Tables[1].Rows[i]["InventoryManufacturerInformation_InvID"] != DBNull.Value ? Convert.ToInt32(ds.Tables[1].Rows[i]["InventoryManufacturerInformation_InvID"]) : 0;
                            vendorinfo.MPN = ds.Tables[1].Rows[i]["MPN"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["MPN"] : string.Empty;
                            vendorinfo.ApprovedManufacturer = ds.Tables[1].Rows[i]["ApprovedManufacturer"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["ApprovedManufacturer"] : string.Empty;
                            vendorinfo.ApprovedVendor = ds.Tables[1].Rows[i]["Acct"] != DBNull.Value ? (string)ds.Tables[1].Rows[i]["Acct"] : string.Empty;
                            vendorinfo.ApprovedVendorId = ds.Tables[1].Rows[i]["ApprovedVendorId"] != DBNull.Value ? Convert.ToString(ds.Tables[1].Rows[i]["ApprovedVendorId"]) : string.Empty;

                            invVendor.Add(vendorinfo);
                            #endregion
                        }

                        _objInv.ApprovedVendors = invVendor.ToArray();

                    }

                }


            }


            return _objInv;
        }
        public DataSet GetAllInventory(Inventory _objInv)
        {


            return _objDLInventory.GetALLInventory(_objInv);
        }

        public DataSet DeleteInventory(string inventoryxml)
        {
            return _objDLInventory.DeleteInventory(inventoryxml);
        }

        public List<UnitOfMeasure> GetALLUnitOfMeasure()
        {


            return _objUom.GetALLUnitOfMeasure();
        }

        public List<Itype> GetALLItype()
        {
            return _objItype.GetALLItype();
        }

        public List<Chart> GetChartByType(int type)
        {
            return _objChart.GetChartByType(type);
        }

        public Inventory CreateInventory(Inventory inv)
        {
            Inventory invdt = null;

            if (inv != null)
            {

                string strxmlparam = string.Empty;
                strxmlparam += "<Inventory>";
                strxmlparam += "<Item>";
                #region Xml Param
                strxmlparam += "<Name>" + inv.Name + "</Name>";
                strxmlparam += "<fDesc>" + inv.fDesc + "</fDesc>";
                strxmlparam += "<Part>" + inv.MPN + "</Part>";
                strxmlparam += "<Status>" + inv.Status + "</Status>";
                strxmlparam += "<SAcct>" + inv.SAcct + "</SAcct>";
                strxmlparam += "<Measure>" + inv.Measure + "</Measure>";
                strxmlparam += "<Tax>" + inv.Tax + "</Tax>";
                strxmlparam += "<Balance>" + inv.Balance + "</Balance>";
                strxmlparam += "<Price1>" + inv.Price1 + "</Price1>";
                strxmlparam += "<Price2>" + inv.Price2 + "</Price2>";
                strxmlparam += "<Price3>" + inv.Price3 + "</Price3>";
                strxmlparam += "<Price4>" + inv.Price4 + "</Price4>";
                strxmlparam += "<Price5>" + inv.Price5 + "</Price5>";
                strxmlparam += "<Remarks>" + inv.Remarks + "</Remarks>";
                strxmlparam += "<Cat>" + inv.Cat + "</Cat>";
                strxmlparam += "<LVendor>" + inv.LVendor + "</LVendor>";
                strxmlparam += "<LCost>" + inv.LCost + "</LCost>";
                strxmlparam += "<AllowZero>" + inv.AllowZero + "</AllowZero>";
                // strxmlparam += "<Type>" + inv.Type + "</Type>";
                strxmlparam += "<InUse>" + inv.InUse + "</InUse>";
                strxmlparam += "<EN>" + inv.EN + "</EN>";
                strxmlparam += "<Hand>" + inv.Hand + "</Hand>";
                strxmlparam += "<Aisle>" + inv.Aisle + "</Aisle>";
                strxmlparam += "<fOrder>" + inv.fOrder + "</fOrder>";
                strxmlparam += "<Min>" + inv.Min + "</Min>";
                strxmlparam += "<Shelf>" + inv.Shelf + "</Shelf>";
                strxmlparam += "<Bin>" + inv.Bin + "</Bin>";
                strxmlparam += "<Requ>" + inv.Requ + "</Requ>";
                strxmlparam += "<Warehouse>" + inv.Warehouse + "</Warehouse>";
                strxmlparam += "<Price6>" + inv.Price6 + "</Price6>";
                strxmlparam += "<Committed>" + inv.Committed + "</Committed>";
                strxmlparam += "<QBInvID>" + inv.QBInvID + "</QBInvID>";
                strxmlparam += "<LastUpdateDate>" + inv.LastUpdateDate + "</LastUpdateDate>";
                strxmlparam += "<QBAccountID>" + inv.QBAccountID + "</QBAccountID>";
                strxmlparam += "<Available>" + inv.Available + "</Available>";
                strxmlparam += "<IssuedOpenJobs>" + inv.IssuedOpenJobs + "</IssuedOpenJobs>";
                strxmlparam += "<Description2>" + inv.Description2 + "</Description2>";
                strxmlparam += "<Description3>" + inv.Description3 + "</Description3>";
                strxmlparam += "<Description4>" + inv.Description4 + "</Description4>";
                if (inv.DateCreated != null & inv.DateCreated != DateTime.MinValue)
                {
                    strxmlparam += "<DateCreated>" + inv.DateCreated + "</DateCreated>";
                }
                else
                {
                    strxmlparam += "<DateCreated></DateCreated>";
                }

                strxmlparam += "<Specification>" + inv.Specification + "</Specification>";
                strxmlparam += "<Specification2>" + inv.Specification2 + "</Specification2>";
                strxmlparam += "<Specification3>" + inv.Specification2 + "</Specification3>";
                strxmlparam += "<Specification4>" + inv.Specification4 + "</Specification4>";
                strxmlparam += "<Revision>" + inv.Revision + "</Revision>";
                if (inv.LastRevisionDate != null & inv.LastRevisionDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastRevisionDate>" + inv.LastRevisionDate + "</LastRevisionDate>";
                }
                else
                {
                    strxmlparam += "<LastRevisionDate></LastRevisionDate>";
                }
                strxmlparam += "<Eco>" + inv.Eco + "</Eco>";
                strxmlparam += "<Drawing>" + inv.Drawing + "</Drawing>";
                strxmlparam += "<Reference>" + inv.Reference + "</Reference>";
                strxmlparam += "<Length>" + inv.Length + "</Length>";
                strxmlparam += "<Width>" + inv.Width + "</Width>";
                strxmlparam += "<Weight>" + inv.Weight + "</Weight>";
                strxmlparam += "<InspectionRequired>" + inv.InspectionRequired + "</InspectionRequired>";
                strxmlparam += "<CoCRequired>" + inv.CoCRequired + "</CoCRequired>";
                strxmlparam += "<ShelfLife>" + inv.ShelfLife + "</ShelfLife>";
                strxmlparam += "<SerializationRequired>" + inv.SerializationRequired + "</SerializationRequired>";
                strxmlparam += "<GLcogs>" + inv.GLcogs + "</GLcogs>";
                strxmlparam += "<GLPurchases>" + inv.GLPurchases + "</GLPurchases>";
                strxmlparam += "<ABCClass>" + inv.ABCClass + "</ABCClass>";
                strxmlparam += "<OHValue>" + inv.OHValue + "</OHValue>";
                strxmlparam += "<OOValue>" + inv.OOValue + "</OOValue>";
                strxmlparam += "<OverIssueAllowance>" + inv.OverIssueAllowance + "</OverIssueAllowance>";
                strxmlparam += "<UnderIssueAllowance>" + inv.UnderIssueAllowance + "</UnderIssueAllowance>";
                strxmlparam += "<InventoryTurns>" + inv.InventoryTurns + "</InventoryTurns>";
                strxmlparam += "<MOQ>" + inv.MOQ + "</MOQ>";
                strxmlparam += "<MinInvQty>" + inv.MinInvQty + "</MinInvQty>";
                strxmlparam += "<MaxInvQty>" + inv.MaxInvQty + "</MaxInvQty>";
                strxmlparam += "<Commodity>" + inv.Commodity + "</Commodity>";

                if (inv.LastReceiptDate != null & inv.LastReceiptDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastReceiptDate>" + inv.LastReceiptDate + "</LastReceiptDate>";
                }
                else
                {
                    strxmlparam += "<LastReceiptDate></LastReceiptDate>";
                }


                strxmlparam += "<EAU>" + inv.EAU + "</EAU>";
                if (inv.EOLDate != null & inv.EOLDate != DateTime.MinValue)
                {
                    strxmlparam += "<EOLDate>" + inv.EOLDate + "</EOLDate>";
                }
                else
                {
                    strxmlparam += "<EOLDate></EOLDate>";
                }

                strxmlparam += "<WarrantyPeriod>" + inv.WarrantyPeriod + "</WarrantyPeriod>";

                if (inv.PODueDate != null & inv.PODueDate != DateTime.MinValue)
                {
                    strxmlparam += "<PODueDate>" + inv.PODueDate + "</PODueDate>";
                }
                else
                {
                    strxmlparam += "<PODueDate></PODueDate>";
                }

                strxmlparam += "<DefaultReceivingLocation>" + inv.DefaultReceivingLocation + "</DefaultReceivingLocation>";
                strxmlparam += "<DefaultInspectionLocation>" + inv.DefaultInspectionLocation + "</DefaultInspectionLocation>";
                strxmlparam += "<LastSalePrice>" + inv.LastSalePrice + "</LastSalePrice>";
                strxmlparam += "<AnnualSalesQty>" + inv.AnnualSalesQty + "</AnnualSalesQty>";
                strxmlparam += "<AnnualSalesAmt>" + inv.AnnualSalesAmt + "</AnnualSalesAmt>";
                strxmlparam += "<QtyAllocatedToSO>" + inv.QtyAllocatedToSO + "</QtyAllocatedToSO>";
                strxmlparam += "<MaxDiscountPercentage>" + inv.MaxDiscountPercentage + "</MaxDiscountPercentage>";
                // strxmlparam += "<ITypeCategory>" + inv.Type + "</ITypeCategory>";
                strxmlparam += "<Height>" + inv.Height + "</Height>";
                strxmlparam += "<UnitCost>" + inv.UnitCost + "</UnitCost>";
                strxmlparam += "<GLSales>" + inv.GLSales + "</GLSales>";
                strxmlparam += "<EOQ>" + inv.EOQ + "</EOQ>";
                strxmlparam += "<LeadTime>" + inv.LeadTime + "</LeadTime>";
                #endregion
                strxmlparam += "</Item>";
                strxmlparam += "</Inventory>";

                invdt = _objDLInventory.CreateInventory(strxmlparam, inv);


            }


            return invdt;

        }

        public void UpdateInventory(Inventory inv)
        {
            Inventory invdt = null;

            if (inv != null)
            {

                string strxmlparam = string.Empty;
                strxmlparam += "<Inventory>";
                strxmlparam += "<Item>";
                #region Xml Param
                strxmlparam += "<ID>" + inv.ID + "</ID>";
                strxmlparam += "<fDesc>" + inv.fDesc + "</fDesc>";
                strxmlparam += "<Part>" + inv.MPN + "</Part>";
                strxmlparam += "<Status>" + inv.Status + "</Status>";
                strxmlparam += "<SAcct>" + inv.SAcct + "</SAcct>";
                strxmlparam += "<Measure>" + inv.Measure + "</Measure>";
                strxmlparam += "<Tax>" + inv.Tax + "</Tax>";
                strxmlparam += "<Balance>" + inv.Balance + "</Balance>";
                strxmlparam += "<Price1>" + inv.Price1 + "</Price1>";
                strxmlparam += "<Price2>" + inv.Price2 + "</Price2>";
                strxmlparam += "<Price3>" + inv.Price3 + "</Price3>";
                strxmlparam += "<Price4>" + inv.Price4 + "</Price4>";
                strxmlparam += "<Price5>" + inv.Price5 + "</Price5>";
                strxmlparam += "<Remarks>" + inv.Remarks + "</Remarks>";
                strxmlparam += "<Cat>" + inv.Cat + "</Cat>";
                strxmlparam += "<LVendor>" + inv.LVendor + "</LVendor>";
                strxmlparam += "<LCost>" + inv.LCost + "</LCost>";
                strxmlparam += "<AllowZero>" + inv.AllowZero + "</AllowZero>";
                // strxmlparam += "<Type>" + inv.Type + "</Type>";
                strxmlparam += "<InUse>" + inv.InUse + "</InUse>";
                strxmlparam += "<EN>" + inv.EN + "</EN>";
                strxmlparam += "<Hand>" + inv.Hand + "</Hand>";
                strxmlparam += "<Aisle>" + inv.Aisle + "</Aisle>";
                strxmlparam += "<fOrder>" + inv.fOrder + "</fOrder>";
                strxmlparam += "<Min>" + inv.Min + "</Min>";
                strxmlparam += "<Shelf>" + inv.Shelf + "</Shelf>";
                strxmlparam += "<Bin>" + inv.Bin + "</Bin>";
                strxmlparam += "<Requ>" + inv.Requ + "</Requ>";
                strxmlparam += "<Warehouse>" + inv.Warehouse + "</Warehouse>";
                strxmlparam += "<Price6>" + inv.Price6 + "</Price6>";
                strxmlparam += "<Committed>" + inv.Committed + "</Committed>";
                strxmlparam += "<QBInvID>" + inv.QBInvID + "</QBInvID>";
                // strxmlparam += "<LastUpdateDate>" + inv.LastUpdateDate + "</LastUpdateDate>";
                strxmlparam += "<QBAccountID>" + inv.QBAccountID + "</QBAccountID>";
                strxmlparam += "<Available>" + inv.Available + "</Available>";
                strxmlparam += "<IssuedOpenJobs>" + inv.IssuedOpenJobs + "</IssuedOpenJobs>";
                strxmlparam += "<Description2>" + inv.Description2 + "</Description2>";
                strxmlparam += "<Description3>" + inv.Description3 + "</Description3>";
                strxmlparam += "<Description4>" + inv.Description4 + "</Description4>";
                //if (inv.DateCreated != null & inv.DateCreated != DateTime.MinValue)
                //{
                //    strxmlparam += "<DateCreated>" + inv.DateCreated + "</DateCreated>";
                //}
                //else
                //{
                //    strxmlparam += "<DateCreated></DateCreated>";
                //}

                strxmlparam += "<Specification>" + inv.Specification + "</Specification>";
                strxmlparam += "<Specification2>" + inv.Specification2 + "</Specification2>";
                strxmlparam += "<Specification3>" + inv.Specification2 + "</Specification3>";
                strxmlparam += "<Specification4>" + inv.Specification4 + "</Specification4>";
                strxmlparam += "<Revision>" + inv.Revision + "</Revision>";
                //if (inv.LastRevisionDate != null & inv.LastRevisionDate != DateTime.MinValue)
                //{
                //    strxmlparam += "<LastRevisionDate>" + inv.LastRevisionDate + "</LastRevisionDate>";
                //}
                //else
                //{
                //    strxmlparam += "<LastRevisionDate></LastRevisionDate>";
                //}
                strxmlparam += "<Eco>" + inv.Eco + "</Eco>";
                strxmlparam += "<Drawing>" + inv.Drawing + "</Drawing>";
                strxmlparam += "<Reference>" + inv.Reference + "</Reference>";
                strxmlparam += "<Length>" + inv.Length + "</Length>";
                strxmlparam += "<Width>" + inv.Width + "</Width>";
                strxmlparam += "<Weight>" + inv.Weight + "</Weight>";
                strxmlparam += "<InspectionRequired>" + inv.InspectionRequired + "</InspectionRequired>";
                strxmlparam += "<CoCRequired>" + inv.CoCRequired + "</CoCRequired>";
                strxmlparam += "<ShelfLife>" + inv.ShelfLife + "</ShelfLife>";
                strxmlparam += "<SerializationRequired>" + inv.SerializationRequired + "</SerializationRequired>";
                strxmlparam += "<GLcogs>" + inv.GLcogs + "</GLcogs>";
                strxmlparam += "<GLPurchases>" + inv.GLPurchases + "</GLPurchases>";
                strxmlparam += "<ABCClass>" + inv.ABCClass + "</ABCClass>";
                strxmlparam += "<OHValue>" + inv.OHValue + "</OHValue>";
                strxmlparam += "<OOValue>" + inv.OOValue + "</OOValue>";
                strxmlparam += "<OverIssueAllowance>" + inv.OverIssueAllowance + "</OverIssueAllowance>";
                strxmlparam += "<UnderIssueAllowance>" + inv.UnderIssueAllowance + "</UnderIssueAllowance>";
                strxmlparam += "<InventoryTurns>" + inv.InventoryTurns + "</InventoryTurns>";
                strxmlparam += "<MOQ>" + inv.MOQ + "</MOQ>";
                strxmlparam += "<MinInvQty>" + inv.MinInvQty + "</MinInvQty>";
                strxmlparam += "<MaxInvQty>" + inv.MaxInvQty + "</MaxInvQty>";
                strxmlparam += "<Commodity>" + inv.Commodity + "</Commodity>";

                if (inv.LastReceiptDate != null & inv.LastReceiptDate != DateTime.MinValue)
                {
                    strxmlparam += "<LastReceiptDate>" + inv.LastReceiptDate + "</LastReceiptDate>";
                }
                else
                {
                    strxmlparam += "<LastReceiptDate></LastReceiptDate>";
                }


                strxmlparam += "<EAU>" + inv.EAU + "</EAU>";
                if (inv.EOLDate != null & inv.EOLDate != DateTime.MinValue)
                {
                    strxmlparam += "<EOLDate>" + inv.EOLDate + "</EOLDate>";
                }
                else
                {
                    strxmlparam += "<EOLDate></EOLDate>";
                }

                strxmlparam += "<WarrantyPeriod>" + inv.WarrantyPeriod + "</WarrantyPeriod>";

                if (inv.PODueDate != null & inv.PODueDate != DateTime.MinValue)
                {
                    strxmlparam += "<PODueDate>" + inv.PODueDate + "</PODueDate>";
                }
                else
                {
                    strxmlparam += "<PODueDate></PODueDate>";
                }

                strxmlparam += "<DefaultReceivingLocation>" + inv.DefaultReceivingLocation + "</DefaultReceivingLocation>";
                strxmlparam += "<DefaultInspectionLocation>" + inv.DefaultInspectionLocation + "</DefaultInspectionLocation>";
                strxmlparam += "<LastSalePrice>" + inv.LastSalePrice + "</LastSalePrice>";
                strxmlparam += "<AnnualSalesQty>" + inv.AnnualSalesQty + "</AnnualSalesQty>";
                strxmlparam += "<AnnualSalesAmt>" + inv.AnnualSalesAmt + "</AnnualSalesAmt>";
                strxmlparam += "<QtyAllocatedToSO>" + inv.QtyAllocatedToSO + "</QtyAllocatedToSO>";
                strxmlparam += "<MaxDiscountPercentage>" + inv.MaxDiscountPercentage + "</MaxDiscountPercentage>";
                // strxmlparam += "<ITypeCategory>" + inv.Type + "</ITypeCategory>";
                strxmlparam += "<Height>" + inv.Height + "</Height>";
                strxmlparam += "<UnitCost>" + inv.UnitCost + "</UnitCost>";
                strxmlparam += "<GLSales>" + inv.GLSales + "</GLSales>";
                strxmlparam += "<EOQ>" + inv.EOQ + "</EOQ>";
                strxmlparam += "<LeadTime>" + inv.LeadTime + "</LeadTime>";
                #endregion
                strxmlparam += "</Item>";
                strxmlparam += "</Inventory>";

                _objDLInventory.UpdateInventory(strxmlparam, inv);


            }



        }

        public List<string> GetInventoryWarehouse(User objProp_User)
        {

            DataSet ds = _objUser.GetInventoryWarehouse(objProp_User);
            List<string> lstwarehouse = new List<string>();
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        lstwarehouse.Add(Convert.ToString(ds.Tables[0].Rows[i]["ID"]));
                    }

                }
            }
            return lstwarehouse;
        }

        public Dictionary<string, string> GetAllVendor(string constr)
        {
            Vendor objVendor = new Vendor();
            objVendor.ConnConfig = constr;
            DataSet ds = _objVendor.GetAllVendor(objVendor);

            Dictionary<string, string> vendordetails = new Dictionary<string, string>();

            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        vendordetails.Add(Convert.ToString(ds.Tables[0].Rows[i]["ID"]), Convert.ToString(ds.Tables[0].Rows[i]["Acct"]));
                    }

                }
            }

            return vendordetails;
        }


        public DataSet GetItemQuantity()
        {
            return _objDLInventory.GetItemQuantity();
        }

        public DataSet GetItemPurchaseOrder(int invId)
        {
            return _objDLInventory.GetItemPurchaseOrder(invId);
        }

        public DataSet GetInvManufacturerInfoByInvAndVendorId(int InventoryId, int ApprovedVendorId)
        {
            List<InventoryManufacturerInformation> lstinvManufactInfo = new List<InventoryManufacturerInformation>();

            InventoryManufacturerInformation invManufactInfo = new InventoryManufacturerInformation();

            DataSet dsinv = _objDLInventory.GetInvManufacturerInfoByInvAndVendorId(InventoryId, ApprovedVendorId);


            //if (dsinv != null)
            //{
            //    if (dsinv.Tables.Count > 0)
            //    {
            //        if (dsinv.Tables[0].Rows.Count > 0)
            //        {
            //            for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
            //            {
            //                invManufactInfo = new InventoryManufacturerInformation();
            //                invManufactInfo.ID = dsinv.Tables[0].Rows[i]["ID"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["ID"]) : 0;
            //                invManufactInfo.InventoryID = dsinv.Tables[0].Rows[i]["InventoryManufacturerInformation_InvID"] != DBNull.Value ? Convert.ToInt32(dsinv.Tables[0].Rows[i]["InventoryManufacturerInformation_InvID"]) : 0;
            //                invManufactInfo.MPN = dsinv.Tables[0].Rows[i]["MPN"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["MPN"]) : string.Empty;
            //                invManufactInfo.ApprovedManufacturer = dsinv.Tables[0].Rows[i]["ApprovedManufacturer"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["ApprovedManufacturer"]) : string.Empty;
            //                invManufactInfo.ApprovedVendorId = dsinv.Tables[0].Rows[i]["ApprovedVendor"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["ApprovedVendor"]) : "0";
            //                invManufactInfo.ApprovedVendorEmail = dsinv.Tables[0].Rows[i]["Email"] != DBNull.Value ? Convert.ToString(dsinv.Tables[0].Rows[i]["Email"]) : string.Empty;
            //                lstinvManufactInfo.Add(invManufactInfo);
            //            }
            //        }
            //    }
            //}

            return dsinv;
        }

        public Commodity CreateCommodity(Commodity comm)
        {

            return _objDLInventory.CreateCommodity(comm);


        }
        public DataSet ReadCommodityById(Commodity com)
        {
            DataSet comdt = null;
            try
            {
                comdt = _objDLInventory.ReadCommodityById(com);
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
                comdt = _objDLInventory.ReadAllCommodity(com);
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
                comdt = _objDLInventory.UpdateCommodity(com);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return comdt;
        }

        //public void UpdateInvoiceTransDetails(Invoices _objInvoices)
        //{
        //    _objDLInvoice.UpdateInvoiceTransDetails(_objInvoices);
        ////}
        //public void DeleteTransInvoiceByRef(Transaction _objTrans)
        //{
        //    _objDLInvoice.DeleteTransInvoiceByRef(_objTrans);
        //}
        //public DataSet GetInvoiceByID(Invoices _objInvoice)
        //{
        //    return _objDLInvoice.GetInvoiceByID(_objInvoice);
        //}
    }
}
