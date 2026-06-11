using Shared_Clipboard_Frontend;
using Shared_Clipboard_Frontend.Data.api_v1.JSON.Auth;
using Shared_Clipboard_Frontend.Data.api_v1.JSON.Clipboard;
using Shared_Clipboard_Frontend.Pages;
using Shared_Clipboard_Frontend.Services.api;
using Shared_Clipboard_Frontend.Services.hubs;
using Shared_Clipboard_Frontend.Services.Secure;
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
        mauiAppBuilder.Services.AddTransient<IAuth, AuthService>();
        mauiAppBuilder.Services.AddTransient<IValidation, Validation>();
        mauiAppBuilder.Services.AddTransient<ISecureStorageService,SecureStorageService>();
        mauiAppBuilder.Services.AddTransient<IClipboardService,ClipboardService>();
        mauiAppBuilder.Services.AddTransient<ClipboardHub>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<LoginViewModel>();
        mauiAppBuilder.Services.AddSingleton<RegisterViewModel>();
        mauiAppBuilder.Services.AddTransient<MainViewModel>();
        mauiAppBuilder.Services.AddSingleton<ClipboardItemViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<LoginPage>();
        mauiAppBuilder.Services.AddSingleton<RegisterPage>();
        mauiAppBuilder.Services.AddTransient<MainPage>();
        return mauiAppBuilder;
    }

}