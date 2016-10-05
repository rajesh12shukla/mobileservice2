
<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddRecContract.aspx.cs" Inherits="AddRecContract" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="js/jquery.formatCurrency.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            var billing = $("#<%=ddlBilling.ClientID%>").val();
            if (billing == 1)
            {
                $("#divCentral").show();
            }

            ///////////// Ajax call for customer auto search ////////////////////                         
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }
            $("#<%=txtCustomer.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetCustomer",
                        //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load customers");
                        }
                        //                        error: function(XMLHttpRequest, textStatus, errorThrown) {
                        //                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                        //                            alert(err.Message);
                        //                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                    $("#<%=txtLocation.ClientID%>").focus();
                    $("#<%=txtLocation.ClientID%>").val('');
                    $("#<%=hdnLocId.ClientID%>").val('');
                    document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
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


            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
                {
                    source: function (request, response) {
                        //                        if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnPatientId.ClientID%>').value != '') {
                            dtaaa.custID = document.getElementById('<%=hdnPatientId.ClientID%>').value;
                        }
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            //                              data: "{'prefixText':'" + request.term + "','con':'" + document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value + "','custID':" + document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value + "}",
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

                        //                        }
                        //                        else {
                        //                            alert('Please choose customer!');
                        //                            $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                        //                            $("#ctl00_ContentPlaceHolder1_txtCustomer").focus();
                        //                            return;
                        //                        }
                    },
                    select: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
                        $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                        document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtLocation.ClientID%>").val(ui.item.label);
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

            ///////////// Validations for auto search ////////////////////
            $("#<%=txtCustomer.ClientID%>").keyup(function (event) {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                if (document.getElementById('<%=txtCustomer.ClientID%>').value == '') {
                    hdnPatientId.value = '';
                }
            });

            $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                    hdnLocId.value = '';
                }
            });

            ///////////// Unit dropdown control handling ////////////////////

            $("#<%=txtUnit.ClientID%>").click(function () {
                $("#divEquip").slideToggle();
                return false;
            });


            /////////////// Format textboxes ///////////////////////////////

            $("#<%=txtBillAmt.ClientID%>").blur(function () {
                $("#<%=txtBillAmt.ClientID%>").formatCurrency();
            });

           <%-- $("#<%=txtDay.ClientID%>").keyup(function(event) {
                if (this.value == '') {
                    this.value = '1';
                }
            });--%>

            $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);
            $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {
                if ($(this).is(':checked')) {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', true);
                }
                else {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', false);
                }
                SelectRows('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnit.ClientID%>');
                CalculateAmount();
                CalculateHours();
            });
        });

        function CheckUncheckAllCheckBoxAsNeeded() {
            var totalCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").size();
            var checkedCheckboxes = $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox:checked").size();

            if (totalCheckboxes == checkedCheckboxes) {
                $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").attr('checked', true);
            }
            else {
                $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").attr('checked', false);
            }
        }

        function CalculateAmount() {
            var grid = document.getElementById('<%=gvEquip.ClientID%>');
            var txtAmount = document.getElementById('<%=txtBillAmt.ClientID%>');
            var cell;
            var total = 0;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[7];
                    cell1 = grid.rows[i].cells[0];

                    if (cell1.childNodes[3].checked == true) {
                        $(cell.childNodes[1]).formatCurrency();
                        if (cell.childNodes[1].value != '') {
                            var text = parseFloat(cell.childNodes[1].value.replace("$", "").replace(/,/g, ""));
                            //                        alert(text);                        
                            total = total + text;
                        }
                    }
                }
                txtAmount.value = total.toFixed(2);
                $("#<%=txtBillAmt.ClientID%>").formatCurrency();
            }
        }

        function CalculateHours() {
            var grid = document.getElementById('<%=gvEquip.ClientID%>');
            var txtHours = document.getElementById('<%=txtBillHours.ClientID%>');
            var cell;
            var total = 0;

            if (grid.rows.length > 0) {
                for (i = 1; i < grid.rows.length; i++) {
                    cell = grid.rows[i].cells[8];
                    cell1 = grid.rows[i].cells[0];

                    if (cell1.childNodes[3].checked == true) {
                        if (cell.childNodes[1].value != '') {
                            var text = parseFloat(cell.childNodes[1].value);
                            total = total + text;
                        }
                    }
                }
                txtHours.value = total.toFixed(2);
            }
        }

        function ChkGL(sender, args) {
            //debugger;
            var hdnGLAcct = document.getElementById('<%=hdnGLAcct.ClientID%>');
            if (hdnGLAcct.value == '') {
                args.IsValid = false;
            }
            if (hdnGLAcct.value == '0')
            {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnPatientId.value == '') {
                args.IsValid = false;
            }
        }

        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%=hdnPatientId.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
        ////////////// Display warning message while there is none contract exist /////////////////
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

        function onDdlBillingChange(val) {
            if (val == 1) {
                
                $("#divCentral").show();
                var countLocations = document.getElementById("<%=ddlSpecifiedLocation.ClientID%>").length
                if ((countLocations - 1) <= 0) {

                    noty({
                        text: 'You cannot select the \'Combined Billing\', As there are No Locations added yet.',
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
        function validateRecContr(val)
        {
            if(val == 1)
            {
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
            else if(val == 2)
            {
                noty({
                    text: 'You cannot select the \'Combined Billing\', As there are No Locations added yet.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });  
            }
        }
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            ///////////// Ajax call for GL acct auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("#<%=txtGLAcct.ClientID%>").autocomplete({

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
                             alert("Due to unexpected errors we were unable to load vendor name");
                         }
                     });
                 },
                 select: function (event, ui) {

                     $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);
                     $("#<%=hdnGLAcct.ClientID%>").val(ui.item.value);
                     return false;
                 },
                 focus: function (event, ui) {
                     $("#<%=txtGLAcct.ClientID%>").val(ui.item.label);

                     return false;
                 },
                 minLength: 0,
                 delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                //debugger;
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
                    return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + result_item + "</a>")
                    .appendTo(ul);
                }
                else {
                    return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + result_item + ", <span>" + result_desc + "</span></a>")
                    .appendTo(ul);
                }
            };           
        });
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
                    <asp:Panel runat="server" ID="pnlSave" >
                         <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Contract</asp:Label></li>
                            <li>
                                <asp:Label CssClass="title_text_Name" ID="lblContrName" runat="server"></asp:Label>
                                </li>
                            
                             <li>
                            <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                <ul class="lnklist-header">
                                     <li>
                                        <asp:LinkButton ID="lnkFirst" ToolTip="First" CssClass="icon-first" runat="server" CausesValidation="False"
                                            OnClick="lnkFirst_Click"></asp:LinkButton>
                                     </li>
                                     <li>
                                        <asp:LinkButton ID="lnkPrevious" CssClass="icon-previous" ToolTip="Previous" runat="server" CausesValidation="False"
                                            OnClick="lnkPrevious_Click"></asp:LinkButton>
                                     </li>
                                     <li>
                                        <asp:LinkButton ID="lnkNext" CssClass="icon-next" ToolTip="Next" runat="server" CausesValidation="False"
                                            OnClick="lnkNext_Click"></asp:LinkButton>
                                     </li>
                                     <li>
                                         <asp:LinkButton ID="lnkLast" ToolTip="Last" CssClass="icon-last" runat="server" CausesValidation="False"
                                            OnClick="lnkLast_Click"></asp:LinkButton>
                                     </li>  
                                </ul>
                            </asp:Panel>               
                             <li>
                                 <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ToolTip="Save" ValidationGroup="rec"
                                     TabIndex="21"></asp:LinkButton>
                             </li>
                            <li>                               
                                <asp:LinkButton CssClass="icon-closed" ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false"
                                    OnClick="lnkClose_Click" TabIndex="22"></asp:LinkButton>                                                        
                            </li>
                         </ul>
                    </asp:Panel>                 
                    
                </div>
            </div>
        <div class="col-lg-12 col-md-12">
            <div class="com-cont">
            <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
                <asp:TabPanel ID="tbpnlGeneral" runat="server" HeaderText="General">
                    <HeaderTemplate>
                        General
                    </HeaderTemplate>
                    <ContentTemplate>
                         <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                     
                    <input id="hdnCon" runat="server" type="hidden" />
                    <input id="hdnPatientId" runat="server" type="hidden" />
                    <input id="hdnLocId" runat="server" type="hidden" />
                    <input id="hdnUnit" runat="server" type="hidden" />
                    <asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button"
                        Style="display: none;" OnClick="btnSelectCustomer_Click" />
                    <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button"
                        Style="display: none;" OnClick="btnSelectLoc_Click" />

                    <div class="col-lg-8 col-md-8">
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Customer Name&nbsp;
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer" ValidationGroup="rec"
                        ErrorMessage="Please select the customer" ClientValidationFunction="ChkCustomer"
                        Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        PopupPosition="TopLeft" TargetControlID="CustomValidator1">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer" ValidationGroup="rec"
                                        Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender" 
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator19">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input" style="position: relative !important">
                                    <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control searchinputloc"
                                        TabIndex="1" autocomplete="off" placeholder="Search by customer name, phone#, address etc." 
                                        OnTextChanged="txtCustomer_TextChanged" AutoPostBack="true"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                        Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Location Name&nbsp;
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocation"
                        Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True" ValidationGroup="rec"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation"
                                        ErrorMessage="Please select the location" ClientValidationFunction="ChkLocation"
                                        Display="None" SetFocusOnError="True" ValidationGroup="rec"></asp:CustomValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="CustomValidator2">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control searchinputloc ui-autocomplete-input"
                                        TabIndex="2" autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                        Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    PO#
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtPO" runat="server" CssClass="form-control"
                                        MaxLength="25" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="col-lg-4 col-md-4">
                       
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Location Address
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control"
                                        MaxLength="255" TabIndex="3"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Equipments
                                </div>
                                <div class="fc-input">
                                    <div>
                                        <asp:TextBox ID="txtUnit" runat="server" CssClass="form-control" MaxLength="400"
                                            autocomplete="off" TabIndex="4" ReadOnly="True"></asp:TextBox>
                                        <div id="divEquip" class="menu_popup_grid shadow" >
                                            <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                DataKeyNames="ID" Width="604px" PageSize="20">
                                                <RowStyle CssClass="evenrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="0px">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" Style="display: none;" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                        <ItemStyle Width="0px"></ItemStyle>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Name" SortExpression="unit">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Bind("unit") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unique #" SortExpression="state">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUID" runat="server"><%#Eval("state")%></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Svc. type" SortExpression="cat">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblServiceType" runat="server"><%#Eval("cat")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"><%# Convert.ToInt32( Eval("status")) == 0 ? "Active" : "Inactive"%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtPrice" runat="server" Width="90px" MaxLength="20"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Hours">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Preferred Worker
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="form-control" TabIndex="5">
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
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                        TabIndex="6">
                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                        <asp:ListItem Value="1">Closed</asp:ListItem>
                                        <asp:ListItem Value="2">Hold</asp:ListItem>
                                        <asp:ListItem Value="3">Completed</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                         
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Service Type
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlServiceType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlServiceType_SelectedIndexChanged"
                                        TabIndex="7">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    GL Account
                                </div>
                                <div class="fc-input">
                                     <asp:TextBox ID="txtGLAcct" runat="server" CssClass="form-control" placeholder="Search by acct#, name"
                                         TabIndex="7"></asp:TextBox>
                                     <asp:HiddenField ID="hdnGLAcct" runat="server" />
                                     <asp:RequiredFieldValidator ID="rfvGLAcct" runat="server" ValidationGroup="rec" ControlToValidate="txtGLAcct"
                                        Display="None" ErrorMessage="Please enter GL account" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                     <asp:ValidatorCalloutExtender ID="vceGLAcct" runat="server" 
                                        TargetControlID="rfvGLAcct" PopupPosition="Right">
                                     </asp:ValidatorCalloutExtender>
                                    <asp:CustomValidator ID="cvGLAcct" runat="server" ControlToValidate="txtGLAcct" ValidationGroup="rec"
                                        ErrorMessage="Please select the GL account" ClientValidationFunction="ChkGL"
                                        Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                    <asp:ValidatorCalloutExtender ID="vceGLAcct1" runat="server" Enabled="True"
                                        PopupPosition="TopLeft" TargetControlID="cvGLAcct">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                            </div>
                        </div>
                   <%-- </div>
                    <div class="col-lg-4 col-md-4">--%>
                        <div class="form-group" >
                             <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label"><b>Escalation:</b></div>
                            </div>
                        </div>
                            <div class="form-col">
                                <div class="fc-label">
                                    Escalation Type
                                </div>
                                <div class="fc-input">
                                     <asp:DropDownList ID="ddlEscType"  runat="server" CssClass="form-control"  TabIndex="16">
                                         <asp:ListItem Value="3" >Manual</asp:ListItem>
                                            <asp:ListItem Value="0" >Commodity Index</asp:ListItem>
                                            <asp:ListItem Value="1" >Escalation</asp:ListItem>
                                            <asp:ListItem Value="2" >Return</asp:ListItem>
                                            
                                        </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                          <div class="form-group" >
                            <div class="form-col">
                                <div class="fc-label">
                                    Escalation Cycle
                                </div>
                                <div class="fc-input">
                                      <asp:TextBox ID="txtEscCycle" runat="server" CssClass="form-control" Rows="4"
                                        TabIndex="16"></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True"
                                        TargetControlID="txtEscCycle" ValidChars="1234567890">
                                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                          <div class="form-group" >
                            <div class="form-col">
                                <div class="fc-label">
                                    Escalation Factor
                                </div>
                                <div class="fc-input">
                                      <asp:TextBox ID="txtEscFactor" runat="server" CssClass="form-control"  Rows="4"
                                        TabIndex="16"></asp:TextBox>
                                     <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True"
                        TargetControlID="txtEscFactor" ValidChars="1234567890.">
                    </asp:FilteredTextBoxExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" >
                            <div class="form-col">
                                <div class="fc-label">
                                    Escalated Last
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtEscdue" runat="server" CssClass="form-control" MaxLength="28"
                                        TabIndex="16"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtEscdue_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtEscdue">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Expiration
                                </div>
                                <div class="fc-input">
                                      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                    <div style="padding-bottom: 15px">
                                        <asp:DropDownList ID="ddlExpiration" AutoPostBack="true" runat="server" CssClass="form-control"
                                            TabIndex="19" OnSelectedIndexChanged="ddlExpiration_SelectedIndexChanged">
                                           <asp:ListItem Value="" >Indefinitely</asp:ListItem>
                                            <asp:ListItem Value="1" >Contract expiration date</asp:ListItem>
                                            <asp:ListItem Value="2" style="display:none" >Number of frequencies</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <asp:TextBox ID="txtUnitExpiration" runat="server" CssClass="form-control" MaxLength="28"
                                        TabIndex="12" Visible="False"></asp:TextBox>

                                    <asp:CalendarExtender ID="txtUnitExpiration_CalendarExtender" runat="server" 
                                        TargetControlID="txtUnitExpiration">
                                    </asp:CalendarExtender>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtUnitExpiration" ValidationGroup="rec"
                                            Display="None" ErrorMessage="Expiration Date Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" 
                                            TargetControlID="RequiredFieldValidator3" PopupPosition="Right">
                                        </asp:ValidatorCalloutExtender>

                                    <asp:TextBox ID="txtNumFreq" runat="server" CssClass="form-control" MaxLength="28"
                                        TabIndex="12" Width="50px" Visible="False"></asp:TextBox>

                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNumFreq" ValidationGroup="rec"
                                            Display="None" ErrorMessage="Expiration Freq Required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                     <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" 
                                            TargetControlID="RequiredFieldValidator4" PopupPosition="Right">
                                        </asp:ValidatorCalloutExtender>
                              </ContentTemplate>
                        </asp:UpdatePanel>
                                </div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Contract Description
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Rows="4"
                                        TabIndex="16"></asp:TextBox>
                                     <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDescription" ValidationGroup="rec"
                                        Display="None" ErrorMessage="Please enter the description" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" 
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator5">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <b>Payment:</b>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Credit Card
                                </div>
                                <div class="fc-input">
                                    <asp:CheckBox ID="chkCredit" runat="server" TabIndex="20" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label"><b>Billing Recurring:</b></div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Billing Start Date
                                    <asp:RequiredFieldValidator ID="rfvBillStartDt" ValidationGroup="rec"
                                        runat="server" ControlToValidate="txtBillStartDt" Display="None" ErrorMessage="Billing Start Date Required"
                                        SetFocusOnError="True">
                                    </asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender
                                        ID="rfvBillStartDt_ValidatorCalloutExtender" runat="server" Enabled="True"
                                        TargetControlID="rfvBillStartDt" >
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtBillStartDt" runat="server" CssClass="form-control" MaxLength="28"
                                        TabIndex="8"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtBillStartDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtBillStartDt">
                                    </asp:CalendarExtender>
                                  
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Billing Frequency
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlBillFreq" runat="server" CssClass="form-control"
                                        TabIndex="9">
                                        <asp:ListItem Value="0">Monthly</asp:ListItem>
                                        <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                        <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                        <asp:ListItem Value="3">3 Times/Year</asp:ListItem>
                                        <asp:ListItem Value="4">Semi-Annually </asp:ListItem>
                                        <asp:ListItem Value="5">Annually </asp:ListItem>
                                        <asp:ListItem Value="6">Never </asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Billing Amount
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ValidationGroup="rec"
                                        ControlToValidate="txtBillAmt" Display="None" ErrorMessage="Billing Amount Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                            ID="RequiredFieldValidator25_ValidatorCalloutExtender" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator25" PopupPosition="Left">
                                        </asp:ValidatorCalloutExtender>
                                    
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtBillAmt" runat="server" CssClass="form-control" MaxLength="20"
                                        TabIndex="10"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label"><b>Customer Level:</b></div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                     <asp:Label ID="lblBilling" runat="server" Text="Billing"></asp:Label>
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
                        <div id="divCentral" class="form-group" style="display:none;">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblSpecifyLoc" runat="server" Text="Specify Location"></asp:Label>
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
                                <div class="fc-label"><b>Location Level:</b></div>
                            </div>
                        </div>
                         <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblContractBill" runat="server" Text="Contract Billing"></asp:Label>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlContractBill" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlContractBill_SelectedIndexChanged" AutoPostBack="true"
                                        TabIndex="8">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <b>Billing Rate:</b>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblBillRate" runat="server" text="Billing Rate"></asp:Label>
                                </div>
                                <div class="fc-input" style="padding-top:5px;">
                                        <asp:TextBox ID="txtBillRate" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblOt" runat="server" text="OT Rate"></asp:Label>
                                </div>
                                <div class="fc-input">
                                        <asp:TextBox ID="txtOt" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblNt" runat="server" text="1.7 Rate"></asp:Label>
                                </div>
                                <div class="fc-input">
                                        <asp:TextBox ID="txtNt" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblDt" runat="server" text="DT Rate"></asp:Label> 
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDt" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblTravel" runat="server" text="Travel Rate"></asp:Label> 
                                </div>
                                <div class="fc-input" style="padding-top:5px;">
                                    <asp:TextBox ID="txtTravel" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <asp:Label ID="lblMileage" runat="server" text="Mileage"></asp:Label> 
                                </div>
                                <div class="fc-input" style="padding-top:5px;">
                                    <asp:TextBox ID="txtMileage" runat="server" AutoCompleteType="None" CssClass="form-control"
                                        MaxLength="15" onkeypress="return isDecimalKey(this,event)" onchange="ConvertDigit(this)"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-4">
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    <b>Recurring Ticket Schedule:</b>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Schedule Start Date
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" ValidationGroup="rec"
                                        runat="server" ControlToValidate="txtScheduleStartDt" Display="None" ErrorMessage="Schedule Start Date Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    
                                    <asp:ValidatorCalloutExtender
                                        ID="RequiredFieldValidator26_ValidatorCalloutExtender" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator26" PopupPosition="Left">
                                    </asp:ValidatorCalloutExtender>
                                   
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtScheduleStartDt" runat="server" CssClass="form-control"
                                        MaxLength="28" TabIndex="11"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtScheduleStartDt_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtScheduleStartDt">
                                    </asp:CalendarExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Schedule Frequency
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlSchFreq" runat="server" CssClass="form-control"
                                        TabIndex="12">
                                        <asp:ListItem Value="-1">Never</asp:ListItem>
                                        <asp:ListItem Value="0">Monthly</asp:ListItem>
                                        <asp:ListItem Value="1">Bi-Monthly</asp:ListItem>
                                        <asp:ListItem Value="2">Quarterly</asp:ListItem>
                                        <asp:ListItem Value="3">Semi-Annually </asp:ListItem>
                                        <asp:ListItem Value="4">Annually</asp:ListItem>
                                       <%-- <asp:ListItem Value="5">Weekly</asp:ListItem>
                                        <asp:ListItem Value="6">Bi-Weekly</asp:ListItem>--%>
                                        <asp:ListItem Value="7">Every 13 Weeks</asp:ListItem>
                                        <asp:ListItem Value="10">Every 2 Years</asp:ListItem>
                                        <asp:ListItem Value="8">Every 3 Years</asp:ListItem>
                                        <asp:ListItem Value="9">Every 5 Years</asp:ListItem>
                                        <asp:ListItem Value="11">Every 7 Years</asp:ListItem>
                                        <asp:ListItem Value="12">On-Demand</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="display:none" >
                            <div class="form-col">
                                <div class="fc-label">
                                    Day
                                </div>
                                <div class="fc-input">
                                   <asp:TextBox ID="txtDay" runat="server" MaxLength="3" TabIndex="15" Width="50px"
                       ></asp:TextBox>
                    <asp:FilteredTextBoxExtender ID="txtDay_FilteredTextBoxExtender" runat="server" Enabled="True"
                        TargetControlID="txtDay" ValidChars="1234567890">
                    </asp:FilteredTextBoxExtender>
                    <asp:DropDownList ID="ddlDay" runat="server" TabIndex="16">
                        <asp:ListItem Value="0">Day</asp:ListItem>
                        <asp:ListItem Value="1">Monday</asp:ListItem>
                        <asp:ListItem Value="2">Tuesday</asp:ListItem>
                        <asp:ListItem Value="3">Wednesday</asp:ListItem>
                        <asp:ListItem Value="4">Thursday</asp:ListItem>
                        <asp:ListItem Value="5">Friday</asp:ListItem>
                        <asp:ListItem Value="6">Saturday</asp:ListItem>
                        <asp:ListItem Value="7">Sunday</asp:ListItem>
                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" style="display:block">
                            <div class="form-col">
                                <div class="fc-label">
                                    Weekends
                                </div>
                                <div class="fc-input">
                                    <asp:CheckBox ID="chkWeekends" runat="server" TabIndex="13" Enabled="true" Checked="true" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Scheduled Time <span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtsTime" ValidationGroup="rec"
                                            Display="None" ErrorMessage="Scheduled Time Required" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                            TargetControlID="RequiredFieldValidator27" PopupPosition="Left">
                                        </asp:ValidatorCalloutExtender>
                                    </span>

                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtsTime" runat="server" CssClass="form-control" MaxLength="10"
                                        TabIndex="14"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99:99" MaskType="Time"
                                        AcceptAMPM="True" TargetControlID="txtsTime">
                                    </asp:MaskedEditExtender>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Total Hours
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtBillHours" ValidationGroup="rec"
                        Display="None" ErrorMessage="Total Hours Required" SetFocusOnError="false"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                        TargetControlID="RequiredFieldValidator2" PopupPosition="BottomLeft">
                                    </asp:ValidatorCalloutExtender>

                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtBillHours" runat="server" CssClass="form-control" MaxLength="20"
                                        TabIndex="15"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>
                    
                    <div class="col-lg-12 col-md-12">
                       
                        <div class="form-group">
                            <div class="form-col">
                                <div class="fc-label">
                                    Remarks
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"
                                        MaxLength="8000" TabIndex="21"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="clearfix"></div>
                </div>
            </div>

                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="tbpnlCustom" runat="server" HeaderText="General">
                    <HeaderTemplate>
                        Custom
                    </HeaderTemplate>
                    <ContentTemplate>
                         <div class="col-lg-12 col-md-12">
                            <div class="com-cont">
                                 <div class="table-scrollable" style="border: none">
                                    <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                        PageSize="20" ShowFooter="true" Width="600px"
                                         ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNO" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFormat" runat="server" Text='<%# Eval("FieldControl") %>' Visible="false"
                                                        Width="300px"></asp:Label>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                    <asp:Label ID="lblValue" runat="server" Text='<%# Eval("Value") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblFormatID" runat="server" Text='<%# Eval("Format") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Desc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" runat="server" Text='<%# Eval("Label") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Value">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlFormat" runat="server" CssClass="form-control" Visible="false">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtValue" MaxLength="50" runat="server" Text='<%# Eval("Value") %>' 
                                                        Visible="false" CssClass="form-control"></asp:TextBox>

                                                  
                                                    <asp:CheckBox ID="chkValue" runat="server" Visible="false" Checked='<%# Eval("FieldControl").ToString().Equals("Checkbox") ? (Eval("Value") == DBNull.Value ? false :(Eval("Value").Equals("1") ? true : false)) : false %>' />
                                                    <asp:MaskedEditExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : (Eval("FieldControl").ToString()=="Date" ? true : false) %>'
                                                        TargetControlID="txtValue" ID="MaskedEditDate" runat="server" Mask="99/99/9999"
                                                        MaskType="Date" UserDateFormat="MonthDayYear">
                                                    </asp:MaskedEditExtender>
                                                     <asp:FilteredTextBoxExtender Enabled='<%# Session["MSM"].ToString() == "TS" ? false : ( Eval("FieldControl").ToString()=="Currency" ? true : false) %>'
                                                        ID="FilteredTextBoxExtender1" TargetControlID="txtValue" runat="server" ValidChars="0123456789.-+">
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
                         </div>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            </div>
        </div>
            <!-- edit-tab start -->
           
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
