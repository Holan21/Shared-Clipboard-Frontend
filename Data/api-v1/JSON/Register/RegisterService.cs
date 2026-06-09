using Shared_Clipboard_Frontend.Services.api;
using System.Net.Http.Headers;

namespace Shared_Clipboard_Frontend.Data.api_v1.JSON.Register
{
    public class RegisterService : IRegister
    {
        public async Task<bool> RegisterAsync(string email,string username, string password)
        {
            string json = "{" + "\"username\":" + username + 
                "\"" + "," + 
                "\"password\":\"" + password + 
                "\"" + "," + 
                "\"email\":\"" + email + 
                "}";

            HttpClient client = new()
            {
                BaseAddress = new Uri(@"https://localhost:7181/api/v1/"),
            };

            var content = new StringContent(json, MediaTypeHeaderValue.Parse("text/json"));
            var response = await client.PostAsync("Auth/register", content);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
