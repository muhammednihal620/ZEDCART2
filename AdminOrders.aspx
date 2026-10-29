
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="AdminOrders.aspx.cs"
    Inherits="AdminOrders" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin Orders - CHEEPCART</title>

    <style type="text/css">
        body
        {
            margin: 0;
            padding: 0;
            font-family: Arial;
            background-color: #f5f7fa;
        }

        .header
        {
            background-color: #0b3d91;
            color: white;
            padding: 18px 30px;
            overflow: hidden;
        }

        .header-title
        {
            float: left;
            font-size: 25px;
            font-weight: bold;
            margin-top: 7px;
        }

        .logout
        {
            float: right;
            background-color: #e53935;
            color: white;
            padding: 9px 18px;
            text-decoration: none;
            border-radius: 5px;
            margin-left: 10px;
        }

        .back-btn
        {
            float: right;
            background-color: #ffffff;
            color: #0b3d91;
            padding: 9px 18px;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            margin-left: 10px;
        }

        .container
        {
            width: 95%;
            margin: 30px auto;
        }

        .page-title
        {
            color: #0b3d91;
            margin-bottom: 20px;
        }

        .search-box
        {
            background-color: white;
            padding: 15px;
            margin-bottom: 20px;
            border-radius: 8px;
        }

        .search-textbox
        {
            width: 250px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .search-btn
        {
            padding: 10px 20px;
            background-color: #1976d2;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .all-btn
        {
            padding: 10px 20px;
            background-color: #777;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
        }

        .orders-grid
        {
            width: 100%;
            background-color: white;
            border-collapse: collapse;
        }

        .orders-grid th
        {
            background-color: #0b3d91;
            color: white;
            padding: 12px;
        }

        .orders-grid td
        {
            padding: 10px;
            border-bottom: 1px solid #ddd;
        }

        .status-dropdown
        {
            padding: 7px;
            border: 1px solid #ccc;
            border-radius: 4px;
        }

        .update-btn
        {
            background-color: #1976d2;
            color: white;
            border: none;
            padding: 7px 12px;
            border-radius: 4px;
            cursor: pointer;
        }

        .message
        {
            display: block;
            margin-bottom: 15px;
            font-weight: bold;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

    <div class="header">

        </div>

        <asp:LinkButton
            ID="btnLogout"
            runat="server"
            CssClass="logout"
            OnClick="btnLogout_Click">
            Logout
        </asp:LinkButton>
        <div class="header-title">
            CHEEPCART - Admin Orders

          
<asp:Button
    ID="btnBack"
    runat="server"
    Text="Back to Dashboard"
    CssClass="back-btn"
    CausesValidation="false"
    OnClick="btnBack_Click" />

    </div>

    <div class="container">

        <h2 class="page-title">Customer Orders</h2>

        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="message">
        </asp:Label>

        <div class="search-box">

            <asp:TextBox
                ID="txtSearchOrderID"
                runat="server"
                CssClass="search-textbox"
                MaxLength="10">
            </asp:TextBox>

            <asp:Button
                ID="btnSearch"
                runat="server"
                Text="Search"
                CssClass="search-btn"
                CausesValidation="false"
                OnClick="btnSearch_Click" />

            <asp:Button
                ID="btnShowAll"
                runat="server"
                Text="Show All"
                CssClass="all-btn"
                CausesValidation="false"
                OnClick="btnShowAll_Click" />

        </div>

        <asp:GridView
            ID="GridView1"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="orders-grid"
            OnRowCommand="GridView1_RowCommand"
            OnRowDataBound="GridView1_RowDataBound"
            EmptyDataText="No customer orders found.">

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

                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>

                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            CssClass="status-dropdown">

                            <asp:ListItem Text="Placed"
                                Value="Placed" />

                            <asp:ListItem Text="Confirmed"
                                Value="Confirmed" />

                            <asp:ListItem Text="Preparing"
                                Value="Preparing" />

                            <asp:ListItem Text="Shipped"
                                Value="Shipped" />

                            <asp:ListItem Text="Delivered"
                                Value="Delivered" />

                            <asp:ListItem Text="Cancelled"
                                Value="Cancelled" />

                        </asp:DropDownList>

                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>

                        <asp:Button
                            ID="btnUpdate"
                            runat="server"
                            Text="Update"
                            CssClass="update-btn"
                            CausesValidation="false"
                            CommandName="UpdateStatus"
                            CommandArgument='<%# Eval("OrderID") %>' />

                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>

        </asp:GridView>

    </div>

</form>
</body>
</html>
