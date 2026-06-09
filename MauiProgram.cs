using Microsoft.Extensions.Logging;
using Shared_Clipboard_Frontend;
using Shared_Clipboard_Frontend.Data.api_v1.JSON.Login;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.Validaiton;
using Shared_Clipboard_Frontend.ViewModels;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
        => MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .RegisterServices()
            .RegisterViewModels()
            .RegisterViews()
            .Build();

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<ILogin, LoginService>();
        mauiAppBuilder.Services.AddTransient<IValidation, Validation>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<LoginViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<LoginPage>();
        return mauiAppBuilder;
    }
}