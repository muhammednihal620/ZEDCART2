<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Cart.aspx.cs" Inherits="Cart" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    Runat="Server">

    <style type="text/css">

        .cart-container {
            width: 90%;
            margin: 40px auto;
            background-color: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 2px 10px #ddd;
        }

        .cart-title {
            text-align: center;
            color: #0b3d91;
            margin-bottom: 30px;
        }

        .cart-table {
            width: 100%;
            border-collapse: collapse;
        }

        .cart-table th {
            background-color: #0b3d91;
            color: white;
            padding: 12px;
        }

        .cart-table td {
            padding: 12px;
            text-align: center;
            border-bottom: 1px solid #ddd;
        }

        .cart-table a {
            text-decoration: none;
            font-weight: bold;
            font-size: 18px;
        }

        .remove-btn {
            color: red;
            font-size: 14px !important;
        }

        .grand-total {
            text-align: right;
            margin-top: 25px;
            font-size: 22px;
            font-weight: bold;
            color: #0b3d91;
        }

        .checkout-btn {
            display: inline-block;
            margin-top: 20px;
            padding: 12px 25px;
            background-color: #ffcc00;
            color: black;
            text-decoration: none;
            border-radius: 6px;
            font-weight: bold;
        }

        .checkout-btn:hover {
            background-color: #0b3d91;
            color: white;
        }

        .empty-cart {
            display: block;
            text-align: center;
            color: #777;
            font-size: 20px;
            padding: 30px;
        }

    </style>

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">

    <div class="cart-container">

        <h2 class="cart-title">
            🛒 Shopping Cart
        </h2>

        <asp:Label ID="Label1"
            runat="server"
            CssClass="empty-cart">
        </asp:Label>

        <asp:GridView ID="GridView1"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="cart-table"
            OnRowCommand="GridView1_RowCommand">

            <Columns>

                <asp:BoundField
                    DataField="ProductName"
                    HeaderText="Product" />

                <asp:BoundField
                    DataField="Price"
                    HeaderText="Price" />

                <asp:TemplateField HeaderText="Quantity">

                    <ItemTemplate>

                        <asp:LinkButton
                            ID="btnMinus"
                            runat="server"
                            Text="−"
                            CommandName="Minus"
                            CommandArgument='<%# Eval("ProductID") %>'>
                        </asp:LinkButton>

                        &nbsp;

                        <asp:Label
                            ID="lblQuantity"
                            runat="server"
                            Text='<%# Eval("Quantity") %>'>
                        </asp:Label>

                        &nbsp;

                        <asp:LinkButton
                            ID="btnPlus"
                            runat="server"
                            Text="+"
                            CommandName="Plus"
                            CommandArgument='<%# Eval("ProductID") %>'>
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

                <asp:BoundField
                    DataField="Total"
                    HeaderText="Total" />

                <asp:TemplateField HeaderText="Action">

                    <ItemTemplate>

                        <asp:LinkButton
                            ID="btnRemove"
                            runat="server"
                            Text="Remove"
                            CssClass="remove-btn"
                            CommandName="RemoveProduct"
                            CommandArgument='<%# Eval("ProductID") %>'>
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>

        </asp:GridView>

        <div class="grand-total">

            Grand Total: ₹

            <asp:Label
                ID="lblGrandTotal"
                runat="server"
                Text="0">
            </asp:Label>

        </div>

        <div style="text-align:right;">

            <a href="Checkout.aspx"
                class="checkout-btn">
                Proceed to Checkout
            </a>

        </div>

    </div>

</asp:Content>