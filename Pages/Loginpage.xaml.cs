using System.Diagnostics;

namespace Shared_Clipboard_Frontend
{

    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
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
