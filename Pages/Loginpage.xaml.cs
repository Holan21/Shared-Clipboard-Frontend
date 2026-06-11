using Shared_Clipboard_Frontend.ViewModels;

namespace Shared_Clipboard_Frontend.Pages
{

    public partial class LoginPage : ContentPage
    {

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
