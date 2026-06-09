using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Clipboard_Frontend.Services.api
{
    public interface IRegister
    {
        Task<bool> RegisterAsync(string email, string username, string password);
    }
}
