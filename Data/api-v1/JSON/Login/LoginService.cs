using Shared_Clipboard_Frontend.Services.api;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Shared_Clipboard_Frontend.Data.api_v1.JSON.Login
{
    public class LoginService : ILogin
    {
        public async Task<string> LoginAsync(string email, string password)
        {
            string json = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

            HttpClient client = new()
            {
                BaseAddress = new Uri(@"https://localhost:7181/api/v1/"),
            };

            var content = new StringContent(json, MediaTypeHeaderValue.Parse("text/json"));
            var result = await client.PostAsync("Auth/login",content);

            return result.ToString();
        }
    }
}
