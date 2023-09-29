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
