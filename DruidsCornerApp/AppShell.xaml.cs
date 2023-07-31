using CommunityToolkit.Mvvm.ComponentModel;

namespace DruidsCornerApp;
using DruidsCornerApp.Views.Login;
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
		// Routing.RegisterRoute(Navigator.GetWelcomePageRoute(), typeof(WelcomePage));
		Routing.RegisterRoute(Navigator.GetBasicSignInPageRoute(), typeof(BasicSignInPage));
		Routing.RegisterRoute(Navigator.GetAccountCreationPageRoute(), typeof(AccountCreationPage));
		Routing.RegisterRoute(Navigator.GetGoogleSignInPageRoute(), typeof(GoogleSignInPage));
		Routing.RegisterRoute(Navigator.GetAccountPasswordResetPageRoute(), typeof(ResetPasswordPage));
    }
}
