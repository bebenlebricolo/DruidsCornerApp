using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class GoogleSignInPage : AutoReloadPage
{
    private readonly GoogleSignInPageViewModel _viewModel;
    
    public GoogleSignInPage(GoogleSignInPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    public override void Build()
    {
        InitializeComponent();
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