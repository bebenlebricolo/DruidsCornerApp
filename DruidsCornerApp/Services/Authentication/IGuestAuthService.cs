namespace DruidsCornerApp.Services.Authentication;

/// <summary>
/// Generic guest mode authenticator
/// </summary>
public interface IGuestAuthService
{
    /// <summary>
    /// Retrieves a public access token in order to access global data from remote services
    /// </summary>
    /// <returns></returns>
    public Task<string?> GetPublicAccessTokenAsync(CancellationToken cancellationToken);
}