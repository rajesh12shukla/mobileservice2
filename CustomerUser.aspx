<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="CustomerUser.aspx.cs" Inherits="CustomerUser" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                    <a href="#">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Customer Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="#">Locations</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Edit Locations</span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Edit Customer User</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" Style="min-width: 520px" ID="lblCustomerName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="true">
                                    <ul class="lnklist-header">
                                        <li style="margin-right:0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" CssClass="icon-first" runat="server" CausesValidation="False" OnClick="lnkFirst_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" CssClass="icon-previous" runat="server" CausesValidation="False" OnClick="lnkPrevious_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CssClass="icon-next" CausesValidation="False" OnClick="lnkNext_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False" OnClick="lnkLast_Click">
                                            </asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>

                            </asp:Panel>
                        </li>
                        <li>
                            <ul class="lnklist-header">
                                <li style="margin-right:0">
                                    <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ToolTip="Save" runat="server" OnClick="btnSubmit_Click"
                                        TabIndex="23"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                        OnClick="lnkClose_Click" TabIndex="24"></asp:LinkButton></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="col-md-6 col-lg-6">
                        <div class="form-col">
                            <div class="fc-label1">
                                Customer Name<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCName"
                                    ErrorMessage="First Name Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator1"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1"
                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender">
                                    </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="75" CssClass="form-control" TabIndex="1"
                                    ID="txtCName"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Customer Address<asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress"
                                    ErrorMessage="Address Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator11"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator11"
                                        ID="RequiredFieldValidator11_ValidatorCalloutExtender">
                                    </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="8000" TextMode="MultiLine" CssClass="form-control"
                                    TabIndex="2" ID="txtAddress"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Customer Address 2
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" CssClass="form-control" TabIndex="3"
                                    ID="txtAddress2"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                City<asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" ErrorMessage="City Required"
                                    Display="None" SetFocusOnError="True" ID="RequiredFieldValidator6"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6"
                                        ID="RequiredFieldValidator6_ValidatorCalloutExtender">
                                    </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="4"
                                    ID="txtCity"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                State/Prov.<asp:RequiredFieldValidator runat="server" InitialValue="State" ControlToValidate="ddlState"
                                    ErrorMessage="State Required" Display="None" SetFocusOnError="True" ID="RequiredFieldValidator7"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7"
                                        ID="RequiredFieldValidator7_ValidatorCalloutExtender">
                                    </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="fc-input">
                                <asp:DropDownList runat="server" CssClass="form-control" TabIndex="5" ToolTip="State"
                                    ID="ddlState">
                                    <asp:ListItem Value="State">State</asp:ListItem>
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
                                    <asp:ListItem Value="AB">Alberta</asp:ListItem>
                                    <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                                    <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                                    <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                                    <asp:ListItem Value="NL">Newfoundland and Labrador</asp:ListItem>
                                    <asp:ListItem Value="NT">Northwest Territories</asp:ListItem>
                                    <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                                    <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                                    <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                                    <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                                    <asp:ListItem Value="ON">Ontario</asp:ListItem>
                                    <asp:ListItem Value="QC">Quebec</asp:ListItem>
                                    <asp:ListItem Value="YT">Yukon</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Zip/Postal Code
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="10" CssClass="form-control" TabIndex="6"
                                    ID="txtZip"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Main Contact
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="7"
                                    ID="txtMaincontact"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Phone
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="28" CssClass="form-control" TabIndex="8"
                                    ID="txtPhoneCust"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Website
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="9"
                                    ID="txtWebsite"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Email
                                <asp:RegularExpressionValidator runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                    ControlToValidate="txtEmail" ErrorMessage="Invalid Email" Display="None" SetFocusOnError="True"
                                    ID="RegularExpressionValidator2"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2"
                                    ID="RegularExpressionValidator2_ValidatorCalloutExtender">
                                </asp:ValidatorCalloutExtender>

                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="50" CssClass="form-control" TabIndex="10"
                                    ID="txtEmail"></asp:TextBox>
                                <asp:FilteredTextBoxExtender runat="server" FilterMode="InvalidChars" InvalidChars=" "
                                    Enabled="True" TargetControlID="txtEmail" ID="txtEmail_FilteredTextBoxExtender">
                                </asp:FilteredTextBoxExtender>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Cellular
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="28" CssClass="form-control" TabIndex="11"
                                    ID="txtCell"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Remarks
                            </div>
                            <div class="fc-input">
                                <asp:TextBox runat="server" MaxLength="8000" TextMode="MultiLine" CssClass="form-control"
                                    Height="74px" TabIndex="19" ID="txtRemarks"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Type
                            </div>
                            <div class="fc-input">
                                <asp:DropDownList runat="server" CssClass="form-control" TabIndex="12"
                                    ID="ddlUserType">
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                Status
                            </div>
                            <div class="fc-input">
                                <asp:DropDownList runat="server" CssClass="form-control" TabIndex="13"
                                    ID="ddlCustStatus">
                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 col-lg-6">
                        <div class="form-col">
                            <div class="fc-label1">
                                Username<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserName"
                                    Display="None" ErrorMessage="Username Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator3">
                                    </asp:ValidatorCalloutExtender>
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="15"
                                    TabIndex="15"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPassword"
                                    Display="None" ErrorMessage="Password Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator4">
                                </asp:ValidatorCalloutExtender>
                                Password
                            </div>
                            <div class="fc-input">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="10"
                                    TabIndex="16"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                View Service History
                            
                            </div>
                            <div class="fc-input">
                                <asp:CheckBox ID="chkMap" runat="server" TabIndex="17" />
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                View Invoices
                            </div>
                            <div class="fc-input">
                                <asp:CheckBox ID="chkScheduleBrd" runat="server" TabIndex="18" />
                            </div>
                        </div>
                        <div class="form-col">
                            <div class="fc-label1">
                                View Equipment
                            </div>
                            <div class="fc-input">
                                <asp:CheckBox ID="chkEquipments" runat="server" TabIndex="18" />
                            </div>
                        </div>

                        <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            Group By Work Order 
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkGrpWO" runat="server" TabIndex="18" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                 <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            View All Open Tickets
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkOpenTicket" runat="server" TabIndex="18" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="table-scrollable">
                        <asp:GridView ID="gvLoc" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                            Width="100%" EmptyDataText="No Locations Found...">
                            <RowStyle CssClass="evenrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <Columns>
                                <asp:TemplateField HeaderText="ID" Visible="False">
                                    <ItemTemplate>
                                        <asp:Label ID="lblloc" runat="server" Text='<%# Bind("loc") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acct #" SortExpression="locid">
                                    <ItemTemplate>
                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText=" Location Name" SortExpression="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("tag") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Address" SortExpression="Address">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFx" runat="server"><%#Eval("Address")%></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="City" SortExpression="city">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCell" runat="server"><%#Eval("city")%></asp:Label>Panel1
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type" SortExpression="type">
                                    <ItemTemplate>
                                        <asp:Label ID="lblType" runat="server"><%#Eval("type")%></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Group" SortExpression="status">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlGroup" CssClass="form-control input-sm input-small" runat="server" DataTextField="Role" DataValueField="ID"
                                            DataSource='<%#dtGroupData%>' SelectedValue='<%# Eval("RoleID") %>'
                                            Width="120px">
                                        </asp:DropDownList>
                                        <asp:Panel ID="Panel3" runat="server" Style="top: 825px; left: 980px; border: 1px solid #316b9d; background-color: white; width: 60px; padding: 5px">
                                            <asp:LinkButton ID="lnkAddGrp" runat="server" OnClick="lnkAddGrp_Click" CausesValidation="false">Add/Edit Group</asp:LinkButton>
                                        </asp:Panel>
                                        <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetX="150" OffsetY="-10" PopupControlID="Panel3"
                                            TargetControlID="ddlGroup">
                                        </asp:HoverMenuExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle CssClass="selectedrowcolor" />
                            <AlternatingRowStyle CssClass="oddrowcolor" />
                        </asp:GridView>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
                        RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel CssClass="custuser-popup" runat="server" ID="Panel1" Style="display: block; background: #fff; border: 1px solid #316b9d;">
                        <asp:Panel runat="Server" ID="Panel2">
                            <div class="pc-title" style="margin-top: 0 !important; color: white;">
                                <asp:Label runat="server" CssClass="title_text" Text="Location Groups"></asp:Label>
                                <div class="pt-right">
                                    <asp:LinkButton ID="lnkClosePop" runat="server" CssClass="popup-anchor"
                                        OnClick="lnkClosePop_Click">Close</asp:LinkButton>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="custuser-popup" style="padding: 8px; padding-right: 10px;">
                            <div class="table-scrollable" style="border: none;">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" ID="gvGroup" OnRowEditing="gvGroup_RowEditing" OnRowCancelingEdit="gvGroup_RowCancelingEdit"
                                            OnRowDeleting="gvGroup_RowDeleting" OnRowUpdating="gvGroup_RowUpdating" ShowFooter="true">
                                            <AlternatingRowStyle CssClass="oddrowcolor"></AlternatingRowStyle>
                                            <Columns>
                                                <asp:TemplateField HeaderText="ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Role") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtName" Text='<%# Eval("Role") %>' runat="server"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtName"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtNameF" runat="server" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNameF"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Username">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUser" runat="server"><%#Eval("Username")%></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtUser" CssClass="form-control" Text='<%# Eval("Username") %>' runat="server"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtUser_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                            TargetControlID="txtUser">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtUser"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>

                                                        <asp:RegularExpressionValidator ID="txtUser_Regexval" runat="server" ValidationGroup="edit" SetFocusOnError="True"
                                                            ControlToValidate="txtUser"
                                                            ErrorMessage="Minimum 6 characters"
                                                            ValidationExpression=".{6}.*" Display="None" />
                                                        <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtUser_Regexval"
                                                            ID="txtUser_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtUserF" CssClass="form-control" runat="server"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtUserF_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                            TargetControlID="txtUserF">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtUserF"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="txtUserF_Regexval" runat="server" ValidationGroup="add" SetFocusOnError="True"
                                                            ControlToValidate="txtUserF"
                                                            ErrorMessage="Minimum 6 characters"
                                                            ValidationExpression=".{6}.*" Display="None" />
                                                        <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtUserF_Regexval"
                                                            ID="txtUserF_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Password">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPassword" runat="server"><%# new string('*', Eval("password").ToString().Length)%></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtPassword" CssClass="form-control" Text='<%# Eval("password") %>' runat="server"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                            TargetControlID="txtPassword">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPassword"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="txtPassword_Regexval" runat="server" ValidationGroup="edit" SetFocusOnError="True"
                                                            ControlToValidate="txtPassword"
                                                            ErrorMessage="Minimum 6 characters"
                                                            ValidationExpression=".{6}.*" Display="None" />
                                                        <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtPassword_Regexval"
                                                            ID="txtPassword_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>

                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtPasswordF" CssClass="form-control" runat="server"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtPasswordF_FilteredTextBoxExtender" runat="server"
                                                            Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                            TargetControlID="txtPasswordF">
                                                        </asp:FilteredTextBoxExtender>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtPasswordF"
                                                            Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" ValidationGroup="add"></asp:RequiredFieldValidator>

                                                        <asp:RegularExpressionValidator ID="txtPasswordF_Regexval" runat="server" SetFocusOnError="True"
                                                            ControlToValidate="txtPasswordF"
                                                            ErrorMessage="Minimum 6 characters"
                                                            ValidationExpression=".{6}.*" Display="None" ValidationGroup="add" />
                                                        <asp:ValidatorCalloutExtender runat="server" Enabled="True" TargetControlID="txtPasswordF_Regexval"
                                                            ID="txtPasswordF_Regexval_ValidatorCalloutExtender" PopupPosition="BottomLeft">
                                                        </asp:ValidatorCalloutExtender>

                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <%--<asp:LinkButton ID="lnkEdit" CommandName="Edit" runat="server"><img title="Edit" alt="Edit" src="images/edit-grid.png"/><i class="fa fa-pencil"</asp:LinkButton>--%>
                                                        <asp:LinkButton ID="lnkEdit" CommandName="Edit" CssClass="btn location-search" runat="server"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                        <%--<asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this group');"><img title="Delete" alt="Delete" src="images/delete-grid.png"/></asp:LinkButton>--%>
                                                        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CssClass="btn location-search" OnClientClick="return confirm('Are you sure you want to delete this group');"><i class="fa fa-trash-o"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lnkUpdate" runat="server" CommandName="Update" CssClass="btn location-search" ValidationGroup="edit"><i class="fa fa-save"></i></asp:LinkButton>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn location-search" CommandName="Cancel"><i class="fa fa-times"></i></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lnkAdd" runat="server" OnClick="lnkAdd_Click" CssClass="btn location-search" ValidationGroup="add"><i class="fa fa-plus"></i></asp:LinkButton>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <RowStyle CssClass="evenrowcolor"></RowStyle>
                                            <FooterStyle CssClass="footer" />
                                            <SelectedRowStyle CssClass="selectedrowcolor"></SelectedRowStyle>
                                        </asp:GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
