
<%@ Page Title="Customer Registration"
    Language="C#"
    MasterPageFile="~/Site.master"
    AutoEventWireup="true"
    CodeFile="Register.aspx.cs"
    Inherits="Register" %>

<asp:Content ID="Content1"
    ContentPlaceHolderID="ContentPlaceHolder1"
    runat="server">

    <style>
        .register-page {
            font-family: Arial, sans-serif;
            background: #f1f5fb;
            padding: 25px;
        }

        .register-card {
            max-width: 440px;
            margin: 25px auto;
            padding: 25px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 3px 15px #ccd4e0;
        }

        .register-card h2 {
            color: #0b3d91;
            text-align: center;
        }

        .register-field {
            margin-bottom: 15px;
        }

        .register-field label {
            display: block;
            margin-bottom: 6px;
            font-weight: bold;
        }

        .register-field input {
            box-sizing: border-box;
            width: 100%;
            padding: 11px;
            border: 1px solid #bbc5d4;
            border-radius: 5px;
        }

        .register-btn {
            display: block;
            width: 100%;
            padding: 12px;
            margin-top: 8px;
            border: 0;
            border-radius: 5px;
            background: #0b3d91;
            color: white;
            font-weight: bold;
            cursor: pointer;
        }

        .register-btn:hover {
            background: #1976d2;
        }

        .register-btn:disabled {
            background: #9aa9bf;
            cursor: not-allowed;
        }

        .register-message {
            display: block;
            margin: 12px 0;
            color: #b42318;
        }

        .register-section {
            border-top: 1px solid #ddd;
            margin-top: 20px;
            padding-top: 15px;
        }

        .otp-timer {
            display: block;
            margin: 10px 0;
            text-align: center;
            color: #0b3d91;
            font-weight: bold;
        }
    </style>

    <script type="text/javascript">
        function startOtpCountdown(seconds) {
            var resendButton = document.getElementById('<%= btnResendOtp.ClientID %>');
            var timerLabel = document.getElementById('lblOtpTimer');

            if (!resendButton || !timerLabel) {
                return;
            }

            if (window.otpCountdownInterval) {
                window.clearInterval(window.otpCountdownInterval);
            }

            var remaining = parseInt(seconds, 10);

            if (isNaN(remaining) || remaining < 0) {
                remaining = 0;
            }

            function updateTimer() {
                if (remaining > 0) {
                    resendButton.disabled = true;
                    timerLabel.innerHTML =
                        'Resend OTP available in ' + remaining + ' seconds';
                    remaining--;
                } else {
                    resendButton.disabled = false;
                    timerLabel.innerHTML = 'You can resend the OTP now.';
                    window.clearInterval(window.otpCountdownInterval);
                }
            }

            updateTimer();

            if (remaining > 0) {
                window.otpCountdownInterval =
                    window.setInterval(updateTimer, 1000);
            }
        }
    </script>

    <div class="register-page">
        <div class="register-card">

            <h2>ZEDCART Registration</h2>

            <asp:Label ID="lblMessage" runat="server"
                CssClass="register-message"
                EnableViewState="false" />

            <div class="register-field">
                <label for="<%= txtFullName.ClientID %>">Full Name</label>
                <asp:TextBox ID="txtFullName" runat="server"
                    MaxLength="100" />
            </div>

            <div class="register-field">
                <label for="<%= txtEmail.ClientID %>">Email Address</label>
                <asp:TextBox ID="txtEmail" runat="server"
                    TextMode="Email" MaxLength="254" />
            </div>

            <asp:Button ID="btnSendOtp" runat="server"
                Text="Send Email OTP"
                CssClass="register-btn"
                OnClick="btnSendOtp_Click" />

            <asp:Panel ID="pnlOtp" runat="server"
                CssClass="register-section"
                Visible="false">

                <div class="register-field">
                    <label for="<%= txtOtp.ClientID %>">
                        Enter 6-digit OTP
                    </label>

                    <asp:TextBox ID="txtOtp" runat="server"
                        MaxLength="6"
                        TextMode="SingleLine" />
                </div>

                <asp:Button ID="btnVerifyOtp" runat="server"
                    Text="Verify Email"
                    CssClass="register-btn"
                    OnClick="btnVerifyOtp_Click" />

                <asp:Label ID="lblOtpTimer" runat="server"
                    ClientIDMode="Static"
                    CssClass="otp-timer" />

                <asp:Button ID="btnResendOtp" runat="server"
                    Text="Resend OTP"
                    CssClass="register-btn"
                    Enabled="false"
                    OnClick="btnResendOtp_Click" />

            </asp:Panel>

            <asp:Panel ID="pnlRegistration" runat="server"
                CssClass="register-section"
                Visible="false">

                <h3>Complete Registration</h3>

                <div class="register-field">
                    <label for="<%= txtPhone.ClientID %>">Phone Number</label>
                    <asp:TextBox ID="txtPhone" runat="server"
                        MaxLength="10" />
                </div>

                <div class="register-field">
                    <label for="<%= txtPassword.ClientID %>">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password" MaxLength="100" />
                </div>

                <div class="register-field">
                    <label for="<%= txtConfirmPassword.ClientID %>">
                        Confirm Password
                    </label>

                    <asp:TextBox ID="txtConfirmPassword" runat="server"
                        TextMode="Password" MaxLength="100" />
                </div>

                <asp:Button ID="btnRegister" runat="server"
                    Text="Create Account"
                    CssClass="register-btn"
                    OnClick="btnRegister_Click" />

            </asp:Panel>

        </div>
    </div>

</asp:Content>
