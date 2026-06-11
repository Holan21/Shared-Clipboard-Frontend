using Shared_Clipboard_Frontend.Services.Secure;

namespace Shared_Clipboard_Frontend.Services.Secure
{
    public partial class SecureStorageService : ISecureStorageService
    {
        private readonly string TokenKey = ConstProvider.TokenKey;

        public Task SaveTokenAsync(string jwt)
            => SecureStorage.Default.SetAsync(TokenKey, jwt ?? string.Empty);

        public async Task<string?> GetTokenAsync()
        {
            var token = await SecureStorage.Default.GetAsync(TokenKey);
            if (string.IsNullOrWhiteSpace(token)) return null;
            return token;
        }

        public async Task<bool> ClearAsync()
            => SecureStorage.Default.Remove(ConstProvider.TokenKey);
        
                
    }
}