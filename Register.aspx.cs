
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

public partial class Register : System.Web.UI.Page
{
    private string ConnectionString
    {
        get
        {
            return ConfigurationManager
                .ConnectionStrings["CheepCartConnection"]
                .ConnectionString;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pnlOtp.Visible = false;
            pnlRegistration.Visible = false;
            btnSendOtp.Visible = true;
            btnResendOtp.Enabled = false;
        }

        if (Session["RegisterEmailVerified"] != null &&
            Session["RegisterEmailVerified"].ToString() == "true")
        {
            txtFullName.Enabled = false;
            txtEmail.Enabled = false;
        }

        // Continue the countdown after a Web Forms postback.
        if (pnlOtp.Visible)
        {
            RegisterCountdownScript(
                GetRemainingCooldownSeconds(),
                "OtpCountdownOnLoad");
        }
    }

    protected void btnSendOtp_Click(object sender, EventArgs e)
    {
        SendOtp();
    }

    protected void btnResendOtp_Click(object sender, EventArgs e)
    {
        // The server checks the 60-second limit too.
        SendOtp();
    }

    private void SendOtp()
    {
        string name = txtFullName.Text.Trim();
        string email = txtEmail.Text.Trim();

        if (name.Length == 0 || name.Length > 100)
        {
            ShowMessage("Please enter a valid name.");
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        if (Session["RegisterEmailVerified"] != null &&
            Session["RegisterEmailVerified"].ToString() == "true")
        {
            ShowMessage("Email is already verified.");
            return;
        }

        // Server-side cooldown: one successful/attempted send per 60 seconds.
        int remainingSeconds = GetRemainingCooldownSeconds();

        if (remainingSeconds > 0)
        {
            ShowMessage(
                "Please wait " + remainingSeconds +
                " seconds before requesting another OTP.");

            if (pnlOtp.Visible)
            {
                RegisterCountdownScript(
                    remainingSeconds,
                    "OtpCountdownCooldown");
            }

            return;
        }

        try
        {
            // Check whether the email already exists.
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM Customers WHERE Email = @Email",
                    con))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
                    {
                        ShowMessage(
                            "An account with this email already exists.");
                        return;
                    }
                }
            }

            string senderEmail =
                ConfigurationManager.AppSettings["EmailAddress"];

            string smtpLogin =
                ConfigurationManager.AppSettings["BrevoSmtpLogin"];

            string smtpKey =
                ConfigurationManager.AppSettings["BrevoSmtpKey"];

            string smtpHost =
                ConfigurationManager.AppSettings["BrevoSmtpHost"];

            int smtpPort;

            if (!int.TryParse(
                ConfigurationManager.AppSettings["BrevoSmtpPort"],
                out smtpPort))
            {
                ShowMessage(
                    "Please check the Brevo SMTP port in Web.config.");
                return;
            }

            if (String.IsNullOrWhiteSpace(senderEmail) ||
                String.IsNullOrWhiteSpace(smtpLogin) ||
                String.IsNullOrWhiteSpace(smtpKey) ||
                String.IsNullOrWhiteSpace(smtpHost))
            {
                ShowMessage(
                    "Please check the Brevo SMTP settings in Web.config.");
                return;
            }

            string otp = GenerateOtp();

            // Start cooldown before attempting SMTP, to prevent repeated
            // rapid requests if the mail server is unavailable.
            Session["RegisterOtpLastSent"] = DateTime.UtcNow;

            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12;

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(senderEmail);
                mail.To.Add(email);
                mail.Subject = "ZEDCART Registration OTP";

                mail.Body =
                    "Hello " + name + ",\r\n\r\n" +
                    "Your ZEDCART verification OTP is: " + otp +
                    "\r\nThis OTP expires in 5 minutes." +
                    "\r\nDo not share this code.";

                using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials =
                        new NetworkCredential(smtpLogin, smtpKey);
                    smtp.Timeout = 30000;

                    smtp.Send(mail);
                }
            }

            // Store the new OTP only after the email send succeeds.
            // A newly sent OTP replaces the previous OTP.
            Session["RegisterOtpCode"] = otp;
            Session["RegisterOtpEmail"] = email;
            Session["RegisterOtpName"] = name;
            Session["RegisterOtpExpiry"] = DateTime.UtcNow.AddMinutes(5);
            Session["RegisterOtpAttempts"] = 0;

            Session.Remove("RegisterEmailVerified");
            Session.Remove("RegisterVerifiedEmail");

            pnlOtp.Visible = true;
            pnlRegistration.Visible = false;

            txtFullName.Enabled = false;
            txtEmail.Enabled = false;
            txtOtp.Text = "";

            btnSendOtp.Visible = false;
            btnResendOtp.Enabled = false;

            ShowMessage(
                "OTP sent successfully. Please check your Inbox or Spam folder.");

            RegisterCountdownScript(60, "OtpCountdownAfterSend");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Register OTP email error: " + ex.ToString());

            // Keep the cooldown even if the mail attempt fails.
            // If this was the first attempt, show the resend controls.
            pnlOtp.Visible = true;
            pnlRegistration.Visible = false;

            txtFullName.Enabled = false;
            txtEmail.Enabled = false;

            btnSendOtp.Visible = false;
            btnResendOtp.Enabled = false;

            ShowMessage(
                "Unable to send email. Please wait for the countdown, " +
                "then try Resend OTP. Check your email configuration if " +
                "the problem continues.");

            RegisterCountdownScript(
                GetRemainingCooldownSeconds(),
                "OtpCountdownAfterFailure");
        }
    }

    protected void btnVerifyOtp_Click(object sender, EventArgs e)
    {
        string savedOtp = Session["RegisterOtpCode"] as string;
        string savedEmail = Session["RegisterOtpEmail"] as string;
        string email = txtEmail.Text.Trim();
        string enteredOtp = txtOtp.Text.Trim();

        if (savedOtp == null || savedEmail == null ||
            !String.Equals(
                savedEmail,
                email,
                StringComparison.OrdinalIgnoreCase))
        {
            ShowMessage("Please request a new OTP.");
            return;
        }

        if (Session["RegisterOtpExpiry"] == null ||
            DateTime.UtcNow > (DateTime)Session["RegisterOtpExpiry"])
        {
            ClearOtpData();

            pnlOtp.Visible = false;
            pnlRegistration.Visible = false;

            txtFullName.Enabled = true;
            txtEmail.Enabled = true;
            btnSendOtp.Visible = true;

            ShowMessage(
                "OTP has expired. Please request a new OTP.");
            return;
        }

        int attempts = Convert.ToInt32(
            Session["RegisterOtpAttempts"] ?? 0);

        if (attempts >= 5)
        {
            ClearOtpData();

            pnlOtp.Visible = false;
            pnlRegistration.Visible = false;

            txtFullName.Enabled = true;
            txtEmail.Enabled = true;
            btnSendOtp.Visible = true;

            ShowMessage(
                "Maximum attempts reached. Please request a new OTP.");
            return;
        }

        Session["RegisterOtpAttempts"] = attempts + 1;

        if (enteredOtp.Length != 6 ||
            !String.Equals(
                enteredOtp,
                savedOtp,
                StringComparison.Ordinal))
        {
            ShowMessage("Invalid OTP.");
            return;
        }

        Session["RegisterEmailVerified"] = "true";
        Session["RegisterVerifiedEmail"] = savedEmail;

        Session.Remove("RegisterOtpCode");
        Session.Remove("RegisterOtpExpiry");
        Session.Remove("RegisterOtpAttempts");

        pnlOtp.Visible = false;
        pnlRegistration.Visible = true;

        txtFullName.Enabled = false;
        txtEmail.Enabled = false;
        btnSendOtp.Visible = false;

        ShowMessage("Email verification successful.");
    }

    protected void btnRegister_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string name = txtFullName.Text.Trim();
        string phone = txtPhone.Text.Trim();
        string password = txtPassword.Text;
        string confirmPassword = txtConfirmPassword.Text;

        if (Session["RegisterEmailVerified"] == null ||
            Session["RegisterEmailVerified"].ToString() != "true" ||
            Session["RegisterVerifiedEmail"] == null ||
            !String.Equals(
                Session["RegisterVerifiedEmail"].ToString(),
                email,
                StringComparison.OrdinalIgnoreCase))
        {
            ShowMessage(
                "Please verify your email using OTP first.");
            return;
        }

        if (name.Length == 0 || name.Length > 100)
        {
            ShowMessage("Please enter a valid name.");
            return;
        }

        if (!IsValidEmail(email))
        {
            ShowMessage("Please enter a valid email address.");
            return;
        }

        if (!Regex.IsMatch(phone, @"^[0-9]{10}$"))
        {
            ShowMessage(
                "Please enter a valid 10-digit phone number.");
            return;
        }

        if (password.Length < 8 || password.Length > 100)
        {
            ShowMessage(
                "Password must contain between 8 and 100 characters.");
            return;
        }

        if (password != confirmPassword)
        {
            ShowMessage("Passwords do not match.");
            return;
        }

        try
        {
            int customerId;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                using (SqlCommand check = new SqlCommand(
                    "SELECT COUNT(*) FROM Customers " +
                    "WHERE Email = @Email OR Phone = @Phone",
                    con))
                {
                    check.Parameters.AddWithValue("@Email", email);
                    check.Parameters.AddWithValue("@Phone", phone);

                    if (Convert.ToInt32(check.ExecuteScalar()) > 0)
                    {
                        ShowMessage(
                            "Email or phone number is already registered.");
                        return;
                    }
                }

                using (SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Customers " +
                    "(FullName, Email, Phone, [Password]) " +
                    "OUTPUT INSERTED.CustomerID " +
                    "VALUES (@Name, @Email, @Phone, @Password)",
                    con))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Password", password);

                    customerId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }

            // Automatically sign in the new customer.
            Session["CustomerID"] = customerId;
            Session["CustomerName"] = name;

            Session.Remove("RegisterEmailVerified");
            Session.Remove("RegisterVerifiedEmail");

            ClearRegisterOtp();

            Response.Redirect("Default.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Trace.TraceError(
                "Customer registration error: " + ex.ToString());

            ShowMessage(
                "Registration failed. Please check the database table and columns.");
        }
    }

    private int GetRemainingCooldownSeconds()
    {
        object value = Session["RegisterOtpLastSent"];

        if (value == null)
        {
            return 0;
        }

        DateTime lastSent;

        if (value is DateTime)
        {
            lastSent = (DateTime)value;
        }
        else if (!DateTime.TryParse(value.ToString(), out lastSent))
        {
            return 0;
        }

        double remaining =
            60.0 - (DateTime.UtcNow - lastSent.ToUniversalTime()).TotalSeconds;

        if (remaining <= 0)
        {
            return 0;
        }

        return (int)Math.Ceiling(remaining);
    }

    private void RegisterCountdownScript(int seconds, string key)
    {
        string script =
            "startOtpCountdown(" + seconds.ToString() + ");";

        ClientScript.RegisterStartupScript(
            GetType(),
            key,
            script,
            true);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            MailAddress address = new MailAddress(email);

            return String.Equals(
                address.Address,
                email,
                StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    private string GenerateOtp()
    {
        byte[] bytes = new byte[4];

        using (RandomNumberGenerator rng =
            RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }

        uint number = BitConverter.ToUInt32(bytes, 0);

        return (100000 + (number % 900000)).ToString();
    }

    // Clears the OTP but keeps the resend cooldown.
    private void ClearOtpData()
    {
        Session.Remove("RegisterOtpCode");
        Session.Remove("RegisterOtpEmail");
        Session.Remove("RegisterOtpName");
        Session.Remove("RegisterOtpExpiry");
        Session.Remove("RegisterOtpAttempts");
    }

    // Clears all registration OTP state after registration completes.
    private void ClearRegisterOtp()
    {
        ClearOtpData();
        Session.Remove("RegisterOtpLastSent");
    }

    private void ShowMessage(string message)
    {
        lblMessage.Text = Server.HtmlEncode(message);
    }
}
