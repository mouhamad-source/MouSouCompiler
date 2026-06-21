using System;
using System.Drawing;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class ResetPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Request.QueryString["token"]))
                {
                    lblMessage.Text = "Invalid or missing reset token.";
                    lblMessage.ForeColor = Color.Red;
                    pnlReset.Visible = false;
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text = "Passwords do not match.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            string token = Request.QueryString["token"];
            var authService = new AuthService();
            var result = authService.ResetPassword(token, txtNewPassword.Text);

            if (result.Success)
            {
                lblMessage.Text = result.Message + " <a href='/pages/Login.aspx'>Go to Login</a>";
                lblMessage.ForeColor = Color.LightGreen;
                pnlReset.Visible = false;
            }
            else
            {
                lblMessage.Text = result.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
    }
}
