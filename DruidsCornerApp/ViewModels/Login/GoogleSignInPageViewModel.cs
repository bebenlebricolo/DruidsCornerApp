using DruidsCornerApp.Models.Google;
using Google.Apis.Auth.OAuth2;

namespace DruidsCornerApp.ViewModels.Login;

using Microsoft.Extensions.Logging;
using DruidsCornerApp.Services;

#if __ANDROID__
using Android;
using Android.Content.PM;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Accounts;
using Android.Gms.Auth.Api.SignIn;
#endif

public partial class GoogleSignInPageViewModel : BaseViewModel
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ISecureStorageService _secureStorageService;
    private readonly ILogger<GoogleSignInPageViewModel> _logger;

    /// <summary>
    /// GoogleSignInPage view model, drives how the page responds.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="authenticationService"></param>
    /// <param name="secureStorageService"></param>
    public GoogleSignInPageViewModel(ILogger<GoogleSignInPageViewModel> logger,
                                     IAuthenticationService authenticationService,
                                     ISecureStorageService secureStorageService
    ) : base("SignIn with Google", false)
    {
        _logger = logger;
        _authenticationService = authenticationService;
        _secureStorageService = secureStorageService;
    }

#if __ANDROID__
    public void CheckPermissions()
    {
        var context = Microsoft.Maui.ApplicationModel.Platform.AppContext;
        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;

        var showAuthenticateAccountsPermission = ContextCompat.CheckSelfPermission(context, Manifest.Permission.AuthenticateAccounts);
        var showRegularAccountsPermission = ContextCompat.CheckSelfPermission(context, Manifest.Permission.GetAccounts);

        // Request permissions
        if (showAuthenticateAccountsPermission != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.AuthenticateAccounts }, 1);
        }

        if (showRegularAccountsPermission != Permission.Granted)
        {
            ActivityCompat.RequestPermissions(activity, new[] { Manifest.Permission.GetAccounts }, 1);
        }

        //do some stuff
        var emails = new List<string>();

        var gmailPattern = Patterns.EmailAddress;

#pragma warning disable CA1416
        var intent = AccountManager.NewChooseAccountIntent(null,
                                                           null,
                                                           new[] { "com.google" },
                                                           null,
                                                           null,
                                                           null,
                                                           null
                                                          );

        // Ask the user to select an account 
        Microsoft.Maui.ApplicationModel.Platform.CurrentActivity!.StartActivityForResult(intent, 1);
    }
#endif

    public GoogleAccount? GetCurrentGoogleAccount()
    {
        GoogleAccount? account = null;
#if __ANDROID__
        CheckPermissions();
        try
        {

            GoogleSignInAccount? gAccount = GoogleSignIn.GetLastSignedInAccount(Microsoft.Maui.ApplicationModel.Platform.AppContext);
            if (gAccount != null)
            {
                account = new GoogleAccount()
                {
                    Email = gAccount.Email,
                    Id = gAccount.Id + "",
                    FullName = gAccount.DisplayName,
                    UserName = gAccount.GivenName
                };
                return account;
            }
            
            // Trying with the local account method now
            var accountManager = AccountManager.Get(Microsoft.Maui.ApplicationModel.Platform.AppContext);
            if (accountManager == null)
            {
                return null;
            }
            
            var localAccounts = accountManager.GetAccountsByType("com.google");
            if (localAccounts.Length != 0)
            {
                var acc = localAccounts.First();
                if (acc.Name == null)
                {
                    return null;
                }
                
                // Minimum common denominator is the account name (...)
                account = new GoogleAccount()
                {
                    Email = acc.Name
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught while retrieving google account : {ex.Message}");
        }
#endif
        return account;
    }

    public async Task SignInWithGoogle()
    {
        try
        {
            WebAuthenticatorResult authResult = await WebAuthenticator.Default.AuthenticateAsync(new Uri("https://mysite.com/mobileauth/Microsoft"),
                                                                                                 new Uri("myapp://")
                                                                                                );

            string accessToken = authResult?.AccessToken;

            // Do something with the token
        }
        catch (TaskCanceledException e)
        {
            // Use stopped auth
        }
    }
}