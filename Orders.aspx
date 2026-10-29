<%@ Page Title="Orders" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Orders" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    Runat="Server">

<style type="text/css">

.orders-page
{
    background-color: white;
    margin: 30px;
    padding: 30px;
    border-radius: 10px;
    box-shadow: 0 2px 10px #ddd;
    box-sizing: border-box;
}

.orders-page h1
{
    color: #0b3d91;
    margin-bottom: 25px;
}

.orders-table
{
    width: 100%;
    border-collapse: collapse;
    box-sizing: border-box;
}

.orders-table th
{
    background-color: #0b3d91;
    color: white;
    padding: 12px;
    text-align: center;
}

.orders-table td
{
    padding: 12px;
    border-bottom: 1px solid #ddd;
    text-align: center;
}

.orders-table tr:hover td
{
    background-color: #f5f7fa;
}

</style>

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">

<div class="orders-page">

    <h1>📦 Orders</h1>

    <asp:GridView
        ID="GridView1"
        runat="server"
        AutoGenerateColumns="False"
        CssClass="orders-table">

        <Columns>

            <asp:BoundField
                DataField="OrderID"
                HeaderText="Order ID" />

            <asp:BoundField
                DataField="CustomerName"
                HeaderText="Customer" />

            <asp:BoundField
                DataField="ProductName"
                HeaderText="Product" />

            <asp:BoundField
                DataField="Price"
                HeaderText="Price" />

            <asp:BoundField
                DataField="Quantity"
                HeaderText="Quantity" />

            <asp:BoundField
                DataField="Total"
                HeaderText="Total" />

            <asp:BoundField
                DataField="OrderDate"
                HeaderText="Order Date"
                DataFormatString="{0:dd-MM-yyyy hh:mm tt}" />

            <asp:BoundField
                DataField="PaymentMethod"
                HeaderText="Payment" />

            <asp:BoundField
                DataField="OrderStatus"
                HeaderText="Status" />

        </Columns>

    </asp:GridView>

</div>

</asp:Content>