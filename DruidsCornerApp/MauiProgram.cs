using CommunityToolkit.Maui;
using DruidsCornerApp.Services;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Utils;
using DruidsCornerApp.ViewModels;
using DruidsCornerApp.ViewModels.Login;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.Login;
using MetroLog.MicrosoftExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
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
// #if __ANDROID__
//             .ConfigureLifecycleEvents(events => 
//             {
//                 events.AddAndroid(android =>
//                                   {
//                                       return android
//                                              .OnActivityResult((activity, requestCode, resultCode, data) =>
//                                              {
//                                                  MainActivity.HandleGoogleSignInEvent(requestCode, resultCode, data); 
//                                              })
//                                              .OnStart((activity) => LogEvent(nameof(AndroidLifecycle.OnStart)))
//                                              .OnCreate((activity, bundle) => LogEvent(nameof(AndroidLifecycle.OnCreate)))
//                                              .OnBackPressed((activity) => LogEvent(nameof(AndroidLifecycle.OnBackPressed)) && false)
//                                              .OnStop((activity) => LogEvent(nameof(AndroidLifecycle.OnStop)));
//                                   }
//                                  );
//             })
// #endif
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

        builder.Logging.AddTraceLogger(options => { options.MinLevel = LogLevel.Information; });
#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}