using System.Reflection;
using System.Text.Json;
using DruidsCornerApp.Models.Config;

namespace DruidsCornerApp.Services.Authentication;

/// <summary>
/// Local authentication configuration provider
/// </summary>
public class LocalAuthConfigProvider : IAuthConfigProvider
{
    public const string DefaultConfigFileName = "AuthConfig.json";
    private static Stream BuildResourceStream(string name)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Determine path
        var resourcePath = assembly.GetManifestResourceNames().Single(str => str.EndsWith(Path.GetFileName(name)));
        
        Stream stream = assembly.GetManifestResourceStream(resourcePath)!;
        return stream;
    }

    /// <summary>
    /// Retrieves authentication configuration from local resources
    /// </summary>
    /// <returns>AuthConfig object, parsed from filestream of empty object</returns>
    public async Task<AuthConfig> GetAuthConfigAsync()
    {
        return await GetAuthConfigAsync(DefaultConfigFileName);
    }
    
    /// <summary>
    /// Retrieves authentication configuration from local resources
    /// </summary>
    /// <param name="name">Configuration file name</param>
    /// <returns>AuthConfig object, parsed from filestream of empty object</returns>
    public async Task<AuthConfig> GetAuthConfigAsync(string name)
    {
        var stream = BuildResourceStream(name);

        var authConfig = await JsonSerializer.DeserializeAsync<AuthConfig>(stream, new JsonSerializerOptions()
        {
            // Fancy options there ..
        });
        
        // Reject null results, or maybe throw an exception instead ?
        if (authConfig == null)
        {
            return new AuthConfig();
        }

        return authConfig;
    }

    /// <summary>
    /// Retrieves authentication configuration from local resources
    /// Synchronous version of the <see cref="GetAuthConfigAsync()"/> method.
    /// </summary>
    /// <returns>AuthConfig object, parsed from filestream of empty object</returns>
    public AuthConfig GetAuthConfig()
    {
        return Task.Run(async () => await GetAuthConfigAsync()).Result;
    }
    
    /// <summary>
    /// Retrieves authentication configuration from local resources
    /// Synchronous version of the <see cref="GetAuthConfigAsync()"/> method.
    /// </summary>
    /// <param name="name">Configuration file name</param>
    /// <returns>AuthConfig object, parsed from filestream of empty object</returns>
    public AuthConfig GetAuthConfig(string name)
    {
        return Task.Run(async () => await GetAuthConfigAsync(name)).Result;
    }
}