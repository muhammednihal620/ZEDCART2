<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="AdminDashboard.aspx.cs"
    Inherits="AdminDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Admin Dashboard</title>

    <style type="text/css">

        body
        {
            margin: 0;
            font-family: Arial;
            background: #f5f7fa;
        }

        .header
        {
            background: #0b3d91;
            color: white;
            padding: 18px 30px;
            overflow: hidden;
        }

        .title
        {
            float: left;
            font-size: 24px;
            font-weight: bold;
        }

        .logout
        {
            float: right;
            color: white;
            text-decoration: none;
            padding: 8px 15px;
            background: #d32f2f;
            border-radius: 5px;
        }

        .container
        {
            width: 90%;
            margin: 35px auto;
        }

        .welcome
        {
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 25px;
            color: #333;
        }

        .cards
        {
            overflow: hidden;
        }

        .card
        {
            float: left;
            width: 21%;
            margin-right: 3%;
            margin-bottom: 25px;
            background: white;
            padding: 25px;
            box-sizing: border-box;
            border-radius: 10px;
            box-shadow: 0 2px 8px #cccccc;
            text-align: center;
        }

        .number
        {
            font-size: 32px;
            font-weight: bold;
            color: #0b3d91;
            display: block;
            margin-bottom: 10px;
        }

        .label
        {
            font-size: 16px;
            color: #555;
        }

        .revenue
        {
            color: #198754;
        }

        .recent-title
        {
            margin-top: 40px;
            margin-bottom: 15px;
            color: #333;
        }

        .recent-grid
        {
            width: 100%;
            background: white;
            border-collapse: collapse;
            box-shadow: 0 2px 8px #cccccc;
        }

        .recent-grid th
        {
            background: #0b3d91;
            color: white;
            padding: 12px;
            text-align: left;
        }

        .recent-grid td
        {
            padding: 12px;
            border-bottom: 1px solid #dddddd;
        }

        .orders-btn
        {
            display: inline-block;
            margin-top: 35px;
            background: #0b3d91;
            color: white;
            padding: 12px 25px;
            text-decoration: none;
            border-radius: 5px;
        }
        .status-summary
{
    background: white;
    padding: 25px;
    border-radius: 10px;
    box-shadow: 0 2px 8px #cccccc;
}

.status-row
{
    margin-bottom: 20px;
}

.status-row > span:first-child
{
    display: inline-block;
    width: 90px;
    font-weight: bold;
    color: #555;
}

.bar
{
    display: inline-block;
    width: 65%;
    height: 18px;
    background: #eeeeee;
    border-radius: 10px;
    overflow: hidden;
    vertical-align: middle;
}

.bar-fill
{
    height: 100%;
    width: 0%;
}

.placed
{
    background: #ffc107;
}

.preparing
{
    background: #0d6efd;
}

.shipped
{
    background: #6f42c1;
}

.delivered
{
    background: #198754;
}

.cancelled
{
    background: #dc3545;
}

.percent
{
    margin-left: 10px;
    font-weight: bold;
    color: #555;
}

    </style>

</head>

<body>

<form id="form1" runat="server">

    <div class="header">

        <div class="title">
            CheepCart - Admin Panel
        </div>

        <asp:LinkButton ID="btnLogout"
            runat="server"
            CssClass="logout"
            OnClick="btnLogout_Click">
            Logout
        </asp:LinkButton>

    </div>


    <div class="container">

        <div class="welcome">
            Admin Dashboard
        </div>


        <!-- STATISTICS -->

        <div class="cards">

            <div class="card">

                <asp:Label ID="lblTotalOrders"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Total Orders
                </span>

            </div>


            <div class="card">

                <asp:Label ID="lblPlaced"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Placed
                </span>

            </div>


            <div class="card">

                <asp:Label ID="lblShipped"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Shipped
                </span>

            </div>


            <div class="card">

                <asp:Label ID="lblDelivered"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Delivered
                </span>

            </div>


            <div class="card">

                <asp:Label ID="lblPreparing"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Preparing
                </span>

            </div>


            <div class="card">

                <asp:Label ID="lblCancelled"
                    runat="server"
                    CssClass="number"
                    Text="0">
                </asp:Label>

                <span class="label">
                    Cancelled
                </span>

            </div>


            <!-- TOTAL REVENUE -->

            <div class="card">

                
                  ₹<asp:Label ID="lblRevenue"
                    runat="server"
                    CssClass="number revenue"
                    Text="0.00">
                </asp:Label>

                <span class="label">
                    Total Revenue
                </span>

            </div>
            <!-- TODAY ORDERS -->

<div class="card">

    <asp:Label ID="lblTodayOrders"
        runat="server"
        CssClass="number"
        Text="0">
    </asp:Label>

    <span class="label">
        Today's Orders
    </span>

</div>


<!-- TODAY REVENUE -->

<div class="card">

    <span class="number revenue">
        ₹<asp:Label ID="lblTodayRevenue"
            runat="server"
            Text="0.00">
        </asp:Label>
    </span>

    <span class="label">
        Today's Revenue
    </span>

</div>
<!-- ORDER STATUS SUMMARY -->

<h2 class="recent-title">
    Order Status Summary
</h2>

<div class="status-summary">

    <div class="status-row">
        <span>Placed</span>

        <div class="bar">
            <div id="barPlaced"
                runat="server"
                class="bar-fill placed">
            </div>
        </div>

        <asp:Label ID="lblPlacedPercent"
            runat="server"
            CssClass="percent">
        </asp:Label>
    </div>


    <div class="status-row">
        <span>Preparing</span>

        <div class="bar">
            <div id="barPreparing"
                runat="server"
                class="bar-fill preparing">
            </div>
        </div>

        <asp:Label ID="lblPreparingPercent"
            runat="server"
            CssClass="percent">
        </asp:Label>
    </div>


    <div class="status-row">
        <span>Shipped</span>

        <div class="bar">
            <div id="barShipped"
                runat="server"
                class="bar-fill shipped">
            </div>
        </div>

        <asp:Label ID="lblShippedPercent"
            runat="server"
            CssClass="percent">
        </asp:Label>
    </div>


    <div class="status-row">
        <span>Delivered</span>

        <div class="bar">
            <div id="barDelivered"
                runat="server"
                class="bar-fill delivered">
            </div>
        </div>

        <asp:Label ID="lblDeliveredPercent"
            runat="server"
            CssClass="percent">
        </asp:Label>
    </div>


    <div class="status-row">
        <span>Cancelled</span>

        <div class="bar">
            <div id="barCancelled"
                runat="server"
                class="bar-fill cancelled">
            </div>
        </div>

        <asp:Label ID="lblCancelledPercent"
            runat="server"
            CssClass="percent">
        </asp:Label>
    </div>

</div>

        </div>


        <!-- RECENT ORDERS -->

        <h2 class="recent-title">
            Recent Orders
        </h2>


        <asp:GridView ID="GridViewRecent"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="recent-grid">

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
                    DataField="Total"
                    HeaderText="Total" />

                <asp:BoundField
                    DataField="OrderStatus"
                    HeaderText="Status" />

            </Columns>

        </asp:GridView>


       
<!-- ADD PRODUCT BUTTON -->

<a href="AdminProducts.aspx"
    class="orders-btn">
    + Add Product
</a>

<!-- MANAGE PRODUCTS BUTTON -->

<a href="AdminProducts.aspx"
    class="orders-btn">
    Manage Products / Add Variants
</a>

<!-- MANAGE ORDERS BUTTON -->

<a href="AdminOrders.aspx"
    class="orders-btn">
    Manage Orders
</a>




    </div>

</form>

</body>

</html>