using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Services;
using System.Configuration;

namespace OnlineCompilerWebForms.pages
{
    public partial class AiGenerator : Page
    {
        private readonly AiService _aiService = new AiService();
        private readonly ProjectService _projectService = new ProjectService();
        private readonly FileService _fileService = new FileService();

        private class AiProjectResponse
        {
            public List<AiProjectFile> files { get; set; }
        }

        private class AiProjectFile
        {
            public string name { get; set; }
            public string content { get; set; }
            public bool isEntryPoint { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this);
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            var projectName = txtProjectName.Text.Trim();
            var description = txtPrompt.Text.Trim();
            var lang = ddlLanguage.SelectedValue;

            if (string.IsNullOrEmpty(projectName) || string.IsNullOrEmpty(description))
            {
                litResult.Text = "<div class='alert alert-danger'><i class='fas fa-exclamation-triangle'></i> Project Name and Description are required.</div>";
                // Reset UI
                ScriptManager.RegisterStartupScript(this, GetType(), "HideLoader",
                    "document.getElementById('loaderUI').style.display = 'none'; document.getElementById('" + inputForm.ClientID + "').style.display = 'block';", true);
                return;
            }

            Page.RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                try
                {
                    var systemPrompt = "You are an expert software architect. The user wants to generate a new project. " +
                                       $"Language: {lang}. " +
                                       "You MUST respond ONLY with a valid JSON object matching this schema, wrapped in a ```json codeblock:\n" +
                                       "{\n" +
                                       "  \"files\": [\n" +
                                       "    { \"name\": \"filename.ext\", \"content\": \"full source code here\", \"isEntryPoint\": true }\n" +
                                       "  ]\n" +
                                       "}\n" +
                                       "Make sure to include all necessary files to satisfy the user's description. One file must be the entry point.";

                    var messages = new List<AiMessage>
                    {
                        new AiMessage { role = "system", content = systemPrompt },
                        new AiMessage { role = "user", content = description }
                    };

                    var aiResponseRaw = await _aiService.SendChatRequestAsync(messages);

                    // Extract JSON from markdown
                    var match = Regex.Match(aiResponseRaw, @"```json\s*(\{.*?\})\s*```", RegexOptions.Singleline);
                    string jsonPayload = match.Success ? match.Groups[1].Value : aiResponseRaw;

                    // Clean up if it didn't use code blocks
                    jsonPayload = jsonPayload.Trim();
                    if (jsonPayload.StartsWith("```json")) jsonPayload = jsonPayload.Substring(7);
                    if (jsonPayload.EndsWith("```")) jsonPayload = jsonPayload.Substring(0, jsonPayload.Length - 3);

                    var resultData = JsonSerializer.Deserialize<AiProjectResponse>(jsonPayload, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (resultData?.files == null || resultData.files.Count == 0)
                    {
                        throw new Exception("AI did not return any valid files.");
                    }

                    // Create project
                    var userId = (Guid)Session["UserId"];
                    var newProject = _projectService.CreateProject(userId, projectName, description, lang);

                    var fileStoragePath = ConfigurationManager.AppSettings["FilePath"] ?? Server.MapPath("~/App_Data/CompilerFiles");
                    var userProjectPath = Path.Combine(fileStoragePath, userId.ToString(), newProject.Id.ToString());

                    // Create files
                    foreach (var f in resultData.files)
                    {
                        var createdFile = _fileService.AddFile(newProject.Id, f.name, f.content, userProjectPath);
                        if (f.isEntryPoint)
                        {
                            _fileService.SetEntryPoint(newProject.Id, createdFile.Id);
                        }
                    }

                    // Redirect to editor
                    Response.Redirect($"Editor.aspx?id={newProject.Id}", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    litResult.Text = $"<div class='alert alert-danger'><i class='fas fa-times-circle'></i> Error generating project: {HttpUtility.HtmlEncode(ex.Message)}</div>";
                    ScriptManager.RegisterStartupScript(this, GetType(), "HideLoader",
                        "document.getElementById('loaderUI').style.display = 'none'; document.getElementById('" + inputForm.ClientID + "').style.display = 'block';", true);
                }
            }));
            Page.ExecuteRegisteredAsyncTasks(); // Required to run the async task
        }
    }
}