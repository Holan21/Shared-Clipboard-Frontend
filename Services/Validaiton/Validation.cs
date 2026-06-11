using System.Text.RegularExpressions;

namespace Shared_Clipboard_Frontend.Services.Validaiton
{
    public class Validation : IValidation
    {

        public (bool, string) ValidateEmail(string email)
        {
            bool resultBool = true;
            string resultString = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.RequiredString, nameof(email));
            }
            else if (!ConstProvider.EmailRegex.IsMatch(email))
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.InvalidString, nameof(email));
            }

            return (resultBool, resultString);
        }

        public (bool, string) ValidatePassword(string password)
        {
            bool resultBool = true;
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(password))
            {
                resultBool = false;
                result = string.Format(ConstProvider.RequiredString, nameof(password));
            }
            else if (password.Length < ConstProvider.MinPasswordLength)
            {
                resultBool = false;
                result = string.Format(ConstProvider.ShortString, nameof(password), ConstProvider.MinPasswordLength);
            }
            else if (!ConstProvider.PasswordRegex.IsMatch(password))
            {
                resultBool = false;
                result = string.Format(ConstProvider.InvalidString, nameof(password));
            }

            return (resultBool, result);
        }

        public (bool, string) ValidateUsername(string username)
        {
            bool resultBool = true;
            string resultString = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.RequiredString, nameof(username));
            }
            else if (username.Length < ConstProvider.MinUsernameLength)
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.ShortString, nameof(username), ConstProvider.MinUsernameLength);
            }
            else if (!ConstProvider.UsernameRegex.IsMatch(username))
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.InvalidString, nameof(username));
            }

            return (resultBool, resultString);
        }

        public (bool, string) ValidateConfirmPassword(string password, string confirmPassword)
        {
            bool resultBool = true;
            string resultString = string.Empty;

            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                resultBool = false;
                resultString = string.Format(ConstProvider.RequiredString, "Confirm password");
            }
            else if (password != confirmPassword)
            {
                resultBool = false;
                resultString = ConstProvider.PasswordsDoNotMatchString;
            }

            return (resultBool, resultString);
        }
    }
}