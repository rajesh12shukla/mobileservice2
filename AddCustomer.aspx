<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddCustomer.aspx.cs" Inherits="AddCustomer" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--<link href="Appearance/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
     <script src="Appearance/js/bootstrap.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

        function onDdlBillingChange(val) {
            if (val == 1) {

                $("#divCentral").show();
                var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
                if ((countLocations - 1) <= 0) {

                    noty({
                        text: 'You cannot select the Combined Billing, As there are No Locations added yet.',
                        type: 'warning',
                        layout: 'topCenter',
                        closeOnSelfClick: false,
                        timeout: 5000,
                        theme: 'noty_theme_default',
                        closable: true
                    });
                }

            }
            else {
                $("#divCentral").hide();
            }
        }
        function ValidateSpecifyLocation() {
            //alert("heloo");
            var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
            if ((countLocations - 1) <= 0) {

                noty({
                    text: 'You cannot select the Combined Billing, As there are No Locations added yet.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
        }
        function AddNewCustomer() {
            $('#setuppopup').modal('show');
        }

        function pageLoad() {
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        }

        function showModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
            //            ev.preventDefault();
            //            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;;
            document.getElementById('<%= Panel2.ClientID %>').style.display = "none";
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function hideModalPopupViaClientCust(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
        }

        ////////////////// Confirm Document Upload ////////////////////
        function ConfirmUpload(value) {
            var filename;
            var fullPath = value;
            if (fullPath) {
                var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                filename = fullPath.substring(startIndex);
                if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                    filename = filename.substring(1);
                }
            }

            if (confirm('Upload ' + filename + '?'))
            { document.getElementById('<%= lnkUploadDoc.ClientID %>').click(); }
            else
            { document.getElementById('<%= lnkPostback.ClientID %>').click(); }
        }

        function checkdelete() {
            return SelectedRowDelete('<%= gvDocuments.ClientID %>', 'file');
        }

        function editticket() {
            $("#<%=gvOpenCalls.ClientID %>").find('tr:not(:first,:last)').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function (index, value) {
                    var ticket = $tr.find('span[id*=lblTicketId]').text();
                    var comp = $tr.find('span[id*=lblComp]').text();
                    var url = "addticket.aspx?id=" + ticket + "&comp=" + comp;
                    window.open(url, '_blank');
                });
            });
        }

        function AlertSageIDUpdate() {
            var hdnAcctID = document.getElementById('<%= hdnAcctID.ClientID %>');
            var txtAcctno = document.getElementById('<%= txtAcctno.ClientID %>');
            var hdnSageIntegration = document.getElementById('<%= hdnSageIntegration.ClientID %>');
            var ret = true;
            if (hdnSageIntegration.value == "1") {
                if ('<%=ViewState["mode"].ToString()  %>' == "1") {
                    if (hdnAcctID.value != txtAcctno.value) {
                        ret = confirm('Sage ID edited, this will create a new Customer in Sage and make existing inactive. Do you want to continue?');
                    }
                }
            }
            return ret;
        }

        function checkMaxLength(textarea, evt, maxLength) {
            if ($("#<%= hdnSageIntegration.ClientID %>").val() == "1") {
                var lines = textarea.value.split("\n");
                for (var i = 0; i < lines.length; i++) {
                    if (lines[i].length <= 30) continue;
                    var j = 0; space = 30;
                    while (j++ <= 30) {
                        if (lines[i].charAt(j) === " ") space = j;
                    }
                    lines[i + 1] = lines[i].substring(space + 1) + (lines[i + 1] || "");
                    lines[i] = lines[i].substring(0, space);
                }
                textarea.value = lines.slice(0, 4).join("\n");
                $("#spnAddress").fadeIn('slow', function () {
                    $(this).delay(500).fadeOut('slow');
                });
            }
        }

        //        function show_confirm() {
        //        var r = confirm("Would you like to add a location?");
        //        if (r == true) {
        //            windows.location.href = "addlocation.aspx";
        //            //            tb_show('', 'addlocation.aspx?o=1&keepThis=true&TB_iframe=true&height=500&width=980', null);            
        //            return callPostBack();
        //        }
        //        else {
        //            return callPostBack();
        //        }
        //        }

        //        function callPostBack() {
        //            if (1 == 2)
        //                return true;d
        //            else
        //                return false;
        //        }      


        //      OnClientClick = "javascript: if( Page_ClientValidate()){return show_confirm();}"
        //////////////// To make textbox value decimal ///////////////////////////
        function isDecimalKey(el, evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode;

            if (charCode == 45) {
                return true;
            }

            var number = el.value.split('.');
            if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }

            if (number.length > 1 && charCode == 46) {
                return false;
            }
            var caratPos = getSelectionStart(el);
            var dotPos = el.value.indexOf(".");
            if (caratPos > dotPos && dotPos > -1 && (number[1].length > 1)) {
                return false;
            }
            return true;
        }
        function getSelectionStart(o) {
            if (o.createTextRange) {
                var r = document.selection.createRange().duplicate()
                r.moveEnd('character', o.value.length)
                if (r.text == '') return o.value.length
                return o.value.lastIndexOf(r.text)
            } else return o.selectionStart
        }
        function ConvertDigit(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                //document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        $(document).ready(function () {
            var billing = $('#<%=ddlBilling.ClientID%>').val();
            if (billing == 1) {

                $("#divCentral").show();
            }
            <%-- $('#<%=txtAddress.ClientID%>').focus(function () {
                $(this).animate({
                    //right: "+=0",
                    width: '520px',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtAddress.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtAddress.ClientID%>').focus(function () {
                $(this).animate({
                    //right: "+=0",
                    width: '520px',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtAddress.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });--%>
        });
    </script>
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
                    <span>Customer Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/Customers.aspx") %>">Customers</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Customers</span>
                </li>
            </ul>--%>
            <div class="page-bar-right">
                <%--<a href="#" class="pbr-save tooltips" data-original-title="Save" data-placement="bottom"><i class="fa fa-check"></i></a>
                    <a href="#" class="pbr-close tooltips" data-original-title="Close" data-placement="bottom" ><i class="fa fa-times"></i></a>--%>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <%--<span><i class="fa fa-plus"></i> Title </span>--%>
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Customer</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblCustomerName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel Visible="true" ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                    <ul class="lnklist-header">
                                        <li style="margin: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CssClass="icon-first" CausesValidation="False"
                                                OnClick="lnkFirst_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CssClass="icon-previous" CausesValidation="False"
                                                OnClick="lnkPrevious_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CssClass="icon-next" CausesValidation="False"
                                                OnClick="lnkNext_Click">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False"
                                                OnClick="lnkLast_Click"></asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            <%--  <div style="float: left;">
                                     <asp:LinkButton CssClass="close_button_Form" ID="lnkClose" runat="server" CausesValidation="false"
                                        OnClick="lnkClose_Click" TabIndex="24">Close</asp:LinkButton>
                                     <asp:LinkButton CssClass="save_button" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                        TabIndex="23">Save</asp:LinkButton>
                                   </div>--%>
                            </asp:Panel>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnPrint" CssClass="icon-print" runat="server" ToolTip="Print" TabIndex="16" CausesValidation="true" 
                                OnClick="btnPrint_Click"> </asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" ToolTip="Save" ForeColor="Green" OnClick="btnSubmit_Click"
                                ValidationGroup="general, rep" OnClientClick="return AlertSageIDUpdate();" TabIndex="23"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ForeColor="Red" runat="server" ToolTip="Close" CausesValidation="false"
                                OnClick="lnkClose_Click" TabIndex="24"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <%--<div class="com-cont">--%>
                <asp:HiddenField ID="hdnMail" runat="server" />
                <asp:HiddenField ID="hdnSageIntegration" runat="server" />

                <div class="title_bar">
                </div>
                <div class="row">
                    <div class="col-lg-12 col-md-12">
                        <div class="com-cont">
                            <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                                <asp:TabPanel runat="server" ID="tpCustomer" HeaderText="Customer Info.">
                                    <HeaderTemplate>
                                        Customer Info.
                                    </HeaderTemplate>
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="alert alert-danger" runat="server" id="divLabelMessage" visible="False">
                                                <button type="button" class="close" data-dismiss="alert">×</button>
                                                You cannot select the Combined Billing, As there are No Locations added yet.
                                            </div>
                                            <div class="clearfix"></div>
                                            <div class="col-sm-6 col-md-6">
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        <asp:Label ID="lblSageID" runat="server" Text="Sage ID"></asp:Label></td>
                                                    </div>
                                                    <div class="fc-input merchant-input">
                                                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                                            <ContentTemplate>
                                                                <asp:TextBox ID="txtAcctno" runat="server" CssClass="form-control" MaxLength="15"
                                                                    TabIndex="1" Width="200px"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnAcctID" runat="server" />
                                                                <asp:Button ID="btnSageID" runat="server" Text="Check" OnClick="btnSageID_Click"
                                                                    CausesValidation="false" />
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        <asp:Label ID="Label4" Visible="False" runat="server" Text="ID"></asp:Label></td>
                                                    </div>
                                                    <div class="fc-label">
                                                        <asp:Label ID="Label5" Visible="False" runat="server"></asp:Label></td>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Customer Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                            ControlToValidate="txtCName" Display="None" ErrorMessage="First Name Required" SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator>
                                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server"
                                                            Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtCName" runat="server" TabIndex="1" CssClass="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Customer Address
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server"
                                                        ControlToValidate="txtAddress" Display="None" ErrorMessage="Address Required"
                                                        SetFocusOnError="True" ValidationGroup="general, rep"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                            ID="RequiredFieldValidator11_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator11">
                                                        </asp:ValidatorCalloutExtender>
                                                        <span id="spnAddress" style="color: red; display: none;">Max 4 lines 30 characters each</span>
                                                    </div>
                                                    <div class="fc-input">


                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control resize"
                                                            ONKEYUP="return checkMaxLength(this, event, 35)"
                                                            TabIndex="3" TextMode="MultiLine"></asp:TextBox>

                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        City<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server"
                                                            ControlToValidate="txtCity" ValidationGroup="general, rep"
                                                            Display="None" ErrorMessage="City Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator6_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator6">
                                                            </asp:ValidatorCalloutExtender>
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" TabIndex="7"
                                                            MaxLength="50"></asp:TextBox>
                                                        <asp:Label ID="lblMsg" runat="server" CssClass="lblMsg" ForeColor="#CC0000"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        State/Prov.

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                            ControlToValidate="ddlState" ValidationGroup="general, rep" InitialValue="State" Display="None" ErrorMessage="State Required"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                ID="RequiredFieldValidator7_ValidatorCalloutExtender"
                                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator7">
                                            </asp:ValidatorCalloutExtender>
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:DropDownList ID="ddlState" runat="server" ToolTip="State" CssClass="form-control"
                                                            Width="200px" TabIndex="8">
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
                                                    <div class="fc-label">
                                                        Zip/Postal Code
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" TabIndex="9"
                                                            MaxLength="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Main Contact&nbsp;
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtMaincontact" runat="server" CssClass="form-control"
                                                            MaxLength="50" TabIndex="10"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Phone
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtPhoneCust" runat="server" CssClass="form-control"
                                                            MaxLength="28" TabIndex="11"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Website
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"
                                                            MaxLength="50" TabIndex="12"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Email
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                    ControlToValidate="txtEmail" Display="None" ErrorMessage="Invalid Email"
                                    SetFocusOnError="True"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                        </asp:ValidatorCalloutExtender>
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                                            MaxLength="50" TabIndex="13"></asp:TextBox>
                                                        <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender"
                                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                            TargetControlID="txtEmail">
                                                        </asp:FilteredTextBoxExtender>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Cellular
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtCell" runat="server" CssClass="form-control"
                                                            TabIndex="14"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-sm-3 col-md-3">
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Type
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlUserType" runat="server" CssClass="form-control" TabIndex="2"
                                                                Width="200px">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Status
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlCustStatus" runat="server"
                                                                CssClass="form-control" TabIndex="4" Width="200px">
                                                                <asp:ListItem Value="0">Active</asp:ListItem>
                                                                <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Billing
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlBilling" runat="server" onChange="onDdlBillingChange(this.options[this.selectedIndex].value);"
                                                                CssClass="form-control" TabIndex="4" Width="200px">
                                                                <asp:ListItem Value="0">Individual</asp:ListItem>
                                                                <asp:ListItem Value="1">Combined</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div id="divCentral" class="form-group" style="display: none;">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Specify Location
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlSpecifiedLocation" runat="server"
                                                                CssClass="form-control" TabIndex="4" Width="200px">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Internet
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:CheckBox ID="chkInternet" runat="server" AutoPostBack="True"
                                                                OnCheckedChanged="chkInternet_CheckedChanged" TabIndex="6" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlInternet" runat="server" Visible="False">
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Username
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                                                            ControlToValidate="txtUserName" Display="None" Enabled="False"
                                                            ErrorMessage="Username Required"
                                                            SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator3_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left"
                                                                TargetControlID="RequiredFieldValidator3">
                                                            </asp:ValidatorCalloutExtender>

                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control"
                                                                            MaxLength="15" TabIndex="15"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <td class="register_lbl">
                                                                <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            Password
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                                                        ControlToValidate="txtPassword" Display="None" Enabled="False"
                                                        ErrorMessage="Password Required"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator4_ValidatorCalloutExtender"
                                                            runat="server" Enabled="True" PopupPosition="Left"
                                                            TargetControlID="RequiredFieldValidator4">
                                                        </asp:ValidatorCalloutExtender>
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:TextBox ID="txtPassword" runat="server"
                                                                                CssClass="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            View Service History
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkMap" runat="server" TabIndex="17" />
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            View Invoices
                                                                        </div>

                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkScheduleBrd" runat="server" TabIndex="18" />
                                                                        </div>
                                                                    </div>
                                                                </div>


                                                                <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            View Equipment
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkEquipments" runat="server" TabIndex="18" />
                                                                        </div>
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

                                                               

                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="chkInternet" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                            <div class="col-sm-3 col-md-3">
                                                 <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Billing Rate
                                                        </div>
                                                        <div class="fc-input">
                                                           <asp:TextBox ID="txtBillRate" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            OT Rate
                                                        </div>
                                                        <div class="fc-input">
                                                           <asp:TextBox ID="txtOt" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            1.7 Rate
                                                        </div>
                                                        <div class="fc-input">
                                                          <asp:TextBox ID="txtNt" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                 <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            DT Rate
                                                        </div>
                                                        <div class="fc-input">
                                                           <asp:TextBox ID="txtDt" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Travel Rate
                                                        </div>
                                                        <div class="fc-input">
                                                           <asp:TextBox ID="txtTravel" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="form-col">
                                                        <div class="fc-label">
                                                            Mileage
                                                        </div>
                                                        <div class="fc-input">
                                                           <asp:TextBox ID="txtMileage" runat="server" CssClass="form-control"
                                                            MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="clearfix"></div>
                                            <div class="col-sm-6 col-md-6">
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Remarks 
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtRemarks" runat="server"
                                                            CssClass="form-control resize"
                                                            TabIndex="15" TextMode="MultiLine"></asp:TextBox>
                                                    </div>
                                                </div>

                                                 <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            Shutdown Alert
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:CheckBox ID="chkShutdownAlert" runat="server" TabIndex="18" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="form-col">
                                                                        <div class="fc-label">
                                                                            Shutdown Alert Message
                                                                        </div>
                                                                        <div class="fc-input">
                                                                            <asp:TextBox CssClass="form-control resize" TextMode="MultiLine" ID="txtAlert" runat="server"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>


                                            </div>
                                        </div>
                                        <div class="table-scrollable" style="border: none">
                                            <table>
                                                <tr>
                                                    <td class="register_lbl" valign="top">&nbsp;</td>
                                                    <td colspan="5">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <div class="table-scrollable">
                                                            <asp:Panel ID="pnlContactsGrid" runat="server">
                                                                <div style="background-color: #316b9d">
                                                                    <ul class="lnklist-header lnklist-panel">
                                                                        <li>
                                                                            <asp:Label CssClass="title_text" ID="Label2" runat="server">Contacts</asp:Label></li>
                                                                        <li>
                                                                            <asp:LinkButton CssClass="icon-addnew" ID="lnkAddnew" ToolTip="Add New"
                                                                                runat="server" CausesValidation="False" OnClick="lnkAddnew_Click" TabIndex="20"></asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton CssClass="icon-edit" ToolTip="Edit"
                                                                                ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False"
                                                                                TabIndex="22"></asp:LinkButton></li>
                                                                        <li>
                                                                            <asp:LinkButton CssClass="icon-delete" ID="btnDelete" ToolTip="Delete"
                                                                                runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                                                                TabIndex="21"></asp:LinkButton></li>
                                                                        <li><a id="lnkMail" class="icon-mail" runat="server" title="Send Mail" tooltip="Send Mail"></a></li>
                                                                    </ul>
                                                                </div>
                                                                <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                    Width="650px" OnRowDataBound="gvContacts_RowDataBound" OnRowCreated="gvContacts_RowCreated">
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
                                                                                <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="ID" Visible="False">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblId" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
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
                                                                                <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="Shutdown Alert" >
                                                                            <ItemTemplate>
                                                                                <asp:CheckBox ID="chkShutdown" runat="server" />
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <RowStyle CssClass="evenrowcolor" />
                                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="6">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <asp:Panel ID="pnlDoc" runat="server" Visible="False">
                                                                        <asp:Panel ID="pnlDocumentButtons" runat="server" Style="background: #316b9d;">
                                                                            <ul class="lnklist-header lnklist-panel">
                                                                                <li>
                                                                                    <asp:Label CssClass="title_text" ID="Label3" runat="server" Style="float: left">Documents</asp:Label></li>
                                                                                <li>
                                                                                    <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" CssClass="icon-delete"
                                                                                        OnClick="lnkDeleteDoc_Click"
                                                                                        OnClientClick="return checkdelete();"></asp:LinkButton>
                                                                                </li>
                                                                                <li>
                                                                                    <table style="float: left; margin-right: 20px; margin-left: 130px; margin-top: 0"
                                                                                        cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:FileUpload ID="FileUpload1" runat="server"
                                                                                                    onchange="ConfirmUpload(this.value);" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                                                                    CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                                                    Style="display: none">Upload</asp:LinkButton><asp:LinkButton
                                                                                                        ID="lnkPostback" runat="server"
                                                                                                        CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </li>
                                                                            </ul>

                                                                        </asp:Panel>
                                                                        <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False"
                                                                            CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                            Width="100%">
                                                                            <RowStyle CssClass="evenrowcolor" />
                                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="ID" Visible="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="File Name">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lblName" runat="server" CausesValidation="false"
                                                                                            CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                                            OnClick="lblName_Click" Text='<%# Eval("filename") %>'> 
                                                                                        </asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="File Type">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("doctype") %>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Portal">
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkPortal" runat="server" Checked='<%# (Eval("portal")!=DBNull.Value) ? Convert.ToBoolean(Eval("portal")): false %>' />
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Remarks">
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtremarks" Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                        </asp:GridView>

                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tpViewlocations" runat="server" HeaderText="View Locations">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-scrollable">
                                                    <asp:Panel runat="server" ID="Panel3" Style="background: #316b9d">
                                                        <ul class="lnklist-header lnklist-panel">
                                                            <li>
                                                                <asp:LinkButton ID="lnkEditLoc" runat="server" OnClick="lnkEditLoc_Click" CssClass="icon-edit" CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkCopyloc" CssClass="icon-copy"
                                                                    runat="server" OnClick="lnkCopyloc_Click" CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton CssClass="icon-delete" ID="lnkDeleteLoc"
                                                                    runat="server" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer1_tpViewlocations_gvLoc','location');"
                                                                    OnClick="lnkDeleteLoc_Click" CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkAddLoc" CssClass="icon-addnew"
                                                                    runat="server" OnClick="lnkAddLoc_Click" CausesValidation="False"></asp:LinkButton></li>
                                                        </ul>
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvLoc" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="100%" AllowPaging="True" AllowSorting="True" OnSorting="gvLoc_Sorting"
                                                        PageSize="10" OnDataBound="gvLoc_DataBound" ShowFooter="True" OnRowDataBound="gvLoc_RowDataBound"
                                                        DataKeyNames="loc" OnRowCommand="gvLoc_RowCommand">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblloc" runat="server" Text='<%# Bind("loc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Acct #" SortExpression="locid">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblId" runat="server" Text='<%# Bind("locid") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                </FooterTemplate>
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
                                                                    <asp:Label ID="lblCell" runat="server"><%#Eval("city")%></asp:Label>
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
                                                            <asp:TemplateField HeaderText="# of Equip" SortExpression="Elevs">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblelev" runat="server"><%#Eval("Elevs")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblequipTotal" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Balance" SortExpression="balance">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBalance" runat="server"><%#Eval("balance")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblBalanceTotal" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <PagerTemplate>
                                                            <div align="center">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First"
                                                                    CausesValidation="false" ImageUrl="images/first.png" />
                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                                    CausesValidation="false"
                                                                    ImageUrl="~/images/Backward.png" />
                                                                &nbsp &nbsp <span>Page</span>
                                                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span>of </span>
                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next"
                                        ImageUrl="images/Forward.png" />
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last"
                                        ImageUrl="images/last.png" />
                                                            </div>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tpViewEquipment" runat="server" HeaderText="View Equipment">
                                    <ContentTemplate>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-scrollable">
                                                    <asp:Panel runat="server" ID="Panel4" Style="background: #316b9d; width: 100%">
                                                        <ul class="lnklist-header lnklist-panel">
                                                            <li>
                                                                <asp:LinkButton
                                                                    ID="lnkEditEquip" runat="server" OnClick="lnkEditEquip_Click" CssClass="icon-edit"
                                                                    CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkcopyEquip" CssClass="icon-copy"
                                                                    runat="server" OnClick="lnkcopyEquip_Click" CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkDeleteEquip" CssClass="icon-delete"
                                                                    runat="server" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer1_tpViewEquipment_gvEquip','Equipment');"
                                                                    OnClick="lnkDeleteEquip_Click" CausesValidation="False"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton ID="lnkAddEquip" CssClass="icon-addnew"
                                                                    runat="server" OnClick="lnkAddEquip_Click" CausesValidation="False"></asp:LinkButton></li>
                                                        </ul>
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        DataKeyNames="ID" Width="100%" AllowSorting="True" AllowPaging="true" PageSize="10"
                                                        ShowFooter="True" OnSorting="gvEquip_Sorting" OnDataBound="gvEquip_DataBound"
                                                        OnRowCommand="gvEquip_RowCommand">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" SortExpression="id" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Name" SortExpression="unit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Manuf." SortExpression="manuf">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmanuf" runat="server" Text='<%# Bind("manuf") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server"><%#Eval("Type")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Service type" SortExpression="cat">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location ID" SortExpression="locid">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocid" runat="server"><%#Eval("LocID")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location" SortExpression="tag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocName" runat="server"><%#Eval("tag")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Address" SortExpression="Address">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrice" runat="server"><%#Eval("Price")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalPrice" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Last Service" SortExpression="last">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbllast" runat="server"><%# Eval("last")!=DBNull.Value? String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "last"))):""%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Installed" SortExpression="since">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSince" runat="server"><%# Eval("since") != DBNull.Value ? String.Format("{0:M/d/yyyy}", Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "since"))) : ""%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <PagerTemplate>
                                                            <div align="center">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false"
                                                                    CommandArgument="First" ImageUrl="images/first.png" />
                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" CausesValidation="false" runat="server"
                                                                    CommandArgument="Prev"
                                                                    ImageUrl="~/images/Backward.png" />
                                                                &nbsp &nbsp <span>Page</span>
                                                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlPagesEquip_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span>of </span>
                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next"
                                        ImageUrl="images/Forward.png" />
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last"
                                        ImageUrl="images/last.png" />
                                                            </div>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tpViewServiceHistory" runat="server" HeaderText="View Service History">
                                    <ContentTemplate>
                                        <div class="search-customer">
                                            <div class="sc-form">
                                                <label>Search for tickets where:</label>
                                                <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True"
                                                    CssClass="form-control input-sm input-small"
                                                    OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
                                                    <asp:ListItem Value="l.tag">Location</asp:ListItem>
                                                    <asp:ListItem Value="t.ID">Ticket #</asp:ListItem>
                                                    <asp:ListItem Value="t.ldesc4">Address</asp:ListItem>
                                                    <asp:ListItem Value="t.cat">Category</asp:ListItem>
                                                    <asp:ListItem Value="t.WorkOrder">WO #</asp:ListItem>
                                                    <asp:ListItem Value="e.unit">Equipment ID</asp:ListItem>
                                                    <asp:ListItem Value="t.fdesc">Reason for service</asp:ListItem>
                                                    <asp:ListItem Value="t.descres">Work Comp Desc</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small"
                                                    Width="183px"></asp:TextBox>
                                                <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control input-sm input-small"
                                                    TabIndex="11" Visible="False" Width="200px">
                                                </asp:DropDownList>
                                                <%--<asp:ImageButton ID="btnSearch" runat="server" CausesValidation="False"
                                                    ImageUrl="images/search.png"
                                                    OnClick="btnSearch_Click" TabIndex="23" ToolTip="Search" />--%>
                                            </div>
                                        </div>
                                        <div class="search-customer" style="padding: 10px 0 10px 0">
                                            <div class="sc-form">
                                                <label>Status</label>
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control input-sm input-small"
                                                    TabIndex="14" Width="200px">
                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                    <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                    <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                    <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                    <asp:ListItem Value="4">Completed</asp:ListItem>
                                                    <asp:ListItem Value="5">Hold</asp:ListItem>
                                                </asp:DropDownList>
                                                <label>From</label>
                                                <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control input-sm input-small"
                                                    MaxLength="50"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtfromDate">
                                                </asp:CalendarExtender>
                                                <label>To</label>
                                                <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm input-small" MaxLength="50"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtToDate">
                                                </asp:CalendarExtender>
                                                <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click" TabIndex="23" ToolTip="Search" CssClass="btn submit" Style="margin-left: 20px;"><i class="fa fa-search"></i></asp:LinkButton>

                                                <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click" CssClass="btn submit">
                                                <i class="fa fa-print"></i>
                                                </asp:LinkButton>
                                                <asp:Label ID="lblRecordCount0" runat="server" Style="font-style: italic;"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>

                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div class="table-scrollable">
                                                    <asp:Panel runat="server" ID="pnlGridButtons" Style="background: #316b9d; width: 100%">
                                                        <ul class="lnklist-header lnklist-panel">
                                                            <li>
                                                                <asp:LinkButton ID="lnkEditTicket" runat="server" OnClientClick="editticket(); return false;" CssClass="icon-edit"
                                                                    OnClick="lnkEditTicket_Click"></asp:LinkButton>
                                                            </li>
                                                        </ul>

                                                    </asp:Panel>
                                                    <asp:GridView ID="gvOpenCalls" runat="server" AutoGenerateColumns="False" Width="100%"
                                                        PageSize="10" AllowPaging="true" ShowFooter="True" CssClass="table table-bordered table-striped table-condensed flip-content" AllowSorting="True"
                                                        OnSorting="gvOpenCalls_Sorting" OnDataBound="gvOpenCalls_DataBound"
                                                        OnRowCommand="gvOpenCalls_RowCommand" OnRowCreated="gvOpenCalls_RowCreated">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    <asp:Label ID="lblComp" Visible="true" Style="display: none" runat="server" Text='<%# Bind("Comp") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ticket #" SortExpression="id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="WO #" SortExpression="WorkOrder">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblWO" runat="server" Text='<%# Bind("WorkOrder") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Equip" SortExpression="Unit">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEquip" runat="server" Text='<%# Bind("Unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Assigned to" SortExpression="dwork">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                    <asp:Label ID="lblRes" runat="server" CssClass="newClassTooltip"
                                                                        Text='<%# ShowHoverText(Eval("description"),Eval("fdescreason")) %>'></asp:Label>
                                                                    <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="-60" OffsetX="320" PopupControlID="lblRes"
                                                                        TargetControlID="lblAssdTo" HoverDelay="250">
                                                                    </asp:HoverMenuExtender>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category" SortExpression="cat">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("cat") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="assignname">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("assignname") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Schedule Date" SortExpression="edate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="Label8" runat="server" Text='<%# Eval("edate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Call Date" SortExpression="cdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCdate" runat="server" Text='<%# Eval("cdate") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Time" SortExpression="Tottime">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTot" runat="server" Text='<%# Eval("Tottime") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle CssClass="footer" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <PagerTemplate>
                                                            <div align="center">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First"
                                                                    CausesValidation="false" ImageUrl="images/first.png" />
                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="false"
                                                                    CommandArgument="Prev"
                                                                    ImageUrl="~/images/Backward.png" />
                                                                &nbsp &nbsp <span>Page</span>
                                                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlPagesOpenCall_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span>of </span>
                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" CausesValidation="false"
                                        ImageUrl="images/Forward.png" />
                                                                &nbsp &nbsp
                                    <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" CausesValidation="false"
                                        ImageUrl="images/last.png" />
                                                            </div>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </ContentTemplate>
                                </asp:TabPanel>
                                <asp:TabPanel ID="tpViewInvoicelinks" runat="server" HeaderText="Transactions">
                                    <ContentTemplate>
                                        <div class="search-customer">
                                            <div class="sc-form">
                                                Select for Invoices where
                                                <asp:DropDownList ID="ddlSearchInv" runat="server" AutoPostBack="True"
                                                    CssClass="form-control input-sm input-small"
                                                    OnSelectedIndexChanged="ddlSearchInv_SelectedIndexChanged">
                                                    <asp:ListItem Value=" ">Select</asp:ListItem>
                                                    <asp:ListItem Value="i.ref">Invoice#</asp:ListItem>
                                                    <asp:ListItem Value="i.fdate">Invoice Date</asp:ListItem>
                                                    <asp:ListItem Value="l.ID">Location ID</asp:ListItem>
                                                    <asp:ListItem Value="l.loc">Location</asp:ListItem>
                                                    <asp:ListItem Value="i.Status">Status</asp:ListItem>
                                                    <asp:ListItem Value="i.Type">Department</asp:ListItem>
                                                    <asp:ListItem Value="i.PO">PO</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control input-sm input-small"
                                                    Width="200px" TabIndex="13" Visible="False">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlStatusInv" runat="server" CssClass="form-control input-sm input-small"
                                                    Width="200px" TabIndex="13" Visible="False">
                                                    <asp:ListItem Value="0">Open</asp:ListItem>
                                                    <asp:ListItem Value="1">Paid</asp:ListItem>
                                                    <asp:ListItem Value="2">Voided</asp:ListItem>
                                                    <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                                    <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddllocation" runat="server" CssClass="form-control input-sm input-small"
                                                    Visible="False">
                                                </asp:DropDownList>
                                                <asp:TextBox ID="txtInvDt" runat="server" CssClass="form-control input-sm input-small"
                                                    Visible="False"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtInvDt_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtInvDt">
                                                </asp:CalendarExtender>
                                                <asp:TextBox ID="txtSearchInv" runat="server"
                                                    CssClass="form-control input-sm input-small"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="search-customer" style="padding: 10px 0 10px 0">
                                            <div class="sc-form">
                                                Date From
                                                <asp:TextBox ID="txtInvDtFrom" runat="server" CssClass="form-control input-sm input-small"
                                                    MaxLength="50" Width="80px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                    TargetControlID="txtInvDtFrom">
                                                </asp:CalendarExtender>
                                                Date To
                                                <asp:TextBox ID="txtInvDtTo" runat="server" CssClass="form-control input-sm input-small"
                                                    MaxLength="50" Width="80px"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                    TargetControlID="txtInvDtTo">
                                                </asp:CalendarExtender>
                                                <%-- <asp:ImageButton ID="lnkSearch" runat="server" ImageUrl="images/search.png"
                                                    OnClick="lnkSearch_Click" ToolTip="Search" CausesValidation="False"></asp:ImageButton>--%>
                                                <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false" ToolTip="Search"
                                                    OnClick="lnkSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                <ul style="padding: 0 5px 0px 5px">
                                                    <li>
                                                        <asp:LinkButton ID="lnkClear" runat="server"
                                                            OnClick="lnkClear_Click" CausesValidation="False">Clear</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click"
                                                            CausesValidation="False">Show All Invoices</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
                                                    </li>
                                                </ul>
                                            </div>

                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="table-scrollable">
                                            <asp:Panel runat="server" ID="Panel5" Style="background: #316b9d; width: 100%;">
                                                <ul class="lnklist-header lnklist-panel">
                                                    <li>
                                                        <asp:LinkButton ID="lnkEditInvoice" runat="server" CausesValidation="False" CssClass="icon-edit" ToolTip="Edit"
                                                            OnClick="lnkEditInvoice_Click"></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkDeleteInvoice" CssClass="icon-delete" ToolTip="Delete"
                                                            runat="server" CausesValidation="False" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer1_tpViewInvoicelinks_gvInvoice','Invoice');"
                                                            OnClick="lnkDeleteInvoice_Click"></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkAddInvoice" CausesValidation="False" runat="server" CssClass="icon-addnew" ToolTip="Add New"
                                                            OnClick="lnkAddInvoice_Click"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lnkCopyInvoice" CssClass="icon-copy" ToolTip="Copy"
                                                            runat="server" Visible="False" CausesValidation="False"
                                                            OnClick="lnkCopyInvoice_Click"></asp:LinkButton></li>
                                                </ul>
                                            </asp:Panel>
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False"
                                                        CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        DataKeyNames="ID" Width="100%" PageSize="10" ShowFooter="True"
                                                        OnSorting="gvInvoice_Sorting"
                                                        AllowPaging="True" AllowSorting="True" OnDataBound="gvInvoice_DataBound"
                                                        OnRowCommand="gvInvoice_RowCommand">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ID" SortExpression="ref" Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice #" SortExpression="ref">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInv" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice date" SortExpression="fdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvDate" runat="server" Text='<%# String.Format("{0:M/d/yyyy}", Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "fDate"))) %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location ID" SortExpression="id">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocID" runat="server" Text='<%#Eval("locid")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Location" SortExpression="tag">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("tag")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%#Eval("fdesc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="amount" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'
                                                                    ForeColor='<%# Convert.ToDouble(Eval("amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server" Text='<%#Eval("status")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server" Text='<%#Eval("Type")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <PagerTemplate>
                                                            <div align="center">
                                                                <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="false"
                                                                    CommandArgument="First" ImageUrl="images/first.png" />
                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" CausesValidation="false"
                                                                    runat="server" CommandArgument="Prev"
                                                                    ImageUrl="~/images/Backward.png" />
                                                                &nbsp &nbsp <span>Page</span>
                                                                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True"
                                                                    OnSelectedIndexChanged="ddlPagesInvoice_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span>of </span>
                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                &nbsp &nbsp
                                                    <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next"
                                                        ImageUrl="images/Forward.png" />
                                                                &nbsp &nbsp
                                                    <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last"
                                                        ImageUrl="images/last.png" />
                                                            </div>
                                                        </PagerTemplate>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>


                                    </ContentTemplate>
                                </asp:TabPanel>
                            </asp:TabContainer>


                            <asp:Panel runat="server" ID="pnlOverlay" Visible="false">
                                <asp:Panel ID="pnlContact0" runat="server" Visible="false">
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="pnlContact" runat="server" CssClass="pnlUpdate" Visible="false">
                                <div class="model-popup-body" style="padding: 0">
                                    <ul class="lnklist-header lnklist-panel">
                                        <li>
                                            <asp:Label CssClass="title_text" ID="Label1" runat="server">Add Contact</asp:Label></li>
                                        <li>
                                            <asp:LinkButton ID="lnkContactSave" runat="server" OnClick="lnkContactSave_Click" CssClass="icon-save" ValidationGroup="cont" ToolTip="Save"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkCancelContact" runat="server" CausesValidation="False" OnClick="LinkButton2_Click" ToolTip="Close" CssClass="icon-closed"></asp:LinkButton></li>
                                    </ul>
                                </div>
                                <div class="form-col" style="padding-top: 5px">
                                    <div class="fc-label">
                                        Contact Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator12">
                                        </asp:ValidatorCalloutExtender>
                                    </div>

                                    <div class="fc-input">
                                        <asp:TextBox ID="txtContcName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-col">
                                    <div class="fc-label">
                                        Phone
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtContPhone"
                        Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator13">
                                        </asp:ValidatorCalloutExtender>

                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtContPhone" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                    </div>
                                </div>


                                <div class="form-col">
                                    <div class="fc-label">
                                        Fax
                                    </div>

                                    <div class="fc-input">
                                        <asp:TextBox ID="txtContFax" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-col">
                                    <div class="fc-label">
                                        Cell
                                    </div>

                                    <div class="fc-input">
                                        <asp:TextBox ID="txtContCell" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                    </div>
                                </div>

                                <div class="form-col">
                                    <div class="fc-label">
                                        Email
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtContEmail"
                                                        Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="RequiredFieldValidator16_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator16">
                                        </asp:ValidatorCalloutExtender>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                            ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                        </asp:ValidatorCalloutExtender>
                                    </div>

                                    <div class="fc-input">
                                        <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtContEmail_FilteredTextBoxExtender"
                                            runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                            TargetControlID="txtContEmail">
                                        </asp:FilteredTextBoxExtender>
                                    </div>
                                     
                                </div>

                                 <div class="form-col">
                                    <div class="fc-label">
                                        Shutdown Alert
                                    </div>

                                    <div class="fc-input">
                                        <asp:CheckBox ID="chkShutdownA" runat="server" />
                                    </div>
                                </div>

                            </asp:Panel>
                            <div class="clearfix"></div>
                            <!-- /.modal-content -->


                            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                                CausesValidation="False" />
                            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                                PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                                DropShadow="false" RepositionMode="RepositionOnWindowResizeAndScroll">
                            </asp:ModalPopupExtender>
                            <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: 1px solid #316b9d; width: 350px;">
                                <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                                    <div class="model-popup-body" style="padding-bottom: 25px">
                                        <a id="hideModalPopupViaClientButton" href="#" style="float: right; color: #fff; height: 16px;">Close</a>
                                        <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Ok" OnClick="hideModalPopupViaServerConfirm_Click"
                                            CausesValidation="False" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px;" />
                                    </div>
                                </asp:Panel>
                                <div style="padding: 20px;">
                                    <span class="lblMsg">Customer saved successfully !</span>
                                    <br />
                                    <br />
                                    <strong>Do you want to add location for the saved customer?</strong>
                                </div>
                            </asp:Panel>
                            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
                                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
                                RepositionMode="RepositionOnWindowResizeAndScroll">
                            </asp:ModalPopupExtender>
                            <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;">
                                <asp:Panel runat="Server" ID="Panel2" Style="background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                                    <div class="title_bar_popup">
                                        <a id="A1" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                    </div>
                                </asp:Panel>
                                <div>
                                    <iframe id="iframeCustomer" runat="server" width="1040px" height="620px" frameborder="0"></iframe>
                                </div>
                            </asp:Panel>
                            <asp:Button runat="server" ID="hideModalPopupViaServer" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; display: none;"
                                Text="Close" OnClick="hideModalPopupViaServer_Click"
                                CausesValidation="false" />
                        </div>
                    </div>
                </div>
                <!-- edit-tab end -->
                <div class="clearfix"></div>

                <%--                </div>--%>
                <!-- END DASHBOARD STATS -->
                <div class="clearfix"></div>
            </div>
        </div>
    </div>
</asp:Content>
