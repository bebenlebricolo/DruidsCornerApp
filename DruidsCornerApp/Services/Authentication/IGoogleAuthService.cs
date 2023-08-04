using DruidsCornerApp.Models.Google;

namespace DruidsCornerApp.Services.Authentication;

/// <summary>
/// Defines a general Google authentication wrapper interface.
/// </summary>
public interface IGoogleAuthService
{
    /// <summary>
    /// Authenticates a single user in order to consume remote app services
    /// </summary>
    /// <returns></returns>
    public Task<GoogleAccount?> AuthenticateAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Logs out the current user
    /// </summary>
    /// <returns></returns>
    public Task LogoutAsync(CancellationToken cancellationToken);
    
    
    /// <summary>
    /// Returns the currently logged in user
    /// </summary>
    /// <returns></returns>
    public Task<GoogleAccount?> GetCurrentUserAsync(CancellationToken cancellationToken);
}