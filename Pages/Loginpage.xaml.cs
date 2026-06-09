using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.ViewModels;
using System.Diagnostics;

namespace Shared_Clipboard_Frontend
{

    public partial class LoginPage : ContentPage
    {
        private readonly LoginViewModel _viewModel;

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }
    }
}
