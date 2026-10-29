using System;
using System.Data.SqlClient;

public partial class OrderDetails : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        con = new SqlConnection(
            "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;Initial Catalog=CheepCart;Integrated Security=True"
        );

        if (Session["CustomerID"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadOrderDetails();
        }
    }


    protected void LoadOrderDetails()
    {
        string orderID = Request.QueryString["id"];

        if (string.IsNullOrEmpty(orderID))
        {
            Response.Redirect("MyOrders.aspx");
            return;
        }


        con.Open();


        SqlCommand cmd = new SqlCommand(
            "SELECT OrderID, CustomerName, Address, Phone, " +
            "Pincode, Landmark, ProductName, Price, Quantity, " +
            "Total, OrderDate, PaymentMethod, OrderStatus " +
            "FROM OrderDetails " +
            "WHERE OrderID=@OrderID " +
            "AND CustomerID=@CustomerID",
            con
        );


        cmd.Parameters.AddWithValue(
            "@OrderID",
            Convert.ToInt32(orderID)
        );


        cmd.Parameters.AddWithValue(
            "@CustomerID",
            Convert.ToInt32(Session["CustomerID"])
        );


        SqlDataReader dr = cmd.ExecuteReader();


        if (dr.Read())
        {
            lblOrderID.Text =
                dr["OrderID"].ToString();

            lblProduct.Text =
                dr["ProductName"].ToString();

            lblPrice.Text =
                "₹" + dr["Price"].ToString();

            lblQuantity.Text =
                dr["Quantity"].ToString();

            lblTotal.Text =
                "₹" + dr["Total"].ToString();

            lblOrderDate.Text =
                Convert.ToDateTime(
                    dr["OrderDate"]
                ).ToString(
                    "dd-MM-yyyy hh:mm tt"
                );

            lblPayment.Text =
                dr["PaymentMethod"].ToString();


            // ORDER STATUS

            string orderStatus =
                dr["OrderStatus"].ToString();

            lblStatus.Text =
                orderStatus;


            if (orderStatus == "Cancelled")
            {
                pnlNormalStatus.Visible = false;

                pnlCancelled.Visible = true;

                lblStatus.CssClass =
                    "detail-value";
            }
            else
            {
                pnlNormalStatus.Visible = true;

                pnlCancelled.Visible = false;
            }
            if (orderStatus == "Placed")
            {
                // Order Placed
            }

            else if (orderStatus == "Preparing")
            {
                stepPreparing.Attributes["class"] =
                    "status-item active";
            }

            else if (orderStatus == "Shipped")
            {
                stepPreparing.Attributes["class"] =
                    "status-item active";

                stepShipped.Attributes["class"] =
                    "status-item active";
            }

            else if (orderStatus == "Delivered")
            {
                stepPreparing.Attributes["class"] =
                    "status-item active";

                stepShipped.Attributes["class"] =
                    "status-item active";

                stepDelivered.Attributes["class"] =
                    "status-item active";
            }
            // DELIVERY DETAILS

            lblCustomerName.Text =
                dr["CustomerName"].ToString();

            lblPhone.Text =
                dr["Phone"].ToString();

            lblAddress.Text =
                dr["Address"].ToString();

            lblPincode.Text =
                dr["Pincode"].ToString();

            lblLandmark.Text =
                dr["Landmark"].ToString();
        }
        else
        {
            con.Close();

            Response.Redirect("MyOrders.aspx");
            return;
        }


        dr.Close();

        con.Close();
    }
}