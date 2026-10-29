
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AdminOrders : System.Web.UI.Page
{
    SqlConnection con;

    string connectionString =
        "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null ||
        Session["Admin"].ToString() != "1")
        {
            Response.Redirect("AdminLogin.aspx", true);
            return;
        }
        // Prevent browser cache
        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);

        Response.Cache.SetNoStore();

        Response.Cache.SetExpires(
            DateTime.UtcNow.AddDays(-1));

        Response.AppendHeader("Pragma", "no-cache");

        Response.AppendHeader(
            "Cache-Control",
            "no-cache, no-store, must-revalidate");


        // Admin Login Check
        if (Session["Admin"] == null ||
            Session["Admin"].ToString() != "1")
        {
            Response.Redirect("AdminLogin.aspx", true);
            return;
        }


        con = new SqlConnection(connectionString);

        if (!IsPostBack)
        {
            LoadOrders();
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminDashboard.aspx", true);
    }



    // LOAD ALL ORDERS
    protected void LoadOrders()
    {
        using (SqlConnection cn =
            new SqlConnection(connectionString))
        {
            string query =
                "SELECT OrderID, CustomerName, " +
                "ProductName, Price, Quantity, Total, " +
                "OrderDate, PaymentMethod, OrderStatus " +
                "FROM OrderDetails ORDER BY OrderID DESC";

            SqlDataAdapter da =
                new SqlDataAdapter(query, cn);

            DataTable dt = new DataTable();

            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }


    // SEARCH ORDER ID
    protected void btnSearch_Click(
        object sender, EventArgs e)
    {
        int orderID;

        if (!int.TryParse(
            txtSearchOrderID.Text.Trim(),
            out orderID))
        {
            lblMessage.Text =
                "Please enter a valid Order ID.";

            lblMessage.ForeColor =
                System.Drawing.Color.Red;

            return;
        }


        using (SqlConnection cn =
            new SqlConnection(connectionString))
        {
            string query =
                "SELECT OrderID, CustomerName, " +
                "ProductName, Price, Quantity, Total, " +
                "OrderDate, PaymentMethod, OrderStatus " +
                "FROM OrderDetails WHERE OrderID=@OrderID";

            SqlDataAdapter da =
                new SqlDataAdapter(query, cn);

            da.SelectCommand.Parameters.AddWithValue(
                "@OrderID", orderID);

            DataTable dt = new DataTable();

            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();

            if (dt.Rows.Count == 0)
            {
                lblMessage.Text =
                    "Order #" + orderID + " not found.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
            }
            else
            {
                lblMessage.Text =
                    "Order #" + orderID + " found.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Green;
            }
        }
    }


    // SHOW ALL ORDERS
    protected void btnShowAll_Click(
        object sender, EventArgs e)
    {
        txtSearchOrderID.Text = "";
        lblMessage.Text = "";

        LoadOrders();
    }


    // UPDATE ORDER STATUS
    protected void GridView1_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UpdateStatus")
        {
            int orderID;

            if (!int.TryParse(
                Convert.ToString(e.CommandArgument),
                out orderID))
            {
                lblMessage.Text = "Invalid Order ID.";
                lblMessage.ForeColor =
                    System.Drawing.Color.Red;
                return;
            }


            Button btn = e.CommandSource as Button;

            if (btn == null)
                return;

            GridViewRow row =
                btn.NamingContainer as GridViewRow;

            if (row == null)
                return;

            DropDownList ddlStatus =
                row.FindControl("ddlStatus")
                as DropDownList;

            if (ddlStatus == null)
            {
                lblMessage.Text =
                    "Status dropdown not found.";

                lblMessage.ForeColor =
                    System.Drawing.Color.Red;

                return;
            }

            string status = ddlStatus.SelectedValue;


            using (SqlConnection cn =
                new SqlConnection(connectionString))
            {
                string query =
                    "UPDATE OrderDetails " +
                    "SET OrderStatus=@OrderStatus " +
                    "WHERE OrderID=@OrderID";

                SqlCommand cmd =
                    new SqlCommand(query, cn);

                cmd.Parameters.AddWithValue(
                    "@OrderStatus", status);

                cmd.Parameters.AddWithValue(
                    "@OrderID", orderID);

                cn.Open();

                int result = cmd.ExecuteNonQuery();


                if (result > 0)
                {
                    lblMessage.Text =
                        "Order #" + orderID +
                        " status updated successfully!";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Green;
                }
                else
                {
                    lblMessage.Text =
                        "Order status update failed.";

                    lblMessage.ForeColor =
                        System.Drawing.Color.Red;
                }
            }


            // Refresh orders
            if (txtSearchOrderID.Text.Trim() != "")
            {
                btnSearch_Click(null, null);
            }
            else
            {
                LoadOrders();
            }
        }
    }


    // SET CURRENT STATUS IN DROPDOWN
    protected void GridView1_RowDataBound(
        object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlStatus =
                e.Row.FindControl("ddlStatus")
                as DropDownList;

            if (ddlStatus != null)
            {
                string status = Convert.ToString(
                    DataBinder.Eval(
                        e.Row.DataItem,
                        "OrderStatus"
                    )).Trim();

                ListItem item =
                    ddlStatus.Items.FindByValue(status);

                if (item == null && status != "")
                {
                    ddlStatus.Items.Add(
                        new ListItem(status, status));
                }

                if (ddlStatus.Items.FindByValue(status) != null)
                {
                    ddlStatus.SelectedValue = status;
                }
            }
        }
    }


    // ADMIN LOGOUT
    protected void btnLogout_Click(
        object sender, EventArgs e)
    {
        Session.Clear();
        Session.RemoveAll();
        Session.Abandon();


        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);

        Response.Cache.SetNoStore();

        Response.Cache.SetExpires(
            DateTime.UtcNow.AddDays(-1));

        Response.AppendHeader("Pragma", "no-cache");

        Response.AppendHeader(
            "Cache-Control",
            "no-cache, no-store, must-revalidate");


        Response.Redirect("AdminLogin.aspx", true);
    }
}
