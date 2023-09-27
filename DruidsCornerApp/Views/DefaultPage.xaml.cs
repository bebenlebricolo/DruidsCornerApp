namespace DruidsCornerApp.Views;
using DruidsCornerApp.ViewModels;

public partial class WelcomePage : ContentPage
{
	private DefaultPageViewModel _viewModel;
	public WelcomePage(DefaultPageViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
	}
    
}

