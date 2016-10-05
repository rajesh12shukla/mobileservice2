<%@ Page Title="" Language="C#" MasterPageFile="~/Popup.master" AutoEventWireup="true"
    CodeFile="ConvertProspect.aspx.cs" Inherits="ConvertProspect" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hdnProspectID" runat="server" />
    <table>
        <tr>
            <td  colspan="4">
                <div class="title_bar_popup">
                    <a id="lnkClose" style="float: right; cursor:pointer; margin-right: 20px;
                        color: #fff; margin-left: 10px; height: 16px;">Close</a>
                    <asp:LinkButton runat="server" ID="lnkConvert" Text="Convert" Style="float: right;
                        margin-right: 20px; color: #fff; margin-left: 10px;" 
                        onclick="lnkConvert_Click" />
                </div>
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                Prospect
            </td>
            <td colspan="3">
                <asp:TextBox runat="server" CssClass="register_input_bg_customer searchinput" Width="520px"
                    ID="txtProspect" autocomplete="off" Enabled="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                Customer Name
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCustName" 
                    ErrorMessage="Customer Name Required" Display="None" SetFocusOnError="True" 
                    ID="RequiredFieldValidator22"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" 
                    PopupPosition="TopLeft" Enabled="True" 
                    TargetControlID="RequiredFieldValidator22" 
                    ID="RequiredFieldValidator22_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td colspan="3">
                <asp:TextBox runat="server" CssClass="register_input_bg_customer searchinput" Width="520px"
                    ID="txtCustName" autocomplete="off"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                Customer Type
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomerType" 
                    ErrorMessage="Customer Type Required" Display="None" SetFocusOnError="True" 
                    ID="RequiredFieldValidator17"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" 
                    TargetControlID="RequiredFieldValidator17" 
                    ID="RequiredFieldValidator17_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td>
                <asp:DropDownList runat="server" CssClass="register_input_bg" TabIndex="12" Width="200px"
                    ID="ddlCustomerType">
                </asp:DropDownList>
            </td>
            <td class="register_lbl">
                Default Worker
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlRoute" 
                    ErrorMessage="Default Worker Required" Display="None" SetFocusOnError="True" 
                    ID="RequiredFieldValidator20"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" 
                    TargetControlID="RequiredFieldValidator20" 
                    ID="RequiredFieldValidator20_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td>
                <asp:DropDownList runat="server" CssClass="register_input_bg_ddl" TabIndex="14" Width="200px"
                    ID="ddlRoute">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                Location Type
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlLocType" 
                    ErrorMessage="Location Type Required" Display="None" SetFocusOnError="True" 
                    ID="RequiredFieldValidator18"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" 
                    TargetControlID="RequiredFieldValidator18" 
                    ID="RequiredFieldValidator18_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td>
                <asp:DropDownList runat="server" CssClass="register_input_bg_ddl" TabIndex="7" Width="200px"
                    ID="ddlLocType">
                </asp:DropDownList>
            </td>
            <td class="register_lbl">
                Default Salesperson
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlTerr" 
                    ErrorMessage="Default Salesperson Required" Display="None" 
                    SetFocusOnError="True" ID="RequiredFieldValidator21"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" 
                    TargetControlID="RequiredFieldValidator21" 
                    ID="RequiredFieldValidator21_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td>
                <asp:DropDownList runat="server" CssClass="register_input_bg_ddl" TabIndex="19" Width="200px"
                    ID="ddlTerr">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                Sales Tax
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSTax" 
                    ErrorMessage="Sales Tax Required" Display="None" SetFocusOnError="True" 
                    ID="RequiredFieldValidator19"></asp:RequiredFieldValidator>

                                    <asp:ValidatorCalloutExtender runat="server" Enabled="True" 
                    TargetControlID="RequiredFieldValidator19" 
                    ID="RequiredFieldValidator19_ValidatorCalloutExtender"></asp:ValidatorCalloutExtender>

                                    
            </td>
            <td>
                <asp:DropDownList runat="server" CssClass="register_input_bg_ddl" TabIndex="7" Width="200px"
                    ID="ddlSTax">
                </asp:DropDownList>
            </td>
            <td class="register_lbl">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td class="register_lbl">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
            <td class="register_lbl">
                &nbsp;
            </td>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
</asp:Content>
