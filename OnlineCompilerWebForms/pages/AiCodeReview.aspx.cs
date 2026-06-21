using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using OnlineCompilerWebForms.Repositories;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.pages
{
    public partial class AiCodeReview : Page
    {
        private readonly ProjectRepository _projectRepository = new ProjectRepository();
        private readonly FileService _fileService = new FileService();
        private readonly AiService _aiService = new AiService();

        private class AiReviewResponse
        {
            public int securityScore { get; set; }
            public int performanceScore { get; set; }
            public int readabilityScore { get; set; }
            public string feedback { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this);

            if (!IsPostBack)
            {
                var userId = (Guid)Session["UserId"];
                var projects = _projectRepository.GetByUserId(userId);

                if (projects != null && projects.Any())
                {
                    ddlProjects.DataSource = projects;
                    ddlProjects.DataBind();
                }
                else
                {
                    btnAnalyze.Enabled = false;
                    btnAnalyze.Text = "No Projects Available";
                }
            }
        }

        protected void btnAnalyze_Click(object sender, EventArgs e)
        {
            var projectIdStr = ddlProjects.SelectedValue;
            if (!Guid.TryParse(projectIdStr, out Guid projectId))
            {
                return;
            }

            Page.RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                try
                {
                    var userId = (Guid)Session["UserId"];
                    var fileStoragePath = ConfigurationManager.AppSettings["FilePath"] ?? Server.MapPath("~/App_Data/CompilerFiles");
                    var userProjectPath = Path.Combine(fileStoragePath, userId.ToString(), projectId.ToString());

                    var files = _fileService.GetProjectFiles(projectId, userProjectPath);

                    if (files == null || !files.Any())
                    {
                        throw new Exception("No files found in the project to analyze.");
                    }

                    var contextBuilder = new System.Text.StringBuilder();
                    contextBuilder.AppendLine("You are a strict, expert AI Code Reviewer. Analyze the following project files and evaluate them.");
                    contextBuilder.AppendLine("You MUST reply exclusively in JSON wrapped in a ```json code block.");
                    contextBuilder.AppendLine("JSON Schema:");
                    contextBuilder.AppendLine("{");
                    contextBuilder.AppendLine("  \"securityScore\": (int 0-100),");
                    contextBuilder.AppendLine("  \"performanceScore\": (int 0-100),");
                    contextBuilder.AppendLine("  \"readabilityScore\": (int 0-100),");
                    contextBuilder.AppendLine("  \"feedback\": \"Detailed markdown string explaining your findings and how to improve.\"");
                    contextBuilder.AppendLine("}");

                    foreach (var f in files)
                    {
                        contextBuilder.AppendLine($"\n--- File: {f.FileName} ---");
                        contextBuilder.AppendLine(f.Content);
                        contextBuilder.AppendLine("--- End of File ---");
                    }

                    var messages = new List<AiMessage>
                    {
                        new AiMessage { role = "system", content = contextBuilder.ToString() },
                        new AiMessage { role = "user", content = "Please generate the Health Report for this project." }
                    };

                    var aiResponseRaw = await _aiService.SendChatRequestAsync(messages);

                    var match = Regex.Match(aiResponseRaw, @"```json\s*(\{.*?\})\s*```", RegexOptions.Singleline);
                    string jsonPayload = match.Success ? match.Groups[1].Value : aiResponseRaw;

                    jsonPayload = jsonPayload.Trim();
                    if (jsonPayload.StartsWith("```json")) jsonPayload = jsonPayload.Substring(7);
                    if (jsonPayload.EndsWith("```")) jsonPayload = jsonPayload.Substring(0, jsonPayload.Length - 3);

                    var report = JsonSerializer.Deserialize<AiReviewResponse>(jsonPayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (report != null)
                    {
                        circleSecurity.InnerText = report.securityScore.ToString();
                        circlePerformance.InnerText = report.performanceScore.ToString();
                        circleReadability.InnerText = report.readabilityScore.ToString();

                        circleSecurity.Style["border-color"] = GetScoreColor(report.securityScore);
                        circlePerformance.Style["border-color"] = GetScoreColor(report.performanceScore);
                        circleReadability.Style["border-color"] = GetScoreColor(report.readabilityScore);

                        litFeedback.Text = HttpUtility.HtmlEncode(report.feedback);
                        pnlResults.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    litFeedback.Text = $"Error generating review: {ex.Message}";
                    pnlResults.Visible = true;
                }
                finally
                {
                    // Re-show the selection UI and hide loader
                    selectionUI.Style["display"] = "block";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideLoader", "document.getElementById('loaderUI').style.display = 'none';", true);
                }
            }));

            // Execute the async task
            Page.ExecuteRegisteredAsyncTasks();
        }

        private string GetScoreColor(int score)
        {
            if (score >= 80) return "#00ff7f"; // Green
            if (score >= 60) return "#f5af19"; // Yellow/Orange
            return "#f14c4c"; // Red
        }
    }
}