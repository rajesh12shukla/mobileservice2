<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddRoute.aspx.cs" Inherits="AddRoute" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Route</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblRoute" runat="server"></asp:Label></li>
                        <li>
                            <asp:LinkButton ID="lnkSave" runat="server" CssClass="icon-save" ToolTip="Save" OnClick="lnkSave_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" ToolTip="Close" CssClass="icon-closed" OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <div class="col-md-8">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>Name</label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtName" runat="server" AutoCompleteType="None" CssClass="form-control"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                ControlToValidate="txtName" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                            </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                            ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator1">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                            </div>
                            <div class="form-col">

                                <div class="fc-label">
                                    <label>
                                        Worker
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                ControlToValidate="ddlRoute" Display="None" ErrorMessage="Worker Required" SetFocusOnError="True">
                                            </asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator9_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator9">
                                        </asp:ValidatorCalloutExtender>
                                    </label>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="form-control" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>
                                        Remarks
                                    </label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control"
                                        Height="50px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

