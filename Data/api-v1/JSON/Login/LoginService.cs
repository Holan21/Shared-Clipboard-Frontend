using Shared_Clipboard_Frontend.Services.api;

namespace Shared_Clipboard_Frontend.Data.api_v1.JSON.Login
{
    internal class LoginService : ILogin
    {
        string ILogin.Login(string email, string password)
        {
            return "Testing DI";
        }
    }
}
