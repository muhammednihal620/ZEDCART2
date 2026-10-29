
using System;
using System.Data;
using System.Data.SqlClient;

public partial class Products : System.Web.UI.Page
{
    private readonly string connectionString =
        "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SearchProducts();
        }
    }

    protected void SearchProducts()
    {
        string search = Request.QueryString["search"];
        string category = Request.QueryString["category"];

        DataTable dt = new DataTable();

        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            string sql =
                "SELECT " +
                "p.ProductID, " +
                "p.ProductName, " +
                "p.Description, " +
                "p.Category, " +
                "v.Price, " +
                "pi.ImageName " +
                "FROM Products p " +
                "INNER JOIN ProductVariants v " +
                "ON p.ProductID = v.ProductID " +
                "LEFT JOIN ProductImages pi " +
                "ON v.VariantID = pi.VariantID " +
                "AND pi.IsMain = 1 " +
                "WHERE 1 = 1 ";

            // Filter by selected category
            if (!string.IsNullOrWhiteSpace(category))
            {
                sql += " AND p.Category = @Category ";
            }

            // Search within the selected category or all products
            if (!string.IsNullOrWhiteSpace(search))
            {
                sql +=
                    " AND (p.ProductName LIKE @Search " +
                    " OR p.Description LIKE @Search " +
                    " OR p.Category LIKE @Search) ";
            }

            sql += " ORDER BY p.ProductID DESC";

            using (SqlCommand cmd =
                new SqlCommand(sql, con))
            {
                if (!string.IsNullOrWhiteSpace(category))
                {
                    cmd.Parameters.Add(
                        "@Category", SqlDbType.VarChar, 100
                    ).Value = category.Trim();
                }

                if (!string.IsNullOrWhiteSpace(search))
                {
                    cmd.Parameters.Add(
                        "@Search", SqlDbType.VarChar, 200
                    ).Value = "%" + search.Trim() + "%";
                }

                using (SqlDataAdapter da =
                    new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
        }

        Repeater1.DataSource = dt;
        Repeater1.DataBind();

        if (dt.Rows.Count == 0)
        {
            lblNoProducts.Visible = true;

            if (!string.IsNullOrWhiteSpace(category))
            {
                lblNoProducts.Text =
                    "No products found in this category.";
            }
            else if (!string.IsNullOrWhiteSpace(search))
            {
                lblNoProducts.Text =
                    "No products found.";
            }
            else
            {
                lblNoProducts.Text =
                    "No products available.";
            }
        }
        else
        {
            lblNoProducts.Visible = false;
        }
    }
}
