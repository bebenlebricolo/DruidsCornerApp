using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class GoogleSignInPage : ContentPage
{
    public GoogleSignInPage(GoogleSignInPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}