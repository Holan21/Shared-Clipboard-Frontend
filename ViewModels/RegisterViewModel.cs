using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Validaiton;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class RegisterViewModel(
        IRegister registerService,
        IValidation validationService) : ObservableObject
    {
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string confirmPassword = string.Empty;

        [ObservableProperty]
        private string usernameError = string.Empty;

        [ObservableProperty]
        private string emailError = string.Empty;

        [ObservableProperty]
        private string passwordError = string.Empty;

        [ObservableProperty]
        private string confirmPasswordError = string.Empty;

        public bool HasUsernameError => !string.IsNullOrEmpty(UsernameError);
        public bool HasEmailError => !string.IsNullOrEmpty(EmailError);
        public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);
        public bool HasConfirmPasswordError => !string.IsNullOrEmpty(ConfirmPasswordError);

        void ClearErrors()
        {
            UsernameError = string.Empty;
            EmailError = string.Empty;
            PasswordError = string.Empty;
            ConfirmPasswordError = string.Empty;
        }

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
                await registerService.RegisterAsync(Username, Email, Password);

                await Application.Current.MainPage.DisplayAlertAsync(
                    "Success",
                    "Registration successful! Please login.",
                    "OK");

                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                PasswordError = "Registration failed: " + ex.Message;
            }
        }

        public void ClearAll()
        {
            username = string.Empty;
            email = string.Empty;
            password = string.Empty;
            confirmPassword = string.Empty;

            OnPropertyChanged(username);
            OnPropertyChanged(email);
            OnPropertyChanged(password);
            OnPropertyChanged(confirmPassword);

            ClearErrors();
        }

        [RelayCommand]
        private async Task GoToLoginPage()
        {
            ClearAll();

            await Shell.Current.GoToAsync("..");
        }
    }
}