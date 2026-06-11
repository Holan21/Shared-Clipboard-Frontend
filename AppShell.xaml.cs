using Shared_Clipboard_Frontend.Pages;
using Shared_Clipboard_Frontend.Services.Secure;

namespace Shared_Clipboard_Frontend
{
    public partial class AppShell : Shell
    {
        private readonly ISecureStorageService _secureStorage;

        public AppShell(ISecureStorageService secureStorage)
        {
            InitializeComponent();
            _secureStorage = secureStorage;
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var token = await _secureStorage.GetTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                await GoToAsync("//MainPage");
            }
            else
            {
                await GoToAsync("//LoginPage");
            }
        }
    }
}