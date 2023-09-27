using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.MainContext;

namespace DruidsCornerApp;
using DruidsCornerApp.Views.Login;
using DruidsCornerApp.Utils;

public partial class BootShell : Shell
{
	public BootShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(Navigator.GetBasicSignInPageRoute(), typeof(BasicSignInPage));
		Routing.RegisterRoute(Navigator.GetAccountCreationPageRoute(), typeof(AccountCreationPage));
		Routing.RegisterRoute(Navigator.GetGoogleSignInPageRoute(), typeof(GoogleSignInPage));
		Routing.RegisterRoute(Navigator.GetAccountPasswordResetPageRoute(), typeof(ResetPasswordPage));
    }
}
