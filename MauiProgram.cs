using Microsoft.Extensions.Logging;
using Shared_Clipboard_Frontend.Data.api_v1.JSON.Login;
using Shared_Clipboard_Frontend.Services.api;

namespace Shared_Clipboard_Frontend
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ILogin, LoginService>();
            builder.Services.AddTransient<LoginPage>();
            return builder.Build();
        }
    }
}
