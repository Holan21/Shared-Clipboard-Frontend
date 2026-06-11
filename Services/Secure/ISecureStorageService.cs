using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Clipboard_Frontend.Services.Secure
{
    public partial interface ISecureStorageService
    {
        Task SaveTokenAsync(string jwt);
        Task<string?> GetTokenAsync();
        Task<bool> ClearAsync();
    }
}
