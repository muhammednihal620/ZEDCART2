
using System;
using System.Data.SqlClient;

public partial class AdminDashboard : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Admin"] == null ||
       Session["Admin"].ToString() != "1")
        {
            Response.Redirect("AdminLogin.aspx", true);
            return;
        }
        // NO CACHE

        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);

        Response.Cache.SetNoStore();

        Response.Cache.SetExpires(
            DateTime.UtcNow.AddDays(-1));

        Response.AppendHeader(
            "Pragma",
            "no-cache");

        Response.AppendHeader(
            "Cache-Control",
            "no-cache, no-store, must-revalidate");


        // ADMIN LOGIN CHECK

        if (Session["Admin"] == null ||
            Session["Admin"].ToString() != "1")
        {
            Response.Redirect(
                "AdminLogin.aspx",
                true);

            return;
        }


        // DATABASE CONNECTION

        con = new SqlConnection(
            "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
            "Initial Catalog=CheepCart;" +
            "Integrated Security=True"
        );


        if (!IsPostBack)
        {
            LoadDashboard();
        }
    }


    protected void LoadDashboard()
    {
        con.Open();


        // =========================================
        // TOTAL ORDERS
        // =========================================

        SqlCommand cmd1 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails",
            con);

        int totalOrders =
            Convert.ToInt32(
                cmd1.ExecuteScalar());

        lblTotalOrders.Text =
            totalOrders.ToString();


        // =========================================
        // TOTAL REVENUE
        // CANCELLED ORDERS NOT INCLUDED
        // =========================================

        SqlCommand cmdRevenue = new SqlCommand(
            "SELECT ISNULL(SUM(Total), 0) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus <> 'Cancelled'",
            con);

        lblRevenue.Text =
            Convert.ToDecimal(
                cmdRevenue.ExecuteScalar()
            ).ToString("0.00");


        // =========================================
        // PLACED
        // =========================================

        SqlCommand cmd2 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus='Placed'",
            con);

        int placed =
            Convert.ToInt32(
                cmd2.ExecuteScalar());

        lblPlaced.Text =
            placed.ToString();


        // =========================================
        // SHIPPED
        // =========================================

        SqlCommand cmd3 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus='Shipped'",
            con);

        int shipped =
            Convert.ToInt32(
                cmd3.ExecuteScalar());

        lblShipped.Text =
            shipped.ToString();


        // =========================================
        // DELIVERED
        // =========================================

        SqlCommand cmd4 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus='Delivered'",
            con);

        int delivered =
            Convert.ToInt32(
                cmd4.ExecuteScalar());

        lblDelivered.Text =
            delivered.ToString();


        // =========================================
        // PREPARING
        // =========================================

        SqlCommand cmd5 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus='Preparing'",
            con);

        int preparing =
            Convert.ToInt32(
                cmd5.ExecuteScalar());

        lblPreparing.Text =
            preparing.ToString();


        // =========================================
        // CANCELLED
        // =========================================

        SqlCommand cmd6 = new SqlCommand(
            "SELECT COUNT(*) " +
            "FROM OrderDetails " +
            "WHERE OrderStatus='Cancelled'",
            con);

        int cancelled =
            Convert.ToInt32(
                cmd6.ExecuteScalar());

        lblCancelled.Text =
            cancelled.ToString();


        // =========================================
        // TODAY'S ORDERS
        // =========================================

        SqlCommand cmdTodayOrders =
            new SqlCommand(
                "SELECT COUNT(*) " +
                "FROM OrderDetails " +
                "WHERE CONVERT(varchar(10), OrderDate, 120) = " +
                "CONVERT(varchar(10), GETDATE(), 120)",
                con);

        lblTodayOrders.Text =
            Convert.ToInt32(
                cmdTodayOrders.ExecuteScalar()
            ).ToString();


        // =========================================
        // TODAY'S REVENUE
        // CANCELLED NOT INCLUDED
        // =========================================

        SqlCommand cmdTodayRevenue =
            new SqlCommand(
                "SELECT ISNULL(SUM(Total), 0) " +
                "FROM OrderDetails " +
                "WHERE OrderStatus <> 'Cancelled' " +
                "AND CONVERT(varchar(10), OrderDate, 120) = " +
                "CONVERT(varchar(10), GETDATE(), 120)",
                con);

        lblTodayRevenue.Text =
            Convert.ToDecimal(
                cmdTodayRevenue.ExecuteScalar()
            ).ToString("0.00");


        // =========================================
        // STATUS SUMMARY BAR
        // =========================================

        if (totalOrders > 0)
        {
            barPlaced.Style["width"] =
                ((placed * 100) / totalOrders) + "%";

            barPreparing.Style["width"] =
                ((preparing * 100) / totalOrders) + "%";

            barShipped.Style["width"] =
                ((shipped * 100) / totalOrders) + "%";

            barDelivered.Style["width"] =
                ((delivered * 100) / totalOrders) + "%";

            barCancelled.Style["width"] =
                ((cancelled * 100) / totalOrders) + "%";


            lblPlacedPercent.Text =
                placed.ToString() + " orders";

            lblPreparingPercent.Text =
                preparing.ToString() + " orders";

            lblShippedPercent.Text =
                shipped.ToString() + " orders";

            lblDeliveredPercent.Text =
                delivered.ToString() + " orders";

            lblCancelledPercent.Text =
                cancelled.ToString() + " orders";
        }
        else
        {
            barPlaced.Style["width"] = "0%";

            barPreparing.Style["width"] = "0%";

            barShipped.Style["width"] = "0%";

            barDelivered.Style["width"] = "0%";

            barCancelled.Style["width"] = "0%";


            lblPlacedPercent.Text =
                "0 orders";

            lblPreparingPercent.Text =
                "0 orders";

            lblShippedPercent.Text =
                "0 orders";

            lblDeliveredPercent.Text =
                "0 orders";

            lblCancelledPercent.Text =
                "0 orders";
        }


        // =========================================
        // RECENT 5 ORDERS
        // =========================================

        SqlCommand cmd7 = new SqlCommand(
            "SELECT TOP 5 OrderID, CustomerName, " +
            "ProductName, Total, OrderStatus " +
            "FROM OrderDetails " +
            "ORDER BY OrderID DESC",
            con);


        SqlDataReader reader =
            cmd7.ExecuteReader();


        GridViewRecent.DataSource =
            reader;

        GridViewRecent.DataBind();


        reader.Close();


        // CLOSE CONNECTION

        con.Close();
    }


    // =========================================
    // LOGOUT
    // =========================================

    protected void btnLogout_Click(
        object sender,
        EventArgs e)
    {
        Session["Admin"] = null;

        Session.Clear();

        Session.RemoveAll();

        Session.Abandon();


        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);

        Response.Cache.SetNoStore();

        Response.Cache.SetExpires(
            DateTime.UtcNow.AddDays(-1));

        Response.AppendHeader(
            "Pragma",
            "no-cache");

        Response.AppendHeader(
            "Cache-Control",
            "no-cache, no-store, must-revalidate");


        Response.Redirect(
            "AdminLogin.aspx",
            true);
    }
}