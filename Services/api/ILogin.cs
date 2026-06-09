namespace Shared_Clipboard_Frontend.Services.api
{
    public interface ILogin
    {
        public Task<string> LoginAsync(string email, string password);

    }
}
