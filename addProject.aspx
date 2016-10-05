<%@ Page Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="addProject.aspx.cs" Inherits="addProject" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_AccountSearch.ascx" TagName="uc_AccountSearch" TagPrefix="uc1" %>
<%@ Register Src="uc_Datepicker.ascx" TagName="uc_Datepicker" TagPrefix="ucd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="js/jquery-ui-1.9.2.custom.js"></script>
    <link href="css/jquery-ui-1.9.2.custom.css" rel="stylesheet" />

    <script type="text/javascript">
        function divExpandCollapse(divname) {
            //debugger;
            var div = document.getElementById(divname);
            var img = document.getElementById('img' + divname);

            if (div.style.display == "none") {
                div.style.display = "inline";
                img.src = "images/icons/minus.gif";
            } else {
                div.style.display = "none";
                img.src = "images/icons/plus.gif";
            }
        }
        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function WarningTemplate() {
            noty({
                text: 'The existing BOM and Milestone are in use. BOM and Milestone items can not be changed.',
                type: 'warning',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: false,
                theme: 'noty_theme_default',
                closable: true
            });
        }
    </script>
    <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }
    </style>
    <script type="text/javascript">

        ///////////// Custom validator function for customer auto search  ////////////////////
        function ChkCustomer(sender, args) {
            var hdnCustID = document.getElementById('<%=hdnCustID.ClientID%>');
            if (hdnCustID.value == '') {
                args.IsValid = false;
            }
        }
        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }
        ///////////// Custom validator function for location auto search  ////////////////////
        function ChkLocation(sender, args) {
            var hdnLocId = document.getElementById('<%=hdnLocID.ClientID%>');
            if (hdnLocId.value == '') {
                args.IsValid = false;
            }
        }
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
                return o.value.dtxtScrapFactor = 0;
            }
        }
        function calBudgetExt(obj) {
            //debugger;
            var valQtyReq = 0;
            var valBudgetExt = 0;
            var valScrapFact = 0;
            var txtScrapFactor = 0;
            var objId = document.getElementById(obj.id);

            var isQtyReq = obj.id.search("txtQtyReq");

            if (isQtyReq == -1) {
                //on txtBudgetUnit textbox change   
                var txtBudgetUnit = document.getElementById(obj.id).value;
                if (isInt(txtBudgetUnit) == true) {
                    document.getElementById(obj.id).value = parseFloat(txtBudgetUnit).toFixed(2);
                    txtBudgetUnit = document.getElementById(obj.id).value;
                }

                var txtQtyReq = document.getElementById(obj.id.replace('txtBudgetUnit', 'txtQtyReq')).value;
                var ddlBType = document.getElementById(obj.id.replace('txtBudgetUnit', 'ddlBType')).value;
                var lblBudgetExt = document.getElementById(obj.id.replace('txtBudgetUnit', 'lblBudgetExt'));
                var hdnBudgetExt = document.getElementById(obj.id.replace('txtBudgetUnit', 'hdnBudgetExt'));
            }
            else {
                //on txtQtyReq textbox change
                var txtQtyReq = document.getElementById(obj.id).value;
                if (isInt(txtQtyReq) == true) {
                    document.getElementById(obj.id).value = parseFloat(txtQtyReq).toFixed(2);
                    txtQtyReq = document.getElementById(obj.id).value;
                }

                var txtBudgetUnit = document.getElementById(obj.id.replace('txtQtyReq', 'txtBudgetUnit')).value;
                var ddlBType = document.getElementById(obj.id.replace('txtQtyReq', 'ddlBType')).value;
                var lblBudgetExt = document.getElementById(obj.id.replace('txtQtyReq', 'lblBudgetExt'));
                var hdnBudgetExt = document.getElementById(obj.id.replace('txtQtyReq', 'hdnBudgetExt'));
            }

            valBudgetExt = txtQtyReq * txtBudgetUnit;
            if (isInt(valBudgetExt) == true) {
                valBudgetExt = valBudgetExt.toFixed(2);
                $(lblBudgetExt).text(valBudgetExt)
                $(hdnBudgetExt).val(valBudgetExt)
            }

        }
        function pageLoad(sender, args) {
            $(function () {

                $("[id*=txtCode]").autocomplete({

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobCode",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {

                                response($.parseJSON(data.d));

                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load job code");
                            }
                        });
                    },
                    select: function (event, ui) {

                        if (ui.item.value == 'AddNew') {
                            //redirect to another page 
                            //instead open popup to add code detail.
                            Showpopup();
                        }
                        if (ui.item.value == 0) {

                        }
                        else {

                            var txtCode = this.id;
                            var hdnCode = document.getElementById(txtCode.replace('txtCode', 'hdnCode'));
                            $(hdnCode).val(ui.item.value);
                            $(this).val(ui.item.label);
                        }

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;
                        //var result_desc = item.acct;
                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });

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
                });

                $("[id*=txtUM]").autocomplete({
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;
                        var str = request.term;

                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetUnitMeasure",
                            data: JSON.stringify(dtaaa),
                            dataType: "json",
                            async: true,
                            success: function (data) {

                                response($.parseJSON(data.d));

                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load unit measure");
                            }
                        });
                    },
                    select: function (event, ui) {
                        if (ui.item.value == 0) {
                        }
                        else {
                            var txtUM = this.id;
                            var hdnUMID = document.getElementById(txtUM.replace('txtUM', 'hdnUMID'));
                            $(hdnUMID).val(ui.item.value);
                            $(this).val(ui.item.label);
                        }
                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("autocomplete")._renderItem = function (ul, item) {

                        var ula = ul;
                        var itema = item;
                        var result_value = item.value;
                        var result_item = item.label;

                        var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        result_item = result_item.replace(x, function (FullMatch, n) {
                            return '<span class="highlight">' + FullMatch + '</span>'
                        });

                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);

                    };
                });
                $("[id*=txtUM]").change(function () {

                    var txtUM = $(this);
                    var strUM = $(this).val();

                    var txtUM1 = $(txtUM).attr('id');
                    var hdnUMID = document.getElementById(txtUM1.replace('txtUM', 'hdnUMID'));

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAutofillByUM",
                        data: '{"prefixText": "' + strUM + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {
                                var strUM = $(txtUM).val();
                                $(txtUM).val('');
                                noty({
                                    text: 'UM \'' + strUM + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: false,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                $(txtUM1).val(ui[0].label);
                                $(hdnUMID).val(ui[0].value);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load UM");
                        }
                    });
                });

            })
            var query = "";
            function u_dta() {
                this.prefixText = null;
                this.con = "";
            }
            $("[id*=txtUserID]").autocomplete({

                source: function (request, response) {
                    var dta = new u_dta();
                    dta.prefixText = request.term;
                    query = request.term;

                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetUsername",
                        data: JSON.stringify(dta),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load username");
                        }
                    });
                },
                select: function (event, ui) {

                    var txtUserID = this.id;
                    var hdnUserID = document.getElementById(txtUserID.replace('txtUserID', 'hdnUserID'));
                    var txtFirstName = document.getElementById(txtUserID.replace('txtUserID', 'txtFirstName'));
                    var txtLastName = document.getElementById(txtUserID.replace('txtUserID', 'txtLastName'));
                    var txtEmail = document.getElementById(txtUserID.replace('txtUserID', 'txtEmail'));
                    var txtMobile = document.getElementById(txtUserID.replace('txtUserID', 'txtMobile'));
                    $(this.id).val(ui.item.label);
                    $(hdnUserID).val(ui.item.value);
                    $(txtFirstName).val(ui.item.fFirst);
                    $(txtLastName).val(ui.item.fLast);
                    $(txtEmail).val(ui.item.Email);
                    $(txtMobile).val(ui.item.Cellular);

                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".usearchinput"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.label;

                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + result_item + "</a>")
                    .appendTo(ul);
                };
            });
            $("[id*=txtSType]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetServiceType",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load service type");
                        }
                    });
                },
                select: function (event, ui) {
                    if (ui.item.value == 'AddNew') {
                        //redirect to another page 
                        //instead open popup to add code detail.
                        Showpopup();
                    }
                    if (ui.item.value == 0) {

                    }
                    else {
                        var txtSType = this.id;
                        var hdnType = document.getElementById(txtSType.replace('txtSType', 'hdnType'));
                        $(hdnType).val(ui.item.value);
                        $(this).val(ui.item.label);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            $.each($(".searchinput"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {
                    var result_value = item.value;
                    var result_item = item.label;
                    //var result_desc = item.acct;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

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
            });
            ///////////// Ajax call for customer auto search ////////////////////                
            var query = "";
            function dta() {
                this.prefixText = null;
                this.con = "";
            }

            var queryloc = "";
            $("#ctl00_ContentPlaceHolder1_txtCustomer").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dta();
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
                        //error: function(result) {
                        //    alert("Due to unexpected errors we were unable to load customers");
                        //}
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            var err = eval("(" + XMLHttpRequest.responseText + ")");
                            alert(err.Message);
                        }
                    });
                },
                select: function (event, ui) {
                    //debugger;
                    $("#ctl00_ContentPlaceHolder1_txtCustomer").val(ui.item.label);
                    $("#ctl00_ContentPlaceHolder1_hdnCustID").val(ui.item.value);
                    $("#ctl00_ContentPlaceHolder1_txtLocation").focus();
                    $("#ctl00_ContentPlaceHolder1_txtLocation").val('');
                    $("#ctl00_ContentPlaceHolder1_hdnLocID").val('');

                    if (ui.item.prospect == 1) {
                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();
                    }
                    else {
                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectCustomer').click();
                    }
                    return false;
                },
                focus: function (event, ui) {

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

            function l_dta() {
                this.prefixText = "";
                this.con = "";
                this.custID = null;
            }
            $("#ctl00_ContentPlaceHolder1_txtLocation").autocomplete({

                source: function (request, response) {
                    var dta = new l_dta();
                    dta.custID = 0;

                    var custID = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustID').value;
                    if (!isNaN(parseInt(custID))) {
                        dta.custID = parseInt(custID);
                    }
                    dta.prefixText = request.term;
                    query = request.term;

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "CustomerAuto.asmx/GetLocation",
                        data: JSON.stringify(dta),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load locations");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                    $("#ctl00_ContentPlaceHolder1_hdnLocID").val(ui.item.value);
                        <%-- $("#<%=hdnCustID.ClientID%>").val('');--%>
                        document.getElementById('ctl00_ContentPlaceHolder1_btnSelectLoc').click();

                        return false;
                    },
                    focus: function (event, ui) {
                        $("#ctl00_ContentPlaceHolder1_txtLocation").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
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
                $("#ctl00_ContentPlaceHolder1_txtCustomer").keyup(function (event) {
                    var hdnCustID = document.getElementById('ctl00_ContentPlaceHolder1_hdnCustID');
                    if (document.getElementById('ctl00_ContentPlaceHolder1_txtCustomer').value == '') {
                        hdnCustID.value = '';
                    }
                });

                $("#ctl00_ContentPlaceHolder1_txtLocation").keyup(function (event) {
                    var hdnLocId = document.getElementById('ctl00_ContentPlaceHolder1_hdnLocID');
                    if (document.getElementById('ctl00_ContentPlaceHolder1_txtLocation').value == '') {
                        hdnLocId.value = '';
                    }
                });
                var query = "";
                function dtaa() {
                    this.prefixText = null;
                    this.con = null;
                    this.custID = null;
                }

                $("#<%=txtPrevilWage.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetWage",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load wage");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtPrevilWage.ClientID%>").val(ui.item.label);
                        $("#<%=hdnPrevilWageID.ClientID%>").val(ui.item.value);
                        return false;
                    },
                    focus: function (event, ui) {
                        $("#<%=txtPrevilWage.ClientID%>").val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).bind('click', function () { $(this).autocomplete("search"); })
                .data("autocomplete")._renderItem = function (ul, item) {
                    //debugger;
                    var ula = ul;
                    var itema = item;
                    var result_value = item.value;
                    var result_item = item.label;
                    //var result_desc = item.acct;
                    var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                    result_item = result_item.replace(x, function (FullMatch, n) {
                        return '<span class="highlight">' + FullMatch + '</span>'
                    });

                    return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                };

            ///////////////////////////////////// Inventroy service ////////////////////////////////////
                    $("#<%=txtInvService.ClientID%>").autocomplete({
                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetInvService",
                        data: JSON.stringify(dtaaa),
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load services");
                        }
                    });
                },
                select: function (event, ui) {
                    $("#<%=txtInvService.ClientID%>").val(ui.item.label);
                    $("#<%=hdnInvServiceID.ClientID%>").val(ui.item.value);
                    return false;
                },
                focus: function (event, ui) {
                    $("#<%=txtInvService.ClientID%>").val(ui.item.label);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).bind('click', function () { $(this).autocomplete("search"); })
            .data("autocomplete")._renderItem = function (ul, item) {
                //debugger;
                var ula = ul;
                //var itema = item;
                var result_value = item.value;
                var result_item = item.label;
                //var result_desc = item.acct;
                var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                result_item = result_item.replace(x, function (FullMatch, n) {
                    return '<span class="highlight">' + FullMatch + '</span>'
                });

                return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + result_item + "</a>")
                    .appendTo(ul);
            };
                $(".custom").change(function () {
                    getCustomVal();
                });
                $(".currency").change(function () {
                    showDecimalVal(this);
                })
                $(".currency").keypress(function (e) {
                    return isDecimalKey(this, e.target)
                })
                $(".date-picker").click(function () {
                    $(this).datepicker();
                })
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
       
        $(document).ready(function () {

            InitializeGrids('<%=gvBOM.ClientID%>');

        });
        //function CalculatePercentage(gridview) {
        //}
        function isNumberKey(evt, txt) {

            var charCode = (evt.which) ? evt.which : event.keyCode

            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        (function ($) {
            $.extend({
                toDictionary: function (query) {
                    var parms = {};
                    var items = query.split("&");
                    for (var i = 0; i < items.length; i++) {
                        var values = items[i].split("=");
                        var key1 = decodeURIComponent(values.shift().replace(/\+/g, '%20'));
                        var key = key1.split('$')[key1.split('$').length - 1];
                        var value = values.join("=")
                        parms[key] = decodeURIComponent(value.replace(/\+/g, '%20'));
                    }
                    return (parms);
                }
            })
        })(jQuery);
        (function ($) {
            $.fn.serializeFormJSON = function () {
                var o = [];
                $(this).find('tr:not(:first, :last)').each(function () {
                    var elements = $(this).find('input, textarea, select')
                    if (elements.size() > 0) {
                        var serialized = $(this).find('input, textarea, select').serialize();
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                });
                return o;
            };
        })(jQuery);
        (function ($) {
            $.fn.serializeCustomJSON = function () {
                var o = [];
                $(".custom").each(function () {
                    if ($(this).length) {
                        var serialized = $(this).serialize();

                        if (serialized == '') {

                            var serialized = $(this).find('input, textarea, select').serialize();

                        }
                        var item = $.toDictionary(serialized);
                        o.push(item);
                    }
                    //}
                });
                return o;
            };
        })(jQuery);
        function InitializeGrids(Gridview) {
            var custom = $(".custom");
            $("input", custom).each(function () {
                $(this).blur();
            });

            $("#" + Gridview).on('click', 'a.addButton', function () {
                var $tr = $(this).closest('table').find('tr').eq(1);
                var $clone = $tr.clone();
                $clone.find('input:text').val('');
                $clone.insertAfter($tr.closest('table').find('tr').eq($tr.closest('table').find('tr').length - 2));
            });

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }

        function DelRow(Gridview) {
            if ($("#" + Gridview).find('input[type="checkbox"]:checked').length == 0) {
                alert('Please select items to delete.');
                return;
            }
            var con = confirm('Are you sure you want to delete the items?');
            if (con == true) {
                $("#" + Gridview).find('tr').each(function () {
                    var $tr = $(this);
                    $tr.find('input[type="checkbox"]:checked').each(function () {
                        if ($("#" + Gridview).find('tr').length > 3) {
                            $(this).closest('tr').remove();
                        }
                        else {
                            $(this).closest('tr').find('input:text').val('');
                        }
                    });
                });
            }
        }
        function removeLine(Gridview) {
            $("#" + Gridview).find('tr').each(function () {
                var $tr = $(this);
                $tr.find('input[type="checkbox"]:checked').each(function () {
                    if ($("#" + Gridview).find('tr').length > 3) {
                        $(this).closest('tr').remove();
                    }
                    else {
                        $(this).closest('tr').find('input:text').val('');
                    }
                });
            });
        }
        function NumericValid(e) {

            //         $("#txtboxToFilter").keydown(function (e) {
            // Allow: backspace, delete, tab, escape, enter and .
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
                // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
            //            });
        }

        function itemJSON() {

            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            var rawDataTeam = $('#<%=gvTeamItems.ClientID%>').serializeFormJSON();
            var formDataTeam = JSON.stringify(rawDataTeam);

            $('#<%=hdnItemTeamJSON.ClientID%>').val(formDataTeam);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);

            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

            var rawCustomData = $(".custom").serializeCustomJSON();

            var formCustomData = JSON.stringify(rawCustomData);
            $('#<%=hdnCustomJSON.ClientID%>').val(formCustomData);

        }
        function getCustomVal() {
            var rawCustomData = $(".custom").serializeCustomJSON();
            var formCustomData = JSON.stringify(rawCustomData);
            $('#<%=hdnCustomJSON.ClientID%>').val(formCustomData);
        }

        function Calculate(Gridview) {

            var tquan = 0;
            var tunit = 0;
            var ttotal = 0;

            $("#" + Gridview).find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var quan = $tr.find('input[id*=txtActual]').val();
                var unit = $tr.find('input[id*=txtBudget]').val();
                var total = 0;

                if (!isNaN(parseFloat(quan))) {
                    tquan += parseFloat(quan);
                }
                if (!isNaN(parseFloat(unit))) {
                    tunit += parseFloat(unit);
                }
                if (!isNaN(parseFloat(quan))) {
                    if (!isNaN(parseFloat(unit))) {
                        total = ((parseFloat(unit) - parseFloat(quan)) / parseFloat(quan)) * 100;
                        ttotal += parseFloat(total);
                    }
                }
                $tr.find('input[id*=txtPercent]').val(total.toFixed(2));
            });

            var $footer = $("#" + Gridview).find('tr').eq($("#" + Gridview).find('tr').length - 1)
            $footer.find('input[id*=txtTPercent]').val(ttotal.toFixed(2));
            $footer.find('input[id*=txtTActual]').val(tquan.toFixed(2));
            $footer.find('input[id*=txtTBudget]').val(tunit.toFixed(2));
        }
        function CheckDelete(Gridview) {

            var gv;
            if (Gridview.includes('gvBOM')) {
                gv = $("#<%= gvBOM.ClientID%>")
            }
            else {
                gv = $("#<%= gvMilestones.ClientID%>")
            }
            var len = gv.find('tr').find('input[type="checkbox"]:checked').length;
            if (len > 1) {
                noty({
                    text: 'Please select any one items to delete.',
                    dismissQueue: true,
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: true,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: false
                });
                return false;
            }
            if (gv.find('input[type="checkbox"]:checked').length == 0) {

                noty({
                    text: 'Please select items to delete.',
                    dismissQueue: true,
                    type: 'warning',
                    layout: 'topCenter',
                    closeOnSelfClick: true,
                    timeout: 5000,
                    theme: 'noty_theme_default',
                    closable: false
                });
                return false;
            }
            else if (gv.find('input[type="checkbox"]:checked').length > 0) {

                return confirm('Do you really want to delete this job item ?');
            }
        }
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
        .shadow {
            /* rgba(0, 0, 0, 0.3) rgb(90, 168, 208)*/
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .shadowHover:hover {
            -moz-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            -webkit-box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
            box-shadow: 0 1px 5px rgba(0, 0, 0, 0.3);
        }

        .hoverGrid {
            display: none;
            position: absolute;
            /* background: #fff;
            border: 1px solid #cccccc;
            color: #6c6c6c;   */
            min-width: 300px;
            max-width: 800px;
            min-height: 20px;
            /*font-weight: bold;*/
            font-size: 14px;
            padding: 5px 5px 5px 5px;
            background: black;
            color: #FFF;
        }

        .transparent {
            zoom: 1;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }

        .roundCorner {
            border: 1px solid #ccc;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
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
                    <span>Project Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/Project.aspx") %>">Project</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Add Project</span>
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Project</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblProjectNo" Text="-" runat="server"></asp:Label></li>

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
                            <asp:LinkButton ID="lnkSaveTemplate" runat="server" CssClass="icon-save" style="margin-left:20px;" ToolTip="Save"
                                OnClick="lnkSaveTemplate_Click"
                                OnClientClick="itemJSON();"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" ToolTip="Close" CssClass="icon-closed"
                                OnClientClick="window.close();"></asp:LinkButton>

                        </li>
                        <li>
                        </li>
                        <li style="text-align:right;">  <asp:Label CssClass="title_text_Name" ID="lblNetAmount" Text="" runat="server" Visible="false" style="font-size:14px;text-align:right;width:100%"></asp:Label> </li>
                    </ul>
                 
                </div>
            </div>
            <asp:HiddenField ID="hdnMilestone" runat="server" />
            <asp:HiddenField ID="hdnCustomJSON" runat="server" />
            
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
             <div class="com-cont">
                  <div class="col-lg-12 col-md-12">
                    <div class="col-lg-12 col-md-12">
                         <div class="col-md-6 col-lg-6">
                                <div id="trEstimate" class="form-col" runat="server" visible="false">
                                    <div class="fc-label">Estimate</div>
                                    <div class="fc-input">
                                        <asp:HyperLink ID="lnkEstimate" runat="server" Target="_blank"></asp:HyperLink>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Project Name
                                            <asp:RequiredFieldValidator ID="rfvProjectName" runat="server"
                                                ControlToValidate="txtREPdesc" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                            </asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender
                                                ID="vceProjectName" runat="server" Enabled="True"
                                                TargetControlID="rfvProjectName">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtREPdesc" runat="server" AutoCompleteType="None" CssClass="form-control" autocomplete="off"
                                                MaxLength="75"></asp:TextBox>
                                        </div>
                                    </div>
                              </div>
                            <asp:UpdatePanel ID="updPnlAddress" runat="server">
                             <ContentTemplate>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Customer
                                            <asp:CustomValidator ID="cvCustomer" runat="server" ClientValidationFunction="ChkCustomer"
                                                ControlToValidate="txtCustomer" Display="None" ErrorMessage="Please select the customer"
                                                SetFocusOnError="True" Enabled="False"></asp:CustomValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCustomer1" runat="server" Enabled="True"
                                                TargetControlID="cvCustomer">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:RequiredFieldValidator ID="rfvCustomer" runat="server" ControlToValidate="txtCustomer"
                                                Display="None" ErrorMessage="Please select the customer" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceCustomer"
                                                runat="server" Enabled="True" TargetControlID="rfvCustomer">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCustomer" CssClass="form-control" runat="server"
                                                 placeholder="Search by customer name, phone#, address etc." autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdnCustID" runat="server" />
                                            <asp:FilteredTextBoxExtender ID="txtCustomer_FilteredTextBoxExtender" runat="server"
                                                Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtCustomer">
                                            </asp:FilteredTextBoxExtender>
                                            <asp:Button ID="btnSelectCustomer" runat="server" CausesValidation="False" OnClick="btnSelectCustomer_Click"
                                                 Style="display: none;" Text="Button" />
                                        </div>
                                    </div>
                                </div> 
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                            Location
                                            <asp:RequiredFieldValidator ID="rfvLocation" runat="server"
                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Location Name Required"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceLocation" 
                                                runat="server" Enabled="True" TargetControlID="rfvLocation">
                                            </asp:ValidatorCalloutExtender>
                                            <asp:CustomValidator ID="cvLocation" runat="server" ClientValidationFunction="ChkLocation"
                                                ControlToValidate="txtLocation" Display="None" ErrorMessage="Please select the location"
                                                SetFocusOnError="True"></asp:CustomValidator>
                                            <asp:ValidatorCalloutExtender ID="vceLocation1" runat="server" Enabled="True"
                                                TargetControlID="cvLocation">
                                            </asp:ValidatorCalloutExtender>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtLocation" CssClass="form-control" runat="server" 
                                                placeholder="Search by location name, phone#, address etc." autocomplete="off"></asp:TextBox>
                                        </div>
                                        <asp:FilteredTextBoxExtender ID="txtLocation_FilteredTextBoxExtender" runat="server"
                                            Enabled="False" FilterMode="InvalidChars" InvalidChars="'\" TargetControlID="txtLocation">
                                        </asp:FilteredTextBoxExtender>
                                        <asp:HiddenField ID="hdnLocID" runat="server" />
                                        <asp:Button ID="btnSelectLoc" runat="server" CausesValidation="False" OnClick="btnSelectLoc_Click"
                                            Style="display: none;" Text="Button" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="form-col">
                                        <div class="fc-label">
                                           Address
                                        </div>
                                        <div class="fc-input">
                                            <asp:Textbox ID="txtAddress" Height="50px" TextMode="MultiLine" CssClass="form-control" runat="server">
                                            </asp:Textbox>
                                        </div>
                                    </div>
                                </div>
                             </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                    
                                </Triggers>
                             </asp:UpdatePanel>
                                <div class="clearfix"></div>
                            </div>
                         <div class="col-md-4 col-lg-4">
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Template Type
                                        <asp:RequiredFieldValidator ID="rfvTemplateType" runat="server" ControlToValidate="ddlTemplate"
                                             Display="None" ErrorMessage="Please select template type" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceTemplateType" runat="server" Enabled="True" TargetControlID="rfvTemplateType" PopupPosition="BottomLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlTemplate" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged"
                                            AutoPostBack="true" onchange="itemJSON();"></asp:DropDownList>

                                    </ContentTemplate>
                                </asp:UpdatePanel>                                                                 
                                    <%--      <uc2:uc_TemplateType ID="Uc_TemplateType" runat="server" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Status
                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" InitialValue="Select Status" ControlToValidate="ddlJobStatus"
                                             Display="None" ErrorMessage="Please select Status" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceStatus" runat="server" Enabled="True" TargetControlID="rfvStatus" PopupPosition="BottomLeft">
                                        </asp:ValidatorCalloutExtender>
                                    </div>
                                    <div class="fc-input">
                                    <asp:DropDownList ID="ddlJobStatus" CssClass="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Department
                                    </div>
                                    <div class="fc-input">
                                       <asp:DropDownList ID="ddlJobType" CssClass="form-control" runat="server" 
                                           OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>     
                             <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        Remarks
                                    </div>
                                    <div class="fc-input">
                                            <asp:TextBox ID="txtREPremarks" runat="server" CssClass="form-control"
                                            Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                         <div class="col-md-2 col-lg-2">
                             <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblRev" runat="server" text="Revenue " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblRevenue" runat="server" text="$0.00"></asp:Label>
                                    </div>
                                </div>
                             </div>
                             <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblLab" runat="server" text="Labor Expense " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblLabor" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                             </div>
                             <div class="form-group">
                                <div class="form-col">
                                   <div class="fc-label">
                                       <asp:Label ID="lblMat" runat="server" text="Material Expense " Font-Bold="true"></asp:Label> 
                                   </div>
                                   <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblMaterialExp" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblExp" runat="server" text="Other Expense " Font-Bold="true"></asp:Label> 
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblExpense" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblTotalExp" runat="server" text="Total Expense " Font-Bold="true"></asp:Label> 
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblTotalExpense" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="form-col"> 
                                    <div class="fc-label">
                                        <asp:Label ID="lblProfit" runat="server" text="Profit Amount " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblProfitAmt" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                             <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblOrdered" runat="server" text="Total On Order " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblTotalOrder" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                             </div>
                             <div class="form-group">
                                <div class="form-col"> 
                                    <div class="fc-label">
                                        <asp:Label ID="lblPercent" runat="server" text="% in Profit " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblPercentProfit" runat="server" Text="$0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label">
                                        <asp:Label ID="lblActual" runat="server" text="Actual Hours " Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblActualHr" runat="server" Text="0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                         </div>
                    </div>
                    <div class="col-lg-12 col-md-12">
                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                       <asp:PlaceHolder ID="PlaceHolderHeader" runat="server">

                       </asp:PlaceHolder>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                     </div>
                    <div class="clearfix"></div>
                    <div class="col-lg-12 col-md-12">
                      <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0">
                         <asp:TabPanel ID="tbpnlAttri" runat="server" HeaderText="Project Attributes">
                            <HeaderTemplate>
                                Attributes
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:Panel ID="Panel6" runat="server">
                                        <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" CssClass="">
                                            <asp:TabPanel ID="tbpnlGeneral" runat="server" HeaderText="Project Attributes">
                                                <HeaderTemplate>
                                                    General
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-lg-12">
                                                    <div class="col-md-6 col-lg-6">
                                                     
                                                        <%--<div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Contract Type
                                                                </div>
                                                                <div class="fc-input">
                                                                     <asp:DropDownList ID="ddlContractType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>--%>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Project Creation Date
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtProjCreationDate" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="txtProjCreationDate_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtProjCreationDate">
                                                                    </asp:CalendarExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    PO #
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtPO" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Sales Order #
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtSalesOrder" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                      <div class="col-md-6 col-lg-6" style="padding-left: 0px;">
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Attach PO 
                                                                </div>
                                                                <div class="fc-input" style="padding-top:5px;">
                                                                    <asp:FileUpload ID="fuAttachPO" runat="server" />
                                                                    <asp:LinkButton ID="lnkUploadDoc" runat="server" CausesValidation="False" OnClick="lnkUploadDoc_Click"
                                                                                    Style="display: none">Upload</asp:LinkButton>
                                                                       <asp:LinkButton ID="lnkPostback" runat="server"
                                                                                    CausesValidation="False" Style="display: none">Postback</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-lg-6">
                                                        <div class="form-group">
                                                            <div class="form-col" style="text-align:right;">
                                                                 <asp:CheckBox ID="chkCertifiedJob" Text="Certified Job" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                  
                                                    <div class="col-md-6 col-lg-6"> 
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Custom 1
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustom1" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Custom 2
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustom2" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Custom 3
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustom3" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Custom 4
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustom4" runat="server" CssClass="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Custom 5
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:TextBox ID="txtCustom5" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    <asp:CalendarExtender ID="txtCustom5_CalendarExtender" runat="server" Enabled="True"
                                                                        TargetControlID="txtCustom5">
                                                                    </asp:CalendarExtender>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    </div>
                                                    <div class="col-md-12 col-lg-12" style="padding-bottom:150px;">
                                                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                                                            <ContentTemplate>
                                                                <asp:PlaceHolder ID="PlaceHolderAttrGeneral" runat="server"></asp:PlaceHolder>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="tbpnlTeam" runat="server" HeaderText="Project Attributes">
                                                <HeaderTemplate>
                                                    Team
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-lg-12">
                                                       <div class="table-scrollable table-addjournal col-lg-12 col-md-12" style="border: none;">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                            <ContentTemplate>
                                                                <fieldset class="roundCorner col-lg-12 col-md-12" style="padding: 10px 10px 10px 10px;">
                                                                    <legend>ITEMS</legend>

                                                                    <asp:GridView ID="gvTeamItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                        PageSize="20" ShowFooter="true" 
                                                                        OnRowCommand="gvTeamItems_RowCommand">
                                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                        <FooterStyle CssClass="footer" />
                                                                        <RowStyle CssClass="evenrowcolor" />
                                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="4%">
                                                                                <HeaderTemplate>
                                                                                    <a id="Button2" class="delButton" onclick="DelRow('<%=gvTeamItems.ClientID%>');"
                                                                                        style="cursor: pointer;">
                                                                                        <img src="images/menu_delete.png" title="Delete" width="18px;" /></a>
                                                                                </HeaderTemplate>
                                                                            <%--<ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>--%>
                                                                                <FooterTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddTeam" CausesValidation="False"
                                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Line No." ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Title" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                        <asp:TextBox ID="txtTitle" CssClass="form-control input-sm input-small" Text='<%# Eval("Title") %>' runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="MOM User ID" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                        <asp:TextBox ID="txtUserID" CssClass="usearchinput form-control input-sm input-small" Text='<%# Eval("UserID") %>'
                                                                                            Placeholder="Search by User ID" runat="server"></asp:TextBox>
                                                                                        <asp:HiddenField ID="hdnUserID" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="First Name" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtFirstName"  CssClass="form-control input-sm input-small" Text='<%# Eval("FirstName") %>' runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Last Name" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtLastName" CssClass="form-control input-sm input-small" Text='<%# Eval("LastName") %>' runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Email" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtEmail" CssClass="form-control input-sm input-small" Text='<%# Eval("Email") %>' runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Mobile #" ItemStyle-Width="7%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtMobile" CssClass="form-control input-sm input-small" Text='<%# Eval("Mobile") %>' runat="server"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </fieldset>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="tbpnlGCInfo" runat="server" HeaderText="Project Attributes">
                                                <HeaderTemplate>
                                                    GC Information
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="col-md-6 col-lg-6">
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Name
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtName" CssClass="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        City
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtCity" CssClass="form-control" runat="server"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        State/Prov
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:DropDownList ID="ddlState" runat="server" CssClass="form-control"
                                                                            TabIndex="4">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        ZIP/Postal Code
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Country
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 col-lg-6">
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Contact Name
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtContactName" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Phone
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                                        <asp:MaskedEditExtender ID="txtPhone_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                            CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                            MaskType="Number" TargetControlID="txtPhone">
                                                                        </asp:MaskedEditExtender>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                             <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Fax
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtFax" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                                        <asp:MaskedEditExtender ID="txtFax_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                            CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                            MaskType="Number" TargetControlID="txtFax">
                                                                        </asp:MaskedEditExtender>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                              <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Email
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtEmailWeb" runat="server" CssClass="form-control"></asp:TextBox>
                                                                          <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                                                            ControlToValidate="txtEmailWeb" Display="None" ErrorMessage="Invalid Email" SetFocusOnError="True"
                                                                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                                         <asp:ValidatorCalloutExtender ID="vceEmail" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                                                            TargetControlID="revEmail">
                                                                         </asp:ValidatorCalloutExtender>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Cellular
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control" onkeypress="return isNumberKey(event,this)"></asp:TextBox>
                                                                        <asp:MaskedEditExtender ID="txtMobile_MaskedEditExtender" runat="server" AutoComplete="False" ClearMaskOnLostFocus="true"
                                                                            CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder=""
                                                                            CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder=""
                                                                            CultureTimePlaceholder="" Enabled="True" ErrorTooltipEnabled="True" Mask="(999)999-9999999999"
                                                                            MaskType="Number" TargetControlID="txtMobile">
                                                                        </asp:MaskedEditExtender>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-12 col-lg-12">
                                                            <div class="form-group">
                                                                <div class="form-col">
                                                                    <div class="fc-label">
                                                                        Remarks
                                                                    </div>
                                                                    <div class="fc-input">
                                                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                      </div>
                                                    <div class="col-md-12 col-lg-12">
                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                            <asp:PlaceHolder ID="PlaceHolderAttriGC" runat="server"></asp:PlaceHolder>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </div>
                                                 
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                            <asp:TabPanel ID="tbpnlEquipment" runat="server" HeaderText="Project Attributes">
                                                <HeaderTemplate>
                                                    Equipment
                                                </HeaderTemplate>
                                                <ContentTemplate>
                                                    <div class="col-md-12 col-lg-12">
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvEquip" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                            DataKeyNames="ID" Width="100%" AllowPaging="True" AllowSorting="True" PageSize="20"
                                                            ShowFooter="True" ShowHeaderWhenEmpty="True" EmptyDataText="No records to display">
                                                            <RowStyle CssClass="evenrowcolor" />
                                                            <FooterStyle CssClass="footer" />
                                                            <Columns>
                                                         <%--   <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>  --%>
                                                                <asp:TemplateField HeaderText="ID" SortExpression="id" Visible="false">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                     <FooterTemplate>
                                                                        <asp:Label ID="lblTotal" runat="server">Total</asp:Label>
                                                                    </FooterTemplate>
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
                                                                <asp:TemplateField HeaderText="Customer" SortExpression="name">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCust" runat="server"><%#Eval("name")%></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Location ID" SortExpression="locid">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocid" runat="server"><%#Eval("LocID")%></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Location" SortExpression="tag">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLocName" runat="server"><%#Eval("tag")%></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Address" SortExpression="Address">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAddress" runat="server"><%#Eval("Address")%></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPrice" runat="server"><%# DataBinder.Eval(Container.DataItem, "Price", "{0:c}")%></asp:Label>
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
                                                            <SelectedRowStyle CssClass="selectedrowcolor" />
                                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                                            <PagerTemplate>
                                                                <div align="center">
                                                                    <asp:ImageButton ID="ImageButton1" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                                                    &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev"
                                                                        ImageUrl="~/images/Backward.png" />
                                                                    &nbsp &nbsp <span>Page</span>
                                                                    <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" >
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
                                                     </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="btnSelectLoc" />
                                                        <asp:AsyncPostBackTrigger ControlID="btnSelectCustomer" />
                                    
                                                    </Triggers>
                                                    </asp:UpdatePanel>
                                                    </div>
                                                    <div class="col-md-12 col-lg-12">
                                                     <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                        <asp:PlaceHolder ID="PlaceHolderAttriEquip" runat="server"></asp:PlaceHolder>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:TabPanel>
                                        </asp:TabContainer>
                               </asp:Panel>
                            </ContentTemplate>
                         </asp:TabPanel>
                        <asp:TabPanel ID="tbpnlFinance" runat="server" HeaderText="Project Finance">
                            <HeaderTemplate>
                                Finance 
                            </HeaderTemplate>
                            <ContentTemplate>
                              <asp:Panel ID="Panel7" runat="server">
                                <asp:TabContainer ID="TabContainer3" runat="server" ActiveTabIndex="0">
                                    <asp:TabPanel ID="tbpnlGeneral1" runat="server" HeaderText="Project General">
                                        <HeaderTemplate>
                                            General
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                         <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                                          <ContentTemplate>
                                            <div class="col-lg-12 col-md-12">
                                                <div class="col-md-6 col-lg-6">
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Expense GL
                                                            </div>
                                                            <div class="fc-input">
                                                                    <uc1:uc_AccountSearch ID="uc_InvExpGL" UpdateMode="Conditional" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Interest GL
                                                            </div>
                                                            <div class="fc-input">
                                                                    <uc1:uc_AccountSearch ID="uc_InterestGL" UpdateMode="Conditional" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Revenue GL 
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtInvService" runat="server" AutoCompleteType="None" CssClass="form-control"
                                                                    MaxLength="255"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnInvServiceID" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Labor GL 
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:TextBox ID="txtPrevilWage" runat="server" AutoCompleteType="None" CssClass="form-control"
                                                                    MaxLength="255"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnPrevilWageID" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-lg-3">
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Posting Type   
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:DropDownList ID="ddlPostingMethod" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="form-col">
                                                            <div class="fc-label">
                                                                Service Type
                                                            </div>
                                                            <div class="fc-input">
                                                                <asp:DropDownList ID="ddlContractType1" runat="server" CssClass="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6 col-lg-6" style="padding-right: 0px;">
                                                        <div class="form-group">
                                                            <div class="form-col" style="text-align: left;">
                                                                <asp:CheckBox ID="chkChargeInt" Text="Charge Interest" runat="server" />
                                                            </div>
                                                        </div>
                                                            
                                                    </div>
                                                    <div class="col-md-6 col-lg-6" style="padding-right: 0px;">
                                                        <div class="form-group">
                                                            <div class="form-col" style="text-align: left; ">
                                                                <asp:CheckBox ID="chkChargeable" Text="Chargeable" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-12 col-lg-12">
                                                        <div class="form-group">
                                                            <div class="form-col" style="text-align: left;">
                                                                <asp:CheckBox ID="chkInvoicing" Text="Close after Invoicing" runat="server" />
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-lg-3">
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
                                            </div>
                                            <div class="col-md-12 col-lg-12">
                                                <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                        <asp:PlaceHolder ID="PlaceHolderFinceGeneral" runat="server"></asp:PlaceHolder>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                            </ContentTemplate>
                                               <Triggers>
                                                 <asp:AsyncPostBackTrigger ControlID="ddlTemplate" />
                                              </Triggers>
                                           </asp:UpdatePanel>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbpnlBudgets" runat="server" HeaderText="Project Budgets">
                                        <HeaderTemplate>
                                            Budgets
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="col-md-12 col-lg-12">
                                                <asp:Panel runat="server" ID="Panel8" Style="background: #316b9d">
                                                    <ul class="lnklist-header lnklist-panel">
                                                        <li>
                                                            <asp:Label CssClass="title_text_Name" ID="lblNetAmountVal" Text="" runat="server" Visible="false" style="font-size:14px;"></asp:Label>
                                                        </li>
                                                    </ul>
                                                </asp:Panel>
                                                <asp:GridView ID="gvBudget" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        DataKeyNames="Type" AllowPaging="True" PageSize="5" Width="100%" OnRowDataBound="gvBudget_RowDataBound"
                                                        ShowFooter="True" EmptyDataText="No Budget Found..." ShowHeaderWhenEmpty="True">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-BackColor="White">
                                                                <ItemTemplate>
                                                                    <a href="JavaScript:divExpandCollapse('div<%# Eval("Type") %>');">
                                                                    <img id="imgdiv<%# Eval("Type") %>" width="9px" border="0" src="images\icons\plus.gif" />
                                                                    </a>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Op Seq" ItemStyle-BackColor="White" ItemStyle-Width="10.5%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblOpSeq" runat="server" Text='All'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" ItemStyle-BackColor="White" ItemStyle-Width="10%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeName") == "" ? " - " :Eval("TypeName") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnType" Value='<%# Bind("Type") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item" ItemStyle-BackColor="White" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblItem" runat="server" Text='All'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                           <asp:TemplateField HeaderText="Budgeted" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="White" ItemStyle-Width="7.5%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblBudgeted" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Budget"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Budget", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Actual" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="White" ItemStyle-Width="8%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblActual" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Actual"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Actual", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Variance" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="White" ItemStyle-Width="8%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblVariance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Variance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Variance", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Commited" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="White" ItemStyle-Width="8%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblCommited" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Commited"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Commited", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Outstanding" ItemStyle-HorizontalAlign="Right" ItemStyle-BackColor="White" ItemStyle-Width="8%">
                                                                <ItemTemplate>
                                                                        <asp:Label ID="lblOutstanding" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Outstanding"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0:c}")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-BackColor="White">
                                                                <ItemTemplate>
                                                                    <tr style="background-color:white">
                                                                        <td colspan="100%">
                                                                            <div id="div<%# Eval("Type") %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                                                <asp:GridView ID="gvChildGrid" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found"
                                                                                CssClass="table table-bordered table-striped table-condensed flip-content" OnRowDataBound="gvChildGrid_RowDataBound"
                                                                                GridLines="None" Width="100%" ShowHeaderWhenEmpty="True">
                                                                                <RowStyle CssClass="evenrowcolor" />
                                                                                <FooterStyle CssClass="footer" />
                                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                                <Columns>
                                                                                     <asp:TemplateField ItemStyle-Width="20px">
                                                                                        <ItemTemplate>
                                                                                            <a href="JavaScript:divExpandCollapse('divChild<%# Eval("Code").ToString()+Eval("Type").ToString() %>');">
                                                                                            <img id="imgdivChild<%# Eval("Code").ToString()+Eval("Type").ToString() %>" width="9px" border="0" src="images\icons\plus.gif" />
                                                                                            </a>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Op Seq" ItemStyle-Width="9%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblOpSeq" runat="server" Text='<%# Eval("Code") == "" ? " - " : Eval("Code") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Type" SortExpression="Type" ItemStyle-Width="10%">
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeName") == "" ? " - " : Eval("TypeName") %>'></asp:Label>
                                                                                            <asp:HiddenField ID="hdnType" Value='<%# Bind("Type") %>' runat="server" />
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item")  == "" ? " - " : Eval("Item") %>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Budgeted" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblBudgeted" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Budget"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Budget", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Actual" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblActual" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Actual"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Actual", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Variance" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblVariance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Variance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Variance", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField HeaderText="Commited" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblCommited" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Commited"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Commited", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                     <asp:TemplateField HeaderText="Outstanding" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                        <ItemTemplate>
                                                                                                <asp:Label ID="lblOutstanding" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Outstanding"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0:c}")%>'></asp:Label>
                                                                                        </ItemTemplate>
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <ItemTemplate>
                                                                                            <tr style="background-color:white">
                                                                                                <td colspan="100%">
                                                                                                    <div id="divChild<%# Eval("Code").ToString()+Eval("Type").ToString() %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                                                                        <asp:GridView ID="gvChildItemGrid" runat="server" AutoGenerateColumns="false" EmptyDataText="No Data Found"
                                                                                                        CssClass="table table-bordered table-striped table-condensed flip-content" OnRowDataBound="gvChildItemGrid_RowDataBound"
                                                                                                        GridLines="None" Width="100%" ShowHeaderWhenEmpty="True">
                                                                                                        <RowStyle CssClass="evenrowcolor" />
                                                                                                        <FooterStyle CssClass="footer" />
                                                                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                                                        <Columns>
                                                                                                             <asp:TemplateField ItemStyle-Width="20px">
                                                                                                                <ItemTemplate>
                                                                                                                    <a href="JavaScript:divExpandCollapse('divInnerChild<%# (Eval("TypeId").ToString() + Eval("Code").ToString() + Eval("Type").ToString()) %>');">
                                                                                                                    <img id="imgdivInnerChild<%# (Eval("TypeId").ToString() + Eval("Code").ToString() + Eval("Type").ToString()) %>" width="9px" border="0" src="images\icons\plus.gif" />
                                                                                                                    </a>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Op Seq" ItemStyle-Width="7.5%">
                                                                                                                <ItemTemplate>
                                                                                                                     <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                                                     <asp:HiddenField ID="hdnItemType" Value='<%# Eval("TypeId") %>' runat="server" />
                                                                                                                     <asp:HiddenField ID="hdnType" Value='<%# Bind("Type") %>' runat="server" />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Type" SortExpression="Type" ItemStyle-Width="10%">
                                                                                                                <ItemTemplate>
                                                                                                                     <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeName")  == "" ? " - " : Eval("TypeName") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblItem" runat="server" Text='<%# Eval("TypeValue") == "" ? " - " : Eval("TypeValue") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Budgeted" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                                                <ItemTemplate>
                                                                                                                        <asp:Label ID="lblBudgeted" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Budget"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Budget", "{0:c}")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Actual" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                                                <ItemTemplate>
                                                                                                                        <asp:Label ID="lblActual" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Actual"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Actual", "{0:c}")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Variance" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                                                <ItemTemplate>
                                                                                                                        <asp:Label ID="lblVariance" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Variance"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Variance", "{0:c}")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Commited" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                                                <ItemTemplate>
                                                                                                                        <asp:Label ID="lblCommited" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Commited"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Commited", "{0:c}")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Outstanding" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="8%">
                                                                                                                <ItemTemplate>
                                                                                                                        <asp:Label ID="lblOutstanding" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Outstanding"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Outstanding", "{0:c}")%>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField>
                                                                                                                <ItemTemplate>
                                                                                                                    <tr style="background-color:white">
                                                                                                                        <td colspan="100%">
                                                                                                                            <div id="divInnerChild<%# (Eval("TypeId").ToString() + Eval("Code").ToString() + Eval("Type").ToString()) %>" style="display: none; position: relative; left: 15px; overflow: auto">
                                                                                                                                <asp:GridView ID="gvInnerChildItemGrid" runat="server" AutoGenerateColumns="false" 
                                                                                                                                CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                                                GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" ShowFooter="true">
                                                                                                                                <RowStyle CssClass="evenrowcolor" />
                                                                                                                                <FooterStyle CssClass="footer" />
                                                                                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                                                                                <Columns>
                                                                                                                                    <asp:TemplateField HeaderText="Op Seq" ItemStyle-Width="2%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                                                                            <asp:HiddenField ID="hdnJobType" Value='<%# Bind("type") %>' runat="server" /> <!-- revenue and expense jobtype-->
                                                                                                                                            <asp:HiddenField ID="hdnItem" Value='<%# Bind("ItemID") %>' runat="server" />
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Type" ItemStyle-Width="2.7%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("TypeValue") == "" ? " - " : Eval("TypeValue") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="5%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("Item") == "" ? " - " : Eval("Item") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                     <asp:TemplateField HeaderText="Ref" ItemStyle-Width="3%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblRef" runat="server" Text='<%# Eval("Ref") == "" ? " - " : Eval("Ref") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Invoice Date" ItemStyle-Width="3%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Eval("InvoiceDate") == DBNull.Value ? " - " : String.Format("{0:MM/dd/yyyy}", Eval("InvoiceDate")) %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblAmount" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Amount"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Amount", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalAmount" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Budgeted" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblBudgetedAmt" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Budget"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Budget", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalBudgetAmt" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Actual" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblActualAmt" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Actual"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Actual", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalActualAmt" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                </Columns>
                                                                                                                                </asp:GridView>
                                                                                                                                <asp:GridView ID="gvInnerChildTicket" runat="server" AutoGenerateColumns="false" 
                                                                                                                                CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                                                                                GridLines="None" Width="100%" ShowHeaderWhenEmpty="false" ShowFooter="true">
                                                                                                                                <RowStyle CssClass="evenrowcolor" />
                                                                                                                                <FooterStyle CssClass="footer" />
                                                                                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                                                                                <Columns>
                                                                                                                                    <asp:TemplateField HeaderText="Op Seq" ItemStyle-Width="2%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                            <asp:Label ID="lblOpSeq" runat="server" Text='<%# Bind("Code") %>'></asp:Label>
                                                                                                                                            <asp:HiddenField ID="hdnJobType" Value='<%# Bind("type") %>' runat="server" /> <!-- revenue and expense jobtype-->
                                                                                                                                            <asp:HiddenField ID="hdnItem" Value='<%# Bind("ItemID") %>' runat="server" />
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Type" ItemStyle-Width="2.7%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblType" runat="server" Text='<%# Bind("TypeValue") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="5%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblItem" runat="server" Text='<%# Bind("Item") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                     <asp:TemplateField HeaderText="Ticket#" ItemStyle-Width="1%" ItemStyle-BackColor="White">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblTicket" runat="server" Text='<%# Bind("TicketID") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                     <asp:TemplateField HeaderText="Budgeted Hours" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White"  FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblBudgetedHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Est"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Est", "{0:n}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                         <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalBudgetHr" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Actual Hours" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblActualHr" runat="server" ForeColor='<%# Convert.ToDouble(Eval("ActualHr"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "ActualHr", "{0:n}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalActualHr" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Hourly Wage" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblHourlyWage" runat="server" ForeColor='<%# Convert.ToDouble(Eval("HourlyWage"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "HourlyWage", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalHourlyWage" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Actual Cost Incurred" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblActualCost" runat="server" ForeColor='<%# Convert.ToDouble(Eval("ActualCostIncur"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "ActualCostIncur", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalActualCost" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Labor Expense" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblLaborExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("LaborExp"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "LaborExp", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                          <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalLaborExp" runat="server"></asp:Label>
                                                                                                                                          </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                    <asp:TemplateField HeaderText="Expenses" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="2%" ItemStyle-BackColor="White" FooterStyle-HorizontalAlign="Right">
                                                                                                                                        <ItemTemplate>
                                                                                                                                                <asp:Label ID="lblExp" runat="server" ForeColor='<%# Convert.ToDouble(Eval("Expenses"))>=0?System.Drawing.Color.Black:System.Drawing.Color.Red %>' Text='<%# DataBinder.Eval(Container.DataItem, "Expenses", "{0:c}") %>'></asp:Label>
                                                                                                                                        </ItemTemplate>
                                                                                                                                        <FooterTemplate>
                                                                                                                                                <asp:Label ID="lblTotalOtherExp" runat="server"></asp:Label>
                                                                                                                                        </FooterTemplate>
                                                                                                                                    </asp:TemplateField>
                                                                                                                                </Columns>
                                                                                                                                </asp:GridView>
                                                                                                                            </div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                            </asp:GridView>
                                                                                                        </div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        </Columns>
                                                                                        </asp:GridView>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        </Columns>
                                                                        </asp:GridView>
                                                <%--  </div>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>--%>
                                                <%--<asp:GridView ID="gvArInvoice" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        DataKeyNames="ID" AllowPaging="True" PageSize="5" Width="100%" OnDataBound="gvArInvoice_DataBound" OnRowCommand="gvArInvoice_RowCommand"
                                                        ShowFooter="True" EmptyDataText="No Invoices Found...">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ID" SortExpression="ref" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice #" SortExpression="ref">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInv" Visible="false" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                    <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("ref") %>' Target="_blank" NavigateUrl='<%# "addinvoice.aspx?uid=" +Eval("ref")  %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice date" SortExpression="fdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Description" SortExpression="fdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvDesc" runat="server" Text='<%# Eval("fDesc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pretax Amount" SortExpression="amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPretaxAmout" runat="server"><%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalPretaxAmt" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sales Tax" SortExpression="stax">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesTax" runat="server"><%# DataBinder.Eval(Container.DataItem, "stax", "{0:c}")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalSalesTax" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice Total" SortExpression="total">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceTotal" runat="server"><%# DataBinder.Eval(Container.DataItem, "total", "{0:c}")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="InvTotalInvoice" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStatus" runat="server"><%#Eval("status")%></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount Due" SortExpression="balance">
                                                                <ItemTemplate>
                                                                    $
                                                                    <asp:Label ID="lblDue" runat="server"><%#Eval("balance")%></asp:Label>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:Label ID="InvTotalDue" runat="server"></asp:Label>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                        <PagerTemplate>
                                                            <div align="center">
                                                               <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                                                &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                    ImageUrl="~/images/Backward.png" />
                                                                &nbsp &nbsp <span>Page</span>
                                                                <asp:DropDownList ID="ddlArPages" runat="server" CausesValidation="false" AutoPostBack="True" OnSelectedIndexChanged="ddlArPages_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <span>of </span>
                                                                <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                &nbsp &nbsp
                                                                <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next" ImageUrl="images/Forward.png" />
                                                                &nbsp &nbsp
                                                                <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last" ImageUrl="images/last.png" />
                                                          </div>
                                                        </PagerTemplate>
                                                    </asp:GridView>--%>
                                                <div id="divJC" runat="server">
                                                <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server" TargetControlID="Panel4"
                                                    ExpandControlID="Image4" CollapseControlID="Image4" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                    CollapsedImage="~/images/arrow_right.png" ImageControlID="Image4" Enabled="True" />
                                                <div id="DivJobC" style="font-weight: bold; font-size: 15px">
                                                    Job Cost:
                                                        <asp:Image ID="Image4" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                                </div>
                                                <asp:Panel ID="Panel4" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                    <asp:GridView ID="gvJOBC" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        DataKeyNames="ref" Width="100%" ShowHeaderWhenEmpty="True" 
                                                        ShowFooter="True" EmptyDataText="No job cost found...">
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="ref" SortExpression="ref" Visible="true">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblfdesc" runat="server" Text='<%# Bind("fdesc") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice date" SortExpression="fdate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="amount">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Phase" SortExpression="Phase">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>

                                                    </asp:GridView>
                                                    <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                        title="Go to top">
                                                        <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                        Go To Top
                                                    </div>
                                                </asp:Panel>
                                            </div>
                                            </div>
                                            <div class="col-md-12 col-lg-12">
                                                <asp:UpdatePanel ID="UpdatePanel16" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                    <asp:PlaceHolder ID="PlaceHolderFinceBudget" runat="server"></asp:PlaceHolder>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbpnlBilling" runat="server" HeaderText="Project Billing">
                                        <HeaderTemplate>
                                            Billing
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div id="divInvoices" runat="server">
                                                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                    <ContentTemplate>
                                                        <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server" TargetControlID="Panel2"
                                                            ExpandControlID="Image2" CollapseControlID="Image2" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                            CollapsedImage="~/images/arrow_right.png" ImageControlID="Image2" Enabled="True" />
                                                        <div id="Div2" style="font-weight: bold; font-size: 15px">
                                                            AR Invoices:
                                                        <asp:Image ID="Image2" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                                        </div>
                                                        <asp:Panel ID="Panel2" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                            <asp:DropDownList ID="ddlInvoiceStatus" runat="server" CssClass="form-control"
                                                                Width="200px" AutoPostBack="true" OnSelectedIndexChanged="ddlInvoiceStatus_SelectedIndexChanged">
                                                                <asp:ListItem Value="-1">All</asp:ListItem>
                                                                <asp:ListItem Value="0">Open</asp:ListItem>
                                                                <asp:ListItem Value="1">Paid</asp:ListItem>
                                                                <asp:ListItem Value="2">Voided</asp:ListItem>
                                                                <asp:ListItem Value="3">Partially Paid</asp:ListItem>
                                                                <asp:ListItem Value="4">Marked as Pending</asp:ListItem>
                                                                <asp:ListItem Value="5">Paid by Credit Card</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblCountInvoice" runat="server" Style="font-style: italic; float: right"></asp:Label>
                                                            <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                DataKeyNames="ID" AllowPaging="True" PageSize="5" Width="100%" OnDataBound="gvInvoice_DataBound" OnRowCommand="gvInvoice_RowCommand"
                                                                ShowFooter="True" EmptyDataText="No invoices found...">
                                                                <RowStyle CssClass="evenrowcolor" />
                                                                <FooterStyle CssClass="footer" />
                                                                <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="ID" SortExpression="ref" Visible="false">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice #" SortExpression="ref">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInv" Visible="false" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("ref") %>' Target="_blank" NavigateUrl='<%# "addinvoice.aspx?uid=" +Eval("ref")  %>'></asp:HyperLink>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Manual Inv. #" SortExpression="manualInv">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMInv" runat="server" Text='<%# Bind("manualInv") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice date" SortExpression="fdate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Pretax Amount" SortExpression="amount">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPretaxAmout" runat="server"><%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalPretaxAmt" runat="server"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Sales Tax" SortExpression="stax">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSalesTax" runat="server"><%# DataBinder.Eval(Container.DataItem, "stax", "{0:c}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotalSalesTax" runat="server"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Invoice Total" SortExpression="total">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblInvoiceTotal" runat="server"><%# DataBinder.Eval(Container.DataItem, "total", "{0:c}")%></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="InvTotalInvoice" runat="server"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblStatus" runat="server"><%#Eval("status")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="PO" SortExpression="po">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPO" runat="server"><%#Eval("po")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Department Type" SortExpression="type">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDepartmentType" runat="server"><%#Eval("type")%></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount Due" SortExpression="balance">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblDue" runat="server"><%# DataBinder.Eval(Container.DataItem, "balance", "{0:c}") %></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="InvTotalDue" runat="server"></asp:Label>
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                                <PagerTemplate>
                                                                    <div align="center">
                                                                        <asp:ImageButton ID="ImageButton1" CausesValidation="false" runat="server" CommandArgument="First" ImageUrl="images/first.png" />
                                                                        &nbsp &nbsp<asp:ImageButton ID="ImageButton2" runat="server" CommandArgument="Prev" CausesValidation="false"
                                                                            ImageUrl="~/images/Backward.png" />
                                                                        &nbsp &nbsp <span>Page</span>
                                                                        <asp:DropDownList ID="ddlPages" runat="server" CausesValidation="false" AutoPostBack="True" OnSelectedIndexChanged="ddlPagesInvoice_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                        <span>of </span>
                                                                        <asp:Label ID="lblPageCount" runat="server"></asp:Label>
                                                                        &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton3" runat="server" CausesValidation="false" CommandArgument="Next" ImageUrl="images/Forward.png" />
                                                                        &nbsp &nbsp
                                                                        <asp:ImageButton ID="ImageButton4" runat="server" CausesValidation="false" CommandArgument="Last" ImageUrl="images/last.png" />
                                                                    </div>
                                                                </PagerTemplate>
                                                            </asp:GridView>
                                                            <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                                title="Go to top">
                                                                <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                                Go To Top
                                                            </div>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                         <div id="divAP" runat="server">
                                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server" TargetControlID="Panel3"
                                                ExpandControlID="Image3" CollapseControlID="Image3" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                CollapsedImage="~/images/arrow_right.png" ImageControlID="Image3" Enabled="True" />
                                            <div id="Div3" style="font-weight: bold; font-size: 15px">
                                                AP Invoices:
                                                    <asp:Image ID="Image3" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                            </div>
                                            <asp:Panel ID="Panel3" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                <asp:GridView ID="gvAPInvoices" runat="server" AutoGenerateColumns="False" 
                                                    CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    DataKeyNames="ID" Width="100%"
                                                    ShowFooter="True" EmptyDataText="No AP invoices found...">
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <FooterStyle CssClass="footer" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Ref" SortExpression="ref" Visible="true">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblID" runat="server" Text='<%# Bind("ref") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description" SortExpression="fdesc">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblfdesc" runat="server" Text='<%# Bind("fdesc") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Invoice date" SortExpression="fdate">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInvDate" runat="server" Text='<%# Eval("fdate", "{0:MM/dd/yy}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" SortExpression="amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblamount" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "amount", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Phase" SortExpression="Phase">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPhase" runat="server" Text='<%# Bind("Phase") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>

                                                </asp:GridView>
                                                <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                    title="Go to top">
                                                    <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                    Go To Top
                                                </div>

                                            </asp:Panel>
                                        </div>
                                         <div class="col-md-12 col-lg-12">
                                            
                                               <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                                                    <ContentTemplate>
                                                     <asp:PlaceHolder ID="PlaceHolderFinceBill" runat="server"></asp:PlaceHolder>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                </asp:TabContainer>
                              </asp:Panel>
                            </ContentTemplate>
                         </asp:TabPanel>
                        <asp:TabPanel ID="tbpnlTicket" runat="server" HeaderText="Project Ticket list">
                            <HeaderTemplate>
                                Ticket List 
                            </HeaderTemplate>
                            <ContentTemplate>
                                 <div id="divTickets" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server" TargetControlID="Panel1"
                                                ExpandControlID="Image1" CollapseControlID="Image1" SuppressPostBack="True" ExpandedImage="~/images/arrow_down.png"
                                                CollapsedImage="~/images/arrow_right.png" ImageControlID="Image1" Enabled="True" />
                                            <div id="Div1" style="font-weight: bold; font-size: 15px">
                                                Tickets:
                                            <asp:Image ID="Image1" runat="server" Style="float: left; height: 15px; cursor: pointer" />
                                            </div>
                                            <asp:Panel ID="Panel1" runat="server" Style="padding: 10px 10px 10px 10px;">
                                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" AutoPostBack="true"
                                                    TabIndex="14" Width="200px" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" CausesValidation="false">
                                                    <asp:ListItem Value="-1">All</asp:ListItem>
                                                    <asp:ListItem Value="-2">All Open</asp:ListItem>
                                                    <asp:ListItem Value="0">Un-Assigned</asp:ListItem>
                                                    <asp:ListItem Value="1">Assigned</asp:ListItem>
                                                    <asp:ListItem Value="2">Enroute</asp:ListItem>
                                                    <asp:ListItem Value="3">Onsite</asp:ListItem>
                                                    <asp:ListItem Value="4">Completed</asp:ListItem>
                                                    <asp:ListItem Value="5">Hold</asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:Label ID="lblTicketCount" runat="server" Style="font-style: italic; float: right"></asp:Label>
                                                <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    Width="100%" EmptyDataText="No tickets to display" PageSize="5" AllowPaging="true" ShowFooter="True" ShowHeaderWhenEmpty="true"
                                                    OnDataBound="gvOpenCalls_DataBound" OnRowCommand="gvOpenCalls_RowCommand" OnRowCreated="gvOpenCalls_RowCreated">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <FooterStyle CssClass="footer" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Ticket #" SortExpression="id">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTicketId" Visible="false" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                                                                <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Bind("id") %>' Target="_blank" NavigateUrl='<%# "addticket.aspx?id=" +Eval("id") +"&comp="+ Eval("comp")+"&pop=1" %>'></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Assigned to" SortExpression="dwork">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAssdTo" runat="server" Text='<%# Bind("dwork") %>'></asp:Label>
                                                                <asp:Label ID="lblRes" runat="server" CssClass="hoverGrid shadow transparent roundCorner"
                                                                    Text='<%# ShowHoverText(Eval("description"),Eval("fdescreason")) %>'></asp:Label>
                                                                <asp:HoverMenuExtender ID="hmeRes" runat="server" OffsetY="20" OffsetX="250" PopupControlID="lblRes"
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
                                                                <asp:Label ID="lbledate" runat="server" Text='<%# Eval("edate", "{0:MM/dd/yy hh:mm tt}") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="EST" SortExpression="est">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblEst" runat="server" Text='<%# Eval("est") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblESTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total Time" SortExpression="Tottime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTot" runat="server" Text='<%# Eval("Tottime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTotalFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="RT" SortExpression="reg">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRT" runat="server" Text='<%# Eval("reg") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblRTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OT" SortExpression="OT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOT" runat="server" Text='<%# Eval("OT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblOTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="NT" SortExpression="NT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNT" runat="server" Text='<%# Eval("NT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblNTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DT" SortExpression="DT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDT" runat="server" Text='<%# Eval("DT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblDTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="TT" SortExpression="TT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTT" runat="server" Text='<%# Eval("TT") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblTTFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Labor Expenses" SortExpression="laborexp" ItemStyle-HorizontalAlign="Right">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLAbExpense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "laborexp", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblLabExpenseFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Expenses" SortExpression="expenses">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblExpense" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "expenses", "{0:c}")%>'></asp:Label>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblExpenseFooter" runat="server"></asp:Label>
                                                            </FooterTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                    </Columns>
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
                                                <div onclick="window.scroll(0,0);" style="font-size: 10px; cursor: pointer; width: 80px;"
                                                    title="Go to top">
                                                    <img id="imgtop" alt="top" src="images/uptotop.gif" />
                                                    Go To Top
                                                </div>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                  </div>
                                 <div class="col-md-12 col-lg-12">
                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="PlaceHolderTicket" runat="server"></asp:PlaceHolder>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tbpnlBom" runat="server" HeaderText="Project BOM">
                            <HeaderTemplate>
                                BOM
                            </HeaderTemplate>
                            <ContentTemplate>
                                <asp:Panel ID="Panel5" runat="server">
                                    <div class="clearfix"></div>
                                    <asp:HiddenField ID="hdnItemJSON" runat="server" />
                                    <asp:HiddenField ID="hdnItemTeamJSON" runat="server" />
                                    <div class="table-scrollable" style="height: 400px;overflow-y: auto;border: none">
                                        <div class="col-lg-12 col-md-12">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                <asp:GridView ID="gvBOM" runat="server" AutoGenerateColumns="False" 
                                                        CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        PageSize="20" ShowFooter="true" OnRowDataBound="gvBOM_RowDataBound"
                                                        OnRowCommand="gvBOM_RowCommand">
                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="1%">
                                                                <HeaderTemplate>
                                                                <%--<a id="Button2" class="delButton" onclick="DelRow('<%=gvBOM.ClientID%>');"
                                                                        style="cursor: pointer;">
                                                                        <img src="images/menu_delete.png" title="Delete" width="18px" /></a>--%>
                                                                    <asp:ImageButton ID="ibDeleteBom" runat="server" CausesValidation="false"
                                                                        OnClientClick="return CheckDelete('gvBOM.ClientID');"
                                                                        OnClick="ibDeleteBom_Click"
                                                                        ImageUrl="images/menu_delete.png" Width="13px" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddProject" CausesValidation="False"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Line No." ItemStyle-Width="1%">
                                                                <ItemTemplate> 
                                                                    <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' style="display:none;"></asp:Label>
                                                                    <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex +1 %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                    <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.DataItemIndex +1 %>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Op Sequence" ItemStyle-Width="3%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("jcode") %>' Width="100%"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnCode" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="9%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlBType" runat="server"  DataTextField="Type" SelectedValue='<%# Eval("Btype") == DBNull.Value ? "0" : Eval("Btype") %>'
                                                                        DataValueField="ID" DataSource='<%#dtBomType%>' OnSelectedIndexChanged="ddlBType_SelectedIndexChanged" 
                                                                        AutoPostBack="true" Width="100%"></asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlItem" runat="server" Width="100%" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true">
                                                                        <%--SelectedValue='<%# Eval("BItem") == DBNull.Value ? "Select Item" : Eval("BItem") %>'--%>
                                                                    </asp:DropDownList>
                                                                    <asp:HiddenField ID="hdnItem" Value='<%# Eval("BItem") %>' runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" ItemStyle-Width="16%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>'
                                                                        Width="100%"></asp:TextBox>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty Required" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtyReq" runat="server" Text='<%# Eval("QtyReq","{0:0.00}") %>' Width="100%" onchange="calBudgetExt(this)"
                                                                         style="text-align:right;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="U/M" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtUM" runat="server" Text='<%# Eval("UM") %>' Width="100%"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnUMID" runat="server" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Scrap %">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtScrapFactor" runat="server" Text='<%# Eval("ScrapFact","{0:0.00}") %>' Width="90px" onchange="onchangeScrapFact(this)" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Budget Unit $" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtBudgetUnit" runat="server" Text='<%# Eval("BudgetUnit","{0:0.00}") %>' Width="100%" onchange="calBudgetExt(this)"
                                                                         style="text-align:right;" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Budget Ext $" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBudgetExt" runat="server" Text='<%# Eval("BudgetExt","{0:0.00}") %>' ></asp:Label>
                                                                    <asp:HiddenField ID="hdnBudgetExt" runat="server" Value='<%# Eval("BudgetExt","{0:0.00}") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="gvBOM" EventName="RowCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                     </div>
                                    <div class="col-md-12 col-lg-12">
                                        <asp:UpdatePanel ID="UpdatePanel15" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <asp:PlaceHolder ID="PlaceHolderBOM" runat="server"></asp:PlaceHolder>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:TabPanel>
                        <asp:TabPanel ID="tbpnMilestone" runat="server" HeaderText="Project Milestone">
                            <HeaderTemplate>
                                Milestones
                            </HeaderTemplate>
                            <ContentTemplate>
                               <div class="table-scrollable" style="height: 400px;overflow-y: auto;border: none">
                                    <div class="col-md-12 col-lg-12">
                                    <asp:UpdatePanel ID="UpdatePanel19" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvMilestones" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                    PageSize="20" ShowFooter="true" OnRowCommand="gvMilestones_RowCommand">
                                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                                    <FooterStyle CssClass="footer" />
                                                    <RowStyle CssClass="evenrowcolor" />
                                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                                    <Columns>
                                                        <asp:TemplateField ItemStyle-Width="1%">
                                                            <HeaderTemplate>
                                                                 <asp:ImageButton ID="ibDeleteMilestone" runat="server" CausesValidation="false"
                                                                        OnClientClick="return CheckDelete('gvMilestones.ClientID');"
                                                                        OnClick="ibDeleteMilestone_Click"
                                                                        ImageUrl="images/menu_delete.png" Width="13px" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" ItemStyle-Width="1%" />
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddMilestone" CausesValidation="False"
                                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Line No." ItemStyle-Width="1%">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex +1 %>'></asp:Label>
                                                                <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Width="65px" style="display:none;"></asp:Label>
                                                                <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnIndex" runat="server" Value='<%# Container.DataItemIndex +1 %>'></asp:HiddenField>
                                                                <asp:HiddenField ID="hdnID" runat="server" Value='<%# Eval("ID") %>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Op Sequence" ItemStyle-Width="3%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtCode" runat="server" Text='<%# Eval("jcode") %>' Width="100%"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnCode" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Type" ItemStyle-Width="6%">
                                                            <ItemTemplate>
                                                                <asp:DropDownList ID="ddlType" runat="server" SelectedValue='<%# Eval("jtype") == DBNull.Value ? 0 : Eval("jtype") %>' Width="100%">
                                                                    <asp:ListItem Value="0">Revenue</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Function" ItemStyle-Width="10%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtSType" runat="server" Text='<%# Eval("Department") %>' Width="100%" placeholder="Select Function"></asp:TextBox>
                                                                <asp:HiddenField ID="hdnType" Value='<%# Eval("type") %>' runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtName" runat="server" MaxLength="100" Text='<%# Eval("MilesName") %>'
                                                                    Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description" ItemStyle-Width="15%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtScope" runat="server" MaxLength="100" Text='<%# Eval("fdesc") %>'
                                                                    Width="100%"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("Amount","{0:0.00}") %>' onkeypress="return isDecimalKey(this,event)" 
                                                                    onchange="showDecimalVal(this)" Width="100%" style="text-align:right;"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Required by" ItemStyle-Width="5%">
                                                            <ItemTemplate>
                                                                <asp:TextBox ID="txtRequiredBy" runat="server" Text='<%# Eval("RequiredBy")!=DBNull.Value? (!(Eval("RequiredBy").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("RequiredBy"))) : "" ) : "" %>'
                                                                    Width="100%">
                                                                  </asp:TextBox>
                                                                <asp:CalendarExtender ID="txtRequiredBy_CalendarExtender" runat="server" Enabled="True"
                                                                    TargetControlID="txtRequiredBy">
                                                                </asp:CalendarExtender>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="gvMilestones" EventName="RowCommand" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="PlaceHolderMilestone" runat="server"></asp:PlaceHolder>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    </div>
                                </div>
                               <div class="clearfix"></div>
                            </ContentTemplate>
                        </asp:TabPanel>
                      </asp:TabContainer>
                      <div class="clearfix"></div>
                    </div>
                   </div>
                 <div class="clearfix"></div>
               </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>