using Microsoft.Extensions.DependencyInjection;
using Shared_Clipboard_Frontend.Services.Secure;

namespace Shared_Clipboard_Frontend
{
    public partial class App : Application
    {
        private readonly ISecureStorageService _service;

        public App(ISecureStorageService service)
        {
            InitializeComponent();
            _service = service;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_service));
        }
    }
}