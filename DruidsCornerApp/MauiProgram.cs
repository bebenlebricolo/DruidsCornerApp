using System.Reflection;
using CommunityToolkit.Maui;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.Config;
using DruidsCornerApp.Utils;
using DruidsCornerApp.ViewModels;
using DruidsCornerApp.ViewModels.Login;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.Login;
using DruidsCornerApp.Views.Recipes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;

namespace DruidsCornerApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureMopups()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        // Registering Pages here (for dependency injection)
        builder.Services.AddTransient<WelcomePage>();
        builder.Services.AddTransient<BasicSignInPage>();
        builder.Services.AddTransient<AccountCreationPage>();
        builder.Services.AddTransient<GoogleSignInPage>();
        builder.Services.AddTransient<ResetPasswordPage>();
        
        // Recipes related pages
        builder.Services.AddTransient<RecipesBrowserPage>();

        // Registering view models here (for dependency injection)
        builder.Services.AddTransient<WelcomePageViewModel>();
        builder.Services.AddTransient<BasicSignInPageViewModel>();
        builder.Services.AddTransient<ResetPasswordPageViewModel>();
        builder.Services.AddTransient<GoogleSignInPageViewModel>();
        builder.Services.AddTransient<AccountCreationPageViewModel>();

#if __ANDROID__
        builder.Services.AddTransient<HttpClient, PlatformHttpClient>(client =>
        {
            var sha1 = PackageUtils.SigToGoogleFormat(PackageUtils.GetPackageDefaultSignature()!);
            var pkgname = PackageUtils.GetPackageName();
            return new PlatformHttpClient(sha1, pkgname);
        });
#endif

        // Registering services here (for dependency injection)
        builder.Services.AddSingleton<IAuthConfigProvider, LocalAuthConfigProvider>();
        builder.Services.AddSingleton<IAuthenticationService, FirebaseAuthenticationService>();
        builder.Services.AddSingleton<ISecureStorageService, SecureStorageService>();
        builder.Services.AddSingleton<IGoogleAccountManager, GoogleAccountManager>();
        builder.Services.AddSingleton<IGuestAuthService, GuestAuthService>();

        // Configuration providers
        builder.Services.AddSingleton<ConfigProvider>();

        //builder.Logging.AddTraceLogger(options => { options.MinLevel = LogLevel.Information; });
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}