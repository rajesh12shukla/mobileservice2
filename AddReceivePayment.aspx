<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddReceivePayment.aspx.cs" Inherits="AddReceivePayment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            float: left;
            min-width: 116px;
            text-align: right;
            padding-top: 5px;
            color: #626262;
            display: block;
            font: normal 13px/18px Helvetica;
            margin-right: 10px;
            height: 42px;
        }

        .auto-style2 {
            height: 42px;
        }

        .popupConfirmation {
            width: 500px;
            height: 133px;
        }

        .popup_Container {
            background-color: #ffffff;
            /*border: 2px solid #000000;*/
            padding: 0px 0px 0px 0px;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }

        .divInnerBody {
            padding-top: 10px;
            padding-left: 10px;
            padding-right: 10px;
            padding-bottom: 10px;
        }

        .parent {
            width: 100%;
        }

            .parent .sub1 {
                padding-top: 5px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
                padding-left: 10px;
                /*background-color: red;*/
            }

            .parent .sub2 {
                /*background-color: blue;*/
                float: left;
                width: 200px;
            }

            .parent .sub3 {
                padding-left: 100px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub33 {
                padding-left: 10px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub4 {
                float: left;
                width: 190px;
            }

            .parent .sub5 {
                padding-left: 10px;
                font-family: Helvetica;
                font-size: 13px;
                text-align: left;
                width: 120px;
                float: left;
                height: 30px;
            }

            .parent .sub6 {
                float: left;
                width: 190px;
            }

        .pnlStyle {
            width: 400px;
            float: left;
            height: 268px;
        }

        .pnlAccount {
            height: 268px;
            float: left;
        }

        .pnlBankAccount {
            width: 920px;
            height: 268px;
            float: right;
        }
    </style>
    <script type="text/javascript">

        var isSave = false;
        <%--$("#<%= hdnButtonClick.ClientID %>").val(0)--%>
        $(document).ready(function () {
            GetInvoiceTotal();
           <%-- $("#<%=btnSubmit.ClientID %>").on("mouseover",
                function () {
                    window.onbeforeunload = null;
                    isSave = true;
                    $("#<%= hdnButtonClick.ClientID %>").val(1)
                });--%>
         <%--   $("#<%=btnSubmit.ClientID %>").on("mouseout",
                function () {
                    window.onbeforeunload = Changes;
                    isSave = false;
                    $("#<%= hdnButtonClick.ClientID %>").val(0)
                });--%>
        })
            function VisibleRowOnFocus(txt) {  //To make row's textbox visible
                //debugger;

                $('#<%=gvInvoice.ClientID %> input:text.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#<%=gvInvoice.ClientID %> select.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");

            });

            var txtPAmount = document.getElementById(txt.id);
            $(txtPAmount).removeClass("texttransparent");
            $(txtPAmount).addClass("non-trans");
        }
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
        function ChkCustID(sender, args) {
            var hdnCustID = document.getElementById('<%= hdnCustID.ClientID %>');
            if (hdnCustID.value == '') {
                args.IsValid = false;
            }
            else if (hdnCustID.value == '0') {
                args.IsValid = false;
            }
        }
        function ChkLocID(sender, args) {
            var hdnLocID = document.getElementById('<%= hdnLocID.ClientID %>');
            if (hdnLocID.value == '') {
                args.IsValid = false;
            }
            else if (hdnLocID.value == '0') {
                args.IsValid = false;
            }
        }
    </script>
    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {
                $('.js-amt').keypress(function (e) {
                    if (e.which != 46 && e.which != 45 && e.which != 46 &&
                           !(e.which >= 48 && e.which <= 57)) {
                        return false;
                    }
                });
                var query = "";
                function dtaa() {
                    this.prefixText = null;
                    this.con = document.getElementById('<%=hdnCon.ClientID %>').value;
                    //this.custID = null;
                }
                $("#<%=txtCustomer.ClientID %>").autocomplete({
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
                                debugger;
                                response($.parseJSON(data.d));
                                debugger;
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
                        $("#<%=txtCustomer.ClientID %>").val(ui.item.label);
                        $("#<%=hdnCustID.ClientID %>").val(ui.item.value);
                        $("#<%=txtLocation.ClientID %>").val('');
                        $("#<%=hdnLocID.ClientID %>").val('');

                        document.getElementById('<%=btnSelectCustomer.ClientID %>').click();

                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtCustomer.ClientID %>").val(ui.item.label);
                        $("#<%=hdnCustID.ClientID %>").val(ui.item.value);
                        $("#<%=txtCustomer.ClientID %>").focus();
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
                $("#<%=txtLocation.ClientID %>").autocomplete({

                    source: function (request, response) {
                        //  if (document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value != '') {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        dtaaa.custID = 0;
                        if (document.getElementById('<%=hdnCustID.ClientID %>').value != '') {
                            dtaaa.custID = document.getElementById('<%=hdnCustID.ClientID %>').value;
                        }
                        queryloc = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/GetLocation",
                            //  data: "{'prefixText':'" + request.term + "','con':'" + document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value + "','custID':" + document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId').value + "}",
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
                        $("#<%=txtLocation.ClientID %>").val(ui.item.label);
                        $("#<%=hdnLocID.ClientID %>").val(ui.item.value);
                        document.getElementById('<%=btnSelectLoc.ClientID %>').click();
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtLocation.ClientID %>").val(ui.item.label);
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

                         $("[id*=txtPAmount]").change(function () {

                             var txtPay = $(this).attr('id');
                             var lblDue = document.getElementById(txtPay.replace('txtPAmount', 'lblDueAmount'));
                             var hdnPrevDue = document.getElementById(txtPay.replace('txtPAmount', 'hdnPrevDue'));
                             var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));

                             var pay = $(this).val().toString().replace(/[\$\(\),]/g, '');
                             debugger;
                             if (pay == '') {
                                 pay = 0;
                                 $(this).val('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                             }

                             if (pay != 0) {
                                 pay = parseFloat(pay);
                                 var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                                 var prevDue = parseFloat($(hdnPrevDue).val())
                                 
                                 if (prevDue < pay) {
                                    pay = prevDue;
                                 }
                                 
                                 due = prevDue - pay;

                                 var rPay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                                 var rDue = cleanUpCurrency('$' + due.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                                 $(this).val(rPay);
                                 $(lblDue).text(rDue);
                                 $(chk).prop('checked', true);
                                 SelectedRowStyle('<%=gvInvoice.ClientID %>')
                     }
                     else {
                         $(chk).prop('checked', false);
                         $(this).closest('tr').removeAttr("style");
                     }
                     GetInvoiceTotal();                    
                 });

                 $("[id*=chkSelect]").change(function () {
                     var chk = $(this).attr('id');
                     var txtPay = document.getElementById(chk.replace('chkSelect', 'txtPAmount'));
                     var lblDue = document.getElementById(chk.replace('chkSelect', 'lblDueAmount'));
                     var hdnPrevDue = document.getElementById(chk.replace('chkSelect', 'hdnPrevDue'));

                     var due = parseFloat($(lblDue).text().toString().replace(/[\$\(\),]/g, ''))
                     var prevDue = parseFloat($(hdnPrevDue).val())
                     var pay = 0;
                     
                     var rpay = cleanUpCurrency('$' + pay.toLocaleString("en-US", { minimumFractionDigits: 2 }));
                     var rprevDue = cleanUpCurrency('$' + prevDue.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                     if ($(this).prop('checked') == true) {
                         
                         $(txtPay).val(rprevDue)
                         $(lblDue).text(rpay)
                         SelectedRowStyle('<%=gvInvoice.ClientID %>')
                     }
                     else if ($(this).prop('checked') == false) {
                         
                         $(txtPay).val(rpay)
                         $(lblDue).text(rprevDue)
                         $(this).closest('tr').removeAttr("style");
                     }
                     GetInvoiceTotal();
                 });
                 $("#<%=txtAmount.ClientID %>").change(function () {
                     var amt = parseFloat($(this).val().replace(/[\$\(\),]/g, ''))
                     var amtval = cleanUpCurrency('$' + amt.toLocaleString("en-US", { minimumFractionDigits: 2 }))
                     $(this).val(amtval)
                 });

               $("#<%=txtCustomer.ClientID %>").keyup(function (event) {
                    var hdnCustID = document.getElementById('<%=hdnCustID.ClientID %>');
                    if (document.getElementById('<%=txtCustomer.ClientID %>').value == '') {
                        hdnCustID.value = '';
                    }
                });

               $("#<%=txtLocation.ClientID %>").keyup(function (event) {
                   var hdnLocId = document.getElementById('<%=hdnLocID.ClientID %>');
                   if (document.getElementById('<%=txtLocation.ClientID %>').value == '') {
                        hdnLocId.value = '';
                    }
                });
             });
         }
        function cleanUpCurrency(s) {
            
            var expression = '-';

            //Check if it is in the proper format
            if (s.match(expression)) {
                //It matched - strip out - and append parentheses 
                return s.replace("$-", "\($") + ")";
                
            }
            else {
                return s;
            }
        }
         function GetInvoiceTotal() {
             var total = 0.00;
             $("[id*=txtPAmount]").each(function () {
                 txtPay = $(this).attr('id')
                 //var expression = /^\$?\(?[\d,\.]*\)?$/;      // to identify parentheses and $
                 var expression = /[\(\)]/g                     // to identify parentheses
                 var chk = document.getElementById(txtPay.replace('txtPAmount', 'chkSelect'));
                 if ($(chk).prop('checked') == true) {
                     if ($(this).val() != '') {
                         debugger;
                         var val = $(this).val()
                         
                         if (val.match(expression))     /// check is parentheses exists (negative value)
                         {
                             total = total - parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                         }
                         else
                         {
                             total = total + parseFloat($(this).val().replace(/[\$\(\),]/g, ''));
                         }
                     }
                 }
             });
             var totalval = cleanUpCurrency('$' + total.toLocaleString("en-US", { minimumFractionDigits: 2 }))
             $("#<%=txtAmount.ClientID %>").val(totalval);
             $("[id*=lblTotalPayAmount]").text(totalval);
         }
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
                    <a href="#">Financial Manager</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                   <a href="<%=ResolveUrl("~/receivepayment.aspx") %>">Receive Payment</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Receive Payment </span>
                </li>
            </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Receive Payment</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Label ID="lblReceiveRef" runat="server" Text="Ref #" Visible="False"></asp:Label></li>
                        <li>
                            <asp:Label ID="lblReceiveID" runat="server" Visible="False" Style="font-weight: bold; font-size: 15px;"></asp:Label></li>
                        <li>
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
                        </li>
                        <li>
                            <asp:Panel ID="pnlSave" runat="server">
                                <asp:LinkButton CssClass="icon-save" ToolTip="Save" ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" ValidationGroup="Payment"
                                    TabIndex="38"></asp:LinkButton>
                                <asp:LinkButton CssClass="icon-closed" ToolTip="Close" ID="lnkClose" runat="server" CausesValidation="false" OnClick="lnkClose_Click"
                                    TabIndex="39"></asp:LinkButton>
                            </asp:Panel>
                        </li>
                    </ul>
                </div>
            </div>
            <asp:Button ID="btnSelectCustomer" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectCustomer_Click" />
            <asp:Button ID="btnSelectLoc" runat="server" Text="Button" Style="display: none;" OnClick="btnSelectLoc_Click"/>
            <asp:HiddenField ID="hdnOwner" runat="server" />
            <!-- edit-tab start -->
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                   <div class="alert alert-success" runat="server" id="divSuccess" > 
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        These month/year period is closed out. You do not have permission to add/update this record.
                    </div>
                    <div>
                        <asp:HiddenField ID="hdnButtonClick" runat="server" />
                        <div class="col-md-12 col-lg-12">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-md-8 col-lg-8">
                                        <input id="hdnCon" runat="server" type="hidden" />
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Customer
                                                <asp:RequiredFieldValidator runat="server" ID="rfvCustomer" ControlToValidate="txtCustomer"
                                                    ErrorMessage="Please select Customer" Display="None" ValidationGroup="Payment">
                                                </asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vceCustomer" runat="server" Enabled="True" PopupPosition="Right"
                                                    TargetControlID="rfvCustomer" />
                                                <asp:CustomValidator ID="cvCustomer" runat="server" ControlToValidate="txtCustomer"  ValidationGroup="Payment"
                                                    ErrorMessage="Please select the existing Customer" ClientValidationFunction="ChkCustID"
                                                    Display="None"></asp:CustomValidator>                
                                                <asp:ValidatorCalloutExtender ID="vceCustomer1" runat="server" Enabled="True"
                                                    TargetControlID="cvCustomer"></asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCustomer" runat="server" CssClass="form-control searchinputloc"
                                                    TabIndex="1" autocomplete="off" placeholder="Search by customer name, phone#, address etc." 
                                                    ></asp:TextBox>
                                   
                                                <asp:HiddenField ID="hdnCustID" runat="server" />                                               
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                Location 
                                                <asp:CustomValidator ID="cvLocation" runat="server" ControlToValidate="txtLocation" ValidationGroup="Payment"
                                                    ErrorMessage="Please select the existing Location" ClientValidationFunction="ChkLocID"
                                                    Display="None"></asp:CustomValidator>                    
                                                <asp:ValidatorCalloutExtender ID="vceLocation1" runat="server" Enabled="True"
                                                    TargetControlID="cvCustomer"></asp:ValidatorCalloutExtender>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control searchinputloc ui-autocomplete-input"
                                                    TabIndex="2" autocomplete="off" placeholder="Search by location name, phone#, address etc."></asp:TextBox>
                                
                                                <asp:HiddenField ID="hdnLocID" runat="server" /> 
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-4 col-lg-4">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                  Customer Balance
                                            </div>
                                            <div class="fc-input" style="padding-top: 5px;text-align:right;">
                                                <asp:Label ID="lblCustomerBalance" runat="server" Font-Size="Small"></asp:Label>      
                                            </div>
                                        </div>
                                         <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblAmount" runat="server" Text="Check amount"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" TabIndex="2"
                                                    MaxLength="15" autocomplete="off" Style="text-align: right" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                <asp:RequiredFieldValidator runat="server" ID="rfvAmount" ControlToValidate="txtAmount"
                                                    ErrorMessage="Please enter Amount." Display="None"
                                                    ValidationGroup="Payment"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vceAmount" runat="server" Enabled="True" PopupPosition="Right"
                                                    TargetControlID="rfvAmount" />
                                                <asp:CustomValidator ID="cvAmount" runat="server" OnServerValidate="cvAmount_ServerValidate"  ValidationGroup="Payment"
                                                    ControlToValidate="txtAmount" ErrorMessage="Received amount should match with total payment." Display="None"></asp:CustomValidator>
                                                <asp:ValidatorCalloutExtender ID="vceAmount1" runat="server" Enabled="True" PopupPosition="BottomLeft" 
                                                    TargetControlID="cvAmount" />
                                            </div>
                                        </div>
                                    </div>   
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlPayment" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                </Triggers>
                            </asp:UpdatePanel>     
                        </div>
                        <div class="col-md-12 col-lg-12">
                            <div class="col-md-4 col-lg-4">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Date
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="15" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtDate"></asp:CalendarExtender>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvDate" ControlToValidate="txtDate"
                                            ErrorMessage="Please enter Date." Display="None"
                                            ValidationGroup="Payment"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDate" />
                                        <asp:RegularExpressionValidator ID="revDate" ControlToValidate ="txtDate" ValidationGroup="Payment" 
                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revDate" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        Payment Method 
                                    </div>
                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlPayment" runat="server" CssClass="form-control"
                                            TabIndex="1" OnSelectedIndexChanged="ddlPayment_SelectedIndexChanged" AutoPostBack="true">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        Status
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-8 col-lg-8">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Memo
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtMemo" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="250" autocomplete="off"></asp:TextBox>
                                        <asp:RequiredFieldValidator runat="server" ID="rfvMemo" ControlToValidate="txtMemo"
                                            ErrorMessage="Please enter Memo." Display="None"
                                            ValidationGroup="Payment"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceMemo" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvMemo" />
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="updPnlCheck" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="col-md-4 col-lg-4">
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCheck" runat="server" Font-Size="Small" Text="Check"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCheck" runat="server" CssClass="form-control" TabIndex="2"
                                                    MaxLength="15" autocomplete="off"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlPayment" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="col-md-4 col-lg-4">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Deposit to
                                    </div>
                                    <div class="fc-input" style="padding-top: 5px">
                                        <asp:Label ID="lblDepositTo" runat="server" Font-Size="Small" Font-Bold="True"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="clear: both;">
                            <div style="padding-top: 5px; padding-bottom: 5px;">
                                <asp:Label ID="lblOutstandingTrnas" runat="server" Font-Size="Medium" Width="100%" Font-Bold="True" Text="Outstanding Transactions" Font-Names="sans-serif, Open Sans"></asp:Label>
                            </div>
                            <div class="table-scrollable" style="border: none; padding-top: 10px;">
                                <asp:UpdatePanel ID="uPnlPayment" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False"
                                                CssClass="table table-bordered table-striped table-condensed flip-content" ShowFooter="True" Width="100%"
                                                ShowHeaderWhenEmpty="True" EmptyDataText="No records Found"
                                                AllowSorting="true" OnSorting="gvInvoice_Sorting">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <FooterStyle CssClass="footer" />
                                                <RowStyle CssClass="evenrowcolor" />
                                                <HeaderStyle Height="10px" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField ItemStyle-Width="1%">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnID" Value='<%# Bind("Ref") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnPaymentID" Value='<%# Bind("PaymentID") %>' runat="server" />
                                                            <asp:HiddenField ID="hdnTransID" Value='<%# Bind("TransID") %>' runat="server" />
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                            <asp:HiddenField ID="hdnCheck" Value='<%# Eval("TransID").Equals(0) ? false : true %>' runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ItemStyle-Width="1%" Visible="false" SortExpression="loc">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLoc" runat="server" Text='<%#Eval("loc")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Ref" ItemStyle-Width="1%" SortExpression="Ref">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRef" runat="server" Text='<%#Eval("Ref")%>' Visible="false"></asp:Label>
                                                            <asp:HyperLink ID="hlRef" runat="server" Text='<%# Bind("Ref") %>' Target="_blank" NavigateUrl='<%# "addinvoice.aspx?uid=" +Eval("ref")  %>' ForeColor="#0066CC"></asp:HyperLink>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="ID" ItemStyle-Width="1%" SortExpression="ID">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server"><%#Eval("ID")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Location name" ItemStyle-Width="20%" SortExpression="Tag">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTag" runat="server"><%#Eval("Tag")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" ItemStyle-Width="10%" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server"><%#Eval("status")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Manual Invoice" ItemStyle-Width="15%" SortExpression="manualInv">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblManualInv" runat="server"><%#Eval("manualInv")%></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Invoice date" ItemStyle-Width="10%" SortExpression="fDate">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fDate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label Text="Totals" runat="server" Font-Bold="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Pretax Amount" ItemStyle-Width="15%" SortExpression="Amount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPretaxAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalPretaxAmount" runat="server" Style="float: right;"  Font-Bold="True"  />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sales Tax" ItemStyle-Width="15%" SortExpression="Stax">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalesTax" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "STax", "{0:c}")%>' Style="float: right;"><%--<%#Eval("balance")%>--%></asp:Label><%--<%# DataBinder.Eval(Container.DataItem, "balance", "{0:c}")%>--%>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalSalesTax" runat="server" Style="float: right;"  Font-Bold="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Original Amount" ItemStyle-Width="15%" SortExpression="OrigAmount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrigAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OrigAmount", "{0:c}")%>' Style="float: right;"><%--<%#Eval("balance")%>--%></asp:Label><%--<%# DataBinder.Eval(Container.DataItem, "balance", "{0:c}")%>--%>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalOrigAmount" runat="server" Style="float: right;"  Font-Bold="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount Due" ItemStyle-Width="17%" SortExpression="DueAmount">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDueAmount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "DueAmount", "{0:c}")%>' Style="float: right;"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalDueAmount" runat="server" Style="float: right;" Font-Bold="True"  />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Payment" SortExpression="paymentAmt">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="hdnPrevDue" Value='<%# Eval("PrevDueAmount") %>' runat="server" />
                                                            <asp:TextBox ID="txtPAmount" runat="server" MaxLength="15" Width="125px" CssClass="texttransparent js-amt" Text='<%# DataBinder.Eval(Container.DataItem, "paymentAmt", "{0:c}") %>'
                                                                onfocus="VisibleRowOnFocus(this);" Height="26px" Style="text-align: right"   EnableViewState= "false"
                                                                ></asp:TextBox> 
                                                                <%-- onkeypress="return isDecimalKey(this,event)"--%>
                                                          <%--  <asp:MaskedEditExtender ID="txtPAmount_MaskedEditExtender" runat="server" Mask="9,999,999.99" ClearMaskOnLostFocus="true"
                                                                MaskType="Number" TargetControlID="txtPAmount" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" DisplayMoney="Left"
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" AcceptNegative="Left"
                                                                CultureTimePlaceholder="" Enabled="True">
                                                            </asp:MaskedEditExtender>--%>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalPayAmount" runat="server" Style="float: right;"  Font-Bold="True" />
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </div>
            </div>
          </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSubmit" />
            </Triggers>
        </asp:UpdatePanel>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

