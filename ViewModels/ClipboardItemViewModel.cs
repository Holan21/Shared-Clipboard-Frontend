 using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class ClipboardItemViewModel(MainViewModel parentViewModel) : ObservableObject
    {
        private readonly MainViewModel _parentViewModel = parentViewModel;

        [ObservableProperty]
        public partial string Content { get; set; } = string.Empty;

        [ObservableProperty]
        public partial DateTime CreatedAt { get; set; }

        [RelayCommand]
        private async Task Copy()
        {
            await Clipboard.Default.SetTextAsync(Content);

            await Application.Current.MainPage.DisplayAlertAsync(
                "Copied",
                "Content copied to clipboard",
                "OK");
        }

        [RelayCommand]
        private void Delete()
        {
            if (_parentViewModel != null)
            {
                _parentViewModel.DeleteItemCommand.Execute(this);
            }
        }
    }
}