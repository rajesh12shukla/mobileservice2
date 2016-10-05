<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddVendor.aspx.cs" Inherits="AddVendor" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function pageLoad(sender, args) {

            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }
            $(document).ready(function () {
                $("#<%=txtDefaultAcct.ClientID%>").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetAccountName",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load accounts");
                            }
                        });
                    },
                    select: function (event, ui) {
                        $("#<%=txtDefaultAcct.ClientID%>").val(ui.item.label);
                        $("#<%=hdnAcctID.ClientID%>").val(ui.item.value);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtDefaultAcct.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                .bind('click', function () { $(this).autocomplete("search"); })
                .data("autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.acct;
                    var result_desc = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }

                    if (result_value == 0) {
                        //return $("<li></li>")
                        //.data("item.autocomplete", item)
                        //.append("<a>" + result_item + "</a>")
                        //.appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + " : <span>" + result_desc + "</span></a>")
                        .appendTo(ul);
                    }
                };
            });
        }
        
    </script>
     <style>
        .ui-autocomplete {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }
    </style>

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
                    <a href="<%=ResolveUrl("~/vendors.aspx") %>">Vendors</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Vendors</span>
                </li>
            </ul>--%>
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add New Vendor</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <ul>
                                    <li style="margin-right:0">
                                        <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save"
                                            TabIndex="38"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                            OnClick="lnkClose_Click" PostBackUrl="~/Vendors.aspx" TabIndex="39"></asp:LinkButton>
                                    </li>
                                </ul>
                            </asp:Panel>
                        </li>
                    </ul>
                </div>
            </div>

            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
                    <asp:TabPanel ID="tbVendorInfo" runat="server" HeaderText="Vendor Info.">
                        <HeaderTemplate>
                             Vendor Info.
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div class="col-lg-12 col-md-12">
                                <div class="col-md-5 col-lg-5">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Vendor ID
                                        </div>
                                        <div class="fc-input">
                                            <asp:HiddenField ID="hdnRolID" runat="server" />
                                            <asp:TextBox ID="txtAccountid" CssClass="form-control" TabIndex="3" runat="server" MaxLength="31"/>
                                            <asp:RequiredFieldValidator ID="rfvAccountNum"
                                                runat="server" ControlToValidate="txtaccountid" Display="None" ErrorMessage="Vendor ID is Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender
                                                ID="vceAcctNum" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvAccountNum" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Name
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtName" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75"/>
                                            <asp:RequiredFieldValidator ID="rfvAcName"
                                                runat="server" ControlToValidate="txtName" Display="None" ErrorMessage="Name is Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender
                                                ID="vceAcctName" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvAcName" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Address
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtAddress" CssClass="form-control" TabIndex="5" runat="server" MaxLength="255"/>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            City
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCity" CssClass="form-control" TabIndex="6" runat="server" MaxLength="50"/>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            State
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" TabIndex="7">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Zip
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtZip" CssClass="form-control" TabIndex="11" runat="server" MaxLength="10"/>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Country
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCountry" CssClass="form-control" Text="United States" TabIndex="8" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-col"> 
                                        <div class="form-col" style="margin-bottom:0px;">
                                            <div class="fc-label">
                                                <h2 class="form-title">General</h2>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Type
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlType" CssClass="form-control" TabIndex="17" runat="server">
                                            <%--    <asp:ListItem Text="Select Type" Value="0" />--%>
                                                <asp:ListItem Text="Cost Of Sales" Value="1" />
                                                <asp:ListItem Text="Overhead" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Credit Limit
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCreditlimit" CssClass="form-control" Width="235px" TabIndex="19" Text="0.0" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            If Paid In
                                        </div>
                                        <div class="fc-input merchant-input">
                                            <asp:TextBox ID="txtDays" CssClass="form-control" Text="10" TabIndex="21" runat="server" Width="200px" />Days
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5 col-lg-5">
                                        <div class="form-col" style="height:30px;">
                                        <div class="fc-label">
                                        </div>
                                        <div class="fc-input">
                                        </div>
                                    </div>
                                    <%--  <div class="form-col" style="height:30px;">
                                        <div class="fc-label">
                                        </div>
                                        <div class="fc-input">
                                        </div>
                                    </div>--%>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Contact Name
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtContact" CssClass="form-control" TabIndex="14" runat="server" MaxLength="50"/>
                                        </div>
                                    </div>
                                        <div class="form-col">
                                        <div class="fc-label">
                                            Phone
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtPhone" CssClass="form-control" TabIndex="12" runat="server" MaxLength="28"/>
                                            <asp:MaskedEditExtender ID="txtPhone_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                MaskType="Number" TargetControlID="txtPhone">
                                            </asp:MaskedEditExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Fax
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtFax" CssClass="form-control" TabIndex="13" runat="server" MaxLength="28"/>
                                                <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                MaskType="Number" TargetControlID="txtFax">
                                            </asp:MaskedEditExtender>
                                        </div>
                                    </div>
                                        <div class="form-col">
                                        <div class="fc-label">
                                            Email
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtEmailid" TextMode="Email" CssClass="form-control" TabIndex="9" runat="server" MaxLength="50"/>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                ControlToValidate="txtEmailid" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True"
                                                TargetControlID="revEmail">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Cellular
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCellular" CssClass="form-control" TabIndex="15" runat="server" MaxLength="28"/>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Web address
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtWebsite" CssClass="form-control" TabIndex="10" runat="server" MaxLength="50"/>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <%-- <b>Control</b>--%>
                                                <h2 class="form-title">Control</h2>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Cellular ShipVia
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtShipvia" CssClass="form-control" TabIndex="16" runat="server" MaxLength="50"/>
                                        </div>
                                    </div>
                                        <div class="form-col">
                                        <div class="fc-label">
                                            Default Acct
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtDefaultAcct" CssClass="form-control" TabIndex="16" runat="server" MaxLength="75"
                                                Placeholder="Search by acct# and name"/>
                                            <asp:HiddenField ID="hdnAcctID" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Balance
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtBalance" CssClass="form-control" TabIndex="20" Text="0.0" runat="server" ReadOnly="True" BackColor="#DBDBDB" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Status
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlStatus" CssClass="form-control" TabIndex="18" runat="server">
                                                <asp:ListItem Text="Select Status" />
                                                <asp:ListItem Text="Active" Value="0" />
                                                <asp:ListItem Text="Inactive" Value="1" />
                                                <asp:ListItem Text="Hold" Value="2" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Terms
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlTerms" runat="server" CssClass="form-control" TabIndex="22">
                                            </asp:DropDownList>
                                        <%--    <asp:ListItem Text="Select Terms" />
                                                <asp:ListItem Text="Net 30 Days" Value="1" />
                                                <asp:ListItem Text="Net 60 Days" Value="2" />
                                                <asp:ListItem Text="%Disc/Net 30 Days" Value="3" />
                                                <asp:ListItem Text="Net 90 Days" Value="4" />
                                                <asp:ListItem Text="Net 180 Days" Value="5" />
                                                <asp:ListItem Text="Net 90 Days" Value="6" />
                                                <asp:ListItem Text="Net Due On 10th" Value="7" />
                                                <asp:ListItem Text="Net 120 Days" Value="8" />
                                                <asp:ListItem Text="Net Due" Value="9" />--%>
                               
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                            </div>
                                            <div class="fc-input">
                                            </div>
                                        </div>
                                    </div>


                                    <table>

                                        <tr>
                                            <td class="sub1">
                                                <asp:Label ID="lblSince" runat="server" Text="Since" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSince" CssClass="form-control" TabIndex="23" runat="server" />
                                            </td>

                                            <td class="sub2">
                                                <asp:Label ID="lblLast" runat="server" Text="Last" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLast" CssClass="form-control" TabIndex="24" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sub2">
                                                <asp:Label ID="lbl1099bx" Text="1099 Box" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt1099" Visible="false" CssClass="form-control" Text="1" TabIndex="25" runat="server" />
                                            </td>
                                            <td class="sub2">
                                                <asp:Label ID="lblGelock" Text="GeoLock" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGeolock" CssClass="form-control" Text="0" TabIndex="26" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="sub2">
                                                <asp:Label ID="lblInuse" Text="InUse" runat="server" Visible="false"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtInuse" CssClass="form-control" Text="1" TabIndex="26" runat="server" />
                                            </td>

                                        </tr>
                                    </table>

                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tbTransaction" runat="server" HeaderText="Transactions">
                         <HeaderTemplate>
                             Transactions
                         </HeaderTemplate>
                         <ContentTemplate>
                             <div class="col-lg-12 col-md-12">
                                <div class="table-scrollable" style="padding-top: 15px; border: none">
                                    <asp:Label ID="lblNoRecord" runat="server" ClientIDMode="Static" Text="No records avaliable." CssClass="hide"></asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ClientIDMode="Static">
                                        <ContentTemplate>
                                            <asp:DataList ID="gvTrans" runat="server" CssClass="table table-bordered table-striped table-condensed flip-content" ClientIDMode="Static"
                                                OnItemDataBound="gvTrans_ItemDataBound">
                                           <%-- <ItemStyle CssClass="evenrowcolor" />
                                                <FooterStyle CssClass="footer" />
                                                <AlternatingItemStyle CssClass="oddrowcolor" />
                                                <SelectedItemStyle CssClass="selectedrowcolor" />
                                                <HeaderStyle CssClass="header" />--%>
                                                <HeaderTemplate>
                                                    <th scope="col" style="width: 1%;"><a>No.</a></th>
                                                    <th scope="col" style="width: 8%;"><a>Date</a></th>
                                                    <th scope="col" style="width: 10%;"><a>Ref</a></th>
                                                    <th scope="col" style="width: 45%;"><a>Description</a></th>
                                                    <th scope="col" style="width: 20%;"><a>Vendor</a></th>
                                                    <th scope="col" style="width: 5%;"><a>Status</a></th>
                                                    <th scope="col" style="width: 8%;"><a>Amount</a></th>
                                                    <th scope="col" style="width: 5%;"><a>Type</a></th>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <td>
                                                        <asp:LinkButton ID="lnkId" Text='<%# Container.ItemIndex + 1 %>' runat="server" Enabled="false"></asp:LinkButton>
                                                        <asp:Label ID="lblId" runat="server" Text='<%#Eval("ID")%>' style="display:none;"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("fDate")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))):""%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblRef" runat="server" Text='<%#Eval("Ref")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblfDesc" runat="server" Text='<%# Eval("fDesc")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("VendorName")%>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("StatusName")%>'></asp:Label>
                                                    </td>
                                                    <td style="text-align:right;">
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'
                                                            ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                                    </td>
                                                </ItemTemplate>
                                                <%--<FooterTemplate>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td></td>
                                                    <td>
                                                        <asp:Label ID="lblFooterAmount" runat="server"></asp:Label>
                                                    </td>
                                                    <td></td>
                                                </FooterTemplate>--%>
                                            </asp:DataList>
                                            <%-- <table id="Pager" cssclass="table table-bordered table-striped table-condensed flip-content" style="width: 100%;">

                                                <tr class="evenrowcolor">
                                                    <td colspan="7">
                                                        <div align="center">
                                                            <a class="pagefirst">
                                                                <img src="images/First.png" style="vertical-align: top !important;" />
                                                            </a>

                                                            &nbsp; &nbsp;
                                                             <a class="pageprev">
                                                                 <img src="images/Backward.png" style="vertical-align: top !important;" />
                                                             </a>
                                                            &nbsp; &nbsp; <span>Page</span>

                                                            <asp:DropDownList name="ddlPages" ID="ddlPages" runat="server" ClientIDMode="Static">
                                                            </asp:DropDownList>
                                                            <span>of </span>
                                                            <span id="lblPageCount"></span>
                                                            &nbsp; &nbsp;

                                                            <a class="pagenext">
                                                                <img src="images/Forward.png" style="vertical-align: top !important;" />
                                                            </a>
                                                            &nbsp; &nbsp;

                                                            <a class="pagelast">
                                                                <img src="images/last.png" style="vertical-align: top !important;" />
                                                            </a>
                                                            
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>--%>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                             </div>
                         </ContentTemplate>
                     </asp:TabPanel>
                </asp:TabContainer>
                    <div class="clearfix"></div>
                </div>
                <!-- edit-tab end -->
                <div class="clearfix"></div>
            </div>
        <!-- edit-tab start -->
      
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
        </div>
    </div>
</asp:Content>

