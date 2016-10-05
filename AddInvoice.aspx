<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddInvoice.aspx.cs" Inherits="AddInvoice" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
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
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load customers");
                            }
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
                    if (result_item) {
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_desc != null) {
                        result_desc = result_desc.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });
                    }
                    if (result_item) {
                        return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul)
                    }
                };
            });
            function dtaa() {

                this.prefixText = null;
                this.con = document.getElementById('<%=hdnCon.ClientID%>').value;
                this.custID = null;
            }
            ///////////// Ajax call for location auto search ////////////////////
            var queryloc = "";
            $("#<%=txtLocation.ClientID%>").autocomplete(
               {
                   source: function (request, response) {

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
                           data: JSON.stringify(dtaaa),
                           dataType: "json",
                           async: true,
                           success: function (data) {
                               response($.parseJSON(data.d));
                           },
                           error: function (result) {
                               alert("Due to unexpected errors we were unable to load customers");
                           }
                       });
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
                if (result_item) {
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                if (result_item) {
                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                        .appendTo(ul);
                }
            };
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

            $("#<%=txtProject.ClientID%>").focusin(function () {
                $("#divProject").slideDown();
            });
            $("#<%=txtProject.ClientID%>").focusout(function () {
                $("#divProject").slideUp();
            });
            $addHandler($get("hideModalPopupViaClientButton"), 'click', hideModalPopupViaClient);
        }
        function Billto() {
            if ($('#<%=chkBillTo.ClientID%>').is(':checked')) {

                $("#<%=txtAddress.ClientID%>").prop("disabled", false);

            } else {

                $("#<%=txtAddress.ClientID%>").prop("disabled", true);
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
            var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }

        function showModalPopupViaClientCust() {
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.show();
        }

        function EditInvoice() {
            document.getElementById('<%=btnEdit.ClientID%>').click();
        }


        function CalculateGridAmount() {
            var grid = document.getElementById('<%=gvInvoices.ClientID%>');
            var stax = document.getElementById('<%=hdnStax.ClientID%>');

            if (grid.rows.length > 0) {

                //                var PricePerTotal = grid.rows[grid.rows.length].cells[4].childNodes[1];
                //                var PretaxAmountTotal = grid.rows[grid.rows.length].cells[6].childNodes[1];
                //                var SalesTaxamountTotal = grid.rows[grid.rows.length].cells[5].childNodes[1];
                //                var AmountTotal = grid.rows[grid.rows.length].cells[7].childNodes[1];

                var PricePerTotal = 0;
                var PretaxAmountTotal = 0;
                var SalesTaxamountTotal = 0;
                var AmountTotal = 0;

                for (i = 1; i < grid.rows.length - 1; i++) {

                    var cell;
                    var cell1;
                    var cell2;
                    var Pretax = 0;
                    var staxAmt = 0;
                    var Amount = 0;

                    cell = grid.rows[i].cells[2];
                    cell1 = grid.rows[i].cells[5];
                    cell3 = grid.rows[i].cells[6];
                    cell5 = grid.rows[i].cells[7];
                    cell2 = grid.rows[i].cells[8];
                    cell4 = grid.rows[i].cells[9];

                    if (cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                        pretax = Math.round((parseFloat(cell.childNodes[1].value) * parseFloat(cell1.childNodes[1].value)) * 100) / 100;
                        cell3.childNodes[1].innerHTML = pretax.toFixed(2);
                    }

                    if (stax.value != '' && cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {
                        if (cell5.childNodes[1].checked == true) {
                            staxAmt = Math.round(((parseFloat(pretax) * parseFloat(stax.value)) / 100) * 100) / 100;
                        }
                        else {
                            staxAmt = 0.00;
                        }
                        cell2.childNodes[1].value = staxAmt.toFixed(2);
                    }

                    //                    if (cell.childNodes[1].value != '' && cell1.childNodes[1].value != '' && cell2.childNodes[1].value != '') {
                    //                        total = Math.round((parseFloat(cell.childNodes[1].value) * parseFloat(cell1.childNodes[1].value) * parseFloat(cell2.childNodes[1].value)) * 100) / 100;
                    //                        cell4.childNodes[1].innerHTML = total.toFixed(2);
                    //                    }
                    if (cell.childNodes[1].value != '' && cell1.childNodes[1].value != '') {

                        //                        if (cell5.childNodes[1].checked == true) {
                        //                            total = pretax + staxAmt;                           
                        //                        }
                        //                        else {
                        //                            total = pretax;
                        //                        }
                        total = pretax + staxAmt;
                        cell4.childNodes[1].innerHTML = total.toFixed(2);
                    }



                    if (cell1.childNodes[1].value != '') {
                        PricePerTotal = Math.round((parseFloat(PricePerTotal) + parseFloat(cell1.childNodes[1].value)) * 100) / 100;
                    }
                    if (cell3.childNodes[1].innerHTML != '') {
                        PretaxAmountTotal = Math.round((parseFloat(PretaxAmountTotal) + parseFloat(cell3.childNodes[1].innerHTML)) * 100) / 100;
                    }
                    if (cell2.childNodes[1].value != '') {
                        SalesTaxamountTotal = Math.round((parseFloat(SalesTaxamountTotal) + parseFloat(cell2.childNodes[1].value)) * 100) / 100;
                    }
                    if (cell4.childNodes[1].innerHTML != '') {
                        AmountTotal = Math.round((parseFloat(AmountTotal) + parseFloat(cell4.childNodes[1].innerHTML)) * 100) / 100;
                    }
                }
                grid.rows[grid.rows.length - 1].cells[5].childNodes[1].innerHTML = PricePerTotal.toFixed(2);
                grid.rows[grid.rows.length - 1].cells[6].childNodes[1].innerHTML = PretaxAmountTotal.toFixed(2);
                grid.rows[grid.rows.length - 1].cells[8].childNodes[1].innerHTML = SalesTaxamountTotal.toFixed(2);
                grid.rows[grid.rows.length - 1].cells[9].childNodes[1].innerHTML = AmountTotal.toFixed(2);
            }
        }

        function BillCodeChanged(ddlID, txtDesc, txtPP) {
            var ddl = document.getElementById(ddlID);
            var strBillid = ddl.options[ddl.selectedIndex].value;
            var Desc = document.getElementById(txtDesc);
            var PP = document.getElementById(txtPP);
            var hdnBillCode = document.getElementById('ctl00_ContentPlaceHolder1_hdnBillCodeJSON');
            debugger;
            if (hdnBillCode != '') {
                var json = hdnBillCode.value
                
                var obj = jQuery.parseJSON(json);

                jQuery.each(obj, function (key, val) {

                    var stop = 0;
                    jQuery.each(val, function (keyu, valu) {

                        if (keyu == "id" && valu == strBillid) {
                            stop = 1;
                        }

                        if (stop == 1 && keyu == "fDesc") {
                            Desc.value = valu;
                        }
                        if (stop == 1 && keyu == "Price1") {
                            PP.value = valu;
                        }
                    });
                });
            }
            SelectCode(ddlID)
        }

        function ConfirmCombineTicket(count) {
            var ret = confirm('There exists ' + count + ' more tickets for the same workorder. Do you want to include them with this invoice?');
            if (ret == true) {
                document.getElementById('<%=hdnCombine.ClientID%>').value = 1;
            }
            else {
                document.getElementById('<%=hdnCombine.ClientID%>').value = 0;
            }
        }

        function hideModalPopupViaClient(ev) {
            ev.preventDefault();
            var modalPopupBehavior = $find('programmaticModalPopupBehavior');
            modalPopupBehavior.hide();
        }

        function SelectCode(obj)
        {
            debugger;
            var ddlval = document.getElementById(obj).value;
            //var invServ = "<%=ViewState["InvServ"]%>";
            var billrate = $("#<%=hdnBillRate.ClientID%>").val();
            debugger;
            if (billrate != '')
            {
                lblPricePer = document.getElementById(obj.replace('ddlBillingCode', 'lblPricePer'));
                $(lblPricePer).val(billrate);
            }
            //if (invServ != '')
            //{
            //    if (ddlval == '') {
            //        document.getElementById(obj.id).value = invServ;
            //    }
            //}
            
        }
    </script>

    <style type="text/css">
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */ /*padding-right: 20px;*/
            position: absolute;
        }
        /* IE 6 doesn't support max-height
     * we use height instead, but this forces the menu to always be this tall
     */ * html .ui-autocomplete {
            height: 300px;
        }

        .highlight {
            background-color: Yellow;
        }

        #container {
            display: block;
            position: relative;
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
                    <span>Billing Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/invoices.aspx")%>">Invoices</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Invoice</span>
                </li>
            </ul>--%>
            <div class="page-bar-right">
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header lnklist-header-span">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Invoice</asp:Label></li>
                            <li>
                                <asp:Label CssClass="title_text_Name" ID="lblInvoice" runat="server"></asp:Label></li>
                            <li>
                                <asp:Label ID="lblInv" runat="server" Text="Invoice #" Visible="False"></asp:Label></li>

                            <li>
                                <asp:Label CssClass="title_text_Name" ID="Label1" runat="server"></asp:Label></li>
                            <li>
                                <asp:Label ID="Label3" runat="server" Text="Invoice #" Visible="False"></asp:Label></li>
                            <li>
                                <asp:Label ID="lblInvoiceName" runat="server" Visible="False" Style="font-weight: bold; font-size: 15px;"></asp:Label></li>
                            <li>
                                <asp:Panel ID="pnlSave" runat="server">
                                    <asp:Panel ID="pnlNext" runat="server" Visible="False">
                                        <ul class="lnklist-header">
                                            <li style="margin-right: 0">
                                                <asp:LinkButton ID="lnkFirst" ToolTip="First" runat="server" CausesValidation="False" CssClass="icon-first"
                                                    OnClick="lnkFirst_Click">
                                                </asp:LinkButton></li>
                                            <li>
                                                <asp:LinkButton ID="lnkPrevious" ToolTip="Previous" runat="server" CausesValidation="False" CssClass="icon-previous"
                                                    OnClick="lnkPrevious_Click">
                                                </asp:LinkButton></li>
                                            <li>
                                                <asp:LinkButton ID="lnkNext" ToolTip="Next" runat="server" CausesValidation="False" CssClass="icon-next"
                                                    OnClick="lnkNext_Click">
                                                </asp:LinkButton></li>
                                            <li>
                                                <asp:LinkButton ID="lnkLast" ToolTip="Last" runat="server" CausesValidation="False" CssClass="icon-last"
                                                    OnClick="lnkLast_Click">
                                                </asp:LinkButton></li>
                                        </ul>
                                    </asp:Panel>
                                </asp:Panel>
                            </li>
                            <li>
                                <asp:LinkButton ID="lnkPrint" CssClass="icon-print" runat="server" ToolTip="Print" TabIndex="16" CausesValidation="true" OnClick="lnkPrint_Click"> </asp:LinkButton></li>
                            <li>
                                <asp:LinkButton ID="lnkMakePayment" runat="server" CssClass="icon-makepayment" ToolTip="Make Payment" TabIndex="15" OnClick="lnkMakePayment_Click" Visible="false"></asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton CssClass="icon-save" ToolTip="Save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                                    TabIndex="14"></asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton CssClass="icon-closed" ID="lnkClose" ToolTip="Close" runat="server" CausesValidation="false" ForeColor="Red"
                                    OnClick="lnkClose_Click" TabIndex="15"></asp:LinkButton></li>
                        </ul>
                    </asp:Panel>
                </div>
            </div>
            
            <!-- edit-tab start -->
        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
            <ContentTemplate>
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div class="alert alert-warning" runat="server" id="divSuccess">
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        These month/year period is closed out. You do not have permission to add/update this record.
                    </div>

                    <input id="hdnCon" runat="server" type="hidden" />
                    <input id="hdnPatientId" runat="server" type="hidden" />
                    <input id="hdnLocId" runat="server" type="hidden" />
                    <input id="hdnFocus" runat="server" type="hidden" />
                    <input id="hdnStax" runat="server" type="hidden" />
                    <input id="hdnTaxRegion" runat="server" type="hidden" />
                    <input id="hdnBillCodeJSON" runat="server" type="hidden" />
                    <input id="hdnCombine" runat="server" type="hidden" />
                    <input id="hdnProjectId" runat="server" type="hidden" />
                    <input id="hdnTotalAmount" runat="server" type="hidden" />
                    
                    <div class="title_bar">
                    </div>
                    <div>
                        <div class="col-lg-12" style="text-align: center">
                            <p style="color: #2887B7; font-size: 35px; font-weight: bold; align-content: center">INVOICE</p>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-4 col-md-4 form-group">
                                <div style="padding: 10px 0px 10px 0px;">
                                    Customer
                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtCustomer"
                                        ErrorMessage="Please select the customer" ClientValidationFunction="ChkCustomer"
                                        Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="CustomValidator1">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer"
                                        Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="Right" TargetControlID="RequiredFieldValidator19">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtCustomer" runat="server"  CssClass="form-control searchinput" autocomplete="off"
                                        TabIndex="1" placeholder="Search by customer name, phone#, address etc."
                                        ToolTip="Customer Name "></asp:TextBox><%-- data-provide="typeahead" searchinput  --%>
                                    <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                        Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button CausesValidation="false" ID="btnSelectCustomer" runat="server" Text="Button"
                                        Style="display: none;" OnClick="btnSelectCustomer_Click"
                                        UseSubmitBehavior="False" />
                                </div>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Location
                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="txtLocation"
                                        ErrorMessage="Please select the location" ClientValidationFunction="ChkLocation"
                                        Display="None" SetFocusOnError="True"></asp:CustomValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        PopupPosition="Right" TargetControlID="CustomValidator2">
                                    </asp:ValidatorCalloutExtender>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLocation"
                                        Display="None" ErrorMessage="Location Name Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator1_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" TargetControlID="RequiredFieldValidator1">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control searchinputloc"
                                        TabIndex="3" autocomplete="off" placeholder="Search by location name, phone#, address etc."
                                        ToolTip="Location Name "></asp:TextBox>
                                    <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                        Enabled="false" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                    </asp:FilteredTextBoxExtender>
                                    <asp:Button CausesValidation="false" ID="btnSelectLoc" runat="server" Text="Button"
                                        Style="display: none;" OnClick="btnSelectLoc_Click" UseSubmitBehavior="False" />
                                </div>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Bill To<asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtAddress"
                                        Display="None" ErrorMessage="Bill To Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" TabIndex="5" Height="92px"
                                        TextMode="MultiLine" ToolTip="Address"
                                        CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-lg-4 col-md-4" style="text-align: center">
                                <asp:Label ID="lblCompNme" runat="server" Style="font-size: 20px; font-weight: bold; color: #000;"></asp:Label><br />
                                <asp:Label ID="lblCompAddress" runat="server"></asp:Label><br />
                                <asp:Label ID="lblCompCity" runat="server"></asp:Label>
                                <asp:Label ID="lblCompState" runat="server"></asp:Label>
                                <asp:Label ID="lblCompZip" runat="server"></asp:Label>
                                <asp:Label ID="lblCompphone" runat="server"></asp:Label><br />
                            </div>
                            <div class="col-lg-4 col-md-4">
                                <div style="padding: 10px 0px 10px 0px;">
                                    Invoice Date<asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server"
                                        ControlToValidate="txtInvoiceDate" Display="None" ErrorMessage="Invoice Date Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator36_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator36">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtInvoiceDate" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="2"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtInvoiceDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtInvoiceDate">
                                    </asp:CalendarExtender>
                                </div>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Manual Invoice #<asp:CheckBox ID="chkBillTo" runat="server" Text="Bill to Address "
                                        Visible="False" />
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="4" ToolTip="Invoice #"></asp:TextBox>
                                </div>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Project #
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox runat="server" MaxLength="10" TabIndex="6" CssClass="form-control" ID="txtProject" Width="50px" 
                                        autocomplete="off"></asp:TextBox>
                                </div>
                                
                                
                                <div id="divProject" class="menu_popup_chklst shadow" >
                                    <asp:GridView ID="gvProject" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                        DataKeyNames="ID" PageSize="20" Width="353px">
                                        <RowStyle CssClass="evenrowcolor" />
                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle Width="0px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date" SortExpression="fdate">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbldate" runat="server"><%#Eval("fdate", "{0:MM/dd/yyyy}")%></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                    </asp:GridView>
                                     <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                        <ContentTemplate>
                                    <asp:Button ID="btnGetCode" runat="server" CausesValidation="false" Text="Button" Style="display: none;" OnClick="btnGetCode_Click" />
                                            </ContentTemplate></asp:UpdatePanel>
                                </div>
                              
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-lg-8">
                                <div style="padding: 10px 0px 10px 0px;">
                                    Invoice Remarks
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtRemarks" runat="server" Height="70px" TextMode="MultiLine"
                                        MaxLength="8000" TabIndex="6" CssClass="form-control"></asp:TextBox>
                                </div>
                               
                            </div>
                            <div class="col-lg-2">
                                 <div style="padding: 25px 0px 10px 0px;">
                                     </div>
                                  <div class="fc-input">
                                     <asp:Image ID="imgVoid" runat="server" ImageUrl="~/images/icons/void.png" style="height:35px;"/>
                                     <asp:ImageButton ID="imgPaid" ImageUrl="~/images/icons/paid.png" runat="server" OnClick="imgPaid_Click" Height="40px"/>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-12">
                            <div class="col-md-3">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                <ContentTemplate>
                                <div style="padding: 10px 0px 10px 0px;">
                                    PO #
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtPO" runat="server" CssClass="form-control" MaxLength="25"
                                        TabIndex="7"></asp:TextBox>
                                </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Sales Tax Name with Rate
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtStaxrate" runat="server" CssClass="form-control" MaxLength="25"
                                        TabIndex="11"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div style="padding: 10px 0px 10px 0px;">
                                    Terms
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server"
                                                ControlToValidate="ddlTerms" Display="None" ErrorMessage="Terms Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator41_ValidatorCalloutExtender"
                                        runat="server" Enabled="True" PopupPosition="TopLeft"
                                        TargetControlID="RequiredFieldValidator41">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:DropDownList ID="ddlTerms" runat="server" CssClass="form-control" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlTerms_SelectedIndexChanged" TabIndex="8">
                                            </asp:DropDownList>

                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                             <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Department Type
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server"
                                        ControlToValidate="ddlDepartment" Display="None" ErrorMessage="Department Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator32_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" TargetControlID="RequiredFieldValidator32" PopupPosition="TopLeft">
                                    </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"
                                        TabIndex="12">
                                    </asp:DropDownList>
                                </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            </div>
                            <div class="col-md-3">
                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Due Date<asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server"
                                        ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date Required"
                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator34_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator34">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" MaxLength="50"
                                        TabIndex="9"></asp:TextBox>
                                    <asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                        TargetControlID="txtDueDate">
                                    </asp:CalendarExtender>
                                </div>
                                             </ContentTemplate>
                                    </asp:UpdatePanel>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Status
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"
                                        TabIndex="13">
                                        <asp:ListItem Value="0">Open</asp:ListItem>
                                        <asp:ListItem Value="1">Paid</asp:ListItem>
                                        <asp:ListItem Value="2">Voided</asp:ListItem>
                                        <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                        <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                        <asp:ListItem Value="5">Paid by Credit Card</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div>
                                </div>
                                <div class="fc-input">
                                </div>
                                <div style="padding: 10px 0px 10px 0px;">
                                    Worker<asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="ddlRoute"
                                        Display="None" ErrorMessage="Mech Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender ID="RequiredFieldValidator35_ValidatorCalloutExtender"
                                            runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator35">
                                        </asp:ValidatorCalloutExtender>
                                </div>
                                <div class="fc-input">
                                    <asp:DropDownList ID="ddlRoute" runat="server" CssClass="form-control"
                                        TabIndex="10">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin-top: 15px">
                            <div class="col-md-12">
                                <%--<div style="border-style: solid solid none solid; background-position: #B8E5FC; background: #B8E5FC; width: 100%; height: 25px; color: #23AEE8; font-weight: bold; font-size: 12px; padding-top: 5px; border-width: 1px; border-color: #a9c6c9;">
                                    
                                    
                                   
                                </div>--%>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                <div>
                                    <div style="background: #316b9d; width: 100%;">
                                        <ul class="lnklist-header lnklist-panel">
                                            <li>
                                                <asp:Label CssClass="title_text" ID="Label2" runat="server"></asp:Label>
                                            </li>
                                            <li>
                                                <asp:LinkButton CssClass="icon-addnew" ToolTip="Add New"
                                                    ID="lnkAddnew" runat="server"
                                                    CausesValidation="False" OnClick="lnkAddnew_Click" TabIndex="15"></asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton CssClass="icon-delete" ToolTip="Delete"
                                                    ID="btnDelete" runat="server" CausesValidation="False"
                                                    OnClick="btnDelete_Click" TabIndex="14" OnClientClick="return confirm('Are you sure you want to delete this record?')"></asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton Style="float: left; color: #2382B2; margin-right: 20px; margin-left: 10px;"
                                                    ID="btnEdit" runat="server" OnClick="btnEdit_Click" CausesValidation="False"
                                                    TabIndex="16" Visible="False">Edit</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                        <div class="portlet-body flip-scroll cont-table">
                 <input id="hdnBillRate" runat="server" type="hidden" />
                                            <asp:GridView ID="gvInvoices" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content" margin-bottom="0px"
                                                ShowFooter="True" TabIndex="0">
                                                <HeaderStyle CssClass="flip-content" />
                                                <RowStyle />
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <FooterStyle CssClass="footer" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnSelected" runat="server" />
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="ID" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" Visible="false" runat="server" Text='<%# Container.DataItemIndex %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Code">
                                                        <ItemTemplate>
                                                               <asp:DropDownList ID="ddlProjectCode" runat="server" CssClass="texttransparent" DataTextField="billtype"
                                                                DataValueField="line" DataSource='<%#dtProjectCodeData%>'   SelectedValue='<%# Eval("code") %>'
                                                                Width="190px" OnSelectedIndexChanged="ddlProjectCode_SelectedIndexChanged" AutoPostBack="true">
                                                                   <asp:ListItem Value="0">-Select-</asp:ListItem>
                                                            </asp:DropDownList>                                                            
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                  
                                                    <asp:TemplateField HeaderText="Quan">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblQuantity" runat="server" Text='<%# Bind("Quan") %>' MaxLength="15"
                                                                Width="60px" CssClass="texttransparent"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="lblQuantity_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="lblQuantity" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="lblQuantity"
                                                                Display="None" ErrorMessage="Quantity Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvQuantity_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvQuantity">
                                                            </asp:ValidatorCalloutExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Billing Code" SortExpression="billcode">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlBillingCode" runat="server" CssClass="texttransparent" DataTextField="BillType"
                                                                DataValueField="ID" DataSource='<%#dtBillingCodeData%>' SelectedValue='<%# Eval("acct") %>'
                                                                Width="150px" onfocustout="SelectCode(this);">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvBillCode" runat="server" ControlToValidate="ddlBillingCode"
                                                                Display="None" ErrorMessage="Billing Code Required" SetFocusOnError="True" Enabled="False"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvBillCode_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvBillCode">
                                                            </asp:ValidatorCalloutExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description" SortExpression="fDesc">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblDescription" runat="server" Text='<%#Eval("fDesc")%>' CssClass="texttransparent"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Price Per" SortExpression="price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblPricePer" runat="server" Style="text-align: right;" CssClass="texttransparent" Text='<%# DataBinder.Eval(Container.DataItem, "price", "{0:n}") %>'
                                                                MaxLength="15" Width="80px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="lblPricePer_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="lblPricePer" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvPricePer" runat="server" ControlToValidate="lblPricePer"
                                                                Display="None" Enabled="False" ErrorMessage="Price Per Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvPricePer_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvPricePer">
                                                            </asp:ValidatorCalloutExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPricePerTotal" runat="server" Style="float: right; text-align: right;"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pretax Amount" SortExpression="priceQuant">
                                                        <ItemTemplate>
                                                            <asp:Label Style="font-weight: bold; color: #2392D7; text-align: right; float: right;" ID="lblPretaxAmount" runat="server"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "priceQuant", "{0:n}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblPretaxAmountTotal" runat="server" Style="float: right; text-align: right;"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Taxable" SortExpression="stax">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkTaxable" runat="server" Checked='<%#Convert.ToBoolean(Eval("stax"))%>'></asp:CheckBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sales Tax amount" SortExpression="STaxAmt">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="lblSalesTax" runat="server" CssClass="texttransparent" Enabled="false"
                                                                Style="color: #2392D7; text-align: right;" Text='<%# DataBinder.Eval(Container.DataItem, "STaxAmt", "{0:n}") %>'
                                                                MaxLength="15" Width="80px"></asp:TextBox>
                                                            <asp:FilteredTextBoxExtender ID="lblSalesTax_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" TargetControlID="lblSalesTax" ValidChars="1234567890.-">
                                                            </asp:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="rfvSalesTax" runat="server" ControlToValidate="lblSalesTax"
                                                                Display="None" Enabled="False" ErrorMessage="Sales Tax Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="rfvSalesTax_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" PopupPosition="TopLeft" TargetControlID="rfvSalesTax">
                                                            </asp:ValidatorCalloutExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblSalesTaxTotal" runat="server" Style="float: right; text-align: right;"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                        <ItemTemplate>
                                                            <asp:Label Style="font-weight: bold; color: #2392D7; text-align: right; float: right;" ID="lblAmount" runat="server"
                                                                Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:n}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblAmountTotal" runat="server" Style="float: right; text-align: right;"></asp:Label>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>
                        <div class="clearfix"></div>
                    </div>
                    <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                        CausesValidation="False" UseSubmitBehavior="False" />
                    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                        PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="pnlUpdateoverlay"
                        DropShadow="True" RepositionMode="RepositionOnWindowResizeAndScroll">
                    </asp:ModalPopupExtender>
                    <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid; width: 350px;">
                        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD; border: solid 1px Gray; color: Black; text-align: center;">
                            <div class="title_bar_popup">
                                <a id="hideModalPopupViaClientButton" href="#" style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Cancel</a>
                                <asp:LinkButton runat="server" ID="hideModalPopupViaServerConfirm" Text="Ok" OnClick="hideModalPopupViaServerConfirm_Click"
                                    CausesValidation="False" Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px;" />
                            </div>
                        </asp:Panel>
                        <div style="padding: 20px;">
                            <strong>
                                <asp:Label ID="lblCount" runat="server"></asp:Label></strong>
                        </div>
                    </asp:Panel>
                    <%--<asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
        CausesValidation="False" />
    <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
        TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
        BackgroundCssClass="pnlUpdateoverlay" PopupDragHandleControlID="programmaticPopupDragHandle"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </asp:ModalPopupExtender>
    <asp:Panel runat="server" ID="programmaticPopup" Style="display: block; background: #fff;
        border: solid;">
        <asp:Panel runat="Server" ID="programmaticPopupDragHandle" Style="cursor: move; background-color: #DDDDDD;
            border: solid 1px Gray; color: Black; height: 10px; text-align: center;">
        </asp:Panel>
        <div>
            <asp:Panel ID="pnlInvoice" runat="server">
                <div class="title_bar_popup">
                    <asp:Label CssClass="title_text" ID="Label1" runat="server"></asp:Label>
                    <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" OnClick="lnkCancelCust_Click"
                        Style="float: right; margin-right: 20px; color: #fff; margin-left: 10px; height: 16px;">Close</asp:LinkButton>
                    <asp:LinkButton ID="lnkSave" runat="server" Height="16px" Style="float: right; margin-right: 20px;
                        color: #fff; margin-left: 10px;" OnClick="lnkCustSave_Click" ValidationGroup="pop">Save</asp:LinkButton>
                </div>
                <table style="width: 100%; padding: 20px;">
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Quantity
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtQuantity"
                                Display="None" ErrorMessage="Quantity Required" SetFocusOnError="True" ValidationGroup="pop"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator37_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator37">
                            </asp:ValidatorCalloutExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtQuantity_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtQuantity" ValidChars="1234567890.-">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Billing Code ID
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBillingCode" runat="server" CssClass="form-control_ddl"
                                TabIndex="13" Width="200px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Description
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks0" runat="server" Height="129px" MaxLength="8000" TabIndex="20"
                                TextMode="MultiLine" Width="407px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Price Per
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtpricePer"
                                Display="None" ErrorMessage="Price Per Required" SetFocusOnError="True" ValidationGroup="pop"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator38_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator38">
                            </asp:ValidatorCalloutExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtpricePer" runat="server" CssClass="form-control" MaxLength="15"
                                TabIndex="7"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtpricePer_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtpricePer" ValidChars="1234567890.-">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Taxable
                        </td>
                        <td>
                            <asp:CheckBox ID="chkTaxable" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Pretax Amount
                        </td>
                        <td>
                            <asp:Label ID="txtPretax" runat="server" CssClass="TotalText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Sales Tax Amount
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="txtSalesTax"
                                Display="None" ErrorMessage="Sales Tax Amount Required" SetFocusOnError="True"
                                ValidationGroup="pop"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator39_ValidatorCalloutExtender"
                                runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator39">
                            </asp:ValidatorCalloutExtender>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSalesTax" runat="server" CssClass="form-control" MaxLength="15"
                                TabIndex="7"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="txtSalesTax_FilteredTextBoxExtender" runat="server"
                                Enabled="True" TargetControlID="txtSalesTax" ValidChars="1234567890.-">
                            </asp:FilteredTextBoxExtender>
                        </td>
                    </tr>
                    <tr>
                        <td class="register_lbl">
                            Amount
                        </td>
                        <td>
                            <asp:Label ID="txtAmount" runat="server" CssClass="TotalText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </div>
    </asp:Panel>--%>
                </div>
            </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
                <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
            </Triggers>
        </asp:UpdatePanel>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
