using DruidsCornerApp.Views;

namespace DruidsCornerApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private async void OnStartBrowsingClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(LoginPage));
	}
}

