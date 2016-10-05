<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddInventory.aspx.cs" Inherits="AddInventory" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-ui-1.9.2.custom.js"></script>
    <link href="css/jquery-ui-1.9.2.custom.css" rel="stylesheet" />

    <script type="text/javascript">

        var ID = '<%=invID%>';

        $(document).ready(function () {

           
            //debugger;
            if (ID != "") {

                $('#txtItemHeaderName').addClass("hide");
                $('#txtEngineeringName').addClass("hide");
                $('#txtFinanceName').addClass("hide");
                $('#txtPurchasingName').addClass("hide");
                $('#txtInventoryName').addClass("hide");
                $('#txtSalesName').addClass("hide");
                $(".name").removeClass("hide");
            }
            else {
                $(".name").addClass("hide");
            }

            $(".calculated").attr("disabled", "disabled");

            $("#purchaseerror").addClass('hideerror');
            $(".numeric").attr("MaxLength", "9");
            $(".integer").attr("MaxLength", "9");

            modalclose();

            //  $("#shade").removeclass("ModalPopupBG");
            $(".numeric").live("lostfocus", function () {
                //alert('dsfdf');
                if ($(this).val = '')
                    $(this).val("0");
            });

            $(".integer").live("lostfocus", function () {
                //alert('dsfdf');
                if ($(this).val = '')
                    $(this).val("0");
            });


            $('#txtItemHeaderName').focusout(function () {
                var Name = $(this).val();
                $('#txtEngineeringName').val(Name);
                $('#txtFinanceName').val(Name);
                $('#txtPurchasingName').val(Name);
                $('#txtInventoryName').val(Name);
                $('#txtSalesName').val(Name);

            });

            $('#txtDes').focusout(function () {
                var Name = $(this).val();
                $('#txtSalesDescription').val(Name);
                $('#txtEngineeringDescription').val(Name);
                $('#txtFinanceDescription').val(Name);
                $('#txtPurchasingDescription').val(Name);
                $('#txtInventoryDescription').val(Name);

            });

            $(".numeric").live("keydown", function (el) {

                //alert(el.keyCode);
                //debugger;
                if (el.shiftKey) {
                    el.preventDefault();
                }
                if (el.keyCode == 46 || el.keyCode == 8 || el.keyCode == 9 || el.key == ".") {



                }
                else {
                    if (el.keyCode < 95) {
                        if (el.keyCode < 48 || el.keyCode > 57) {
                            el.preventDefault();
                        }
                    }
                    else {
                        if (el.keyCode < 96 || el.keyCode > 105) {
                            el.preventDefault();
                        }
                    }
                }
            });

            $('.numeric').on('change', function () {
                this.value = this.value.trim() != "" ? parseFloat(this.value).toFixed(2) : 0.00;
            });


            $(".integer").live("keydown", function (el) {

                //alert(el.keyCode);
                //debugger;
                if (el.shiftKey) {
                    el.preventDefault();
                }
                if (el.keyCode == 46 || el.keyCode == 8 || el.keyCode == 9) {



                }
                else {
                    if (el.keyCode < 95) {
                        if (el.keyCode < 48 || el.keyCode > 57) {
                            el.preventDefault();
                        }
                    }
                    else {
                        if (el.keyCode < 96 || el.keyCode > 105) {
                            el.preventDefault();
                        }
                    }
                }
            });

            $('.integer').on('change', function () {
                this.value = this.value.trim() != "" ? parseInt(this.value, 0) : 0;
            });



            $("#btnSubmit").live("click", function () {
                if (($("#dtlVendors tr")).length <= 0) {

                    $("#purchaseerror").removeClass('hideerror');
                    $("#purchaseerror").addClass('showerror');
                }


            });


        });

        $(function () {

            $(".date-picker").datepicker();



            $("#btndekVendorInfo").live("click", function () {

                if (($("#dtlVendors tr").not("#dtlVendors tr:first-child").find("#chkvenitem:checked")).length <= 0) {
                    //alert(($("#dtlVendors tr").find("#chkvenitem:checked")).length);
                    return false;
                }

                //$("#dtlVendors tr").not("#dtlVendors tr:first-child").each(function (i, row) {

                //    var $row = $(row);

                //    var $chk = $row.find("#chkvenitem");
                //    if ($chk.is(":checked") == true) {
                //        // $chk.parent("tr").remove();
                //      //  $(this).closest('tr').addClass("vendordelete");
                //        $(this).closest('tr').attr("visibility", "hidden")


                //    }
                //});


            });


            $("#btnSubmit").live("click", function () {

                $(".numeric").each(function (i, v) {
                  
                    if ($(this).val().trim() == '') {
                       
                        $(this).val('0');
                    }

                });

                $(".integer").each(function (i, v) {
                    if ($(this).val().trim() == '') {
                        
                        $(this).val('0');
                    }

                });

                if (Page_ClientValidate("Inv")) {
                    //alert("valid");
                }
                else {
                    for (var i = 0; i < Page_Validators.length; i++) {
                        var val = Page_Validators[i];
                        var ctrl = document.getElementById(val.controltovalidate);
                        if (ctrl != null && ctrl.style != null) {
                            if (!val.isvalid) {

                                ctrl.style.borderColor = '#FF0000';
                                ctrl.style.backgroundColor = '#fce697';
                            }
                            else {
                                ctrl.style.borderColor = '';
                                ctrl.style.backgroundColor = '';
                            }
                        }
                    }

                }




            });

            $("#btnaddVendorInfo").live("click", function () {

                showapprovedvendormodal();

            });


            $("#btneditVendorInfo").live("click", function () {

                if (($("#dtlVendors tr").not("#dtlVendors tr:first-child").find("#chkvenitem:checked")).length <= 0) {
                    //alert(($("#dtlVendors tr").find("#chkvenitem:checked")).length);
                    return false;
                }
                else if (($("#dtlVendors tr").not("#dtlVendors tr:first-child").find("#chkvenitem:checked")).length > 1) {
                    return false;
                }
                else {

                    ($("#dtlVendors tr").not("#dtlVendors tr:first-child").find("#chkvenitem:checked")).each(function () {

                        var id = ($(this).parent("td").parent("tr")).find("#hdnid").val();
                        var vendorid = ($(this).parent("td").parent("tr")).find("#lblvendorid").text();
                        var mpn = ($(this).parent("td").parent("tr")).find("#lblmpn").text();
                        var name = ($(this).parent("td").parent("tr")).find("#lblName").text();

                        $("#txtInventoryMPN").val(mpn);
                        $("#txtInventoryApprovedManufacturer").val(name);
                        $("#ddlInventoryApprovedVendor").val(vendorid);
                        $("#hdninvvendinfo").val(id);
                    });


                    showapprovedvendormodal();
                }
            });

            $("#lnkCloseInventoryWarehouse").live("click", function () {

                modalclose();

            });


            $("#linkquoterequestid").live('click', function () {

                showquoterequestmodal();
            });


            $("#ddlApprovedVendorrequestquote").live("change", function () {

                // debugger
                $('#ddlMPNMannufacturer option').remove();
                $("#txtvendoremail").val('');

                var vendor = $('#ddlApprovedVendorrequestquote option:selected').val();
                $.ajax({
                    type: "POST",
                    url: "AddInventory.aspx/GetApprovedVendorInfo",
                    data: '{strinvID:"' +<%=invID%> +'",Vendor: "' + vendor + '"}',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: OnSuccess,
                    failure: function (response) {
                        //alert("zdfsds");


                    },
                    error: function (response) {

                        //alert("sdfjdzjz");
                    }
                });
            });


            $("#lnksendRequestQuote").live("click", function () {


                $.ajax({
                    type: "POST",
                    url: "AddInventory.aspx/SendMail",
                    data: '{ToEmail:"' + $("#txtvendoremail").val() + '",Quantity: "' + $("#txtquotequantity").val() + '",body:"' + $("#txtemailcontentent").val() + '"}',

                    contentType: "application/json; charset=utf-8",
                    dataType: "json",

                    success: function (response) {


                        if (response.d.Header.HasError == false) {


                            noty({
                                text: response.d.ReponseObject,
                                type: 'success',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }

                        else if (response.d.Header.HasError == true) {


                            noty({
                                text: response.d.ReponseObject,
                                type: 'error',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }

                        modalclose();

                    }

                });


                return false;
            });


            $("#btnclose").live("click", function () {

                modalclose();

                return false;

            });
        });


        function modalclose() {

            $("#programmaticPopup").attr("style", "display:none");
            $("#programmaticPopup").removeClass("programmaticPopup");
            $("#shade").removeClass("ModalPopupBG");

            $("#programmaticRequestQuotePopup").attr("style", "display:none");
            $("#programmaticRequestQuotePopup").removeClass("programmaticPopup");
            $("#shade").removeClass("ModalPopupBG");


            $("#txtInventoryMPN").val('');
            $("#txtInventoryApprovedManufacturer").val('');
            $("#ddlInventoryApprovedVendor").val('0');
            $("#hdninvvendinfo").val('');
            $('#ddlMPNMannufacturer option').remove();

            //txtInventoryMPN.Text = string.Empty;
            //txtInventoryApprovedManufacturer.Text = string.Empty;
            //ddlInventoryApprovedVendor.ClearSelection();
            //hdninvvendinfo.Value = "";

            $("#ddlApprovedVendorrequestquote").val('0');
            $("#txtvendoremail").val('');
            $("#txtquotequantity").val('');
            $("#txtemailcontentent").val('');

        }

        function showapprovedvendormodal() {
            $("#programmaticPopup").attr("style", "display:block");
            $("#programmaticPopup").addClass("programmaticPopup");
            $("#shade").addClass("ModalPopupBG");
        }

        function showquoterequestmodal() {
            $("#programmaticRequestQuotePopup").attr("style", "display:block");
            $("#programmaticRequestQuotePopup").addClass("programmaticPopup");
            $("#shade").addClass("ModalPopupBG");
        }
        function OnSuccess(response) {
            //debugger;
            // ('#lblMsg').text('An error was encountered.');


            //setTimeout(function () {
            //    $("#dvnotification").show();
            //}, 1);
            var result = response.d.ReponseObject;


            // alert(response.d.ReponseObject.InventoryManufacturerInformationId);
            var sel = $('#ddlMPNMannufacturer');
            $.each(result.ManufacturerInfo, function () {

                var items = this;

                sel.append('<option value="' + items.ID + '">' + items.MPN + "-" + items.Manufacturer + '</option>');
            });

            $("#txtvendoremail").val(result.Email);

            //alert(result.Email);



        }


    </script>


    <style>
        .show {
            visibility: visible;
        }

        .hide {
            visibility: hidden;
        }

        .pac-container {
            width: 700px !important;
        }

        .pc-titlesmall {
            background: #316b9d;
            font-size: 10px;
            color: #dadedf;
            line-height: 3px !important;
        }

        .mcpWidth {
            width: 68% !important;
        }

        .fixedheadergridv {
            position: absolute;
            top: 145px;
        }

        .fixedfootergridv {
            position: absolute;
            top: 413px;
        }

        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
            position: fixed;
            left: 0px;
            top: 0px;
            z-index: 10000;
            width: 1349px;
            height: 688px;
        }

        .error {
            color: #F00;
            vertical-align: top;
            position: absolute;
        }

        .hideerror {
            display: none;
        }

        .showerror {
            display: inline;
        }

        .errorTab {
            visibility: hidden;
            left: auto;
            z-index: 100000;
            width: 1px;
            float: right;
        }

            .errorTab tr span {
                color: red;
            }

        .programmaticPopup {
            background: rgb(255, 255, 255) none repeat scroll 0% 0%;
            border: medium solid;
            position: fixed;
            z-index: 100001;
            left: 460px;
            top: 1px;
        }
    </style>
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
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Item</asp:Label></li>

                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSubmit" ToolTip="Save" runat="server" OnClick="btnSubmit_Click"
                                TabIndex="1" ValidationGroup="Inv" ClientIDMode="Static"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"
                                TabIndex="2"></asp:LinkButton></li>
                    </ul>

                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">

                <div class="com-cont">
                    <div class="sc-form">
                        <input id="hdnPatientId" runat="server" type="hidden" />
                        <asp:HiddenField ID="hdnMail" runat="server" />
                        <asp:HiddenField ID="hdnSageIntegration" runat="server" />
                        <label>On Hand</label>
                        <asp:TextBox ID="txtOnHnad" runat="server" CssClass="calculated form-control input-sm input-small" ClientIDMode="Static"></asp:TextBox>
                        <label>On Order</label>
                        <asp:TextBox ID="txtOnOrder" runat="server" CssClass="calculated form-control input-sm input-small" ClientIDMode="Static"></asp:TextBox>
                        <label>Committed</label>
                        <asp:TextBox ID="txtOnComitted" runat="server" CssClass="calculated form-control input-sm input-small" ClientIDMode="Static"></asp:TextBox>
                        <label>Avaliable</label>
                        <asp:TextBox ID="txtOnAvaliable" runat="server" CssClass="calculated form-control input-sm input-small" ClientIDMode="Static"></asp:TextBox>
                        <label>Issued to Open jobs</label>
                        <asp:TextBox ID="txtIssuedtoOpenjobs" runat="server" CssClass="calculated form-control input-sm input-small" ClientIDMode="Static"></asp:TextBox>
                        <%--<input type="button" class="btn blue"  title="Details" name="Details" />--%>
                        <asp:LinkButton ID="LinkButton1" runat="server" CssClass="btn blue">Details</asp:LinkButton>
                    </div>
                    <div class="clearfix"></div>
                    <div style="padding-top: 30px;">

                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" ClientIDMode="Static">
                            <asp:TabPanel runat="server" ID="tpItemHeader" HeaderText="Item Header" ClientIDMode="Static">


                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Item Header
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <%--<asp:UpdatePanel runat="server" ID="Updateform" UpdateMode="Conditional">

                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlHeaderNameName"  /></Triggers>
                                        <ContentTemplate>--%>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 100%">
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="rfvName" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="txtItemHeaderName" Display="Static" ErrorMessage="*"
                                                            SetFocusOnError="True" ToolTip="P/N name is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <%--   <asp:ValidatorCalloutExtender ID="vce1" runat="server" Enabled="True" BehaviorID="b1" ClientIDMode="Static"
                                                            TargetControlID="rfvName">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            P/N                                                            
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtItemHeaderName" runat="server" CssClass="form-control" ClientIDMode="Static" TabIndex="3"></asp:TextBox>

                                                            <asp:DropDownList ID="ddlHeaderNameName" runat="server" CssClass="name form-control" OnSelectedIndexChanged="ddlHeaderNameName_SelectedIndexChanged" AutoPostBack="true" TabIndex="3"></asp:DropDownList>


                                                            <%--<asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True"
                                                                PopupPosition="Right" TargetControlID="rfvName" />--%>
                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="rfvUom" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="ddlUOM" Display="Static" ErrorMessage="*"
                                                            SetFocusOnError="True" InitialValue="0" ToolTip="UOM is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True" BehaviorID="b2"
                                                            TargetControlID="rfvUom" ClientIDMode="Static">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            UOM
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlUOM" runat="server" CssClass="form-control" TabIndex="11"></asp:DropDownList>


                                                            <%-- <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                PopupPosition="Right" TargetControlID="rfvUom" />--%>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="txtDes" Display="Static" ErrorMessage="*"
                                                            SetFocusOnError="True" ToolTip="Description is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                            TargetControlID="RequiredFieldValidator1">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75" ClientIDMode="Static" />


                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="ddlInvStatus" Display="Dynamic" ErrorMessage="*"
                                                            SetFocusOnError="True" ToolTip="Satus is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <div class="fc-label">
                                                            Status
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlInvStatus" runat="server" CssClass="form-control" TabIndex="12"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 2
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes2" CssClass="form-control" TabIndex="5" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="txtDateCreated" Display="Dynamic" ErrorMessage="*"
                                                            SetFocusOnError="True" ToolTip="Created date is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <div class="fc-label">
                                                            Date Created
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDateCreated" CssClass="form-control" TabIndex="13" runat="server" ClientIDMode="Static" ReadOnly="true" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 3
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes3" CssClass="form-control" TabIndex="6" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Category
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" TabIndex="14"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Description 4
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtDes4" CssClass="form-control" TabIndex="7" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Custom 1
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtCustom1" CssClass="form-control" TabIndex="15" runat="server" MaxLength="75" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblatt" runat="server" Text="Attach"></asp:Label>

                                                                </td>
                                                                <td>
                                                                    <asp:FileUpload ID="flattachment" runat="server" Style="width: 80px;" TabIndex="8" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbltranshistory" runat="server" Text="Transaction History"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txttranhistorystartdate" CssClass="form-control input-sm input-small date-pickerer" TabIndex="9" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txttranhistoryenddate" CssClass="form-control input-sm input-small date-picker" TabIndex="10" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>

                                                                    <a class="btn submit"><i class="fa fa-search"></i>
                                                                    </a>


                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            Remarks
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtRemarks" CssClass="form-control" TabIndex="16" runat="server" TextMode="MultiLine" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                    <%-- </ContentTemplate>
                                    </asp:UpdatePanel>--%>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tpEngineering" runat="server" HeaderText="Engineering" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Engineering
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 100%">
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            P/N
                                                        </div>
                                                        <div class="fc-input">

                                                            <asp:TextBox ID="txtEngineeringName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false" TabIndex="17"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlEngineeringName" runat="server" CssClass="name form-control" Enabled="false" TabIndex="17"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label3" runat="server" Text="Revision"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtRevision" CssClass="form-control input-sm input-small" TabIndex="22" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label4" runat="server" Text="Revision Date"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtRevisionDate" CssClass="form-control input-sm input-small date-picker" TabIndex="23" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label5" runat="server" Text="ECO #"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtECO" CssClass="form-control input-sm input-small" TabIndex="24" runat="server" MaxLength="75" />
                                                                </td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtEngineeringDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>

                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label6" runat="server" Text="Drawing #"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtDrawing" CssClass="form-control input-sm input-small" TabIndex="25" runat="server" ClientIDMode="Static" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:Label ID="Label7" runat="server" Text="Attach Drawing File(s)"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:FileUpload runat="server" ID="flDrawing" ClientIDMode="Static" TabIndex="26" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label1" runat="server" Text="Reference"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 100%;">
                                                                    <asp:TextBox ID="txtReference" CssClass="form-control" TabIndex="27" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>

                                                        </table>

                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 2
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification2" CssClass="form-control" TabIndex="19" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label8" runat="server" Text="Length"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtLength" CssClass="numeric form-control" TabIndex="28" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label9" runat="server" Text="Width"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtWidth" CssClass="numeric form-control" TabIndex="29" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label10" runat="server" Text="Height"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtHeight" CssClass="numeric form-control" TabIndex="30" runat="server" Style="width: 100px;" />
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="Label11" runat="server" Text="Weight"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtWeight" CssClass="numeric form-control" TabIndex="31" runat="server" Style="width: 100px;" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 3
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification3" CssClass="form-control" TabIndex="20" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="Label12" runat="server" Text="Shelf Life(days)"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:TextBox ID="txtshelflife" CssClass="numeric form-control input-sm input-small integer" TabIndex="32" runat="server" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="Insp Required" runat="server" TextAlign="Left" ID="chkInspRequired" TabIndex="33" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="CoC Required" runat="server" TextAlign="Left" ID="chkCoCpRequired" TabIndex="34" />
                                                                </td>
                                                                <td style="padding-right: 10px;">
                                                                    <asp:CheckBox Text="Serialization Required" runat="server" TextAlign="Left" ID="chkSerializationRequired" TabIndex="35" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 40%;">
                                                        <div class="fc-label">
                                                            Specification 4
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtSpecification4" CssClass="form-control" TabIndex="21" runat="server" MaxLength="75" />

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 60%; padding-left: 10px;">
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpFinance" HeaderText="Finance" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Finance
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 100%">
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <div class="fc-label">
                                                            P/N
                                                        </div>
                                                        <div class="fc-input">

                                                            <asp:TextBox ID="txtFinanceName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                            <asp:DropDownList ID="ddlFinanceName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label2" runat="server" Text="Unit Cost"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:TextBox ID="txtunitCost" CssClass="numeric calculated form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label13" runat="server" Text="Last Purchase Cost"></asp:Label>
                                                                </td>
                                                                <td style="width: 25%;">
                                                                    <asp:TextBox ID="txtLastPurchaseCost" CssClass="numeric calculated form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">

                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtFinanceDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>

                                                    <div style="float: left; width: 50%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label15" runat="server" Text="Last Purchase From"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <%--asp:TextBox ID="txtLastPurchaseFrom" CssClass="form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />--%>
                                                                    <asp:DropDownList ID="ddlLastPurchaseFromVendor" runat="server" CssClass="calculated form-control"></asp:DropDownList>
                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label16" runat="server" Text="Last Purchase Date"></asp:Label>
                                                                </td>
                                                                <td style="width: 25%;">
                                                                    <asp:TextBox ID="txtLastPurchaseDate" CssClass="calculated form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>

                                                </div>


                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <%-- <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True" BehaviorID="b3" ClientIDMode="Static"
                                                            TargetControlID="RequiredFieldValidator4">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="ddlglsales" Display="Static" ErrorMessage="*" InitialValue="0"
                                                            SetFocusOnError="True" ToolTip="GL Sales is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <div class="fc-label">
                                                            GL Sales
                                                             
                                                        </div>
                                                        <div class="fc-input">


                                                            <asp:DropDownList ID="ddlglsales" runat="server" CssClass="form-control"></asp:DropDownList>


                                                        </div>

                                                    </div>
                                                    <div style="float: left; width: 50%; padding-left: 10px;">

                                                        <table>
                                                            <tr>

                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label14" runat="server" Text="OH Value"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:TextBox ID="txtOHVal" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 10px; width: 25%;">
                                                                    <asp:Label ID="Label17" runat="server" Text="OO Value"></asp:Label>
                                                                </td>
                                                                <td style="width: 25%;">
                                                                    <asp:TextBox ID="txtOOVal" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="Inv"
                                                            runat="server" ControlToValidate="ddlglcogs" Display="Static" ErrorMessage="*" InitialValue="0"
                                                            SetFocusOnError="True" ToolTip=" GL COGS is required." CssClass="error"></asp:RequiredFieldValidator>
                                                        <%--  <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True" BehaviorID="b4" ClientIDMode="Static"
                                                            TargetControlID="RequiredFieldValidator5">
                                                        </asp:ValidatorCalloutExtender>--%>
                                                        <div class="fc-label">
                                                            GL COGS
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:DropDownList ID="ddlglcogs" runat="server" CssClass="form-control"></asp:DropDownList>

                                                        </div>
                                                    </div>
                                                    <div style="float: left; width: 50%; padding-left: 10px;">

                                                        <table>
                                                            <tr>
                                                                <td style="padding-right: 10px; width: 15%;">
                                                                    <asp:Label ID="Label18" runat="server" Text="Comitted Value"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 50%;">
                                                                    <asp:TextBox ID="txtComittedValue" CssClass="calculated numeric form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>


                                                            </tr>

                                                        </table>


                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">
                                                        <table>
                                                            <tr>
                                                                <td style="padding-right: 10px; width: 16%;">
                                                                    <asp:Label ID="Label23" runat="server" Text="ABC Class"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 16%;">
                                                                    <asp:DropDownList ID="ddlABC" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </td>
                                                                <td style="padding-right: 10px; width: 10%;">
                                                                    <asp:Label ID="Label24" runat="server" Text="Taxable"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 10px; width: 10%;">
                                                                    <asp:CheckBox ID="ckhTaxable" runat="server" />
                                                                </td>
                                                                <td style="padding-right: 10px; width: 16%;">
                                                                    <asp:Label ID="Label25" runat="server" Text="Inventory Turns"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 10px; width: 16%;">
                                                                    <asp:TextBox ID="txtInventoryTurns" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>

                                                        </table>
                                                    </div>
                                                    <div style="float: left; width: 50%; padding-left: 10px;">
                                                    </div>
                                                </div>


                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpPurchasing" HeaderText="Purchasing" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Purchasing
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 100%">
                                                <div class="form-col">
                                                    <div style="float: left; clear: right; width: 50%;">
                                                        <div class="fc-label">
                                                            P/N
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtPurchasingName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>

                                                            <asp:DropDownList ID="ddlPurchasingName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td style="padding-right: 3px;">
                                                                    <asp:Label ID="Label19" runat="server" Text="Next PO due Date"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 3px;">
                                                                    <asp:TextBox ID="txtNextPoDate" CssClass="calculated form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 3px;">
                                                                    <asp:Label ID="Label20" runat="server" Text="Last Unit Cost"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 3px;">
                                                                    <asp:TextBox ID="txtLastUnitCost" CssClass="calculated numeric form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 3px;">
                                                                    <asp:Label ID="Label32" runat="server" Text="Last Purchase From"></asp:Label>
                                                                </td>
                                                                <td style="">
                                                                    <asp:TextBox ID="txtLastVendor" CssClass="calculated form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>


                                                            </tr>

                                                        </table>
                                                    </div>


                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; width: 50%;">

                                                        <div class="fc-label">
                                                            Description
                                                        </div>
                                                        <div class="fc-input">
                                                            <asp:TextBox ID="txtPurchasingDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />

                                                        </div>
                                                    </div>

                                                    <div style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td style="padding-right: 3px; width: 15%;">
                                                                    <asp:Label ID="Label21" runat="server" Text="Last Purchase Date"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%;">
                                                                    <asp:TextBox ID="txtLastPODate" CssClass="calculated form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%;">
                                                                    <asp:Label ID="Label22" runat="server" Text="Last Receipt Date"></asp:Label>
                                                                </td>
                                                                <td style="width: 15%;">
                                                                    <asp:TextBox ID="txtLastReceiptDate" CssClass="calculated form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                            </tr>

                                                        </table>

                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; clear: right; width: 50%;">
                                                        <span id="purchaseerror" title="P/N name is required." class="error">*</span>

                                                        <div class="fc-label">
                                                            Vendor
                                                        </div>
                                                        <div class="fc-input">
                                                            <div class="pc-titlesmall">

                                                                <ul class="lnklist-header">

                                                                    <li>
                                                                        <asp:LinkButton CssClass="icon-addnew" ID="btnaddVendorInfo" ToolTip="Add New" runat="server" OnClick="btnaddVendorInfo_Click"
                                                                            ClientIDMode="Static"></asp:LinkButton>
                                                                    </li>
                                                                    <li>

                                                                        <asp:LinkButton CssClass="icon-edit" ID="btneditVendorInfo" ToolTip="Edit" runat="server" OnClick="btneditVendorInfo_Click"
                                                                            ClientIDMode="Static"></asp:LinkButton>
                                                                    </li>
                                                                    <li>
                                                                        <%-- <a class="icon-delete" title="Delete" id="btndekVendorInfo"></a>--%>
                                                                        <asp:LinkButton CssClass="icon-delete" ID="btndekVendorInfo" ToolTip="Delete" runat="server"
                                                                            ClientIDMode="Static" OnClick="btndekVendorInfo_Click"></asp:LinkButton>
                                                                    </li>

                                                                </ul>
                                                            </div>
                                                            <asp:Panel ScrollBars="Vertical" runat="server" Height="120px">

                                                                <asp:UpdatePanel runat="server" ID="upgrd" UpdateMode="Conditional">
                                                                    <Triggers>
                                                                        <asp:AsyncPostBackTrigger ControlID="lnkSaveInventoryWarehouse" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="lnkCloseInventoryWarehouse" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btnaddVendorInfo" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btneditVendorInfo" EventName="Click" />
                                                                        <asp:AsyncPostBackTrigger ControlID="btndekVendorInfo" EventName="Click" />

                                                                    </Triggers>
                                                                    <ContentTemplate>

                                                                        <asp:DataList ID="dtlVendors" runat="server" CssClass="table table-bordered table-striped table-condensed flip-content" ClientIDMode="Static" Width="100%">
                                                                            <ItemStyle CssClass="evenrowcolor" />
                                                                            <FooterStyle CssClass="footer" />
                                                                            <AlternatingItemStyle CssClass="oddrowcolor" />
                                                                            <SelectedItemStyle CssClass="selectedrowcolor" />



                                                                            <HeaderTemplate>


                                                                                <th>&nbsp;</th>
                                                                                <th scope="col"><a>MPN</a></th>
                                                                                <th scope="col"><a>Approved Manufacturer</a></th>
                                                                                <th scope="col"><a>Approved Vendor</a></th>


                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkvenitem" runat="server" />
                                                                                    <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("ID")%>' />
                                                                                </td>


                                                                                <td>
                                                                                    <%--<asp:HiddenField ID="hdnSelected" runat="server" />
                                                                            <asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" />
                                                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("MPN") %>' ClientIDMode="Static" />--%>
                                                                                    <asp:Label ID="lblmpn" runat="server" Text='<%#Eval("MPN")%>'></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("ApprovedManufacturer")%>'></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblvendorname" runat="server" Text='<%#Eval("ApprovedVendor")%>'></asp:Label>
                                                                                    <asp:Label ID="lblvendorid" runat="server" Text='<%#Eval("ApprovedVendorId")%>' CssClass="hide"></asp:Label>
                                                                                </td>



                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <td>
                                                                                    <asp:CheckBox ID="chkvenitem" runat="server" />
                                                                                    <asp:HiddenField ID="hdnid" runat="server" Value='<%#Eval("ID")%>' />
                                                                                </td>
                                                                                <td>
                                                                                    <%-- <asp:HiddenField ID="hdnSelected" runat="server" />
                                                                            <asp:CheckBox ID="chkSelect" runat="server" ClientIDMode="Static" />
                                                                            <asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("MPN") %>' ClientIDMode="Static" />--%>
                                                                                    <asp:Label ID="lblmpn" runat="server" Text='<%#Eval("MPN")%>'></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("ApprovedManufacturer")%>'></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblvendorname" runat="server" Text='<%#Eval("ApprovedVendor")%>'></asp:Label>
                                                                                    <asp:Label ID="lblvendorid" CssClass="hide" runat="server" Text='<%#Eval("ApprovedVendorId")%>'></asp:Label>
                                                                                </td>



                                                                            </AlternatingItemTemplate>

                                                                        </asp:DataList>


                                                                    </ContentTemplate>


                                                                </asp:UpdatePanel>
                                                            </asp:Panel>


                                                        </div>
                                                    </div>

                                                    <div style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <table style="width: 100%;">

                                                            <tr>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:Label ID="Label26" runat="server" Text="Commodity"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <%--<asp:TextBox ID="txtCommodity" CssClass="calculated form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />--%>
                                                                    <asp:DropDownList ID="ddlCommodity" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:Label ID="Label27" runat="server" Text="EAU"></asp:Label>
                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:TextBox ID="txtEAU" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:Label ID="Label33" runat="server" Text="EOL Date"></asp:Label>
                                                                </td>
                                                                <td style="width: 15%;">
                                                                    <asp:TextBox ID="txtEOLDate" CssClass="form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </td>

                                                            </tr>
                                                            <tr style="height: 10px;"></tr>
                                                            <tr style="height: 10px;"></tr>
                                                            <tr>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:Label ID="Label34" runat="server" Text="Warranty Period"></asp:Label>

                                                                </td>
                                                                <td style="padding-right: 3px; width: 15%">
                                                                    <asp:TextBox ID="txtWarrantyPeriod" runat="server" CssClass="form-control input-sm input-small integer"></asp:TextBox>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:HyperLink Text="Quote Request" runat="server" ID="linkquoterequestid" ClientIDMode="Static" CssClass="btn blue"></asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div style="float: left; clear: right; width: 50%;">
                                                        <div style="float: left; width: 100%;">
                                                            <table>

                                                                <tr>
                                                                    <td class="fc-label">
                                                                        <asp:Label ID="Label28" runat="server" Text="Lead Time"></asp:Label>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLeadTime" CssClass="numeric form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label29" runat="server" Text="MOQ"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMOQ" CssClass="numeric form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="Label30" runat="server" Text="EOQ"></asp:Label>

                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtEOQ" CssClass="numeric form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>

                                                                </tr>

                                                            </table>
                                                        </div>
                                                    </div>
                                                    <div style="float: right; clear: right; width: 50%; padding-left: 5px;">
                                                        <div style="float: left; width: 100%;">
                                                            <table>
                                                                <tr>
                                                                    <td style="padding-right: 3px; width: 20%">
                                                                        <asp:Label ID="Label31" runat="server" Text="Purchase Transaction History"></asp:Label>

                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <asp:TextBox ID="TextBox5" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <asp:TextBox ID="TextBox6" CssClass="form-control" TabIndex="4" runat="server" MaxLength="75" />
                                                                    </td>
                                                                    <td style="padding-right: 3px; width: 15%">
                                                                        <a class="btn submit"><i class="fa fa-search"></i></a>
                                                                    </td>

                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpInventory" HeaderText="Inventory" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Inventory
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">

                                                <div class="form-col">

                                                    <div class="fc-label">
                                                        P/N
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtInventoryName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlInventoryName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                    </div>

                                                </div>
                                                <div class="form-col">
                                                    <div class="fc-label">
                                                        Description
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtInventoryDescription" CssClass="form-control" TabIndex="18" runat="server" MaxLength="75" Enabled="false" ClientIDMode="Static" />
                                                    </div>
                                                </div>

                                                <div class="form-col">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="Inv"
                                                        runat="server" ControlToValidate="ddlWareHouse" Display="Dynamic" ErrorMessage="*" InitialValue="0"
                                                        SetFocusOnError="True" ToolTip="WareHouse name is required." CssClass="error"></asp:RequiredFieldValidator>
                                                    <div class="fc-label">
                                                        WareHouse
                                                    </div>
                                                    <div class="fc-input">

                                                        <asp:DropDownList ID="ddlWareHouse" runat="server" CssClass="form-control"></asp:DropDownList>
                                                    </div>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Aisle
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">


                                                                    <asp:TextBox ID="txtAisle" CssClass="form-control input-sm input-small integer" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Shelf
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtShelf" CssClass="form-control input-sm input-small integer" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>

                                                            <td>
                                                                <div class="fc-label">
                                                                    Bin
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtBin" CssClass="form-control input-sm input-small integer" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>

                                                        </tr>

                                                    </table>



                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Date Last Used
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtDateLastUsed" CssClass="calculated form-control input-sm input-small date-picker" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <%--<td>
                                                                <div class="fc-label">
                                                                    Shelf Life
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtShelfLife2" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>--%>
                                                        </tr>

                                                    </table>



                                                </div>

                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel runat="server" ID="tpSales" HeaderText="Sales" ClientIDMode="Static">
                                <HeaderTemplate>
                                    <table class="errorTab">
                                        <tbody>
                                            <tr>
                                                <td><span>!</span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                    Sales
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="col-lg-12 col-md-12">
                                        <div class="com-cont">
                                            <div class="col-md-5 col-lg-5" style="width: 75%">

                                                <div class="form-col">

                                                    <div class="fc-label">
                                                        P/N
                                                    </div>
                                                    <div class="fc-input">
                                                        <asp:TextBox ID="txtSalesName" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false"></asp:TextBox>
                                                        <asp:DropDownList ID="ddlSalesName" runat="server" CssClass="name form-control" Enabled="false"></asp:DropDownList>
                                                    </div>

                                                </div>

                                                <div class="form-col">

                                                    <div class="fc-label">
                                                        Description
                                                    </div>
                                                    <div class="fc-input">

                                                        <asp:TextBox ID="txtSalesDescription" CssClass="form-control" TabIndex="4" runat="server" ClientIDMode="Static" Enabled="false" />
                                                    </div>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 1
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice1" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Max Discount %
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtMaxDiscount" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 2
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice2" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Last Sales Price
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtLastSalesPrice" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 3
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice3" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Annual Sales Quantity
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtAnnualSalesQuantity" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 4
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice4" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Annual Sales $
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtAnnualSales" CssClass="calculated numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 5
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice5" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                                <div class="form-col">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div class="fc-label">
                                                                    Price 6
                                                                </div>
                                                            </td>
                                                            <td>
                                                                <div class="fc-input">

                                                                    <asp:TextBox ID="txtPrice6" CssClass="numeric form-control input-sm input-small" TabIndex="4" runat="server" MaxLength="75" />
                                                                </div>

                                                            </td>


                                                        </tr>

                                                    </table>

                                                </div>

                                            </div>

                                            <div class="clearfix"></div>
                                        </div>

                                        <!-- edit-tab end -->
                                        <div class="clearfix"></div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>

                    </div>



                    <div class="clearfix"></div>
                </div>

            </div>
        </div>
        <!-- edit-tab end -->
        <div class="clearfix"></div>




        <%--<div style="width: 500px;">
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none;"
                CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="programmaticModalPopup" BehaviorID="programmaticModalPopupBehavior"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="programmaticPopup"
                PopupDragHandleControlID="programmaticPopupDragHandle" BackgroundCssClass="ModalPopupBG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="programmaticPopup" Style="display: none; background: #fff; border: solid;"
                CssClass="roundCorner shadow setup-model1" ClientIDMode="Static">
                <div class="title_bar_popup" runat="server">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <asp:Label CssClass="title_text" ID="Label35" runat="server">Approver Info </asp:Label>
                </div>
                <div style="padding: 15px">


                    <asp:Panel ID="pnlInventoryWarehouse" runat="server">
                        <asp:HiddenField ID="hdninvvendinfo" runat="server" />
                        <table style="width: 100%; height: 150px">
                            <tr>
                                <td>MPN
                                        <asp:RequiredFieldValidator ID="rqInventoryMPN" runat="server" ControlToValidate="txtInventoryMPN"
                                            Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                        TargetControlID="rqInventoryMPN">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInventoryMPN" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>Approved Manufacturer
                                            <asp:RequiredFieldValidator ID="rqApprovedManufacturer" runat="server" ControlToValidate="txtInventoryApprovedManufacturer"
                                                Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                        TargetControlID="rqApprovedManufacturer">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtInventoryApprovedManufacturer" runat="server" CssClass="form-control" MaxLength="30"></asp:TextBox>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>Approved Vendor
                                             <asp:RequiredFieldValidator ID="rqApprovedVendor" runat="server" ControlToValidate="ddlInventoryApprovedVendor"
                                                 Display="None" ErrorMessage="Approved Vendor Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                        TargetControlID="rqApprovedVendor">
                                    </asp:ValidatorCalloutExtender>
                                </td>
                                <td>
                               
                                    <asp:DropDownList ID="ddlInventoryApprovedVendor" CssClass="form-control" runat="server"></asp:DropDownList>
                                </td>
                                <td>&nbsp;
                                </td>
                            </tr>
                        </table>
                     
                        <div class="custsetup-btn">
                            <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="lnkCloseInventoryWarehouse" Text="Close" CausesValidation="False" OnClick="lnkCloseInventoryWarehouse_Click" />
                            <asp:LinkButton ID="lnkSaveInventoryWarehouse" runat="server" CssClass="btn blue"
                                ValidationGroup="invware" OnClick="lnkSaveInventoryWarehouse_Click">Save Changes</asp:LinkButton>
                        </div>
                    </asp:Panel>



                </div>
            </asp:Panel>

            <div class="clearfix"></div>



        </div>--%>

        <div style="width: 500px;">
            <input tabindex="-1" name="hiddenTargetControlForModalPopup" value="" id="hiddenTargetControlForModalPopup" style="display: none;" type="submit">

            <div id="programmaticPopup">

                <div class="title_bar_popup">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <asp:Label CssClass="title_text" ID="Label35" runat="server">Approver Info </asp:Label>
                </div>
                <div style="padding: 15px">


                    <div id="pnlInventoryWarehouse">

                        <asp:HiddenField ID="hdninvvendinfo" runat="server" ClientIDMode="Static" />
                        <table style="width: 100%; height: 150px">
                            <tbody>
                                <tr>
                                    <td>MPN
                                       
                                           <asp:RequiredFieldValidator ID="rqInventoryMPN" runat="server" ControlToValidate="txtInventoryMPN"
                                               Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                            TargetControlID="rqInventoryMPN">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInventoryMPN" runat="server" CssClass="form-control" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Approved Manufacturer
                                           
                                       <asp:RequiredFieldValidator ID="rqApprovedManufacturer" runat="server" ControlToValidate="txtInventoryApprovedManufacturer"
                                           Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                            TargetControlID="rqApprovedManufacturer">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInventoryApprovedManufacturer" runat="server" CssClass="form-control" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Approved Vendor
                                            
                                         <asp:RequiredFieldValidator ID="rqApprovedVendor" runat="server" ControlToValidate="ddlInventoryApprovedVendor"
                                             Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invware" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                            TargetControlID="rqApprovedVendor">
                                        </asp:ValidatorCalloutExtender>

                                    </td>
                                    <td>

                                        <asp:DropDownList ID="ddlInventoryApprovedVendor" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                            </tbody>
                        </table>


                        <div class="custsetup-btn">
                            <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="lnkCloseInventoryWarehouse" ClientIDMode="Static" Text="Close" CausesValidation="False" OnClick="lnkCloseInventoryWarehouse_Click" />
                            <asp:LinkButton ID="lnkSaveInventoryWarehouse" runat="server" CssClass="btn blue"
                                ValidationGroup="invware" OnClick="lnkSaveInventoryWarehouse_Click" ClientIDMode="Static">Save Changes</asp:LinkButton>
                        </div>


                    </div>



                </div>

            </div>


            <div id="programmaticRequestQuotePopup">

                <div class="title_bar_popup">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
                    <asp:Label CssClass="title_text" ID="Label36" runat="server">Request Quote </asp:Label>
                </div>
                <div style="padding: 15px">


                    <div id="pnlRequestQuote">

                        <asp:HiddenField ID="hdninvid" runat="server" ClientIDMode="Static" />
                        <table style="width: 100%;">
                            <tbody>
                                <tr style="padding-bottom: 5px;">
                                    <td>Approved Vendor
                                            
                                         <asp:RequiredFieldValidator ID="rqApprovedVendorrequestquote" runat="server" ControlToValidate="ddlApprovedVendorrequestquote"
                                             Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                            TargetControlID="rqApprovedVendorrequestquote">
                                        </asp:ValidatorCalloutExtender>

                                    </td>
                                    <td>

                                        <asp:DropDownList ID="ddlApprovedVendorrequestquote" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <br />
                                </tr>

                                <tr>
                                    <td style="padding-top: 3px;">MPN & Manufacturer 
                                       
                                           <asp:RequiredFieldValidator ID="rqmpnrequestquote" runat="server" ControlToValidate="ddlMPNMannufacturer"
                                               Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                            TargetControlID="rqmpnrequestquote">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td style="padding-top: 3px;">
                                        <asp:DropDownList ID="ddlMPNMannufacturer" CssClass="form-control" runat="server" ClientIDMode="Static"></asp:DropDownList>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <td style="padding-top: 3px;">TO 
                                       
                                           <asp:RequiredFieldValidator ID="rqemail" runat="server" ControlToValidate="txtvendoremail"
                                               Display="None" ErrorMessage="Vendor email is required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                            TargetControlID="rqemail">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td style="padding-top: 3px;">
                                        <asp:TextBox ID="txtvendoremail" runat="server" ClientIDMode="Static" TextMode="Email" CssClass="form-control"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <td style="padding-top: 3px;">Quantity 
                                       
                                           <asp:RequiredFieldValidator ID="rqquantity" runat="server" ControlToValidate="txtquotequantity"
                                               Display="None" ErrorMessage="Quantity is Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                            TargetControlID="rqquantity">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td style="padding-top: 3px;">
                                        <asp:TextBox ID="txtquotequantity" runat="server" ClientIDMode="Static" CssClass="numeric form-control" Width="100px"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr></tr>
                                <tr>
                                    <%--<td>Quantity 
                                       
                                           <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtquotequantity"
                                               Display="None" ErrorMessage="Quantity is Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote" InitialValue="0"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8" runat="server" Enabled="True"
                                            TargetControlID="rqquantity">
                                        </asp:ValidatorCalloutExtender>
                                    </td>--%>
                                    <td colspan="2" style="border-bottom-style: groove; border-color: aliceblue; padding-top: 3px;">

                                        <asp:TextBox ID="txtemailcontentent" runat="server" ClientIDMode="Static" TextMode="MultiLine" CssClass="form-control" Style="min-width: 586px; min-height: 152px;"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr></tr>
                                <%-- <tr>
                                    <td>MPN
                                       
                                           <asp:RequiredFieldValidator ID="rqmpnrequestquote" runat="server" ControlToValidate="txtmpnrequestquote"
                                               Display="None" ErrorMessage="MPN Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                            TargetControlID="rqmpnrequestquote">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtmpnrequestquote" runat="server" CssClass="form-control" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>Approved Manufacturer
                                           
                                       <asp:RequiredFieldValidator ID="rqApprovedManufacturerrequestquote" runat="server" ControlToValidate="txtApprovedManufacturerrequestquote"
                                           Display="None" ErrorMessage="Approved Manufacturer Required" SetFocusOnError="True" ValidationGroup="invwarerequestquote"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                            TargetControlID="rqApprovedManufacturerrequestquote">
                                        </asp:ValidatorCalloutExtender>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtApprovedManufacturerrequestquote" runat="server" CssClass="form-control" MaxLength="30" ClientIDMode="Static"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>--%>
                            </tbody>
                        </table>


                        <div class="custsetup-btn">
                            <asp:Button CssClass="btn default" data-dismiss="modal" runat="server" ID="btnclose" ClientIDMode="Static" Text="Close" CausesValidation="False" />
                            <asp:LinkButton ID="lnksendRequestQuote" runat="server" CssClass="btn blue"
                                ValidationGroup="invwarerequestquote" ClientIDMode="Static">Send Quote</asp:LinkButton>
                        </div>


                    </div>



                </div>

            </div>


            <div class="clearfix"></div>



            <div id="shade"></div>
        </div>




    </div>
    <!-- END DASHBOARD STATS -->
    <div class="clearfix"></div>
</asp:Content>
