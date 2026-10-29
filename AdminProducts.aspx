
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="AdminProducts.aspx.cs"
    Inherits="AdminProducts" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Admin Products</title>

    <style type="text/css">
        body {
            margin: 0;
            font-family: Arial;
            background: #f5f7fa;
        }

        .admin-page {
            width: 850px;
            max-width: 90%;
            margin: 30px auto;
            background: white;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 2px 10px #ddd;
        }

        .title {
            color: #0b3d91;
            font-size: 28px;
            font-weight: bold;
            margin-bottom: 25px;
        }

        .section-title {
            background: #0b3d91;
            color: white;
            padding: 12px;
            border-radius: 5px;
            margin-top: 25px;
            margin-bottom: 20px;
            font-size: 18px;
            font-weight: bold;
        }

        .form-row {
            margin-bottom: 16px;
        }

        .form-row label {
            display: block;
            font-weight: bold;
            color: #444;
            margin-bottom: 7px;
        }

        .textbox {
            width: 100%;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
            font-size: 14px;
        }

        .textarea {
            width: 100%;
            height: 80px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            box-sizing: border-box;
        }

        .variant-box {
            background: #f8f9fb;
            border: 1px solid #ddd;
            padding: 18px;
            border-radius: 8px;
            margin-bottom: 15px;
        }

        .variant-heading {
            color: #0b3d91;
            font-weight: bold;
            font-size: 17px;
            margin-bottom: 15px;
        }

        .add-btn {
            background: #0b3d91;
            color: white;
            border: none;
            padding: 12px 22px;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            margin: 8px 0 18px 0;
        }

        .save-btn {
            background: #138443;
            color: white;
            border: none;
            padding: 13px 25px;
            border-radius: 5px;
            cursor: pointer;
            font-weight: bold;
            font-size: 16px;
        }

        .message {
            display: block;
            margin-top: 15px;
            font-weight: bold;
            overflow-wrap: anywhere;
        }

        .image-note {
            color: #777;
            font-size: 13px;
            margin-top: 5px;
        }

        .back-btn {
            display: inline-block;
            margin-top: 20px;
            color: #0b3d91;
            text-decoration: none;
            font-weight: bold;
        }
    </style>
</head>

<body>
<form id="form1" runat="server">

<div class="admin-page">

    <div class="title">Admin - Add Product</div>

    <div class="section-title">1. Product Details</div>

    <div class="form-row">
        <label>Product Name</label>
        <asp:TextBox ID="txtProductName" runat="server"
            CssClass="textbox"></asp:TextBox>
    </div>

    <div class="form-row">
        <label>Description</label>
        <asp:TextBox ID="txtDescription" runat="server"
            TextMode="MultiLine" CssClass="textarea"></asp:TextBox>
    </div>

    <div class="form-row">
        <label>Category</label>
        <asp:TextBox ID="txtCategory" runat="server"
            CssClass="textbox"></asp:TextBox>
    </div>

    <div class="section-title">2. Product Variants</div>

    <asp:HiddenField ID="hfVariantCount" runat="server"
        Value="1" />

    <asp:PlaceHolder ID="phVariants" runat="server">
    </asp:PlaceHolder>

    <asp:Button ID="btnAddVariant" runat="server"
        Text="+ Add More Variant"
        CssClass="add-btn"
        OnClick="btnAddVariant_Click" />

    <div class="section-title">3. Product Images</div>

    <div class="form-row">
        <label>Image 1 (Main Image)</label>
        <asp:FileUpload ID="fuImage1" runat="server" />
    </div>

    <div class="form-row">
        <label>Image 2</label>
        <asp:FileUpload ID="fuImage2" runat="server" />
    </div>

    <div class="form-row">
        <label>Image 3</label>
        <asp:FileUpload ID="fuImage3" runat="server" />
    </div>

    <div class="form-row">
        <label>Image 4</label>
        <asp:FileUpload ID="fuImage4" runat="server" />
        <div class="image-note">
            Up to 4 images per product. Images are linked
            to the first variant.
        </div>
    </div>

    <asp:Button ID="btnAddProduct" runat="server"
        Text="Add Product"
        CssClass="save-btn"
        OnClick="btnAddProduct_Click" />

    <asp:Label ID="lblMessage" runat="server"
        CssClass="message"></asp:Label>

    <br />
    <div class="section-title">4. Manage Products</div>
    <div class="form-row">
    <label>Search by Product ID</label>

    <asp:TextBox ID="txtSearchProductID"
        runat="server"
        CssClass="textbox"
        placeholder="Enter Product ID">
    </asp:TextBox>

    <asp:Button ID="btnSearchProduct"
        runat="server"
        Text="Search"
        CssClass="add-btn"
        CausesValidation="false"
        OnClick="btnSearchProduct_Click" />

    <asp:Button ID="btnShowAllProducts"
        runat="server"
        Text="Show All"
        CssClass="add-btn"
        CausesValidation="false"
        OnClick="btnShowAllProducts_Click" />
</div>

<asp:GridView ID="gvProducts" runat="server"
    AutoGenerateColumns="False"
    DataKeyNames="ProductID"
    Width="100%"
    CellPadding="8"
    GridLines="Both"
    OnRowCommand="gvProducts_RowCommand">

    <Columns>
        <asp:BoundField DataField="ProductID"
            HeaderText="Product ID" />

        <asp:BoundField DataField="ProductName"
            HeaderText="Product Name" />

        <asp:BoundField DataField="Category"
            HeaderText="Category" />

        <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:Button ID="btnDelete" runat="server"
                    Text="Delete"
                    CommandName="DeleteProduct"
                    CommandArgument='<%# Eval("ProductID") %>'
                    OnClientClick="return confirm('ഈ product delete ചെയ്യണോ?');" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>

</asp:GridView>

<asp:Label ID="lblDeleteMessage" runat="server"
    CssClass="message"></asp:Label>

    <a href="AdminDashboard.aspx" class="back-btn">
        &lt; Back to Admin Dashboard
    </a>

</div>
</form>
</body>
</html>
