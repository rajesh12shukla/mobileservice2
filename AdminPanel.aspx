<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AdminPanel.aspx.cs" Inherits="AdminPanel" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function AlertPing(control) {
            var ddlGPSPing = document.getElementById(control.id);

            if (ddlGPSPing.value == 0) {
                alert('Setting the ping interval to zero will cause significant rise in the phone battery consumption. Recommended ping interval is 1 minute.');
            }
        }
    </script>
    <%--   <script type="text/javascript">
        function hideModalPopup() {
            jQuery("#setuppopup").modal("hide");
            window.location.reload();
        }
    </script>--%>
    <style>
        #programmaticModalPopupBehavior_backgroundElement {
            background: rgba(0,0,0,0.5);
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <%-- <ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="/Home.aspx">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Database Maintenance</a>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="Label3" runat="server">Database Maintenance</asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <ul class="lnklist-header">
                                    <li style="margin-right: 0">
                                        <asp:LinkButton CssClass="icon-changepassword" ID="LinkButton2" runat="server" ToolTip="Change Password" CausesValidation="false"
                                            OnClick="LinkButton1_Click" TabIndex="39" OnClientClick="$('#setuppopup').modal('show')"></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton CssClass="icon-gpssetting" ID="LinkButton3" runat="server" ForeColor="Green" OnClick="lnkGpsSettings_Click" ToolTip="GPS Settings"
                                            TabIndex="38"></asp:LinkButton></li>
                                </ul>
                            </asp:Panel>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="table-scrollable" style="border: none">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:Panel runat="server" ID="Panel1" Style="background: #316b9d; width: 100%;">
                                    <ul class="lnklist-header lnklist-panel">
                                        <li>
                                            <asp:LinkButton ToolTip="Delete" CssClass="icon-delete"
                                                ID="btnDelete" runat="server" OnClick="btnDelete_Click" CausesValidation="False"
                                                OnClientClick="javascript:return confirm('Do you want to delete selected database details? However database will not be deleted, you can again add database details from add existing database.')"></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkAddnew" ToolTip="Add New Database" CssClass="icon-addnew" 
                                                runat="server" CausesValidation="False" OnClick="lnkAddnew_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkExisting" ToolTip="Add Existing Database" style="color:white"
                                                runat="server" CausesValidation="False" OnClick="lnkExisting_Click">Add Existing Database</asp:LinkButton></li>
                                        <li><a href="#" onclick="javascript:$('#ctl00_ContentPlaceHolder1_lblMsgLogin').text('');"
                                            style="float: left; color: #2382B2; margin-right: 20px; display: none;" id="button">Login Database</a></li>
                                    </ul>
                                </asp:Panel>
                                <asp:GridView ID="gvControl" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                    DataKeyNames="id" Width="100%" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="gvControl_PageIndexChanging"
                                    OnRowDataBound="gvControl_RowDataBound" OnSorting="gvControl_Sorting" PageSize="20"
                                    OnDataBound="gvControl_DataBound" OnRowCommand="gvControl_RowCommand">
                                    <RowStyle CssClass="evenrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnSelected" runat="server" />
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Database Name" SortExpression="dbname">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDBname" runat="server" Text='<%#Eval("dbname")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name" SortExpression="companyname">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcompanyname" runat="server" Text='<%#Eval("companyname")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                            <ItemTemplate>
                                                <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
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
            <!-- edit-tab end -->
        </div>
    </div>
    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="pnlOverlay"
        PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="pnlOverlay" Visible="false">
        <%--<div class="title_bar_popup">
            <asp:Label CssClass="title_text" ID="Label15" runat="server">Database Maintenance</asp:Label>
        </div>--%>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <Triggers>
                <%--<asp:PostBackTrigger ControlID="btnUpload" />--%>
            </Triggers>
            <ContentTemplate>
                <asp:Panel ID="pnlGPSSett" runat="server" Visible="false" Style="border: 1px solid #316b9d">
                    <div class="title_bar_popup">
                        <asp:Label CssClass="title_text" ID="Label2" Style="color: white" runat="server">GPS Settings</asp:Label>
                        <asp:LinkButton ID="lnkCloseGPS" runat="server" CausesValidation="False" OnClick="lnkCloseGPS_Click"
                            Style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                        <asp:LinkButton ID="lnkGPS" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                            OnClick="lnkGPS_Click">Save</asp:LinkButton>
                    </div>

                    <div style="padding: 15px; background-color: white">
                        <table style="width: 350px; background-color: white; height: 70px;">
                            <tr>
                                <td>GPS ping interval
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlGPSPing" runat="server" onchange="AlertPing(this)" class="form-control"
                                        ToolTip="This feature is for GPS tracking app. The interval selected will be the interval at which the GPS tracking app pings the GPS satellites to get the location. Maximum the time is minimum will be the phone battery consumption. Setting the interval to zero will cause huge amout of phone battery use. Recommended interval is 1 minute.">
                                        <asp:ListItem Text="0" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="30 Second" Value="30000"></asp:ListItem>
                                        <asp:ListItem Text="1 Minute" Value="60000"></asp:ListItem>
                                        <asp:ListItem Text="2 Minutes" Value="120000"></asp:ListItem>
                                        <asp:ListItem Text="3 Minutes" Value="180000"></asp:ListItem>
                                        <asp:ListItem Text="4 Minutes" Value="240000"></asp:ListItem>
                                        <asp:ListItem Text="5 Minutes" Value="300000"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMSgGPS" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlContact" runat="server" Visible="false" Style="border: 1px solid #316b9d">
                    <div class="title_bar_popup">
                        <asp:Label CssClass="title_text" ID="Label1" Style="color: white" runat="server">Change Password</asp:Label>
                        <asp:LinkButton ID="lnkCancelContact" runat="server" CausesValidation="False" OnClick="LinkButton2_Click"
                            Style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                        <asp:LinkButton ID="lnkContactSave" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                            OnClick="lnkContactSave_Click"
                            ValidationGroup="cont">Save</asp:LinkButton>
                    </div>
                    <div align="center">
                        <asp:Label ID="Label4" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                    </div>
                    <div style="padding: 15px; background-color: white">
                        <table style="width: 350px; background-color: white; height: 120px;">
                            <tr>
                                <td>User Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminUser" runat="server" class="form-control" placeholder="Username"
                                        ToolTip="Username"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Password
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAdminPass" runat="server" class="form-control" placeholder="Password"
                                        ToolTip="Password"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Label ID="lblMsgAdmin" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

    </asp:Panel>
</asp:Content>
