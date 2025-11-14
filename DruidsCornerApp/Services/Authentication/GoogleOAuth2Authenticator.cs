namespace DruidsCornerApp.Services.Authentication;

public class GoogleOAuth2Authenticator : IWebAuthenticator
{
    public Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions)
    {
        throw new NotImplementedException();
    }

    public Task<WebAuthenticatorResult> AuthenticateAsync(WebAuthenticatorOptions webAuthenticatorOptions, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
