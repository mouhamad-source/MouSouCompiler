using System;
using System.Drawing;
using System.Web.UI.WebControls;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class Projects : System.Web.UI.Page
    {
        private ProjectService _projectService = new ProjectService();

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this);

            if (!IsPostBack)
            {
                BindProjects();
            }
        }

        private void BindProjects()
        {
            Guid userId = (Guid)Session["UserId"];
            var projects = _projectService.GetUserProjects(userId);
            rptProjects.DataSource = projects;
            rptProjects.DataBind();
        }

        

        protected void rptProjects_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteProject")
            {
                Guid projectId = new Guid(e.CommandArgument.ToString());
                Guid userId = (Guid)Session["UserId"];
                
                _projectService.DeleteProject(projectId, userId);
                
                lblMessage.Text = "Project deleted successfully.";
                lblMessage.ForeColor = Color.LightGreen;
                
                BindProjects();
            }
        }
    }
}
