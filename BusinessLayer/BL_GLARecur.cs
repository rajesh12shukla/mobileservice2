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
    public class BL_GLARecur
    {
        DL_GLARecur _objDLGLARecur = new DL_GLARecur();
        public DataSet GetAllRecurrTrans(Journal objJournal)
        {
            return _objDLGLARecur.GetAllRecurrTrans(objJournal);
        }
        public DataSet GetProcessRecurrCount(Journal objJournal)
        {
            return _objDLGLARecur.GetProcessRecurrCount(objJournal);
        }
        public DataSet GetTransDataByRef(Transaction objTrans)
        {
            return _objDLGLARecur.GetTransDataByRef(objTrans);
        }
        public void DeleteGLARecur(Journal objJournal)
        {
            _objDLGLARecur.DeleteGLARecur(objJournal);
        }
        public void DeleteRecurTrans(Journal objJournal)
        {
            _objDLGLARecur.DeleteRecurTrans(objJournal);
        }
        public void DeleteRecurTransByID(Transaction objTrans)
        {
            _objDLGLARecur.DeleteRecurTransByID(objTrans);
        }
        public void AddRecur(Journal objJournal)
        {
            _objDLGLARecur.AddRecur(objJournal);
        }
        public void AddRecurTrans(Transaction objTrans)
        {
            _objDLGLARecur.AddRecurTrans(objTrans);
        }
        public void UpdateRecur(Journal objJournal)
        {
            _objDLGLARecur.UpdateRecur(objJournal);
        }
        public void UpdateRecurTrans(Transaction objTrans)
        {
            _objDLGLARecur.UpdateRecurTrans(objTrans);
        }
        public int GetMaxRecurRef(Journal objJournal)
        {
            return _objDLGLARecur.GetMaxRecurRef(objJournal);
        }
        public DataSet GetProcessTransByDate(Journal objJournal)
        {
            return _objDLGLARecur.GetProcessTransByDate(objJournal);
        }
        public DataSet GetMinRecurDate(Journal objJournal)
        {
            return _objDLGLARecur.GetMinRecurDate(objJournal);
        }
    }
}
