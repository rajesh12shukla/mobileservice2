<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddTicket.aspx.cs" Inherits="AddTicket" ValidateRequest="false" %>

<%@ Register Src="uc_CustomerSearch.ascx" TagName="uc_customersearch" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="js/signature/jquery.signaturepad.css" />
    <%--<script type="text/javascript" src="js/Signature/jquery.signaturepad.js"></script>--%>
    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false&amp;libraries=places&key=AIzaSyDedmuEC-1d__Jc1M3OLx1NIAnc_gMRbhE"></script>

    <script type="text/javascript" src="js/jquery.geocomplete.js"></script>
    <script src="Appearance/js/bootstrap-filestyle.js"></script>
    <script type="text/javascript" src="js/quickcodes.js"></script>

    <script type="text/javascript">

        /////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////        Page load      ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        // $(document).ready(function () {
        function pageLoad() {

            $('.sigPad').signaturePad();

            jQuery('.sign-title-l').click(function () {
                jQuery('#hdnDrawdata').val("");
            });

            ////When your form is submitted
            //$("form").submit(function (e) {
            //    //Display your loading icon
            //    if (Page_ClientValidate()) {
            //        $("#loading").show();
            //    }
            //});

            $("#NearWorkers").empty();
            $("#NearWorkers").append("<th style='padding:4px 5px; text-align:center'>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");

            var isTS = '<%= Session["MSM"].ToString() %>';
            var isTSint = '<%=ViewState["tsint"].ToString() %>';
            if (isTSint != "1") {
                if (isTS == 'TS') {
                    document.getElementById("<%=ddlStatus.ClientID%>").options[4].disabled = true;
                    document.getElementById("<%=ddlStatus.ClientID%>").options[4].onclick = function () { alert('Ticket can only be completed from Total Service.'); };
                }
            }

            ValidateChargeable();
            CheckIsProspect();
            calculateTotalTime();
            calculateMileage();
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
                        url: "CustomerAuto.asmx/GetCustomerProspect",
                        //data: '{"prefixText":' + JSON.stringify(request.term) + ',"con":' + JSON.stringify(document.getElementById('ctl00_ContentPlaceHolder1_hdnCon').value) + '}',
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        //                        error: function(result) {
                        //                            alert("Due to unexpected errors we were unable to load customers");
                        //                        }
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    if (ui.item.prospect == 1) {
                        $("#<%=txtLocation.ClientID%>").val('');
                        $("#<%=hdnLocId.ClientID%>").val(ui.item.value);
                        $("#<%=txtLocation.ClientID%>").attr("disabled", "disabled");
                        $("#<%=hdnProspect.ClientID%>").val('1');
                        document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                    }
                    else {
                        $("#<%=hdnPatientId.ClientID%>").val(ui.item.value);
                        $("#<%=txtLocation.ClientID%>").focus();
                        $("#<%=txtLocation.ClientID%>").val('');
                        $("#<%=hdnLocId.ClientID%>").val('');
                        $("#<%=hdnProspect.ClientID%>").val('');
                        document.getElementById('<%=btnSelectCustomer.ClientID%>').click();
                    }
                    return false;
                },
                focus: function (event, ui) {
                    // $("#<%=txtCustomer.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                var result_item = item.label;
                var result_desc = item.desc;
                var result_Prospect = item.prospect;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                var color = 'Black';
                if (result_Prospect != 0) {
                    color = 'brown';
                }
                return $("<li></li>")
		        .data("item.autocomplete", item)
		        .append("<a style='color:" + color + ";'>" + result_item + ", <span style='color:gray;'>" + result_desc + "</span></a>")
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
                        //  $("#<%=txtLocation.ClientID%>").val(ui.item.label);
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



            ///////////////////////Ajax call for equipment search//////////////////////////////
                function dataEquip() {
                    this.prefixText = null;
                }
                function dataEmpty() {
                    this.d = "";
                }

                $("#<%= txtUnit.ClientID %>").autocomplete({
                source: function (request, response) {
                    if ($("#<%=hdnLocId.ClientID%>").val() == '') {
                        var objdataEquip = new dataEquip();
                        objdataEquip.prefixText = request.term;
                        query = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "CustomerAuto.asmx/getEquipment",
                            data: JSON.stringify(objdataEquip),
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) { }
                        });
                    }
                    else {
                        var objdataEmpty = new dataEmpty();
                        response(objdataEmpty);
                    }
                },
                select: function (event, ui) {
                    $("#<%= txtUnit.ClientID %>").val(ui.item.label);
                    $("#<%= hdnUnitID.ClientID %>").val(ui.item.value);
                    $("#<%=txtLocation.ClientID%>").val(ui.item.tag);
                    $("#<%=hdnLocId.ClientID%>").val(ui.item.Loc);
                    document.getElementById('<%=btnSelectLoc.ClientID%>').click();
                    return false;
                },
                focus: function (event, ui) {
                    //$("#<%= txtUnit.ClientID %>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            })
            .data("autocomplete")._renderItem = function (ul, item) {
                debugger;
                var result_item = item.label;
                if (item.state != "") {
                    result_item = item.label + ", Unique# " + item.state;
                }
                var result_desc = "Customer: " + item.custname;
                var result_descLoc = "Location: " + item.LID + "-" + item.tag;

                var x = new RegExp('\\b' + query, 'ig');
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });
                if (result_desc != null) {
                    result_desc = result_desc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                if (result_descLoc != null) {
                    result_descLoc = result_descLoc.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });
                }
                return $("<li></li>")
		        .data("item.autocomplete", item)
		        .append("<a>" + result_item + "<BR/> <span style='color:Gray;'>" + result_desc + "</span><BR/> <span style='color:Gray;'>" + result_descLoc + "</span></a>")
		        .appendTo(ul);
            };



            ///////////// Validations for auto search ////////////////////
            $("#<%=txtCustomer.ClientID%>").keyup(function (e) {
                var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                var hdnLocID = document.getElementById('<%=hdnLocId.ClientID%>');

                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnPatientId.value = '';
                    hdnLocID.value = '';
                }

                //                if (txtCust.value == '') {
                //                    hdnPatientId.value = '';
                //                }

            });

            $("#<%=txtCustomer.ClientID%>").blur(function (e) {
                CheckIsProspect();
            });

            $("#<%=txtLocation.ClientID%>").keyup(function (event) {
                var hdnLocId = document.getElementById('<%=hdnLocId.ClientID%>');
                if (document.getElementById('<%=txtLocation.ClientID%>').value == '') {
                    hdnLocId.value = '';
                }
            });


            ///////////// Unit dropdown control handling ////////////////////
            $("#<%=txtUnit.ClientID%>").keyup(function (event) {
                var hdnUnitId = document.getElementById('<%=hdnUnitID.ClientID%>');
                if (document.getElementById('<%=txtUnit.ClientID%>').value == '') {
                    hdnUnitId.value = '';
                }
            });

            $("#<%=txtProject.ClientID%>").keyup(function (event) {
                var hdnProjectId = document.getElementById('<%=hdnProjectId.ClientID%>');
                if (document.getElementById('<%=txtProject.ClientID%>').value == '') {
                    hdnProjectId.value = '';
                    $('#<%= ddlTemplate.ClientID %>').prop('disabled', false);
                    $('#divNewPeoject').show();
                }
                ToggleValidator();
            });

            $("#<%=txtJobCode.ClientID%>").keyup(function (event) {
                var hdnProjectCode = document.getElementById('<%=hdnProjectCode.ClientID%>');
                if (document.getElementById('<%=txtJobCode.ClientID%>').value == '') {
                    hdnProjectCode.value = '';
                }
            });
            $("#<%=txtUnit.ClientID%>").click(function () {
                $("#divEquip").slideToggle();
                return false;
            });

            $("#<%=txtProject.ClientID%>").click(function () {
                $("#divProject").slideToggle();
                return false;
            });

            $("#<%=txtJobCode.ClientID%>").click(function () {
                $("#divProjectCode").slideToggle();
                return false;
            });

            ///////////// Select text onclick handling ////////////////////
            selectAll('<%=txtEnrTime.ClientID%>');
            selectAll('<%=txtOnsitetime.ClientID%>');
            selectAll('<%=txtComplTime.ClientID%>');
            selectAll('<%=txtRT.ClientID%>');
            selectAll('<%=txtOT.ClientID%>');
            selectAll('<%=txtNT.ClientID%>');
            selectAll('<%=txtDT.ClientID%>');
            selectAll('<%=txtTT.ClientID%>');

            ///////////// Calculate total time fields handling (KEYUP) ////////////////////
            $("#<%=txtRT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtOT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtNT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtDT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });

            $("#<%=txtTT.ClientID%>").keyup(function (event) {
                calculateTotalTime();
            });


            /////////// Calculate total time fields handling (BLUR)  ////////////////////
            $("#<%=txtRT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtOT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtNT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtDT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });

            $("#<%=txtTT.ClientID%>").blur(function (event) {
                if (this.value == '') {
                    this.value = '0.00';
                } calculateTotalTime();
            });



            ///////////// Calculate time fields handling (KEYUP)  ////////////////////
            $("#<%=txtEnrTime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            $("#<%=txtOnsitetime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            $("#<%=txtComplTime.ClientID%>").keyup(function (event) {
                calculate_Time();
            });

            ///////////// Calculate time fields handling (BLUR)  ////////////////////
            //            $("#<%=txtEnrTime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });

            //            $("#<%=txtOnsitetime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });

            //            $("#<%=txtComplTime.ClientID%>").blur(function(event) {
            //                calculate_Time();
            //            });


            ///////////// Calculate Mileage (KEYUP) ////////////////////
            $("#<%=txtMileStart.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            $("#<%=txtMileEnd.ClientID%>").keyup(function (event) {
                calculateMileage();
            });

            ///////////// Calculate Mileage (BLUR) ////////////////////
            $("#<%=txtMileStart.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            $("#<%=txtMileEnd.ClientID%>").blur(function (event) {
                calculateMileage();
            });

            ///////////// Quick Codes //////////////

            $('#<%=txtReason.ClientID%>').change(function () { $("#<%=hdnIsEdited.ClientID%>").val('1'); });

            $("#<%=txtReason.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtReason.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#<%=txtWorkCompl.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtWorkCompl.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            $("#<%=txtRemarks.ClientID%>").keyup(function (event) {
                replaceQuickCodes(event, '<%=txtRemarks.ClientID%>', $("#<%=hdnCon.ClientID%>").val());
            });

            ////////////////////////////// Textbox resize ///////////////////
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
                    height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtReason.ClientID%>').focus(function () {
                $(this).animate({
                    width: '520px'
                    , height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });


            $('#<%=txtReason.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%'
                , height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtRecommendation.ClientID%>').focus(function () {
                $(this).animate({
                    width: '520px'
                    , height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtRecommendation.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtCreditReason.ClientID%>').focus(function () {
                $(this).animate({
                    width: '520px'
                    , height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtCreditReason.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtWorkCompl.ClientID%>').focus(function () {
                $(this).animate({
                    width: '520px'
                    , height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });


            $('#<%=txtWorkCompl.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%'
                    , height: '63px'
                }, 500, function () {
                    // Animation complete.
                });
            });


            ///////////// Invoice ////////////////////////////////////
            //            $('#<%=txtInvoiceNo.ClientID%>').blur(function() {
            //                CheckIsInvoiced();
            //            });

            $('#<%=chkInvoice.ClientID%>').change(function () {
                ValidateChargeable();
            });

            ///////////// Signature box handling  ////////////////////


            $("#signbg").click(function () {
                if (isCanvasSupported()) {
                    $("#sign").toggle();
                    $("#sign").focus();
                }
                else {
                    alert('Signature not supported on this web browser.');
                }
            });

            $("#sign").blur(function () {
                $("#sign").hide();
            });


            $("#convertpngbtn").click(function () {
                $("#sign").hide();
                toImage();
            });

            var oImg = document.getElementById("<%=imgSign.ClientID%>");
            var ImgHdn = document.getElementById("<%=hdnImg.ClientID%>");
            oImg.src = ImgHdn.value;




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
                $("#Coord").toggle();
                initialize();
            });

            $('#<%= ddlCategory.ClientID %>').change(function () {
                CategoryImage();
            });

            CategoryImage();

            //CallNearestWorker();

            initialize();
            //            window.onbeforeunload = function() {
            //                if(!window.btn_clicked){window.opener.document.getElementById("ctl00_ContentPlaceHolder1_btnSearch").click();}};

            //document.body.classList.add('page-sidebar-closed');
            //document.getElementById('ctl00_toggleMenu').classList.add('page-sidebar-menu-closed');

            if ($('#<%= hdnProjectId.ClientID %>').val() != "" && $('#<%= hdnProjectId.ClientID %>').val() != "0") {
                $('#<%= ddlTemplate.ClientID %>').prop('disabled', true);
                $('#divNewPeoject').hide();
            }
            else {
                $('#<%= ddlTemplate.ClientID %>').prop('disabled', false);
                $('#divNewPeoject').show();
            }


            $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").click(CheckUncheckAllCheckBoxAsNeeded);
            $("#<%=gvEquip.ClientID%> input[id*='chkAll']:checkbox").click(function () {
                if ($(this).is(':checked')) {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', true);
                }
                else {
                    $("#<%=gvEquip.ClientID%> input[id*='chkSelect']:checkbox").attr('checked', false);
                }
                SelectRows('<%=gvEquip.ClientID%>', '<%=txtUnit.ClientID%>', '<%=hdnUnitID.ClientID%>');

            });

            //});
        }




        /////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////        Functions      ///////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

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

        function isCanvasSupported() {
            var elem = document.createElement('canvas');
            return !!(elem.getContext && elem.getContext('2d'));
        }


        function CategoryImage() {
            var ddlCat = document.getElementById('<%=ddlCategory.ClientID%>');
             var cat = ddlCat.options[ddlCat.selectedIndex].value;
             $("#<%=imgCategory.ClientID%>").attr("src", 'imagehandler.ashx?catid=' + cat);
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

        function ace_itemSelected(sender, e) {
            //            var hdnPatientId = document.getElementById('ctl00_ContentPlaceHolder1_hdnPatientId');
            //            hdnPatientId.value = e.get_value();
            //            document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
        }

        function cancel() {
            ////window.parent.document.getElementById('ctl00_ContentPlaceHolder1_hideModalPopupViaServer').click();
            //var conf = confirm('Do you want to close the ticket screen?')
            //if (conf) {
            window.close();
            //}
        }

        ///////////////////////////////    Convert signature to image      ////////////////////////////////
        function toImage() {
            var hdnDrawdata = document.getElementById("hdnDrawdata");
            var hdnImg = document.getElementById("<%=hdnImg.ClientID%>");
            var oImgElement = document.getElementById("<%=imgSign.ClientID%>");
            var canvas = document.getElementById("canvas");
            if (hdnDrawdata.value != "") {
                var img = canvas.toDataURL("image/png");
                oImgElement.src = img;
                hdnImg.value = img;
            }
        }

        ////////////////////////    Select all text in textbox on click     ////////////////////////////
        function selectAll(id) {
            var eraInput = document.getElementById(id);
            $("#" + id).click(function () {
                //                setTimeout(function() {
                eraInput.setSelectionRange(0, 9999);
                //                }, 10);
            });
        }

        ////////////////////////    Calculate time difference between ER-OS-CT    ////////////////////////////
        function calculate_Time() {

            var txtER = $("#<%=txtEnrTime.ClientID%>").val();
            var txtOS = $("#<%=txtOnsitetime.ClientID%>").val();
            var txtCT = $("#<%=txtComplTime.ClientID%>").val();

            if (txtOS != '' && txtER != '' && txtOS != '__:__ AM' && txtOS != '__:__ PM' && txtER != '__:__ AM' && txtER != '__:__ PM') {
                var diff = Math.round(((new Date('01/01/2009 ' + txtOS) - new Date('01/01/2009 ' + txtER)) / 1000 / 60 / 60) * 100) / 100;
                $("#<%=txtTT.ClientID%>").val(diff.toFixed(2));
            }
            else {
                $("#<%=txtTT.ClientID%>").val('0.00');
            }

            if (txtOS != '' && txtCT != '' && txtOS != '__:__ AM' && txtOS != '__:__ PM' && txtCT != '__:__ AM' && txtCT != '__:__ PM') {
                var diff = Math.round(((new Date('01/01/2009 ' + txtCT) - new Date('01/01/2009 ' + txtOS)) / 1000 / 60 / 60) * 100) / 100;
                $("#<%=txtRT.ClientID%>").val(diff.toFixed(2));
            }
            else {
                $("#<%=txtRT.ClientID%>").val('0.00');
            }

            calculateTotalTime();
        }

        ////////////////////////    Calculate total time    ////////////////////////////
        function calculateTotalTime() {
            var txtRT = $("#<%=txtRT.ClientID%>").val();
            var txtOT = $("#<%=txtOT.ClientID%>").val();
            var txtNT = $("#<%=txtNT.ClientID%>").val();
            var txtDT = $("#<%=txtDT.ClientID%>").val();
            var txtTT = $("#<%=txtTT.ClientID%>").val();

            if (txtRT == '') {
                txtRT = '0.00';
            }
            if (txtOT == '') {
                txtOT = '0.00';
            }
            if (txtNT == '') {
                txtNT = '0.00';
            }
            if (txtDT == '') {
                txtDT = '0.00';
            }
            if (txtTT == '') {
                txtTT = '0.00';
            }

            var total = Math.round((parseFloat(txtRT) + parseFloat(txtOT) + parseFloat(txtNT) + parseFloat(txtDT) + parseFloat(txtTT)) * 100) / 100;

            $("#<%=txtTotal.ClientID%>").text(total.toFixed(2));

        }

        ////////////////////  Check Translation ///////////////////
        function checkTranslation() {

            ToggleValidator();

            if (Page_ClientValidate()) {

                var charge = ValidateChargeable();
                if (charge == false) {
                    return false;
                }

                var prospect = NewProspectAlert();
                if (prospect == false) {
                    return false;
                }

                var hdnMultiLang = $("#<%=hdnMultiLang.ClientID%>").val();
                var hdnLang = $("#<%=hdnLang.ClientID%>").val();
                var hdnIsEdited = $("#<%=hdnIsEdited.ClientID%>").val();

                if (hdnLang != 'english' && hdnIsEdited == '1' && hdnMultiLang == '1') {

                    //                var r = confirm('The ticket has not been converted to ' + hdnLang + '. Do you want to convert the ticket to ' + hdnLang + ' ?');
                    var r = confirm('The reason for service text has been changed but not translated to Spanish/English. Click OK to open the translation box else cancel to continue save the ticket.');
                    if (r == true) {
                        //$('#<%=pnlTranslate.ClientID%>').show();
                        $('#<%=txtReason.ClientID%>').focus();
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
        }

        ////////////////////  Check Follow-up ticket ///////////////////
        function CheckWorkComplete(ticid, checked, msg, comp, invoice) {

            //            var checked = $("#<%=chkWorkComp.ClientID%>").is(':checked');
            if (checked == 0) {
                var r = confirm('Would you like to create a follow-up ticket at this time?');
                if (r == true) {
                    window.location.href = "addticket.aspx?copy=1&follow=1&id=" + ticid + "&comp=" + comp;
                }
                else {
                    noty({ text: msg, dismissQueue: true, type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
                }
            }
            else {
                noty({ text: msg, dismissQueue: true, type: 'success', layout: 'topCenter', closeOnSelfClick: true, timeout: false, theme: 'noty_theme_default', closable: false });
            }
            if (invoice == 1) {
                window.open('addinvoice.aspx?o=1&tickid=' + ticid, "Invoice", "height=768,width=1280,scrollbars=yes");
            }
        }



        /////////////////// Calculate Traveled Mileage //////////////////
        function calculateMileage() {
            var txtSM = $("#<%=txtMileStart.ClientID%>").val();
            var txtEM = $("#<%=txtMileEnd.ClientID%>").val();

            if (txtSM == '') {
                txtSM = '0';
            }
            if (txtEM == '') {
                txtEM = '0';
            }

            var total = parseInt(txtEM) - parseInt(txtSM);

            $("#<%=txtMileTraveled.ClientID%>").text(total);
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
            { document.getElementById('ctl00_ContentPlaceHolder1_TabContainer2_TabPanel3_lnkUploadDoc').click(); }
            else
            { document.getElementById('ctl00_ContentPlaceHolder1_TabContainer2_TabPanel3_lnkPostback').click(); }
        }

        //////////////// Confirm Mail Send to worker ///////////////////
        function notyConfirm() {
            noty({
                dismissQueue: true,
                layout: 'topCenter',
                theme: 'noty_theme_default',
                animateOpen: { height: 'toggle' },
                animateClose: { height: 'toggle' },
                easing: 'swing',
                text: 'Do you want to send text message to worker?',
                type: 'alert',
                speed: 500,
                timeout: false,
                closeButton: false,
                closeOnSelfClick: true,
                closeOnSelfOver: false,
                force: false,
                onShow: false,
                onShown: false,
                onClose: false,
                onClosed: false,
                buttons: [
                            {
                                type: 'btn btn-primary', text: 'Ok', click: function ($noty) {
                                    // this = button element
                                    // $noty = $noty element
                                    $noty.close();
                                    $("#<%=btnMail.ClientID%>").click();
                                    // noty({ force: true, text: 'Sending Mail', type: 'success', layout: 'topCenter' });
                                }
                            },
                                {
                                    type: 'btn btn-danger', text: 'Cancel', click: function ($noty) {
                                        $noty.close();
                                        //noty({ force: true, text: 'You clicked "Cancel" button', type: 'error', layout: 'topLeft' });
                                    }
                                }
                ],
                modal: true,
                template: '<div class="noty_message"><span class="noty_text"></span><div class="noty_close"></div></div>',
                cssPrefix: 'noty_',
                custom:
                {
                    container: null
                }
            });
                    }

                    ////////////////// Called when save form and validation caused on tab 1 //////////////
                    function tabchng() {

                        tc = $find("<%=TabContainer2.ClientID%>");
                        tc.set_activeTabIndex(0);
                    }


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
                        $("#<%= txtAddress.ClientID %>").bind('input propertychange', function () {
                            $("#txtGoogleAutoc").val('');
                        });
                        $("#txtGoogleAutoc").on("blur", function () {
                            $("#txtGoogleAutoc").val('');
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

                        if (document.getElementById('<%= hdnComp.ClientID %>').value != "2" && document.getElementById('<%= hdnComp.ClientID %>').value != "1") {
                            CallNearestWorker();
                        }
                    }

                    function InitMap(lat, lng) {
                        var address = new google.maps.LatLng(lat, lng);
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

                    function ValidateTime(sender, args) {
                        var start = document.getElementById('<%=txtEnrTime.ClientID%>').value;
                        var end = document.getElementById('<%=txtOnsitetime.ClientID%>').value;

                        var fromdt = "2010/01/01 " + start;
                        var todt = "2010/01/01 " + end;

                        var from = new Date(Date.parse(fromdt));
                        var to = new Date(Date.parse(todt));

                        if (from > to) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    }

                    function ValidateTimeComplete(sender, args) {
                        var start = document.getElementById('<%=txtOnsitetime.ClientID%>').value;
                        var end = document.getElementById('<%=txtComplTime.ClientID%>').value;

                        var fromdt = "2010/01/01 " + start;
                        var todt = "2010/01/01 " + end;

                        var from = new Date(Date.parse(fromdt));
                        var to = new Date(Date.parse(todt));

                        if (from > to) {
                            args.IsValid = false;
                        } else {
                            args.IsValid = true;
                        }
                    }

                    function ValidateChargeable() {
                        var chkChargeable = document.getElementById('<%=chkChargeable.ClientID%>');
                        var chkInvoice = document.getElementById('<%=chkInvoice.ClientID%>');
                        var txtInvoiceID = document.getElementById('<%=txtInvoiceNo.ClientID%>');

                        if (chkInvoice.checked == true) {

                            if (chkChargeable.checked == false) {
                                alert('Ticket must be chargeable to generate invoice.');
                                chkInvoice.checked = false;
                                return false;
                            }
                            else {
                                return true;
                            }
                        }
                        else {
                            return true;
                        }
                    }

                    function CheckIsProspect() {
                        var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                        var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                        var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                        var hdnProspect = document.getElementById('<%=hdnProspect.ClientID%>');
                        if (hdnPatientId.value == '' && txtCust.value != '') {
                            txtLoc.disabled = true;
                            txtLoc.value = '';
                            hdnProspect.value = "1";
                            txtCust.style.color = "brown";
                            ValidatorEnable(document.getElementById('<%=RequiredFieldValidator1.ClientID%>'), false);
                            //document.getElementById('<%=btnValidateLocation.ClientID%>').click();
                            document.getElementById('<%=txtAcctno.ClientID%>').value = '--';
                        }
                        else {
                            hdnProspect.value = "";
                        }
                    }

                    function NewProspectAlert() {
                        var hdnPatientId = document.getElementById('<%=hdnPatientId.ClientID%>');
                        var txtLoc = document.getElementById('<%=txtLocation.ClientID%>');
                        var txtCust = document.getElementById('<%=txtCustomer.ClientID%>');
                        var hdnLocID = document.getElementById('<%=hdnLocId.ClientID%>');
                        var lblalertprospect = document.getElementById('<%=lblAlertProspect.ClientID%>');
                        if (hdnPatientId.value == '' && txtCust.value != '' && hdnLocID.value == '') {
                            //                var isTS = '<%= Session["MSM"].ToString() %>';
                            //                if (isTS != 'TS') {
                            var r = confirm('You have not selected an existing Customer/Lead. The entered Customer name will be considered as a new Lead. Click OK to continue creating a new Lead or click on Cancel to select an existing Customer/Lead.');
                            return r;
                            //                }
                            //                else{
                            //                    alert('Please select existing Customer/Lead.');
                            //                    return false;
                            //                }
                        }
                    }

                    function CheckIsInvoiced() {
                        var txtInvoiceID = document.getElementById('<%=txtInvoiceNo.ClientID%>');
                        var chkInvoice = document.getElementById('<%=chkInvoice.ClientID%>');
                        var chkCharge = document.getElementById('<%=chkChargeable.ClientID%>');

                        if (txtInvoiceID.value != '') {
                            chkInvoice.checked = false;
                        }
                    }

                    //        function CallNearestWorker() {
                    //        
                    //            //CallNearestWorkerList('');
                    //            var ddlDefRoute = document.getElementById('<%= ddlDefRoute.ClientID %>');
        //            $.when(CallNearestWorkerList('')).then(CallNearestWorkerList(ddlDefRoute.options[ddlDefRoute.selectedIndex].text));
        //            //CallNearestWorkerList(ddlDefRoute.options[ddlDefRoute.selectedIndex].text);
        //        }

        function CallNearestWorker() {
            // if (document.getElementById('<%= hdnComp.ClientID %>').value != "2" && document.getElementById('<%= hdnComp.ClientID %>').value != "1") {
            var txtlat = document.getElementById('<%= lat.ClientID %>');
            var txtlng = document.getElementById('<%= lng.ClientID %>');

            if (txtlat.value == '') {

                var mainaddress = document.getElementById('<%= txtAddress.ClientID %>').value;
                var city = document.getElementById('<%= txtCity.ClientID %>').value;
                var state = document.getElementById('<%= ddlState.ClientID %>').value;
                var zip = document.getElementById('<%= txtZip.ClientID %>').value;
                var address = mainaddress + ', ' + city + ', ' + state + ', ' + zip;

                if (mainaddress != '') {
                    $("#wait").show();
                    codeAddress(address, function (latlng) {
                        txtlat.value = latlng.lat();
                        txtlng.value = latlng.lng();
                        InitMap(latlng.lat(), latlng.lng());
                        CallNearestWorkerList(latlng.lat(), latlng.lng());
                    });
                }
            }
            else {
                $("#wait").show();
                CallNearestWorkerList(txtlat.value, txtlng.value);
            }
            //}
        }



        function CallNearestWorkerList(lat, lng) {
            var worker = '';
            if (lat != '') {
                if (lng != '') {
                    function NearWorkerData() {
                        this.lat = null;
                        this.lng = null;
                        this.worker = null;
                    }

                    var objNearWorkerData = new NearWorkerData();
                    objNearWorkerData.lat = lat;
                    objNearWorkerData.lng = lng;
                    objNearWorkerData.worker = worker;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "NearWorker.asmx/GetNearWorker",
                        data: JSON.stringify(objNearWorkerData),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            drawTable($.parseJSON(data.d), worker);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //var err = eval("(" + XMLHttpRequest.responseText + ")");
                            //alert(err.Message);
                        }
                    }).done(CallDefaultworker);
                }
            }
        }

        function CallDefaultworker() {
            var ddlDefRoute = document.getElementById('<%= ddlDefRoute.ClientID %>');
            var worker = ddlDefRoute.options[ddlDefRoute.selectedIndex].text
            var lat = document.getElementById('<%= lat.ClientID %>').value;
            var lng = document.getElementById('<%= lng.ClientID %>').value;
            if (lat != '') {
                if (lng != '') {
                    function NearWorkerData() {
                        this.lat = null;
                        this.lng = null;
                        this.worker = null;
                    }

                    var objNearWorkerData = new NearWorkerData();
                    objNearWorkerData.lat = lat;
                    objNearWorkerData.lng = lng;
                    objNearWorkerData.worker = worker;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "NearWorker.asmx/GetNearWorker",
                        data: JSON.stringify(objNearWorkerData),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            drawTable($.parseJSON(data.d), worker);
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            //var err = eval("(" + XMLHttpRequest.responseText + ")");
                            //alert(err.Message);
                        }
                    });
                }
            }
            $("#wait").hide();
        }

        function drawTable(data, worker) {
            if (worker == "") {
                $("#NearWorkers").empty();
                $("#NearWorkers").append("<th>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");
                for (var i = 0; i < data.length; i++) {
                    drawRow(data[i]);
                }
            }
            else {
                for (var i = 0; i < data.length; i++) {
                    drawRowDefault(data[i]);
                }
            }
        }
        function drawRow(rowData) {
            //if (rowData.latitude != "" && rowData.longitude != "") {
            if (rowData.GPS == 1) {
                getGeoAddress(rowData.latitude, rowData.longitude, function (addr) {
                    createrow(rowData, addr);
                });
            }
            else {
                createrow(rowData, rowData.address);
            }
        }
        function createrow(rowData, addr) {
            var row = $("<tr/>")
            $("#NearWorkers").append(row);
            var strrow = "<td><hr/><b>" + rowData.worker + "</b> <i>" + rowData.dist + " miles at " + rowData.Time + "</i><br/>";
            if (addr != '') {
                strrow += "<span style='font-size:7pt;'>" + addr + "</span>";
            }
            strrow += "</td>";
            row.append($(strrow));
        }

        function drawRowDefault(rowData) {
            //if (rowData.latitude != "" && rowData.longitude != "") {
            if (rowData.GPS == 1) {
                getGeoAddress(rowData.latitude, rowData.longitude, function (addr) {
                    CreateRowDefault(rowData, addr);
                });
            }
            else {
                CreateRowDefault(rowData, rowData.address);
            }
        }
        function CreateRowDefault(rowData, addr) {
            var row = $("<tr/>")
            row.prependTo("#NearWorkers > tbody");
            var strrow = "<td style='background-color:yellow;'><hr/><b>" + rowData.worker + "</b> <i>" + rowData.dist + " miles at " + rowData.Time + "</i><br/>";
            if (addr != '') {
                strrow += "<span style='font-size:7pt;'>" + addr + "</span>";
            }
            strrow += "</td>";
            row.append($(strrow));
        }

        function getGeoAddress(lat, lng, callback) {
            var latlng = new google.maps.LatLng(lat, lng);
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {
                        callback(results[1].formatted_address);
                    }
                }
                else {
                    callback('');
                }
            });
        }

        function codeAddress(address, callback) {
            var success = 0;
            var geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'address': address }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0].geometry.location_type == 'ROOFTOP') {
                        callback(results[0].geometry.location);
                        success = 1;
                    }
                }
            });
            //            if (success == 0) {
            //                $("#NearWorkers").empty();
            //                $("#NearWorkers").append("<th>Nearest Workers <img src='images/refresh2.png' alt='refresh' title='Refresh' onclick='CallNearestWorker();' style='cursor:pointer; float:right' /></th>");
            //            }
        }
        $(":file").filestyle({
            buttonName: "btn-primary",
        });

        function ToggleValidator() {
            var chk = false;
            var valName = document.getElementById("<%=rfvjtempl.ClientID%>");
            if (document.getElementById("<%=ddlStatus.ClientID%>").value == '4') {
                chk = (document.getElementById("<%=hdnProjectId.ClientID%>").value == '' || document.getElementById("<%=hdnProjectId.ClientID%>").value == '0') ? true : false
            }
            ValidatorEnable(valName, chk);
        }

        function checkMaxLength(textarea, evt, maxLength) {
            if ($("#<%= hdnSageInt.ClientID %>").val() == "1") {
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

        function OpenWOreport() {
            if ($("#<%= txtWO.ClientID %>").val() != "" && $("#<%= hdnLocId.ClientID %>").val() != "")//&& $("#<%= chkReviewed.ClientID %>").is(':checked') == true && $("#<%= chkInternet.ClientID %>").is(':checked') == true
            {
                window.open("printwo.aspx?wo=" + $("#<%= txtWO.ClientID %>").val() + "&lid=" + $("#<%= hdnLocId.ClientID %>").val(), '_blank');
            }
        }

        function NewProject() {
            $('#<%= hdnProjectId.ClientID %>').val("");
            $('#<%= txtProject.ClientID %>').val("--New Project--");
            $('#<%= ddlTemplate.ClientID %>').prop('disabled', false);
            $('#divNewPeoject').show();
            $("#divProject").slideToggle();
        }


        <%-- function AlertHourPercent() {
            var grid = document.getElementById('<%=gvEquip.ClientID%>');
             var cell;
             var total = 0;

             if (grid.rows.length > 0) {
                 for (i = 1; i < grid.rows.length; i++) {
                     cell = grid.rows[i].cells[7];
                     cell1 = grid.rows[i].cells[0];
                     if (cell1.childNodes[3].checked == true) {
                         if (cell.childNodes[1].value != '') {
                             var text = cell.childNodes[1].value;                             
                             total = total + text;
                         }
                     }
                 }
                 if (total > 100) {
                     alert('Total % must not be less than 100');
                 }
                 else if (total < 100) {
                     alert('Total % must not be greater than 100');
                 }
                 else if (total = 0) {
                     alert('Total must be 100');
                 }
             }
        }

        function SelectEquipFromAutoselect(unitid) {

            var Gridview = document.getElementById('<%=gvEquip.ClientID%>');

            $(Gridview).find('tr:not(:first)').each(function () {
                var $tr = $(this);
                var id = $tr.find('span[id*=lblID]');
                var select = $tr.find('input[id*=chkSelect]');

                if (id.text() == unitid)
                    select.prop('checked', true);
            });
        }--%>

    </script>

    <style type="text/css">
        .ui-autocomplete {
            max-width: 800px;
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

        .highlighted {
            background-color: Yellow;
        }

        .menu_popup_chklst {
            position: absolute;
            /*top: 251px;
            right: 57px;*/
            z-index: 1;
            display: none;
            background: transparent;
            overflow: auto; /*border:solid 1px black;  	width:550px; */
            max-height: 260px;
            min-height: 10px;
            overflow-x: hidden;
        }

        .shadow {
            /* rgba(0, 0, 0, 0.3) rgb(90, 168, 208)*/
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
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
                    <span>Schedule Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/TicketListView.aspx") %>">Ticket List View</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Tickets</span>
                </li>
            </ul>--%>
            <div class="page-bar-right">
                <%--          <asp:ImageButton ID="lnkSave" AlternateText="Save" ToolTip="Save Ticket" ImageUrl="images/save1.png"
                    Height="20px" runat="server" Style="float: left; margin-right: 15px" OnClick="lnkSave_Click"
                    TabIndex="36" OnClientClick="tabchng(); return checkTranslation();" />--%>


                <%--    <asp:ImageButton ID="lnkPrint" AlternateText="Print" ToolTip="Save and Print Ticket"
                    ImageUrl="images/print1.png" Height="20px" runat="server" Style="float: left; margin-right: 15px"
                    OnClick="lnkPrint_Click" TabIndex="36" OnClientClick="tabchng(); return checkTranslation();" />--%>


                <%-- <a runat="server" id="lnkCancelContact1"href="#" onclick="cancel();"
                    style="float: left; margin-right: 5px" tabindex="36">
                    <img alt="Close" src="images/close1.png" height="20px" /></a>--%>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <asp:Panel runat="server" ID="pnlGridButtons">
                        <ul class="lnklist-header">
                            <li>
                                <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Ticket</asp:Label></li>
                            <li>
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
                                </li>
                            <li>
                                <asp:LinkButton CssClass="icon-save" ID="lnkSave" ToolTip="Save Ticket" runat="server" CausesValidation="false" OnClick="lnkSave_Click"
                                    OnClientClick="tabchng(); return checkTranslation();"></asp:LinkButton></li>
                            <li>
                                <asp:LinkButton CssClass="icon-print" ID="lnkPrint" runat="server" CausesValidation="false"
                                    OnClick="lnkPrint_Click" ToolTip="Save and Print Ticket" OnClientClick="tabchng(); return checkTranslation();"></asp:LinkButton></li>
                            <li>
                                <asp:HyperLink ID="lnkPDF" runat="server" style="float:left" Target="_blank" Visible="false">
                                  <img alt="PDF" src="images/pdficon.png" style="width:26px" title="PDF"/>
                                    </asp:HyperLink>
                            </li>
                            <li>
                                <a class="icon-closed" id="lnkCancelContact" title="Close" onclick="window.close();" ></a>

                            </li>
                            <li>
                                <asp:Button ID="btnMail" runat="server" Text="Mail" Style="display: none;" OnClick="btnMail_Click"
                                    CausesValidation="False" />
                            </li>
                        </ul>
                    </asp:Panel>

                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" ChildrenAsTriggers="true">
                    <ContentTemplate>

                    <asp:HiddenField ID="hdnReviewed" runat="server" />
                    <asp:HiddenField ID="hdnProspect" runat="server" />
                    <asp:HiddenField ID="hdnImg" runat="server" />
                    <input id="hdnCon" runat="server" type="hidden" />
                    <input id="hdnPatientId" runat="server" type="hidden" />
                    <input id="hdnLocId" runat="server" type="hidden" />
                    <input id="hdnUnitID" runat="server" type="hidden" />
                    <input id="hdnProjectId" runat="server" type="hidden" />
                    <input id="hdnRolID" runat="server" type="hidden" />
                    <input id="hdnLang" runat="server" type="hidden" />
                    <input id="hdnMultiLang" runat="server" type="hidden" />
                    <input id="hdnFormId" runat="server" type="hidden" />
                    <input id="hdnComp" runat="server" type="hidden" />
                    <input id="hdnProjectCode" runat="server" type="hidden" />
                    <input id="hdnSageInt" runat="server" type="hidden" />

                    <asp:Button ID="btnValidateLocation" runat="server" Style="display: none;" OnClick="txtCustomer_TextChanged"
                        CausesValidation="False" />
                    </ContentTemplate>
                    </asp:UpdatePanel>
                    <div>
                        <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
                            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Ticket Info">
                                <HeaderTemplate>
                                    Ticket Info.
                                </HeaderTemplate>
                                <ContentTemplate>
                                     <asp:UpdatePanel ID="UpdatePanel5" runat="server" ChildrenAsTriggers="true">
                                         <Triggers>
                                             <asp:PostBackTrigger ControlID="lnkConvert" />
                                         </Triggers>
                                        <ContentTemplate>
                                    <div class="row">
                                        <asp:Panel ID="pnlCustomer" runat="server" Visible="False" Width="100%">
                                            <table class="roundCorner shadow" style="border: solid 1px red" width="1000px" align="center">
                                                <tr>
                                                    <td colspan="2">
                                                        <div style="text-align: center">
                                                            Please select an existing customer or leave the field blank and this will create
                                            a new Customer.
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">
                                                        <uc1:uc_customersearch ID="uc_CustomerSearch1" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;
                                                    </td>
                                                    <td>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                        <div class="col-md-9 col-lg-9">
                                            <div class="row">
                                                <div class="col-md-12 col-lg-12">
                                                    <div class="row">
                                                        <div class="col-lg-12">
                                                            <table id="tblTicketID" cellpadding="0" cellspacing="0" runat="server"
                                                                visible="False">
                                                                <tr runat="server">
                                                                    <td runat="server" class="register_lbl">
                                                                        <asp:Label ID="lblTicketLabel" runat="server" Text="Ticket #" Visible="False"></asp:Label>
                                                                    </td>
                                                                    <td runat="server">
                                                                        <asp:Image ID="imgHigh" runat="server" Visible="False" Width="16px" ToolTip="Declined"
                                                                            ImageUrl="images/exclamation.png" />
                                                                    </td>
                                                                    <td runat="server">
                                                                        <asp:Label ID="lblTicketnumber" runat="server" Style="font-weight: bold; font-size: 15px; float: left;"
                                                                            Visible="False"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>

                                                       <div class="col-md-4 col-lg-4">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    <a href="#" onclick="OpenWOreport();">WO #</a>
                                                                         <asp:HiddenField ID="hdnWO" runat="server" />
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtWO"
                                                                        Display="None" ErrorMessage="WO # Required" SetFocusOnError="True" Enabled="False"
                                                                        ValidationGroup="wo"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator33_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator33">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input merchant-input">
                                                                    <asp:TextBox ID="txtWO" runat="server" CssClass="form-control" MaxLength="10"
                                                                        TabIndex="1"></asp:TextBox>
                                                                    <asp:ImageButton ID="lblRelatedTickets" runat="server" ToolTip="Related Tickets"
                                                                        CausesValidation="False" OnClick="lblRelatedTickets_Click" Style="float: right;"
                                                                        ImageUrl="images/tickets.png" Height="20px" />
                                                                    <div runat="server" id="divRelated" style="padding: 2px; max-height: 300px; position: absolute; min-width: 260px; background-color: #fff;"
                                                                        class="roundCorner shadow" visible="False">
                                                                        <asp:LinkButton ID="lnkCloseRelated" runat="server" OnClick="lnkCloseRelated_Click"
                                                                            CausesValidation="False">Close</asp:LinkButton>
                                                                        <div style="overflow-y: auto; overflow-x: hidden; max-height: 280px;" class="roundCorner">
                                                                            <asp:ListView ID="lstRelatedTickets" runat="server">
                                                                                <LayoutTemplate>
                                                                                    <table border="0" cellpadding="1" width="260px">
                                                                                        <tr>
                                                                                            <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <EmptyDataTemplate>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td>No records available.
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </EmptyDataTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <div class="roundCorner" style="background-color: <%#Eval("color") %>">
                                                                                                <asp:HyperLink runat="server" ID="lblId" NavigateUrl='<%# String.Format("addticket.aspx?id={0}&comp={1}", Eval("id"), Eval("comp")) %>'
                                                                                                    Text='<%#Eval("ID") %>'></asp:HyperLink>
                                                                                                <strong>
                                                                                                    <asp:Label runat="server" ID="lblLoc"><%#Eval("locname") %></asp:Label></strong>
                                                                                                <br />
                                                                                                <i>
                                                                                                    <asp:Label runat="server" ID="lblReason"><%# objGeneralFunctions.TruncateWithText(Eval("fdesc").ToString(), 29)%></asp:Label></i>
                                                                                                <br />
                                                                                                <asp:Label runat="server" ID="lblDate"><%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %></asp:Label>
                                                                                                <asp:Label runat="server" ID="lblStatus"><%#Eval("assignname") %></asp:Label>
                                                                                            </div>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:ListView>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                            
                                                        <div class="col-md-4 col-lg-4">
                                                            <div class="form-col">
                                                                <asp:HyperLink ID="lnkOpport" runat="server" Style="float: right;"
                                                                    Target="_blank" Visible="False"></asp:HyperLink>
                                                                <div class="fc-label">
                                                                    Invoice #
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtInvoiceNo" runat="server" CssClass="form-control" MaxLength="12"
                                                                        TabIndex="4" Enabled="False"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="txtInvoiceNo_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="False" FilterType="Numbers" TargetControlID="txtInvoiceNo">
                                                                    </asp:FilteredTextBoxExtender>
                                                                </div>
                                                                <a id="lnkInvoice" runat="server" target="_blank" title="Invoice" visible="False"
                                                                    style="cursor: pointer;">
                                                                    <asp:Image ID="imgInv" runat="server" Style="height: 28px; margin: 1px 2px 0px 5px"
                                                                        ToolTip="Invoice" ImageUrl="images/Get_invoice.png" />
                                                                </a>
                                                            </div>
                                                        </div>

                                                           </div>
                                                </div>

                                                    <div class="col-md-12 col-lg-12">
                                                    <div class="row">

                                                        <div class="col-md-4 col-lg-4">
                                                        
                                                                <div class="4project-col">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Project#
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <td>
                                                                            <asp:TextBox runat="server" TabIndex="2" CssClass="form-control"  ID="txtProject" autocomplete="off"></asp:TextBox>
                                                                            <asp:Button ID="btnGetCode" runat="server" CausesValidation="False" Text="Button" Style="display: none;" OnClick="btnGetCode_Click" />
                                                                            <div id="divProject" class="menu_popup_chklst shadow">
                                                                                <div class="table"><a onclick="NewProject();">Add New</a></div>
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
                                                                            </div>
                                                                        </td>
                                                                    </div>
                                                                </div>
                                                                 </div>
                                                             </div>

                                                        <div class="col-md-4 col-lg-4" id="divNewPeoject" style="display:none">
                                                                <div class="4project-col">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                       New Project
                                                                    </div>
                                                                    <div class="fc-input">
                                                                                    <asp:DropDownList ID="ddlTemplate" runat="server"  onchange="ToggleValidator();"
                                                                        CssClass="form-control" />
                                                                         <asp:RequiredFieldValidator ID="rfvjtempl" runat="server" ControlToValidate="ddlTemplate"
                                                                        Display="None" ErrorMessage="Please select the Project Template" InitialValue="0" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender8"
                                                                        runat="server" Enabled="True"  TargetControlID="rfvjtempl">
                                                                    </asp:ValidatorCalloutExtender>
                                                                 </div>
                                                                </div>
                                                                </div>
                                                         </div>

                                                        <div class="col-md-4 col-lg-4">
                                                                <div class="4project-col">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Project Type
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox runat="server" TabIndex="3"  CssClass="form-control" ID="txtJobCode" autocomplete="off"></asp:TextBox>
                                                                        <div id="divProjectCode" class="menu_popup_chklst shadow">
                                                                            <asp:GridView ID="gvProjectCode" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                DataKeyNames="ID" PageSize="20" Width="353px">
                                                                                <RowStyle CssClass="evenrowcolor" />
                                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                                <Columns>
                                                                                     <asp:TemplateField HeaderText="Type">
                                                                                        <ItemTemplate>
                                                                                                                                                                                        
                                                                                            <asp:Label ID="lblID" style="display:none" runat="server" Text='<%# Eval("code") %>'></asp:Label>
                                                                                             <asp:Label ID="lblIDname" style="display:none" runat="server" Text='<%# Eval("line") +":"+ Eval("code") +":"+ Eval("fdesc")  %>'></asp:Label>
                                                                                            <asp:Label ID="lblIDname1" style="display:none" runat="server" Text='<%# Eval("Code") +"/"+ Eval("line") +"/"+ Eval("fdesc")  %>'></asp:Label>

                                                                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("bomtype") %>'></asp:Label>
                                                                                            <asp:Label ID="lblphase" style="display:none" runat="server" Text='<%# Eval("line") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="0px" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblDesc" runat="server"><%#Eval("fdesc")%></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                            </asp:GridView>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                      </div>
                                                            </div>

                                                          

                                                    </div>
                                                </div>
                                                <div class="col-md-8 col-lg-8">
                                                    <div class="row">
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Customer Name
                                                                        <asp:LinkButton ID="lnkConvert" runat="server" Visible="False" OnClick="lnkConvert_Click"
                                                                            CausesValidation="False">Convert Lead</asp:LinkButton>
                                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ChkCustomer"
                                                                        ControlToValidate="txtCustomer" Display="None" ErrorMessage="Please select the customer"
                                                                        SetFocusOnError="True" Enabled="False"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server" Enabled="True"
                                                                        PopupPosition="BottomLeft" TargetControlID="CustomValidator1">
                                                                    </asp:ValidatorCalloutExtender>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtCustomer"
                                                                        Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator19_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator19">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:Label ID="lblAlertProspect" runat="server" Style="padding: 3px; left: 211px; top: 38px; display: none; position: absolute; color: Gray; background-color: White;"
                                                                        CssClass="roundCorner transparent shadow" Text="You have not selected the existing customer/lead. This will add new lead."></asp:Label>
                                                                    <div>
                                                                        <asp:TextBox ID="txtCustomer" runat="server" autocomplete="off" CssClass="form-control searchinput"
                                                                            placeholder="Search by customer name, phone#, address etc." TabIndex="5"></asp:TextBox>
                                                                        <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                                            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                                                        </asp:FilteredTextBoxExtender>
                                                                       
                                                                        <asp:Button ID="btnSelectCustomer" runat="server" CausesValidation="False" OnClick="btnSelectCustomer_Click"
                                                                            Style="display: none;" Text="Button" />
                                                                       
                                                                    </div>                                                                   
                                                                </div>
                                                            </div>
                                                        </div>

                                                          <div class="col-md-12 col-lg-12">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                     <asp:Label ID="lblSageid" runat="server" Text="SageID"></asp:Label>
                                                                   </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustSageID" runat="server" CssClass="form-control" Width="137px" ReadOnly="True" MaxLength="10"></asp:TextBox>
                                                                </div>
                                                                </div>
                                                              </div>
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Location Name<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
                                                                        ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator1_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator1">
                                                                        </asp:ValidatorCalloutExtender>
                                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ChkLocation"
                                                                        ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                                        SetFocusOnError="True"></asp:CustomValidator>
                                                                    <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                                                        TargetControlID="CustomValidator2">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtLocation" runat="server" autocomplete="off" CssClass="form-control searchinputloc"
                                                                        placeholder="Search by location name, phone#, address etc." TabIndex="6"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                                                        Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                                                    </asp:FilteredTextBoxExtender>
                                                                   
                                                                    <asp:Button ID="btnSelectLoc" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                                                        Style="display: none;" Text="Button" />
                                                                            
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 col-lg-6">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    ID
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtAcctno" TabIndex="7" runat="server" CssClass="form-control" ReadOnly="True" MaxLength="15"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-lg-12 ">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Address<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtAddress"
                                                                        Display="None" ErrorMessage="Address Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator11_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator11">
                                                                        </asp:ValidatorCalloutExtender>
                                                                    <a id="mapLink" style="display: none">
                                                                        <img src="images/map.ico" title="Map" class="shadowHover" width="20px" height="20px" />
                                                                    </a>
                                                                    <span id="spnAddress" style="color:red; display:none; ">Max 2 lines 30 characters each</span>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <div style="border:1px solid #cecece">
                                                                    <input id="txtGoogleAutoc" name="name" style="width: 100%; height:20px; border:none" placeholder="Search address here on Google Maps"></input>                                                                   
                                                                       
                                                                         <asp:TextBox ID="txtAddress" runat="server" 
                                                                        ONKEYUP="return checkMaxLength(this, event, 35)"
                                                                        MaxLength="255" TabIndex="14" TextMode="MultiLine"
                                                                         style="border:none; width: 100%;"></asp:TextBox>
                                                                          
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 col-lg-6 col-sm-6">
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
                                                                        TabIndex="15"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    State/Prov.<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server"
                                                                        InitialValue="State" ControlToValidate="ddlState" Display="None" ErrorMessage="State Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator7_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator7">
                                                                        </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control" TabIndex="19"
                                                                        ToolTip="State">
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
                                                                    <asp:TextBox ID="txtZip" runat="server" CssClass="form-control" MaxLength="10"
                                                                        TabIndex="21"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Main Contact
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtMaincontact" runat="server" CssClass="form-control" MaxLength="50"
                                                                        TabIndex="25"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Phone
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtPhoneCust" runat="server" CssClass="form-control" MaxLength="28"
                                                                        TabIndex="28"></asp:TextBox>
                                                                    <asp:MaskedEditExtender ID="txtPhoneCust_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                        ClearMaskOnLostFocus="False" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                        CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                        CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999"
                                                                        MaskType="Number" TargetControlID="txtPhoneCust">
                                                                    </asp:MaskedEditExtender>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    <span>Caller<asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server"
                                                                        ControlToValidate="txtNameWho" Display="None" ErrorMessage="Caller Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator31_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator31">
                                                                        </asp:ValidatorCalloutExtender>
                                                                    </span>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtNameWho" runat="server" CssClass="form-control" MaxLength="30"
                                                                        TabIndex="30"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Caller Phone
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCell" runat="server" CssClass="form-control" MaxLength="28"
                                                                        TabIndex="32"></asp:TextBox>
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
                                                                    <asp:LinkButton ID="btnCodes" runat="server" CausesValidation="False" OnClick="btnCodes_Click">Reason for service</asp:LinkButton>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtReason"
                                                                        Display="None" ErrorMessage="Reason for Service Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator30_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator30">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <div runat="server" id="pnlTranslate" style="display: none;">
                                                                        <asp:HiddenField ID="hdnIsEdited" runat="server" Value="0" />
                                                                        <div height="16px">
                                                                            Spanish text (sent from/to device)
                                                                            <asp:LinkButton ID="lnkTransReasonToEnglish" runat="server" CausesValidation="False"
                                                                                OnClick="lnkTransReasonToEnglish_Click">
                                                                               Translate to English</asp:LinkButton>
                                                                            <a id="lnkCloseTranslate" onclick="$('#<%=pnlTranslate.ClientID%>').hide();" style="float: right; vertical-align: top; margin-right: 10px; cursor: pointer">X</a>
                                                                        </div>
                                                                        <asp:TextBox ID="txtTranslate" runat="server"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                    <div id="divTransIconReason" runat="server" style="height: 55px; background-color: Gray; display: none">
                                                                        <div style="margin: 5px 5px 5px 5px">
                                                                            <asp:LinkButton ID="lnkTranslate" runat="server" CausesValidation="False" OnClick="lnkTranslate_Click">
                                                                            <img alt="Translate" src="images/translate.png" class="shadowHover" title="Translate to Spanish/English"
                                                                        width="20" height="20" /></asp:LinkButton>
                                                                        </div>
                                                                        <div style="margin: 5px 5px 5px 5px">
                                                                            <a id="btnTranslate" onclick="$('#<%=pnlTranslate.ClientID%>').show();" tabindex="32">
                                                                                <img alt="Translate" src="images/showdiv.png" class="shadowHover" title="Show Spanish Text"
                                                                                    width="20" height="20" />
                                                                            </a>
                                                                        </div>
                                                                    </div>
                                                                    <asp:HoverMenuExtender ID="HoverMenuExtender1" runat="server" PopupControlID="divTransIconReason"
                                                                        TargetControlID="txtReason" Enabled="True" DynamicServicePath="" HoverDelay="1"
                                                                        OffsetX="-30">
                                                                    </asp:HoverMenuExtender>
                                                                    <asp:TextBox ID="txtReason" runat="server" Style="position: relative; z-index: 100"
                                                                        MaxLength="255" TabIndex="32" TextMode="MultiLine"
                                                                        CssClass="form-control"></asp:TextBox>
                                                                    <asp:Panel ID="pnlCodes" runat="server" CssClass="pnl-codes" Visible="False">
                                                                        <div class="model-popup-body">
                                                                            <asp:Label ID="lblCodeHeader" runat="server" CssClass="title_text"></asp:Label>
                                                                            <asp:LinkButton ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click" Style="color: white; float: right; margin-left: 10px"
                                                                                Text="Close" CssClass="buttonsBox" />
                                                                            <asp:LinkButton ID="btnDone" runat="server" CausesValidation="False" OnClick="btnDone_Click" Style="color: white; float: right;"
                                                                                Text="Save" CssClass="buttonsBox" />
                                                                        </div>
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                                                        <ContentTemplate>
                                                                                            <div>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td valign="top" style="width: 200px">
                                                                                                            <fieldset class="roundCorner" style="border: 1px solid #D9D9D9; padding: 5px 5px 5px 5px; margin: 5px 5px 5px 5px;">
                                                                                                                <h3><b>Categories </b></h3>
                                                                                                                <asp:ListBox ID="ddlCodeCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCodeCat_SelectedIndexChanged"
                                                                                                                    Style="color: #686767; font: 14px/31px Arial,Helvetica,sans-serif;"
                                                                                                                    Height="170px" Width="165px"></asp:ListBox>
                                                                                                            </fieldset>
                                                                                                        </td>
                                                                                                        <td valign="top" style="width: 200px">
                                                                                                            <fieldset class="roundCorner" style="border: 1px solid #D9D9D9; padding: 5px 5px 5px 5px; margin: 5px 5px 5px 5px;">
                                                                                                                <h3><b>Description </b></h3>
                                                                                                                <div style="height: 170px; width: 225px; overflow-y: scroll;">
                                                                                                                    <asp:CheckBoxList ID="chklstCodes" runat="server">
                                                                                                                    </asp:CheckBoxList>
                                                                                                                </div>
                                                                                                            </fieldset>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </div>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    <asp:LinkButton ID="btnCodesCmpl" runat="server" CausesValidation="False" OnClick="btnCodesCmpl_Click"
                                                                        Text="Work complete desc." Enabled="False"></asp:LinkButton>
                                                                    <asp:Label ID="lblWCD" runat="server" Visible="False"></asp:Label>
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtWorkCompl"
                                                                        Display="None" Enabled="False" ErrorMessage="Work complete description Required"
                                                                        SetFocusOnError="True" Style="z-index: 9999 !important;"></asp:RequiredFieldValidator>
                                                                    <asp:ValidatorCalloutExtender ID="RequiredFieldValidator28_ValidatorCalloutExtender"
                                                                        runat="server" Enabled="True" PopupPosition="TopLeft" TargetControlID="RequiredFieldValidator28">
                                                                    </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <div runat="server" id="pnlTransDesc" style="display: none;">
                                                                        <div>
                                                                            Spanish text (sent from/to device)
                                                                                <asp:LinkButton ID="lnkTransDescToEnglish" runat="server" CausesValidation="False"
                                                                                    OnClick="lnkTransDescToEnglish_Click">
                                                                                   Translate to English</asp:LinkButton>
                                                                            <a id="lnkClosetransDesc" onclick="$('#<%=pnlTransDesc.ClientID%>').hide();" style="float: right; vertical-align: top; margin-right: 10px; cursor: pointer">X</a>
                                                                        </div>
                                                                        <asp:TextBox ID="txtTransDesc" runat="server"
                                                                            TextMode="MultiLine"></asp:TextBox>
                                                                    </div>
                                                                    <div id="divTransicon" runat="server" style="width: 30px; height: 55px; background-color: Gray; display: none">
                                                                        <div style="margin: 5px 5px 5px 5px">
                                                                            <asp:LinkButton ID="lnkTransEnglish" runat="server" CausesValidation="False" OnClick="lnkTransEnglish_Click">
                                             <img alt="Translate" src="images/translate.png" class="shadowHover" title="Translate to Spanish/English"
                                            width="20" height="20" /></asp:LinkButton>
                                                                        </div>
                                                                        <div style="margin: 5px 5px 5px 5px">
                                                                            <a id="lnkTransDesc" onclick="$('#<%=pnlTransDesc.ClientID%>').show();" tabindex="33">
                                                                                <img alt="Translate" src="images/showdiv.png" class="shadowHover" title="Show Spanish Text"
                                                                                    width="20" height="20" />
                                                                            </a>
                                                                        </div>
                                                                    </div>
                                                                    <asp:HoverMenuExtender ID="HoverMenuExtender2" runat="server" PopupControlID="divTransicon"
                                                                        TargetControlID="txtWorkCompl" Enabled="True" DynamicServicePath="" HoverDelay="1"
                                                                        OffsetX="-30">
                                                                    </asp:HoverMenuExtender>
                                                                    <asp:TextBox ID="txtWorkCompl" runat="server" MaxLength="255" TabIndex="34" Style="z-index: 9999; position: relative"
                                                                        CssClass="form-control" TextMode="MultiLine" Enabled="False"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Recommendation
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtRecommendation" runat="server" CssClass="form-control" Style="position: relative; z-index: 100"
                                                                        Enabled="False" TextMode="MultiLine"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    <asp:CheckBox ID="chkCreditHold" runat="server" TabIndex="33" Text="Hold Service" />
                                                                    <asp:CheckBox ID="chkDispAlert" runat="server" TabIndex="34" Text="Alert" />
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCreditReason" runat="server" CssClass="form-control" Style="position: relative; z-index: 100"
                                                                        Height="63px" placeholder="Reason" TextMode="MultiLine" TabIndex="35" ToolTip="Reason"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 col-lg-6 col-sm-6">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Category<asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server"
                                                                        ControlToValidate="ddlCategory" Display="None" ErrorMessage="Category Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator20_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator20">
                                                                        </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input merchant-input">
                                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"
                                                                        TabIndex="16">
                                                                    </asp:DropDownList>
                                                                    <asp:Image ID="imgCategory" runat="server" AlternateText="Icon" ToolTip="Category Icon"
                                                                        Width="26px" CssClass="roundCorner" Style="float: right; margin-top: 1px;" />
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Equipment
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtUnit" runat="server"  Style="position: relative; z-index: 100"
                                                                         autocomplete="off" CssClass="form-control" TextMode="MultiLine"
                                                                        TabIndex="20"></asp:TextBox>
                                                                    <asp:Button ID="btnEquip" CausesValidation="False" OnClick="btnEquip_Click" style="display:none" runat="server" Text="Button" />
                                                                    <div id="divEquip" class="menu_popup_chklst shadow" style="width:640px">

                                                                        <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                            DataKeyNames="ID" PageSize="20">
                                                                            <RowStyle CssClass="evenrowcolor" />
                                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                            <Columns>
                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Style="display: none;" Text='<%# Bind("id") %>'></asp:Label>
                                                                                          <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                    </ItemTemplate>
                                                                                      <HeaderTemplate>
                                                            <asp:CheckBox ID="chkAll" runat="server" />
                                                        </HeaderTemplate>
                                                                                    <ItemStyle Width="0px" />
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
                                                                                     <asp:TemplateField HeaderText="% Hours">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtHours" runat="server" Width="50px" MaxLength="20"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                            </Columns>
                                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                        </asp:GridView>
                                                                            
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Department<asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server"
                                                                        ControlToValidate="ddlDepartment" Display="None" ErrorMessage="Department Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="RequiredFieldValidator32_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator32">
                                                                        </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"
                                                                        TabIndex="22">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Wage<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                                                                        ControlToValidate="ddlWage" Enabled="False" Display="None" ErrorMessage="Wage Required"
                                                                        SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                            ID="ValidatorCalloutExtender9" runat="server" Enabled="True"
                                                                            TargetControlID="RequiredFieldValidator2">
                                                                        </asp:ValidatorCalloutExtender>
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlWage" runat="server" CssClass="form-control"
                                                                        TabIndex="22">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Service Item
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlService" runat="server" CssClass="form-control"
                                                                        TabIndex="26" OnSelectedIndexChanged="ddlService_SelectedIndexChanged"
                                                                        AutoPostBack="True">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Payroll Item
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlPayroll" runat="server" CssClass="form-control"
                                                                        TabIndex="31">
                                                                    </asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div>
                                                                    <b>Time spent</b>
                                                                </div>
                                                                <div>
                                                                    <ul class="time-spent">
                                                                        <li>
                                                                            <div class="ts-title">RT</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtRT" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="15">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtRT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtRT" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtRT_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                    CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtRT">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">OT</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtOT" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="16">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtOT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtOT" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtOT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtOT">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">1.7</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtNT" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="17">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtNT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtNT" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtNT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtNT">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">DT</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtDT" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="18">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtDT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtDT" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtDT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtDT">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">TT</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtTT" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtTT_FilteredTextBoxExtender" runat="server" Enabled="True"
                                                                                    TargetControlID="txtTT" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtTT_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtTT">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">Total</div>
                                                                            <div>
                                                                                <asp:Label ID="txtTotal" runat="server" Style="font-weight: bold;">0</asp:Label>
                                                                            </div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div>
                                                                    <b>Expenses</b>
                                                                </div>
                                                                <div>
                                                                    <ul class="time-spent">
                                                                        <li>
                                                                            <div class="ts-title">Misc</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtExpMisc" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtExpMisc_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="txtExpMisc" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtExpMisc_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                    CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpMisc">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">Toll</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtExpToll" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtExpToll_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="txtExpToll" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtExpToll_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpToll">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">Zone</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtExpZone" runat="server" CssClass="form-control" MaxLength="28"
                                                                                    step="any" TabIndex="19">0.00</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtExpZone_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="txtExpZone" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtExpZone_MaskedEditExtender" runat="server" CultureAMPMPlaceholder=""
                                                                                    CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder=""
                                                                                    CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder=""
                                                                                    Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtExpZone">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div>
                                                                    <b>Mileage</b>
                                                                </div>
                                                                <div>
                                                                    <ul class="time-spent">
                                                                        <li>
                                                                            <div class="ts-title">Starting</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtMileStart" runat="server" CssClass="form-control"
                                                                                    MaxLength="28" step="any" TabIndex="19">0</asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="txtMileStart_FilteredTextBoxExtender" runat="server"
                                                                                    Enabled="True" TargetControlID="txtMileStart" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtMileStart_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                    CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtMileStart">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">Ending</div>
                                                                            <div class="ts-input">
                                                                                <asp:TextBox ID="txtMileEnd" runat="server" CssClass="form-control"
                                                                                    MaxLength="28" step="any" TabIndex="19">0</asp:TextBox><asp:FilteredTextBoxExtender
                                                                                        ID="txtMileEnd_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="txtMileEnd"
                                                                                        ValidChars="1234567890.-">
                                                                                    </asp:FilteredTextBoxExtender>
                                                                                <asp:MaskedEditExtender ID="txtMileEnd_MaskedEditExtender" runat="server" AutoComplete="False"
                                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                                    CultureTimePlaceholder="" Enabled="False" Mask="99.99" MaskType="Number" TargetControlID="txtMileEnd">
                                                                                </asp:MaskedEditExtender>
                                                                            </div>
                                                                        </li>
                                                                        <li>
                                                                            <div class="ts-title">Traveled</div>
                                                                            <div>
                                                                                <asp:Label ID="txtMileTraveled" runat="server" Style="font-weight: bold;">0</asp:Label>
                                                                            </div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="form-col">
                                                                <div>
                                                                    <b>Custom Fields</b>
                                                                </div>
                                                                <div>

                                                                    <table style="height: 140px">
                                                                        <tr>
                                                                            <td class="register_lbl">
                                                                                <asp:Label ID="lblCustomTick1" runat="server">TicketCustom1</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTickCustom1" TabIndex="39" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="register_lbl">
                                                                                <asp:Label ID="lblCustomTick2" runat="server">TicketCustom2</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTickCustom2" TabIndex="40" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="register_lbl">
                                                                                <asp:Label ID="lblCustomTick5" runat="server">TicketCustom3</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtTickCustom3" TabIndex="41" runat="server" MaxLength="28" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True"
                                                                                    TargetControlID="txtTickCustom3" ValidChars="1234567890.-">
                                                                                </asp:FilteredTextBoxExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="register_lbl">
                                                                                <asp:Label ID="lblCustomTick3" runat="server">TicketCheckbox1</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkTickCustom1" TabIndex="42" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="register_lbl">
                                                                                <asp:Label ID="lblCustomTick4" runat="server">TicketCheckbox2</asp:Label>
                                                                            </td>
                                                                            <td>
                                                                                <asp:CheckBox ID="chkTickCustom2" TabIndex="43" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-4 col-lg-4">

                                                    <div class="form-col">
                                                        <div>
                                                            Options
                                                        </div>
                                                        <div>

                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkInternet" runat="server" TabIndex="8" Text="Internet" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkChargeable" runat="server" TabIndex="9" Text="Chargeable " />
                                                                    </td>

                                                                    <td rowspan="2"></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkReviewed" runat="server" TabIndex="10" Text="Reviewed" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkWorkComp" runat="server" Checked="True" TabIndex="11" Text="Work Complete" />
                                                                    </td>

                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkInvoice" runat="server" TabIndex="12" Text="Invoice" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkTimeTrans" runat="server" TabIndex="13" Text="Timesheet" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                       
                                                            <div>Entered by
                                                                </div>
                                                            <div>
                                                                <asp:TextBox ID="txtFby" CssClass="form-control" runat="server"></asp:TextBox>
                                                            </div>
                                                           
                                                        </div>
                                                        <div class="form-col">
                                                        <div class="col-lg-6">
                                                            <div>
                                                                Date Called In
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtCallDt"
                                                                    Display="None" ErrorMessage="Date Called In Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator23_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator23">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:MaskedEditValidator ID="MaskedEditValidator5" runat="server" ControlExtender="txtCallTime_MaskedEditExtender"
                                                                    ControlToValidate="txtCallTime" Display="None" ErrorMessage="MaskedEditValidator5"
                                                                    InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                                <asp:ValidatorCalloutExtender ID="MaskedEditValidator5_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="MaskedEditValidator5">
                                                                </asp:ValidatorCalloutExtender>
                                                            </div>
                                                            <div>
                                                                <asp:TextBox ID="txtCallDt" runat="server" CssClass="form-control" MaxLength="28"
                                                                    TabIndex="17"></asp:TextBox>
                                                                <asp:CalendarExtender ID="txtCallDt_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtCallDt">
                                                                </asp:CalendarExtender>
                                                            </div>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            <div>
                                                                Time
                                                            </div>
                                                            <div>
                                                                <asp:TextBox ID="txtCallTime" runat="server" CssClass="form-control" MaxLength="28"
                                                                    TabIndex="18"></asp:TextBox>
                                                                <asp:MaskedEditExtender ID="txtCallTime_MaskedEditExtender" runat="server" AcceptAMPM="True"
                                                                    CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                    CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                    CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtCallTime">
                                                                </asp:MaskedEditExtender>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div class="col-lg-6">
                                                            Date Scheduled<asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server"
                                                                ControlToValidate="txtSchDt" Display="None" ErrorMessage="Date Scheduled Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="RequiredFieldValidator24_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator24">
                                                                </asp:ValidatorCalloutExtender>
                                                            <asp:MaskedEditValidator ID="MaskedEditValidator3" runat="server" ControlExtender="txtSchTime_MaskedEditExtender"
                                                                ControlToValidate="txtSchTime" Display="None" EmptyValueMessage="Time is required"
                                                                ErrorMessage="MaskedEditValidator3" InvalidValueMessage="Time is invalid" IsValidEmpty="False"
                                                                SetFocusOnError="True"></asp:MaskedEditValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender3" runat="server" Enabled="True"
                                                                PopupPosition="Left" TargetControlID="MaskedEditValidator3">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:TextBox ID="txtSchDt" runat="server" CssClass="form-control" MaxLength="28"
                                                                TabIndex="23"></asp:TextBox>
                                                            <asp:CalendarExtender ID="txtSchDt_CalendarExtender" runat="server" Enabled="True"
                                                                TargetControlID="txtSchDt">
                                                            </asp:CalendarExtender>
                                                        </div>
                                                        <div class="col-lg-6">
                                                            Time
                                                                 <asp:TextBox ID="txtSchTime" runat="server" CssClass="form-control" MaxLength="28"
                                                                     TabIndex="24"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="txtSchTime_MaskedEditExtender" runat="server" AcceptAMPM="True"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtSchTime">
                                                            </asp:MaskedEditExtender>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <span>Status<asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server"
                                                                ControlToValidate="ddlStatus" Display="None" ErrorMessage="Status Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="RequiredFieldValidator22_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator22">
                                                                </asp:ValidatorCalloutExtender>
                                                            </span>
                                                        </div>
                                                        <div>
                                                            <asp:DropDownList ID="ddlStatus" runat="server" AutoPostBack="True" CssClass="form-control"
                                                                OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" TabIndex="27">
                                                                <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                                <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                                <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                                <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                                <asp:ListItem Value="4">Completed</asp:ListItem>
                                                                <asp:ListItem Value="5">Hold</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <span>Worker<asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server"
                                                                ControlToValidate="ddlRoute" Display="None" Enabled="False" ErrorMessage="Worker Required"
                                                                SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                                                    ID="RequiredFieldValidator25_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                                    PopupPosition="Left" TargetControlID="RequiredFieldValidator25">
                                                                </asp:ValidatorCalloutExtender>
                                                                <asp:Label ID="lblWorkStatus" runat="server" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                                                            </span>
                                                        </div>
                                                        <div>
                                                            <asp:DropDownList ID="ddlRoute" runat="server" CssClass="form-control" TabIndex="30"
                                                                AutoPostBack="True" OnSelectedIndexChanged="ddlRoute_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <asp:LinkButton ID="btnEnroute" runat="server" CausesValidation="False" OnClick="btnEnroute_Click"
                                                                Text="Enroute"></asp:LinkButton>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtEnrTime"
                                                                Display="None" Enabled="False" ErrorMessage="Enroute time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator26_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator26">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtEnrTime" runat="server" CssClass="form-control" MaxLength="28"
                                                                TabIndex="28"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AcceptAMPM="True"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtEnrTime">
                                                            </asp:MaskedEditExtender>
                                                            <asp:MaskedEditValidator ID="MaskedEditValidator1" runat="server" ControlExtender="MaskedEditExtender1"
                                                                ControlToValidate="txtEnrTime" Display="None" ErrorMessage="MaskedEditValidator1"
                                                                InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender4" runat="server" Enabled="True"
                                                                PopupPosition="Left" TargetControlID="MaskedEditValidator1">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <asp:LinkButton ID="btnOnsite" runat="server" CausesValidation="False" OnClick="btnOnsite_Click"
                                                                Text="Onsite"></asp:LinkButton>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtOnsitetime"
                                                                Display="None" Enabled="False" ErrorMessage="Onsite time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator29_ValidatorCalloutExtender"
                                                                runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator29">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtOnsitetime" runat="server" CssClass="form-control" MaxLength="28"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99:99" MaskType="Time"
                                                                AcceptAMPM="True" TargetControlID="txtOnsitetime" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True">
                                                            </asp:MaskedEditExtender>
                                                            <asp:MaskedEditValidator ID="MaskedEditValidator2" runat="server" ControlExtender="MaskedEditExtender2"
                                                                ControlToValidate="txtOnsitetime" InvalidValueMessage="Time is invalid" Display="None"
                                                                SetFocusOnError="True" ErrorMessage="MaskedEditValidator2" />
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender5" runat="server" Enabled="True"
                                                                TargetControlID="MaskedEditValidator2" PopupPosition="Left">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:CustomValidator ID="CustomValidator3" runat="server" Display="None" ErrorMessage="Time can't be less than enroute time."
                                                                ControlToValidate="txtOnsitetime" ClientValidationFunction="ValidateTime" SetFocusOnError="True"></asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="CustomValidator3_ValidatorCalloutExtender" runat="server"
                                                                Enabled="True" TargetControlID="CustomValidator3" PopupPosition="Left">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <span>
                                                                <asp:LinkButton ID="btnComplete" runat="server" CausesValidation="False" OnClick="btnComplete_Click"
                                                                    Text="Complete"></asp:LinkButton>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtComplTime"
                                                                    Display="None" Enabled="False" ErrorMessage="Completed time Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator27_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator27">
                                                                </asp:ValidatorCalloutExtender>
                                                            </span>
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtComplTime" runat="server" CssClass="form-control" MaxLength="28"
                                                                TabIndex="30"></asp:TextBox>
                                                            <asp:MaskedEditExtender ID="MaskedEditExtender3" runat="server" AcceptAMPM="True"
                                                                CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat=""
                                                                CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True" Mask="99:99" MaskType="Time" TargetControlID="txtComplTime">
                                                            </asp:MaskedEditExtender>
                                                            <asp:MaskedEditValidator ID="MaskedEditValidator4" runat="server" ControlExtender="MaskedEditExtender3"
                                                                ControlToValidate="txtComplTime" Display="None" ErrorMessage="MaskedEditValidator4"
                                                                InvalidValueMessage="Time is invalid" SetFocusOnError="True"></asp:MaskedEditValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender6" runat="server" Enabled="True"
                                                                PopupPosition="Left" TargetControlID="MaskedEditValidator4">
                                                            </asp:ValidatorCalloutExtender>
                                                            <asp:CustomValidator ID="CustomValidator4" runat="server" Display="None" ErrorMessage="Time can't be less than onsite time."
                                                                ControlToValidate="txtComplTime" ClientValidationFunction="ValidateTimeComplete"
                                                                SetFocusOnError="True"></asp:CustomValidator>
                                                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender7" runat="server" Enabled="True"
                                                                TargetControlID="CustomValidator4" PopupPosition="Left">
                                                            </asp:ValidatorCalloutExtender>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            EST
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtEST" runat="server" CssClass="form-control" MaxLength="28"
                                                                TabIndex="36">00.00</asp:TextBox>
                                                            <asp:MaskedEditExtender ID="txtEST_MaskedEditExtender" runat="server" Mask="99.99"
                                                                MaskType="Number" TargetControlID="txtEST" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                CultureTimePlaceholder="" Enabled="True">
                                                            </asp:MaskedEditExtender>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            Default Worker <span>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="ddlDefRoute"
                                                                    Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator34_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator34">
                                                                </asp:ValidatorCalloutExtender>
                                                        </div>
                                                        <div>
                                                            <asp:DropDownList ID="ddlDefRoute" runat="server" TabIndex="37" Style="margin-left: 3px" CssClass="form-control"
                                                                onchange="CallNearestWorker();">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-col">
                                                        <div>
                                                            <asp:Label ID="lblRemarks" runat="server" Text="Location Remarks" Visible="False"></asp:Label>
                                                        </div>
                                                        <div>
                                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TabIndex="38"
                                                                MaxLength="8000"
                                                                TextMode="MultiLine"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3 col-lg-3">

                                            <div>
                                                <input id="locality" disabled="disabled" style="display: none;" />
                                                <input id="country" disabled="disabled" style="display: none;" />
                                                <div id="Coord" style="display: block; background-color: #E5E3DF; border: 1px solid;">

                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="lat"
                                                                    Display="None" ErrorMessage="Required" Enabled="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator35_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator35">
                                                                </asp:ValidatorCalloutExtender>
                                                                <label>
                                                                    Lat:</label>
                                                                <input id="lat" runat="server" type="text" onfocus="this.blur();" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="lng"
                                                                    Display="None" ErrorMessage="Required" Enabled="False" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                                <asp:ValidatorCalloutExtender ID="RequiredFieldValidator36_ValidatorCalloutExtender"
                                                                    runat="server" Enabled="True" PopupPosition="Left" TargetControlID="RequiredFieldValidator36">
                                                                </asp:ValidatorCalloutExtender>
                                                                <label>
                                                                    Lng:</label>
                                                                <input id="lng" runat="server" type="text" onfocus="this.blur();" />
                                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="map" style="background-color: #E5E3DF; border: 1px solid; height: 150px; overflow: hidden; display: block;">
                                                </div>
                                            </div>
                                            <table id="NearWorkers" class="roundCorner" style="margin: 5px 0px 0px 0; border-radius: 5px; border: 1px solid #000; width: 100%">
                                            </table>
                                            <div id="wait" style="display: none">
                                                <div>
                                                    <img src="images/wheel.gif" />
                                                </div>
                                                <span>Loading
                                            Nearest Workers...</span>
                                            </div>
                                            <div style="margin-top:25px">
                                                <asp:ListView ID="lstRecentCalls" runat="server">
                                                    <LayoutTemplate>
                                                        <table border="0" cellpadding="1" width="100%">
                                                            <th><div class="roundCorner" style="text-align:center" >Recent Calls</div></th>
                                                            <tr>
                                                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <EmptyDataTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>No records available.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <div class="roundCorner" 
                                                                    style="<%# ( Convert.ToString(Eval("elev")) == hdnUnitID.Value  && hdnUnitID.Value!=""  && hdnUnitID.Value!="0") ?  "background-color:yellow" : "background-color:white" %>" >
                                                                   <strong> 
                                                                        <asp:HyperLink runat="server" ID="lblId" 
                                                                        NavigateUrl='<%# String.Format("addticket.aspx?id={0}&comp={1}", Eval("id"), Eval("comp")) %>'
                                                                        Text='<%# "Ticket#" + Eval("ID") %>' Target="_blank">
                                                                        </asp:HyperLink> 
                                                                   </strong>
                                                                       <asp:Label runat="server" ID="lblStatus"><%# RecentCallsDetails( Eval("assignname").ToString(), Eval("worker").ToString(), Eval("cat").ToString(),Eval("elevname").ToString() )%></asp:Label>
                                                                    <br />
                                                                   <i><asp:Label runat="server" ID="lblDate"><%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %></asp:Label></i> 
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                            <div class="signature">
                                                <b>Signature</b>
                                                <div id="signbg">
                                                    <asp:Image ID="imgSign" runat="server" />
                                                </div>
                                            </div>
                                            <div>
                                                <div id="sign" tabindex="0" style="display: none" class="sign_popup sigPad">
                                                    <div class="sign-title">
                                                        <a class="sign-title-l clearButton">Clear Signature</a>
                                                        <a id="convertpngbtn" class="sign-title-r">Accept</a>
                                                    </div>
                                                    <div class="sig">
                                                        <div class="typed">
                                                        </div>
                                                        <canvas class="pad" height="150" style="border: 1px solid black; position: relative; background-color: #fff;"
                                                            id="canvas"></canvas>
                                                        <input id="hdnDrawdata" tabindex="43" type="hidden" name="output" class="output" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                       

                                    </div>
                                     </ContentTemplate>
                                    </asp:UpdatePanel>
 
                                </ContentTemplate>
                            </asp:TabPanel>

                              
                            <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Custom/MCP">
                                <ContentTemplate>
                                    <div class="col-lg-6 col-md-6">
                                        <legend><b>Custom Fields</b></legend>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom1" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst1" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom2" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst2" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom3" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst3" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom4" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst4" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom5" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtCst5" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom6" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:CheckBox ID="chkCst1" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-col">
                                            <div class="fc-label">
                                                <asp:Label ID="lblCustom7" runat="server"></asp:Label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:CheckBox ID="chkCst2" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-lg-6 col-md-6">
                                        <legend><b>MCP</b> </legend>
                                        <div class="table-scrollable">
                                            <asp:GridView ID="gvRepDetails" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                Width="730px">
                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                <RowStyle CssClass="evenrowcolor" />
                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Equip" SortExpression="Equip">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEquip" runat="server" Text='<%# Bind("Equip") %>'></asp:Label>
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
                                                    <asp:TemplateField HeaderText="Freq" SortExpression="freq">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFreq" runat="server" Text='<%# Bind("freq") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Comments" SortExpression="comment">
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
                                                    <asp:TemplateField HeaderText="Next Due " SortExpression="NextDateDue">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNextDateDue" runat="server" Text='<%# Eval("NextDateDue", "{0:MM/dd/yyyy}")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>



                                    <div class="clearfix"></div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Documents">
                                <HeaderTemplate>
                                    Documents
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="table-scrollable" style="border: none">
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Panel ID="pnlDocumentButtons" runat="server" Style="background: #316b9d; width: 100%;">
                                                        <ul class="lnklist-header lnklist-panel">
                                                            <li>
                                                                <asp:LinkButton ID="lnkDeleteDoc" runat="server" CausesValidation="False" OnClick="lnkDeleteDoc_Click" ToolTip="Delete" CssClass="icon-delete"
                                                                    OnClientClick="return SelectedRowDelete('ctl00_ContentPlaceHolder1_TabContainer2_TabPanel3_gvDocuments','file');" ></asp:LinkButton></li>
                                                            <li>
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:FileUpload ID="FileUpload1" runat="server" onchange="ConfirmUpload(this.value);" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:LinkButton ID="lnkUploadDoc" runat="server"
                                                                                 CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                                Style="display: none">Upload</asp:LinkButton><asp:LinkButton ID="lnkPostback" runat="server"
                                                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </li>
                                                        </ul>
                                                    </asp:Panel>
                                                    <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        Width="650px">
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
                                                            <asp:TemplateField HeaderText="Path">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPath" runat="server" Text='<%# Eval("Path") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>

                    </div>
                    <asp:HiddenField runat="server" ID="hdnFocus" />
                    <div class="pnlUpdateoverlay" style="z-index: 900000; display: none;" id="loading"
                        align="center">
                        <img id="Image1" alt="Loading..." src="images/loader_round.gif" style="position: absolute; left: 50%; top: 50%; margin-left: -32px; margin-top: -32px; display: block;" />
                    </div>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
         
    <%--<script language="javascript" type="text/javascript">
        Sys.Application.add_load
(
function () {
    window.setTimeout(focus, 1);
}
)
        function focus() {
            document.getElementById('<%=hdnFocus.Value %>').focus();
        }
    </script>--%>

</asp:Content>