using DruidsCornerApp.ViewModels.Login;

namespace DruidsCornerApp.Views.Login;

public partial class ResetPasswordPage : AutoReloadPage
{
    public ResetPasswordPage(ResetPasswordPageViewModel viewModel)
    {
        BindingContext = viewModel;
    }

    public override void Build()
    {
        InitializeComponent();
    }
}