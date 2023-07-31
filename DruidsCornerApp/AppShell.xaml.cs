using CommunityToolkit.Mvvm.ComponentModel;

namespace DruidsCornerApp;
using DruidsCornerApp.Views;
using DruidsCornerApp.Utils;

public partial class AppShell : Shell
{
	public string StartupPage
	{
		get => Navigator.GetWelcomePageRoute();
	}

	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(Navigator.GetBasicSignInPageRoute(), typeof(BasicSignInPage));
		Routing.RegisterRoute(Navigator.GetAccountCreationPageRoute(), typeof(AccountCreationPage));
		Routing.RegisterRoute(Navigator.GetGoogleSignInPageRoute(), typeof(GoogleSignInPage));
	}
}
