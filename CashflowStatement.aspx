<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="CashflowStatement.aspx.cs" Inherits="CashflowStatement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
        <div class="page-content">

        <div class="page-cont-top">
            <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Financial Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Statement of Cashflow</span>
                </li>

            </ul>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                   Statement of Cashflow
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                          
                    <div class="table-scrollable" style="border: none">
                        <div class="col-lg-12 col-md-12">
                            <div class="search-customer">
                                <div class="sc-form">
                                    <div>
                                        <label for="">Start</label>
                                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" onkeypress="return false;" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtStartDate">
                                        </asp:CalendarExtender>
                                        <label for="" style="margin-left: 39px; margin-bottom: 19px;">End</label>
                                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control"
                                            MaxLength="50" Width="130px" onkeypress="return false;" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtEndDate">
                                        </asp:CalendarExtender>
                                        <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false" ToolTip="Refresh"
                                       ><i class="fa fa-refresh"></i></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        
                        <div class="clearfix"></div>
                        <div style="width:100%; display: inline-block; padding:8px 15px;">
                            <span style="float:left; cursor:pointer">
                                 <asp:Label runat="server" ID="lblNet"  Font-Bold="True" />
                            </span>
                            <span style="float:right; cursor:pointer">
                                 <asp:Label runat="server" ID="lblNetAmount" Font-Underline="False" Font-Bold="True" />
                            </span>
                        </div>
                    </div>

                </div>
        </div>
        </div>
       </div>
</asp:Content>

