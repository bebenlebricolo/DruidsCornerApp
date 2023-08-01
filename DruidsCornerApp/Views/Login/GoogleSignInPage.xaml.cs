using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class GoogleSignInPage : ContentPage
{
    private readonly GoogleSignInPageViewModel _viewModel;
    
    public GoogleSignInPage(GoogleSignInPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        var account = _viewModel.GetCurrentGoogleAccount();
        if (account != null)
        {
            CurrentAccountLabel.Text = account.Email;
        }
    }
}