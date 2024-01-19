using System.Text.Json;
using DruidsCornerApiClient.Utils;
using DruidsCornerApp.Models.References;
using DruidsCornerApp.Services.StaticData;
using Microsoft.Extensions.Logging;
namespace DruidsCornerApp.Services.ResourceProviders;


/// <summary>
/// Hop provider plays the role as a resource provider (resource, in this context, refers to hop, yeast, beer style, etc...) from database.
/// Performs caching to speed up subsequent calls.
/// </summary>
public class HopProvider
{
    /// <summary>
    /// Hops data now is only cached in the app memory (RAM).
    /// Later on, it'll be possible to use device's persistent caching system in order to dump data.
    /// </summary>
    private List<HopModel> _hops = new();

    private readonly ILogger<HopProvider>? _logger;

    public HopProvider(ILogger<HopProvider>? logger)
    {
        _logger = logger;
    }

    private void ReadHopsFromEmbeddedResource()
    {
        _logger?.LogInformation("Reading stream for {hopsFile}", StaticDataProvider.GetFileName(Source.Hops));
        var stream = StaticDataProvider.GetFileStream(Source.Hops);
        if (stream != null)
        {
            _logger?.LogInformation("Acquired stream for {hopsFile}", StaticDataProvider.GetFileName(Source.Hops));
            Dictionary<string, List<object>>? dict = JsonSerializer.Deserialize<Dictionary<string, List<object>>>(stream, JsonOptionProvider.GetJsonOptions());
            if (dict != null)
            {
                foreach (var hopDict in dict["hops"])
                {
                    var hopJsonStr = JsonSerializer.Serialize(hopDict);
                    var hop = JsonSerializer.Deserialize<HopModel>(hopJsonStr, JsonOptionProvider.GetJsonOptions());
                    if (hop == null)
                    {
                        continue;
                    }
                    _logger?.LogInformation("Deserialized hop {name}", hop.Name);
                    _logger?.LogInformation("Hop Caryophyllene content : {content}", hop.Caryophyllene);
                    _hops.Add(hop);
                }
            }
        }
    }

    private void EnsureInMemory()
    {
        // Retrieve data first
        if (_hops.Count == 0)
        {
            ReadHopsFromEmbeddedResource();
        }
    }

    public HopModel? GetFromId(string id)
    {
        EnsureInMemory();

        var targeted = _hops.SingleOrDefault(item => item?.Id == id, null);
        return targeted;
    }

    public List<HopModel> GetAllHops()
    {
        EnsureInMemory();
        return _hops;
    }

}
