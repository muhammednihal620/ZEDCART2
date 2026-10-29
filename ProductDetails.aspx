
<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="ProductDetails.aspx.cs"
    Inherits="ProductDetails" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="head"
    Runat="Server">

<style type="text/css">

.product-details {
    width: 94%;
    margin: 35px auto;
    background: white;
    padding: 30px;
    border-radius: 12px;
    box-shadow: 0 2px 12px #ddd;
    display: flex;
    gap: 40px;
    align-items: flex-start;
}

.product-gallery {
    width: 45%;
    text-align: center;
}

.main-image-frame {
    position: relative;
    width: 100%;
    height: 390px;
    display: flex;
    align-items: center;
    justify-content: center;
    overflow: hidden;
    background: white;
    touch-action: pan-y;
}

.main-product-image {
    width: 100%;
    max-width: 370px;
    height: 370px;
    object-fit: contain;
    user-select: none;
    -webkit-user-drag: none;
}

.gallery-arrow {
    position: absolute;
    top: 45%;
    width: 40px;
    height: 46px;
    background: white;
    border: 1px solid #ddd;
    border-radius: 5px;
    font-size: 26px;
    cursor: pointer;
    z-index: 5;
}

.gallery-prev {
    left: 5px;
}

.gallery-next {
    right: 5px;
}

.thumbnail-list {
    display: flex;
    justify-content: center;
    gap: 10px;
    flex-wrap: wrap;
    margin-top: 15px;
}

.product-thumb {
    width: 65px;
    height: 65px;
    object-fit: contain;
    border: 2px solid #ddd;
    border-radius: 5px;
    padding: 3px;
    cursor: pointer;
    background: white;
}

.product-thumb.active {
    border-color: #0b3d91;
}

.product-details-info {
    width: 55%;
}

.product-details-info h1 {
    color: #0b3d91;
    font-size: 32px;
    margin-top: 5px;
}

.description {
    color: #444;
    font-size: 17px;
    line-height: 1.6;
}

.price {
    color: #e53935;
    font-size: 30px;
    font-weight: bold;
    margin: 20px 0;
}

.variant-info p {
    margin: 12px 0;
}

.quantity {
    font-size: 17px;
    margin: 25px 0;
}

.quantity select {
    padding: 8px;
    margin-left: 10px;
}

.add-cart, .buy-now {
    color: white;
    border: none;
    padding: 13px 20px;
    border-radius: 5px;
    font-size: 16px;
    cursor: pointer;
    margin: 5px 5px 5px 0;
}

.add-cart {
    background: #0b3d91;
}

.buy-now {
    background: #28a745;
}

@media screen and (max-width: 750px) {
    .product-details {
        display: block;
        width: 100%;
        padding: 15px;
    }

    .product-gallery, .product-details-info {
        width: 100%;
    }

    .main-image-frame {
        height: 320px;
    }

    .main-product-image {
        height: 300px;
    }
}

</style>

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">

<div class="product-details">

    <div class="product-gallery">

        <div class="main-image-frame" id="imageSwipeArea">

            <button type="button"
                class="gallery-arrow gallery-prev"
                onclick="changeProductImage(-1); return false;">
                &#8249;
            </button>

            <asp:Image ID="imgProduct"
                runat="server"
                
                CssClass="main-product-image"
                AlternateText="Product image" />

            <button type="button"
                class="gallery-arrow gallery-next"
                onclick="changeProductImage(1); return false;">
                &#8250;
            </button>

        </div>

        <div class="thumbnail-list" id="thumbnails">

            <asp:Repeater ID="rptImages" runat="server">
                <ItemTemplate>
                    <img
                        class="product-thumb"
                        src='<%# "Images/" + Eval("ImageName") %>'
                        alt="Product thumbnail"
                        onclick="showProductImage(this);" />
                </ItemTemplate>
            </asp:Repeater>

        </div>

    </div>


    <div class="product-details-info">

        <h1>
            <asp:Label ID="lblProductName" runat="server"></asp:Label>
        </h1>

        <p class="description">
            <asp:Label ID="lblDescription" runat="server"></asp:Label>
        </p>

         
<div class="price">
    ₹<asp:Label ID="lblPrice" runat="server"></asp:Label>

    <span style="font-size:18px; color:#777; margin-left:10px;">
        <del>₹<asp:Label ID="lblMRP" runat="server"></asp:Label></del>
    </span>

    <span style="font-size:16px; color:green; margin-left:8px;">
        <asp:Label ID="lblOffer" runat="server"></asp:Label>
    </span>
</div>

<p style="color:green;">
    You save ₹<asp:Label ID="lblSavings" runat="server"></asp:Label>
</p>

       
<div class="variant-info">

    <p>
        <b>Color:</b>
        <asp:DropDownList ID="ddlColor" runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlColor_SelectedIndexChanged">
        </asp:DropDownList>
    </p>

    <p>
        <b>Size:</b>
        <asp:DropDownList ID="ddlSize" runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlSize_SelectedIndexChanged">
        </asp:DropDownList>
    </p>

    <p>
        <b>Model:</b>
        <asp:Label ID="lblModel" runat="server"></asp:Label>
    </p>

    <p>
        <b>Available Stock:</b>
        <asp:Label ID="lblStock" runat="server"></asp:Label>
    </p>

</div>

        <div class="quantity">

            Quantity:

            <asp:DropDownList ID="ddlQuantity" runat="server">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Value="2">2</asp:ListItem>
                <asp:ListItem Value="3">3</asp:ListItem>
                <asp:ListItem Value="4">4</asp:ListItem>
                <asp:ListItem Value="5">5</asp:ListItem>
            </asp:DropDownList>

        </div>

        <asp:Button ID="btnAddCart"
            runat="server"
            Text="🛒 Add to Cart"
            CssClass="add-cart"
            OnClick="btnAddCart_Click" />

        <asp:Button ID="btnBuyNow"
            runat="server"
            Text="⚡ Buy Now"
            CssClass="buy-now"
            OnClick="btnBuyNow_Click" />

    </div>

</div>


<script type="text/javascript">

    var currentImageIndex = 0;
    var swipeStartX = 0;

    function getProductThumbnails() {
        var area = document.getElementById('thumbnails');
        var images = [];

        if (area) {
            var allImages = area.getElementsByTagName('img');

            for (var i = 0; i < allImages.length; i++) {
                images.push(allImages[i]);
            }
        }

        return images;
    }

    function showProductImage(thumbnail) {
        var mainImage = document.getElementById('<%= imgProduct.ClientID %>');
        var images = getProductThumbnails();

        if (!mainImage || !thumbnail) {
            return;
        }

        mainImage.src = thumbnail.src;

        for (var i = 0; i < images.length; i++) {
            images[i].className = 'product-thumb';

            if (images[i] == thumbnail) {
                currentImageIndex = i;
                images[i].className = 'product-thumb active';
            }
        }
    }

    function changeProductImage(direction) {
        var images = getProductThumbnails();

        if (images.length < 2) {
            return;
        }

        currentImageIndex = currentImageIndex + direction;

        if (currentImageIndex >= images.length) {
            currentImageIndex = 0;
        }

        if (currentImageIndex < 0) {
            currentImageIndex = images.length - 1;
        }

        showProductImage(images[currentImageIndex]);
    }

    function setupImageSwipe() {
        var swipeArea = document.getElementById('imageSwipeArea');

        if (swipeArea) {
            swipeArea.addEventListener('touchstart', function(e) {
                if (e.touches.length > 0) {
                    swipeStartX = e.touches[0].clientX;
                }
            }, false);

            swipeArea.addEventListener('touchend', function(e) {
                if (e.changedTouches.length > 0) {
                    var swipeEndX = e.changedTouches[0].clientX;
                    var distance = swipeEndX - swipeStartX;

                    if (distance < -40) {
                        changeProductImage(1);
                    }
                    else if (distance > 40) {
                        changeProductImage(-1);
                    }
                }
            }, false);
        }

        var images = getProductThumbnails();

        if (images.length > 0) {
            showProductImage(images[0]);
        }
    }

    if (window.addEventListener) {
        window.addEventListener('load', setupImageSwipe, false);
    }
    else {
        window.onload = setupImageSwipe;
    }

</script>

</asp:Content>

