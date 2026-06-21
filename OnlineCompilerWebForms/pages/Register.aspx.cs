using System;
using System.Drawing;
using OnlineCompilerWebForms.Model.DTOs;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected async void btnRegister_Click(object sender, EventArgs e)
        {
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text = "Passwords do not match.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            var dto = new RegisterDTO
            {
                Username = txtUsername.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text
            };

            var authService = new AuthService();
            var result = await authService.RegisterAsync(dto);

            if (result.Success)
            {
                lblMessage.Text = result.Message;
                lblMessage.ForeColor = Color.LightGreen;
                txtUsername.Text = "";
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
