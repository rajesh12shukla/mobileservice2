<%@ Page Title="" Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true" CodeFile="AddPO.aspx.cs" Inherits="AddPO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <style>
        .ui-autocomplete {
            max-height: 300px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            z-index: 1000 !important;
        }
        .ModalPopupBG {
            background-color: black;
            filter: alpha(opacity=50);
            opacity: 0.7;
        }
    </style>
    <script type="text/javascript">
      
        function GetClientUTC() {
            var now = new Date();
            var offset = now.getTimezoneOffset();
            document.getElementById('<%= txtDate.ClientID %>').value = (now.getMonth() + 1)+"/"+now.getDate()+"/"+now.getFullYear();
            var due = new Date(now.setTime(now.getTime() + 30 * 86400000));
            document.getElementById('<%= txtDueDate.ClientID %>').value = (due.getMonth() + 1) + "/" + due.getDate() + "/" + due.getFullYear();
        }
        $(document).ready(function () {
            $('#ui-active-menuitem').click(function () {
                $('.ui-menu-item').removeClass('ui-autocomplete-loading');
            })
           
            function dta() {
                this.prefixText = null;
                //this.con = null;
            }

            InitializeGrids('<%=gvGLItems.ClientID%>');
          

            ///////////// Ajax call for vendor auto search ////////////////////                
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
                    var str = ui.item.Name;
                    if (str == "No Record Found!") {
                        $("#<%=txtVendor.ClientID%>").val("");
                    }
                    else
                    {
                        $("#<%=txtVendor.ClientID%>").val(ui.item.Name);
                        $("#<%=hdnVendorID.ClientID%>").val(ui.item.ID);

                        //debugger;
                       $("#<%=ddlTerms.ClientID%>").val(ui.item.Terms);
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
        function InitializeGrids(Gridview) {

            var rowone = $("#" + Gridview).find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });
        }
        function itemJSON() {
            var rawData = $('#<%=gvGLItems.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);
        }
        ///////////// Custom validator function for vendor auto search  ////////////////////
        function ChkVendor(sender, args) {
            var hdnVendorId = document.getElementById('<%=hdnVendorID.ClientID%>');
            if (hdnVendorId.value == '') {
                args.IsValid = false;
            }
        }
        function clearPhase(txt) {
            if ($(txt).val() == '') {
                var hdnPID = document.getElementById(txt.id.replace('txtGvPhase', 'hdnPID'));
                $(hdnPID).val('0');
            }
        }
        function dtaa() {
            this.prefixText = null;
            this.con = null;
        }
        function makeReadonly(txt) {
            $("#" + txt.id).prop('readonly', true);
        }
        function addedItem(item, itemId, phaseId, typeId, type, fdesc)
        {
            noty({
                text: 'BOM Item added successfully!',
                type: 'success',
                layout: 'topCenter',
                closeOnSelfClick: false,
                timeout: 5000,
                theme: 'noty_theme_default',
                closable: true
            });
           
            var rowItem = $("#<%=hdnRowField.ClientID%>").val();
            var rowItemId = document.getElementById(rowItem.replace('txtGvItem', 'hdnItemID'));
            var rowDesc = document.getElementById(rowItem.replace('txtGvItem', 'txtGvDesc'));
            var rowPhase = document.getElementById(rowItem.replace('txtGvItem', 'txtGvPhase'));
            var rowPid = document.getElementById(rowItem.replace('txtGvItem', 'hdnPID'));
            var rowtid = document.getElementById(rowItem.replace('txtGvItem', 'hdnTypeId'));
            
            document.getElementById(rowItem).value = item;
            $(rowItemId).val(itemId);
            $(rowDesc).val(fdesc);
            $(rowPhase).val(type);
            $(rowPid).val(phaseId);
            $(rowtid).val(typeId);
            ResetBom();
            //window.parent.document.getElementById('btnCancel').click();
        }
        function pageLoad(sender, args) {
            $(function () {
                $("#<%=txtQty.ClientID%>").change(function () {
                    var budgetunit = $("#<%=txtBudgetUnit.ClientID%>").val();
                    var qty = $(this).val();
                    if (budgetunit != "" && qty != "") {
                        var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                        $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                    }
                    if(budgetunit != "")
                    {
                        $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                    }
                    if(qty != "")
                    {
                        $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                    }
                });
                $("#<%=txtBudgetUnit.ClientID%>").change(function () {
                    var budgetunit = $(this).val();
                    var qty = $("#<%=txtQty.ClientID%>").val();
                    if (budgetunit != "" && qty != "") {
                        var budgetext = parseFloat(qty) * parseFloat(budgetunit);
                        $("#<%=lblBudgetExt.ClientID%>").text(budgetext.toFixed(2));
                    }
                    if (budgetunit != "") {
                        $("#<%=txtBudgetUnit.ClientID%>").val(parseFloat(budgetunit).toFixed(2));
                    }
                    if (qty != "") {
                        $("#<%=txtQty.ClientID%>").val(parseFloat(qty).toFixed(2));
                    }
                });
                $("[id*=txtGvJob]").focusout(function () {

                    var txtGvJob = $(this).attr('id');
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                   
                    if ($(hdnJobID).val() != "" & $(hdnJobID).val() != "0")
                    {
                        var txtGvAcctNo = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvAcctNo'));
                        var strAcctNo = $(txtGvAcctNo).val();

                        var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                        var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));
                        var txtGvAcctName = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'txtGvAcctName'));
                        if (strAcctNo == '') {
                            var vendorId = $('#<%=hdnVendorID.ClientID%>').val();
                            if (vendorId != '') {
                                $.ajax({
                                    type: "POST",
                                    contentType: "application/json; charset=utf-8",
                                    url: "AccountAutoFill.asmx/GetGLbyVendor",
                                    data: '{"vendor": "' + vendorId + '"}',
                                    dataType: "json",
                                    async: true,
                                    success: function (data) {
                                        var ui = $.parseJSON(data.d);

                                        if (ui.length > 0) {
                                            var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct;

                                            $(txtGvAcctNo).val(strAcct);
                                            $(hdnAcctID).val(ui[0].DA);
                                            //$(txtGvAcctName).val(ui[0].DefaultAcct);
                                        }
                                    },
                                    error: function (result) {
                                        alert("Due to unexpected errors we were unable to load default acct#");
                                    }
                                });
                            }
                        }

                        var txtGvDue = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvDue'));
                        var txtDueDate = document.getElementById('<%=txtDueDate.ClientID%>');

                        if (txtGvDue.value == '') {
                            $(txtGvDue).val(txtDueDate.value);
                        }
                    }
                });
               
                $("[id*=txtGvAcctNo]").change(function () {
                  
                    var txtGvAcctNo = $(this);
                    var strAcctNo = $(this).val();

                    var txtGvAcctNo1 = $(txtGvAcctNo).attr('id');
                    var hdnAcctID = document.getElementById(txtGvAcctNo1.replace('txtGvAcctNo', 'hdnAcctID'));

                    if (strAcctNo != '') {
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
                                    var strAcct = ui[0].Acct + ' - ' + ui[0].fDesc;

                                    $(txtGvAcctNo).val(strAcct);
                                    $(hdnAcctID).val(ui[0].ID);
                                }
                            },
                            error: function (result) {
                                alert("Due to unexpected errors we were unable to load Acct#");
                            }
                        });
                    }
                    else {
                        var vendorId = $('#<%=hdnVendorID.ClientID%>').val();
                        if (vendorId != '') {
                            $.ajax({
                                type: "POST",
                                contentType: "application/json; charset=utf-8",
                                url: "AccountAutoFill.asmx/GetGLbyVendor",
                                data: '{"vendor": "' + vendorId + '"}',
                                dataType: "json",
                                async: true,
                                success: function (data) {
                                    //debugger;
                                    var ui = $.parseJSON(data.d);

                                    if (ui.length > 0) {
                                        var strAcct = ui[0].Acct + ' - ' + ui[0].DefaultAcct
                                        $(txtGvAcctNo1).val(strAcct);
                                        $(hdnAcctID).val(ui[0].DA);
                                    }
                                },
                                error: function (result) {
                                    alert("Due to unexpected errors we were unable to load default acct#");
                                }
                            });
                        }
                    }
                });

            
                $("[id*=txtGvAcctNo]").autocomplete({
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

                        if (ui.item.value == 0)
                            window.location.href = "addcoa.aspx";
                        else {
                            var txtGvAcctName = this.id;
                            var hdnAcctID = document.getElementById(txtGvAcctName.replace('txtGvAcctNo', 'hdnAcctID'));
                            var strAcct = ui.item.acct + " - " + ui.item.label;
                            $(hdnAcctID).val(ui.item.value);
                            $(this).val(strAcct);
                        }

                        return false;
                    },
                    focus: function (event, ui) {
                        $(this).val(ui.item.acct);

                        return false;
                    },
                    //change: function (event, ui) {
                       
                        //var txtGvAcctNo = this.id;
                        //var hdnAcctID = document.getElementById(txtGvAcctNo.replace('txtGvAcctNo', 'hdnAcctID'));
                        //var strAcct = document.getElementById(txtGvAcctNo).value;

                        //if (strAcct == '') {
                        //    $(hdnAcctID).val('')
                        //}
                    //},
                    minLength: 0,
                    delay: 250
                })
                $.each($(".searchinput"), function (index, item) {
                    $(item).data("autocomplete")._renderItem = function (ul, item) {
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
                            
                        }
                        else {
                            return $("<li></li>")
                            .data("item.autocomplete", item)
                            .append("<a>" + result_item + ", <span style='color:Gray;'>" + result_desc + "</span></a>")
                            .appendTo(ul);
                        }
                    };
                });
              
            });

            $("[id*=txtGvPhase]").autocomplete({

                source: function (request, response) {

                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvPhase', 'hdnJobID'));
                    var prefixText = request.term;
                    var job = document.getElementById(job.id).value;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhase",
                        data: '{"jobID": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load type.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });
                    return false;
                },
                deferRequestBy: 200,
                select: function (event, ui) {

                    var txtGvPhase = this.id;
                    var hdnTypeId = document.getElementById(txtGvPhase.replace('txtGvPhase', 'hdnTypeId'));

                    var str = ui.item.TypeName;
                    if (str == "No Record Found!") {
                        $(this).val("");
                    }
                    else {
                        $(hdnTypeId).val(ui.item.Type);
                        $(this).val(ui.item.TypeName);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.TypeName);
                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".phsearchinput"), function (index, item) {
                
                $(item).data("autocomplete")._renderItem = function (ul, item) {    
                    var ula = ul;
                    var itema = item;
                    var result_value = item.Type;
                    var result_item = item.TypeName;
                      
                    return $("<li></li>")
                    .data("item.autocomplete", item)
                    .append("<a>" + result_item + "</a>")
                    .appendTo(ul);
                };
            });
            $("[id*=txtGvItem]").change(function () {

                var txtGvItem = $(this);
                var strItem = $(this).val();

                var txtGvItem1 = $(txtGvItem).attr('id');
                var hdnTypeId = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvItem1.replace('txtGvItem', 'txtGvDesc'));
                var job = document.getElementById(txtGvItem1.replace('txtGvItem', 'hdnJobID')).value;
                var typeId = $(hdnTypeId).val();

                if (strItem != "") {

                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAutoFillItem",
                        data: '{"prefixText": "' + strItem + '", "typeId": "' + typeId + '", "job": "' + job + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            var ui = $.parseJSON(data.d);
                            debugger;
                            if (ui.length == 0) {

                                //$(txtGvItem).val('');
                                $(hdnItemID).val('');
                                $(hdnPID).val('');
                                //$(txtGvDesc).val('');
                                noty({
                                    text: 'Item \'' + strItem + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                $(txtGvItem).val(ui[0].ItemDesc);
                                $(hdnItemID).val(ui[0].ItemID);
                                $(hdnPID).val(ui[0].Line);
                                $(txtGvDesc).val(ui[0].fDesc);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Item");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnItemID).val('');
                }
                //else {
                //    $(hdnPID).val('');
                //    $(txtGvItem).val('');
                //    $(hdnItemID).val('');
                //    $(txtGvDesc).val('');
                //}
            });
            $("[id*=txtGvPhase]").change(function () {

                var txtGvPhase = $(this);
                var strPhase = $(this).val();

                var txtGvPhase1 = $(txtGvPhase).attr('id');
                var hdnTypeId = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnTypeId'));
                var hdnPID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnPID'));
                var txtGvItem = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvItem'));
                var hdnItemID = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'hdnItemID'));
                var txtGvDesc = document.getElementById(txtGvPhase1.replace('txtGvPhase', 'txtGvDesc'));

                if (strPhase != "") {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetAutoFillPhase",
                        data: '{"prefixText": "' + strPhase + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {

                            var ui = $.parseJSON(data.d);

                            if (ui.length == 0) {
                                $(txtGvPhase).val('');
                                $(hdnTypeId).val('');
                                $(hdnPID).val('');
                                $(txtGvItem).val('');
                                $(hdnItemID).val('');
                                noty({
                                    text: 'Type \'' + strPhase + '\' doesn\'t exist!',
                                    type: 'warning',
                                    layout: 'topCenter',
                                    closeOnSelfClick: false,
                                    timeout: 5000,
                                    theme: 'noty_theme_default',
                                    closable: true
                                });
                            }
                            else {
                                var lbl = ui[0].Label;
                                var val = ui[0].Value;
                                $(txtGvPhase).val(lbl);
                                $(hdnTypeId).val(val);
                            }
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load Type");
                        }
                    });
                }
                else {
                    $(hdnPID).val('');
                    $(hdnTypeId).val('');
                    $(txtGvItem).val('');
                    $(hdnItemID).val('');
                    $(txtGvDesc).val('');
                }
            });
          $("[id*=txtGvItem]").autocomplete({

                source: function (request, response) {
                    
                    var curr_control = this.element.attr('id');
                    var job = document.getElementById(curr_control.replace('txtGvItem', 'hdnJobID')).value;
                    
                    var typeId = document.getElementById(curr_control.replace('txtGvItem', 'hdnTypeId')).value;
                    var prefixText = request.term;
                 
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetPhaseByItem",
                        data: '{"typeId": "'+ typeId +'", "jobId": "' + job + '", "prefixText": "' + prefixText + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load item.");
                        },
                        complete: function () {
                            $(this).data('requestRunning', false);
                        }
                    });

                    return false;
                },
                deferRequestBy: 200,
                select: function (event, ui) {

                    var curr_control = this.id;
                    var hdnItemID = document.getElementById(curr_control.replace('txtGvItem', 'hdnItemID'));
                    var txtGvDesc = document.getElementById(curr_control.replace('txtGvItem', 'txtGvDesc'));
                    var hdnPID = document.getElementById(curr_control.replace('txtGvItem', 'hdnPID'));
                    var job = document.getElementById(curr_control.replace('txtGvItem', 'hdnJobID')).value;

                    var str = ui.item.ItemDesc;
                    var strId = ui.item.ItemID;

                    if (strId == "0") {
                        $(this).val("");
                        $(hdnItemID).val("");
                        $(hdnPID).val("");
                        <%--  $("#<%=hdnJobId.ClientID%>").val(job);
                        $("#<%=hdnRowField.ClientID%>").val(curr_control);
                        var modal = $find("mpeBomItem");
                        modal.show();--%>
                    }
                    else {
                        $(txtGvDesc).val(ui.item.fDesc);
                        $(hdnItemID).val(ui.item.ItemID);
                        $(hdnPID).val(ui.item.Line);
                        $(this).val(ui.item.ItemDesc);
                    }
                    return false;
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.ItemDesc);
                    return false;
                },
               <%-- change: function (event, ui) {
                    var txtGvItem = this.id;
                    var hdnTypeId = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnTypeId'));
                    var hdnPID = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnPID'));
                    var txtGvPhase = document.getElementById(txtGvItem.replace('txtGvItem', 'txtGvPhase'));
                    var hdnItemID = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnItemID'));
                    var job = document.getElementById(txtGvItem.replace('txtGvItem', 'hdnJobID')).value;

                    var strItem = document.getElementById(txtGvItem).value;

                    if (strItem == '') {
                        $(hdnItemID).val('')
                    }
                    else {
                        debugger;

                        if ($(hdnPID).val() == "0") {
                            var value;
                            value = confirm('Item doesn\'t exists in BOM. Do you want to add this new BOM item ?');
                            if (value == true) {
                                var itemid = $(hdnItemID).val();
                                var typeid = $(hdnTypeId).val();
                                $("#<%=hdnJobId.ClientID%>").val(job);
                                $("#<%=hdnRowField.ClientID%>").val(txtGvItem);
                                $("#<%=txtItem.ClientID%>").val(strItem);
                                $("#<%=hdnItemID.ClientID%>").val(itemid);
                                $("#<%=ddlBomType.ClientID%>").val(typeid);
                                var modal = $find("mpeBomItem");
                                modal.show();
                            }
                            else {
                                $(this).val('');
                                $(txtGvPhase).val('');
                                $(hdnItemID).val('');
                                $(hdnTypeId).val('');
                            }
                        }
                    }
                   
                },--%>
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".pisearchinput"), function (index, item) {

                $(item).data("autocomplete")._renderItem = function (ul, item) {
                    var ula = ul;
                    var itema = item;
                    var result_value = item.ItemID;
                    var result_item = item.ItemDesc;
                    var result_line = item.Line;


                    if (result_line == "0") {
                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a>" + result_item + "</a>")
                        .appendTo(ul);
                    }
                    else {
                        return $("<li></li>")
                        .data("item.autocomplete", item)
                        .append("<a><b>" + result_item + "</b></a>")
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
                            alert("Due to unexpected errors we were unable to load location details");
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
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".jsearchinput"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {
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
                        .append("<a><b> Project: </b> "+ result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                        .appendTo(ul);
                    }
                };
            });
           
            $("[id*=txtGvJob]").autocomplete({

                source: function (request, response) {
                    var dtaaa = new dtaa();
                    dtaaa.prefixText = request.term;
                    query = request.term;
                    var str = request.term;
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "AccountAutoFill.asmx/GetJobLocations",
                        data: '{"prefixText": "' + dtaaa.prefixText + '", "IsJob": "' + true + '", "con": "' + dtaaa.con + '"}',
                        dataType: "json",
                        async: true,
                        success: function (data) {
                            response($.parseJSON(data.d));
                        },
                        error: function (result) {
                            alert("Due to unexpected errors we were unable to load project details");
                        }
                    });
                },
                select: function (event, ui) {

                    var txtGvJob = this.id;
                    var txtGvLoc = document.getElementById(txtGvJob.replace('txtGvJob', 'txtGvLoc'));
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));

                    $(hdnJobID).val(ui.item.ID);
                    var jobStr = ui.item.ID + ", " + ui.item.fDesc;
                    $(this).val(jobStr);
                    $(txtGvLoc).val(ui.item.Tag);

                    return false;
                },
                change: function (event, ui) {
                    var txtGvJob = this.id;
                    var hdnJobID = document.getElementById(txtGvJob.replace('txtGvJob', 'hdnJobID'));
                    var strJob = document.getElementById(txtGvJob).value;

                    if (strJob == '') {
                        $(hdnJobID).val('')
                    }
                },
                focus: function (event, ui) {
                    $(this).val(ui.item.fDesc);

                    return false;
                },
                minLength: 0,
                delay: 250
            }).click(function () {
                $(this).autocomplete('search', $(this).val())
            })
            $.each($(".psearchinput"), function (index, item) {
                $(item).data("autocomplete")._renderItem = function (ul, item) {
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
                        .append("<a><b> Project: </b> " + result_value + ", " + result_item + ", <span style='color:Gray;'><b> Loc: </b>" + result_desc + "</span></a>")
                        .appendTo(ul);
                    }
                };
            });
        }
    </script>

    <script type="text/javascript">
        function ResetBom()
        {
            $("#<%=txtBudgetUnit.ClientID%>").val('0.00');
            $("#<%=lblBudgetExt.ClientID%>").val('0.00');
            $("#<%=txtQty.ClientID%>").val('');
            $("#<%=txtOpSeq.ClientID%>").val('');
            $("#<%=txtItem.ClientID%>").val('');
            $("#<%=hdnItemID.ClientID%>").val('');
            $("#<%=txtJobDesc.ClientID%>").val('');
            $("#<%=txtUM.ClientID%>").val('');
            $("#<%=ddlBomType.ClientID%>").val('0');
        }
       <%-- function cancel() {
            window.parent.document.getElementById('<%=mpeBomItem.ClientID%>').click();

        }--%>
        function cancel() {
            window.parent.document.getElementById('btnCancel').click();
            var rowItem = $("#<%=hdnRowField.ClientID%>").val();
            var rowItemId = document.getElementById(rowItem.replace('txtGvItem', 'hdnItemID'));
            var rowPhase = document.getElementById(rowItem.replace('txtGvItem', 'txtGvPhase'));
            var rowPid = document.getElementById(rowItem.replace('txtGvItem', 'hdnPID'));
            var rowtid = document.getElementById(rowItem.replace('txtGvItem', 'hdnTypeId'));
          
            document.getElementById(rowItem).value = '';
            $(rowItemId).val('');
            $(rowPhase).val('');
            $(rowPid).val('');
            $(rowtid).val('');

            ResetBom();
        }
       
        /////////////////// To calculate Total and to make Gridview Amount Value to 2 decimal ////////////
        function CalTotalVal(obj) {
           

            var txt = obj.id;
            
            var txtGvQuan;
            var txtGvPrice;
            var txtGvAmount;

            if(txt.indexOf("Quan") >= 0)
            {
                txtGvQuan = document.getElementById(txt);
                txtGvPrice = document.getElementById(txt.replace('txtGvQuan', 'txtGvPrice'));
                txtGvAmount = document.getElementById(txt.replace('txtGvQuan', 'txtGvAmount'));
            }
            else if(txt.indexOf("Price") >= 0)
            {
                txtGvPrice = document.getElementById(txt);
                txtGvQuan = document.getElementById(txt.replace('txtGvPrice', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt.replace('txtGvPrice', 'txtGvAmount'));
            }
            else if (txt.indexOf("Amount") >= 0)
            {
                txtGvPrice = document.getElementById(txt.replace('txtGvAmount', 'txtGvPrice'));
                txtGvQuan = document.getElementById(txt.replace('txtGvAmount', 'txtGvQuan'));
                txtGvAmount = document.getElementById(txt);
            }

            if (!jQuery.trim($(txtGvQuan).val()) == '') {
                if (isNaN(parseFloat($(txtGvQuan).val()))) {
                    $(txtGvQuan).val('0.00');
                }
            }

            if (!jQuery.trim($(txtGvPrice).val()) == '') {
                if (isNaN(parseFloat($(txtGvPrice).val()))) {
                    $(txtGvPrice).val('0.00');
                }
            }
           
            if (!jQuery.trim($(txtGvQuan).val()) == '' && !jQuery.trim($(txtGvPrice).val()) == '') {
                if (!isNaN(parseFloat($(txtGvQuan).val())) && !isNaN(parseFloat($(txtGvPrice).val()))) {
                    var valAmount = parseFloat($(txtGvQuan).val()) * parseFloat($(txtGvPrice).val());
                    $(txtGvAmount).val(valAmount.toFixed(2));
                } 
            }
            CalculateTotalAmt();

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
        }
        function CalculateTotal(obj) {

            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                CalTotalVal(obj);
            }
            else {
                CalTotalVal(obj);
            }
            
            CalculateTotalAmt();
        }
        function CalculateTotalAmt() {

            var tAmount = 0.00;
            $("[id*=txtGvAmount]").each(function () {

                if (!jQuery.trim($(this).val()) == '') {
                    if (!isNaN(parseFloat($(this).val()))) {

                        var totalAmount = jQuery(this).parent().parent().find('.clsAmount').val();
                        if (totalAmount != null && totalAmount != "") {
                            tAmount = tAmount + parseFloat($(this).val());
                        }
                    } else
                        $(this).val('');
                }
                else {
                    $(this).val('');
                }
            });

            $('#<%=lblTotalAmount.ClientID%>').text(tAmount.toFixed(2));
            $('#<%=hdnTotal.ClientID%>').val(tAmount.toFixed(2));
        }
       
        ////////////// To check is entered charcter is number or not//////////////
        function isNum(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

        //////////////// To make textbox value decimal ///////////////////////////
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
      
        //////////////////////To make row's textbox visible///////////////////////////////////////////
        function VisibleRow(row, txt, gridview, event) {  //
           
            var rowst = document.getElementById(row)

            var grid = document.getElementById(gridview);
            $('#' + gridview + ' input:text.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");
            });
            $('#' + gridview + ' select.non-trans').each(function () {

                $(this).removeClass("non-trans");
                $(this).addClass("texttransparent");

            });

            var txtGvAcctNo = document.getElementById(txt);
            $(txtGvAcctNo).removeClass("texttransparent");
            $(txtGvAcctNo).addClass("non-trans");

            var txtGvAmount = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvAmount'));
            $(txtGvAmount).removeClass("texttransparent");
            $(txtGvAmount).addClass("non-trans");

            var txtGvPrice = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPrice'));
            $(txtGvPrice).removeClass("texttransparent");
            $(txtGvPrice).addClass("non-trans");

            var txtGvQuan = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvQuan'));
            $(txtGvQuan).removeClass("texttransparent");
            $(txtGvQuan).addClass("non-trans");
            
            var txtGvJob = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvJob'));
            $(txtGvJob).removeClass("texttransparent");
            $(txtGvJob).addClass("non-trans");

            var txtGvItem = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvItem'));
            $(txtGvItem).removeClass("texttransparent");
            $(txtGvItem).addClass("non-trans");

            var txtGvLoc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvLoc'));
            $(txtGvLoc).removeClass("texttransparent");
            $(txtGvLoc).addClass("non-trans");

            var txtGvPhase = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvPhase'));
            $(txtGvPhase).removeClass("texttransparent");
            $(txtGvPhase).addClass("non-trans");
            
            var txtGvDue = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDue'));
            $(txtGvDue).removeClass("texttransparent");
            $(txtGvDue).addClass("non-trans");
            var txtGvDesc = document.getElementById(txt.replace('txtGvAcctNo', 'txtGvDesc'));
            $(txtGvDesc).removeClass("texttransparent");
            $(txtGvDesc).addClass("non-trans");
        }
        function aceItem_itemSelected(sender, e) {
            var hdnItemID = document.getElementById('<%= hdnItemID.ClientID %>');
            hdnItemID.value = e.get_value();
        }
        function SetContextKey()
        {
            var value = $get("<%=ddlBomType.ClientID %>").value;
            $find('<%=AutoCompleteExtender3.ClientID%>').set_contextKey($get("<%=ddlBomType.ClientID %>").value);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add New Purchase Order</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblUserName" runat="server"></asp:Label></li>
                        <li>
                            <asp:Label ID="lblPO" runat="server" Text="PO #" Visible="False"></asp:Label></li>
                        <li>
                            <asp:Label ID="lblPOId" runat="server" Visible="False" Style="font-weight: bold; font-size: 15px;"></asp:Label></li>
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
                        <li>
                            <%--<asp:LinkButton ID="lnkPrint" CssClass="icon-print" runat="server" ToolTip="Print" TabIndex="16" CausesValidation="true" OnClick="lnkPrint_Click"> </asp:LinkButton></li>--%>
                           
                          <ul class="nav navbar-nav pull-right">
                                <li class="dropdown dropdown-user">
                                    <a href="#" title="Reports" data-toggle="dropdown" class="dropdown-toggle icon-print" 
                                        data-hover="dropdown" data-close-others="true" style="padding: 2px 2px 1px 2px !important"></a>
                                    <ul id="dynamicUI" class="dropdown-menu dropdown-menu-default">
                                        <li>
                                            <asp:LinkButton ID="lnk_PO" runat="server" OnClick="lnk_PO_Click">PO Report</asp:LinkButton></li>
                                            <%--<a href='PrintPO.aspx?id=<%=Request.QueryString["id"].ToString() %>'><span>PO Report</span><div style="clear: both;"></div>
                                        </a>--%>
                                        <li style="margin-left: 0px;"><asp:LinkButton ID="lnk_Madden" runat="server" OnClick="lnk_Madden_Click">Madden - PO Report</asp:LinkButton></li>
                                            <%-- <a href='PrintMaddenPO.aspx?id=<%=Request.QueryString["id"].ToString() %>'><span>Madden - PO Report</span><div style="clear: both;"></div>
                                        </a>--%>
                                    </ul>
                                </li>
                            </ul>
                        </li>                        
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
              
                <div class="com-cont">
                    <div class="alert alert-success" runat="server" id="divSuccess" >
                        <button type="button" class="close" data-dismiss="alert">×</button>
                        These month/year period is closed out. You do not have permission to add/update this record.
                    </div>
                    <asp:HiddenField ID="hdnBatch" runat="server" />
                    <asp:HiddenField ID="hdnTransID" runat="server" />
                    <asp:HiddenField ID="hdnStatus" runat="server" />
                    <asp:HiddenField ID="hdnItemJSON" runat="server" />
                    <asp:HiddenField ID="hdnTotal" runat="server" />
                    <asp:HiddenField ID="hdnRowField" runat="server" />
                    <div class="row">
                        <div id="pnlBills" class="pnlBills">
                            <div class="col-lg-12 col-md-12">
                                <div class="col-md-6 col-lg-6">
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
                                                TargetControlID="cvVendor"></asp:ValidatorCalloutExtender>
                                            <asp:Button ID="btnSelectVendor" runat="server" CausesValidation="False" OnClick="btnSelectVendor_Click"
                                                Style="display: none;" Text="Button" />
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" TabIndex="1" MaxLength="75"
                                                    placeholder="Search by vendor"></asp:TextBox>
                                            <asp:HiddenField ID="hdnVendorID" runat="server" />
                                        </div>
                                    </div>
                                      <asp:UpdatePanel ID="updPnlAddress" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                         <div class="form-col">
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
                                            <label>Ship To</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtShipTo" runat="server" Rows="3"  CssClass="form-control" MaxLength="2000" TextMode="MultiLine">
                                            </asp:TextBox>
                                             <asp:RequiredFieldValidator ID="rfvShipTo"
                                                runat="server" ControlToValidate="txtShipTo" Display="None" ErrorMessage="Ship to is Required"  ValidationGroup="po"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceShipTo" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvShipTo" />
                                        </div>
                                    </div>
                                     <div class="form-col">
                                        <div class="fc-label">
                                            <label>Comments</label>
                                        </div>
                                        <div class="fc-input">
                                             <asp:TextBox ID="txtDesc" runat="server" Rows="3"  CssClass="form-control" MaxLength="2000" TextMode="MultiLine">
                                            </asp:TextBox>
                                              <asp:RequiredFieldValidator ID="rfvDesc"
                                                runat="server" ControlToValidate="txtDesc" Display="None" ErrorMessage="Comments is Required"  ValidationGroup="po"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceDesc" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvDesc" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                    <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>Courier</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtShipVia" runat="server" CssClass="form-control" TabIndex="2">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSelectVendor" />
                                </Triggers>
                                </asp:UpdatePanel>
                                     <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>Courier Acct # </label>
                                        </div>
                                        <div class="fc-input" style="padding-top: 5px">
                                            <asp:TextBox ID="txtCourrierAcct" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>Date  </label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TabIndex="2"
                                                MaxLength="15" onkeypress="return false;"></asp:TextBox>
                                            <asp:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txtDate">
                                            </asp:CalendarExtender>
                                            <asp:RequiredFieldValidator ID="rfvDate"  ValidationGroup="po"
                                                    runat="server" ControlToValidate="txtDate" Display="None" ErrorMessage="Date is Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvDate" />
                                             <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate = "txtDate" ValidationGroup="po" 
                                                ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                            </asp:RegularExpressionValidator>
                                            <asp:ValidatorCalloutExtender ID="vceDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                TargetControlID="rfvDate1" />
                                        </div>
                                    </div>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                        <ContentTemplate>
                                        <div class="form-col">
                                            <div class="fc-label" style="width: 90px;">
                                                <label>Due</label>
                                            </div>
                                            <div class="fc-input">
                                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TabIndex="2"
                                                    onkeypress="return false;" ></asp:TextBox>
                                                <asp:CalendarExtender ID="txtDueDate_CalendarExtender" runat="server" Enabled="True"
                                                    TargetControlID="txtDueDate">
                                                </asp:CalendarExtender>
                                                <asp:RequiredFieldValidator ID="rfvDueDate"  ValidationGroup="po"
                                                    runat="server" ControlToValidate="txtDueDate" Display="None" ErrorMessage="Due Date is Required"
                                                    SetFocusOnError="True"></asp:RequiredFieldValidator>
                                                <asp:ValidatorCalloutExtender ID="vceDueDate" runat="server" Enabled="True"
                                                    PopupPosition="Right" TargetControlID="rfvDueDate" />
                                                <asp:RegularExpressionValidator ID="rfvDueDate1" ControlToValidate = "txtDueDate" ValidationGroup="po" 
                                                    ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                    runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                </asp:RegularExpressionValidator>
                                                <asp:ValidatorCalloutExtender ID="vceDueDate1" runat="server" Enabled="True" PopupPosition="Right"
                                                    TargetControlID="rfvDueDate1" />
                                            </div>
                                         </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ddlTerms" />
                                        </Triggers>
                                     </asp:UpdatePanel>
                                    <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>Payment Terms </label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:DropDownList ID="ddlTerms" runat="server" CssClass="form-control" 
                                                 TabIndex="8">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvTerms"  ValidationGroup="po" InitialValue="" 
                                                runat="server" ControlToValidate="ddlTerms" Display="None" ErrorMessage="Please select terms"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vceTerms" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvTerms" />
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>PO Reason Code </label>
                                        </div>
                                        <div class="fc-input" style="padding-top: 5px">
                                            <asp:TextBox ID="txtPOCode" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" 
                                               ></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label" style="width: 90px;">
                                            <label>PO Revision </label>
                                        </div>
                                        <div class="fc-input" style="padding-top: 5px">
                                            <asp:TextBox ID="txtPoRevision" runat="server" CssClass="form-control" TabIndex="2" MaxLength="3"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3">
                                   <div class="form-col">
                                        <div class="fc-label">
                                            <label>PO#</label>
                                        </div>
                                        <div class="fc-input">
                                             <asp:TextBox ID="txtPO" runat="server" CssClass="form-control" TabIndex="2" onkeypress="return isNumberKey(event,this)"
                                                MaxLength="50"></asp:TextBox>
                                             <asp:RequiredFieldValidator ID="rfvPO"  ValidationGroup="po" 
                                                runat="server" ControlToValidate="txtPO" Display="None" ErrorMessage="Please enter PO"
                                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            <asp:ValidatorCalloutExtender ID="vcePO" runat="server" Enabled="True"
                                                PopupPosition="Right" TargetControlID="rfvPO" />

                                        </div>
                                   </div>
                                   <div class="form-col">
                                        <div class="fc-label">
                                            <label>Total</label>
                                        </div>
                                        <div class="fc-input" style="padding-top: 5px">
                                            $<asp:Label ID="lblTotalAmount" runat="server" Font-Size="Small" Style="padding-left: 5px" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-col">
                                        <div class="fc-label">
                                            <label>Created By</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="form-control" TabIndex="2" 
                                                MaxLength="50"></asp:TextBox>
                                        </div>
                                    </div>
                                   <div class="form-col">
                                        <div class="fc-label">
                                            <label>Approved</label>
                                        </div>
                                        <div class="fc-input">
                                            <asp:CheckBox ID="chkApproved" runat="server" />
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
                                    <div class="form-col"
                                        </div>
                                        <div class="fc-input" style="padding-top: 5px">
                                            <asp:TextBox ID="txtFOB" runat="server" CssClass="form-control" TabIndex="2"></asp:TextBox>
                                        </div>
                                    </div>
                                   
                                </div>
                            </div>
                        </div>
                        <div class="clearfix"></div>
                        <div style="padding: 15px 15px 0 15px;" class="ajax__tab_xp ajax__tab_container ajax__tab_default">
                            <asp:UpdatePanel ID="updPnlGLItems" runat="server">
                                <ContentTemplate>
                                    <div class="table-scrollable">
                                        <asp:GridView ID="gvGLItems" runat="server" ShowFooter="true" AutoGenerateColumns="false"
                                            CssClass="table table-bordered table-striped table-condensed flip-content" OnRowCommand="gvGLItems_RowCommand">
                                            <AlternatingRowStyle CssClass="oddrowcolor" />
                                            <FooterStyle CssClass="footer" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Project" ItemStyle-Width="12%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvJob" runat="server" CssClass="form-control texttransparent psearchinput"
                                                            Width="100%" Height="26px" Text='<%# Bind("JobName") %>' Style="font-size:12px;"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnJobID" Value='<%# Bind("JobID") %>' runat="server" />
                                                        <asp:HiddenField ID="hdnIndex" Value='<%# ((GridViewRow) Container).RowIndex %>' runat="server" />
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Type" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvPhase" runat="server" CssClass="form-control texttransparent phsearchinput"
                                                            Width="100%" Height="26px" Text='<%# Bind("Phase") %>' onchange="clearPhase(this)" Style="font-size:12px;"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnPID" Value='<%# Bind("PhaseID") %>' runat="server" />
                                                        <asp:HiddenField ID="hdnTypeId" Value='<%# Bind("TypeID") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvItem" runat="server" CssClass="form-control texttransparent pisearchinput"
                                                            Width="100%" Height="26px" Text='<%# Bind("ItemDesc") %>' style="font-size:12px;"></asp:TextBox>
                                                        <asp:HiddenField ID="hdnItemID" Value='<%# Bind("Inv") %>' runat="server" />
                                                    </ItemTemplate> 
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Item Description" ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvDesc" runat="server" CssClass="form-control texttransparent"
                                                           Width="100%" Height="26px" Text='<%# Bind("fDesc") %>' style="font-size:12px;"></asp:TextBox>
                                                    </ItemTemplate> 
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acct No." ItemStyle-Width="15%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvAcctNo" runat="server" CssClass="form-control texttransparent searchinput"
                                                                Text='<%# Bind("AcctNo") %>' Width="100%" Height="26px" style="font-size:12px;"></asp:TextBox> 
                                                        <asp:HiddenField ID="hdnId" runat="server" Value='<%# Bind("RowID") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnTID" runat="server" Value='<%# Bind("ID") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Bind("Line") %>'></asp:HiddenField>
                                                        <asp:HiddenField ID="hdnRquan" runat="server" Value='<%# Bind("Rquan") %>'></asp:HiddenField>
                                                         <asp:HiddenField ID="hdnAcctID" runat="server" Value='<%# Bind("AcctID") %>'></asp:HiddenField>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quan" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvQuan" runat="server" CssClass="form-control texttransparent" autocomplete="off"
                                                            MaxLength="15" Width="100%" Height="26px" Text='<%# Bind("Quan") %>' 
                                                            Style="text-align: right;font-size:12px;" onchange="CalTotalVal(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvPrice" runat="server" CssClass="form-control texttransparent" autocomplete="off"
                                                            MaxLength="15" Width="100%" Height="26px" Text='<%# Bind("Price") %>' 
                                                            Style="text-align: right;font-size:12px;" onchange="CalTotalVal(this);"></asp:TextBox>
                                                        <%--onkeypress="return isDecimalKey(this,event)"--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="$ Amount" ItemStyle-Width="8%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvAmount" runat="server" CssClass="form-control texttransparent clsAmount" autocomplete="off"
                                                            MaxLength="15" Width="100%" onkeypress="return isDecimalKey(this,event)" Height="26px" Text='<%# Bind("Amount") %>' 
                                                            Style="font-size:12px;text-align: right" onchange="CalculateTotal(this);"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                  <asp:TemplateField HeaderText="Due Date" ItemStyle-Width="7%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvDue" runat="server" CssClass="form-control texttransparent"
                                                            Width="100%" Height="26px" Text='<%#  Eval("Due")==DBNull.Value? "" : String.Format("{0:MM/dd/yyyy}", Eval("Due")) %>' Style="font-size:12px;"
                                                            ></asp:TextBox>
                                                         <asp:CalendarExtender ID="txtGvDue_CalendarExtender" runat="server" Enabled="True" 
                                                             TargetControlID="txtGvDue">
                                                         </asp:CalendarExtender>
                                                        <asp:RegularExpressionValidator ID="revGvDue" ControlToValidate = "txtGvDue" ValidationGroup="po" 
                                                            ValidationExpression = "^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                                            runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                                                        </asp:RegularExpressionValidator>
                                                        <asp:ValidatorCalloutExtender ID="vceGvDue" runat="server" Enabled="True" PopupPosition="Right"
                                                            TargetControlID="revGvDue" />
                                                    </ItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Location Name" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtGvLoc" runat="server" CssClass="form-control texttransparent jsearchinput"
                                                            Width="100%" Height="26px" Text='<%# Bind("Loc") %>' onchange="clearJob(this)" Style="font-size:12px;"></asp:TextBox>
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
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnAddNewLines" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div style="padding-left: 15px">
                            <asp:Button ID="btnAddNewLines" runat="server" CausesValidation="false" OnClientClick="itemJSON();" 
                                OnClick="btnAddNewLines_Click" Text="Add New Lines" CssClass="btn btn-primary"  />
                        </div>
                    </div>
                    <div class="col-lg-12 col-md-12" style="padding-top: 10px;">
                        <asp:Label ID="lblTC" runat="server"></asp:Label>
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

    </div>
        <a href="#" runat="server" value="Add New" id="btnAddNew"></a>
        <asp:ModalPopupExtender ID="mpeBomItem" BackgroundCssClass="ModalPopupBG" BehaviorID="mpeBomItem" 
            runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
            TargetControlID="btnAddNew" PopupControlID="pnlTemplate" 
            Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
        </asp:ModalPopupExtender>
        
        <div class="popup_Buttons" style="display: none">
            <input id="btnOkay" value="Done" type="button" />
            <input id="btnCancel" value="Cancel" type="button" />
        </div>
        
        <div id="pnlTemplate" class="table-subcategory" style="display: none;width:560px;height:480px; border:groove; border-width:thin; border-color:blue;">
            <div class="popup_Container">
                <div class="popup_Body">
                    <div class="model-popup-body" style="padding-bottom: 24px;height: 48px;">
                        <div class="col-lg-12 col-md-12">
                            <div class="pc-title">
                                <asp:Label CssClass="title_text" Style="float: left" ID="lblBomItem" runat="server"> Add BOM Item </asp:Label>
                                <div style="float: right;">
                                    <ul class="lnklist-header">
                                        <li>
                                            <asp:LinkButton CssClass="icon-save" ID="lbtnItemSubmit" runat="server" ValidationGroup="item" ToolTip="Save"
                                                TabIndex="38" OnClick="lbtnItemSubmit_Click"></asp:LinkButton>
                                        </li>
                                        <li>
                                            <a id="lbtnClose" style="color: white" onclick="cancel();" class="icon-closed close_button_Form"> </a>
                                        </li>
                                    </ul> 
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-12 col-md-12" style="padding-left: 0px;padding-right: 0px;">
                    <div class="com-cont">
                        <div class="row">
                            <div class="col-lg-12 col-md-12">
                                <asp:HiddenField ID="hdnJobId" runat="server" />
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Op Sequenceass="form-col">
                                    <div class="fc-label">
                                        <label>Op Sequence</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtOpSeq" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Op Sequence"
                                            MaxLength="50" onkeyup="EmptyValue(this);"></asp:TextBox>
                                      
                                        <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtOpSeq"
                                            EnableCaching="False" ServiceMethod="GetCode" UseContextKey="True" MinimumPrefixLength="0"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListElementID="lstAcct"
                                            ID="AutoCompleteExtender2" DelimiterCharacters="" CompletionInterval="250">
                                        </asp:AutoCompleteExtender>
                                        <div id="lstAcct"></div>
                                        <asp:RequiredFieldValidator ID="rfvOpSeq"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtOpSeq" Display="None" ErrorMessage="Please enter Op Sequence"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceOpSeq" runat="server" Enabled="True"
                                            PopupPosition="TopLeft" TargetControlID="rfvOpSeq" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Type</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:DropDownList ID="ddlBomType" runat="server" DataTextField="Type" CssClass="form-control" onchange="SetContextKey()"
                                            ></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvType"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="ddlBomType" Display="None" ErrorMessage="Please select type"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceType" runat="server" Enabled="True"
                                            PopupPosition="BottomLeft" TargetControlID="rfvType" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Item</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtItem" runat="server" CssClass="form-control" TabIndex="2" placeholder="Select Item" onkeyup="SetContextKey()"
                                            MaxLength="50"></asp:TextBox>
                                        <asp:HiddenField ID="hdnItemID" runat="server" />
                                        <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtItem"
                                            EnableCaching="False" ServiceMethod="GetItems" UseContextKey="True" MinimumPrefixLength="0"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListElementID="lstItem" 
                                            OnClientItemSelected="aceItem_itemSelected"
                                            ID="AutoCompleteExtender3" DelimiterCharacters="" CompletionInterval="250">
                                        </asp:AutoCompleteExtender>
                                        <div id="lstItem"></div>
                                        <asp:RequiredFieldValidator ID="rfvItem"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtItem" Display="None" ErrorMessage="Please select item"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceItem" runat="server" Enabled="True"
                                            PopupPosition="TopLeft" TargetControlID="rfvItem" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Description</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtJobDesc" runat="server" CssClass="form-control" TabIndex="2" 
                                            MaxLength="50"></asp:TextBox>    
                                        <asp:RequiredFieldValidator ID="rfvJobDesc"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtJobDesc" Display="None" ErrorMessage="Please enter description"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceJobDesc" runat="server" Enabled="True"
                                            PopupPosition="BottomLeft" TargetControlID="rfvJobDesc" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Qty Required</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" TabIndex="2" 
                                            MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>    
                                        <asp:RequiredFieldValidator ID="rfvQty"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtQty" Display="None" ErrorMessage="Please enter quantity required"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceQty" runat="server" Enabled="True"
                                            PopupPosition="BottomLeft" TargetControlID="rfvQty" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>U/M</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtUM" runat="server" CssClass="form-control" TabIndex="2" 
                                            MaxLength="50" placeholder="Select U/M"></asp:TextBox>
                                        <asp:AutoCompleteExtender runat="server" Enabled="True" ServicePath="" TargetControlID="txtUM"
                                            EnableCaching="False" ServiceMethod="GetUM" UseContextKey="false" MinimumPrefixLength="0"
                                            CompletionListCssClass="autocomplete_completionListElement"
                                            CompletionListItemCssClass="autocomplete_listItem"
                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                            CompletionListElementID="lstUM"
                                            ID="AutoCompleteExtender1" DelimiterCharacters="" CompletionInterval="250">
                                        </asp:AutoCompleteExtender>
                                        <div id="lstUM"></div>
                                        <asp:RequiredFieldValidator ID="rfvUM"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtUM" Display="None" ErrorMessage="Please select U/M"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceUM" runat="server" Enabled="True"
                                            PopupPosition="TopLeft" TargetControlID="rfvUM" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Budget Unit</label>
                                    </div>
                                    <div class="fc-input">
                                        <asp:TextBox ID="txtBudgetUnit" runat="server" CssClass="form-control" TabIndex="2" 
                                            MaxLength="50" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>    
                                        <asp:RequiredFieldValidator ID="rfvBudgetUnit"  ValidationGroup="item" 
                                            runat="server" ControlToValidate="txtBudgetUnit" Display="None" ErrorMessage="Please enter Budget Unit"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        <asp:ValidatorCalloutExtender ID="vceBudgetUnit" runat="server" Enabled="True"
                                            PopupPosition="BottomLeft" TargetControlID="rfvBudgetUnit" />
                                    </div>
                                </div>
                                <div class="form-col">
                                    <div class="fc-label">
                                        <label>Budget Ext</label>
                                    </div>
                                    <div class="fc-input" style="padding-top:5px;">
                                        <asp:Label ID="lblBudgetExt" runat="server" Text="0.00"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
           
            </label>

           
</asp:Content>

