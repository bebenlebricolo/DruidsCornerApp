namespace DruidsCornerApp.Services;

public interface ISecureStorageService
{
    /// <summary>
    /// Store user token in secure storage
    /// </summary>
    /// <param name="token"></param>
    public Task StoreTokenAsync(string token);

    /// <summary>
    /// Retrieves Stored token from secure storage
    /// </summary>
    /// <returns></returns>
    public Task<string?> GetStoredTokenAsync();


    /// <summary>
    /// Stores user password in secure storage
    /// </summary>
    /// <param name="password"></param>
    public Task StorePasswordAsync(string password);

    /// <summary>
    /// Fetches user password from secure storage
    /// </summary>
    /// <returns></returns>
    public Task<string?> GetStoredPassword();

    /// <summary>
    /// Store user email in secure storage
    /// </summary>
    /// <param name="email"></param>
    public Task StoreEmailAsync(string email);

    /// <summary>
    /// Fetches user email from secure storage
    /// </summary>
    /// <returns></returns>
    public Task<string?> GetStoredEmailAsync();
}