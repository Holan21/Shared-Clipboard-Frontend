using Shared_Clipboard_Frontend.Services.api;
using System.Diagnostics;

namespace Shared_Clipboard_Frontend
{

    public partial class LoginPage : ContentPage
    {
        private readonly ILogin _login;

        public LoginPage(ILogin login)
        {
            _login = login;
            InitializeComponent();
        }

        private void OnPressLoginButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            Debug.Print("Pressed");
        }

        private void OnRealesedLoginButton(object sender, EventArgs e)
        {
            var button = sender as Button;
            Debug.Print("Realsed");
        }
    }
}
