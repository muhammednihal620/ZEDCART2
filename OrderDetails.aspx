<%@ Page Title="Order Details" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="OrderDetails.aspx.cs" Inherits="OrderDetails" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    Runat="Server">

<style type="text/css">

.order-details-page
{
    background-color: #f5f7fa;
    padding: 30px;
    margin-top: 20px;
    border-radius: 10px;
}

.order-title
{
    color: #0b3d91;
    font-size: 30px;
    margin-bottom: 5px;
}

.order-subtitle
{
    color: #777;
    margin-bottom: 25px;
}

.order-box
{
    background-color: white;
    padding: 25px;
    border-radius: 10px;
    box-shadow: 0 2px 10px #ddd;
    max-width: 900px;
}

.order-heading
{
    color: #0b3d91;
    font-size: 21px;
    font-weight: bold;
    border-bottom: 1px solid #ddd;
    padding-bottom: 10px;
    margin-bottom: 20px;
}

.detail-row
{
    display: table;
    width: 100%;
    margin-bottom: 15px;
}

.detail-label
{
    display: table-cell;
    width: 180px;
    font-weight: bold;
    color: #555;
}

.detail-value
{
    display: table-cell;
    color: #222;
}

.total
{
    color: #e53935;
    font-size: 22px;
    font-weight: bold;
}

.status
{
    color: #0b8f3c;
    font-weight: bold;
}


/* DELIVERY STATUS */

.status-box
{
    background-color: #f8fbff;
    border: 1px solid #e0e6ef;
    padding: 20px;
    border-radius: 8px;
    margin-bottom: 25px;
}

.status-item
{
    display: flex;
    align-items: center;
}

.status-circle
{
    width: 34px;
    height: 34px;
    line-height: 34px;
    text-align: center;
    border-radius: 50%;
    background-color: #cccccc;
    color: white;
    font-weight: bold;
    margin-right: 15px;
}

.status-item.active .status-circle
{
    background-color: #28a745;
}

.status-text
{
    display: flex;
    flex-direction: column;
}

.status-text b
{
    color: #333;
    font-size: 15px;
}

.status-text span
{
    color: #777;
    font-size: 13px;
    margin-top: 3px;
}

.status-line
{
    height: 22px;
    border-left: 2px solid #cccccc;
    margin-left: 16px;
}


/* CANCELLED STATUS */

.cancelled-box
{
    background-color: #fff5f5;
    border: 1px solid #f1b5b5;
    padding: 25px;
    border-radius: 8px;
    text-align: center;
    margin-bottom: 25px;
}

.cancelled-icon
{
    width: 50px;
    height: 50px;
    line-height: 50px;
    border-radius: 50%;
    background-color: #e53935;
    color: white;
    font-size: 25px;
    font-weight: bold;
    margin: 0 auto 12px auto;
}

.cancelled-title
{
    color: #e53935;
    font-size: 22px;
    font-weight: bold;
}

.cancelled-text
{
    color: #777;
    margin-top: 8px;
    font-size: 14px;
}


/* BACK BUTTON */

.back-btn
{
    display: inline-block;
    margin-top: 20px;
    padding: 10px 18px;
    background-color: #0b3d91;
    color: white;
    text-decoration: none;
    border-radius: 5px;
}

.back-btn:hover
{
    background-color: #082f70;
}

</style>

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">

<div class="order-details-page">

    <h1 class="order-title">
        📦 Order Details
    </h1>

    <p class="order-subtitle">
        View your order information
    </p>


    <div class="order-box">


        <!-- ORDER INFORMATION -->

        <div class="order-heading">
            Order Information
        </div>


        <div class="detail-row">

            <span class="detail-label">
                Order ID
            </span>

            <asp:Label ID="lblOrderID"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Product
            </span>

            <asp:Label ID="lblProduct"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Price
            </span>

            <asp:Label ID="lblPrice"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Quantity
            </span>

            <asp:Label ID="lblQuantity"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Total Amount
            </span>

            <asp:Label ID="lblTotal"
                runat="server"
                CssClass="detail-value total">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Order Date
            </span>

            <asp:Label ID="lblOrderDate"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Payment
            </span>

            <asp:Label ID="lblPayment"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Order Status
            </span>

            <asp:Label ID="lblStatus"
                runat="server"
                CssClass="detail-value status">
            </asp:Label>

        </div>


        <!-- DELIVERY STATUS -->

        <div class="order-heading">
            Delivery Status
        </div>


        <!-- NORMAL ORDER STATUS -->

        <asp:Panel ID="pnlNormalStatus"
            runat="server">

            <div class="status-box">


                <div class="status-item active">

                    <div class="status-circle">
                        ✓
                    </div>

                    <div class="status-text">

                        <b>Order Placed</b>

                        <span>
                            Your order has been placed successfully
                        </span>

                    </div>

                </div>


                <div class="status-line">
                </div>


                <div id="stepPreparing"
    runat="server"
    class="status-item">

    <div class="status-circle">
        3
    </div>

    <div class="status-text">

        <b>Preparing</b>

        <span>
            Your order is being prepared
        </span>

    </div>

</div>

                <div class="status-line">
                </div>


                <div class="status-item">

                    <div class="status-circle">
                        3
                    </div>

                    <div class="status-text">

                        <b>Preparing</b>

                        <span>
                            Your order is being prepared
                        </span>

                    </div>

                </div>


                <div class="status-line">
                </div>


                <div id="stepShipped"
    runat="server"
    class="status-item">

    <div class="status-circle">
        4
    </div>

    <div class="status-text">

        <b>Shipped</b>

        <span>
            Your order will be shipped soon
        </span>

    </div>

</div>

                <div class="status-line">
                </div>


                <div id="stepDelivered"
    runat="server"
    class="status-item">

    <div class="status-circle">
        5
    </div>

    <div class="status-text">

        <b>Delivered</b>

        <span>
            Your order will be delivered to you
        </span>

    </div>

</div>

            </div>

        </asp:Panel>


        <!-- CANCELLED ORDER STATUS -->

        <asp:Panel ID="pnlCancelled"
            runat="server"
            Visible="false">

            <div class="cancelled-box">

                <div class="cancelled-icon">
                    ✕
                </div>

                <div class="cancelled-title">
                    Order Cancelled
                </div>

                <div class="cancelled-text">
                    This order has been cancelled successfully.
                </div>

            </div>

        </asp:Panel>


        <!-- DELIVERY DETAILS -->

        <div class="order-heading">
            Delivery Details
        </div>


        <div class="detail-row">

            <span class="detail-label">
                Customer Name
            </span>

            <asp:Label ID="lblCustomerName"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Phone
            </span>

            <asp:Label ID="lblPhone"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Address
            </span>

            <asp:Label ID="lblAddress"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Pincode
            </span>

            <asp:Label ID="lblPincode"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <div class="detail-row">

            <span class="detail-label">
                Landmark
            </span>

            <asp:Label ID="lblLandmark"
                runat="server"
                CssClass="detail-value">
            </asp:Label>

        </div>


        <!-- BACK -->

        <a href="MyOrders.aspx"
            class="back-btn">
            ← Back to My Orders
        </a>


    </div>

</div>

</asp:Content>