using DruidsCornerApp.Controls;
using DruidsCornerApp.Models.Google;
using DruidsCornerApp.Services.Authentication;
using DruidsCornerApp.Utils;
using DruidsCornerApp.Views.Login;

namespace DruidsCornerApp.ViewModels.Login;

using Microsoft.Extensions.Logging;
using DruidsCornerApp.Services;

public class GoogleSignInEventArgs : EventArgs
{
    public GoogleAccount? account = null;
};

public partial class GoogleSignInPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly IGoogleAccountManager _googleAccountManager;
    private readonly ILogger<GoogleSignInPageViewModel> _logger;
    
    private Tuple<Task, LoginPopup>? _loadingPopup = null;
    private Task<GoogleAccount?>? _signInTask = null;

    public delegate void StatusUpdateHandler(object sender, GoogleSignInEventArgs e);

    public event StatusUpdateHandler OnSignInFinished;

    
    /// <summary>
    /// GoogleSignInPage view model, drives how the page responds.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
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

    public async void StartPopulatingUi()
    {
        // Start processing ...
        _loadingPopup = PopupUtils.CreateAndShowLoggingPopupAsync(Title);
        _signInTask = _googleAccountManager.GetCurrentGoogleAccountAsync(CancellationToken.None);
        
        var account = await _signInTask;
        PopupUtils.SetLoginPopupCompletedTask(_loadingPopup.Item2, "Accounts retrieved");
        await Task.Delay(500);
        await _loadingPopup.Item2.Close();

        // Notify the UI that we now can add the new account as a button
        OnSignInFinished(this, new GoogleSignInEventArgs() { account = account });
    }
}