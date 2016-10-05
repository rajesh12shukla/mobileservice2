<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="ControlPanel.aspx.cs" Inherits="ControlPanel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        function ConfirmUpload() {
            document.getElementById('<%=btnUpload.ClientID%>').click();
        }
        function GetSelectedValue(ddlCountry)
        {
            var selectedValue = ddlCountry.value;
            if(selectedValue =="1")
            {
                $(".GST").show();
            }
            else
            {
                $(".GST").hide();
            }
        }
     
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            var ddlCountry = document.getElementById('<%=ddlCountry.ClientID%>')
            if (ddlCountry.value == "1")
            {
                $(".GST").show();
            } else {
                $(".GST").hide();
            }

            ///////////// Ajax call for GL acct auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("#<%=txtGSTGL.ClientID%>").autocomplete({

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

                    $("#<%=txtGSTGL.ClientID%>").val(ui.item.label);
                     $("#<%=hdnGSTGL.ClientID%>").val(ui.item.value);
                     return false;
                 },
                focus: function (event, ui) {
                    $("#<%=txtGSTGL.ClientID%>").val(ui.item.label);

                     return false;
                 },
                 minLength: 0,
                 delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                debugger;
                var ula = ul;
                var itema = item;
                var result_value = item.value;
                var result_item = item.label;
                var result_desc = item.acct;
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
                    .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                    .appendTo(ul);
                }
            };
           
        });
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">

        <div class="page-cont-top">
            <%--<ul class="page-breadcrumb">
                <li>
                    <i class="fa fa-home"></i>
                     <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                      <span>Program Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
              
                <li>
                    <span>Control Panel</span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Control Panel</asp:Label></li>
                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" TabIndex="24" ToolTip="Save"
                                OnClick="btnSubmit_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                TabIndex="25" OnClick="lnkClose_Click"></asp:LinkButton></li>                        
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="white-bg">
                    <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                    <div class="title_bar">
                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label>
                        <asp:Panel ID="pnlSave" runat="server" Style="float: left; margin-left: 44px; height: 10px;">
                            <div style="float: left;">
                            </div>
                        </asp:Panel>
                    </div>
                    <div>
                        <div class="col-lg-12 col-md-12">
                            <div class="row">
                                <div class="col-lg-4 col-md-4">
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <b>Company Info :</b>
                                            </div>
                                        </div>
                                    </div>
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
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCompany" runat="server" CssClass="form-control" MaxLength="75"
                                                    TabIndex="1"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Address
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" MaxLength="255"
                                                    TabIndex="3"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                City
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
                                                    Display="None" ErrorMessage="State Required" SetFocusOnError="True" InitialValue="State"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator7_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator7">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlState" runat="server" ToolTip="State"
                                                    CssClass="form-control" TabIndex="7">

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
                                                    TabIndex="9"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                   <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Country     
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlCountry" runat="server" ToolTip="Country"
                                                    CssClass="form-control" TabIndex="10" onchange="GetSelectedValue(this)">
                                                    <asp:ListItem Value="0">United States</asp:ListItem>
                                                    <asp:ListItem Value="1">Canada</asp:ListItem>
                                                    <asp:ListItem Value="2">United Kingdom</asp:ListItem>
                                                 </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group GST">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                 <asp:Label ID="lblGSTReg" runat="server" Text="GST Reg#"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtGSTReg" runat="server" CssClass="form-control" 
                                                TabIndex="11"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group GST">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                 <asp:Label ID="lblGSTRate" runat="server" Text="GST Rate %"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtGSTRate" runat="server" CssClass="form-control" 
                                                TabIndex="12" MaxLength="10"></asp:TextBox>
                                                 <asp:MaskedEditExtender ID="txtGSTRate_MaskedEditExtender" runat="server" Enabled="False"
                                                    Mask="9,999,999.99" TargetControlID="txtGSTRate" MaskType="Number" DisplayMoney="Left"
                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="">
                                                </asp:MaskedEditExtender>
                                                <asp:FilteredTextBoxExtender ID="txtGSTRate_FilteredTextBoxExtender" runat="server"
                                                    TargetControlID="txtGSTRate" ValidChars="0123456789." Enabled="True">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group GST">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblGSTGL" runat="server" Text="GST GL"></asp:Label>  
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtGSTGL" runat="server" CssClass="form-control" 
                                                TabIndex="13"></asp:TextBox>
                                                <asp:HiddenField ID="hdnGSTGL" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                
                                </div>
                                <div class="col-lg-4 col-md-4">
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <b>Contact Info :</b>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Contact Name
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtContName" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Telephone
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtTele" runat="server" CssClass="form-control" MaxLength="20"
                                                    TabIndex="4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Fax
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" MaxLength="20"
                                                    TabIndex="6"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Email<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                    ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="8"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtEmail">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Web Address
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtWebAdd" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="10"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-4 col-md-4">
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-label" style="text-align: left">
                                                <b>Company logo :</b>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="form-col">
                                            <div class="fc-input">
                                                <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/No_image.png" />
                                                <br />
                                                <br />
                                                <asp:FileUpload ID="FileUpload1" CssClass="form-control" runat="server" Height="30px" TabIndex="11" onchange="ConfirmUpload();" />
                                                <asp:Button ID="btnUpload" runat="server" CssClass="form-control" OnClick="btnUpload_Click" Text="Upload Logo"
                                                    ValidationGroup="img" TabIndex="23" Style="display: none;" />
                                                <asp:RegularExpressionValidator ID="revImage" runat="server" ControlToValidate="FileUpload1"
                                                    ValidationExpression="^.*\.((j|J)(p|P)(e|E)?(g|G)|(g|G)(i|I)(f|F)|(p|P)(n|N)(g|G)|(b|B)(m|M)(p|P))$"
                                                    ValidationGroup="img" Display="None" ErrorMessage=" ! Invalid image type. Only JPEG, GIF, PNG and BMP allowed."
                                                    SetFocusOnError="True" />
                                                <asp:ValidatorCalloutExtender ID="revImage_ValidatorCalloutExtender" runat="server"
                                                    Enabled="True" TargetControlID="revImage" PopupPosition="Left">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-8 col-md-8">
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Remarks
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtRemarks" runat="server" Height="66px" TextMode="MultiLine"
                                            CssClass="form-control" TabIndex="12"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-10 col-md-10">
                            <div class="col-lg-5 col-md-5">
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <b>Database Info :</b>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Database Type
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlDBType" runat="server" CssClass="form-control"
                                                TabIndex="13">
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
                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator2">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtDB" runat="server" CssClass="form-control"
                                                TabIndex="15"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <asp:Label ID="lblCustReg" runat="server" Text="Customer Registration"></asp:Label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkCustRegistrn" runat="server" TabIndex="14" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Multi language
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkMultilang" runat="server" TabIndex="17" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Mobile Service Email
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkMSEmail" runat="server" TabIndex="19" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <asp:Label ID="Label1" runat="server" Text="QB Account Integration"
                                                ToolTip="Sync data between MSM and Quickbooks" Visible="False"></asp:Label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkAcctIntegration" runat="server"
                                                ToolTip="Sync data between MSM and Quickbooks" OnCheckedChanged="chkAcctIntegration_CheckedChanged"
                                                TabIndex="15" Visible="False" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-5 col-md-5">
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <b>Quickbooks Sync:</b>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Sync Employee Only
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkSyncEmp" runat="server" TabIndex="14" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Mileage Service Item
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList runat="server" CssClass="form-control" TabIndex="16" Width="200px" ID="ddlService"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Labor Service Item
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList runat="server" CssClass="form-control"
                                                TabIndex="18" Width="200px" ID="ddlServicelabor">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Expense Service Item
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList runat="server" CssClass="form-control"
                                                TabIndex="20" Width="200px" ID="ddlServiceExpense">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <asp:Label ID="Label2" runat="server" Text="QB File Path (on server)"
                                                ToolTip="Give the path of Quickbooks data file located on the hosted server."
                                                Visible="False"></asp:Label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtFilePath" runat="server" MaxLength="500" TabIndex="17" Width="200px"
                                                ToolTip="Give the path of Quickbooks data file located on the hosted server."
                                                Enabled="False" CssClass="form-control" Visible="False"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-10 col-md-10">
                            <div class="col-lg-5 col-md-5">
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <b>General Info :</b>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Year-End Month
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlYearEnd" runat="server" CssClass="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
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
