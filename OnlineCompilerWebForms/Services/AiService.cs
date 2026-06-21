using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OnlineCompilerWebForms.Services
{
    public class AiMessage
    {
        public string role { get; set; }
        public string content { get; set; }
    }

    public class AiRequest
    {
        public string model { get; set; }
        public List<AiMessage> messages { get; set; }
        public double temperature { get; set; }
    }

    public class AiResponse
    {
        public List<AiChoice> choices { get; set; }
    }

    public class AiChoice
    {
        public AiMessage message { get; set; }
    }

    public class AiService
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        
        public async Task<string> SendChatRequestAsync(List<AiMessage> messages)
        {
            var baseUrl = ConfigurationManager.AppSettings["AiApiBaseUrl"];
            var apiKey = ConfigurationManager.AppSettings["AiApiKey"];
            var modelName = ConfigurationManager.AppSettings["AiModelName"];

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
            {
                return "Error: AI API configuration is missing in Web.config. Please set AiApiBaseUrl and AiApiKey.";
            }

            var requestBody = new AiRequest
            {
                model = modelName ?? "deepseek-coder",
                messages = messages,
                temperature = 0.2 // low temp for coding tasks
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            
            var request = new HttpRequestMessage(HttpMethod.Post, baseUrl);
            request.Headers.Add("Authorization", $"Bearer {apiKey}");
            request.Headers.Add("HTTP-Referer", "http://localhost:44312/");
            request.Headers.Add("X-Title", "Online Compiler IDE");
            request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<AiResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return result?.choices?.FirstOrDefault()?.message?.content ?? "No response received from AI.";
                }
                else
                {
                    return $"API Error ({response.StatusCode}): {responseContent}";
                }
            }
            catch (Exception ex)
            {
                return $"Exception during AI API call: {ex.Message}";
            }
        }
    }
}
