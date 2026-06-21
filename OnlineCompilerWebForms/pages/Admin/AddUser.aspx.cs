using System;
using System.Data.SqlClient;
using System.Web.UI;
using OnlineCompilerWebForms.Repositories;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.pages.Admin
{
    public partial class AddUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAdmin(this); // Ensure only admins can access this page
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            // Validate passwords
            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowError("Passwords do not match.");
                return;
            }

            string username = txtUsername.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
            {
                ShowError("Username and Email are required.");
                return;
            }

            // Check if user already exists
            if (IsUserExists(username, email))
            {
                ShowError("Username or Email already exists. Please choose different ones.");
                return;
            }

            // Hash the password
            string passwordHash = PasswordHasher.HashPassword(txtPassword.Text);

            Guid newUserId = Guid.NewGuid();

            string insertQuery = @"
                INSERT INTO Users (Id, Username, Email, PasswordHash, Role, IsEmailConfirmed, IsLocked, FailedLoginAttempts, CreatedAt)
                VALUES (@Id, @Username, @Email, @PasswordHash, @Role, @IsEmailConfirmed, @IsLocked, 0, GETUTCDATE())";

            int rows = DatabaseHelper.ExecuteNonQuery(insertQuery,
                new SqlParameter("@Id", newUserId),
                new SqlParameter("@Username", username),
                new SqlParameter("@Email", email),
                new SqlParameter("@PasswordHash", passwordHash),
                new SqlParameter("@Role", ddlRole.SelectedValue),
                new SqlParameter("@IsEmailConfirmed", chkEmailConfirmed.Checked),
                new SqlParameter("@IsLocked", chkLocked.Checked));

            if (rows > 0)
            {
                // Success – redirect back to Dashboard with a success message
                string successMsg = $"User '{username}' created successfully.";
                Response.Redirect("Dashboard.aspx?msg=" + Server.UrlEncode(successMsg));
            }
            else
            {
                ShowError("Failed to create user. Please try again.");
            }
        }

        private bool IsUserExists(string username, string email)
        {
            string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username OR Email = @Email";
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query,
                new SqlParameter("@Username", username),
                new SqlParameter("@Email", email)));
            return count > 0;
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}