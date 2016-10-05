<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="ChartOfAccount.aspx.cs" Inherits="ChartOfAccount" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function CheckDelete() {
            var result = false;
            $("#<%=gvChartOfAccount.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to delete this Account ?');
            }
            else {
                alert('Please select an account to delete.')
                return false;
            }
        }
        function CheckSelect() {
            var result = false;
            $("#<%=gvChartOfAccount.ClientID%> tr").each(function () {
                var checkBox = $(this).find("input[type='checkbox']");
                if (checkBox.is(":checked")) {
                    result = true;
                }
            });

            if (result == true) {
                return confirm('Do you really want to copy this record?');
            }
            else {
                alert('Please select an account to copy.')
                return false;
            }
        }
        function notifyDelete() {
           
            noty({
                text: 'You can not delete this record!',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        function defaultNotifyDelete() {
            
            noty({
                text: 'You can not delete default account!',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
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
                    <span>Financial Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Chart Of Account</span>
                </li>

            </ul> --%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="Label1" runat="server">Chart of Account</asp:Label></li>
                        <li>
                            <asp:LinkButton CssClass="icon-addnew" ToolTip="Add New" ID="lnkAddnew"
                                runat="server" OnClick="lnkAddnew_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-edit" ToolTip="Edit"
                                ID="btnEdit" runat="server" OnClick="btnEdit_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-copy" ID="lnkCopy" ToolTip="Copy"
                                runat="server" OnClick="lnkCopy_Click" OnClientClick="return CheckSelect();"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-delete" ToolTip="Delete"
                                ID="lnkDelete" runat="server" OnClientClick="return CheckDelete();" CausesValidation="False"
                                OnClick="lnkDelete_Click"></asp:LinkButton></li>
                        <li>
                            <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear: both;"></div>
                                        </a></li>
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="LinkButton1" ToolTip="Close" ForeColor="Red" runat="server" CausesValidation="false"
                                OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="col-lg-12 col-md-12">

                                    <div class="search-customer">
                                        <div class="sc-form" style="position: relative;">

                                            <label for="">Search the accounts where</label>
                                            <asp:DropDownList ID="ddlSearch" runat="server" CssClass="form-control input-sm input-small" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged" AutoPostBack="true">
                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                <asp:ListItem Value="1">Acct No.</asp:ListItem>
                                                <asp:ListItem Value="2">Account Name</asp:ListItem>
                                                <asp:ListItem Value="3">Balance</asp:ListItem>
                                            </asp:DropDownList>

                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small "
                                                placeholder="Search.."></asp:TextBox>

                                            <asp:DropDownList ID="ddlBalanceCondition" runat="server"
                                                CssClass="form-control input-sm input-small ">
                                                <asp:ListItem Value="0"> -- Condition -- </asp:ListItem>
                                                <asp:ListItem Value="1">&lt;&gt;</asp:ListItem>
                                                <asp:ListItem Value="2">&lt;</asp:ListItem>
                                                <asp:ListItem Value="3">&gt;</asp:ListItem>
                                                <asp:ListItem Value="4">&lt;=</asp:ListItem>
                                                <asp:ListItem Value="5">&gt;=</asp:ListItem>
                                            </asp:DropDownList>

                                        </div>
                                    </div>
                                    <div class="search-customer sat-select">
                                        <div class="sc-form">
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control input-sm input-small">
                                            </asp:DropDownList>

                                            <asp:DropDownList ID="ddlSubAcCategory" runat="server" CssClass="form-control input-sm input-small">
                                            </asp:DropDownList>

                                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm input-small">
                                            </asp:DropDownList>

                                            <asp:LinkButton ID="btnSearch" CssClass="btn submit" ToolTip="Search" runat="server" CausesValidation="false"
                                                OnClick="btnSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                        <ul>
                                            <li>
                                                <asp:UpdatePanel ID="updRecordCount" runat="server">
                                                    <ContentTemplate>
                                                        <span>
                                                            <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label></span>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="clearfix"></div>
                    <div class="table-scrollable" style="border: none">
                        <asp:UpdatePanel ID="uPnlChart" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />

                            </Triggers>
                            <ContentTemplate>
                                <%--<asp:Panel runat="server" ID="pnlGridButtons" Style="border-style: solid solid none solid; padding-top: 4px; background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; border-width: 1px; border-color: #a9c6c9; margin-top: 10px;">
                                   
                                </asp:Panel>--%>
                                <asp:GridView ID="gvChartOfAccount" CssClass="table table-bordered table-striped table-condensed flip-content" runat="server" AutoGenerateColumns="False"
                                    ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                    ShowFooter="True"
                                    EnableModelValidation="True"
                                    OnRowDataBound="gvChartOfAccount_RowDataBound" OnRowCommand="gvChartOfAccount_RowCommand"
                                    OnPageIndexChanging="gvChartOfAccount_PageIndexChanging"
                                    AllowSorting="true" AllowPaging="true" PageSize="20" OnSorting="gvChartOfAccount_Sorting" OnDataBound="gvChartOfAccount_DataBound">
                                    <FooterStyle CssClass="footer" />
                                    <RowStyle CssClass="evenrowcolor" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <Columns>
                                        <asp:TemplateField ItemStyle-Width="1%">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                <asp:HiddenField ID="hdnTypeId" Value='<%#Eval("Type") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Acct" HeaderText="Acct" SortExpression="Acct" ItemStyle-Width="6%" />
                                        <asp:BoundField DataField="fDesc" HeaderText="Description" SortExpression="fDesc" ItemStyle-Width="20%" />

                                        <asp:TemplateField HeaderText="Type" ItemStyle-Width="6%" SortExpression="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("AcctType") %>' CommandArgument='<%#Eval("ID")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sub" ItemStyle-Width="5%" SortExpression="Sub">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSub" runat="server" Text='<%# Bind("Sub") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label Text="Page Total" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label Text="Grand Total" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Debit" ItemStyle-Width="8%" SortExpression="ID" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebit" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%# Convert.ToDouble(Eval("balance")) > 0 ? DataBinder.Eval(Container.DataItem, "Balance", "{0:c}") : "$0.00" %>' />
                                            </ItemTemplate>
                                            <%-- <FooterTemplate>
                                                    <asp:Label ID="lblTotalDebit" runat="server"></asp:Label>
                                                </FooterTemplate>--%>
                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label ID="lblTotalDebit" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalDebit" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit" ItemStyle-Width="8%" SortExpression="ID" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate >
                                                <asp:Label ID="lblCredit" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%# Convert.ToDouble(Eval("balance")) < 0 ? string.Format("{0:c}", (Convert.ToDouble(Eval("balance")) * -1)) : "$0.00" %>'/>
                                            </ItemTemplate>

                                            <FooterTemplate>
                                                <div style="padding: 0 0 5px 0">
                                                    <asp:Label ID="lblTotalCredit" runat="server" />
                                                </div>
                                                <div>
                                                    <asp:Label ID="lblGrandTotalCredit" runat="server" />
                                                </div>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="8%" SortExpression="Status">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatus" runat="server" CommandArgument='<%#Eval("ID")%>' Text='<%#Eval("AcctStatus")%>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>

                                    <PagerTemplate>
                                        <div align="center">
                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                            &nbsp &nbsp
                                            <asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
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
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
        <!-- edit-tab end -->
        <div class="clearfix"></div>
    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
</asp:Content>
<%--</form>
</body>
</html>--%>
