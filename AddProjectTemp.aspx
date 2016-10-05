<%@ Page Language="C#" MasterPageFile="~/HomeMaster.master" AutoEventWireup="true"
    CodeFile="AddProjectTemp.aspx.cs" Inherits="AddProjectTemp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="uc_AccountSearch.ascx" TagName="uc_AccountSearch" TagPrefix="uc1" %>
<%@ Register Src="uc_gvChecklist.ascx" TagName="uc_gvChecklist" TagPrefix="uc2" %>

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
       

        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
            }
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
            function isInt(value) {
                var x = parseFloat(value);
                return !isNaN(value) && (x | 0) === x;
            }
            //Function to Show ModalPopUp
            function Showpopup() {
                $find('mpeAddCode').show();
            }

            $(document).ready(function () {
          
        });

            function CalculatePercentage(gridview) {

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
            function showDecimalVal(obj)
            {  
                if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                    document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
                }
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

            function itemJSON() {
            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);

            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

        }

    </script>
     
    <script type="text/javascript">
        <%--$(document).ready(function () {
            tc = $find("<%=TabContainer2.ClientID%>");
            tc.set_activeTabIndex(0);
        })--%>

        function pageLoad(sender, args) {

            var query = "";
            function dtaa() {
                this.prefixText = null;
                this.con = null;
                this.custID = null;
            }

            $(document).ready(function () {
                /////////////////////////////////// Wage ////////////////////////////////////

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
                   
                    var ula = ul;
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

                function dtaa() {
                    this.prefixText = null;
                    this.con = null;
                    this.custID = null;
                }
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
                        <span>Project Manager</span>
                        <i class="fa fa-angle-right"></i>
                    </li>
                    <li>
                         <a href="<%=ResolveUrl("~/ProjectTemplate.aspx") %>">Project Template</a>                        
                        <i class="fa fa-angle-right"></i>
                    </li>
                    <li>
                        <span>Add Project Template</span>
                    </li>
                </ul>--%>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title">
                    <ul class="lnklist-header">
                        <li>
                            <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Add Project Template</asp:Label></li>
                        <li>
                            <asp:Label CssClass="title_text_Name" ID="lblProjectNo" runat="server"></asp:Label></li>
                      
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
                            <asp:LinkButton ID="lnkSaveTemplate" runat="server" CssClass="icon-save" ToolTip="Save"
                                OnClick="lnkSaveTemplate_Click" OnClientClick="itemJSON();"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" CssClass="icon-closed" ToolTip="Close"
                                OnClick="lnkCloseTemplate_Click"></asp:LinkButton></li>
                    </ul>
                </div>
            </div>

            <!-- edit-tab start -->

            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                      <div class="col-lg-12 col-md-12">
                        <div class="com-cont">
                            <asp:HiddenField ID="hdnMilestone" runat="server" />
                            <asp:HiddenField ID="hdnItemJSON" runat="server" />
                            <asp:Panel ID="pnlREPT" runat="server">
                                <div class="col-md-12 col-md-12">
                                    <div class="col-md-6 col-md-6">
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    Template Name
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server"
                                                        ControlToValidate="txtREPdesc" Display="None" ErrorMessage="Name Required" SetFocusOnError="True">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender
                                                        ID="RequiredFieldValidator9_ValidatorCalloutExtender" runat="server" Enabled="True"
                                                        TargetControlID="RequiredFieldValidator9">
                                                    </asp:ValidatorCalloutExtender>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtREPdesc" runat="server" AutoCompleteType="None" CssClass="form-control"
                                                        MaxLength="255"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Department</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlJobType" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlJobType_SelectedIndexChanged"
                                                        AutoPostBack="true"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvJobType" runat="server" InitialValue="Select Department"
                                                        ControlToValidate="ddlJobType" Display="None" ErrorMessage="Department Required" SetFocusOnError="True">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:ValidatorCalloutExtender ID="vceJobType" runat="server" Enabled="True"
                                                        TargetControlID="rfvJobType"></asp:ValidatorCalloutExtender>
                                                </div>
                                            </div>
                                        </div>
                                        <%-- <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Department Qty</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtDepartmentQty" runat="server" AutoCompleteType="None" CssClass="form-control"
                                                        MaxLength="10"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>--%>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Status</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="0">Active</asp:ListItem>
                                                        <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                    <div class="col-md-6 col-lg-6">
                                        <div class="form-group" id="tempRev" runat="server">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Template Rev</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtTempRev" runat="server" AutoCompleteType="None" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group" id="tempRemarks" runat="server">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Template Remarks</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtTempRemarks" runat="server" AutoCompleteType="None" CssClass="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Alert Type</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:DropDownList ID="ddlAlertType" runat="server" CssClass="form-control">
                                                        <asp:ListItem Value="Select Type">Select Type</asp:ListItem>
                                                        <asp:ListItem Value="0">Email</asp:ListItem>
                                                        <asp:ListItem Value="1">Text</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6 col-lg-6" style="padding-right: 0px;">
                                            <div class="form-group">
                                                <div class="fc-input" style="text-align: left; padding-left: 120px;">
                                                    <asp:CheckBox ID="chkAlert" Text="Alert Manager" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="col-md-6 col-lg-6" style="padding-right: 0px;">
                                            <div class="form-group">
                                                <div class="fc-input" style="text-align: left; padding-left: 110px;">
                                                    <asp:CheckBox ID="chkMilestone" runat="server" Text="Milestone Manager" />
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                                <div class="clearfix"></div>
                                <div class="col-md-12 col-md-12" style="padding-top: 10px;">
                                    <div class="col-md-12 col-md-12">
                                        <div class="form-group">
                                            <div class="form-col">
                                                <div class="fc-label">
                                                    <label>Remarks</label>
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="txtREPremarks" runat="server" CssClass="form-control"
                                                        Height="50px" MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>
                            <div class="clearfix"></div>
                        </div>
                     </div>
                    <div class="table-scrollable" style="border: none">
                        <asp:UpdatePanel ID="updPnlProjTemp" runat="server">
                            <ContentTemplate>
                                <asp:TabContainer ID="TabContainer2" runat="server" ActiveTabIndex="0"> <%--ActiveTabIndex="0"--%>
                                <%--<asp:TabPanel ID="tbpnlCust" runat="server" HeaderText="Project Header">
                                        <HeaderTemplate>
                                            Header
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                        </ContentTemplate>
                                    </asp:TabPanel>--%>
                                    <asp:TabPanel ID="tbpnlFinance" runat="server" HeaderText="Project Finance">
                                        <HeaderTemplate>
                                            Finance 
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="col-lg-12 col-md-12">
                                                <div class="com-cont">
                                                    <div class="col-md-6 col-md-6">
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Expense GL
                                                                </div>
                                                                <div class="fc-input">
                                                                   <%-- <asp:TextBox ID="txtInvExpGL" runat="server" AutoCompleteType="None" CssClass="form-control searchinputloc" placeholder="Search by account # and name"
                                                                        MaxLength="255"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnInvExpGLID" runat="server" />--%>
                                                                     <uc1:uc_AccountSearch ID="uc_InvExpGL" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="form-col">
                                                                <div class="fc-label">
                                                                    Interest GL
                                                                </div>
                                                                <div class="fc-input">
                                                                   <%-- <asp:TextBox ID="txtInterestGL" runat="server" AutoCompleteType="None" CssClass="form-control" placeholder="Search by account # and name"
                                                                        MaxLength="255"></asp:TextBox>
                                                                    <asp:HiddenField ID="hdnInterestGLID" runat="server" />--%>
                                                                       <uc1:uc_AccountSearch ID="uc_InterestGL" runat="server" />
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
                                                    <div class="col-md-6 col-lg-6">
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
                                                                    <%-- Contract Type--%> Service Type
                                                                </div>
                                                                <div class="fc-input">
                                                                    <asp:DropDownList ID="ddlContractType" runat="server" CssClass="form-control"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-7 col-lg-7" style="padding-right: 0px;">
                                                            <div class="form-group">
                                                                <div class="form-col" style="text-align: left; padding-left: 120px;">
                                                                    <asp:CheckBox ID="chkChargeInt" Text="Charge Interest" runat="server" />
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="form-col" style="text-align: left; padding-left: 120px;">
                                                                    <asp:CheckBox ID="chkInvoicing" Text="Close after Invoicing" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-5 col-lg-5" style="padding-right: 0px;">
                                                            <div class="form-group">
                                                                <div class="form-col" style="text-align: left; padding-left: 120px;">
                                                                    <asp:CheckBox ID="chkChargeable" Text="Chargeable" runat="server" />
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbpnlBOM" runat="server" HeaderText="Project BOM">
                                        <HeaderTemplate>
                                            BOM
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="table-scrollable" style="height: 400px;overflow-y: auto;border: none">
                                                <div class="col-lg-12 col-md-12">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                            <ContentTemplate>
                                                                <asp:GridView ID="gvBOM" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                                        PageSize="20" ShowFooter="true" OnRowDataBound="gvBOM_RowDataBound"
                                                                        OnRowCommand="gvBOM_RowCommand">
                                                                        <AlternatingRowStyle CssClass="oddrowcolor" />
                                                                        <FooterStyle CssClass="footer" />
                                                                        <RowStyle CssClass="evenrowcolor" />
                                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                                        <Columns>
                                                                            <asp:TemplateField ItemStyle-Width="1%">
                                                                                <HeaderTemplate>
                                                                                    <a id="Button2" class="delButton" onclick="DelRow('<%=gvBOM.ClientID%>');"
                                                                                        style="cursor: pointer;">
                                                                                        <img src="images/menu_delete.png" title="Delete" width="18px" /></a>
                                                                                </HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <FooterTemplate>
                                                                                    <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddBOMItem" CausesValidation="False"
                                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                        ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                                                </FooterTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Line No." ItemStyle-Width="1%">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                                    <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
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
                                                                                    <asp:DropDownList ID="ddlItem" runat="server"  Width="100%"  OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true">
                                                                                        <%--SelectedValue='<%# Eval("BItem") == DBNull.Value ? "Select Item" : Eval("BItem") %>'--%>
                                                                                    </asp:DropDownList>
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
                                                                                    <asp:TextBox ID="txtQtyReq" runat="server" Text='<%# Eval("QtyReq","{0:0.00}") %>' style="text-align:right;" 
                                                                                        Width="100%" onchange="calBudgetExt(this)" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="U/M" ItemStyle-Width="5%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtUM" runat="server" Text='<%# Eval("UM") %>' Width="100%"></asp:TextBox>
                                                                                    <asp:HiddenField ID="hdnUMID" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                           <%-- <asp:TemplateField HeaderText="Scrap %">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtScrapFactor" runat="server" Text='<%# Eval("ScrapFact","{0:0.00}") %>' Width="90px" onchange="onchangeScrapFact(this)" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>--%>
                                                                            <asp:TemplateField HeaderText="Budget Unit $" ItemStyle-Width="5%">
                                                                                <ItemTemplate>
                                                                                    <asp:TextBox ID="txtBudgetUnit" runat="server" Text='<%# Eval("BudgetUnit","{0:0.00}") %>' Width="100%" 
                                                                                        style="text-align:right;" onchange="calBudgetExt(this)" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Budget Ext $" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBudgetExt" runat="server" Text='<%# Eval("BudgetExt","{0:0.00}") %>'></asp:Label>
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
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbpnlMilestone" runat="server" HeaderText="Project Milestones">
                                        <HeaderTemplate>
                                            Milestones
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="table-scrollable" style="height: 400px;overflow-y: auto;border: none">
                                            <div class="col-lg-12 col-md-12">
                                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
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
                                                                            <a id="Button2" class="delButton" onclick="DelRow('<%=gvMilestones.ClientID%>');"
                                                                                style="cursor: pointer;">
                                                                                <img src="images/menu_delete.png" title="Delete" width="18px" /></a>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddMilestoneItem" CausesValidation="False"
                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                                ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSON();" />
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Line No." ItemStyle-Width="1%">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                            <asp:HiddenField ID="hdnLine" runat="server" Value='<%# Eval("Line") %>'></asp:HiddenField>
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
                                                                            <asp:TextBox ID="txtSType" runat="server" Text='<%# Eval("Department") %>' Width="100%"
                                                                                placeholder="Select Function"></asp:TextBox>
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
                                                                            <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("Amount") %>' onkeypress="return isDecimalKey(this,event)" onchange="showDecimalVal(this)"
                                                                               style="text-align:right;" Width="100%"></asp:TextBox>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Required by" ItemStyle-Width="5%">
                                                                        <ItemTemplate>
                                                                            <asp:TextBox ID="txtRequiredBy" runat="server" Text='<%# Eval("RequiredBy")!=DBNull.Value? (!(Eval("RequiredBy").Equals(DateTime.MinValue)) ? (String.Format("{0:MM/dd/yyyy}", Eval("RequiredBy"))) : "" ) : "" %>'
                                                                                Width="100%"></asp:TextBox>
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
                                            </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:TabPanel>
                                    <asp:TabPanel ID="tbpnlCustom" runat="server" HeaderText="Project Custom">
                                        <HeaderTemplate>
                                            Custom
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="col-lg-12 col-md-12 table-scrollable" style="border: none">
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                               <ContentTemplate>
                                               <asp:GridView ID="gvCustom" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        PageSize="20" ShowFooter="true" Width="100%" OnRowCommand="gvCustom_RowCommand"
                                                        OnRowDataBound="gvCustom_RowDataBound">
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="10px" HeaderStyle-Width="10px" FooterStyle-Width="10px">
                                                                <HeaderTemplate>
                                                                    <asp:ImageButton ID="ibtnDeleteCItem" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Custom field.')"
                                                                        CausesValidation="false" ToolTip="Delete" ImageUrl="images/menu_delete.png" runat="server"
                                                                        OnClick="ibtnDeleteCItem_Click" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Visible="false"></asp:Label>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="lnkAddnewRow" runat="server" CausesValidation="False" ImageUrl="images/add.png"
                                                                        Width="20px" ToolTip="Add New Row" OnClick="lnkAddnewRow_Click" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Label"
                                                                FooterStyle-VerticalAlign="Middle" ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDesc" runat="server" Text='<%# Eval("Label") %>' Width="220px" style='width:200px!important;'
                                                                        CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvDescT" runat="server" ControlToValidate="lblDesc"
                                                                        Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="ctempl"></asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                                <FooterStyle VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Tab"
                                                                FooterStyle-VerticalAlign="Middle" ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTab" runat="server" Width="100%" CssClass="form-control input-sm input-small" style='width:210px!important;'
                                                                         DataValueField="ID" DataSource='<%#dtTab%>' DataTextField="tabname" 
                                                                        SelectedValue='<%# Eval("tblTabID") == DBNull.Value ? 0 : Eval("tblTabID") %>'>
                                                                       
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="Format" Visible="true" HeaderStyle-Width="490px" ItemStyle-Width="490px" FooterStyle-Width="490px">
                                                                <ItemTemplate>
                                                                    <table style="border-spacing:0;padding:0;">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlFormat" runat="server" Width="100px" AutoPostBack="true" CssClass="form-control input-sm input-small" 
                                                                                    OnSelectedIndexChanged="ddlFormat_SelectedIndexChanged"  DataValueField="value" DataSource='<%#dtFormat%>' DataTextField="format" 
                                                                                    SelectedValue='<%# Eval("format") == DBNull.Value ? 0 : Eval("format") %>'>
                                                                                 </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Panel ID="pnlCustomValue" runat="server" Visible="false">
                                                                                    <table style="border-spacing:0px;padding:0px;">
                                                                                        <tr>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:DropDownList ID="ddlCustomValue" Width="100px" runat="server" AutoPostBack="true" 
                                                                                                    OnSelectedIndexChanged="ddlCustomValue_SelectedIndexChanged" CssClass="form-control input-sm input-small">
                                                                                                    <asp:ListItem Value="">--Add New--</asp:ListItem>
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <asp:TextBox ID="txtCustomValue" Width="100px" runat="server" CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                                <table style="border-spacing:0;padding:0;">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:LinkButton ID="lnkAddCustomValue" CommandName="AddCustomValue" CommandArgument='<%# Container.DataItemIndex %>'  
                                                                                                                runat="server" CausesValidation="False">Add</asp:LinkButton>
                                                                                                            <asp:LinkButton ID="lnkUpdateCustomValue" CommandName="UpdateCustomValue" Visible="false" runat="server" 
                                                                                                                CommandArgument='<%# Container.DataItemIndex %>'  CausesValidation="False">Update</asp:LinkButton>
                                                                                                        </td>
                                                                                                        <td>
                                                                                                            <asp:LinkButton ID="lnkDelCustomValue" CommandName="DeleteCustomValue"  CommandArgument='<%# Container.DataItemIndex %>' 
                                                                                                                CausesValidation="False" Visible="false" runat="server">
                                                                                                            <img height="12px" alt="Delete" title="Delete" src="images/delete-grid.png" /></asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                        <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>
                                                                    </div>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlJobType" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                     </asp:TabPanel>
                                    <%--#cc01:Added by rajesh start --%>
                                    <asp:TabPanel ID="tbpnlCustom2" runat="server" HeaderText="Project Custom">
                                        <HeaderTemplate>
                                            Estimate
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="col-lg-12 col-md-12 table-scrollable" style="border: none">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                               <ContentTemplate>
                                               <asp:GridView ID="gvCustom2" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                                                        PageSize="20" ShowFooter="true" Width="100%" 
                                                        OnRowDataBound="gvCustom2_RowDataBound">
                                                        <FooterStyle CssClass="footer" />
                                                        <RowStyle CssClass="evenrowcolor" />
                                                        <SelectedRowStyle CssClass="selectedrowcolor" />
                                                        <Columns>
                                                            <asp:TemplateField ItemStyle-Width="10px" HeaderStyle-Width="10px" FooterStyle-Width="10px">
                                                                <HeaderTemplate>
                                                                    <asp:ImageButton ID="ibtnDeleteCItem2" OnClientClick="return confirm('Are you sure you want to delete the items? This will delete the items from Custom field.')"
                                                                        CausesValidation="false" ToolTip="Delete" ImageUrl="images/menu_delete.png" runat="server"
                                                                        OnClick="ibtnDeleteCItem2_Click" />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIndex" Visible="false" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                                                   <%-- <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblLine" runat="server" Text='<%# Eval("Line") %>' Visible="false"></asp:Label>--%>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="lnkAddnewRow1" runat="server" CausesValidation="False" ImageUrl="images/add.png"
                                                                        Width="20px" ToolTip="Add New Row" OnClick="lnkAddnewRow1_Click" />
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Label"
                                                                FooterStyle-VerticalAlign="Middle" ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="170px">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="lblDesc" runat="server"  Width="220px" style='width:200px!important;'
                                                                        CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvDescT" runat="server" ControlToValidate="lblDesc"
                                                                        Display="Dynamic" ErrorMessage="***Required***" SetFocusOnError="True" ValidationGroup="ctempl"></asp:RequiredFieldValidator>
                                                                </ItemTemplate>
                                                                <FooterStyle VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Calculations"
                                                                FooterStyle-VerticalAlign="Middle" ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="200px">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlTab1" runat="server" Width="100%" CssClass="form-control input-sm input-small" style='width:210px!important;'>
                                                                        
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Percentage"
                                                                FooterStyle-VerticalAlign="Middle" ItemStyle-Width="200px" HeaderStyle-Width="200px" FooterStyle-Width="200px">
                                                                <ItemTemplate>
                                                                     <asp:TextBox ID="txtPercentage" runat="server"  Width="220px" style='width:200px!important;'
                                                                        CssClass="form-control input-sm input-small"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" Visible="true" HeaderStyle-Width="490px" ItemStyle-Width="490px" FooterStyle-Width="490px">
                                                                <ItemTemplate>
                                                                   <asp:TextBox ID="txtAmount" runat="server"  Width="220px" style='width:200px!important;'
                                                                        CssClass="form-control input-sm input-small"></asp:TextBox> 
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <div style="float: left; margin-top: 5px; margin-left: 10px;">
                                                                        <asp:Label ID="lblRowCount" runat="server" Text=""></asp:Label>
                                                                    </div>
                                                                </FooterTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlJobType" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                     </asp:TabPanel>
                                    <%--#cc01:Added by rajesh end --%>
                                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Project Checklist" Visible="false">
                                        <HeaderTemplate>
                                            Checklist
                                        </HeaderTemplate>
                                        <ContentTemplate>
                                            <div class="col-lg-12 col-md-12">
                                             <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:PlaceHolder ID="PlaceHolder1" runat="server">

                                                    </asp:PlaceHolder>
                                                </ContentTemplate>
                                             </asp:UpdatePanel>
                                            </div>
                                        </ContentTemplate>
                                     </asp:TabPanel>
                                </asp:TabContainer>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <a href="#" runat="server" value="Add New" id="btnAddNew" onclick="Javascript:storeAcctNum();"></a><%--onclick="getAccount();"--%>

            <asp:ModalPopupExtender ID="mpeAddCode" BackgroundCssClass="ModalPopupBG"
                runat="server" CancelControlID="btnCancel" OkControlID="btnOkay"
                TargetControlID="btnAddNew"
                PopupControlID="pnlAddCode" Drag="true" PopupDragHandleControlID="PopupHeader" OnOkScript="ReloadPage();">
            </asp:ModalPopupExtender>
            <div class="popup_Buttons" style="display: none">
                <input id="btnOkay" value="Done" type="button" />
                <input id="btnCancel" value="Cancel" type="button" />
            </div>

            <div id="pnlAddCode" class="table-subcategory" style="display: none;">
                <div class="popup_Container">
                    <div class="popup_Body">
                        <div class="model-popup-body" style="padding-bottom: 24px;">
                            <asp:Label CssClass="title_text" Style="float: left" ID="lblAddCode" runat="server">Add Code</asp:Label>

                            <div style="float: right;">
                                <a class="close_button_Form" id="lbtnClose" style="color: white" onclick="cancel();">Close </a>
                                <asp:LinkButton CssClass="save_button" ID="lbtnCodeSubmit" Style="color: white" runat="server" OnClick="lbtnCodeSubmit_Click"
                                    TabIndex="38" CausesValidation="true" ValidationGroup="valSubAccount"> Save </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <div class="divInnerBody">
                        Temporary Model popup
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>