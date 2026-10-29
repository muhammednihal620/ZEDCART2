
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MyOrders : System.Web.UI.Page
{
    string connectionString =
        "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CustomerID"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadMyOrders();
        }
    }

    private void LoadMyOrders()
    {
        string query =
            "SELECT OrderID, ProductName, Price, Quantity, " +
            "Total, OrderDate, PaymentMethod, OrderStatus " +
            "FROM OrderDetails WHERE CustomerID=@CustomerID " +
            "ORDER BY OrderID DESC";

        BindOrders(query, null);
    }

    private void BindOrders(string query, string filterValue)
    {
        lblMessage.Text = "";

        try
        {
            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.Add("@CustomerID",
                    SqlDbType.Int).Value =
                    Convert.ToInt32(Session["CustomerID"]);

                if (filterValue != null)
                {
                    if (query.IndexOf("@ProductName") >= 0)
                    {
                        cmd.Parameters.Add("@ProductName",
                            SqlDbType.VarChar, 100).Value =
                            "%" + filterValue + "%";
                    }
                    else if (query.IndexOf("@OrderStatus") >= 0)
                    {
                        cmd.Parameters.Add("@OrderStatus",
                            SqlDbType.VarChar, 50).Value =
                            filterValue;
                    }
                }

                using (SqlDataAdapter da =
                    new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text =
                "Error: " + Server.HtmlEncode(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string query =
            "SELECT OrderID, ProductName, Price, Quantity, " +
            "Total, OrderDate, PaymentMethod, OrderStatus " +
            "FROM OrderDetails WHERE CustomerID=@CustomerID " +
            "AND ProductName LIKE @ProductName " +
            "ORDER BY OrderID DESC";

        BindOrders(query, txtSearch.Text.Trim());
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        string query =
            "SELECT OrderID, ProductName, Price, Quantity, " +
            "Total, OrderDate, PaymentMethod, OrderStatus " +
            "FROM OrderDetails WHERE CustomerID=@CustomerID " +
            "AND OrderStatus=@OrderStatus " +
            "ORDER BY OrderID DESC";

        BindOrders(query, ddlStatus.SelectedValue);
    }

    protected void GridView1_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "CancelOrder")
            return;

        int orderID;

        if (!int.TryParse(
            Convert.ToString(e.CommandArgument), out orderID))
        {
            lblMessage.Text = "Invalid Order ID.";
            return;
        }

        try
        {
            string status = GetOrderStatus(orderID);

            if (status == null)
            {
                lblMessage.Text = "Order not found.";
                return;
            }

            hfCancelOrderID.Value = orderID.ToString();
            btnConfirmCancel.Visible = false;

            if (status.Equals("Placed",
                StringComparison.OrdinalIgnoreCase) ||
                status.Equals("Preparing",
                StringComparison.OrdinalIgnoreCase))
            {
                lblModalMessage.Text =
                    "Do you want to cancel Order #" +
                    orderID + "?";

                btnConfirmCancel.Visible = true;
            }
            else if (status.Equals("Shipped",
                StringComparison.OrdinalIgnoreCase))
            {
                lblModalMessage.Text =
                    "Order #" + orderID +
                    " has already been shipped. " +
                    "You cannot cancel this order.";
            }
            else if (status.Equals("Delivered",
                StringComparison.OrdinalIgnoreCase))
            {
                lblModalMessage.Text =
                    "Order #" + orderID +
                    " has already been delivered. " +
                    "Cancellation is not allowed.";
            }
            else if (status.Equals("Cancelled",
                StringComparison.OrdinalIgnoreCase))
            {
                lblModalMessage.Text =
                    "Order #" + orderID +
                    " is already cancelled.";
            }
            else
            {
                lblModalMessage.Text =
                    "This order cannot be cancelled " +
                    "because of its current status.";
            }

            pnlCancelModal.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text =
                "Error: " + Server.HtmlEncode(ex.Message);
        }
    }

    private string GetOrderStatus(int orderID)
    {
        using (SqlConnection con =
            new SqlConnection(connectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT OrderStatus FROM OrderDetails " +
            "WHERE OrderID=@OrderID " +
            "AND CustomerID=@CustomerID", con))
        {
            cmd.Parameters.Add("@OrderID",
                SqlDbType.Int).Value = orderID;

            cmd.Parameters.Add("@CustomerID",
                SqlDbType.Int).Value =
                Convert.ToInt32(Session["CustomerID"]);

            con.Open();

            object result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
                return null;

            return Convert.ToString(result).Trim();
        }
    }

    protected void btnConfirmCancel_Click(
        object sender, EventArgs e)
    {
        int orderID;

        if (!int.TryParse(hfCancelOrderID.Value, out orderID))
        {
            lblMessage.Text = "Invalid Order ID.";
            pnlCancelModal.Visible = false;
            return;
        }

        try
        {
            // Check the latest status again before updating.
            string status = GetOrderStatus(orderID);

            if (status == null)
            {
                lblModalMessage.Text = "Order not found.";
                btnConfirmCancel.Visible = false;
                pnlCancelModal.Visible = true;
                return;
            }

            if (!status.Equals("Placed",
                StringComparison.OrdinalIgnoreCase) &&
                !status.Equals("Preparing",
                StringComparison.OrdinalIgnoreCase))
            {
                lblModalMessage.Text =
                    "This order can no longer be cancelled. " +
                    "Current status: " + status;

                btnConfirmCancel.Visible = false;
                pnlCancelModal.Visible = true;
                return;
            }

            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(
                "UPDATE OrderDetails " +
                "SET OrderStatus='Cancelled' " +
                "WHERE OrderID=@OrderID " +
                "AND CustomerID=@CustomerID " +
                "AND OrderStatus IN ('Placed','Preparing')",
                con))
            {
                cmd.Parameters.Add("@OrderID",
                    SqlDbType.Int).Value = orderID;

                cmd.Parameters.Add("@CustomerID",
                    SqlDbType.Int).Value =
                    Convert.ToInt32(Session["CustomerID"]);

                con.Open();

                int updated = cmd.ExecuteNonQuery();

                if (updated > 0)
                {
                    pnlCancelModal.Visible = false;
                    lblMessage.Text =
                        "Order #" + orderID +
                        " cancelled successfully.";
                }
                else
                {
                    lblModalMessage.Text =
                        "Order status changed. Please try again.";

                    btnConfirmCancel.Visible = false;
                    pnlCancelModal.Visible = true;
                    return;
                }
            }

            LoadMyOrders();
        }
        catch (Exception ex)
        {
            pnlCancelModal.Visible = false;
            lblMessage.Text =
                "Cancel error: " +
                Server.HtmlEncode(ex.Message);
        }
    }

    protected void btnCloseModal_Click(
        object sender, EventArgs e)
    {
        pnlCancelModal.Visible = false;
    }
}
