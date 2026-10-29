
using System;
using System.Data;
using System.Web;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
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

        // Customer login status
        bool isLoggedIn = Session["CustomerID"] != null;

        lnkLogin.Visible = !isLoggedIn;
        lnkRegister.Visible = !isLoggedIn;
        lnkMyOrders.Visible = isLoggedIn;
        btnLogout.Visible = isLoggedIn;

        // Update cart count on every request
        LoadCartCount();
    }

    // Load cart quantity count
    protected void LoadCartCount()
    {
        int count = 0;

        DataTable cart = Session["Cart"] as DataTable;

        if (cart != null && cart.Columns.Contains("Quantity"))
        {
            foreach (DataRow row in cart.Rows)
            {
                if (row["Quantity"] != DBNull.Value)
                {
                    int quantity;

                    if (Int32.TryParse(
                        Convert.ToString(row["Quantity"]),
                        out quantity))
                    {
                        count += quantity;
                    }
                }
            }
        }

        lblCartCount.Text = "(" + count + ")";
    }

    // Customer logout
    protected void btnLogout_Click(object sender, EventArgs e)
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

        Response.Redirect("~/Default.aspx", false);
        Context.ApplicationInstance.CompleteRequest();
    }

    // Product search
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string search = txtSearch.Text.Trim();

        if (!String.IsNullOrEmpty(search))
        {
            Response.Redirect(
                "~/Products.aspx?search=" +
                Server.UrlEncode(search), false);
        }
        else
        {
            Response.Redirect("~/Products.aspx", false);
        }

        Context.ApplicationInstance.CompleteRequest();
    }
}
