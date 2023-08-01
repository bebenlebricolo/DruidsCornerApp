namespace DruidsCornerApp.Views;
using DruidsCornerApp.ViewModels;

public partial class WelcomePage : AutoReloadPage
{
    private WelcomePageViewModel _viewModel;
    
    public WelcomePage(WelcomePageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public override void Build()
    {
        InitializeComponent();
    }
    
}

