<%@ Page Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true"
    CodeFile="AddEstimate.aspx.cs" Inherits="AddEstimate" Title="Estimate - Mobile Office Manager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">




    <script type="text/javascript">

        function pageLoad() {

            InitializeGrids('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
            InitializeGrids('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');

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


            AutoCompleteText('CustomerAuto.asmx/getTaskContacts', '<%= txtCont.ClientID %>', '<%= hdnROLId.ClientID %>', null, null, '<%= hdnOwnerID.ClientID %>')

            $("#<%= txtCont.ClientID %>").keyup(function (e) {
                var hdnId = document.getElementById('<%= hdnROLId.ClientID %>');
                if ((e.which >= 46 && e.which <= 90) || (e.which >= 96 && e.which <= 105) || (e.which >= 186 && e.which <= 222) || e.which == 8) {
                    hdnId.value = '';
                }
                if (e.value == '') {
                    hdnId.value = '';
                }
            });

        }

        function InitializeGrids(Gridview) {

            $("#" + Gridview).on('click', 'a.addButton', function () {
                var $tr = $(this).closest('table').find('tr').eq(1);
                var $clone = $tr.clone();
                $clone.find('input:text').val('');
                $clone.insertAfter($tr.closest('table').find('tr').eq($tr.closest('table').find('tr').length - 2));
            });

            CalculatePercentage(Gridview);

            //            var rowone = $("#" + Gridview).find('tr').eq(1);
            //            $("input", rowone).each(function() {
            //                $(this).blur();
            //            });
        }

        //        function CalculatePercentage(Gridview) {
        //            $("#" + Gridview).find('tr:not(:first, :last)').each(function() {
        //                var ddlMeasure = $(this).find('select[id*=ddlMeasure]')
        //                //                Measure(ddlMeasure);
        //                //            });
        //                //        }

        //                //        function Measure(ddlMeasure) {
        //                var txtQuan = $(ddlMeasure).closest('tr').find('input[id*=txtQuan]');
        //                var txtCost = $(ddlMeasure).closest('tr').find('input[id*=txtUnitCost]');
        //                var txtTotal = $(ddlMeasure).closest('tr').find('input[id*=txtTotal]');
        //                
        //                if ($(ddlMeasure).val() == "2") {
        //                    txtQuan.val("1");
        //                    txtQuan.attr("onfocus", "this.blur();");
        //                    txtQuan.attr("class", "texttransparent");
        //                    if (!isNaN(parseFloat(txtCost.val())))
        //                        txtTotal.val(parseFloat(txtQuan.val()) * parseFloat(txtCost.val()));
        //                }
        //                else {
        //                    txtQuan.removeAttr("onfocus");
        //                    txtQuan.removeAttr("class");
        //                }
        //                
        //                if ($(ddlMeasure).val() == "3") {

        //                    var total = 0;
        //                    var txtGtotal = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('input[id*=txtGrandTotalT]').val();
        //                    if (!isNaN(parseFloat(txtGtotal))) {
        //                        total = parseFloat(txtGtotal);
        //                    }
        //                    //                $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:not(:first, :last)').each(function() {
        //                    //                    var $tr = $(this);
        //                    //                    var amt = $tr.find('input[id*=txtGrandTotal]').val();
        //                    //                    var ddlMeasuretr = $tr.find('select[id*=ddlMeasure]').val();
        //                    //                    if (ddlMeasuretr != '3') {
        //                    //                        if (!isNaN(parseFloat(amt))) {
        //                    //                            total += parseFloat(amt);
        //                    //                        }
        //                    //                    }
        //                    //                });
        //                    txtCost.val(total);
        //                    txtCost.attr("onfocus", "this.blur();");
        //                    txtCost.attr("class", "texttransparent");
        //                    if (!isNaN(parseFloat(txtUnit.val())))
        //                        txtTotal.val((total * parseFloat(txtQuan.val())) / 100);
        //                }
        //                else {
        //                    txtCost.removeAttr("onfocus");
        //                    txtCost.removeAttr("class");
        //                }

        //                Calculate('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
        //                Calculate('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');
        //            });
        //        }



        //function Calculate(Gridview) {
        function CalculatePercentage(Gridview) {

            var tquan = 0;
            var tunit = 0;
            var ttotal = 0;

            $("#" + Gridview).find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                //                var quan = $tr.find('input[id*=txtQuan]').val();
                var txtquan = $tr.find('input[id*=txtQuan]');
                //                var cost = $tr.find('input[id*=txtUnitCost]').val();
                var txtcost = $tr.find('input[id*=txtUnitCost]');
                var ddlMeasure = $tr.find('select[id*=ddlMeasure]').val();
                var ddlCurrency = $tr.find('select[id*=ddlCurrency]').val();
                var hdnExchange = $('#<%= hdnExchangeRate.ClientID %>').val();
                var total = 0;

                //                if (!isNaN(parseFloat(quan))) {
                //                    tquan += parseFloat(quan);
                //                }
                //                if (!isNaN(parseFloat(cost))) {
                //                    tunit += parseFloat(cost);
                //                }

                //                if (!isNaN(parseFloat(quan))) {
                //                    if (!isNaN(parseFloat(cost))) {


                if (ddlMeasure == '1') {
                    if (!isNaN(parseFloat(txtquan.val()))) {
                        if (!isNaN(parseFloat(txtcost.val()))) {
                            if (ddlCurrency == 'CDN') {
                                total = parseFloat(txtquan.val()) * parseFloat(txtcost.val()) * parseFloat(hdnExchange);
                            } else {
                                total = parseFloat(txtquan.val()) * parseFloat(txtcost.val());
                            }
                        }
                    }
                }

                if (ddlMeasure == '2') {
                    txtquan.val("1");
                    txtquan.attr("onfocus", "this.blur();");
                    txtquan.attr("class", "texttransparent");

                    if (!isNaN(parseFloat(txtquan.val()))) {
                        if (!isNaN(parseFloat(txtcost.val()))) {
                            if (ddlCurrency == 'CDN') {
                                total = parseFloat(txtquan.val()) * parseFloat(txtcost.val()) * parseFloat(hdnExchange);
                            } else {
                                total = parseFloat(txtquan.val()) * parseFloat(txtcost.val());
                            }
                        }
                    }
                }
                else {
                    txtquan.removeAttr("onfocus");
                    txtquan.removeAttr("class");
                }

                if (ddlMeasure == '3') {

                    var txtGtotal = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('input[id*=txtGrandTotalT]').val();
                    if (!isNaN(parseFloat(txtGtotal))) {
                        txtcost.val(parseFloat(txtGtotal));
                    }
                    txtcost.attr("onfocus", "this.blur();");
                    txtcost.attr("class", "texttransparent");

                    if (!isNaN(parseFloat(txtquan.val()))) {
                        if (!isNaN(parseFloat(txtcost.val()))) {
                            if (ddlCurrency == 'CDN') {
                                total = ((parseFloat(txtquan.val()) * parseFloat(txtcost.val())) / 100) * parseFloat(hdnExchange);
                            }
                            else {
                                total = (parseFloat(txtquan.val()) * parseFloat(txtcost.val())) / 100;
                            }
                        }
                    }
                }
                else {
                    txtcost.removeAttr("onfocus");
                    txtcost.removeAttr("class");
                }

                ttotal += parseFloat(total);
                //                    }
                //                }
                /********* ROW Calculate material totals*********/
                $tr.find('input[id*=txtTotal]').val(total);


                if (Gridview == 'ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems') {

                    calculateDynamicColumns($tr);

                    /********* ROW Calculate material + totals = GrandTotal*********/
                    var mattotal = $tr.find('input[id*=txtTotal]').val();
                    var labtotal = $tr.find('input[id*=txtLaborTotal]').val();
                    var Grandtotal = 0;
                    if (!isNaN(parseFloat(mattotal))) {
                        if (!isNaN(parseFloat(labtotal))) {
                            Grandtotal = parseFloat(mattotal) + parseFloat(labtotal);
                        }
                    }
                    $tr.find('input[id*=txtGrandTotal]').val(Grandtotal);
                }
            });


            /********* FOOTER Calculate material totals*********/
            var $footer = $("#" + Gridview).find('tr').eq($("#" + Gridview).find('tr').length - 1)
            //            $footer.find('input[id*=txtTQuan]').val(tquan);
            //            $footer.find('input[id*=txtTUnitCost]').val(tunit);
            $footer.find('input[id*=txtTTotal]').val(ttotal);



            /********* FOOTER Calculate material+labor = Grandtotals*********/
            if (Gridview == 'ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems') {

                $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:last').each(function () {
                    var $tr = $(this);
                    var $tds = $tr.find('td');
                    var tdtotal = 0;
                    $tds.each(function (index) {
                        var ttotal = 0;
                        if (index > 8 && index < $tds.length - 2) {
                            $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:not(:first, :last)').each(function () {
                                var amt = $(this).find('td').eq(index).find('input[type=text]').val();

                                if (!isNaN(parseFloat(amt))) {
                                    ttotal += parseFloat(amt);
                                }
                            });
                            var rate = $tr.find('td').eq(index).find('input[type=hidden]').val();
                            var Totalrate = parseFloat(rate) * ttotal;
                            $tr.find('td').eq(index).find('input[type=text]').val(Totalrate);
                            tdtotal += Totalrate;
                        }
                    });
                    $tr.find('input[id*=txtLaborTotal]').val(tdtotal);
                    //                });
                    //            
                    //                $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:last').each(function() {
                    //                    var $tr = $(this);
                    var mattotal = $tr.find('input[id*=txtTTotal]').val();
                    var labtotal = $tr.find('input[id*=txtLaborTotal]').val();
                    var Grandtotal = 0;
                    if (!isNaN(parseFloat(mattotal))) {
                        if (!isNaN(parseFloat(labtotal))) {
                            Grandtotal = parseFloat(mattotal) + parseFloat(labtotal);
                        }
                    }
                    $tr.find('input[id*=txtGrandTotal]').val(Grandtotal);
                });
            }

            /*********Calculate AllmeasureItems+PercentageItems list total = Finaltotal*********/
            var percfoot = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage").find('tr:last');
            var perctotal = percfoot.find('input[id*=txtTTotal]').val();
            var foot = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:last');
            var itttotal = foot.find('input[id*=txtGrandTotal]').val();
            <%--if (!isNaN(parseFloat(perctotal))) {
                if (!isNaN(parseFloat(itttotal))) {
                    $('#<%= txtTotalFinal.ClientID %>').val(parseFloat(itttotal) + parseFloat(perctotal));
                }
            }--%>
        }



        function calculateDynamicColumns($tr) {
            var ttotal = 0;

            ////////////            $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:not(:first, :last)').each(function() {
            ////////////                var $tr = $(this);


            //                var amt = $tr.find('input[id*=txt' + txt + ']').val();
            //                if (!isNaN(parseFloat(amt))) {
            //                    ttotal += parseFloat(amt);
            //                }

            var $tds = $tr.find('td');
            var tdtotal = 0;
            $tds.each(function (index) {

                if (index > 8 && index < $tds.length - 2) {
                    var tdamt = $(this).find('input[type=text]').val();
                    var foot = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:last');
                    var hdnID = foot.find('td').eq(index).find('input[type=hidden]').attr('id');

                    var footamount = foot.find('input[id=' + hdnID + ']').val();

                    if (!isNaN(parseFloat(tdamt))) {
                        tdtotal += parseFloat(tdamt) * parseFloat(footamount);
                    }
                }
            });
            $tr.find('input[id*=txtLaborTotal]').val(tdtotal);

            //////////                var mattotal = $tr.find('input[id*=txtTotal]').val();
            //////////                var Grandtotal = 0;
            //////////                if (!isNaN(parseFloat(mattotal))) {
            //////////                    Grandtotal = parseFloat(mattotal) + tdtotal;
            //////////                }
            //////////                $tr.find('input[id*=txtGrandTotal]').val(Grandtotal);
            //////////            });

            //            var $footer = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr').eq($("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr').length - 1)
            //            var rate = $footer.find('input[id*=hdn' + txt + 'T]').val();
            //            var Totalrate = parseFloat(rate) * ttotal;
            //            $footer.find('input[id*=txt' + txt + 'T]').val(Totalrate);


            /********Footer**********/
            //            $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:last').each(function() {
            //                var $tr = $(this);
            //                var $tds = $(this).find('td');
            //                var tdtotal = 0;
            //                $tds.each(function(index) {
            //                    if (index > 8 && index < $tds.length - 2) {
            //                        var tdamt = $(this).find('input[type=text]').val();
            //                        if (!isNaN(parseFloat(tdamt))) {
            //                            tdtotal += parseFloat(tdamt);
            //                        }
            //                    }
            //                });

            //                $tr.find('input[id*=txtLaborTotal]').val(tdtotal);
            //            });                


            ////////                var mattotal = $tr.find('input[id*=txtTTotal]').val();
            ////////                var Grandtotal = 0;
            ////////                if (!isNaN(parseFloat(mattotal))) {
            ////////                    Grandtotal = parseFloat(mattotal) + tdtotal;
            ////////                }
            ////////                $tr.find('input[id*=txtGrandTotal]').val(Grandtotal);

            ////////                var percfoot = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage").find('tr:last');
            ////////                var perctotal = percfoot.find('input[id*=txtTTotal]').val();
            ////////                //if (!isNaN(parseFloat(perctotal))) {
            ////////                  <%--  $('#<%= txtTotalFinal.ClientID %>').val(parseFloat(Grandtotal) + parseFloat(perctotal));--%>
            ////////                }



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
                            CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
                            CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');
                        }
                        else {
                            $(this).closest('tr').find('input:text').val('');
                            CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
                            CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');
                        }
                    });
                });
            }
        }

        function DelRowEstimate(Gridview) {
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
            var rawData = $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawDatap = $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage').serializeFormJSON();
            var formDatap = JSON.stringify(rawDatap);
            $('#<%=hdnItemJSONPerc.ClientID%>').val(formDatap);

            itemJSONEstimate();
        }

        function itemJSONEstimate() {
            var rawData = $('#<%=gvBOM.ClientID%>').serializeFormJSON();
            var formData = JSON.stringify(rawData);

            $('#<%=hdnBOMItemJSON.ClientID%>').val(formData);

            var rawMileData = $('#<%=gvMilestones.ClientID%>').serializeFormJSON();
            var formMileData = JSON.stringify(rawMileData);

            $('#<%=hdnMilestone.ClientID%>').val(formMileData);

        }
        function showDecimalVal(obj) {
            if (!isNaN(parseFloat(document.getElementById(obj.id).value))) {
                document.getElementById(obj.id).value = parseFloat(document.getElementById(obj.id).value).toFixed(2);
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
                if (document.selection == undefined) return 0;
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
        function calTotalPrice(obj) {

            var objId = document.getElementById(obj.id);
            var currentVal = objId.value;
            var txtTotalPriceObject, txtHdnTotalPrice;
            var TotalPrice;
            if (objId.value) {
                var isPercentage = obj.id.search("txtPercntge");
                if (isPercentage != -1) {

var totalAmount = document.getElementById(obj.id.replace('txtPercntge', 'txtBudgetUnit')).textContent | document.getElementById(obj.id.replace('txtPercntge', 'txtBudgetUnit')).innerHTML;
                    //var totalAmount = document.getElementById(obj.id.replace('txtPercntge', 'lblBudgetExt')).textContent | document.getElementById(obj.id.replace('txtPercntge', 'lblBudgetExt')).innerHTML;
                    TotalPrice = (totalAmount * currentVal) / 100 + totalAmount;
                    TotalPrice = TotalPrice.toFixed(2);
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtPercntge', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtPercntge', 'hdnTotalPrice'));
                    var amountTxtBox = document.getElementById(obj.id.replace('txtPercntge', 'txtAmt'));
                    amountTxtBox.disabled = true;
                }
                else {
                    var totalAmount = document.getElementById(obj.id.replace('txtAmt', 'lblBudgetExt')).textContent | document.getElementById(obj.id.replace('txtAmt', 'lblBudgetExt')).innerHTML;
                    TotalPrice = parseInt(currentVal) + parseInt(totalAmount);
                    TotalPrice = TotalPrice.toFixed(2);
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtAmt', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtAmt', 'hdnTotalPrice'));
                    var percentageTxtBox = document.getElementById(obj.id.replace('txtAmt', 'txtPercntge'));
                    percentageTxtBox.disabled = true;
                }
                $(txtTotalPriceObject).text(TotalPrice);
                $(txtHdnTotalPrice).val(TotalPrice);
            }
            else {
                var isPercentage = obj.id.search("txtPercntge");
                if (isPercentage == -1) {
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtAmt', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtAmt', 'hdnTotalPrice'));
                }
                else {
                    txtTotalPriceObject = document.getElementById(obj.id.replace('txtPercntge', 'lblTotalPrice'));
                    txtHdnTotalPrice = document.getElementById(obj.id.replace('txtPercntge', 'hdnTotalPrice'));
                }
                $(txtTotalPriceObject).text(" ");
                $(txtHdnTotalPrice).val(" ");
                document.getElementById(obj.id.replace('txtPercntge', 'txtAmt')).disabled = false;
                document.getElementById(obj.id.replace('txtAmt', 'txtPercntge')).disabled = false;
            }
        }
       <%-- function AddNewBucket(dropdown) {
            if (dropdown.selectedIndex == 1) {
                document.getElementById('<%= iframe.ClientID %>').src = "addestimatebucket.aspx";
                $find('PMPBehaviour').show();
            }
            else if (dropdown.selectedIndex == 0) {
            }
            else {
                var cnf = confirm('Click Ok to edit the bucket or click cancel to insert to template.')
                if (cnf == true) {
                    document.getElementById('<%= iframe.ClientID %>').src = "addestimatebucket.aspx?uid=" + dropdown.options[dropdown.selectedIndex].value;
                    $find('PMPBehaviour').show();
                } else {
                    document.getElementById('<%= btnInsertBuck.ClientID %>').click();
                }
            }
    }--%>
    function dtaa() {
        this.prefixText = null;
        this.con = null;
        this.custID = null;
    }
    function isInt(value) {
        var x = parseFloat(value);
        return !isNaN(value) && (x | 0) === x;
    }
    function AddTemplate(dropdown) {

        if (dropdown.selectedIndex == 0) {
            return false;
        }
        else {
            var cnf = confirm('Do you want to insert the template items?')
            if (cnf == true) { itemJSON(); __doPostBack(dropdown.id, ''); } else { return false; }
        }
    }

    function ChkCustomer(sender, args) {
        var hdnId = document.getElementById('<%=hdnROLId.ClientID%>');
        if (hdnId.value == '') {
            args.IsValid = false;
        }
    }

    function NumericValid(e) {
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
    }


    $(document).ready(function () {
        if ($(window).width() > 767) {
            $('#<%=txtREPremarks.ClientID%>').focus(function () {
                $(this).animate({
                    //right: "+=0",
                    width: '520px',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });

            $('#<%=txtREPremarks.ClientID%>').blur(function () {
                $(this).animate({
                    width: '100%',
                    height: '75px'
                }, 500, function () {
                    // Animation complete.
                });
            });
        }
        $("#flip").click(function () {
            $("#panelSlide").slideToggle("slow");
        });
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
                        //debugger;
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
                // debugger;
                $(this).val(ui.item.label);
                return false;
            },
            minLength: 0,
            delay: 250
        }).bind('click', function () { $(this).autocomplete("search"); })
        $.each($(".searchinput"), function (index, item) {
            $(item).data("autocomplete")._renderItem = function (ul, item) {
                // debugger;
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

    });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-cont-top">
        <%--<ul class="page-breadcrumb">
            <li>
                <i class="fa fa-home"></i>
                <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="#">Sales Manager</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>--%>
        <%--<a href="#">Estimate</a>--%>
        <%--<a href="<%=ResolveUrl("~/estimate.aspx") %>">Estimate</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <span>Add Estimate</span>
            </li>
        </ul>--%>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
            <ul class="lnklist-header">
                <li>
                    <asp:Label CssClass="title_text" ID="Label13" runat="server">Add Estimate</asp:Label></li>
                <li>
                    <asp:LinkButton ID="lnkSaveTemplate" OnClientClick="itemJSON();" runat="server" ValidationGroup="templ" ToolTip="Save" CssClass="icon-save" OnClick="lnkSaveTemplate_Click"></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" ForeColor="Red" OnClick="lnkCloseTemplate_Click" ToolTip="Close" CssClass="icon-closed"></asp:LinkButton></li>
            </ul>



        </div>
        <div class="ae-content">
            <asp:HiddenField ID="hdnItemJSON" runat="server" />
            <asp:HiddenField ID="hdnROLId" runat="server" />
            <asp:HiddenField ID="hdnOwnerID" runat="server" />
            <asp:HiddenField ID="hdnItemJSONPerc" runat="server" />
            <asp:HiddenField ID="hdnBOMItemJSON" runat="server" />
            <asp:HiddenField ID="hdnMilestone" runat="server" />
            <asp:Panel ID="pnlREPT" runat="server">
                <div class="col-lg-8 col-md-8">
                    <table>
                        <tr id="trProj" runat="server" visible="false">
                            <td class="register_lbl">Project</td>
                            <td>
                                <asp:HyperLink ID="lnkProject" runat="server" Target="_blank"></asp:HyperLink></td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                    <div class="form-col">
                        <div class="fc-label1">
                            Contact
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server"
                        ControlToValidate="txtCont" Display="None" ErrorMessage="Contact Required"
                        SetFocusOnError="True" ValidationGroup="templ">
                    </asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="RequiredFieldValidator40_ValidatorCalloutExtender"
                                runat="server" Enabled="True" TargetControlID="RequiredFieldValidator40">
                            </asp:ValidatorCalloutExtender>
                            <asp:CustomValidator ID="CustomValidator1" runat="server"
                                ClientValidationFunction="ChkCustomer" ControlToValidate="txtCont"
                                Display="None" ErrorMessage="Please select the Contact from list"
                                SetFocusOnError="True" ValidationGroup="templ"></asp:CustomValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" runat="server"
                                Enabled="True" PopupPosition="TopLeft" TargetControlID="CustomValidator1">
                            </asp:ValidatorCalloutExtender>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtCont" runat="server" CssClass="form-control"
                                TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Estimate Name
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtName" runat="server" TabIndex="2" AutoCompleteType="None" CssClass="form-control"
                                MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                Display="None" ErrorMessage="Name Required" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                TargetControlID="RequiredFieldValidator1">
                            </asp:ValidatorCalloutExtender>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Estimate No
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="TxtEstimateNo" runat="server" AutoCompleteType="None" CssClass="form-control"
                                MaxLength="255"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Date
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="TxtDate" runat="server" AutoCompleteType="None" CssClass="form-control" autocomplete="off"
                                MaxLength="255"></asp:TextBox>
                            <asp:CalendarExtender ID="TxtDate_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="TxtDate">
                            </asp:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfDate"
                                runat="server" ControlToValidate="TxtDate" Display="None" ErrorMessage="Date is Required" ValidationGroup="search"
                                SetFocusOnError="True"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="vceDateRf" runat="server" Enabled="True"
                                PopupPosition="Right" TargetControlID="rfDate" />
                            <asp:RegularExpressionValidator ID="rfvDate1" ControlToValidate="TxtDate" ValidationGroup="search"
                                ValidationExpression="^(0?[1-9]|1[012])[\/\-](0?[1-9]|[12][0-9]|3[01])[\/\-]\d{4}$"
                                runat="server" ErrorMessage="Invalid Date format. Valid Date Format 01/12/2001" Display="None">
                            </asp:RegularExpressionValidator>
                            <asp:ValidatorCalloutExtender ID="vceDate" runat="server" Enabled="True" PopupPosition="Right"
                                TargetControlID="rfvDate1" />
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Desc<asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtREPdesc"
                                Display="None" ErrorMessage="Description Required" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                    ID="RequiredFieldValidator9_ValidatorCalloutExtender" runat="server" Enabled="True"
                                    TargetControlID="RequiredFieldValidator9">
                                </asp:ValidatorCalloutExtender>
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtREPdesc" TabIndex="3" runat="server" CssClass="form-control" MaxLength="255"
                                AutoCompleteType="None"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Status
                        </div>
                        <div class="fc-input">
                            <asp:DropDownList ID="ddlStatus" TabIndex="4" CssClass="form-control" runat="server">
                                <asp:ListItem Value="0">Open</asp:ListItem>
                                <asp:ListItem Value="1">Canceled</asp:ListItem>
                                <asp:ListItem Value="2">Withdrawn</asp:ListItem>
                                <asp:ListItem Value="3">Disqualified</asp:ListItem>
                                <asp:ListItem Value="4" Enabled="false">Sold</asp:ListItem>
                                <asp:ListItem Value="5" Enabled="false">Competitor</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Remarks
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtREPremarks" runat="server" TabIndex="5" CssClass="form-control" Rows="3"
                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div style="padding-top: 20px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:HiddenField ID="hdnExchangeRate" runat="server" />
                            <div class="row">
                                <%--<div class="col-md-4">
                                    <asp:DropDownList ID="ddlBucket" TabIndex="7" runat="server" onchange="AddNewBucket(this);" CssClass="form-control">
                                    </asp:DropDownList>
                                </div>--%>
                                <ul>
                                    <li>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlTemplate" TabIndex="8" runat="server" CssClass="form-control" AutoPostBack="true" onchange=" return AddTemplate(this);"
                                                OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </li>
                                    <li>         
                                        <div class="col-md-1">
                                        <div class="fc-label1">
                                            Job Type:
                                        </div>      
                                        </div>                                                                              
                                    </li>
                                    <li>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtJobType" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                    </li>
                                </ul>

                                <%--<div class="col-md-4">
                                    <a id="btnnewlab" tabindex="9" class="btn btn-primary form-control" runat="server" onclick="$('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_iframe1').attr('src','addlaboritem.aspx');  $find('PMPBehaviour1').show();"
                                        style="cursor: pointer; background-color: #357ebd; color: white">Manage Labor Items</a>
                                    <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary" OnClick="btnEdit_Click" Text="Edit" OnClientClick="return confirm('Editing the items will reset the labor and currency rates to latest. Do you want to continue?');" />
                                </div>--%>
                            </div>

                            <%--    <div class="form-col">
                                Total
                        <asp:TextBox ID="txtTotalFinal" runat="server" CssClass="texttransparent"
                            onclick="this.blur();"></asp:TextBox>
                            </div>
                            <asp:Button ID="hideModalPopupViaServer" OnClientClick="itemJSON();" runat="server"
                                Style="display: none" OnClick="hideModalPopupViaServer_Click" />
                            <asp:ImageButton ID="btnInsertBuck" Width="15px" OnClientClick="itemJSON();" OnClick="btnInsertBuck_Click"
                                ToolTip="Insert" runat="server" ImageUrl="images/update.png" Style="display: none;" />--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                CausesValidation="False" />
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" Style="display: none"
                CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: 1px solid #316b9d;">
                <div>
                    <iframe id="iframe" runat="server" scrolling="no" frameborder="0" class="iframe-bucket"></iframe>
                </div>
            </asp:Panel>
            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender2" BehaviorID="PMPBehaviour1"
                TargetControlID="hiddenTargetControlForModalPopup2" PopupControlID="Panel2" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="Panel2" Style="display: none; background: #fff; border: 1px solid #316b9d;">
                <div>
                    <iframe id="iframe1" runat="server" scrolling="no" frameborder="0" class="iframe-bucket"></iframe>
                </div>
            </asp:Panel>
            <div class="clearfix"></div>
            <%--  <div class="row">--%>
            <%--<div class="col-md-8">--%>
            <div class="table-scrollable" style="border: none">
                <asp:UpdatePanel ID="updPnlEstimateTemp" runat="server">
                    <ContentTemplate>
                        <asp:TabContainer ID="TabContainer" runat="server" ActiveTabIndex="0">
                            <%--ActiveTabIndex="0"--%>
                            <asp:TabPanel ID="tbpnlBOM" runat="server" HeaderText="Project BOM">
                                <HeaderTemplate>
                                    BOM
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="table-scrollable" style="height: 400px; overflow-y: auto; border: none">
                                        <div class="col-lg-12 col-md-12">
                                            <asp:UpdatePanel ID="UpdPnlBOM" runat="server">
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
                                                                    <a id="Button2" class="delButton" onclick="DelRowEstimate('<%=gvBOM.ClientID%>');"
                                                                        style="cursor: pointer;">
                                                                        <img src="images/menu_delete.png" title="Delete" width="18px" /></a>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:ImageButton ID="imgBtnAdd" runat="server" CommandName="AddBOMItem" CausesValidation="False"
                                                                        CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                        ImageUrl="~/images/add.png" Width="18px" OnClientClick="itemJSONEstimate();" />
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
                                                            <asp:TemplateField HeaderText="Type" ItemStyle-Width="25px">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlBType" runat="server" DataTextField="Type" SelectedValue='<%# Eval("Btype") == DBNull.Value ? 0 : Eval("Btype") %>'
                                                                        DataValueField="ID" DataSource='<%#dtBomType%>' OnSelectedIndexChanged="ddlBType_SelectedIndexChanged"
                                                                        AutoPostBack="true" Width="130px">
                                                                    </asp:DropDownList>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Item" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlItem" runat="server" Width="130px"
                                                                        OnSelectedIndexChanged="ddlItem_SelectedIndexChanged" AutoPostBack="true">
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
                                                            <asp:TemplateField HeaderText="Vendor" ItemStyle-Width="15%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtVendorEst" runat="server" Text='<%# Eval("Vendor") %>' Width="100px"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ItemStyle-Width="15%">
                                                                <HeaderTemplate>
                                                                    $:
            <asp:DropDownList ID="ddlCurrencyEst" DataSource='<%#dtCurrency%>' AutoPostBack="true" DataTextField="Name" DataValueField="ID" Style="width: 100px" runat="server" OnSelectedIndexChanged="CurrencyChanged">
            </asp:DropDownList>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:DropDownList ID="ddlCurrencyEstbind" DataSource='<%#dtCurrency%>' AutoPostBack="true" DataTextField="Name" DataValueField="ID" Style="width: 100px" runat="server"></asp:DropDownList>
                                                                    <%--<%# Eval("Country") %>--%>
                                                                    <%--<asp:DropDownList ID="ddlCurrencyEst" DataSource='<%#dtCurrency%>' AutoPostBack="true" DataTextField="Name" DataValueField="ID" Style="width: 100px" runat="server" SelectedValue='<%# Eval("currency") == DBNull.Value ? 0 : Eval("currency") %>'>--%>
                                                                    <%-- <asp:listitem value="0">select</asp:listitem>
                                                                                <asp:listitem value="1">US</asp:listitem>
                                                                                <asp:listitem value="2">CDN</asp:listitem>--%>
                                                                    <%--</asp:DropDownList>--%>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty Required" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtQtyReq" runat="server" Text='<%# Eval("QtyReq","{0:0.00}") %>' Style="text-align: right;"
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
                                                                        Style="text-align: right;" onchange="calBudgetExt(this)" onkeypress="return isDecimalKey(this,event)"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Budget Ext $" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblBudgetExt" runat="server" Text='<%# Eval("BudgetExt","{0:0.00}") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnBudgetExt" runat="server" Value='<%# Eval("BudgetExt","{0:0.00}") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Percentage %" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtPercntge" runat="server" Text='<%# Eval("jPercent") %>' Width="100%"
                                                                        OnTextChanged="txtPercntge_TextChanged" AutoPostBack="true" ></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount $" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:TextBox ID="txtAmt" runat="server" Text='<%# Eval("Amount","{0:0.00}") %>' Width="100%"
                                                                        onchange="calTotalPrice(this)" onkeypress="return isDecimalKey(this,event)" AutoPostBack="true" OnTextChanged="txtAmt_TextChanged"></asp:TextBox>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Total Price" ItemStyle-Width="5%">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice","{0:0.00}") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnTotalPrice" runat="server" Value='<%# Eval("TotalPrice","{0:0.00}") %>' />
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
                                    <div class="table-scrollable" style="height: 400px; overflow-y: auto; border: none">
                                        <div class="col-lg-12 col-md-12">
                                            <asp:UpdatePanel ID="UpdatePnlMilestone" runat="server">
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
                                                                        Style="text-align: right;" Width="100%"></asp:TextBox>
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
                            <asp:TabPanel ID="tbpnlNotes" runat="server" HeaderText="Notes">
                                <HeaderTemplate>
                                    Notes
                                </HeaderTemplate>
                                <ContentTemplate>
                                    <div class="table-scrollable" style="height: 400px; overflow-y: auto; border: none">
                                        <div class="col-lg-12 col-md-12">
                                            <div class="form-col">
                                                <div class="fc-label1">
                                                    Remarks
                                                </div>
                                                <div class="fc-input">
                                                    <asp:TextBox ID="TextBox1" runat="server" Style="height: 400px" TabIndex="5" CssClass="form-control" Rows="3"
                                                        MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:TabPanel>
                            <asp:TabPanel ID="tbpnlDocuments" runat="server" HeaderText="Documents">
                                <HeaderTemplate>
                                    Documents
                                </HeaderTemplate>
                            </asp:TabPanel>
                        </asp:TabContainer>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--</div>
                <div style="height:200px;width:100px" class="col-md-8">
                    <div style="text-decoration:underline" id="flip">
                       <strong>Estimate Total Click Here</strong>
                    </div> 
                    <div id="panelSlide" style="display:none">
                        <table class="table table-bordered">
                            <tr>
                                <td>Contract Amount</td>
                                <td>$1000.00</td>
                            </tr>
                        </table>
                    </div>
                </div>--%>

            <%--</div>--%>
        </div>


    </div>
</asp:Content>
