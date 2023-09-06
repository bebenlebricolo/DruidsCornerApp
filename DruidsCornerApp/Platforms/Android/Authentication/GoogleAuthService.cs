using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using DruidsCornerApp.Models.Google;
using DruidsCornerApp.Platforms.Android.Models;

namespace DruidsCornerApp.Services.Authentication;

public partial class GoogleAuthService : IGoogleAuthService
{
    private MainActivity _activity;
    private GoogleSignInClient _client;

    public GoogleAuthService(string refClientId, List<string>? customScopes = null)
    {
        // Get current activity
        _activity = (MainActivity?)Platform.CurrentActivity!;
        
        // Config Auth Option
        var gsoBuilder = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                         .RequestIdToken(refClientId)
                         .RequestEmail()
                         .RequestId()
                         .RequestProfile();
        
        // Process scopes now
        if (customScopes?.Count > 0)
        {
            foreach (var scope in customScopes)
            {
                gsoBuilder.RequestScopes(new Scope(scope));
            }
        }
        
        var gso = gsoBuilder.Build();
        
        // Get client
        _client = GoogleSignIn.GetClient(_activity, gso);
    }

    public async partial Task<GoogleAccount?> AuthenticateAsync(CancellationToken cancellationToken)
    {
        _activity.StartActivityForResult(_client.SignInIntent, (int) CustomCodes.GoogleSignInScoped);
        _activity.PendingGoogleAccountSignin = true;
        await _activity.WaitForAccountListingFinishedAsync(cancellationToken);

        if (_activity.GoogleAccount != null)
        {
            return GoogleAccountManager.ConvertAccountFrom(_activity.GoogleAccount);
        }
        return null;
    }

    public async partial Task LogoutAsync(CancellationToken cancellationToken)
    {
        await _client.SignOutAsync();
        
        // Forget the account now
        _activity.GoogleAccount = null;
    }

    public async partial Task<GoogleAccount?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var account = await _client.SilentSignInAsync();
        if (account != null)
        {
            return GoogleAccountManager.ConvertAccountFrom(account);
        }
        
        // Forget the account now
        _activity.GoogleAccount = null;
        return null;
    }
}