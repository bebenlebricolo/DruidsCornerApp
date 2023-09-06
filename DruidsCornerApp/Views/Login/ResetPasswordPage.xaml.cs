using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage(ResetPasswordPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}