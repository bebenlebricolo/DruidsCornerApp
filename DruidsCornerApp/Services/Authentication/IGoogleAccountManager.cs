using DruidsCornerApp.Models.Google;

namespace DruidsCornerApp.Services.Authentication;

/// <summary>
/// Wraps platform specific code in order to retrieve the current Google Account used on the device
/// </summary>
public interface IGoogleAccountManager
{
    /// <summary>
    /// Retrieves the Current Google Account of the device, if any.
    /// </summary>
    /// <returns>Google account or null, if none could be retrieved.</returns>
    public Task<GoogleAccount?> GetCurrentGoogleAccountAsync(CancellationToken cancellationToken);
}