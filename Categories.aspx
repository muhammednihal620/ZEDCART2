
<%@ Page Title="Categories - ZED CART"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Categories.aspx.cs"
    Inherits="Categories" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    runat="server">

<style>
.categories-page {
    max-width: 1200px;
    margin: auto;
}
.categories-title {
    text-align: center;
    color: #0b3d91;
    font-size: 32px;
    margin-bottom: 10px;
}
.categories-subtitle {
    text-align: center;
    color: #666;
    margin-bottom: 30px;
}
.category-grid {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 22px;
}
.category-card {
    display: block;
    background: #fff;
    border-radius: 12px;
    padding: 30px 15px;
    text-align: center;
    text-decoration: none;
    color: #222;
    border: 1px solid #e5e7eb;
    box-shadow: 0 4px 12px rgba(0,0,0,0.06);
    transition: 0.2s;
}
.category-card:hover {
    transform: translateY(-5px);
    border-color: #0b3d91;
    box-shadow: 0 8px 20px rgba(0,0,0,0.12);
}
.category-icon {
    font-size: 48px;
    margin-bottom: 15px;
}
.category-card h3 {
    color: #0b3d91;
    margin: 8px 0;
}
.category-card p {
    color: #666;
    font-size: 14px;
    margin-bottom: 0;
}
@media(max-width:700px) {
    .category-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr));
        gap: 12px;
    }
    .category-card {
        padding: 22px 8px;
    }
    .category-icon {
        font-size: 38px;
    }
    .categories-title {
        font-size: 26px;
    }
}
@media(max-width:380px) {
    .category-grid {
        grid-template-columns: 1fr;
    }
}
</style>

</asp:Content>

<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

<div class="categories-page">

    <h1 class="categories-title">Shop By Category</h1>

    <p class="categories-subtitle">
        Explore our products and find what you need.
    </p>

    <div class="category-grid">
        <a class="category-card"
   href="Products.aspx?category=jewllery">

    <div class="category-icon">💍</div>

    <h3>Jewellery</h3>

    <p>Discover beautiful jewellery</p>

</a>

        <a class="category-card"
           href="Products.aspx?category=Mens%20Fashion">
            <div class="category-icon">👔</div>
            <h3>Men's Fashion</h3>
            <p>Fashion and clothing for men</p>
        </a>

        <a class="category-card"
           href="Products.aspx?category=Womens%20Fashion">
            <div class="category-icon">👗</div>
            <h3>Women's Fashion</h3>
            <p>Trendy styles and clothing</p>
        </a>

        <a class="category-card"
           href="Products.aspx?category=Electronics">
            <div class="category-icon">📱</div>
            <h3>Electronics</h3>
            <p>Electronic gadgets and accessories</p>
        </a>

        <a class="category-card"
           href="Products.aspx?category=Beauty">
            <div class="category-icon">🧴</div>
            <h3>Beauty & Personal Care</h3>
            <p>Beauty products and personal care</p>
        </a>

        <a class="category-card"
           href="Products.aspx?category=Home">
            <div class="category-icon">🏠</div>
            <h3>Home & Kitchen</h3>
            <p>Useful products for your home</p>
        </a>

        <a class="category-card"
           href="Products.aspx?category=Accessories">
            <div class="category-icon">⌚</div>
            <h3>Accessories</h3>
            <p>Accessories for everyday use</p>
        </a>

    </div>

</div>

</asp:Content>
