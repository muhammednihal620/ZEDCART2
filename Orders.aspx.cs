using System;
using System.Data;
using System.Data.SqlClient;

public partial class Orders : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        con = new SqlConnection(
            "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;Initial Catalog=CheepCart;Integrated Security=True"
        );

        if (!IsPostBack)
        {
            LoadOrders();
        }
    }

    protected void LoadOrders()
    {
        con.Open();

        SqlDataAdapter da = new SqlDataAdapter(
            "SELECT * FROM OrderDetails ORDER BY OrderID DESC",
            con
        );

        DataTable dt = new DataTable();

        da.Fill(dt);

        GridView1.DataSource = dt;
        GridView1.DataBind();

        con.Close();
    }

}