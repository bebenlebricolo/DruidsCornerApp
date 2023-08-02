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

    protected Account? PickGoogleAccount()
    {
        // Trying with the local account method now
        var accountManager = AccountManager.Get(Microsoft.Maui.ApplicationModel.Platform.AppContext);
        if (accountManager == null)
        {
            return null;
        }

        return accountManager.GetAccountsByType("com.google").ToList().FirstOrDefault<Account>();
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
        var localAccount = PickGoogleAccount();
        return localAccount;
    }

    public async partial Task<GoogleAccount?> GetCurrentGoogleAccountAsync(CancellationToken cancellationToken)
    {
        GoogleAccount? account = null;
        CheckAndRequestPermissions();
        try
        {
            
#if false
            GoogleSignInAccount? gAccount = GoogleSignIn.GetLastSignedInAccount(Microsoft.Maui.ApplicationModel.Platform.AppContext);
            if (gAccount != null)
            {
                account = ConvertAccountFrom(gAccount);
                return account;
            }
#endif

            // Just pick the local one if it has already been selected
            var localAccount = PickGoogleAccount();
            if (localAccount == null || string.IsNullOrEmpty(localAccount.Name))
            {
                localAccount = await LetUserChooseAccountAsync(cancellationToken);
            }

            // Well, now we don't have other solutions than to 
            // Request the user to enter its account credentials again !
            if (localAccount == null)
            {
                return null;
            }

            // Minimum common denominator is the account name (...)
            // Todo : use GoogleSignIn / PlayServices apis to retrieve a bit more data for this account.
            // This way you won’t get a consent screen  
            var mainActivity =  (MainActivity) Platform.CurrentActivity!;
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

            await mainActivity.WaitForSignInAsync(cancellationToken);
            var googleAccount = mainActivity.GoogleAccount;
            if (googleAccount != null)
            {
                account = ConvertAccountFrom(googleAccount);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Exception caught while retrieving google account : {ex.Message}");
        }

        return account;
    }
}