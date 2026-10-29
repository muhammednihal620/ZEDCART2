
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Cart : System.Web.UI.Page
{
    string connectionString =
        "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request.QueryString["action"];

        // Add the selected variant only on the incoming request.
        if (!IsPostBack &&
            action == "cart" &&
            Request.QueryString["product"] != null)
        {
            AddProductToCart();
            return;
        }

        if (!IsPostBack)
        {
            LoadCart();
        }
    }

    // ADD PRODUCT TO CART FROM DATABASE
    private void AddProductToCart()
    {
        int productID;
        int variantID;
        int quantityToAdd;

        if (!Int32.TryParse(
                Request.QueryString["product"], out productID) ||
            !Int32.TryParse(
                Request.QueryString["variant"], out variantID))
        {
            Response.Redirect("Cart.aspx");
            return;
        }

        if (!Int32.TryParse(
                Request.QueryString["quantity"], out quantityToAdd) ||
            quantityToAdd < 1)
        {
            quantityToAdd = 1;
        }

        string productName = "";
        string color = "";
        string size = "";
        decimal price = 0;
        int stock = 0;

        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            connection.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT p.ProductName, v.Color, v.Size, " +
                "v.Price, v.Stock " +
                "FROM Products p " +
                "INNER JOIN ProductVariants v " +
                "ON p.ProductID = v.ProductID " +
                "WHERE p.ProductID = @ProductID " +
                "AND v.VariantID = @VariantID",
                connection);

            cmd.Parameters.AddWithValue(
                "@ProductID", productID);

            cmd.Parameters.AddWithValue(
                "@VariantID", variantID);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                productName = reader["ProductName"].ToString();
                color = reader["Color"].ToString();
                size = reader["Size"].ToString();
                price = Convert.ToDecimal(reader["Price"]);
                stock = Convert.ToInt32(reader["Stock"]);
            }
            else
            {
                reader.Close();
                Response.Redirect("Products.aspx");
                return;
            }

            reader.Close();
        }

        if (quantityToAdd > stock)
        {
            Label1.Text = "Requested quantity is not available.";
            LoadCart();
            return;
        }

        DataTable cart;

        if (Session["Cart"] == null)
        {
            cart = new DataTable();

            cart.Columns.Add("ProductID", typeof(int));
            cart.Columns.Add("VariantID", typeof(int));
            cart.Columns.Add("ProductName", typeof(string));
            cart.Columns.Add("Color", typeof(string));
            cart.Columns.Add("Size", typeof(string));
            cart.Columns.Add("Price", typeof(decimal));
            cart.Columns.Add("Quantity", typeof(int));
            cart.Columns.Add("Total", typeof(decimal));

            Session["Cart"] = cart;
        }
        else
        {
            cart = (DataTable)Session["Cart"];

            // Ensure the cart has the columns required by this code.
            if (!cart.Columns.Contains("VariantID"))
            {
                cart = CreateNewCartTable();
                Session["Cart"] = cart;
            }
        }

        DataRow existingRow = null;

        foreach (DataRow row in cart.Rows)
        {
            if (Convert.ToInt32(row["ProductID"]) == productID &&
                Convert.ToInt32(row["VariantID"]) == variantID)
            {
                existingRow = row;
                break;
            }
        }

        int newQuantity = quantityToAdd;

        if (existingRow != null)
        {
            newQuantity =
                Convert.ToInt32(existingRow["Quantity"]) +
                quantityToAdd;
        }

        if (newQuantity > stock)
        {
            Label1.Text =
                "Only " + stock +
                " item(s) are currently available for this variant.";
            LoadCart();
            return;
        }

        if (existingRow != null)
        {
            existingRow["Quantity"] = newQuantity;
            existingRow["Total"] = price * newQuantity;
        }
        else
        {
            DataRow row = cart.NewRow();

            row["ProductID"] = productID;
            row["VariantID"] = variantID;
            row["ProductName"] = productName;
            row["Color"] = color;
            row["Size"] = size;
            row["Price"] = price;
            row["Quantity"] = quantityToAdd;
            row["Total"] = price * quantityToAdd;

            cart.Rows.Add(row);
        }

        Session["Cart"] = cart;

        Response.Redirect("Cart.aspx");
    }

    private DataTable CreateNewCartTable()
    {
        DataTable cart = new DataTable();

        cart.Columns.Add("ProductID", typeof(int));
        cart.Columns.Add("VariantID", typeof(int));
        cart.Columns.Add("ProductName", typeof(string));
        cart.Columns.Add("Color", typeof(string));
        cart.Columns.Add("Size", typeof(string));
        cart.Columns.Add("Price", typeof(decimal));
        cart.Columns.Add("Quantity", typeof(int));
        cart.Columns.Add("Total", typeof(decimal));

        return cart;
    }

    // LOAD CART
    protected void LoadCart()
    {
        if (Session["Cart"] == null)
        {
            GridView1.Visible = false;
            lblGrandTotal.Text = "0.00";
            Label1.Text = "Your cart is empty.";
            return;
        }

        DataTable cart = (DataTable)Session["Cart"];

        if (cart.Rows.Count == 0)
        {
            GridView1.Visible = false;
            lblGrandTotal.Text = "0.00";
            Label1.Text = "Your cart is empty.";
            return;
        }

        GridView1.Visible = true;
        Label1.Text = "";

        GridView1.DataSource = cart;
        GridView1.DataBind();

        decimal grandTotal = 0;

        foreach (DataRow row in cart.Rows)
        {
            grandTotal += Convert.ToDecimal(row["Total"]);
        }

        lblGrandTotal.Text = grandTotal.ToString("0.00");
    }

    // PLUS / MINUS / REMOVE
    protected void GridView1_RowCommand(
        object sender,
        GridViewCommandEventArgs e)
    {
        if (Session["Cart"] == null)
            return;

        int variantID;

        if (!Int32.TryParse(
            Convert.ToString(e.CommandArgument), out variantID))
            return;

        DataTable cart = (DataTable)Session["Cart"];
        DataRow targetRow = null;

        foreach (DataRow row in cart.Rows)
        {
            if (Convert.ToInt32(row["VariantID"]) == variantID)
            {
                targetRow = row;
                break;
            }
        }

        if (targetRow == null)
            return;

        int quantity = Convert.ToInt32(targetRow["Quantity"]);
        decimal price = Convert.ToDecimal(targetRow["Price"]);

        if (e.CommandName == "Plus")
        {
            int stock = GetVariantStock(variantID);

            if (quantity < stock)
            {
                quantity++;
                targetRow["Quantity"] = quantity;
                targetRow["Total"] = price * quantity;
            }
            else
            {
                Label1.Text = "Maximum available stock reached.";
            }
        }
        else if (e.CommandName == "Minus")
        {
            quantity--;

            if (quantity <= 0)
            {
                cart.Rows.Remove(targetRow);
            }
            else
            {
                targetRow["Quantity"] = quantity;
                targetRow["Total"] = price * quantity;
            }
        }
        else if (e.CommandName == "RemoveProduct")
        {
            cart.Rows.Remove(targetRow);
        }

        Session["Cart"] = cart;
        LoadCart();
    }

    private int GetVariantStock(int variantID)
    {
        using (SqlConnection connection =
            new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT Stock FROM ProductVariants " +
                "WHERE VariantID = @VariantID",
                connection);

            cmd.Parameters.AddWithValue(
                "@VariantID", variantID);

            connection.Open();

            object result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
                return 0;

            return Convert.ToInt32(result);
        }
    }
}

