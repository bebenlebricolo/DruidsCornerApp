using Android.Accounts;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using DruidsCornerApp.Models;
using DruidsCornerApp.Models.Google;
using DruidsCornerApp.Platforms.Android.Models;
using CancellationToken = System.Threading.CancellationToken;
using Task = System.Threading.Tasks.Task;

[Activity(Theme = "@style/Maui.SplashTheme",
          //Theme = "@style/Maui.MainTheme.NoActionBar",
          MainLauncher = true,
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
                                 ConfigChanges.SmallestScreenSize | ConfigChanges.Density
         )]
public class MainActivity : MauiAppCompatActivity
{
    public GoogleSignInAccount? GoogleAccount = null;
    public bool PendingLocalAccount = false;

    public void HandleGoogleSignInEvent(Result resultCode, Intent? data)
    {

        if (resultCode == Result.Ok)
        {
            // This task is already finished (that's why, per the documentation https://developers.google.com/android/reference/com/google/android/gms/auth/api/signin/GoogleSignIn#public-static-taskgooglesigninaccount-getsignedinaccountfromintent-intent-data
            // we should directly invoke the GetResult() from the returned Java task.
            // Otherwise the system freezes.
            GoogleSignInAccount googleSignInAccount =
                (GoogleSignInAccount) GoogleSignIn.GetSignedInAccountFromIntent(data).GetResult(Java.Lang.Class.FromType(typeof(ApiException)));
            GoogleAccount = googleSignInAccount;
        }
    }

    public void HandleAccountPickupEvent(Result resultCode, Intent? data)
    {
        if (resultCode == Result.Ok)
        {
            PendingLocalAccount = false;
        }
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        switch (requestCode)
        {
            case (int)CustomCodes.GoogleSignIn:
                HandleGoogleSignInEvent(resultCode, data);
                break;

            case (int)CustomCodes.ChooseGoogleAccount:
                break;

            default:
                // Nothing to do yet
                break;
        }

        base.OnActivityResult(requestCode, resultCode, data);
    }


    public async Task WaitForAccountListingFinishedAsync(CancellationToken cancellationToken)
    {
        while (GoogleAccount == null && !cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500, cancellationToken);
        }
    }

    public async Task WaitForAccountPickupAsync(CancellationToken cancellationToken)
    {
        while (PendingLocalAccount == true && !cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500, cancellationToken);
        }
    }
}