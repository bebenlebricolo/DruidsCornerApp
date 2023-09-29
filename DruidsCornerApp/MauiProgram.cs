using CommunityToolkit.Maui;
using DruidsCornerApiClient.Services;
using DruidsCornerApiClient.Services.Interfaces;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Services.Config;
using DruidsCornerApp.Utils;
using DruidsCornerApp.ViewModels;
using DruidsCornerApp.ViewModels.Login;
using DruidsCornerApp.ViewModels.MainContext;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.Login;
using DruidsCornerApp.Views.MainContext;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Sharpnado.Tabs;

namespace DruidsCornerApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .UseSharpnadoTabs(true)
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
        builder.Services.AddTransient<RecipeExplorerPage>();
        builder.Services.AddTransient<HopReferenceView>();
        builder.Services.AddTransient<ReferencesPage>();

        // Registering view models here (for dependency injection)
        builder.Services.AddTransient<DefaultPageViewModel>();
        builder.Services.AddTransient<BasicSignInPageViewModel>();
        builder.Services.AddTransient<ResetPasswordPageViewModel>();
        builder.Services.AddTransient<GoogleSignInPageViewModel>();
        builder.Services.AddTransient<AccountCreationPageViewModel>();
        
        builder.Services.AddTransient<RecipeExplorerViewModel>();
        builder.Services.AddTransient<ReferencesPageViewModel>();
        builder.Services.AddTransient<HopReferenceViewModel>();
        
        builder.Services.AddTransient<MainClient>(service =>
        {
            var configProvider = service.GetService<ConfigProvider>();
            var clientConfiguration = Task.Run(async () => await configProvider!.GetConfigAsync()).Result;
            var logger = service.GetService<ILogger<BaseClient>>();
            return new MainClient(logger!, clientConfiguration!, new HttpClient());
        });
        
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