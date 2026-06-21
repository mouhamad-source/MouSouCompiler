using System;

namespace OnlineCompilerWebForms.Services
{
    public class TokenService
    {
        public string GenerateToken()
        {
            return Guid.NewGuid().ToString("N") + "-" + Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "").Replace("/", "");
        }
    }
}
