using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DataLayer;
using BusinessEntity;

namespace BusinessLayer
{
    public class BL_User
    {
        DL_User objDL_User = new DL_User();

        public DataSet getUserAuthorization(User objPropUser)
        {
            return objDL_User.getUserAuthorization(objPropUser);
        }

        public DataSet getUserLoginAuthorization(User objPropUser)
        {
            return objDL_User.getUserLoginAuthorization(objPropUser);
        }

        public DataSet getTSUserAuthorization(User objPropUser)
        {
            return objDL_User.getTSUserAuthorization(objPropUser);
        }

        public DataSet getCustomerType(User objPropUser)
        {
            return objDL_User.getCustomerType(objPropUser);
        }

        public DataSet getcategoryAll(User objPropUser)
        {
            return objDL_User.getcategoryAll(objPropUser);
        }

        public DataSet getEquiptype(User objPropUser)
        {
            return objDL_User.getEquiptype(objPropUser);
        }

        public DataSet getEquipmentCategory(User objPropUser)
        {
            return objDL_User.getEquipmentCategory(objPropUser);
        }

        public DataSet getMCPS(User objPropUser)
        {
            return objDL_User.getMCPS(objPropUser);
        }

        public DataSet getServiceType(User objPropUser)
        {
            return objDL_User.getServiceType(objPropUser);
        }

        public void AddSalesTax(User objPropUser)
        {
            objDL_User.AddSalesTax(objPropUser);
        }

        public void UpdateSalesTax(User objPropUser)
        {
            objDL_User.UpdateSalesTax(objPropUser);
        }

        public void AddQBSalesTax(User objPropUser)
        {
            objDL_User.AddQBSalesTax(objPropUser);
        }

        public void AddBillCode(User objPropUser)
        {
            objDL_User.AddBillCode(objPropUser);
        }

        public void AddQBBillCode(User objPropUser)
        {
            objDL_User.AddQBBillCode(objPropUser);
        }

        public void InsertWareHouse(User objPropUser)
        {
            objDL_User.InsertWareHouse(objPropUser);
        }

        public void DeleteCustomerQB(User objPropUser)
        {
            objDL_User.DeleteCustomerQB(objPropUser);
        }

        public void UpdateBillCode(User objPropUser)
        {
            objDL_User.UpdateBillCode(objPropUser);
        }

        public void UpdateWarehouse(User objPropUser)
        {
            objDL_User.UpdateWarehouse(objPropUser);
        }



        public void DeleteDiagnostic(User objPropUser)
        {
            objDL_User.DeleteDiagnostic(objPropUser);
        }

        public DataSet getDepartment(User objPropUser)
        {
            return objDL_User.getDepartment(objPropUser);
        }

        public DataSet getBillCodes(User objPropUser)
        {
            return objDL_User.getBillCodes(objPropUser);
        }

        public DataSet getBillCodesByID(User objPropUser)
        {
            return objDL_User.getBillCodesByID(objPropUser);
        }

        public DataSet getSalesTax(User objPropUser)
        {
            return objDL_User.getSalesTax(objPropUser);
        }

        public DataSet getSalesTaxByTaxType(User objPropUser)
        {
            return objDL_User.getSalesTaxByTaxType(objPropUser);
        }

        public DataSet getMSMSalesTax(User objPropUser)
        {
            return objDL_User.getMSMSalesTax(objPropUser);
        }

        public DataSet getQBSalesTax(User objPropUser)
        {
            return objDL_User.getQBSalesTax(objPropUser);
        }

        public DataSet getMSMLoctype(User objPropUser)
        {
            return objDL_User.getMSMLoctype(objPropUser);
        }

        public DataSet getMSMDepartment(User objPropUser)
        {
            return objDL_User.getMSMDepartment(objPropUser);
        }

        public DataSet getMSMBillcode(User objPropUser)
        {
            return objDL_User.getMSMBillcode(objPropUser);
        }

        public DataSet getMSMterms(User objPropUser)
        {
            return objDL_User.getMSMterms(objPropUser);
        }

        public DataSet getMSMAccount(User objPropUser)
        {
            return objDL_User.getMSMAccount(objPropUser);
        }

        public DataSet getMSMPatrollWage(User objPropUser)
        {
            return objDL_User.getMSMPatrollWage(objPropUser);
        }

        public DataSet getMSMVendor(User objPropUser)
        {
            return objDL_User.getMSMVendor(objPropUser);
        }

        public DataSet getQBDepartment(User objPropUser)
        {
            return objDL_User.getQBDepartment(objPropUser);
        }

        public DataSet getMSMCustomertype(User objPropUser)
        {
            return objDL_User.getMSMCustomertype(objPropUser);
        }

        public DataSet getQBCustomertype(User objPropUser)
        {
            return objDL_User.getQBCustomertype(objPropUser);
        }

        public DataSet getSalesTaxByID(User objPropUser)
        {
            return objDL_User.getSalesTaxByID(objPropUser);
        }

        public DataSet getlocationType(User objPropUser)
        {
            return objDL_User.getlocationType(objPropUser);
        }

        public DataSet getEMP(User objPropUser)
        {
            return objDL_User.getEMP(objPropUser);
        }

        public int getEMPStatus(User objPropUser)
        {
            return objDL_User.getEMPStatus(objPropUser);
        }

        public DataSet getEMPwithDeviceID(User objPropUser)
        {
            return objDL_User.getEMPwithDeviceID(objPropUser);
        }

        public DataSet getEMPScheduler(User objPropUser)
        {
            return objDL_User.getEMPScheduler(objPropUser);
        }

        public DataSet getEMPSuper(User objPropUser)
        {
            return objDL_User.getEMPSuper(objPropUser);
        }

        public int getLoginSuper(User objPropUser)
        {
            return objDL_User.getLoginSuper(objPropUser);
        }

        public int getISSuper(User objPropUser)
        {
            return objDL_User.getISSuper(objPropUser);
        }

        public DataSet getDatabases(User objPropUser)
        {
            return objDL_User.getDatabases(objPropUser);
        }

        public DataSet getAdminControlByID(User objPropUser)
        {
            return objDL_User.getAdminControlByID(objPropUser);
        }

        public DataSet CheckDB(User objPropUser)
        {
            return objDL_User.CheckDB(objPropUser);
        }

        public DataSet GetAdminPassword(User objPropUser)
        {
            return objDL_User.GetAdminPassword(objPropUser);
        }

        public int AddUser(User objPropUser)
        {
            return objDL_User.AddUser(objPropUser);
        }

        public DataSet AddQBUser(User objPropUser)
        {
            return objDL_User.AddQBUser(objPropUser);
        }

        public DataSet getRoute(User objPropUser)
        {
            return objDL_User.getRoute(objPropUser);
        }

        public DataSet getTerritory(User objPropUser)
        {
            return objDL_User.getTerritory(objPropUser);
        }

        public void AddCustomer(User objPropUser)
        {
            objDL_User.AddCustomer(objPropUser);
        }

        public void AddCustomerQB(User objPropUser)
        {
            objDL_User.AddCustomerQB(objPropUser);
        }

        public void AddCustomerQBMapping(User objPropUser)
        {
            objDL_User.AddCustomerQBMapping(objPropUser);
        }

        public int AddCustomerSage(User objPropUser)
        {
            return objDL_User.AddCustomerSage(objPropUser);
        }

        public void AddCustomertest(User objPropUser)
        {
            objDL_User.AddCustomertest(objPropUser);
        }

        public void AddEquipment(User objPropUser)
        {
            objDL_User.AddEquipment(objPropUser);
        }

        public void AddEquipmentImport(User objPropUser)
        {
            objDL_User.AddEquipmentImport(objPropUser);
        }

        public void DeleteCompany(User objPropUser)
        {
            objDL_User.DeleteCompany(objPropUser);
        }

        public void AddCompany(User objPropUser)
        {
            objDL_User.AddCompany(objPropUser);
        }

        public DataSet getTerms(User objPropUser)
        {
            return objDL_User.getTerms(objPropUser);
        }

        public void UpdateCompany(User objPropUser)
        {
            objDL_User.UpdateCompany(objPropUser);
        }

        public void UpdateControl(User objPropUser)
        {
            objDL_User.UpdateControl(objPropUser);
        }

        public void CreateDatabase(User objPropUser)
        {
            objDL_User.CreateDatabase(objPropUser);
        }

        public void AddCustomerType(User objPropUser)
        {
            objDL_User.AddCustomerType(objPropUser);
        }

        public void AddQBCustomerType(User objPropUser)
        {
            objDL_User.AddQBCustomerType(objPropUser);
        }

        public void AddQBLocType(User objPropUser)
        {
            objDL_User.AddQBLocType(objPropUser);
        }

        public void AddCategory(User objPropUser)
        {
            objDL_User.AddCategory(objPropUser);
        }

        public void AddEquipType(User objPropUser)
        {
            objDL_User.AddEquipType(objPropUser);
        }
        public void AddEquipCateg(User objPropUser)
        {
            objDL_User.AddEquipCateg(objPropUser);
        }

        public void AddMCPS(User objPropUser)
        {
            objDL_User.AddMCPS(objPropUser);
        }
        public void AddServiceType(User objPropUser)
        {
            objDL_User.AddServiceType(objPropUser);
        }

        public void AddLocationType(User objPropUser)
        {
            objDL_User.AddLocationType(objPropUser);
        }

        public void UpdateCustomerType(User objPropUser)
        {
            objDL_User.UpdateCustomerType(objPropUser);
        }

        public void UpdateCategory(User objPropUser)
        {
            objDL_User.UpdateCategory(objPropUser);
        }
        public void UpdateServiceType(User objPropUser)
        {
            objDL_User.UpdateServiceType(objPropUser);
        }
        public void UpdateLocationType(User objPropUser)
        {
            objDL_User.UpdateLocationType(objPropUser);
        }
        public void DeleteCustomerType(User objPropUser)
        {
            objDL_User.DeleteCustomerType(objPropUser);
        }
        public void DeleteCustomerTypeByListID(User objPropUser)
        {
            objDL_User.DeleteCustomerTypeByListID(objPropUser);
        }

        public void DeleteCategory(User objPropUser)
        {
            objDL_User.DeleteCategory(objPropUser);
        }
        public void DeleteEquiptype(User objPropUser)
        {
            objDL_User.DeleteEquiptype(objPropUser);
        }
        public void DeleteEquipCateg(User objPropUser)
        {
            objDL_User.DeleteEquipCateg(objPropUser);
        }
        public void DeleteMCPS(User objPropUser)
        {
            objDL_User.DeleteMCPS(objPropUser);
        }
        public void DeleteServicetype(User objPropUser)
        {
            objDL_User.DeleteServicetype(objPropUser);
        }

        public void DeleteBillingCode(User objPropUser)
        {
            objDL_User.DeleteBillingCode(objPropUser);
        }

        public void DeleteBillingCodebyListID(User objPropUser)
        {
            objDL_User.DeleteBillingCodebyListID(objPropUser);
        }

        public void DeleteSalesTax(User objPropUser)
        {
            objDL_User.DeleteSalesTax(objPropUser);
        }

        public void DeleteSalesTaxByListID(User objPropUser)
        {
            objDL_User.DeleteSalesTaxByListID(objPropUser);
        }

        public void DeleteDepartment(User objPropUser)
        {
            objDL_User.DeleteDepartment(objPropUser);
        }

        public void DeleteDepartmentByListID(User objPropUser)
        {
            objDL_User.DeleteDepartmentByListID(objPropUser);
        }

        public void DeleteLocType(User objPropUser)
        {
            objDL_User.DeleteLocType(objPropUser);
        }

        public void DeleteLocTypeByListID(User objPropUser)
        {
            objDL_User.DeleteLocTypeByListID(objPropUser);
        }

        public void AddDepartment(User objPropUser)
        {
            objDL_User.AddDepartment(objPropUser);
        }

        public void AddWage(Wage _objWage)
        {
            objDL_User.AddWage(_objWage);
        }

        public void UpdateDepartment(User objPropUser)
        {
            objDL_User.UpdateDepartment(objPropUser);
        }

        public void UpdateWage(Wage _objWage)
        {
            objDL_User.UpdateWage(_objWage);
        }

        public void AddQBDepartment(User objPropUser)
        {
            objDL_User.AddQBDepartment(objPropUser);
        }

        public void AddQBTerms(User objPropUser)
        {
            objDL_User.AddQBTerms(objPropUser);
        }

        public void AddQBPayrollWage(User objPropUser)
        {
            objDL_User.AddQBPayrollWage(objPropUser);
        }

        public void AddDatabaseName(User objPropUser)
        {
            objDL_User.AddDatabaseName(objPropUser);
        }

        public void UpdateDatabaseName(User objPropUser)
        {
            objDL_User.UpdateDatabaseName(objPropUser);
        }


        public void UpdateAdminPassword(User objPropUser)
        {
            objDL_User.UpdateAdminPassword(objPropUser);
        }



        public void AddLocation(User objPropUser)
        {
            objDL_User.AddLocation(objPropUser);
        }

        public void AddQBLocation(User objPropUser)
        {
            objDL_User.AddQBLocation(objPropUser);
        }

        public void AddQBLocationMapping(User objPropUser)
        {
            objDL_User.AddQBLocationMapping(objPropUser);
        }

        public void UpdateLocation(User objPropUser)
        {
            objDL_User.UpdateLocation(objPropUser);
        }

        public void UpdateCustomer(User objPropUser)
        {
            objDL_User.UpdateCustomer(objPropUser);
        }

        public void UpdateEquipment(User objPropUser)
        {
            objDL_User.UpdateEquipment(objPropUser);
        }

        public void AddMassMCP(User objPropUser)
        {
            objDL_User.AddMassMCP(objPropUser);
        }

        public void UpdateCustomerContact(User objPropUser)
        {
            objDL_User.UpdateCustomerContact(objPropUser);
        }

        public void UpdateUser(User objPropUser)
        {
            objDL_User.UpdateUser(objPropUser);
        }

        public void UpdateTSUser(User objPropUser)
        {
            objDL_User.UpdateTSUser(objPropUser);
        }

        public void UpdateQBCustomerID(User objPropUser)
        {
            objDL_User.UpdateQBCustomerID(objPropUser);
        }

        public void UpdateQBsalestaxID(User objPropUser)
        {
            objDL_User.UpdateQBsalestaxID(objPropUser);
        }

        public void UpdateQBDepartmentID(User objPropUser)
        {
            objDL_User.UpdateQBDepartmentID(objPropUser);
        }

        public void UpdateQBInvID(User objPropUser)
        {
            objDL_User.UpdateQBInvID(objPropUser);
        }

        public void UpdateQBTermsID(User objPropUser)
        {
            objDL_User.UpdateQBTermsID(objPropUser);
        }

        public void UpdateQBAccountID(User objPropUser)
        {
            objDL_User.UpdateQBAccountID(objPropUser);
        }

        public void UpdateQBVendorID(User objPropUser)
        {
            objDL_User.UpdateQBVendorID(objPropUser);
        }

        public void UpdateQBWageID(User objPropUser)
        {
            objDL_User.UpdateQBWageID(objPropUser);
        }

        public void UpdateQBJobtypeID(User objPropUser)
        {
            objDL_User.UpdateQBJobtypeID(objPropUser);
        }

        public void UpdateQBcustomertypeID(User objPropUser)
        {
            objDL_User.UpdateQBcustomertypeID(objPropUser);
        }

        public void UpdateQBLocationID(User objPropUser)
        {
            objDL_User.UpdateQBLocationID(objPropUser);
        }

        public void CreateDBObjects(User objPropUser)
        {
            objDL_User.CreateDBObjects(objPropUser);
        }

        public DataSet getUser(User objPropUser)
        {
            return objDL_User.getUser(objPropUser);
        }

        public DataSet getUserForSupervisor(User objPropUser)
        {
            return objDL_User.getUserForSupervisor(objPropUser);
        }

        public DataSet getSelectedUser(User objPropUser)
        {
            return objDL_User.getSelectedUser(objPropUser);
        }


        public DataSet getUsersSuper(User objPropUser)
        {
            return objDL_User.getUsersSuper(objPropUser);
        }

        public DataSet getSupervisor(User objPropUser)
        {
            return objDL_User.getSupervisor(objPropUser);
        }

        public DataSet getOpenCalls(User objPropUser)
        {
            return objDL_User.getOpenCalls(objPropUser);
        }

        public DataSet getOpenCallsMapScreen(User objPropUser)
        {
            return objDL_User.getOpenCallsMapScreen(objPropUser);
        }


        public DataSet getFieldUser(User objPropUser)
        {
            return objDL_User.getFieldUser(objPropUser);
        }


        public DataSet getControl(User objPropUser)
        {
            return objDL_User.getControl(objPropUser);
        }

        public DataSet getLogo(User objPropUser)
        {
            return objDL_User.getLogo(objPropUser);
        }

        public DataSet getControlBranch(User objPropUser)
        {
            return objDL_User.getControlBranch(objPropUser);
        }


        public DataSet getAdminControl(User objPropUser)
        {
            return objDL_User.getAdminControl(objPropUser);
        }

        public DataSet getAdminAuthorization(User objPropUser)
        {
            return objDL_User.getAdminAuthorization(objPropUser);
        }

        public DataSet getCustomers(User objPropUser)
        {
            return objDL_User.getCustomers(objPropUser);
        }

        public DataSet getMSMCustomers(User objPropUser)
        {
            return objDL_User.getMSMCustomers(objPropUser);
        }

        public DataSet getMSMCustomersMapping(User objPropUser)
        {
            return objDL_User.getMSMCustomersMapping(objPropUser);
        }

        public DataSet getCustomersSageAdd(User objPropUser)
        {
            return objDL_User.getCustomersSageAdd(objPropUser);
        }

        public DataSet getLocationsSageAdd(User objPropUser)
        {
            return objDL_User.getLocationsSageAdd(objPropUser);
        }

        public DataSet getLocationsSageNA(User objPropUser)
        {
            return objDL_User.getLocationsSageNA(objPropUser);
        }

        public DataSet geCustomersSageNA(User objPropUser)
        {
            return objDL_User.geCustomersSageNA(objPropUser);
        }

        public void UpdateSageID(User objPropUser)
        {
            objDL_User.UpdateSageID(objPropUser);
        }

        public void UpdateLocSageID(User objPropUser)
        {
            objDL_User.UpdateLocSageID(objPropUser);
        }

        public DataSet getCustomersSageUpdate(User objPropUser)
        {
            return objDL_User.getCustomersSageUpdate(objPropUser);
        }

        public DataSet getLocationsSageUpdate(User objPropUser)
        {
            return objDL_User.getLocationsSageUpdate(objPropUser);
        }

        public DataSet getCustomersForSageDelete(User objPropUser)
        {
            return objDL_User.getCustomersForSageDelete(objPropUser);
        }

        public DataSet getLocationsForSageDelete(User objPropUser)
        {
            return objDL_User.getLocationsForSageDelete(objPropUser);
        }

        public DataSet getMSMLocation(User objPropUser)
        {
            return objDL_User.getMSMLocation(objPropUser);
        }

        public DataSet getMSMLocationMapping(User objPropUser)
        {
            return objDL_User.getMSMLocationMapping(objPropUser);
        }

        public DataSet getQBCustomers(User objPropUser)
        {
            return objDL_User.getQBCustomers(objPropUser);
        }

        public DataSet getQBLocation(User objPropUser)
        {
            return objDL_User.getQBLocation(objPropUser);
        }


        public DataSet getLocations(User objPropUser)
        {
            return objDL_User.getLocations(objPropUser);
        }

        public DataSet getUserSearch(User objPropUser)
        {
            return objDL_User.getUserSearch(objPropUser);
        }

        public DataSet getCustomerSearch(User objPropUser)
        {
            return objDL_User.getCustomerSearch(objPropUser);
        }

        public DataSet getCustomerAuto(User objPropUser)
        {
            return objDL_User.getCustomerAuto(objPropUser);
        }

        public DataSet getAccountAuto(User objPropUser)
        {
            return objDL_User.getAccountAuto(objPropUser);
        }

        public DataSet getCustomerAutojquery(User objPropUser)
        {
            return objDL_User.getCustomerAutojquery(objPropUser);
        }

        public DataSet getCustomerProspectAutojquery(User objPropUser)
        {
            return objDL_User.getCustomerProspectAutojquery(objPropUser);
        }

        public DataSet getTaskContactsSearch(User objPropUser)
        {
            return objDL_User.getTaskContactsSearch(objPropUser);
        }

        public DataSet getElevSearch(User objPropUser)
        {
            return objDL_User.getElevSearch(objPropUser);
        }




        public DataSet getLocationAutojquery(User objPropUser)
        {
            return objDL_User.getLocationAutojquery(objPropUser);
        }

        public DataSet getLocationSearch(User objPropUser)
        {
            return objDL_User.getLocationSearch(objPropUser);
        }

        public DataSet getCompany(User objPropUser)
        {
            return objDL_User.getCompany(objPropUser);
        }

        public DataSet getUserByID(User objPropUser)
        {
            return objDL_User.getUserByID(objPropUser);
        }

        public string getUserLangByID(User objPropUser)
        {
            return objDL_User.getUserLangByID(objPropUser);
        }

        public DataSet getCustomerByID(User objPropUser)
        {
            return objDL_User.getCustomerByID(objPropUser);
        }

        public DataSet getCustomerAddress(User objPropUser)
        {
            return objDL_User.getCustomerAddress(objPropUser);
        }

        public string getCustomerEmail(User objPropUser)
        {
            return objDL_User.getCustomerEmail(objPropUser);
        }

        public DataSet getequipByID(User objPropUser)
        {
            return objDL_User.getequipByID(objPropUser);
        }

        public DataSet getequipREPDetails(User objPropUser)
        {
            return objDL_User.getequipREPDetails(objPropUser);
        }

        public DataSet getCustomerForReport(User objPropUser)
        {
            return objDL_User.getCustomerForReport(objPropUser);
        }


        public DataSet getLocationByCustomerID(User objPropUser)
        {
            return objDL_User.getLocationByCustomerID(objPropUser);
        }

        public DataSet getLocationByID(User objPropUser)
        {
            return objDL_User.getLocationByID(objPropUser);
        }

        public DataSet getLocationByIDReport(User objPropUser)
        {
            return objDL_User.getLocationByIDReport(objPropUser);
        }

        public DataSet getElevUnit(User objPropUser)
        {
            return objDL_User.getElevUnit(objPropUser);
        }

        public DataSet getElev(User objPropUser)
        {
            return objDL_User.getElev(objPropUser);
        }

        public DataSet getCategory(User objPropUser)
        {
            return objDL_User.getCategory(objPropUser);
        }

        public DataSet gettrial(User objPropUser)
        {
            return objDL_User.gettrial(objPropUser);
        }

        public DataSet gettrialUser(User objPropUser)
        {
            return objDL_User.gettrialUser(objPropUser);
        }

        public DataSet getLicenseInfoUser(User objPropUser)
        {
            return objDL_User.getLicenseInfoUser(objPropUser);
        }

        public void UpdateTrial(User objPropUser)
        {
            objDL_User.UpdateTrial(objPropUser);
        }

        public void UpdateReg(User objPropUser)
        {
            objDL_User.UpdateReg(objPropUser);
        }

        public void UpdateRegUser(User objPropUser)
        {
            objDL_User.UpdateRegUser(objPropUser);
        }

        public void UpdateSupervisorUser(User objPropUser)
        {
            objDL_User.UpdateSupervisorUser(objPropUser);
        }

        public void DeleteUser(User objPropUser)
        {
            objDL_User.DeleteUser(objPropUser);
        }

        public void DeleteCustomer(User objPropUser)
        {
            objDL_User.DeleteCustomer(objPropUser);
        }

        public void DeleteCustomerBySageID(User objPropUser)
        {
            objDL_User.DeleteCustomerBySageID(objPropUser);
        }

        public void DeleteLocationBySageID(User objPropUser)
        {
            objDL_User.DeleteLocationBySageID(objPropUser);
        }

        public void DeleteCustomerByListID(User objPropUser)
        {
            objDL_User.DeleteCustomerByListID(objPropUser);
        }

        public void DeleteEquipment(User objPropUser)
        {
            objDL_User.DeleteEquipment(objPropUser);
        }

        public void DeleteLocation(User objPropUser)
        {
            objDL_User.DeleteLocation(objPropUser);
        }

        public void DeleteLocationByListID(User objPropUser)
        {
            objDL_User.DeleteLocationByListID(objPropUser);
        }

        public void DeleteEmployeeByListID(User objPropUser)
        {
            objDL_User.DeleteEmployeeByListID(objPropUser);
        }

        public void UpdateRolCoordinates(User objPropUser)
        {
            objDL_User.UpdateRolCoordinates(objPropUser);
        }

        public DataSet getWarehouse(User objPropUser)
        {
            return objDL_User.getWarehouse(objPropUser);
        }

        public string getUserEmail(User objPropUser)
        {
            return objDL_User.getUserEmail(objPropUser);
        }
        public string getUserPager(User objPropUser)
        {
            return objDL_User.getUserPager(objPropUser);
        }

        public string getUserDeviceID(User objPropUser)
        {
            return objDL_User.getUserDeviceID(objPropUser);
        }

        public void UpdateDefaultWorkerLocation(User objPropUser)
        {
            objDL_User.UpdateDefaultWorkerLocation(objPropUser);
        }

        public void UpdateLocationAddress(User objPropUser)
        {
            objDL_User.UpdateLocationAddress(objPropUser);
        }

        public void UserRegistrationTransfer(User objPropUser)
        {
            objDL_User.UserRegistrationTransfer(objPropUser);
        }

        public int GetUserSyncStatus(User objPropUser)
        {
            return objDL_User.GetUserSyncStatus(objPropUser);
        }

        public DataSet GetSyncItems(User objPropUser)
        {
            return objDL_User.GetSyncItems(objPropUser);
        }

        public void AddEquipmentTemplate(User objPropUser)
        {
            objDL_User.AddEquipmentTemplate(objPropUser);
        }

        public void AddCustomTemplate(User objPropUser)
        {
            objDL_User.AddCustomTemplate(objPropUser);
        }

        public DataSet getSalesPerson(User objPropUser)
        {
            return objDL_User.getSalesPerson(objPropUser);
        }

        public DataSet getTaskUsers(User objPropUser)
        {
            return objDL_User.getTaskUsers(objPropUser);
        }

        public void UpdateAnnualAmount(User objPropUser)
        {
            objDL_User.UpdateAnnualAmount(objPropUser);
        }

        public void UpdateCustomerUser(User objPropUser)
        {
            objDL_User.UpdateCustomerUser(objPropUser);
        }

        public DataSet getTimesheetEmp(User objPropUser)
        {
            return objDL_User.getTimesheetEmp(objPropUser);
        }
        public DataSet getSavedTimesheetEmp(User objPropUser)
        {
            return objDL_User.getSavedTimesheetEmp(objPropUser);
        }
        public DataSet getSavedTimesheet(User objPropUser)
        {
            return objDL_User.getSavedTimesheet(objPropUser);
        }

        public DataSet GetTimesheetTicketsByEmp(User objPropUser)
        {
            return objDL_User.GetTimesheetTicketsByEmp(objPropUser);
        }

        public void AddTimesheet(User objPropUser)
        {
            objDL_User.AddTimesheet(objPropUser);
        }

        public void ProcessTimesheet(User objPropUser)
        {
            objDL_User.ProcessTimesheet(objPropUser);
        }

        public DataSet getScreens(User objPropUser)
        {
            return objDL_User.getScreens(objPropUser);
        }

        public DataSet getScreensByUser(User objPropUser)
        {
            return objDL_User.getScreensByUser(objPropUser);
        }

        public void UpdateUserPermission(User objPropUser)
        {
            objDL_User.UpdateUserPermission(objPropUser);
        }

        public DataSet AddSageLocation(User objPropUser)
        {
            return objDL_User.AddSageLocation(objPropUser);
        }

        public DataSet getGetSageExportTickets(User objPropUser)
        {
            return objDL_User.getGetSageExportTickets(objPropUser);
        }
        public void UpdatePeriodClosedDate(User objPropUser)
        {
            objDL_User.UpdatePeriodClosedDate(objPropUser);
        }
        public DataSet GetUserAddress(User objPropUser)
        {
            return objDL_User.GetUserAddress(objPropUser);
        }
        public DataSet GetBillCodeSearch(User objPropUser)
        {
            return objDL_User.GetBillCodeSearch(objPropUser);
        }
        public DataSet GetServiceTypeByType(User objPropUser)
        {
            return objDL_User.GetServiceTypeByType(objPropUser);
        }

        public DataSet getWage(User objPropUser)
        {
            return objDL_User.getWage(objPropUser);
        }
        public DataSet getSTax(User objPropUser)
        {
            return objDL_User.getSTax(objPropUser);
        }
        public DataSet GetWageByID(Wage _objWage)
        {
            return objDL_User.GetWageByID(_objWage);
        }
        public void DeleteWageByID(Wage _objWage)
        {
            objDL_User.DeleteWageByID(_objWage);
        }
        public DataSet GetAllWage(Wage _objWage)
        {
            return objDL_User.GetAllWage(_objWage);
        }
        public DataSet GetUserSearch(User _objUser)
        {
            return objDL_User.GetUserSearch(_objUser);
        }

        public void UpdateDocInfo(User objpropUser)
        {
            objDL_User.UpdateDocInfo(objpropUser);
        }
        public DataSet getElevByLoc(User objPropUser)
        {
            return objDL_User.getElevByLoc(objPropUser);
        }
        public DataSet GetAllTc(User objPropUser)
        {
            return objDL_User.GetAllTc(objPropUser);
        }
        public DataSet GetSearchPages(User objPropUser)
        {
            return objDL_User.GetSearchPages(objPropUser);
        }
        public void AddTerms(User _objUser)
        {
            objDL_User.AddTerms(_objUser);
        }
        public void UpdateTerms(User _objUser)
        {
            objDL_User.UpdateTerms(_objUser);
        }
        public bool IsExistPage(User _objUser)
        {
            return objDL_User.IsExistPage(_objUser);
        }
        public void DeleteTermsCondition(User _objUser)
        {
            objDL_User.DeleteTermsCondition(_objUser);
        }
        public bool IsExistPageForUpdate(User _objUser)
        {
            return objDL_User.IsExistPageForUpdate(_objUser);
        }
        public DataSet GetAllUseTax(User objPropUser)
        {
            return objDL_User.GetAllUseTax(objPropUser);
        }

        public void CreateWarehouse(User objPropUser)
        {
            objDL_User.CreateWarehouse(objPropUser);
        }
        public void UpdateInventoryWarehouse(User objPropUser)
        {
            objDL_User.UpdateInventoryWarehouse(objPropUser);
        }
        public DataSet GetInventoryWarehouse(User objPropUser)
        {
            return objDL_User.GetInventoryWarehouse(objPropUser);
        }
        public DataSet GetJobBillRatesById(User objPropUser)
        {
            return objDL_User.GetJobBillRatesById(objPropUser);
        }
    }
}
