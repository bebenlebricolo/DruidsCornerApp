using DruidsCornerApiClient.Models;

namespace DruidsCornerApp.Services.Config;

/// <summary>
/// Provides configuration properties to the app using the appsettings.json file.
/// This method is used instead of the IConfiguration default providers as this is a .net Maui app and
/// aspnet core is not really well supported for now
/// (and having proper Json classes is at least as good as relying on the IConfiguration.GetSection{type} anyways, IMHO).
/// </summary>
public interface IConfigProvider
{
    /// <summary>
    /// Reads the configuration from embedded appConfig.json resources
    /// Note that it'll behave differently as the regular aspnet IConfiguration as
    /// this custom implementation doesn't need to support various environments.
    /// </summary>
    /// <param name="noCache">If set, cached values will be ignored and a call to embedded resources + deserialization
    /// will be performed, otherwise cached values are preferred</param>
    /// <returns></returns>
    public Task<ClientConfiguration?> GetConfigAsync(bool noCache = false);
}