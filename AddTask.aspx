<%@ Page Title="" Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true"
    CodeFile="AddTask.aspx.cs" Inherits="AddTask" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        $(document).ready(function () {
            setInterval(serviceCall, 10000);
            AutoCompleteText('CustomerAuto.asmx/getTaskContacts', '<%= txtName.ClientID %>', '<%= hdnId.ClientID %>', '<%= btnFillTasks.ClientID %>', '<%= lblType.ClientID %>', null)

            $("#<%= txtName.ClientID %>").keyup(function (e) {
                var hdnId = document.getElementById('<%= hdnId.ClientID %>');
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnId.value = '';
                }
                if (e.value == '') {
                    hdnId.value = '';
                }
            });

            $(document).ready(function () {
                if ($(window).width() > 767) {
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
                    $('#<%=txtResol.ClientID%>').focus(function () {
                        $(this).animate({
                            //right: "+=0",
                            width: '520px',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });

                    $('#<%=txtResol.ClientID%>').blur(function () {
                        $(this).animate({
                            width: '100%',
                            height: '75px'
                        }, 500, function () {
                            // Animation complete.
                        });
                    });
                }
            });

        });

        function ChkCustomer(sender, args) {
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
            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'AddProspect.aspx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"-1","uid":"0"}',
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
        <%--  <ul class="page-breadcrumb">
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
                <a href="<%=ResolveUrl("~/Tasks.aspx") %>">Tasks</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <span>Add Tasks</span>
            </li>
        </ul>--%>
        <div class="page-bar-right">
        </div>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
            <ul class="lnklist-header">
                <li>
                    <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Tasks</asp:Label></li>
                <li>
                    <asp:CheckBox ID="chkFollowUp" runat="server" Text="Follow-Up" Visible="false" CssClass="save_button" /></li>
                <li>
                    <asp:LinkButton ID="lnkSave" runat="server" CssClass="icon-save" ToolTip="Save" ForeColor="Green" OnClick="lnkSave_Click"
                        TabIndex="11"></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lnkClose" runat="server" ForeColor="Red" ToolTip="Close" CausesValidation="False" CssClass="icon-closed"
                        TabIndex="12" OnClick="lnkClose_Click"></asp:LinkButton>

                </li>
            </ul>
            <div style="float: right"></div>
        </div>
        <div class="ae-content">
            <asp:HiddenField ID="hdnId" runat="server" />
            <a id="maillink" href="#Div5" style="display: none; position: fixed; top: 0; left: 615px;">
                <div id="dvmailct" style="width: 100%; height: 100%; vertical-align: middle; text-align: center; padding: 5px; background-color: Black"
                    class="transparent roundCorner shadow">
                </div>
            </a>
            <asp:Button ID="btnFillTasks" runat="server" Text="Button" CausesValidation="false"
                Style="display: none" OnClick="btnFillTasks_Click" />
            <asp:Panel runat="server" ID="Popup" Width="100%">
                <asp:Panel ID="pnlProspects" runat="server">
                    <asp:Menu ID="menuLeads" runat="server" Orientation="Horizontal" CssClass="menu">
                        <StaticMenuItemStyle ItemSpacing="20px" />
                        <Items>
                            <asp:MenuItem Text="Open Tasks(0)" NavigateUrl="#Div1" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                            <asp:MenuItem Text="Task History(0)" NavigateUrl="#hist" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                            <asp:MenuItem Text="Contacts(0)" NavigateUrl="#Div3" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                            <asp:MenuItem Text="Emails(0)" NavigateUrl="#Div5" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                            <asp:MenuItem Text="System Info" NavigateUrl="#sys"></asp:MenuItem>
                        </Items>
                    </asp:Menu>
                    <div class="clearfix"></div>
                    <div style="padding-top: 10px">
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" CollapseControlID="Image1"
                            CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image1"
                            ExpandedImage="~/images/arrow_down.png" ImageControlID="Image1" SuppressPostBack="True"
                            TargetControlID="Panel1">
                        </asp:CollapsiblePanelExtender>
                        <div id="tasks" runat="server" style="font-weight: bold; font-size: 15px">
                            Task Information:
                                <asp:Image ID="Image1" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                            <asp:TextBox ID="lblType" runat="server" CssClass="texttransparent" onfocus="this.blur();"
                                Style="color: Gray; font-style: italic; width: 60px">
                            </asp:TextBox>
                        </div>
                        <asp:Panel ID="Panel1" runat="server" Style="padding-top: 10px">
                            <div class="col-md-6 col-lg-6">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Contact<asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtName"
                                            Display="None" ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                ID="RequiredFieldValidator40_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                TargetControlID="RequiredFieldValidator40">
                                            </asp:ValidatorCalloutExtender>
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkCustomer"
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
                                        Subject<asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtSubject"
                                            Display="None" ErrorMessage="Subject Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                ID="RequiredFieldValidator41_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                TargetControlID="RequiredFieldValidator41">
                                            </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-md-6 col-lg-6">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Due Date<asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server"
                                            ControlToValidate="txtCallDt" Display="None" ErrorMessage="Date Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                ID="RequiredFieldValidator42_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                TargetControlID="RequiredFieldValidator42">
                                            </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input merchant-input">
                                        <asp:TextBox ID="txtCallDt" runat="server" CssClass="form-control" TabIndex="3" Width="110px"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtCallDt_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtCallDt">
                                        </asp:CalendarExtender>
                                        <asp:TextBox ID="txtCallTime" runat="server" CssClass="form-control" TabIndex="4" Width="83px"></asp:TextBox>
                                        <asp:MaskedEditExtender ID="txtCallTime_MaskedEditExtender" runat="server" AcceptAMPM="True"
                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                            CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                            CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtCallTime">
                                        </asp:MaskedEditExtender>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-lg-6">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Assigned to<asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server"
                                            ControlToValidate="ddlAssigned" Display="None" ErrorMessage="User Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                ID="RequiredFieldValidator43_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                TargetControlID="RequiredFieldValidator43">
                                            </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlAssigned" runat="server" CssClass="form-control" TabIndex="5"
                                            Width="200px">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8 col-lg-8">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Description
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" Rows="3"
                                            TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6 col-lg-6">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Status
                                    </div>
                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" CssClass="form-control"
                                            OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" TabIndex="7">
                                            <asp:ListItem Value="0">Open</asp:ListItem>
                                            <asp:ListItem Value="1">Completed</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8 col-lg-8">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Resolution
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtResol" runat="server" CssClass="form-control" Rows="3"
                                            TabIndex="8" TextMode="MultiLine" Enabled="False"></asp:TextBox>
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
                        </div>
                        <asp:Panel ID="Panel2" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">
                            <div class="table-scrollable">
                                <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                    <ul class="lnklist-header lnklist-panel">
                                        <li>
                                            <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/AddTask.aspx" ToolTip="Add New" CssClass="icon-addnew" Target="_blank"></asp:HyperLink>
                                        </li>
                                    </ul>
                                </div>
                                <asp:GridView ID="gvTasksOpen" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
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
                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                title="Go to top">
                                <img id="img1" alt="top" src="images/uptotop.gif" />
                                Go To Top
                            </div>
                        </asp:Panel>
                    </div>
                    <div style="padding-top: 10px">
                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" CollapseControlID="Image3"
                            CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image3"
                            ExpandedImage="~/images/arrow_down.png" ImageControlID="Image3" SuppressPostBack="True"
                            TargetControlID="Panel3">
                        </asp:CollapsiblePanelExtender>
                        <div id="hist" style="font-weight: bold; font-size: 15px">
                            Tasks History:
                                <asp:Image ID="Image3" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                        </div>
                        <asp:Panel ID="Panel3" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">
                            <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                <ul class="lnklist-header lnklist-panel">
                                    <li>
                                        <asp:HyperLink ID="HyperLink1" runat="server" ToolTip="Add New" CssClass="icon-addnew" NavigateUrl="~/AddTask.aspx"
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
                                            <asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject") %>'></asp:Label>
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
                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                title="Go to top">
                                <img id="imgtop" alt="top" src="images/uptotop.gif" />
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
                                            <a href='<%# "email.aspx?to=" + Eval("Email") +"&rol="+hdnId.Value %>' target="_blank">
                                                <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <RowStyle CssClass="evenrowcolor" />
                                <SelectedRowStyle CssClass="selectedrowcolor" />
                            </asp:GridView>
                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                title="Go to top">
                                <img id="img2" alt="top" src="images/uptotop.gif" />
                                Go To Top
                            </div>
                        </asp:Panel>
                    </div>
                    <div style="padding-top: 10px">
                        <asp:Panel ID="pnlEmail" runat="server">
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="Panel5"
                                ExpandControlID="Image5" CollapseControlID="Image5" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                CollapsedImage="~/images/arrow_right.png" ImageControlID="Image5" Enabled="True" />
                            <div id="Div5" style="font-weight: bold; font-size: 15px">
                                Emails:
                                    <asp:Image ID="Image5" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                            </div>
                            <asp:Panel ID="Panel5" runat="server" Style="padding: 10px 10px 10px 10px; width: 100%">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">

                                    <ContentTemplate>
                                        <asp:Panel ID="Panel8" runat="server" class="salesgriddiv" Style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:HyperLink ID="lnkNewEmail" Target="_blank" runat="server" CssClass="icon-addnew" ToolTip="New Email"></asp:HyperLink></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" ToolTip="Refresh" CssClass="icon-refresh1" OnClick="lnkRefreshMails_Click"></asp:LinkButton></li>
                                                <li>
                                                    <asp:HiddenField ID="hdnMailct" runat="server" />
                                                </li>
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
                                                        <asp:HyperLink ID="lnkSub" NavigateUrl='<%# "email.aspx?aid=" + Eval("guid")  +"&rol="+hdnId.Value  %>'
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
                                <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                    title="Go to top">
                                    <img id="img4" alt="top" src="images/uptotop.gif" />
                                    Go To Top
                                </div>
                            </asp:Panel>
                        </asp:Panel>
                    </div>
                    <div style="padding-top: 10px">
                        <asp:Panel ID="pnlSysInfo" runat="server">
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender9" runat="server" CollapseControlID="Image7"
                                CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="Image7"
                                ExpandedImage="~/images/arrow_down.png" ImageControlID="Image7" SuppressPostBack="True"
                                TargetControlID="Panel7">
                            </asp:CollapsiblePanelExtender>
                            <div id="sys" style="font-weight: bold; font-size: 15px">
                                <span>System Info</span>:
                                    <asp:Image ID="Image7" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                            </div>
                            <asp:Panel ID="Panel7" runat="server" CssClass="roundCorner" Style="padding: 10px 10px 10px 10px;">
                                <table>
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
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </div>

                </asp:Panel>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
