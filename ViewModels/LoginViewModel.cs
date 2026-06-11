using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Pages;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Validaiton;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class LoginViewModel(
        IAuth authService,
        IValidation validationService) : ObservableObject
    {
        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EmailError { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string PasswordError { get; set; } = string.Empty;


        public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
        public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);

        void ClearErrors()
        {
            EmailError = string.Empty;
            PasswordError = string.Empty;
        }

        partial void OnEmailErrorChanged(string value) => OnPropertyChanged(nameof(HasEmailError));
        partial void OnPasswordErrorChanged(string value) => OnPropertyChanged(nameof(HasPasswordError));

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
                var response = await authService.LoginAsync(Email, Password);
                if (response == System.Net.HttpStatusCode.OK)
                {
                    await Shell.Current.GoToAsync("//" + nameof(MainPage));
                }

            }
            catch (Exception)
            {
                PasswordError = "Login failed";
            }
        }

        [RelayCommand]
        private async Task GoToRegister()
        {
            await Shell.Current.GoToAsync( "//" +nameof(RegisterPage));
        }
    }
}
