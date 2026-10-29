using System;
using System.Data.SqlClient;

public partial class Login : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache
        );

        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));

        con = new SqlConnection(
            "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;Initial Catalog=CheepCart;Integrated Security=True"
        );
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        con.Open();

        SqlCommand cmd = new SqlCommand(
            "SELECT CustomerID, FullName FROM Customers " +
            "WHERE Email=@Email AND Password=@Password",
            con
        );

        cmd.Parameters.AddWithValue(
            "@Email",
            txtEmail.Text.Trim()
        );

        cmd.Parameters.AddWithValue(
            "@Password",
            txtPassword.Text
        );

        SqlDataReader dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            Session["CustomerID"] = dr["CustomerID"].ToString();
            Session["CustomerName"] = dr["FullName"].ToString();

            con.Close();

            string returnUrl = Request.QueryString["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                Response.Redirect(returnUrl);
            }
            else
            {
                Response.Redirect("Default.aspx");
            }
        }
        else
        {
            Label1.Text = "Invalid Email or Password";
            Label1.ForeColor = System.Drawing.Color.Red;

            con.Close();
        }
    }
}