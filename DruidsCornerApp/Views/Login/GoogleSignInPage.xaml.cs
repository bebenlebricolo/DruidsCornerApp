using DruidsCornerApp.Models.Google;
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
        _viewModel.OnAccountListingFinished += GoogleAccountsListingHandler;
    }


    public void GoogleAccountsListingHandler(object sender, GoogleSignInEventArgs payload)
    {
        foreach (var account in payload.GoogleAccounts)
        {
            CardsLayout.Add(GoogleSignInPageViewModel.BuildGoogleAccountCard(account));
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = _viewModel.StartGoogleAccountListing();
        
        // Populate the UI with an activity indicator ...
    }

    public void ClearCardsList()
    {
        //CardsLayout.Children.Clear();
    }
}