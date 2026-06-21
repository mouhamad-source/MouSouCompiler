using System;
using System.Drawing;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnSubmit_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();
            if (string.IsNullOrEmpty(email)) return;

            var authService = new AuthService();
            var result = await authService.ForgotPasswordAsync(email);

            if (result.Success)
            {
                lblMessage.Text = result.Message;
                lblMessage.ForeColor = Color.LightGreen;
                txtEmail.Text = "";
            }
            else
            {
                lblMessage.Text = result.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
    }
}
