using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntity
{
    public class JobT
    {
        public int Job;
        private DataSet _ds;
        private string _ConnConfig;
        public int ID;
        public string fDesc;
        public Int16 Type;
        public Int16 NRev;
        public Int16 NDed;
        public int Count;
        public string Remarks;
        public int InvExp;
        public int InvServ;
        public int Wage;
        public string CType;
        public Int16 Status;
        public Int16 Charge;
        public Int16 Post;
        public Int16 fInt;
        public int GLInt;
        public Int16 JobClose;
        public string TemplateRev;
        public string RevRemarks;
        public int MilestoneType;
        public bool IsExist;
        public Int16 Line;
        public int ItemID;
        public double QtyReq;
        public double ScrapFact;
        public double BudgetUnit;
        public double BudgetExt;
        public string SearchValue;
        public bool IsDefault;
        
        public Int16 AlertType;
        public bool AlertMgr;
        public bool MilestoneMgr;
        //public bool FinanceMgr;
        //public bool ProjectMgr;
        public int ServiceTypeID;
        public string ServiceName;
        public bool IsExistRecurr;

        private DataSet _projectItem;
        private DataTable _projectDt;
        private DataTable _MilestoneDt;
        private DataTable _CustomTabItem;
        private DataTable _CustomItem;
        private DataTable _CustomItemDelete;
        private DataTable _EstimateData;
        public string PageUrl;
        public string UM;
        public string Code;
        public Int16 BomType;
        public int Phase;
        public int TypeId;
        public string Item;
        public string TypeName;
        public DataTable CustomItemDelete
        {
            get { return _CustomItemDelete; }
            set { _CustomItemDelete = value; }
        }
        public DataTable EstimateData
        {
            get { return _EstimateData; }
            set { _EstimateData = value; }
        }
        public DataTable CustomTabItem
        {
            get { return _CustomTabItem; }
            set { _CustomTabItem = value; }
        }
        public DataTable CustomItem
        {
            get { return _CustomItem; }
            set { _CustomItem = value; }
        }
        public DataTable MilestoneDt
        {
            get { return _MilestoneDt; }
            set { _MilestoneDt = value; }
        }
        public DataTable ProjectDt
        {
            get { return _projectDt; }
            set { _projectDt = value; }
        }
        public DataSet ProjectItem
        {
            get { return _projectItem; }
            set { _projectItem = value; }
        }
        public string ConnConfig
        {
            get { return _ConnConfig; }
            set { _ConnConfig = value; }
        }
        public DataSet Ds
        {
            get { return _ds; }
            set { _ds = value; }
        }
    }
}
