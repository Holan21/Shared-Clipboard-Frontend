using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.ViewModels;

namespace Shared_Clipboard_Frontend.Pages;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}


}