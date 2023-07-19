using DruidsCornerApp.Views;

namespace DruidsCornerApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

	}
}
