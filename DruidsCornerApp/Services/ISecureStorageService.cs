namespace DruidsCornerApp.Services;

public interface ISecureStorageService
{

#region GeneralAccessors
    /// <summary>
    /// Fetches an arbitrary data pair from secure storage.
    /// Returns null if data cannot be located.
    /// </summary>
    /// <param name="key">Key of the stored data</param>
    /// <returns>Data content (string) or null if none could be found.</returns>
    public Task<string?> GetAsync(string key);

    /// <summary>
    /// Stores an arbitrary value in secure storage
    /// Key management is external to this service
    /// </summary>
    /// <param name="key">Key of the data pair</param>
    /// <param name="value">Content of the data pair</param>
    public Task StoreAsync(string key, string value);

    /// <summary>
    /// Remove arbitrary data from secure storage
    /// Key management is external to this service
    /// </summary>
    /// <param name="key">Key of the data pair</param>
    public void Remove(string key);

    /// <summary>
    /// Frees the whole secure storage block
    /// </summary>
    public void RemoveAllData();
    
#endregion GeneralAccessors
}