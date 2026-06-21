using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using OnlineCompilerWebForms.Model.DTOs;

namespace OnlineCompilerWebForms.Utils
{
    public static class ServiceLinkMicroserver
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<string> FetchFromMicroservice(string url, CompilerRequest request)
        {
            var serializer = new JavaScriptSerializer();
            var jsonContent = serializer.Serialize(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, httpContent);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
