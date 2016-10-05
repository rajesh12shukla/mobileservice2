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
    public class BL_BankAccount
    {
        DL_BankAccount _objDLBank = new DL_BankAccount();

        public int AddRol(Rol objRol)
        {
            return _objDLBank.AddRol(objRol);
        }
        public void AddBank(Bank objBank)
        {
            _objDLBank.AddBank(objBank);
        }
        public void UpdateRol(Rol objRol)
        {
            _objDLBank.UpdateRol(objRol);
        }
        public void UpdateBank(Bank objBank)
        {
            _objDLBank.UpdateBank(objBank);
        }
        public DataSet GetStates(State _objState)
        {
            return _objDLBank.GetStates(_objState);
        }
        public DataSet GetRolByID(Rol objRol)
        {
            return _objDLBank.GetRolByID(objRol);
        }
        public DataSet GetBankByChart(Bank objBank)
        {
            return _objDLBank.GetBankByChart(objBank);
        }
        public DataSet IsExistBankAcct(Bank objBank)
        {
            return _objDLBank.IsExistBankAcct(objBank);
        }
        public DataSet GetAllBankNames(Bank objBank)
        {
            return _objDLBank.GetAllBankNames(objBank);
        }
        public DataSet GetBankByID(Bank objBank)
        {
            return _objDLBank.GetBankByID(objBank);
        }
        public void UpdateBankBalance(Bank _objBank)
        {
            _objDLBank.UpdateBankBalance(_objBank);
        }
        public void UpdateBankBalanceNcheck(Bank _objBank)
        {
            _objDLBank.UpdateBankBalanceNcheck(_objBank);
        }
        public void UpdateBankRecon(Bank _objBank)
        {
            _objDLBank.UpdateBankRecon(_objBank);
        }
        public int GetChartByBank(Bank _objBank)
        {
            return _objDLBank.GetChartByBank(_objBank);
        }
        public int GetBankIDByChart(Bank _objBank)
        {
            return _objDLBank.GetBankIDByChart(_objBank);
        }
        public DataSet GetBankRolByID(Bank objBank)
        {
            return _objDLBank.GetBankRolByID(objBank);
        }
        public void DeleteRolByID(Rol objRol)
        {
            _objDLBank.DeleteRolByID(objRol);
        }
        public void BankRecon(Bank objBank)
        {
            _objDLBank.BankRecon(objBank);
        }
    }
}
