<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddExistingDB.aspx.cs" Inherits="AddExistingDB" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="/Home.aspx">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                     <span>Database Maintenance</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <asp:Label ID="lblAddEditUser" runat="server" Text="Edit Existing Database"></asp:Label>
                </li>
            </ul>--%>
        </div>

        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Edit Existing Database</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                               
                                <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save"
                                    TabIndex="38"></asp:LinkButton>
                                 <asp:LinkButton CssClass="icon-closed" ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false"
                                    OnClick="lnkClose_Click" TabIndex="39"></asp:LinkButton> 
                            </asp:Panel>
                        </li>
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">

                <div class="com-cont">
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Company Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtCompany" Display="None" ErrorMessage="Company Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" Width="100%" MaxLength="75"
                                        TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Database Type
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:DropDownList ID="ddlDBType" runat="server" CssClass="form-control" Width="100%"
                                        TabIndex="12">
                                        <asp:ListItem Value="MSM">MSM</asp:ListItem>
                                        <asp:ListItem Value="TS">TS</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Database<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtDB"
                                        Display="None" ErrorMessage="Database Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtDB" runat="server" CssClass="form-control" Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDSN" runat="server" CssClass="register_input_bg" Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDuser" runat="server" CssClass="register_input_bg" Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDpass" runat="server" CssClass="register_input_bg" Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div></div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
