using System;
using System.Data;
using System.Data.SqlClient;

public partial class Checkout : System.Web.UI.Page
{
    SqlConnection con;

    protected void Page_Load(object sender, EventArgs e)
    {
        con = new SqlConnection(
            "Data Source=ASUSVIVOBOOK\\SQLEXPRESS;" +
            "Initial Catalog=CheepCart;" +
            "Integrated Security=True"
        );

        if (Session["CustomerID"] == null)
        {
            string returnUrl = Request.RawUrl;

            Response.Redirect(
                "Login.aspx?returnUrl=" +
                Server.UrlEncode(returnUrl)
            );

            return;
        }

        if (!IsPostBack)
        {
            string action =
                Request.QueryString["action"];

            if (action == "buy")
            {
                LoadBuyNow();
            }
            else
            {
                LoadCart();
            }
        }
    }


    protected void LoadBuyNow()
    {
        int productID;

        if (!int.TryParse(
            Request.QueryString["product"],
            out productID))
        {
            Response.Redirect("Products.aspx", true);
            return;
        }

        int quantity = 1;

        if (Request.QueryString["quantity"] != null)
        {
            int.TryParse(
                Request.QueryString["quantity"],
                out quantity);

            if (quantity <= 0)
            {
                quantity = 1;
            }
        }

        con.Open();

        SqlCommand cmd =
            new SqlCommand(
                "SELECT TOP 1 " +
                "p.ProductID, " +
                "p.ProductName, " +
                "v.Price, " +
                "v.Stock " +
                "FROM Products p " +
                "INNER JOIN ProductVariants v " +
                "ON p.ProductID = v.ProductID " +
                "WHERE p.ProductID=@ProductID",
                con
            );

        cmd.Parameters.AddWithValue(
            "@ProductID",
            productID);

        SqlDataReader dr =
            cmd.ExecuteReader();

        if (!dr.Read())
        {
            dr.Close();
            con.Close();

            Response.Redirect(
                "Products.aspx",
                true);

            return;
        }

        int price =
            Convert.ToInt32(
                Convert.ToDecimal(dr["Price"]));

        string productName =
            dr["ProductName"].ToString();

        int stock =
            Convert.ToInt32(dr["Stock"]);

        dr.Close();
        con.Close();


        if (stock <= 0)
        {
            ShowMessage(
                "This product is out of stock.");
            return;
        }

        if (quantity > stock)
        {
            quantity = stock;
        }


        DataTable dt =
            new DataTable();

        dt.Columns.Add(
            "ProductID",
            typeof(int));

        dt.Columns.Add(
            "ProductName",
            typeof(string));

        dt.Columns.Add(
            "Price",
            typeof(int));

        dt.Columns.Add(
            "Quantity",
            typeof(int));

        dt.Columns.Add(
            "Total",
            typeof(int));


        DataRow row =
            dt.NewRow();

        row["ProductID"] =
            productID;

        row["ProductName"] =
            productName;

        row["Price"] =
            price;

        row["Quantity"] =
            quantity;

        row["Total"] =
            price * quantity;

        dt.Rows.Add(row);


        Session["BuyNowCart"] =
            dt;


        GridView1.DataSource =
            dt;

        GridView1.DataBind();


        lblGrandTotal.Text =
            (price * quantity).ToString();


        if (Session["CustomerName"] != null)
        {
            txtName.Text =
                Session["CustomerName"].ToString();
        }
    }


    protected void LoadCart()
    {
        if (Session["Cart"] == null)
        {
            Response.Redirect(
                "Cart.aspx",
                true);

            return;
        }


        DataTable cart =
            (DataTable)Session["Cart"];


        if (cart.Rows.Count == 0)
        {
            Response.Redirect(
                "Cart.aspx",
                true);

            return;
        }


        GridView1.DataSource =
            cart;

        GridView1.DataBind();


        int grandTotal = 0;


        foreach (DataRow row in cart.Rows)
        {
            grandTotal +=
                Convert.ToInt32(
                    row["Total"]);
        }


        lblGrandTotal.Text =
            grandTotal.ToString();


        if (Session["CustomerName"] != null)
        {
            txtName.Text =
                Session["CustomerName"].ToString();
        }
    }


    protected void Button1_Click(
        object sender,
        EventArgs e)
    {
        if (Session["CustomerID"] == null)
        {
            Response.Redirect(
                "Login.aspx",
                true);

            return;
        }


        if (txtName.Text.Trim() == "")
        {
            ShowMessage(
                "Please enter customer name");

            txtName.Focus();

            return;
        }


        if (txtPhone.Text.Trim() == "")
        {
            ShowMessage(
                "Please enter mobile number");

            txtPhone.Focus();

            return;
        }


        if (txtAddress.Text.Trim() == "")
        {
            ShowMessage(
                "Please enter delivery address");

            txtAddress.Focus();

            return;
        }


        if (txtPincode.Text.Trim() == "")
        {
            ShowMessage(
                "Please enter pincode");

            txtPincode.Focus();

            return;
        }


        if (txtLandmark.Text.Trim() == "")
        {
            ShowMessage(
                "Please enter landmark");

            txtLandmark.Focus();

            return;
        }


        if (txtPhone.Text.Trim().Length != 10)
        {
            ShowMessage(
                "Enter valid 10 digit mobile number");

            txtPhone.Focus();

            return;
        }


        if (txtPincode.Text.Trim().Length != 6)
        {
            ShowMessage(
                "Enter valid 6 digit pincode");

            txtPincode.Focus();

            return;
        }


        string action =
            Request.QueryString["action"];


        DataTable orderTable = null;


        if (action == "buy")
        {
            if (Session["BuyNowCart"] == null)
            {
                Response.Redirect(
                    "Products.aspx",
                    true);

                return;
            }

            orderTable =
                (DataTable)Session["BuyNowCart"];
        }
        else
        {
            if (Session["Cart"] == null)
            {
                Response.Redirect(
                    "Cart.aspx",
                    true);

                return;
            }

            orderTable =
                (DataTable)Session["Cart"];
        }


        if (orderTable.Rows.Count == 0)
        {
            Response.Redirect(
                "Cart.aspx",
                true);

            return;
        }


        SqlTransaction transaction = null;


        try
        {
            con.Open();

            transaction =
                con.BeginTransaction();


            foreach (DataRow row in orderTable.Rows)
            {
                SqlCommand cmd =
                    new SqlCommand(
                        "INSERT INTO OrderDetails " +
                        "(CustomerID, CustomerName, Address, Phone, " +
                        "Pincode, Landmark, ProductName, Price, " +
                        "Quantity, Total, OrderDate, PaymentMethod, " +
                        "OrderStatus) " +

                        "VALUES " +

                        "(@CustomerID, @CustomerName, @Address, @Phone, " +
                        "@Pincode, @Landmark, @ProductName, @Price, " +
                        "@Quantity, @Total, @OrderDate, @PaymentMethod, " +
                        "@OrderStatus)",
                        con,
                        transaction
                    );


                cmd.Parameters.AddWithValue(
                    "@CustomerID",
                    Convert.ToInt32(
                        Session["CustomerID"]));


                cmd.Parameters.AddWithValue(
                    "@CustomerName",
                    txtName.Text.Trim());


                cmd.Parameters.AddWithValue(
                    "@Address",
                    txtAddress.Text.Trim());


                cmd.Parameters.AddWithValue(
                    "@Phone",
                    txtPhone.Text.Trim());


                cmd.Parameters.AddWithValue(
                    "@Pincode",
                    txtPincode.Text.Trim());


                cmd.Parameters.AddWithValue(
                    "@Landmark",
                    txtLandmark.Text.Trim());


                cmd.Parameters.AddWithValue(
                    "@ProductName",
                    row["ProductName"].ToString());


                cmd.Parameters.AddWithValue(
                    "@Price",
                    Convert.ToInt32(
                        row["Price"]));


                cmd.Parameters.AddWithValue(
                    "@Quantity",
                    Convert.ToInt32(
                        row["Quantity"]));


                cmd.Parameters.AddWithValue(
                    "@Total",
                    Convert.ToInt32(
                        row["Total"]));


                cmd.Parameters.AddWithValue(
                    "@OrderDate",
                    DateTime.Now);


                cmd.Parameters.AddWithValue(
                    "@PaymentMethod",
                    ddlPayment.SelectedValue);


                cmd.Parameters.AddWithValue(
                    "@OrderStatus",
                    "Placed");


                cmd.ExecuteNonQuery();
            }


            transaction.Commit();


            if (action == "buy")
            {
                Session["BuyNowCart"] =
                    null;
            }
            else
            {
                Session["Cart"] =
                    null;
            }


            pnlOrderSuccess.Visible = true;

        }
        catch (Exception ex)
        {
            if (transaction != null)
            {
                transaction.Rollback();
            }


            ShowMessage(
                "Order failed: " +
                ex.Message.Replace("'", ""));
        }
        finally
        {
            if (con.State ==
                System.Data.ConnectionState.Open)
            {
                con.Close();
            }
        }
    }


    protected void ShowMessage(string message)
    {
        ClientScript.RegisterStartupScript(
            this.GetType(),
            Guid.NewGuid().ToString(),
            "alert('" +
            message.Replace("'", "") +
            "');",
            true
        );
    }

    protected void btnContinueShopping_Click(
        object sender, EventArgs e)
    {
        Response.Redirect("MyOrders.aspx");
    }
}