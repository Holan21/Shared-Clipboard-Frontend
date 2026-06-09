using System.Text.RegularExpressions;

namespace Shared_Clipboard_Frontend.Services.Validaiton
{
    public class Validation : IValidation
    {
        private readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private readonly Regex PasswordRegex = new(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{8,}$", RegexOptions.Compiled);

        private readonly int _minLength = 8;

        private readonly string _requiredString = "❌{0} required";
        private readonly string _invalidString = "❌{0} is invalid";
        private readonly string _shortString = "❌{0} is minimum {1} characters";
        public (bool,string) ValidateEmail(string email)
        {
            bool resultBool = true;
            string resultString = string.Empty;

            if (string.IsNullOrWhiteSpace(email))
            {
                resultBool = false;
                resultString = string.Format( _requiredString,nameof(email));
            }

            if (!EmailRegex.IsMatch(email))
            {
                resultBool = false;
                resultString = string.Format(_invalidString,nameof(email));
            }

            return (resultBool,resultString);
        }

        public (bool,string) ValidatePassword(string password)
        {
            bool resultBool = true;
            string result = string.Empty;

            if (string.IsNullOrWhiteSpace(password))
            {
                resultBool = false;
                result = string.Format(_requiredString,nameof(password));
            }

            if (password.Length < _minLength)
            {
                resultBool = false;
                result = string.Format(_shortString, nameof(password), _minLength);
            }

            if (!PasswordRegex.IsMatch(password))
            {
                resultBool = false;
                result = string.Format(_invalidString,nameof(password));
            }

            return (resultBool,result);

        }
    }
}
