using OnlineCompilerWebForms.Model.DTOs;
using OnlineCompilerWebForms.Security;
using OnlineCompilerWebForms.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineCompilerWebForms.pages
{
    public partial class Editor : System.Web.UI.Page
    {
        private readonly ProjectService _projectService = new ProjectService();
        private readonly FileService _fileService = new FileService();

        private Guid CurrentUserId => (Guid)Session["UserId"];
        private Guid ActiveProjectId => new Guid(hfProjectId.Value);

        // Base path for file storage from web.config
        private string FileStoragePath => ConfigurationManager.AppSettings["FilePath"] ??
            Server.MapPath("~/App_Data/CompilerFiles");

        protected void Page_Load(object sender, EventArgs e)
        {
            AuthGuard.RequireAuthentication(this);
            if (!IsPostBack) InitializePage();
        }

        private void InitializePage()
        {
            if (!Guid.TryParse(Request.QueryString["id"], out Guid projectId))
            {
                Response.Redirect("/pages/Projects.aspx", false);
                return;
            }

            var project = _projectService.GetProject(projectId, CurrentUserId);
            if (project == null)
            {
                Response.Redirect("Projects.aspx", false);
                return;
            }

            hfProjectId.Value = project.Id.ToString();
            hfProjectLanguage.Value = project.Language;
            litProjectName.Text = Server.HtmlEncode(project.Name);
            LoadProjectFiles();
        }

        private void LoadProjectFiles()
        {
            var files = _fileService.GetProjectFiles(ActiveProjectId, GetUserProjectPath());
            rptFiles.DataSource = files;
            rptFiles.DataBind();

            // If no active file set, pick the entry point or first file
            if (string.IsNullOrEmpty(hfActiveFileId.Value) && files != null && files.Count > 0)
            {
                var entry = files.FirstOrDefault(f => f.IsEntryPoint) ?? files.First();
                SetActiveFile(entry.Id, entry.FileName, entry.Content);
            }
        }
        private string GetUserProjectPath()
        {
            return Path.Combine(FileStoragePath, CurrentUserId.ToString(), ActiveProjectId.ToString());
        }

        // FontAwesome icons (matching front-end)
        protected string GetFileIcon(string filename)
        {
            if (filename.EndsWith(".cs")) return "fas fa-file-code";
            if (filename.EndsWith(".py")) return "fab fa-python";
            if (filename.EndsWith(".json")) return "fas fa-file-alt";
            if (filename.EndsWith(".txt")) return "fas fa-file-alt";
            return "fas fa-file";
        }

        private void SaveCurrentFileContent()
        {
            if (!string.IsNullOrEmpty(hfActiveFileId.Value))
            {
                _fileService.SaveFileContent(
                    new Guid(hfActiveFileId.Value),
                    hfActiveFileContent.Value,
                    GetUserProjectPath());
            }
        }


        private void SetActiveFile(Guid fileId, string fileName, string content)
        {
            hfActiveFileId.Value = fileId.ToString();
            // Header update is now done via JavaScript, so we only need to pass data to the client
            hfFileToLoad.Value = content;

            string safeContent = HttpUtility.JavaScriptStringEncode(content);
            string safeFileName = HttpUtility.JavaScriptStringEncode(fileName);
            ScriptManager.RegisterStartupScript(this, GetType(), "UpdateMonaco",
                $"updateMonacoContent('{safeContent}', '{safeFileName}');", true);

            // Update the sidebar panel to show active highlight
            upSidebar.Update();
        }

        protected void rptFiles_ItemDataBound(object sender, RepeaterItemEventArgs e) { }

        protected void rptFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SaveCurrentFileContent();
            if (!Guid.TryParse(e.CommandArgument?.ToString(), out Guid fileId)) return;

            switch (e.CommandName)
            {
                case "OpenFile":
                    var files = _fileService.GetProjectFiles(ActiveProjectId, GetUserProjectPath());
                    var target = files?.FirstOrDefault(f => f.Id == fileId);
                    if (target != null)
                    {
                        SetActiveFile(target.Id, target.FileName, target.Content);
                        LoadProjectFiles();
                        upSidebar.Update();
                    }
                    break;
                case "SetEntryPoint":
                    _fileService.SetEntryPoint(ActiveProjectId, fileId);
                    LoadProjectFiles();
                    break;
                case "DeleteFile":
                    _fileService.DeleteFile(fileId, GetUserProjectPath());
                    if (hfActiveFileId.Value == fileId.ToString())
                    {
                        hfActiveFileId.Value = "";
                        hfFileToLoad.Value = "";
                        ScriptManager.RegisterStartupScript(this, GetType(), "ClearMonaco",
                            "if(editor) editor.setValue(''); document.getElementById('activeFileNameDisplay').textContent='Welcome';", true);
                    }
                    LoadProjectFiles();
                    break;
            }
        }

        protected void btnAddFile_Click(object sender, EventArgs e)
        {
            SaveCurrentFileContent();
            string fileName = txtNewFileName.Text.Trim();
            if (string.IsNullOrEmpty(fileName)) return;
            if (!IsValidFileName(fileName))
            {
                ShowConsoleError("Invalid file name. Use letters, digits, underscore, hyphen, and single dot.");
                return;
            }
            var newFile = _fileService.AddFile(ActiveProjectId, fileName, GetDefaultContent(fileName), GetUserProjectPath());
            txtNewFileName.Text = "";
            LoadProjectFiles();
            SetActiveFile(newFile.Id, newFile.FileName, newFile.Content);
        }
        protected void btnRenameFileHidden_Click(object sender, EventArgs e)
        {
            SaveCurrentFileContent();
            if (!Guid.TryParse(hfRenameFileId.Value, out Guid fileId)) return;
            string newName = hfNewFileName.Value?.Trim();
            if (string.IsNullOrEmpty(newName)) return;
            if (!IsValidFileName(newName))
            {
                ShowConsoleError("Invalid file name.");
                return;
            }
            _fileService.RenameFile(fileId, newName, GetUserProjectPath());
            if (hfActiveFileId.Value == fileId.ToString())
            {
                // Update header via JS
                ScriptManager.RegisterStartupScript(this, GetType(), "UpdateHeader",
                    $"document.getElementById('activeFileNameDisplay').textContent='{HttpUtility.JavaScriptStringEncode(newName)}';", true);
            }
            hfRenameFileId.Value = hfNewFileName.Value = "";
            LoadProjectFiles();
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            SaveCurrentFileContent();
            // Switch to terminal tab client-side
            ScriptManager.RegisterStartupScript(this, GetType(), "SwitchTab", "switchBottomTab('terminal', null);", true);
            litTerminalOutput.Text = "<span style='color:#00d2ff;'><i class='fas fa-hourglass-half'></i> Executing...</span>";
            litMetadataOutput.Text = "<span style='color:#00d2ff;'>Fetching metadata...</span>";
            upConsole.Update();
            upMetadata.Update();

            Page.RegisterAsyncTask(new PageAsyncTask(async () =>
            {
                var response = await ExecuteCodeAsync();
                if (response != null)
                {
                    DisplayTerminalOutput(response);
                    DisplayMetadata(response.Metadata);
                }
                else
                {
                    litTerminalOutput.Text = "<span style='color:#f14c4c;'>Failed to parse microservice response.</span>";
                    litMetadataOutput.Text = "";
                }
                upConsole.Update();
                upMetadata.Update();
            }));
        }
        private async Task<MicroserviceResponse> ExecuteCodeAsync()
        {
            var files = _fileService.GetProjectFiles(ActiveProjectId, GetUserProjectPath());
            if (files == null || files.Count == 0)
                return new MicroserviceResponse { Output = "",
                    Errors = new List<ErrorInfo>
    {
        new ErrorInfo { Message = $"not found files" }
    }, ExitCode = -1 };

            var entry = files.FirstOrDefault(f => f.IsEntryPoint) ?? files.First();
            var argsList = txtArgs.Text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(a => a.Trim())
                                       .ToList();

            var request = new
            {
                language = hfProjectLanguage.Value,
                files = files.Select(f => new { name = f.FileName, code = f.Content }),
                stdin = txtStdin.Text,
                timeoutLimit = 50000,
                args = argsList
            };

            string jsonRequest = JsonSerializer.Serialize(request);
            string microserviceUrl = ConfigurationManager.AppSettings["CompilerMicroserviceUrl"];
            if (string.IsNullOrEmpty(microserviceUrl))
            {
                return new MicroserviceResponse
                {
                    Errors = new List<ErrorInfo>
    {
        new ErrorInfo { Message = $"Microservice URL missing." }
    },
                    ExitCode = -1
                };
            }
                //return new MicroserviceResponse { Errors = new List<string> { "Microservice URL missing." }, ExitCode = -1 };

            using (var client = new System.Net.Http.HttpClient())
            {
                var content = new System.Net.Http.StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(microserviceUrl, content);
                if (httpResponse.IsSuccessStatusCode)
                {
                    string jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<MicroserviceResponse>(jsonResponse);
                }
                else
                {
                    //return new MicroserviceResponse { Errors = new List<string> { $"HTTP {httpResponse.StatusCode}" }, ExitCode = -1 };
                    return new MicroserviceResponse
                    {
                        Errors = new List<ErrorInfo>
    {
        new ErrorInfo { Message = $"HTTP {httpResponse.StatusCode}" }
    },
                        ExitCode = -1
                    };
                }
            }
        }
        private void DisplayTerminalOutput(MicroserviceResponse resp)
        {
            var html = new StringBuilder();
            if (resp.ExitCode == 0 && (resp.Errors == null || resp.Errors.Count == 0))
                html.Append("<span style='color:#00ff7f;font-weight:bold;'><i class='fas fa-check-circle'></i> SUCCESS (Exit 0)</span><br/>");
            else
                html.Append($"<span style='color:#f14c4c;font-weight:bold;'><i class='fas fa-times-circle'></i> FAILED (Exit {resp.ExitCode})</span><br/>");

            if (!string.IsNullOrEmpty(resp.Output))
                html.Append($"<pre style='color:#ccc;margin-top:8px;'>{HttpUtility.HtmlEncode(resp.Output)}</pre>");

            if (resp.Errors != null && resp.Errors.Count > 0)
            {
                html.Append("<div style='color:#ff6b6b;margin-top:8px;'><strong>Errors:</strong><ul>");
                foreach (var err in resp.Errors)
                {
                    // Only show if it's a real error (you might want to filter out the one that matches Output if the microservice sends it incorrectly)
                    if (!string.IsNullOrEmpty(err.Message))
                    {
                        html.Append($"<li>{HttpUtility.HtmlEncode(err.Message)}");
                        if (!string.IsNullOrEmpty(err.File) && err.File != "unknown")
                            html.Append($" (File: {err.File}, Line: {err.Line})");
                        html.Append("</li>");
                    }
                }
                html.Append("</ul></div>");
            }

            litTerminalOutput.Text = html.ToString();
            
            // Set errors for Auto-Fix
            var errorsJson = resp.Errors != null ? JsonSerializer.Serialize(resp.Errors) : "[]";
            ScriptManager.RegisterStartupScript(this, GetType(), "SetErrors", $"if(document.getElementById('hfLastErrors')) document.getElementById('hfLastErrors').value = '{HttpUtility.JavaScriptStringEncode(errorsJson)}';", true);
        }

        private void DisplayMetadata(MetadataInfo meta)
        {
            if (meta == null)
            {
                litMetadataOutput.Text = "<span style='color:#f14c4c;'>No metadata available.</span>";
                return;
            }

            var html = new StringBuilder();
            html.Append("<table class='metadata-table'>");
            html.Append($"<tr><th>Language</th><td>{HttpUtility.HtmlEncode(meta.Language)}</td></tr>");
            html.Append($"<tr><th>Container ID</th><td style='word-break:break-all;'>{HttpUtility.HtmlEncode(meta.ContainerId)}</td></tr>");
            html.Append($"<tr><th>Timestamp</th><td>{HttpUtility.HtmlEncode(meta.Timestamp)}</td></tr>");
            html.Append($"<tr><th>Status</th><td>{HttpUtility.HtmlEncode(meta.Status)}</td></tr>");

            if (meta.ResourceUsage != null)
            {
                html.Append($"<tr><th>CPU</th><td>{HttpUtility.HtmlEncode(meta.ResourceUsage.Cpu)}</td></tr>");
                html.Append($"</table><th>Memory</th><td>{HttpUtility.HtmlEncode(meta.ResourceUsage.Memory)}</td></tr>");
            }

            if (meta.FilesExecuted != null && meta.FilesExecuted.Count > 0)
            {
                html.Append("<tr><th>Files Executed</th><td>");
                html.Append(string.Join("<br/>", meta.FilesExecuted.Select(f => HttpUtility.HtmlEncode(f))));
                html.Append("</td></tr>");
            }

            if (meta.Warnings != null && meta.Warnings.Count > 0)
            {
                html.Append("<tr><th>Warnings</th><td>");
                html.Append(string.Join("<br/>", meta.Warnings.Select(w => HttpUtility.HtmlEncode(w))));
                html.Append("</td></tr>");
            }

            html.Append("</table>");
            litMetadataOutput.Text = html.ToString();
        }

        protected void btnClearConsole_Click(object sender, EventArgs e)
        {
            litTerminalOutput.Text = "<span style='color:#858585;'><i class='fas fa-magic'></i> Console cleared.</span>";
            upConsole.Update();
        }

        private static bool IsValidFileName(string name) =>
            !string.IsNullOrWhiteSpace(name) && Regex.IsMatch(name, @"^[\w\-]+(\.[\w]+)?$");

        private static string GetDefaultContent(string fileName)
        {
            if (fileName.EndsWith(".cs")) return "using System;\n\nclass Program\n{\n    static void Main()\n    {\n        Console.WriteLine(\"Hello World\");\n    }\n}\n";
            if (fileName.EndsWith(".py")) return "# Python code\nprint('Hello World')\n";
            return "";
        }

        private void ShowConsoleError(string msg)
        {
            litTerminalOutput.Text = $"<span style='color:#f14c4c;'>{HttpUtility.HtmlEncode(msg)}</span>";
            upConsole.Update();
        }
    }
}