using System;
using System.Drawing;
using System.Web.Security;
using OnlineCompilerWebForms.Model.DTOs;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            var dto = new LoginDTO
            {
                UsernameOrEmail = txtUsernameOrEmail.Text.Trim(),
                Password = txtPassword.Text,
                RememberMe = chkRememberMe.Checked
            };

            var authService = new AuthService();
            var result = authService.Login(dto);

            if (result.Success)
            {
                Session["UserId"] = result.User.Id;
                Session["Role"] = result.User.Role;
                Session["Username"] = result.User.Username;

                FormsAuthentication.SetAuthCookie(result.User.Username, dto.RememberMe);

                Response.Redirect("~/settings");
            }
            else
            {
                lblMessage.Text = result.Message;
                lblMessage.ForeColor = Color.Red;
            }
        }
    }
}
