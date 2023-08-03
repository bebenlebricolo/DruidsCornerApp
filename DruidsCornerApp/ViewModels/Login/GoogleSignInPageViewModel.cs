using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DruidsCornerApp.Controls;
using DruidsCornerApp.Models.Google;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Utils;
using DruidsCornerApp.Views.Login;

namespace DruidsCornerApp.ViewModels.Login;

using Microsoft.Extensions.Logging;
using DruidsCornerApp.Services;

/// <summary>
/// Event arguments whenever the Google accounts Listing finished
/// </summary>
public class GoogleSignInEventArgs : EventArgs
{
    /// <summary>
    /// The list of google accounts retrieved on this device
    /// </summary>
    public List<GoogleAccount> GoogleAccounts = new List<GoogleAccount>();
};

public partial class GoogleSignInPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly IGoogleAccountManager _googleAccountManager;
    private readonly ILogger<GoogleSignInPageViewModel> _logger;

    public ObservableCollection<Frame> GoogleAccountsCards { get; private set; } = new ObservableCollection<Frame>();
    //private Tuple<Task, LoginPopup>? _loadingPopup = null;
    
    /// <summary>
    /// Custom event delegate that'll be user to notify the UI
    /// that new objects need to populate the page
    /// -> Needs to be implemented (and registered) by the UI upon creation
    /// </summary>
    public delegate void GoogleAccountListingUpdateHandler(object sender, GoogleSignInEventArgs e);

    /// <summary>
    /// Custom event handler that'll notify listeners that the AccountListing operation os over
    /// </summary>
    public event GoogleAccountListingUpdateHandler? OnAccountListingFinished;


    /// <summary>
    /// GoogleSignInPage view model, drives how the page responds.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    /// <param name="googleAccountManager"></param>
    public GoogleSignInPageViewModel(ILogger<GoogleSignInPageViewModel> logger,
                                     IAuthenticationService authenticationService,
                                     ISecureStorageService secureStorageService,
                                     IGoogleAccountManager googleAccountManager
    ) : base("SignIn with Google", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
        _googleAccountManager = googleAccountManager;
    }
    
    public async Task SignInWithGoogleOauth()
    {
        try
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(new Uri("https://mysite.com/mobileauth/Microsoft"),
                                                                                                 new Uri("myapp://")
                                                                                                );

            string accessToken = authResult?.AccessToken ?? "";
            // Do something with the token
        }
        catch (TaskCanceledException e)
        {
            // Use stopped auth
            _logger.LogError(e.Message);
        }
    }

    [RelayCommand]
    public async Task StartGoogleAccountListing()
    {
        // Prevent multiple calls when the first one didn't complete
        if (IsLoading)
        {
            return;
        }
        
        IsLoading = true;
        // Start processing ...
        //_loadingPopup = PopupUtils.CreateAndShowLoggingPopupAsync(Title);
        var listingTask = _googleAccountManager.ListGoogleAccountsOnDeviceAsync(CancellationToken.None);
        var accounts = await listingTask;
        
        //PopupUtils.SetLoginPopupCompletedTask(_loadingPopup.Item2, "Accounts retrieved");
        //await Task.Delay(500);
        //await _loadingPopup.Item2.Close();

        // Notify the UI that we now can add the new account as a button
        OnAccountListingFinished!(this, new GoogleSignInEventArgs() { GoogleAccounts = accounts });
        
        // Fill in the account cards
        // Should update the UI accordingly
        GoogleAccountsCards.Clear();
        foreach (var account in accounts)
        {
            var card = BuildGoogleAccountCard(account);
            GoogleAccountsCards.Add(card);
        }
        IsLoading = false;
    }
    
#region CardBuilders
    public static Image BuildUserPicture(GoogleAccount account)
    {
        var userPicture = new Image()
        {
            HeightRequest = 80,
            WidthRequest = 80
        };
        
        var photoUrl = account.PhotoUrl;
        if (photoUrl != null)
        {
            userPicture.Source = ImageSource.FromUri(new Uri(photoUrl));
        }
        else
        {
            // Default picture for now
            userPicture.Source = ImageSource.FromFile("dotnet_bot.svg");
        }

        return userPicture;
    }

    public static Frame BuildUserPictureFrame(Image userPicture)
    {
        var imageFrame = new Frame()
        {
            Content = userPicture,
            CornerRadius = 15,
            Padding = new Thickness(0,0),
            Margin = new Thickness(0,0),
            IsClippedToBounds = true,
            Background = null,
            BorderColor = Colors.Transparent,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Start,
        };

        return imageFrame;
    }

    public static Label BuildUserNameLabel(GoogleAccount account)
    {
        return new Label()
        {
            Text = account.FullName,
            FontSize = 20,
            VerticalOptions = LayoutOptions.Center
        };
    }
    
    public static Label BuildEmailLabel(GoogleAccount account)
    {
        return new Label()
        {
            Text = account.Email,
            FontSize = 10,
            VerticalOptions = LayoutOptions.Center
        };
    }

    public static VerticalStackLayout BuildLabelStackLayout(Label nameLabel, Label emailLabel)
    {
        return new VerticalStackLayout()
        {
            Children =
            {
                nameLabel,
                emailLabel
            },
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Center
        };
    }

    public static Grid BuildGoogleAccountGrid(Frame imageFrame, VerticalStackLayout labelStackLayout)
    {
        var grid = new Grid()
        {
            ColumnDefinitions = new ColumnDefinitionCollection(new[]
                                                               {
                                                                   new ColumnDefinition(GridLength.Auto),
                                                                   new ColumnDefinition(GridLength.Star)
                                                               }
                                                              ),
            ColumnSpacing = 30,
            Margin = new Thickness(0,0),
            Padding = new Thickness(0,0)
        };
                
        // Adding the elements inside
        grid.Add(imageFrame, 0,0);
        grid.Add(labelStackLayout, 1,0);

        return grid;
    }

    public static Frame BuildGoogleAccountCard(GoogleAccount account)
    {
        var userPicture = BuildUserPicture(account);
        var imageFrame = BuildUserPictureFrame(userPicture);
        var nameLabel = BuildUserNameLabel(account);
        var emailLabel = BuildEmailLabel(account);
        var labelStackLayout = BuildLabelStackLayout(nameLabel, emailLabel);
        var grid = BuildGoogleAccountGrid(imageFrame, labelStackLayout);
               
        // Put everyone into a nice frame
        var buttonFrame = new Frame()
        {
            CornerRadius = 10,
            BackgroundColor = Colors.White,
            BorderColor = Colors.DeepSkyBlue,
            Content = grid,
            Margin = new Thickness(0,0),
            Padding = new Thickness(10,10)
        };
        return buttonFrame;
    }
    
#endregion CardBuilders

    
    
    
}