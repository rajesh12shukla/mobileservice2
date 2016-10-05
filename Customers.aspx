<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="Customers.aspx.cs" Inherits="Customers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <link rel="stylesheet" href="css/css3-dropdown-menu/assets/css/styles.css" />
    <link rel="stylesheet" href="css/css3-dropdown-menu/assets/font-awesome/css/font-awesome.css" />--%>

    <script type="text/javascript">
        jQuery(document).ready(function () {
            //$("#ctl00_ContentPlaceHolder1_drpReports").val('Print');

            //To remove added li before appending again
            $('#colorNav #dynamicUI li').remove();



            //To append li dynamically with Report Id, Report Name and image with global and private.
            //            for (var i = 0; i < reports.length; i++) {              
            //                var t1 = reports[i];
            //
            //            }

            // if (reports.length == 0) {
            //                $('#colorNav #dynamicUI').append('<li><a href="CustomersReport.aspx?reportId=0"><img src="images/Addnew.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Add Report</span><div style="clear:both;"></div></a></li>')
            // $('#colorNav #dynamicUI').append('<li><a href="CustomersReport.aspx?reportId=0&reportName=CustomerDetail"><img src="images/globe.png" width="18px" style="float:left;padding:5px 10px;" /><span style="float:left;padding-top:5px;">Customer Detail</span><div style="clear:both;"></div></a></li>')
            // }

            //efficient-&-compact JQuery way
            $(reports).each(function (index, report) {
                //                alert('ReportId: ' + report.ReportId +
                //      ' ReportName: ' + report.ReportName +
                //      ' IsGlobal: ' + report.IsGlobal
                //        );

                var imagePath = null;
                if (report.IsGlobal == true) {
                    imagePath = "images/globe.png";
                }
                else {
                    imagePath = "images/blog_private.png";
                }
                $('#dynamicUI').append('<li><a href="CustomersReport.aspx?reportId=' + report.ReportId + '&reportName=' + report.ReportName + '&type=Customer"><span>' + report.ReportName + '</span><div style="clear:both;"></div></a></li>')
            });
            /////


            // var countries = ['United States', 'Canada', 'Argentina', 'Armenia', 'United States', 'Canada', 'Argentina', 'Armenia'];
            // $.each(countries, function(index, userName) {
            //$('#colorNav #dynamicUI').append('  <li><a href="CustomersReport.aspx?reportId=4&reportName=Nitin"><img src="images/print.png" width="20px" /> &nbsp;&nbsp;&nbsp;&nbsp;' + userName + '</a></li>')
            // });
        });
    </script>

    <style>
        .print-dropdown {
            width: 50%; /* Set width. Do not set height else it will fail in IE8-10. Use padding for height. */
            color: #fff;
            font-weight: bold;
            font-size: 12px;
            line-height: 1.2em;
            margin: 0 0 10px;
            padding: 3px 16px; /* use this to set a specific height for your dropdown (DO NOT use the attribute 'height') */
            border: 1px solid transparent;
            cursor: pointer;
            text-indent: 0.01px;
            text-overflow: "";
            background: url('images/print_btn3.png') no-repeat 100% 0px transparent; /* add your own arrow image */
            -webkit-appearance: none; /* gets rid of default appearance in Webkit browsers*/
            -moz-appearance: none; /* Get rid of default appearance for older Firefox browsers */
            -ms-appearance: none; /* get rid of default appearance for IE8, 9 and 10*/
            appearance: none;
            width: 70px;
            float: left;
            height: 22px;
        }

            .print-dropdown option {
                background: #fff; /* style the dropdown bg color */
                color: #000;
            }

            .print-dropdown:hover {
                border: 1px solid #EEE;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Customers</asp:Label></li>
                            <li>
                                <asp:LinkButton CssClass="icon-addnew" ID="lnkAddnew" runat="server" ToolTip="Add new" OnClick="lnkAddnew_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton CssClass="icon-edit" ID="btnEdit" runat="server" ToolTip="Edit" OnClick="btnSubmit_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton CssClass="icon-copy" ID="btnCopy" runat="server" ToolTip="Copy" OnClick="btnCopy_Click"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton CssClass="icon-delete" ID="btnDelete" runat="server" ToolTip="Delete" OnClick="btnDelete_Click" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_gvUsers','customer');"></asp:LinkButton></li>
                            <li>
                                <ul class="nav navbar-nav pull-right" style="float: left !important">
                                    <li class="dropdown dropdown-user">
                                        <a href="customersreport.aspx" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important" ></a>
                                        <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                            <li><a href="CustomersReport.aspx?type=Customer"><span>Add New Report</span><div style="clear:both;"></div></a></li>
                                        </ul>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close" CssClass="icon-closed"
                                    OnClick="lnkClose_Click"></asp:LinkButton>
                            </li>
                        </ul>
                    </asp:Panel>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div class="title_bar">

                            <div id="divSpace" class="Close_button">
                            </div>
                            <asp:LinkButton ID="lnkSyncQB" CssClass="buttons" runat="server" OnClick="lnkSyncQB_Click"
                                Visible="False">Sync with QB</asp:LinkButton>
                        </div>
                        <div class="search-customer">
                            <div class="sc-form">
                                <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        Search for customers where
                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                    CssClass="form-control input-sm input-small">
                                    <asp:ListItem>Select</asp:ListItem>
                                    <asp:ListItem Value="name">Name</asp:ListItem>
                                    <asp:ListItem Value="Address">Address</asp:ListItem>
                                    <asp:ListItem Value="o.Status">Status</asp:ListItem>
                                    <asp:ListItem Value="o.type">Type</asp:ListItem>
                                    <asp:ListItem Value="City">City</asp:ListItem>
                                    <asp:ListItem Value="Phone">Phone</asp:ListItem>
                                    <asp:ListItem Value="Website">Website</asp:ListItem>
                                    <asp:ListItem Value="Email">Email</asp:ListItem>
                                    <asp:ListItem Value="Cellular">Cellular</asp:ListItem>
                                    <asp:ListItem Value="sageid">Sage ID</asp:ListItem>
                                </asp:DropDownList>
                                        <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                        <asp:DropDownList ID="rbStatus" runat="server" Visible="False" CssClass="form-control input-sm input-small">
                                            <asp:ListItem Value="0">Active</asp:ListItem>
                                            <asp:ListItem Value="1">Inactive</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control input-sm input-small"
                                            Visible="False">
                                        </asp:DropDownList>
                                        <%--<asp:ImageButton ID="lnkSearch" runat="server" ImageUrl="images/search.png" OnClick="lnkSearch_Click"
                                    ToolTip="Search" />--%>
                                        <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false"
                                            OnClick="lnkSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <ul>
                                <li>
                                    <asp:LinkButton ID="lnkClear" runat="server" OnClick="lnkClear_Click">Clear</asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click">Show All Customers</asp:LinkButton></li>
                                <li>
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <span>
                                                <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label></span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </li>
                            </ul>
                        </div>
                        <div class="clearfix">
                        </div>
                        <div class="table-scrollable" style="padding-top: 15px; border: none">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lnkSearch" />
                                    <asp:AsyncPostBackTrigger ControlID="lnkShowall" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" OnRowCommand="gvUsers_RowCommand"
                                        CssClass="table table-bordered table-striped table-condensed flip-content" OnRowDataBound="gvUsers_RowDataBound" DataKeyNames="ID"
                                        Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvUsers_PageIndexChanging"
                                        OnSorting="gvUsers_Sorting" PageSize="20" ShowFooter="True" OnDataBound="gvUsers_DataBound">
                                        <RowStyle CssClass="evenrowcolor" />
                                        <FooterStyle CssClass="footer" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="hdnSelected" runat="server" />
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             
                                            <asp:TemplateField HeaderText="Sage ID" Visible="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSageId" runat="server" Text='<%# Bind("sageid") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField>
                                                                <ItemTemplate>
                                                                  <asp:Image ID="imgQB" runat="server" Width="16px" ToolTip="Synced" ImageUrl='<%# Eval("qbcustomerid").ToString() != "" ? "images/qb32.png" : "data:image/png;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"%>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="name" HeaderStyle-Width="130px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server"><%#Eval("name")%></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                </FooterTemplate>
                                                <HeaderStyle Width="200px" />
                                                <FooterStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Address" SortExpression="address" HeaderStyle-Width="180px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAddress" runat="server"><%#Eval("address")%></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="200px" />
                                                <FooterStyle Width="200px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Type" SortExpression="Type" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="100px" />
                                                <FooterStyle Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="City" SortExpression="City">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCity" runat="server"><%#Eval("City")%></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="150px" />
                                                <FooterStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phone" SortExpression="Phone">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPhone" runat="server"><%#Eval("Phone")%></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="150px" />
                                                <FooterStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Website" SortExpression="Website" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblWebsite" runat="server"><%#Eval("Website")%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Email" SortExpression="Email" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cellular" SortExpression="Cellular" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCellular" runat="server"><%#Eval("Cellular")%></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle Width="150px" />
                                                <FooterStyle Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="# of locations" SortExpression="loc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbllocs" runat="server"><%#Eval("loc")%></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblLocsTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="# of Equipments" SortExpression="equip">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblequip" runat="server"><%#Eval("equip")%></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblequipTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="# of calls" SortExpression="opencall">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblopencall" runat="server"><%#Eval("opencall")%></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblopencallTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Balance" SortExpression="Balance"  ItemStyle-HorizontalAlign="Right" >
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBalance" runat="server"  ForeColor='<%# Convert.ToDouble(Eval("balance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Balance", "{0:c}")%>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblBalanceTotal" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
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
                    <div class="clearfix"></div>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
