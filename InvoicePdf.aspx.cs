
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

public partial class InvoicePdf : System.Web.UI.Page
{
    private readonly string connectionString =
        @"Data Source=ASUSVIVOBOOK\SQLEXPRESS;" +
        "Initial Catalog=CheepCart;" +
        "Integrated Security=True";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;

        if (Session["CustomerID"] == null)
        {
            Response.Redirect("Login.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return;
        }

        int orderID;

        if (!int.TryParse(Request.QueryString["OrderID"], out orderID)
            || orderID <= 0)
        {
            Response.StatusCode = 400;
            Response.ContentType = "text/plain";
            Response.Write("Invalid Order ID.");
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return;
        }

        int customerID;

        if (!int.TryParse(
            Convert.ToString(Session["CustomerID"]), out customerID))
        {
            Response.Redirect("Login.aspx", false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return;
        }

        try
        {
            byte[] pdfBytes;

            using (SqlConnection con =
                new SqlConnection(connectionString))
            {
                string query =
                    "SELECT OrderID, CustomerName, Address, Phone, " +
                    "Pincode, Landmark, ProductName, Price, Quantity, " +
                    "Total, OrderDate, PaymentMethod, OrderStatus " +
                    "FROM OrderDetails " +
                    "WHERE OrderID = @OrderID " +
                    "AND CustomerID = @CustomerID";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@OrderID", SqlDbType.Int).Value =
                        orderID;

                    cmd.Parameters.Add("@CustomerID", SqlDbType.Int).Value =
                        customerID;

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            Response.StatusCode = 404;
                            Response.ContentType = "text/plain";
                            Response.Write(
                                "Order not found or access denied.");

                            HttpContext.Current.ApplicationInstance
                                .CompleteRequest();
                            return;
                        }

                        string customerName =
                            GetValue(dr, "CustomerName");

                        string address = GetValue(dr, "Address");
                        string phone = GetValue(dr, "Phone");
                        string pincode = GetValue(dr, "Pincode");
                        string landmark = GetValue(dr, "Landmark");
                        string productName = GetValue(dr, "ProductName");
                        string paymentMethod =
                            GetValue(dr, "PaymentMethod");

                        string status = GetValue(dr, "OrderStatus");

                        int quantity = 0;
                        int.TryParse(
                            GetValue(dr, "Quantity"), out quantity);

                        decimal price = 0;
                        decimal.TryParse(
                            GetValue(dr, "Price"), out price);

                        decimal total = 0;
                        decimal.TryParse(
                            GetValue(dr, "Total"), out total);

                        string orderDate = "";

                        if (dr["OrderDate"] != DBNull.Value)
                        {
                            orderDate = Convert.ToDateTime(
                                dr["OrderDate"]).ToString("dd-MM-yyyy");
                        }

                        using (MemoryStream ms = new MemoryStream())
                        {
                            Document doc = new Document(
                                PageSize.A4, 36, 36, 36, 36);

                            PdfWriter.GetInstance(doc, ms);
                            doc.Open();

                            try
                            {
                                iTextSharp.text.Font titleFont =
                                    FontFactory.GetFont(
                                        FontFactory.HELVETICA_BOLD, 22);

                                iTextSharp.text.Font headingFont =
                                    FontFactory.GetFont(
                                        FontFactory.HELVETICA_BOLD, 12);

                                iTextSharp.text.Font normalFont =
                                    FontFactory.GetFont(
                                        FontFactory.HELVETICA, 11);

                                Paragraph title =
                                    new Paragraph("ZEDCART", titleFont);

                                title.Alignment = Element.ALIGN_CENTER;
                                doc.Add(title);

                                Paragraph company = new Paragraph(
                                    "Nallalam Bazar, Kozhikode, Kerala - 673027\n" +
                                    "Contact: 5874509976\n\n",
                                    normalFont);

                                company.Alignment = Element.ALIGN_CENTER;
                                doc.Add(company);

                                Paragraph invoiceTitle =
                                    new Paragraph("INVOICE\n\n", headingFont);

                                invoiceTitle.Alignment =
                                    Element.ALIGN_CENTER;

                                doc.Add(invoiceTitle);

                                PdfPTable table = new PdfPTable(2);
                                table.WidthPercentage = 100;
                                table.SetWidths(new float[] { 35, 65 });

                                AddRow(table, "Order ID",
                                    orderID.ToString(),
                                    headingFont, normalFont);

                                AddRow(table, "Order Date",
                                    orderDate, headingFont, normalFont);

                                AddRow(table, "Customer Name",
                                    customerName, headingFont, normalFont);

                                AddRow(table, "Address",
                                    address, headingFont, normalFont);

                                AddRow(table, "Pincode",
                                    pincode, headingFont, normalFont);

                                AddRow(table, "Landmark",
                                    landmark, headingFont, normalFont);

                                AddRow(table, "Phone",
                                    phone, headingFont, normalFont);

                                AddRow(table, "Product",
                                    productName, headingFont, normalFont);

                                AddRow(table, "Quantity",
                                    quantity.ToString(),
                                    headingFont, normalFont);

                                AddRow(table, "Unit Price",
                                    "Rs. " + price.ToString("0.00"),
                                    headingFont, normalFont);

                                AddRow(table, "Total",
                                    "Rs. " + total.ToString("0.00"),
                                    headingFont, normalFont);

                                AddRow(table, "Payment Method",
                                    paymentMethod, headingFont, normalFont);

                                AddRow(table, "Order Status",
                                    status, headingFont, normalFont);

                                doc.Add(table);

                                Paragraph footer = new Paragraph(
                                    "\nThank you for shopping with ZedCart!",
                                    normalFont);

                                footer.Alignment = Element.ALIGN_CENTER;
                                doc.Add(footer);
                            }
                            finally
                            {
                                if (doc.IsOpen())
                                    doc.Close();
                            }

                            pdfBytes = ms.ToArray();
                        }
                    }
                }
            }

            // Send the completed PDF to the browser.
            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            Response.AddHeader(
                "Content-Disposition",
                "attachment; filename=ZedCart_Invoice_" +
                orderID.ToString() + ".pdf");

            Response.AddHeader(
                "Content-Length",
                pdfBytes.Length.ToString());


            Response.BinaryWrite(pdfBytes);

            // Do not call Response.End() here.
            // It can throw ThreadAbortException.
            HttpContext.Current.ApplicationInstance.CompleteRequest();
            return;

        }
        catch (Exception ex)
        {
            // Log the full exception on the server in production.
            System.Diagnostics.Trace.TraceError(
                "Invoice PDF error: " + ex.ToString());

            if (!Response.IsClientConnected)
                return;

            Response.Clear();
            Response.StatusCode = 500;
            Response.TrySkipIisCustomErrors = true;
            Response.ContentType = "text/plain";

            Response.Write(
                "Invoice download failed. Please try again later.");

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    private string GetValue(SqlDataReader dr, string columnName)
    {
        if (dr[columnName] == DBNull.Value)
            return "";

        return Convert.ToString(dr[columnName]);
    }

    private void AddRow(
        PdfPTable table,
        string heading,
        string value,
        iTextSharp.text.Font headingFont,
        iTextSharp.text.Font normalFont)
    {
        PdfPCell cell1 = new PdfPCell(
            new Phrase(heading, headingFont));

        PdfPCell cell2 = new PdfPCell(
            new Phrase(value ?? "", normalFont));

        cell1.Padding = 7;
        cell2.Padding = 7;

        table.AddCell(cell1);
        table.AddCell(cell2);
    }
}
