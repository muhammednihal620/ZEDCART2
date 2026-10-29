
<%@ Page Title="ZED CART - Home" Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="_Default" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">

    <style type="text/css">
        .hero {
            min-height: 350px;
            background: linear-gradient(90deg, #0b3d91, #1976d2);
            border-radius: 12px;
            margin: 20px 0;
            display: flex;
            align-items: center;
            justify-content: space-between;
            padding: 40px;
            color: white;
            gap: 20px;
        }

        .hero-text {
            flex: 1;
        }

        .hero-text h1 {
            font-size: 42px;
            margin: 0 0 15px;
        }

        .hero-text p {
            font-size: 18px;
            line-height: 1.6;
            margin-bottom: 25px;
        }

        .shop-btn {
            background: #ffcc00;
            color: #111;
            padding: 13px 25px;
            text-decoration: none;
            font-weight: bold;
            border-radius: 6px;
            display: inline-block;
        }

        .hero-image {
            width: 30%;
            text-align: center;
            font-size: 110px;
        }

        .section-title {
            text-align: center;
            margin: 35px 0 20px;
            color: #0b3d91;
            font-size: 28px;
        }

        .categories {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 18px;
        }

        .category {
            background: white;
            padding: 22px 12px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 2px 10px #ddd;
        }

        .category-icon {
            font-size: 40px;
        }

        .category h3 {
            color: #0b3d91;
        }

        .featured-title {
            text-align: center;
            margin: 45px 0 25px;
            color: #0b3d91;
            font-size: 28px;
        }

        .product-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 20px;
            margin-bottom: 40px;
        }

        .product-card {
            background: white;
            padding: 18px;
            text-align: center;
            border-radius: 10px;
            box-shadow: 0 2px 10px #ddd;
            display: flex;
            flex-direction: column;
            align-items: center;
            min-width: 0;
        }

        .product-image {
            height: 160px;
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .product-image img {
            width: 100%;
            height: 150px;
            object-fit: contain;
        }

        .product-card h3 {
            width: 100%;
            min-height: 48px;
            color: #222;
            font-size: 17px;
            margin: 10px 0;
        }

        .price {
            color: #e53935;
            font-size: 21px;
            font-weight: bold;
            margin-top: auto;
            padding-top: 10px;
        }

        .buy-btn,
        .details-btn {
            display: inline-block;
            margin-top: 12px;
            padding: 10px 12px;
            color: white;
            text-decoration: none;
            border-radius: 5px;
            font-size: 13px;
        }

        .buy-btn {
            background: #0b3d91;
        }

        .details-btn {
            background: #28a745;
        }

        .no-products {
            text-align: center;
            padding: 30px;
            color: #666;
            background: white;
            border-radius: 8px;
        }

        @media (max-width: 900px) {
            .categories,
            .product-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
            }

            .hero-text h1 {
                font-size: 34px;
            }
        }

        @media (max-width: 600px) {
            .hero {
                min-height: 0;
                padding: 25px 18px;
                flex-direction: column;
                text-align: center;
            }

            .hero-text h1 {
                font-size: 30px;
            }

            .hero-text p {
                font-size: 16px;
            }

            .hero-image {
                width: 100%;
                font-size: 75px;
            }

            .categories,
            .product-grid {
                grid-template-columns: repeat(2, minmax(0, 1fr));
                gap: 10px;
            }

            .category {
                padding: 15px 8px;
            }

            .product-card {
                padding: 10px;
            }

            .product-image {
                height: 120px;
            }

            .product-image img {
                height: 115px;
            }

            .product-card h3 {
                font-size: 14px;
            }

            .price {
                font-size: 18px;
            }

            .buy-btn,
            .details-btn {
                font-size: 12px;
                padding: 9px 7px;
            }
        }
    </style>

</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <div class="hero">
        <div class="hero-text">
            <h1>Shop Smart. Shop Better.</h1>
            <p>
                Discover amazing products at the best prices.
                Everything you need, all in one place.
            </p>

            <a href="Products.aspx" class="shop-btn">
                SHOP NOW
            </a>
        </div>

        <div class="hero-image">🛍️</div>
    </div>

    <h2 class="section-title">Shop By Category</h2>

    <div class="categories">
        <div class="category">
            <div class="category-icon">📱</div>
            <h3>Mobiles</h3>
            <p>Latest smartphones and accessories</p>
        </div>

        <div class="category">
            <div class="category-icon">👕</div>
            <h3>Fashion</h3>
            <p>Trending clothes and fashion</p>
        </div>

        <div class="category">
            <div class="category-icon">💄</div>
            <h3>Beauty</h3>
            <p>Beauty and personal care products</p>
        </div>

        <div class="category">
            <div class="category-icon">🏠</div>
            <h3>Home</h3>
            <p>Products for your beautiful home</p>
        </div>
    </div>

    <h2 class="featured-title">Featured Products</h2>

    <asp:Panel ID="pnlNoProducts"
        runat="server"
        Visible="false"
        CssClass="no-products">
        No products are available at the moment.
    </asp:Panel>

    <div class="product-grid">
        <asp:Repeater ID="rptProducts" runat="server">
            <ItemTemplate>
                <div class="product-card">
                    <div class="product-image">
                        <img
                            src='<%# ResolveUrl("~/Images/") + Server.UrlPathEncode(Convert.ToString(Eval("ImageName"))) %>'
                            alt='<%# Server.HtmlEncode(Convert.ToString(Eval("ProductName"))) %>' />
                    </div>

                    <h3>
                        <%#: Eval("ProductName") %>
                    </h3>

                    <div class="price">
                        ₹<%# Eval("Price", "{0:0.00}") %>
                    </div>

                    <a
                        href='<%# "ProductDetails.aspx?id=" + Eval("ProductID") %>'
                        class="details-btn">
                        👁 View Details
                    </a>

                    <a
                        href='<%# "Checkout.aspx?product=" + Eval("ProductID") + "&action=buy&quantity=1" %>'
                        class="buy-btn">
                        ⚡ Buy Now
                    </a>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

</asp:Content>
