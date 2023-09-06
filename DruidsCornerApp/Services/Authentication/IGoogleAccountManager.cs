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
    public Task<List<GoogleAccount>> ListGoogleAccountsOnDeviceAsync(CancellationToken cancellationToken);
    
    /// <summary>
    /// Retrieves the current google account from device (if any)
    /// This calls GooglePlayServices api and looks for the last signedIn account on the device.
    /// </summary>
    /// <returns></returns>
    public GoogleAccount? GetCurrentGoogleAccount();

    /// <summary>
    /// Perform account signIn using the provided GoogleAccount
    /// </summary>
    /// <param name="account"></param>
    /// <returns>True : signIn is successful, false, error encountered</returns>
    public Task<bool> SignInWithAccountAsync(GoogleAccount account);
}