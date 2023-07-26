using DruidsCornerApp.Views;
using DruidsCornerApp.Controls.Entries;

namespace DruidsCornerApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		Routing.RegisterRoute(nameof(AccountCreationPage), typeof(AccountCreationPage));

		// New custom entry mappings
		Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(OutlinedEntry), (handler, view) =>
		{
#if __ANDROID__
			handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
		});
		
	}
}
