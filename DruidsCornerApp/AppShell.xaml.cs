using CommunityToolkit.Mvvm.ComponentModel;
using DruidsCornerApp.Models;
using DruidsCornerApp.Services;
using DruidsCornerApp.Views;
using DruidsCornerApp.Views.MainContext;

namespace DruidsCornerApp;
using DruidsCornerApp.Views.Login;
using DruidsCornerApp.Utils;

public partial class AppShell : Shell
{
	private readonly ISecureStorageService _storageService;
	
	public AppShell(ISecureStorageService storageService)
	{
		InitializeComponent();
		// Routing.RegisterRoute(Navigator.GetWelcomePageRoute(), typeof(WelcomePage));
		// Routing.RegisterRoute(Navigator.GetBasicSignInPageRoute(), typeof(BasicSignInPage));
		// Routing.RegisterRoute(Navigator.GetAccountCreationPageRoute(), typeof(AccountCreationPage));
		// Routing.RegisterRoute(Navigator.GetGoogleSignInPageRoute(), typeof(GoogleSignInPage));
		// Routing.RegisterRoute(Navigator.GetAccountPasswordResetPageRoute(), typeof(ResetPasswordPage));
		//
		// // Recipe-related pages
		// Routing.RegisterRoute(Navigator.GetRecipesBrowserPageRoute(), typeof(RecipeExplorerPage));

		_storageService = storageService;
	}

	protected override async void OnAppearing()
	{
		var firstStart = await _storageService.GetAsync(GlobalApplicationStates.AppFirstBootKey);
		if(firstStart == null  || bool.Parse(firstStart))
		{
			// Now we can skip this check next time
			await _storageService.StoreAsync(GlobalApplicationStates.AppFirstBootKey, false.ToString());
		}
		//await GoToAsync(Navigator.GetRecipesBrowserPageRoute());
		await GoToAsync("///Explore");
	}
}
