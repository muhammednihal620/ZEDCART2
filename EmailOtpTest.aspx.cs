
using System;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Security.Cryptography;

public partial class EmailOtpTest : Page
{
    // Replace these with your actual Brevo SMTP credentials.
    private const string SmtpLogin = "YOUR_BREVO_SMTP_LOGIN";
    private const string SmtpKey = "YOUR_BREVO_SMTP_KEY";
    private const string SenderEmail = "YOUR_VERIFIED_EMAIL";

    protected void Page_Load(object sender, EventArgs e)
    {
        bool hasOtp = Session["EmailOtpTest_Code"] != null;

        pnlVerify.Visible = hasOtp;

        if (hasOtp)
        {
            int remaining = GetRemainingSeconds();

            btnResendOtp.Enabled = remaining <= 0;

            RegisterCountdown(remaining, "OtpCountdownPageLoad");
        }
        else
        {
            btnResendOtp.Enabled = false;
        }
    }

    protected void btnSendOtp_Click(object sender, EventArgs e)
    {
        SendOtp();
    }

    protected void btnResendOtp_Click(object sender, EventArgs e)
    {
        SendOtp();
    }

    private void SendOtp()
    {
        string email = txtEmail.Text.Trim();

        if (GetRemainingSeconds() > 0)
        {
            int remaining = GetRemainingSeconds();

            pnlVerify.Visible = Session["EmailOtpTest_Code"] != null;

            ShowMessage(
                "Please wait " + remaining +
                " seconds before requesting another OTP.");

            RegisterCountdown(remaining, "OtpCountdownWait");
            return;
        }

        MailAddress recipient;

        try
        {
            recipient = new MailAddress(email);

            if (!String.Equals(
                recipient.Address,
                email,
                StringComparison.OrdinalIgnoreCase))
            {
                ShowMessage("Please enter a valid email address.");
                return;
            }
        }
        catch
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        try
        {
            int otpNumber;

            using (RandomNumberGenerator rng =
                RandomNumberGenerator.Create())
            {
                byte[] bytes = new byte[4];
                rng.GetBytes(bytes);

                uint randomValue = BitConverter.ToUInt32(bytes, 0);
                otpNumber = (int)(100000 + (randomValue % 900000));
            }

            string otp = otpNumber.ToString();

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(SenderEmail, "ZEDCART");
                mail.To.Add(recipient);
                mail.Subject = "ZEDCART Login OTP";
                mail.Body =
                    "Your ZEDCART OTP is: " + otp +
                    "\r\nIt expires in 5 minutes." +
                    "\r\nDo not share this OTP with anyone.";

                using (SmtpClient smtp =
                    new SmtpClient("smtp-relay.brevo.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials =
                        new NetworkCredential(SmtpLogin, SmtpKey);
                    smtp.Timeout = 30000;
                    smtp.Send(mail);
                }
            }

            // Store the new OTP only after the email was sent successfully.
            // This replaces the previous OTP.
            Session["EmailOtpTest_Code"] = otp;
            Session["EmailOtpTest_Expiry"] =
                DateTime.UtcNow.AddMinutes(5);
            Session["EmailOtpTest_Address"] = email;
            Session["EmailOtpTest_LastSent"] = DateTime.UtcNow;
            Session["EmailOtpTest_Attempts"] = 0;

            pnlVerify.Visible = true;
            txtOtp.Text = "";
            btnResendOtp.Enabled = false;

            ShowMessage("OTP sent successfully. Check your email.");

            RegisterCountdown(60, "OtpCountdownAfterSend");
        }
        catch (SmtpException ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Brevo SMTP error: " + ex);

            pnlVerify.Visible =
                Session["EmailOtpTest_Code"] != null;

            ShowMessage(
                "Email could not be sent. Check your Brevo SMTP settings.");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Email OTP error: " + ex);

            pnlVerify.Visible =
                Session["EmailOtpTest_Code"] != null;

            ShowMessage("An error occurred while sending the OTP.");
        }
    }

    protected void btnVerify_Click(object sender, EventArgs e)
    {
        string expected =
            Session["EmailOtpTest_Code"] as string;

        string savedEmail =
            Session["EmailOtpTest_Address"] as string;

        string entered = txtOtp.Text.Trim();
        object expiryObject = Session["EmailOtpTest_Expiry"];

        DateTime expiry;

        if (String.IsNullOrEmpty(expected) ||
            expiryObject == null ||
            !DateTime.TryParse(
                expiryObject.ToString(), out expiry) ||
            DateTime.UtcNow > expiry)
        {
            ClearOtp();
            pnlVerify.Visible = false;

            ShowMessage(
                "OTP expired. Please request a new OTP.");
            return;
        }

        if (String.IsNullOrEmpty(savedEmail) ||
            !String.Equals(
                savedEmail,
                txtEmail.Text.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            ClearOtp();
            pnlVerify.Visible = false;

            ShowMessage(
                "Email address changed. Please request a new OTP.");
            return;
        }

        int attempts = Convert.ToInt32(
            Session["EmailOtpTest_Attempts"] ?? 0);

        if (attempts >= 5)
        {
            ClearOtp();
            pnlVerify.Visible = false;

            ShowMessage(
                "Maximum verification attempts reached. Request a new OTP.");
            return;
        }

        Session["EmailOtpTest_Attempts"] = attempts + 1;

        if (!String.Equals(
            entered,
            expected,
            StringComparison.Ordinal))
        {
            pnlVerify.Visible = true;

            ShowMessage(
                "Incorrect OTP. Please try again.");
            return;
        }

        ClearOtp();
        pnlVerify.Visible = false;

        ShowMessage(
            "Email OTP verified successfully!");
    }

    private int GetRemainingSeconds()
    {
        object value = Session["EmailOtpTest_LastSent"];

        if (value == null)
            return 0;

        DateTime lastSent;

        if (value is DateTime)
        {
            lastSent = (DateTime)value;
        }
        else if (!DateTime.TryParse(
            value.ToString(), out lastSent))
        {
            return 0;
        }

        double remaining = 60 -
            (DateTime.UtcNow -
             lastSent.ToUniversalTime()).TotalSeconds;

        if (remaining <= 0)
            return 0;

        return (int)Math.Ceiling(remaining);
    }

    private void RegisterCountdown(int seconds, string key)
    {
        if (seconds < 0)
            seconds = 0;

        string script =
            "startOtpCountdown(" + seconds + ");";

        ClientScript.RegisterStartupScript(
            GetType(),
            key,
            script,
            true);
    }

    private void ClearOtp()
    {
        Session.Remove("EmailOtpTest_Code");
        Session.Remove("EmailOtpTest_Expiry");
        Session.Remove("EmailOtpTest_Address");
        Session.Remove("EmailOtpTest_Attempts");

        // Keep LastSent so the 60-second cooldown still applies.
    }

    private void ShowMessage(string message)
    {
        lblMessage.Text = Server.HtmlEncode(message);
    }
}
