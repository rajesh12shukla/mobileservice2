<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddReceivePO.aspx.cs" Inherits="AddReceivePO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }
    </style>
    <script type="text/javascript">
        function isDecimalKey(el, evt) {

            var charCode = (evt.which) ? evt.which : event.keyCode;

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
        function isNum(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        function makeDecimal(obj)
        {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function pageLoad(sender, args) {
            $(document).ready(function () {

                ///////////// Ajax call for vendor auto search ////////////////////                
                function dta() {
                    this.prefixText = null;
                    //this.con = null;
                }
                $("#<%=txtPO.ClientID%>").change(function () {
                    debugger;
                    var dta1 = new dta();
                    dta1.prefixText = $(this).val();

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPOById",
                        data: '{"prefixText": "' + $(this).val() + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            //debugger;
                            var ui = $.parseJSON(data.d);
                            //debugger;
                            if (ui.length == 0) {
                                var strPo = $("#<%=txtPO.ClientID%>").val();
                                $("#<%=txtPO.ClientID%>").val('');
                                noty({
                                    text: 'PO #' + strPo + ' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: false,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                $("#<%=txtDueDate.ClientID%>").val(ui[0].Due);
                                $("#<%=txtVendor.ClientID%>").val(ui[0].VendorName);
                                $("#<%=txtAddress.ClientID%>").val(ui[0].Address);
                                $("#<%=hdnVendorID.ClientID%>").val(ui[0].Vendor);
                                $("#<%=txtShipTo.ClientID%>").val(ui[0].ShipTo);
                                $("#<%=txtCreatedBy.ClientID%>").val(ui[0].fBy);
                                $("#<%=txtStatus.ClientID%>").val(ui[0].StatusName);
                                $("#<%=txtComments.ClientID%>").val(ui[0].fDesc);
                                $("#<%=hdnAmount.ClientID%>").val(ui[0].Amount);
                             
                                document.getElementById('<%=btnSelectVendor.ClientID%>').click();
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load PO");
                        }
                    });

                });
                var query = "";
                function dtaa() {
                    this.prefixText = null;
                    this.con = null;
                    this.Acct = null;
                }
                $("#<%=txtVendor.ClientID%>").autocomplete({

                    source: function (request, response) {

                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetVendorName",
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
                        //debugger;
                        var str = ui.item.Name;
                        if (str == "No Record Found!") {
                            $("#<%=txtVendor.ClientID%>").val("");
                        }
                        else {
                            $("#<%=txtVendor.ClientID%>").val(ui.item.Name);
                            $("#<%=hdnVendorID.ClientID%>").val(ui.item.ID);
                            document.getElementById('<%=btnSelectVendor.ClientID%>').click();
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtVendor.ClientID%>").val(ui.item.Name);

                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                   .data("autocomplete")._renderItem = function (ul, item) {
                       var ula = ul;
                       var itema = item;
                       var result_value = item.ID;
                       var result_item = item.Name;
                       //var result_desc = item.acct;
                       var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                       result_item = result_item.replace(x, function (FullMatch, n) {
                           return '<span class="highlight">' + FullMatch + '</span>'
                       });
                       //if (result_desc != null) {
                       //    result_desc = result_desc.replace(x, function (FullMatch, n) {
                       //        return '<span class="highlight">' + FullMatch + '</span>'
                       //    });
                       //}

                       if (result_value == 0) {
                           return $("<li></li>")
                           .data("item.autocomplete", item)
                           .append("<a>" + result_item + "</a>")
                           .appendTo(ul);
                       }
                       else {
                           return $("<li></li>")
                           .data("item.autocomplete", item)
                           .append("<a>" + result_item + "</a>")
                           .appendTo(ul);
                       }

                   };
                $("[id*=chkSelectItem]").change(function () {
                    var chk = $(this).attr('id');
                    var txtReceive = document.getElementById(chk.replace('chkSelectItem', 'txtReceive'));
                    var lblDue = document.getElementById(chk.replace('chkSelectItem', 'lblOutstand'));
                    var txtReceiveQty = document.getElementById(chk.replace('chkSelectItem', 'txtReceiveQty'));
                    
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    
                    if ($(this).prop('checked') == true) {

                        $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        SelectedRowStyle('<%=gvPOItems.ClientID %>')
                    }
                    else if ($(this).prop('checked') == false) {
                        due = 0

                        $(txtReceiveQty).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        $(txtReceive).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        $(this).closest('tr').removeAttr("style");
                    }
                    GetTotal();
                });
                $("[id*=txtReceiveQty]").change(function () {
                    var txtReceiveQty = $(this).attr('id');
                    var lblDue = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'lblBOQty'));
                    var chk = document.getElementById(txtReceiveQty.replace('txtReceive', 'chkSelectItem'));
                    var qty = document.getElementById(txtReceiveQty.replace('txtReceiveQty', 'txtReceive'));
                    var due = parseFloat($(lblDue).text())

                    if ($(this).val() == '') {
                        $(this).val('0.00');
                    }
                    else {
                        var receive = parseFloat($(this).val());
                        $(qty).val('0.00');
                        if (receive > due) {
                            $(this).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        }
                        else {
                            $(this).val(receive.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        }
                    }
                    checkGrid();
                    GetTotal();
                });
                $("[id*=txtReceive]").change(function () {
                    var txtReceive = $(this).attr('id');
                    var lblDue = document.getElementById(txtReceive.replace('txtReceive', 'lblOutstand'));
                    var chk = document.getElementById(txtReceive.replace('txtReceive', 'chkSelectItem'));
                    var qty = document.getElementById(txtReceive.replace('txtReceive', 'txtReceiveQty'));
                    var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                    
                    if($(this).val() == '')
                    {
                        $(this).val('0.00');
                    }
                    else 
                    {
                        var receive = parseFloat($(this).val());
                        $(qty).val('0.00');
                        if(receive > due)
                        {
                            $(this).val(due.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        }
                        else {
                            $(this).val(receive.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                        }
                        
                    }
                    checkGrid();
                    GetTotal();
                });
            });
        }
        function checkGrid()
        {
            $("[id*=txtReceive]").each(function () {
                txtPay = $(this).attr('id');
                var txtQty = document.getElementById(txtPay.replace('txtReceive', 'txtReceiveQty'));
                var chk = document.getElementById(txtPay.replace('txtReceive', 'chkSelectItem'));

                if (parseFloat($(txtQty).val()) > 0 || parseFloat($(this).val()) > 0) {
                    $(chk).prop('checked', true);
                    SelectedRowStyle('<%=gvPOItems.ClientID %>')
                }
                else {
                    $(chk).prop('checked', false);
                    $(this).closest('tr').removeAttr("style");
                }
            });
        }
        function GetTotal() {
            var total = 0;
            var totalqty = 0;
            $("[id*=txtReceive]").each(function () {
                txtPay = $(this).attr('id');
                var expression = /[\(\)]/g                     // to identify parentheses
                var chk = document.getElementById(txtPay.replace('txtReceive', 'chkSelectItem'));
                
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {
                        debugger;
                        var val = $(this).val()

                        if (val.match(expression))     /// check is parentheses exists (negative value)
                        {
                            total = total - parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                        else {
                            total = total + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                        }
                    }
                }
            });
            var totalval = total.toLocaleString("en-US", { minimumFractionDigits: 2 })
            $("#<%=lblTotal.ClientID%>").text("$"+totalval);
            $("[id*=lblReceiveFooter]").text(totalval);

            $("[id*=txtReceiveQty]").each(function () {
                txtQty = $(this).attr('id');
                var chk = document.getElementById(txtQty.replace('txtReceiveQty', 'chkSelectItem'));
                if ($(chk).prop('checked') == true) {
                    if ($(this).val() != '') {
                        totalqty = totalqty + parseFloat($(this).val());
                    }
                }
            });
            var qtotalval = totalqty.toLocaleString("en-US", { minimumFractionDigits: 2 })
            $("[id*=lblReceiveQtyFooter]").text(qtotalval);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>

        <div class="page-content">

            <div class="page-cont-top">
            </div>
            <div class="clearfix"></div>
            <div class="row">
                <div class="col-lg-12 col-md-12">
                    <div class="pc-title">
                        <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add PO Reception</asp:Label></li>
                            <li>
                                <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                            <li>
                                <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                    <ul class="lnklist-header">
                                        <li style="margin-right: 0">
                                            <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" CssClass="icon-first">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" CssClass="icon-previous">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" CssClass="icon-next">
                                            </asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" CssClass="icon-last">
                                            </asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            </li>
                            <li>
                                <asp:Panel ID="pnlSave" runat="server">
                                    <ul>
                                        <li style="margin-right: 0">
                                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" runat="server" ToolTip="Save" TabIndex="38" OnClick="btnSubmit_Click"
                                                OnClientClick="itemJSON();" ValidationGroup="po"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" CausesValidation="false" ToolTip="Close"
                                                TabIndex="39" OnClick="lnkClose_Click"></asp:LinkButton></li>
                                    </ul>
                                </asp:Panel>
                            </li>
                        </ul>
                    </div>
                </div>

                <!-- edit-tab start -->
                <div class="col-lg-12 col-md-12">


                    <div class="com-cont">
                        <div class="alert alert-success" runat="server" id="divSuccess">
                            <button type="button" class="close" data-dismiss="alert">×</button>
                            These month/year period is closed out. You do not have permission to add/update this record.
                        </div>
                   <%-- <asp:HiddenField ID="hdnBatch" runat="server" />
                        <asp:HiddenField ID="hdnTransID" runat="server" />
                        <asp:HiddenField ID="hdnStatus" runat="server" />
                        <asp:HiddenField ID="hdnTotal" runat="server" />--%>
                        <asp:HiddenField ID="hdnItemJSON" runat="server" />
                        <div class="row">
                            <div id="pnlBills" class="pnlBills">
                                <div class="col-md-12 col-lg-12"> 
                                    <div class="col-md-12 col-lg-12"> 
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                            <div class="col-md-3 col-lg-3">
                                             <div class="form-col">
                                                <div class="fc-label">
                                                    <label>PO#</label>
                                                </div>
                                                <div class="fc-input">
                                                  
                                                    <asp:HiddenField ID="hdnAmount" runat="server" />
                                                    <asp:TextBox ID="txtPO" runat="server" CssClass="form-control" TabIndex="2" onkeypress="return isNum(event,this)"
                                                        MaxLength="50" autocomplete="off" placeholder="Enter PO#"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPO" ValidationGroup="po"
                                                        runat="server" ControlToValidate="txtPO" Display="None" ErrorMessage="Please enter PO"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvPO" />
                                                    <asp:Button ID="btnSelectPO" runat="server" OnClick="btnSelectPO_Click" Visible="false"/>
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Date  </label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="2"
                                                        MaxLength="15"  autocomplete="off" onkeypress="return false;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtDate" >
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvDate" ValidationGroup="po"
                                                        runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvDate" />
                                                    <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="txtDate" ValidationGroup="po"
                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                        TargetControlID="rfvDate1" />
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Due</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TabIndex="2"
                                                        onkeypress="return false;"></asp:TextBox>
                                                    <asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                        TargetControlID="txtDueDate">
                                                    </asp:CalendarExtender>
                                                    <asp:RequiredFieldValidator ID="rfvDueDate" ValidationGroup="po"
                                                        runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date is Required"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                    <asp:RegularExpressionValidator ID="rfvDueDate1" ControlToValidate="txtDueDate" ValidationGroup="po"
                                                        ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                        runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                    </asp:RegularExpressionValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                        TargetControlID="rfvDueDate1" />
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Ref</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtRef" runat="server" CssClass="form-control" TabIndex="2" autocomplete="off"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvRef"  ValidationGroup="po" 
                                                        runat="server" ControlToValidate="txtRef" Display="None" ErrorMessage="Ref is Required"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceRef" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvRef" />
                                                </div>
                                            </div>
                                        </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <div class="col-md-3 col-lg-3">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Vendor</label>
                                                    <asp:RequiredFieldValidator runat="server" ID="rfvVendor" ErrorMessage="Please select Vendor"
                                                        Display="None" ControlToValidate="txtVendor" ValidationGroup="po"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceVendor" runat="server" Enabled="True" PopupPosition="Right"
                                                        TargetControlID="rfvVendor" />
                                                    <asp:CustomValidator ID="cvVendor" runat="server" ClientValidationFunction="ChkVendor"
                                                        ControlToValidate="txtVendor" Display="None" ErrorMessage="Please select the vendor"
                                                        SetFocusOnError="True" Enabled="False"></asp:CustomValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceVendor1" runat="server" Enabled="True"
                                                        TargetControlID="cvVendor">
                                                    </asp:ValidatorCalloutExtender>
                                                    <asp:Button ID="btnSelectVendor" runat="server" CausesValidation="False" OnClick="btnSelectVendor_Click"
                                                        Style="display: none;" Text="Button" />
                                                </div>
                                                <div class="fc-input">
                                                <%--<asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control"
                                                    TabIndex="1">
                                                    </asp:DropDownList>--%>
                                                    <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" TabIndex="1" MaxLength="75"
                                                        placeholder="Search by vendor" OnTextChanged="txtVendor_TextChanged" autocomplete="off"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnVendorID" runat="server" />
                                                </div>
                                            </div>
                                            </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:UpdatePanel ID="updPnlAddress" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <div class="form-col" style="margin-bottom: 32px;">
                                                        <div class="fc-label">
                                                            Address
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtAddress" runat="server" Rows="3" CssClass="form-control" MaxLength="2000"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Trk/WB #</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtTrkWB" runat="server" CssClass="form-control" TabIndex="1" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-lg-3">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Reception#</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtReception" runat="server" Rows="3" CssClass="form-control" >
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                        runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required" ValidationGroup="po"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvShipTo" />
                                                </div>
                                            </div>
                                            <div class="form-col" style="margin-bottom: 32px;">
                                                <div class="fc-label">
                                                    <label>Ship To</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtShipTo" runat="server" Rows="3" CssClass="form-control" MaxLength="2000" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvShipTo"
                                                        runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required" ValidationGroup="po"
                                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceShipTo" runat="server" Enabled="True"
                                                        PopupPosition="Right" TargetControlID="rfvShipTo" />
                                                </div>
                                            </div>
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Created By</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="form-control" TabIndex="2"
                                                        MaxLength="50" autocomplete="off"></asp:TextBox>
                                                </div>
                                            </div>
                                             <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Status</label>
                                                </div>
                                                <div class="fc-input" style="padding-top: 5px">
                                                    <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control" TabIndex="2"
                                                        Text="New"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-lg-3">
                                            <div style="padding: 15px 15px 0 15px;" class="ajax__tab_xp ajax__tab_container ajax__tab_default">
                                                 Upcoming PO Receptions
                                                <asp:UpdatePanel ID="updPnlPo" runat="server">
                                                    <ContentTemplate>
                                                       <div class="table-scrollable">
                                                          <asp:GridView ID="gvPO" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                            CssClass="table table-bordered table-striped table-condensed flip-content"
                                                              ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                            <FooterStyle CssClass="footer" />
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-Width="1%">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelect_CheckedChanged" AutoPostBack="true" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-Width="1%" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" Text='<%# Bind("PO") %>' runat="server" />
                                                                        <asp:Label ID="lblfDate" Text='<%#  Eval("fdate")!=DBNull.Value? (String.Format("{0:MM/dd/yyyy}", Eval("fdate"))) : "" %>' runat="server" />
                                                                        <asp:Label ID="lblDue" Text='<%#  Eval("Due")!=DBNull.Value? (String.Format("{0:MM/dd/yyyy}", Eval("Due"))) : "" %>' runat="server" />
                                                                        <asp:Label ID="lblAmount" Text='<%# Bind("amount") %>' runat="server" />
                                                                        <asp:Label ID="lblComment" Text='<%# Bind("fDesc") %>' runat="server" />
                                                                        <asp:Label ID="lblVendorName" Text='<%# Bind("VendorName") %>' runat="server" />
                                                                        <asp:Label ID="lblVendor" Text='<%# Bind("Vendor") %>' runat="server" />
                                                                        <asp:Label ID="lblAddress" Text='<%# Bind("Address") %>' runat="server" />
                                                                        <asp:Label ID="lblStatus" Text='<%# Bind("Status") %>' runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="PO" HeaderText="PO#" ReadOnly="True" SortExpression="PO" />
                                                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" ReadOnly="True" SortExpression="VendorName"/>
                                                                <asp:BoundField DataField="Due" HeaderText="Due Date" ReadOnly="True" SortExpression="Due" DataFormatString="{0:MM/dd/yy}" />
                                                            </Columns>
                                                          </asp:GridView>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                        <div class="col-md-6 col-lg-6">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>PO Comments</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtComments" runat="server" Rows="3" CssClass="form-control" >
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-lg-3">
                                             <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Total</label>
                                                </div>
                                                <div class="fc-input" style="padding-top: 7px;">
                                                    <asp:Label ID="lblTotal" runat="server" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9 col-lg-9">
                                        <div class="col-md-12 col-lg-12"> 
                                            <div class="form-col"> 
                                                <div class="fc-label">
                                                    <label>Reception Comments</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtRcomments" runat="server" Rows="3" CssClass="form-control" MaxLength="2000" TextMode="MultiLine">
                                                    </asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-9 col-lg-9">
                                        <div class="col-md-12 col-lg-12"> 
                                            <div class="clearfix"></div>
                                            <asp:UpdatePanel ID="updPnlPOItem" runat="server">
                                                <ContentTemplate>
                                                    <div class="table-scrollable">
                                                        <asp:GridView ID="gvPOItems" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="True" EmptyDataText="No records Found" 
                                                        CssClass="table table-bordered table-striped table-condensed flip-content" >
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="1%">
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelectItem" runat="server"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" Text='<%# Bind("PO") %>' runat="server" />
                                                                    <asp:Label ID="lblLine" Text='<%# Bind("Line") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="fDesc" ItemStyle-Width="4%" HeaderText="Description" ReadOnly="True" SortExpression="fDesc" ItemStyle-VerticalAlign="Middle"/>
                                                            <asp:BoundField DataField="Due" ItemStyle-Width="2%" HeaderText="Due Date" ReadOnly="True" SortExpression="DueDate" DataFormatString="{0:MM/dd/yy}" ItemStyle-VerticalAlign="Middle" />
                                                            <asp:BoundField DataField="Job" ItemStyle-Width="5%" HeaderText="Project #" ReadOnly="True" SortExpression="Project" ItemStyle-VerticalAlign="Middle"/>
                                                            <asp:BoundField DataField="Phase" ItemStyle-Width="3%" HeaderText="Type" ReadOnly="True" SortExpression="Type" ItemStyle-VerticalAlign="Middle"/>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="Qty Ordered" SortExpression="Quan" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQuan" Text='<%# Eval("OrderedQuan")%>' runat="server" Style="width:100px;"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField ItemStyle-Width="4%" HeaderText="Ordered $" SortExpression="Ordered" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrdered" Text='<%# DataBinder.Eval(Container.DataItem, "Ordered", "{0:c}")%>' runat="server" Style="width:100px;"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="Prev Qty Received" SortExpression="PrvQty" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrvQty" Text='<%# Eval("PrvInQuan")%>' runat="server" Style="width:100px;"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="Prev Received $" SortExpression="PrvIn" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPrvIn" Text='<%# DataBinder.Eval(Container.DataItem, "PrvIn", "{0:c}")%>' runat="server" Style="width:100px;"/> <!-- Balance due balance --> 
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="BO Qty" SortExpression="Ordered" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBOQty" Text='<%# Eval("OutstandQuan")%>' runat="server" Style="width:100px;"/>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="BO $" SortExpression="Outstanding" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOutstand" Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0:c}")%>' runat="server" Style="width:100px;"/> <!-- Selected -->
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField ItemStyle-Width="4%" HeaderText="Received Qty" SortExpression="ReceivedQty" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                     <asp:TextBox ID="txtReceiveQty" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(this,event)" 
                                                                         Width="100%" Height="26px" Text='<%# Bind("ReceivedQuan") %>'  onblur="makeDecimal(this);"
                                                                         Style="font-size:12px;text-align:right;" autocomplete="off"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                     <asp:Label ID="lblReceiveQtyFooter" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="4%" HeaderText="Received $" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtReceive" runat="server" CssClass="form-control" onkeypress="return isDecimalKey(this,event)"
                                                                        Width="100%" Height="26px" Text='<%# Bind("Received") %>' Style="font-size:12px;text-align:right;" autocomplete="off"
                                                                        onblur="makeDecimal(this);"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                     <asp:Label ID="lblReceiveFooter" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <PagerTemplate>
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
                                                        </PagerTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- edit-tab end -->
                <div class="clearfix"></div>
            </div>
            <!-- END DASHBOARD STATS -->
            <div class="clearfix"></div>
        </div>

    </div>
</asp:Content>

