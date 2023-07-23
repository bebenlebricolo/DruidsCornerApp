using CommunityToolkit.Maui;
using DruidsCornerApp.Services;
using DruidsCornerApp.ViewModels;
using DruidsCornerApp.Views;
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
		builder.Services.AddSingleton<LoginPage>();
		builder.Services.AddSingleton<AccountCreationPage>();
		
		// Registering view models here (for dependency injection)
		builder.Services.AddSingleton<LoginPageViewModel>();
		builder.Services.AddSingleton<AccountCreationPageViewModel>();
		
		// Registering services here (for dependency injection)
		builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
		builder.Services.AddScoped<ISecureStorageService, SecureStorageService>();

		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
