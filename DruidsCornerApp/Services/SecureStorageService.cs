namespace DruidsCornerApp.Services;

public class SecureStorageService : ISecureStorageService
{
    private static readonly string TokenKey = "TOKEN";
    private static readonly string PasswordKey = "PASSWORD";
    private static readonly string EmailKey = "EMAIL";

    /// <summary>
    /// Store user token in secure storage
    /// </summary>
    /// <param name="token"></param>
    public async Task StoreTokenAsync(string token)
    {
        await SecureStorage.SetAsync(TokenKey, token);
    }

    /// <summary>
    /// Retrieves Stored token from secure storage
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetStoredTokenAsync()
    {
        return await SecureStorage.GetAsync(TokenKey);
    }


    /// <summary>
    /// Stores user password in secure storage
    /// </summary>
    /// <param name="password"></param>
    public async Task StorePasswordAsync(string password)
    {
        await SecureStorage.SetAsync(PasswordKey, password);
    }

    /// <summary>
    /// Fetches user password from secure storage
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetStoredPassword()
    {
        return await SecureStorage.GetAsync(PasswordKey);
    }


    /// <summary>
    /// Store user email in secure storage
    /// </summary>
    /// <param name="email"></param>
    public async Task StoreEmailAsync(string email)
    {
        await SecureStorage.SetAsync(EmailKey, email);
    }

    /// <summary>
    /// Fetches user email from secure storage
    /// </summary>
    /// <returns></returns>
    public async Task<string?> GetStoredEmailAsync()
    {
        return await SecureStorage.GetAsync(EmailKey);
    }
}