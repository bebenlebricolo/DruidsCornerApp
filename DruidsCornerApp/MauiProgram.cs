using CommunityToolkit.Maui;
using DruidsCornerApp.Services;
using DruidsCornerApp.ViewModels;
using DruidsCornerApp.ViewModels.Login;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.Login;
using MetroLog.MicrosoftExtensions;
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
		builder.Services.AddSingleton<WelcomePage>();
		builder.Services.AddSingleton<BasicSignInPage>();
		builder.Services.AddSingleton<AccountCreationPage>();
		builder.Services.AddSingleton<GoogleSignInPage>();
		
		// Registering view models here (for dependency injection)
		builder.Services.AddSingleton<BasicSignInPageViewModel>();
		builder.Services.AddSingleton<GoogleSignInPageViewModel>();
		builder.Services.AddSingleton<AccountCreationPageViewModel>();
		builder.Services.AddSingleton<WelcomePageViewModel>();
		
		// Registering services here (for dependency injection)
		builder.Services.AddSingleton<IAuthConfigProvider, LocalAuthConfigProvider>();
		builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
		builder.Services.AddScoped<ISecureStorageService, SecureStorageService>();

		builder.Logging.AddTraceLogger(options =>
		{
			options.MinLevel = LogLevel.Information;
		});
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
