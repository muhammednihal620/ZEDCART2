
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="About.aspx.cs" Inherits="About"
    MasterPageFile="~/Site.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <style>
        .about-container {
            max-width: 1100px;
            margin: 35px auto;
            padding: 20px;
            font-family: Arial, sans-serif;
            color: #333;
        }

        .about-hero {
            background: linear-gradient(135deg, #0b3d91, #1976d2);
            color: white;
            text-align: center;
            padding: 50px 20px;
            border-radius: 12px;
        }

        .about-hero h1 {
            margin: 0 0 15px;
            font-size: 36px;
        }

        .about-section {
            background: #ffffff;
            margin-top: 22px;
            padding: 25px;
            border-radius: 10px;
            box-shadow: 0 3px 12px rgba(0,0,0,0.08);
        }

        .about-section h2 {
            color: #0b3d91;
            margin-top: 0;
        }

        .about-section ul {
            line-height: 2;
        }

        .about-footer {
            margin-top: 25px;
            padding: 25px;
            background: #fff8dc;
            border-left: 5px solid #ffcc00;
            border-radius: 8px;
            text-align: center;
        }

        @media (max-width: 600px) {
            .about-hero h1 {
                font-size: 28px;
            }

            .about-container {
                padding: 12px;
            }

            .about-section {
                padding: 18px;
            }
        }
    </style>

    <div class="about-container">

        <div class="about-hero">
            <h1>About ZEDCART</h1>
            <p>Your Trusted Online Shopping Destination</p>
            <p>Quality Products. Affordable Prices. Convenient Shopping.</p>
        </div>

        <div class="about-section">
            <h2>Welcome to ZEDCART</h2>
            <p>
                ZEDCART is an online shopping platform designed to make
                shopping simple, convenient and enjoyable.
                We aim to bring a variety of products together
                in one place for our customers.
            </p>
        </div>

        <div class="about-section">
            <h2>Our Mission</h2>
            <p>
                Our mission is to provide customers with quality products,
                affordable prices and a smooth online shopping experience.
            </p>
        </div>

        <div class="about-section">
            <h2>Why Choose ZEDCART?</h2>
            <ul>
                <li>A variety of products</li>
                <li>Affordable prices and offers</li>
                <li>Easy and convenient online shopping</li>
                <li>Simple ordering process</li>
                <li>Available payment options, including Cash on Delivery
                    where supported</li>
                <li>Customer-focused service</li>
            </ul>
        </div>

        <div class="about-section">
            <h2>Our Vision</h2>
            <p>
                We aim to build a trusted online shopping platform
                where customers can shop with confidence and convenience.
            </p>
        </div>

        <div class="about-footer">
            <h2>Thank You for Choosing ZEDCART!</h2>
            <p>Your satisfaction is our priority.</p>
        </div>

    </div>

</asp:Content>
