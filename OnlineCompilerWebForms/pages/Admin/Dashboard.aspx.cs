using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.WebControls;
using OnlineCompilerWebForms.Repositories;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Utils;

namespace OnlineCompilerWebForms.pages.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAdmin(this);

            // Show message from AddUser page if present
            if (!IsPostBack)
            {
                if (Request.QueryString["msg"] != null)
                {
                    string msg = Request.QueryString["msg"];
                    lblMessage.Text = msg;
                    lblMessage.ForeColor = msg.Contains("successfully") ? Color.LightGreen : Color.OrangeRed;
                }
                BindUsers();
            }
        }

        private void BindUsers()
        {
            string query = "SELECT Id, Username, Email, Role, IsEmailConfirmed, IsLocked, FailedLoginAttempts FROM Users ORDER BY CreatedAt DESC";
            DataTable dt = DatabaseHelper.ExecuteQuery(query);
            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }

        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Guid userId = new Guid(e.CommandArgument.ToString());

            switch (e.CommandName)
            {
                case "UnlockUser":
                    UpdateLockState(userId, false);
                    lblMessage.Text = "User unlocked successfully.";
                    lblMessage.ForeColor = Color.LightGreen;
                    break;
                case "LockUser":
                    UpdateLockState(userId, true);
                    lblMessage.Text = "User locked successfully.";
                    lblMessage.ForeColor = Color.LightGreen;
                    break;
                case "DeleteUser":
                    DeleteUser(userId);
                    lblMessage.Text = "User deleted successfully.";
                    lblMessage.ForeColor = Color.LightGreen;
                    break;
            }
            BindUsers();
        }

        private void UpdateLockState(Guid userId, bool isLocked)
        {
            string query = "UPDATE Users SET IsLocked = @LockState, FailedLoginAttempts = 0 WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query,
                new SqlParameter("@LockState", isLocked),
                new SqlParameter("@Id", userId));
        }

        private void DeleteUser(Guid userId)
        {
            if (userId == GetCurrentUserId())
            {
                lblMessage.Text = "You cannot delete your own admin account.";
                lblMessage.ForeColor = Color.Red;
                return;
            }

            string query = "DELETE FROM Users WHERE Id = @Id";
            DatabaseHelper.ExecuteNonQuery(query, new SqlParameter("@Id", userId));
        }

        private Guid GetCurrentUserId()
        {
            return (Guid)Session["UserId"];  // Ensure your session stores the GUID after login
        }

        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");
                if (btnDelete != null)
                {
                    Guid userId = (Guid)gvUsers.DataKeys[e.Row.RowIndex].Value;
                    if (userId == GetCurrentUserId())
                    {
                        btnDelete.Enabled = false;
                        btnDelete.ToolTip = "You cannot delete your own account.";
                    }
                }
            }
        }
    }
}