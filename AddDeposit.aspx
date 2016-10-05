<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddDeposit.aspx.cs" Inherits="AddDeposit" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .parent {
            width: 100%;
        }

            .parent .sub1 {
                padding-top: 5px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
                padding-left: 10px;
                /*background-color: red;*/
            }

            .parent .sub2 {
                /*background-color: blue;*/
                /*padding-top: 5px;*/
                float: left;
                width: 200px;
            }

            .parent .sub3 {
                padding-left: 100px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub33 {
                padding-top: 5px;
                padding-left: 10px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub4 {
                float: left;
                width: 190px;
            }

            .parent .sub5 {
                padding-left: 10px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
                padding-top: 5px;
            }

            .parent .sub6 {
                float: left;
                width: 190px;
            }
    </style>
    <script type="text/javascript">
        function makeReadonly(txt) {
            $("#" + txt.id).prop('readonly', true);
        }
        function VisibleRow(row, txt, gridview, event) {  //To make row's textbox visible

            var rowst = document.getElementById(row)

            var grid = document.getElementById(gridview);
            $('#<%=gvDeposit.ClientID %> input:text.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#<%=gvDeposit.ClientID %> select.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");

            });

            var txtDescription = document.getElementById(txt);
            $(txtDescription).removeClass("texttransparent");
            $(txtDescription).addClass("non-trans");

            //var ddlPaymentMethod = document.getElementById(txt.replace('txtDescription', 'ddlPaymentMethod'));
            //$(ddlPaymentMethod).removeClass("texttransparent");
            //$(ddlPaymentMethod).addClass("non-trans");

            //var txtCheckNo = document.getElementById(txt.replace('txtDescription', 'txtCheckNo'));
            //$(txtCheckNo).removeClass("texttransparent");
            //$(txtCheckNo).addClass("non-trans");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <%-- <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Financial Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/managedeposit.aspx") %>">Make Deposit</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Make Deposit</span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Make Deposit</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                         <li>
                            <asp:Label ID="lblRef" runat="server" Text="Ref #" Visible="False"></asp:Label></li>
                        <li>
                            <asp:Label ID="lblRefId" runat="server" Visible="False" Style="font-weight: bold; font-size: 15px;"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="true">
                                    <ul class="lnklist-header">
                                        <li style="margin-right: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" CssClass="icon-first"
                                                OnClick="lnkFirst_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" CssClass="icon-previous"
                                                OnClick="lnkPrevious_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" CssClass="icon-next"
                                                OnClick="lnkNext_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" CssClass="icon-last"
                                                OnClick="lnkLast_Click">
                                            </asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>

                            </asp:Panel>
                        </li>
                         <li>
                            <asp:LinkButton CssClass="icon-save" ToolTip="Save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="Deposit"
                                TabIndex="38"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                TabIndex="39" OnClick="lnkClose_Click"></asp:LinkButton>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                     <div class="alert alert-success" runat="server" id="divSuccess" > 
                    <button type="button" class="close" data-dismiss="alert">×</button>
                    These month/year period is closed out. You do not have permission to add/update this record.
                    </div>
                
               <asp:UpdatePanel ID="uPnlDeposit" runat="server">
                   <ContentTemplate>
                    <div class="col-md-3 col-lg-3">
                        <div class="form-col">
                            <div class="fc-label">
                                        <label>Deposit to </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtDepositTo" runat="server" CssClass="form-control" TabIndex="2"
                               MaxLength="15" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label">
                                        <label>Deposit Total </label>
                            </div>
                            <div class="fc-input" style="padding-top: 5px;">
                                <asp:UpdatePanel ID="uPnlDepositTotal" runat="server">
                                    <ContentTemplate>
                                        <asp:Label ID="lblDepositTotal" runat="server" Font-Size="Small"  Font-Bold="True"></asp:Label>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3">
                        <div class="form-col">
                            <div class="fc-label">
                                <label>Date </label>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="2"
                                MaxLength="15" autocomplete="off"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtDate">
                                </asp:CalendarExtender>
                                <asp:RequiredFieldValidator runat="server" ID="rfvDate" ControlToValidate="txtDate"
                                    ErrorMessage="Please enter Date." Display="None"
                                    ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvDate" />
                                <asp:RegularExpressionValidator ID="revDate" ControlToValidate = "txtDate" ValidationGroup="Deposit" 
                                    ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="revDate" />
                            </div>
                        </div>
                        <div class="form-col">
                                <asp:Image ID="imgCleared" runat="server" ImageUrl="~/images/icons/Cleared.png"/>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3">
                        <div class="form-col">
                            <div class="fc-label">
                                <label>Memo </label>
                            </div>
                            <div class="fc-input">
                                    <asp:TextBox ID="txtMemo" runat="server" CssClass="form-control" TabIndex="2"
                                    MaxLength="250" autocomplete="off"></asp:TextBox>
                                    <asp:RequiredFieldValidator runat="server" ID="rfvMemo" ControlToValidate="txtMemo"
                                    ErrorMessage="Please enter Memo." Display="None"
                                    ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvMemo" />
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 col-lg-3">
                        <div class="form-col">
                            <div class="fc-label">
                                <label>Bank </label>
                            </div>
                            <div class="fc-input">
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                    ErrorMessage="Please select Bank" Display="None" InitialValue="0"
                                    ValidationGroup="Deposit"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                    TargetControlID="rfvBank" />
                            </div>
                        </div>
                    </div>
                
                <%--    <asp:UpdatePanel ID="uPnlDeposit" runat="server">
                            <ContentTemplate>--%>
                    <div style="margin-top: 40px;" class="table-scrollable">
                        <asp:GridView ID="gvReceivePayment" runat="server" AutoGenerateColumns="False"
                            CssClass="table table-bordered table-striped table-condensed flip-content" ShowFooter="True" Width="100%"
                            ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                            <AlternatingRowStyle CssClass="oddrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <RowStyle CssClass="evenrowcolor" />
                            <HeaderStyle Height="10px" />
                            <SelectedRowStyle CssClass="selectedrowcolor" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="1%">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                        <asp:HiddenField ID="hdnSelected" runat="server" />
                                        <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PaymentReceivedDate" HeaderText="Date" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="3%" />
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("customerName") ==string.Empty ? " - " : Eval("customerName") %>' 
                                                ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ItemStyle-Width="10%" HeaderText="Location" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                            <asp:Label ID="lblTag" runat="server" Text='<%# Eval("Tag") ==string.Empty ? " - " : Eval("Tag") %>' 
                                                ></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" ReadOnly="True" HeaderStyle-Width="5%"/>
                                <asp:TemplateField ItemStyle-Width="5%" HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'
                                            Style="float: right;"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </div>

                    <div style="border: none" class="table-scrollable">
                        <div style="padding-top: 5px; padding-bottom: 5px;">
                            <asp:Label ID="lblCustomerPayment" runat="server" Font-Size="Small" Width="100%" Font-Bold="True"></asp:Label>
                        </div>
                
                                <asp:GridView ID="gvDeposit" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-bordered table-striped table-condensed flip-content" ShowFooter="True" Width="100%"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                    > <%--OnRowDataBound="gvDeposit_RowDataBound"--%>
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                    <RowStyle CssClass="evenrowcolor" />
                                    <HeaderStyle Height="10px" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="10%" HeaderText="Customer Name" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("customerName") ==string.Empty ? " - " : Eval("customerName") %>' 
                                                        ></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-Width="7%" HeaderText="Location" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                    <asp:Label ID="lblTag" runat="server" Text='<%# Eval("Tag").ToString() ==string.Empty ? " - " : Eval("Tag") %>'>
                                                    </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="From Account" ItemStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUndepositedFund" runat="server">Undeposited Funds</asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="250" Width="100%" 
                                                    Height="26px" autocomplete="off"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CheckNumber" HeaderText="Check No." ReadOnly="True"  ItemStyle-Width="5%"/>
                                        <asp:BoundField DataField="PaymentMethod" HeaderText="Payment Method" ReadOnly="True" ItemStyle-Width="5%"/>
                                        <asp:TemplateField ItemStyle-Width="1%" HeaderText="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            
                    </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                    </Triggers>
                </asp:UpdatePanel>        
                </div>
            </div>
           
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

