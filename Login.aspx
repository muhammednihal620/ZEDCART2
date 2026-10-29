<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">

        .login-page {
            width: 450px;
            margin: 50px auto;
            background-color: white;
            padding: 35px;
            border-radius: 10px;
            box-shadow: 0 2px 10px #ddd;
        }

        .login-page h1 {
            text-align: center;
            color: #0b3d91;
            margin-bottom: 30px;
        }

        .form-row {
            margin-bottom: 20px;
        }

        .form-row label {
            display: block;
            font-weight: bold;
            margin-bottom: 8px;
        }

        .input-box {
            width: 100%;
            padding: 12px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }

        .login-btn {
            width: 100%;
            padding: 13px;
            background-color: #0b3d91;
            color: white;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
        }

        .login-btn:hover {
            background-color: #ffcc00;
            color: black;
        }

        .message {
            display: block;
            margin-top: 15px;
            text-align: center;
        }

        .register-link {
            display: block;
            text-align: center;
            margin-top: 20px;
        }

    </style>

</asp:Content>


<asp:Content ID="Content2"
    ContentPlaceHolderID="ContentPlaceHolder1"
    Runat="Server">

    <div class="login-page">

        <h1>Customer Login</h1>

        <div class="form-row">
            <label>Email</label>

            <asp:TextBox ID="txtEmail"
                runat="server"
                CssClass="input-box">
            </asp:TextBox>
        </div>


        <div class="form-row">
            <label>Password</label>

            <asp:TextBox ID="txtPassword"
                runat="server"
                CssClass="input-box"
                TextMode="Password">
            </asp:TextBox>
        </div>


        <asp:Button ID="Button1"
            runat="server"
            Text="Login"
            CssClass="login-btn"
            OnClick="Button1_Click" />


        <asp:Label ID="Label1"
            runat="server"
            CssClass="message">
        </asp:Label>


        <a href="Register.aspx" class="register-link">
            Don't have an account? Register
        </a>

    </div>

</asp:Content>