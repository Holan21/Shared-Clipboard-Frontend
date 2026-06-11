using Microsoft.AspNetCore.SignalR.Client;
using Shared_Clipboard_Frontend.Services.Secure;
using System;
using System.Threading.Tasks;

namespace Shared_Clipboard_Frontend.Services.hubs
{
    public class ClipboardHub : IAsyncDisposable
    {
        private readonly ISecureStorageService _secureStorage;
        public Action<object?>? OnAdd;
        public Action<object?>? OnDelete;
        private HubConnection? _conn;
        private readonly string _hubUrl = ConstProvider.BaseURL + ConstProvider.CliapBoardHubController;
        private bool _starting;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public bool IsConnected => _conn?.State == HubConnectionState.Connected;

        public ClipboardHub(ISecureStorageService secureStorage)
        {
            _secureStorage = secureStorage;
        }

        public async Task StartAsync()
        {
            await _lock.WaitAsync();
            try
            {
                if (_starting) return;
                _starting = true;

                if (_conn != null)
                {
                    if (_conn.State == HubConnectionState.Connected) return;
                    await _conn.StartAsync();
                    return;
                }

                var builder = new HubConnectionBuilder();
                builder.WithUrl(_hubUrl, options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var t = await _secureStorage.GetTokenAsync();
                        return string.IsNullOrEmpty(t) ? string.Empty : t;
                    };
                    options.Transports = Microsoft.AspNetCore.Http.Connections.HttpTransportType.WebSockets
                                       | Microsoft.AspNetCore.Http.Connections.HttpTransportType.LongPolling;
#if ANDROID
                    options.HttpMessageHandlerFactory = (handler) =>
                    {
                        if (handler is HttpClientHandler clientHandler)
                        {
                            clientHandler.Proxy = null;
                            clientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
                        }
                        return handler;
                    };
#endif
                });
                builder.WithAutomaticReconnect();
                _conn = builder.Build();

                _conn.Reconnecting += error =>
                {
                    Console.WriteLine($"Reconnecting: {error?.Message}");
                    return Task.CompletedTask;
                };
                _conn.Reconnected += connectionId =>
                {
                    Console.WriteLine($"Reconnected: {connectionId}");
                    return Task.CompletedTask;
                };
                _conn.Closed += async error =>
                {
                    Console.WriteLine($"Closed: {error?.Message}");
                    await Task.CompletedTask;
                };

                _conn.On<object>("ClipboardAdd", payload =>
                {
                    MainThread.BeginInvokeOnMainThread(() => OnAdd?.Invoke(payload));
                    return Task.CompletedTask;
                });
                _conn.On<object>("ClipboardDelete", payload =>
                {
                    MainThread.BeginInvokeOnMainThread(() => OnDelete?.Invoke(payload));
                    return Task.CompletedTask;
                });

                await _conn.StartAsync();
            }
            finally
            {
                _starting = false;
                _lock.Release();
            }
        }

        public async Task StopAsync()
        {
            if (_conn == null) return;
            var stopTask = _conn.StopAsync();
            var timeoutTask = Task.Delay(3000);
            var completed = await Task.WhenAny(stopTask, timeoutTask);
            if (completed == timeoutTask)
                Console.WriteLine("StopAsync timeout, disposing anyway");
            try
            {
                var disposeTask = _conn.DisposeAsync().AsTask();
                var disposeTimeout = Task.Delay(1000);
                await Task.WhenAny(disposeTask, disposeTimeout);
            }
            catch { }
            _conn = null;
        }

        public async Task SendAddAsync(string text)
        {
            if (_conn == null || _conn.State != HubConnectionState.Connected) return;
            try { await _conn.InvokeAsync("SendAdd", text); }
            catch (Exception ex) { Console.WriteLine($"SendAdd error: {ex.Message}"); }
        }

        public async Task SendDeleteAsync(string text)
        {
            if (_conn == null || _conn.State != HubConnectionState.Connected) return;
            try { await _conn.InvokeAsync("SendDelete", text); }
            catch (Exception ex) { Console.WriteLine($"SendDelete error: {ex.Message}"); }
        }

        public async ValueTask DisposeAsync() => await StopAsync();
    }
}