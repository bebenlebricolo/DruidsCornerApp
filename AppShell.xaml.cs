using DruidsCornerApp.Controls;
using DruidsCornerApp.Views;

namespace DruidsCornerApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

		// New custom entry mappings
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(OutlinedEntry), (handler, view) =>
		{
#if __ANDROID__
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
		});
		
	}
}
