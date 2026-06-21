using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;
using OnlineCompilerWebForms.Services;

namespace OnlineCompilerWebForms.Handlers
{
    public class AiChatRequest
    {
        public Guid ProjectId { get; set; }
        public string Prompt { get; set; }
        public List<string> MentionedFileNames { get; set; }
        public string ActiveFileName { get; set; }
        public string ActiveFileContent { get; set; }
        public bool IsAutoFix { get; set; }
    }

    public class AiChatHandler : HttpTaskAsyncHandler, IRequiresSessionState
    {
        private readonly FileService _fileService = new FileService();
        private readonly AiService _aiService = new AiService();

        public override async Task ProcessRequestAsync(HttpContext context)
        {
            if (context.Session["UserId"] == null)
            {
                context.Response.StatusCode = 401;
                context.Response.Write("Unauthorized");
                return;
            }

            if (context.Request.HttpMethod != "POST")
            {
                context.Response.StatusCode = 405;
                context.Response.Write("Method Not Allowed");
                return;
            }

            try
            {
                using (var reader = new StreamReader(context.Request.InputStream))
                {
                    var body = await reader.ReadToEndAsync();
                    var request = JsonSerializer.Deserialize<AiChatRequest>(body, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (request == null || string.IsNullOrWhiteSpace(request.Prompt))
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Write("Invalid request.");
                        return;
                    }

                    var userId = (Guid)context.Session["UserId"];
                    var fileStoragePath = ConfigurationManager.AppSettings["FilePath"] ?? context.Server.MapPath("~/App_Data/CompilerFiles");
                    var userProjectPath = Path.Combine(fileStoragePath, userId.ToString(), request.ProjectId.ToString());

                    // Gather context
                    var allProjectFiles = _fileService.GetProjectFiles(request.ProjectId, userProjectPath);
                    var contextBuilder = new System.Text.StringBuilder();

                    contextBuilder.AppendLine("You are an expert AI coding assistant integrated into a web-based IDE.");
                    contextBuilder.AppendLine("The user is working on a project. You must help them write, debug, and explain code.");
                    
                    if (request.IsAutoFix)
                    {
                        contextBuilder.AppendLine("The user has requested an AUTO-FIX for an error that occurred during execution.");
                        contextBuilder.AppendLine("If you need to provide a code fix, format your response using a markdown code block starting with ` ```csharp ` (or python, etc) followed by the entire corrected file content.");
                        contextBuilder.AppendLine("Do not provide partial code. Provide the FULL file content in the code block so it can be automatically applied.");
                    }
                    else
                    {
                        contextBuilder.AppendLine("When suggesting code changes to a file, format your suggested code inside a markdown code block starting with the language (e.g., ` ```csharp `). Provide the complete code or the specific snippet the user needs.");
                    }

                    if (!string.IsNullOrEmpty(request.ActiveFileName) && !string.IsNullOrEmpty(request.ActiveFileContent))
                    {
                        contextBuilder.AppendLine($"\n--- Currently Active File: {request.ActiveFileName} ---");
                        contextBuilder.AppendLine(request.ActiveFileContent);
                        contextBuilder.AppendLine("--- End of Active File ---");
                    }

                    if (request.MentionedFileNames != null && request.MentionedFileNames.Count > 0 && allProjectFiles != null)
                    {
                        foreach (var fileName in request.MentionedFileNames)
                        {
                            var mentionedFile = allProjectFiles.FirstOrDefault(f => f.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase));
                            if (mentionedFile != null && mentionedFile.FileName != request.ActiveFileName)
                            {
                                contextBuilder.AppendLine($"\n--- Mentioned File Context: {mentionedFile.FileName} ---");
                                contextBuilder.AppendLine(mentionedFile.Content);
                                contextBuilder.AppendLine("--- End of Mentioned File ---");
                            }
                        }
                    }

                    var messages = new List<AiMessage>
                    {
                        new AiMessage { role = "system", content = contextBuilder.ToString() },
                        new AiMessage { role = "user", content = request.Prompt }
                    };

                    var aiResponse = await _aiService.SendChatRequestAsync(messages);

                    context.Response.ContentType = "application/json";
                    context.Response.Write(JsonSerializer.Serialize(new { reply = aiResponse }));
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(JsonSerializer.Serialize(new { error = ex.Message }));
            }
        }
    }
}
