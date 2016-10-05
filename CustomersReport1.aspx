<%@ Page Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true"
    CodeFile="CustomersReport1.aspx.cs" Inherits="Default2" Title="" EnableEventValidation="false" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="asp" Namespace="Saplin.Controls" Assembly="DropDownCheckBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%-- <script src="js/ResizeColumnsGrid/colResizable-1.5.min.js" type="text/javascript"></script>--%>

    <script src="js/ColumnResizeWithReorder/jquery.dataTables.js" type="text/javascript"></script>

    <script src="js/ColumnResizeWithReorder/ColReorderWithResize.js" type="text/javascript"></script>

    <script src="js/ReportsJs/CustomerReport.js" type="text/javascript"></script>

    <%--<script type="text/javascript" src="http://cdn.rawgit.com/niklas.vh/html2canvas/master/dist/html2canvas.min.js"></script>--%>

    <script type="text/javascript" src="js/Printdiv/jquery.print.js"></script>

    <link rel="stylesheet" href="css/css3-dropdown-menu/assets/css/styles.css" />
    <link rel="stylesheet" href="css/css3-dropdown-menu/assets/font-awesome/css/font-awesome.css" />

    <script type="text/javascript" src="js/BlockUI/jquery.blockUI.js"></script>

    <script type="text/javascript">

        function ExportToExcel() {

            var lstColumn = '';
            var lstColumnWidth = '';

            if ($('#tblResize tr').length > 0) {
                $('#tblResize tr th').each(function() {
                    lstColumn += $(this).html() + "^";
                    lstColumnWidth += $(this).css('width') + "^";
                });
            }
            else {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function(index, element) {
                    lstColumn += $(element).val() + "^";
                    lstColumnWidth += "125px" + "^";
                });
            }

            $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstColumn);

            $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

            var html = $("#tblResize_wrapper").html();
            html = $.trim(html);
            html = html.replace(/>/g, '&gt;');
            html = html.replace(/</g, '&lt;');
            $("input[id$='hdnDivToExport']").val(html);
            //  console.log($("input[id$='hdnDivToExport']").val());

            document.getElementById('<%=btnExportExcel.ClientID %>').click();

        }


        function ExportToPDF() {

            var lstColumn = '';
            var lstColumnWidth = '';

            if ($('#tblResize tr').length > 0) {
                $('#tblResize tr th').each(function() {
                    lstColumn += $(this).html() + "^";
                    lstColumnWidth += $(this).css('width') + "^";
                });
            }
            else {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function(index, element) {
                    lstColumn += $(element).val() + "^";
                    lstColumnWidth += "125px" + "^";
                });
            }

            $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstColumn);

            $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

            document.getElementById('<%=btnExportPDF.ClientID %>').click();

        }


        function btnSendReport() {
            var lstColumn = '';
            var lstColumnWidth = '';

            if ($('#tblResize tr').length > 0) {
                $('#tblResize tr th').each(function() {
                    lstColumn += $(this).html() + "^";
                    lstColumnWidth += $(this).css('width') + "^";
                });
            }
            else {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function(index, element) {
                    lstColumn += $(element).val() + "^";
                    lstColumnWidth += "125px" + "^";
                });
            }

            $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstColumn);

            $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

            document.getElementById('<%=btnSendReport.ClientID %>').click();
        }

        function btnSaveReport() {
            var lstColumn = '';
            var lstColumnWidth = '';

            if ($('#tblResize tr').length > 0) {
                $('#tblResize tr th').each(function() {
                    lstColumn += $(this).html() + "^";
                    lstColumnWidth += $(this).css('width') + "^";
                });
            }
            else {
                $('#ctl00_ContentPlaceHolder1_lstColumnSort option').each(function(index, element) {
                    lstColumn += $(element).val() + "^";
                    lstColumnWidth += "125px" + "^";
                });
            }

            $("#ctl00_ContentPlaceHolder1_hdnLstColumns").val(lstColumn);

            $("#ctl00_ContentPlaceHolder1_hdnColumnWidth").val(lstColumnWidth);

            document.getElementById('<%=btnSaveReport2.ClientID %>').click();
        }

        function SendPDFReport(Id) {
            $("#ctl00_ContentPlaceHolder1_dvGridReport").block({ message: null, overlayCSS: { backgroundColor: ''} });
            $(".title_bar").block({ message: null, overlayCSS: { backgroundColor: ''} });
            $("#ctl00_Menu").block({ message: null, overlayCSS: { backgroundColor: ''} });

            $("#ctl00_ContentPlaceHolder1_txtTo").val('');
            $("#ctl00_ContentPlaceHolder1_txtCc").val('');
            $("#ctl00_ContentPlaceHolder1_txtSubject").val($("#ctl00_ContentPlaceHolder1_hdnCustomizeReportName").val());
            $("#dvEmailPanel").show();

            var html = $("#tblResize_wrapper").html();
            html = $.trim(html);
            html = html.replace(/>/g, '&gt;');
            html = html.replace(/</g, '&lt;');
            $("input[id$='hdnDivToExport']").val(html);

            $("input[id$='hdnSendReportType']").val(Id);
        }



        function EmailCancel() {
            $("#ctl00_ContentPlaceHolder1_dvGridReport").unblock();
            $(".title_bar").unblock();
            $("#ctl00_Menu").unblock();
        }

        jQuery(document).ready(function() {



            //            $("#ctl00_ContentPlaceHolder1_grdCustomerReportData td,th").css("word-wrap", "break-word");

            //            $(".GridViewStyle").colResizable({
            //                liveDrag: true,
            //                fixed: true,
            //                gripInnerHtml: "<div class='grip'></div>",
            //                draggingClass: "dragging",
            //                onResize: onSampleResized

            //            });


            var oTable = $('#tblResize').dataTable({
                "sDom": 'Rplfrti',
                "oColReorder": {
                    "headerContextMenu": true
                },
                "bPaginate": false,
                "bFilter": false,
                "bSort": false,
                "bAutoWidth": true,
                "bInfo": false
            });
        });

        //        var onSampleResized = function(e) {
        //            var columns = $(e.currentTarget).find("th");
        //            var msg = "";
        //            columns.each(function() { msg += $(this).width() + ","; })
        //            // $.cookie("colWidth", msg);
        //        };
        
    </script>

    <style type="text/css">
        @import "js/ColumnResizeWithReorder/ColReorder.css";
        /*----- Buttons style -----*/.myButton
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f9f9f9), color-stop(1, #e9e9e9));
            background: -moz-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -webkit-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -o-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: -ms-linear-gradient(top, #f9f9f9 5%, #e9e9e9 100%);
            background: linear-gradient(to bottom, #f9f9f9 5%, #e9e9e9 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#f9f9f9', endColorstr='#e9e9e9',GradientType=0);
            background-color: #f9f9f9;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            cursor: pointer;
            color: #666666;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 3px 15px 3px 15px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #ffffff;
        }
        .myButton:hover
        {
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #e9e9e9), color-stop(1, #f9f9f9));
            background: -moz-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
            background: -webkit-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
            background: -o-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
            background: -ms-linear-gradient(top, #e9e9e9 5%, #f9f9f9 100%);
            background: linear-gradient(to bottom, #e9e9e9 5%, #f9f9f9 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#e9e9e9', endColorstr='#f9f9f9',GradientType=0);
            background-color: #e9e9e9;
        }
        .myButton:active
        {
            position: relative;
            top: 1px;
        }
        .BlueButton
        {
            -moz-box-shadow: 3px 4px 0px 0px #1564ad;
            -webkit-box-shadow: 3px 4px 0px 0px #1564ad;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #79bbff), color-stop(1, #378de5));
            background: -moz-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -webkit-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -o-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: -ms-linear-gradient(top, #79bbff 5%, #378de5 100%);
            background: linear-gradient(to bottom, #79bbff 5%, #378de5 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#79bbff', endColorstr='#378de5',GradientType=0);
            background-color: #79bbff;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #337bc4;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 3px 15px 3px 15px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #528ecc;
        }
        .BlueButton:hover
        {
            color: #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #378de5), color-stop(1, #79bbff));
            background: -moz-linear-gradient(top, #378de5 5%, #79bbff 100%);
            background: -webkit-linear-gradient(top, #378de5 5%, #79bbff 100%);
            background: -o-linear-gradient(top, #378de5 5%, #79bbff 100%);
            background: -ms-linear-gradient(top, #378de5 5%, #79bbff 100%);
            background: linear-gradient(to bottom, #378de5 5%, #79bbff 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#378de5', endColorstr='#79bbff',GradientType=0);
            background-color: #378de5;
        }
        .BlueButton:active
        {
            position: relative;
            top: 1px;
        }
        /*----- Button style end here -----*//*----- Model -----*/.modal-box
        {
            display: none;
            position: absolute;
            z-index: 1000;
            width: 60%;
            background: white;
            border-bottom: 1px solid #aaa;
            border-radius: 4px;
            box-shadow: 0 3px 9px rgba(0, 0, 0, 0.5);
            border: 1px solid rgba(0, 0, 0, 0.1);
            background-clip: padding-box;
        }
        .modal-box header
        {
            background-color: #f2972b;
        }
        .modal-box header, .modal-box .modal-header
        {
            /*padding: 1.25em 1.5em;*/
            padding: 0.50em 0.5em;
            border-bottom: 1px solid #ddd;
        }
        .modal-box header h3, .modal-box header h4, .modal-box .modal-header h3, .modal-box .modal-header h4
        {
            margin: 0;
            color: White;
        }
        .modal-box .modal-body
        {
            padding: 2em 1.5em;
        }
        .modal-box footer, .modal-box .modal-footer
        {
            padding: 1em;
            border-top: 1px solid #ddd;
            background: rgba(0, 0, 0, 0.02);
            text-align: right;
        }
        .modal-overlay
        {
            opacity: 0;
            filter: alpha(opacity=0);
            position: absolute;
            top: 0;
            left: 0;
            z-index: 900;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.3) !important;
        }
        footer a
        {
            width: 68px;
            text-align: center;
        }
        a.close
        {
            line-height: 1;
            font-size: 1.7em;
            font-weight: bold;
            position: absolute;
            top: 1%;
            right: 2%;
            text-decoration: none;
            color: #fff;
        }
        a.close:hover
        {
            color: #222;
            -webkit-transition: color 1s ease;
            -moz-transition: color 1s ease;
            transition: color 1s ease;
        }
        /*----- Model end here -----*//*----- Tabs -----*/.tabs
        {
            width: 100%;
            display: inline-block;
        }
        /*----- Tab Links -----*//* Clearfix */.tab-links:after
        {
            display: block;
            clear: both;
            content: '';
        }
        .tab-links li
        {
            margin: 0px 1px;
            float: left;
            list-style: none;
        }
        .tab-links a
        {
            padding: 5px 20px;
            display: inline-block;
            border-radius: 4px 4px 0px 0px;
            border-bottom: 0px;
            background: #cdcdcd;
            font-size: 12px;
            font-weight: 600;
            color: #4c4c4c;
            transition: all linear 0.15s;
            text-align: center;
            width: 100px;
        }
        .tab-links a:hover
        {
            background: #a7cce5;
            text-decoration: none;
        }
        li.active a, li.active a:hover
        {
            background: #fff;
            color: #4c4c4c;
            border: 1px solid #CDCDCD;
            border-bottom: 0;
        }
        /*----- Content of Tabs -----*/.tab-content
        {
            padding: 15px;
            border-radius: 3px;
            box-shadow: -1px 1px 5px rgba(0,0,0,0.15);
            background: #fff;
        }
        .tab
        {
            display: none;
        }
        .tab.active
        {
            display: block;
        }
        /*----- Tabs end here -----*//*Css design for checkbox list and radio button list*/.ListControl input[type=checkbox], input[type=radio]
        {
            display: none;
        }
        .ListControl label
        {
            display: inline;
            float: left;
            color: #000;
            cursor: pointer;
            text-indent: 20px;
            white-space: nowrap;
        }
        .ListControl input[type=checkbox] + label
        {
            display: block;
            width: 1em;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192);
            border-radius: 0.25em;
            background: rgb(240,240,240);
            background-image: -moz-linear-gradient(rgb(240,240,240),rgb(240,240,240));
            background-image: -ms-linear-gradient(rgb(240,240,240),rgb(240,240,240));
            background-image: -o-linear-gradient(rgb(240,240,240),rgb(240,240,240));
            background-image: -webkit-linear-gradient(rgb(240,240,240),rgb(240,240,240));
            background-image: linear-gradient(rgb(240,240,240),rgb(240,240,240));
            vertical-align: middle;
            line-height: 1em;
            font-size: 12px;
        }
        .ListControl input[type=checkbox]:checked + label::before
        {
            content: "\2714";
            color: #000;
            height: 1em;
            line-height: 1.1em;
            width: 1em;
            font-weight: normal;
            margin-right: 6px;
            margin-left: -20px;
        }
        .ListControl input[type=radio] + label
        {
            display: block;
            width: 1em;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192);
            border-radius: 1em;
            background: rgb(240, 240, 240);
            background-image: -moz-linear-gradient(rgb(240,240,240),rgb(240, 240, 240));
            background-image: -ms-linear-gradient(rgb(240,240,240),rgb(240, 240, 240));
            background-image: -o-linear-gradient(rgb(240,240,240),rgb(240, 240, 240));
            background-image: -webkit-linear-gradient(rgb(240,240,240),rgb(240, 240, 240));
            background-image: linear-gradient(rgb(240,240,240),rgb(240, 240, 240));
            vertical-align: middle;
            line-height: 1em;
            font-size: 12px;
        }
        .ListControl input[type=radio]:checked + label::before
        {
            /*content: "\2716";*/
            content: "\2714";
            color: #000;
            display: inline;
            width: 1em;
            height: 1em;
            margin-right: 6px;
            margin-left: -20px;
        }
        /*end here*//*Single checkbox design*/.CheckBoxLabel
        {
            white-space: nowrap;
        }
        .SingleCheckbox input[type=checkbox]
        {
            display: none;
        }
        .SingleCheckbox label
        {
            display: block;
            float: left;
            color: #000;
            cursor: pointer;
        }
        .SingleCheckbox input[type=checkbox] + label
        {
            width: 1em;
            height: 1em;
            border: 0.0625em solid rgb(192,192,192);
            border-radius: 0.25em;
            background: rgb(211,168,255);
            background-image: -moz-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -ms-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -o-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: -webkit-linear-gradient(rgb(240,240,240),rgb(211,168,255));
            background-image: linear-gradient(rgb(240,240,240),rgb(211,168,255));
            vertical-align: middle;
            line-height: 1em;
            text-indent: 20px;
            font-size: 14px;
        }
        .SingleCheckbox input[type=checkbox]:checked + label::before
        {
            content: "\2714";
            color: #fff;
            height: 1em;
            line-height: 1.1em;
            width: 1em;
            font-weight: 900;
            margin-right: 6px;
            margin-left: -20px;
        }
        .UpArrow
        {
            background: url(images/arrow_up.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 25px;
            padding: 2px;
        }
        .DownArrow
        {
            background: url(images/Down_Arrow.png) no-repeat;
            cursor: pointer;
            border: none;
            width: 25px;
        }
        .ddlstyle
        {
            width: 200px;
        }
        .ddlstyle option
        {
            padding-left: 25px;
            padding-top: 3px;
            height: 20px;
            font-size: 12px;
        }
        .highlight
        {
            background-color: #3399FF !important;
            color: White;
        }
        #tblFilterChoices tr:nth-child(odd)
        {
            background: #FBF5EF;
        }
        #tblFilterChoices tr:nth-child(even)
        {
            background: #F5ECCE;
        }
        #tblFilterChoices tr:hover
        {
            cursor: pointer;
        }
        /*Resize and reorder column*/#tblResize th
        {
            text-decoration: underline;
            text-align: center;
            font-family: Arial;
            font-size: 12px !important;
        }
        #tblResize th td
        {
            font-family: Arial;
            font-size: 11px !important;
        }
        #tblResize tr td
        {
            font-family: Arial;
            font-size: 11px !important;
        }
        .dataTable th, .dataTable td
        {
            overflow: hidden;
            white-space: nowrap;
        }
        #tblResize.dataTable
        {
            border-spacing: 0;
            margin: 0;
        }
        .dataTable th, .dataTable td
        {
            border-right: 1px solid black;
            max-width: 400px;
        }
        .dataTable th:last-child, .dataTable td:last-child
        {
            border-right: none;
        }
        #tblResize.display td
        {
            padding: 3px 4px 4px 3px;
        }
        .selcol1
        {
            float: left;
            margin-right: 20px;
        }
        .selcol2
        {
            float: left;
        }
        #tblResize .resize-header
        {
            position: relative;
            color: black;
            font-size: 11px;
            border: 1px solid transparent;
            text-align: left;
            padding-left: 25px;
        }
        #tblResize .resize-header:after
        {
            position: absolute;
            right: 0;
            top: 30%;
            width: 7px;
            height: 8px;
            content: " ";
            background: url("images/icons_big/list-bullet2.PNG") no-repeat;
        }
        #tblResize .resize-header:first-child:before
        {
            position: absolute;
            left: 0;
            top: 30%;
            width: 7px;
            height: 8px;
            content: " ";
            background: url("images/icons_big/list-bullet2.PNG") no-repeat;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%-- <asp:HiddenField ID="hfImageData" runat="server" />
    <asp:Button ID="btnExport" Text="Export to Image" runat="server" UseSubmitBehavior="false"
        OnClick="ExportToImage" OnClientClick="return ConvertToImage(this)" />--%>
    <div class="title_bar" style="height: 32px">
        <div style="float: left">
            <table>
                <tr>
                    <td>
                        <asp:Label CssClass="title_text" ID="lblHeader" runat="server">Customers Report</asp:Label>
                    </td>
                    <td style="padding-left: 20px;">
                        <asp:DropDownList ID="drpReports" runat="server" Width="170px" Height="24px" CssClass="register_input_bg_ddl_small ddlstyle"
                            AutoPostBack="true" OnSelectedIndexChanged="drpReports_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <%-- <td>
                        <select style="width: 200px" class="tech" name="tech" id="tech" onchange="showValue(this)">
                            <option value="calendar" data-image="images/globe.png">Calendar</option>
                            <option value="shopping_cart" data-image="images/globe.png">Shopping Cart</option>
                            <option value="cd" data-image="images/globe.png" name="cd">CD</option>
                        </select>
                    </td>--%>
                    <td style="padding-left: 20px;">
                        <a id="btnNewReport" class="myButton js-open-modal" href="#" data-modal-id="popup">New
                            Report</a>
                    </td>
                    <td style="padding-left: 2px;">
                        <asp:Button runat="server" ID="btnSaveReport2" CssClass="myButton" Text="Save Report"
                            OnClientClick="btnSaveReport();" OnClick="btnSaveReport2_Click" />
                    </td>
                    <td style="padding-left: 2px">
                        <asp:Button ID="btnDeleteReport" runat="server" CssClass="myButton" Text="Delete Report"
                            OnClientClick="if (!UserDeleteConfirmation()) return false;" OnClick="btnDeleteReport_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="float: right; padding-right: 50px">
            <table>
                <tr>
                    <td>
                        <a id="btnCustomizeReport" class="myButton js-open-modal" href="#" data-modal-id="popup">
                            Customize Report</a>
                    </td>
                    <td>
                        <%--<input id="btnPrint" type="button" value="Print" class="myButton"  />--%>
                        <a id="btnPrint" class="myButton" href="#">Print</a>
                    </td>
                    <td>
                        <%--<input id="btnEmail" type="button" value="Email" class="myButton" />--%>
                        <div id="dvBtnEmail">
                            <ul style="width: 66px">
                                <li class="green" style="position: relative;"><span class="">
                                    <img src="images/email_btn.png" height="23px" style="padding-top: 2px;" />
                                    <div id="dvEmail" style="left: 90%">
                                        <ul id="dvEmailOptions" style="">
                                        </ul>
                                    </div>
                                </span></li>
                            </ul>
                        </div>
                    </td>
                    <td>
                        <div id="colorNav">
                            <ul style="width: 66px">
                                <li class="green" style="position: relative;"><span class="">
                                    <img src="images/export_btn.png" height="23px" style="padding-top: 2px;" />
                                    <div id="dynamic-div" style="left: 90%">
                                        <ul id="dynamicUI" style="">
                                        </ul>
                                    </div>
                                </span></li>
                            </ul>
                        </div>
                    </td>
                    <td>
                        <asp:Button ID="btnClose" runat="server" Text="Close" class="myButton" OnClick="btnClose_Click" />
                    </td>
                    <td>
                        <div style="display: none">
                            <asp:Button ID="btnExportPDF" runat="server" Text="Export" class="myButton" OnClick="btnExportPDF_Click" />
                            <asp:Button ID="btnExportExcel" runat="server" Text="Export" class="myButton" OnClick="ExportToExcel" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="margin-left: 5px; padding-top: 20px;" align="center">
        <%-- <asp:Button ID="btnPrintReport" runat="server" Text="Print" OnClick="btnPrintReport_Click" />
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true"
            EnableDatabaseLogonPrompt="false" HasCrystalLogo="False" HasRefreshButton="True"
            DisplayToolbar="True" BestFitPage="False" Width="1300px" />--%>
        <div id="dvGridReport" runat="server" style="display: none;">
            <%-- <div align="left" style="padding-left: 30px; display: none;">
                <asp:ImageButton ID="btnFirst" runat="server" CommandArgument="First" ImageUrl="images/first.png"
                    Width="22px" OnClick="btnFirst_Click" />
                &nbsp &nbsp<asp:ImageButton ID="btnPrev" runat="server" CommandArgument="Prev" Width="22px"
                    ImageUrl="~/images/Backward.png" OnClick="btnPrev_Click" />
                <asp:ImageButton ID="btnForward" runat="server" CommandArgument="Next" ImageUrl="images/Forward.png"
                    Width="22px" OnClick="btnForward_Click" />
                &nbsp &nbsp
                <asp:ImageButton ID="btnLast" runat="server" CommandArgument="Last" ImageUrl="images/last.png"
                    Width="22px" OnClick="btnLast_Click" />
                &nbsp; &nbsp; &nbsp; <span style="color: Black">Go to page:</span>
                <asp:DropDownList ID="ddlPages" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPages_SelectedIndexChanged">
                </asp:DropDownList>
                <span style="color: Black">of </span>
                <asp:Label ID="lblPageCount" runat="server" ForeColor="Black"></asp:Label>
                &nbsp &nbsp <span style="color: Black">Display:</span>
                <asp:DropDownList ID="drpGridRow" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpGridRow_SelectedIndexChanged">
                    <asp:ListItem Value="15">15</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="30">25</asp:ListItem>
                </asp:DropDownList>
                &nbsp; <span style="color: Black">records per page</span>
            </div>
            <br />--%>
            <br />
            <div id="dvHeader" align="left" style="padding-left: 40px;">
                <div id="dvMainHeader" style="float: left;">
                    <table>
                        <tr>
                            <td>
                                <asp:Image ID="imgLogo" runat="server" Width="150px" Height="150px" />
                            </td>
                            <td style="padding-left: 35px;" colspan="4">
                                <table width="500px" style="height: 100px;">
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblCompanyName" Font-Bold="true" Font-Size="17px" ForeColor="Black"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblCompAddress" ForeColor="Black" Font-Size="14px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label runat="server" ID="lblCompEmail" ForeColor="Black" Font-Size="12px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="float: right; padding-right: 50px;">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblTime" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDate" runat="server" Font-Bold="true" Font-Size="11px" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="clear: both">
                </div>
                <div id="dvSubHeader" style="display: block" runat="server" align="center">
                    <table id="tblSubHeader">
                        <tr>
                            <td align="center">
                                <asp:Label runat="server" ID="lblCompanyName2" Font-Bold="true" Font-Size="17px"
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label runat="server" ID="lblReportTitle" Font-Bold="true" Font-Size="14px" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label runat="server" ID="lblSubTitle" Font-Bold="false" Font-Size="12px" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div style="width: 1200px; overflow: auto; padding-top: 20px;" align="left">
                <%--   <asp:GridView ID="grdCustomerReportData" runat="server" CssClass="GridViewStyle"
                    Visible="false" HeaderStyle-Font-Bold="true" HeaderStyle-Font-Size="13px" HeaderStyle-BorderColor="Black"
                    AllowPaging="true" PageSize="15" Width="900px" PagerSettings-Visible="false"
                    ForeColor="Black" Font-Size="12px" RowStyle-Height="30px" OnRowDataBound="grdCustomerReportData_RowDataBound"
                    OnDataBound="grdCustomerReportData_DataBound" OnPageIndexChanging="grdCustomerReportData_PageIndexChanging">
                </asp:GridView>--%>
                <asp:PlaceHolder ID="PlaceHolder1" runat="server" />
            </div>
        </div>
        <br />
    </div>
    <div id="popup" class="modal-box">
        <header>
    <a href="#" class="js-modal-close close">×</a>
    <h3><span id="spnModelTitle"></span></h3>
  </header>
        <div class="modal-body">
            <div class="tabs">
                <ul class="tab-links">
                    <li class="active"><a href="#tab1">Display</a></li>
                    <li><a href="#tab2">Filters</a></li>
                    <li><a href="#tab3">Header/Footer</a></li>
                    <li><a href="#tab4">Setting</a></li>
                </ul>
                <div class="tab-content">
                    <div id="tab1" class="tab active">
                        <%-- <p>
                            Tab #1 content goes here!</p>
                        <p>
                            Donec pulvinar neque sed semper lacinia. Curabitur lacinia ullamcorper nibh; quis
                            imperdiet velit eleifend ac. Donec blandit mauris eget aliquet lacinia! Donec pulvinar
                            massa interdum risus ornare mollis.</p>--%>
                        <fieldset style="border-color: #CDCDCD;">
                            <legend style="color: Black; font-size: 11px;"><b>COLUMNS:</b></legend>
                            <div>
                                <div style="float: left;">
                                    <div id="dvColumn" style="margin: 20px; padding: 10px; width: 150px; border: 1px solid #CDCDCD;
                                        height: 200px; overflow: auto; float: left">
                                        <asp:CheckBoxList ID="chkColumnList" runat="server" ForeColor="Black" Font-Size="12px"
                                            BorderStyle="Solid" BorderColor="Black" CssClass="ListControl">
                                        </asp:CheckBoxList>
                                    </div>
                                    <div style="float: right;">
                                        <table style="overflow: auto; margin-top: 20px;">
                                            <tr>
                                                <td>
                                                    <%-- <input id="MoveRight" type="button" value=" >> " />
                                                    <br />
                                                    <input id="MoveLeft" type="button" value=" << " /><br />--%>
                                                    <input id="MoveUp" type="button" class="UpArrow" />
                                                    <br />
                                                    <input id="MoveDown" type="button" class="DownArrow" />
                                                    <%--  <input id="Delete" type="button" value=" Delete Item " />
                                                    <input id="ReadAll" type="button" value=" Read All " />--%>
                                                </td>
                                                <td style="padding-left: 10px;">
                                                    <asp:ListBox ID="lstColumnSort" runat="server" SelectionMode="Multiple" Height="217px"
                                                        Width="150px">
                                                        <%-- <asp:ListItem Value="to1">to list 1</asp:ListItem>
                                                        <asp:ListItem Value="to2">to list 2</asp:ListItem>
                                                        <asp:ListItem Value="to3">to list 3</asp:ListItem>
                                                        <asp:ListItem Value="to4">to list 4</asp:ListItem>
                                                        <asp:ListItem Value="to5">to list 5</asp:ListItem>--%>
                                                    </asp:ListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                            <div id="dvSort" style="float: right; margin: 20px;">
                                <table>
                                    <tr>
                                        <td>
                                            Sort by
                                        </td>
                                        <td style="padding-left: 15px">
                                            <asp:DropDownList ID="drpSortBy" runat="server" CssClass="register_input_bg_ddl_small"
                                                Width="150px">
                                                <%--   <asp:ListItem>Name</asp:ListItem>
                                                    <asp:ListItem>City</asp:ListItem>
                                                    <asp:ListItem>State</asp:ListItem>
                                                    <asp:ListItem>Type</asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Sort in
                                        </td>
                                        <td style="padding-left: 15px">
                                            <asp:RadioButtonList ID="rdbOrders" runat="server" CssClass="ListControl">
                                                <asp:ListItem Selected="True" Value="1">Ascending order</asp:ListItem>
                                                <asp:ListItem Value="2">Descending order</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-top: 50px">
                                            Put a check mark next to each column that
                                            <br />
                                            you want to appear in the report.
                                            <br />
                                            <br />
                                            And set the order of the columns
                                            <br />
                                            with up and down arrow.
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                    </div>
                    <div id="tab2" class="tab" style="height: 280px;">
                        <div style="float: left;">
                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                <legend style="color: Black; font-size: 11px;"><b>CHOOSE FILTER:</b></legend>
                                <div style="float: left; margin: 20px;">
                                    <asp:ListBox ID="lstFilter" runat="server" Height="217px" Width="150px"></asp:ListBox>
                                </div>
                                <div id="Div2" style="float: right; margin: 20px; width: 220px;">
                                    <table id="tblState" style="display: none">
                                        <tr>
                                            <td>
                                                State
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="ddlState" runat="server" AddJQueryReference="false" UseButtons="false"
                                                    Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                    <Items>
                                                        <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
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
                                                    </Items>
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblStateRef" style="display: none">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlStateReference" runat="server" ToolTip="State" CssClass="register_input_bg"
                                                    Width="150px">
                                                    <asp:ListItem Value="All">All</asp:ListItem>
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
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblName" style="display: none">
                                        <tr>
                                            <td>
                                                Name
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--<asp:TextBox ID="txtName" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                <%--  <asp:DropDownList ID="drpName" runat="server" CssClass="register_input_bg" Width="150px">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownCheckBoxes ID="drpName" runat="server" AddJQueryReference="false" UseButtons="false"
                                                    Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblCity" style="display: none">
                                        <tr>
                                            <td>
                                                City
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <asp:TextBox ID="txtCity" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                <asp:DropDownCheckBoxes ID="drpCity" runat="server" AddJQueryReference="false" UseButtons="false"
                                                    Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblZip" style="display: none">
                                        <tr>
                                            <td>
                                                Zip
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtZip" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblPhone" style="display: none">
                                        <tr>
                                            <td>
                                                Phone
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPhone" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblFax" style="display: none">
                                        <tr>
                                            <td>
                                                Fax
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFax" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblContact" style="display: none">
                                        <tr>
                                            <td>
                                                Contact
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtContact" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblAddress" style="display: none">
                                        <tr>
                                            <td>
                                                Address
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <asp:TextBox ID="txtAddress" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                <asp:DropDownCheckBoxes ID="drpAddress" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="170"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblEmail" style="display: none">
                                        <tr>
                                            <td>
                                                Email
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblCountry" style="display: none">
                                        <tr>
                                            <td>
                                                Country
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCountry" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblWebsite" style="display: none">
                                        <tr>
                                            <td>
                                                Website
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblCellular" style="display: none">
                                        <tr>
                                            <td>
                                                Cellular
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCellular" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblCategory" style="display: none">
                                        <tr>
                                            <td>
                                                Category
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drpCategory" runat="server" CssClass="register_input_bg" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblType" style="display: none">
                                        <tr>
                                            <td>
                                                Type
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <asp:DropDownList ID="drpType" runat="server" CssClass="register_input_bg" Width="150px">
                                                </asp:DropDownList>--%>
                                                <asp:DropDownCheckBoxes ID="drpType" runat="server" AddJQueryReference="false" UseButtons="false"
                                                    Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Types" />
                                                    <%-- <asp:ListItem Text="Mango" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="Apple" Value="2"></asp:ListItem>
                                                    <asp:ListItem Text="Banana" Value="3"></asp:ListItem>--%>
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblBalance" style="display: none">
                                        <tr>
                                            <td colspan="4">
                                                Balance
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:RadioButton ID="rdbAny" runat="server" GroupName="Balance" Text="Any" CssClass="ListControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEqual" runat="server" Width="40px" GroupName="Balance" Text="="
                                                    CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBalEqual" runat="server" CssClass="register_input_bg" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbLessAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                    Text="&lt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBalLessAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                                <span style="padding-left: 10px;">and</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbGreaterAndEqual" runat="server" Width="40px" GroupName="Balance"
                                                    Text="&gt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBalGreaterAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblStatus" style="display: none">
                                        <tr>
                                            <td>
                                                Status
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="drpStatus" runat="server" CssClass="register_input_bg" Width="150px">
                                                    <asp:ListItem Value="Status">All</asp:ListItem>
                                                    <asp:ListItem Value="0">Active</asp:ListItem>
                                                    <asp:ListItem Value="1">Inactive</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationId" style="display: none">
                                        <tr>
                                            <td>
                                                Location Id
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationId" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationName" style="display: none">
                                        <tr>
                                            <td>
                                                Location Name
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationName" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationAddress" style="display: none">
                                        <tr>
                                            <td>
                                                Location Address
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationAddress" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationCity" style="display: none">
                                        <tr>
                                            <td>
                                                Location City
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationCity" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationState" style="display: none">
                                        <tr>
                                            <td>
                                                Location State
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationState" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                    <Items>
                                                        <%--<asp:ListItem Value="All">All</asp:ListItem>--%>
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
                                                    </Items>
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationZip" style="display: none">
                                        <tr>
                                            <td>
                                                Location Zip
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%--  <asp:TextBox ID="txtLocationZip" runat="server" CssClass="register_input_bg" Width="150px"></asp:TextBox>--%>
                                                <asp:DropDownCheckBoxes ID="drpLocationZip" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLocationType" style="display: none">
                                        <tr>
                                            <td>
                                                Location Type
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpLocationType" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblEquipmentName" style="display: none">
                                        <tr>
                                            <td>
                                                Equipment Name
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpEquipmentName" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblManuf" style="display: none">
                                        <tr>
                                            <td>
                                                Manuf
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpManuf" runat="server" AddJQueryReference="false" UseButtons="false"
                                                    Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblEquipmentType" style="display: none">
                                        <tr>
                                            <td>
                                                Equipment Type
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpEquipmentType" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblServiceType" style="display: none">
                                        <tr>
                                            <td>
                                                Service Type
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownCheckBoxes ID="drpServiceType" runat="server" AddJQueryReference="false"
                                                    UseButtons="false" Width="150px" UseSelectAllNode="false">
                                                    <Style2 SelectBoxWidth="200" DropDownBoxBoxWidth="200" DropDownBoxBoxHeight="130"
                                                        SelectBoxCssClass="ListControl" />
                                                    <Texts SelectBoxCaption="Select" />
                                                </asp:DropDownCheckBoxes>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblEquipmentPrice" style="display: none">
                                        <tr>
                                            <td colspan="4">
                                                Equipment Price
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:RadioButton ID="rdbEquipmentPriceAny" runat="server" GroupName="ep" Text="Any"
                                                    CssClass="ListControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipmentPriceEqual" runat="server" Width="40px" GroupName="ep"
                                                    Text="=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipmentPriceEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipmentPriceLessAndEqual" runat="server" Width="40px" GroupName="ep"
                                                    Text="&lt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipmentPriceLessAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                                <span style="padding-left: 10px;">and</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipmentPriceGreaterAndEqual" runat="server" Width="40px"
                                                    GroupName="ep" Text="&gt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipmentPriceGreatAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblLoc" style="display: none">
                                        <tr>
                                            <td colspan="4">
                                                Loc
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:RadioButton ID="rdbLocAny" runat="server" GroupName="loc" Text="Any" CssClass="ListControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbLocEqual" runat="server" Width="40px" GroupName="loc" Text="="
                                                    CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocEqual" runat="server" CssClass="register_input_bg" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbLocLessAndEqual" runat="server" Width="40px" GroupName="loc"
                                                    Text="&lt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocLessAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                                <span style="padding-left: 10px;">and</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbLocGreaterAndEqual" runat="server" Width="40px" GroupName="loc"
                                                    Text="&gt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLocGreatAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblEquip" style="display: none">
                                        <tr>
                                            <td colspan="4">
                                                Equipment
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:RadioButton ID="rdbEquipAny" runat="server" GroupName="equip" Text="Any" CssClass="ListControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipEqual" runat="server" Width="40px" GroupName="equip"
                                                    Text="=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipEqual" runat="server" CssClass="register_input_bg" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipLessAndEqual" runat="server" Width="40px" GroupName="equip"
                                                    Text="&lt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipLessAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                                <span style="padding-left: 10px;">and</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbEquipGreaterAndEqual" runat="server" Width="40px" GroupName="equip"
                                                    Text="&gt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEquipGreatAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table id="tblOpenCalls" style="display: none">
                                        <tr>
                                            <td colspan="4">
                                                Open Calls
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <asp:RadioButton ID="rdbOCAny" runat="server" GroupName="oc" Text="Any" CssClass="ListControl" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbOCEqual" runat="server" Width="40px" GroupName="oc" Text="="
                                                    CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOCEqual" runat="server" CssClass="register_input_bg" Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbOCLessAndEqual" runat="server" Width="40px" GroupName="oc"
                                                    Text="&lt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOCLessAndEqual" runat="server" CssClass="register_input_bg" Width="80px"></asp:TextBox>
                                                <span style="padding-left: 10px;">and</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rdbOCGreaterAndEqual" runat="server" Width="40px" GroupName="oc"
                                                    Text="&gt;=" CssClass="ListControl" />
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOCGreatAndEqual" runat="server" CssClass="register_input_bg"
                                                    Width="80px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </fieldset>
                        </div>
                        <div style="float: right;">
                            <fieldset style="border-color: #CDCDCD; width: 260px;">
                                <legend style="color: Black; font-size: 11px;"><b>CURRENT FILTER CHOICES</b></legend>
                                <div style="margin-top: 20px; margin-left: 7px;">
                                    <div style="height: 150px; border: solid 1px black; overflow: auto;">
                                        <table width="247px">
                                            <thead>
                                                <tr style="background: gray; color: White;">
                                                    <td width="100px" style="height: 25px;">
                                                        FILTER
                                                    </td>
                                                    <td width="220px" style="height: 25px">
                                                        SET TO
                                                    </td>
                                                </tr>
                                            </thead>
                                        </table>
                                        <table id="tblFilterChoices" width="247px">
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                                <br />
                                <div style="text-align: center; padding-bottom: 10px;">
                                    <input type="button" class="myButton" value="Remove Selected Filter" id="btnRemoveFilter" />
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <div id="tab3" class="tab" style="height: 300px;">
                        <div style="float: left;">
                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                <legend style="color: Black; font-size: 11px;"><b>SHOW HEADER INFORMATION:</b></legend>
                                <table style="width: 400px;">
                                    <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                            <asp:CheckBox runat="server" ID="chkMainHeader" Text="Main Header" CssClass="ListControl"
                                                Checked="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkCompanyName" Text="Company Name" CssClass="ListControl" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCompanyName" runat="server" Width="200px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                            <asp:CheckBox runat="server" ID="chkReportTitle" Text="Report Title" CssClass="ListControl" />
                                        </td>
                                        <td style="padding-top: 8px;">
                                            <asp:TextBox ID="txtReportTitle" runat="server" Width="200px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                            <asp:CheckBox runat="server" ID="chkSubtitle" Text="Subtitle" CssClass="ListControl" />
                                        </td>
                                        <td style="padding-top: 8px;">
                                            <asp:TextBox ID="txtSubtitle" runat="server" Width="200px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px; padding-top: 8px;">
                                            <asp:CheckBox runat="server" ID="chkDatePrepared" Text="Date Prepared" CssClass="ListControl"
                                                Checked="true" />
                                        </td>
                                        <td style="padding-top: 8px;">
                                            <asp:DropDownList ID="drpDatePrepared" runat="server" Width="215px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px">
                                                <asp:ListItem Value="12/31/01" Selected="True">12/31/01</asp:ListItem>
                                                <asp:ListItem Value="Dec 31, 01">Dec 31, 01</asp:ListItem>
                                                <asp:ListItem Value="December 31, 01">December 31, 01</asp:ListItem>
                                                <asp:ListItem Value="Dec 31, 2001">Dec 31, 2001</asp:ListItem>
                                                <asp:ListItem Value="December 31, 2001">December 31, 2001</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkTimePrepared" Text="Time Prepared" CssClass="ListControl"
                                                Checked="true" />
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkReportBasis" Text="Report Basis" CssClass="ListControl" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkPrintHeaderOnFPage" Text="Print header on pages after first page"
                                                CssClass="ListControl" />
                                        </td>
                                    </tr>--%>
                                </table>
                            </fieldset>
                            <br />
                            <fieldset style="border-color: #CDCDCD; width: 460px;">
                                <legend style="color: Black; font-size: 11px;"><b>SHOW FOOTER INFORMATION:</b></legend>
                                <table style="width: 400px;">
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px;">
                                            <asp:CheckBox runat="server" ID="chkPageNumber" Text="Page Number" CssClass="ListControl" />
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpPageNumber" runat="server" Width="215px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px">
                                                <asp:ListItem Value="Page 1">Page 1</asp:ListItem>
                                                <asp:ListItem Value="pg 1">pg 1</asp:ListItem>
                                                <asp:ListItem Value="p 1">p 1</asp:ListItem>
                                                <asp:ListItem Value="<1>"><1></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px; padding-left: 25px;">
                                            <asp:CheckBox runat="server" ID="chkExtraFootLine" Text="Extra Footer Line" CssClass="ListControl" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExtraFooterLine" runat="server" Width="200px" CssClass="register_input_bg"
                                                Enabled="false" Height="22px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <%-- <tr>
                                        <td colspan="2" style="width: 150px; padding-left: 25px; padding-top: 12px;">
                                            <asp:CheckBox runat="server" ID="chkPrtFootonFPage" Text="Print footer on pages page"
                                                CssClass="ListControl" />
                                        </td>
                                    </tr>--%>
                                </table>
                            </fieldset>
                        </div>
                        <div style="float: right">
                            <fieldset style="border-color: #CDCDCD; width: 230px; height: 277px;">
                                <legend style="color: Black; font-size: 11px;"><b>PAGE LAYOUT:</b></legend>
                                <table style="width: 180px;">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span>Alignment</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 8px;padding-left:15px;">
                                            <asp:DropDownList ID="drpAlignment" runat="server" Width="170px" CssClass="register_input_bg"
                                                Height="22px">
                                                <asp:ListItem Value="Standard" Selected="True">Standard</asp:ListItem>
                                                <asp:ListItem Value="Left">Left</asp:ListItem>
                                                <asp:ListItem Value="Right">Right</asp:ListItem>
                                                <asp:ListItem Value="Centered">Centered</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                    <div id="tab4" class="tab" style="height: 150px;">
                        <div style="float: left;">
                            <fieldset style="border-color: #CDCDCD; width: 460px;height:100px;">
                                <legend style="color: Black; font-size: 11px;"><b>PDF Page Size:</b></legend>
                                <table style="width: 400px;padding-top:15px;padding-left:15px;">
                                    <tr>
                                        <td>
                                            Select Size:
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="drpPDFPageSize" runat="server" Width="215px" CssClass="register_input_bg"
                                                Height="22px">
                                                <asp:ListItem Value="11X17">11X17</asp:ListItem>
                                                <asp:ListItem Value="A0">A0</asp:ListItem>
                                                <asp:ListItem Value="A1">A1</asp:ListItem>
                                                <asp:ListItem Value="A10">A10</asp:ListItem>
                                                <asp:ListItem Value="A2">A2</asp:ListItem>
                                                <asp:ListItem Value="A3">A3</asp:ListItem>
                                                <asp:ListItem Value="A4" Selected="True">A4</asp:ListItem>
                                                <asp:ListItem Value="A4_LANDSCAPE">A4_LANDSCAPE</asp:ListItem>
                                                <asp:ListItem Value="A5">A5</asp:ListItem>
                                                <asp:ListItem Value="A6">A6</asp:ListItem>
                                                <asp:ListItem Value="A7">A7</asp:ListItem>
                                                <asp:ListItem Value="A8">A8</asp:ListItem>
                                                <asp:ListItem Value="A9">A9</asp:ListItem>
                                                <asp:ListItem Value="ARCH_A">ARCH_A</asp:ListItem>
                                                <asp:ListItem Value="ARCH_B">ARCH_B</asp:ListItem>
                                                <asp:ListItem Value="ARCH_C">ARCH_C</asp:ListItem>
                                                <asp:ListItem Value="ARCH_D">ARCH_D</asp:ListItem>
                                                <asp:ListItem Value="ARCH_E">ARCH_E</asp:ListItem>
                                                <asp:ListItem Value="B0">B0</asp:ListItem>
                                                <asp:ListItem Value="B1">B1</asp:ListItem>
                                                <asp:ListItem Value="B10">B10</asp:ListItem>
                                                <asp:ListItem Value="B2">B2</asp:ListItem>
                                                <asp:ListItem Value="B3">B3</asp:ListItem>
                                                <asp:ListItem Value="B4">B4</asp:ListItem>
                                                <asp:ListItem Value="B5">B5</asp:ListItem>
                                                <asp:ListItem Value="B6">B6</asp:ListItem>
                                                <asp:ListItem Value="B7">B7</asp:ListItem>
                                                <asp:ListItem Value="B8">B8</asp:ListItem>
                                                <asp:ListItem Value="B9">B9</asp:ListItem>
                                                <asp:ListItem Value="CROWN_OCTAVO">CROWN_OCTAVO</asp:ListItem>
                                                <asp:ListItem Value="CROWN_QUARTO">CROWN_QUARTO</asp:ListItem>
                                                <asp:ListItem Value="DEMY_OCTAVO">DEMY_OCTAVO</asp:ListItem>
                                                <asp:ListItem Value="DEMY_QUARTO">DEMY_QUARTO</asp:ListItem>
                                                <asp:ListItem Value="EXECUTIVE">EXECUTIVE</asp:ListItem>
                                                <asp:ListItem Value="FLSA">FLSA</asp:ListItem>
                                                <asp:ListItem Value="FLSE">FLSE</asp:ListItem>
                                                <asp:ListItem Value="HALFLETTER">HALFLETTER</asp:ListItem>
                                                <asp:ListItem Value="ID_1">ID_1</asp:ListItem>
                                                <asp:ListItem Value="ID_2">ID_2</asp:ListItem>
                                                <asp:ListItem Value="ID_3">ID_3</asp:ListItem>
                                                <asp:ListItem Value="LARGE_CROWN_OCTAVO">LARGE_CROWN_OCTAVO</asp:ListItem>
                                                <asp:ListItem Value="LARGE_CROWN_QUARTO">LARGE_CROWN_QUARTO</asp:ListItem>
                                                <asp:ListItem Value="LEDGER">LEDGER</asp:ListItem>
                                                <asp:ListItem Value="LEGAL">LEGAL</asp:ListItem>
                                                <asp:ListItem Value="LEGAL_LANDSCAPE">LEGAL_LANDSCAPE</asp:ListItem>
                                                <asp:ListItem Value="LETTER">LETTER</asp:ListItem>
                                                <asp:ListItem Value="LETTER_LANDSCAPE">LETTER_LANDSCAPE</asp:ListItem>
                                                <asp:ListItem Value="NOTE">NOTE</asp:ListItem>
                                                <asp:ListItem Value="PENGUIN_LARGE_PAPERBACK">PENGUIN_LARGE_PAPERBACK</asp:ListItem>
                                                <asp:ListItem Value="PENGUIN_SMALL_PAPERBACK">PENGUIN_SMALL_PAPERBACK</asp:ListItem>
                                                <asp:ListItem Value="POSTCARD">POSTCARD</asp:ListItem>
                                                <asp:ListItem Value="ROYAL_OCTAVO">ROYAL_OCTAVO</asp:ListItem>
                                                <asp:ListItem Value="ROYAL_QUARTO">ROYAL_QUARTO</asp:ListItem>
                                                <asp:ListItem Value="SMALL_PAPERBACK">SMALL_PAPERBACK</asp:ListItem>
                                                <asp:ListItem Value="TABLOID">TABLOID</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <footer>
    <%--<asp:Button runat="server" ID="btnApply" CssClass="BlueButton" Text ="Apply" Width="100px"/>  --%>
    <a href="#" class="BlueButton" id="btnApply" data-modal-id="dvSaveReport">Apply</a>  
    <a href="#" class="js-modal-close myButton" id="btnCancel">Cancel</a>
    
    
  </footer>
    </div>
    <div id="dvSaveReport" class="modal-box" runat="Server">
        <header> <a href="#" class="js-modal-close
    close">×</a> <h3>Save Report</h3> </header>
        <div class="modal-body">
            <table style="padding-left: 250px; height: 50px;">
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">Report Name:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="txtReportName" runat="server" Width="200px" Height="20px" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">Is Global:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <div>
                            <input type="checkbox" id="chkIsGlobal" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <footer> <asp:Button
    ID="btnSaveReport" runat="server" CssClass="BlueButton" Text="Save Report" OnClientClick="return
    EmptyReportName();" OnClick="btnSaveReport_Click" /> <a href="#" class="js-modal-close
    myButton" id="btnCancel2">Cancel</a> </footer>
    </div>
    <div id="dvEmailPanel" class="modal-box">
        <header> <a href="#" class="js-modal-close close" onclick="EmailCancel();">×</a>
    <h3><span id="Span1">Send Report:</span></h3> </header>
        <div class="modal-body">
            <table style="padding-left: 150px; height: 50px;">
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">From:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="txtFrom" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">To:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="txtTo" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">Cc:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="txtCc" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <%-- <tr> <td> &nbsp; </td> </tr> <tr> <td style="text-align: right">
    <b style="font-size: 14px;">Bcc:</b> </td> <td style="padding-left: 10px;"> <asp:TextBox
    ID="txtBcc" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
    </td> </tr>--%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;">Subject:</b>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="txtSubject" runat="server" Width="400px" Height="20px" Font-Size="14px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <%-- <tr> <td style="text-align: right">
    <b style="font-size: 14px;">Attached File:</b> </td> <td style="padding-left: 10px;">
    <div> <asp:Label ID="lblAttachedFile" runat="server"></asp:Label> </div> </td> </tr>--%>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <b style="font-size: 14px;"></b>
                    </td>
                    <td style="padding-left: 10px;">
                        <div>
                            <asp:TextBox TextMode="MultiLine" ID="txtBody" runat="server" Width="400px" Height="130px"
                                Text="This is report email
    sent from Mobile Office Manager. Please find the Report attached."></asp:TextBox>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <footer> <asp:Button ID="btnSendReport" runat="server"
    CssClass="BlueButton" Text="Send Report" OnClientClick="btnSendReport();" OnClick="btnSendReport_Click"
    /> <a href="#" class="js-modal-close myButton" id="btnEmailCancel" onclick="EmailCancel();">Cancel</a>
    </footer>
    </div>
    <asp:HiddenField runat="server" ID="hdnColumnList" />
    <asp:HiddenField runat="server" ID="hdnColumnWidth" />
    <asp:HiddenField runat="server" ID="hdnCustomizeReportName" />
    <asp:HiddenField runat="server" ID="hdnReportAction" />
    <asp:HiddenField runat="server" ID="hdnDrpSortBy" />
    <asp:HiddenField runat="server" ID="hdnLstColumns" />
    <asp:HiddenField runat="server" ID="hdnFilterColumns" />
    <asp:HiddenField runat="server" ID="hdnFilterValues" />
    <asp:HiddenField runat="server" ID="hdnMainHeader" />
    <asp:HiddenField runat="server" ID="hdnDivToExport" />
    <asp:HiddenField runat="server" ID="hdnSendReportType" />
</asp:Content>
