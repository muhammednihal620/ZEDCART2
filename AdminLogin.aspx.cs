
using System;

public partial class AdminLogin : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Prevent browser cache
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


        // Already logged in? Go to Admin Dashboard
        if (Session["Admin"] != null &&
            Session["Admin"].ToString() == "1")
        {
            Response.Redirect("AdminDashboard.aspx", true);
            return;
        }
    }


    protected void btnLogin_Click(
        object sender,
        EventArgs e)
    {
        string username =
            txtUsername.Text.Trim();

        string password =
            txtPassword.Text;

        // Check Admin Login
        if (username == "nihalpes85@gmail.com" &&
            password == "85475099")
        {
            Session["Admin"] = "1";

            Response.Redirect(
                "AdminDashboard.aspx",
                true);
        }
        else
        {
            lblMessage.Text =
                "Invalid username or password";

            lblMessage.ForeColor =
                System.Drawing.Color.Red;
        }
    }
}
