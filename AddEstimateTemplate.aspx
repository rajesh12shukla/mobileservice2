<%@ Page Language="C#" MasterPageFile="~/NewSalesMaster.master" AutoEventWireup="true"
    EnableEventValidation="false" CodeFile="AddEstimateTemplate.aspx.cs" Inherits="AddEstimateTemplate"
    Title="Estimate Template - Mobile Office Manager" %>

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

            //            CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
        }

        function InitializeGrids(Gridview) {

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


        function CalculatePercentage(Gridview) {
            $("#" + Gridview).find('tr:not(:first, :last)').each(function () {
                var ddlMeasure = $(this).find('select[id*=ddlMeasure]')
                Measure(ddlMeasure);
            });
        }

        function Measure(ddlMeasure) {
            var txtUnit = $(ddlMeasure).closest('tr').find('input[id*=txtQuan]');
            var txtCost = $(ddlMeasure).closest('tr').find('input[id*=txtUnitCost]');
            var txtTotal = $(ddlMeasure).closest('tr').find('input[id*=txtTotal]');
            if ($(ddlMeasure).val() == "2") {
                txtUnit.val("1");
                txtUnit.attr("onfocus", "this.blur();");
                txtUnit.attr("class", "texttransparent");
                if (!isNaN(parseFloat(txtCost.val())))
                    txtTotal.val(parseFloat(txtUnit.val()) * parseFloat(txtCost.val()));
            }
            else {
                txtUnit.removeAttr("onfocus");
                txtUnit.removeAttr("class");
            }



            if ($(ddlMeasure).val() == "3") {

                var total = 0;
                $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:not(:first, :last)').each(function () {
                    var $tr = $(this);
                    var amt = $tr.find('input[id*=txtTotal]').val();
                    var ddlMeasuretr = $tr.find('select[id*=ddlMeasure]').val();
                    if (ddlMeasuretr != '3') {
                        if (!isNaN(parseFloat(amt))) {
                            total += parseFloat(amt);
                        }
                    }
                });
                txtCost.val(total);
                txtCost.attr("onfocus", "this.blur();");
                txtCost.attr("class", "texttransparent");
                if (!isNaN(parseFloat(txtUnit.val())))
                    txtTotal.val((total * parseFloat(txtUnit.val())) / 100);
            }
            else {
                txtCost.removeAttr("onfocus");
                txtCost.removeAttr("class");
            }

            Calculate('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');
            Calculate('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');
        }

        function Calculate(Gridview) {

            var tquan = 0;
            var tunit = 0;
            var ttotal = 0;

            $("#" + Gridview).find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var quan = $tr.find('input[id*=txtQuan]').val();
                var unit = $tr.find('input[id*=txtUnitCost]').val();
                var ddlMeasure = $tr.find('select[id*=ddlMeasure]').val();
                var total = 0;

                if (!isNaN(parseFloat(quan))) {

                    tquan += parseFloat(quan);
                }
                if (!isNaN(parseFloat(unit))) {

                    tunit += parseFloat(unit);
                }

                if (!isNaN(parseFloat(quan))) {
                    if (!isNaN(parseFloat(unit))) {
                        if (ddlMeasure != '3')
                            total = parseFloat(quan) * parseFloat(unit);
                        else
                            total = (parseFloat(quan) * parseFloat(unit)) / 100;

                        ttotal += parseFloat(total);
                    }
                }
                $tr.find('input[id*=txtTotal]').val(total);

            });

            var $footer = $("#" + Gridview).find('tr').eq($("#" + Gridview).find('tr').length - 1)
            //            $footer.find('input[id*=txtTQuan]').val(tquan);
            //            $footer.find('input[id*=txtTUnitCost]').val(tunit);
            $footer.find('input[id*=txtTTotal]').val(ttotal);
        }

        function calculateDynamic(txt) {
            var ttotal = 0;
            $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr:not(:first, :last)').each(function () {
                var $tr = $(this);
                var amt = $tr.find('input[id*=txt' + txt + ']').val();
                if (!isNaN(parseFloat(amt))) {
                    ttotal += parseFloat(amt);
                }
            });
            var $footer = $("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr').eq($("#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems").find('tr').length - 1)
            var rate = $footer.find('input[id*=hdn' + txt + 'T]').val();
            var Totalrate = parseFloat(rate) * ttotal;
            $footer.find('input[id*=txt' + txt + 'T]').val(Totalrate);
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
            var rawData = $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);

            var rawDatap = $('#ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage').serializeFormJSON();
            var formDatap = JSON.stringify(rawDatap);
            $('#<%=hdnItemJSONPerc.ClientID%>').val(formDatap);
        }

        function AddNewBucket(dropdown) {
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
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-cont-top">
        <%-- <ul class="page-breadcrumb">
            <li>
                <i class="fa fa-home"></i>
                <a href="<%=ResolveUrl("~/Home.aspx") %>">Home</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="#">Sales Manager</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <a href="<%=ResolveUrl("~/estimatetemplate.aspx") %>">Estimate Template</a>
                <i class="fa fa-angle-right"></i>
            </li>
            <li>
                <span>Add Estimate Template</span>
            </li>
        </ul>--%>
        <div class="page-bar-right">
        </div>
    </div>
    <div class="add-estimate">
        <div class="ra-title">
            <ul class="lnklist-header">
                <li>
                    <asp:Label CssClass="title_text" ID="Label13" runat="server">Add Estimate Template</asp:Label></li>
                <li>
                    <asp:LinkButton ID="lnkSaveTemplate" OnClientClick="itemJSON();" runat="server"
                        ValidationGroup="templ" ToolTip="Save" CssClass="icon-save"
                        OnClick="lnkSaveTemplate_Click"></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lnkCloseTemplate" runat="server" CausesValidation="False" CssClass="icon-closed" ToolTip="Close"
                        OnClick="lnkCloseTemplate_Click"></asp:LinkButton></li>
                <li>
                    <asp:LinkButton ID="lnkUploadTemplate" OnClientClick="itemJSON();" runat="server"
                        ValidationGroup="templ" ToolTip="Upload Template" CssClass="icon-save"
                        OnClick="lnkUploadTemplate_Click"></asp:LinkButton></li>
            </ul>
        </div>
        <div class="ae-content">
            <asp:HiddenField ID="hdnItemJSON" runat="server" />
            <asp:HiddenField ID="hdnItemJSONPerc" runat="server" />
            <asp:Panel ID="pnlREPT" runat="server">
                <div class="col-lg-8 col-md-8">
                    <div class="form-col">
                        <div class="fc-label1">
                            Template Name
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="255"
                                AutoCompleteType="None"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtName"
                                Display="None" ErrorMessage="Name Required" SetFocusOnError="True" ValidationGroup="templ"></asp:RequiredFieldValidator>
                            <asp:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" Enabled="True"
                                TargetControlID="RequiredFieldValidator1">
                            </asp:ValidatorCalloutExtender>
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
                            <asp:TextBox ID="txtREPdesc" runat="server" CssClass="form-control" MaxLength="255"
                                AutoCompleteType="None"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Remarks
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtREPremarks" runat="server" CssClass="form-control" Rows="3"
                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-col">
                        <div class="fc-label1">
                            Upload Template
                        </div>
                        <div class="fc-input">
                            <asp:FileUpload id="FileUploadControl" runat="server" CssClass="form-control" />
                           <%-- <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Rows="3"
                                MaxLength="200" TextMode="MultiLine"></asp:TextBox>--%>
                        </div>
                    </div>
                    <div class="form-col">
                       
                        <div class="fc-label1">
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="form-control" style="cursor: pointer; background-color: #357ebd; color: white" />
                        </div>
                    </div>
                </div>
                <div class="clearfix"></div>
                <div class="row" style="padding-top: 10px">
                    <div class="col-md-4">
                        <asp:DropDownList ID="ddlBucket" CssClass="form-control" runat="server" onchange="AddNewBucket(this);">
                        </asp:DropDownList>
                    </div>

                    <div class="col-md-4">
                        <a id="btnnewlab" class="btn btn-primary form-control"
                            onclick="$('#<%= iframe1.ClientID %>').attr('src','addlaboritem.aspx');  $find('PMPBehaviour1').show();"
                            style="cursor: pointer; background-color: #357ebd; color: white">Manage Labor Items</a>
                    </div>
                </div>
                <div style="padding-top: 10px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="table-scrollable" style="border: none">
                                <asp:GridView ID="gvTemplateItems" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-bordered table-striped table-condensed flip-content" PageSize="20"
                                    ShowFooter="true" Width="100%">
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                    <RowStyle CssClass="evenrowcolor" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <a id="Button2" class="btn location-search"
                                                    onclick="DelRow('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');"
                                                    style="cursor: pointer;"><i class="fa fa-trash-o"></i>
                                                </a>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox CssClass="checker" ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <a id="Button1" class="addButton btn location-search" style="cursor: pointer;"><i class="fa fa-plus"></i>
                                                </a>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SNO" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndex" runat="server"
                                                    Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" Text='<%# Eval("code") %>'
                                                    Width="80px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtScope" runat="server" MaxLength="100" CssClass="form-control"
                                                    Text='<%# Eval("Scope") %>' Width="200px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-VerticalAlign="Middle" HeaderText="Vendor">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" Text='<%# Eval("Vendor") %>'
                                                    Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Quan" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtQuan" runat="server" CssClass="form-control"
                                                    Onblur="CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');"
                                                    onkeydown="NumericValid(event);" Text='<%# Eval("Quantity") %>' Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTQuan" runat="server" CssClass="form-control texttransparent"
                                                    onfocus="this.blur();" Width="50px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unit Cost" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control"
                                                    Onblur="CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');"
                                                    onkeydown="NumericValid(event);" Text='<%# Eval("Cost") %>' Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTUnitCost" runat="server" CssClass="form-control texttransparent"
                                                    onfocus="this.blur();" Width="50px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="$" Visible="true">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCurrency" runat="server" Width="100px" CssClass="form-control"
                                                    SelectedValue='<%# Eval("currency") %>'>
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem Value="US">US</asp:ListItem>
                                                    <asp:ListItem Value="CDN">CDN</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-VerticalAlign="Middle" HeaderText="Measure">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlMeasure" runat="server" CssClass="form-control" Width="100px"
                                                    onchange="CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItems');CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');"
                                                    SelectedValue='<%# Eval("measure") == DBNull.Value ? 1 :Eval("measure") %>'>
                                                    <asp:ListItem Value="1">Per Unit</asp:ListItem>
                                                    <asp:ListItem Value="2">Flat Rate</asp:ListItem>
                                                    <%--<asp:ListItem Value="3">Percentage</asp:ListItem>--%>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <FooterStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTotal" runat="server" onkeydown="NumericValid(event);" CssClass="form-control"
                                                    ReadOnly="true" Text='<%# Eval("amount") %>' Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTTotal" runat="server" CssClass="texttransparent"
                                                    onfocus="this.blur();" Width="50px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div class="table-scrollable" style="border: none">
                                <asp:GridView ID="gvTemplateItemsPercentage" runat="server" AutoGenerateColumns="False"
                                    CssClass="table table-bordered table-striped table-condensed flip-content" PageSize="20" ShowFooter="true" Width="100%">
                                    <AlternatingRowStyle CssClass="oddrowcolor" />
                                    <FooterStyle CssClass="footer" />
                                    <RowStyle CssClass="evenrowcolor" />
                                    <SelectedRowStyle CssClass="selectedrowcolor" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <a id="Button2" style="cursor: pointer;" class="btn location-search" onclick="DelRow('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');">
                                                    <i class="fa fa-trash-o"></i></a>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <a id="Button1" style="cursor: pointer;" class="addButton btn location-search" title="Add new row">
                                                    <i class="fa fa-plus"></i></a>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SNO" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblIndex" runat="server" Text="<%# Container.DataItemIndex +1 %>"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Code">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtCode" CssClass="form-control" runat="server" Width="80px" Text='<%# Eval("code") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtScope" CssClass="form-control" MaxLength="100" runat="server" Text='<%# Eval("Scope") %>'
                                                    Width="200px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Vendor" FooterStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control" Text='<%# Eval("Vendor") %>' Width="100px"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Percentage" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox Width="50px" onkeydown="NumericValid(event);" Onblur="CalculatePercentage('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_gvTemplateItemsPercentage');"
                                                    ID="txtQuan" runat="server" CssClass="form-control" Text='<%# Eval("Quantity") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox Width="50px" ID="txtTQuan" runat="server" CssClass="texttransparent"
                                                    onfocus="this.blur();"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Subtotal" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtUnitCost" runat="server" CssClass="form-control"
                                                    Text='<%# Eval("Cost") %>' Width="50px"></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox ID="txtTUnitCost" runat="server" CssClass="texttransparent"
                                                    onfocus="this.blur();" Width="50px"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="$" Visible="true">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlCurrency" CssClass="form-control" Width="100px" runat="server" SelectedValue='<%# Eval("currency") %>'>
                                                    <asp:ListItem Value="">Select</asp:ListItem>
                                                    <asp:ListItem Value="US">US</asp:ListItem>
                                                    <asp:ListItem Value="CDN">CDN</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterStyle-VerticalAlign="Middle" HeaderText="Measure">
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlMeasure" runat="server" CssClass="form-control" Width="100px"
                                                    SelectedValue='<%# Eval("measure") == DBNull.Value ? 3 :Eval("measure") %>'>
                                                    <%-- <asp:ListItem Value="1">Per Unit</asp:ListItem>
                                                            <asp:ListItem Value="2">Flat Rate</asp:ListItem>--%>
                                                    <asp:ListItem Value="3">Percentage</asp:ListItem>
                                                </asp:DropDownList>
                                            </ItemTemplate>
                                            <FooterStyle VerticalAlign="Middle" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total" Visible="true">
                                            <ItemTemplate>
                                                <asp:TextBox Width="50px" onkeydown="NumericValid(event);" ID="txtTotal" runat="server" CssClass="form-control"
                                                    ReadOnly="true" Text='<%# Eval("amount") %>'></asp:TextBox>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:TextBox Width="50px" ID="txtTTotal" runat="server" CssClass="texttransparent"
                                                    onfocus="this.blur();"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <div>
                                <asp:Button ID="hideModalPopupViaServer" OnClientClick="itemJSON();" runat="server"
                                    Style="display: none" OnClick="hideModalPopupViaServer_Click" />
                                <asp:ImageButton ID="btnInsertBuck" Width="15px" OnClientClick="itemJSON();" OnClick="btnInsertBuck_Click"
                                    ToolTip="Insert" runat="server" ImageUrl="images/update.png" Style="display: none;" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup" Style="display: none"
                CausesValidation="False" />
            <asp:Button runat="server" ID="hiddenTargetControlForModalPopup2" Style="display: none"
                CausesValidation="False" />
            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender1" BehaviorID="PMPBehaviour"
                TargetControlID="hiddenTargetControlForModalPopup" PopupControlID="Panel1"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="Panel1" Style="display: none; background: #fff; border: 1px solid #316b9d;">
                <div>
                    <iframe id="iframe" runat="server" scrolling="no" frameborder="0" class="iframe-bucket1"></iframe>
                </div>
            </asp:Panel>
            <asp:ModalPopupExtender runat="server" ID="ModalPopupExtender2" BehaviorID="PMPBehaviour1"
                TargetControlID="hiddenTargetControlForModalPopup2" PopupControlID="Panel2" BackgroundCssClass="pnlUpdateoverlay"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </asp:ModalPopupExtender>
            <asp:Panel runat="server" ID="Panel2" Style="display: none; background: #fff; border: 1px solid #316b9d;">
                <div>
                    <iframe id="iframe1" runat="server" scrolling="no" frameborder="0" class="iframe-bucket1"></iframe>
                </div>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
