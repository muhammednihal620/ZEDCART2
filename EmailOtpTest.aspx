
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="EmailOtpTest.aspx.cs"
    Inherits="EmailOtpTest" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>ZEDCART Email OTP Test</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <style>
        body {
            font-family: Arial, sans-serif;
            background: #f1f5fb;
            padding: 25px;
        }
        .card {
            max-width: 420px;
            margin: 30px auto;
            padding: 25px;
            background: white;
            border-radius: 10px;
        }
        .field { margin-bottom: 15px; }
        .field input {
            box-sizing: border-box;
            width: 100%;
            padding: 11px;
        }
        .btn {
            width: 100%;
            padding: 12px;
            background: #0b3d91;
            color: white;
            border: 0;
            cursor: pointer;
        }
        .message {
            display: block;
            margin: 12px 0;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
    <div class="card">
        <h2>ZEDCART Email OTP Test</h2>

        <asp:Label ID="lblMessage" runat="server"
            CssClass="message" EnableViewState="false" />

        <div class="field">
            <label>Email Address</label>
            <asp:TextBox ID="txtEmail" runat="server"
                TextMode="Email" MaxLength="254" />
        </div>

        <asp:Button ID="btnSendOtp" runat="server"
            Text="Send OTP" CssClass="btn"
            OnClick="btnSendOtp_Click" />

        <asp:Panel ID="pnlVerify" runat="server"
            Visible="false">
            <hr />
            <div class="field">
                <label>6-digit OTP</label>
                <asp:TextBox ID="txtOtp" runat="server"
                    MaxLength="6" />
            </div>

               
<asp:Button ID="btnVerify" runat="server"
    Text="Verify OTP" CssClass="btn"
    OnClick="btnVerify_Click" />

<asp:Label ID="lblOtpTimer" runat="server"
    ClientIDMode="Static"
    CssClass="message" />

<asp:Button ID="btnResendOtp" runat="server"
    Text="Resend OTP"
    CssClass="btn"
    OnClick="btnResendOtp_Click"
    Enabled="false" />

</asp:Panel>

    </div>
</form>
</body>
</html>
