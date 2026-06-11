using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.hubs;
using Shared_Clipboard_Frontend.Services.Secure;

namespace Shared_Clipboard_Frontend.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<ClipboardItemViewModel> clipboardItems = new();

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        private readonly ClipboardHub _hubService;
        private readonly IClipboardService _clipboardService;
        private readonly ISecureStorageService _secureStorage;
        private bool _isInitialized;
        private CancellationTokenSource? _loadCts;

        public MainViewModel(ClipboardHub hubService, IClipboardService clipboard, ISecureStorageService secureStorage)
        {
            _hubService = hubService;
            _clipboardService = clipboard;
            _secureStorage = secureStorage;
            _hubService.OnAdd += AddClipboardItemFromHub;
            _hubService.OnDelete += DeleteClipboardItemFromHub;
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            if (_isInitialized) return;
            _isInitialized = true;
            await LoadClipboardItemsAsync();
        }

        private async Task LoadClipboardItemsAsync()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = new CancellationTokenSource();
            var ct = _loadCts.Token;
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                var items = await _clipboardService.GetAllClipboardItems(ct);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    if (ct.IsCancellationRequested) return;
                    ClipboardItems.Clear();
                    foreach (var item in items.OrderByDescending(x => x.CreatedAt))
                    {
                        ClipboardItems.Add(new ClipboardItemViewModel(this)
                        {
                            Content = item.Data,
                            CreatedAt = item.CreatedAt
                        });
                    }
                });
            }
            catch (OperationCanceledException) { }
            catch (Exception ex)
            {
                ErrorMessage = $"Loading error: {ex.Message}";
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Application.Current.MainPage?.DisplayAlert("Error", ErrorMessage, "OK");
                });
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddClipboardItem()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync(
                "Add Clipboard Item", "Enter text:", "Add", "Cancel", "Enter text here...");
            if (string.IsNullOrWhiteSpace(result)) return;
            if (ClipboardItems.Any(i => i.Content == result))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "This text already exists", "OK");
                return;
            }
            var newItem = new ClipboardItemViewModel(this)
            {
                Content = result,
                CreatedAt = DateTime.Now
            };
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ClipboardItems.Insert(0, newItem);
            });
            await _hubService.SendAddAsync(result);
        }

        [RelayCommand]
        private async Task DeleteItem(ClipboardItemViewModel? item)
        {
            if (item == null) return;
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ClipboardItems.Remove(item);
            });
            await _hubService.SendDeleteAsync(item.Content);
        }

        [RelayCommand]
        private async Task RefreshItems()
        {
            await LoadClipboardItemsAsync();
        }

        [RelayCommand]
        private async Task Logout()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = null;
            await _hubService.StopAsync();
            await _secureStorage.ClearAsync();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ClipboardItems.Clear();
                ErrorMessage = string.Empty;
            });
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public void AddClipboardItemFromHub(object? payload)
        {
            var text = payload?.ToString() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return;
            var exists = ClipboardItems.Any(i => i.Content == text);
            if (exists) return;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ClipboardItems.Insert(0, new ClipboardItemViewModel(this)
                {
                    Content = text,
                    CreatedAt = DateTime.Now
                });
            });
        }

        public void DeleteClipboardItemFromHub(object? payload)
        {
            var text = payload?.ToString();
            if (string.IsNullOrEmpty(text)) return;
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var item = ClipboardItems.FirstOrDefault(i => i.Content == text);
                if (item != null) ClipboardItems.Remove(item);
            });
        }

        public async Task AfterLogin()
        {
            await _hubService.StartAsync();
            await LoadClipboardItemsAsync();
        }

        public async Task OnLogout()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();
            _loadCts = null;
            _ = _hubService.StopAsync();
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                ClipboardItems.Clear();
                ErrorMessage = string.Empty;
            });
        }
    }
}