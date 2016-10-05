<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="BillingCodes.aspx.cs" Inherits="BillingCodes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function openModelPopup() {
            $("#BillingCodePopup").modal('show');
            //$("#bsadsaasic").modal('show');
            event.preventDefault();
        }
        function ace_itemSelected(sender, e) {
            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            hdnPatientId.value = e.get_value();
        }
        function EmptyValue(txt)
        {
            if ($(txt).val() == '') { $('#<%= hdnPatientId.ClientID %>').val(''); }
        }
        function ChkGL(sender, args) {
            var hdnGL = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnGL.value == '') {
                args.IsValid = false;
            }
            else if(hdnGL.value == '0') {
                args.IsValid = false;
            }
        }
    </script>
    <style type="text/css">
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     
    <div class="page-content">
        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Billing Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>--%>
            <%-- <li>
                        <a href="/invoices.aspx">Invoices</a>
                        <i class="fa fa-angle-right"></i>
                    </li>--%>
            <%--<li>
                    <span>Billing Codes</span>
                </li>
            </ul>--%>
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="Panel1">
                        <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Billing Codes</asp:Label></li>

                            <li>
                                <asp:Panel runat="server" ID="pnlGridButtons">
                                    <ul class="lnklist-header">
                                        <li style="margin-right:0">
                                            <asp:LinkButton CssClass="icon-addnew" ID="LinkButton10" ToolTip="Add New"
                                                runat="server" OnClick="lnkAddBillingCodes_Click" CausesValidation="False"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton CssClass="icon-edit" ToolTip="Edit"
                                                ID="LinkButton8" runat="server" OnClick="lnkEditBillingCodes_Click" CausesValidation="False"></asp:LinkButton>
                                        <li>
                                            <asp:LinkButton CssClass="icon-delete" ID="LinkButton9" ToolTip="Delete"
                                                runat="server" OnClientClick="return confirm('Do you really want to delete this Billing Code ?');"
                                                OnClick="lnkDeleteBillingCodes_Click" CausesValidation="False"></asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            </li>
                            <li>
                                <ul class="nav navbar-nav pull-right">
                                    <li class="dropdown dropdown-user">
                                        <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                        <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                            <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear:both;"></div></a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                    OnClick="lnkClose_Click"></asp:LinkButton></li>
                        </ul>
                    </asp:Panel>

                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="table-scrollable" style="border: none">
                        <div>
                            <%--<asp:Panel runat="server" ID="pnlGridButtons" Style="border-style: solid solid none solid; background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;">
                            </asp:Panel>--%>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:GridView ID="gvBillCodes" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                        Width="100%" PageSize="20" AllowPaging="True" AllowSorting="True" OnDataBound="gvBillCodes_DataBound"
                                        OnRowCommand="gvBillCodes_RowCommand" OnSorting="gvBillCodes_Sorting">
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Billing Code" SortExpression="Name">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBillID" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    <asp:Label ID="lblStatusID" Visible="false" runat="server" Text='<%# Eval("Statusid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="GL Account" SortExpression="account">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblaccount" runat="server" Text='<%# Eval("account") %>'></asp:Label>
                                                    <asp:Label ID="lblAcctID" runat="server" Visible="false" Text='<%# Eval("sacct") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Rate" SortExpression="Price1">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblbalance" runat="server" Text='<%# Eval("Price1") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Measure" SortExpression="Measure">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMeasure" runat="server" Text='<%# Eval("Measure") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Remarks" SortExpression="Remarks">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblrem" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle CssClass="footer" />
                                        <RowStyle CssClass="evenrowcolor" />
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                        <PagerTemplate>
                                            <div align="center">
                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                    ImageUrl="~/images/Backward.png" />
                                                &nbsp &nbsp <span>Page</span>
                                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span>of </span>
                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                &nbsp &nbsp
                                <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png" />
                                                &nbsp &nbsp
                                <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" ImageUrl="images/last.png" />
                                            </div>
                                        </PagerTemplate>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>
            </div>
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                BackgroundCssClass="ModalPopupBG" PopupDragHandleControlID="TitlePop"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="programmaticPopup" Style="display: block; background: #fff; border: 1px solid #316b9d;"
                CssClass="table-billing">
                <div class="title_bar_popup" id="TitlePop" runat="server" style="cursor: move;">
                    <asp:Label CssClass="title_text" ID="Label8" Style="color: white" runat="server">Bill Code</asp:Label>
                    <asp:LinkButton ID="LinkButton11" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                        OnClick="lnkBillingCodesClose_Click">Close</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton12" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                        ValidationGroup="serv" OnClick="lnkBillingCodesSave_Click">Save</asp:LinkButton>
                </div>
                <div>
                    <asp:Panel ID="pnlBillCode" runat="server">
                        <div class="col-lg-12" style="padding-top: 15px">
                            <asp:HiddenField ID="hdnBillID" runat="server" />
                            <input id="hdnPatientId" runat="server" type="hidden" />
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Billing Code
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtBillCode"
                                            Display="None" ErrorMessage="Billing Code Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBillCode" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Billing Code Description</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBillDesc" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                               <div class="form-group">
                                <div class="form-col">
                                      <div class="fc-label">
                                        <label>GL Account</label>
                                        <asp:RequiredFieldValidator ID="rfvGL" runat="server" ControlToValidate="txtAccount"
                                            Display="None" ErrorMessage="GL Account is Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceGL" PopupPosition="BottomLeft"
                                            runat="server" Enabled="True" TargetControlID="rfvGL">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:CustomValidator ID="cvGL" runat="server" ClientValidationFunction="ChkGL" ValidationGroup="serv"
                                            ControlToValidate="txtAccount" Display="None" ErrorMessage="Please select GL Account" 
                                            SetFocusOnError="True"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="vceGL1" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="cvGL">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                            <asp:TextBox ID="txtAccount" runat="server" CssClass="form-control searchinput"
                                onkeyup="EmptyValue(this);"
                                autocomplete="off" placeholder="Search by Account # or Description"></asp:TextBox>
                            <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtAccount"
                                EnableCaching="False" ServiceMethod="GetAccounts" UseContextKey="True" MinimumPrefixLength="0"
                                CompletionListCssClass="autocomplete_completionListElement" 
                                CompletionListItemCssClass="autocomplete_listItem"
                                CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                CompletionListElementID="ListDivisor" 
                                OnClientItemSelected="ace_itemSelected"
                                ID="AutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                            </asp:AutoCompleteExtender>

                                 </div>   </div></div>
                            <div ID="ListDivisor"></div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Status</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Rate
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtBillBal"
                                Display="None" ErrorMessage="Rate Required" SetFocusOnError="True" ValidationGroup="serv"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender" PopupPosition="BottomLeft"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBillBal" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtBillBal_FilteredTextBoxExtender" runat="server"
                                            Enabled="True" TargetControlID="txtBillBal" ValidChars="1234567890.-">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Measure</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBillMeasure" runat="server" MaxLength="10" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Remarks</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBillRemarks" TextMode="MultiLine" runat="server" MaxLength="200" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
