using System.Text.RegularExpressions;

namespace Shared_Clipboard_Frontend.Services.Validaiton
{
    public class Validation : IValidation
    {
        private readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", RegexOptions.Compiled);
        private readonly Regex UsernameRegex = new(@"^[a-zA-Z0-9_-]{3,}$", RegexOptions.Compiled);

        private readonly int _minPasswordLength = 8;
        private readonly int _minUsernameLength = 3;

        private readonly string _requiredString = "❌{0} required";
        private readonly string _invalidString = "❌{0} is invalid";
        private readonly string _shortString = "❌{0} is minimum {1} characters";
        private readonly string _passwordsDoNotMatchString = "❌Passwords do not match";

        public (bool, string) ValidateEmail(string email)
        {
            bool resultBool = true;
            string resultString = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                resultBool = false;
                resultString = string.Format(_requiredString, nameof(email));
            }
            else if (!EmailRegex.IsMatch(email))
            {
                resultBool = false;
                resultString = string.Format(_invalidString, nameof(email));
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
                result = string.Format(_requiredString, nameof(password));
            }
            else if (password.Length < _minPasswordLength)
            {
                resultBool = false;
                result = string.Format(_shortString, nameof(password), _minPasswordLength);
            }
            else if (!PasswordRegex.IsMatch(password))
            {
                resultBool = false;
                result = string.Format(_invalidString, nameof(password));
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
                resultString = string.Format(_requiredString, nameof(username));
            }
            else if (username.Length < _minUsernameLength)
            {
                resultBool = false;
                resultString = string.Format(_shortString, nameof(username), _minUsernameLength);
            }
            else if (!UsernameRegex.IsMatch(username))
            {
                resultBool = false;
                resultString = string.Format(_invalidString, nameof(username));
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
                resultString = string.Format(_requiredString, "Confirm password");
            }
            else if (password != confirmPassword)
            {
                resultBool = false;
                resultString = _passwordsDoNotMatchString;
            }

            return (resultBool, resultString);
        }
    }
}