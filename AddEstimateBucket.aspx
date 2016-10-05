<%@ Page Language="C#" MasterPageFile="~/Popup.master" AutoEventWireup="true" CodeFile="AddEstimateBucket.aspx.cs"
    Inherits="AddEstimateBucket" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">

        function pageLoad() {
            $("#ctl00_ContentPlaceHolder1_gvBucketItems").on('click', 'a.addButton', function () {
                var $tr = $(this).closest('table').find('tr').eq(1);
                var $clone = $tr.clone();
                $clone.find('input:text').val('');
                $clone.insertAfter($tr.closest('table').find('tr').eq($tr.closest('table').find('tr').length - 2));
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

            var rowone = $("#ctl00_ContentPlaceHolder1_gvBucketItems").find('tr').eq(1);
            $("input", rowone).each(function () {
                $(this).blur();
            });

            $("#ctl00_ContentPlaceHolder1_gvBucketItems").find('tr:not(:first, :last)').each(function () {
                var ddlMeasure = $(this).find('select[id*=ddlMeasure]')
                Measure(ddlMeasure);
            });
        }

        function DelRow() {
            if ($("#ctl00_ContentPlaceHolder1_gvBucketItems").find('input[type="checkbox"]:checked').length == 0) {
                alert('Please select items to delete.');
                return;
            }
            var con = confirm('Are you sure you want to delete the items?');
            if (con == true) {
                $("#ctl00_ContentPlaceHolder1_gvBucketItems").find('tr').each(function () {
                    var $tr = $(this);
                    $tr.find('input[type="checkbox"]:checked').each(function () {
                        if ($("#ctl00_ContentPlaceHolder1_gvBucketItems").find('tr').length > 3) {
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
            var rawData = $('#ctl00_ContentPlaceHolder1_gvBucketItems').serializeFormJSON();
            var formData = JSON.stringify(rawData);
            $('#<%=hdnItemJSON.ClientID%>').val(formData);
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

        function Measure(ddlMeasure) {
            var txtUnit = $(ddlMeasure).closest('tr').find('input[id*=txtUnit]');
            if ($(ddlMeasure).val() == "2") {
                txtUnit.val("1");
                txtUnit.attr("readonly", "true");
            }
            else {
                txtUnit.removeAttr("readonly");
            }
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title" style="margin-top: 0 !important">
                    <asp:Label CssClass="title_text" ID="Label1" runat="server">Add/Edit Bucket</asp:Label>
                    <div class="pt-right">
                        <asp:LinkButton ID="lnkSave" runat="server" Height="16px" CssClass="popup-anchor"
                            OnClick="lnkContactSave_Click" TabIndex="4"
                            OnClientClick="itemJSON();">Save</asp:LinkButton>
                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CssClass="popup-anchor"
                            TabIndex="5"
                            OnClientClick=" window.parent.document.getElementById('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_hideModalPopupViaServer').click();">Close</asp:LinkButton>
                    </div>
                </div>
            </div>

            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <asp:HiddenField ID="hdnItemJSON" runat="server" />
                    <div class="form-col">
                        <div class="fc-label2">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="txtName"
                                Display="None" ErrorMessage="Required" SetFocusOnError="True"></asp:RequiredFieldValidator><asp:ValidatorCalloutExtender
                                    ID="RequiredFieldValidator36_ValidatorCalloutExtender" runat="server" Enabled="True" PopupPosition="BottomLeft"
                                    TargetControlID="RequiredFieldValidator36">
                                </asp:ValidatorCalloutExtender>
                            Name
                        </div>
                        <div class="fc-input">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" TabIndex="1"></asp:TextBox>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                    <div class="table-scrollable">
                        <asp:GridView ID="gvBucketItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                            PageSize="20" ShowFooter="true">
                            <AlternatingRowStyle CssClass="oddrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <RowStyle CssClass="evenrowcolor" />
                            <SelectedRowStyle CssClass="selectedrowcolor" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <a id="Button2" style="cursor: pointer;" class="btn location-search" onclick="DelRow();">
                                            <i class="fa fa-trash-o"></i></a>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <a id="Button1" style="cursor: pointer;" class="btn location-search" title="Add new row">
                                            <i class="fa fa-plus"></i></a>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Code" FooterStyle-VerticalAlign="Middle">
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
                                        <asp:TextBox ID="txtVendor" CssClass="form-control" runat="server" Text='<%# Eval("Vendor") %>' Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unit" FooterStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtUnit" CssClass="form-control" runat="server" onkeydown="NumericValid(event);" Width="100px"
                                            Text='<%# Eval("unit") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cost" FooterStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtCost" CssClass="form-control" runat="server" onkeydown="NumericValid(event);" Width="100px"
                                            Text='<%# Eval("cost") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Measure" FooterStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlMeasure" CssClass="form-control" runat="server" SelectedValue='<%# Eval("measure") %>'
                                            onchange="Measure(this);">
                                            <asp:ListItem Value="1">Per Unit</asp:ListItem>
                                            <asp:ListItem Value="2">Flat Rate</asp:ListItem>
                                            <asp:ListItem Value="3">Percentage</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <!-- edit-tab end -->
            <div class="clearfix"></div>
        </div>
        <!-- END DASHBOARD STATS -->
        <div class="clearfix"></div>
    </div>
</asp:Content>
