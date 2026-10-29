
<%@ Page Title="My Orders"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="MyOrders.aspx.cs"
    Inherits="MyOrders" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">

    <link href="MyOrders.css"
        rel="stylesheet"
        type="text/css" />

</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <div class="orders-page">
        <div class="orders-container">

            <h1>My Orders</h1>

            <!-- Product Search -->
            <div class="orders-toolbar">

                <asp:TextBox
                    ID="txtSearch"
                    runat="server"
                    CssClass="searchbox"
                    placeholder="Search product name">
                </asp:TextBox>

                <asp:Button
                    ID="btnSearch"
                    runat="server"
                    Text="Search"
                    CssClass="orders-btn"
                    OnClick="btnSearch_Click" />

                <asp:DropDownList
                    ID="ddlStatus"
                    runat="server"
                    CssClass="status-filter">

                    <asp:ListItem
                        Text="All Statuses"
                        Value="" />

                    <asp:ListItem
                        Text="Placed"
                        Value="Placed" />

                    <asp:ListItem
                        Text="Preparing"
                        Value="Preparing" />

                    <asp:ListItem
                        
                        Value="Shipped" />

                    <asp:ListItem
                        Text="Delivered"
                        Value="Delivered" />

                    <asp:ListItem
                        Text="Cancelled"
                        Value="Cancelled" />

                </asp:DropDownList>

                <asp:Button
                    ID="btnFilter"
                    runat="server"
                    Text="Filter Status"
                    CssClass="orders-btn"
                    OnClick="btnFilter_Click" />

            </div>

            <!-- Message -->
            <asp:Label
                ID="lblMessage"
                runat="server"
                CssClass="orders-message"
                EnableViewState="false">
            </asp:Label>

            <!-- Orders Table -->
            <div class="orders-table-wrapper">

                <asp:GridView
                    ID="GridView1"
                    runat="server"
                    CssClass="orders-grid"
                    AutoGenerateColumns="False"
                    DataKeyNames="OrderID"
                    EmptyDataText="No orders found."
                    OnRowCommand="GridView1_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="OrderID"
                            HeaderText="Order ID" />

                        <asp:BoundField
                            DataField="ProductName"
                            HeaderText="Product" />

                        <asp:BoundField
                            DataField="Price"
                            HeaderText="Price"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="Quantity"
                            HeaderText="Quantity" />

                        <asp:BoundField
                            DataField="Total"
                            HeaderText="Total"
                            DataFormatString="{0:N2}" />

                        <asp:BoundField
                            DataField="OrderDate"
                            HeaderText="Order Date"
                            DataFormatString="{0:dd-MM-yyyy}" />

                        <asp:BoundField
                            DataField="PaymentMethod"
                            HeaderText="Payment" />

                        <!-- Order Status -->
                        <asp:TemplateField
                            HeaderText="Order Status">

                            <ItemStyle
                                CssClass="status-cell" />

                            <ItemTemplate>

                                <span class='<%# "status-box status-" +
                                    Convert.ToString(
                                        Eval("OrderStatus")
                                    ).ToLowerInvariant() %>'>
                                    <%# Eval("OrderStatus") %>
                                </span>

                            </ItemTemplate>

                        </asp:TemplateField>

                        <!-- Actions -->
                        <asp:TemplateField
                            HeaderText="Action">

                            <ItemTemplate>

                                <asp:HyperLink
                                    ID="lnkInvoice"
                                    runat="server"
                                    Text="View Invoice"
                                    CssClass="invoice-link"
                                    NavigateUrl='<%# "InvoicePdf.aspx?OrderID=" +
                                        Eval("OrderID") %>'>
                                </asp:HyperLink>

                                <br />
                                <br />

                                <asp:LinkButton
                                    ID="btnCancel"
                                    runat="server"
                                    Text="Cancel Order"
                                    CssClass="cancel-link"
                                    CommandName="CancelOrder"
                                    CommandArgument='<%# Eval("OrderID") %>'
                                    CausesValidation="false">
                                </asp:LinkButton>

                            </ItemTemplate>

                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

            <!-- Back to Home -->
            <asp:HyperLink
                ID="lnkHome"
                runat="server"
                NavigateUrl="~/Default.aspx"
                Text="← Back to Home"
                CssClass="home-link">
            </asp:HyperLink>

            <!-- Cancel Order Hidden Field -->
            <asp:HiddenField
                ID="hfCancelOrderID"
                runat="server" />

            <!-- Cancel Confirmation Modal -->
            <asp:Panel
                ID="pnlCancelModal"
                runat="server"
                CssClass="modal-overlay"
                Visible="false">

                <div class="modal-box">

                    <h3>Cancel Order?</h3>

                    <asp:Label
                        ID="lblModalMessage"
                        runat="server"
                        Text="Do you want to cancel this order?">
                    </asp:Label>

                    <br />
                    <br />

                    <asp:Button
                        ID="btnConfirmCancel"
                        runat="server"
                        Text="Yes, Cancel"
                        CssClass="confirm-btn"
                        OnClick="btnConfirmCancel_Click"
                        CausesValidation="false" />

                    <asp:Button
                        ID="btnCloseModal"
                        runat="server"
                        Text="Close"
                        CssClass="close-btn"
                        CausesValidation="false"
                        OnClick="btnCloseModal_Click" />

                </div>

            </asp:Panel>

        </div>
    </div>

</asp:Content>
