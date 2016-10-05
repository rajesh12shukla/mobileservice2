<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddLocation.aspx.cs" Inherits="AddLocation" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="http://maps.googleapis.com/maps/api/js?sensor=false&amp;libraries=places&key=AIzaSyDedmuEC-1d__Jc1M3OLx1NIAnc_gMRbhE"></script>
    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <script type="text/javascript">

        $(function () {
            $("#txtGoogleAutoc").geocomplete({
                map: false,
                details: "#divmain",
                types: ["geocode", "establishment"],
                address: "#<%= txtAddress.ClientID %>",
                city: "#<%= txtCity.ClientID %>",
                state: "#<%= ddlState.ClientID %>",
                zip: "#<%= txtZip.ClientID %>",
                lat: "#<%= lat.ClientID %>",
                lng: "#<%= lng.ClientID %>"
            });
            initialize();
            $("#<%= txtAddress.ClientID %>").bind('input propertychange', function () {
                $("#txtGoogleAutoc").val('');
            });
            $("#txtGoogleAutoc").on("blur", function () {
                $("#txtGoogleAutoc").val('');
            });
        });

        //        $(function() {
        //            //            var addresspicker = $('#<%= txtAddress.ClientID %>').addresspicker();

        //            var addresspickerMap = $('#<%= txtAddress.ClientID %>').addresspicker({
        //                elements: {
        //                    map: "#map",
        //                    lat: "#ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_lat",
        //                    lng: "#ctl00_ContentPlaceHolder1_TabContainer1_tpLocation_lng",
        //                    locality: '#locality',
        //                    country: '#country'
        //                }
        //            });

        //            var gmarker = addresspickerMap.addresspicker("marker");

        //            gmarker.setVisible(true);
        //            addresspickerMap.addresspicker("updatePosition");

        //        });

        function pageLoad() {
            $addHandler($get("showModalPopupClientButton"), 'click', showModalPopupViaClient);
            $addHandler($get("A1"), 'click', hideModalPopupViaClientCust);
        }

        function hideModalPopupViaClientCust(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.hide();
        }

        function showModalPopupViaClientCust(lblTicketId, lblComp) {
            //            ev.preventDefault();
            //            document.getElementById('<%= iframeCustomer.ClientID %>').width = "1024px";
            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;;
            document.getElementById('<%= Panel2.ClientID %>').style.display = "none";
            var modalPopupBehavior = $find('PMPBehaviour');
            modalPopupBehavior.show();
        }


        function showModalPopupViaClient(ev) {
            ev.preventDefault();
            document.getElementById('<%= iframe.ClientID %>').src = "AddCustomer.aspx?o=1";

            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }
        function ace_itemSelected(sender, e) {

            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            hdnPatientId.value = e.get_value();
            document.getElementById('<%= ddlCustomer.ClientID %>').value = hdnPatientId.value;
            document.getElementById('<%= btnSelectCustomer.ClientID %>').click();
        }
        function dispWarningContract() {

            noty({
                text: 'You cannot select the \'Combined on One Invoice\', As there are No Contracts added yet.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
        }
        $(document).ready(function () {
            $('#<%= txtCustomer.ClientID %>').keyup(function (event) {
                var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
                hdnPatientId.value = '';
            });

            $('#<%= txtAddress.ClientID %>').keyup(function (ev) {
                //                if (event.which != 27 && event.which != 37 && event.which != 38 && event.which != 39 && event.which != 40 && event.which != 13) {
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    var txtLat = document.getElementById('<%= lat.ClientID %>');
                    var txtLng = document.getElementById('<%= lng.ClientID %>');
                    txtLat.value = '';
                    txtLng.value = '';
                }
            });

            $("#mapLink").click(function () {
                $("#map").toggle();
                initialize();
            });

            //            $("#ctl00_ContentPlaceHolder1_txtCustomer").blur(function(event) {
            //                var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
            //                if (hdnPatientId.value == '') {
            //                    alert("Please select the existing customer.");
            //                }
            //            });

            <%--if ($(window).width() > 767) {
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
                });
            }--%>

        });

        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('<%= hdnPatientId.ClientID %>');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }

        function ChkAddress() {
            var txtBillAdd = document.getElementById('<%= txtBillAdd.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');

            var txtAddress = document.getElementById('<%= txtAddress.ClientID %>');
            var txtCity = document.getElementById('<%= txtCity.ClientID %>');
            var ddlState = document.getElementById('<%= ddlState.ClientID %>');
            var txtZip = document.getElementById('<%= txtZip.ClientID %>');

            var chkAdd = document.getElementById('<%= chkAddress.ClientID %>');

            if (chkAdd.checked == true) {
                txtBillAdd.value = txtAddress.value;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;
                txtBillZip.value = txtZip.value;
            }
        }
        function ChkCustomerAddress() {

            var txtBillAdd = document.getElementById('<%= txtBillAdd.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');

            var txtAddress = document.getElementById('<%= hdnCustomerAddress.ClientID %>');
            var txtCity = document.getElementById('<%= hdnCustomerCity.ClientID %>');
            var ddlState = document.getElementById('<%= hdnCustomerState.ClientID %>');
            var txtZip = document.getElementById('<%= hdnCustomerZipCode.ClientID %>');

            var chkAdd = document.getElementById('<%= chkCustomerAddress.ClientID %>');

            if (chkAdd.checked == true) {
                txtBillAdd.value = txtAddress.value;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;
                txtBillZip.value = txtZip.value;
            }
        }

        function initialize() {
            var address = new google.maps.LatLng(document.getElementById('<%= lat.ClientID %>').value, document.getElementById('<%= lng.ClientID %>').value);
            var marker;
            var map;
            var mapOptions = {
                zoom: 13,
                mapTypeId: google.maps.MapTypeId.ROADMAP,
                center: address
            };

            map = new google.maps.Map(document.getElementById('map'),
          mapOptions);

            marker = new google.maps.Marker({
                map: map,
                draggable: false,
                position: address
            });
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
                        ret = confirm('Account # edited, this will create a new Job in Sage and make existing inactive. Do you want to continue?');
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
                 textarea.value = lines.slice(0, 2).join("\n");
                 $("#spnAddress").fadeIn('slow', function () {
                     $(this).delay(500).fadeOut('slow');
                 });
             }
        }
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
    </script>
    <style>
        .pac-container {
    width: 700px !important;
}
    </style>
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
                    <span>Customer Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/locations.aspx") %>">Location</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Location</span>
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Location</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblLocationName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                    <ul>
                                        <li style="margin: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" CssClass="icon-first" runat="server" OnClick="lnkFirst_Click"
                                                CausesValidation="False"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" CssClass="icon-previous" runat="server" CausesValidation="False"
                                                OnClick="lnkPrevious_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CssClass="icon-next" CausesValidation="False"
                                                OnClick="lnkNext_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CssClass="icon-last" CausesValidation="False"
                                                OnClick="lnkLast_Click"></asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            </asp:Panel>
                        </li>
                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ToolTip="Save" runat="server" OnClick="btnSubmit_Click" OnClientClick="return AlertSageIDUpdate();"
                                TabIndex="24"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false"
                                OnClick="lnkClose_Click" TabIndex="25"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div>
                        <input id="hdnPatientId" runat="server" type="hidden" />
                        <asp:HiddenField ID="hdnMail" runat="server" />
                        <asp:HiddenField ID="hdnSageIntegration" runat="server" />

                    </div>
                    <div>
                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel runat="server" ID="tpLocation" HeaderText="Location Info.">
                                <HeaderTemplate>
                                    Location Info.
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-md-8 col-mg-8">
                                          <div class="form-col">
                                                    <div class="fc-label">
                                                        <asp:Label ID="Label4" Visible="False" runat="server" Text="ID"></asp:Label></td>
                                                    </div>
                                                    <div class="fc-label">
                                                               <asp:Label ID="Label5" Visible="False" runat="server" ></asp:Label></td>
                                                    </div>
                                                </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Customer Name
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer"
                                        ErrorMessage="Please select the existing customer" ClientValidationFunction="ChkCustomer"
                                        Display="None"></asp:CustomValidator><asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                            runat="server" Enabled="True" TargetControlID="CustomValidator1">
                                        </asp:ValidatorCalloutExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer"
                                                    Display="None" ErrorMessage="Customer Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator19_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator19">
                                                    </asp:ValidatorCalloutExtender>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlCustomer"
                                                    Display="None" ErrorMessage="Customer Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="ValidatorCalloutExtender1" runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input merchant-input">
                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control searchinput"
                                                    TabIndex="1" autocomplete="off" placeholder="Search by customer name/address/phone#/city"></asp:TextBox>
                                                <span>
                                                    <a id="showModalPopupClientButton" class="btn cn-search" title="Add new customer" href="#"><i class="fa fa-plus"></i></a></span>
                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtCustomer"
                                                    EnableCaching="False" ServiceMethod="GetCustomers" UseContextKey="True" MinimumPrefixLength="0"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" OnClientItemSelected="ace_itemSelected"
                                                    ID="AutoCompleteExtender" DelimiterCharacters="" CompletionInterval="250">
                                                </asp:AutoCompleteExtender>
                                                <asp:DropDownList ID="ddlCustomer" Style="display: none;" runat="server" CssClass="form-control"
                                                    Width="200px" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlCustomer_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <asp:Button CausesValidation="False" ID="btnSelectCustomer" runat="server" Text="Button"
                                                    Style="display: none;" OnClick="ddlCustomer_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Location Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                    ControlToValidate="txtLocName" Display="None" ErrorMessage="Location Name Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtLocName" runat="server" CssClass="form-control searchinput"
                                                    MaxLength="100" TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-4 col-mg-4">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Account #
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtAcctno"
                                        Display="None" ErrorMessage="Account # Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator20">
                                        </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                    <ContentTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtAcctno" runat="server" CssClass="form-control" MaxLength="15"
                                                                        TabIndex="3"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnAcctID" runat="server" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnSageID" runat="server" Text="Check" OnClick="btnSageID_Click" CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Address<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtAddress"
                                                    Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator11_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator11">
                                                    </asp:ValidatorCalloutExtender>
                                                </br>
                                                <span id="spnAddress" style="color:red; display:none; ">Max 2 lines 30 characters each</span>
                                                </br>
                                            </div>
                                            <div class="fc-input">
                                            <div style="border:1px solid #cecece">
                                               <div>
                                                   <input id="txtGoogleAutoc" name="name" style="width: 240px; height:23px;  border:none;" placeholder="Search address here on Google Maps"></input>
                                               </div>  
                                                <div id="divmain">                                                   
                                                    <asp:TextBox ID="txtAddress" runat="server"
                                                        ONKEYUP="return checkMaxLength(this, event, 35)"
                                                         MaxLength="255"
                                                        TabIndex="4" TextMode="MultiLine" name="name" style="border:none; max-width: 240px;min-width: 240px;"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                       
                                        <div class="form-col">
                                            <div class="fc-label">
                                                City<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtCity"
                                                    Display="None" ErrorMessage="City Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator6_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator6">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="5" name="locality"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                State/Prov.<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" InitialValue="State"
                                                    ControlToValidate="ddlState" Display="None" ErrorMessage="State Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator7_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator7">
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
                                        <div class="form-col">
                                            <div class="fc-label">Zip/Postal Code</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="10"
                                                    TabIndex="7"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            
                                            <div class="fc-label">Lat/Lng</div>
                                            <div class="fc-input" >
                                                <div  style="width:194px; float:left;">
                                                <input id="lat" runat="server" tabindex="8" class="form-control" type="text"  style="width:95px; float:left;"  />
                                                <input id="lng" runat="server" tabindex="9" class="form-control" type="text"  style="width:95px; float:right;"  />
                                                <input id="locality" disabled="disabled" style="display: none;">
                                                <input id="country" disabled="disabled" style="display: none;">
                                                </div>
                                                <input id="mapLink" class="btn purple btn-sm  map-btn" type="button" style="display: block; float:right;" value="Map" />
                                                <div id="map" style="height: 156px; width: 240px; display:none; ">
                                                    </div>
                                                 </div>

                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Sales Tax
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlSTax" runat="server" CssClass="form-control"
                                                    TabIndex="10">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-col">
                                            <div class="fc-label">
                                                Default Worker
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server"
                                        ControlToValidate="ddlRoute" Display="None"
                                        ErrorMessage="Default Worker Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator17_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True"
                                                    TargetControlID="RequiredFieldValidator17">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlRoute" runat="server" CssClass="form-control"
                                                    TabIndex="11">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom1" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst1" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-mg-4">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Main Contact
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtMaincontact" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="12"></asp:TextBox>
                                            </div>
                                        </div>

                                        <div class="form-col">
                                            <div class="fc-label">
                                                Phone
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtPhoneCust" runat="server" CssClass="form-control" MaxLength="28"
                                                    TabIndex="13"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">Fax</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control"
                                                    MaxLength="28" TabIndex="14"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">Website</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control"
                                                    MaxLength="50" TabIndex="15"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">Email</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"
                                                    MaxLength="50" TabIndex="16"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">Cellular</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCell" runat="server" CssClass="form-control"
                                                    MaxLength="28" TabIndex="17"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">Type</div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"
                                                    TabIndex="18">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Default Salesperson<asp:RequiredFieldValidator ID="RequiredFieldValidator18"
                                                    runat="server" ControlToValidate="ddlTerr" Display="None"
                                                    ErrorMessage="Default Salesperson Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator18_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="Left"
                                                        TargetControlID="RequiredFieldValidator18">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlTerr" runat="server" CssClass="form-control"
                                                    TabIndex="19">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom2" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst2" runat="server" CssClass="form-control"
                                                    TabIndex="20"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblContractBill" runat="server" Text="Contract Billing"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlContractBill" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlContractBill_SelectedIndexChanged" AutoPostBack="True"
                                                    TabIndex="21">
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="Label6" runat="server" Text="Default Terms"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlTerms" runat="server" CssClass="form-control" 
                                                    TabIndex="21">
                                                </asp:DropDownList>                                               
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-lg-4">
                                        <div class="form-col">
                                            <div class="fc-label"><b>Billing Address</b></div>
                                            <div class="fc-input">
                                                <label>
                                                    <asp:CheckBox ID="chkAddress" runat="server" TabIndex="20" Text="Same as office address" onclick="javascript:ChkAddress();" /></label>
                                                <label>
                                                    <asp:CheckBox ID="chkCustomerAddress" runat="server" TabIndex="21" Text="Same as customer address" onclick="javascript:ChkCustomerAddress();" /></label>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Address<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBillAdd"
                                                    Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator2_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        PopupPosition="Left" TargetControlID="RequiredFieldValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillAdd" runat="server"
                                                     ONKEYUP="return checkMaxLength(this, event, 35)"
                                                    CssClass="form-control" MaxLength="255"
                                                    TabIndex="22" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                City<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtBillCity"
                                                    Display="None" ErrorMessage="City Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator3_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        PopupPosition="Left" TargetControlID="RequiredFieldValidator3">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillCity" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="23"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                State/Prov.<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" InitialValue="State"
                                                    ControlToValidate="ddlBillState" Display="None" ErrorMessage="State Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator4_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        PopupPosition="Left" TargetControlID="RequiredFieldValidator4">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlBillState" runat="server" ToolTip="State" CssClass="form-control"
                                                    TabIndex="24">
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
                                                <asp:TextBox ID="txtBillZip" runat="server" CssClass="form-control"
                                                    TabIndex="25" MaxLength="10"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Status
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlLocStatus" runat="server" TabIndex="26" CssClass="form-control">
                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-col">
                                            <div class="credit-dispathc">
                                                <label>
                                                    Credit Hold
                                                    <asp:CheckBox ID="chkCreditHold" runat="server" /></label><br>
                                                <br>
                                                <label>Dispatch Alert<asp:CheckBox ID="chkDispAlert" runat="server" /></label>
                                            </div>
                                            <div class="cd-textarea">
                                                <asp:TextBox ID="txtCreditReason" runat="server" TabIndex="27"
                                                    CssClass="form-control" TextMode="MultiLine" placeholder="Reason"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="col-md-8 col-lg-8">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Remarks
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="4" TabIndex="28"
                                                    MaxLength="8000" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <b>Service Email:</b>
                                            </div>
                                            <div class="fc-input">
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                To<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                    ControlToValidate="txtEmailTo" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                    SetFocusOnError="True"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmailTo" runat="server" MaxLength="50" TabIndex="29"
                                                    CssClass="form-control"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmailTo_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtEmailTo">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                CC<asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                                    ControlToValidate="txtEmailCC" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                    SetFocusOnError="True"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="RegularExpressionValidator3_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator3">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmailCC" runat="server" MaxLength="50" TabIndex="30"
                                                    CssClass="form-control"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmailCC_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtEmailCC">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <b>Invoice Email:</b>
                                            </div>
                                            <div class="fc-input">
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                To<asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                                    ControlToValidate="txtEmailToInv" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                    SetFocusOnError="True"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="RegularExpressionValidator4_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator4">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmailToInv" runat="server" MaxLength="50" TabIndex="31"
                                                    CssClass="form-control"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmailToInv_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtEmailToInv">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                CC<asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server"
                                                    ControlToValidate="txtEmailCCInv" Display="None" ErrorMessage="Invalid E-Mail Address"
                                                    ValidationExpression="^((\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4}\s*[,;:]){1,100}?)?(\s*[a-zA-Z0-9\._%-]+@[a-zA-Z0-9\.-]+\.[a-zA-Z]{2,4})*$"
                                                    SetFocusOnError="True"></asp:RegularExpressionValidator><asp:ValidatorCalloutExtender ID="RegularExpressionValidator5_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator5">
                                                    </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmailCCInv" runat="server" MaxLength="50" TabIndex="32"
                                                    CssClass="form-control"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmailCCInv_FilteredTextBoxExtender"
                                                    runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars=" "
                                                    TargetControlID="txtEmailCCInv">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-lg-4">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <b>Billing Rate</b>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Billing Rate
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillRate" runat="server" CssClass="form-control"
                                                    MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                OT Rate
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtOt" runat="server" CssClass="form-control"
                                                    MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Travel Rate
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtTravel" runat="server" CssClass="form-control"
                                                    MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                1.7 Rate
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtNt" runat="server" CssClass="form-control"
                                                    MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                DT Rate
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtDt" runat="server" CssClass="form-control"
                                                    MaxLength="15" TabIndex="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                            </div>
                                        </div>
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
                                    <div>
                                        <asp:HiddenField ID="hdnCustomerAddress" runat="server" />
                                        <asp:HiddenField ID="hdnCustomerCity" runat="server" />
                                        <asp:HiddenField ID="hdnCustomerState" runat="server" />
                                        <asp:HiddenField ID="hdnCustomerZipCode" runat="server" />
                                    </div>
                                    <div class="col-md-12 col-lg-12">
                                        <div class="row">
                                            <div class="table-scrollable" style="border: none">
                                                <div style="padding-bottom: 15px">
                                                    <div style="background: #316b9d; width: 100%;">
                                                        <ul class="lnklist-header lnklist-panel">
                                                            <li>
                                                                <asp:Label CssClass="title_text" ID="Label2" runat="server">Contacts</asp:Label></li>
                                                            <li>
                                                                <asp:LinkButton CssClass="icon-addnew" Style="float: right" ID="lnkAddnew" ToolTip="Add New" runat="server" CausesValidation="False" OnClick="lnkAddnew_Click" TabIndex="21"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:LinkButton CssClass="icon-edit" Style="float: right"
                                                                    ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False"
                                                                    TabIndex="23" ToolTip="Edit"></asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton CssClass="icon-delete" Style="float: right"
                                                                    ID="btnDelete" runat="server" CausesValidation="False" OnClick="btnDelete_Click"
                                                                    TabIndex="22" ToolTip="Delete"></asp:LinkButton></li>
                                                            <li>
                                                                <asp:HyperLink ID="lnkMail" CssClass="icon-mail" runat="server" ToolTip="Send Mail"></asp:HyperLink></li>
                                                        </ul>
                                                    </div>
                                                    <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="100%" OnRowDataBound="gvContacts_RowDataBound" OnRowCreated="gvContacts_RowCreated">
                                                        <RowStyle CssClass="evenrowcolor" />
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
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    </asp:GridView>
                                                </div>
                                                <div class="clearfix"></div>
                                                <div>
                                                    <asp:Panel ID="pnlDoc" runat="server" Visible="False">
                                                        <asp:Panel ID="pnlDocumentButtons" runat="server" Style="background: #316b9d; width: 100%;">
                                                            <ul class="lnklist-header lnklist-panel">
                                                                <li>
                                                                    <asp:Label CssClass="title_text" ID="Label3" runat="server" Style="float: left">Documents</asp:Label></li>
                                                                <li>
                                                                    <asp:LinkButton ID="lnkDeleteDoc" runat="server" CssClass="icon-delete" CausesValidation="False" OnClick="lnkDeleteDoc_Click"
                                                                        OnClientClick="return checkdelete();"></asp:LinkButton></li>
                                                                <li>
                                                                    <table style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:FileUpload ID="FileUpload1" runat="server" onchange="ConfirmUpload(this.value);" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                                    Style="display: none">Upload</asp:LinkButton>
                                                                                <asp:LinkButton ID="lnkPostback" runat="server"
                                                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </li>
                                                            </ul>
                                                        </asp:Panel>
                                                        <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
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
                                                                        <asp:LinkButton ID="lblName" runat="server" CausesValidation="false" CommandArgument='<%#Eval("filename") + "," + Eval("Path") %>'
                                                                            OnClick="lblName_Click" Text='<%# Eval("filename") %>'> </asp:LinkButton>
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
                                                                        <asp:TextBox ID="txtremarks"  Width="500px" runat="server" Text='<%# Eval("remarks") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </div>
                                                <div class="clearfix"></div>
                                            </div>

                                        </div>

                                        <table id="tblMain">
                                            <tr>
                                                <td colspan="8" align="center">
                                                    <asp:Label ID="lblMsg" CssClass="lblMsg" runat="server" ForeColor="#CC0000"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tpViewEquipment" runat="server" HeaderText="View Equipment">
                                <ContentTemplate>
                                    <div class="table-scrollable" style="border: none">
                                        <asp:Panel runat="server" ID="pnlEqGridButtons" Style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:LinkButton CssClass="icon-edit" ID="lnkEditEq" ToolTip="Edit" runat="server" OnClick="lnkEditEq_Click" CausesValidation="False"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="btnCopyEQ" runat="server" CssClass="icon-copy" ToolTip="Copy"
                                                        OnClick="btnCopyEQ_Click" CausesValidation="False"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkDeleteEQ" runat="server" CssClass="icon-delete" ToolTip="Delete" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer1_tpViewEquipment_gvEquip','Equipment');"
                                                        OnClick="lnkDeleteEQ_Click" CausesValidation="False"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkAddEQ" runat="server" CssClass="icon-addnew" ToolTip="Add New" OnClick="lnkAddEQ_Click" CausesValidation="False"></asp:LinkButton></li>
                                            </ul>
                                        </asp:Panel>
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    DataKeyNames="ID" Width="100%" AllowSorting="True" PageSize="10" ShowFooter="True"
                                                    OnSorting="gvEquip_Sorting" AllowPaging="true" OnDataBound="gvEquip_DataBound"
                                                    OnRowCommand="gvEquip_RowCommand">
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
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
                                                    <FooterStyle CssClass="footer" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <PagerTemplate>
                                                        <div align="center">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" CausesValidation="false" ImageUrl="images/first.png" />
                                                            &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                ImageUrl="~/images/Backward.png" />
                                                            &nbsp &nbsp <span>Page</span>
                                                            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesEquip_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span>of </span>
                                                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                            &nbsp &nbsp
                                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" CausesValidation="false" ImageUrl="images/Forward.png" />
                                                            &nbsp &nbsp
                                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" CausesValidation="false" ImageUrl="images/last.png" />
                                                        </div>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpHistory" HeaderText="Location History.">
                                <HeaderTemplate>
                                    Location History
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="search-customer">
                                        <div class="sc-form">
                                            <label>Search for tickets where :</label>
                                            <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True"
                                                CssClass="form-control input-sm input-small" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged">
                                                <asp:ListItem Value=" ">Select</asp:ListItem>
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
                                            <asp:DropDownList ID="ddlCategory" runat="server"
                                                CssClass="form-control input-sm input-small" TabIndex="11" Visible="False" Width="200px">
                                            </asp:DropDownList>

                                            <asp:Label ID="lblRecordCount0" runat="server" Style="font-style: italic;"></asp:Label>

                                        </div>
                                    </div>
                                    <div class="search-customer" style="padding: 10px 0 10px 0">
                                        <div class="sc-form">
                                            <label>Status</label>
                                            <asp:DropDownList ID="ddlStatus" runat="server"
                                                CssClass="form-control input-sm input-small" TabIndex="14" Width="200px">
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
                                            <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtfromDate">
                                            </asp:CalendarExtender>
                                            <label>To</label>
                                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm input-small"
                                                MaxLength="50"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server"
                                                Enabled="True" TargetControlID="txtToDate">
                                            </asp:CalendarExtender>

                                            <asp:LinkButton ID="btnSearch" CssClass="btn submit" runat="server" CausesValidation="false" TabIndex="23"
                                                OnClick="btnSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            <%--<asp:ImageButton ID="btnSearch" runat="server" CausesValidation="False"
                                                ImageUrl="images/search.png" OnClick="btnSearch_Click" TabIndex="23"
                                                ToolTip="Search" />--%>
                                            <asp:LinkButton ID="lnkPrint" runat="server" OnClick="lnkPrint_Click"
                                                CssClass="btn submit">
                                            <i class="fa fa-print"></i>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-scrollable">
                                        <asp:Panel runat="server" ID="pnlGridButtons" Style="background: #316b9d;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:LinkButton ID="lnkEditTicket" OnClientClick="editticket(); return false;" CssClass="icon-edit" ToolTip="Edit" runat="server" OnClick="lnkEditTicket_Click"></asp:LinkButton></li>
                                                <li>
                                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="icon-addnew" ToolTip="Add New" 
                                                         Target="_blank"></asp:HyperLink>
                                                   </li>
                                            </ul>
                                        </asp:Panel>
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvOpenCalls" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    PageSize="10" AllowPaging="true" ShowFooter="True" AllowSorting="True"
                                                    OnSorting="gvOpenCalls_Sorting" OnDataBound="gvOpenCalls_DataBound" OnRowCommand="gvOpenCalls_RowCommand" OnRowCreated="gvOpenCalls_RowCreated">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                                <asp:Label ID="lblComp" Visible="true" Style="display: none"
                                                                    runat="server" Text='<%# Bind("Comp") %>'></asp:Label>
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
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <PagerTemplate>
                                                        <div align="center">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" CausesValidation="false" ImageUrl="images/first.png" />
                                                            &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                ImageUrl="~/images/Backward.png" />
                                                            &nbsp &nbsp <span>Page</span>
                                                            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesOpenCall_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span>of </span>
                                                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                            &nbsp &nbsp
                                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" CausesValidation="false" ImageUrl="images/Forward.png" />
                                                            &nbsp &nbsp
                                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" CausesValidation="false" ImageUrl="images/last.png" />
                                                        </div>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpInvoices" HeaderText="View Invoice">
                                <HeaderTemplate>
                                    Transactions
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="search-customer">
                                        <div class="sc-form">
                                            Select for Invoices where
                                            <asp:DropDownList ID="ddlSearchInv" runat="server" AutoPostBack="True" CssClass="form-control input-sm input-small"
                                                OnSelectedIndexChanged="ddlSearchInv_SelectedIndexChanged">
                                                <asp:ListItem Value=" ">Select</asp:ListItem>
                                                <asp:ListItem Value="i.ref">Invoice#</asp:ListItem>
                                                <asp:ListItem Value="i.fdate">Invoice Date</asp:ListItem>
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
                                            <asp:TextBox ID="txtInvDt" runat="server" CssClass="form-control input-sm input-small" Visible="False"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtInvDt_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtInvDt">
                                            </asp:CalendarExtender>
                                            <asp:TextBox ID="txtSearchInv" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="search-customer" style="padding: 10px 0 10px 0">
                                        <div class="sc-form">
                                            <label>Date From</label>
                                            <asp:TextBox ID="txtInvDtFrom" runat="server" CssClass="form-control input-sm input-small" MaxLength="50" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True"
                                                TargetControlID="txtInvDtFrom">
                                            </asp:CalendarExtender>
                                            <label>Date To</label>
                                            <asp:TextBox ID="txtInvDtTo" runat="server" CssClass="form-control input-sm input-small" MaxLength="50" Width="80px"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" Enabled="True"
                                                TargetControlID="txtInvDtTo">
                                            </asp:CalendarExtender>
                                            <%--<asp:ImageButton ID="lnkSearch" runat="server" ImageUrl="images/search.png"
                                                OnClick="lnkSearch_Click" ToolTip="Search" CausesValidation="False"></asp:ImageButton>--%>
                                            <asp:LinkButton ID="lnkSearch" CssClass="btn submit" runat="server" CausesValidation="false"
                                                OnClick="lnkSearch_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                        <ul style="padding: 0 5px 0px 5px; float: left">
                                            <li>
                                                <asp:LinkButton ID="lnkClear" runat="server"
                                                    OnClick="lnkClear_Click" CausesValidation="False">Clear</asp:LinkButton></li>
                                            <li>
                                                <asp:LinkButton ID="lnkShowAll" runat="server" OnClick="lnkShowAll_Click"
                                                    CausesValidation="False">Show All Invoices</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:Label ID="lblRecordCount" runat="server" Style="font-style: italic;"></asp:Label>
                                            </li>
                                        </ul>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div class="table-scrollable">
                                        <asp:Panel runat="server" ID="Panel5" Style="background: #316b9d;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:LinkButton ID="lnkEditInvoice" CssClass="icon-edit" runat="server" CausesValidation="False" OnClick="lnkEditInvoice_Click"></asp:LinkButton></li>

                                                <li>
                                                    <asp:LinkButton ID="lnkDeleteInvoice" CssClass="icon-delete"
                                                        runat="server" CausesValidation="False" OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_gvEquip','Invoice');"
                                                        OnClick="lnkDeleteInvoice_Click"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkAddInvoice" CausesValidation="False" CssClass="icon-addnew" runat="server" OnClick="lnkAddInvoice_Click"></asp:LinkButton></li>
                                                <li>
                                                    <asp:LinkButton ID="lnkCopyInvoice" CssClass="icon-copy"
                                                        runat="server" Visible="False" CausesValidation="False" OnClick="lnkCopyInvoice_Click"></asp:LinkButton></li>
                                            </ul>
                                        </asp:Panel>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

                                            <ContentTemplate>
                                                <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    DataKeyNames="ID" PageSize="10" ShowFooter="True" OnSorting="gvInvoice_Sorting"
                                                    AllowPaging="True" AllowSorting="True" OnDataBound="gvInvoice_DataBound" OnRowCommand="gvInvoice_RowCommand">
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
                                                            <asp:TemplateField HeaderText="ID" Visible="False">
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
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" CausesValidation="false" ImageUrl="images/first.png" />
                                                            &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                ImageUrl="~/images/Backward.png" />
                                                            &nbsp &nbsp <span>Page</span>
                                                            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesInvoice_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span>of </span>
                                                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                            &nbsp &nbsp
                                                            <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" CausesValidation="false" ImageUrl="images/Forward.png" />
                                                            &nbsp &nbsp
                                                            <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" CausesValidation="false" ImageUrl="images/last.png" />
                                                        </div>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:TabPanel>

                              <asp:TabPanel runat="server" ID="tpProjects" HeaderText="Projects">
                                <HeaderTemplate>
                                    Projects
                                </HeaderTemplate>
                                <ContentTemplate>
                                     <asp:Panel runat="server" ID="Panel3" Style="background: #316b9d;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                     <asp:HyperLink ID="lnkAddProject" runat="server" CssClass="icon-addnew" ToolTip="Add New" 
                                                                                 Target="_blank"></asp:HyperLink>
                                                </li>
                                            </ul>
                                        </asp:Panel>
                                    <asp:GridView ID="gvProject" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-bordered table-striped table-condensed flip-content"
                                        Width="100%" PageSize="20" ShowFooter="true"
                                       >
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <FooterStyle CssClass="footer" />
                                        <RowStyle CssClass="evenrowcolor" />
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                        <Columns>

                                            <asp:TemplateField HeaderText="Project #" SortExpression="id">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                    <asp:HyperLink ID="lnkJob" runat="server" NavigateUrl='<%# "addproject.aspx?uid=" + Eval("id") %>'
                                                                Target="_blank" Text='<%# Bind("ID") %>'></asp:HyperLink>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desc" SortExpression="fdesc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblstatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Created" SortExpression="fdate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Hours" SortExpression="Hour">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHour" runat="server" Text='<%# Eval("Hour") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblHourFooter" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Billed" SortExpression="TotalBilled">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRev" runat="server" Text='<%# Eval("TotalBilled") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblRevFooter" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total Expenses" SortExpression="TotalExp">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTotalExp" runat="server" Text='<%# Eval("TotalExp") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotalExpFooter" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Net" SortExpression="net">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNet" runat="server" Text='<%# Eval("net") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblNetFooter" runat="server"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       <%--  <PagerTemplate>
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
                                        </PagerTemplate>--%>
                                    </asp:GridView>
                                </ContentTemplate>
                            </asp:TabPanel>

                             <asp:TabPanel runat="server" ID="tpAlerts" HeaderText="Alerts">
                                <HeaderTemplate>
                                    Alerts
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <asp:GridView ID="gvAlerts" runat="server" AutoGenerateColumns="False" 
                                        CssClass="table table-bordered table-striped table-condensed flip-content"
                                            PageSize="20" ShowFooter="true" Width="600px" margin-top="20px">
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                        <asp:Label ID="lblID" runat="server" Visible="false" Text='<%# Eval("alertid") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:ImageButton ID="ibtnDeleteItem" OnClientClick="return confirm('Are you sure you want to delete the items?')"
                                                            CausesValidation="false" ToolTip="Delete" ImageUrl="images/menu_delete.png" runat="server"
                                                            OnClick="ibtnDeleteItem_Click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SNO" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Code">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtCode" MaxLength="25" runat="server" Text='<%# Eval("alertCode") %>'
                                                            Width="100px" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtCode"
                                                            Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:ImageButton ID="lnkAddnewRow" runat="server" CausesValidation="False" OnClick="lnkAddnewRow_Click"
                                                            ImageUrl="images/add.png" Width="20px" ToolTip="Add New Row" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Subject" >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("alertsubject") %>' Width="300px" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescT" runat="server" ControlToValidate="lblDesc"
                                                            Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                    </FooterTemplate>
                                                    <FooterStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Message" >
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="lblMessage" runat="server" Text='<%# Eval("alertmessage") %>' Width="300px" CssClass="form-control"></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="rfvDescoT" runat="server" ControlToValidate="lblMessage"
                                                            Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator>
                                                    </ItemTemplate>
                                                     <FooterTemplate>
                                                        <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                            <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>
                                                        </div>
                                                    </FooterTemplate>
                                                    <FooterStyle VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                               
                                              
                                            </Columns>
                                        </asp:GridView>
                                </ContentTemplate>
                            </asp:TabPanel>

                        </asp:TabContainer>
                    </div>

                    <div class="clearfix"></div>
                    <asp:Panel runat="server" ID="pnlOverlay" Visible="false">
                    </asp:Panel>
                    <%-- <asp:Button runat="server" ID="Button1" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="abcd" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="Button1" PopupControlID="pnlContact"
                        RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>--%>
                    <asp:Panel ID="pnlContact" runat="server" CssClass="pnlUpdate" Visible="false">
                        <div class="model-popup-body" style="padding: 0">

                            <ul class="lnklist-header lnklist-panel">
                                <li>
                                    <asp:Label CssClass="title_text" ID="Label1" runat="server">Add Contact</asp:Label>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkContactSave" runat="server" CssClass="icon-save"
                                        OnClick="lnkContactSave_Click" ToolTip="Save"
                                        ValidationGroup="cont"></asp:LinkButton></li>
                                <li>
                                    <asp:LinkButton ID="lnkCancelContact" runat="server" CausesValidation="False" OnClick="LinkButton2_Click"
                                        CssClass="icon-closed" ToolTip="Close"></asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <div class="form-col">
                            <div>
                                Contact Name
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator12">
                                </asp:ValidatorCalloutExtender>
                            </div>
                            <div>
                                <asp:TextBox ID="txtContcName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div>
                                Phone
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtContPhone"
                            Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator13">
                                </asp:ValidatorCalloutExtender>
                            </div>
                            <div>
                                <asp:TextBox ID="txtContPhone" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div>
                                Fax
                            </div>
                            <div>
                                <asp:TextBox ID="txtContFax" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div>
                                Cell
                            </div>
                            <div>
                                <asp:TextBox ID="txtContCell" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-col">
                            <div>
                                Email
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtContEmail"
                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator16_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator16">
                                </asp:ValidatorCalloutExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                    Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                    ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                </asp:ValidatorCalloutExtender>
                            </div>
                            <div>
                                <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                            </div>
                        </div>

                    </asp:Panel>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid;">
                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                            <div class="title_bar_popup1">
                                <%--   <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px; position: absolute; top: 10px; right: 100px"                                    Text="Close" 
                                    OnClick="hideModalPopupViaServerConfirm_Click" CausesValidation="false" ForeColor="Black"/>--%>
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Style="float: right; margin-right: 10px; color: white; margin-left: 10px; height: 16px; position: absolute; top: 30px; right: 100px"
                                    Text="Close"
                                    OnClick="hideModalPopupViaServerConfirm_Click" CausesValidation="false" />
                            </div>
                        </asp:Panel>
                        <div>
                            <iframe id="iframe" width="1000px" height="580px" frameborder="0" runat="server"></iframe>
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
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
        <!-- edit-tab end -->
        <div class="clearfix"></div>

    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
</asp:Content>
