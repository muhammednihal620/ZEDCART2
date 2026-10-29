
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public partial class AdminProducts : System.Web.UI.Page
{
    int variantCount = 1;

    private string ConnectionString
    {
        get
        {
            return "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
                   "Initial Catalog=CheepCart;" +
                   "Integrated Security=True";
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["Admin"] == null ||
            Session["Admin"].ToString() != "1")
        {
            Response.Redirect("AdminLogin.aspx", true);
            return;
        }

        string postedCount = Request.Form[hfVariantCount.UniqueID];

        if (!String.IsNullOrEmpty(postedCount))
        {
            int count;
            if (Int32.TryParse(postedCount, out count) &&
                count >= 1 && count <= 100)
            {
                variantCount = count;
            }
        }

        hfVariantCount.Value = variantCount.ToString();
        CreateVariantRows(variantCount);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
        Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
        Response.AppendHeader("Pragma", "no-cache");
        Response.AppendHeader(
            "Cache-Control", "no-cache, no-store, must-revalidate");

        if (Session["Admin"] == null ||
            Session["Admin"].ToString() != "1")
        {
            Response.Redirect("AdminLogin.aspx", true);
            return;
        }

        if (!IsPostBack)
        {
            LoadProducts();
        }
    }

    private void LoadProducts()
    {
        using (SqlConnection cn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT ProductID, ProductName, Category " +
            "FROM Products ORDER BY ProductID DESC", cn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvProducts.DataSource = dt;
            gvProducts.DataBind();
        }
    }

    protected void btnSearchProduct_Click(object sender, EventArgs e)
    {
        int productID;

        if (!Int32.TryParse(txtSearchProductID.Text.Trim(), out productID) ||
            productID <= 0)
        {
            lblDeleteMessage.Text = "Please enter a valid Product ID.";
            lblDeleteMessage.ForeColor = System.Drawing.Color.Red;
            LoadProducts();
            return;
        }

        using (SqlConnection cn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT ProductID, ProductName, Category " +
            "FROM Products WHERE ProductID = @ProductID", cn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddWithValue("@ProductID", productID);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            if (dt.Rows.Count == 0)
            {
                lblDeleteMessage.Text = "No product found with that ID.";
                lblDeleteMessage.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                lblDeleteMessage.Text = "Product found.";
                lblDeleteMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
    }

    protected void btnShowAllProducts_Click(object sender, EventArgs e)
    {
        txtSearchProductID.Text = "";
        lblDeleteMessage.Text = "";
        LoadProducts();
    }

    protected void gvProducts_RowCommand(
        object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "DeleteProduct")
            return;

        int productID;

        if (!Int32.TryParse(Convert.ToString(e.CommandArgument), out productID) ||
            productID <= 0)
        {
            lblDeleteMessage.Text = "Invalid Product ID.";
            lblDeleteMessage.ForeColor = System.Drawing.Color.Red;
            return;
        }

        using (SqlConnection cn = new SqlConnection(ConnectionString))
        {
            cn.Open();
            SqlTransaction tr = cn.BeginTransaction();

            try
            {
                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM ProductImages WHERE ProductID = @ProductID",
                    cn, tr))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM ProductVariants WHERE ProductID = @ProductID",
                    cn, tr))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Products WHERE ProductID = @ProductID",
                    cn, tr))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productID);

                    if (cmd.ExecuteNonQuery() == 0)
                        throw new Exception("Product not found.");
                }

                tr.Commit();

                lblDeleteMessage.Text = "Product deleted successfully.";
                lblDeleteMessage.ForeColor = System.Drawing.Color.Green;
            }
            catch (Exception ex)
            {
                try { tr.Rollback(); }
                catch { }

                lblDeleteMessage.Text = "Delete failed: " + ex.Message;
                lblDeleteMessage.ForeColor = System.Drawing.Color.Red;
            }
        }

        int searchID;

        if (Int32.TryParse(txtSearchProductID.Text.Trim(), out searchID) &&
            searchID > 0)
        {
            SearchProductByID(searchID);
        }
        else
        {
            LoadProducts();
        }
    }

    private void SearchProductByID(int productID)
    {
        using (SqlConnection cn = new SqlConnection(ConnectionString))
        using (SqlCommand cmd = new SqlCommand(
            "SELECT ProductID, ProductName, Category " +
            "FROM Products WHERE ProductID = @ProductID", cn))
        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        {
            cmd.Parameters.AddWithValue("@ProductID", productID);

            DataTable dt = new DataTable();
            da.Fill(dt);

            gvProducts.DataSource = dt;
            gvProducts.DataBind();

            if (dt.Rows.Count == 0)
            {
                lblDeleteMessage.Text =
                    "Product not found (it may have been deleted).";
                lblDeleteMessage.ForeColor = System.Drawing.Color.Green;
            }
        }
    }

    private void CreateVariantRows(int count)
    {
        phVariants.Controls.Clear();

        for (int i = 0; i < count; i++)
            AddVariantRow(i);
    }

    private void AddVariantRow(int index)
    {
        Panel box = new Panel();
        box.ID = "variantBox" + index;
        box.CssClass = "variant-box";

        Label heading = new Label();
        heading.CssClass = "variant-heading";
        heading.Text = "Variant " + (index + 1);
        box.Controls.Add(heading);

        AddField(box, "Color", "txtColor" + index);
        AddField(box, "Size", "txtSize" + index);
        AddField(box, "Model", "txtModel" + index);
        AddField(box, "MRP (Original Price)", "txtMRP" + index);
        AddField(box, "Selling Price (Offer Price)", "txtPrice" + index);
        AddField(box, "Stock", "txtStock" + index);

        Panel imageRow = new Panel();
        imageRow.CssClass = "form-row";

        Label imageLabel = new Label();
        imageLabel.Text = "Variant Image";
        imageLabel.AssociatedControlID = "fuVariantImage" + index;

        FileUpload fuVariantImage = new FileUpload();
        fuVariantImage.ID = "fuVariantImage" + index;

        imageRow.Controls.Add(imageLabel);
        imageRow.Controls.Add(fuVariantImage);
        box.Controls.Add(imageRow);

        phVariants.Controls.Add(box);
    }

    private void AddField(Panel box, string caption, string controlID)
    {
        Panel row = new Panel();
        row.CssClass = "form-row";

        Label label = new Label();
        label.Text = caption;
        label.AssociatedControlID = controlID;

        TextBox txt = new TextBox();
        txt.ID = controlID;
        txt.CssClass = "textbox";

        row.Controls.Add(label);
        row.Controls.Add(txt);
        box.Controls.Add(row);
    }

    private Control FindControlRecursive(Control root, string id)
    {
        if (root.ID == id)
            return root;

        foreach (Control child in root.Controls)
        {
            Control found = FindControlRecursive(child, id);

            if (found != null)
                return found;
        }

        return null;
    }

    protected void btnAddVariant_Click(object sender, EventArgs e)
    {
        if (variantCount >= 100)
        {
            ShowError("Maximum 100 variants allowed.");
            return;
        }

        AddVariantRow(variantCount);
        variantCount++;

        hfVariantCount.Value = variantCount.ToString();

        lblMessage.Text =
            "New variant added. Enter its details and image.";
        lblMessage.ForeColor = System.Drawing.Color.Green;
    }

    protected void btnAddProduct_Click(object sender, EventArgs e)
    {
        if (txtProductName.Text.Trim() == "")
        {
            ShowError("Please enter product name.");
            return;
        }

        bool hasImage =
            fuImage1.HasFile ||
            fuImage2.HasFile ||
            fuImage3.HasFile ||
            fuImage4.HasFile;

        if (!hasImage)
        {
            for (int i = 0; i < variantCount; i++)
            {
                FileUpload upload = FindControlRecursive(
                    phVariants, "fuVariantImage" + i) as FileUpload;

                if (upload != null && upload.HasFile)
                {
                    hasImage = true;
                    break;
                }
            }
        }

        if (!hasImage)
        {
            ShowError("Please select at least one product image.");
            return;
        }

        List<decimal> mrps = new List<decimal>();
        List<decimal> prices = new List<decimal>();
        List<int> stocks = new List<int>();

        for (int i = 0; i < variantCount; i++)
        {
            TextBox txtMRP = FindControlRecursive(
                phVariants, "txtMRP" + i) as TextBox;

            TextBox txtPrice = FindControlRecursive(
                phVariants, "txtPrice" + i) as TextBox;

            TextBox txtStock = FindControlRecursive(
                phVariants, "txtStock" + i) as TextBox;

            decimal mrp, price;
            int stock;

            if (txtMRP == null ||
                !Decimal.TryParse(txtMRP.Text.Trim(), out mrp) ||
                mrp <= 0)
            {
                ShowError("Enter valid MRP for Variant " + (i + 1));
                return;
            }

            if (txtPrice == null ||
                !Decimal.TryParse(txtPrice.Text.Trim(), out price) ||
                price <= 0 || price > mrp)
            {
                ShowError(
                    "Selling Price must be greater than 0 and not more than MRP for Variant " +
                    (i + 1));
                return;
            }

            if (txtStock == null ||
                !Int32.TryParse(txtStock.Text.Trim(), out stock) ||
                stock < 0)
            {
                ShowError("Enter valid stock for Variant " + (i + 1));
                return;
            }

            mrps.Add(mrp);
            prices.Add(price);
            stocks.Add(stock);
        }

        string folderPath = Server.MapPath("~/Images/");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        bool transactionStarted = false;
        SqlTransaction transaction = null;

        try
        {
            using (SqlConnection cn = new SqlConnection(ConnectionString))
            {
                cn.Open();

                transaction = cn.BeginTransaction();
                transactionStarted = true;

                int productID;

                using (SqlCommand productCmd = new SqlCommand(
                    "INSERT INTO Products " +
                    "(ProductName, Description, Category) " +
                    "VALUES (@ProductName, @Description, @Category); " +
                    "SELECT SCOPE_IDENTITY();",
                    cn, transaction))
                {
                    productCmd.Parameters.AddWithValue(
                        "@ProductName", txtProductName.Text.Trim());

                    productCmd.Parameters.AddWithValue(
                        "@Description", txtDescription.Text.Trim());

                    productCmd.Parameters.AddWithValue(
                        "@Category", txtCategory.Text.Trim());

                    productID = Convert.ToInt32(
                        productCmd.ExecuteScalar());
                }

                int firstVariantID = 0;

                for (int i = 0; i < variantCount; i++)
                {
                    TextBox txtColor = FindControlRecursive(
                        phVariants, "txtColor" + i) as TextBox;

                    TextBox txtSize = FindControlRecursive(
                        phVariants, "txtSize" + i) as TextBox;

                    TextBox txtModel = FindControlRecursive(
                        phVariants, "txtModel" + i) as TextBox;

                    if (txtColor == null ||
                        txtSize == null ||
                        txtModel == null)
                    {
                        throw new Exception(
                            "Variant fields could not be found.");
                    }

                    int variantID;

                    using (SqlCommand variantCmd = new SqlCommand(
                        "INSERT INTO ProductVariants " +
                        "(ProductID, Color, Size, Model, MRP, Price, Stock) " +
                        "VALUES (@ProductID, @Color, @Size, @Model, @MRP, @Price, @Stock); " +
                        "SELECT SCOPE_IDENTITY();",
                        cn, transaction))
                    {
                        variantCmd.Parameters.AddWithValue(
                            "@ProductID", productID);

                        variantCmd.Parameters.AddWithValue(
                            "@Color", txtColor.Text.Trim());

                        variantCmd.Parameters.AddWithValue(
                            "@Size", txtSize.Text.Trim());

                        variantCmd.Parameters.AddWithValue(
                            "@Model", txtModel.Text.Trim());

                        variantCmd.Parameters.AddWithValue(
                            "@MRP", mrps[i]);

                        variantCmd.Parameters.AddWithValue(
                            "@Price", prices[i]);

                        variantCmd.Parameters.AddWithValue(
                            "@Stock", stocks[i]);

                        variantID = Convert.ToInt32(
                            variantCmd.ExecuteScalar());
                    }

                    if (i == 0)
                        firstVariantID = variantID;

                    FileUpload variantUpload = FindControlRecursive(
                        phVariants, "fuVariantImage" + i) as FileUpload;

                    if (variantUpload != null && variantUpload.HasFile)
                    {
                        SaveImage(
                            variantUpload,
                            productID,
                            variantID,
                            folderPath,
                            false,
                            cn,
                            transaction);
                    }
                }

                SaveImage(
                    fuImage1, productID, firstVariantID,
                    folderPath, true, cn, transaction);

                SaveImage(
                    fuImage2, productID, firstVariantID,
                    folderPath, false, cn, transaction);

                SaveImage(
                    fuImage3, productID, firstVariantID,
                    folderPath, false, cn, transaction);

                SaveImage(
                    fuImage4, productID, firstVariantID,
                    folderPath, false, cn, transaction);

                transaction.Commit();
                transactionStarted = false;
            }

            lblMessage.Text =
                "Product and " + variantCount +
                " variant(s) added successfully!";

            lblMessage.ForeColor = System.Drawing.Color.Green;

            txtProductName.Text = "";
            txtDescription.Text = "";
            txtCategory.Text = "";

            variantCount = 1;
            hfVariantCount.Value = "1";

            CreateVariantRows(1);
            LoadProducts();
        }
        catch (Exception ex)
        {
            if (transactionStarted && transaction != null)
            {
                try { transaction.Rollback(); }
                catch { }
            }

            lblMessage.Text = "Error: " + ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            if (transaction != null)
                transaction.Dispose();
        }
    }

    // Resize and compress every uploaded product image.
    // Output format: JPEG, maximum dimensions: 1000 x 1000.
    private void SaveImage(
        FileUpload upload,
        int productID,
        int variantID,
        string folderPath,
        bool isMain,
        SqlConnection cn,
        SqlTransaction transaction)
    {
        if (upload == null || !upload.HasFile)
            return;

        string extension =
            Path.GetExtension(upload.FileName).ToLowerInvariant();

        if (extension != ".jpg" &&
            extension != ".jpeg" &&
            extension != ".png")
        {
            throw new Exception(
                "Only JPG, JPEG and PNG images are supported.");
        }

        // Maximum size for each uploaded image: 20 MB.
        if (upload.PostedFile.ContentLength > 20 * 1024 * 1024)
        {
            throw new Exception(
                "Each image must be 20 MB or less.");
        }

        string uniqueName =
            Guid.NewGuid().ToString("N") + ".jpg";

        string imagePath =
            Path.Combine(folderPath, uniqueName);

        try
        {
            using (System.Drawing.Image original =
                System.Drawing.Image.FromStream(
                    upload.PostedFile.InputStream))
            {
                int originalWidth = original.Width;
                int originalHeight = original.Height;

                if (originalWidth <= 0 || originalHeight <= 0)
                    throw new Exception("Invalid image dimensions.");

                // Do not enlarge small images.
                double scale = Math.Min(
                    1.0,
                    Math.Min(
                        1000.0 / originalWidth,
                        1000.0 / originalHeight));

                int newWidth = Math.Max(
                    1, (int)Math.Round(originalWidth * scale));

                int newHeight = Math.Max(
                    1, (int)Math.Round(originalHeight * scale));

                using (Bitmap resized = new Bitmap(
                    newWidth, newHeight, PixelFormat.Format24bppRgb))
                {
                    resized.SetResolution(96, 96);

                    using (Graphics graphics = Graphics.FromImage(resized))
                    {
                        // White background keeps transparent PNG images
                        // from turning black when saved as JPEG.
                        graphics.Clear(System.Drawing.Color.White);

                        graphics.CompositingQuality =
                            CompositingQuality.HighQuality;

                        graphics.InterpolationMode =
                            InterpolationMode.HighQualityBicubic;

                        graphics.SmoothingMode =
                            SmoothingMode.HighQuality;

                        graphics.PixelOffsetMode =
                            PixelOffsetMode.HighQuality;

                        graphics.DrawImage(
                            original,
                            new Rectangle(0, 0, newWidth, newHeight));
                    }

                    ImageCodecInfo jpegCodec = null;

                    foreach (ImageCodecInfo codec in
                        ImageCodecInfo.GetImageEncoders())
                    {
                        if (codec.MimeType == "image/jpeg")
                        {
                            jpegCodec = codec;
                            break;
                        }
                    }

                    if (jpegCodec == null)
                        throw new Exception("JPEG encoder not available.");

                    using (EncoderParameters encoderParams =
                        new EncoderParameters(1))
                    {
                        encoderParams.Param[0] =
                            new EncoderParameter(
                                System.Drawing.Imaging.Encoder.Quality,
                                82L);

                        resized.Save(
                            imagePath, jpegCodec, encoderParams);
                    }
                }
            }

            using (SqlCommand imageCmd = new SqlCommand(
                "INSERT INTO ProductImages " +
                "(ProductID, VariantID, ImageName, IsMain) " +
                "VALUES (@ProductID, @VariantID, @ImageName, @IsMain)",
                cn, transaction))
            {
                imageCmd.Parameters.AddWithValue(
                    "@ProductID", productID);

                imageCmd.Parameters.AddWithValue(
                    "@VariantID", variantID);

                imageCmd.Parameters.AddWithValue(
                    "@ImageName", uniqueName);

                imageCmd.Parameters.AddWithValue(
                    "@IsMain", isMain);

                imageCmd.ExecuteNonQuery();
            }
        }
        catch
        {
            if (File.Exists(imagePath))
                File.Delete(imagePath);

            throw;
        }
    }

    private void ShowError(string message)
    {
        lblMessage.Text = message;
        lblMessage.ForeColor = System.Drawing.Color.Red;
    }
}
