using Shared_Clipboard_Frontend.ViewModels;
using System;

namespace Shared_Clipboard_Frontend.Pages
{
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.AfterLogin();
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                await _viewModel.OnLogout();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnDisappearing error: {ex.Message}");
            }
        }
    }
}