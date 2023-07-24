using DruidsCornerApp.Models.Config;

namespace DruidsCornerApp.Services;

/// <summary>
/// Global interface for authentication configuration providers
/// </summary>
public interface IAuthConfigProvider
{
    /// <summary>
    /// Returns an Authentication Configuration object from either local resources or remote resources
    /// </summary>
    /// <returns></returns>
    public abstract Task<AuthConfig> GetAuthConfigAsync();
    
    /// <summary>
    /// Returns an Authentication Configuration object from either local resources or remote resources
    /// Variant of <see cref="GetAuthConfigAsync"/>, without the async trait 
    /// </summary>
    /// <returns></returns>
    public abstract AuthConfig GetAuthConfig();
}