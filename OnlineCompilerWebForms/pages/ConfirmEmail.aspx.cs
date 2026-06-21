using System;
using System.Drawing;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class ConfirmEmail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                if (string.IsNullOrEmpty(token))
                {
                    lblMessage.Text = "Invalid or missing token.";
                    lblMessage.ForeColor = Color.Red;
                    return;
                }

                var authService = new AuthService();
                bool success = authService.ConfirmEmail(token);

                if (success)
                {
                    lblMessage.Text = "Your email has been successfully confirmed. You can now login.";
                    lblMessage.ForeColor = Color.LightGreen;
                }
                else
                {
                    lblMessage.Text = "Failed to confirm email. The token may be invalid or expired.";
                    lblMessage.ForeColor = Color.Red;
                }
            }
        }
    }
}
