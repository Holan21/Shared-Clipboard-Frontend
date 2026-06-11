using Shared_Clipboard_Frontend.Models;
using Shared_Clipboard_Frontend.Services;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Secure;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Shared_Clipboard_Frontend.Data.api_v1.JSON.Clipboard
{
    public class ClipboardService : IClipboardService
    {
        private readonly ISecureStorageService _storage;

        public ClipboardService(ISecureStorageService storage)
        {
            _storage = storage;
        }

        private static HttpClient CreateHttpClient()
        {
#if ANDROID
            var handler = new HttpClientHandler();
            handler.Proxy = null;
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            return new HttpClient(handler) { BaseAddress = new Uri(ConstProvider.BaseURL), Timeout = TimeSpan.FromSeconds(10) };
#else
            return new HttpClient { BaseAddress = new Uri(ConstProvider.BaseURL), Timeout = TimeSpan.FromSeconds(10) };
#endif
        }

        public async Task<List<ClipboardItem>> GetAllClipboardItems(CancellationToken cancellationToken = default)
        {
            using var client = CreateHttpClient();
            var key = await _storage.GetTokenAsync();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", key);
            var result = await client.GetAsync(ConstProvider.GetAllClipboardItemsController, cancellationToken);
            if (!result.IsSuccessStatusCode)
            {
                var error = await result.Content.ReadAsStringAsync(cancellationToken);
                throw new HttpRequestException($"Exception {result.StatusCode}, {error}");
            }
            string response = await result.Content.ReadAsStringAsync(cancellationToken);
            var clipboardItems = JsonSerializer.Deserialize<List<ClipboardItem>>(response, ConstProvider.JsonOptions);
            return clipboardItems ?? new List<ClipboardItem>();
        }
    }
}