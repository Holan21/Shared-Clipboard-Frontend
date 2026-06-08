using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Services.api;
using System.Diagnostics;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class LoginViewModel(ILogin loginService) : ObservableObject
    {

        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;
        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [RelayCommand]
        async Task LoginAsync()
        {
            Debug.Print(await loginService.LoginAsync(Email, Password));
        }
    }
}
