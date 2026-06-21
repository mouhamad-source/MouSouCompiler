using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OnlineCompilerWebForms.Model.DTOs;

namespace OnlineCompilerWebForms.Services
{
    public class MicroserviceClient
    {
        private static readonly HttpClient client = new HttpClient();
        
        static MicroserviceClient()
        {
            client.Timeout = System.TimeSpan.FromSeconds(60);
        }

        public async Task<dynamic> ExecuteCodeAsync(string microserviceUrl, ExecutionRequestDto request)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(microserviceUrl, httpContent);
                string responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Return dynamic object so UI can read whatever microservice schema sends!
                    return JsonConvert.DeserializeObject<dynamic>(responseBody);
                }
                
                // Construct a mock dynamic response for HTTP failures
                return JsonConvert.DeserializeObject<dynamic>($@"{{
                    ""Output"": """",
                    ""Errors"": [""HTTP Error {(int)response.StatusCode}: {responseBody.Replace("\"", "\\\"")}\""]
                }}");
            }
            catch (TaskCanceledException)
            {
                return JsonConvert.DeserializeObject<dynamic>($@"{{
                    ""Output"": """",
                    ""Errors"": [""Microservice request timed out.""]
                }}");
            }
            catch (HttpRequestException ex)
            {
                return JsonConvert.DeserializeObject<dynamic>($@"{{
                    ""Output"": """",
                    ""Errors"": [""Connection failed: {ex.Message.Replace("\"", "\\\"")}""]
                }}");
            }
        }
    }
}
