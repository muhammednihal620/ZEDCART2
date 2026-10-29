
using System;
using System.Data;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    private readonly string connectionString =
        @"Data Source=ASUSVIVOBOOK\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True;";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadProducts();
        }
    }

    private void LoadProducts()
    {
        const string query = @"
            SELECT TOP 4
                p.ProductID,
                p.ProductName,
                p.Description,
                v.Price,
                (
                    SELECT TOP 1 pi.ImageName
                    FROM ProductImages pi
                    WHERE pi.ProductID = p.ProductID
                      AND pi.IsMain = 1
                    ORDER BY pi.ImageID
                ) AS ImageName
            FROM Products p
            INNER JOIN ProductVariants v
                ON v.ProductID = p.ProductID
            WHERE v.VariantID = (
                SELECT MIN(v2.VariantID)
                FROM ProductVariants v2
                WHERE v2.ProductID = p.ProductID
            )
            ORDER BY p.ProductID DESC;";

        try
        {
            DataTable dt = new DataTable();

            using (SqlConnection con =
                new SqlConnection(connectionString))
            using (SqlCommand cmd =
                new SqlCommand(query, con))
            using (SqlDataAdapter da =
                new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }

            rptProducts.DataSource = dt;
            rptProducts.DataBind();

            pnlNoProducts.Visible = (dt.Rows.Count == 0);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Default page product loading error: " + ex);

            rptProducts.DataSource = null;
            rptProducts.DataBind();

            pnlNoProducts.Visible = true;
            pnlNoProducts.Controls.Clear();

            pnlNoProducts.Controls.Add(
                new System.Web.UI.LiteralControl(
                    "Products could not be loaded. Please try again later."));
        }
    }
}
