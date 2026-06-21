using System;
using System.Drawing;
using System.Web.UI;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class CreateProject : System.Web.UI.Page
    {
        private ProjectService _projectService = new ProjectService();

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this); // Ensure logged in
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtProjectName.Text))
            {
                ShowError("Project name is required.");
                return;
            }

            Guid userId = (Guid)Session["UserId"];

            try
            {
                var project = _projectService.CreateProject(
                    userId,
                    txtProjectName.Text.Trim(),
                    txtDescription.Text.Trim(),
                    ddlLanguage.SelectedValue
                );

               
                Response.Redirect($"/pages/Editor.aspx?id={project.Id}" , false );
            }
            catch (Exception ex)
            {
                ShowError("Error creating project: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }
    }
}