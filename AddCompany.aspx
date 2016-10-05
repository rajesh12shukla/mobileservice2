<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddCompany.aspx.cs" Inherits="AddCompany" %>

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
                    <span>Database Maintenance</span>--%>
            <%-- <a href="#">Program Manager</a>--%>
            <%--<i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add New Database</span>
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
                            <asp:Label CssClass="title_text" ID="Label1" runat="server">Add New Database</asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" TabIndex="38"></asp:LinkButton>
                                <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click" TabIndex="39"></asp:LinkButton>
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
                                <div class="fc-label"><b>Company Info:</b></div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Company Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                        runat="server" ControlToValidate="txtCompany" Display="None"
                                        ErrorMessage="Company Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right"
                                        TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="75" TabIndex="1"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Address
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtAddress" runat="server"
                                        CssClass="form-control" Width="100%" MaxLength="255" TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    City
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="50" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    State
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" Width="100%"
                                        TabIndex="4" ToolTip="State">

                                        <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                        <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                        <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                        <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                        <asp:ListItem Value="CA">California</asp:ListItem>
                                        <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                        <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                        <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                        <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                        <asp:ListItem Value="FL">Florida</asp:ListItem>
                                        <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                        <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                        <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                        <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                        <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                        <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                        <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                        <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                        <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                        <asp:ListItem Value="ME">Maine</asp:ListItem>
                                        <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                        <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                        <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                        <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                        <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                        <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                        <asp:ListItem Value="MT">Montana</asp:ListItem>
                                        <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                        <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                        <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                        <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                        <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                        <asp:ListItem Value="NY">New York</asp:ListItem>
                                        <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                        <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                        <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                        <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                        <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                        <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                        <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                        <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                        <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                        <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                        <asp:ListItem Value="TX">Texas</asp:ListItem>
                                        <asp:ListItem Value="UT">Utah</asp:ListItem>
                                        <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                        <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                        <asp:ListItem Value="WA">Washington</asp:ListItem>
                                        <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                        <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                        <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Zip
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="10" TabIndex="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label"><b>Contact Info:</b></div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Contact name
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtContName" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="50" TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Telephone
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtTele" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="20" TabIndex="6"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Fax
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="20" TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Email<asp:RegularExpressionValidator ID="RegularExpressionValidator1"
                                        runat="server" ControlToValidate="txtEmail" Display="None"
                                        ErrorMessage="Invalid Email" SetFocusOnError="True"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="50" TabIndex="8"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"
                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                        TargetControlID="txtEmail">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    WebAddress
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtWebAdd" runat="server" CssClass="form-control" Width="100%"
                                        MaxLength="50" TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <b>Database Info:</b>
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
                                        TabIndex="10">
                                        <asp:ListItem Value="MSM">MSM</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Database<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtDB" Display="None" ErrorMessage="Database Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="BottomRight"
                                        TargetControlID="RequiredFieldValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtDB" runat="server" CssClass="form-control" Width="100%"
                                        TabIndex="11"></asp:TextBox>
                                    <%-- <asp:FilteredTextBoxExtender runat="server"
                                        ID="FilteredTextBoxExtender_txtDB" TargetControlID="txtDB"
                                        ValidChars="abcdefghijklmnopqrstuvwxyz">
                                    </asp:FilteredTextBoxExtender>--%>
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="col-lg-10 col-md-10">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>Remarks</label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtRemarks" runat="server" Height="66px" TextMode="MultiLine" CssClass="form-control"
                                        Width="100%"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-8 col-md-8">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDSN" runat="server" class="form-control"
                                        Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDuser" runat="server"
                                        class="form-control" Visible="False"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDpass" runat="server"
                                        class="form-control" Visible="False"></asp:TextBox>
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
