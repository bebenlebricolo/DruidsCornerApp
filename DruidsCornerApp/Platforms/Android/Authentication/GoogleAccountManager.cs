using System.Diagnostics;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using DruidsCornerApp.Models;
using DruidsCornerApp.Models.Google;
using DruidsCornerApp.Platforms.Android.Models;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.Authentication;

using Android.App;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using Android;
using Android.Content.PM;
using Android.Util;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Android.Accounts;
using Android.Gms.Auth.Api.SignIn;

public class ActivityResultCallback : Java.Lang.Object, IActivityResultCallback
{
    readonly Action<ActivityResult> _callback;
    public ActivityResultCallback(Action<ActivityResult> callback) => _callback = callback;
    public ActivityResultCallback(TaskCompletionSource<ActivityResult> tcs) => _callback = tcs.SetResult;
    public void OnActivityResult(Java.Lang.Object? p0) => _callback((ActivityResult)p0!);
}

public partial class GoogleAccountManager : IGoogleAccountManager
{
    protected void CheckAndRequestPermissions()
    {
        var context = Microsoft.Maui.ApplicationModel.Platform.AppContext;
        var activity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;

        if (activity == null)
        {
            return;
        }

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
    }

    public static GoogleAccount ConvertAccountFrom(GoogleSignInAccount account)
    {
        return new GoogleAccount
        {
            PhotoUrl = account.PhotoUrl.ToString(),
            Email = account.Email,
            Id = account.Id,
            FullName = account.DisplayName,
            UserName = account.GivenName
        };
    }

    protected List<Account> ListAvailableLocalGoogleAccounts()
    {
        var outList = new List<Account>();
        // Trying with the local account method now
        var accountManager = AccountManager.Get(Microsoft.Maui.ApplicationModel.Platform.AppContext);
        if (accountManager != null)
        {
            outList =  accountManager.GetAccountsByType("com.google").ToList();
        }

        return outList;
    }

    private void StartGoogleAccountsListingActivity(MainActivity mainActivity)
    {
        try
        {
            // Configure sign-in to request the user's ID, email address, and basic
            // profile. ID and basic profile are included in DEFAULT_SIGN_IN.
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                      .RequestEmail()
                                      .RequestProfile()
                                      .Build();

            // Build a GoogleSignInClient with the options specified by gso.
            // We need the main activity to provide the Google Sign In Client (don't know why ...)
            GoogleSignInClient client = GoogleSignIn.GetClient(Platform.CurrentActivity, gso);
            Intent signInIntent = client.SignInIntent;
                
            mainActivity.StartActivityForResult(signInIntent, (int) CustomCodes.GoogleSignIn);
        }
        catch (Exception ex)
        {
            // Uh-oh!
            _logger.LogError($"{ex.Message}");
        }
    }
    
    
    protected async Task<Account?> LetUserChooseAccountAsync(CancellationToken cancellationToken)
    {
        // Starts a new Android activity that lets the user choose the account he/she wants to use with the app.
#pragma warning disable CA1416
        var intent = AccountManager.NewChooseAccountIntent(null,
                                                           null,
                                                           new[] { "com.google" },
                                                           null,
                                                           null,
                                                           null,
                                                           null
                                                          );
#pragma warning restore CA1416


        // Ask the user to select an account
        MainActivity currentActivity = (MainActivity) Platform.CurrentActivity!;
        
        currentActivity.StartActivityForResult(intent, (int) CustomCodes.ChooseGoogleAccount);
        await currentActivity.WaitForAccountPickupAsync(cancellationToken);
        
        // When ready, pick the first available account
        var localAccount = ListAvailableLocalGoogleAccounts();
        return localAccount.FirstOrDefault();
    }

    public async partial Task<List<GoogleAccount>> ListGoogleAccountsOnDeviceAsync(CancellationToken cancellationToken)
    {
        List<GoogleAccount> outList = new List<GoogleAccount>();
        CheckAndRequestPermissions();
        try
        {
#if false
            GoogleSignInAccount? gAccount = GoogleSignIn.GetLastSignedInAccount(Microsoft.Maui.ApplicationModel.Platform.AppContext);
            if (gAccount != null)
            {
                account = ConvertAccountFrom(gAccount);
                outList.Add(account);
                return outList;
            }
#endif

            // Just pick the local one if it has already been selected
            var localAccounts = ListAvailableLocalGoogleAccounts();
            if (localAccounts.Count == 0)
            {
                var pickedAccount = await LetUserChooseAccountAsync(cancellationToken);
                if (pickedAccount == null)
                {
                    // Uh-oh ! No account retrieved from this method !
                    _logger.LogError("Could not retrieve user account from user interaction !");
                    return new List<GoogleAccount>();
                }
            }

            // Well, now we don't have other solutions than to 
            // Request the user to enter its account credentials again !
            if (localAccounts.Count == 0)
            {
                // return the empty list
                return outList;
            }

            
            // Start the Google Sign In Activity and retrieve its result asynchronously
            var mainActivity =  (MainActivity) Platform.CurrentActivity!;
            StartGoogleAccountsListingActivity(mainActivity);
            await mainActivity.WaitForAccountListingFinishedAsync(cancellationToken);            
            
            var googleAccount = mainActivity.GoogleAccount;
            if (googleAccount != null)
            {
                outList.Add(ConvertAccountFrom(googleAccount));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught while retrieving google account : {ex.Message}");
        }

        return outList;
    }
}