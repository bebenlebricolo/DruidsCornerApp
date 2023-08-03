using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Extensions;
using Android.OS;
using DruidsCornerApp.Models;
using DruidsCornerApp.Platforms.Android.Models;

[Activity(Theme = "@style/Maui.SplashTheme",
          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout |
                                 ConfigChanges.SmallestScreenSize | ConfigChanges.Density
         )]
public class GoogleSignInActivity : MauiAppCompatActivity
{
    private bool _running = false;
    public GoogleSignInAccount? GoogleAccount = null;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        StartSignInProcess();
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
    {
        base.OnActivityResult(requestCode, resultCode, data);
      
        _running = false;
        // See if we can get something here
    }

    public void StartSignInProcess()
    {
        _running = true;
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

            var componentActivity = (AndroidX.Activity.ComponentActivity)this;
            componentActivity.StartActivityForResult(signInIntent, (int) CustomCodes.GoogleSignIn);
        }
        catch (Exception ex)
        {
            
            _running = false;
        }
    }

    public async Task WaitForSignInAsync(CancellationToken cancellationToken)
    {
        while (_running && !cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(500, cancellationToken);
        }
    }
}