using BusinessEntity;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class BL_Vendor
    {
        DL_Vendor _objDLVendor = new DL_Vendor();
        public void AddVendor(Vendor objVendor)
        {
            _objDLVendor.AddVendor(objVendor);
        }
        public void UpdateVendor(Vendor objVendor)
        {
            _objDLVendor.UpdateVendor(objVendor);
        }
        public DataSet GetVendor(Vendor objVendor)
        {
            return _objDLVendor.GetVendor(objVendor);
        }
        public void DeleteVendor(Vendor objVendor)
        {
            _objDLVendor.DeleteVendor(objVendor);
        }
        public DataSet GetAll(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendor(objVendor);
        }
        public DataSet IsExistsForInsertVendor(Vendor objVendor)
        {
            return _objDLVendor.IsExistsForInsertVendor(objVendor);
        }
        public DataSet IsExistForUpdateVendor(Vendor objVendor)
        {
            return _objDLVendor.IsExistForUpdateVendor(objVendor);
        }
        public DataSet GetAllVendorDetails(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorDetails(objVendor);
        }
        public DataSet GetAllVenderGridview(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorGridview(objVendor);
        }
        public DataSet GetVendorEdit(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendorEdit(objVendor);
        }
        public DataSet GetAllVendors(Vendor objVendor)
        {
            return _objDLVendor.GetAllVendors(objVendor);
        }
        public DataSet GetVendorSearch(Vendor objVendor)
        {
            return _objDLVendor.GetVendorSearch(objVendor);
        }
        public bool IsExistVendorDetails(Vendor _objVendor)
        {
            return _objDLVendor.IsExistVendorDetails(_objVendor);
        }
        public DataSet GetVendorRolDetails(Vendor _objVendor)
        {
            return _objDLVendor.GetVendorRolDetails(_objVendor);
        }
        public DataSet GetVendorGLById(Vendor objVendor)
        {
            return _objDLVendor.GetVendorGLById(objVendor);
        }

        public DataSet GetVendorListDetails(Vendor objVendor)
        {
            return _objDLVendor.GetVendorListDetails(objVendor);
        }

        //RAHIL's
        public DataSet GetVendorAcct(Vendor _objVendor)
        {
            return _objDLVendor.GetVendorAcct(_objVendor);
        }

        public DataSet GetAllVenderAjaxSearch(Vendor objVendor)
        {

            return _objDLVendor.GetAllVenderAjaxSearch(objVendor);
        }
    }
}
