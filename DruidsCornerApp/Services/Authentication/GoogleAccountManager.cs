using DruidsCornerApp.Models.Google;
using Microsoft.Extensions.Logging;

namespace DruidsCornerApp.Services.Authentication;

/// <summary>
/// Retrieves Google Account using platform specific code
/// </summary>
public partial class GoogleAccountManager : IGoogleAccountManager
{
    private readonly ILogger<GoogleAccountManager> _logger;

    public GoogleAccountManager(ILogger<GoogleAccountManager> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Implemented in the Platforms/(Platform)/Authentication/*** folders
    /// For every targeted platforms
    /// </summary>
    /// <returns></returns>
#if __ANDROID__
    public partial Task<List<GoogleAccount>> ListGoogleAccountsOnDeviceAsync(CancellationToken cancellationToken);

    public partial GoogleAccount? GetCurrentGoogleAccount();

    public partial Task<bool> SignInWithAccountAsync(GoogleAccount account);

    // Standard .Net7.0 implementation, overriden at build time by the platform specific one.
#else
    public Task<List<GoogleAccount>> ListGoogleAccountsOnDeviceAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult<List<GoogleAccount>>(new List<GoogleAccount>());
    }

    public GoogleAccount? GetCurrentGoogleAccount()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SignInWithAccountAsync(GoogleAccount account)
    {
        return Task.FromResult<bool>(false);
    }


#endif
}