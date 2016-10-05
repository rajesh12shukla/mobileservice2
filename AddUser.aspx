<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/HomeMaster.master"
    CodeFile="AddUser.aspx.cs" Inherits="AddUser" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function cancel() {
            window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
        }

        function AddNewMerchant(dropdown) {
            if (dropdown.selectedIndex == 1) {
                TogglePopUp();
            }
        }

        function TogglePopUp() {
            debugger;
            var pnlMerchant = document.getElementById('<%= pnlMerchant.ClientID %>');
            if (pnlMerchant.style.display == 'block') {
                $find("programmaticModalPopupBehavior").hide();
                return false;
            } else {
                $find("programmaticModalPopupBehavior").show();
                return false;
            }
        }

        $(document).ready(function () {
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
            }
        });
    </script>
    <script type="text/javascript">
        function hideModalPopup() {
            jQuery("#setuppopup").modal("hide");
            window.location.reload();
        }
    </script>
    <style type="text/css">
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
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
                    <span>Program Manager</span>--%>
            <%-- <a href="#">Program Manager</a>--%>
            <%--      <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/Users.aspx") %>">Users</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <asp:Label ID="lblAddEditUser" runat="server" Text="Add User"></asp:Label>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add User</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                    <ul class="lnklist-header">
                                        <li style="margin-right: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" OnClick="lnkFirst_Click" CssClass="icon-first"
                                                CausesValidation="False" TabIndex="34"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" TabIndex="35" ToolTip="Previous" runat="server" CssClass="icon-previous"
                                                CausesValidation="False" OnClick="lnkPrevious_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" TabIndex="36" ToolTip="Next" runat="server" CausesValidation="False" CssClass="icon-next"
                                                OnClick="lnkNext_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" TabIndex="37" ToolTip="Last" runat="server" CausesValidation="False" CssClass="icon-last"
                                                OnClick="lnkLast_Click"></asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            </asp:Panel>
                        </li>
                        <li>
                            <ul>
                                <li style="margin-right: 0">
                                    <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save"
                                        TabIndex="38"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false"
                                        OnClick="lnkClose_Click" TabIndex="39"></asp:LinkButton></li>
                                <li><a runat="server" id="lnkCancelContact" href="#" onclick="cancel();" class="close_button_Form"
                                    tabindex="36" visible="false">Close</a></li>
                            </ul>
                        </li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="col-lg-5 col-md-5 col-sm-5">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    First Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                        ControlToValidate="txtFName" Display="None" ErrorMessage="First Name Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server"
                                        ControlToValidate="txtMName" Display="None" ErrorMessage="Middle Name Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>--%>
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:TextBox ID="txtFName" runat="server" CssClass="form-control" MaxLength="15"
                                        TabIndex="1"></asp:TextBox>
                                    <asp:TextBox ID="txtMName" runat="server" width="31px" MaxLength="5"
                                        TabIndex="2" ToolTip="Middle Name"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Last Name<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                        ControlToValidate="txtLName" Display="None" ErrorMessage="Last Name Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator2">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtLName" runat="server" CssClass="form-control" MaxLength="15"
                                        TabIndex="2"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>Customer Name</label>
                                </div>
                                <div class="fc-input">
                                    <input type="text" tabindex="3" class="form-control" />
                                </div>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Address<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtAddress"
                                        Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator5_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator5">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" MaxLength="8000"
                                        TabIndex="4"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    City<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCity"
                                        Display="None" ErrorMessage="City Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator6_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator6">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="5"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    State<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlState"
                                        Display="None" ErrorMessage="State Required" SetFocusOnError="True" InitialValue="State"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator7_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator7">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="form-control"
                                        TabIndex="6">
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
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Zip
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="10"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Telephone<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server"
                                        ControlToValidate="txtTelephone" Display="None" ErrorMessage="Telephone Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator8_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator8">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtTelephone" runat="server" CssClass="form-control" MaxLength="22"
                                        TabIndex="8"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Cellular
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" MaxLength="22"
                                        TabIndex="9"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Email<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender
                                            ID="RegularExpressionValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtEmail"
                                        Display="None" ErrorMessage="Email Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator9_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator9">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="10"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Text Message
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtMsg" runat="server" CssClass="form-control" TabIndex="11"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblMultiLang" runat="server" Text="Language"></asp:Label>
                                    <%--      <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlLang"
                                        Display="None" ErrorMessage="Language Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator12">
                                    </asp:ValidatorCalloutExtender>--%>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlLang" runat="server" CssClass="form-control" TabIndex="10">
                                        <asp:ListItem Value="">-- Select --</asp:ListItem>
                                        <asp:ListItem Value="english">English</asp:ListItem>
                                        <asp:ListItem Value="spanish">Spanish</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Merchant ID
                                </div>
                                <div class="fc-input merchant-input">
                                    <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlMerchantID" runat="server" onchange="AddNewMerchant(this);"
                                                CssClass="form-control" Style="float: left;" TabIndex="12">
                                            </asp:DropDownList>
                                            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                                                <ContentTemplate>
                                                    <asp:ImageButton ID="imgbtnMerchant" runat="server" CausesValidation="False" ImageUrl="~/images/edit.png"
                                                        OnClick="imgbtnMerchant_Click" ToolTip="Edit" Height="30px" TabIndex="10" />
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Status
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="form-control" TabIndex="13">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    User Type
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control"
                                                OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged" TabIndex="14" AutoPostBack="True">
                                                <asp:ListItem Value="0">Office</asp:ListItem>
                                                <asp:ListItem Value="1">Field</asp:ListItem>
                                            </asp:DropDownList>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblSales" runat="server" Text="Salesperson"></asp:Label>
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkSalesperson" runat="server" AutoPostBack="True" OnCheckedChanged="chkSalesperson_CheckedChanged"
                                                TabIndex="32" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Email Account
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="chkEmailAcc" runat="server" AutoPostBack="True" Enabled="False"
                                                OnCheckedChanged="chkEmailAcc_CheckedChanged" TabIndex="15" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Date of Termination
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtTerminationDt" runat="server" CssClass="form-control" MaxLength="10"
                                        TabIndex="16"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtTerminationDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtTerminationDt">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Date of Hiring<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server"
                                        ControlToValidate="txtHireDt" Display="None" ErrorMessage="Date of Hiring Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator10_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            PopupPosition="Right" TargetControlID="RequiredFieldValidator10">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtHireDt" runat="server" CssClass="form-control" MaxLength="10"
                                        TabIndex="17"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtHireDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtHireDt">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Department
                                </div>
                                <div class="fc-input">
                                    <div style="overflow-y: scroll; overflow-x: hidden; height: 100px; width: 198px">
                                        <asp:CheckBoxList ID="ddlDepartment" runat="server"
                                            TabIndex="18" SelectionMode="Multiple">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Pay Method
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlPayMethod" runat="server" CssClass="form-control"
                                        TabIndex="19" AutoPostBack="True" OnSelectedIndexChanged="ddlPayMethod_SelectedIndexChanged1">
                                        <asp:ListItem Value="0">Salaried</asp:ListItem>
                                        <asp:ListItem Value="1">Hourly</asp:ListItem>
                                        <asp:ListItem Value="2">Fixed Hours</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Hours
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlPayMethod" />
                                        </Triggers>
                                        <ContentTemplate>

                                            <asp:TextBox ID="txtHours" runat="server" CssClass="form-control"
                                                MaxLength="28" step="any" TabIndex="20" Enabled="False"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtHours_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="txtHours" ValidChars="1234567890.-">
                                            </asp:FilteredTextBoxExtender>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Amount
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlPayMethod" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"
                                                MaxLength="28" step="any" TabIndex="25" Enabled="False"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="txtAmount_FilteredTextBoxExtender" runat="server"
                                                Enabled="True" TargetControlID="txtAmount" ValidChars="1234567890.-">
                                            </asp:FilteredTextBoxExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Pay Period
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlPayPeriod" runat="server" CssClass="form-control"
                                        TabIndex="26">
                                        <asp:ListItem Value="0">Weekly</asp:ListItem>
                                        <asp:ListItem Value="1">Bi-Weekly</asp:ListItem>
                                        <asp:ListItem Value="2">Semi-Monthly</asp:ListItem>
                                        <asp:ListItem Value="3">Monthly</asp:ListItem>
                                        <asp:ListItem Value="4">Semi-Annually</asp:ListItem>
                                        <asp:ListItem Value="5">Annually</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Mileage Rate
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtMileageRate" runat="server"
                                        CssClass="form-control"
                                        MaxLength="28" step="any" TabIndex="27"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtMileageRate_FilteredTextBoxExtender" runat="server"
                                        Enabled="True" TargetControlID="txtMileageRate" ValidChars="1234567890.-">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Emp ID
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtEmpID" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="28"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>Remarks</label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtRemarks" runat="server" Rows="3" TextMode="MultiLine"
                                        MaxLength="8000" TabIndex="57" CssClass="form-control pnlAccounts"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>Start Date</label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtStartDate" runat="server" TabIndex="58" autocomplete="off"
                                         CssClass="form-control" onkeypress="return false;"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtStartDate">  </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <label>End Date</label>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtEndDate" runat="server" TabIndex="58" autocomplete="off"
                                         CssClass="form-control" onkeypress="return false;" ></asp:TextBox>
                                    <asp:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Enabled="True"
                                         TargetControlID="txtEndDate">  </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <asp:UpdatePanel runat="server" ID="updatepnlemail">
                            <ContentTemplate>
                                <asp:Panel ID="pnlEmailAccount" runat="server" Visible="False">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Incoming Mail Server (IMAP)<asp:RequiredFieldValidator ID="RequiredFieldValidator19"
                                                runat="server" ControlToValidate="txtInServer" Display="None" ErrorMessage="Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator19">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtInServer" runat="server" CssClass="form-control"
                                                MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Port<asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtinPort"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator20">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtinPort" runat="server" CssClass="form-control" Width="70px"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtinPort">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Username<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                ControlToValidate="txtInUSername" Display="None" ErrorMessage="Invalid Username"
                                                SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender
                                                    ID="RegularExpressionValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    TargetControlID="RegularExpressionValidator2" PopupPosition="Left">
                                                </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtInUSername"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator21_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator21">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtInUSername" runat="server" CssClass="form-control"
                                                TabIndex="1" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Password<asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                ControlToValidate="txtInPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator22">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtInPassword" runat="server" CssClass="form-control"
                                                TabIndex="1" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <label>SSL</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkSSL" runat="server" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                        </div>
                                        <div class="fc-input">
                                            <asp:Button ID="btnTestIncoming" runat="server" CssClass="btn btn-primary" OnClick="btnTestIncoming_Click" Text="Test Settings" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Outgoing Mail Server (SMTP)<asp:RequiredFieldValidator ID="RequiredFieldValidator23"
                                                runat="server" ControlToValidate="txtOutServer" Display="None" ErrorMessage="Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator23_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator23">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtOutServer" runat="server" CssClass="form-control"
                                                TabIndex="1" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Port<asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtOutPort"
                                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator24">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtOutPort" runat="server" CssClass="form-control" Width="70px"
                                                TabIndex="1"></asp:TextBox>
                                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                                FilterMode="ValidChars" FilterType="Numbers" TargetControlID="txtOutPort">
                                            </asp:FilteredTextBoxExtender>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Username<asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                ControlToValidate="txtOutUsername" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                    ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtOutUsername"
                                                    Display="None" ErrorMessage="Invalid Username" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender
                                                        ID="RegularExpressionValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RegularExpressionValidator3" PopupPosition="Left">
                                                    </asp:ValidatorCalloutExtender>
                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator25_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator25">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtOutUsername" runat="server" CssClass="form-control"
                                                TabIndex="1" MaxLength="100"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Password<asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server"
                                                ControlToValidate="txtOutPassword" Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator26">
                                                </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtOutPassword" runat="server" CssClass="form-control"
                                                TabIndex="1" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Same as Incoming
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkSame" runat="server" AutoPostBack="True" OnCheckedChanged="chkSame_CheckedChanged" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                        </div>
                                        <div class="fc-input">
                                            <asp:Button ID="btnTestOut" runat="server" OnClick="btnTestOut_Click" CssClass="btn btn-primary" Text="Test Settings" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="col-lg-7 col-md-7 col-sm-7">
                        <div class="permisson">
                            <fieldset class="roundCorner">
                                <div class="col-md-10 table-scrollable-borderless">
                                    <h3><b>Permissions</b></h3>
                                    <table class="table_Permission" style="height: 300px" runat="server" id="tblPermission">
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkTicket" runat="server" Text="Create ticket" TabIndex="29" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkMassReview" runat="server" Text="Mass Review" TabIndex="30" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkWorkDt" runat="server" Text="Edit work date" TabIndex="31" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkEmpMainten" runat="server" Text="Employee Maintenance" TabIndex="32" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkLocation" runat="server" Text="Location remarks" TabIndex="33" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkTimestampFix" runat="server" Text="Timestamps Fixed" TabIndex="34" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkServiceHist" runat="server" Text="Access service history" TabIndex="35" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkEditEquipment" runat="server" Text="Edit Equipments"
                                                    TabIndex="36" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkPurchaseOrd" runat="server" Text="Add purchase orders" TabIndex="37" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkAddEquipments" runat="server" Text="Add Equipments"
                                                    TabIndex="38" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkExpenses" runat="server" Text="Enter expenses" TabIndex="39" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkFinanceMgr" runat="server" Text="Financial Manager"
                                                    TabIndex="40" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkProgram" runat="server" Text="Program functions" TabIndex="41" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkFinanceStatement" runat="server" Text="Financial Statement"
                                                    TabIndex="42" />
                                            </td>
                                          
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkAccessUser" runat="server" Text="Access users" TabIndex="43" />
                                            </td>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkAccountPayable" runat="server" Text="Account Payable"
                                                    TabIndex="44" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkDispatch" runat="server" Text="Email Dispatch" TabIndex="45" />
                                            </td>
                                       
                                        </tr>
                                        <tr>
                                            <td class="permission-box">
                                                <asp:CheckBox ID="chkSalesMgr" runat="server" Text="Sales Manager" TabIndex="47" />
                                            </td>
                                            <td></td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                            <fieldset class="roundCorner">
                                <div class="col-sm-8 table-scrollable-borderless">
                                    <h3>
                                        <b>Page Permissions</b> </h3>
                                    <asp:GridView ID="gvPages" runat="server"
                                        AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content">
                                        <RowStyle CssClass="evenrowcolor" />
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAccess" runat="server" Checked='<%# Bind("access") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Screen">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScreen" runat="server" Text='<%# Bind("pagename") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Add" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkAdd" runat="server" Checked='<%# Bind("add") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="View" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkView" runat="server" Checked='<%# Bind("view") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkEdit" runat="server" Checked='<%# Bind("edit") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete" Visible="false">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkDelete" runat="server" Checked='<%# Bind("delete") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>

                                </div>
                            </fieldset>
                            <fieldset class="roundCorner">
                                <div class="col-md-8 table-scrollable-borderless">
                                    <h3>
                                        <b>Login Credentials</b> </h3>
                                    <table style="width: 100%; height: 100px">
                                        <tr>
                                            <td class="register_lbl">Username<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUserName"
                                                Display="None" ErrorMessage="Username Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="RequiredFieldValidator3">
                                                </asp:ValidatorCalloutExtender>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="48"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtUserName_FilteredTextBoxExtender" runat="server"
                                                    Enabled="true" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtUserName">
                                                </asp:FilteredTextBoxExtender>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="register_lbl">Password<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtPassword"
                                                Display="None" ErrorMessage="Password Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator4_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="RequiredFieldValidator4">
                                                </asp:ValidatorCalloutExtender>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" MaxLength="10"
                                                    TabIndex="49"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtPassword_FilteredTextBoxExtender" runat="server"
                                                    Enabled="false" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\`"
                                                    TargetControlID="txtPassword">
                                                </asp:FilteredTextBoxExtender>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                            <asp:Panel ID="pnlMomCred" runat="server" Visible="false">
                                <fieldset class="roundCorner">
                                    <h3><b>MOM Credentials</b> </h3>
                                    <table style="width: 100%;">
                                        <tr>
                                            <td class="register_lbl">Username<asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                                ControlToValidate="txtMOMUserName" Display="None" ErrorMessage="Username Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator17_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="RequiredFieldValidator17">
                                                </asp:ValidatorCalloutExtender>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMOMUserName" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="28"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtMOMUserName_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtMOMUserName">
                                                </asp:FilteredTextBoxExtender>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="register_lbl">Password<asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server"
                                                ControlToValidate="txtMOMPassword" Display="None" ErrorMessage="Password Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                    ID="RequiredFieldValidator18_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="RequiredFieldValidator18">
                                                </asp:ValidatorCalloutExtender>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtMOMPassword" runat="server" CssClass="form-control" MaxLength="10"
                                                    TabIndex="29"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtMOMPassword_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars="~*()+|}{&quot;:?&gt;&lt;,/;'[]\ `"
                                                    TargetControlID="txtMOMPassword">
                                                </asp:FilteredTextBoxExtender>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </asp:Panel>
                            <div class="col-md-8 table-scrollable-borderless">
                                <asp:UpdatePanel ID="UpdatePanel21" runat="server">
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlWorker" runat="server" Enabled="False">
                                            <fieldset class="roundCorner">
                                                <h3><b>Options</b> </h3>
                                                <table style="height: 300px">
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblSchbrd" runat="server" Text="Schedule Board"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkScheduleBrd" runat="server" TabIndex="50" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblMap" runat="server" Text="Map"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkMap" runat="server" AutoPostBack="True" OnCheckedChanged="chkMap_CheckedChanged"
                                                                TabIndex="51" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblDeviceID" runat="server" Text="Device ID"></asp:Label>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtDeviceID"
                                                                Display="None" Enabled="False" ErrorMessage="Device ID Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator11">
                                                            </asp:ValidatorCalloutExtender>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtDeviceID" runat="server" CssClass="form-control" MaxLength="50"
                                                                TabIndex="52"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblIsSuper" runat="server" Text="Is Supervisor"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSuper" runat="server" AutoPostBack="True" OnCheckedChanged="chkSuper_CheckedChanged"
                                                                TabIndex="53" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblSuper" runat="server" Text="Supervisor"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlSuper" runat="server" AutoPostBack="True" CssClass="form-control"
                                                                OnSelectedIndexChanged="ddlSuper_SelectedIndexChanged" TabIndex="54">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblDefwork" runat="server" Text="Default Worker"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkDefaultWorker" runat="server" TabIndex="55" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="register_lbl">
                                                            <asp:Label ID="lblHourlyR" runat="server" Text="Hourly Rate"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtHourlyRate" runat="server" CssClass="form-control"
                                                                MaxLength="28" step="any" TabIndex="56"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="txtHourlyRate_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="txtHourlyRate" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="col-md-8 table-scrollable-borderless">
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <div runat="server" id="pnlGrid" visible="false" style="margin-left: 20px;">
                                        <asp:Panel runat="server" ID="pnlGridButtons" Style="background: #316b9d;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li><asp:Label CssClass="title_text" ID="lblWorkers" runat="server">Workers</asp:Label></li>
                                                <li>
                                                    <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False" ToolTip="Edit" CssClass="icon-edit"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkDone" runat="server" Visible="false" OnClick="lnkDone_Click" CausesValidation="False">Done</asp:LinkButton></li>
                                            </ul>
                                        </asp:Panel>
                                        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            DataKeyNames="UserID" Width="721px" AllowSorting="True" OnSorting="gvUsers_Sorting"
                                            PageSize="20">
                                            <RowStyle CssClass="evenrowcolor" />
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnSelected" runat="server" />
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblId" runat="server" Text='<%# Bind("UserID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="TypeID" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTypeid" runat="server" Text='<%# Bind("usertypeid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Username" SortExpression="fuser">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("fuser") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="First Name" SortExpression="ffirst">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label3" runat="server"><%#Eval("ffirst")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Name" SortExpression="last">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLN" runat="server"><%#Eval("last")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" SortExpression="usertype">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblActive" runat="server"><%#Eval("usertype")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Supervisor" SortExpression="super">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsuper" runat="server"><%#Eval("super")%></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="pnlMerchant"
                        BackgroundCssClass="ModalPopupBG" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid;" CssClass="roundCorner shadow custsetup-popup">
                        <div>
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <iframe id="iframeTicket" runat="server" width="1024px" height="600px" frameborder="0"
                                        src=""></iframe>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </asp:Panel>
                    <asp:Button runat="server" ID="hideModalPopupViaServer" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; display: none;"
                        Text="Close" OnClick="hideModalPopupViaServer_Click"
                        CausesValidation="false" />
                    <div class="clearfix"></div>
                    <asp:Panel runat="server" ID="pnlMerchant" CssClass="table-merchant" Style="background-color: #fff; border: 1px solid #316b9d">
                        <%--<div class="pnlUpdateoverlay">
                        </div>--%>
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlMerchantID" />
                                <asp:AsyncPostBackTrigger ControlID="imgbtnMerchant" />
                            </Triggers>
                            <ContentTemplate>
                                <div id="divInner">
                                    <div runat="server" id="titleBar" class="model-popup-body">
                                        <asp:Label CssClass="title_text" ID="Label1" Style="color: white" runat="server">Merchant</asp:Label>
                                        <asp:LinkButton ID="lnkCancelMerchant" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                                            OnClick="lnkCancelMerchant_Click">Close</asp:LinkButton>
                                        <asp:LinkButton ID="lnkSaveMerchant" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                            ValidationGroup="mrt" OnClick="lnkSaveMerchant_Click">Save</asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnDelete" Visible="false" runat="server" ImageUrl="images/delete.png" CausesValidation="false"
                                            ToolTip="Delete Merchant" Style="float: right; color: #fff; margin-right: 15px; height: 20px; width: 20px;"
                                            OnClick="imgbtnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete the Merchant?')">Delete</asp:LinkButton>
                                    </div>
                                    <div style="padding: 15px">
                                        <table style="height: 200px; padding: 10px">
                                            <tr>
                                                <td>Merchant ID
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtMerchantID"
                                    Display="None" ErrorMessage="Merchant ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="BottomRight" TargetControlID="RequiredFieldValidator13">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMerchantID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnMerchantInfoID" runat="server" Value="0" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Login ID
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtLoginID"
                                    Display="None" ErrorMessage="Login ID Required" SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator14_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="TopRight" TargetControlID="RequiredFieldValidator14">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtLoginID" runat="server" class="form-control" MaxLength="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Username<asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server"
                                                    ControlToValidate="txtMerchantUsername" Display="None" ErrorMessage="Username Required"
                                                    SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator15_ValidatorCalloutExtender" PopupPosition="TopRight" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator15">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMerchantUsername" runat="server" class="form-control"
                                                        MaxLength="20"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Password<asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server"
                                                    ControlToValidate="txtMerchantPassword" Display="None" ErrorMessage="Password Required"
                                                    SetFocusOnError="True" ValidationGroup="mrt"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator16_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator16">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtMerchantPassword" runat="server" class="form-control"
                                                        MaxLength="100"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
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
