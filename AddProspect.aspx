<%@ Page Title="" Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true"
    CodeFile="AddProspect.aspx.cs" Inherits="AddProspect" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_customersearch" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="http://maps.googleapis.com/maps/api/js?sensor=false&amp;libraries=places&key=AIzaSyDedmuEC-1d__Jc1M3OLx1NIAnc_gMRbhE"></script>

    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>

    <script type="text/javascript">
        var newEMailCount = 0;
        $(function () {
            $("#<%= txtAddress.ClientID %>").geocomplete({
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
        });

        $(document).ready(function () {
            setInterval(serviceCall, 10000);
            $('#<%= txtAddress.ClientID %>').keyup(function (event) {
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
                $("#Coord").toggle();
                initialize();
            });

        });

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


        function ChkAddress() {
            var txtBillAdd = document.getElementById('<%= txtBillAddress.ClientID %>');
            var txtBillCity = document.getElementById('<%= txtBillCity.ClientID %>');
            var ddlBillState = document.getElementById('<%= ddlBillState.ClientID %>');
            var txtBillZip = document.getElementById('<%= txtBillZip.ClientID %>');
            var txtBillPhone = document.getElementById('<%= txtBillPhone.ClientID %>');

            var txtAddress = document.getElementById('<%= txtAddress.ClientID %>');
            var txtCity = document.getElementById('<%= txtCity.ClientID %>');
            var ddlState = document.getElementById('<%= ddlState.ClientID %>');
            var txtZip = document.getElementById('<%= txtZip.ClientID %>');
            var txtPhone = document.getElementById('<%= txtPhone.ClientID %>');

            var chkAdd = document.getElementById('<%= chkAddress.ClientID %>');

            if (chkAdd.checked == true) {
                txtBillAdd.value = txtAddress.value;
                txtBillCity.value = txtCity.value;
                ddlBillState.value = ddlState.value;
                txtBillZip.value = txtZip.value;
                txtBillPhone.value = txtPhone.value;
            }
        }

        function serviceCall() {
            var rol = document.getElementById('<%= hdnRol.ClientID %>');
            if (rol.value != '') {
                $.ajax({
                    type: "POST",
                    url: 'AddProspect.aspx/CheckEmail',
                    data: '{"rol":"' + rol.value + '","type":"-1","uid":"0"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        //                    alert(msg.d);
                        MailCount(msg.d)
                    },
                    error: function (e) {
                        //                        alert(jQuery.parseJSON(e));
                    }
                });
            }
        }

        function MailCount(d) {

            var newmail = 0;
            var hdnct = document.getElementById('<%= hdnMailct.ClientID %>').value;

            if (hdnct != '') {
                newmail = hdnct;
            }

            //            alert(newmail + ' -- ' + d);
            if (newmail != d) {
                document.getElementById('dvmailct').innerHTML = d - newmail + ' New Email(s)';
                $("#maillink").show();
            }
            else {
                $("#maillink").hide();
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-cont-top">
        <%--<ul class="page-breadcrumb">
            <li>
                <i class="fa fa-home"></i>
                <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="#">Sales Manager</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="<%=ResolveUrl("~/prospects.aspx") %>">Leads</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                
            </li>
        </ul>--%>
        <div class="page-bar-right">
            <%--<asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Leads</asp:Label>--%>
        </div>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
            <asp:Panel runat="server" ID="pnlGridButtons">
                <ul class="lnklist-header">
                    <li>
                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Leads</asp:Label></li>
                    <li>
                        <asp:LinkButton ID="lnkSave" runat="server" CssClass="icon-save" ToolTip="Save" OnClick="lnkSave_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton ID="lnkClose" runat="server" CausesValidation="False" ToolTip="Close" CssClass="icon-closed" OnClick="lnkClose_Click"></asp:LinkButton></li>
                    <li>
                        <asp:LinkButton ID="lnkConvert" Visible="false" runat="server"
                            ToolTip="Convert to Customer/Location" CssClass="save_button"
                            OnClick="lnkConvert_Click">Convert</asp:LinkButton>
                    </li>
                </ul>
            </asp:Panel>

        </div>
        <div class="ae-content">
            <asp:HiddenField ID="hdnRol" runat="server" />

            <a id="maillink" href="#Div5" style="display: none; position: fixed; top: 0; left: 615px;">
                <div id="dvmailct" style="width: 100%; height: 100%; vertical-align: middle; text-align: center; padding: 5px; background-color: Black"
                    class="transparent roundCorner shadow">
                </div>
            </a>
            <asp:Panel runat="server" ID="Popup" Width="100%">
                <div>
                    <asp:Panel ID="pnlProspects" runat="server">
                        <div class="col-md-12 col-lg-12">
                            <div class="row">
                                <div style="padding-bottom: 15px">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                            <asp:Menu ID="menuLeads" runat="server" CssClass="menu" Orientation="Horizontal">
                                                <StaticMenuItemStyle ItemSpacing="20px" />
                                                <Items>
                                                    <asp:MenuItem Text="Contacts(0)" NavigateUrl="#Div1" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="Open Tasks(0)" NavigateUrl="#Div2" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="Task History(0)" NavigateUrl="#Div3" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="Opportunities(0)" NavigateUrl="#Div4" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="Emails(0)" NavigateUrl="#Div5" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="Notes and Attachments(0)" NavigateUrl="#Div6" SeparatorImageUrl="images/menu_bg_s.png"></asp:MenuItem>
                                                    <asp:MenuItem Text="System Info" NavigateUrl="#Div7"></asp:MenuItem>
                                                </Items>
                                            </asp:Menu>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <div class="clearfix"></div>
                                <asp:Panel ID="pnlCustomer" runat="server" Visible="false" Width="800px">
                                    <table class="roundCorner shadow" style="border: solid 1px red" width="100%">
                                        <tr>
                                            <td colspan="2">
                                                <div style="text-align: center">
                                                    Please select an existing customer or leave the field blank and this will 
                                                   create a new Customer.
                                                </div>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                    <td class="register_lbl">
                                        Customer Name
                                        <asp:CustomValidator ID="CustomValidator2" runat="server" 
                                            ClientValidationFunction="ChkCust" ControlToValidate="txtCustomer" 
                                            Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:CustomValidator>
                                        <asp:ValidatorCalloutExtender ID="CustomValidator2_ValidatorCalloutExtender" 
                                            runat="server" Enabled="True" PopupPosition="TopLeft" 
                                            TargetControlID="CustomValidator2">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" 
                                            CssClass="form-control_address searchinput" 
                                            placeholder="Search by customer name, phone#, address etc." TabIndex="3" 
                                            ToolTip="Customer Name "></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" 
                                            runat="server" Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" 
                                            TargetControlID="txtCustomer">
                                        </asp:FilteredTextBoxExtender>
                                    </td>
                                </tr>--%>
                                        <tr>
                                            <td colspan="2">
                                                <uc1:uc_customersearch ID="uc_CustomerSearch1" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <div class="clearfix"></div>

                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" CollapseControlID="imgExpand1"
                                    CollapsedImage="~/images/arrow_right.png" Enabled="True" ExpandControlID="imgExpand1"
                                    ExpandedImage="~/images/arrow_down.png" ImageControlID="imgExpand1" SuppressPostBack="true"
                                    TargetControlID="pnlLeaddeta" />
                                <div id="AddressInfo0" runat="server" style="font-weight: bold; padding-bottom: 15px; font-size: 15px">
                                    Lead Information:
                                <asp:Image ID="imgExpand1" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                </div>
                                <asp:Panel ID="pnlLeaddeta" runat="server">
                                    <div class="col-md-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Lead Name
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtProspectName"
                                                    Display="None" ErrorMessage="Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtProspectName" runat="server" CssClass="form-control" MaxLength="75"
                                                    TabIndex="1"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Lead Customer
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server"
                                                ControlToValidate="txtCustomer" Display="None"
                                                ErrorMessage="Customer Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator47_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator47">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control" MaxLength="75"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Source
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="2"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Status
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" TabIndex="3">
                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Type
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="ddlType"
                                                Display="None" ErrorMessage="Type Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator45_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator45">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" TabIndex="4">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Assigned To
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator44" Enabled="true" runat="server" ControlToValidate="ddlSalesperson"
                                                Display="None" ErrorMessage="Salesperson Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator44_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator44">
                                                </asp:ValidatorCalloutExtender>

                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlSalesperson" runat="server" CssClass="form-control"
                                                    TabIndex="5">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-lg-8">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Remarks
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" Rows="3"
                                                    MaxLength="255" TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </asp:Panel>
                                <div class="clearfix"></div>

                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="pnlAddress"
                                    ExpandControlID="imgExpandTemp" CollapseControlID="imgExpandTemp" SuppressPostBack="True"
                                    ExpandedImage="~/images/arrow_down.png" CollapsedImage="~/images/arrow_right.png"
                                    ImageControlID="imgExpandTemp" Enabled="True" />
                                <div id="AddressInfo" runat="server" style="font-weight: bold; padding-bottom: 15px; font-size: 15px">
                                    Address Information:
                                <asp:Image ID="imgExpandTemp" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                </div>
                                <asp:Panel ID="pnlAddress" runat="server" Width="100%">
                                    <div class="col-md-6">
                                        <div class="form-col">
                                            <div class="fc-label" style="position: relative">
                                                Shipping Address
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtAddress"
                                                Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator41">
                                                </asp:ValidatorCalloutExtender>
                                                <a id="mapLink">
                                                    <img alt="map" class="shadowHover" height="20px" src="images/map.ico" title="Map"
                                                        width="20px" />
                                                </a>
                                                <div>
                                                    <input id="locality" disabled="disabled" style="display: none;" />
                                                    <input id="country" disabled="disabled" style="display: none;" />
                                                    <div class="map-leads-box">
                                                        <div id="Coord" class="map-leads">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Lat:
                                                                        </label>
                                                                        <input id="lat" runat="server" type="text" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <label>
                                                                            Lng:
                                                                        </label>
                                                                        <input id="lng" runat="server" type="text" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div id="map" class="map-prospect" style="display: none;">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="fc-input">

                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"
                                                    MaxLength="255" TabIndex="6" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                City
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="txtCity"
                                                Display="None" ErrorMessage="City Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator42_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator42">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="7"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                State/Prov.
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="ddlState"
                                                Display="None" ErrorMessage="State Required" SetFocusOnError="True" InitialValue="State"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator43_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RequiredFieldValidator43">
                                                </asp:ValidatorCalloutExtender>

                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" ToolTip="State"
                                                    TabIndex="8">
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
                                                <asp:TextBox ID="txtZip" runat="server" MaxLength="10" CssClass="form-control"
                                                    TabIndex="9"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Phone
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" MaxLength="28"
                                                    TabIndex="10"></asp:TextBox>
                                                <asp:MaskedEditExtender ID="txtPhone_MaskedEditExtender" runat="server" AutoComplete="False"
                                                    ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtPhone">
                                                </asp:MaskedEditExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Cellular
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" MaxLength="28"
                                                    TabIndex="11"></asp:TextBox>
                                                <asp:MaskedEditExtender ID="txtCell_MaskedEditExtender" runat="server" AutoComplete="False"
                                                    ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtCell">
                                                </asp:MaskedEditExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Email
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmail"
                                                Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="RegularExpressionValidator2_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" TargetControlID="RegularExpressionValidator2">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="12"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="txtEmail_FilteredTextBoxExtender" runat="server"
                                                    Enabled="True" FilterMode="InvalidChars" InvalidChars=" " TargetControlID="txtEmail">
                                                </asp:FilteredTextBoxExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Main Contact
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtMaincontact" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="13"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:CheckBox ID="chkAddress" runat="server" onclick="javascript:ChkAddress();" />
                                            </div>
                                            <div class="fc-input" style="padding-top: 6px">
                                                Same as shipping address
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Billing Address
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillAddress" runat="server" CssClass="form-control"
                                                    Height="65px" MaxLength="255" TabIndex="14" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                City
                                              
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillCity" runat="server" CssClass="form-control" MaxLength="50"
                                                    TabIndex="15"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                State/Prov.
                                                
                                            </div>
                                            <div class="fc-input">
                                                <asp:DropDownList ID="ddlBillState" runat="server" CssClass="form-control"
                                                    TabIndex="16" ToolTip="State">
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
                                                <asp:TextBox ID="txtBillZip" runat="server" CssClass="form-control" MaxLength="10"
                                                    TabIndex="17"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Phone
                                               
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtBillPhone" runat="server" CssClass="form-control" MaxLength="28"
                                                    TabIndex="18"></asp:TextBox>
                                                <asp:MaskedEditExtender ID="txtBillPhone_MaskedEditExtender" runat="server" AutoComplete="False"
                                                    ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtBillPhone">
                                                </asp:MaskedEditExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Fax
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" MaxLength="28"
                                                    TabIndex="19"></asp:TextBox>
                                                <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False"
                                                    ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                    CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                    CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                    MaskType="Number" TargetControlID="txtFax">
                                                </asp:MaskedEditExtender>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Website
                                                
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" TabIndex="20"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="clearfix"></div>
                                </asp:Panel>
                                <div class="clearfix"></div>

                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="lnkContactSave" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Panel1"
                                            ExpandControlID="Image1" CollapseControlID="Image1" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                            CollapsedImage="~/images/arrow_right.png" ImageControlID="Image1" Enabled="True" />
                                        <div id="Div1" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                            Contacts:
                                        <asp:Image ID="Image1" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                        </div>
                                        <asp:Panel ID="Panel1" runat="server" Style="width: 100%">

                                            <div style="background: #316b9d; width: 100%;">
                                                <ul class="lnklist-header lnklist-panel">
                                                    <li style="margin-left: 3px">
                                                        <asp:LinkButton ID="lnkAddnew" runat="server" CausesValidation="False" CssClass="icon-addnew" ToolTip="Add New"
                                                            TabIndex="21" OnClick="lnkAddnew_Click"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="False" CssClass="icon-edit" ToolTip="Edit"
                                                            TabIndex="23" OnClick="btnEdit_Click"></asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return SelectedRowDelete('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvContacts','contact');"
                                                            CausesValidation="False" CssClass="icon-delete" ToolTip="Delete"
                                                            TabIndex="22" OnClick="btnDelete_Click"></asp:LinkButton>
                                                    </li>
                                                    <%--<div style="background-position: #B8E5FC; background: #316b9d; width: auto; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;">--%>
                                                </ul>
                                            </div>
                                            <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                Width="100%" EmptyDataText="No records to display">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkSelectCon" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex %>"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblId0" runat="server" Text='<%# Bind("ContactID") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName0" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
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
                                                            <a href='<%# "email.aspx?to=" + Eval("Email")+"&rol="+hdnRol.Value %>' target="_blank">
                                                                <asp:Label ID="lblEmail" runat="server"><%#Eval("Email")%></asp:Label></a>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <RowStyle CssClass="evenrowcolor" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                            </asp:GridView>
                                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                                title="Go to top">
                                                <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                Go To Top
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:Panel ID="pnlOpenTasks" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel2"
                                        ExpandControlID="Image2" CollapseControlID="Image2" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image2" Enabled="True" />
                                    <div id="Div2" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                        Open Tasks:
                                    <asp:Image ID="Image2" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel2" runat="server" Style="width: 100%">
                                        <div style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="icon-addnew" ToolTip="Add New" Target="_blank"></asp:HyperLink>
                                                </li>
                                            </ul>
                                        </div>
                                        <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" PageSize="20" EmptyDataText="No records to display">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") %>'
                                                            Target="_blank" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date/Date Done" SortExpression="duedate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                            Text='<%# Eval("duedate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="# Days" SortExpression="days">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" SortExpression="Remarks" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resolution" SortExpression="result" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResult" runat="server" Text='<%# Eval("result") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned to" SortExpression="fUser">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfUser" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                            title="Go to top">
                                            <img id="img1" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>

                                <asp:Panel ID="pnlTaskH" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender5" runat="server" TargetControlID="Panel3"
                                        ExpandControlID="Image3" CollapseControlID="Image3" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image3" Enabled="True" />
                                    <div id="Div3" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                        Task History:
                                    <asp:Image ID="Image3" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel3" runat="server" Style="width: 100%">
                                        <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:HyperLink ID="HyperLink1" CssClass="icon-addnew" runat="server" ToolTip="Add New" Target="_blank"></asp:HyperLink>
                                                </li>
                                            </ul>
                                        </div>
                                        <asp:GridView ID="gvTasksCompleted" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" PageSize="20" EmptyDataText="No records to display">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkSubject" NavigateUrl='<%# "addtask.aspx?uid=" + Eval("id") %>'
                                                            Target="_blank" runat="server" Text='<%# Eval("Subject") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Due Date/Date Done" SortExpression="duedate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDuedate" runat="server" Style='<%#  Eval("statusid").ToString()!="1" ?( string.Format("color:{0}",Convert.ToDateTime( Eval("duedate") )<= System.DateTime.Now ? "RED": "BLACK")) : "Black" %>'
                                                            Text='<%# Eval("duedate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="# Days" SortExpression="days">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldays" runat="server" Text='<%#   Eval("days").ToString().Replace("-",String.Empty) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" SortExpression="Remarks" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resolution" SortExpression="result" ItemStyle-Width="200px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblResult" runat="server" Text='<%# Eval("result") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="200px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assigned to" SortExpression="fUser">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblfUser" runat="server" Text='<%# Eval("fUser") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                            title="Go to top">
                                            <img id="img2" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                                <div class="clearfix"></div>

                                <asp:Panel ID="pnlOpp" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender6" runat="server" TargetControlID="Panel4"
                                        ExpandControlID="Image4" CollapseControlID="Image4" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image4" Enabled="True" />
                                    <div id="Div4" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                        Opportunities:
                                    <asp:Image ID="Image4" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel4" runat="server" Style="width: 100%">
                                        <div class="salesgriddiv" style="background: #316b9d; width: 100%;">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:HyperLink ID="lnkAddopp" CssClass="icon-addnew" ToolTip="Add New" runat="server" Target="_blank"></asp:HyperLink>
                                                </li>
                                            </ul>
                                        </div>
                                        <asp:GridView ID="gvOpportunity" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                            Width="100%" PageSize="20" EmptyDataText="No records to display">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Name" SortExpression="fdesc">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lnkname" NavigateUrl='<%# "addopprt.aspx?uid=" + Eval("id") %>'
                                                            Target="_blank" runat="server" Text='<%# Eval("fdesc") %>'></asp:HyperLink>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Close Date" SortExpression="duedate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbldate" runat="server" Text='<%# Eval("closedate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Probability" SortExpression="Probability">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProbab" runat="server" Text='<%# Eval("Probability") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                            title="Go to top">
                                            <img id="img3" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                                <div class="clearfix"></div>
                                <asp:Panel ID="pnlEmail" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender7" runat="server" TargetControlID="Panel5"
                                        ExpandControlID="Image5" CollapseControlID="Image5" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image5" Enabled="True" />
                                    <div id="Div5" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                        Emails:
                                    <asp:Image ID="Image5" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel5" runat="server" Style="width: 100%">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                            <ContentTemplate>
                                                <asp:Panel ID="Panel8" runat="server" class="salesgriddiv" Style="background: #316b9d; width: 100%;">
                                                    <ul class="lnklist-header lnklist-panel">
                                                        <li>
                                                            <asp:HyperLink ID="lnkNewEmail" Target="_blank" runat="server" CssClass="icon-mail" ToolTip="New Email"></asp:HyperLink></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkRefreshMails" runat="server" CausesValidation="False" OnClick="lnkRefreshMails_Click" CssClass="icon-refresh1" ToolTip="Refresh"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:HiddenField ID="hdnMailct" runat="server" />
                                                        </li>
                                                    </ul>
                                                </asp:Panel>
                                                <asp:GridView ID="gvmail" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    Width="100%" EmptyDataText="No records to display" PageSize="10" AllowPaging="True"
                                                    AllowSorting="True" OnDataBound="gvmail_DataBound" OnSorting="gvmail_Sorting"
                                                    OnRowCommand="gvmail_RowCommand">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <FooterStyle CssClass="footer" />
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Image runat="server" ID="imgType" Width="11px" ImageUrl='<%# Eval("type").ToString() != "0" ? "images/uparr.png" : "images/downarr.png"%>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Subject" SortExpression="subject" ItemStyle-Width="250px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkMsgID" Visible="false" runat="server" Text='<%# Eval("guid") %>'></asp:Label>
                                                                <asp:HyperLink ID="lnkSub" NavigateUrl='<%# "email.aspx?aid=" + Eval("guid") +"&rol="+hdnRol.Value %>'
                                                                    Target="_blank" runat="server" Text='<%# Eval("subject") %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From" SortExpression="[from]" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkFrom" runat="server" Text='<%# Eval("from") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To" ItemStyle-Width="150px">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lnkTo" runat="server" Text='<%# Eval("to") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date Sent" ItemStyle-Width="130px" SortExpression="SentDate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSentdate" runat="server" Text='<%# Eval("SentDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="130px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rec. Date" ItemStyle-Width="130px" SortExpression="recDate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRecdate" runat="server" Text='<%# Eval("recDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="130px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <PagerTemplate>
                                                        <div align="center">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png"
                                                                CausesValidation="false" />
                                                            &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                                CausesValidation="false" ImageUrl="~/images/Backward.png" />
                                                            &nbsp &nbsp <span>Page</span>
                                                            <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <span>of </span>
                                                            <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                            &nbsp &nbsp
                                                        <asp:ImageButton ID="ImageButton3" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png"
                                                            CausesValidation="false" />
                                                            &nbsp &nbsp
                                                        <asp:ImageButton ID="ImageButton4" runat="server" CommandArgument="Last" ImageUrl="images/last.png"
                                                            CausesValidation="false" />
                                                        </div>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                            title="Go to top">
                                            <img id="img4" alt="top" src="images/uptotop.gif" />
                                            Go To Top
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                                <div class="clearfix"></div>

                                <asp:Panel ID="pnlnotes" runat="server">
                                    <asp:UpdatePanel ID="updatepnl" runat="server">
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="gvDocuments" />
                                        </Triggers>
                                        <ContentTemplate>
                                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender8" runat="server" TargetControlID="Panel6"
                                                ExpandControlID="Image6" CollapseControlID="Image6" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                CollapsedImage="~/images/arrow_right.png" ImageControlID="Image6" Enabled="True" />
                                            <div id="Div6" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                                <span>Notes and Attachments</span>:
                                            <asp:Image ID="Image6" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                            </div>
                                            <asp:Panel ID="Panel6" runat="server" Style="width: 100%">
                                                <asp:Panel ID="pnlDocumentButtons" runat="server" class="salesgriddiv" Style="background: #316b9d; width: 100%;">
                                                    <ul class="lnklist-header lnklist-panel">
                                                        <li>
                                                            <asp:LinkButton ID="lnkEditNote" runat="server" CausesValidation="False" CssClass="icon-edit" ToolTip="Edit"
                                                                TabIndex="23" OnClick="lnkEditNote_Click"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" CssClass="icon-delete" ToolTip="Delete"
                                                                OnClientClick="return SelectedRowDelete('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvDocuments','note');"></asp:LinkButton></li>
                                                        <li>
                                                            <asp:LinkButton ID="lnkAddNote" runat="server" CausesValidation="False" OnClick="lnkAddNote_Click"
                                                                TabIndex="21" CssClass="icon-addnew" ToolTip="Add New"></asp:LinkButton></li>
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
                                                        <asp:TemplateField HeaderText="Subject">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSub" runat="server" Text='<%# Eval("subject") %>'></asp:Label>
                                                                <asp:Label ID="lblBody" Visible="false" runat="server" Text='<%# Eval("body") %>'></asp:Label>
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
                                                    </Columns>
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                </asp:GridView>
                                                <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px; padding: 10px"
                                                    title="Go to top">
                                                    <img id="img5" alt="top" src="images/uptotop.gif" />
                                                    Go To Top
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </asp:Panel>
                                <div class="clearfix"></div>
                                <asp:Panel ID="pnlSysInfo" runat="server">
                                    <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender9" runat="server" TargetControlID="Panel7"
                                        ExpandControlID="Image7" CollapseControlID="Image7" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                        CollapsedImage="~/images/arrow_right.png" ImageControlID="Image7" Enabled="True" />
                                    <div id="Div7" style="font-weight: bold; font-size: 15px; padding-bottom: 15px">
                                        <span>System Info</span>:
                                    <asp:Image ID="Image7" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                    </div>
                                    <asp:Panel ID="Panel7" runat="server" Style="width: 100%"
                                        CssClass="roundCorner">
                                        <table>

                                            <tr>
                                                <td width="70px">&nbsp;
                                                </td>
                                                <td width="70px">Created By
                                                </td>
                                                <td width="300px">
                                                    <asp:Label ID="lblCreate" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td width="100px">Last Updated By
                                                </td>
                                                <td width="300px">
                                                    <asp:Label ID="lblUpdate" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                            </tr>

                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="clearfix"></div>
                <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                    CausesValidation="False" />
                <asp:ModalPopupExtender runat="server" ID="ModalPopup" BehaviorID="Behavior" TargetControlID="hiddenTargetControlForModalPopup"
                    PopupControlID="pnlPop" RepositionMode="RepositionOnWindowResizeAndScroll">
                </asp:ModalPopupExtender>
                <div>
                    <asp:Panel ID="pnlPop" runat="server" Style="background-color: #fff; width: 400px; border: 1px solid #316b9d;">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <Triggers>
                                <asp:PostBackTrigger ControlID="lnkUploadDoc" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:Panel ID="pnlContact" runat="server" CssClass="model-popup">
                                    <div class="model-popup-body">
                                        <asp:Label CssClass="title_text" ID="Label1" runat="server">Add Contact</asp:Label>
                                        <asp:LinkButton ID="lnkCancelContact" runat="server" CausesValidation="False" OnClick="lnkCancelContact_Click"
                                            Style="float: right; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                                        <asp:LinkButton ID="lnkContactSave" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                            OnClick="lnkContactSave_Click"
                                            ValidationGroup="cont">Save</asp:LinkButton>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div style="padding: 10px">
                                        <table style="width: 100%; height: 250px; padding: 20px;">
                                            <tr>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td class="register_lbl">Contact Name
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtContcName"
                                            Display="None" ErrorMessage="Contact Name Required" SetFocusOnError="True" ValidationGroup="cont"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator12_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator12">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContcName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="register_lbl">Phone
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtContPhone"
                                            Display="None" ErrorMessage="Phone Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator13_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator13">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContPhone" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AutoComplete="False"
                                                        ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                        MaskType="Number" TargetControlID="txtContPhone">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="register_lbl">Fax
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContFax" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="False"
                                                        ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                        MaskType="Number" TargetControlID="txtContFax">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="register_lbl">Cell
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContCell" runat="server" CssClass="form-control" MaxLength="22"></asp:TextBox>
                                                    <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" AutoComplete="False"
                                                        ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                        MaskType="Number" TargetControlID="txtContCell">
                                                    </asp:MaskedEditExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="register_lbl">Email
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtContEmail"
                                            Display="None" ErrorMessage="Email Required" SetFocusOnError="True" ValidationGroup="cont"
                                            Enabled="False"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator16_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator16">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtContEmail"
                                                        Display="None" ErrorMessage="Invalid Email" ValidationGroup="cont" SetFocusOnError="True"
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="RegularExpressionValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RegularExpressionValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtContEmail" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAttach" runat="server">
                                    <div class="title_bar_popup">
                                        <asp:Label ID="Label2" runat="server" CssClass="title_text" Style="color: white">Note</asp:Label>
                                        <a onclick="$find('Behavior').hide()" style="cursor: pointer; float: right; color: #fff; margin-left: 10px; height: 16px;">Close</a>
                                        <asp:LinkButton ID="lnkUploadDoc" runat="server" Style="float: right; color: #fff; margin-left: 10px;"
                                            OnClick="lnkUploadDoc_Click" ValidationGroup="note">Save</asp:LinkButton>
                                    </div>
                                    <div class="clearfix"></div>
                                    <div style="padding: 10px">
                                        <div class="form-col">
                                            <div class="fc-label2">
                                                Subject<asp:HiddenField ID="hdnNoteID" runat="server" />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="txtNoteSub"
                                                    Display="None" ErrorMessage="Subject Required" SetFocusOnError="True" ValidationGroup="note"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator46_ValidatorCalloutExtender"
                                                    runat="server" Enabled="True" PopupPosition="BottomLeft" TargetControlID="RequiredFieldValidator46">
                                                </asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtNoteSub" runat="server" CssClass="form-control" MaxLength="70"
                                                    Width="250px"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label2">
                                                Body
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtNoteBody" runat="server" CssClass="form-control"
                                                    MaxLength="50" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label2">
                                                Attachment
                                            </div>
                                            <div class="fc-input">
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                                <asp:LinkButton ID="lnkPostback" runat="server" CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>
</asp:Content>
