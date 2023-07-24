using DruidsCornerApp.Models.Config;
using System.Reflection;
using System.Text.Json;

namespace DruidsCornerApp.Services;

/// <summary>
/// Local authentication configuration provider
/// </summary>
public class LocalAuthConfigProvider : IAuthConfigProvider
{
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
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<AuthConfig> GetAuthConfigAsync()
    {
        const string resourceName = "DruidsCornerApp/AuthConfig.json";
        var stream = BuildResourceStream(resourceName);

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

    public AuthConfig GetAuthConfig()
    {
        const string resourceName = "DruidsCornerApp/AuthConfig.json";
        var stream = BuildResourceStream(resourceName);

        var authConfig = JsonSerializer.Deserialize<AuthConfig>(stream, new JsonSerializerOptions()
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
}