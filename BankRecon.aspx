<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="BankRecon.aspx.cs" Inherits="BankRecon" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/MS_style.css" rel="stylesheet" />
    <link href="css/chosen.css" rel="stylesheet" />
    <script src="js/chosen.jquery.js" type="text/javascript"></script>
    <style>
    body:nth-of-type(1) img[src*="Blank.gif"] {
        display: none;
    }
</style>
    <script type="text/javascript">
          $(document).ready(function () {
              $("#<%=txtServiceAccount.ClientID%>").prop("disabled", true); 
              $("#<%=txtInterestAccount.ClientID%>").prop("disabled", true);
              $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true); 
              $("#<%=txtInterestDate.ClientID%>").prop("disabled", true);
              if (parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val()) > 0) {
                  $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", false);
                  $("#<%=txtServiceAccount.ClientID%>").prop("disabled", false);
              }
              if (parseFloat($("#<%=txtInterestAmount.ClientID%>").val()) > 0) {
                  $("#<%=txtInterestDate.ClientID%>").prop("disabled", false);
                  $("#<%=txtInterestAccount.ClientID%>").prop("disabled", false);
              }
          });
       
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
          function VisibleTxtService(obj)
          {
              if (obj.id == "ctl00_ContentPlaceHolder1_txtServiceChrgAmount")
              {
                  var endBalance = parseFloat(document.getElementById(obj.id).value);
                  if (endBalance > 0) {
                     
                      $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", false);
                      $("#<%=txtServiceAccount.ClientID%>").prop("disabled", false);
                  }
                  else {
                      $("#<%=txtServiceChargeDate.ClientID%>").prop("disabled", true);
                      $("#<%=txtServiceAccount.ClientID%>").prop("disabled", true);
                  }
              }
              else {
                  var endBalance = parseFloat(document.getElementById(obj.id).value);
                  if (endBalance > 0) {
                 
                      $("#<%=txtInterestDate.ClientID%>").prop("disabled", false);
                      $("#<%=txtInterestAccount.ClientID%>").prop("disabled", false);
                  }
                  else {
                      $("#<%=txtInterestDate.ClientID%>").prop("disabled", true);
                      $("#<%=txtInterestAccount.ClientID%>").prop("disabled", true);
                  }
              }
              if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                  document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
              }
              
              calculateDifference()
          }
          function calculateCheckAmount()
          {
              var counter = 0;
              var checkAmount = 0.00;
              $("#<%=gvCheck.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {

                  if ($(this).is(':checked'))
                  {
                      SelectedRowStyle('<%=gvCheck.ClientID %>')
                      var chkSelect = $(this).attr('id');
                     
                      var hdnAmount = document.getElementById(chkSelect.replace('chkSelect','hdnAmount'));
                      var amount = parseFloat(hdnAmount.value);

                      checkAmount = checkAmount + amount;
                      counter++;
                  }
                  else
                  {
                      $(this).closest('tr').removeAttr("style");
                  }
              });
            
              $("#<%=lblCheckCount.ClientID%>").text(counter);
              $("#<%=lblCheckAmount.ClientID%>").text(checkAmount.toFixed(2));
              calculateDifference()
          }
          function calculateDepositAmount()
          {
              var counter = 0;
              var depositAmount = 0.00;
              $("#<%=gvDeposit.ClientID%> input[id*='chkSelect']:checkbox").each(function (index) {

                  if ($(this).is(':checked'))
                  {
                      SelectedRowStyle('<%=gvDeposit.ClientID %>')
                      var chkSelect = $(this).attr('id');

                      var hdnAmount = document.getElementById(chkSelect.replace('chkSelect', 'hdnAmount'));
                      var amount = parseFloat(hdnAmount.value);
                      
                      depositAmount = depositAmount + amount;
                      counter++;
                  }
                  else
                  {
                      $(this).closest('tr').removeAttr("style");
                  }
              });

              $("#<%=lblDepositCount.ClientID%>").text(counter);
              $("#<%=lblDepositAmount.ClientID%>").text(depositAmount.toFixed(2));
              calculateDifference()
          }
          function calculateDifference()
          {
              var totalCheckAmt = parseFloat($("#<%=lblCheckAmount.ClientID%>").text());
              var totalDepositAmt = parseFloat($("#<%=lblDepositAmount.ClientID%>").text());
              var totalEndBalance = 0.00; var serviceChrg = 0.00; var interestChrg = 0.00;
              var totalBalance = parseFloat($("#<%=lblBeginBalance.ClientID%>").text());
              if (!isNaN(parseFloat($("#<%=txtEndingBalance.ClientID%>").val()))) {
                  totalEndBalance = parseFloat($("#<%=txtEndingBalance.ClientID%>").val());
              }
              if (!isNaN(parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val()))) {
                  serviceChrg = parseFloat($("#<%=txtServiceChrgAmount.ClientID%>").val());
              }
              if (!isNaN(parseFloat($("#<%=txtInterestAmount.ClientID%>").val()))) {
                  interestChrg = parseFloat($("#<%=txtInterestAmount.ClientID%>").val());
              }
              //var difference = ((totalCheckAmt - totalDepositAmt) + totalEndBalance) + totalBalance;
              
              var difference = totalEndBalance - totalBalance;
              
              difference = difference + totalCheckAmt;
              difference = difference - totalDepositAmt;
              difference = (-difference) - serviceChrg;
              difference = difference + interestChrg;


             <%-- var lblDifference = document.getElementById("<%=lblDifference.ClientID%>");
              lblDifference.innerHTML = difference.toFixed(2);--%>
              //$("#<%=lblDifference.ClientID%>").text(difference.toFixed(2));
              $("#<%=lblDifference.ClientID%>").html(difference.toFixed(2));
              
              $("#<%=hdnDifference.ClientID%>").val(difference.toFixed(2))
              //$("#<%=lblDifference.ClientID%>").innerHTML(difference.toFixed(2));
              if (!isNaN(parseFloat($("#<%=txtEndingBalance.ClientID%>").val()))) {
                  $("#<%=txtEndingBalance.ClientID%>").val(parseFloat($("#<%=txtEndingBalance.ClientID%>").val()).toFixed(2));
              }
              
              //Difference = (Checks - Deposits) + Ending Balance
          
          }
        function ValidateBankRecon(val, diff)
        {
            if(val==1)
            {
                noty({
                    text: 'Please fill Service charge/Interest Details.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
            else if(val==2)
            {
                noty({
                    text: 'You bank reconciliation is off by $'+diff+'. Please correct any mistakes you may have made before proceeding.',
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: false,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: true
                });
            }
            calculateCheckAmount();
            calculateDepositAmount();
            calculateDifference();
        }
          function displayBankRecon()
          {
              $("#BankRecon").hide();
              $("#BankReconReport").show();
          }

          
     </script>
    <script type="text/javascript">
   
        $(document).ready(function () {

            ///////////// Ajax call for account auto search ////////////////////                
            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.Acct = null;
            }
            $("#<%=txtServiceAccount.ClientID%>").autocomplete({

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
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {

                    $("#<%=txtServiceAccount.ClientID%>").val(ui.item.label);
                    $("#<%=hdnServiceAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtServiceAccount.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
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

            $("#<%=txtInterestAccount.ClientID%>").autocomplete({

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
                            alert("Due to unexpected errors we were unable to load account name");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtInterestAccount.ClientID%>").val(ui.item.label);
                    $("#<%=hdnInterestAcct.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtInterestAccount.ClientID%>").val(ui.item.label);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).data("autocomplete")._renderItem = function (ul, item) {
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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="page-content">
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                <ul class="lnklist-header">
                    <li>
                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Bank Reconciliation</asp:Label></li>
                    <li>
                        <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                    <li>
                        <asp:Panel ID="pnlSave" runat="server">
                            <ul>
                                <li style="margin-right:0">
                                    <asp:LinkButton CssClass="icon-save" ID="lnkBtnSave" runat="server" ToolTip="Reconcile" OnClick="lnkBtnSave_Click" ValidationGroup="bankrecon"
                                        TabIndex="38"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton CssClass="icon-closed" ID="lnkBtnClose" runat="server"  ToolTip="Close" OnClick="lnkBtnClose_Click"
                                            TabIndex="39"></asp:LinkButton>
                                </li>
                            </ul>
                        </asp:Panel>
                    </li>
                </ul>
            </div>
            </div>

            <div id="BankRecon" style="display:block;">
            <!-- edit-tab start -->
            <div class="col-sm-12 col-md-12">
                <div class="com-cont">
                    <asp:HiddenField ID="hdnDifference" runat="server" />
                <div class="col-md-4 col-lg-4">
                    <div class="form-col">
                        <div class="fc-label">
                            Bank Account
                        </div>
                        <div class="fc-input">
                                <asp:DropDownList ID="ddlBank" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlBank_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="rfvBank" ControlToValidate="ddlBank"
                                            ErrorMessage="Select Bank account." Display="None" InitialValue="0"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceBank" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvBank" />
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                <asp:HiddenField ID="hdnChartID" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Ending Balance  <%-- Statement Balance--%>

                        </div>
                        <div class="fc-input">
                            <asp:UpdatePanel ID="upEndingBalance" runat="server">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtEndingBalance" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(this,event)" onchange="calculateDifference()" autocomplete="off" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                             <asp:RequiredFieldValidator runat="server" ID="rfvEndingBalance" ControlToValidate="txtEndingBalance"
                                            ErrorMessage="Please enter Ending Balance." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceEndingBalance" runat="server" Enabled="True" PopupPosition="BottomRight"
                            TargetControlID="rfvEndingBalance" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Service Charge
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtServiceChrgAmount" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(this,event)" onchange="VisibleTxtService(this);" />
                                <asp:RequiredFieldValidator runat="server" ID="rfvServiceChrgAmount" ControlToValidate="txtServiceChrgAmount"
                                            ErrorMessage="Please enter Service charge amount." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceServiceChrgAmount" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvServiceChrgAmount" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Interest
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtInterestAmount" CssClass="form-control" runat="server" onkeypress="return isDecimalKey(this,event)" onchange="VisibleTxtService(this);"/>
                                <asp:RequiredFieldValidator runat="server" ID="rfvInterestAmount" ControlToValidate="txtServiceChrgAmount"
                                            ErrorMessage="Please enter Interest amount." Display="None"
                                            ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender ID="vceInterestAmount" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvInterestAmount" />
                        </div>
                    </div>
                    </div>
                         
                <div class="col-md-4 col-lg-4">
                    <div class="form-col">
                        <div class="fc-label">
                            Statement Date
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtStatementDate" CssClass="form-control"  runat="server" 
                                    OnTextChanged="txtStatementDate_TextChanged" AutoPostBack="true" autocomplete="off"/>
                                <asp:CalendarExtender ID="txtStatementDate_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtStatementDate">
                                </asp:CalendarExtender>
                                <asp:RequiredFieldValidator runat="server" ID="rfvStatementDate" ControlToValidate="txtStatementDate"
                                    ErrorMessage="Please enter Statement date." Display="None"
                                    ValidationGroup="bankrecon"></asp:RequiredFieldValidator>
                                <asp:ValidatorCalloutExtender  ID="vceStatementDate" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="rfvStatementDate" />
                                <asp:RegularExpressionValidator ID="revStatementDate" ControlToValidate = "txtStatementDate" ValidationGroup="bankrecon" 
                                    ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                </asp:RegularExpressionValidator>
                                <asp:ValidatorCalloutExtender ID="vceStatementDate1" runat="server" Enabled="True" PopupPosition="Right"
                                    TargetControlID="revStatementDate" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Difference
                        </div>
                                
                        <div class="fc-input" style="padding-top: 5px;">
                            <asp:UpdatePanel ID="upDifference" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblDifference" runat="server" ></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                                
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Service Charge Date
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtServiceChargeDate" CssClass="form-control"  runat="server" onkeypress="return false;"/>
                                <asp:CalendarExtender ID="txtServiceChargeDate_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtServiceChargeDate">
                                </asp:CalendarExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Interest Date
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtInterestDate" CssClass="form-control"  runat="server" onkeypress="return false;"/>
                                <asp:CalendarExtender ID="txtInterestDate_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txtInterestDate">
                                </asp:CalendarExtender>
                        </div>
                    </div>
                </div>
                         
                <div class="col-lg-4 col-md-4">
                    <div class="form-col">
                            
                    </div>
                    <div class="form-col" style="margin-top: 30px;">
                        <div class="fc-label">
                            Begining Balance <%--  Balance --%>
                        </div>
                        <div class="fc-input"  style="padding-top: 5px;">
                            <asp:UpdatePanel ID="updVendorBal" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblBeginBalance" runat="server"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                                </Triggers>
                                </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Service Charge Account
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtServiceAccount" CssClass="form-control searchinput" runat="server" autocomplete="off" />
                                <asp:HiddenField ID="hdnServiceAcct" runat="server" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label">
                            Interest Account
                        </div>
                        <div class="fc-input">
                                <asp:TextBox ID="txtInterestAccount" CssClass="form-control searchinput" runat="server" />
                            <asp:HiddenField ID="hdnInterestAcct" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div> 
                </div>
                <div class="clearfix"></div> 
        </div>
            <div class="clearfix"></div>
            
            <div class="col-sm-12 col-md-12">
            <div class="com-cont">
                <asp:Label ID="lblItemMarked" runat="server" Text="Items you have marked cleared" Font-Bold="true" Font-Size="Small"></asp:Label> <br />             
                <div class="col-md-6 col-lg-6">
                        <div class="form-col">
                        <div class="col-md-6">
                            <asp:Label ID="lblDepositCount" runat="server" Font-Bold="true" Font-Size="Small" Text="0"></asp:Label>  Deposits and Other Credits    
                        </div>
                        <div class="col-md-6">
                                <asp:Label ID="lblDepositAmount" runat="server" Font-Bold="true" Font-Size="Small" Text="0.00"></asp:Label> <br />
                        </div>
                    </div>
                        <div class="form-col">
                        <div class="col-md-6">
                                <asp:Label ID="lblCheckCount" runat="server" Font-Bold="true" Font-Size="Small" Text="0"></asp:Label>  Checks and Payments 
                        </div>
                        <div class="col-md-6">
                                <asp:Label ID="lblCheckAmount" runat="server" Font-Bold="true" Font-Size="Small" Text="0.00"></asp:Label> 
                        </div>
                    </div>
                </div>
                    <div class="clearfix"></div>
            </div>
                <div class="clearfix"></div>
        </div>
            <div class="clearfix"></div>
            <div class="col-sm-12 col-md-12">
            <div class="com-cont">
          <asp:UpdatePanel ID="upTrans" runat="server">
                <ContentTemplate>
                <div class="col-md-6 col-lg-6">
                    <div style="font-weight: bold; padding-bottom: 15px; font-size: 15px">
                        Open checks/credits
                    </div>
                    <asp:GridView ID="gvCheck" runat="server" 
                                AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="True" 
                                EmptyDataText="No records Found" 
                                CssClass="table table-bordered table-striped table-condensed flip-content"
                                EnableModelValidation="True">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                            <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                            <asp:HiddenField ID="hdnAmount" Value='<%# Bind("Amount") %>' runat="server" />
                                            <asp:HiddenField ID="hdnBatch" Value='<%# Bind("Batch") %>' runat="server" />
                                            <asp:HiddenField ID="hdnTypeNum" Value='<%# Bind("TypeNum") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField HeaderText="Ref">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRef" runat="server" Font-Size="Small" Text='<%# Bind("Ref")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfDate" runat="server" Font-Size="Small" Text='<%# Bind("fDate","{0:MM/dd/yyyy}")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfDesc" runat="server" Font-Size="Small" Text='<%# Bind("fDesc")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Font-Size="Small" Text='<%# Bind("TypeName")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Font-Size="Small" Text='<%# DataBinder.Eval(Container.DataItem, "Amt", "{0:c}")%>' 
                                                ForeColor='<%# Convert.ToDouble(Eval("Amt"))<0?System.Drawing.Color.Red: System.Drawing.Color.Black %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                </div> 
                <div class="col-md-6 col-lg-6">
                    <div style="font-weight: bold; padding-bottom: 15px; font-size: 15px">
                        Open deposits/debits
                    </div>
                    <asp:GridView ID="gvDeposit" runat="server"
                                AutoGenerateColumns="False"
                                ShowHeaderWhenEmpty="True" 
                                EmptyDataText="No records Found" 
                                CssClass="table table-bordered table-striped table-condensed flip-content"
                                EnableModelValidation="True">
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="1%">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                            <asp:HiddenField ID="hdnID" Value='<%# Bind("ID") %>' runat="server" />
                                            <asp:HiddenField ID="hdnBatch" Value='<%# Bind("Batch") %>' runat="server" />
                                            <asp:HiddenField ID="hdnTypeNum" Value='<%# Bind("TypeNum") %>' runat="server" />
                                            <asp:HiddenField ID="hdnAmount" Value='<%# Bind("Amount") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Ref">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRef" runat="server" Font-Size="Small" Text='<%# Bind("Ref")  %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfDate" runat="server" Font-Size="Small" Text='<%# Bind("fDate","{0:MM/dd/yyyy}") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblfDesc" runat="server" Font-Size="Small" Text='<%# Bind("fDesc")  %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblType" runat="server" Font-Size="Small" Text='<%# Bind("TypeName")  %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAmount" runat="server" Font-Size="Small" Text='<%# DataBinder.Eval(Container.DataItem, "Amt", "{0:c}")%>' 
                                                ForeColor='<%# Convert.ToDouble(Eval("Amt"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>  
                </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlBank" />
                    <asp:AsyncPostBackTrigger ControlID="txtStatementDate" />
                </Triggers>
            </asp:UpdatePanel>
                <div class="clearfix"></div>
            </div>
                <div class="clearfix"></div>
        </div>        
        </div>
          
            <div id="BankReconReport" style="display:none;">
            <div class="col-sm-12 col-md-12">
            <div class="com-cont">
             
                    <rsweb:ReportViewer ID="rvBankRecon" runat="server" Width="800px" Height="1500px"
                                        BorderColor="Gray" BorderStyle="None" BorderWidth="1px" ShowPageNavigationControls="true" PageCountMode="Actual"
                                        AsyncRendering="false" ShowZoomControl="False">
                    </rsweb:ReportViewer>
                    
                <div class="clearfix"></div>
            </div>
                 
            <div class="clearfix"></div>
            </div>
    </div>
        </div>
    </div>
</asp:Content>

