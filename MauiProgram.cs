using CommunityToolkit.Maui;
using DruidsCornerApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiCommunityToolkit()
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Registering new services / view models here (for dependency injection)
		builder.Services.AddSingleton<LoginPageViewModel>();
		
#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
