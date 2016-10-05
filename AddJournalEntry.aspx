<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddJournalEntry.aspx.cs" Inherits="JournalEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="css/MS_style.css" rel="stylesheet" />
    <style type="text/css">

        .texttransparent {
            border-bottom: 1px solid #999;
            height: 30px;
            background: transparent;
        }

        .auto-style1 {
            height: 20px;
        }

        .gv_lbl {
            font-size: 14px;
        }

        .evenrowcolor {
            width: 50px;
        }

        .ajax__validatorcallout_callout_arrow_cell {
            width: 40px;
            padding-right: 0px !important;
        }

        .ajax__validatorcallout_callout_cell {
            padding-right: 0px !important;
        }

        .ajax_popup_accountname {
            left: 190px !important;
            top: 390px !important;
        }

        .ajax__validatorcallout div {
            border: solid 1px Black !important;
            background-color: LemonChiffon !important;
        }
        /*.ajax__validatorcallout_popup_table {
            border: solid 1px Black  !important;
            background-color: LemonChiffon  !important;
        }*/
        .ajax__validatorcallout_innerdiv .formTable tr td .ajax__validatorcallout_popup_table tr td {
            padding: 3px 0 3px 0;
        }

        .btnStyle {
            width: 104px;
            height: 29px;
        }
    </style>
    <link href="css/chosen.css" rel="stylesheet" />
    <script src="js/chosen.jquery.js" type="text/javascript"></script>

    <script type="text/javascript">
        function clearEntry() {
            var chkIsRecur = $('#<%= chkIsRecurr.ClientID %>').is(':checked');
            if (chkIsRecur == false) {
                var hdnTransID = $("#<%= hdnTransID.ClientID %>").val();
                $("#<%= txtEntryNo.ClientID %>").val(hdnTransID);
                $("#<%= hdnIsRecurr.ClientID %>").val("false");
            }
            else {
                $("#<%= txtEntryNo.ClientID %>").val('');
                $("#<%= hdnIsRecurr.ClientID %>").val("true");

            }
        }

        //$("[id*=txtGvDebit]").live('keyup', function () {
        //    BalanceProof();
        //});

        //$("[id*=txtGvCredit]").live('keyup', function () {
        //    BalanceProof();
        //});

        function BalanceProof() {
            var totalDebit = 0.00;
            var totalCredit = 0.00;
            var balance = 0;
            //var _isDebit = false;
            $("[id*=txtGvDebit]").each(function () {
                //totalDebit = totalDebit + parseFloat($(this).val());
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalDebit = totalDebit + parseFloat($(this).val());
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            $("[id*=txtGvCredit]").each(function () {
                //totalDebit = totalDebit + parseFloat($(this).val());
                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {
                        totalCredit = totalCredit + parseFloat($(this).val());
                    }
                    else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });
            balance = totalDebit - totalCredit;
            if (balance == 0) {
                $("[id*=txtProof]").css('color', 'Black');
                $('#<%=hdnIsPositive.ClientID %>').val("true");
                $("[id*=txtProof]").val("0.00");
            }
            else {
                if (balance < 0) {
                    balance = balance * -1;
                    $("[id*=txtProof]").val(totalDebit.toFixed(2).toString());
                    $("[id*=txtProof]").css('color', 'Red');
                    $('#<%=hdnIsPositive.ClientID %>').val("false");
                }
                else {
                    $("[id*=txtProof]").css('color', 'Black');
                    $('#<%=hdnIsPositive.ClientID %>').val("true");
                }

                $("[id*=txtProof]").val(balance.toFixed(2).toString());
            }

        }
        $(document).ready(function () {
          
         
            $('#<%=hdnIsPositive.ClientID %>').val("true");
            var config = {
                '.chosen-select': {},
                '.chosen-select-deselect': { allow_single_deselect: true },
                '.chosen-select-no-single': { disable_search_threshold: 10 },
                '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
                '.chosen-select-width': { width: "95%" }
            }
            for (var selector in config) {
                $(selector).chosen(config[selector]);
            }

            if ($(window).width() > 767) {
                $('#<%=txtDescription.ClientID%>').focus(function () {
                    $(this).animate({
                        //right: "+=0",
                        width: '520px',
                        height: '75px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });

                $('#<%=txtDescription.ClientID%>').blur(function () {
                    $(this).animate({
                        width: '100%',
                        height: '46px'
                    }, 500, function () {
                        // Animation complete.
                    });
                });
            }
            $("[id*=txtGvAcctNo]").change(function () {

                var txtGvAcctNo = $(this);
                var strAcctNo = $(this).val();
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "AccountAutoFill.asmx/GetChartByAcct",
                    data: '{"prefixText": "' + strAcctNo + '"}',
                    dataType: "json",
                    async: true,
                    success: function (data) {
                        var ui = $.parseJSON(data.d);

                        if (ui.length == 0) {
                            var strAcct = $(txtGvAcctNo).val();
                            $(txtGvAcctNo).val('');
                            noty({
                                text: 'Acct #' + strAcct + ' doesn\'t exist!',
                                type: 'warning',
                                layout: 'topCenter',
                                closeOnSelfClick: false,
                                timeout: false,
                                theme: 'noty_theme_default',
                                closable: true
                            });
                        }
                        else {
                            var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                            var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                            var txtGvAcctName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvAcctName'));

                            $(hdnAcctID).val(ui[0].ID);
                            $(txtGvAcctName).val(ui[0].fDesc);
                        }
                    },
                    error: function (result) {
                        alert("Due to unexpected errors we were unable to load Acct#");
                    }
                });
            });

        })

        function validate() {
            var valProof = $("#<%= txtProof.ClientID %>").val();
            if (parseFloat(valProof) > 0) {
                noty({ text: 'Your adjustment is out of balance', type: 'warning', layout: 'topCenter', closeOnSelfClick: false, timeout: false, theme: 'noty_theme_default', closable: true });
                return false;
            }
            else
            {
                var field = 'id';
                var url = window.location.href;
                if (url.indexOf('?' + field + '=') != -1)
                    return true;
                else {
                    return confirm('Do you want to save this Journal entry ?');
                }
                
            }
        }
        function Balance(obj) {
            //debugger;
            var objId = document.getElementById(obj.id);

            var isDebit = obj.id.search("txtGvDebit");
            if (isDebit == -1) {
                //on credit textbox change   
                var valCredit = document.getElementById(obj.id).value;
                if (isInt(valCredit) == true)
                    document.getElementById(obj.id).value = parseFloat(valCredit).toFixed(2);

                var txtGvDebit = document.getElementById(obj.id.replace('txtGvCredit', 'txtGvDebit'));
                $(txtGvDebit).val('0.00');
            }
            else {
                //on debit textbox change
                var valDebit = document.getElementById(obj.id).value;
                if (isInt(valCredit) == true)
                    document.getElementById(obj.id).value = parseFloat(valDebit).toFixed(2);

                var txtGvCredit = document.getElementById(obj.id.replace('txtGvDebit', 'txtGvCredit'));
                $(txtGvCredit).val('0.00');
            }
            BalanceProof();
        }
        function isInt(value) {
            var x = parseFloat(value);
            return !isNaN(value) && (x | 0) === x;
        }
        function VisibleRow(row, txt, gridview, event) {  //To make row's textbox visible
            //debugger;
            var rowst = document.getElementById(row)

            var grid = document.getElementById(gridview);
            $('#<%=gvJournal.ClientID %> input:text.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");
            });
            $('#<%=gvJournal.ClientID %> select.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");

            });

            var txtGvAcctNo = document.getElementById(txt);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("form-control1");

            var txtGvtransDes = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvtransDes'));
            $(txtGvtransDes).removeClass("texttransparent");
            $(txtGvtransDes).addClass("form-control1");

            var txtGvDebit = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDebit'));
            $(txtGvDebit).removeClass("texttransparent");
            $(txtGvDebit).addClass("form-control1");

            var txtGvCredit = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvCredit'));
            $(txtGvCredit).removeClass("texttransparent");
            $(txtGvCredit).addClass("form-control1");

            //debugger;
            if ($("[id*=txtGvLoc]").length) {
                var txtGvLoc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvLoc'));
                $(txtGvLoc).removeClass("texttransparent");
                $(txtGvLoc).addClass("form-control1");

                var txtGvPhase = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPhase'));
                $(txtGvPhase).removeClass("texttransparent");
                $(txtGvPhase).addClass("form-control1");
            }
        }
        function VisibleRowOnFocus(txt) {  //To make row's textbox visible
            //debugger;

            $('#<%=gvJournal.ClientID %> input:text.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");
            });
            $('#<%=gvJournal.ClientID %> select.form-control1').each(function () {

                $(this).removeClass("form-control1");
                $(this).addClass("texttransparent");

            });

            var txtGvAcctNo = document.getElementById(txt.id);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("form-control1");

            var txtGvtransDes = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvtransDes'));
            $(txtGvtransDes).removeClass("texttransparent");
            $(txtGvtransDes).addClass("form-control1");

            var txtGvDebit = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvDebit'));
            $(txtGvDebit).removeClass("texttransparent");
            $(txtGvDebit).addClass("form-control1");

            var txtGvCredit = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvCredit'));
            $(txtGvCredit).removeClass("texttransparent");
            $(txtGvCredit).addClass("form-control1");
            if ($("[id*=txtGvLoc]").length) {

                var txtGvLoc = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvLoc'));
                $(txtGvLoc).removeClass("texttransparent");
                $(txtGvLoc).addClass("form-control1");

                var txtGvPhase = document.getElementById(txt.id.replace('txtGvAcctNo', 'txtGvPhase'));
                $(txtGvPhase).removeClass("texttransparent");
                $(txtGvPhase).addClass("form-control1");
            }
        }
        function ClearRow(row) // To clear GridView Row on delete button click
        {
            //debugger;
            //var rowst = row.replace('_', '');
            $("#" + row + " input:text").each(function () {
                $(this).val("");

            });
            $("#" + row + " select").each(function () {
                $(this).val("");
            });
        }
        function clearJob(txt) {
            if ($(txt).val() == '') {
                var txtJobID = document.getElementById(txt.id.replace('txtGvJob', 'txtGvLoc'));
                $(txtJobID).val('');
            }
        }
        function clearPhase(txt) {
            if ($(txt).val() == '') {
                var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                $(hdnPID).val('0');
            }
        }
        //function ClearGridView() // Clear GridView
        //{
        //    $("[id*=ddlGvAcctName]").each(function () {
        //        $(this).val('');
        //    });
        //    $("[id*=txtGvtransDes]").each(function () {
        //                $(this).val('');
        //    });
        //    $("[id*=txtGvDebit]").each(function () {
        //                $(this).val('');
        //    });
        //    $("[id*=txtGvCredit]").each(function () {
        //                $(this).val('');
        //    });
        //}
        //function SetAccountID(ddlClientID,value)
        //{
        //    debugger;
        //    var hdnAcctID = document.getElementById(ddl.replace('ddlGvAcctName', 'hdnAcctID'));
        //    $(hdnAcctID).val(value);
        //}
        <%--function ValidateDdl(sender,args)
        {
            if (document.getElementById('<%=ddlGvAcctName.ClientID %>') != null)
            {
                var ddlGvAcctName = document.getElementById('<%=ddlGvAcctName.ClientID %>');

                if (ddlGvAcctName.value == '0') {
                    args.IsValid = false;//This shows the validation error message and stops execution at client side itself.
                }
                else if (ddlGvAcctName.value == ' Select Account ') {
                    args.IsValid = false;
                }
                else {
                    args.IsValid = true;//This will return to the server side. 
                }
            }
        }--%>
        //function DeleteRow(RowId)
        //{
        //    $("#" + RowId).parent().parent().remove();
        //}
    </script>

    <script type="text/javascript">
        function pageLoad(sender, args) {
            $(function () {

                //txtGvAcctName
                $("[id*=txtGvAcctNo]").autocomplete({
                    //appendTo: ".searchinput",
                    //open: function() {
                    //    var position = $("#results").position(),
                    //        left = position.left, top = position.top;

                    //    $("#results > ul").css({left: left + 20 + "px",
                    //        top: top + 4 + "px" });

                    //},
                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
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
                        //debugger;

                        if (ui.item.value == 0)
                            window.location.href = "addcoa.aspx";
                        else {
                            //debugger;
                            var txtGvAcctName = this.id;
                            var hdnAcctID = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'hdnAcctID'));
                            var txtGvAcctName = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'txtGvAcctName'));
                            $(txtGvAcctName).val(ui.item.label);
                            $(hdnAcctID).val(ui.item.value);
                            $(this).val(ui.item.acct);
                        }

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.acct);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("autocomplete")._renderItem = function (ul, item) {
                        debugger;
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

                $("[id*=txtGvPhase]").autocomplete({

                    source: function (request, response) {

                        var curr_control = this.element.attr('id');//request.term;//this.id;
                        var job = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));

                        var dtaaa = new dtaa();
                        dtaaa.jobID = document.getElementById(job.id).value;
                        //if (dtaaa.jobID != "") {

                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/GetAllPhase",
                                data: JSON.stringify(dtaaa),
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    response($.parseJSON(data.d));
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load phase details");
                                },
                                complete: function () {
                                    //debugger;
                                    $(this).data('requestRunning', false);
                                }
                            });
                        //}
                        return false;
                    },
                    deferRequestBy: 200,
                    select: function (event, ui) {

                        var txtGvPhase = this.id;
                        var hdnPID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnPID'));
                        var txtGvLoc = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvLoc'));
                        var txtGvJob = document.getElementById(txtGvPhase.replace('txtGvPhase', 'txtGvJob'));
                        var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));
                        
                        $(hdnPID).val(ui.item.Line);
                        $(this).val(ui.item.Desc);
                        $(txtGvLoc).val(ui.item.LocName);
                        $(txtGvJob).val(ui.item.JobName);
                        $(hdnJobID).val(ui.item.Job);
                        
                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.label);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).click(function () {
                    $(this).autocomplete('search', $(this).val())
                    //var txtGvPhase = this.id;
                    //var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));

                    //if ($(hdnJobID).val() != "") {
                    //    $(this).autocomplete('search', $(this).val())
                    //}
                    //else {
                    //    $(this).val("");
                    //    return false;
                    //}
                })
                $.each($(".psearchinput"), function (index, item) {
                    // debugger;
                    $(item).data("autocomplete")._renderItem = function (ul, item) {
                        //   debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.Line;
                        var result_item = item.Desc;
                        var result_desc = item.Line;
                        var result_type = item.PhaseType;
                        //var x = new RegExp('\\b' + query, 'ig'); // notice the escape \ here...            
                        //result_item = result_item.replace(x, function (FullMatch, n) {
                        //    return '<span class="highlight">' + FullMatch + '</span>'
                        //});
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
                            if (result_type == "0")
                                result_type = "Revenue";
                            else if (result_type == "1")
                                result_type = "Expense";

                            return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_type + "</span></a>")
                            .appendTo(ul);
                        }
                    };
                });


                $("[id*=txtGvLoc]").autocomplete({

                    source: function (request, response) {
                        var dtaaa = new dtaa();
                        dtaaa.prefixText = request.term;
                        query = request.term;

                        var str = request.term;
                        //debugger;
                        $.ajax({
                            type: "POST",
                            contentType: "application/json; charset=utf-8",
                            url: "AccountAutoFill.asmx/GetJobLocations",
                            data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + false + '", "con": "' + dtaaa.con + '"}',
                            dataType: "json",
                            async: true,
                            success: function (data) {
                                response($.parseJSON(data.d));
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load phase details");
                            }
                        });
                    },
                    select: function (event, ui) {

                        var txtGvLoc = this.id;
                        var txtGvJob = document.getElementById(txtGvLoc.replace('txtGvLoc', 'txtGvJob'));
                        var hdnJobID = document.getElementById(txtGvLoc.replace('txtGvLoc', 'hdnJobID'));

                        $(hdnJobID).val(ui.item.ID);
                        $(txtGvJob).val(ui.item.fDesc);
                        $(this).val(ui.item.Tag);

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.fDesc);
                        return false;
                    },
                    minLength: 0,
                    delay: 250
                }).click(function () {
                    //$(this).trigger('keydown.autocomplete');
                    $(this).autocomplete('search', $(this).val())
                })
                $.each($(".jsearchinput"), function (index, item) {
                    $(item).data("autocomplete")._renderItem = function (ul, item) {
                        //debugger;
                        var ula = ul;
                        var itema = item;
                        var result_value = item.ID;
                        var result_item = item.fDesc;
                        var result_desc = item.Tag;
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
                            .append("<a><b> Job: </b> " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                            .appendTo(ul);
                        }
                    };
                });

            });
        }
        function dtaa() {
            this.prefixText = null;
            this.con = null;
        }

        $("[id*=txtGvAcctName]").on("keydown", function () {
            debugger;
            $("#" + this.id).prop('readonly', true);
        });
        $("[id*=txtGvJob]").on("keydown", function () {
            $("#" + this.id).prop('readonly', true);
        });
        $("[id*=txtGvPhase]").on("keydown", function () {
            //debugger;
            var txtGvPhase = this.id;
            var hdnJobID = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnJobID'));
            if ($(hdnJobID).val() == "")
                return false;
        });
    </script>

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
                    <span>Financial Manager</span>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%=ResolveUrl("~/journalentry.aspx") %>">Journal Entries</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <asp:Label ID="lblAddEditUser" runat="server" Text="Add Journal Entries"></asp:Label>
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
                            <asp:Label ID="lblHeader" CssClass="title_text" style="margin-left:0" runat="server">Journal Entries</asp:Label></li>
                        <li>
                            <asp:LinkButton CssClass="icon-save" ID="btnSaveNew" ToolTip="Add New" runat="server" OnClientClick="return validate()" OnClick="btnSaveNew_Click" ValidationGroup="Journal"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton CssClass="icon-closed" ID="lnkClose" runat="server" ToolTip="Close" CausesValidation="false" OnClick="lnkClose_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>
            <!-- edit-tab start -->
            
            <div class="col-lg-12 col-md-12">
                
                <div class="com-cont">
                    <div class="alert alert-success" runat="server" id="divSuccess" > <!-- alert-dismissable alert-success-->
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        These month/year period is closed out. You do not have permission to add/update this record.
                    </div>

                    <div>
                        <asp:HiddenField ID="hdnBatchID" runat="server" />
                        <asp:HiddenField ID="hdnIsRecurr" runat="server" />
                        <asp:HiddenField ID="hdnIsPositive" runat="server" />
                        <asp:HiddenField ID="hdnTransID" runat="server" />

                        <div class="col-lg-3 col-md-3">
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Date
                                        <asp:RequiredFieldValidator runat="server" ID="rfvTransDate" ErrorMessage="Please select Date" ControlToValidate="txtTransDate"
                                              ValidationGroup="Journal" Display="None"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceTransDate" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvTransDate" />
                                    </div>
                                    <div class="fc-input1">
                                        <asp:TextBox ID="txtTransDate" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="15" autocomplete="off"></asp:TextBox>
                                        <asp:CalendarExtender ID="txtTransDate_CalendarExtender" runat="server" Enabled="True"
                                            TargetControlID="txtTransDate">
                                        </asp:CalendarExtender>

                                        <asp:RegularExpressionValidator ID="revTransDate" ControlToValidate ="txtTransDate" ValidationGroup="Journal" 
                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                        </asp:RegularExpressionValidator>
                                        <asp:ValidatorCalloutExtender ID="vceTransDate1" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="revTransDate" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Description<asp:RequiredFieldValidator runat="server" ID="rfvDescription" ErrorMessage="Please enter Description"
                                            Display="None" ControlToValidate="txtDescription" ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceDescription" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvDescription" />
                                    </div>
                                    <div class="fc-input1">
                                        <asp:TextBox ID="txtDescription" runat="server" Height="46px" CssClass="form-control" Style="position: relative; z-index: 100"
                                            TabIndex="6" Rows="2" Columns="10"
                                            TextMode="MultiLine" MaxLength="75"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3">
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Proof
                                    </div>
                                    <div class="fc-input1">
                                        <asp:TextBox ID="txtProof" runat="server" CssClass="form-control" TabIndex="2" align="right"
                                            MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Job Specific
                                    </div>
                                    <div class="fc-input1">
                                        <asp:CheckBox ID="chkJobSpecific" runat="server" OnCheckedChanged="chkJobSpecific_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3">
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Entry No.
                                            <asp:RequiredFieldValidator runat="server" ID="rfvEntryNo" ControlToValidate="txtEntryNo"
                                                ErrorMessage="Please enter Entry no." Display="None"
                                                ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceEntryNo" runat="server" Enabled="True" PopupPosition="Right"
                                            TargetControlID="rfvEntryNo" />
                                    </div>
                                    <div class="fc-input1">
                                        <asp:TextBox ID="txtEntryNo" runat="server" CssClass="form-control" TabIndex="2"
                                            MaxLength="15"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <div class="fc-label2">
                                        Is Recurring
                                    </div>
                                    <div class="fc-input1">
                                        <asp:CheckBox ID="chkIsRecurr" runat="server" OnCheckedChanged="chkIsRecurr_CheckedChanged" AutoPostBack="true" />
                                    </div>
                                </div>
                            </div>
                            
                            
                        </div>
                        <div class="col-lg-3 col-md-3">
                            <div class="form-col">
                                <asp:UpdatePanel ID="updPnlFrequency" runat="server">
                                    <ContentTemplate>
                                        <div class="fc-label2">
                                            <div>
                                                <asp:Label ID="lblFrequency" runat="server" Text="Frequency" />
                                            </div>
                                        </div>
                                        <div class="fc-input1">
                                            <div>
                                                <asp:DropDownList ID="ddlFrequency" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </div>
                                            <asp:RequiredFieldValidator runat="server" ID="rfvFrequency" ErrorMessage="Please select Frequency"
                                                Display="None" ControlToValidate="ddlFrequency" ValidationGroup="Journal"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceFrequency" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvFrequency" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="chkIsRecurr" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-group">
                                <div class="form-col">
                                    <asp:Image ID="imgCleared" runat="server" ImageUrl="~/images/icons/Cleared.png"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="table-scrollable table-addjournal" style="border: none">
                        <asp:UpdatePanel ID="updPnlAcctName" runat="server">
                            <ContentTemplate>
                                <asp:GridView ID="gvJournal" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                    CssClass="altrowstable" Width="100%"
                                    OnRowCommand="gvJournal_RowCommand">
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                   
                                    <Columns>
                                        <asp:TemplateField HeaderText="Account" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblId" runat="server" Text='<%# ((GridViewRow) Container).RowIndex %>'></asp:Label>
                                                <asp:Label ID="lblTID" runat="server" Text='<%# Eval("ID") == DBNull.Value ? "" : Eval("ID") %>'></asp:Label>
                                                <asp:Label ID="lblTimeStamp" runat="server" Text='<%# Bind("TimeStamp") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acct No.">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="texttransparent searchinput"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("AcctNo") == DBNull.Value ? "" : Eval("AcctNo") %>' onfocus="VisibleRowOnFocus(this)"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvAcctName" runat="server" Width="100%" MaxLength="15"
                                                    autocomplete="off" CssClass="texttransparent " Text='<%# Bind("Account") %>' onkeypress="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnAcctID" Value='<%# Eval("AcctID") == DBNull.Value ? "" : Eval("AcctID") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Memo">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvtransDes" runat="server" CssClass="texttransparent"
                                                    Width="100%" autocomplete="off" Text='<%# Bind("Description") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="$ Debit">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvDebit" runat="server" CssClass="texttransparent clsDebit " autocomplete="off"
                                                    MaxLength="15" Width="100%" onchange="Balance(this)" Text='<%# Bind("Debit") %>' Style="text-align: right" OnTextChanged="txtGvDebit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="$ Credit">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvCredit" runat="server" CssClass="texttransparent clsCredit" autocomplete="off"
                                                    MaxLength="15" Width="100%" onchange="Balance(this)" Text='<%# Bind("Credit") %>' Style="text-align: right" OnTextChanged="txtGvCredit_TextChanged" AutoPostBack="true"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Location Name">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvLoc" runat="server" CssClass="texttransparent jsearchinput"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("Loc") == DBNull.Value ? "" : Eval("Loc") %>' onchange="clearJob(this)"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Project">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvJob" runat="server" CssClass="texttransparent"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("JobName") == DBNull.Value ? "" : Eval("JobName") %>' onkeypress="return false;"></asp:TextBox>
                                                <asp:HiddenField ID="hdnJobID" Value='<%# Eval("JobID") == DBNull.Value ? "" : Eval("JobID") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtGvPhase" runat="server" CssClass="texttransparent psearchinput"
                                                    MaxLength="15" Width="100%" Text='<%# Eval("Phase") == DBNull.Value ? "" : Eval("Phase") %>' onchange="clearPhase(this)"></asp:TextBox>
                                                <asp:HiddenField ID="hdnPID" Value='<%# Eval("PhaseID") == DBNull.Value ? "" : Eval("PhaseID") %>' runat="server" />
                                            </ItemTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibDelete" runat="server" CommandName="DeleteTransaction"
                                                    CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                    ImageUrl="~/images/glyphicons-17-bin.png" Width="13px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="chkJobSpecific" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div style="padding-top: 15px; border-bottom-width: 15px; padding-bottom: 15px;">
                            <asp:Button ID="btnAddNewLines" runat="server" CausesValidation="false"
                                OnClick="lbtnAddNewLines_Click" Text="Add New Lines" CssClass="btn btn-primary" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- edit-tab end -->
        <div class="clearfix"></div>

        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>

