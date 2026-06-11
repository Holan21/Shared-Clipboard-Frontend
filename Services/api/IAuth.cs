using System.Net;

namespace Shared_Clipboard_Frontend.Services.api
{
    public interface IAuth
    {
        public Task<HttpStatusCode> LoginAsync(string email, string password);
        Task<HttpStatusCode> RegisterAsync(string email, string username, string password);

    }
}
