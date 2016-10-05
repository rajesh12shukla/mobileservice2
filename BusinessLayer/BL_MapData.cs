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
    public class BL_MapData
    {
        DL_MapData objDL_MapData = new DL_MapData();

        public void AddMapData(MapData objMapData)
        {
            objDL_MapData.AddMapData(objMapData);
        }

        public void UpdateTicket(MapData objMapData)
        {
            objDL_MapData.UpdateTicket(objMapData);
        }

        public void DeleteTicket(MapData objMapData)
        {
            objDL_MapData.DeleteTicket(objMapData);
        }

        public void UpdateTicketStatus(MapData objMapData)
        {
            objDL_MapData.UpdateTicketStatus(objMapData);
        }

        public void UpdateTicketResize(MapData objMapData)
        {
            objDL_MapData.UpdateTicketResize(objMapData);
        }


        public void AddTicket(MapData objMapData)
        {
            objDL_MapData.AddTicket(objMapData);
        }

        public void AddTicketTS(MapData objMapData)
        {
            objDL_MapData.AddTicketTS(objMapData);
        }

        public int UpdateTicketInfo(MapData objMapData)
        {
            return objDL_MapData.UpdateTicketInfo(objMapData);
        }

        public int UpdateTicketInfoTS(MapData objMapData)
        {
            return objDL_MapData.UpdateTicketInfoTS(objMapData);
        }


        public DataSet GetTimestmpLocation(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocation(objMapData);
        }

        public DataSet getlocationAddress(MapData objMapData)
        {
            return objDL_MapData.getlocationAddress(objMapData);
        }

        public DataSet GetOpenTicket(MapData objMapData)
        {
            return objDL_MapData.GetOpenTicket(objMapData);
        }

        public DataSet GetOpenTicketScheduler(MapData objMapData)
        {
            return objDL_MapData.GetOpenTicketScheduler(objMapData);
        }

        public DataSet GetReportTicket(MapData objMapData)
        {
            return objDL_MapData.GetReportTicket(objMapData);
        }

        public DataSet GetClosedTicket(MapData objMapData)
        {
            return objDL_MapData.GetClosedTicket(objMapData);
        }

        public DataSet GetClosedTicketDTable(MapData objMapData)
        {
            return objDL_MapData.GetClosedTicketDTable(objMapData);
        }

        public DataSet GetNearWorkers(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkers(objMapData);
        }

        public DataSet GetNearWorkersByTime(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkersByTime(objMapData);
        }

        public DataSet GetNearWorkersDummy(MapData objMapData)
        {
            return objDL_MapData.GetNearWorkersDummy(objMapData);
        }

        public DataSet GetTimestmpLocationList(MapData objMapData)
        {
            return objDL_MapData.GetTimestmpLocationList(objMapData);
        }

        public DataSet GetCurrentLocation(MapData objMapData)
        {
            return objDL_MapData.GetCurrentLocation(objMapData);
        }

        public DataSet GetTechCurrentLocation(MapData objMapData)
        {
            return objDL_MapData.GetTechCurrentLocation(objMapData);
        }

        public DataSet GetTicketByID(MapData objMapData)
        {
            return objDL_MapData.GetTicketByID(objMapData);
        }

        public DataSet GetTicketbyWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetTicketbyWorkorder(objMapData);
        }

        public DataSet GetWorkorderDate(MapData objMapData)
        {
            return objDL_MapData.GetWorkorderDate(objMapData);
        }

        public string GetopportunityTicket(MapData objMapData)
        {
            return objDL_MapData.GetopportunityTicket(objMapData);
        }

        public DataSet GetChargeableTickets(MapData objMapData)
        {
            return objDL_MapData.GetChargeableTickets(objMapData);
        }

        public DataSet GetChargeableTicketsMapping(MapData objMapData)
        {
            return objDL_MapData.GetChargeableTicketsMapping(objMapData);
        }

        public DataSet getCallHistoryLocationOLD(MapData objMapData)
        {
            return objDL_MapData.getCallHistoryLocationOLD(objMapData);
        }

        public DataSet getCallHistory(MapData objMapData)
        {
            return objDL_MapData.getCallHistory(objMapData);
        }

        public DataSet getTicketdetailsReport(MapData objMapData)
        {
            return objDL_MapData.getTicketdetailsReport(objMapData);
        }

        public DataSet GetTicketsByWorkerDateOLD(MapData objMapData)
        {
            return objDL_MapData.GetTicketsByWorkerDateOLD(objMapData);
        }

        public DataSet GetRecurringTickets(MapData objMapData)
        {
            return objDL_MapData.GetRecurringTickets(objMapData);
        }

        public void AddFile(MapData objMapData)
        {
            objDL_MapData.AddFile(objMapData);
        }

        public void UpdateQBInvoiceTicketID(MapData objMapData)
        {
            objDL_MapData.UpdateQBInvoiceTicketID(objMapData);
        }

        public void UpdateQBTimeTxnIDTicket(MapData objMapData)
        {
            objDL_MapData.UpdateQBTimeTxnIDTicket(objMapData);
        }

        public void UpdateFile(MapData objMapData)
        {
            objDL_MapData.UpdateFile(objMapData);
        }

        public void DeleteFile(MapData objMapData)
        {
            objDL_MapData.DeleteFile(objMapData);
        }

        public DataSet SelectTempDocumentFile(MapData objMapData)
        {
            return objDL_MapData.SelectTempDocumentFile(objMapData);
        }

        public DataSet GetDocuments(MapData objMapData)
        {
            return objDL_MapData.GetDocuments(objMapData);
        }

        public DataSet GetLibrary(MapData objMapData)
        {
            return objDL_MapData.GetLibrary(objMapData);
        }

        public DataSet GetSignature(MapData objMapData)
        {
            return objDL_MapData.GetSignature(objMapData);
        }

        public DataSet GetTicketSignature(MapData objMapData)
        {
            return objDL_MapData.GetTicketSignature(objMapData);
        }

        public DataSet GetInvoiceTicketByWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetInvoiceTicketByWorkorder(objMapData);
        }

        public DataSet GetTicketsByWorkorder(MapData objMapData)
        {
            return objDL_MapData.GetTicketsByWorkorder(objMapData);
        }

        public DataSet GetRecentCallsLoc(MapData objMapData)
        {
            return objDL_MapData.GetRecentCallsLoc(objMapData);
        }

        public DataSet GetTicketTime(MapData objMapData)
        {
            return objDL_MapData.GetTicketTime(objMapData);
        }

        public DataSet GetTicketTimeMapping(MapData objMapData)
        {
            return objDL_MapData.GetTicketTimeMapping(objMapData);
        }

        public void UpdateReviewStatus(MapData objMapData)
        {
            objDL_MapData.UpdateReviewStatus(objMapData);
        }

        public void IndexMapdata(MapData objMapData)
        {
            objDL_MapData.IndexMapdata(objMapData);
        }

        public DataSet getElevByTicket(MapData objMapData)
        {
            return objDL_MapData.getElevByTicket(objMapData);
        }
    }
}
