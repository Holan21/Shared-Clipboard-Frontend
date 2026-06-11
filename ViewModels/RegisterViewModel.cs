using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Data.api_v1.JSON.Auth;
using Shared_Clipboard_Frontend.Pages;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Validaiton;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class RegisterViewModel(
        IAuth authService,
        IValidation validationService) : ObservableObject
    {
        [ObservableProperty]
        public partial string Username { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Email { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Password { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ConfirmPassword { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string UsernameError { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string EmailError { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string PasswordError { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string ConfirmPasswordError { get; set; } = string.Empty;

        public bool HasUsernameError => !string.IsNullOrEmpty(UsernameError);
        public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
        public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);
        public bool HasConfirmPasswordError => !string.IsNullOrEmpty(ConfirmPasswordError);

        partial void OnUsernameErrorChanged(string value) => OnPropertyChanged(nameof(HasUsernameError));
        partial void OnEmailErrorChanged(string value) => OnPropertyChanged(nameof(HasEmailError));
        partial void OnPasswordErrorChanged(string value) => OnPropertyChanged(nameof(HasPasswordError));
        partial void OnConfirmPasswordErrorChanged(string value) => OnPropertyChanged(nameof(HasConfirmPasswordError));

        partial void OnUsernameChanged(string value)
        {
            if (!string.IsNullOrEmpty(UsernameError))
                UsernameError = string.Empty;
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

        partial void OnConfirmPasswordChanged(string value)
        {
            if (!string.IsNullOrEmpty(ConfirmPasswordError))
                ConfirmPasswordError = string.Empty;
        }

        void ClearErrors()
        {
            UsernameError = string.Empty;
            EmailError = string.Empty;
            PasswordError = string.Empty;
            ConfirmPasswordError = string.Empty;
        }

        public void ClearAll()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;

            ClearErrors();
        }

        [RelayCommand]
        async Task RegisterAsync()
        {
            ClearErrors();

            var (isValidUsername, feedbackUsername) = validationService.ValidateUsername(Username);
            var (isValidEmail, feedbackEmail) = validationService.ValidateEmail(Email);
            var (isValidPassword, feedbackPassword) = validationService.ValidatePassword(Password);
            var (isValidConfirmPassword, feedbackConfirmPassword) = validationService.ValidateConfirmPassword(Password, ConfirmPassword);

            if (!isValidUsername)
                UsernameError = feedbackUsername;

            if (!isValidEmail)
                EmailError = feedbackEmail;

            if (!isValidPassword)
                PasswordError = feedbackPassword;

            if (!isValidConfirmPassword)
                ConfirmPasswordError = feedbackConfirmPassword;

            if (!isValidUsername || !isValidEmail || !isValidPassword || !isValidConfirmPassword)
                return;

            try
            {
                await authService.RegisterAsync(Email, Username, Password);

                await Application.Current.MainPage.DisplayAlertAsync(
                    "Success",
                    "Registration successful! Please login.",
                    "OK");
                ClearAll();

                await Shell.Current.GoToAsync("//" + nameof(LoginPage));
            }
            catch (Exception ex)
            {
                PasswordError = "Registration failed: " + ex.Message;
            }
        }

        [RelayCommand]
        private async Task GoToLoginPage()
        {
            ClearAll();

            await Shell.Current.GoToAsync("//" + nameof(LoginPage));
        }
    }
}