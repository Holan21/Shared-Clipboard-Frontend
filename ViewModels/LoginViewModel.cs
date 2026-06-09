using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Validaiton;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class LoginViewModel(
        ILogin loginService,
        IValidation validationService) : ObservableObject
    {
        [ObservableProperty]
        string email = string.Empty;

        [ObservableProperty]
        string password = string.Empty;

        [ObservableProperty]
        string emailError = string.Empty;

        partial void OnEmailErrorChanged(string value) => OnPropertyChanged(nameof(HasEmailError));

        [ObservableProperty]
        string passwordError = string.Empty;

        partial void OnPasswordErrorChanged(string value) => OnPropertyChanged(nameof(HasPasswordError));

        public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
        public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);

        [RelayCommand]
        async Task LoginAsync()
        {
            ClearErrors();

            var (isValidEmail, feedbackEmail) = validationService.ValidateEmail(Email);
            var (isValidPassword, feedbackPassword) = validationService.ValidatePassword(Password);

            if (!isValidEmail)
                EmailError = feedbackEmail;

            if (!isValidPassword)
                PasswordError = feedbackPassword;
            
            if (!isValidEmail || !isValidPassword)
                return;

            try
            {
                await loginService.LoginAsync(Email, Password);
            }
            catch (Exception ex)
            {
                PasswordError = "Login failed: " + ex.Message;
            }
        }

        void ClearErrors()
        {
            EmailError = string.Empty;
            PasswordError = string.Empty;
        }

        partial void OnEmailChanged(string value)
        {
            if (!string.IsNullOrEmpty(EmailError))
                EmailError = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            if (!string.IsNullOrEmpty(PasswordError))
                PasswordError = string.Empty;
        }
    }
}
