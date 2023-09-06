namespace DruidsCornerApp.Services;

public class SecureStorageService : ISecureStorageService
{
    public async Task<string?> GetAsync(string key)
    {
        return await SecureStorage.GetAsync(key);
    }

    public async Task StoreAsync(string key, string value)
    {
        await SecureStorage.SetAsync(key, value);
    }

    public void Remove(string key)
    {
        SecureStorage.Remove(key);
    }

    /// <summary>
    /// Frees the whole SecureStorage block
    /// </summary>
    public void RemoveAllData()
    {
        SecureStorage.RemoveAll();
    }
}