namespace DruidsCornerApp.Views;
using DruidsCornerApp.ViewModels;

public partial class WelcomePage : ContentPage
{
	private WelcomePageViewModel _viewModel;
	public WelcomePage(WelcomePageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}
    
}

