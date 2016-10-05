<%@ Page Title="" Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true"
    CodeFile="AddOpprt.aspx.cs" Inherits="AddOpprt" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_CustomerSearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            setInterval(serviceCall, 10000);
            AutoCompleteText('CustomerAuto.asmx/getTaskContacts', '<%= txtName.ClientID %>', '<%= hdnId.ClientID %>', '<%= btnFillTasks.ClientID %>', '<%= lblType.ClientID %>', '<%= hdnOwnerID.ClientID %>')

            $("#<%= txtName.ClientID %>").keyup(function (e) {
                var hdnId = document.getElementById('<%= hdnId.ClientID %>');
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnId.value = '';
                }
                if (e.value == '') {
                    hdnId.value = '';
                }
            });

            $("#<%=txtAmount.ClientID%>").blur(function () {
                $(this).formatCurrency();
            });

            if ($(window).width() > 767) {
                $('#<%=txtRemarks.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtRemarks.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtDesc.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtDesc.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });
            }

        });

        function ChkLoc(sender, args) {
            var hdnId = document.getElementById('<%=hdnId.ClientID%>');
            if (hdnId.value == '') {
                args.IsValid = false;
            }
        }

        function CheckFollowup(id, checked) {

            var r = confirm('Would you like to create a follow-up task at this time?');
            if (r == true) {
                window.setTimeout(function () { window.location.href = "addtask.aspx?fl=2&uid=" + id }, 2000);
            }

        }

        function serviceCall() {
            var rol = document.getElementById('<%= hdnId.ClientID %>');
            var type = -1;
            var UID = 0;
            if (document.getElementById('<%= hdnMailType.ClientID %>').value != '')
                type = document.getElementById('<%= hdnMailType.ClientID %>').value;
            if (document.getElementById('<%= hdnUID.ClientID %>').value != '')
                UID = document.getElementById('<%= hdnUID.ClientID %>').value;

            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'AddProspect.aspx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"' + type + '","uid":"' + UID + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //                    alert(msg.d);
                        MailCount(msg.d)
                    },
                    error: function (e) {
                        //                        alert(jQuery.parseJSON(e));
                    }
                });
            }
        }

        function MailCount(d) {

            var newmail = 0;
            var hdnct = document.getElementById('<%= hdnMailct.ClientID %>').value;

            if (hdnct != '') {
                newmail = hdnct;
            }

            //            alert(newmail + ' -- ' + d);
            if (newmail != d) {
                document.getElementById('dvmailct').innerHTML = d - newmail + ' New Email(s)';
                $("#maillink").show();
            }
            else {
                $("#maillink").hide();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-cont-top">
        <%--<ul class="page-breadcrumb">
            <li>
                <i class="fa fa-home"></i>
                <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="#">Sales Manager</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="<%=ResolveUrl("~/opportunity.aspx") %>">Opportunities</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <span>Add Opportunities</span>
            </li>
        </ul>--%>
        <div class="page-bar-right">
            
           
        </div>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
             <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header">
                            <li>
                                 <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Opportunity</asp:Label></li>
                            <li>
                                <asp:CheckBox ID="chkClosed" Style="float: right" runat="server" Text="Closed" /></li>
                            <li>
                                 <asp:LinkButton ID="lnkSave" runat="server" CssClass="icon-save" ToolTip="Save"  OnClick="lnkSave_Click"
                                    TabIndex="11"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" CssClass="icon-closed" ToolTip="Close"
                                TabIndex="12" OnClick="lnkClose_Click"></asp:LinkButton>
                            </li>
                            </ul>
                 </asp:Panel>
           
        </div>
        <div class="ae-content">
            <asp:HiddenField ID="hdnUID" runat="server" />
            <asp:HiddenField ID="hdnId" runat="server" />
            <asp:HiddenField ID="hdnOwnerID" runat="server" />
            <%--<asp:HiddenField ID="hdnCustID" runat="server" />--%>
            <a id="maillink" href="#Div6" style="display: none; position: fixed; top: 0; left: 615px;">
                <div id="dvmailct" style="width: 100%; height: 100%; vertical-align: middle; text-align: center; padding: 5px; background-color: Black"
                    class="transparent roundCorner shadow">
                </div>
            </a>
            <asp:Button ID="btnFillTasks" runat="server" Text="Button" CausesValidation="false"
                Style="display: none" OnClick="btnFillTasks_Click" />
            <asp:Panel runat="server" ID="Popup" Width="100%">
                <div class="col-lg-12">
                    <div class="row">
                        <asp:Panel ID="pnlProspects" runat="server">
                            <asp:Menu ID="menuLeads" runat="server" Orientation="Horizontal" CssClass="menu">
                                <StaticMenuItemStyle ItemSpacing="20px" />
                                <Items>
                                    <asp:MenuItem Text="Additional Info" NavigateUrl="#add" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="Open Tasks(0)" NavigateUrl="#Div1" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="Task History(0)" NavigateUrl="#Div2" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="Contacts(0)" NavigateUrl="#Div3" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="Emails(0)" NavigateUrl="#Div6" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="Notes and attachments(0)" NavigateUrl="#Div4" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                    <asp:MenuItem Text="System Info" NavigateUrl="#Div5"></asp:MenuItem>
                                </Items>
                            </asp:Menu>
                            <div class="clearfix"></div>
                            <asp:Panel ID="pnlCustomer" runat="server" Visible="false" Width="800px">
                                <table width="100%" style="border: solid 1px red" class="roundCorner shadow">
                                    <tr>
                                        <td colspan="2">
                                            <div style="text-align: center">
                                                You are about to close this Opportunity and create a new Location. Please select
                                                an existing customer or leave the field blank and this will create a new Customer.
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <uc1:uc_CustomerSearch ID="uc_CustomerSearch1" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;
                                        </td>
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <div style="padding-top: 20px">
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="Image1"
                                    CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image1"
                                    ExpandedImage="~/images/arrow_down.png" ImageControlID="Image1" SuppressPostBack="True"
                                    TargetControlID="Panel1">
                                </asp:CollapsiblePanelExtender>
                                <div id="tasks" runat="server" style="font-weight: bold; font-size: 15px">
                                    Opportunity Information:
                                <asp:Image ID="Image1" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    <asp:TextBox ID="lblType" runat="server" CssClass="texttransparent" onfocus="this.blur();"
                                        Style="color: Gray; font-style: italic; width: 60px">
                                    </asp:TextBox>
                                </div>
                                <asp:Panel ID="Panel1" runat="server">
                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Contact<asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtName"
                                                    Display="None" ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator40_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator40">
                                                    </asp:ValidatorCalloutExtender>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkLoc"
                                                    ControlToValidate="txtName" Display="None" ErrorMessage="Please select the Contact from list"
                                                    SetFocusOnError="True"></asp:CustomValidator>
                                                <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                    PopupPosition="TopLeft" TargetControlID="CustomValidator1">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Opportunity Name<asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server"
                                                    ControlToValidate="txtOppName" Display="None" ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator41_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator41">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtOppName" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Close Date<asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server"
                                                    ControlToValidate="txtCloseDate" Display="None" ErrorMessage="Date Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator42_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator42">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCloseDate" runat="server" CssClass="form-control" TabIndex="3"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtCloseDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtCloseDate">
                                                </asp:CalendarExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Status
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="5">
                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                    <asp:ListItem Value="2">Hold</asp:ListItem>
                                                    <asp:ListItem Value="3">Sold</asp:ListItem>
                                                    <asp:ListItem Value="4">Quoted</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Stage
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlStage" runat="server" CssClass="form-control" TabIndex="6" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged">                                                    
                                                </asp:DropDownList>                                                
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Assigned<asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server"
                                                    ControlToValidate="ddlAssigned" Enabled="false" Display="None" ErrorMessage="Owner Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator44_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator44">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlAssigned" runat="server" CssClass="form-control" TabIndex="5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Probability
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlProbab" runat="server" CssClass="form-control" TabIndex="4">
                                                    <asp:ListItem Value="0">Excellent</asp:ListItem>
                                                    <asp:ListItem Value="1">Very Good</asp:ListItem>
                                                    <asp:ListItem Value="2">Good</asp:ListItem>
                                                    <asp:ListItem Value="3">Average</asp:ListItem>
                                                    <asp:ListItem Value="4">Poor</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Amount<asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtAmount"
                                                    Display="None" ErrorMessage="Amount Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator43_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator43">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TabIndex="6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-8 col-lg-8">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Remarks
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtRemarks" runat="server" Rows="3" CssClass="form-control"
                                                    TabIndex="7" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" CollapseControlID="Image3"
                                    CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image3"
                                    ExpandedImage="~/images/arrow_down.png" ImageControlID="Image3" SuppressPostBack="True"
                                    TargetControlID="Panel3">
                                </asp:CollapsiblePanelExtender>
                                <div id="add" style="font-weight: bold; font-size: 15px">
                                    Additional Information:
                                <asp:Image ID="Image3" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                </div>
                                <asp:Panel ID="Panel3" runat="server">
                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Next Step
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtNextStep" runat="server" CssClass="form-control"
                                                    TabIndex="8"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Lead Source
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlSource" runat="server" CssClass="form-control" TabIndex="9">
                                                    <asp:ListItem Value="">None</asp:ListItem>
                                                    <asp:ListItem Value="Advertisement">Advertisement</asp:ListItem>
                                                    <asp:ListItem Value="Customer Referral">Customer Referral</asp:ListItem>
                                                    <asp:ListItem Value="Employee Referral">Employee Referral</asp:ListItem>
                                                    <asp:ListItem Value="External Referral">External Referral</asp:ListItem>
                                                    <asp:ListItem Value="Partner">Partner</asp:ListItem>
                                                    <asp:ListItem Value="Public Relations">Public Relations</asp:ListItem>
                                                    <asp:ListItem Value="Seminar - Internal">Seminar - Internal</asp:ListItem>
                                                    <asp:ListItem Value="Seminar - Partner">Seminar - Partner</asp:ListItem>
                                                    <asp:ListItem Value="Trade Show">Trade Show</asp:ListItem>
                                                    <asp:ListItem Value="Web">Web</asp:ListItem>
                                                    <asp:ListItem Value="Word of mouth">Word of mouth</asp:ListItem>
                                                    <asp:ListItem Value="Other">Other</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-8 col-lg-8">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Desc
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtDesc" runat="server" Rows="3" CssClass="form-control"
                                                    TextMode="MultiLine" TabIndex="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel2"
                                    ExpandControlID="Image2" CollapseControlID="Image2" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                    CollapsedImage="~/images/arrow_right.png" ImageControlID="Image2" Enabled="True">
                                </asp:CollapsiblePanelExtender>
                                <div id="Div1" style="font-weight: bold; font-size: 15px">
                                    Open Tasks:
                                <asp:Image ID="Image2" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    <asp:Panel ID="Panel2" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">

                                        <div class="table-scrollable" style="border:none">
                                            <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                                <ul class="lnklist-header lnklist-panel">
                                                    <li style="margin-left:3px">
                                                 <asp:HyperLink ID="HyperLink2" runat="server" CssClass="icon-addnew" ToolTip="Add New" NavigateUrl="~/AddTask.aspx"
                                                    Target="_blank"></asp:HyperLink>
                                                        </li>
                                                    </ul>
                                            </div>
                                            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                Width="100%" PageSize="20" EmptyDataText="No records to display">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <FooterStyle CssClass="footer" />
                                                <RowStyle CssClass="evenrowcolor" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") %>'
                                                                Target="_blank" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Due Date/Date Done" SortExpression="duedate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                                Text='<%# Eval("duedate") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="# Days" SortExpression="days">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Desc" SortExpression="Remarks" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Resolution" SortExpression="result" ItemStyle-Width="200px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblResult" runat="server" Text='<%# Eval("result") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="200px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Assigned to" SortExpression="fUser">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblfUser" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                            title="Go to top">
                                            <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div>

                                    </asp:Panel>
                                </div>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Panel4"
                                    ExpandControlID="Image4" CollapseControlID="Image4" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                    CollapsedImage="~/images/arrow_right.png" ImageControlID="Image4" Enabled="True">
                                </asp:CollapsiblePanelExtender>
                                <div id="Div2" style="font-weight: bold; font-size: 15px">
                                    Task History:
                                <asp:Image ID="Image4" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                </div>
                                <asp:Panel ID="Panel4" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">
                                    <div class="table-scrollable" style="border:none">
                                        <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li style="margin-left:3px">
                                             <asp:HyperLink ID="HyperLink1" runat="server" CssClass="icon-addnew" ToolTip="Add New" NavigateUrl="~/AddTask.aspx"
                                                Target="_blank"></asp:HyperLink>
                                                </li>
                                                </ul>
                                        </div>
                                        <asp:GridView ID="gvTasksCompleted" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" PageSize="20" EmptyDataText="No records to display">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") %>'
                                                            Target="_blank" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date/Date Done" SortExpression="duedate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                            Text='<%# Eval("duedate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="# Days" SortExpression="days">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" SortExpression="Remarks" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resolution" SortExpression="result" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResult" runat="server" Text='<%# Eval("result") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned to" SortExpression="fUser">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfUser" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                    <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                        title="Go to top">
                                        <img id="img1" alt="top" src="images/uptotop.gif" />
                                        Go To Top
                                    </div>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="Panel6"
                                    ExpandControlID="Image6" CollapseControlID="Image6" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                    CollapsedImage="~/images/arrow_right.png" ImageControlID="Image6" Enabled="True">
                                </asp:CollapsiblePanelExtender>
                                <div id="Div3" style="font-weight: bold; font-size: 15px">
                                    Contacts:
                                <asp:Image ID="Image6" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                </div>
                                <asp:Panel ID="Panel6" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">
                                    <div class="table-scrollable" style="border:none">
                                        <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" EmptyDataText="No records to display">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName0" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Phone" SortExpression="Phone">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPhn" runat="server"><%#Eval("Phone")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fax" SortExpression="Fax">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFx" runat="server"><%#Eval("Fax")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cell" SortExpression="Cell">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCell" runat="server"><%#Eval("Cell")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email" SortExpression="Email">
                                                    <ItemTemplate>
                                                        <a href='<%# Request.QueryString["uid"] != null ? "email.aspx?to=" + Eval("Email") + "&rol="+ hdnId.Value + "&op=" + Request.QueryString["uid"].ToString() : "email.aspx?to=" + Eval("Email")+ "&rol="+ hdnId.Value %>'
                                                            target="_blank">
                                                            <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label></a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                        </asp:GridView>
                                    </div>
                                    <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                        title="Go to top">
                                        <img id="img2" alt="top" src="images/uptotop.gif" />
                                        Go To Top
                                    </div>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:Panel ID="pnlEmail" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="Panel8"
                                        ExpandControlID="Image8" CollapseControlID="Image8" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image8" Enabled="True" />
                                    <div id="Div6" style="font-weight: bold; font-size: 15px">
                                        Emails:
                                    <asp:Image ID="Image8" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <div>
                                        <asp:Panel ID="Panel8" runat="server" Style="width: 100%; padding: 10px 10px 10px 10px;">
                                            <div class="table-scrollable" style="height: auto !important"">
                                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="Panel9" runat="server" class="salesgriddiv" Style="background: #316b9d; width: 100%;">
                                                            <ul class="lnklist-header lnklist-panel">
                                                                <li> <asp:LinkButton ID="lnkAllMail" runat="server" CausesValidation="False" OnClick="lnkAllMail_Click">Show All</asp:LinkButton></li>
                                                                <li> <asp:LinkButton ID="lnkSpecific" runat="server" CausesValidation="False" OnClick="lnkSpecific_Click">Specific</asp:LinkButton></li>
                                                                <li> <asp:HyperLink ID="lnkNewEmail" Target="_blank" runat="server" CssClass="icon-mail" ToolTip="New email" NavigateUrl="email.aspx"></asp:HyperLink></li>
                                                                <li> <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" CssClass="icon-refresh1" ToolTip="Refresh" OnClick="lnkRefreshMails_Click"></asp:LinkButton></li>
                                                                <li><asp:HiddenField ID="hdnMailct" runat="server" /></li>
                                                                <li> <asp:HiddenField ID="hdnMailType" runat="server" /></li>
                                                            </ul>
                                                        </asp:Panel>
                                                        <asp:GridView ID="gvmail" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                            Width="100%" EmptyDataText="No records to display" PageSize="10" AllowPaging="True"
                                                            AllowSorting="True" OnDataBound="gvmail_DataBound" OnSorting="gvmail_Sorting"
                                                            OnRowCommand="gvmail_RowCommand">
                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                            <FooterStyle CssClass="footer" />
                                                            <RowStyle CssClass="evenrowcolor" />
                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:Image runat="server" ID="imgType" Width="11px" ImageUrl='<%# Eval("type").ToString() != "0" ? "images/uparr.png" : "images/downarr.png"%>' />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Subject" SortExpression="subject" ItemStyle-Width="250px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lnkMsgID" Visible="false" runat="server" Text='<%# Eval("guid") %>'></asp:Label>
                                                                        <asp:HyperLink ID="lnkSub" NavigateUrl='<%# Request.QueryString["uid"] != null ? "email.aspx?aid=" + Eval("guid") + "&op=" + Request.QueryString["uid"].ToString() +"&rol="+ hdnId.Value :  "email.aspx?aid=" + Eval("guid") +"&rol="+ hdnId.Value %>'
                                                                            Target="_blank" runat="server" Text='<%# Eval("subject") %>'></asp:HyperLink>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="From" SortExpression="[from]" ItemStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lnkFrom" runat="server" Text='<%# Eval("from") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="To" ItemStyle-Width="150px">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lnkTo" runat="server" Text='<%# Eval("to") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Date Sent" ItemStyle-Width="130px" SortExpression="SentDate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSentdate" runat="server" Text='<%# Eval("SentDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Rec. Date" ItemStyle-Width="130px" SortExpression="recDate">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRecdate" runat="server" Text='<%# Eval("recDate") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <PagerTemplate>
                                                                <div align="center">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png"
                                                                        CausesValidation="false" />
                                                                    &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                                        CausesValidation="false" ImageUrl="~/images/Backward.png" />
                                                                    &nbsp &nbsp <span>Page</span>
                                                                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                    <span>of </span>
                                                                    <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                    &nbsp &nbsp
                                                        <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png"
                                                            CausesValidation="false" />
                                                                    &nbsp &nbsp
                                                        <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" ImageUrl="images/last.png"
                                                            CausesValidation="false" />
                                                                </div>
                                                            </PagerTemplate>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                title="Go to top">
                                                <img id="img4" alt="top" src="images/uptotop.gif" />
                                                Go To Top
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:Panel ID="pnlNotes" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" CollapseControlID="Image5"
                                        CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image5"
                                        ExpandedImage="~/images/arrow_down.png" ImageControlID="Image5" SuppressPostBack="True"
                                        TargetControlID="Panel5">
                                    </asp:CollapsiblePanelExtender>
                                    <div id="Div4" style="font-weight: bold; font-size: 15px">
                                        Notes &amp; Attachments:
                                    <asp:Image ID="Image5" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>

                                    <asp:Panel ID="Panel5" runat="server" Style="width: 100%; padding: 10px 10px 10px 10px;">
                                        <div style="height: auto !important">
                                        <div class="table-scrollable" style="border:none">
                                            <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                Width="100%">
                                                <RowStyle CssClass="evenrowcolor" />
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Subject">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSub" runat="server" Text='<%# Eval("subject") %>'></asp:Label>
                                                            <asp:Label ID="lblBody" runat="server" Text='<%# Eval("body") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Name">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="File Type">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                            </asp:GridView>
                                        </div>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                            title="Go to top">
                                            <img id="img3" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div></div>
                                    </asp:Panel>

                                </asp:Panel>
                            </div>
                            <div style="padding-top: 10px">
                                <asp:Panel ID="pnlSysInfo" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender9" runat="server" TargetControlID="Panel7"
                                        ExpandControlID="Image7" CollapseControlID="Image7" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image7" Enabled="True">
                                    </asp:CollapsiblePanelExtender>
                                    <div id="Div5" style="font-weight: bold; font-size: 15px">
                                        <span>System Info</span>:
                                    <asp:Image ID="Image7" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel7" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%"
                                        CssClass="roundCorner">
                                        <table>
                                            <tr>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="300px">&nbsp;
                                                </td>
                                                <td width="100px">&nbsp;
                                                </td>
                                                <td width="300px">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="70px">Created By
                                                </td>
                                                <td width="300px">
                                                    <asp:Label ID="lblCreate" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td width="100px">Last Updated By
                                                </td>
                                                <td width="300px">
                                                    <asp:Label ID="lblUpdate" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="300px">&nbsp;
                                                </td>
                                                <td width="100px">&nbsp;
                                                </td>
                                                <td width="300px">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
