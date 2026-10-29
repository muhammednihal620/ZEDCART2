
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class ProductDetails : System.Web.UI.Page
{
    string connectionString =
        "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int productID;

            if (!int.TryParse(Request.QueryString["id"], out productID))
            {
                Response.Redirect("Products.aspx", true);
                return;
            }

            LoadProduct(productID);
        }
    }

    private int GetProductID()
    {
        int productID;

        if (!int.TryParse(Request.QueryString["id"], out productID))
            return 0;

        return productID;
    }

    protected void LoadProduct(int productID)
    {
        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT ProductName, Description " +
                "FROM Products WHERE ProductID=@ProductID", con);

            cmd.Parameters.AddWithValue("@ProductID", productID);

            SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                dr.Close();
                Response.Redirect("Products.aspx", true);
                return;
            }

            lblProductName.Text = dr["ProductName"].ToString();
            lblDescription.Text = dr["Description"].ToString();

            dr.Close();

            SqlCommand colorCmd = new SqlCommand(
                "SELECT DISTINCT Color FROM ProductVariants " +
                "WHERE ProductID=@ProductID ORDER BY Color", con);

            colorCmd.Parameters.AddWithValue("@ProductID", productID);

            SqlDataAdapter da = new SqlDataAdapter(colorCmd);
            DataTable colors = new DataTable();
            da.Fill(colors);

            ddlColor.Items.Clear();
            ddlColor.DataSource = colors;
            ddlColor.DataTextField = "Color";
            ddlColor.DataValueField = "Color";
            ddlColor.DataBind();

            if (ddlColor.Items.Count == 0)
            {
                lblPrice.Text = "0.00";
                lblMRP.Text = "0.00";
                lblOffer.Text = "";
                lblSavings.Text = "0.00";
                lblStock.Text = "0";
                imgProduct.ImageUrl = "Images/no-image.jpg";
                return;
            }

            BindSizes(productID);

            if (ddlSize.Items.Count > 0)
            {
                LoadSelectedVariant(productID);
            }
        }
    }

    private void BindSizes(int productID)
    {
        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT VariantID, Size, Model " +
                "FROM ProductVariants " +
                "WHERE ProductID=@ProductID AND Color=@Color " +
                "ORDER BY Size, VariantID", con);

            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue(
                "@Color", ddlColor.SelectedValue);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable sizes = new DataTable();
            da.Fill(sizes);

            ddlSize.Items.Clear();

            foreach (DataRow row in sizes.Rows)
            {
                string variantID =
                    row["VariantID"].ToString();

                string size =
                    row["Size"].ToString();

                string model =
                    row["Model"].ToString();

                string displayText = size;

                if (model != "")
                {
                    displayText = size + " - " + model;
                }

                ddlSize.Items.Add(
                    new ListItem(displayText, variantID));
            }
        }
    }

    protected void ddlColor_SelectedIndexChanged(
        object sender, EventArgs e)
    {
        int productID = GetProductID();

        if (productID == 0)
            return;

        BindSizes(productID);

        if (ddlSize.Items.Count > 0)
        {
            LoadSelectedVariant(productID);
        }
        else
        {
            ViewState["VariantID"] = null;
            lblPrice.Text = "0.00";
            lblMRP.Text = "0.00";
            lblOffer.Text = "";
            lblSavings.Text = "0.00";
            lblStock.Text = "0";
            lblModel.Text = "";
            rptImages.DataSource = null;
            rptImages.DataBind();
            imgProduct.ImageUrl = "Images/no-image.jpg";
        }
    }

    protected void ddlSize_SelectedIndexChanged(
        object sender, EventArgs e)
    {
        int productID = GetProductID();

        if (productID == 0)
            return;

        LoadSelectedVariant(productID);
    }

    private void LoadSelectedVariant(int productID)
    {
        int variantID;

        if (!int.TryParse(
            ddlSize.SelectedValue, out variantID))
            return;

        using (SqlConnection con =
            new SqlConnection(connectionString))
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT VariantID, Color, Size, Model, " +
                "Price, MRP, Stock " +
                "FROM ProductVariants " +
                "WHERE VariantID=@VariantID " +
                "AND ProductID=@ProductID " +
                "AND Color=@Color", con);

            cmd.Parameters.AddWithValue("@VariantID", variantID);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue(
                "@Color", ddlColor.SelectedValue);

            SqlDataReader dr = cmd.ExecuteReader();

            if (!dr.Read())
            {
                dr.Close();
                return;
            }

            ViewState["VariantID"] =
                dr["VariantID"].ToString();

            lblModel.Text = dr["Model"].ToString();

            decimal sellingPrice =
                Convert.ToDecimal(dr["Price"]);

            decimal mrp = sellingPrice;

            if (dr["MRP"] != DBNull.Value)
            {
                mrp = Convert.ToDecimal(dr["MRP"]);
            }

            lblPrice.Text = sellingPrice.ToString("0.00");
            lblMRP.Text = mrp.ToString("0.00");

            decimal savings = mrp - sellingPrice;

            if (savings < 0)
                savings = 0;

            int offerPercent = 0;

            if (mrp > 0 && sellingPrice < mrp)
            {
                offerPercent = (int)Math.Round(
                    ((mrp - sellingPrice) / mrp) * 100,
                    MidpointRounding.AwayFromZero);
            }

            lblOffer.Text = offerPercent > 0
                ? offerPercent.ToString() + "% OFF"
                : "";

            lblSavings.Text = savings.ToString("0.00");

            int stock = Convert.ToInt32(dr["Stock"]);
            lblStock.Text = stock.ToString();

            dr.Close();

            LoadVariantImages(con, productID, variantID);
        }
    }

    private void LoadVariantImages(
        SqlConnection con, int productID, int variantID)
    {
        SqlCommand imageCmd = new SqlCommand(
            "SELECT ImageName FROM ProductImages " +
            "WHERE ProductID=@ProductID " +
            "AND VariantID=@VariantID " +
            "ORDER BY IsMain DESC, ImageID ASC", con);

        imageCmd.Parameters.AddWithValue(
            "@ProductID", productID);

        imageCmd.Parameters.AddWithValue(
            "@VariantID", variantID);

        SqlDataAdapter da = new SqlDataAdapter(imageCmd);
        DataTable images = new DataTable();
        da.Fill(images);

        rptImages.DataSource = images;
        rptImages.DataBind();

        if (images.Rows.Count > 0)
        {
            imgProduct.ImageUrl =
                "Images/" +
                images.Rows[0]["ImageName"].ToString();
        }
        else
        {
            imgProduct.ImageUrl = "Images/no-image.jpg";
        }
    }

    protected void btnAddCart_Click(
        object sender, EventArgs e)
    {
        AddSelectedProductToCart("cart");
    }

    protected void btnBuyNow_Click(
        object sender, EventArgs e)
    {
        AddSelectedProductToCart("buy");
    }

    private void AddSelectedProductToCart(string action)
    {
        int productID = GetProductID();
        int variantID;
        int quantity;
        int stock;

        if (productID == 0 ||
            !int.TryParse(
                Convert.ToString(ViewState["VariantID"]),
                out variantID))
        {
            return;
        }

        if (!int.TryParse(
            ddlQuantity.SelectedValue, out quantity))
        {
            quantity = 1;
        }

        if (!int.TryParse(lblStock.Text, out stock))
        {
            stock = 0;
        }

        if (quantity < 1 || quantity > stock)
        {
            lblStock.Text =
                stock.ToString() + " (Not enough stock)";
            return;
        }

        string destination =
            action == "buy" ? "Checkout.aspx" : "Cart.aspx";

        Response.Redirect(
            destination +
            "?product=" + productID +
            "&variant=" + variantID +
            "&action=" + action +
            "&quantity=" + quantity);
    }
}