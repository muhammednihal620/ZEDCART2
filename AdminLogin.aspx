<%@ Page Language="C#" AutoEventWireup="true"
CodeFile="AdminLogin.aspx.cs"
Inherits="AdminLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Admin Login</title>


<style type="text/css">
    body
    {
        margin: 0;
        padding: 0;
        font-family: Arial;
        background-color: #f5f7fa;
    }

    .login-container
    {
        width: 380px;
        margin: 100px auto;
        background-color: white;
        padding: 35px;
        border-radius: 10px;
        box-shadow: 0 2px 15px #ccc;
    }

    .title
    {
        text-align: center;
        color: #0b3d91;
        font-size: 28px;
        font-weight: bold;
        margin-bottom: 10px;
    }

    .subtitle
    {
        text-align: center;
        color: #777;
        margin-bottom: 30px;
    }

    .label
    {
        display: block;
        font-weight: bold;
        color: #444;
        margin-bottom: 7px;
    }

    .textbox
    {
        width: 100%;
        padding: 11px;
        border: 1px solid #ccc;
        border-radius: 5px;
        margin-bottom: 18px;
        box-sizing: border-box;
    }

    .login-btn
    {
        width: 100%;
        padding: 12px;
        background-color: #0b3d91;
        color: white;
        border: none;
        border-radius: 5px;
        font-size: 16px;
        font-weight: bold;
        cursor: pointer;
    }

    .login-btn:hover
    {
        background-color: #082f70;
    }

    .message
    {
        display: block;
        text-align: center;
        margin-top: 15px;
    }
</style>


</head>

<body>

<form id="form1" runat="server">


<div class="login-container">

    <div class="title">
        Admin Login
    </div>

    <div class="subtitle">
        Login to manage orders
    </div>

    <asp:Label ID="Label1"
        runat="server"
        Text="Username"
        CssClass="label">
    </asp:Label>

    <asp:TextBox ID="txtUsername"
        runat="server"
        CssClass="textbox">
    </asp:TextBox>

    <asp:Label ID="Label2"
        runat="server"
        Text="Password"
        CssClass="label">
    </asp:Label>

    <asp:TextBox ID="txtPassword"
        runat="server"
        CssClass="textbox"
        TextMode="Password">
    </asp:TextBox>

    <asp:Button ID="btnLogin"
        runat="server"
        Text="Login"
        CssClass="login-btn"
        OnClick="btnLogin_Click" />

    <asp:Label ID="lblMessage"
        runat="server"
        CssClass="message">
    </asp:Label>

</div>


</form>

</body>
</html>
