<%@ Page Language="C#" MasterPageFile="~/Popup.master" AutoEventWireup="true" CodeFile="AddLaborItem.aspx.cs"
    Inherits="AddLaborItem" Title="Untitled Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-content">
        <div class="row">
            <div class="col-lg-12 col-md-12">
                <div class="pc-title" style="margin-top: 0 !important">
                    <asp:Label CssClass="title_text" ID="Label1" runat="server">Labor Items</asp:Label>
                    <div class="pt-right">
                        <asp:LinkButton ID="lnkCancel" runat="server" CausesValidation="False" CssClass="popup-anchor"
                            TabIndex="5"
                            OnClientClick=" window.parent.document.getElementById('ctl00_ctl00_ContentPlaceHolder1_ContentPlaceHolder1_hideModalPopupViaServer').click();">Close</asp:LinkButton>
                    </div>
                </div>
            </div>
            <!-- edit-tab start -->
            <div class="col-lg-12 col-md-12">
                <div class="com-cont">
                    <div style="overflow-y: scroll; max-height: 250px">
                        <asp:GridView ID="gvLaborItems" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-striped table-condensed flip-content"
                            PageSize="20" ShowFooter="true">
                            <AlternatingRowStyle CssClass="oddrowcolor" />
                            <FooterStyle CssClass="footer" />
                            <RowStyle CssClass="evenrowcolor" />
                            <SelectedRowStyle CssClass="selectedrowcolor" />
                            <Columns>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        <asp:TextBox ID="txtName" CssClass="form-control" MaxLength="100" runat="server" Text='<%# Eval("item") %>'
                                            Width="170px" ReadOnly="true"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtName" CssClass="form-control" placeholder="Enter Item Name" MaxLength="100" runat="server"
                                            Text='<%# Eval("item") %>' Width="170px"></asp:TextBox>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rate" FooterStyle-VerticalAlign="Middle">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAmount" CssClass="form-control" runat="server" Text='<%# Eval("amount") %>' Width="100px"></asp:TextBox>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtAmount" CssClass="form-control" placeholder="Enter Item Rate" runat="server" Text='<%# Eval("amount") %>'
                                            Width="100px"></asp:TextBox>
                                    </FooterTemplate>
                                    <FooterStyle VerticalAlign="Middle"></FooterStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgSave" runat="server" ImageUrl="images/save.png" CssClass="btn location-search"
                                            OnClick="imgSave_Click" OnClientClick="return confirm('Do you want to save the item? This will affect the rate on all the templates.');"><i class="fa fa-save"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <div class="footerclass">
                                            <asp:LinkButton ID="imgAdd" runat="server" CssClass="btn location-search"
                                            OnClick="imgAdd_Click"><i class="fa fa-plus"></i></asp:LinkButton>
                                        </div>                                        
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="imgDel" runat="server" ForeColor="Red" CssClass="btn location-search"
                                            OnClick="imgDel_Click" OnClientClick="return confirm('Do you want to delete the item? This will delete the item from all the templates.');"><i class="fa fa-trash-o"></i></asp:LinkButton>
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

