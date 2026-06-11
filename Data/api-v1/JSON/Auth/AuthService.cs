using Shared_Clipboard_Frontend.Services;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Secure;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared_Clipboard_Frontend.Data.api_v1.JSON.Auth
{
    public class AuthService : IAuth
    {
        private readonly ISecureStorageService _secureStorageService;

        public AuthService(ISecureStorageService secureStorage)
        {
            _secureStorageService = secureStorage;
        }

        private static HttpClient CreateHttpClient()
        {
#if ANDROID
            var handler = new HttpClientHandler();
            handler.Proxy = null;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return new HttpClient(handler) { BaseAddress = new Uri(ConstProvider.BaseURL) };
#else
            return new HttpClient { BaseAddress = new Uri(ConstProvider.BaseURL) };
#endif
        }

        public async Task<HttpStatusCode> LoginAsync(string email, string password)
        {
            try
            {
                var payload = new { email, password };
                var json = JsonSerializer.Serialize(payload);

                using var client = CreateHttpClient();
                var content = new StringContent(json, Encoding.UTF8, ConstProvider.MediaType);
                var result = await client.PostAsync(ConstProvider.LoginController, content);
                string response = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                    await _secureStorageService.SaveTokenAsync(response);
                return result.StatusCode;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Login Error", ex.Message, "OK");
            }
            return HttpStatusCode.ServiceUnavailable;
        }

        public async Task<HttpStatusCode> RegisterAsync(string email, string username, string password)
        {
            var payload = new { email, username, password };
            var json = JsonSerializer.Serialize(payload);

            using var client = CreateHttpClient();
            var content = new StringContent(json, Encoding.UTF8, ConstProvider.MediaType);
            var response = await client.PostAsync(ConstProvider.RegisterController, content);
            return response.StatusCode;
        }
    }
}