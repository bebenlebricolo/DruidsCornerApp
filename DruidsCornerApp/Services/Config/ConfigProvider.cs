using System.Reflection;
using System.Text.Json;
using DruidsCornerApp.Models.Config;
using Microsoft.Extensions.Logging;
using DruidsCornerApiClient.Models;
namespace DruidsCornerApp.Services.Config;

/// <summary>
/// Provides configuration properties to the app using the appsettings.json file.
/// This method is used instead of the IConfiguration default providers as this is a .net Maui app and
/// aspnet core is not really well supported for now
/// (and having proper Json classes is at least as good as relying on the IConfiguration.GetSection{type} anyways, IMHO).
/// </summary>
public class ConfigProvider : IConfigProvider
{
    private const string ConfigFilename = "appsettings.json";
    private readonly ILogger<ConfigProvider> _logger;
    private ClientConfiguration? _config = null;

    public ConfigProvider(ILogger<ConfigProvider> logger)
    {
        _logger = logger;
    }

    public static Stream? GetConfigFileStream()
    {
        // Load configuration from embedded resources
        var a = Assembly.GetExecutingAssembly();
        var stream = a.GetManifestResourceStream($"DruidsCornerApp.{ConfigFilename}");
        return stream;
    }

    /// <summary>
    /// Reads the configuration from embedded appConfig.json resources
    /// Note that it'll behave differently as the regular aspnet IConfiguration as
    /// this custom implementation doesn't need to support various environments.
    /// </summary>
    /// <param name="noCache">If set, cached values will be ignored and a call to embedded resources + deserialization
    /// will be performed, otherwise cached values are preferred</param>
    /// <returns></returns>
    public async Task<ClientConfiguration?> GetConfigAsync(bool noCache = false)
    {
        // Cached data
        if (!noCache && _config != null)
        {
            return _config;
        }

        var stream = GetConfigFileStream();
        if (stream == null)
        {
            return null;
        }

        try
        {
            var data = await JsonSerializer.DeserializeAsync<ClientConfiguration>(stream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            });
            stream.Close();

            // Cache configuration for later reuse
            if (!noCache && data != null)
            {
                _config = data;
            }

            return data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Cannot read configuration file ! : {ex.Message}");
            return null;
        }
    }
}