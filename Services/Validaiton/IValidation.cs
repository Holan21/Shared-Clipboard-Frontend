using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Clipboard_Frontend.Services.Validaiton
{
    public interface IValidation
    {
        (bool, string) ValidateConfirmPassword(string password, string confirmPassword);
        (bool,string) ValidateEmail(string email);
        (bool,string) ValidatePassword(string password);
        (bool, string) ValidateUsername(string username);
    }
}
