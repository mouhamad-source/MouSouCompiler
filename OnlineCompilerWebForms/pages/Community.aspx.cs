using System;
using System.Linq;
using System.Web.UI;
using OnlineCompilerWebForms.Repositories;

namespace OnlineCompilerWebForms.pages
{
    public partial class Community : Page
    {
        private readonly ProjectRepository _projectRepository = new ProjectRepository();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCommunityProjects();
            }
        }

        private void LoadCommunityProjects()
        {
            try
            {
                // Fetch all projects (non-deleted) from all users
                var allProjects = _projectRepository.GetAllProjects();

                if (allProjects != null && allProjects.Count > 0)
                {
                    rptProjects.DataSource = allProjects;
                    rptProjects.DataBind();
                    litNoProjects.Visible = false;
                }
                else
                {
                    litNoProjects.Visible = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error loading community projects: " + ex.Message);
                litNoProjects.Visible = true;
            }
        }
    }
}