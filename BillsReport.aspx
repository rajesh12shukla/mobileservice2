<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="BillsReport.aspx.cs" Inherits="BillsReport" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        body:nth-of-type(1) img[src*="Blank.gif"] {
                display: none;
        }
    </style>
    <script type="text/javascript">

        function showMailReport() {
            jQuery("#txtTo").text = "";
            jQuery("#txtCC").text = "";
            $("#programmaticModalPopup").show();
            $('#incomepopup').modal('show');
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="page-content">
        <div class="page-cont-top">
           
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Bills Report</asp:Label></li>
                        <li><a id="LinkButton1" onclick="showMailReport();" title="Mail Report" class="icon-mail"></a></li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                    <li class="dropdown dropdown-user">
                                        <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                        <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                            <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                            </a></li>
                                            <li style="margin-left: 0px;"> <a href="APAgingReport.aspx"><span>AP Aging Report</span><div style="clear: both;"></div>
                                            </a></li>
                                        </ul>
                                    </li>
                                </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="close"
                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
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
                                        <label style="margin-left: 39px; margin-bottom: 19px;">
                                            Search for invoices
                                        </label>
                                        <asp:DropDownList ID="ddlInvoice" runat="server" CssClass="form-control" TabIndex="1"  Width="130px"
                                             OnSelectedIndexChanged="ddlInvoice_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">All </asp:ListItem>
                                                <asp:ListItem Value="1">Outstanding </asp:ListItem>
                                                <asp:ListItem Value="2">Due </asp:ListItem>
                                        </asp:DropDownList>
                               
                                    <asp:TextBox ID="txtSearchDate" runat="server" CssClass="form-control" autocomplete="off"
                                                MaxLength="10" Width="130px"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtSearchDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtSearchDate">
                                </asp:CalendarExtender>
                                        <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false" ToolTip="Refresh"
                                            OnClick="lnkSearch_Click"><i class="fa fa-refresh"></i></asp:LinkButton>

                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="clearfix"></div>

                        <div class="col-lg-12 col-md-12">
                            <rsweb:ReportViewer ID="rvBills" runat="server" Width="1200px" Height="1500px"
                                BorderColor="Gray" BorderStyle="None" BorderWidth="1px" PageCountMode="Actual"
                                AsyncRendering="false" ShowZoomControl="False" OnReportRefresh="rvBills_ReportRefresh" >
                            </rsweb:ReportViewer>
                        </div>
                    </div>
                    <div class="modal fade" id="incomepopup" tabindex="-1" role="basic" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header" style="padding: 0px">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                                    <div style="background: #316b9d; padding: 10px 15px; font-size: 15px; color: #dadedf; line-height: 20px !important;">
                                        <h4 class="modal-title">
                                            <asp:Label CssClass="title_text" ID="Label15" runat="server">Setup </asp:Label>
                                        </h4>
                                    </div>
                                </div>
                                <div class="modal-body">
                                    <asp:Panel runat="server" ID="programmaticPopup">
                                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle">
                                        </asp:Panel>
                                        <div class="col-lg-12">
                                            <div class="form-col">
                                                <div>
                                                    From
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtFrom_FilteredTextBoxExtender"
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtFrom">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                        ControlToValidate="txtFrom" Display="None"
                                                        ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                        ControlToValidate="txtFrom" Display="None"
                                                        ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                        ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    To
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtTo_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtTo">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                        ControlToValidate="txtTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="txtTo" Display="None"
                                                        ErrorMessage="Please Enter E-Mail Address" SetFocusOnError="True"
                                                        ValidationGroup="mail"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    CC
                                                </div>
                                                <div>
                                                    <asp:TextBox ID="txtCC" runat="server" CssClass="form-control"></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtCC_FilteredTextBoxExtender" runat="server"
                                                        Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                        TargetControlID="txtCC">
                                                    </asp:FilteredTextBoxExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                        ControlToValidate="txtCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                        ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                        ValidationGroup="mail"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div>
                                                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Columns="50"
                                                        Rows="5" CssClass="form-control" Text="This is report email sent from Mobile Office Manager. Please find the Bills Summary Report attached."></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                                <div class="modal-footer custsetup-btn">
                                    <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="LinkButton4" Text="Close" CausesValidation="False" />

                                    <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Send" OnClick="hideModalPopupViaServerConfirm_Click"
                                        CssClass="btn blue" ValidationGroup="mail" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <!-- /.modal-content -->
                        </div>
                        <!-- /.modal-dialog -->
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

