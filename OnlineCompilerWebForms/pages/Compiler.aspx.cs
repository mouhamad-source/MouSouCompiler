using OnlineCompilerWebForms.Utils;
using OnlineCompilerWebForms.Model.DTOs;
using System;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace OnlineCompilerWebForms.pages
{
    public partial class Compiler : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("~/pages/Login.aspx");
            }
        }

        protected async void btnRun_Click(object sender, EventArgs e)
        {
            btnRun.Enabled = false;
            litResult.Text = "<span style='color: #4ec9b0;'>🔄 Compiling & running... This may take a few seconds.</span>";
            litExecutionTime.Visible = false;

            try
            {
                var request = new CompilerRequest
                {
                    Language = ddlLanguage.SelectedValue,
                    Code = txtCode.Text
                };

                string microserviceUrl = ConfigurationManager.AppSettings["CompilerMicroserviceUrl"];
                string jsonResponse = await ServiceLinkMicroserver.FetchFromMicroservice(microserviceUrl, request);

                var serializer = new JavaScriptSerializer();
                dynamic result = serializer.Deserialize<dynamic>(jsonResponse);

                string output = result["Output"] ?? "";
                string error = result["Error"] ?? "";
                bool success = (bool)(result["Success"] ?? false);
                string execTime = result["ExecutionTime"] ?? "";

                if (success)
                {
                    if (string.IsNullOrWhiteSpace(output))
                    {
                        litResult.Text = "<span class='success-message'>✅ Program executed successfully (no output).</span>";
                    }
                    else
                    {
                        string encodedOutput = HttpUtility.HtmlEncode(output);
                        string formattedOutput = encodedOutput.Replace("\n", "<br />");
                        litResult.Text = $"<pre class='mb-0'>{formattedOutput}</pre>";
                    }
                }
                else
                {
                    string errorMsg = string.IsNullOrWhiteSpace(error) ? output : error;
                    string encodedError = HttpUtility.HtmlEncode(errorMsg);
                    string formattedError = encodedError.Replace("\n", "<br />");

                    string errorType = errorMsg.Contains("Compilation failed") || errorMsg.Contains("error CS")
                        ? "Compilation Error"
                        : "Runtime Error";

                    litResult.Text = $@"
                        <div class='error-message'>
                            <strong>⚠ {errorType}</strong>
                            <div class='mt-2' style='font-family: monospace;'>{formattedError}</div>
                        </div>";
                }

                if (!string.IsNullOrEmpty(execTime))
                {
                    litExecutionTime.Text = $"<span class='exec-time-badge'>⏱ {execTime}</span>";
                    litExecutionTime.Visible = true;
                }
                else
                {
                    litExecutionTime.Visible = false;
                }
            }
            catch (Exception ex)
            {
                string errorHtml = $@"
                    <div class='error-message'>
                        <strong>☁ Microservice Error</strong>
                        <div class='mt-2'>Unable to reach the compiler service. Please try again later.</div>
                        <div class='mt-2 small text-muted'>Details: {HttpUtility.HtmlEncode(ex.Message)}</div>
                    </div>";
                litResult.Text = errorHtml;
                litExecutionTime.Visible = false;
            }
            finally
            {
                btnRun.Enabled = true;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtCode.Text = "";
            litResult.Text = "<span style='color: #858585;'>✨ Editor cleared. Write your code and click Run.</span>";
            litExecutionTime.Visible = false;
        }
    }
}
