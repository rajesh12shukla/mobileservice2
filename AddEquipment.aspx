<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddEquipment.aspx.cs" Inherits="AddEquipment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        var changes = 0;
        $(document).on("change", ":input", function () {
            changes = 1;
        });

        $(document).ready(function () {
            var queryloc = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%= hdnCon.ClientID %>').value;
                this.custID = null;
            }
            $("#<%= txtLocation.ClientID %>").autocomplete(
                {
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        var hdnpat = document.getElementById('<%= hdnPatientId.ClientID %>').value;
                        if (hdnpat != '') {
                            dtaaa.custID = hdnpat;
                        }

                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load customers");
                            }
                            //                            error: function(XMLHttpRequest, textStatus, errorThrown) {
                            //                                var err = eval("(" + XMLHttpRequest.responseText + ")");
                            //                                alert(err.Message);
                            //                            }
                        });

                    },
                    select: function (event, ui) {
                        $("#<%= txtLocation.ClientID %>").val(ui.item.label);
                        $("#<%= hdnLocId.ClientID %>").val(ui.item.value);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%= txtLocation.ClientID %>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
            .data("autocomplete")._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
                var x = new RegExp('\\b' + queryloc, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                return $("<li></li>")
		        .data("item.autocomplete", item)
		        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
		        .appendTo(ul);
            };


            $("#<%= txtLocation.ClientID %>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%= hdnLocId.ClientID %>');
                if (document.getElementById('<%= txtLocation.ClientID %>').value == '') {
                    hdnLocId.value = '';
                }
            });

            ///////////////// Prevent page from leaving before save ////////////////////
            if ($('#<%= btnSubmit.ClientID %>').length) {
                document.getElementById('<%= btnSubmit.ClientID %>').onclick = function () {
                    //            tabchng();                
                    debugger;
                    switchToTab();
                    if (Page_ClientValidate() == true) {
                        window.btn_clicked = true;
                    }
                };
            }

            window.onbeforeunload = function () {
                if (!window.btn_clicked) {
                    //if ($("#<%= hdnSaved.ClientID %>").val() != "1") {
                    if (changes == 1) {
                        return 'Changes not saved.';
                    }
                    //}
                }
            };

        });
        //////////////////////

        function ChangeConfirm(ddl) {
            var totalRows = $("#<%=gvCtemplItems.ClientID %> tr").length;
            if (totalRows > 0) {
                if (!confirm('Are you sure you want to change the template? This will erase the information under the current template.')) {
                    $(ddl).val($('#<%= hdnSelectedVal.ClientID %>').val());
                    return false;
                }
            }
        }

        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%= hdnLocId.ClientID %>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }


        ////////////////// Called when save form and validation caused on tab 1 //////////////
        function tabchng() {
            tc = $find("<%=TabContainer1.ClientID%>");
            tc.set_activeTabIndex(0);
        }

        function switchToTab() {
            var tabContainer = $find("<%= TabContainer1.ClientID %>");
            if (Page_ClientValidate('general') == false) {
                $find("<%= TabContainer1.ClientID %>").set_activeTabIndex(0);
            }
            else if (Page_ClientValidate('rep') == false) {
                $find("<%= TabContainer1.ClientID %>").set_activeTabIndex(2);
            }
        Page_ClientValidate();
    }

    function showModalPopupViaClientCust(lblTicketId, lblComp) {
        //            document.getElementById('<%= iframeCustomer.ClientID %>').src = "addticket.aspx?id=" + lblTicketId + "&comp=" + lblComp;
        //            var modalPopupBehavior = $find('PMPBehaviour');
        //            modalPopupBehavior.show();
        //alert(lblTicketId);

        if ('<%= Session["type"].ToString() %>' == 'c') {
            window.open('Printticket.aspx?id=' + lblTicketId + '&c=' + lblComp + '&pop=1', '_blank');
        }
        else {
            window.open('addticket.aspx?id=' + lblTicketId + '&comp=' + lblComp + '&pop=1', '_blank');
        }
    }

    function linkstyle(internet) {
        alert(internet);
        var style = "cursor:pointer";
        if ('<%= Session["type"].ToString() %>' == 'c') {
            if (internet == "0") { style = "color:black" }
        }
        alert(style);
        return style;
    }

    function hideModalPopup() {
        var modalPopupBehavior = $find('PMPBehaviour');
        modalPopupBehavior.hide();
        document.getElementById('<%= iframeCustomer.ClientID %>').src = "";
    }

    function ClientSidePrint(idDiv) {
        var w = 300;
        var h = 300;
        var l = (window.screen.availWidth - w) / 2;
        var t = (window.screen.availHeight - h) / 2;

        var sOption = "toolbar=no,location=no,directories=no,menubar=no,scrollbars=yes,width=" + w + ",height=" + h + ",left=" + l + ",top=" + t;
        // Get the HTML content of the div
        var sDivText = window.document.getElementById(idDiv).src;
        var unittext = window.document.getElementById('<%= txtEquipID.ClientID %>').value;
        var loctext = window.document.getElementById('<%= txtLocation.ClientID %>').value;
        // Open a new window
        var objWindow = window.open("", "Print", sOption);
        // Write the div element to the window

        objWindow.document.write("<div style='width:100%;'>");
        objWindow.document.write("<img id='img' src='" + sDivText + "'/>");
        objWindow.document.write("</div><div>" + unittext + " - " + loctext + "</div>");

        objWindow.document.close();

        setTimeout(function () { objWindow.print(); objWindow.close(); }, 1000);

    }

    </script>

    <style type="text/css">
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */ /*padding-right: 20px;*/
        }
        /* IE 6 doesn't support max-height
	     * we use height instead, but this forces the menu to always be this tall
	     */ * html .ui-autocomplete {
            height: 300px;
        }

        .highlight {
            background-color: Yellow;
        }

        .mycheckbox input[type="checkbox"] {
            margin-right: 5px;
        }

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
                    <a href="<%=ResolveUrl("~/equipments.aspx") %>">Equipments</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Equipment</span>
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Equipment</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblEquipName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:Panel ID="pnlNext" runat="server" Visible="false">
                                    <ul>
                                        <li style="margin: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" CssClass="icon-first" runat="server" CausesValidation="False"
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
                            </asp:Panel>
                        </li>

                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                ValidationGroup="general, rep" ToolTip="Save" TabIndex="16"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false"
                                OnClick="lnkClose_Click" TabIndex="17"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont" style="min-height: 450px;">
                    <input id="hdnPatientId" runat="server" type="hidden" />
                    <input id="hdnCon" runat="server" type="hidden" />
                    <input id="hdnLocId" runat="server" type="hidden" />
                    <input id="hdnSaved" runat="server" type="hidden" />
                    <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0">
                        <asp:TabPanel runat="server" ID="tpGeneral" HeaderText="General" TabIndex="0">
                            <ContentTemplate>
                                <asp:Panel ID="pnlEquipments" runat="server">
                                   
                                       
                                                <div style="float:right; width:500px">
                                                    <asp:Panel runat="server" ID="pnlQR" Visible="False">
                                                        <asp:Image ID="imgQR" runat="server" Height="250px" Width="250px" />
                                                        <a onclick="ClientSidePrint('<%= imgQR.ClientID %>');" style="cursor: pointer;" class="btn location-search" title="Print QR Code">
                                                            <i class="fa fa-print"></i></a>
                                                    </asp:Panel>
                                                </div>
                                           
                                        
                                    <div class="col-lg-6 col-md-6">
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Location Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                        ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                        SetFocusOnError="True" ValidationGroup="general"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation"
                                                        ErrorMessage="Please select the location" ClientValidationFunction="ChkLocation"
                                                        Display="None" SetFocusOnError="True" ValidationGroup="general"></asp:CustomValidator>
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                        TargetControlID="CustomValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control searchinputloc"
                                                        TabIndex="1" autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                                    <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                        Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2 col-md-2">
                                                    Equipment ID
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEquipID"
                                                    Display="None" ErrorMessage="Equipment ID Required" SetFocusOnError="True" ValidationGroup="general"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator2_ValidatorCalloutExtender"
                                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator2">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtEquipID" runat="server" CssClass="form-control" MaxLength="20"
                                                        TabIndex="2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Manufacturer
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtManuf" runat="server" CssClass="form-control searchinputloc"
                                                        TabIndex="3" autocomplete="off" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Description
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" MaxLength="50"
                                                        TabIndex="4"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Type
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control"
                                                        TabIndex="5">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Service type
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control"
                                                        TabIndex="6">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Category
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control"
                                                        TabIndex="7">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    <%--</div>
                                    <div class="col-lg-6 col-md-6">--%>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Serial #
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtSerial" runat="server" CssClass="form-control searchinputloc"
                                                        TabIndex="9" autocomplete="off" MaxLength="50"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Unique #
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtUnique" runat="server" CssClass="form-control" MaxLength="25"
                                                        TabIndex="10"></asp:TextBox>
                                                </div>
                                              <%--  <div class="fc-input">
                                                    <asp:Panel runat="server" ID="pnlQR" Visible="False">
                                                        <asp:Image ID="imgQR" runat="server" Height="250px" Width="250px" />
                                                        <a onclick="ClientSidePrint('<%= imgQR.ClientID %>');" style="cursor: pointer;" class="btn location-search" title="Print QR Code">
                                                            <i class="fa fa-print"></i></a>
                                                    </asp:Panel>
                                                </div>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Installed
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtInstalled" runat="server" CssClass="form-control" MaxLength="50"
                                                        TabIndex="11"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtInstalled_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtInstalled">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Service since
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtSince" runat="server" CssClass="form-control" MaxLength="50"
                                                        TabIndex="12"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtSince_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtSince">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Last Service
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtLast" runat="server" CssClass="form-control" MaxLength="50"
                                                        TabIndex="13"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtLast_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtLast">
                                                    </asp:CalendarExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Price
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" MaxLength="28"
                                                        TabIndex="14">00.00</asp:TextBox>
                                                    <asp:MaskedEditExtender ID="txtPrice_MaskedEditExtender" runat="server" Enabled="False"
                                                        Mask="9,999,999.99" TargetControlID="txtPrice" MaskType="Number" DisplayMoney="Left"
                                                        CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                        CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                        CultureTimePlaceholder="">
                                                    </asp:MaskedEditExtender>
                                                    <asp:FilteredTextBoxExtender ID="txtPrice_FilteredTextBoxExtender" runat="server"
                                                        TargetControlID="txtPrice" ValidChars="0123456789." Enabled="True">
                                                    </asp:FilteredTextBoxExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label col-md-2">
                                                    Status
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="rbStatus" runat="server" CssClass="form-control"
                                                        TabIndex="15">
                                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    
                                    <div class="form-group col-lg-10">
                                        <div class="form-col">
                                            <div class="fc-label col-md-2">Remarks</div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtRemarks" runat="server" Height="57px" TextMode="MultiLine"
                                                    TabIndex="8" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
 
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel runat="server" ID="tpCustom" HeaderText="Custom" Visible="true" TabIndex="0">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-lg-6">
                                            <div class="form-group">
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        <label>Select Template</label>
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:DropDownList ID="ddlCustTemplate" onchange="ChangeConfirm(this);" CssClass="form-control"
                                                            runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCustTemplate_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                        <input id="hdnSelectedVal" runat="server" type="hidden" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-10">
                                            <div class="table-scrollable" style="border: none">
                                                <asp:GridView ID="gvCtemplItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    PageSize="20" ShowFooter="true" Width="600px">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="SNO" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("formatmom") %>' Visible="false"
                                                                    Width="300px"></asp:Label>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Eval("customid") %>' Visible="false"></asp:Label>
                                                                <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                <asp:Label ID="lblValueh" runat="server" Text='<%# Eval("Value") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Desc">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Value">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlFormat" runat="server" CssClass="form-control" Visible="false">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="lblValue" MaxLength="50" runat="server" Text='<%# Eval("Value") %>' 
                                                                    Visible='<%# Eval("formatmom").ToString()=="Dropdown" ? false : true %>' CssClass="form-control"></asp:TextBox>
                                                                <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : (Eval("formatmom").ToString()=="Short Date" ? true : false) %>'
                                                                    TargetControlID="lblValue" ID="MaskedEditDate" runat="server" Mask="99/99/9999"
                                                                    MaskType="Date" UserDateFormat="MonthDayYear">
                                                                </asp:MaskedEditExtender>
                                                                <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Short Time" ? true : false) %>'
                                                                    ID="MaskedEditTime" runat="server" AcceptAMPM="True" Mask="99:99" MaskType="Time"
                                                                    TargetControlID="lblValue">
                                                                </asp:MaskedEditExtender>
                                                                <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Currency" ? true : false) %>'
                                                                    ID="FilteredTextBoxExtender1" TargetControlID="lblValue" runat="server" ValidChars="0123456789.-+">
                                                                </asp:FilteredTextBoxExtender>
                                                                <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("formatmom").ToString()=="Numeric" ? true : false) %>'
                                                                    ID="FilteredTextBoxExtender2" TargetControlID="lblValue" runat="server" ValidChars="0123456789.-+">
                                                                </asp:FilteredTextBoxExtender>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                    <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>
                                                                </div>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle CssClass="footer" />
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel runat="server" ID="tpREP" HeaderText="MCP Template" Visible="true" Style="min-height: 350px" TabIndex="0">
                            <ContentTemplate>
                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>
                                        <div class="col-lg-12" style="padding-bottom: 20px">
                                            <div style="float: left; background-color: #ccc;"
                                                class="roundCorner">
                                                <asp:GridView ID="gvSelectTemplate" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField SortExpression="fdesc">
                                                            <HeaderTemplate>
                                                                Template
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lblRepTempName" OnClick="cbRepTemplate_SelectedIndexChanged"
                                                                    CausesValidation="false" runat="server" Text='<%# Eval("fdesc") %>' OnClientClick="changes = 1;"></asp:LinkButton>
                                                                <asp:Label ID="lblRepTempId" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Last Date
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtStartDate" runat="server" Width="70px"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtStartDate">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="table-scrollable">                                            
                                            <asp:Panel runat="server" ID="pnlGridButtons" Style="background: #316b9d; width: 100%;">
                                                <ul class="lnklist-header lnklist-panel">
                                                    <li>
                                                        <asp:LinkButton ID="btnAddNewItem" runat="server" OnClick="btnAddNewItem_Click" ToolTip="Add New" CssClass="icon-addnew"
                                                            CausesValidation="False"></asp:LinkButton></li>
                                                    <li>
                                                        <asp:LinkButton ID="btnDeleteItem" runat="server" CausesValidation="False" CssClass="icon-delete" ToolTip="Delete"
                                                            OnClick="btnDeleteItem_Click"></asp:LinkButton></li>
                                                </ul>
                                                </asp:Panel>
                                                <asp:GridView ID="gvTemplateItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    Width="100%" AllowSorting="true" OnSorting="gvTemplateItems_Sorting">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                Code
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Section">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSection" MaxLength="25" runat="server" Text='<%# Eval("section") %>'
                                                                    Width="100px"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Enabled="false" Text='<%# Eval("Name") %>'
                                                                    Width="120px"></asp:Label>
                                                                <asp:Label ID="lblEquipT" Visible="false" runat="server" Text='<%# Eval("EquipT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Desc" SortExpression="fdesc">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("fdesc") %>' Width="200px"></asp:TextBox>
                                                                <asp:RequiredFieldValidator ID="rfvDesc" runat="server" ValidationGroup="rep" ControlToValidate="lblDesc"
                                                                    Display="Dynamic" ErrorMessage="*" SetFocusOnError="True" Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                            <FooterStyle VerticalAlign="Middle" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtLdate" runat="server" OnTextChanged="txtLdate_TextChanged" AutoPostBack="true"
                                                                    Width="70px" Text='<%#Eval("lastdate").ToString().Length>0? Convert.ToDateTime(Eval("lastdate")).ToShortDateString():"" %>'></asp:TextBox>
                                                                <asp:CalendarExtender ID="lblLdate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtLdate">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Freq.">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlFreq" Width="100px" runat="server" SelectedValue='<%# Eval("Frequency") %>'
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlFreq_SelectedIndexChanged">
                                                                     <asp:ListItem Value="-1">-Select-</asp:ListItem>
                                                            <asp:ListItem Value="0">Daily</asp:ListItem>
                                                            <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                            <asp:ListItem Value="2">Bi-Weekly</asp:ListItem>
                                                            <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                            <asp:ListItem Value="4">Bi-Monthly</asp:ListItem>
                                                            <asp:ListItem Value="5">Quarterly</asp:ListItem>
                                                            <asp:ListItem Value="6">Semi-Annually </asp:ListItem>
                                                            <asp:ListItem Value="7">Annually</asp:ListItem>
                                                            <asp:ListItem Value="8">One Time</asp:ListItem>
                                                            <asp:ListItem Value="9">3 Times a Year</asp:ListItem>
                                                            <asp:ListItem Value="10">Every 2 Year</asp:ListItem>
                                                            <asp:ListItem Value="11">Every 3 Year</asp:ListItem>
                                                            <asp:ListItem Value="12">Every 5 Year</asp:ListItem>
                                                            <asp:ListItem Value="13">Every 7 Year</asp:ListItem>
                                                            <asp:ListItem Value="14">On-Demand</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Next Due Date">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtDuedate" Width="70px" runat="server" Text='<%#Eval("NextDateDue").ToString().Length>0? Convert.ToDateTime(Eval("NextDateDue")).ToShortDateString():"" %>'></asp:TextBox>
                                                                <asp:CalendarExtender ID="lblDuedate_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtDuedate">
                                                                </asp:CalendarExtender>
                                                                <asp:RequiredFieldValidator ID="rfvNextDate" runat="server" ValidationGroup="rep"
                                                                    ControlToValidate="txtDuedate" Display="Dynamic" ErrorMessage="*" SetFocusOnError="True"
                                                                    Font-Bold="True" Font-Size="Larger"></asp:RequiredFieldValidator>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel runat="server" ID="tpnlREPH" HeaderText="MCP History" Visible="false"
                            TabIndex="3">
                            <ContentTemplate>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="search-customer">
                                            <div class="sc-form">
                                                Search
                                                             <asp:DropDownList ID="ddlSearch" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSearch_SelectedIndexChanged"
                                                                 CssClass="form-control input-sm input-small">
                                                                 <asp:ListItem Value=" ">--Select--</asp:ListItem>
                                                                 <asp:ListItem Value="rd.ticketID">Ticket #</asp:ListItem>
                                                                 <asp:ListItem Value="fwork">Worker</asp:ListItem>
                                                                 <asp:ListItem Value="rd.Code">Code</asp:ListItem>
                                                                 <asp:ListItem Value="eti.fDesc">Desc</asp:ListItem>
                                                                 <asp:ListItem Value="template">Template</asp:ListItem>
                                                                 <asp:ListItem Value="eti.frequency">Frequency</asp:ListItem>
                                                             </asp:DropDownList>
                                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control input-sm input-small" Width="183px"></asp:TextBox>
                                                <asp:TextBox ID="txtCodeSearch" runat="server" CssClass="form-control input-sm input-small"
                                                    Visible="False" Width="183px" autocomplete="off"></asp:TextBox>
                                                <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtCodeSearch"
                                                    EnableCaching="False" ServiceMethod="GetCodes" UseContextKey="True" MinimumPrefixLength="0"
                                                    CompletionListCssClass="autocomplete_completionListElement" CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" ID="AutoCompleteExtender"
                                                    DelimiterCharacters="" CompletionInterval="250">
                                                </asp:AutoCompleteExtender>
                                                <asp:DropDownList ID="ddlTemplate" runat="server" Visible="false" CssClass="form-control input-sm input-small">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlFreq" runat="server" Visible="false" CssClass="form-control input-sm input-small">
                                                    <asp:ListItem Value="0">Daily</asp:ListItem>
                                                    <asp:ListItem Value="1">Weekly</asp:ListItem>
                                                    <asp:ListItem Value="2">Bi-Weekly</asp:ListItem>
                                                    <asp:ListItem Value="3">Monthly</asp:ListItem>
                                                    <asp:ListItem Value="4">Bi-Monthly</asp:ListItem>
                                                    <asp:ListItem Value="5">Quarterly</asp:ListItem>
                                                    <asp:ListItem Value="6">Semi-Annually </asp:ListItem>
                                                    <asp:ListItem Value="7">Annually</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="ddlDates" runat="server" CssClass="form-control input-sm input-small">
                                                    <asp:ListItem Value="0">Last Date</asp:ListItem>
                                                    <asp:ListItem Value="1">Next Due Date</asp:ListItem>
                                                </asp:DropDownList>
                                                From
                                                             <asp:TextBox ID="txtfromDate" runat="server" CssClass="form-control input-sm input-small" MaxLength="50"
                                                                 Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtfromDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtfromDate">
                                                </asp:CalendarExtender>
                                                To
                                                             <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control input-sm input-small" MaxLength="50"
                                                                 Width="100px"></asp:TextBox>
                                                <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtToDate">
                                                </asp:CalendarExtender>
                                                <asp:LinkButton ID="btnSearch" runat="server" CausesValidation="False" CssClass="btn submit"
                                                    OnClick="btnSearch_Click" TabIndex="23" ToolTip="Search"><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                            <ul>
                                                <li>
                                                    <asp:LinkButton ID="lnkclear" runat="server" OnClick="lnkclear_Click">Clear</asp:LinkButton></li>
                                                <li>
                                                    <asp:Label ID="lblRecordCountHist" runat="server" Style="font-style: italic;"></asp:Label></li>
                                            </ul>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="clearfix"></div>
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" />
                                    </Triggers>
                                    <ContentTemplate>
                                         <asp:Panel runat="server" ID="Panel2" Style="background: #316b9d; margin-top:10px">
                                            <ul class="lnklist-header lnklist-panel">
                                                <li>
                                                    <asp:HyperLink ID="HyperLink2" runat="server" CssClass="icon-addnew" ToolTip="Add New" 
                                                         Target="_blank"></asp:HyperLink>
                                                   </li>
                                            </ul>
                                        </asp:Panel>
                                        <asp:GridView ID="gvRepDetails" runat="server" AutoGenerateColumns="False" CssClass="altrowstable"
                                            Width="1300px" PageSize="10" AllowPaging="true" ShowFooter="True" AllowSorting="True"
                                            OnDataBound="gvRepDetails_DataBound" OnRowCommand="gvRepDetails_RowCommand" OnSorting="gvRepDetails_Sorting">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <RowStyle CssClass="evenrowcolor" />
                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Ticket #" SortExpression="ticketid">
                                                    <ItemTemplate>
                                                        <a style="cursor: pointer"
                                                            onclick='javascript:showModalPopupViaClientCust(<%# Eval("ticketid") %>,<%# Eval("comp") %>);'>
                                                            <asp:Label ID="lblTicketId" runat="server" Text='<%# Bind("ticketid") %>'></asp:Label>
                                                        </a>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Worker" SortExpression="fwork">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblWork" runat="server" Text='<%# Bind("fwork") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Code" SortExpression="Code">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCode" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Section" SortExpression="Section">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSection" runat="server" Text='<%# Bind("Section") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Template" SortExpression="template">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTemplate" runat="server" Text='<%# Bind("template") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Desc" SortExpression="fDesc">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("fDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Frequency" SortExpression="freq">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFreq" runat="server" Text='<%# Bind("freq") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comments" SortExpression="comments">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComments" runat="server" Text='<%# Bind("comment") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Date" SortExpression="Lastdate">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastdate" runat="server" Text='<%# Eval("LastDate", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Next Due Date " SortExpression="NextDateDue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNextDateDue" runat="server" Text='<%# Eval("NextDateDue", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerTemplate>
                                                <div align="center">
                                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" CausesValidation="false"
                                                        ImageUrl="images/first.png" />
                                                    &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                        CausesValidation="false" ImageUrl="~/images/Backward.png" />
                                                    &nbsp &nbsp <span>Page</span>
                                                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
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
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </ContentTemplate>
                        </asp:TabPanel>
                    </asp:TabContainer>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>

    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
        BackgroundCssClass="ModalPopupBG" PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: 1px solid #316b9d;"
        CssClass="roundCorner shadow">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; color: Black; text-align: center;">
        </asp:Panel>
        <div class="iframe-eqipment">
            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnlREPT" runat="server" Visible="false">
                        <div class="model-popup-body">
                            <asp:Label CssClass="title_text" ID="Label13" runat="server">Select MCP Template</asp:Label>
                            <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" Style="float: right; color: #fff; margin-left: 10px; height: 16px;"
                                OnClick="lnkCloseTemplate_Click">Close</asp:LinkButton>
                            <asp:LinkButton ID="lnkSaveTemplate" runat="server" Height="16px" Style="float: right; color: #fff; margin-left: 10px;"
                                ValidationGroup="templ00"
                                OnClick="lnkSaveTemplate_Click">Add</asp:LinkButton>
                        </div>
                        <asp:HiddenField ID="HdnEquipTempID" runat="server" />
                        <div style="padding: 10px">
                            <table style="padding: 20px;">
                                <tr>
                                    <td>
                                        <b>MCP Template</b>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRepTemp"
                                            Display="None" ErrorMessage="Required" SetFocusOnError="True" ValidationGroup="templ00"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                            TargetControlID="RequiredFieldValidator3">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlRepTemp" runat="server" CssClass="form-control"
                                            ValidationGroup="templ">
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Last Date
                                    </td>
                                    <td style="padding-top: 10px">
                                        <asp:TextBox ID="txtLastDate" runat="server" CssClass="form-control" MaxLength="25"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtLastDate">
                                        </asp:CalendarExtender>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>

    <asp:Button runat="server" ID="btnhidden" Style="display: none" CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
        TargetControlID="btnhidden" PopupControlID="Panel1"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: solid;">
        <div>
            <iframe id="iframeCustomer" runat="server" class="iframe-eqipment" frameborder="0"></iframe>
        </div>
    </asp:Panel>
    <input id="ctl00_ContentPlaceHolder1_hideModalPopupViaServer" style="display: none;"
        onclick="hideModalPopup();" type="button" value="button" />
</asp:Content>
