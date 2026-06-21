using System;
using System.Drawing;
using OnlineCompilerWebForms.Repositories;
using OnlineCompilerWebForms.Security;

namespace OnlineCompilerWebForms.pages
{
    public partial class AccountSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this);

            if (!IsPostBack)
            {
                LoadUserData();
            }
        }

        private void LoadUserData()
        {
            var userRepo = new UserRepository();
            var user = userRepo.GetByUsername(Session["Username"].ToString());
            
            if (user != null)
            {
                txtUsername.Text = user.Username;
                txtEmail.Text = user.Email;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text) || string.IsNullOrEmpty(txtNewPassword.Text))
            {
                lblMessage.Text = "Please fill in all password fields.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                lblMessage.Text = "New passwords do not match.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            var userRepo = new UserRepository();
            var user = userRepo.GetByUsername(Session["Username"].ToString());

            if (!PasswordHasher.VerifyPassword(txtCurrentPassword.Text, user.PasswordHash))
            {
                lblMessage.Text = "Current password is incorrect.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            user.PasswordHash = PasswordHasher.HashPassword(txtNewPassword.Text);
            userRepo.Update(user);

            lblMessage.Text = "Password updated successfully.";
            lblMessage.ForeColor = Color.LightGreen;
            
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
        }
    }
}
