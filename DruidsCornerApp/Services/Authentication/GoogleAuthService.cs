using DruidsCornerApp.Models.Google;

namespace DruidsCornerApp.Services.Authentication;

public partial class GoogleAuthService : IGoogleAuthService
{
#if __ANDROID__
    public partial Task<GoogleAccount?> AuthenticateAsync(CancellationToken cancellationToken);

    public partial Task LogoutAsync(CancellationToken cancellationToken);

    public partial Task<GoogleAccount?> GetCurrentUserAsync(CancellationToken cancellationToken);
#else

    public GoogleAuthService(string refClientId, List<string>? customScopes = null)
    {
    }

    public Task<GoogleAccount?> AuthenticateAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<GoogleAccount?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
#endif
}